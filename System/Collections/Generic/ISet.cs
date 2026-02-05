using System;

namespace System.Collections.Generic
{
	// Token: 0x020003CE RID: 974
	[global::__DynamicallyInvokable]
	public interface ISet<T> : ICollection<T>, IEnumerable<T>, IEnumerable
	{
		// Token: 0x0600253B RID: 9531
		[global::__DynamicallyInvokable]
		bool Add(T item);

		// Token: 0x0600253C RID: 9532
		[global::__DynamicallyInvokable]
		void UnionWith(IEnumerable<T> other);

		// Token: 0x0600253D RID: 9533
		[global::__DynamicallyInvokable]
		void IntersectWith(IEnumerable<T> other);

		// Token: 0x0600253E RID: 9534
		[global::__DynamicallyInvokable]
		void ExceptWith(IEnumerable<T> other);

		// Token: 0x0600253F RID: 9535
		[global::__DynamicallyInvokable]
		void SymmetricExceptWith(IEnumerable<T> other);

		// Token: 0x06002540 RID: 9536
		[global::__DynamicallyInvokable]
		bool IsSubsetOf(IEnumerable<T> other);

		// Token: 0x06002541 RID: 9537
		[global::__DynamicallyInvokable]
		bool IsSupersetOf(IEnumerable<T> other);

		// Token: 0x06002542 RID: 9538
		[global::__DynamicallyInvokable]
		bool IsProperSupersetOf(IEnumerable<T> other);

		// Token: 0x06002543 RID: 9539
		[global::__DynamicallyInvokable]
		bool IsProperSubsetOf(IEnumerable<T> other);

		// Token: 0x06002544 RID: 9540
		[global::__DynamicallyInvokable]
		bool Overlaps(IEnumerable<T> other);

		// Token: 0x06002545 RID: 9541
		[global::__DynamicallyInvokable]
		bool SetEquals(IEnumerable<T> other);
	}
}
