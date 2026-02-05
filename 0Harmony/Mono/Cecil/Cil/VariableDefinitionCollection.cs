using System;
using Mono.Collections.Generic;

namespace Mono.Cecil.Cil
{
	// Token: 0x020001C4 RID: 452
	internal sealed class VariableDefinitionCollection : Collection<VariableDefinition>
	{
		// Token: 0x06000E4B RID: 3659 RVA: 0x00030AF2 File Offset: 0x0002ECF2
		internal VariableDefinitionCollection()
		{
		}

		// Token: 0x06000E4C RID: 3660 RVA: 0x00030AFA File Offset: 0x0002ECFA
		internal VariableDefinitionCollection(int capacity)
			: base(capacity)
		{
		}

		// Token: 0x06000E4D RID: 3661 RVA: 0x00030B03 File Offset: 0x0002ED03
		protected override void OnAdd(VariableDefinition item, int index)
		{
			item.index = index;
		}

		// Token: 0x06000E4E RID: 3662 RVA: 0x00030B0C File Offset: 0x0002ED0C
		protected override void OnInsert(VariableDefinition item, int index)
		{
			item.index = index;
			for (int i = index; i < this.size; i++)
			{
				this.items[i].index = i + 1;
			}
		}

		// Token: 0x06000E4F RID: 3663 RVA: 0x00030B03 File Offset: 0x0002ED03
		protected override void OnSet(VariableDefinition item, int index)
		{
			item.index = index;
		}

		// Token: 0x06000E50 RID: 3664 RVA: 0x00030B44 File Offset: 0x0002ED44
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
