using System;

namespace Mono.Cecil.Cil
{
	// Token: 0x02000136 RID: 310
	public abstract class VariableReference
	{
		// Token: 0x17000329 RID: 809
		// (get) Token: 0x06000B64 RID: 2916 RVA: 0x0002490F File Offset: 0x00022B0F
		// (set) Token: 0x06000B65 RID: 2917 RVA: 0x00024917 File Offset: 0x00022B17
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

		// Token: 0x1700032A RID: 810
		// (get) Token: 0x06000B66 RID: 2918 RVA: 0x00024920 File Offset: 0x00022B20
		public int Index
		{
			get
			{
				return this.index;
			}
		}

		// Token: 0x06000B67 RID: 2919 RVA: 0x00024928 File Offset: 0x00022B28
		internal VariableReference(TypeReference variable_type)
		{
			this.variable_type = variable_type;
		}

		// Token: 0x06000B68 RID: 2920
		public abstract VariableDefinition Resolve();

		// Token: 0x06000B69 RID: 2921 RVA: 0x0002493E File Offset: 0x00022B3E
		public override string ToString()
		{
			if (this.index >= 0)
			{
				return "V_" + this.index;
			}
			return string.Empty;
		}

		// Token: 0x040006E8 RID: 1768
		internal int index = -1;

		// Token: 0x040006E9 RID: 1769
		protected TypeReference variable_type;
	}
}
