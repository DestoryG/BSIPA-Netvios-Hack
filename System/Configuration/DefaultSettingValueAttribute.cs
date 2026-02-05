using System;

namespace System.Configuration
{
	// Token: 0x0200009A RID: 154
	[AttributeUsage(AttributeTargets.Property)]
	public sealed class DefaultSettingValueAttribute : Attribute
	{
		// Token: 0x06000598 RID: 1432 RVA: 0x00022A06 File Offset: 0x00020C06
		public DefaultSettingValueAttribute(string value)
		{
			this._value = value;
		}

		// Token: 0x170000D6 RID: 214
		// (get) Token: 0x06000599 RID: 1433 RVA: 0x00022A15 File Offset: 0x00020C15
		public string Value
		{
			get
			{
				return this._value;
			}
		}

		// Token: 0x04000C37 RID: 3127
		private readonly string _value;
	}
}
