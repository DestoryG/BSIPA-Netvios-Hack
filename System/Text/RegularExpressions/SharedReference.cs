using System;
using System.Threading;

namespace System.Text.RegularExpressions
{
	// Token: 0x0200068A RID: 1674
	internal sealed class SharedReference
	{
		// Token: 0x06003DDE RID: 15838 RVA: 0x000FD558 File Offset: 0x000FB758
		internal object Get()
		{
			if (Interlocked.Exchange(ref this._locked, 1) == 0)
			{
				object target = this._ref.Target;
				this._locked = 0;
				return target;
			}
			return null;
		}

		// Token: 0x06003DDF RID: 15839 RVA: 0x000FD589 File Offset: 0x000FB789
		internal void Cache(object obj)
		{
			if (Interlocked.Exchange(ref this._locked, 1) == 0)
			{
				this._ref.Target = obj;
				this._locked = 0;
			}
		}

		// Token: 0x04002CE7 RID: 11495
		private WeakReference _ref = new WeakReference(null);

		// Token: 0x04002CE8 RID: 11496
		private int _locked;
	}
}
