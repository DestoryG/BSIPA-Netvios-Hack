using System;
using System.ComponentModel;
using System.Security;
using System.Text;

namespace System
{
	// Token: 0x02000038 RID: 56
	[EditorBrowsable(EditorBrowsableState.Never)]
	public static class StringNormalizationExtensions
	{
		// Token: 0x060002D8 RID: 728 RVA: 0x00010DB1 File Offset: 0x0000EFB1
		[EditorBrowsable(EditorBrowsableState.Never)]
		public static bool IsNormalized(this string value)
		{
			return value.IsNormalized(NormalizationForm.FormC);
		}

		// Token: 0x060002D9 RID: 729 RVA: 0x00010DBA File Offset: 0x0000EFBA
		[SecurityCritical]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public static bool IsNormalized(this string value, NormalizationForm normalizationForm)
		{
			return value.IsNormalized(normalizationForm);
		}

		// Token: 0x060002DA RID: 730 RVA: 0x00010DC3 File Offset: 0x0000EFC3
		[EditorBrowsable(EditorBrowsableState.Never)]
		public static string Normalize(this string value)
		{
			return value.Normalize(NormalizationForm.FormC);
		}

		// Token: 0x060002DB RID: 731 RVA: 0x00010DCC File Offset: 0x0000EFCC
		[SecurityCritical]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public static string Normalize(this string value, NormalizationForm normalizationForm)
		{
			return value.Normalize(normalizationForm);
		}
	}
}
