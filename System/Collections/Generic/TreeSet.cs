using System;
using System.Runtime.Serialization;

namespace System.Collections.Generic
{
	// Token: 0x020003C8 RID: 968
	[Serializable]
	internal class TreeSet<T> : SortedSet<T>
	{
		// Token: 0x060024D2 RID: 9426 RVA: 0x000AB6C3 File Offset: 0x000A98C3
		public TreeSet()
		{
		}

		// Token: 0x060024D3 RID: 9427 RVA: 0x000AB6CB File Offset: 0x000A98CB
		public TreeSet(IComparer<T> comparer)
			: base(comparer)
		{
		}

		// Token: 0x060024D4 RID: 9428 RVA: 0x000AB6D4 File Offset: 0x000A98D4
		public TreeSet(ICollection<T> collection)
			: base(collection)
		{
		}

		// Token: 0x060024D5 RID: 9429 RVA: 0x000AB6DD File Offset: 0x000A98DD
		public TreeSet(ICollection<T> collection, IComparer<T> comparer)
			: base(collection, comparer)
		{
		}

		// Token: 0x060024D6 RID: 9430 RVA: 0x000AB6E7 File Offset: 0x000A98E7
		public TreeSet(SerializationInfo siInfo, StreamingContext context)
			: base(siInfo, context)
		{
		}

		// Token: 0x060024D7 RID: 9431 RVA: 0x000AB6F4 File Offset: 0x000A98F4
		internal override bool AddIfNotPresent(T item)
		{
			bool flag = base.AddIfNotPresent(item);
			if (!flag)
			{
				global::System.ThrowHelper.ThrowArgumentException(global::System.ExceptionResource.Argument_AddingDuplicate);
			}
			return flag;
		}
	}
}
