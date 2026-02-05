using System;

namespace System.Configuration
{
	// Token: 0x0200009F RID: 159
	[AttributeUsage(AttributeTargets.Class)]
	public sealed class SettingsGroupNameAttribute : Attribute
	{
		// Token: 0x060005A0 RID: 1440 RVA: 0x00022A5B File Offset: 0x00020C5B
		public SettingsGroupNameAttribute(string groupName)
		{
			this._groupName = groupName;
		}

		// Token: 0x170000D9 RID: 217
		// (get) Token: 0x060005A1 RID: 1441 RVA: 0x00022A6A File Offset: 0x00020C6A
		public string GroupName
		{
			get
			{
				return this._groupName;
			}
		}

		// Token: 0x04000C3A RID: 3130
		private readonly string _groupName;
	}
}
