using System;
using System.Globalization;

namespace System.Runtime.Serialization
{
	// Token: 0x0200007C RID: 124
	public class DateTimeFormat
	{
		// Token: 0x06000938 RID: 2360 RVA: 0x000297F9 File Offset: 0x000279F9
		public DateTimeFormat(string formatString)
			: this(formatString, DateTimeFormatInfo.CurrentInfo)
		{
		}

		// Token: 0x06000939 RID: 2361 RVA: 0x00029807 File Offset: 0x00027A07
		public DateTimeFormat(string formatString, IFormatProvider formatProvider)
		{
			if (formatString == null)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("formatString");
			}
			if (formatProvider == null)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("formatProvider");
			}
			this.formatString = formatString;
			this.formatProvider = formatProvider;
			this.dateTimeStyles = DateTimeStyles.RoundtripKind;
		}

		// Token: 0x17000153 RID: 339
		// (get) Token: 0x0600093A RID: 2362 RVA: 0x00029844 File Offset: 0x00027A44
		public string FormatString
		{
			get
			{
				return this.formatString;
			}
		}

		// Token: 0x17000154 RID: 340
		// (get) Token: 0x0600093B RID: 2363 RVA: 0x0002984C File Offset: 0x00027A4C
		public IFormatProvider FormatProvider
		{
			get
			{
				return this.formatProvider;
			}
		}

		// Token: 0x17000155 RID: 341
		// (get) Token: 0x0600093C RID: 2364 RVA: 0x00029854 File Offset: 0x00027A54
		// (set) Token: 0x0600093D RID: 2365 RVA: 0x0002985C File Offset: 0x00027A5C
		public DateTimeStyles DateTimeStyles
		{
			get
			{
				return this.dateTimeStyles;
			}
			set
			{
				this.dateTimeStyles = value;
			}
		}

		// Token: 0x0400034F RID: 847
		private string formatString;

		// Token: 0x04000350 RID: 848
		private IFormatProvider formatProvider;

		// Token: 0x04000351 RID: 849
		private DateTimeStyles dateTimeStyles;
	}
}
