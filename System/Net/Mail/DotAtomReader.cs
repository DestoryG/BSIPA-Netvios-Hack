using System;
using System.Net.Mime;

namespace System.Net.Mail
{
	// Token: 0x0200025B RID: 603
	internal static class DotAtomReader
	{
		// Token: 0x060016F8 RID: 5880 RVA: 0x000761C0 File Offset: 0x000743C0
		internal static int ReadReverse(string data, int index)
		{
			int num = index;
			while (0 <= index && ((int)data[index] > MailBnfHelper.Ascii7bitMaxValue || data[index] == MailBnfHelper.Dot || MailBnfHelper.Atext[(int)data[index]]))
			{
				index--;
			}
			if (num == index)
			{
				throw new FormatException(SR.GetString("MailHeaderFieldInvalidCharacter", new object[] { data[index] }));
			}
			if (data[index + 1] == MailBnfHelper.Dot)
			{
				throw new FormatException(SR.GetString("MailHeaderFieldInvalidCharacter", new object[] { MailBnfHelper.Dot }));
			}
			return index;
		}
	}
}
