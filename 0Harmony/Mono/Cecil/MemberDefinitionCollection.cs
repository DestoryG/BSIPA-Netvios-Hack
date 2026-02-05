using System;
using Mono.Collections.Generic;

namespace Mono.Cecil
{
	// Token: 0x02000136 RID: 310
	internal sealed class MemberDefinitionCollection<T> : Collection<T> where T : IMemberDefinition
	{
		// Token: 0x0600087A RID: 2170 RVA: 0x00021A8C File Offset: 0x0001FC8C
		internal MemberDefinitionCollection(TypeDefinition container)
		{
			this.container = container;
		}

		// Token: 0x0600087B RID: 2171 RVA: 0x00021A9B File Offset: 0x0001FC9B
		internal MemberDefinitionCollection(TypeDefinition container, int capacity)
			: base(capacity)
		{
			this.container = container;
		}

		// Token: 0x0600087C RID: 2172 RVA: 0x00021AAB File Offset: 0x0001FCAB
		protected override void OnAdd(T item, int index)
		{
			this.Attach(item);
		}

		// Token: 0x0600087D RID: 2173 RVA: 0x00021AAB File Offset: 0x0001FCAB
		protected sealed override void OnSet(T item, int index)
		{
			this.Attach(item);
		}

		// Token: 0x0600087E RID: 2174 RVA: 0x00021AAB File Offset: 0x0001FCAB
		protected sealed override void OnInsert(T item, int index)
		{
			this.Attach(item);
		}

		// Token: 0x0600087F RID: 2175 RVA: 0x00021AB4 File Offset: 0x0001FCB4
		protected sealed override void OnRemove(T item, int index)
		{
			MemberDefinitionCollection<T>.Detach(item);
		}

		// Token: 0x06000880 RID: 2176 RVA: 0x00021ABC File Offset: 0x0001FCBC
		protected sealed override void OnClear()
		{
			foreach (T t in this)
			{
				MemberDefinitionCollection<T>.Detach(t);
			}
		}

		// Token: 0x06000881 RID: 2177 RVA: 0x00021B08 File Offset: 0x0001FD08
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

		// Token: 0x06000882 RID: 2178 RVA: 0x00021B58 File Offset: 0x0001FD58
		private static void Detach(T element)
		{
			element.DeclaringType = null;
		}

		// Token: 0x04000313 RID: 787
		private TypeDefinition container;
	}
}
