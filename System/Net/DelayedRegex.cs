using System;
using System.Text.RegularExpressions;

namespace System.Net
{
	// Token: 0x02000186 RID: 390
	[Serializable]
	internal class DelayedRegex
	{
		// Token: 0x06000E7E RID: 3710 RVA: 0x0004BB34 File Offset: 0x00049D34
		internal DelayedRegex(string regexString)
		{
			if (regexString == null)
			{
				throw new ArgumentNullException("regexString");
			}
			this._AsString = regexString;
		}

		// Token: 0x06000E7F RID: 3711 RVA: 0x0004BB51 File Offset: 0x00049D51
		internal DelayedRegex(Regex regex)
		{
			if (regex == null)
			{
				throw new ArgumentNullException("regex");
			}
			this._AsRegex = regex;
		}

		// Token: 0x17000337 RID: 823
		// (get) Token: 0x06000E80 RID: 3712 RVA: 0x0004BB6E File Offset: 0x00049D6E
		internal Regex AsRegex
		{
			get
			{
				if (this._AsRegex == null)
				{
					this._AsRegex = new Regex(this._AsString + "[/]?", RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.CultureInvariant);
				}
				return this._AsRegex;
			}
		}

		// Token: 0x06000E81 RID: 3713 RVA: 0x0004BBA0 File Offset: 0x00049DA0
		public override string ToString()
		{
			if (this._AsString == null)
			{
				return this._AsString = this._AsRegex.ToString();
			}
			return this._AsString;
		}

		// Token: 0x0400126F RID: 4719
		private Regex _AsRegex;

		// Token: 0x04001270 RID: 4720
		private string _AsString;
	}
}
