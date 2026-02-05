using System;
using System.Collections;

namespace System.Diagnostics
{
	// Token: 0x020004B4 RID: 1204
	public class TraceListenerCollection : IList, ICollection, IEnumerable
	{
		// Token: 0x06002CDE RID: 11486 RVA: 0x000C9AE1 File Offset: 0x000C7CE1
		internal TraceListenerCollection()
		{
			this.list = new ArrayList(1);
		}

		// Token: 0x17000ADA RID: 2778
		public TraceListener this[int i]
		{
			get
			{
				return (TraceListener)this.list[i];
			}
			set
			{
				this.InitializeListener(value);
				this.list[i] = value;
			}
		}

		// Token: 0x17000ADB RID: 2779
		public TraceListener this[string name]
		{
			get
			{
				foreach (object obj in this)
				{
					TraceListener traceListener = (TraceListener)obj;
					if (traceListener.Name == name)
					{
						return traceListener;
					}
				}
				return null;
			}
		}

		// Token: 0x17000ADC RID: 2780
		// (get) Token: 0x06002CE2 RID: 11490 RVA: 0x000C9B84 File Offset: 0x000C7D84
		public int Count
		{
			get
			{
				return this.list.Count;
			}
		}

		// Token: 0x06002CE3 RID: 11491 RVA: 0x000C9B94 File Offset: 0x000C7D94
		public int Add(TraceListener listener)
		{
			this.InitializeListener(listener);
			object critSec = TraceInternal.critSec;
			int num;
			lock (critSec)
			{
				num = this.list.Add(listener);
			}
			return num;
		}

		// Token: 0x06002CE4 RID: 11492 RVA: 0x000C9BE4 File Offset: 0x000C7DE4
		public void AddRange(TraceListener[] value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			for (int i = 0; i < value.Length; i++)
			{
				this.Add(value[i]);
			}
		}

		// Token: 0x06002CE5 RID: 11493 RVA: 0x000C9C18 File Offset: 0x000C7E18
		public void AddRange(TraceListenerCollection value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			int count = value.Count;
			for (int i = 0; i < count; i++)
			{
				this.Add(value[i]);
			}
		}

		// Token: 0x06002CE6 RID: 11494 RVA: 0x000C9C54 File Offset: 0x000C7E54
		public void Clear()
		{
			this.list = new ArrayList();
		}

		// Token: 0x06002CE7 RID: 11495 RVA: 0x000C9C61 File Offset: 0x000C7E61
		public bool Contains(TraceListener listener)
		{
			return ((IList)this).Contains(listener);
		}

		// Token: 0x06002CE8 RID: 11496 RVA: 0x000C9C6A File Offset: 0x000C7E6A
		public void CopyTo(TraceListener[] listeners, int index)
		{
			((ICollection)this).CopyTo(listeners, index);
		}

		// Token: 0x06002CE9 RID: 11497 RVA: 0x000C9C74 File Offset: 0x000C7E74
		public IEnumerator GetEnumerator()
		{
			return this.list.GetEnumerator();
		}

		// Token: 0x06002CEA RID: 11498 RVA: 0x000C9C81 File Offset: 0x000C7E81
		internal void InitializeListener(TraceListener listener)
		{
			if (listener == null)
			{
				throw new ArgumentNullException("listener");
			}
			listener.IndentSize = TraceInternal.IndentSize;
			listener.IndentLevel = TraceInternal.IndentLevel;
		}

		// Token: 0x06002CEB RID: 11499 RVA: 0x000C9CA7 File Offset: 0x000C7EA7
		public int IndexOf(TraceListener listener)
		{
			return ((IList)this).IndexOf(listener);
		}

		// Token: 0x06002CEC RID: 11500 RVA: 0x000C9CB0 File Offset: 0x000C7EB0
		public void Insert(int index, TraceListener listener)
		{
			this.InitializeListener(listener);
			object critSec = TraceInternal.critSec;
			lock (critSec)
			{
				this.list.Insert(index, listener);
			}
		}

		// Token: 0x06002CED RID: 11501 RVA: 0x000C9D00 File Offset: 0x000C7F00
		public void Remove(TraceListener listener)
		{
			((IList)this).Remove(listener);
		}

		// Token: 0x06002CEE RID: 11502 RVA: 0x000C9D0C File Offset: 0x000C7F0C
		public void Remove(string name)
		{
			TraceListener traceListener = this[name];
			if (traceListener != null)
			{
				((IList)this).Remove(traceListener);
			}
		}

		// Token: 0x06002CEF RID: 11503 RVA: 0x000C9D2C File Offset: 0x000C7F2C
		public void RemoveAt(int index)
		{
			object critSec = TraceInternal.critSec;
			lock (critSec)
			{
				this.list.RemoveAt(index);
			}
		}

		// Token: 0x17000ADD RID: 2781
		object IList.this[int index]
		{
			get
			{
				return this.list[index];
			}
			set
			{
				TraceListener traceListener = value as TraceListener;
				if (traceListener == null)
				{
					throw new ArgumentException(SR.GetString("MustAddListener"), "value");
				}
				this.InitializeListener(traceListener);
				this.list[index] = traceListener;
			}
		}

		// Token: 0x17000ADE RID: 2782
		// (get) Token: 0x06002CF2 RID: 11506 RVA: 0x000C9DC4 File Offset: 0x000C7FC4
		bool IList.IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000ADF RID: 2783
		// (get) Token: 0x06002CF3 RID: 11507 RVA: 0x000C9DC7 File Offset: 0x000C7FC7
		bool IList.IsFixedSize
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06002CF4 RID: 11508 RVA: 0x000C9DCC File Offset: 0x000C7FCC
		int IList.Add(object value)
		{
			TraceListener traceListener = value as TraceListener;
			if (traceListener == null)
			{
				throw new ArgumentException(SR.GetString("MustAddListener"), "value");
			}
			this.InitializeListener(traceListener);
			object critSec = TraceInternal.critSec;
			int num;
			lock (critSec)
			{
				num = this.list.Add(value);
			}
			return num;
		}

		// Token: 0x06002CF5 RID: 11509 RVA: 0x000C9E3C File Offset: 0x000C803C
		bool IList.Contains(object value)
		{
			return this.list.Contains(value);
		}

		// Token: 0x06002CF6 RID: 11510 RVA: 0x000C9E4A File Offset: 0x000C804A
		int IList.IndexOf(object value)
		{
			return this.list.IndexOf(value);
		}

		// Token: 0x06002CF7 RID: 11511 RVA: 0x000C9E58 File Offset: 0x000C8058
		void IList.Insert(int index, object value)
		{
			TraceListener traceListener = value as TraceListener;
			if (traceListener == null)
			{
				throw new ArgumentException(SR.GetString("MustAddListener"), "value");
			}
			this.InitializeListener(traceListener);
			object critSec = TraceInternal.critSec;
			lock (critSec)
			{
				this.list.Insert(index, value);
			}
		}

		// Token: 0x06002CF8 RID: 11512 RVA: 0x000C9EC4 File Offset: 0x000C80C4
		void IList.Remove(object value)
		{
			object critSec = TraceInternal.critSec;
			lock (critSec)
			{
				this.list.Remove(value);
			}
		}

		// Token: 0x17000AE0 RID: 2784
		// (get) Token: 0x06002CF9 RID: 11513 RVA: 0x000C9F0C File Offset: 0x000C810C
		object ICollection.SyncRoot
		{
			get
			{
				return this;
			}
		}

		// Token: 0x17000AE1 RID: 2785
		// (get) Token: 0x06002CFA RID: 11514 RVA: 0x000C9F0F File Offset: 0x000C810F
		bool ICollection.IsSynchronized
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06002CFB RID: 11515 RVA: 0x000C9F14 File Offset: 0x000C8114
		void ICollection.CopyTo(Array array, int index)
		{
			object critSec = TraceInternal.critSec;
			lock (critSec)
			{
				this.list.CopyTo(array, index);
			}
		}

		// Token: 0x040026EA RID: 9962
		private ArrayList list;
	}
}
