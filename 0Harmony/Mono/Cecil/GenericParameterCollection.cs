using System;
using Mono.Collections.Generic;

namespace Mono.Cecil
{
	// Token: 0x02000116 RID: 278
	internal sealed class GenericParameterCollection : Collection<GenericParameter>
	{
		// Token: 0x060007D5 RID: 2005 RVA: 0x00020578 File Offset: 0x0001E778
		internal GenericParameterCollection(IGenericParameterProvider owner)
		{
			this.owner = owner;
		}

		// Token: 0x060007D6 RID: 2006 RVA: 0x00020587 File Offset: 0x0001E787
		internal GenericParameterCollection(IGenericParameterProvider owner, int capacity)
			: base(capacity)
		{
			this.owner = owner;
		}

		// Token: 0x060007D7 RID: 2007 RVA: 0x00020597 File Offset: 0x0001E797
		protected override void OnAdd(GenericParameter item, int index)
		{
			this.UpdateGenericParameter(item, index);
		}

		// Token: 0x060007D8 RID: 2008 RVA: 0x000205A4 File Offset: 0x0001E7A4
		protected override void OnInsert(GenericParameter item, int index)
		{
			this.UpdateGenericParameter(item, index);
			for (int i = index; i < this.size; i++)
			{
				this.items[i].position = i + 1;
			}
		}

		// Token: 0x060007D9 RID: 2009 RVA: 0x00020597 File Offset: 0x0001E797
		protected override void OnSet(GenericParameter item, int index)
		{
			this.UpdateGenericParameter(item, index);
		}

		// Token: 0x060007DA RID: 2010 RVA: 0x000205DA File Offset: 0x0001E7DA
		private void UpdateGenericParameter(GenericParameter item, int index)
		{
			item.owner = this.owner;
			item.position = index;
			item.type = this.owner.GenericParameterType;
		}

		// Token: 0x060007DB RID: 2011 RVA: 0x00020600 File Offset: 0x0001E800
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

		// Token: 0x040002E3 RID: 739
		private readonly IGenericParameterProvider owner;
	}
}
