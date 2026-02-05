using System;

namespace Mono.Cecil
{
	// Token: 0x02000100 RID: 256
	internal struct CustomAttributeArgument
	{
		// Token: 0x17000060 RID: 96
		// (get) Token: 0x060006A9 RID: 1705 RVA: 0x0001EAB1 File Offset: 0x0001CCB1
		public TypeReference Type
		{
			get
			{
				return this.type;
			}
		}

		// Token: 0x17000061 RID: 97
		// (get) Token: 0x060006AA RID: 1706 RVA: 0x0001EAB9 File Offset: 0x0001CCB9
		public object Value
		{
			get
			{
				return this.value;
			}
		}

		// Token: 0x060006AB RID: 1707 RVA: 0x0001EAC1 File Offset: 0x0001CCC1
		public CustomAttributeArgument(TypeReference type, object value)
		{
			Mixin.CheckType(type);
			this.type = type;
			this.value = value;
		}

		// Token: 0x0400028E RID: 654
		private readonly TypeReference type;

		// Token: 0x0400028F RID: 655
		private readonly object value;
	}
}
