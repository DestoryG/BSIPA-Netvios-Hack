using System;

namespace System.Collections.Specialized
{
	// Token: 0x020003B5 RID: 949
	[Serializable]
	public class StringCollection : IList, ICollection, IEnumerable
	{
		// Token: 0x17000908 RID: 2312
		public string this[int index]
		{
			get
			{
				return (string)this.data[index];
			}
			set
			{
				this.data[index] = value;
			}
		}

		// Token: 0x17000909 RID: 2313
		// (get) Token: 0x060023AC RID: 9132 RVA: 0x000A8727 File Offset: 0x000A6927
		public int Count
		{
			get
			{
				return this.data.Count;
			}
		}

		// Token: 0x1700090A RID: 2314
		// (get) Token: 0x060023AD RID: 9133 RVA: 0x000A8734 File Offset: 0x000A6934
		bool IList.IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700090B RID: 2315
		// (get) Token: 0x060023AE RID: 9134 RVA: 0x000A8737 File Offset: 0x000A6937
		bool IList.IsFixedSize
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060023AF RID: 9135 RVA: 0x000A873A File Offset: 0x000A693A
		public int Add(string value)
		{
			return this.data.Add(value);
		}

		// Token: 0x060023B0 RID: 9136 RVA: 0x000A8748 File Offset: 0x000A6948
		public void AddRange(string[] value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			this.data.AddRange(value);
		}

		// Token: 0x060023B1 RID: 9137 RVA: 0x000A8764 File Offset: 0x000A6964
		public void Clear()
		{
			this.data.Clear();
		}

		// Token: 0x060023B2 RID: 9138 RVA: 0x000A8771 File Offset: 0x000A6971
		public bool Contains(string value)
		{
			return this.data.Contains(value);
		}

		// Token: 0x060023B3 RID: 9139 RVA: 0x000A877F File Offset: 0x000A697F
		public void CopyTo(string[] array, int index)
		{
			this.data.CopyTo(array, index);
		}

		// Token: 0x060023B4 RID: 9140 RVA: 0x000A878E File Offset: 0x000A698E
		public StringEnumerator GetEnumerator()
		{
			return new StringEnumerator(this);
		}

		// Token: 0x060023B5 RID: 9141 RVA: 0x000A8796 File Offset: 0x000A6996
		public int IndexOf(string value)
		{
			return this.data.IndexOf(value);
		}

		// Token: 0x060023B6 RID: 9142 RVA: 0x000A87A4 File Offset: 0x000A69A4
		public void Insert(int index, string value)
		{
			this.data.Insert(index, value);
		}

		// Token: 0x1700090C RID: 2316
		// (get) Token: 0x060023B7 RID: 9143 RVA: 0x000A87B3 File Offset: 0x000A69B3
		public bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700090D RID: 2317
		// (get) Token: 0x060023B8 RID: 9144 RVA: 0x000A87B6 File Offset: 0x000A69B6
		public bool IsSynchronized
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060023B9 RID: 9145 RVA: 0x000A87B9 File Offset: 0x000A69B9
		public void Remove(string value)
		{
			this.data.Remove(value);
		}

		// Token: 0x060023BA RID: 9146 RVA: 0x000A87C7 File Offset: 0x000A69C7
		public void RemoveAt(int index)
		{
			this.data.RemoveAt(index);
		}

		// Token: 0x1700090E RID: 2318
		// (get) Token: 0x060023BB RID: 9147 RVA: 0x000A87D5 File Offset: 0x000A69D5
		public object SyncRoot
		{
			get
			{
				return this.data.SyncRoot;
			}
		}

		// Token: 0x1700090F RID: 2319
		object IList.this[int index]
		{
			get
			{
				return this[index];
			}
			set
			{
				this[index] = (string)value;
			}
		}

		// Token: 0x060023BE RID: 9150 RVA: 0x000A87FA File Offset: 0x000A69FA
		int IList.Add(object value)
		{
			return this.Add((string)value);
		}

		// Token: 0x060023BF RID: 9151 RVA: 0x000A8808 File Offset: 0x000A6A08
		bool IList.Contains(object value)
		{
			return this.Contains((string)value);
		}

		// Token: 0x060023C0 RID: 9152 RVA: 0x000A8816 File Offset: 0x000A6A16
		int IList.IndexOf(object value)
		{
			return this.IndexOf((string)value);
		}

		// Token: 0x060023C1 RID: 9153 RVA: 0x000A8824 File Offset: 0x000A6A24
		void IList.Insert(int index, object value)
		{
			this.Insert(index, (string)value);
		}

		// Token: 0x060023C2 RID: 9154 RVA: 0x000A8833 File Offset: 0x000A6A33
		void IList.Remove(object value)
		{
			this.Remove((string)value);
		}

		// Token: 0x060023C3 RID: 9155 RVA: 0x000A8841 File Offset: 0x000A6A41
		void ICollection.CopyTo(Array array, int index)
		{
			this.data.CopyTo(array, index);
		}

		// Token: 0x060023C4 RID: 9156 RVA: 0x000A8850 File Offset: 0x000A6A50
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.data.GetEnumerator();
		}

		// Token: 0x04001FE6 RID: 8166
		private ArrayList data = new ArrayList();
	}
}
