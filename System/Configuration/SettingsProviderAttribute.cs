using System;

namespace System.Configuration
{
	// Token: 0x020000A1 RID: 161
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Property)]
	public sealed class SettingsProviderAttribute : Attribute
	{
		// Token: 0x060005A4 RID: 1444 RVA: 0x00022A89 File Offset: 0x00020C89
		public SettingsProviderAttribute(string providerTypeName)
		{
			this._providerTypeName = providerTypeName;
		}

		// Token: 0x060005A5 RID: 1445 RVA: 0x00022A98 File Offset: 0x00020C98
		public SettingsProviderAttribute(Type providerType)
		{
			if (providerType != null)
			{
				this._providerTypeName = providerType.AssemblyQualifiedName;
			}
		}

		// Token: 0x170000DB RID: 219
		// (get) Token: 0x060005A6 RID: 1446 RVA: 0x00022AB5 File Offset: 0x00020CB5
		public string ProviderTypeName
		{
			get
			{
				return this._providerTypeName;
			}
		}

		// Token: 0x04000C3C RID: 3132
		private readonly string _providerTypeName;
	}
}
