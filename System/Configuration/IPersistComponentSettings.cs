using System;

namespace System.Configuration
{
	// Token: 0x02000092 RID: 146
	public interface IPersistComponentSettings
	{
		// Token: 0x170000CF RID: 207
		// (get) Token: 0x0600056F RID: 1391
		// (set) Token: 0x06000570 RID: 1392
		bool SaveSettings { get; set; }

		// Token: 0x170000D0 RID: 208
		// (get) Token: 0x06000571 RID: 1393
		// (set) Token: 0x06000572 RID: 1394
		string SettingsKey { get; set; }

		// Token: 0x06000573 RID: 1395
		void LoadComponentSettings();

		// Token: 0x06000574 RID: 1396
		void SaveComponentSettings();

		// Token: 0x06000575 RID: 1397
		void ResetComponentSettings();
	}
}
