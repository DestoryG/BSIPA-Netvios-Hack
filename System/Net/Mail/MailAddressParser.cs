using System;
using System.Collections.Generic;
using System.Net.Mime;
using System.Text;

namespace System.Net.Mail
{
	// Token: 0x0200026C RID: 620
	internal static class MailAddressParser
	{
		// Token: 0x06001749 RID: 5961 RVA: 0x00076D0C File Offset: 0x00074F0C
		internal static MailAddress ParseAddress(string data)
		{
			int num = data.Length - 1;
			return MailAddressParser.ParseAddress(data, false, ref num);
		}

		// Token: 0x0600174A RID: 5962 RVA: 0x00076D30 File Offset: 0x00074F30
		internal static IList<MailAddress> ParseMultipleAddresses(string data)
		{
			IList<MailAddress> list = new List<MailAddress>();
			for (int i = data.Length - 1; i >= 0; i--)
			{
				list.Insert(0, MailAddressParser.ParseAddress(data, true, ref i));
			}
			return list;
		}

		// Token: 0x0600174B RID: 5963 RVA: 0x00076D68 File Offset: 0x00074F68
		private static MailAddress ParseAddress(string data, bool expectMultipleAddresses, ref int index)
		{
			index = MailAddressParser.ReadCfwsAndThrowIfIncomplete(data, index);
			bool flag = false;
			if (data[index] == MailBnfHelper.EndAngleBracket)
			{
				flag = true;
				index--;
			}
			string text = MailAddressParser.ParseDomain(data, ref index);
			if (data[index] != MailBnfHelper.At)
			{
				throw new FormatException(SR.GetString("MailAddressInvalidFormat"));
			}
			index--;
			string text2 = MailAddressParser.ParseLocalPart(data, ref index, flag, expectMultipleAddresses);
			if (flag)
			{
				if (index < 0 || data[index] != MailBnfHelper.StartAngleBracket)
				{
					throw new FormatException(SR.GetString("MailHeaderFieldInvalidCharacter", new object[] { (index >= 0) ? data[index] : MailBnfHelper.EndAngleBracket }));
				}
				index--;
				index = WhitespaceReader.ReadFwsReverse(data, index);
			}
			string text3;
			if (index >= 0 && (!expectMultipleAddresses || data[index] != MailBnfHelper.Comma))
			{
				text3 = MailAddressParser.ParseDisplayName(data, ref index, expectMultipleAddresses);
			}
			else
			{
				text3 = string.Empty;
			}
			return new MailAddress(text3, text2, text);
		}

		// Token: 0x0600174C RID: 5964 RVA: 0x00076E60 File Offset: 0x00075060
		private static int ReadCfwsAndThrowIfIncomplete(string data, int index)
		{
			index = WhitespaceReader.ReadCfwsReverse(data, index);
			if (index < 0)
			{
				throw new FormatException(SR.GetString("MailAddressInvalidFormat"));
			}
			return index;
		}

		// Token: 0x0600174D RID: 5965 RVA: 0x00076E80 File Offset: 0x00075080
		private static string ParseDomain(string data, ref int index)
		{
			index = MailAddressParser.ReadCfwsAndThrowIfIncomplete(data, index);
			int num = index;
			if (data[index] == MailBnfHelper.EndSquareBracket)
			{
				index = DomainLiteralReader.ReadReverse(data, index);
			}
			else
			{
				index = DotAtomReader.ReadReverse(data, index);
			}
			string text = data.Substring(index + 1, num - index);
			index = MailAddressParser.ReadCfwsAndThrowIfIncomplete(data, index);
			return MailAddressParser.NormalizeOrThrow(text);
		}

		// Token: 0x0600174E RID: 5966 RVA: 0x00076EE0 File Offset: 0x000750E0
		private static string ParseLocalPart(string data, ref int index, bool expectAngleBracket, bool expectMultipleAddresses)
		{
			index = MailAddressParser.ReadCfwsAndThrowIfIncomplete(data, index);
			int num = index;
			if (data[index] == MailBnfHelper.Quote)
			{
				index = QuotedStringFormatReader.ReadReverseQuoted(data, index, true);
			}
			else
			{
				index = DotAtomReader.ReadReverse(data, index);
				if (index >= 0 && !MailBnfHelper.Whitespace.Contains(data[index]) && data[index] != MailBnfHelper.EndComment && (!expectAngleBracket || data[index] != MailBnfHelper.StartAngleBracket) && (!expectMultipleAddresses || data[index] != MailBnfHelper.Comma) && data[index] != MailBnfHelper.Quote)
				{
					throw new FormatException(SR.GetString("MailHeaderFieldInvalidCharacter", new object[] { data[index] }));
				}
			}
			string text = data.Substring(index + 1, num - index);
			index = WhitespaceReader.ReadCfwsReverse(data, index);
			return MailAddressParser.NormalizeOrThrow(text);
		}

		// Token: 0x0600174F RID: 5967 RVA: 0x00076FC4 File Offset: 0x000751C4
		private static string ParseDisplayName(string data, ref int index, bool expectMultipleAddresses)
		{
			int num = WhitespaceReader.ReadCfwsReverse(data, index);
			string text;
			if (num >= 0 && data[num] == MailBnfHelper.Quote)
			{
				index = QuotedStringFormatReader.ReadReverseQuoted(data, num, true);
				int num2 = index + 2;
				text = data.Substring(num2, num - num2);
				index = WhitespaceReader.ReadCfwsReverse(data, index);
				if (index >= 0 && (!expectMultipleAddresses || data[index] != MailBnfHelper.Comma))
				{
					throw new FormatException(SR.GetString("MailHeaderFieldInvalidCharacter", new object[] { data[index] }));
				}
			}
			else
			{
				int num3 = index;
				index = QuotedStringFormatReader.ReadReverseUnQuoted(data, index, true, expectMultipleAddresses);
				text = data.Substring(index + 1, num3 - index);
				text = text.Trim();
			}
			return MailAddressParser.NormalizeOrThrow(text);
		}

		// Token: 0x06001750 RID: 5968 RVA: 0x00077078 File Offset: 0x00075278
		internal static string NormalizeOrThrow(string input)
		{
			string text;
			try
			{
				text = input.Normalize(NormalizationForm.FormC);
			}
			catch (ArgumentException ex)
			{
				throw new FormatException(SR.GetString("MailAddressInvalidFormat"), ex);
			}
			return text;
		}
	}
}
