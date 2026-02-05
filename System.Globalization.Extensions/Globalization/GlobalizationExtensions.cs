using System;

namespace System.Globalization
{
	// Token: 0x02000004 RID: 4
	public static class GlobalizationExtensions
	{
		// Token: 0x06000006 RID: 6 RVA: 0x00002098 File Offset: 0x00000298
		public static StringComparer GetStringComparer(this CompareInfo compareInfo, CompareOptions options)
		{
			if (compareInfo == null)
			{
				throw new ArgumentNullException("compareInfo");
			}
			if (options == CompareOptions.Ordinal)
			{
				return StringComparer.Ordinal;
			}
			if (options == CompareOptions.OrdinalIgnoreCase)
			{
				return StringComparer.OrdinalIgnoreCase;
			}
			if ((options & ~(CompareOptions.IgnoreCase | CompareOptions.IgnoreNonSpace | CompareOptions.IgnoreSymbols | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth | CompareOptions.StringSort)) != CompareOptions.None)
			{
				throw new ArgumentException("Value of flags is invalid.", "options");
			}
			return new CultureAwareComparer(compareInfo, options);
		}
	}
}
