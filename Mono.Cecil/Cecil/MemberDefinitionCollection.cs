using System;
using Mono.Collections.Generic;

namespace Mono.Cecil
{
	// Token: 0x0200007F RID: 127
	internal sealed class MemberDefinitionCollection<T> : Collection<T> where T : IMemberDefinition
	{
		// Token: 0x060004E8 RID: 1256 RVA: 0x00013260 File Offset: 0x00011460
		internal MemberDefinitionCollection(TypeDefinition container)
		{
			this.container = container;
		}

		// Token: 0x060004E9 RID: 1257 RVA: 0x0001326F File Offset: 0x0001146F
		internal MemberDefinitionCollection(TypeDefinition container, int capacity)
			: base(capacity)
		{
			this.container = container;
		}

		// Token: 0x060004EA RID: 1258 RVA: 0x0001327F File Offset: 0x0001147F
		protected override void OnAdd(T item, int index)
		{
			this.Attach(item);
		}

		// Token: 0x060004EB RID: 1259 RVA: 0x0001327F File Offset: 0x0001147F
		protected sealed override void OnSet(T item, int index)
		{
			this.Attach(item);
		}

		// Token: 0x060004EC RID: 1260 RVA: 0x0001327F File Offset: 0x0001147F
		protected sealed override void OnInsert(T item, int index)
		{
			this.Attach(item);
		}

		// Token: 0x060004ED RID: 1261 RVA: 0x00013288 File Offset: 0x00011488
		protected sealed override void OnRemove(T item, int index)
		{
			MemberDefinitionCollection<T>.Detach(item);
		}

		// Token: 0x060004EE RID: 1262 RVA: 0x00013290 File Offset: 0x00011490
		protected sealed override void OnClear()
		{
			foreach (T t in this)
			{
				MemberDefinitionCollection<T>.Detach(t);
			}
		}

		// Token: 0x060004EF RID: 1263 RVA: 0x000132DC File Offset: 0x000114DC
		private void Attach(T element)
		{
			if (element.DeclaringType == this.container)
			{
				return;
			}
			if (element.DeclaringType != null)
			{
				throw new ArgumentException("Member already attached");
			}
			element.DeclaringType = this.container;
		}

		// Token: 0x060004F0 RID: 1264 RVA: 0x0001332C File Offset: 0x0001152C
		private static void Detach(T element)
		{
			element.DeclaringType = null;
		}

		// Token: 0x040000F9 RID: 249
		private TypeDefinition container;
	}
}
