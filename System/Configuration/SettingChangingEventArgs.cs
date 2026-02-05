using System;
using System.ComponentModel;

namespace System.Configuration
{
	// Token: 0x0200007D RID: 125
	public class SettingChangingEventArgs : CancelEventArgs
	{
		// Token: 0x06000500 RID: 1280 RVA: 0x00020CD8 File Offset: 0x0001EED8
		public SettingChangingEventArgs(string settingName, string settingClass, string settingKey, object newValue, bool cancel)
			: base(cancel)
		{
			this._settingName = settingName;
			this._settingClass = settingClass;
			this._settingKey = settingKey;
			this._newValue = newValue;
		}

		// Token: 0x170000AF RID: 175
		// (get) Token: 0x06000501 RID: 1281 RVA: 0x00020CFF File Offset: 0x0001EEFF
		public object NewValue
		{
			get
			{
				return this._newValue;
			}
		}

		// Token: 0x170000B0 RID: 176
		// (get) Token: 0x06000502 RID: 1282 RVA: 0x00020D07 File Offset: 0x0001EF07
		public string SettingClass
		{
			get
			{
				return this._settingClass;
			}
		}

		// Token: 0x170000B1 RID: 177
		// (get) Token: 0x06000503 RID: 1283 RVA: 0x00020D0F File Offset: 0x0001EF0F
		public string SettingName
		{
			get
			{
				return this._settingName;
			}
		}

		// Token: 0x170000B2 RID: 178
		// (get) Token: 0x06000504 RID: 1284 RVA: 0x00020D17 File Offset: 0x0001EF17
		public string SettingKey
		{
			get
			{
				return this._settingKey;
			}
		}

		// Token: 0x04000C0F RID: 3087
		private string _settingClass;

		// Token: 0x04000C10 RID: 3088
		private string _settingName;

		// Token: 0x04000C11 RID: 3089
		private string _settingKey;

		// Token: 0x04000C12 RID: 3090
		private object _newValue;
	}
}
