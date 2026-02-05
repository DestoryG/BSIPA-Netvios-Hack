using System;
using System.Security;
using System.Security.Permissions;
using System.Threading;
using Microsoft.Win32;

namespace System.Net
{
	// Token: 0x02000119 RID: 281
	internal static class ComNetOS
	{
		// Token: 0x06000B1F RID: 2847 RVA: 0x0003D4E4 File Offset: 0x0003B6E4
		[EnvironmentPermission(SecurityAction.Assert, Unrestricted = true)]
		static ComNetOS()
		{
			OperatingSystem osversion = Environment.OSVersion;
			try
			{
				ComNetOS.IsAspNetServer = Thread.GetDomain().GetData(".appDomain") != null;
			}
			catch
			{
			}
			ComNetOS.IsWin7orLater = osversion.Version >= new Version(6, 1);
			ComNetOS.IsWin7Sp1orLater = osversion.Version >= new Version(6, 1, 7601);
			ComNetOS.IsWin8orLater = osversion.Version >= new Version(6, 2);
			ComNetOS.InstallationType = ComNetOS.GetWindowsInstallType();
			if (Logging.On)
			{
				Logging.PrintInfo(Logging.Web, SR.GetString("net_osinstalltype", new object[] { ComNetOS.InstallationType }));
			}
		}

		// Token: 0x06000B20 RID: 2848 RVA: 0x0003D5A8 File Offset: 0x0003B7A8
		[RegistryPermission(SecurityAction.Assert, Read = "HKEY_LOCAL_MACHINE\\Software\\Microsoft\\Windows NT\\CurrentVersion")]
		private static WindowsInstallationType GetWindowsInstallType()
		{
			WindowsInstallationType windowsInstallationType;
			try
			{
				using (RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("Software\\Microsoft\\Windows NT\\CurrentVersion"))
				{
					string text = registryKey.GetValue("InstallationType") as string;
					if (string.IsNullOrEmpty(text))
					{
						if (Logging.On)
						{
							Logging.PrintWarning(Logging.Web, SR.GetString("net_empty_osinstalltype", new object[] { "Software\\Microsoft\\Windows NT\\CurrentVersion\\InstallationType" }));
						}
						windowsInstallationType = WindowsInstallationType.Unknown;
					}
					else if (string.Compare(text, "Client", StringComparison.OrdinalIgnoreCase) == 0)
					{
						windowsInstallationType = WindowsInstallationType.Client;
					}
					else if (string.Compare(text, "Server", StringComparison.OrdinalIgnoreCase) == 0)
					{
						windowsInstallationType = WindowsInstallationType.Server;
					}
					else if (string.Compare(text, "Server Core", StringComparison.OrdinalIgnoreCase) == 0)
					{
						windowsInstallationType = WindowsInstallationType.ServerCore;
					}
					else if (string.Compare(text, "Embedded", StringComparison.OrdinalIgnoreCase) == 0)
					{
						windowsInstallationType = WindowsInstallationType.Embedded;
					}
					else
					{
						if (Logging.On)
						{
							Logging.PrintError(Logging.Web, SR.GetString("net_unknown_osinstalltype", new object[] { text }));
						}
						windowsInstallationType = WindowsInstallationType.Unknown;
					}
				}
			}
			catch (UnauthorizedAccessException ex)
			{
				if (Logging.On)
				{
					Logging.PrintWarning(Logging.Web, SR.GetString("net_cant_determine_osinstalltype", new object[] { "Software\\Microsoft\\Windows NT\\CurrentVersion", ex.Message }));
				}
				windowsInstallationType = WindowsInstallationType.Unknown;
			}
			catch (SecurityException ex2)
			{
				if (Logging.On)
				{
					Logging.PrintWarning(Logging.Web, SR.GetString("net_cant_determine_osinstalltype", new object[] { "Software\\Microsoft\\Windows NT\\CurrentVersion", ex2.Message }));
				}
				windowsInstallationType = WindowsInstallationType.Unknown;
			}
			return windowsInstallationType;
		}

		// Token: 0x04000F6A RID: 3946
		private const string OSInstallTypeRegKey = "Software\\Microsoft\\Windows NT\\CurrentVersion";

		// Token: 0x04000F6B RID: 3947
		private const string OSInstallTypeRegKeyPath = "HKEY_LOCAL_MACHINE\\Software\\Microsoft\\Windows NT\\CurrentVersion";

		// Token: 0x04000F6C RID: 3948
		private const string OSInstallTypeRegName = "InstallationType";

		// Token: 0x04000F6D RID: 3949
		private const string InstallTypeStringClient = "Client";

		// Token: 0x04000F6E RID: 3950
		private const string InstallTypeStringServer = "Server";

		// Token: 0x04000F6F RID: 3951
		private const string InstallTypeStringServerCore = "Server Core";

		// Token: 0x04000F70 RID: 3952
		private const string InstallTypeStringEmbedded = "Embedded";

		// Token: 0x04000F71 RID: 3953
		internal static readonly bool IsAspNetServer;

		// Token: 0x04000F72 RID: 3954
		internal static readonly bool IsWin7orLater;

		// Token: 0x04000F73 RID: 3955
		internal static readonly bool IsWin7Sp1orLater;

		// Token: 0x04000F74 RID: 3956
		internal static readonly bool IsWin8orLater;

		// Token: 0x04000F75 RID: 3957
		internal static readonly WindowsInstallationType InstallationType;
	}
}
