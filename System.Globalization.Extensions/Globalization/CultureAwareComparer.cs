using System;

namespace System.Globalization
{
	// Token: 0x02000005 RID: 5
	[Serializable]
	internal sealed class CultureAwareComparer : StringComparer
	{
		// Token: 0x06000007 RID: 7 RVA: 0x000020EF File Offset: 0x000002EF
		internal CultureAwareComparer(CompareInfo compareInfo, CompareOptions options)
		{
			this._compareInfo = compareInfo;
			this._options = options;
		}

		// Token: 0x06000008 RID: 8 RVA: 0x00002105 File Offset: 0x00000305
		public override int Compare(string x, string y)
		{
			if (x == y)
			{
				return 0;
			}
			if (x == null)
			{
				return -1;
			}
			if (y == null)
			{
				return 1;
			}
			return this._compareInfo.Compare(x, y, this._options);
		}

		// Token: 0x06000009 RID: 9 RVA: 0x0000212A File Offset: 0x0000032A
		public override bool Equals(string x, string y)
		{
			return x == y || (x != null && y != null && this._compareInfo.Compare(x, y, this._options) == 0);
		}

		// Token: 0x0600000A RID: 10 RVA: 0x00002150 File Offset: 0x00000350
		public override int GetHashCode(string obj)
		{
			if (obj == null)
			{
				throw new ArgumentNullException("obj");
			}
			return this._compareInfo.GetHashCode(obj, this._options & ~CompareOptions.StringSort);
		}

		// Token: 0x0600000B RID: 11 RVA: 0x00002178 File Offset: 0x00000378
		public override bool Equals(object obj)
		{
			CultureAwareComparer cultureAwareComparer = obj as CultureAwareComparer;
			return cultureAwareComparer != null && this._options == cultureAwareComparer._options && this._compareInfo.Equals(cultureAwareComparer._compareInfo);
		}

		// Token: 0x0600000C RID: 12 RVA: 0x000021B0 File Offset: 0x000003B0
		public override int GetHashCode()
		{
			return this._compareInfo.GetHashCode() ^ (int)(this._options & (CompareOptions)2147483647);
		}

		// Token: 0x04000002 RID: 2
		internal const CompareOptions ValidCompareMaskOffFlags = ~(CompareOptions.IgnoreCase | CompareOptions.IgnoreNonSpace | CompareOptions.IgnoreSymbols | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth | CompareOptions.StringSort);

		// Token: 0x04000003 RID: 3
		private readonly CompareInfo _compareInfo;

		// Token: 0x04000004 RID: 4
		private readonly CompareOptions _options;
	}
}
