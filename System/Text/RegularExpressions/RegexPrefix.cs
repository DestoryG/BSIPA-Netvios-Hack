using System;

namespace System.Text.RegularExpressions
{
	// Token: 0x02000697 RID: 1687
	internal sealed class RegexPrefix
	{
		// Token: 0x06003EBD RID: 16061 RVA: 0x001054E4 File Offset: 0x001036E4
		internal RegexPrefix(string prefix, bool ci)
		{
			this._prefix = prefix;
			this._caseInsensitive = ci;
		}

		// Token: 0x17000EBC RID: 3772
		// (get) Token: 0x06003EBE RID: 16062 RVA: 0x001054FA File Offset: 0x001036FA
		internal string Prefix
		{
			get
			{
				return this._prefix;
			}
		}

		// Token: 0x17000EBD RID: 3773
		// (get) Token: 0x06003EBF RID: 16063 RVA: 0x00105502 File Offset: 0x00103702
		internal bool CaseInsensitive
		{
			get
			{
				return this._caseInsensitive;
			}
		}

		// Token: 0x17000EBE RID: 3774
		// (get) Token: 0x06003EC0 RID: 16064 RVA: 0x0010550A File Offset: 0x0010370A
		internal static RegexPrefix Empty
		{
			get
			{
				return RegexPrefix._empty;
			}
		}

		// Token: 0x04002DCC RID: 11724
		internal string _prefix;

		// Token: 0x04002DCD RID: 11725
		internal bool _caseInsensitive;

		// Token: 0x04002DCE RID: 11726
		internal static RegexPrefix _empty = new RegexPrefix(string.Empty, false);
	}
}
