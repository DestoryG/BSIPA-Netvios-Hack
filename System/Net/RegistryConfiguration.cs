using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Security;
using System.Security.Permissions;
using Microsoft.Win32;

namespace System.Net
{
	// Token: 0x02000152 RID: 338
	internal static class RegistryConfiguration
	{
		// Token: 0x06000BDE RID: 3038 RVA: 0x0004024C File Offset: 0x0003E44C
		[RegistryPermission(SecurityAction.Assert, Read = "HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\.NETFramework")]
		public static int GlobalConfigReadInt(string configVariable, int defaultValue)
		{
			object obj = RegistryConfiguration.ReadConfig(RegistryConfiguration.GetNetFrameworkVersionedPath(), configVariable, RegistryValueKind.DWord);
			if (obj != null)
			{
				return (int)obj;
			}
			return defaultValue;
		}

		// Token: 0x06000BDF RID: 3039 RVA: 0x00040274 File Offset: 0x0003E474
		[RegistryPermission(SecurityAction.Assert, Read = "HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\.NETFramework")]
		public static string GlobalConfigReadString(string configVariable, string defaultValue)
		{
			object obj = RegistryConfiguration.ReadConfig(RegistryConfiguration.GetNetFrameworkVersionedPath(), configVariable, RegistryValueKind.String);
			if (obj != null)
			{
				return (string)obj;
			}
			return defaultValue;
		}

		// Token: 0x06000BE0 RID: 3040 RVA: 0x0004029C File Offset: 0x0003E49C
		[RegistryPermission(SecurityAction.Assert, Read = "HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\.NETFramework")]
		public static int AppConfigReadInt(string configVariable, int defaultValue)
		{
			object obj = RegistryConfiguration.ReadConfig(RegistryConfiguration.GetAppConfigPath(configVariable), RegistryConfiguration.GetAppConfigValueName(), RegistryValueKind.DWord);
			if (obj != null)
			{
				return (int)obj;
			}
			return defaultValue;
		}

		// Token: 0x06000BE1 RID: 3041 RVA: 0x000402C8 File Offset: 0x0003E4C8
		[RegistryPermission(SecurityAction.Assert, Read = "HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\.NETFramework")]
		public static string AppConfigReadString(string configVariable, string defaultValue)
		{
			object obj = RegistryConfiguration.ReadConfig(RegistryConfiguration.GetAppConfigPath(configVariable), RegistryConfiguration.GetAppConfigValueName(), RegistryValueKind.String);
			if (obj != null)
			{
				return (string)obj;
			}
			return defaultValue;
		}

		// Token: 0x06000BE2 RID: 3042 RVA: 0x000402F4 File Offset: 0x0003E4F4
		private static object ReadConfig(string path, string valueName, RegistryValueKind kind)
		{
			object obj = null;
			try
			{
				using (RegistryKey registryKey = Registry.LocalMachine.OpenSubKey(path))
				{
					if (registryKey == null)
					{
						return obj;
					}
					try
					{
						object value = registryKey.GetValue(valueName, null);
						if (value != null && registryKey.GetValueKind(valueName) == kind)
						{
							obj = value;
						}
					}
					catch (UnauthorizedAccessException)
					{
					}
					catch (IOException)
					{
					}
				}
			}
			catch (SecurityException)
			{
			}
			catch (ObjectDisposedException)
			{
			}
			return obj;
		}

		// Token: 0x06000BE3 RID: 3043 RVA: 0x00040390 File Offset: 0x0003E590
		private static string GetNetFrameworkVersionedPath()
		{
			return string.Format(CultureInfo.InvariantCulture, "SOFTWARE\\Microsoft\\.NETFramework\\v{0}", new object[] { Environment.Version.ToString(3) });
		}

		// Token: 0x06000BE4 RID: 3044 RVA: 0x000403C2 File Offset: 0x0003E5C2
		private static string GetAppConfigPath(string valueName)
		{
			return string.Format(CultureInfo.InvariantCulture, "{0}\\{1}", new object[]
			{
				RegistryConfiguration.GetNetFrameworkVersionedPath(),
				valueName
			});
		}

		// Token: 0x06000BE5 RID: 3045 RVA: 0x000403E8 File Offset: 0x0003E5E8
		[SecurityPermission(SecurityAction.Assert, UnmanagedCode = true)]
		private static string GetAppConfigValueName()
		{
			string text = "Unknown";
			Process currentProcess = Process.GetCurrentProcess();
			try
			{
				ProcessModule mainModule = currentProcess.MainModule;
				text = mainModule.FileName;
			}
			catch (NotSupportedException)
			{
			}
			catch (Win32Exception)
			{
			}
			catch (InvalidOperationException)
			{
			}
			try
			{
				text = Path.GetFullPath(text);
			}
			catch (ArgumentException)
			{
			}
			catch (SecurityException)
			{
			}
			catch (NotSupportedException)
			{
			}
			catch (PathTooLongException)
			{
			}
			return text;
		}

		// Token: 0x04001122 RID: 4386
		private const string netFrameworkPath = "SOFTWARE\\Microsoft\\.NETFramework";

		// Token: 0x04001123 RID: 4387
		private const string netFrameworkVersionedPath = "SOFTWARE\\Microsoft\\.NETFramework\\v{0}";

		// Token: 0x04001124 RID: 4388
		private const string netFrameworkFullPath = "HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\.NETFramework";
	}
}
