using System;
using System.Globalization;

namespace System.Text.RegularExpressions
{
	// Token: 0x02000696 RID: 1686
	internal sealed class RegexFC
	{
		// Token: 0x06003EB7 RID: 16055 RVA: 0x00105390 File Offset: 0x00103590
		internal RegexFC(bool nullable)
		{
			this._cc = new RegexCharClass();
			this._nullable = nullable;
		}

		// Token: 0x06003EB8 RID: 16056 RVA: 0x001053AC File Offset: 0x001035AC
		internal RegexFC(char ch, bool not, bool nullable, bool caseInsensitive)
		{
			this._cc = new RegexCharClass();
			if (not)
			{
				if (ch > '\0')
				{
					this._cc.AddRange('\0', ch - '\u0001');
				}
				if (ch < '\uffff')
				{
					this._cc.AddRange(ch + '\u0001', char.MaxValue);
				}
			}
			else
			{
				this._cc.AddRange(ch, ch);
			}
			this._caseInsensitive = caseInsensitive;
			this._nullable = nullable;
		}

		// Token: 0x06003EB9 RID: 16057 RVA: 0x0010541B File Offset: 0x0010361B
		internal RegexFC(string charClass, bool nullable, bool caseInsensitive)
		{
			this._cc = RegexCharClass.Parse(charClass);
			this._nullable = nullable;
			this._caseInsensitive = caseInsensitive;
		}

		// Token: 0x06003EBA RID: 16058 RVA: 0x00105440 File Offset: 0x00103640
		internal bool AddFC(RegexFC fc, bool concatenate)
		{
			if (!this._cc.CanMerge || !fc._cc.CanMerge)
			{
				return false;
			}
			if (concatenate)
			{
				if (!this._nullable)
				{
					return true;
				}
				if (!fc._nullable)
				{
					this._nullable = false;
				}
			}
			else if (fc._nullable)
			{
				this._nullable = true;
			}
			this._caseInsensitive |= fc._caseInsensitive;
			this._cc.AddCharClass(fc._cc);
			return true;
		}

		// Token: 0x06003EBB RID: 16059 RVA: 0x001054BB File Offset: 0x001036BB
		internal string GetFirstChars(CultureInfo culture)
		{
			if (this._caseInsensitive)
			{
				this._cc.AddLowercase(culture);
			}
			return this._cc.ToStringClass();
		}

		// Token: 0x06003EBC RID: 16060 RVA: 0x001054DC File Offset: 0x001036DC
		internal bool IsCaseInsensitive()
		{
			return this._caseInsensitive;
		}

		// Token: 0x04002DC9 RID: 11721
		internal RegexCharClass _cc;

		// Token: 0x04002DCA RID: 11722
		internal bool _nullable;

		// Token: 0x04002DCB RID: 11723
		internal bool _caseInsensitive;
	}
}
