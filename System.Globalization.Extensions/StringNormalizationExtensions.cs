using System;
using System.Text;

namespace System
{
	// Token: 0x02000003 RID: 3
	public static class StringNormalizationExtensions
	{
		// Token: 0x06000002 RID: 2 RVA: 0x00002058 File Offset: 0x00000258
		public static bool IsNormalized(this string strInput)
		{
			return strInput.IsNormalized(NormalizationForm.FormC);
		}

		// Token: 0x06000003 RID: 3 RVA: 0x00002061 File Offset: 0x00000261
		public static bool IsNormalized(this string strInput, NormalizationForm normalizationForm)
		{
			if (strInput == null)
			{
				throw new ArgumentNullException("strInput");
			}
			return strInput.IsNormalized(normalizationForm);
		}

		// Token: 0x06000004 RID: 4 RVA: 0x00002078 File Offset: 0x00000278
		public static string Normalize(this string strInput)
		{
			return strInput.Normalize(NormalizationForm.FormC);
		}

		// Token: 0x06000005 RID: 5 RVA: 0x00002081 File Offset: 0x00000281
		public static string Normalize(this string strInput, NormalizationForm normalizationForm)
		{
			if (strInput == null)
			{
				throw new ArgumentNullException("strInput");
			}
			return strInput.Normalize(normalizationForm);
		}
	}
}
