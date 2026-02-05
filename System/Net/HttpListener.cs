using System;
using System.Collections;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Authentication.ExtendedProtection;
using System.Security.Permissions;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace System.Net
{
	// Token: 0x020000F7 RID: 247
	public sealed class HttpListener : IDisposable
	{
		// Token: 0x06000899 RID: 2201 RVA: 0x0002F8B8 File Offset: 0x0002DAB8
		public HttpListener()
		{
			if (Logging.On)
			{
				Logging.Enter(Logging.HttpListener, this, "HttpListener", "");
			}
			if (!UnsafeNclNativeMethods.HttpApi.Supported)
			{
				throw new PlatformNotSupportedException();
			}
			this.m_State = HttpListener.State.Stopped;
			this.m_InternalLock = new object();
			this.m_DefaultServiceNames = new ServiceNameStore();
			this.m_TimeoutManager = new HttpListenerTimeoutManager(this);
			this.m_ExtendedProtectionPolicy = new ExtendedProtectionPolicy(PolicyEnforcement.Never);
			if (Logging.On)
			{
				Logging.Exit(Logging.HttpListener, this, "HttpListener", "");
			}
		}

		// Token: 0x170001AE RID: 430
		// (get) Token: 0x0600089A RID: 2202 RVA: 0x0002F95D File Offset: 0x0002DB5D
		internal CriticalHandle RequestQueueHandle
		{
			get
			{
				return this.m_RequestQueueHandle;
			}
		}

		// Token: 0x170001AF RID: 431
		// (get) Token: 0x0600089B RID: 2203 RVA: 0x0002F968 File Offset: 0x0002DB68
		// (set) Token: 0x0600089C RID: 2204 RVA: 0x0002F988 File Offset: 0x0002DB88
		public AuthenticationSchemeSelector AuthenticationSchemeSelectorDelegate
		{
			get
			{
				HttpListener.AuthenticationSelectorInfo authenticationDelegate = this.m_AuthenticationDelegate;
				if (authenticationDelegate != null)
				{
					return authenticationDelegate.Delegate;
				}
				return null;
			}
			set
			{
				this.CheckDisposed();
				try
				{
					new SecurityPermission(SecurityPermissionFlag.ControlPrincipal).Demand();
					this.m_AuthenticationDelegate = new HttpListener.AuthenticationSelectorInfo(value, true);
				}
				catch (SecurityException ex)
				{
					this.m_SecurityException = ex;
					this.m_AuthenticationDelegate = new HttpListener.AuthenticationSelectorInfo(value, false);
				}
			}
		}

		// Token: 0x170001B0 RID: 432
		// (get) Token: 0x0600089D RID: 2205 RVA: 0x0002F9E0 File Offset: 0x0002DBE0
		// (set) Token: 0x0600089E RID: 2206 RVA: 0x0002F9E8 File Offset: 0x0002DBE8
		public HttpListener.ExtendedProtectionSelector ExtendedProtectionSelectorDelegate
		{
			get
			{
				return this.m_ExtendedProtectionSelectorDelegate;
			}
			set
			{
				this.CheckDisposed();
				if (value == null)
				{
					throw new ArgumentNullException();
				}
				if (!AuthenticationManager.OSSupportsExtendedProtection)
				{
					throw new PlatformNotSupportedException(SR.GetString("security_ExtendedProtection_NoOSSupport"));
				}
				this.m_ExtendedProtectionSelectorDelegate = value;
			}
		}

		// Token: 0x170001B1 RID: 433
		// (get) Token: 0x0600089F RID: 2207 RVA: 0x0002FA17 File Offset: 0x0002DC17
		// (set) Token: 0x060008A0 RID: 2208 RVA: 0x0002FA1F File Offset: 0x0002DC1F
		public AuthenticationSchemes AuthenticationSchemes
		{
			get
			{
				return this.m_AuthenticationScheme;
			}
			set
			{
				this.CheckDisposed();
				if ((value & (AuthenticationSchemes.Digest | AuthenticationSchemes.Negotiate | AuthenticationSchemes.Ntlm)) != AuthenticationSchemes.None)
				{
					new SecurityPermission(SecurityPermissionFlag.ControlPrincipal).Demand();
				}
				this.m_AuthenticationScheme = value;
			}
		}

		// Token: 0x170001B2 RID: 434
		// (get) Token: 0x060008A1 RID: 2209 RVA: 0x0002FA42 File Offset: 0x0002DC42
		// (set) Token: 0x060008A2 RID: 2210 RVA: 0x0002FA4C File Offset: 0x0002DC4C
		public ExtendedProtectionPolicy ExtendedProtectionPolicy
		{
			get
			{
				return this.m_ExtendedProtectionPolicy;
			}
			set
			{
				this.CheckDisposed();
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				if (!AuthenticationManager.OSSupportsExtendedProtection && value.PolicyEnforcement == PolicyEnforcement.Always)
				{
					throw new PlatformNotSupportedException(SR.GetString("security_ExtendedProtection_NoOSSupport"));
				}
				if (value.CustomChannelBinding != null)
				{
					throw new ArgumentException(SR.GetString("net_listener_cannot_set_custom_cbt"), "CustomChannelBinding");
				}
				this.m_ExtendedProtectionPolicy = value;
			}
		}

		// Token: 0x170001B3 RID: 435
		// (get) Token: 0x060008A3 RID: 2211 RVA: 0x0002FAB1 File Offset: 0x0002DCB1
		public ServiceNameCollection DefaultServiceNames
		{
			get
			{
				return this.m_DefaultServiceNames.ServiceNames;
			}
		}

		// Token: 0x170001B4 RID: 436
		// (get) Token: 0x060008A4 RID: 2212 RVA: 0x0002FABE File Offset: 0x0002DCBE
		// (set) Token: 0x060008A5 RID: 2213 RVA: 0x0002FAC6 File Offset: 0x0002DCC6
		public string Realm
		{
			get
			{
				return this.m_Realm;
			}
			set
			{
				this.CheckDisposed();
				this.m_Realm = value;
			}
		}

		// Token: 0x060008A6 RID: 2214 RVA: 0x0002FAD8 File Offset: 0x0002DCD8
		private void ValidateV2Property()
		{
			object internalLock = this.m_InternalLock;
			lock (internalLock)
			{
				this.CheckDisposed();
				this.SetupV2Config();
			}
		}

		// Token: 0x060008A7 RID: 2215 RVA: 0x0002FB20 File Offset: 0x0002DD20
		private void SetUrlGroupProperty(UnsafeNclNativeMethods.HttpApi.HTTP_SERVER_PROPERTY property, IntPtr info, uint infosize)
		{
			uint num = UnsafeNclNativeMethods.HttpApi.HttpSetUrlGroupProperty(this.m_UrlGroupId, property, info, infosize);
			if (num != 0U)
			{
				HttpListenerException ex = new HttpListenerException((int)num);
				if (Logging.On)
				{
					Logging.Exception(Logging.HttpListener, this, "HttpSetUrlGroupProperty:: Property: " + property.ToString(), ex);
				}
				throw ex;
			}
		}

		// Token: 0x060008A8 RID: 2216 RVA: 0x0002FB74 File Offset: 0x0002DD74
		internal unsafe void SetServerTimeout(int[] timeouts, uint minSendBytesPerSecond)
		{
			this.ValidateV2Property();
			UnsafeNclNativeMethods.HttpApi.HTTP_TIMEOUT_LIMIT_INFO http_TIMEOUT_LIMIT_INFO = default(UnsafeNclNativeMethods.HttpApi.HTTP_TIMEOUT_LIMIT_INFO);
			http_TIMEOUT_LIMIT_INFO.Flags = UnsafeNclNativeMethods.HttpApi.HTTP_FLAGS.HTTP_RECEIVE_REQUEST_FLAG_COPY_BODY;
			http_TIMEOUT_LIMIT_INFO.DrainEntityBody = (ushort)timeouts[1];
			http_TIMEOUT_LIMIT_INFO.EntityBody = (ushort)timeouts[0];
			http_TIMEOUT_LIMIT_INFO.RequestQueue = (ushort)timeouts[2];
			http_TIMEOUT_LIMIT_INFO.IdleConnection = (ushort)timeouts[3];
			http_TIMEOUT_LIMIT_INFO.HeaderWait = (ushort)timeouts[4];
			http_TIMEOUT_LIMIT_INFO.MinSendRate = minSendBytesPerSecond;
			IntPtr intPtr = new IntPtr((void*)(&http_TIMEOUT_LIMIT_INFO));
			this.SetUrlGroupProperty(UnsafeNclNativeMethods.HttpApi.HTTP_SERVER_PROPERTY.HttpServerTimeoutsProperty, intPtr, (uint)Marshal.SizeOf(typeof(UnsafeNclNativeMethods.HttpApi.HTTP_TIMEOUT_LIMIT_INFO)));
		}

		// Token: 0x170001B5 RID: 437
		// (get) Token: 0x060008A9 RID: 2217 RVA: 0x0002FBF7 File Offset: 0x0002DDF7
		public HttpListenerTimeoutManager TimeoutManager
		{
			get
			{
				this.ValidateV2Property();
				return this.m_TimeoutManager;
			}
		}

		// Token: 0x170001B6 RID: 438
		// (get) Token: 0x060008AA RID: 2218 RVA: 0x0002FC05 File Offset: 0x0002DE05
		public static bool IsSupported
		{
			get
			{
				return UnsafeNclNativeMethods.HttpApi.Supported;
			}
		}

		// Token: 0x170001B7 RID: 439
		// (get) Token: 0x060008AB RID: 2219 RVA: 0x0002FC0C File Offset: 0x0002DE0C
		public bool IsListening
		{
			get
			{
				return this.m_State == HttpListener.State.Started;
			}
		}

		// Token: 0x170001B8 RID: 440
		// (get) Token: 0x060008AC RID: 2220 RVA: 0x0002FC19 File Offset: 0x0002DE19
		// (set) Token: 0x060008AD RID: 2221 RVA: 0x0002FC21 File Offset: 0x0002DE21
		public bool IgnoreWriteExceptions
		{
			get
			{
				return this.m_IgnoreWriteExceptions;
			}
			set
			{
				this.CheckDisposed();
				this.m_IgnoreWriteExceptions = value;
			}
		}

		// Token: 0x170001B9 RID: 441
		// (get) Token: 0x060008AE RID: 2222 RVA: 0x0002FC30 File Offset: 0x0002DE30
		// (set) Token: 0x060008AF RID: 2223 RVA: 0x0002FC38 File Offset: 0x0002DE38
		public bool UnsafeConnectionNtlmAuthentication
		{
			get
			{
				return this.m_UnsafeConnectionNtlmAuthentication;
			}
			set
			{
				this.CheckDisposed();
				if (this.m_UnsafeConnectionNtlmAuthentication == value)
				{
					return;
				}
				object syncRoot = this.DisconnectResults.SyncRoot;
				lock (syncRoot)
				{
					if (this.m_UnsafeConnectionNtlmAuthentication != value)
					{
						this.m_UnsafeConnectionNtlmAuthentication = value;
						if (!value)
						{
							foreach (object obj in this.DisconnectResults.Values)
							{
								HttpListener.DisconnectAsyncResult disconnectAsyncResult = (HttpListener.DisconnectAsyncResult)obj;
								disconnectAsyncResult.AuthenticatedConnection = null;
							}
						}
					}
				}
			}
		}

		// Token: 0x170001BA RID: 442
		// (get) Token: 0x060008B0 RID: 2224 RVA: 0x0002FCEC File Offset: 0x0002DEEC
		private Hashtable DisconnectResults
		{
			get
			{
				if (this.m_DisconnectResults == null)
				{
					object internalLock = this.m_InternalLock;
					lock (internalLock)
					{
						if (this.m_DisconnectResults == null)
						{
							this.m_DisconnectResults = Hashtable.Synchronized(new Hashtable());
						}
					}
				}
				return this.m_DisconnectResults;
			}
		}

		// Token: 0x060008B1 RID: 2225 RVA: 0x0002FD4C File Offset: 0x0002DF4C
		internal unsafe void AddPrefix(string uriPrefix)
		{
			if (Logging.On)
			{
				Logging.Enter(Logging.HttpListener, this, "AddPrefix", "uriPrefix:" + uriPrefix);
			}
			string text = null;
			try
			{
				if (uriPrefix == null)
				{
					throw new ArgumentNullException("uriPrefix");
				}
				new WebPermission(NetworkAccess.Accept, uriPrefix).Demand();
				this.CheckDisposed();
				int num;
				if (string.Compare(uriPrefix, 0, "http://", 0, 7, StringComparison.OrdinalIgnoreCase) == 0)
				{
					num = 7;
				}
				else
				{
					if (string.Compare(uriPrefix, 0, "https://", 0, 8, StringComparison.OrdinalIgnoreCase) != 0)
					{
						throw new ArgumentException(SR.GetString("net_listener_scheme"), "uriPrefix");
					}
					num = 8;
				}
				bool flag = false;
				int num2 = num;
				while (num2 < uriPrefix.Length && uriPrefix[num2] != '/' && (uriPrefix[num2] != ':' || flag))
				{
					if (uriPrefix[num2] == '[')
					{
						if (flag)
						{
							num2 = num;
							break;
						}
						flag = true;
					}
					if (flag && uriPrefix[num2] == ']')
					{
						flag = false;
					}
					num2++;
				}
				if (num == num2)
				{
					throw new ArgumentException(SR.GetString("net_listener_host"), "uriPrefix");
				}
				if (uriPrefix[uriPrefix.Length - 1] != '/')
				{
					throw new ArgumentException(SR.GetString("net_listener_slash"), "uriPrefix");
				}
				text = ((uriPrefix[num2] == ':') ? string.Copy(uriPrefix) : (uriPrefix.Substring(0, num2) + ((num == 7) ? ":80" : ":443") + uriPrefix.Substring(num2)));
				try
				{
					fixed (string text2 = text)
					{
						char* ptr = text2;
						if (ptr != null)
						{
							ptr += RuntimeHelpers.OffsetToStringData / 2;
						}
						num = 0;
						while (ptr[num] != ':')
						{
							ptr[num] = (char)CaseInsensitiveAscii.AsciiToLower[(int)((byte)ptr[num])];
							num++;
						}
					}
				}
				finally
				{
					string text2 = null;
				}
				if (this.m_State == HttpListener.State.Started)
				{
					uint num3 = this.InternalAddPrefix(text);
					if (num3 != 0U)
					{
						if (num3 == 183U)
						{
							throw new HttpListenerException((int)num3, SR.GetString("net_listener_already", new object[] { text }));
						}
						throw new HttpListenerException((int)num3);
					}
				}
				this.m_UriPrefixes[uriPrefix] = text;
				this.m_DefaultServiceNames.Add(uriPrefix);
			}
			catch (Exception ex)
			{
				if (Logging.On)
				{
					Logging.Exception(Logging.HttpListener, this, "AddPrefix", ex);
				}
				throw;
			}
			finally
			{
				if (Logging.On)
				{
					Logging.Exit(Logging.HttpListener, this, "AddPrefix", "prefix:" + text);
				}
			}
		}

		// Token: 0x170001BB RID: 443
		// (get) Token: 0x060008B2 RID: 2226 RVA: 0x0002FFE0 File Offset: 0x0002E1E0
		public HttpListenerPrefixCollection Prefixes
		{
			get
			{
				if (Logging.On)
				{
					Logging.Enter(Logging.HttpListener, this, "Prefixes_get", "");
				}
				this.CheckDisposed();
				if (this.m_Prefixes == null)
				{
					this.m_Prefixes = new HttpListenerPrefixCollection(this);
				}
				return this.m_Prefixes;
			}
		}

		// Token: 0x060008B3 RID: 2227 RVA: 0x00030020 File Offset: 0x0002E220
		internal bool RemovePrefix(string uriPrefix)
		{
			if (Logging.On)
			{
				Logging.Enter(Logging.HttpListener, this, "RemovePrefix", "uriPrefix:" + uriPrefix);
			}
			try
			{
				this.CheckDisposed();
				if (uriPrefix == null)
				{
					throw new ArgumentNullException("uriPrefix");
				}
				if (!this.m_UriPrefixes.Contains(uriPrefix))
				{
					return false;
				}
				if (this.m_State == HttpListener.State.Started)
				{
					this.InternalRemovePrefix((string)this.m_UriPrefixes[uriPrefix]);
				}
				this.m_UriPrefixes.Remove(uriPrefix);
				this.m_DefaultServiceNames.Remove(uriPrefix);
			}
			catch (Exception ex)
			{
				if (Logging.On)
				{
					Logging.Exception(Logging.HttpListener, this, "RemovePrefix", ex);
				}
				throw;
			}
			finally
			{
				if (Logging.On)
				{
					Logging.Exit(Logging.HttpListener, this, "RemovePrefix", "uriPrefix:" + uriPrefix);
				}
			}
			return true;
		}

		// Token: 0x060008B4 RID: 2228 RVA: 0x00030114 File Offset: 0x0002E314
		internal void RemoveAll(bool clear)
		{
			if (Logging.On)
			{
				Logging.Enter(Logging.HttpListener, this, "RemoveAll", "");
			}
			try
			{
				this.CheckDisposed();
				if (this.m_UriPrefixes.Count > 0)
				{
					if (this.m_State == HttpListener.State.Started)
					{
						foreach (object obj in this.m_UriPrefixes.Values)
						{
							string text = (string)obj;
							this.InternalRemovePrefix(text);
						}
					}
					if (clear)
					{
						this.m_UriPrefixes.Clear();
						this.m_DefaultServiceNames.Clear();
					}
				}
			}
			finally
			{
				if (Logging.On)
				{
					Logging.Exit(Logging.HttpListener, this, "RemoveAll", "");
				}
			}
		}

		// Token: 0x060008B5 RID: 2229 RVA: 0x000301F4 File Offset: 0x0002E3F4
		private IntPtr DangerousGetHandle()
		{
			return ((HttpRequestQueueV2Handle)this.m_RequestQueueHandle).DangerousGetHandle();
		}

		// Token: 0x060008B6 RID: 2230 RVA: 0x00030208 File Offset: 0x0002E408
		[SecurityPermission(SecurityAction.Assert, Flags = SecurityPermissionFlag.UnmanagedCode)]
		internal void EnsureBoundHandle()
		{
			if (!this.m_RequestHandleBound)
			{
				object internalLock = this.m_InternalLock;
				lock (internalLock)
				{
					if (!this.m_RequestHandleBound)
					{
						ThreadPool.BindHandle(this.DangerousGetHandle());
						this.m_RequestHandleBound = true;
					}
				}
			}
		}

		// Token: 0x060008B7 RID: 2231 RVA: 0x00030268 File Offset: 0x0002E468
		private unsafe void SetupV2Config()
		{
			ulong num = 0UL;
			if (this.m_V2Initialized)
			{
				return;
			}
			try
			{
				uint num2 = UnsafeNclNativeMethods.HttpApi.HttpCreateServerSession(UnsafeNclNativeMethods.HttpApi.Version, &num, 0U);
				if (num2 != 0U)
				{
					throw new HttpListenerException((int)num2);
				}
				this.m_ServerSessionHandle = new HttpServerSessionHandle(num);
				num = 0UL;
				num2 = UnsafeNclNativeMethods.HttpApi.HttpCreateUrlGroup(this.m_ServerSessionHandle.DangerousGetServerSessionId(), &num, 0U);
				if (num2 != 0U)
				{
					throw new HttpListenerException((int)num2);
				}
				this.m_UrlGroupId = num;
				this.m_V2Initialized = true;
			}
			catch (Exception ex)
			{
				this.m_State = HttpListener.State.Closed;
				if (this.m_ServerSessionHandle != null)
				{
					this.m_ServerSessionHandle.Close();
				}
				if (Logging.On)
				{
					Logging.Exception(Logging.HttpListener, this, "SetupV2Config", ex);
				}
				throw;
			}
		}

		// Token: 0x060008B8 RID: 2232 RVA: 0x00030324 File Offset: 0x0002E524
		public void Start()
		{
			if (Logging.On)
			{
				Logging.Enter(Logging.HttpListener, this, "Start", "");
			}
			object internalLock = this.m_InternalLock;
			lock (internalLock)
			{
				try
				{
					this.CheckDisposed();
					if (this.m_State != HttpListener.State.Started)
					{
						this.SetupV2Config();
						this.CreateRequestQueueHandle();
						this.AttachRequestQueueToUrlGroup();
						try
						{
							this.AddAllPrefixes();
						}
						catch (HttpListenerException)
						{
							this.DetachRequestQueueFromUrlGroup();
							this.ClearDigestCache();
							throw;
						}
						this.m_State = HttpListener.State.Started;
					}
				}
				catch (Exception ex)
				{
					this.m_State = HttpListener.State.Closed;
					this.CloseRequestQueueHandle();
					this.CleanupV2Config();
					if (Logging.On)
					{
						Logging.Exception(Logging.HttpListener, this, "Start", ex);
					}
					throw;
				}
				finally
				{
					if (Logging.On)
					{
						Logging.Exit(Logging.HttpListener, this, "Start", "");
					}
				}
			}
		}

		// Token: 0x060008B9 RID: 2233 RVA: 0x00030434 File Offset: 0x0002E634
		private void CleanupV2Config()
		{
			if (!this.m_V2Initialized)
			{
				return;
			}
			uint num = UnsafeNclNativeMethods.HttpApi.HttpCloseUrlGroup(this.m_UrlGroupId);
			if (num != 0U && Logging.On)
			{
				Logging.PrintError(Logging.HttpListener, this, "CloseV2Config", SR.GetString("net_listener_close_urlgroup_error", new object[] { num }));
			}
			this.m_UrlGroupId = 0UL;
			this.m_ServerSessionHandle.Close();
		}

		// Token: 0x060008BA RID: 2234 RVA: 0x0003049C File Offset: 0x0002E69C
		private unsafe void AttachRequestQueueToUrlGroup()
		{
			UnsafeNclNativeMethods.HttpApi.HTTP_BINDING_INFO http_BINDING_INFO = default(UnsafeNclNativeMethods.HttpApi.HTTP_BINDING_INFO);
			http_BINDING_INFO.Flags = UnsafeNclNativeMethods.HttpApi.HTTP_FLAGS.HTTP_RECEIVE_REQUEST_FLAG_COPY_BODY;
			http_BINDING_INFO.RequestQueueHandle = this.DangerousGetHandle();
			IntPtr intPtr = new IntPtr((void*)(&http_BINDING_INFO));
			this.SetUrlGroupProperty(UnsafeNclNativeMethods.HttpApi.HTTP_SERVER_PROPERTY.HttpServerBindingProperty, intPtr, (uint)Marshal.SizeOf(typeof(UnsafeNclNativeMethods.HttpApi.HTTP_BINDING_INFO)));
		}

		// Token: 0x060008BB RID: 2235 RVA: 0x000304E8 File Offset: 0x0002E6E8
		private unsafe void DetachRequestQueueFromUrlGroup()
		{
			UnsafeNclNativeMethods.HttpApi.HTTP_BINDING_INFO http_BINDING_INFO = default(UnsafeNclNativeMethods.HttpApi.HTTP_BINDING_INFO);
			http_BINDING_INFO.Flags = UnsafeNclNativeMethods.HttpApi.HTTP_FLAGS.NONE;
			http_BINDING_INFO.RequestQueueHandle = IntPtr.Zero;
			IntPtr intPtr = new IntPtr((void*)(&http_BINDING_INFO));
			uint num = UnsafeNclNativeMethods.HttpApi.HttpSetUrlGroupProperty(this.m_UrlGroupId, UnsafeNclNativeMethods.HttpApi.HTTP_SERVER_PROPERTY.HttpServerBindingProperty, intPtr, (uint)Marshal.SizeOf(typeof(UnsafeNclNativeMethods.HttpApi.HTTP_BINDING_INFO)));
			if (num != 0U && Logging.On)
			{
				Logging.PrintError(Logging.HttpListener, this, "DetachRequestQueueFromUrlGroup", SR.GetString("net_listener_detach_error", new object[] { num }));
			}
		}

		// Token: 0x060008BC RID: 2236 RVA: 0x0003056C File Offset: 0x0002E76C
		public void Stop()
		{
			if (Logging.On)
			{
				Logging.Enter(Logging.HttpListener, this, "Stop", "");
			}
			try
			{
				object internalLock = this.m_InternalLock;
				lock (internalLock)
				{
					this.CheckDisposed();
					if (this.m_State == HttpListener.State.Stopped)
					{
						return;
					}
					this.RemoveAll(false);
					this.DetachRequestQueueFromUrlGroup();
					this.CloseRequestQueueHandle();
					this.m_State = HttpListener.State.Stopped;
				}
				this.ClearDigestCache();
			}
			catch (Exception ex)
			{
				if (Logging.On)
				{
					Logging.Exception(Logging.HttpListener, this, "Stop", ex);
				}
				throw;
			}
			finally
			{
				if (Logging.On)
				{
					Logging.Exit(Logging.HttpListener, this, "Stop", "");
				}
			}
		}

		// Token: 0x060008BD RID: 2237 RVA: 0x0003064C File Offset: 0x0002E84C
		private void CreateRequestQueueHandle()
		{
			HttpRequestQueueV2Handle httpRequestQueueV2Handle = null;
			uint num = UnsafeNclNativeMethods.SafeNetHandles.HttpCreateRequestQueue(UnsafeNclNativeMethods.HttpApi.Version, null, null, 0U, out httpRequestQueueV2Handle);
			if (num != 0U)
			{
				throw new HttpListenerException((int)num);
			}
			if (HttpListener.SkipIOCPCallbackOnSuccess && !UnsafeNclNativeMethods.SetFileCompletionNotificationModes(httpRequestQueueV2Handle, UnsafeNclNativeMethods.FileCompletionNotificationModes.SkipCompletionPortOnSuccess | UnsafeNclNativeMethods.FileCompletionNotificationModes.SkipSetEventOnHandle))
			{
				throw new HttpListenerException(Marshal.GetLastWin32Error());
			}
			this.m_RequestQueueHandle = httpRequestQueueV2Handle;
		}

		// Token: 0x060008BE RID: 2238 RVA: 0x00030699 File Offset: 0x0002E899
		private void CloseRequestQueueHandle()
		{
			if (this.m_RequestQueueHandle != null && !this.m_RequestQueueHandle.IsInvalid)
			{
				this.m_RequestQueueHandle.Close();
				this.m_RequestHandleBound = false;
			}
		}

		// Token: 0x060008BF RID: 2239 RVA: 0x000306C4 File Offset: 0x0002E8C4
		public void Abort()
		{
			if (Logging.On)
			{
				Logging.Enter(Logging.HttpListener, this, "Abort", "");
			}
			object internalLock = this.m_InternalLock;
			lock (internalLock)
			{
				try
				{
					if (this.m_State != HttpListener.State.Closed)
					{
						if (this.m_State == HttpListener.State.Started)
						{
							this.DetachRequestQueueFromUrlGroup();
							this.CloseRequestQueueHandle();
						}
						this.CleanupV2Config();
						this.ClearDigestCache();
					}
				}
				catch (Exception ex)
				{
					if (Logging.On)
					{
						Logging.Exception(Logging.HttpListener, this, "Abort", ex);
					}
					throw;
				}
				finally
				{
					this.m_State = HttpListener.State.Closed;
					if (Logging.On)
					{
						Logging.Exit(Logging.HttpListener, this, "Abort", "");
					}
				}
			}
		}

		// Token: 0x060008C0 RID: 2240 RVA: 0x000307A4 File Offset: 0x0002E9A4
		public void Close()
		{
			if (Logging.On)
			{
				Logging.Enter(Logging.HttpListener, this, "Close", "");
			}
			try
			{
				((IDisposable)this).Dispose();
			}
			catch (Exception ex)
			{
				if (Logging.On)
				{
					Logging.Exception(Logging.HttpListener, this, "Close", ex);
				}
				throw;
			}
			finally
			{
				if (Logging.On)
				{
					Logging.Exit(Logging.HttpListener, this, "Close", "");
				}
			}
		}

		// Token: 0x060008C1 RID: 2241 RVA: 0x0003082C File Offset: 0x0002EA2C
		private void Dispose(bool disposing)
		{
			if (!disposing)
			{
				return;
			}
			if (Logging.On)
			{
				Logging.Enter(Logging.HttpListener, this, "Dispose", "");
			}
			object internalLock = this.m_InternalLock;
			lock (internalLock)
			{
				try
				{
					if (this.m_State != HttpListener.State.Closed)
					{
						this.Stop();
						this.CleanupV2Config();
					}
				}
				catch (Exception ex)
				{
					if (Logging.On)
					{
						Logging.Exception(Logging.HttpListener, this, "Dispose", ex);
					}
					throw;
				}
				finally
				{
					this.m_State = HttpListener.State.Closed;
					if (Logging.On)
					{
						Logging.Exit(Logging.HttpListener, this, "Dispose", "");
					}
				}
			}
		}

		// Token: 0x060008C2 RID: 2242 RVA: 0x000308F8 File Offset: 0x0002EAF8
		void IDisposable.Dispose()
		{
			this.Dispose(true);
		}

		// Token: 0x060008C3 RID: 2243 RVA: 0x00030904 File Offset: 0x0002EB04
		private uint InternalAddPrefix(string uriPrefix)
		{
			return UnsafeNclNativeMethods.HttpApi.HttpAddUrlToUrlGroup(this.m_UrlGroupId, uriPrefix, 0UL, 0U);
		}

		// Token: 0x060008C4 RID: 2244 RVA: 0x00030924 File Offset: 0x0002EB24
		private bool InternalRemovePrefix(string uriPrefix)
		{
			uint num = UnsafeNclNativeMethods.HttpApi.HttpRemoveUrlFromUrlGroup(this.m_UrlGroupId, uriPrefix, 0U);
			return num != 1168U;
		}

		// Token: 0x060008C5 RID: 2245 RVA: 0x0003094C File Offset: 0x0002EB4C
		private void AddAllPrefixes()
		{
			if (this.m_UriPrefixes.Count > 0)
			{
				foreach (object obj in this.m_UriPrefixes.Values)
				{
					string text = (string)obj;
					uint num = this.InternalAddPrefix(text);
					if (num != 0U)
					{
						if (num == 183U)
						{
							throw new HttpListenerException((int)num, SR.GetString("net_listener_already", new object[] { text }));
						}
						throw new HttpListenerException((int)num);
					}
				}
			}
		}

		// Token: 0x060008C6 RID: 2246 RVA: 0x000309E8 File Offset: 0x0002EBE8
		public unsafe HttpListenerContext GetContext()
		{
			if (Logging.On)
			{
				Logging.Enter(Logging.HttpListener, this, "GetContext", "");
			}
			SyncRequestContext syncRequestContext = null;
			HttpListenerContext httpListenerContext = null;
			bool flag = false;
			checked
			{
				HttpListenerContext httpListenerContext2;
				try
				{
					this.CheckDisposed();
					if (this.m_State == HttpListener.State.Stopped)
					{
						throw new InvalidOperationException(SR.GetString("net_listener_mustcall", new object[] { "Start()" }));
					}
					if (this.m_UriPrefixes.Count == 0)
					{
						throw new InvalidOperationException(SR.GetString("net_listener_mustcall", new object[] { "AddPrefix()" }));
					}
					uint num = 4096U;
					ulong num2 = 0UL;
					syncRequestContext = new SyncRequestContext((int)num);
					uint num4;
					for (;;)
					{
						uint num3 = 0U;
						num4 = UnsafeNclNativeMethods.HttpApi.HttpReceiveHttpRequest(this.m_RequestQueueHandle, num2, 1U, syncRequestContext.RequestBlob, num, &num3, null);
						if (num4 == 87U && num2 != 0UL)
						{
							num2 = 0UL;
						}
						else if (num4 == 234U)
						{
							num = num3;
							num2 = syncRequestContext.RequestBlob->RequestId;
							syncRequestContext.Reset((int)num);
						}
						else
						{
							if (num4 != 0U)
							{
								break;
							}
							if (this.ValidateRequest(syncRequestContext))
							{
								httpListenerContext = this.HandleAuthentication(syncRequestContext, out flag);
							}
							if (flag)
							{
								syncRequestContext = null;
								flag = false;
							}
							if (httpListenerContext != null)
							{
								goto Block_13;
							}
							if (syncRequestContext == null)
							{
								syncRequestContext = new SyncRequestContext((int)num);
							}
							num2 = 0UL;
						}
					}
					throw new HttpListenerException((int)num4);
					Block_13:
					httpListenerContext2 = httpListenerContext;
				}
				catch (Exception ex)
				{
					if (Logging.On)
					{
						Logging.Exception(Logging.HttpListener, this, "GetContext", ex);
					}
					throw;
				}
				finally
				{
					if (syncRequestContext != null && !flag)
					{
						syncRequestContext.ReleasePins();
						syncRequestContext.Close();
					}
					if (Logging.On)
					{
						Logging.Exit(Logging.HttpListener, this, "GetContext", "HttpListenerContext#" + ValidationHelper.HashString(httpListenerContext) + " RequestTraceIdentifier#" + ((httpListenerContext != null) ? httpListenerContext.Request.RequestTraceIdentifier.ToString() : "<null>"));
					}
				}
				return httpListenerContext2;
			}
		}

		// Token: 0x060008C7 RID: 2247 RVA: 0x00030BD4 File Offset: 0x0002EDD4
		internal unsafe bool ValidateRequest(RequestContextBase requestMemory)
		{
			if (requestMemory.RequestBlob->Headers.UnknownHeaderCount > 1000)
			{
				this.SendError(requestMemory.RequestBlob->RequestId, HttpStatusCode.BadRequest, null);
				return false;
			}
			return true;
		}

		// Token: 0x060008C8 RID: 2248 RVA: 0x00030C08 File Offset: 0x0002EE08
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public IAsyncResult BeginGetContext(AsyncCallback callback, object state)
		{
			if (Logging.On)
			{
				Logging.Enter(Logging.HttpListener, this, "BeginGetContext", "");
			}
			ListenerAsyncResult listenerAsyncResult = null;
			try
			{
				this.CheckDisposed();
				if (this.m_State == HttpListener.State.Stopped)
				{
					throw new InvalidOperationException(SR.GetString("net_listener_mustcall", new object[] { "Start()" }));
				}
				listenerAsyncResult = new ListenerAsyncResult(this, state, callback);
				uint num = listenerAsyncResult.QueueBeginGetContext();
				if (num != 0U && num != 997U)
				{
					throw new HttpListenerException((int)num);
				}
			}
			catch (Exception ex)
			{
				if (Logging.On)
				{
					Logging.Exception(Logging.HttpListener, this, "BeginGetContext", ex);
				}
				throw;
			}
			finally
			{
				if (Logging.On)
				{
					Logging.Enter(Logging.HttpListener, this, "BeginGetContext", "IAsyncResult#" + ValidationHelper.HashString(listenerAsyncResult));
				}
			}
			return listenerAsyncResult;
		}

		// Token: 0x060008C9 RID: 2249 RVA: 0x00030CE8 File Offset: 0x0002EEE8
		public HttpListenerContext EndGetContext(IAsyncResult asyncResult)
		{
			if (Logging.On)
			{
				Logging.Enter(Logging.HttpListener, this, "EndGetContext", "IAsyncResult#" + ValidationHelper.HashString(asyncResult));
			}
			HttpListenerContext httpListenerContext = null;
			try
			{
				this.CheckDisposed();
				if (asyncResult == null)
				{
					throw new ArgumentNullException("asyncResult");
				}
				ListenerAsyncResult listenerAsyncResult = asyncResult as ListenerAsyncResult;
				if (listenerAsyncResult == null || listenerAsyncResult.AsyncObject != this)
				{
					throw new ArgumentException(SR.GetString("net_io_invalidasyncresult"), "asyncResult");
				}
				if (listenerAsyncResult.EndCalled)
				{
					throw new InvalidOperationException(SR.GetString("net_io_invalidendcall", new object[] { "EndGetContext" }));
				}
				listenerAsyncResult.EndCalled = true;
				httpListenerContext = listenerAsyncResult.InternalWaitForCompletion() as HttpListenerContext;
				if (httpListenerContext == null)
				{
					throw listenerAsyncResult.Result as Exception;
				}
			}
			catch (Exception ex)
			{
				if (Logging.On)
				{
					Logging.Exception(Logging.HttpListener, this, "EndGetContext", ex);
				}
				throw;
			}
			finally
			{
				if (Logging.On)
				{
					Logging.Exit(Logging.HttpListener, this, "EndGetContext", (httpListenerContext == null) ? "<no context>" : ("HttpListenerContext#" + ValidationHelper.HashString(httpListenerContext) + " RequestTraceIdentifier#" + httpListenerContext.Request.RequestTraceIdentifier.ToString()));
				}
			}
			return httpListenerContext;
		}

		// Token: 0x060008CA RID: 2250 RVA: 0x00030E2C File Offset: 0x0002F02C
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public Task<HttpListenerContext> GetContextAsync()
		{
			return Task<HttpListenerContext>.Factory.FromAsync(new Func<AsyncCallback, object, IAsyncResult>(this.BeginGetContext), new Func<IAsyncResult, HttpListenerContext>(this.EndGetContext), null);
		}

		// Token: 0x060008CB RID: 2251 RVA: 0x00030E51 File Offset: 0x0002F051
		[SecurityPermission(SecurityAction.Assert, Flags = SecurityPermissionFlag.UnmanagedCode)]
		[SecurityPermission(SecurityAction.Assert, Flags = SecurityPermissionFlag.ControlPrincipal)]
		internal static WindowsIdentity CreateWindowsIdentity(IntPtr userToken, string type, WindowsAccountType acctType, bool isAuthenticated)
		{
			return new WindowsIdentity(userToken, type, acctType, isAuthenticated);
		}

		// Token: 0x060008CC RID: 2252 RVA: 0x00030E5C File Offset: 0x0002F05C
		internal unsafe HttpListenerContext HandleAuthentication(RequestContextBase memoryBlob, out bool stoleBlob)
		{
			string text = null;
			stoleBlob = false;
			string verb = UnsafeNclNativeMethods.HttpApi.GetVerb(memoryBlob.RequestBlob);
			string knownHeader = UnsafeNclNativeMethods.HttpApi.GetKnownHeader(memoryBlob.RequestBlob, 24);
			ulong connectionId = memoryBlob.RequestBlob->ConnectionId;
			ulong requestId = memoryBlob.RequestBlob->RequestId;
			bool flag = memoryBlob.RequestBlob->pSslInfo != null;
			HttpListener.DisconnectAsyncResult disconnectAsyncResult = (HttpListener.DisconnectAsyncResult)this.DisconnectResults[connectionId];
			if (this.UnsafeConnectionNtlmAuthentication)
			{
				if (knownHeader == null)
				{
					WindowsPrincipal windowsPrincipal = ((disconnectAsyncResult == null) ? null : disconnectAsyncResult.AuthenticatedConnection);
					if (windowsPrincipal != null)
					{
						stoleBlob = true;
						HttpListenerContext httpListenerContext = new HttpListenerContext(this, memoryBlob);
						httpListenerContext.SetIdentity(windowsPrincipal, null);
						httpListenerContext.Request.ReleasePins();
						return httpListenerContext;
					}
				}
				else if (disconnectAsyncResult != null)
				{
					disconnectAsyncResult.AuthenticatedConnection = null;
				}
			}
			stoleBlob = true;
			HttpListenerContext httpListenerContext2 = null;
			NTAuthentication ntauthentication = null;
			NTAuthentication ntauthentication2 = null;
			NTAuthentication ntauthentication3 = null;
			AuthenticationSchemes authenticationSchemes = AuthenticationSchemes.None;
			AuthenticationSchemes authenticationSchemes2 = this.AuthenticationSchemes;
			ExtendedProtectionPolicy extendedProtectionPolicy = this.m_ExtendedProtectionPolicy;
			HttpListenerContext httpListenerContext3;
			try
			{
				if (disconnectAsyncResult != null && !disconnectAsyncResult.StartOwningDisconnectHandling())
				{
					disconnectAsyncResult = null;
				}
				if (disconnectAsyncResult != null)
				{
					ntauthentication = disconnectAsyncResult.Session;
				}
				httpListenerContext2 = new HttpListenerContext(this, memoryBlob);
				HttpListener.AuthenticationSelectorInfo authenticationDelegate = this.m_AuthenticationDelegate;
				if (authenticationDelegate != null)
				{
					try
					{
						httpListenerContext2.Request.ReleasePins();
						authenticationSchemes2 = authenticationDelegate.Delegate(httpListenerContext2.Request);
						httpListenerContext2.AuthenticationSchemes = authenticationSchemes2;
						if (!authenticationDelegate.AdvancedAuth && (authenticationSchemes2 & (AuthenticationSchemes.Digest | AuthenticationSchemes.Negotiate | AuthenticationSchemes.Ntlm)) != AuthenticationSchemes.None)
						{
							throw this.m_SecurityException;
						}
						goto IL_01A7;
					}
					catch (Exception ex)
					{
						if (NclUtilities.IsFatal(ex))
						{
							throw;
						}
						if (Logging.On)
						{
							Logging.PrintError(Logging.HttpListener, this, "HandleAuthentication", SR.GetString("net_log_listener_delegate_exception", new object[] { ex }));
						}
						this.SendError(requestId, HttpStatusCode.InternalServerError, null);
						HttpListener.FreeContext(ref httpListenerContext2, memoryBlob);
						return null;
					}
				}
				stoleBlob = false;
				IL_01A7:
				HttpListener.ExtendedProtectionSelector extendedProtectionSelectorDelegate = this.m_ExtendedProtectionSelectorDelegate;
				if (extendedProtectionSelectorDelegate != null)
				{
					extendedProtectionPolicy = extendedProtectionSelectorDelegate(httpListenerContext2.Request);
					if (extendedProtectionPolicy == null)
					{
						extendedProtectionPolicy = new ExtendedProtectionPolicy(PolicyEnforcement.Never);
					}
					httpListenerContext2.ExtendedProtectionPolicy = extendedProtectionPolicy;
				}
				int num = -1;
				if (knownHeader != null && (authenticationSchemes2 & ~AuthenticationSchemes.Anonymous) != AuthenticationSchemes.None)
				{
					num = 0;
					while (num < knownHeader.Length && knownHeader[num] != ' ' && knownHeader[num] != '\t' && knownHeader[num] != '\r' && knownHeader[num] != '\n')
					{
						num++;
					}
					if (num < knownHeader.Length)
					{
						if ((authenticationSchemes2 & AuthenticationSchemes.Negotiate) != AuthenticationSchemes.None && string.Compare(knownHeader, 0, "Negotiate", 0, num, StringComparison.OrdinalIgnoreCase) == 0)
						{
							authenticationSchemes = AuthenticationSchemes.Negotiate;
						}
						else if ((authenticationSchemes2 & AuthenticationSchemes.Ntlm) != AuthenticationSchemes.None && string.Compare(knownHeader, 0, "NTLM", 0, num, StringComparison.OrdinalIgnoreCase) == 0)
						{
							authenticationSchemes = AuthenticationSchemes.Ntlm;
						}
						else if ((authenticationSchemes2 & AuthenticationSchemes.Digest) != AuthenticationSchemes.None && string.Compare(knownHeader, 0, "Digest", 0, num, StringComparison.OrdinalIgnoreCase) == 0)
						{
							authenticationSchemes = AuthenticationSchemes.Digest;
						}
						else if ((authenticationSchemes2 & AuthenticationSchemes.Basic) != AuthenticationSchemes.None && string.Compare(knownHeader, 0, "Basic", 0, num, StringComparison.OrdinalIgnoreCase) == 0)
						{
							authenticationSchemes = AuthenticationSchemes.Basic;
						}
						else if (Logging.On)
						{
							Logging.PrintWarning(Logging.HttpListener, this, "HandleAuthentication", SR.GetString("net_log_listener_unsupported_authentication_scheme", new object[] { knownHeader, authenticationSchemes2 }));
						}
					}
				}
				HttpStatusCode httpStatusCode = HttpStatusCode.InternalServerError;
				bool flag2 = false;
				if (authenticationSchemes == AuthenticationSchemes.None)
				{
					if (Logging.On)
					{
						Logging.PrintWarning(Logging.HttpListener, this, "HandleAuthentication", SR.GetString("net_log_listener_unmatched_authentication_scheme", new object[]
						{
							ValidationHelper.ToString(authenticationSchemes2),
							(knownHeader == null) ? "<null>" : knownHeader
						}));
					}
					if ((authenticationSchemes2 & AuthenticationSchemes.Anonymous) != AuthenticationSchemes.None)
					{
						if (!stoleBlob)
						{
							stoleBlob = true;
							httpListenerContext2.Request.ReleasePins();
						}
						return httpListenerContext2;
					}
					httpStatusCode = HttpStatusCode.Unauthorized;
					HttpListener.FreeContext(ref httpListenerContext2, memoryBlob);
				}
				else
				{
					byte[] array = null;
					byte[] array2 = null;
					string text2 = null;
					num++;
					while (num < knownHeader.Length && (knownHeader[num] == ' ' || knownHeader[num] == '\t' || knownHeader[num] == '\r' || knownHeader[num] == '\n'))
					{
						num++;
					}
					string text3 = ((num < knownHeader.Length) ? knownHeader.Substring(num) : "");
					IPrincipal principal = null;
					switch (authenticationSchemes)
					{
					case AuthenticationSchemes.Digest:
					{
						ChannelBinding channelBinding = this.GetChannelBinding(connectionId, flag, extendedProtectionPolicy);
						ntauthentication3 = new NTAuthentication(true, "WDigest", null, this.GetContextFlags(extendedProtectionPolicy, flag), channelBinding);
						SecurityStatus securityStatus;
						text2 = ntauthentication3.GetOutgoingDigestBlob(text3, verb, null, this.Realm, false, false, out securityStatus);
						if (securityStatus == SecurityStatus.OK)
						{
							text2 = null;
						}
						if (ntauthentication3.IsValidContext)
						{
							SafeCloseHandle safeCloseHandle = null;
							try
							{
								if (!this.CheckSpn(ntauthentication3, flag, extendedProtectionPolicy))
								{
									httpStatusCode = HttpStatusCode.Unauthorized;
								}
								else
								{
									httpListenerContext2.Request.ServiceName = ntauthentication3.ClientSpecifiedSpn;
									safeCloseHandle = ntauthentication3.GetContextToken(out securityStatus);
									if (securityStatus != SecurityStatus.OK)
									{
										httpStatusCode = this.HttpStatusFromSecurityStatus(securityStatus);
									}
									else if (safeCloseHandle == null)
									{
										httpStatusCode = HttpStatusCode.Unauthorized;
									}
									else
									{
										principal = new WindowsPrincipal(HttpListener.CreateWindowsIdentity(safeCloseHandle.DangerousGetHandle(), "Digest", WindowsAccountType.Normal, true));
									}
								}
							}
							finally
							{
								if (safeCloseHandle != null)
								{
									safeCloseHandle.Close();
								}
							}
							ntauthentication2 = ntauthentication3;
							if (text2 != null)
							{
								text = "Digest " + text2;
							}
						}
						else
						{
							httpStatusCode = this.HttpStatusFromSecurityStatus(securityStatus);
						}
						break;
					}
					case AuthenticationSchemes.Negotiate:
					case AuthenticationSchemes.Ntlm:
					{
						string text4 = ((authenticationSchemes == AuthenticationSchemes.Ntlm) ? "NTLM" : "Negotiate");
						if (ntauthentication != null && ntauthentication.Package == text4)
						{
							ntauthentication3 = ntauthentication;
						}
						else
						{
							ChannelBinding channelBinding = this.GetChannelBinding(connectionId, flag, extendedProtectionPolicy);
							ntauthentication3 = new NTAuthentication(true, text4, null, this.GetContextFlags(extendedProtectionPolicy, flag), channelBinding);
						}
						try
						{
							array = Convert.FromBase64String(text3);
						}
						catch (FormatException)
						{
							httpStatusCode = HttpStatusCode.BadRequest;
							flag2 = true;
						}
						if (!flag2)
						{
							SecurityStatus securityStatus;
							array2 = ntauthentication3.GetOutgoingBlob(array, false, out securityStatus);
							flag2 = !ntauthentication3.IsValidContext;
							if (flag2)
							{
								if (securityStatus == SecurityStatus.InvalidHandle && ntauthentication == null && array != null && array.Length != 0)
								{
									securityStatus = SecurityStatus.InvalidToken;
								}
								httpStatusCode = this.HttpStatusFromSecurityStatus(securityStatus);
							}
						}
						if (array2 != null)
						{
							text2 = Convert.ToBase64String(array2);
						}
						if (!flag2)
						{
							if (ntauthentication3.IsCompleted)
							{
								SafeCloseHandle safeCloseHandle2 = null;
								try
								{
									if (!this.CheckSpn(ntauthentication3, flag, extendedProtectionPolicy))
									{
										httpStatusCode = HttpStatusCode.Unauthorized;
									}
									else
									{
										httpListenerContext2.Request.ServiceName = ntauthentication3.ClientSpecifiedSpn;
										SecurityStatus securityStatus;
										safeCloseHandle2 = ntauthentication3.GetContextToken(out securityStatus);
										if (securityStatus != SecurityStatus.OK)
										{
											httpStatusCode = this.HttpStatusFromSecurityStatus(securityStatus);
										}
										else
										{
											WindowsPrincipal windowsPrincipal2 = new WindowsPrincipal(HttpListener.CreateWindowsIdentity(safeCloseHandle2.DangerousGetHandle(), ntauthentication3.ProtocolName, WindowsAccountType.Normal, true));
											principal = windowsPrincipal2;
											if (this.UnsafeConnectionNtlmAuthentication && ntauthentication3.ProtocolName == "NTLM")
											{
												if (disconnectAsyncResult == null)
												{
													this.RegisterForDisconnectNotification(connectionId, ref disconnectAsyncResult);
												}
												if (disconnectAsyncResult != null)
												{
													object syncRoot = this.DisconnectResults.SyncRoot;
													lock (syncRoot)
													{
														if (this.UnsafeConnectionNtlmAuthentication)
														{
															disconnectAsyncResult.AuthenticatedConnection = windowsPrincipal2;
														}
													}
												}
											}
										}
									}
									break;
								}
								finally
								{
									if (safeCloseHandle2 != null)
									{
										safeCloseHandle2.Close();
									}
								}
							}
							ntauthentication2 = ntauthentication3;
							text = ((authenticationSchemes == AuthenticationSchemes.Ntlm) ? "NTLM" : "Negotiate");
							if (!string.IsNullOrEmpty(text2))
							{
								text = text + " " + text2;
							}
						}
						break;
					}
					case AuthenticationSchemes.Digest | AuthenticationSchemes.Negotiate:
						break;
					default:
						if (authenticationSchemes == AuthenticationSchemes.Basic)
						{
							try
							{
								array = Convert.FromBase64String(text3);
								text3 = WebHeaderCollection.HeaderEncoding.GetString(array, 0, array.Length);
								num = text3.IndexOf(':');
								if (num != -1)
								{
									string text5 = text3.Substring(0, num);
									string text6 = text3.Substring(num + 1);
									principal = new GenericPrincipal(new HttpListenerBasicIdentity(text5, text6), null);
								}
								else
								{
									httpStatusCode = HttpStatusCode.BadRequest;
								}
							}
							catch (FormatException)
							{
							}
						}
						break;
					}
					if (principal != null)
					{
						httpListenerContext2.SetIdentity(principal, text2);
					}
					else
					{
						if (Logging.On)
						{
							Logging.PrintWarning(Logging.HttpListener, this, "HandleAuthentication", SR.GetString("net_log_listener_create_valid_identity_failed"));
						}
						HttpListener.FreeContext(ref httpListenerContext2, memoryBlob);
					}
				}
				ArrayList arrayList = null;
				if (httpListenerContext2 == null)
				{
					if (text != null)
					{
						HttpListener.AddChallenge(ref arrayList, text);
					}
					else
					{
						if (ntauthentication2 != null)
						{
							if (ntauthentication2 == ntauthentication3)
							{
								ntauthentication3 = null;
							}
							if (ntauthentication2 != ntauthentication)
							{
								NTAuthentication ntauthentication4 = ntauthentication2;
								ntauthentication2 = null;
								ntauthentication4.CloseContext();
							}
							else
							{
								ntauthentication2 = null;
							}
						}
						if (httpStatusCode != HttpStatusCode.Unauthorized)
						{
							this.SendError(requestId, httpStatusCode, null);
							return null;
						}
						arrayList = this.BuildChallenge(authenticationSchemes2, connectionId, out ntauthentication2, extendedProtectionPolicy, flag);
					}
				}
				if (disconnectAsyncResult == null && ntauthentication2 != null)
				{
					this.RegisterForDisconnectNotification(connectionId, ref disconnectAsyncResult);
					if (disconnectAsyncResult == null)
					{
						if (ntauthentication2 != null)
						{
							if (ntauthentication2 == ntauthentication3)
							{
								ntauthentication3 = null;
							}
							if (ntauthentication2 != ntauthentication)
							{
								NTAuthentication ntauthentication5 = ntauthentication2;
								ntauthentication2 = null;
								ntauthentication5.CloseContext();
							}
							else
							{
								ntauthentication2 = null;
							}
						}
						this.SendError(requestId, HttpStatusCode.InternalServerError, null);
						HttpListener.FreeContext(ref httpListenerContext2, memoryBlob);
						return null;
					}
				}
				if (ntauthentication != ntauthentication2)
				{
					if (ntauthentication == ntauthentication3)
					{
						ntauthentication3 = null;
					}
					NTAuthentication ntauthentication6 = ntauthentication;
					ntauthentication = ntauthentication2;
					disconnectAsyncResult.Session = ntauthentication2;
					if (ntauthentication6 != null)
					{
						if ((authenticationSchemes2 & AuthenticationSchemes.Digest) != AuthenticationSchemes.None)
						{
							this.SaveDigestContext(ntauthentication6);
						}
						else
						{
							ntauthentication6.CloseContext();
						}
					}
				}
				if (httpListenerContext2 == null)
				{
					this.SendError(requestId, (arrayList != null && arrayList.Count > 0) ? HttpStatusCode.Unauthorized : HttpStatusCode.Forbidden, arrayList);
					httpListenerContext3 = null;
				}
				else
				{
					if (!stoleBlob)
					{
						stoleBlob = true;
						httpListenerContext2.Request.ReleasePins();
					}
					httpListenerContext3 = httpListenerContext2;
				}
			}
			catch
			{
				HttpListener.FreeContext(ref httpListenerContext2, memoryBlob);
				if (ntauthentication2 != null)
				{
					if (ntauthentication2 == ntauthentication3)
					{
						ntauthentication3 = null;
					}
					if (ntauthentication2 != ntauthentication)
					{
						NTAuthentication ntauthentication7 = ntauthentication2;
						ntauthentication2 = null;
						ntauthentication7.CloseContext();
					}
					else
					{
						ntauthentication2 = null;
					}
				}
				throw;
			}
			finally
			{
				try
				{
					if (ntauthentication != null && ntauthentication != ntauthentication2)
					{
						if (ntauthentication2 == null && disconnectAsyncResult != null)
						{
							disconnectAsyncResult.Session = null;
						}
						if ((authenticationSchemes2 & AuthenticationSchemes.Digest) != AuthenticationSchemes.None)
						{
							this.SaveDigestContext(ntauthentication);
						}
						else
						{
							ntauthentication.CloseContext();
						}
					}
					if (ntauthentication3 != null && ntauthentication != ntauthentication3 && ntauthentication2 != ntauthentication3)
					{
						ntauthentication3.CloseContext();
					}
				}
				finally
				{
					if (disconnectAsyncResult != null)
					{
						disconnectAsyncResult.FinishOwningDisconnectHandling();
					}
				}
			}
			return httpListenerContext3;
		}

		// Token: 0x060008CD RID: 2253 RVA: 0x000318A0 File Offset: 0x0002FAA0
		private static void FreeContext(ref HttpListenerContext httpContext, RequestContextBase memoryBlob)
		{
			if (httpContext != null)
			{
				httpContext.Request.DetachBlob(memoryBlob);
				httpContext.Close();
				httpContext = null;
			}
		}

		// Token: 0x060008CE RID: 2254 RVA: 0x000318C0 File Offset: 0x0002FAC0
		internal void SetAuthenticationHeaders(HttpListenerContext context)
		{
			HttpListenerRequest request = context.Request;
			HttpListenerResponse response = context.Response;
			NTAuthentication ntauthentication;
			ArrayList arrayList = this.BuildChallenge(context.AuthenticationSchemes, request.m_ConnectionId, out ntauthentication, context.ExtendedProtectionPolicy, request.IsSecureConnection);
			if (arrayList != null)
			{
				if (ntauthentication != null)
				{
					this.SaveDigestContext(ntauthentication);
				}
				foreach (object obj in arrayList)
				{
					string text = (string)obj;
					response.Headers.AddInternal("WWW-Authenticate", text);
				}
			}
		}

		// Token: 0x060008CF RID: 2255 RVA: 0x00031964 File Offset: 0x0002FB64
		private static bool ScenarioChecksChannelBinding(bool isSecureConnection, ProtectionScenario scenario)
		{
			return isSecureConnection && scenario == ProtectionScenario.TransportSelected;
		}

		// Token: 0x060008D0 RID: 2256 RVA: 0x00031970 File Offset: 0x0002FB70
		private ChannelBinding GetChannelBinding(ulong connectionId, bool isSecureConnection, ExtendedProtectionPolicy policy)
		{
			if (policy.PolicyEnforcement == PolicyEnforcement.Never)
			{
				if (Logging.On)
				{
					Logging.PrintInfo(Logging.HttpListener, this, SR.GetString("net_log_listener_no_cbt_disabled"));
				}
				return null;
			}
			if (!isSecureConnection)
			{
				if (Logging.On)
				{
					Logging.PrintInfo(Logging.HttpListener, this, SR.GetString("net_log_listener_no_cbt_http"));
				}
				return null;
			}
			if (!AuthenticationManager.OSSupportsExtendedProtection)
			{
				if (Logging.On)
				{
					Logging.PrintInfo(Logging.HttpListener, this, SR.GetString("net_log_listener_no_cbt_platform"));
				}
				return null;
			}
			if (policy.ProtectionScenario == ProtectionScenario.TrustedProxy)
			{
				if (Logging.On)
				{
					Logging.PrintInfo(Logging.HttpListener, this, SR.GetString("net_log_listener_no_cbt_trustedproxy"));
				}
				return null;
			}
			ChannelBinding channelBindingFromTls = this.GetChannelBindingFromTls(connectionId);
			if (Logging.On)
			{
				Logging.PrintInfo(Logging.HttpListener, this, SR.GetString("net_log_listener_cbt"));
			}
			return channelBindingFromTls;
		}

		// Token: 0x060008D1 RID: 2257 RVA: 0x00031A38 File Offset: 0x0002FC38
		private bool CheckSpn(NTAuthentication context, bool isSecureConnection, ExtendedProtectionPolicy policy)
		{
			if (context.IsKerberos)
			{
				if (Logging.On)
				{
					Logging.PrintInfo(Logging.HttpListener, this, SR.GetString("net_log_listener_no_spn_kerberos"));
				}
				return true;
			}
			if (policy.PolicyEnforcement == PolicyEnforcement.Never)
			{
				if (Logging.On)
				{
					Logging.PrintInfo(Logging.HttpListener, this, SR.GetString("net_log_listener_no_spn_disabled"));
				}
				return true;
			}
			if (HttpListener.ScenarioChecksChannelBinding(isSecureConnection, policy.ProtectionScenario))
			{
				if (Logging.On)
				{
					Logging.PrintInfo(Logging.HttpListener, this, SR.GetString("net_log_listener_no_spn_cbt"));
				}
				return true;
			}
			if (!AuthenticationManager.OSSupportsExtendedProtection)
			{
				if (Logging.On)
				{
					Logging.PrintInfo(Logging.HttpListener, this, SR.GetString("net_log_listener_no_spn_platform"));
				}
				return true;
			}
			string clientSpecifiedSpn = context.ClientSpecifiedSpn;
			if (string.IsNullOrEmpty(clientSpecifiedSpn))
			{
				if (policy.PolicyEnforcement == PolicyEnforcement.WhenSupported)
				{
					if (Logging.On)
					{
						Logging.PrintInfo(Logging.HttpListener, this, SR.GetString("net_log_listener_no_spn_whensupported"));
					}
					return true;
				}
				if (Logging.On)
				{
					Logging.PrintInfo(Logging.HttpListener, this, SR.GetString("net_log_listener_spn_failed_always"));
				}
				return false;
			}
			else
			{
				if (ServiceNameCollection.Match(clientSpecifiedSpn, "http/localhost"))
				{
					if (Logging.On)
					{
						Logging.PrintInfo(Logging.HttpListener, this, SR.GetString("net_log_listener_no_spn_loopback"));
					}
					return true;
				}
				if (Logging.On)
				{
					Logging.PrintInfo(Logging.HttpListener, this, SR.GetString("net_log_listener_spn", new object[] { clientSpecifiedSpn }));
				}
				ServiceNameCollection serviceNames = this.GetServiceNames(policy);
				bool flag = serviceNames.Contains(clientSpecifiedSpn);
				if (Logging.On)
				{
					if (flag)
					{
						Logging.PrintInfo(Logging.HttpListener, this, SR.GetString("net_log_listener_spn_passed"));
					}
					else
					{
						Logging.PrintInfo(Logging.HttpListener, this, SR.GetString("net_log_listener_spn_failed"));
						if (serviceNames.Count == 0)
						{
							Logging.PrintWarning(Logging.HttpListener, this, "CheckSpn", SR.GetString("net_log_listener_spn_failed_empty"));
						}
						else
						{
							Logging.PrintInfo(Logging.HttpListener, this, SR.GetString("net_log_listener_spn_failed_dump"));
							foreach (object obj in serviceNames)
							{
								string text = (string)obj;
								Logging.PrintInfo(Logging.HttpListener, this, "\t" + text);
							}
						}
					}
				}
				return flag;
			}
		}

		// Token: 0x060008D2 RID: 2258 RVA: 0x00031C68 File Offset: 0x0002FE68
		private ServiceNameCollection GetServiceNames(ExtendedProtectionPolicy policy)
		{
			ServiceNameCollection serviceNameCollection;
			if (policy.CustomServiceNames == null)
			{
				if (this.m_DefaultServiceNames.ServiceNames.Count == 0)
				{
					throw new InvalidOperationException(SR.GetString("net_listener_no_spns"));
				}
				serviceNameCollection = this.m_DefaultServiceNames.ServiceNames;
			}
			else
			{
				serviceNameCollection = policy.CustomServiceNames;
			}
			return serviceNameCollection;
		}

		// Token: 0x060008D3 RID: 2259 RVA: 0x00031CB8 File Offset: 0x0002FEB8
		private ContextFlags GetContextFlags(ExtendedProtectionPolicy policy, bool isSecureConnection)
		{
			ContextFlags contextFlags = ContextFlags.Connection;
			if (policy.PolicyEnforcement != PolicyEnforcement.Never)
			{
				if (policy.PolicyEnforcement == PolicyEnforcement.WhenSupported)
				{
					contextFlags |= ContextFlags.AllowMissingBindings;
				}
				if (policy.ProtectionScenario == ProtectionScenario.TrustedProxy)
				{
					contextFlags |= ContextFlags.ProxyBindings;
				}
			}
			return contextFlags;
		}

		// Token: 0x060008D4 RID: 2260 RVA: 0x00031CF6 File Offset: 0x0002FEF6
		private static void AddChallenge(ref ArrayList challenges, string challenge)
		{
			if (challenge != null)
			{
				challenge = challenge.Trim();
				if (challenge.Length > 0)
				{
					if (challenges == null)
					{
						challenges = new ArrayList(4);
					}
					challenges.Add(challenge);
				}
			}
		}

		// Token: 0x060008D5 RID: 2261 RVA: 0x00031D24 File Offset: 0x0002FF24
		private ArrayList BuildChallenge(AuthenticationSchemes authenticationScheme, ulong connectionId, out NTAuthentication newContext, ExtendedProtectionPolicy policy, bool isSecureConnection)
		{
			ArrayList arrayList = null;
			newContext = null;
			if ((authenticationScheme & AuthenticationSchemes.Negotiate) != AuthenticationSchemes.None)
			{
				HttpListener.AddChallenge(ref arrayList, "Negotiate");
			}
			if ((authenticationScheme & AuthenticationSchemes.Ntlm) != AuthenticationSchemes.None)
			{
				HttpListener.AddChallenge(ref arrayList, "NTLM");
			}
			if ((authenticationScheme & AuthenticationSchemes.Digest) != AuthenticationSchemes.None)
			{
				NTAuthentication ntauthentication = null;
				try
				{
					ChannelBinding channelBinding = this.GetChannelBinding(connectionId, isSecureConnection, policy);
					ntauthentication = new NTAuthentication(true, "WDigest", null, this.GetContextFlags(policy, isSecureConnection), channelBinding);
					SecurityStatus securityStatus;
					string outgoingDigestBlob = ntauthentication.GetOutgoingDigestBlob(null, null, null, this.Realm, false, false, out securityStatus);
					if (ntauthentication.IsValidContext)
					{
						newContext = ntauthentication;
					}
					HttpListener.AddChallenge(ref arrayList, "Digest" + (string.IsNullOrEmpty(outgoingDigestBlob) ? "" : (" " + outgoingDigestBlob)));
				}
				finally
				{
					if (ntauthentication != null && newContext != ntauthentication)
					{
						ntauthentication.CloseContext();
					}
				}
			}
			if ((authenticationScheme & AuthenticationSchemes.Basic) != AuthenticationSchemes.None)
			{
				HttpListener.AddChallenge(ref arrayList, "Basic realm=\"" + this.Realm + "\"");
			}
			return arrayList;
		}

		// Token: 0x060008D6 RID: 2262 RVA: 0x00031E18 File Offset: 0x00030018
		private void RegisterForDisconnectNotification(ulong connectionId, ref HttpListener.DisconnectAsyncResult disconnectResult)
		{
			try
			{
				HttpListener.DisconnectAsyncResult disconnectAsyncResult = new HttpListener.DisconnectAsyncResult(this, connectionId);
				this.EnsureBoundHandle();
				uint num = UnsafeNclNativeMethods.HttpApi.HttpWaitForDisconnect(this.m_RequestQueueHandle, connectionId, disconnectAsyncResult.NativeOverlapped);
				if (num == 0U || num == 997U)
				{
					disconnectResult = disconnectAsyncResult;
					this.DisconnectResults[connectionId] = disconnectResult;
				}
				if (num == 0U && HttpListener.SkipIOCPCallbackOnSuccess)
				{
					disconnectAsyncResult.IOCompleted(num, 0U, disconnectAsyncResult.NativeOverlapped);
				}
			}
			catch (Win32Exception ex)
			{
				uint nativeErrorCode = (uint)ex.NativeErrorCode;
			}
		}

		// Token: 0x060008D7 RID: 2263 RVA: 0x00031E9C File Offset: 0x0003009C
		private unsafe void SendError(ulong requestId, HttpStatusCode httpStatusCode, ArrayList challenges)
		{
			UnsafeNclNativeMethods.HttpApi.HTTP_RESPONSE http_RESPONSE = default(UnsafeNclNativeMethods.HttpApi.HTTP_RESPONSE);
			http_RESPONSE.Version = default(UnsafeNclNativeMethods.HttpApi.HTTP_VERSION);
			http_RESPONSE.Version.MajorVersion = 1;
			http_RESPONSE.Version.MinorVersion = 1;
			http_RESPONSE.StatusCode = (ushort)httpStatusCode;
			string text = HttpStatusDescription.Get(httpStatusCode);
			uint num = 0U;
			byte[] bytes = Encoding.Default.GetBytes(text);
			byte[] array;
			byte* ptr;
			if ((array = bytes) == null || array.Length == 0)
			{
				ptr = null;
			}
			else
			{
				ptr = &array[0];
			}
			http_RESPONSE.pReason = (sbyte*)ptr;
			http_RESPONSE.ReasonLength = (ushort)bytes.Length;
			byte[] bytes2 = Encoding.Default.GetBytes("0");
			byte[] array2;
			byte* ptr2;
			if ((array2 = bytes2) == null || array2.Length == 0)
			{
				ptr2 = null;
			}
			else
			{
				ptr2 = &array2[0];
			}
			(&http_RESPONSE.Headers.KnownHeaders)[11].pRawValue = (sbyte*)ptr2;
			(&http_RESPONSE.Headers.KnownHeaders)[11].RawValueLength = (ushort)bytes2.Length;
			http_RESPONSE.Headers.UnknownHeaderCount = checked((ushort)((challenges == null) ? 0 : challenges.Count));
			GCHandle[] array3 = null;
			UnsafeNclNativeMethods.HttpApi.HTTP_UNKNOWN_HEADER[] array4 = null;
			GCHandle gchandle = default(GCHandle);
			GCHandle gchandle2 = default(GCHandle);
			if (http_RESPONSE.Headers.UnknownHeaderCount > 0)
			{
				array3 = new GCHandle[(int)http_RESPONSE.Headers.UnknownHeaderCount];
				array4 = new UnsafeNclNativeMethods.HttpApi.HTTP_UNKNOWN_HEADER[(int)http_RESPONSE.Headers.UnknownHeaderCount];
			}
			uint num2;
			try
			{
				if (http_RESPONSE.Headers.UnknownHeaderCount > 0)
				{
					gchandle = GCHandle.Alloc(array4, GCHandleType.Pinned);
					http_RESPONSE.Headers.pUnknownHeaders = (UnsafeNclNativeMethods.HttpApi.HTTP_UNKNOWN_HEADER*)(void*)Marshal.UnsafeAddrOfPinnedArrayElement(array4, 0);
					gchandle2 = GCHandle.Alloc(HttpListener.s_WwwAuthenticateBytes, GCHandleType.Pinned);
					sbyte* ptr3 = (sbyte*)(void*)Marshal.UnsafeAddrOfPinnedArrayElement(HttpListener.s_WwwAuthenticateBytes, 0);
					for (int i = 0; i < array3.Length; i++)
					{
						byte[] bytes3 = Encoding.Default.GetBytes((string)challenges[i]);
						array3[i] = GCHandle.Alloc(bytes3, GCHandleType.Pinned);
						array4[i].pName = ptr3;
						array4[i].NameLength = (ushort)HttpListener.s_WwwAuthenticateBytes.Length;
						array4[i].pRawValue = (sbyte*)(void*)Marshal.UnsafeAddrOfPinnedArrayElement(bytes3, 0);
						array4[i].RawValueLength = checked((ushort)bytes3.Length);
					}
				}
				num2 = UnsafeNclNativeMethods.HttpApi.HttpSendHttpResponse(this.m_RequestQueueHandle, requestId, 0U, &http_RESPONSE, null, &num, SafeLocalFree.Zero, 0U, null, null);
			}
			finally
			{
				if (gchandle.IsAllocated)
				{
					gchandle.Free();
				}
				if (gchandle2.IsAllocated)
				{
					gchandle2.Free();
				}
				if (array3 != null)
				{
					for (int j = 0; j < array3.Length; j++)
					{
						if (array3[j].IsAllocated)
						{
							array3[j].Free();
						}
					}
				}
			}
			array2 = null;
			array = null;
			if (num2 != 0U)
			{
				HttpListenerContext.CancelRequest(this.m_RequestQueueHandle, requestId);
			}
		}

		// Token: 0x060008D8 RID: 2264 RVA: 0x00032190 File Offset: 0x00030390
		private static int GetTokenOffsetFromBlob(IntPtr blob)
		{
			IntPtr intPtr = Marshal.ReadIntPtr(blob, (int)Marshal.OffsetOf(HttpListener.ChannelBindingStatusType, "ChannelToken"));
			return (int)IntPtrHelper.Subtract(intPtr, blob);
		}

		// Token: 0x060008D9 RID: 2265 RVA: 0x000321C0 File Offset: 0x000303C0
		private static int GetTokenSizeFromBlob(IntPtr blob)
		{
			return Marshal.ReadInt32(blob, (int)Marshal.OffsetOf(HttpListener.ChannelBindingStatusType, "ChannelTokenSize"));
		}

		// Token: 0x060008DA RID: 2266 RVA: 0x000321DC File Offset: 0x000303DC
		internal unsafe ChannelBinding GetChannelBindingFromTls(ulong connectionId)
		{
			if (Logging.On)
			{
				Logging.Enter(Logging.HttpListener, "HttpListener#" + ValidationHelper.HashString(this) + "::GetChannelBindingFromTls() connectionId: " + connectionId.ToString());
			}
			int num = HttpListener.RequestChannelBindStatusSize + 128;
			SafeLocalFreeChannelBinding safeLocalFreeChannelBinding = null;
			uint num2 = 0U;
			uint num3;
			for (;;)
			{
				byte[] array = new byte[num];
				byte[] array2;
				byte* ptr;
				if ((array2 = array) == null || array2.Length == 0)
				{
					ptr = null;
				}
				else
				{
					ptr = &array2[0];
				}
				num3 = UnsafeNclNativeMethods.HttpApi.HttpReceiveClientCertificate(this.RequestQueueHandle, connectionId, 1U, ptr, (uint)num, &num2, null);
				if (num3 == 0U)
				{
					int tokenOffsetFromBlob = HttpListener.GetTokenOffsetFromBlob((IntPtr)((void*)ptr));
					int tokenSizeFromBlob = HttpListener.GetTokenSizeFromBlob((IntPtr)((void*)ptr));
					safeLocalFreeChannelBinding = SafeLocalFreeChannelBinding.LocalAlloc(tokenSizeFromBlob);
					if (safeLocalFreeChannelBinding.IsInvalid)
					{
						break;
					}
					Marshal.Copy(array, tokenOffsetFromBlob, safeLocalFreeChannelBinding.DangerousGetHandle(), tokenSizeFromBlob);
				}
				else
				{
					if (num3 != 234U)
					{
						goto IL_00E4;
					}
					int tokenSizeFromBlob2 = HttpListener.GetTokenSizeFromBlob((IntPtr)((void*)ptr));
					num = HttpListener.RequestChannelBindStatusSize + tokenSizeFromBlob2;
				}
				array2 = null;
				if (num3 == 0U)
				{
					return safeLocalFreeChannelBinding;
				}
			}
			throw new OutOfMemoryException();
			IL_00E4:
			if (num3 == 87U)
			{
				if (Logging.On)
				{
					Logging.PrintError(Logging.HttpListener, "HttpListener#" + ValidationHelper.HashString(this) + "::GetChannelBindingFromTls() " + SR.GetString("net_ssp_dont_support_cbt"));
				}
				return null;
			}
			throw new HttpListenerException((int)num3);
		}

		// Token: 0x060008DB RID: 2267 RVA: 0x00032318 File Offset: 0x00030518
		internal void CheckDisposed()
		{
			if (this.m_State == HttpListener.State.Closed)
			{
				throw new ObjectDisposedException(base.GetType().FullName);
			}
		}

		// Token: 0x060008DC RID: 2268 RVA: 0x00032336 File Offset: 0x00030536
		private HttpStatusCode HttpStatusFromSecurityStatus(SecurityStatus status)
		{
			if (NclUtilities.IsCredentialFailure(status))
			{
				return HttpStatusCode.Unauthorized;
			}
			if (NclUtilities.IsClientFault(status))
			{
				return HttpStatusCode.BadRequest;
			}
			return HttpStatusCode.InternalServerError;
		}

		// Token: 0x060008DD RID: 2269 RVA: 0x0003235C File Offset: 0x0003055C
		private void SaveDigestContext(NTAuthentication digestContext)
		{
			if (this.m_SavedDigests == null)
			{
				Interlocked.CompareExchange<HttpListener.DigestContext[]>(ref this.m_SavedDigests, new HttpListener.DigestContext[1024], null);
			}
			NTAuthentication ntauthentication = null;
			ArrayList arrayList = null;
			HttpListener.DigestContext[] savedDigests = this.m_SavedDigests;
			lock (savedDigests)
			{
				if (!this.IsListening)
				{
					digestContext.CloseContext();
					return;
				}
				int num = (((num = Environment.TickCount) == 0) ? 1 : num);
				this.m_NewestContext = (this.m_NewestContext + 1) & 1023;
				int timestamp = this.m_SavedDigests[this.m_NewestContext].timestamp;
				ntauthentication = this.m_SavedDigests[this.m_NewestContext].context;
				this.m_SavedDigests[this.m_NewestContext].timestamp = num;
				this.m_SavedDigests[this.m_NewestContext].context = digestContext;
				if (this.m_OldestContext == this.m_NewestContext)
				{
					this.m_OldestContext = (this.m_NewestContext + 1) & 1023;
				}
				while (num - this.m_SavedDigests[this.m_OldestContext].timestamp >= 300 && this.m_SavedDigests[this.m_OldestContext].context != null)
				{
					if (arrayList == null)
					{
						arrayList = new ArrayList();
					}
					arrayList.Add(this.m_SavedDigests[this.m_OldestContext].context);
					this.m_SavedDigests[this.m_OldestContext].context = null;
					this.m_OldestContext = (this.m_OldestContext + 1) & 1023;
				}
				if (ntauthentication != null && num - timestamp <= 10000)
				{
					if (this.m_ExtraSavedDigests == null || num - this.m_ExtraSavedDigestsTimestamp > 10000)
					{
						arrayList = this.m_ExtraSavedDigestsBaking;
						this.m_ExtraSavedDigestsBaking = this.m_ExtraSavedDigests;
						this.m_ExtraSavedDigestsTimestamp = num;
						this.m_ExtraSavedDigests = new ArrayList();
					}
					this.m_ExtraSavedDigests.Add(ntauthentication);
					ntauthentication = null;
				}
			}
			if (ntauthentication != null)
			{
				ntauthentication.CloseContext();
			}
			if (arrayList != null)
			{
				for (int i = 0; i < arrayList.Count; i++)
				{
					((NTAuthentication)arrayList[i]).CloseContext();
				}
			}
		}

		// Token: 0x060008DE RID: 2270 RVA: 0x0003259C File Offset: 0x0003079C
		private void ClearDigestCache()
		{
			if (this.m_SavedDigests == null)
			{
				return;
			}
			ArrayList[] array = new ArrayList[3];
			HttpListener.DigestContext[] savedDigests = this.m_SavedDigests;
			lock (savedDigests)
			{
				array[0] = this.m_ExtraSavedDigestsBaking;
				this.m_ExtraSavedDigestsBaking = null;
				array[1] = this.m_ExtraSavedDigests;
				this.m_ExtraSavedDigests = null;
				this.m_NewestContext = 0;
				this.m_OldestContext = 0;
				array[2] = new ArrayList();
				for (int i = 0; i < 1024; i++)
				{
					if (this.m_SavedDigests[i].context != null)
					{
						array[2].Add(this.m_SavedDigests[i].context);
						this.m_SavedDigests[i].context = null;
					}
					this.m_SavedDigests[i].timestamp = 0;
				}
			}
			for (int j = 0; j < array.Length; j++)
			{
				if (array[j] != null)
				{
					for (int k = 0; k < array[j].Count; k++)
					{
						((NTAuthentication)array[j][k]).CloseContext();
					}
				}
			}
		}

		// Token: 0x04000DDF RID: 3551
		private static readonly Type ChannelBindingStatusType = typeof(UnsafeNclNativeMethods.HttpApi.HTTP_REQUEST_CHANNEL_BIND_STATUS);

		// Token: 0x04000DE0 RID: 3552
		private static readonly int RequestChannelBindStatusSize = Marshal.SizeOf(typeof(UnsafeNclNativeMethods.HttpApi.HTTP_REQUEST_CHANNEL_BIND_STATUS));

		// Token: 0x04000DE1 RID: 3553
		internal static readonly bool SkipIOCPCallbackOnSuccess = ComNetOS.IsWin8orLater;

		// Token: 0x04000DE2 RID: 3554
		private const int UnknownHeaderLimit = 1000;

		// Token: 0x04000DE3 RID: 3555
		private static byte[] s_WwwAuthenticateBytes = new byte[]
		{
			87, 87, 87, 45, 65, 117, 116, 104, 101, 110,
			116, 105, 99, 97, 116, 101
		};

		// Token: 0x04000DE4 RID: 3556
		private HttpListener.AuthenticationSelectorInfo m_AuthenticationDelegate;

		// Token: 0x04000DE5 RID: 3557
		private AuthenticationSchemes m_AuthenticationScheme = AuthenticationSchemes.Anonymous;

		// Token: 0x04000DE6 RID: 3558
		private SecurityException m_SecurityException;

		// Token: 0x04000DE7 RID: 3559
		private string m_Realm;

		// Token: 0x04000DE8 RID: 3560
		private CriticalHandle m_RequestQueueHandle;

		// Token: 0x04000DE9 RID: 3561
		private bool m_RequestHandleBound;

		// Token: 0x04000DEA RID: 3562
		private volatile HttpListener.State m_State;

		// Token: 0x04000DEB RID: 3563
		private HttpListenerPrefixCollection m_Prefixes;

		// Token: 0x04000DEC RID: 3564
		private bool m_IgnoreWriteExceptions;

		// Token: 0x04000DED RID: 3565
		private bool m_UnsafeConnectionNtlmAuthentication;

		// Token: 0x04000DEE RID: 3566
		private HttpListener.ExtendedProtectionSelector m_ExtendedProtectionSelectorDelegate;

		// Token: 0x04000DEF RID: 3567
		private ExtendedProtectionPolicy m_ExtendedProtectionPolicy;

		// Token: 0x04000DF0 RID: 3568
		private ServiceNameStore m_DefaultServiceNames;

		// Token: 0x04000DF1 RID: 3569
		private HttpServerSessionHandle m_ServerSessionHandle;

		// Token: 0x04000DF2 RID: 3570
		private ulong m_UrlGroupId;

		// Token: 0x04000DF3 RID: 3571
		private HttpListenerTimeoutManager m_TimeoutManager;

		// Token: 0x04000DF4 RID: 3572
		private bool m_V2Initialized;

		// Token: 0x04000DF5 RID: 3573
		private Hashtable m_DisconnectResults;

		// Token: 0x04000DF6 RID: 3574
		private object m_InternalLock;

		// Token: 0x04000DF7 RID: 3575
		internal Hashtable m_UriPrefixes = new Hashtable();

		// Token: 0x04000DF8 RID: 3576
		private const int DigestLifetimeSeconds = 300;

		// Token: 0x04000DF9 RID: 3577
		private const int MaximumDigests = 1024;

		// Token: 0x04000DFA RID: 3578
		private const int MinimumDigestLifetimeSeconds = 10;

		// Token: 0x04000DFB RID: 3579
		private HttpListener.DigestContext[] m_SavedDigests;

		// Token: 0x04000DFC RID: 3580
		private ArrayList m_ExtraSavedDigests;

		// Token: 0x04000DFD RID: 3581
		private ArrayList m_ExtraSavedDigestsBaking;

		// Token: 0x04000DFE RID: 3582
		private int m_ExtraSavedDigestsTimestamp;

		// Token: 0x04000DFF RID: 3583
		private int m_NewestContext;

		// Token: 0x04000E00 RID: 3584
		private int m_OldestContext;

		// Token: 0x020006FD RID: 1789
		private class AuthenticationSelectorInfo
		{
			// Token: 0x0600407D RID: 16509 RVA: 0x0010E77E File Offset: 0x0010C97E
			internal AuthenticationSelectorInfo(AuthenticationSchemeSelector selectorDelegate, bool canUseAdvancedAuth)
			{
				this.m_SelectorDelegate = selectorDelegate;
				this.m_CanUseAdvancedAuth = canUseAdvancedAuth;
			}

			// Token: 0x17000EE7 RID: 3815
			// (get) Token: 0x0600407E RID: 16510 RVA: 0x0010E794 File Offset: 0x0010C994
			internal AuthenticationSchemeSelector Delegate
			{
				get
				{
					return this.m_SelectorDelegate;
				}
			}

			// Token: 0x17000EE8 RID: 3816
			// (get) Token: 0x0600407F RID: 16511 RVA: 0x0010E79C File Offset: 0x0010C99C
			internal bool AdvancedAuth
			{
				get
				{
					return this.m_CanUseAdvancedAuth;
				}
			}

			// Token: 0x040030B8 RID: 12472
			private AuthenticationSchemeSelector m_SelectorDelegate;

			// Token: 0x040030B9 RID: 12473
			private bool m_CanUseAdvancedAuth;
		}

		// Token: 0x020006FE RID: 1790
		// (Invoke) Token: 0x06004081 RID: 16513
		public delegate ExtendedProtectionPolicy ExtendedProtectionSelector(HttpListenerRequest request);

		// Token: 0x020006FF RID: 1791
		private enum State
		{
			// Token: 0x040030BB RID: 12475
			Stopped,
			// Token: 0x040030BC RID: 12476
			Started,
			// Token: 0x040030BD RID: 12477
			Closed
		}

		// Token: 0x02000700 RID: 1792
		private struct DigestContext
		{
			// Token: 0x040030BE RID: 12478
			internal NTAuthentication context;

			// Token: 0x040030BF RID: 12479
			internal int timestamp;
		}

		// Token: 0x02000701 RID: 1793
		private class DisconnectAsyncResult : IAsyncResult
		{
			// Token: 0x17000EE9 RID: 3817
			// (get) Token: 0x06004084 RID: 16516 RVA: 0x0010E7A4 File Offset: 0x0010C9A4
			internal unsafe NativeOverlapped* NativeOverlapped
			{
				get
				{
					return this.m_NativeOverlapped;
				}
			}

			// Token: 0x17000EEA RID: 3818
			// (get) Token: 0x06004085 RID: 16517 RVA: 0x0010E7AC File Offset: 0x0010C9AC
			public object AsyncState
			{
				get
				{
					throw ExceptionHelper.PropertyNotImplementedException;
				}
			}

			// Token: 0x17000EEB RID: 3819
			// (get) Token: 0x06004086 RID: 16518 RVA: 0x0010E7B3 File Offset: 0x0010C9B3
			public WaitHandle AsyncWaitHandle
			{
				get
				{
					throw ExceptionHelper.PropertyNotImplementedException;
				}
			}

			// Token: 0x17000EEC RID: 3820
			// (get) Token: 0x06004087 RID: 16519 RVA: 0x0010E7BA File Offset: 0x0010C9BA
			public bool CompletedSynchronously
			{
				get
				{
					throw ExceptionHelper.PropertyNotImplementedException;
				}
			}

			// Token: 0x17000EED RID: 3821
			// (get) Token: 0x06004088 RID: 16520 RVA: 0x0010E7C1 File Offset: 0x0010C9C1
			public bool IsCompleted
			{
				get
				{
					throw ExceptionHelper.PropertyNotImplementedException;
				}
			}

			// Token: 0x06004089 RID: 16521 RVA: 0x0010E7C8 File Offset: 0x0010C9C8
			internal DisconnectAsyncResult(HttpListener httpListener, ulong connectionId)
			{
				this.m_OwnershipState = 1;
				this.m_HttpListener = httpListener;
				this.m_ConnectionId = connectionId;
				this.m_NativeOverlapped = new Overlapped
				{
					AsyncResult = this
				}.UnsafePack(HttpListener.DisconnectAsyncResult.s_IOCallback, null);
			}

			// Token: 0x0600408A RID: 16522 RVA: 0x0010E810 File Offset: 0x0010CA10
			internal bool StartOwningDisconnectHandling()
			{
				int num;
				while ((num = Interlocked.CompareExchange(ref this.m_OwnershipState, 1, 0)) == 2)
				{
					Thread.SpinWait(1);
				}
				return num < 2;
			}

			// Token: 0x0600408B RID: 16523 RVA: 0x0010E83B File Offset: 0x0010CA3B
			internal void FinishOwningDisconnectHandling()
			{
				if (Interlocked.CompareExchange(ref this.m_OwnershipState, 0, 1) == 2)
				{
					this.HandleDisconnect();
				}
			}

			// Token: 0x0600408C RID: 16524 RVA: 0x0010E853 File Offset: 0x0010CA53
			internal unsafe void IOCompleted(uint errorCode, uint numBytes, NativeOverlapped* nativeOverlapped)
			{
				HttpListener.DisconnectAsyncResult.IOCompleted(this, errorCode, numBytes, nativeOverlapped);
			}

			// Token: 0x0600408D RID: 16525 RVA: 0x0010E85E File Offset: 0x0010CA5E
			private unsafe static void IOCompleted(HttpListener.DisconnectAsyncResult asyncResult, uint errorCode, uint numBytes, NativeOverlapped* nativeOverlapped)
			{
				Overlapped.Free(nativeOverlapped);
				if (Interlocked.Exchange(ref asyncResult.m_OwnershipState, 2) == 0)
				{
					asyncResult.HandleDisconnect();
				}
			}

			// Token: 0x0600408E RID: 16526 RVA: 0x0010E87C File Offset: 0x0010CA7C
			private unsafe static void WaitCallback(uint errorCode, uint numBytes, NativeOverlapped* nativeOverlapped)
			{
				Overlapped overlapped = Overlapped.Unpack(nativeOverlapped);
				HttpListener.DisconnectAsyncResult disconnectAsyncResult = (HttpListener.DisconnectAsyncResult)overlapped.AsyncResult;
				HttpListener.DisconnectAsyncResult.IOCompleted(disconnectAsyncResult, errorCode, numBytes, nativeOverlapped);
			}

			// Token: 0x0600408F RID: 16527 RVA: 0x0010E8A8 File Offset: 0x0010CAA8
			private void HandleDisconnect()
			{
				this.m_HttpListener.DisconnectResults.Remove(this.m_ConnectionId);
				if (this.m_Session != null)
				{
					if (this.m_Session.Package == "WDigest")
					{
						this.m_HttpListener.SaveDigestContext(this.m_Session);
					}
					else
					{
						this.m_Session.CloseContext();
					}
				}
				IDisposable disposable = ((this.m_AuthenticatedConnection == null) ? null : (this.m_AuthenticatedConnection.Identity as IDisposable));
				if (disposable != null && this.m_AuthenticatedConnection.Identity.AuthenticationType == "NTLM" && this.m_HttpListener.UnsafeConnectionNtlmAuthentication)
				{
					disposable.Dispose();
				}
				int num = Interlocked.Exchange(ref this.m_OwnershipState, 3);
			}

			// Token: 0x17000EEE RID: 3822
			// (get) Token: 0x06004090 RID: 16528 RVA: 0x0010E968 File Offset: 0x0010CB68
			// (set) Token: 0x06004091 RID: 16529 RVA: 0x0010E970 File Offset: 0x0010CB70
			internal WindowsPrincipal AuthenticatedConnection
			{
				get
				{
					return this.m_AuthenticatedConnection;
				}
				set
				{
					this.m_AuthenticatedConnection = value;
				}
			}

			// Token: 0x17000EEF RID: 3823
			// (get) Token: 0x06004092 RID: 16530 RVA: 0x0010E979 File Offset: 0x0010CB79
			// (set) Token: 0x06004093 RID: 16531 RVA: 0x0010E981 File Offset: 0x0010CB81
			internal NTAuthentication Session
			{
				get
				{
					return this.m_Session;
				}
				set
				{
					this.m_Session = value;
				}
			}

			// Token: 0x040030C0 RID: 12480
			private static readonly IOCompletionCallback s_IOCallback = new IOCompletionCallback(HttpListener.DisconnectAsyncResult.WaitCallback);

			// Token: 0x040030C1 RID: 12481
			private ulong m_ConnectionId;

			// Token: 0x040030C2 RID: 12482
			private HttpListener m_HttpListener;

			// Token: 0x040030C3 RID: 12483
			private unsafe NativeOverlapped* m_NativeOverlapped;

			// Token: 0x040030C4 RID: 12484
			private int m_OwnershipState;

			// Token: 0x040030C5 RID: 12485
			private WindowsPrincipal m_AuthenticatedConnection;

			// Token: 0x040030C6 RID: 12486
			private NTAuthentication m_Session;

			// Token: 0x040030C7 RID: 12487
			internal const string NTLM = "NTLM";
		}
	}
}
