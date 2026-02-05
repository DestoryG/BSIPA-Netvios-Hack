using System;
using System.Globalization;
using System.IO;
using System.Security;
using System.Security.Permissions;
using Microsoft.Win32;

namespace System.Net
{
	// Token: 0x02000106 RID: 262
	internal static class HttpSysSettings
	{
		// Token: 0x060009AE RID: 2478 RVA: 0x00036007 File Offset: 0x00034207
		static HttpSysSettings()
		{
			HttpSysSettings.ReadHttpSysRegistrySettings();
		}

		// Token: 0x1700020B RID: 523
		// (get) Token: 0x060009AF RID: 2479 RVA: 0x0003601E File Offset: 0x0003421E
		public static bool EnableNonUtf8
		{
			get
			{
				return HttpSysSettings.enableNonUtf8;
			}
		}

		// Token: 0x1700020C RID: 524
		// (get) Token: 0x060009B0 RID: 2480 RVA: 0x00036027 File Offset: 0x00034227
		public static bool FavorUtf8
		{
			get
			{
				return HttpSysSettings.favorUtf8;
			}
		}

		// Token: 0x060009B1 RID: 2481 RVA: 0x00036030 File Offset: 0x00034230
		[RegistryPermission(SecurityAction.Assert, Read = "HKEY_LOCAL_MACHINE\\System\\CurrentControlSet\\Services\\HTTP\\Parameters")]
		private static void ReadHttpSysRegistrySettings()
		{
			try
			{
				RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("System\\CurrentControlSet\\Services\\HTTP\\Parameters");
				if (registryKey == null)
				{
					HttpSysSettings.LogWarning("ReadHttpSysRegistrySettings", "net_log_listener_httpsys_registry_null", new object[] { "System\\CurrentControlSet\\Services\\HTTP\\Parameters" });
				}
				else
				{
					using (registryKey)
					{
						HttpSysSettings.enableNonUtf8 = HttpSysSettings.ReadRegistryValue(registryKey, "EnableNonUtf8", true);
						HttpSysSettings.favorUtf8 = HttpSysSettings.ReadRegistryValue(registryKey, "FavorUtf8", true);
					}
				}
			}
			catch (SecurityException ex)
			{
				HttpSysSettings.LogRegistryException("ReadHttpSysRegistrySettings", ex);
			}
			catch (ObjectDisposedException ex2)
			{
				HttpSysSettings.LogRegistryException("ReadHttpSysRegistrySettings", ex2);
			}
		}

		// Token: 0x060009B2 RID: 2482 RVA: 0x000360EC File Offset: 0x000342EC
		private static bool ReadRegistryValue(RegistryKey key, string valueName, bool defaultValue)
		{
			try
			{
				if (key.GetValueKind(valueName) == RegistryValueKind.DWord)
				{
					return Convert.ToBoolean(key.GetValue(valueName), CultureInfo.InvariantCulture);
				}
			}
			catch (UnauthorizedAccessException ex)
			{
				HttpSysSettings.LogRegistryException("ReadRegistryValue", ex);
			}
			catch (IOException ex2)
			{
				HttpSysSettings.LogRegistryException("ReadRegistryValue", ex2);
			}
			catch (SecurityException ex3)
			{
				HttpSysSettings.LogRegistryException("ReadRegistryValue", ex3);
			}
			catch (ObjectDisposedException ex4)
			{
				HttpSysSettings.LogRegistryException("ReadRegistryValue", ex4);
			}
			return defaultValue;
		}

		// Token: 0x060009B3 RID: 2483 RVA: 0x0003618C File Offset: 0x0003438C
		private static void LogRegistryException(string methodName, Exception e)
		{
			HttpSysSettings.LogWarning(methodName, "net_log_listener_httpsys_registry_error", new object[] { "System\\CurrentControlSet\\Services\\HTTP\\Parameters", e });
		}

		// Token: 0x060009B4 RID: 2484 RVA: 0x000361AB File Offset: 0x000343AB
		private static void LogWarning(string methodName, string message, params object[] args)
		{
			if (Logging.On)
			{
				Logging.PrintWarning(Logging.HttpListener, typeof(HttpSysSettings), methodName, SR.GetString(message, args));
			}
		}

		// Token: 0x04000EC7 RID: 3783
		private const string httpSysParametersKey = "System\\CurrentControlSet\\Services\\HTTP\\Parameters";

		// Token: 0x04000EC8 RID: 3784
		private const bool enableNonUtf8Default = true;

		// Token: 0x04000EC9 RID: 3785
		private const bool favorUtf8Default = true;

		// Token: 0x04000ECA RID: 3786
		private const string enableNonUtf8Name = "EnableNonUtf8";

		// Token: 0x04000ECB RID: 3787
		private const string favorUtf8Name = "FavorUtf8";

		// Token: 0x04000ECC RID: 3788
		private static volatile bool enableNonUtf8 = true;

		// Token: 0x04000ECD RID: 3789
		private static volatile bool favorUtf8 = true;
	}
}
