using System;

namespace System.Configuration
{
	// Token: 0x0200009E RID: 158
	[AttributeUsage(AttributeTargets.Class)]
	public sealed class SettingsGroupDescriptionAttribute : Attribute
	{
		// Token: 0x0600059E RID: 1438 RVA: 0x00022A44 File Offset: 0x00020C44
		public SettingsGroupDescriptionAttribute(string description)
		{
			this._desc = description;
		}

		// Token: 0x170000D8 RID: 216
		// (get) Token: 0x0600059F RID: 1439 RVA: 0x00022A53 File Offset: 0x00020C53
		public string Description
		{
			get
			{
				return this._desc;
			}
		}

		// Token: 0x04000C39 RID: 3129
		private readonly string _desc;
	}
}
