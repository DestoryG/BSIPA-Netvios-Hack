using System;

namespace Mono.Cecil
{
	// Token: 0x02000101 RID: 257
	internal struct CustomAttributeNamedArgument
	{
		// Token: 0x17000062 RID: 98
		// (get) Token: 0x060006AC RID: 1708 RVA: 0x0001EAD7 File Offset: 0x0001CCD7
		public string Name
		{
			get
			{
				return this.name;
			}
		}

		// Token: 0x17000063 RID: 99
		// (get) Token: 0x060006AD RID: 1709 RVA: 0x0001EADF File Offset: 0x0001CCDF
		public CustomAttributeArgument Argument
		{
			get
			{
				return this.argument;
			}
		}

		// Token: 0x060006AE RID: 1710 RVA: 0x0001EAE7 File Offset: 0x0001CCE7
		public CustomAttributeNamedArgument(string name, CustomAttributeArgument argument)
		{
			Mixin.CheckName(name);
			this.name = name;
			this.argument = argument;
		}

		// Token: 0x04000290 RID: 656
		private readonly string name;

		// Token: 0x04000291 RID: 657
		private readonly CustomAttributeArgument argument;
	}
}
