using System;
using IPA.Logging;

namespace SongCore.Utilities
{
	// Token: 0x02000017 RID: 23
	internal static class Logging
	{
		// Token: 0x06000131 RID: 305 RVA: 0x00006270 File Offset: 0x00004470
		internal static void Log(string message)
		{
			Logging.logger.Info(message ?? "");
		}

		// Token: 0x06000132 RID: 306 RVA: 0x00006286 File Offset: 0x00004486
		internal static void Log(string message, Logger.Level level)
		{
			Logging.logger.Log(level, message ?? "");
		}

		// Token: 0x0400006F RID: 111
		public static Logger logger;
	}
}
