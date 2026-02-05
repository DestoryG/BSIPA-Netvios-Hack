using System;
using System.Collections;

namespace System.Diagnostics
{
	// Token: 0x020004FD RID: 1277
	public class ProcessModuleCollection : ReadOnlyCollectionBase
	{
		// Token: 0x0600305D RID: 12381 RVA: 0x000DB621 File Offset: 0x000D9821
		protected ProcessModuleCollection()
		{
		}

		// Token: 0x0600305E RID: 12382 RVA: 0x000DB629 File Offset: 0x000D9829
		public ProcessModuleCollection(ProcessModule[] processModules)
		{
			base.InnerList.AddRange(processModules);
		}

		// Token: 0x17000BCF RID: 3023
		public ProcessModule this[int index]
		{
			get
			{
				return (ProcessModule)base.InnerList[index];
			}
		}

		// Token: 0x06003060 RID: 12384 RVA: 0x000DB650 File Offset: 0x000D9850
		public int IndexOf(ProcessModule module)
		{
			return base.InnerList.IndexOf(module);
		}

		// Token: 0x06003061 RID: 12385 RVA: 0x000DB65E File Offset: 0x000D985E
		public bool Contains(ProcessModule module)
		{
			return base.InnerList.Contains(module);
		}

		// Token: 0x06003062 RID: 12386 RVA: 0x000DB66C File Offset: 0x000D986C
		public void CopyTo(ProcessModule[] array, int index)
		{
			base.InnerList.CopyTo(array, index);
		}
	}
}
