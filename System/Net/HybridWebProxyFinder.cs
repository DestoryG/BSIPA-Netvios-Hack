using System;
using System.Collections.Generic;
using System.IO;
using System.Security;
using System.Security.Permissions;
using Microsoft.Win32;

namespace System.Net
{
	// Token: 0x0200010E RID: 270
	internal sealed class HybridWebProxyFinder : IWebProxyFinder, IDisposable
	{
		// Token: 0x06000AF3 RID: 2803 RVA: 0x0003CA8C File Offset: 0x0003AC8C
		static HybridWebProxyFinder()
		{
			HybridWebProxyFinder.InitializeFallbackSettings();
		}

		// Token: 0x06000AF4 RID: 2804 RVA: 0x0003CA93 File Offset: 0x0003AC93
		public HybridWebProxyFinder(AutoWebProxyScriptEngine engine)
		{
			this.engine = engine;
			this.winHttpFinder = new WinHttpWebProxyFinder(engine);
			this.currentFinder = this.winHttpFinder;
		}

		// Token: 0x17000283 RID: 643
		// (get) Token: 0x06000AF5 RID: 2805 RVA: 0x0003CABA File Offset: 0x0003ACBA
		public bool IsValid
		{
			get
			{
				return this.currentFinder.IsValid;
			}
		}

		// Token: 0x06000AF6 RID: 2806 RVA: 0x0003CAC8 File Offset: 0x0003ACC8
		public bool GetProxies(Uri destination, out IList<string> proxyList)
		{
			if (this.currentFinder.GetProxies(destination, out proxyList))
			{
				return true;
			}
			if (HybridWebProxyFinder.allowFallback && this.currentFinder.IsUnrecognizedScheme && this.currentFinder == this.winHttpFinder)
			{
				if (this.netFinder == null)
				{
					this.netFinder = new NetWebProxyFinder(this.engine);
				}
				this.currentFinder = this.netFinder;
				return this.currentFinder.GetProxies(destination, out proxyList);
			}
			return false;
		}

		// Token: 0x06000AF7 RID: 2807 RVA: 0x0003CB3C File Offset: 0x0003AD3C
		public void Abort()
		{
			this.currentFinder.Abort();
		}

		// Token: 0x06000AF8 RID: 2808 RVA: 0x0003CB49 File Offset: 0x0003AD49
		public void Reset()
		{
			this.winHttpFinder.Reset();
			if (this.netFinder != null)
			{
				this.netFinder.Reset();
			}
			this.currentFinder = this.winHttpFinder;
		}

		// Token: 0x06000AF9 RID: 2809 RVA: 0x0003CB75 File Offset: 0x0003AD75
		public void Dispose()
		{
			this.Dispose(true);
		}

		// Token: 0x06000AFA RID: 2810 RVA: 0x0003CB7E File Offset: 0x0003AD7E
		private void Dispose(bool disposing)
		{
			if (disposing)
			{
				this.winHttpFinder.Dispose();
				if (this.netFinder != null)
				{
					this.netFinder.Dispose();
				}
			}
		}

		// Token: 0x06000AFB RID: 2811 RVA: 0x0003CBA4 File Offset: 0x0003ADA4
		[RegistryPermission(SecurityAction.Assert, Read = "HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\.NETFramework")]
		private static void InitializeFallbackSettings()
		{
			HybridWebProxyFinder.allowFallback = false;
			try
			{
				using (RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\.NETFramework"))
				{
					try
					{
						object value = registryKey.GetValue("LegacyWPADSupport", null);
						if (value != null && registryKey.GetValueKind("LegacyWPADSupport") == RegistryValueKind.DWord)
						{
							HybridWebProxyFinder.allowFallback = (int)value == 1;
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
		}

		// Token: 0x04000F4F RID: 3919
		private const string allowFallbackKey = "SOFTWARE\\Microsoft\\.NETFramework";

		// Token: 0x04000F50 RID: 3920
		private const string allowFallbackKeyPath = "HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\.NETFramework";

		// Token: 0x04000F51 RID: 3921
		private const string allowFallbackValueName = "LegacyWPADSupport";

		// Token: 0x04000F52 RID: 3922
		private static bool allowFallback;

		// Token: 0x04000F53 RID: 3923
		private NetWebProxyFinder netFinder;

		// Token: 0x04000F54 RID: 3924
		private WinHttpWebProxyFinder winHttpFinder;

		// Token: 0x04000F55 RID: 3925
		private BaseWebProxyFinder currentFinder;

		// Token: 0x04000F56 RID: 3926
		private AutoWebProxyScriptEngine engine;
	}
}
