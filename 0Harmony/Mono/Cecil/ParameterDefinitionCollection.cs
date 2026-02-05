using System;
using Mono.Collections.Generic;

namespace Mono.Cecil
{
	// Token: 0x0200015A RID: 346
	internal sealed class ParameterDefinitionCollection : Collection<ParameterDefinition>
	{
		// Token: 0x06000AAD RID: 2733 RVA: 0x00025578 File Offset: 0x00023778
		internal ParameterDefinitionCollection(IMethodSignature method)
		{
			this.method = method;
		}

		// Token: 0x06000AAE RID: 2734 RVA: 0x00025587 File Offset: 0x00023787
		internal ParameterDefinitionCollection(IMethodSignature method, int capacity)
			: base(capacity)
		{
			this.method = method;
		}

		// Token: 0x06000AAF RID: 2735 RVA: 0x00025597 File Offset: 0x00023797
		protected override void OnAdd(ParameterDefinition item, int index)
		{
			item.method = this.method;
			item.index = index;
		}

		// Token: 0x06000AB0 RID: 2736 RVA: 0x000255AC File Offset: 0x000237AC
		protected override void OnInsert(ParameterDefinition item, int index)
		{
			item.method = this.method;
			item.index = index;
			for (int i = index; i < this.size; i++)
			{
				this.items[i].index = i + 1;
			}
		}

		// Token: 0x06000AB1 RID: 2737 RVA: 0x00025597 File Offset: 0x00023797
		protected override void OnSet(ParameterDefinition item, int index)
		{
			item.method = this.method;
			item.index = index;
		}

		// Token: 0x06000AB2 RID: 2738 RVA: 0x000255F0 File Offset: 0x000237F0
		protected override void OnRemove(ParameterDefinition item, int index)
		{
			item.method = null;
			item.index = -1;
			for (int i = index + 1; i < this.size; i++)
			{
				this.items[i].index = i - 1;
			}
		}

		// Token: 0x0400043C RID: 1084
		private readonly IMethodSignature method;
	}
}
