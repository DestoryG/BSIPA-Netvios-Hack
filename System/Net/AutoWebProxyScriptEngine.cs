using System;
using System.Collections.Generic;
using System.Net.Configuration;
using System.Net.NetworkInformation;
using System.Security.Permissions;
using System.Security.Principal;
using System.Threading;

namespace System.Net
{
	// Token: 0x02000191 RID: 401
	internal class AutoWebProxyScriptEngine
	{
		// Token: 0x06000F69 RID: 3945 RVA: 0x0004FC6C File Offset: 0x0004DE6C
		[SecurityPermission(SecurityAction.Assert, Flags = SecurityPermissionFlag.ControlPrincipal)]
		internal AutoWebProxyScriptEngine(WebProxy proxy, bool useRegistry)
		{
			this.webProxy = proxy;
			this.m_UseRegistry = useRegistry;
			this.m_AutoDetector = AutoWebProxyScriptEngine.AutoDetector.CurrentAutoDetector;
			this.m_NetworkChangeStatus = this.m_AutoDetector.NetworkChangeStatus;
			SafeRegistryHandle.RegOpenCurrentUser(131097U, out this.hkcu);
			if (this.m_UseRegistry)
			{
				this.ListenForRegistry();
				this.m_Identity = WindowsIdentity.GetCurrent();
			}
			this.webProxyFinder = new HybridWebProxyFinder(this);
		}

		// Token: 0x06000F6A RID: 3946 RVA: 0x0004FCE0 File Offset: 0x0004DEE0
		private void EnterLock(ref int syncStatus)
		{
			if (syncStatus == 0)
			{
				lock (this)
				{
					if (syncStatus != 4)
					{
						syncStatus = 1;
						while (this.m_LockHeld)
						{
							Monitor.Wait(this);
							if (syncStatus == 4)
							{
								Monitor.Pulse(this);
								goto IL_003E;
							}
						}
						syncStatus = 2;
						this.m_LockHeld = true;
					}
					IL_003E:;
				}
			}
		}

		// Token: 0x06000F6B RID: 3947 RVA: 0x0004FD48 File Offset: 0x0004DF48
		private void ExitLock(ref int syncStatus)
		{
			if (syncStatus != 0 && syncStatus != 4)
			{
				lock (this)
				{
					this.m_LockHeld = false;
					if (syncStatus == 3)
					{
						this.webProxyFinder.Reset();
						syncStatus = 4;
					}
					else
					{
						syncStatus = 0;
					}
					Monitor.Pulse(this);
				}
			}
		}

		// Token: 0x06000F6C RID: 3948 RVA: 0x0004FDAC File Offset: 0x0004DFAC
		internal void Abort(ref int syncStatus)
		{
			lock (this)
			{
				switch (syncStatus)
				{
				case 0:
					syncStatus = 4;
					break;
				case 1:
					syncStatus = 4;
					Monitor.PulseAll(this);
					break;
				case 2:
					syncStatus = 3;
					this.webProxyFinder.Abort();
					break;
				}
			}
		}

		// Token: 0x1700036B RID: 875
		// (get) Token: 0x06000F6D RID: 3949 RVA: 0x0004FE18 File Offset: 0x0004E018
		// (set) Token: 0x06000F6E RID: 3950 RVA: 0x0004FE20 File Offset: 0x0004E020
		internal bool AutomaticallyDetectSettings
		{
			get
			{
				return this.automaticallyDetectSettings;
			}
			set
			{
				if (this.automaticallyDetectSettings != value)
				{
					this.automaticallyDetectSettings = value;
					this.webProxyFinder.Reset();
				}
			}
		}

		// Token: 0x1700036C RID: 876
		// (get) Token: 0x06000F6F RID: 3951 RVA: 0x0004FE3D File Offset: 0x0004E03D
		// (set) Token: 0x06000F70 RID: 3952 RVA: 0x0004FE45 File Offset: 0x0004E045
		internal Uri AutomaticConfigurationScript
		{
			get
			{
				return this.automaticConfigurationScript;
			}
			set
			{
				if (!object.Equals(this.automaticConfigurationScript, value))
				{
					this.automaticConfigurationScript = value;
					this.webProxyFinder.Reset();
				}
			}
		}

		// Token: 0x1700036D RID: 877
		// (get) Token: 0x06000F71 RID: 3953 RVA: 0x0004FE67 File Offset: 0x0004E067
		internal ICredentials Credentials
		{
			get
			{
				return this.webProxy.Credentials;
			}
		}

		// Token: 0x06000F72 RID: 3954 RVA: 0x0004FE74 File Offset: 0x0004E074
		internal bool GetProxies(Uri destination, out IList<string> proxyList)
		{
			int num = 0;
			return this.GetProxies(destination, out proxyList, ref num);
		}

		// Token: 0x06000F73 RID: 3955 RVA: 0x0004FE90 File Offset: 0x0004E090
		internal bool GetProxies(Uri destination, out IList<string> proxyList, ref int syncStatus)
		{
			proxyList = null;
			this.CheckForChanges(ref syncStatus);
			if (!this.webProxyFinder.IsValid)
			{
				if (this.retryWinHttpGetProxyForUrlTimer == null && SettingsSectionInternal.Section.AutoConfigUrlRetryInterval != 0)
				{
					long num = (long)(SettingsSectionInternal.Section.AutoConfigUrlRetryInterval * 1000);
					this.retryWinHttpGetProxyForUrlTimer = new Timer(delegate(object s)
					{
						WeakReference<AutoWebProxyScriptEngine> weakReference = (WeakReference<AutoWebProxyScriptEngine>)s;
						AutoWebProxyScriptEngine autoWebProxyScriptEngine;
						if (weakReference.TryGetTarget(out autoWebProxyScriptEngine))
						{
							autoWebProxyScriptEngine.executeWinHttpGetProxyForUrl = true;
						}
					}, new WeakReference<AutoWebProxyScriptEngine>(this), num, num);
				}
				if (!this.executeWinHttpGetProxyForUrl)
				{
					return false;
				}
				this.executeWinHttpGetProxyForUrl = false;
			}
			else if (this.retryWinHttpGetProxyForUrlTimer != null)
			{
				this.retryWinHttpGetProxyForUrlTimer.Dispose();
				this.retryWinHttpGetProxyForUrlTimer = null;
			}
			bool flag;
			try
			{
				this.EnterLock(ref syncStatus);
				if (syncStatus != 2)
				{
					flag = false;
				}
				else
				{
					flag = this.webProxyFinder.GetProxies(destination, out proxyList);
				}
			}
			finally
			{
				this.ExitLock(ref syncStatus);
			}
			return flag;
		}

		// Token: 0x06000F74 RID: 3956 RVA: 0x0004FF74 File Offset: 0x0004E174
		internal WebProxyData GetWebProxyData()
		{
			WebProxyDataBuilder webProxyDataBuilder;
			if (ComNetOS.IsWin7orLater)
			{
				webProxyDataBuilder = new WinHttpWebProxyBuilder();
			}
			else
			{
				webProxyDataBuilder = new RegBlobWebProxyDataBuilder(this.m_AutoDetector.Connectoid, this.hkcu);
			}
			return webProxyDataBuilder.Build();
		}

		// Token: 0x06000F75 RID: 3957 RVA: 0x0004FFB0 File Offset: 0x0004E1B0
		internal void Close()
		{
			if (this.m_AutoDetector != null)
			{
				int num = 0;
				try
				{
					this.EnterLock(ref num);
					if (this.m_AutoDetector != null)
					{
						this.registrySuppress = true;
						if (this.registryChangeEventPolicy != null)
						{
							this.registryChangeEventPolicy.Close();
							this.registryChangeEventPolicy = null;
						}
						if (this.registryChangeEventLM != null)
						{
							this.registryChangeEventLM.Close();
							this.registryChangeEventLM = null;
						}
						if (this.registryChangeEvent != null)
						{
							this.registryChangeEvent.Close();
							this.registryChangeEvent = null;
						}
						if (this.regKeyPolicy != null && !this.regKeyPolicy.IsInvalid)
						{
							this.regKeyPolicy.Close();
						}
						if (this.regKeyLM != null && !this.regKeyLM.IsInvalid)
						{
							this.regKeyLM.Close();
						}
						if (this.regKey != null && !this.regKey.IsInvalid)
						{
							this.regKey.Close();
						}
						if (this.hkcu != null)
						{
							this.hkcu.RegCloseKey();
							this.hkcu = null;
						}
						if (this.m_Identity != null)
						{
							this.m_Identity.Dispose();
							this.m_Identity = null;
						}
						this.webProxyFinder.Dispose();
						this.m_AutoDetector = null;
					}
				}
				finally
				{
					this.ExitLock(ref num);
				}
			}
		}

		// Token: 0x06000F76 RID: 3958 RVA: 0x00050100 File Offset: 0x0004E300
		internal void ListenForRegistry()
		{
			if (!this.registrySuppress)
			{
				if (this.registryChangeEvent == null)
				{
					this.ListenForRegistryHelper(ref this.regKey, ref this.registryChangeEvent, IntPtr.Zero, "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Internet Settings\\Connections");
				}
				if (this.registryChangeEventLM == null)
				{
					this.ListenForRegistryHelper(ref this.regKeyLM, ref this.registryChangeEventLM, UnsafeNclNativeMethods.RegistryHelper.HKEY_LOCAL_MACHINE, "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Internet Settings\\Connections");
				}
				if (this.registryChangeEventPolicy == null)
				{
					this.ListenForRegistryHelper(ref this.regKeyPolicy, ref this.registryChangeEventPolicy, UnsafeNclNativeMethods.RegistryHelper.HKEY_LOCAL_MACHINE, "SOFTWARE\\Policies\\Microsoft\\Windows\\CurrentVersion\\Internet Settings");
				}
				if (this.registryChangeEvent == null && this.registryChangeEventLM == null && this.registryChangeEventPolicy == null)
				{
					this.registrySuppress = true;
				}
			}
		}

		// Token: 0x06000F77 RID: 3959 RVA: 0x000501A4 File Offset: 0x0004E3A4
		private void ListenForRegistryHelper(ref SafeRegistryHandle key, ref AutoResetEvent changeEvent, IntPtr baseKey, string subKey)
		{
			uint num = 0U;
			if (key == null || key.IsInvalid)
			{
				if (baseKey == IntPtr.Zero)
				{
					if (this.hkcu != null)
					{
						num = this.hkcu.RegOpenKeyEx(subKey, 0U, 131097U, out key);
					}
					else
					{
						num = 1168U;
					}
				}
				else
				{
					num = SafeRegistryHandle.RegOpenKeyEx(baseKey, subKey, 0U, 131097U, out key);
				}
				if (num == 0U)
				{
					changeEvent = new AutoResetEvent(false);
				}
			}
			if (num == 0U)
			{
				num = key.RegNotifyChangeKeyValue(true, 4U, changeEvent.SafeWaitHandle, true);
			}
			if (num != 0U)
			{
				if (key != null && !key.IsInvalid)
				{
					try
					{
						num = key.RegCloseKey();
					}
					catch (Exception ex)
					{
						if (NclUtilities.IsFatal(ex))
						{
							throw;
						}
					}
				}
				key = null;
				if (changeEvent != null)
				{
					changeEvent.Close();
					changeEvent = null;
				}
			}
		}

		// Token: 0x06000F78 RID: 3960 RVA: 0x0005026C File Offset: 0x0004E46C
		private void RegistryChanged()
		{
			if (Logging.On)
			{
				Logging.PrintWarning(Logging.Web, SR.GetString("net_log_proxy_system_setting_update"));
			}
			WebProxyData webProxyData;
			using (this.m_Identity.Impersonate())
			{
				webProxyData = this.GetWebProxyData();
			}
			this.webProxy.Update(webProxyData);
		}

		// Token: 0x06000F79 RID: 3961 RVA: 0x000502D0 File Offset: 0x0004E4D0
		private void ConnectoidChanged()
		{
			if (Logging.On)
			{
				Logging.PrintWarning(Logging.Web, SR.GetString("net_log_proxy_update_due_to_ip_config_change"));
			}
			this.m_AutoDetector = AutoWebProxyScriptEngine.AutoDetector.CurrentAutoDetector;
			if (this.m_UseRegistry)
			{
				WebProxyData webProxyData;
				using (this.m_Identity.Impersonate())
				{
					webProxyData = this.GetWebProxyData();
				}
				this.webProxy.Update(webProxyData);
			}
			if (this.automaticallyDetectSettings)
			{
				this.webProxyFinder.Reset();
			}
		}

		// Token: 0x06000F7A RID: 3962 RVA: 0x0005035C File Offset: 0x0004E55C
		internal void CheckForChanges()
		{
			int num = 0;
			this.CheckForChanges(ref num);
		}

		// Token: 0x06000F7B RID: 3963 RVA: 0x00050374 File Offset: 0x0004E574
		[SecurityPermission(SecurityAction.Assert, Flags = SecurityPermissionFlag.ControlPrincipal)]
		private void CheckForChanges(ref int syncStatus)
		{
			try
			{
				bool flag = AutoWebProxyScriptEngine.AutoDetector.CheckForNetworkChanges(ref this.m_NetworkChangeStatus);
				bool flag2 = false;
				if (flag || this.needConnectoidUpdate)
				{
					try
					{
						this.EnterLock(ref syncStatus);
						if (flag || this.needConnectoidUpdate)
						{
							this.needConnectoidUpdate = syncStatus != 2;
							if (!this.needConnectoidUpdate)
							{
								this.ConnectoidChanged();
								flag2 = true;
							}
						}
					}
					finally
					{
						this.ExitLock(ref syncStatus);
					}
				}
				if (this.m_UseRegistry)
				{
					bool flag3 = false;
					AutoResetEvent autoResetEvent = this.registryChangeEvent;
					if (this.registryChangeDeferred || (flag3 = autoResetEvent != null && autoResetEvent.WaitOne(0, false)))
					{
						try
						{
							this.EnterLock(ref syncStatus);
							if (flag3 || this.registryChangeDeferred)
							{
								this.registryChangeDeferred = syncStatus != 2;
								if (!this.registryChangeDeferred && this.registryChangeEvent != null)
								{
									try
									{
										using (this.m_Identity.Impersonate())
										{
											this.ListenForRegistryHelper(ref this.regKey, ref this.registryChangeEvent, IntPtr.Zero, "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Internet Settings\\Connections");
										}
									}
									catch
									{
										throw;
									}
									this.needRegistryUpdate = true;
								}
							}
						}
						finally
						{
							this.ExitLock(ref syncStatus);
						}
					}
					flag3 = false;
					autoResetEvent = this.registryChangeEventLM;
					if (this.registryChangeLMDeferred || (flag3 = autoResetEvent != null && autoResetEvent.WaitOne(0, false)))
					{
						try
						{
							this.EnterLock(ref syncStatus);
							if (flag3 || this.registryChangeLMDeferred)
							{
								this.registryChangeLMDeferred = syncStatus != 2;
								if (!this.registryChangeLMDeferred && this.registryChangeEventLM != null)
								{
									try
									{
										using (this.m_Identity.Impersonate())
										{
											this.ListenForRegistryHelper(ref this.regKeyLM, ref this.registryChangeEventLM, UnsafeNclNativeMethods.RegistryHelper.HKEY_LOCAL_MACHINE, "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Internet Settings\\Connections");
										}
									}
									catch
									{
										throw;
									}
									this.needRegistryUpdate = true;
								}
							}
						}
						finally
						{
							this.ExitLock(ref syncStatus);
						}
					}
					flag3 = false;
					autoResetEvent = this.registryChangeEventPolicy;
					if (this.registryChangePolicyDeferred || (flag3 = autoResetEvent != null && autoResetEvent.WaitOne(0, false)))
					{
						try
						{
							this.EnterLock(ref syncStatus);
							if (flag3 || this.registryChangePolicyDeferred)
							{
								this.registryChangePolicyDeferred = syncStatus != 2;
								if (!this.registryChangePolicyDeferred && this.registryChangeEventPolicy != null)
								{
									try
									{
										using (this.m_Identity.Impersonate())
										{
											this.ListenForRegistryHelper(ref this.regKeyPolicy, ref this.registryChangeEventPolicy, UnsafeNclNativeMethods.RegistryHelper.HKEY_LOCAL_MACHINE, "SOFTWARE\\Policies\\Microsoft\\Windows\\CurrentVersion\\Internet Settings");
										}
									}
									catch
									{
										throw;
									}
									this.needRegistryUpdate = true;
								}
							}
						}
						finally
						{
							this.ExitLock(ref syncStatus);
						}
					}
					if (this.needRegistryUpdate)
					{
						try
						{
							this.EnterLock(ref syncStatus);
							if (this.needRegistryUpdate && syncStatus == 2)
							{
								this.needRegistryUpdate = false;
								if (!flag2)
								{
									this.RegistryChanged();
								}
							}
						}
						finally
						{
							this.ExitLock(ref syncStatus);
						}
					}
				}
			}
			catch (ObjectDisposedException)
			{
			}
		}

		// Token: 0x040012AF RID: 4783
		private bool automaticallyDetectSettings;

		// Token: 0x040012B0 RID: 4784
		private Uri automaticConfigurationScript;

		// Token: 0x040012B1 RID: 4785
		private WebProxy webProxy;

		// Token: 0x040012B2 RID: 4786
		private IWebProxyFinder webProxyFinder;

		// Token: 0x040012B3 RID: 4787
		private bool executeWinHttpGetProxyForUrl;

		// Token: 0x040012B4 RID: 4788
		private Timer retryWinHttpGetProxyForUrlTimer;

		// Token: 0x040012B5 RID: 4789
		private bool m_LockHeld;

		// Token: 0x040012B6 RID: 4790
		private bool m_UseRegistry;

		// Token: 0x040012B7 RID: 4791
		private int m_NetworkChangeStatus;

		// Token: 0x040012B8 RID: 4792
		private AutoWebProxyScriptEngine.AutoDetector m_AutoDetector;

		// Token: 0x040012B9 RID: 4793
		private SafeRegistryHandle hkcu;

		// Token: 0x040012BA RID: 4794
		private WindowsIdentity m_Identity;

		// Token: 0x040012BB RID: 4795
		private SafeRegistryHandle regKey;

		// Token: 0x040012BC RID: 4796
		private SafeRegistryHandle regKeyLM;

		// Token: 0x040012BD RID: 4797
		private SafeRegistryHandle regKeyPolicy;

		// Token: 0x040012BE RID: 4798
		private AutoResetEvent registryChangeEvent;

		// Token: 0x040012BF RID: 4799
		private AutoResetEvent registryChangeEventLM;

		// Token: 0x040012C0 RID: 4800
		private AutoResetEvent registryChangeEventPolicy;

		// Token: 0x040012C1 RID: 4801
		private bool registryChangeDeferred;

		// Token: 0x040012C2 RID: 4802
		private bool registryChangeLMDeferred;

		// Token: 0x040012C3 RID: 4803
		private bool registryChangePolicyDeferred;

		// Token: 0x040012C4 RID: 4804
		private bool needRegistryUpdate;

		// Token: 0x040012C5 RID: 4805
		private bool needConnectoidUpdate;

		// Token: 0x040012C6 RID: 4806
		private bool registrySuppress;

		// Token: 0x0200073F RID: 1855
		private static class SyncStatus
		{
			// Token: 0x040031B6 RID: 12726
			internal const int Unlocked = 0;

			// Token: 0x040031B7 RID: 12727
			internal const int Locking = 1;

			// Token: 0x040031B8 RID: 12728
			internal const int LockOwner = 2;

			// Token: 0x040031B9 RID: 12729
			internal const int AbortedLocked = 3;

			// Token: 0x040031BA RID: 12730
			internal const int Aborted = 4;
		}

		// Token: 0x02000740 RID: 1856
		private class AutoDetector
		{
			// Token: 0x060041C6 RID: 16838 RVA: 0x001119DC File Offset: 0x0010FBDC
			private static void Initialize()
			{
				if (!AutoWebProxyScriptEngine.AutoDetector.s_Initialized)
				{
					object obj = AutoWebProxyScriptEngine.AutoDetector.s_LockObject;
					lock (obj)
					{
						if (!AutoWebProxyScriptEngine.AutoDetector.s_Initialized)
						{
							AutoWebProxyScriptEngine.AutoDetector.s_CurrentAutoDetector = new AutoWebProxyScriptEngine.AutoDetector(UnsafeNclNativeMethods.RasHelper.GetCurrentConnectoid(), 1);
							if (NetworkChange.CanListenForNetworkChanges)
							{
								AutoWebProxyScriptEngine.AutoDetector.s_AddressChange = new NetworkAddressChangePolled();
							}
							if (UnsafeNclNativeMethods.RasHelper.RasSupported)
							{
								AutoWebProxyScriptEngine.AutoDetector.s_RasHelper = new UnsafeNclNativeMethods.RasHelper();
							}
							AutoWebProxyScriptEngine.AutoDetector.s_CurrentVersion = 1;
							AutoWebProxyScriptEngine.AutoDetector.s_Initialized = true;
						}
					}
				}
			}

			// Token: 0x060041C7 RID: 16839 RVA: 0x00111A70 File Offset: 0x0010FC70
			internal static bool CheckForNetworkChanges(ref int changeStatus)
			{
				AutoWebProxyScriptEngine.AutoDetector.Initialize();
				AutoWebProxyScriptEngine.AutoDetector.CheckForChanges();
				int num = changeStatus;
				changeStatus = Volatile.Read(ref AutoWebProxyScriptEngine.AutoDetector.s_CurrentVersion);
				return num != changeStatus;
			}

			// Token: 0x060041C8 RID: 16840 RVA: 0x00111AA0 File Offset: 0x0010FCA0
			private static void CheckForChanges()
			{
				bool flag = false;
				if (AutoWebProxyScriptEngine.AutoDetector.s_RasHelper != null && AutoWebProxyScriptEngine.AutoDetector.s_RasHelper.HasChanged)
				{
					AutoWebProxyScriptEngine.AutoDetector.s_RasHelper.Reset();
					flag = true;
				}
				if (AutoWebProxyScriptEngine.AutoDetector.s_AddressChange != null && AutoWebProxyScriptEngine.AutoDetector.s_AddressChange.CheckAndReset())
				{
					flag = true;
				}
				if (flag)
				{
					int num = Interlocked.Increment(ref AutoWebProxyScriptEngine.AutoDetector.s_CurrentVersion);
					AutoWebProxyScriptEngine.AutoDetector.s_CurrentAutoDetector = new AutoWebProxyScriptEngine.AutoDetector(UnsafeNclNativeMethods.RasHelper.GetCurrentConnectoid(), num);
				}
			}

			// Token: 0x17000F04 RID: 3844
			// (get) Token: 0x060041C9 RID: 16841 RVA: 0x00111B0D File Offset: 0x0010FD0D
			internal static AutoWebProxyScriptEngine.AutoDetector CurrentAutoDetector
			{
				get
				{
					AutoWebProxyScriptEngine.AutoDetector.Initialize();
					return AutoWebProxyScriptEngine.AutoDetector.s_CurrentAutoDetector;
				}
			}

			// Token: 0x060041CA RID: 16842 RVA: 0x00111B1B File Offset: 0x0010FD1B
			private AutoDetector(string connectoid, int currentVersion)
			{
				this.m_Connectoid = connectoid;
				this.m_CurrentVersion = currentVersion;
			}

			// Token: 0x17000F05 RID: 3845
			// (get) Token: 0x060041CB RID: 16843 RVA: 0x00111B31 File Offset: 0x0010FD31
			internal string Connectoid
			{
				get
				{
					return this.m_Connectoid;
				}
			}

			// Token: 0x17000F06 RID: 3846
			// (get) Token: 0x060041CC RID: 16844 RVA: 0x00111B39 File Offset: 0x0010FD39
			internal int NetworkChangeStatus
			{
				get
				{
					return this.m_CurrentVersion;
				}
			}

			// Token: 0x040031BB RID: 12731
			private static volatile NetworkAddressChangePolled s_AddressChange;

			// Token: 0x040031BC RID: 12732
			private static volatile UnsafeNclNativeMethods.RasHelper s_RasHelper;

			// Token: 0x040031BD RID: 12733
			private static int s_CurrentVersion;

			// Token: 0x040031BE RID: 12734
			private static volatile AutoWebProxyScriptEngine.AutoDetector s_CurrentAutoDetector;

			// Token: 0x040031BF RID: 12735
			private static volatile bool s_Initialized;

			// Token: 0x040031C0 RID: 12736
			private static object s_LockObject = new object();

			// Token: 0x040031C1 RID: 12737
			private readonly string m_Connectoid;

			// Token: 0x040031C2 RID: 12738
			private readonly int m_CurrentVersion;
		}
	}
}
