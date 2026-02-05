using System;
using System.Security.Permissions;

namespace System.Configuration
{
	// Token: 0x020000BC RID: 188
	[ConfigurationPermission(SecurityAction.Assert, Unrestricted = true)]
	internal static class PrivilegedConfigurationManager
	{
		// Token: 0x17000109 RID: 265
		// (get) Token: 0x06000645 RID: 1605 RVA: 0x000242CC File Offset: 0x000224CC
		internal static ConnectionStringSettingsCollection ConnectionStrings
		{
			get
			{
				return ConfigurationManager.ConnectionStrings;
			}
		}

		// Token: 0x06000646 RID: 1606 RVA: 0x000242D3 File Offset: 0x000224D3
		internal static object GetSection(string sectionName)
		{
			return ConfigurationManager.GetSection(sectionName);
		}
	}
}
