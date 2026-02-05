using System;
using System.Collections;
using System.Globalization;

namespace System
{
	// Token: 0x0200005E RID: 94
	[Serializable]
	internal class InvariantComparer : IComparer
	{
		// Token: 0x0600041F RID: 1055 RVA: 0x0001D961 File Offset: 0x0001BB61
		internal InvariantComparer()
		{
			this.m_compareInfo = CultureInfo.InvariantCulture.CompareInfo;
		}

		// Token: 0x06000420 RID: 1056 RVA: 0x0001D97C File Offset: 0x0001BB7C
		public int Compare(object a, object b)
		{
			string text = a as string;
			string text2 = b as string;
			if (text != null && text2 != null)
			{
				return this.m_compareInfo.Compare(text, text2);
			}
			return Comparer.Default.Compare(a, b);
		}

		// Token: 0x0400051B RID: 1307
		private CompareInfo m_compareInfo;

		// Token: 0x0400051C RID: 1308
		internal static readonly InvariantComparer Default = new InvariantComparer();
	}
}
