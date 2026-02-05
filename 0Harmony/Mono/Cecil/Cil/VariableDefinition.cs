using System;

namespace Mono.Cecil.Cil
{
	// Token: 0x020001FA RID: 506
	internal sealed class VariableDefinition : VariableReference
	{
		// Token: 0x1700034A RID: 842
		// (get) Token: 0x06000F51 RID: 3921 RVA: 0x00033CB9 File Offset: 0x00031EB9
		public bool IsPinned
		{
			get
			{
				return this.variable_type.IsPinned;
			}
		}

		// Token: 0x06000F52 RID: 3922 RVA: 0x00033CC6 File Offset: 0x00031EC6
		public VariableDefinition(TypeReference variableType)
			: base(variableType)
		{
		}

		// Token: 0x06000F53 RID: 3923 RVA: 0x00010978 File Offset: 0x0000EB78
		public override VariableDefinition Resolve()
		{
			return this;
		}
	}
}
