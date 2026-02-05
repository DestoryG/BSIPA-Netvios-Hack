using System;
using Mono.Collections.Generic;

namespace Mono.Cecil
{
	// Token: 0x02000062 RID: 98
	internal sealed class GenericParameterCollection : Collection<GenericParameter>
	{
		// Token: 0x06000451 RID: 1105 RVA: 0x00011E7E File Offset: 0x0001007E
		internal GenericParameterCollection(IGenericParameterProvider owner)
		{
			this.owner = owner;
		}

		// Token: 0x06000452 RID: 1106 RVA: 0x00011E8D File Offset: 0x0001008D
		internal GenericParameterCollection(IGenericParameterProvider owner, int capacity)
			: base(capacity)
		{
			this.owner = owner;
		}

		// Token: 0x06000453 RID: 1107 RVA: 0x00011E9D File Offset: 0x0001009D
		protected override void OnAdd(GenericParameter item, int index)
		{
			this.UpdateGenericParameter(item, index);
		}

		// Token: 0x06000454 RID: 1108 RVA: 0x00011EA8 File Offset: 0x000100A8
		protected override void OnInsert(GenericParameter item, int index)
		{
			this.UpdateGenericParameter(item, index);
			for (int i = index; i < this.size; i++)
			{
				this.items[i].position = i + 1;
			}
		}

		// Token: 0x06000455 RID: 1109 RVA: 0x00011E9D File Offset: 0x0001009D
		protected override void OnSet(GenericParameter item, int index)
		{
			this.UpdateGenericParameter(item, index);
		}

		// Token: 0x06000456 RID: 1110 RVA: 0x00011EDE File Offset: 0x000100DE
		private void UpdateGenericParameter(GenericParameter item, int index)
		{
			item.owner = this.owner;
			item.position = index;
			item.type = this.owner.GenericParameterType;
		}

		// Token: 0x06000457 RID: 1111 RVA: 0x00011F04 File Offset: 0x00010104
		protected override void OnRemove(GenericParameter item, int index)
		{
			item.owner = null;
			item.position = -1;
			item.type = GenericParameterType.Type;
			for (int i = index + 1; i < this.size; i++)
			{
				this.items[i].position = i - 1;
			}
		}

		// Token: 0x040000D1 RID: 209
		private readonly IGenericParameterProvider owner;
	}
}
