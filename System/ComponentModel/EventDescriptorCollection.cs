using System;
using System.Collections;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.ComponentModel
{
	// Token: 0x02000550 RID: 1360
	[ComVisible(true)]
	[HostProtection(SecurityAction.LinkDemand, Synchronization = true)]
	public class EventDescriptorCollection : ICollection, IEnumerable, IList
	{
		// Token: 0x06003306 RID: 13062 RVA: 0x000E309C File Offset: 0x000E129C
		public EventDescriptorCollection(EventDescriptor[] events)
		{
			this.events = events;
			if (events == null)
			{
				this.events = new EventDescriptor[0];
				this.eventCount = 0;
			}
			else
			{
				this.eventCount = this.events.Length;
			}
			this.eventsOwned = true;
		}

		// Token: 0x06003307 RID: 13063 RVA: 0x000E30EA File Offset: 0x000E12EA
		public EventDescriptorCollection(EventDescriptor[] events, bool readOnly)
			: this(events)
		{
			this.readOnly = readOnly;
		}

		// Token: 0x06003308 RID: 13064 RVA: 0x000E30FC File Offset: 0x000E12FC
		private EventDescriptorCollection(EventDescriptor[] events, int eventCount, string[] namedSort, IComparer comparer)
		{
			this.eventsOwned = false;
			if (namedSort != null)
			{
				this.namedSort = (string[])namedSort.Clone();
			}
			this.comparer = comparer;
			this.events = events;
			this.eventCount = eventCount;
			this.needSort = true;
		}

		// Token: 0x17000C7A RID: 3194
		// (get) Token: 0x06003309 RID: 13065 RVA: 0x000E314E File Offset: 0x000E134E
		public int Count
		{
			get
			{
				return this.eventCount;
			}
		}

		// Token: 0x17000C7B RID: 3195
		public virtual EventDescriptor this[int index]
		{
			get
			{
				if (index >= this.eventCount)
				{
					throw new IndexOutOfRangeException();
				}
				this.EnsureEventsOwned();
				return this.events[index];
			}
		}

		// Token: 0x17000C7C RID: 3196
		public virtual EventDescriptor this[string name]
		{
			get
			{
				return this.Find(name, false);
			}
		}

		// Token: 0x0600330C RID: 13068 RVA: 0x000E3180 File Offset: 0x000E1380
		public int Add(EventDescriptor value)
		{
			if (this.readOnly)
			{
				throw new NotSupportedException();
			}
			this.EnsureSize(this.eventCount + 1);
			EventDescriptor[] array = this.events;
			int num = this.eventCount;
			this.eventCount = num + 1;
			array[num] = value;
			return this.eventCount - 1;
		}

		// Token: 0x0600330D RID: 13069 RVA: 0x000E31CA File Offset: 0x000E13CA
		public void Clear()
		{
			if (this.readOnly)
			{
				throw new NotSupportedException();
			}
			this.eventCount = 0;
		}

		// Token: 0x0600330E RID: 13070 RVA: 0x000E31E1 File Offset: 0x000E13E1
		public bool Contains(EventDescriptor value)
		{
			return this.IndexOf(value) >= 0;
		}

		// Token: 0x0600330F RID: 13071 RVA: 0x000E31F0 File Offset: 0x000E13F0
		void ICollection.CopyTo(Array array, int index)
		{
			this.EnsureEventsOwned();
			Array.Copy(this.events, 0, array, index, this.Count);
		}

		// Token: 0x06003310 RID: 13072 RVA: 0x000E320C File Offset: 0x000E140C
		private void EnsureEventsOwned()
		{
			if (!this.eventsOwned)
			{
				this.eventsOwned = true;
				if (this.events != null)
				{
					EventDescriptor[] array = new EventDescriptor[this.Count];
					Array.Copy(this.events, 0, array, 0, this.Count);
					this.events = array;
				}
			}
			if (this.needSort)
			{
				this.needSort = false;
				this.InternalSort(this.namedSort);
			}
		}

		// Token: 0x06003311 RID: 13073 RVA: 0x000E3274 File Offset: 0x000E1474
		private void EnsureSize(int sizeNeeded)
		{
			if (sizeNeeded <= this.events.Length)
			{
				return;
			}
			if (this.events == null || this.events.Length == 0)
			{
				this.eventCount = 0;
				this.events = new EventDescriptor[sizeNeeded];
				return;
			}
			this.EnsureEventsOwned();
			int num = Math.Max(sizeNeeded, this.events.Length * 2);
			EventDescriptor[] array = new EventDescriptor[num];
			Array.Copy(this.events, 0, array, 0, this.eventCount);
			this.events = array;
		}

		// Token: 0x06003312 RID: 13074 RVA: 0x000E32EC File Offset: 0x000E14EC
		public virtual EventDescriptor Find(string name, bool ignoreCase)
		{
			EventDescriptor eventDescriptor = null;
			if (ignoreCase)
			{
				for (int i = 0; i < this.Count; i++)
				{
					if (string.Equals(this.events[i].Name, name, StringComparison.OrdinalIgnoreCase))
					{
						eventDescriptor = this.events[i];
						break;
					}
				}
			}
			else
			{
				for (int j = 0; j < this.Count; j++)
				{
					if (string.Equals(this.events[j].Name, name, StringComparison.Ordinal))
					{
						eventDescriptor = this.events[j];
						break;
					}
				}
			}
			return eventDescriptor;
		}

		// Token: 0x06003313 RID: 13075 RVA: 0x000E3365 File Offset: 0x000E1565
		public int IndexOf(EventDescriptor value)
		{
			return Array.IndexOf<EventDescriptor>(this.events, value, 0, this.eventCount);
		}

		// Token: 0x06003314 RID: 13076 RVA: 0x000E337C File Offset: 0x000E157C
		public void Insert(int index, EventDescriptor value)
		{
			if (this.readOnly)
			{
				throw new NotSupportedException();
			}
			this.EnsureSize(this.eventCount + 1);
			if (index < this.eventCount)
			{
				Array.Copy(this.events, index, this.events, index + 1, this.eventCount - index);
			}
			this.events[index] = value;
			this.eventCount++;
		}

		// Token: 0x06003315 RID: 13077 RVA: 0x000E33E4 File Offset: 0x000E15E4
		public void Remove(EventDescriptor value)
		{
			if (this.readOnly)
			{
				throw new NotSupportedException();
			}
			int num = this.IndexOf(value);
			if (num != -1)
			{
				this.RemoveAt(num);
			}
		}

		// Token: 0x06003316 RID: 13078 RVA: 0x000E3414 File Offset: 0x000E1614
		public void RemoveAt(int index)
		{
			if (this.readOnly)
			{
				throw new NotSupportedException();
			}
			if (index < this.eventCount - 1)
			{
				Array.Copy(this.events, index + 1, this.events, index, this.eventCount - index - 1);
			}
			this.events[this.eventCount - 1] = null;
			this.eventCount--;
		}

		// Token: 0x06003317 RID: 13079 RVA: 0x000E3477 File Offset: 0x000E1677
		public IEnumerator GetEnumerator()
		{
			if (this.events.Length == this.eventCount)
			{
				return this.events.GetEnumerator();
			}
			return new ArraySubsetEnumerator(this.events, this.eventCount);
		}

		// Token: 0x06003318 RID: 13080 RVA: 0x000E34A6 File Offset: 0x000E16A6
		public virtual EventDescriptorCollection Sort()
		{
			return new EventDescriptorCollection(this.events, this.eventCount, this.namedSort, this.comparer);
		}

		// Token: 0x06003319 RID: 13081 RVA: 0x000E34C5 File Offset: 0x000E16C5
		public virtual EventDescriptorCollection Sort(string[] names)
		{
			return new EventDescriptorCollection(this.events, this.eventCount, names, this.comparer);
		}

		// Token: 0x0600331A RID: 13082 RVA: 0x000E34DF File Offset: 0x000E16DF
		public virtual EventDescriptorCollection Sort(string[] names, IComparer comparer)
		{
			return new EventDescriptorCollection(this.events, this.eventCount, names, comparer);
		}

		// Token: 0x0600331B RID: 13083 RVA: 0x000E34F4 File Offset: 0x000E16F4
		public virtual EventDescriptorCollection Sort(IComparer comparer)
		{
			return new EventDescriptorCollection(this.events, this.eventCount, this.namedSort, comparer);
		}

		// Token: 0x0600331C RID: 13084 RVA: 0x000E3510 File Offset: 0x000E1710
		protected void InternalSort(string[] names)
		{
			if (this.events == null || this.events.Length == 0)
			{
				return;
			}
			this.InternalSort(this.comparer);
			if (names != null && names.Length != 0)
			{
				ArrayList arrayList = new ArrayList(this.events);
				int num = 0;
				int num2 = this.events.Length;
				for (int i = 0; i < names.Length; i++)
				{
					for (int j = 0; j < num2; j++)
					{
						EventDescriptor eventDescriptor = (EventDescriptor)arrayList[j];
						if (eventDescriptor != null && eventDescriptor.Name.Equals(names[i]))
						{
							this.events[num++] = eventDescriptor;
							arrayList[j] = null;
							break;
						}
					}
				}
				for (int k = 0; k < num2; k++)
				{
					if (arrayList[k] != null)
					{
						this.events[num++] = (EventDescriptor)arrayList[k];
					}
				}
			}
		}

		// Token: 0x0600331D RID: 13085 RVA: 0x000E35ED File Offset: 0x000E17ED
		protected void InternalSort(IComparer sorter)
		{
			if (sorter == null)
			{
				TypeDescriptor.SortDescriptorArray(this);
				return;
			}
			Array.Sort(this.events, sorter);
		}

		// Token: 0x17000C7D RID: 3197
		// (get) Token: 0x0600331E RID: 13086 RVA: 0x000E3605 File Offset: 0x000E1805
		int ICollection.Count
		{
			get
			{
				return this.Count;
			}
		}

		// Token: 0x17000C7E RID: 3198
		// (get) Token: 0x0600331F RID: 13087 RVA: 0x000E360D File Offset: 0x000E180D
		bool ICollection.IsSynchronized
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000C7F RID: 3199
		// (get) Token: 0x06003320 RID: 13088 RVA: 0x000E3610 File Offset: 0x000E1810
		object ICollection.SyncRoot
		{
			get
			{
				return null;
			}
		}

		// Token: 0x06003321 RID: 13089 RVA: 0x000E3613 File Offset: 0x000E1813
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x17000C80 RID: 3200
		object IList.this[int index]
		{
			get
			{
				return this[index];
			}
			set
			{
				if (this.readOnly)
				{
					throw new NotSupportedException();
				}
				if (index >= this.eventCount)
				{
					throw new IndexOutOfRangeException();
				}
				this.EnsureEventsOwned();
				this.events[index] = (EventDescriptor)value;
			}
		}

		// Token: 0x06003324 RID: 13092 RVA: 0x000E3657 File Offset: 0x000E1857
		int IList.Add(object value)
		{
			return this.Add((EventDescriptor)value);
		}

		// Token: 0x06003325 RID: 13093 RVA: 0x000E3665 File Offset: 0x000E1865
		void IList.Clear()
		{
			this.Clear();
		}

		// Token: 0x06003326 RID: 13094 RVA: 0x000E366D File Offset: 0x000E186D
		bool IList.Contains(object value)
		{
			return this.Contains((EventDescriptor)value);
		}

		// Token: 0x06003327 RID: 13095 RVA: 0x000E367B File Offset: 0x000E187B
		int IList.IndexOf(object value)
		{
			return this.IndexOf((EventDescriptor)value);
		}

		// Token: 0x06003328 RID: 13096 RVA: 0x000E3689 File Offset: 0x000E1889
		void IList.Insert(int index, object value)
		{
			this.Insert(index, (EventDescriptor)value);
		}

		// Token: 0x06003329 RID: 13097 RVA: 0x000E3698 File Offset: 0x000E1898
		void IList.Remove(object value)
		{
			this.Remove((EventDescriptor)value);
		}

		// Token: 0x0600332A RID: 13098 RVA: 0x000E36A6 File Offset: 0x000E18A6
		void IList.RemoveAt(int index)
		{
			this.RemoveAt(index);
		}

		// Token: 0x17000C81 RID: 3201
		// (get) Token: 0x0600332B RID: 13099 RVA: 0x000E36AF File Offset: 0x000E18AF
		bool IList.IsReadOnly
		{
			get
			{
				return this.readOnly;
			}
		}

		// Token: 0x17000C82 RID: 3202
		// (get) Token: 0x0600332C RID: 13100 RVA: 0x000E36B7 File Offset: 0x000E18B7
		bool IList.IsFixedSize
		{
			get
			{
				return this.readOnly;
			}
		}

		// Token: 0x040029A2 RID: 10658
		private EventDescriptor[] events;

		// Token: 0x040029A3 RID: 10659
		private string[] namedSort;

		// Token: 0x040029A4 RID: 10660
		private IComparer comparer;

		// Token: 0x040029A5 RID: 10661
		private bool eventsOwned = true;

		// Token: 0x040029A6 RID: 10662
		private bool needSort;

		// Token: 0x040029A7 RID: 10663
		private int eventCount;

		// Token: 0x040029A8 RID: 10664
		private bool readOnly;

		// Token: 0x040029A9 RID: 10665
		public static readonly EventDescriptorCollection Empty = new EventDescriptorCollection(null, true);
	}
}
