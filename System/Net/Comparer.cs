using System;
using System.Collections;

namespace System.Net
{
	// Token: 0x020000D5 RID: 213
	internal class Comparer : IComparer
	{
		// Token: 0x06000737 RID: 1847 RVA: 0x00027EB4 File Offset: 0x000260B4
		int IComparer.Compare(object ol, object or)
		{
			Cookie cookie = (Cookie)ol;
			Cookie cookie2 = (Cookie)or;
			int num;
			if ((num = string.Compare(cookie.Name, cookie2.Name, StringComparison.OrdinalIgnoreCase)) != 0)
			{
				return num;
			}
			if ((num = string.Compare(cookie.Domain, cookie2.Domain, StringComparison.OrdinalIgnoreCase)) != 0)
			{
				return num;
			}
			if ((num = string.Compare(cookie.Path, cookie2.Path, StringComparison.Ordinal)) != 0)
			{
				return num;
			}
			return 0;
		}
	}
}
