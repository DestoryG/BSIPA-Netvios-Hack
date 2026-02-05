using System;

namespace Google.Protobuf.WellKnownTypes
{
	// Token: 0x0200003A RID: 58
	public static class TimeExtensions
	{
		// Token: 0x0600031D RID: 797 RVA: 0x0000DB38 File Offset: 0x0000BD38
		public static Timestamp ToTimestamp(this DateTime dateTime)
		{
			return Timestamp.FromDateTime(dateTime);
		}

		// Token: 0x0600031E RID: 798 RVA: 0x0000DB40 File Offset: 0x0000BD40
		public static Timestamp ToTimestamp(this DateTimeOffset dateTimeOffset)
		{
			return Timestamp.FromDateTimeOffset(dateTimeOffset);
		}

		// Token: 0x0600031F RID: 799 RVA: 0x0000DB48 File Offset: 0x0000BD48
		public static Duration ToDuration(this TimeSpan timeSpan)
		{
			return Duration.FromTimeSpan(timeSpan);
		}
	}
}
