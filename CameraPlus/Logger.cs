using System;
using IPA.Logging;

namespace CameraPlus
{
	// Token: 0x02000009 RID: 9
	internal static class Logger
	{
		// Token: 0x1700000D RID: 13
		// (get) Token: 0x06000039 RID: 57 RVA: 0x00005A16 File Offset: 0x00003C16
		// (set) Token: 0x0600003A RID: 58 RVA: 0x00005A1D File Offset: 0x00003C1D
		internal static Logger log { private get; set; }

		// Token: 0x0600003B RID: 59 RVA: 0x00005A25 File Offset: 0x00003C25
		internal static void Log(string message, Logger.Level severity = Logger.Level.Info)
		{
			Logger.log.Log(severity, message);
		}
	}
}
