using System;
using DynamicOpenVR.Logging;

namespace DynamicOpenVR.BeatSaber
{
	// Token: 0x02000002 RID: 2
	internal class IPALogHandler : ILogHandler
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		public void Trace(object message)
		{
			Plugin.logger.Trace((message != null) ? message.ToString() : null);
		}

		// Token: 0x06000002 RID: 2 RVA: 0x00002068 File Offset: 0x00000268
		public void Debug(object message)
		{
			Plugin.logger.Debug((message != null) ? message.ToString() : null);
		}

		// Token: 0x06000003 RID: 3 RVA: 0x00002080 File Offset: 0x00000280
		public void Info(object message)
		{
			Plugin.logger.Info((message != null) ? message.ToString() : null);
		}

		// Token: 0x06000004 RID: 4 RVA: 0x00002098 File Offset: 0x00000298
		public void Notice(object message)
		{
			Plugin.logger.Notice((message != null) ? message.ToString() : null);
		}

		// Token: 0x06000005 RID: 5 RVA: 0x000020B0 File Offset: 0x000002B0
		public void Warn(object message)
		{
			Plugin.logger.Warn((message != null) ? message.ToString() : null);
		}

		// Token: 0x06000006 RID: 6 RVA: 0x000020C8 File Offset: 0x000002C8
		public void Error(object message)
		{
			Plugin.logger.Error((message != null) ? message.ToString() : null);
		}

		// Token: 0x06000007 RID: 7 RVA: 0x000020E0 File Offset: 0x000002E0
		public void Critical(object message)
		{
			Plugin.logger.Critical((message != null) ? message.ToString() : null);
		}
	}
}
