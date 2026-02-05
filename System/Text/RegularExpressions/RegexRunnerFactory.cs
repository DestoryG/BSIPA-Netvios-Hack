using System;
using System.ComponentModel;

namespace System.Text.RegularExpressions
{
	// Token: 0x020006A6 RID: 1702
	[EditorBrowsable(EditorBrowsableState.Never)]
	public abstract class RegexRunnerFactory
	{
		// Token: 0x06003FB6 RID: 16310
		protected internal abstract RegexRunner CreateInstance();
	}
}
