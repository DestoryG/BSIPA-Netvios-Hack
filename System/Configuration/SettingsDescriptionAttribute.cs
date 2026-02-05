using System;

namespace System.Configuration
{
	// Token: 0x0200009D RID: 157
	[AttributeUsage(AttributeTargets.Property)]
	public sealed class SettingsDescriptionAttribute : Attribute
	{
		// Token: 0x0600059C RID: 1436 RVA: 0x00022A2D File Offset: 0x00020C2D
		public SettingsDescriptionAttribute(string description)
		{
			this._desc = description;
		}

		// Token: 0x170000D7 RID: 215
		// (get) Token: 0x0600059D RID: 1437 RVA: 0x00022A3C File Offset: 0x00020C3C
		public string Description
		{
			get
			{
				return this._desc;
			}
		}

		// Token: 0x04000C38 RID: 3128
		private readonly string _desc;
	}
}
