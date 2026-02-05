using System;

namespace System.Runtime.Serialization
{
	// Token: 0x02000084 RID: 132
	internal class ExtensionDataMember
	{
		// Token: 0x17000164 RID: 356
		// (get) Token: 0x0600096B RID: 2411 RVA: 0x0002A375 File Offset: 0x00028575
		// (set) Token: 0x0600096C RID: 2412 RVA: 0x0002A37D File Offset: 0x0002857D
		public string Name
		{
			get
			{
				return this.name;
			}
			set
			{
				this.name = value;
			}
		}

		// Token: 0x17000165 RID: 357
		// (get) Token: 0x0600096D RID: 2413 RVA: 0x0002A386 File Offset: 0x00028586
		// (set) Token: 0x0600096E RID: 2414 RVA: 0x0002A38E File Offset: 0x0002858E
		public string Namespace
		{
			get
			{
				return this.ns;
			}
			set
			{
				this.ns = value;
			}
		}

		// Token: 0x17000166 RID: 358
		// (get) Token: 0x0600096F RID: 2415 RVA: 0x0002A397 File Offset: 0x00028597
		// (set) Token: 0x06000970 RID: 2416 RVA: 0x0002A39F File Offset: 0x0002859F
		public IDataNode Value
		{
			get
			{
				return this.value;
			}
			set
			{
				this.value = value;
			}
		}

		// Token: 0x17000167 RID: 359
		// (get) Token: 0x06000971 RID: 2417 RVA: 0x0002A3A8 File Offset: 0x000285A8
		// (set) Token: 0x06000972 RID: 2418 RVA: 0x0002A3B0 File Offset: 0x000285B0
		public int MemberIndex
		{
			get
			{
				return this.memberIndex;
			}
			set
			{
				this.memberIndex = value;
			}
		}

		// Token: 0x0400039B RID: 923
		private string name;

		// Token: 0x0400039C RID: 924
		private string ns;

		// Token: 0x0400039D RID: 925
		private IDataNode value;

		// Token: 0x0400039E RID: 926
		private int memberIndex;
	}
}
