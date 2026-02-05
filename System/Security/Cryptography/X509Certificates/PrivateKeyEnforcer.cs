using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Security.Permissions;
using Microsoft.Win32;

namespace System.Security.Cryptography.X509Certificates
{
	// Token: 0x02000467 RID: 1127
	internal static class PrivateKeyEnforcer
	{
		// Token: 0x060029E8 RID: 10728 RVA: 0x000BEB0C File Offset: 0x000BCD0C
		[MethodImpl(MethodImplOptions.NoInlining)]
		internal static void VerifyNotPfx(byte[] rawData, string settingName, ref int setting)
		{
			PrivateKeyEnforcer.Impl.VerifyNotPfx(rawData, settingName, ref setting);
		}

		// Token: 0x02000876 RID: 2166
		private static class Impl
		{
			// Token: 0x0600455B RID: 17755 RVA: 0x001216C0 File Offset: 0x0011F8C0
			[SecuritySafeCritical]
			[EnvironmentPermission(SecurityAction.Assert, Unrestricted = true)]
			internal static void VerifyNotPfx(byte[] rawData, string settingName, ref int setting)
			{
				PrivateKeyEnforcer.Impl.PrivateKeySetting privateKeySetting = (PrivateKeyEnforcer.Impl.PrivateKeySetting)setting;
				if (privateKeySetting == PrivateKeyEnforcer.Impl.PrivateKeySetting.Uninitialized)
				{
					privateKeySetting = (PrivateKeyEnforcer.Impl.ReadPrivateKeySetting(settingName) ? PrivateKeyEnforcer.Impl.PrivateKeySetting.Enabled : PrivateKeyEnforcer.Impl.PrivateKeySetting.Disabled);
					setting = (int)privateKeySetting;
				}
				if (privateKeySetting == PrivateKeyEnforcer.Impl.PrivateKeySetting.Enabled)
				{
					X509ContentType certContentType = X509Certificate2.GetCertContentType(rawData);
					if (certContentType == X509ContentType.Pfx)
					{
						throw new CryptographicException(SR.GetString("Cryptography_X509_PfxBlobsNotAllowed"));
					}
				}
			}

			// Token: 0x0600455C RID: 17756 RVA: 0x00121704 File Offset: 0x0011F904
			[SecuritySafeCritical]
			[EnvironmentPermission(SecurityAction.Assert, Unrestricted = true)]
			private static bool ReadPrivateKeySetting(string settingName)
			{
				bool flag = false;
				string environmentVariable = Environment.GetEnvironmentVariable("COMPlus_" + settingName);
				if (environmentVariable != null && bool.TryParse(environmentVariable, out flag))
				{
					return flag;
				}
				if (PrivateKeyEnforcer.Impl.TryReadSettingFromRegistry(settingName, Registry.CurrentUser, ref flag))
				{
					return flag;
				}
				return !PrivateKeyEnforcer.Impl.TryReadSettingFromRegistry(settingName, Registry.LocalMachine, ref flag) || flag;
			}

			// Token: 0x0600455D RID: 17757 RVA: 0x00121758 File Offset: 0x0011F958
			[SecuritySafeCritical]
			[SecurityPermission(SecurityAction.Assert, UnmanagedCode = true)]
			[RegistryPermission(SecurityAction.Assert, Unrestricted = true)]
			private static bool TryReadSettingFromRegistry(string regValueName, RegistryKey regKey, ref bool value)
			{
				try
				{
					using (RegistryKey registryKey = regKey.OpenSubKey("SOFTWARE\\Microsoft\\.NETFramework", false))
					{
						if (registryKey != null)
						{
							object value2 = registryKey.GetValue(regValueName);
							if (value2 != null)
							{
								value = Convert.ToBoolean(value2, CultureInfo.InvariantCulture);
								return true;
							}
						}
					}
				}
				catch
				{
				}
				return false;
			}

			// Token: 0x02000932 RID: 2354
			private enum PrivateKeySetting
			{
				// Token: 0x04003DD4 RID: 15828
				Uninitialized,
				// Token: 0x04003DD5 RID: 15829
				Enabled,
				// Token: 0x04003DD6 RID: 15830
				Disabled
			}
		}
	}
}
