using System;

namespace Mono.Cecil
{
	// Token: 0x02000051 RID: 81
	public struct CustomAttributeNamedArgument
	{
		// Token: 0x1700004C RID: 76
		// (get) Token: 0x06000338 RID: 824 RVA: 0x0001051F File Offset: 0x0000E71F
		public string Name
		{
			get
			{
				return this.name;
			}
		}

		// Token: 0x1700004D RID: 77
		// (get) Token: 0x06000339 RID: 825 RVA: 0x00010527 File Offset: 0x0000E727
		public CustomAttributeArgument Argument
		{
			get
			{
				return this.argument;
			}
		}

		// Token: 0x0600033A RID: 826 RVA: 0x0001052F File Offset: 0x0000E72F
		public CustomAttributeNamedArgument(string name, CustomAttributeArgument argument)
		{
			Mixin.CheckName(name);
			this.name = name;
			this.argument = argument;
		}

		// Token: 0x04000088 RID: 136
		private readonly string name;

		// Token: 0x04000089 RID: 137
		private readonly CustomAttributeArgument argument;
	}
}
