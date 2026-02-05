using System;
using System.Net.Mime;

namespace System.Net.Mail
{
	// Token: 0x02000275 RID: 629
	internal static class QuotedStringFormatReader
	{
		// Token: 0x060017AB RID: 6059 RVA: 0x00078A6C File Offset: 0x00076C6C
		internal static int ReadReverseQuoted(string data, int index, bool permitUnicode)
		{
			index--;
			for (;;)
			{
				index = WhitespaceReader.ReadFwsReverse(data, index);
				if (index < 0)
				{
					goto IL_0075;
				}
				int num = QuotedPairReader.CountQuotedChars(data, index, permitUnicode);
				if (num > 0)
				{
					index -= num;
				}
				else
				{
					if (data[index] == MailBnfHelper.Quote)
					{
						break;
					}
					if (!QuotedStringFormatReader.IsValidQtext(permitUnicode, data[index]))
					{
						goto Block_4;
					}
					index--;
				}
				if (index < 0)
				{
					goto IL_0075;
				}
			}
			return index - 1;
			Block_4:
			throw new FormatException(SR.GetString("MailHeaderFieldInvalidCharacter", new object[] { data[index] }));
			IL_0075:
			throw new FormatException(SR.GetString("MailHeaderFieldInvalidCharacter", new object[] { MailBnfHelper.Quote }));
		}

		// Token: 0x060017AC RID: 6060 RVA: 0x00078B10 File Offset: 0x00076D10
		internal static int ReadReverseUnQuoted(string data, int index, bool permitUnicode, bool expectCommaDelimiter)
		{
			for (;;)
			{
				index = WhitespaceReader.ReadFwsReverse(data, index);
				if (index < 0)
				{
					return index;
				}
				int num = QuotedPairReader.CountQuotedChars(data, index, permitUnicode);
				if (num > 0)
				{
					index -= num;
				}
				else
				{
					if (expectCommaDelimiter && data[index] == MailBnfHelper.Comma)
					{
						return index;
					}
					if (!QuotedStringFormatReader.IsValidQtext(permitUnicode, data[index]))
					{
						break;
					}
					index--;
				}
				if (index < 0)
				{
					return index;
				}
			}
			throw new FormatException(SR.GetString("MailHeaderFieldInvalidCharacter", new object[] { data[index] }));
		}

		// Token: 0x060017AD RID: 6061 RVA: 0x00078B8D File Offset: 0x00076D8D
		private static bool IsValidQtext(bool allowUnicode, char ch)
		{
			if ((int)ch > MailBnfHelper.Ascii7bitMaxValue)
			{
				return allowUnicode;
			}
			return MailBnfHelper.Qtext[(int)ch];
		}
	}
}
