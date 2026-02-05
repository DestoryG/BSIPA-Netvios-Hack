using System;

namespace Mono.Cecil
{
	// Token: 0x02000050 RID: 80
	public struct CustomAttributeArgument
	{
		// Token: 0x1700004A RID: 74
		// (get) Token: 0x06000335 RID: 821 RVA: 0x000104F9 File Offset: 0x0000E6F9
		public TypeReference Type
		{
			get
			{
				return this.type;
			}
		}

		// Token: 0x1700004B RID: 75
		// (get) Token: 0x06000336 RID: 822 RVA: 0x00010501 File Offset: 0x0000E701
		public object Value
		{
			get
			{
				return this.value;
			}
		}

		// Token: 0x06000337 RID: 823 RVA: 0x00010509 File Offset: 0x0000E709
		public CustomAttributeArgument(TypeReference type, object value)
		{
			Mixin.CheckType(type);
			this.type = type;
			this.value = value;
		}

		// Token: 0x04000086 RID: 134
		private readonly TypeReference type;

		// Token: 0x04000087 RID: 135
		private readonly object value;
	}
}
