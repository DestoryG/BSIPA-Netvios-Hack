using System;
using Mono.Collections.Generic;

namespace Mono.Cecil
{
	// Token: 0x020000A0 RID: 160
	internal sealed class ParameterDefinitionCollection : Collection<ParameterDefinition>
	{
		// Token: 0x060006F6 RID: 1782 RVA: 0x000169A5 File Offset: 0x00014BA5
		internal ParameterDefinitionCollection(IMethodSignature method)
		{
			this.method = method;
		}

		// Token: 0x060006F7 RID: 1783 RVA: 0x000169B4 File Offset: 0x00014BB4
		internal ParameterDefinitionCollection(IMethodSignature method, int capacity)
			: base(capacity)
		{
			this.method = method;
		}

		// Token: 0x060006F8 RID: 1784 RVA: 0x000169C4 File Offset: 0x00014BC4
		protected override void OnAdd(ParameterDefinition item, int index)
		{
			item.method = this.method;
			item.index = index;
		}

		// Token: 0x060006F9 RID: 1785 RVA: 0x000169DC File Offset: 0x00014BDC
		protected override void OnInsert(ParameterDefinition item, int index)
		{
			item.method = this.method;
			item.index = index;
			for (int i = index; i < this.size; i++)
			{
				this.items[i].index = i + 1;
			}
		}

		// Token: 0x060006FA RID: 1786 RVA: 0x000169C4 File Offset: 0x00014BC4
		protected override void OnSet(ParameterDefinition item, int index)
		{
			item.method = this.method;
			item.index = index;
		}

		// Token: 0x060006FB RID: 1787 RVA: 0x00016A20 File Offset: 0x00014C20
		protected override void OnRemove(ParameterDefinition item, int index)
		{
			item.method = null;
			item.index = -1;
			for (int i = index + 1; i < this.size; i++)
			{
				this.items[i].index = i - 1;
			}
		}

		// Token: 0x04000204 RID: 516
		private readonly IMethodSignature method;
	}
}
