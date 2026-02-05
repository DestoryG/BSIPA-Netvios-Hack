using System;
using System.Collections;

namespace System.Diagnostics
{
	// Token: 0x020004F4 RID: 1268
	internal class OrdinalCaseInsensitiveComparer : IComparer
	{
		// Token: 0x0600301B RID: 12315 RVA: 0x000D9774 File Offset: 0x000D7974
		public int Compare(object a, object b)
		{
			string text = a as string;
			string text2 = b as string;
			if (text != null && text2 != null)
			{
				return string.Compare(text, text2, StringComparison.OrdinalIgnoreCase);
			}
			return Comparer.Default.Compare(a, b);
		}

		// Token: 0x04002876 RID: 10358
		internal static readonly OrdinalCaseInsensitiveComparer Default = new OrdinalCaseInsensitiveComparer();
	}
}
