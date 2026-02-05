using System;

namespace System.Runtime.Serialization
{
	// Token: 0x0200008B RID: 139
	internal class ISerializableDataMember
	{
		// Token: 0x17000185 RID: 389
		// (get) Token: 0x060009BC RID: 2492 RVA: 0x0002A7D6 File Offset: 0x000289D6
		// (set) Token: 0x060009BD RID: 2493 RVA: 0x0002A7DE File Offset: 0x000289DE
		internal string Name
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

		// Token: 0x17000186 RID: 390
		// (get) Token: 0x060009BE RID: 2494 RVA: 0x0002A7E7 File Offset: 0x000289E7
		// (set) Token: 0x060009BF RID: 2495 RVA: 0x0002A7EF File Offset: 0x000289EF
		internal IDataNode Value
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

		// Token: 0x040003B2 RID: 946
		private string name;

		// Token: 0x040003B3 RID: 947
		private IDataNode value;
	}
}
