using System;
using System.Collections;

namespace System.Net
{
	// Token: 0x020000CE RID: 206
	internal sealed class InterlockedStack
	{
		// Token: 0x060006E1 RID: 1761 RVA: 0x00025FB1 File Offset: 0x000241B1
		internal InterlockedStack()
		{
		}

		// Token: 0x060006E2 RID: 1762 RVA: 0x00025FC4 File Offset: 0x000241C4
		internal void Push(object pooledStream)
		{
			if (pooledStream == null)
			{
				throw new ArgumentNullException("pooledStream");
			}
			object syncRoot = this._stack.SyncRoot;
			lock (syncRoot)
			{
				this._stack.Push(pooledStream);
				this._count = this._stack.Count;
			}
		}

		// Token: 0x060006E3 RID: 1763 RVA: 0x00026030 File Offset: 0x00024230
		internal object Pop()
		{
			object syncRoot = this._stack.SyncRoot;
			object obj2;
			lock (syncRoot)
			{
				object obj = null;
				if (0 < this._stack.Count)
				{
					obj = this._stack.Pop();
					this._count = this._stack.Count;
				}
				obj2 = obj;
			}
			return obj2;
		}

		// Token: 0x04000CB2 RID: 3250
		private readonly Stack _stack = new Stack();

		// Token: 0x04000CB3 RID: 3251
		private int _count;
	}
}
