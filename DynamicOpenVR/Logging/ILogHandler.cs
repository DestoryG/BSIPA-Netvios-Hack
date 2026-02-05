using System;

namespace DynamicOpenVR.Logging
{
	// Token: 0x020000D3 RID: 211
	public interface ILogHandler
	{
		// Token: 0x060001CD RID: 461
		void Trace(object message);

		// Token: 0x060001CE RID: 462
		void Debug(object message);

		// Token: 0x060001CF RID: 463
		void Info(object message);

		// Token: 0x060001D0 RID: 464
		void Notice(object message);

		// Token: 0x060001D1 RID: 465
		void Warn(object message);

		// Token: 0x060001D2 RID: 466
		void Error(object message);

		// Token: 0x060001D3 RID: 467
		void Critical(object message);
	}
}
