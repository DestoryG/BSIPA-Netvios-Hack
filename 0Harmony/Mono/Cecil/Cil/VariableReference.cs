using System;

namespace Mono.Cecil.Cil
{
	// Token: 0x020001FB RID: 507
	internal abstract class VariableReference
	{
		// Token: 0x1700034B RID: 843
		// (get) Token: 0x06000F54 RID: 3924 RVA: 0x00033CCF File Offset: 0x00031ECF
		// (set) Token: 0x06000F55 RID: 3925 RVA: 0x00033CD7 File Offset: 0x00031ED7
		public TypeReference VariableType
		{
			get
			{
				return this.variable_type;
			}
			set
			{
				this.variable_type = value;
			}
		}

		// Token: 0x1700034C RID: 844
		// (get) Token: 0x06000F56 RID: 3926 RVA: 0x00033CE0 File Offset: 0x00031EE0
		public int Index
		{
			get
			{
				return this.index;
			}
		}

		// Token: 0x06000F57 RID: 3927 RVA: 0x00033CE8 File Offset: 0x00031EE8
		internal VariableReference(TypeReference variable_type)
		{
			this.variable_type = variable_type;
		}

		// Token: 0x06000F58 RID: 3928
		public abstract VariableDefinition Resolve();

		// Token: 0x06000F59 RID: 3929 RVA: 0x00033CFE File Offset: 0x00031EFE
		public override string ToString()
		{
			if (this.index >= 0)
			{
				return "V_" + this.index.ToString();
			}
			return string.Empty;
		}

		// Token: 0x0400094F RID: 2383
		internal int index = -1;

		// Token: 0x04000950 RID: 2384
		protected TypeReference variable_type;
	}
}
