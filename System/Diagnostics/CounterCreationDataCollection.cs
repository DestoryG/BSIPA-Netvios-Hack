using System;
using System.Collections;

namespace System.Diagnostics
{
	// Token: 0x020004C1 RID: 1217
	[Serializable]
	public class CounterCreationDataCollection : CollectionBase
	{
		// Token: 0x06002D6C RID: 11628 RVA: 0x000CC637 File Offset: 0x000CA837
		public CounterCreationDataCollection()
		{
		}

		// Token: 0x06002D6D RID: 11629 RVA: 0x000CC63F File Offset: 0x000CA83F
		public CounterCreationDataCollection(CounterCreationDataCollection value)
		{
			this.AddRange(value);
		}

		// Token: 0x06002D6E RID: 11630 RVA: 0x000CC64E File Offset: 0x000CA84E
		public CounterCreationDataCollection(CounterCreationData[] value)
		{
			this.AddRange(value);
		}

		// Token: 0x17000AF8 RID: 2808
		public CounterCreationData this[int index]
		{
			get
			{
				return (CounterCreationData)base.List[index];
			}
			set
			{
				base.List[index] = value;
			}
		}

		// Token: 0x06002D71 RID: 11633 RVA: 0x000CC67F File Offset: 0x000CA87F
		public int Add(CounterCreationData value)
		{
			return base.List.Add(value);
		}

		// Token: 0x06002D72 RID: 11634 RVA: 0x000CC690 File Offset: 0x000CA890
		public void AddRange(CounterCreationData[] value)
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

		// Token: 0x06002D73 RID: 11635 RVA: 0x000CC6C4 File Offset: 0x000CA8C4
		public void AddRange(CounterCreationDataCollection value)
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

		// Token: 0x06002D74 RID: 11636 RVA: 0x000CC700 File Offset: 0x000CA900
		public bool Contains(CounterCreationData value)
		{
			return base.List.Contains(value);
		}

		// Token: 0x06002D75 RID: 11637 RVA: 0x000CC70E File Offset: 0x000CA90E
		public void CopyTo(CounterCreationData[] array, int index)
		{
			base.List.CopyTo(array, index);
		}

		// Token: 0x06002D76 RID: 11638 RVA: 0x000CC71D File Offset: 0x000CA91D
		public int IndexOf(CounterCreationData value)
		{
			return base.List.IndexOf(value);
		}

		// Token: 0x06002D77 RID: 11639 RVA: 0x000CC72B File Offset: 0x000CA92B
		public void Insert(int index, CounterCreationData value)
		{
			base.List.Insert(index, value);
		}

		// Token: 0x06002D78 RID: 11640 RVA: 0x000CC73A File Offset: 0x000CA93A
		public virtual void Remove(CounterCreationData value)
		{
			base.List.Remove(value);
		}

		// Token: 0x06002D79 RID: 11641 RVA: 0x000CC748 File Offset: 0x000CA948
		protected override void OnValidate(object value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			if (!(value is CounterCreationData))
			{
				throw new ArgumentException(SR.GetString("MustAddCounterCreationData"));
			}
		}
	}
}
