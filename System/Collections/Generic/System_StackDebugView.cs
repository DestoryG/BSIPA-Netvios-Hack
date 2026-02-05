using System;
using System.Diagnostics;

namespace System.Collections.Generic
{
	// Token: 0x020003BE RID: 958
	internal sealed class System_StackDebugView<T>
	{
		// Token: 0x06002411 RID: 9233 RVA: 0x000A9136 File Offset: 0x000A7336
		public System_StackDebugView(Stack<T> stack)
		{
			if (stack == null)
			{
				throw new ArgumentNullException("stack");
			}
			this.stack = stack;
		}

		// Token: 0x1700091A RID: 2330
		// (get) Token: 0x06002412 RID: 9234 RVA: 0x000A9153 File Offset: 0x000A7353
		[DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
		public T[] Items
		{
			get
			{
				return this.stack.ToArray();
			}
		}

		// Token: 0x04001FF4 RID: 8180
		private Stack<T> stack;
	}
}
