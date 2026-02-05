using System;

namespace Mono.Cecil.Cil
{
	// Token: 0x02000135 RID: 309
	public sealed class VariableDefinition : VariableReference
	{
		// Token: 0x17000328 RID: 808
		// (get) Token: 0x06000B61 RID: 2913 RVA: 0x000248F9 File Offset: 0x00022AF9
		public bool IsPinned
		{
			get
			{
				return this.variable_type.IsPinned;
			}
		}

		// Token: 0x06000B62 RID: 2914 RVA: 0x00024906 File Offset: 0x00022B06
		public VariableDefinition(TypeReference variableType)
			: base(variableType)
		{
		}

		// Token: 0x06000B63 RID: 2915 RVA: 0x00002740 File Offset: 0x00000940
		public override VariableDefinition Resolve()
		{
			return this;
		}
	}
}
