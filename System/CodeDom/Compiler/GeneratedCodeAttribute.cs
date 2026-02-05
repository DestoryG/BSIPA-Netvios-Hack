using System;

namespace System.CodeDom.Compiler
{
	// Token: 0x0200067C RID: 1660
	[AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = false)]
	[global::__DynamicallyInvokable]
	public sealed class GeneratedCodeAttribute : Attribute
	{
		// Token: 0x06003D2C RID: 15660 RVA: 0x000FBB2D File Offset: 0x000F9D2D
		[global::__DynamicallyInvokable]
		public GeneratedCodeAttribute(string tool, string version)
		{
			this.tool = tool;
			this.version = version;
		}

		// Token: 0x17000E95 RID: 3733
		// (get) Token: 0x06003D2D RID: 15661 RVA: 0x000FBB43 File Offset: 0x000F9D43
		[global::__DynamicallyInvokable]
		public string Tool
		{
			[global::__DynamicallyInvokable]
			get
			{
				return this.tool;
			}
		}

		// Token: 0x17000E96 RID: 3734
		// (get) Token: 0x06003D2E RID: 15662 RVA: 0x000FBB4B File Offset: 0x000F9D4B
		[global::__DynamicallyInvokable]
		public string Version
		{
			[global::__DynamicallyInvokable]
			get
			{
				return this.version;
			}
		}

		// Token: 0x04002C91 RID: 11409
		private readonly string tool;

		// Token: 0x04002C92 RID: 11410
		private readonly string version;
	}
}
