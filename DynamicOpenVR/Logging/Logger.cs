using System;

namespace DynamicOpenVR.Logging
{
	// Token: 0x020000D4 RID: 212
	public static class Logger
	{
		// Token: 0x060001D4 RID: 468 RVA: 0x00005FA3 File Offset: 0x000041A3
		internal static void Trace(object message)
		{
			Logger.handler.Trace(message);
		}

		// Token: 0x060001D5 RID: 469 RVA: 0x00005FB0 File Offset: 0x000041B0
		internal static void Debug(object message)
		{
			Logger.handler.Debug(message);
		}

		// Token: 0x060001D6 RID: 470 RVA: 0x00005FBD File Offset: 0x000041BD
		internal static void Info(object message)
		{
			Logger.handler.Info(message);
		}

		// Token: 0x060001D7 RID: 471 RVA: 0x00005FCA File Offset: 0x000041CA
		internal static void Notice(object message)
		{
			Logger.handler.Notice(message);
		}

		// Token: 0x060001D8 RID: 472 RVA: 0x00005FD7 File Offset: 0x000041D7
		internal static void Warn(object message)
		{
			Logger.handler.Warn(message);
		}

		// Token: 0x060001D9 RID: 473 RVA: 0x00005FE4 File Offset: 0x000041E4
		internal static void Error(object message)
		{
			Logger.handler.Error(message);
		}

		// Token: 0x060001DA RID: 474 RVA: 0x00005FF1 File Offset: 0x000041F1
		internal static void Critical(object message)
		{
			Logger.handler.Critical(message);
		}

		// Token: 0x0400087E RID: 2174
		public static ILogHandler handler = new UnityDebugLogHandler();
	}
}
