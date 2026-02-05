using System;
using System.Threading;

namespace System.Text.RegularExpressions
{
	// Token: 0x02000689 RID: 1673
	internal sealed class ExclusiveReference
	{
		// Token: 0x06003DDB RID: 15835 RVA: 0x000FD4B0 File Offset: 0x000FB6B0
		internal object Get()
		{
			if (Interlocked.Exchange(ref this._locked, 1) != 0)
			{
				return null;
			}
			object @ref = this._ref;
			if (@ref == null)
			{
				this._locked = 0;
				return null;
			}
			this._obj = @ref;
			return @ref;
		}

		// Token: 0x06003DDC RID: 15836 RVA: 0x000FD4E8 File Offset: 0x000FB6E8
		internal void Release(object obj)
		{
			if (obj == null)
			{
				throw new ArgumentNullException("obj");
			}
			if (this._obj == obj)
			{
				this._obj = null;
				this._locked = 0;
				return;
			}
			if (this._obj == null && Interlocked.Exchange(ref this._locked, 1) == 0)
			{
				if (this._ref == null)
				{
					this._ref = (RegexRunner)obj;
				}
				this._locked = 0;
				return;
			}
		}

		// Token: 0x04002CE4 RID: 11492
		private RegexRunner _ref;

		// Token: 0x04002CE5 RID: 11493
		private object _obj;

		// Token: 0x04002CE6 RID: 11494
		private int _locked;
	}
}
