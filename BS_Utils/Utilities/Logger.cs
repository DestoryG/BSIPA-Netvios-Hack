using System;
using IPA.Logging;

namespace BS_Utils.Utilities
{
	// Token: 0x02000006 RID: 6
	public class Logger
	{
		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000077 RID: 119 RVA: 0x00003BEC File Offset: 0x00001DEC
		// (set) Token: 0x06000078 RID: 120 RVA: 0x00003BF3 File Offset: 0x00001DF3
		internal static Logger log { get; set; }

		// Token: 0x06000079 RID: 121 RVA: 0x00003BFB File Offset: 0x00001DFB
		public static void Log(string modName, string message)
		{
			Logger.log.Info("[" + modName + "]  " + message);
		}

		// Token: 0x0600007A RID: 122 RVA: 0x00003C18 File Offset: 0x00001E18
		internal static void Log(string message)
		{
			Logger.log.Info("[BS-Utils]  " + message);
		}

		// Token: 0x0600007B RID: 123 RVA: 0x00003C2F File Offset: 0x00001E2F
		internal static void Log(string message, Logger.Level level)
		{
			Logger.log.Log(level, "[BS-Utils]  " + message);
		}
	}
}
