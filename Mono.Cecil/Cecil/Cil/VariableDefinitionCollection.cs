using System;
using Mono.Collections.Generic;

namespace Mono.Cecil.Cil
{
	// Token: 0x02000100 RID: 256
	internal sealed class VariableDefinitionCollection : Collection<VariableDefinition>
	{
		// Token: 0x06000A64 RID: 2660 RVA: 0x000218E3 File Offset: 0x0001FAE3
		internal VariableDefinitionCollection()
		{
		}

		// Token: 0x06000A65 RID: 2661 RVA: 0x000218EB File Offset: 0x0001FAEB
		internal VariableDefinitionCollection(int capacity)
			: base(capacity)
		{
		}

		// Token: 0x06000A66 RID: 2662 RVA: 0x000218F4 File Offset: 0x0001FAF4
		protected override void OnAdd(VariableDefinition item, int index)
		{
			item.index = index;
		}

		// Token: 0x06000A67 RID: 2663 RVA: 0x00021900 File Offset: 0x0001FB00
		protected override void OnInsert(VariableDefinition item, int index)
		{
			item.index = index;
			for (int i = index; i < this.size; i++)
			{
				this.items[i].index = i + 1;
			}
		}

		// Token: 0x06000A68 RID: 2664 RVA: 0x000218F4 File Offset: 0x0001FAF4
		protected override void OnSet(VariableDefinition item, int index)
		{
			item.index = index;
		}

		// Token: 0x06000A69 RID: 2665 RVA: 0x00021938 File Offset: 0x0001FB38
		protected override void OnRemove(VariableDefinition item, int index)
		{
			item.index = -1;
			for (int i = index + 1; i < this.size; i++)
			{
				this.items[i].index = i - 1;
			}
		}
	}
}
