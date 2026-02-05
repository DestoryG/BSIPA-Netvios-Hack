using System;
using System.Globalization;
using System.Runtime.Serialization;

namespace System.Text
{
	// Token: 0x02000060 RID: 96
	internal struct SurrogateChar
	{
		// Token: 0x060006FC RID: 1788 RVA: 0x0002056C File Offset: 0x0001E76C
		public SurrogateChar(int ch)
		{
			if (ch < 65536 || ch > 1114111)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentException(global::System.Runtime.Serialization.SR.GetString("Surrogate char '0x{0}' not valid. Surrogate chars range from 0x10000 to 0x10FFFF.", new object[] { ch.ToString("X", CultureInfo.InvariantCulture) }), "ch"));
			}
			this.lowChar = (char)(((ch - 65536) & 1023) + 56320);
			this.highChar = (char)(((ch - 65536 >> 10) & 1023) + 55296);
		}

		// Token: 0x060006FD RID: 1789 RVA: 0x000205F4 File Offset: 0x0001E7F4
		public SurrogateChar(char lowChar, char highChar)
		{
			if (lowChar < '\udc00' || lowChar > '\udfff')
			{
				string text = "Low surrogate char '0x{0}' not valid. Low surrogate chars range from 0xDC00 to 0xDFFF.";
				object[] array = new object[1];
				int num = 0;
				int num2 = (int)lowChar;
				array[num] = num2.ToString("X", CultureInfo.InvariantCulture);
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentException(global::System.Runtime.Serialization.SR.GetString(text, array), "lowChar"));
			}
			if (highChar < '\ud800' || highChar > '\udbff')
			{
				string text2 = "High surrogate char '0x{0}' not valid. High surrogate chars range from 0xD800 to 0xDBFF.";
				object[] array2 = new object[1];
				int num3 = 0;
				int num2 = (int)highChar;
				array2[num3] = num2.ToString("X", CultureInfo.InvariantCulture);
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentException(global::System.Runtime.Serialization.SR.GetString(text2, array2), "highChar"));
			}
			this.lowChar = lowChar;
			this.highChar = highChar;
		}

		// Token: 0x170000A4 RID: 164
		// (get) Token: 0x060006FE RID: 1790 RVA: 0x0002069B File Offset: 0x0001E89B
		public char LowChar
		{
			get
			{
				return this.lowChar;
			}
		}

		// Token: 0x170000A5 RID: 165
		// (get) Token: 0x060006FF RID: 1791 RVA: 0x000206A3 File Offset: 0x0001E8A3
		public char HighChar
		{
			get
			{
				return this.highChar;
			}
		}

		// Token: 0x170000A6 RID: 166
		// (get) Token: 0x06000700 RID: 1792 RVA: 0x000206AB File Offset: 0x0001E8AB
		public int Char
		{
			get
			{
				return (int)(this.lowChar - '\udc00') | ((int)((int)(this.highChar - '\ud800') << 10) + 65536);
			}
		}

		// Token: 0x040002C2 RID: 706
		private char lowChar;

		// Token: 0x040002C3 RID: 707
		private char highChar;

		// Token: 0x040002C4 RID: 708
		public const int MinValue = 65536;

		// Token: 0x040002C5 RID: 709
		public const int MaxValue = 1114111;

		// Token: 0x040002C6 RID: 710
		private const char surHighMin = '\ud800';

		// Token: 0x040002C7 RID: 711
		private const char surHighMax = '\udbff';

		// Token: 0x040002C8 RID: 712
		private const char surLowMin = '\udc00';

		// Token: 0x040002C9 RID: 713
		private const char surLowMax = '\udfff';
	}
}
