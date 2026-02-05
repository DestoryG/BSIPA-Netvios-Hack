using System;

namespace Mono.Cecil.Cil
{
	// Token: 0x0200011C RID: 284
	public sealed class VariableDebugInformation : DebugInformation
	{
		// Token: 0x17000300 RID: 768
		// (get) Token: 0x06000AF1 RID: 2801 RVA: 0x00023DB9 File Offset: 0x00021FB9
		public int Index
		{
			get
			{
				return this.index.Index;
			}
		}

		// Token: 0x17000301 RID: 769
		// (get) Token: 0x06000AF2 RID: 2802 RVA: 0x00023DC6 File Offset: 0x00021FC6
		// (set) Token: 0x06000AF3 RID: 2803 RVA: 0x00023DCE File Offset: 0x00021FCE
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

		// Token: 0x17000302 RID: 770
		// (get) Token: 0x06000AF4 RID: 2804 RVA: 0x00023DD7 File Offset: 0x00021FD7
		// (set) Token: 0x06000AF5 RID: 2805 RVA: 0x00023DDF File Offset: 0x00021FDF
		public VariableAttributes Attributes
		{
			get
			{
				return (VariableAttributes)this.attributes;
			}
			set
			{
				this.attributes = (ushort)value;
			}
		}

		// Token: 0x17000303 RID: 771
		// (get) Token: 0x06000AF6 RID: 2806 RVA: 0x00023DE8 File Offset: 0x00021FE8
		// (set) Token: 0x06000AF7 RID: 2807 RVA: 0x00023DF6 File Offset: 0x00021FF6
		public bool IsDebuggerHidden
		{
			get
			{
				return this.attributes.GetAttributes(1);
			}
			set
			{
				this.attributes = this.attributes.SetAttributes(1, value);
			}
		}

		// Token: 0x06000AF8 RID: 2808 RVA: 0x00023E0B File Offset: 0x0002200B
		internal VariableDebugInformation(int index, string name)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			this.index = new VariableIndex(index);
			this.name = name;
		}

		// Token: 0x06000AF9 RID: 2809 RVA: 0x00023E34 File Offset: 0x00022034
		public VariableDebugInformation(VariableDefinition variable, string name)
		{
			if (variable == null)
			{
				throw new ArgumentNullException("variable");
			}
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			this.index = new VariableIndex(variable);
			this.name = name;
			this.token = new MetadataToken(TokenType.LocalVariable);
		}

		// Token: 0x040006AD RID: 1709
		private string name;

		// Token: 0x040006AE RID: 1710
		private ushort attributes;

		// Token: 0x040006AF RID: 1711
		internal VariableIndex index;
	}
}
