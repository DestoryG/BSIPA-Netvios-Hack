using System;
using System.Diagnostics;

namespace System.Collections.Generic
{
	// Token: 0x020003BD RID: 957
	internal sealed class System_QueueDebugView<T>
	{
		// Token: 0x0600240F RID: 9231 RVA: 0x000A910C File Offset: 0x000A730C
		public System_QueueDebugView(Queue<T> queue)
		{
			if (queue == null)
			{
				throw new ArgumentNullException("queue");
			}
			this.queue = queue;
		}

		// Token: 0x17000919 RID: 2329
		// (get) Token: 0x06002410 RID: 9232 RVA: 0x000A9129 File Offset: 0x000A7329
		[DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
		public T[] Items
		{
			get
			{
				return this.queue.ToArray();
			}
		}

		// Token: 0x04001FF3 RID: 8179
		private Queue<T> queue;
	}
}
