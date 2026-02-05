using System;
using System.Collections.Generic;
using System.Globalization;

namespace System.Net.Mime
{
	// Token: 0x02000251 RID: 593
	internal class SmtpDateTime
	{
		// Token: 0x06001686 RID: 5766 RVA: 0x00074B64 File Offset: 0x00072D64
		internal static IDictionary<string, TimeSpan> InitializeShortHandLookups()
		{
			return new Dictionary<string, TimeSpan>
			{
				{
					"UT",
					TimeSpan.Zero
				},
				{
					"GMT",
					TimeSpan.Zero
				},
				{
					"EDT",
					new TimeSpan(-4, 0, 0)
				},
				{
					"EST",
					new TimeSpan(-5, 0, 0)
				},
				{
					"CDT",
					new TimeSpan(-5, 0, 0)
				},
				{
					"CST",
					new TimeSpan(-6, 0, 0)
				},
				{
					"MDT",
					new TimeSpan(-6, 0, 0)
				},
				{
					"MST",
					new TimeSpan(-7, 0, 0)
				},
				{
					"PDT",
					new TimeSpan(-7, 0, 0)
				},
				{
					"PST",
					new TimeSpan(-8, 0, 0)
				}
			};
		}

		// Token: 0x06001687 RID: 5767 RVA: 0x00074C38 File Offset: 0x00072E38
		internal SmtpDateTime(DateTime value)
		{
			this.date = value;
			switch (value.Kind)
			{
			case DateTimeKind.Unspecified:
				this.unknownTimeZone = true;
				return;
			case DateTimeKind.Utc:
				this.timeZone = TimeSpan.Zero;
				return;
			case DateTimeKind.Local:
			{
				TimeSpan utcOffset = TimeZoneInfo.Local.GetUtcOffset(value);
				this.timeZone = this.ValidateAndGetSanitizedTimeSpan(utcOffset);
				return;
			}
			default:
				return;
			}
		}

		// Token: 0x06001688 RID: 5768 RVA: 0x00074C9C File Offset: 0x00072E9C
		internal SmtpDateTime(string value)
		{
			string text;
			this.date = this.ParseValue(value, out text);
			if (!this.TryParseTimeZoneString(text, out this.timeZone))
			{
				this.unknownTimeZone = true;
			}
		}

		// Token: 0x170004B3 RID: 1203
		// (get) Token: 0x06001689 RID: 5769 RVA: 0x00074CD4 File Offset: 0x00072ED4
		internal DateTime Date
		{
			get
			{
				if (this.unknownTimeZone)
				{
					return DateTime.SpecifyKind(this.date, DateTimeKind.Unspecified);
				}
				DateTimeOffset dateTimeOffset = new DateTimeOffset(this.date, this.timeZone);
				return dateTimeOffset.LocalDateTime;
			}
		}

		// Token: 0x0600168A RID: 5770 RVA: 0x00074D10 File Offset: 0x00072F10
		public override string ToString()
		{
			if (this.unknownTimeZone)
			{
				return string.Format("{0} {1}", this.FormatDate(this.date), "-0000");
			}
			return string.Format("{0} {1}", this.FormatDate(this.date), this.TimeSpanToOffset(this.timeZone));
		}

		// Token: 0x0600168B RID: 5771 RVA: 0x00074D64 File Offset: 0x00072F64
		internal void ValidateAndGetTimeZoneOffsetValues(string offset, out bool positive, out int hours, out int minutes)
		{
			if (offset.Length != 5)
			{
				throw new FormatException(SR.GetString("MailDateInvalidFormat"));
			}
			positive = offset.StartsWith("+");
			if (!int.TryParse(offset.Substring(1, 2), NumberStyles.None, CultureInfo.InvariantCulture, out hours))
			{
				throw new FormatException(SR.GetString("MailDateInvalidFormat"));
			}
			if (!int.TryParse(offset.Substring(3, 2), NumberStyles.None, CultureInfo.InvariantCulture, out minutes))
			{
				throw new FormatException(SR.GetString("MailDateInvalidFormat"));
			}
			if (minutes > 59)
			{
				throw new FormatException(SR.GetString("MailDateInvalidFormat"));
			}
		}

		// Token: 0x0600168C RID: 5772 RVA: 0x00074DFC File Offset: 0x00072FFC
		internal void ValidateTimeZoneShortHandValue(string value)
		{
			for (int i = 0; i < value.Length; i++)
			{
				if (!char.IsLetter(value, i))
				{
					throw new FormatException(SR.GetString("MailHeaderFieldInvalidCharacter"));
				}
			}
		}

		// Token: 0x0600168D RID: 5773 RVA: 0x00074E34 File Offset: 0x00073034
		internal string FormatDate(DateTime value)
		{
			return value.ToString("ddd, dd MMM yyyy HH:mm:ss", CultureInfo.InvariantCulture);
		}

		// Token: 0x0600168E RID: 5774 RVA: 0x00074E54 File Offset: 0x00073054
		internal DateTime ParseValue(string data, out string timeZone)
		{
			if (string.IsNullOrEmpty(data))
			{
				throw new FormatException(SR.GetString("MailDateInvalidFormat"));
			}
			int num = data.IndexOf(':');
			if (num == -1)
			{
				throw new FormatException(SR.GetString("MailHeaderFieldInvalidCharacter"));
			}
			int num2 = data.IndexOfAny(SmtpDateTime.allowedWhiteSpaceChars, num);
			if (num2 == -1)
			{
				throw new FormatException(SR.GetString("MailHeaderFieldInvalidCharacter"));
			}
			string text = data.Substring(0, num2).Trim();
			DateTime dateTime;
			if (!DateTime.TryParseExact(text, SmtpDateTime.validDateTimeFormats, CultureInfo.InvariantCulture, DateTimeStyles.AllowWhiteSpaces, out dateTime))
			{
				throw new FormatException(SR.GetString("MailDateInvalidFormat"));
			}
			string text2 = data.Substring(num2).Trim();
			int num3 = text2.IndexOfAny(SmtpDateTime.allowedWhiteSpaceChars);
			if (num3 != -1)
			{
				text2 = text2.Substring(0, num3);
			}
			if (string.IsNullOrEmpty(text2))
			{
				throw new FormatException(SR.GetString("MailDateInvalidFormat"));
			}
			timeZone = text2;
			return dateTime;
		}

		// Token: 0x0600168F RID: 5775 RVA: 0x00074F38 File Offset: 0x00073138
		internal bool TryParseTimeZoneString(string timeZoneString, out TimeSpan timeZone)
		{
			timeZone = TimeSpan.Zero;
			if (timeZoneString == "-0000")
			{
				return false;
			}
			if (timeZoneString[0] == '+' || timeZoneString[0] == '-')
			{
				bool flag;
				int num;
				int num2;
				this.ValidateAndGetTimeZoneOffsetValues(timeZoneString, out flag, out num, out num2);
				if (!flag)
				{
					if (num != 0)
					{
						num *= -1;
					}
					else if (num2 != 0)
					{
						num2 *= -1;
					}
				}
				timeZone = new TimeSpan(num, num2, 0);
				return true;
			}
			this.ValidateTimeZoneShortHandValue(timeZoneString);
			if (SmtpDateTime.timeZoneOffsetLookup.ContainsKey(timeZoneString))
			{
				timeZone = SmtpDateTime.timeZoneOffsetLookup[timeZoneString];
				return true;
			}
			return false;
		}

		// Token: 0x06001690 RID: 5776 RVA: 0x00074FD0 File Offset: 0x000731D0
		internal TimeSpan ValidateAndGetSanitizedTimeSpan(TimeSpan span)
		{
			TimeSpan timeSpan = new TimeSpan(span.Days, span.Hours, span.Minutes, 0, 0);
			if (Math.Abs(timeSpan.Ticks) > SmtpDateTime.timeSpanMaxTicks)
			{
				throw new FormatException(SR.GetString("MailDateInvalidFormat"));
			}
			return timeSpan;
		}

		// Token: 0x06001691 RID: 5777 RVA: 0x00075020 File Offset: 0x00073220
		internal string TimeSpanToOffset(TimeSpan span)
		{
			if (span.Ticks == 0L)
			{
				return "+0000";
			}
			uint num = (uint)Math.Abs(Math.Floor(span.TotalHours));
			uint num2 = (uint)Math.Abs(span.Minutes);
			string text = ((span.Ticks > 0L) ? "+" : "-");
			if (num < 10U)
			{
				text += "0";
			}
			text += num.ToString();
			if (num2 < 10U)
			{
				text += "0";
			}
			return text + num2.ToString();
		}

		// Token: 0x04001745 RID: 5957
		internal const string unknownTimeZoneDefaultOffset = "-0000";

		// Token: 0x04001746 RID: 5958
		internal const string utcDefaultTimeZoneOffset = "+0000";

		// Token: 0x04001747 RID: 5959
		internal const int offsetLength = 5;

		// Token: 0x04001748 RID: 5960
		internal const int maxMinuteValue = 59;

		// Token: 0x04001749 RID: 5961
		internal const string dateFormatWithDayOfWeek = "ddd, dd MMM yyyy HH:mm:ss";

		// Token: 0x0400174A RID: 5962
		internal const string dateFormatWithoutDayOfWeek = "dd MMM yyyy HH:mm:ss";

		// Token: 0x0400174B RID: 5963
		internal const string dateFormatWithDayOfWeekAndNoSeconds = "ddd, dd MMM yyyy HH:mm";

		// Token: 0x0400174C RID: 5964
		internal const string dateFormatWithoutDayOfWeekAndNoSeconds = "dd MMM yyyy HH:mm";

		// Token: 0x0400174D RID: 5965
		internal static readonly string[] validDateTimeFormats = new string[] { "ddd, dd MMM yyyy HH:mm:ss", "dd MMM yyyy HH:mm:ss", "ddd, dd MMM yyyy HH:mm", "dd MMM yyyy HH:mm" };

		// Token: 0x0400174E RID: 5966
		internal static readonly char[] allowedWhiteSpaceChars = new char[] { ' ', '\t' };

		// Token: 0x0400174F RID: 5967
		internal static readonly IDictionary<string, TimeSpan> timeZoneOffsetLookup = SmtpDateTime.InitializeShortHandLookups();

		// Token: 0x04001750 RID: 5968
		internal static readonly long timeSpanMaxTicks = 3599400000000L;

		// Token: 0x04001751 RID: 5969
		internal static readonly int offsetMaxValue = 9959;

		// Token: 0x04001752 RID: 5970
		private readonly DateTime date;

		// Token: 0x04001753 RID: 5971
		private readonly TimeSpan timeZone;

		// Token: 0x04001754 RID: 5972
		private readonly bool unknownTimeZone;
	}
}
