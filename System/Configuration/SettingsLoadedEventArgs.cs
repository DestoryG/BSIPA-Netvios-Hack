using System;

namespace System.Configuration
{
	// Token: 0x0200007E RID: 126
	public class SettingsLoadedEventArgs : EventArgs
	{
		// Token: 0x06000505 RID: 1285 RVA: 0x00020D1F File Offset: 0x0001EF1F
		public SettingsLoadedEventArgs(SettingsProvider provider)
		{
			this._provider = provider;
		}

		// Token: 0x170000B3 RID: 179
		// (get) Token: 0x06000506 RID: 1286 RVA: 0x00020D2E File Offset: 0x0001EF2E
		public SettingsProvider Provider
		{
			get
			{
				return this._provider;
			}
		}

		// Token: 0x04000C13 RID: 3091
		private SettingsProvider _provider;
	}
}
