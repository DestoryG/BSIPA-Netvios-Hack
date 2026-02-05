using System;
using System.Net.Mime;

namespace System.Net.Mail
{
	// Token: 0x0200025A RID: 602
	internal static class DomainLiteralReader
	{
		// Token: 0x060016F7 RID: 5879 RVA: 0x0007610C File Offset: 0x0007430C
		internal static int ReadReverse(string data, int index)
		{
			index--;
			for (;;)
			{
				index = WhitespaceReader.ReadFwsReverse(data, index);
				if (index < 0)
				{
					goto IL_0083;
				}
				int num = QuotedPairReader.CountQuotedChars(data, index, false);
				if (num > 0)
				{
					index -= num;
				}
				else
				{
					if (data[index] == MailBnfHelper.StartSquareBracket)
					{
						break;
					}
					if ((int)data[index] > MailBnfHelper.Ascii7bitMaxValue || !MailBnfHelper.Dtext[(int)data[index]])
					{
						goto IL_0055;
					}
					index--;
				}
				if (index < 0)
				{
					goto IL_0083;
				}
			}
			return index - 1;
			IL_0055:
			throw new FormatException(SR.GetString("MailHeaderFieldInvalidCharacter", new object[] { data[index] }));
			IL_0083:
			throw new FormatException(SR.GetString("MailHeaderFieldInvalidCharacter", new object[] { MailBnfHelper.EndSquareBracket }));
		}
	}
}
