using System;
using System.Collections;

namespace System.Diagnostics
{
	// Token: 0x02000501 RID: 1281
	public class ProcessThreadCollection : ReadOnlyCollectionBase
	{
		// Token: 0x060030A6 RID: 12454 RVA: 0x000DBE44 File Offset: 0x000DA044
		protected ProcessThreadCollection()
		{
		}

		// Token: 0x060030A7 RID: 12455 RVA: 0x000DBE4C File Offset: 0x000DA04C
		public ProcessThreadCollection(ProcessThread[] processThreads)
		{
			base.InnerList.AddRange(processThreads);
		}

		// Token: 0x17000BF4 RID: 3060
		public ProcessThread this[int index]
		{
			get
			{
				return (ProcessThread)base.InnerList[index];
			}
		}

		// Token: 0x060030A9 RID: 12457 RVA: 0x000DBE73 File Offset: 0x000DA073
		public int Add(ProcessThread thread)
		{
			return base.InnerList.Add(thread);
		}

		// Token: 0x060030AA RID: 12458 RVA: 0x000DBE81 File Offset: 0x000DA081
		public void Insert(int index, ProcessThread thread)
		{
			base.InnerList.Insert(index, thread);
		}

		// Token: 0x060030AB RID: 12459 RVA: 0x000DBE90 File Offset: 0x000DA090
		public int IndexOf(ProcessThread thread)
		{
			return base.InnerList.IndexOf(thread);
		}

		// Token: 0x060030AC RID: 12460 RVA: 0x000DBE9E File Offset: 0x000DA09E
		public bool Contains(ProcessThread thread)
		{
			return base.InnerList.Contains(thread);
		}

		// Token: 0x060030AD RID: 12461 RVA: 0x000DBEAC File Offset: 0x000DA0AC
		public void Remove(ProcessThread thread)
		{
			base.InnerList.Remove(thread);
		}

		// Token: 0x060030AE RID: 12462 RVA: 0x000DBEBA File Offset: 0x000DA0BA
		public void CopyTo(ProcessThread[] array, int index)
		{
			base.InnerList.CopyTo(array, index);
		}
	}
}
