using System;
using System.Configuration;
using System.Globalization;
using System.Reflection;
using System.Security;
using System.Security.Permissions;
using System.Security.Principal;
using System.Threading;

namespace System.Net.Configuration
{
	// Token: 0x02000330 RID: 816
	internal sealed class DefaultProxySectionInternal
	{
		// Token: 0x06001D37 RID: 7479 RVA: 0x0008B034 File Offset: 0x00089234
		[SecurityPermission(SecurityAction.Assert, Flags = SecurityPermissionFlag.ControlPrincipal)]
		internal DefaultProxySectionInternal(DefaultProxySection section)
		{
			if (!section.Enabled)
			{
				return;
			}
			if (section.Proxy.AutoDetect == ProxyElement.AutoDetectValues.Unspecified && section.Proxy.ScriptLocation == null && string.IsNullOrEmpty(section.Module.Type) && section.Proxy.UseSystemDefault != ProxyElement.UseSystemDefaultValues.True && section.Proxy.ProxyAddress == null && section.Proxy.BypassOnLocal == ProxyElement.BypassOnLocalValues.Unspecified && section.BypassList.Count == 0)
			{
				if (section.Proxy.UseSystemDefault == ProxyElement.UseSystemDefaultValues.False)
				{
					this.webProxy = new EmptyWebProxy();
					return;
				}
				try
				{
					new SecurityPermission(SecurityPermissionFlag.UnmanagedCode | SecurityPermissionFlag.ControlPrincipal).Assert();
					using (WindowsIdentity.Impersonate(IntPtr.Zero))
					{
						CodeAccessPermission.RevertAssert();
						this.webProxy = new WebRequest.WebProxyWrapper(new WebProxy(true));
					}
					goto IL_0309;
				}
				catch
				{
					throw;
				}
			}
			if (!string.IsNullOrEmpty(section.Module.Type))
			{
				Type type = Type.GetType(section.Module.Type, true, true);
				if ((type.Attributes & TypeAttributes.VisibilityMask) != TypeAttributes.Public)
				{
					throw new ConfigurationErrorsException(SR.GetString("net_config_proxy_module_not_public"));
				}
				if (!typeof(IWebProxy).IsAssignableFrom(type))
				{
					throw new InvalidCastException(SR.GetString("net_invalid_cast", new object[] { type.FullName, "IWebProxy" }));
				}
				this.webProxy = (IWebProxy)Activator.CreateInstance(type, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.CreateInstance, null, new object[0], CultureInfo.InvariantCulture);
			}
			else
			{
				if (section.Proxy.UseSystemDefault == ProxyElement.UseSystemDefaultValues.True && section.Proxy.AutoDetect == ProxyElement.AutoDetectValues.Unspecified && section.Proxy.ScriptLocation == null)
				{
					try
					{
						new SecurityPermission(SecurityPermissionFlag.UnmanagedCode | SecurityPermissionFlag.ControlPrincipal).Assert();
						using (WindowsIdentity.Impersonate(IntPtr.Zero))
						{
							CodeAccessPermission.RevertAssert();
							this.webProxy = new WebProxy(false);
						}
						goto IL_01FE;
					}
					catch
					{
						throw;
					}
				}
				this.webProxy = new WebProxy();
			}
			IL_01FE:
			WebProxy webProxy = this.webProxy as WebProxy;
			if (webProxy != null)
			{
				if (section.Proxy.AutoDetect != ProxyElement.AutoDetectValues.Unspecified)
				{
					webProxy.AutoDetect = section.Proxy.AutoDetect == ProxyElement.AutoDetectValues.True;
				}
				if (section.Proxy.ScriptLocation != null)
				{
					webProxy.ScriptLocation = section.Proxy.ScriptLocation;
				}
				if (section.Proxy.BypassOnLocal != ProxyElement.BypassOnLocalValues.Unspecified)
				{
					webProxy.BypassProxyOnLocal = section.Proxy.BypassOnLocal == ProxyElement.BypassOnLocalValues.True;
				}
				if (section.Proxy.ProxyAddress != null)
				{
					webProxy.Address = section.Proxy.ProxyAddress;
				}
				int count = section.BypassList.Count;
				if (count > 0)
				{
					string[] array = new string[section.BypassList.Count];
					for (int i = 0; i < count; i++)
					{
						array[i] = section.BypassList[i].Address;
					}
					webProxy.BypassList = array;
				}
				if (section.Module.Type == null)
				{
					this.webProxy = new WebRequest.WebProxyWrapper(webProxy);
				}
			}
			IL_0309:
			if (this.webProxy != null && section.UseDefaultCredentials)
			{
				this.webProxy.Credentials = SystemNetworkCredential.defaultCredential;
			}
		}

		// Token: 0x17000740 RID: 1856
		// (get) Token: 0x06001D38 RID: 7480 RVA: 0x0008B3A0 File Offset: 0x000895A0
		internal static object ClassSyncObject
		{
			get
			{
				if (DefaultProxySectionInternal.classSyncObject == null)
				{
					object obj = new object();
					Interlocked.CompareExchange(ref DefaultProxySectionInternal.classSyncObject, obj, null);
				}
				return DefaultProxySectionInternal.classSyncObject;
			}
		}

		// Token: 0x06001D39 RID: 7481 RVA: 0x0008B3CC File Offset: 0x000895CC
		internal static DefaultProxySectionInternal GetSection()
		{
			object obj = DefaultProxySectionInternal.ClassSyncObject;
			DefaultProxySectionInternal defaultProxySectionInternal;
			lock (obj)
			{
				DefaultProxySection defaultProxySection = PrivilegedConfigurationManager.GetSection(ConfigurationStrings.DefaultProxySectionPath) as DefaultProxySection;
				if (defaultProxySection == null)
				{
					defaultProxySectionInternal = null;
				}
				else
				{
					try
					{
						defaultProxySectionInternal = new DefaultProxySectionInternal(defaultProxySection);
					}
					catch (Exception ex)
					{
						if (NclUtilities.IsFatal(ex))
						{
							throw;
						}
						throw new ConfigurationErrorsException(SR.GetString("net_config_proxy"), ex);
					}
				}
			}
			return defaultProxySectionInternal;
		}

		// Token: 0x17000741 RID: 1857
		// (get) Token: 0x06001D3A RID: 7482 RVA: 0x0008B450 File Offset: 0x00089650
		internal IWebProxy WebProxy
		{
			get
			{
				return this.webProxy;
			}
		}

		// Token: 0x04001C26 RID: 7206
		private IWebProxy webProxy;

		// Token: 0x04001C27 RID: 7207
		private static object classSyncObject;
	}
}
