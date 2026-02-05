using System;
using System.Collections;
using System.Diagnostics;
using System.Globalization;
using System.Net.Configuration;
using System.Net.Security;
using System.Runtime.CompilerServices;
using System.Security.Authentication;
using System.Threading;

namespace System.Net
{
	// Token: 0x02000160 RID: 352
	public class ServicePointManager
	{
		// Token: 0x170002CC RID: 716
		// (get) Token: 0x06000C63 RID: 3171 RVA: 0x00042A9B File Offset: 0x00040C9B
		// (set) Token: 0x06000C64 RID: 3172 RVA: 0x00042AB9 File Offset: 0x00040CB9
		private static int InternalConnectionLimit
		{
			get
			{
				if (ServicePointManager.s_ConfigTable == null)
				{
					ServicePointManager.s_ConfigTable = ServicePointManager.ConfigTable;
				}
				return ServicePointManager.s_ConnectionLimit;
			}
			set
			{
				if (ServicePointManager.s_ConfigTable == null)
				{
					ServicePointManager.s_ConfigTable = ServicePointManager.ConfigTable;
				}
				ServicePointManager.s_UserChangedLimit = true;
				ServicePointManager.s_ConnectionLimit = value;
			}
		}

		// Token: 0x170002CD RID: 717
		// (get) Token: 0x06000C65 RID: 3173 RVA: 0x00042AE0 File Offset: 0x00040CE0
		private static int PersistentConnectionLimit
		{
			get
			{
				if (ComNetOS.IsAspNetServer)
				{
					return 10;
				}
				return 2;
			}
		}

		// Token: 0x06000C66 RID: 3174 RVA: 0x00042AF0 File Offset: 0x00040CF0
		[Conditional("DEBUG")]
		internal static void DebugMembers(int requestHash)
		{
			try
			{
				foreach (object obj in ServicePointManager.s_ServicePointTable)
				{
					WeakReference weakReference = (WeakReference)obj;
					if (weakReference != null && weakReference.IsAlive)
					{
						ServicePoint servicePoint = (ServicePoint)weakReference.Target;
					}
				}
			}
			catch (Exception ex)
			{
				if (ex is ThreadAbortException || ex is StackOverflowException || ex is OutOfMemoryException)
				{
					throw;
				}
			}
		}

		// Token: 0x170002CE RID: 718
		// (get) Token: 0x06000C67 RID: 3175 RVA: 0x00042B90 File Offset: 0x00040D90
		private static Hashtable ConfigTable
		{
			get
			{
				if (ServicePointManager.s_ConfigTable == null)
				{
					Hashtable hashtable = ServicePointManager.s_ServicePointTable;
					lock (hashtable)
					{
						if (ServicePointManager.s_ConfigTable == null)
						{
							ConnectionManagementSectionInternal section = ConnectionManagementSectionInternal.GetSection();
							Hashtable hashtable2 = null;
							if (section != null)
							{
								hashtable2 = section.ConnectionManagement;
							}
							if (hashtable2 == null)
							{
								hashtable2 = new Hashtable();
							}
							if (hashtable2.ContainsKey("*"))
							{
								int num = (int)hashtable2["*"];
								if (num < 1)
								{
									num = ServicePointManager.PersistentConnectionLimit;
								}
								ServicePointManager.s_ConnectionLimit = num;
							}
							ServicePointManager.s_ConfigTable = hashtable2;
						}
					}
				}
				return ServicePointManager.s_ConfigTable;
			}
		}

		// Token: 0x170002CF RID: 719
		// (get) Token: 0x06000C68 RID: 3176 RVA: 0x00042C3C File Offset: 0x00040E3C
		internal static TimerThread.Callback IdleServicePointTimeoutDelegate
		{
			get
			{
				return ServicePointManager.s_IdleServicePointTimeoutDelegate;
			}
		}

		// Token: 0x06000C69 RID: 3177 RVA: 0x00042C44 File Offset: 0x00040E44
		private static void IdleServicePointTimeoutCallback(TimerThread.Timer timer, int timeNoticed, object context)
		{
			ServicePoint servicePoint = (ServicePoint)context;
			if (Logging.On)
			{
				Logging.PrintInfo(Logging.Web, SR.GetString("net_log_closed_idle", new object[]
				{
					"ServicePoint",
					servicePoint.GetHashCode()
				}));
			}
			Hashtable hashtable = ServicePointManager.s_ServicePointTable;
			lock (hashtable)
			{
				ServicePointManager.s_ServicePointTable.Remove(servicePoint.LookupString);
			}
			servicePoint.ReleaseAllConnectionGroups();
		}

		// Token: 0x06000C6A RID: 3178 RVA: 0x00042CD4 File Offset: 0x00040ED4
		private ServicePointManager()
		{
		}

		// Token: 0x170002D0 RID: 720
		// (get) Token: 0x06000C6B RID: 3179 RVA: 0x00042CDC File Offset: 0x00040EDC
		// (set) Token: 0x06000C6C RID: 3180 RVA: 0x00042CE8 File Offset: 0x00040EE8
		public static SecurityProtocolType SecurityProtocol
		{
			get
			{
				ServicePointManager.EnsureConfigurationLoaded();
				return ServicePointManager.s_SecurityProtocolType;
			}
			set
			{
				ServicePointManager.EnsureConfigurationLoaded();
				ServicePointManager.ValidateSecurityProtocol(value);
				ServicePointManager.s_SecurityProtocolType = value;
			}
		}

		// Token: 0x06000C6D RID: 3181 RVA: 0x00042CFC File Offset: 0x00040EFC
		private static void ValidateSecurityProtocol(SecurityProtocolType value)
		{
			SecurityProtocolType securityProtocolType = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls13;
			if ((value & ~(securityProtocolType != SecurityProtocolType.SystemDefault)) != SecurityProtocolType.SystemDefault)
			{
				throw new NotSupportedException(SR.GetString("net_securityprotocolnotsupported"));
			}
		}

		// Token: 0x170002D1 RID: 721
		// (get) Token: 0x06000C6E RID: 3182 RVA: 0x00042D25 File Offset: 0x00040F25
		internal static bool DisableStrongCrypto
		{
			get
			{
				ServicePointManager.EnsureConfigurationLoaded();
				return ServicePointManager.s_disableStrongCrypto;
			}
		}

		// Token: 0x170002D2 RID: 722
		// (get) Token: 0x06000C6F RID: 3183 RVA: 0x00042D31 File Offset: 0x00040F31
		internal static bool DisableSystemDefaultTlsVersions
		{
			get
			{
				ServicePointManager.EnsureConfigurationLoaded();
				return ServicePointManager.s_disableSystemDefaultTlsVersions;
			}
		}

		// Token: 0x170002D3 RID: 723
		// (get) Token: 0x06000C70 RID: 3184 RVA: 0x00042D3D File Offset: 0x00040F3D
		internal static bool DisableSendAuxRecord
		{
			get
			{
				ServicePointManager.EnsureConfigurationLoaded();
				return ServicePointManager.s_disableSendAuxRecord;
			}
		}

		// Token: 0x170002D4 RID: 724
		// (get) Token: 0x06000C71 RID: 3185 RVA: 0x00042D49 File Offset: 0x00040F49
		internal static bool DisableCertificateEKUs
		{
			get
			{
				ServicePointManager.EnsureConfigurationLoaded();
				return ServicePointManager.s_disableCertificateEKUs;
			}
		}

		// Token: 0x170002D5 RID: 725
		// (get) Token: 0x06000C72 RID: 3186 RVA: 0x00042D55 File Offset: 0x00040F55
		internal static SslProtocols DefaultSslProtocols
		{
			get
			{
				ServicePointManager.EnsureConfigurationLoaded();
				return ServicePointManager.s_defaultSslProtocols;
			}
		}

		// Token: 0x170002D6 RID: 726
		// (get) Token: 0x06000C73 RID: 3187 RVA: 0x00042D61 File Offset: 0x00040F61
		internal static bool UseHttpPipeliningAndBufferPooling
		{
			get
			{
				ServicePointManager.EnsureConfigurationLoaded();
				return ServicePointManager.s_useHttpPipeliningAndBufferPooling;
			}
		}

		// Token: 0x170002D7 RID: 727
		// (get) Token: 0x06000C74 RID: 3188 RVA: 0x00042D6D File Offset: 0x00040F6D
		internal static bool UseSafeSynchronousClose
		{
			get
			{
				ServicePointManager.EnsureConfigurationLoaded();
				return ServicePointManager.s_useSafeSynchronousClose;
			}
		}

		// Token: 0x170002D8 RID: 728
		// (get) Token: 0x06000C75 RID: 3189 RVA: 0x00042D79 File Offset: 0x00040F79
		internal static bool UseStrictRfcInterimResponseHandling
		{
			get
			{
				ServicePointManager.EnsureConfigurationLoaded();
				return ServicePointManager.s_useStrictRfcInterimResponseHandling;
			}
		}

		// Token: 0x170002D9 RID: 729
		// (get) Token: 0x06000C76 RID: 3190 RVA: 0x00042D85 File Offset: 0x00040F85
		internal static bool AllowDangerousUnicodeDecompositions
		{
			get
			{
				ServicePointManager.EnsureConfigurationLoaded();
				return ServicePointManager.s_allowDangerousUnicodeDecompositions;
			}
		}

		// Token: 0x170002DA RID: 730
		// (get) Token: 0x06000C77 RID: 3191 RVA: 0x00042D91 File Offset: 0x00040F91
		internal static bool AllowFullDomainLiterals
		{
			get
			{
				ServicePointManager.EnsureConfigurationLoaded();
				return ServicePointManager.s_allowFullDomainLiterals;
			}
		}

		// Token: 0x170002DB RID: 731
		// (get) Token: 0x06000C78 RID: 3192 RVA: 0x00042D9D File Offset: 0x00040F9D
		internal static bool UseStrictIPv6AddressParsing
		{
			get
			{
				ServicePointManager.EnsureConfigurationLoaded();
				return ServicePointManager.s_useStrictIPv6AddressParsing;
			}
		}

		// Token: 0x170002DC RID: 732
		// (get) Token: 0x06000C79 RID: 3193 RVA: 0x00042DA9 File Offset: 0x00040FA9
		internal static bool AllowAllUriEncodingExpansion
		{
			get
			{
				ServicePointManager.EnsureConfigurationLoaded();
				return ServicePointManager.s_allowAllUriEncodingExpansion;
			}
		}

		// Token: 0x170002DD RID: 733
		// (get) Token: 0x06000C7A RID: 3194 RVA: 0x00042DB5 File Offset: 0x00040FB5
		internal static bool FinishProxyTunnelConnectionEarly
		{
			get
			{
				ServicePointManager.EnsureConfigurationLoaded();
				return ServicePointManager.s_finishProxyTunnelConnectionEarly;
			}
		}

		// Token: 0x170002DE RID: 734
		// (get) Token: 0x06000C7B RID: 3195 RVA: 0x00042DC1 File Offset: 0x00040FC1
		internal static bool AllowNewLineInFtpCommand
		{
			get
			{
				ServicePointManager.EnsureConfigurationLoaded();
				return ServicePointManager.s_allowNewLineInFtpCommand;
			}
		}

		// Token: 0x170002DF RID: 735
		// (get) Token: 0x06000C7C RID: 3196 RVA: 0x00042DCD File Offset: 0x00040FCD
		internal static bool AllowSmtpFallbackToPlainText
		{
			get
			{
				ServicePointManager.EnsureConfigurationLoaded();
				return ServicePointManager.s_allowSmtpFallbackToPlainText;
			}
		}

		// Token: 0x170002E0 RID: 736
		// (get) Token: 0x06000C7D RID: 3197 RVA: 0x00042DD9 File Offset: 0x00040FD9
		// (set) Token: 0x06000C7E RID: 3198 RVA: 0x00042DE0 File Offset: 0x00040FE0
		public static int MaxServicePoints
		{
			get
			{
				return ServicePointManager.s_MaxServicePoints;
			}
			set
			{
				ExceptionHelper.WebPermissionUnrestricted.Demand();
				if (!ValidationHelper.ValidateRange(value, 0, 2147483647))
				{
					throw new ArgumentOutOfRangeException("value");
				}
				ServicePointManager.s_MaxServicePoints = value;
			}
		}

		// Token: 0x170002E1 RID: 737
		// (get) Token: 0x06000C7F RID: 3199 RVA: 0x00042E0B File Offset: 0x0004100B
		// (set) Token: 0x06000C80 RID: 3200 RVA: 0x00042E12 File Offset: 0x00041012
		public static int DefaultConnectionLimit
		{
			get
			{
				return ServicePointManager.InternalConnectionLimit;
			}
			set
			{
				ExceptionHelper.WebPermissionUnrestricted.Demand();
				if (value > 0)
				{
					ServicePointManager.InternalConnectionLimit = value;
					return;
				}
				throw new ArgumentOutOfRangeException("value", SR.GetString("net_toosmall"));
			}
		}

		// Token: 0x170002E2 RID: 738
		// (get) Token: 0x06000C81 RID: 3201 RVA: 0x00042E3D File Offset: 0x0004103D
		// (set) Token: 0x06000C82 RID: 3202 RVA: 0x00042E4C File Offset: 0x0004104C
		public static int MaxServicePointIdleTime
		{
			get
			{
				return ServicePointManager.s_ServicePointIdlingQueue.Duration;
			}
			set
			{
				ExceptionHelper.WebPermissionUnrestricted.Demand();
				if (!ValidationHelper.ValidateRange(value, -1, 2147483647))
				{
					throw new ArgumentOutOfRangeException("value");
				}
				if (ServicePointManager.s_ServicePointIdlingQueue.Duration != value)
				{
					ServicePointManager.s_ServicePointIdlingQueue = TimerThread.GetOrCreateQueue(value);
				}
			}
		}

		// Token: 0x170002E3 RID: 739
		// (get) Token: 0x06000C83 RID: 3203 RVA: 0x00042E98 File Offset: 0x00041098
		// (set) Token: 0x06000C84 RID: 3204 RVA: 0x00042EA4 File Offset: 0x000410A4
		public static bool UseNagleAlgorithm
		{
			get
			{
				return SettingsSectionInternal.Section.UseNagleAlgorithm;
			}
			set
			{
				SettingsSectionInternal.Section.UseNagleAlgorithm = value;
			}
		}

		// Token: 0x170002E4 RID: 740
		// (get) Token: 0x06000C85 RID: 3205 RVA: 0x00042EB1 File Offset: 0x000410B1
		// (set) Token: 0x06000C86 RID: 3206 RVA: 0x00042EBD File Offset: 0x000410BD
		public static bool Expect100Continue
		{
			get
			{
				return SettingsSectionInternal.Section.Expect100Continue;
			}
			set
			{
				SettingsSectionInternal.Section.Expect100Continue = value;
			}
		}

		// Token: 0x170002E5 RID: 741
		// (get) Token: 0x06000C87 RID: 3207 RVA: 0x00042ECA File Offset: 0x000410CA
		// (set) Token: 0x06000C88 RID: 3208 RVA: 0x00042ED6 File Offset: 0x000410D6
		public static bool EnableDnsRoundRobin
		{
			get
			{
				return SettingsSectionInternal.Section.EnableDnsRoundRobin;
			}
			set
			{
				SettingsSectionInternal.Section.EnableDnsRoundRobin = value;
			}
		}

		// Token: 0x170002E6 RID: 742
		// (get) Token: 0x06000C89 RID: 3209 RVA: 0x00042EE3 File Offset: 0x000410E3
		// (set) Token: 0x06000C8A RID: 3210 RVA: 0x00042EEF File Offset: 0x000410EF
		public static int DnsRefreshTimeout
		{
			get
			{
				return SettingsSectionInternal.Section.DnsRefreshTimeout;
			}
			set
			{
				if (value < -1)
				{
					SettingsSectionInternal.Section.DnsRefreshTimeout = -1;
					return;
				}
				SettingsSectionInternal.Section.DnsRefreshTimeout = value;
			}
		}

		// Token: 0x170002E7 RID: 743
		// (get) Token: 0x06000C8B RID: 3211 RVA: 0x00042F0C File Offset: 0x0004110C
		// (set) Token: 0x06000C8C RID: 3212 RVA: 0x00042F13 File Offset: 0x00041113
		[Obsolete("CertificatePolicy is obsoleted for this type, please use ServerCertificateValidationCallback instead. http://go.microsoft.com/fwlink/?linkid=14202")]
		public static ICertificatePolicy CertificatePolicy
		{
			get
			{
				return ServicePointManager.GetLegacyCertificatePolicy();
			}
			set
			{
				ExceptionHelper.UnmanagedPermission.Demand();
				ServicePointManager.s_CertPolicyValidationCallback = new CertPolicyValidationCallback(value);
			}
		}

		// Token: 0x06000C8D RID: 3213 RVA: 0x00042F2C File Offset: 0x0004112C
		internal static ICertificatePolicy GetLegacyCertificatePolicy()
		{
			if (ServicePointManager.s_CertPolicyValidationCallback == null)
			{
				return null;
			}
			return ServicePointManager.s_CertPolicyValidationCallback.CertificatePolicy;
		}

		// Token: 0x170002E8 RID: 744
		// (get) Token: 0x06000C8E RID: 3214 RVA: 0x00042F45 File Offset: 0x00041145
		internal static CertPolicyValidationCallback CertPolicyValidationCallback
		{
			get
			{
				return ServicePointManager.s_CertPolicyValidationCallback;
			}
		}

		// Token: 0x170002E9 RID: 745
		// (get) Token: 0x06000C8F RID: 3215 RVA: 0x00042F4E File Offset: 0x0004114E
		// (set) Token: 0x06000C90 RID: 3216 RVA: 0x00042F67 File Offset: 0x00041167
		public static RemoteCertificateValidationCallback ServerCertificateValidationCallback
		{
			get
			{
				if (ServicePointManager.s_ServerCertValidationCallback == null)
				{
					return null;
				}
				return ServicePointManager.s_ServerCertValidationCallback.ValidationCallback;
			}
			set
			{
				ExceptionHelper.InfrastructurePermission.Demand();
				if (value == null)
				{
					ServicePointManager.s_ServerCertValidationCallback = null;
					return;
				}
				ServicePointManager.s_ServerCertValidationCallback = new ServerCertValidationCallback(value);
			}
		}

		// Token: 0x170002EA RID: 746
		// (get) Token: 0x06000C91 RID: 3217 RVA: 0x00042F8C File Offset: 0x0004118C
		internal static ServerCertValidationCallback ServerCertValidationCallback
		{
			get
			{
				return ServicePointManager.s_ServerCertValidationCallback;
			}
		}

		// Token: 0x170002EB RID: 747
		// (get) Token: 0x06000C92 RID: 3218 RVA: 0x00042F95 File Offset: 0x00041195
		// (set) Token: 0x06000C93 RID: 3219 RVA: 0x00042F9C File Offset: 0x0004119C
		public static bool ReusePort
		{
			get
			{
				return ServicePointManager.s_reusePort;
			}
			set
			{
				ServicePointManager.s_reusePort = value;
			}
		}

		// Token: 0x170002EC RID: 748
		// (get) Token: 0x06000C94 RID: 3220 RVA: 0x00042FA4 File Offset: 0x000411A4
		// (set) Token: 0x06000C95 RID: 3221 RVA: 0x00042FAB File Offset: 0x000411AB
		internal static bool? ReusePortSupported
		{
			get
			{
				return ServicePointManager.s_reusePortSupported;
			}
			set
			{
				ServicePointManager.s_reusePortSupported = value;
			}
		}

		// Token: 0x170002ED RID: 749
		// (get) Token: 0x06000C96 RID: 3222 RVA: 0x00042FB3 File Offset: 0x000411B3
		// (set) Token: 0x06000C97 RID: 3223 RVA: 0x00042FBF File Offset: 0x000411BF
		public static bool CheckCertificateRevocationList
		{
			get
			{
				return SettingsSectionInternal.Section.CheckCertificateRevocationList;
			}
			set
			{
				ExceptionHelper.UnmanagedPermission.Demand();
				SettingsSectionInternal.Section.CheckCertificateRevocationList = value;
			}
		}

		// Token: 0x170002EE RID: 750
		// (get) Token: 0x06000C98 RID: 3224 RVA: 0x00042FD6 File Offset: 0x000411D6
		public static EncryptionPolicy EncryptionPolicy
		{
			get
			{
				return SettingsSectionInternal.Section.EncryptionPolicy;
			}
		}

		// Token: 0x170002EF RID: 751
		// (get) Token: 0x06000C99 RID: 3225 RVA: 0x00042FE2 File Offset: 0x000411E2
		internal static bool CheckCertificateName
		{
			get
			{
				return SettingsSectionInternal.Section.CheckCertificateName;
			}
		}

		// Token: 0x06000C9A RID: 3226 RVA: 0x00042FF0 File Offset: 0x000411F0
		internal static string MakeQueryString(Uri address)
		{
			if (address.IsDefaultPort)
			{
				return address.Scheme + "://" + address.DnsSafeHost;
			}
			return string.Concat(new string[]
			{
				address.Scheme,
				"://",
				address.DnsSafeHost,
				":",
				address.Port.ToString()
			});
		}

		// Token: 0x06000C9B RID: 3227 RVA: 0x0004305A File Offset: 0x0004125A
		internal static string MakeQueryString(Uri address1, bool isProxy)
		{
			if (isProxy)
			{
				return ServicePointManager.MakeQueryString(address1) + "://proxy";
			}
			return ServicePointManager.MakeQueryString(address1);
		}

		// Token: 0x06000C9C RID: 3228 RVA: 0x00043076 File Offset: 0x00041276
		public static ServicePoint FindServicePoint(Uri address)
		{
			return ServicePointManager.FindServicePoint(address, null);
		}

		// Token: 0x06000C9D RID: 3229 RVA: 0x00043080 File Offset: 0x00041280
		public static ServicePoint FindServicePoint(string uriString, IWebProxy proxy)
		{
			Uri uri = new Uri(uriString);
			return ServicePointManager.FindServicePoint(uri, proxy);
		}

		// Token: 0x06000C9E RID: 3230 RVA: 0x0004309C File Offset: 0x0004129C
		public static ServicePoint FindServicePoint(Uri address, IWebProxy proxy)
		{
			HttpAbortDelegate httpAbortDelegate = null;
			int num = 0;
			ProxyChain proxyChain;
			return ServicePointManager.FindServicePoint(address, proxy, out proxyChain, ref httpAbortDelegate, ref num);
		}

		// Token: 0x06000C9F RID: 3231 RVA: 0x000430BC File Offset: 0x000412BC
		internal static ServicePoint FindServicePoint(Uri address, IWebProxy proxy, out ProxyChain chain, ref HttpAbortDelegate abortDelegate, ref int abortState)
		{
			if (address == null)
			{
				throw new ArgumentNullException("address");
			}
			bool flag = false;
			chain = null;
			Uri uri = null;
			if (proxy != null && !address.IsLoopback)
			{
				IAutoWebProxy autoWebProxy = proxy as IAutoWebProxy;
				if (autoWebProxy != null)
				{
					chain = autoWebProxy.GetProxies(address);
					abortDelegate = chain.HttpAbortDelegate;
					try
					{
						Thread.MemoryBarrier();
						if (abortState != 0)
						{
							Exception ex = new WebException(NetRes.GetWebStatusString(WebExceptionStatus.RequestCanceled), WebExceptionStatus.RequestCanceled);
							throw ex;
						}
						chain.Enumerator.MoveNext();
						uri = chain.Enumerator.Current;
						goto IL_008E;
					}
					finally
					{
						abortDelegate = null;
					}
				}
				if (!proxy.IsBypassed(address))
				{
					uri = proxy.GetProxy(address);
				}
				IL_008E:
				if (uri != null)
				{
					address = uri;
					flag = true;
				}
			}
			return ServicePointManager.FindServicePointHelper(address, flag);
		}

		// Token: 0x06000CA0 RID: 3232 RVA: 0x00043180 File Offset: 0x00041380
		internal static ServicePoint FindServicePoint(ProxyChain chain)
		{
			if (!chain.Enumerator.MoveNext())
			{
				return null;
			}
			Uri uri = chain.Enumerator.Current;
			return ServicePointManager.FindServicePointHelper((uri == null) ? chain.Destination : uri, uri != null);
		}

		// Token: 0x06000CA1 RID: 3233 RVA: 0x000431C8 File Offset: 0x000413C8
		private static ServicePoint FindServicePointHelper(Uri address, bool isProxyServicePoint)
		{
			if (isProxyServicePoint && address.Scheme != Uri.UriSchemeHttp)
			{
				Exception ex = new NotSupportedException(SR.GetString("net_proxyschemenotsupported", new object[] { address.Scheme }));
				throw ex;
			}
			string text = ServicePointManager.MakeQueryString(address, isProxyServicePoint);
			ServicePoint servicePoint = null;
			Hashtable hashtable = ServicePointManager.s_ServicePointTable;
			lock (hashtable)
			{
				WeakReference weakReference = ServicePointManager.s_ServicePointTable[text] as WeakReference;
				if (weakReference != null)
				{
					servicePoint = (ServicePoint)weakReference.Target;
				}
				if (servicePoint == null)
				{
					if (ServicePointManager.s_MaxServicePoints > 0 && ServicePointManager.s_ServicePointTable.Count >= ServicePointManager.s_MaxServicePoints)
					{
						Exception ex2 = new InvalidOperationException(SR.GetString("net_maxsrvpoints"));
						throw ex2;
					}
					int num = ServicePointManager.InternalConnectionLimit;
					string text2 = ServicePointManager.MakeQueryString(address);
					bool flag2 = ServicePointManager.s_UserChangedLimit;
					if (ServicePointManager.ConfigTable.ContainsKey(text2))
					{
						num = (int)ServicePointManager.ConfigTable[text2];
						flag2 = true;
					}
					servicePoint = new ServicePoint(address, ServicePointManager.s_ServicePointIdlingQueue, num, text, flag2, isProxyServicePoint);
					weakReference = new WeakReference(servicePoint);
					ServicePointManager.s_ServicePointTable[text] = weakReference;
				}
			}
			return servicePoint;
		}

		// Token: 0x06000CA2 RID: 3234 RVA: 0x00043304 File Offset: 0x00041504
		internal static ServicePoint FindServicePoint(string host, int port)
		{
			if (host == null)
			{
				throw new ArgumentNullException("address");
			}
			bool flag = false;
			string text = "ByHost:" + host + ":" + port.ToString(CultureInfo.InvariantCulture);
			ServicePoint servicePoint = null;
			Hashtable hashtable = ServicePointManager.s_ServicePointTable;
			lock (hashtable)
			{
				WeakReference weakReference = ServicePointManager.s_ServicePointTable[text] as WeakReference;
				if (weakReference != null)
				{
					servicePoint = (ServicePoint)weakReference.Target;
				}
				if (servicePoint == null)
				{
					if (ServicePointManager.s_MaxServicePoints > 0 && ServicePointManager.s_ServicePointTable.Count >= ServicePointManager.s_MaxServicePoints)
					{
						Exception ex = new InvalidOperationException(SR.GetString("net_maxsrvpoints"));
						throw ex;
					}
					int num = ServicePointManager.InternalConnectionLimit;
					bool flag3 = ServicePointManager.s_UserChangedLimit;
					string text2 = host + ":" + port.ToString(CultureInfo.InvariantCulture);
					if (ServicePointManager.ConfigTable.ContainsKey(text2))
					{
						num = (int)ServicePointManager.ConfigTable[text2];
						flag3 = true;
					}
					servicePoint = new ServicePoint(host, port, ServicePointManager.s_ServicePointIdlingQueue, num, text, flag3, flag);
					weakReference = new WeakReference(servicePoint);
					ServicePointManager.s_ServicePointTable[text] = weakReference;
				}
			}
			return servicePoint;
		}

		// Token: 0x06000CA3 RID: 3235 RVA: 0x00043440 File Offset: 0x00041640
		[FriendAccessAllowed]
		internal static void CloseConnectionGroups(string connectionGroupName)
		{
			Hashtable hashtable = ServicePointManager.s_ServicePointTable;
			lock (hashtable)
			{
				foreach (object obj in ServicePointManager.s_ServicePointTable)
				{
					WeakReference weakReference = ((DictionaryEntry)obj).Value as WeakReference;
					if (weakReference != null)
					{
						ServicePoint servicePoint = (ServicePoint)weakReference.Target;
						if (servicePoint != null)
						{
							servicePoint.CloseConnectionGroupInternal(connectionGroupName);
						}
					}
				}
			}
		}

		// Token: 0x06000CA4 RID: 3236 RVA: 0x000434E8 File Offset: 0x000416E8
		public static void SetTcpKeepAlive(bool enabled, int keepAliveTime, int keepAliveInterval)
		{
			if (!enabled)
			{
				ServicePointManager.s_UseTcpKeepAlive = false;
				ServicePointManager.s_TcpKeepAliveTime = 0;
				ServicePointManager.s_TcpKeepAliveInterval = 0;
				return;
			}
			ServicePointManager.s_UseTcpKeepAlive = true;
			if (keepAliveTime <= 0)
			{
				throw new ArgumentOutOfRangeException("keepAliveTime");
			}
			if (keepAliveInterval <= 0)
			{
				throw new ArgumentOutOfRangeException("keepAliveInterval");
			}
			ServicePointManager.s_TcpKeepAliveTime = keepAliveTime;
			ServicePointManager.s_TcpKeepAliveInterval = keepAliveInterval;
		}

		// Token: 0x06000CA5 RID: 3237 RVA: 0x00043548 File Offset: 0x00041748
		private static void LoadConfiguration()
		{
			ServicePointManager.s_reusePort = ServicePointManager.TryInitialize<bool>(new Func<bool, bool>(ServicePointManager.LoadReusePortConfiguration), false);
			ServicePointManager.s_useHttpPipeliningAndBufferPooling = ServicePointManager.TryInitialize<bool>(new Func<bool, bool>(ServicePointManager.LoadUseHttpPipeliningAndBufferPoolingConfiguration), true);
			ServicePointManager.s_useSafeSynchronousClose = ServicePointManager.TryInitialize<bool>(new Func<bool, bool>(ServicePointManager.LoadUseSafeSynchronousClose), true);
			ServicePointManager.s_useStrictRfcInterimResponseHandling = ServicePointManager.TryInitialize<bool>(new Func<bool, bool>(ServicePointManager.LoadUseStrictRfcInterimResponseHandlingConfiguration), true);
			ServicePointManager.s_allowDangerousUnicodeDecompositions = ServicePointManager.TryInitialize<bool>(new Func<bool, bool>(ServicePointManager.LoadAllowDangerousUnicodeDecompositionsConfiguration), false);
			ServicePointManager.s_useStrictIPv6AddressParsing = ServicePointManager.TryInitialize<bool>(new Func<bool, bool>(ServicePointManager.LoadUseStrictIPv6AddressParsingConfiguration), true);
			ServicePointManager.s_allowAllUriEncodingExpansion = ServicePointManager.TryInitialize<bool>(new Func<bool, bool>(ServicePointManager.LoadAllowAllUriEncodingExpansionConfiguration), false);
			ServicePointManager.s_allowFullDomainLiterals = ServicePointManager.TryInitialize<bool>(new Func<bool, bool>(ServicePointManager.LoadAllowFullDomainLiteralsConfiguration), false);
			ServicePointManager.s_finishProxyTunnelConnectionEarly = ServicePointManager.TryInitialize<bool>(new Func<bool, bool>(ServicePointManager.LoadFinishProxyTunnelConnectionEarlyConfiguration), true);
			ServicePointManager.s_allowNewLineInFtpCommand = ServicePointManager.TryInitialize<bool>(new Func<bool, bool>(ServicePointManager.LoadAllowNewLineInFtpCommandConfiguration), false);
			ServicePointManager.s_allowSmtpFallbackToPlainText = ServicePointManager.TryInitialize<bool>(new Func<bool, bool>(ServicePointManager.LoadAllowSmtpFallbackToPlainTextConfiguration), false);
			ServicePointManager.s_disableStrongCrypto = ServicePointManager.TryInitialize<bool>(new Func<bool, bool>(ServicePointManager.LoadDisableStrongCryptoConfiguration), true);
			ServicePointManager.s_disableSendAuxRecord = ServicePointManager.TryInitialize<bool>(new Func<bool, bool>(ServicePointManager.LoadDisableSendAuxRecordConfiguration), false);
			ServicePointManager.s_disableSystemDefaultTlsVersions = ServicePointManager.TryInitialize<bool>(new Func<bool, bool>(ServicePointManager.LoadDisableSystemDefaultTlsVersionsConfiguration), true);
			ServicePointManager.s_disableCertificateEKUs = ServicePointManager.TryInitialize<bool>(new Func<bool, bool>(ServicePointManager.LoadDisableCertificateEKUsConfiguration), false);
			ServicePointManager.s_defaultSslProtocols = ServicePointManager.TryInitialize<SslProtocols>(new Func<SslProtocols, SslProtocols>(ServicePointManager.LoadSecureProtocolConfiguration), SslProtocols.Default);
			ServicePointManager.s_SecurityProtocolType = (SecurityProtocolType)ServicePointManager.s_defaultSslProtocols;
		}

		// Token: 0x06000CA6 RID: 3238 RVA: 0x000436D4 File Offset: 0x000418D4
		private static bool LoadDisableStrongCryptoConfiguration(bool disable)
		{
			if (LocalAppContextSwitches.DontEnableSchUseStrongCrypto)
			{
				int num = RegistryConfiguration.GlobalConfigReadInt("SchUseStrongCrypto", 0);
				disable = num != 1;
			}
			else
			{
				int num = RegistryConfiguration.GlobalConfigReadInt("SchUseStrongCrypto", 1);
				disable = num == 0;
			}
			return disable;
		}

		// Token: 0x06000CA7 RID: 3239 RVA: 0x00043714 File Offset: 0x00041914
		private static bool LoadDisableSendAuxRecordConfiguration(bool disable)
		{
			return LocalAppContextSwitches.DontEnableSchSendAuxRecord || RegistryConfiguration.AppConfigReadInt("System.Net.ServicePointManager.SchSendAuxRecord", 1) == 0 || RegistryConfiguration.GlobalConfigReadInt("SchSendAuxRecord", 1) == 0 || disable;
		}

		// Token: 0x06000CA8 RID: 3240 RVA: 0x00043750 File Offset: 0x00041950
		private static bool LoadDisableSystemDefaultTlsVersionsConfiguration(bool disable)
		{
			if (LocalAppContextSwitches.DontEnableSystemDefaultTlsVersions)
			{
				int num = RegistryConfiguration.GlobalConfigReadInt("SystemDefaultTlsVersions", 0);
				disable = num != 1;
			}
			else
			{
				int num2 = RegistryConfiguration.GlobalConfigReadInt("SystemDefaultTlsVersions", 1);
				disable = num2 == 0;
			}
			if (!disable)
			{
				int num3 = RegistryConfiguration.AppConfigReadInt("System.Net.ServicePointManager.SystemDefaultTlsVersions", 1);
				disable = num3 != 1;
			}
			return disable;
		}

		// Token: 0x06000CA9 RID: 3241 RVA: 0x000437A8 File Offset: 0x000419A8
		private static SslProtocols LoadSecureProtocolConfiguration(SslProtocols defaultValue)
		{
			if (!ServicePointManager.s_disableSystemDefaultTlsVersions)
			{
				defaultValue = SslProtocols.None;
			}
			else if (!ServicePointManager.s_disableStrongCrypto)
			{
				defaultValue = SslProtocols.Tls | SslProtocols.Tls11 | SslProtocols.Tls12 | SslProtocols.Tls13;
			}
			else
			{
				defaultValue = SslProtocols.Default;
			}
			if (!ServicePointManager.s_disableStrongCrypto || !ServicePointManager.s_disableSystemDefaultTlsVersions)
			{
				string text = RegistryConfiguration.AppConfigReadString("System.Net.ServicePointManager.SecurityProtocol", null);
				SecurityProtocolType securityProtocolType;
				if (Enum.TryParse<SecurityProtocolType>(text, out securityProtocolType))
				{
					ServicePointManager.ValidateSecurityProtocol(securityProtocolType);
					defaultValue = (SslProtocols)securityProtocolType;
				}
			}
			return defaultValue;
		}

		// Token: 0x06000CAA RID: 3242 RVA: 0x00043808 File Offset: 0x00041A08
		private static bool LoadReusePortConfiguration(bool reusePortInternal)
		{
			int num = RegistryConfiguration.GlobalConfigReadInt("HWRPortReuseOnSocketBind", 0);
			if (num == 1)
			{
				if (Logging.On)
				{
					Logging.PrintInfo(Logging.Web, typeof(ServicePointManager), SR.GetString("net_log_set_socketoption_reuseport_default_on"));
				}
				reusePortInternal = true;
			}
			return reusePortInternal;
		}

		// Token: 0x06000CAB RID: 3243 RVA: 0x00043850 File Offset: 0x00041A50
		private static bool LoadDisableCertificateEKUsConfiguration(bool disable)
		{
			return LocalAppContextSwitches.DontCheckCertificateEKUs || RegistryConfiguration.AppConfigReadInt("System.Net.ServicePointManager.RequireCertificateEKUs", 1) == 0 || RegistryConfiguration.GlobalConfigReadInt("RequireCertificateEKUs", 1) == 0 || disable;
		}

		// Token: 0x06000CAC RID: 3244 RVA: 0x0004388C File Offset: 0x00041A8C
		private static bool LoadUseHttpPipeliningAndBufferPoolingConfiguration(bool useFeature)
		{
			return RegistryConfiguration.AppConfigReadInt("System.Net.ServicePointManager.UseHttpPipeliningAndBufferPooling", 1) != 0 && RegistryConfiguration.GlobalConfigReadInt("UseHttpPipeliningAndBufferPooling", 1) != 0 && useFeature;
		}

		// Token: 0x06000CAD RID: 3245 RVA: 0x000438BC File Offset: 0x00041ABC
		private static bool LoadUseSafeSynchronousClose(bool useFeature)
		{
			return RegistryConfiguration.AppConfigReadInt("System.Net.ServicePointManager.UseSafeSynchronousClose", 1) != 0 && RegistryConfiguration.GlobalConfigReadInt("UseSafeSynchronousClose", 1) != 0 && useFeature;
		}

		// Token: 0x06000CAE RID: 3246 RVA: 0x000438EC File Offset: 0x00041AEC
		private static bool LoadUseStrictRfcInterimResponseHandlingConfiguration(bool useFeature)
		{
			return RegistryConfiguration.AppConfigReadInt("System.Net.ServicePointManager.UseStrictRfcInterimResponseHandling", 1) != 0 && RegistryConfiguration.GlobalConfigReadInt("UseStrictRfcInterimResponseHandling", 1) != 0 && useFeature;
		}

		// Token: 0x06000CAF RID: 3247 RVA: 0x0004391C File Offset: 0x00041B1C
		private static bool LoadAllowDangerousUnicodeDecompositionsConfiguration(bool useFeature)
		{
			int num = RegistryConfiguration.AppConfigReadInt("System.Uri.AllowDangerousUnicodeDecompositions", 0);
			if (num == 1)
			{
				return true;
			}
			num = RegistryConfiguration.GlobalConfigReadInt("AllowDangerousUnicodeDecompositions", 0);
			return num == 1 || useFeature;
		}

		// Token: 0x06000CB0 RID: 3248 RVA: 0x00043950 File Offset: 0x00041B50
		private static bool LoadUseStrictIPv6AddressParsingConfiguration(bool useFeature)
		{
			return RegistryConfiguration.AppConfigReadInt("System.Uri.UseStrictIPv6AddressParsing", 1) != 0 && RegistryConfiguration.GlobalConfigReadInt("UseStrictIPv6AddressParsing", 1) != 0 && useFeature;
		}

		// Token: 0x06000CB1 RID: 3249 RVA: 0x00043980 File Offset: 0x00041B80
		private static bool LoadAllowAllUriEncodingExpansionConfiguration(bool useFeature)
		{
			int num = RegistryConfiguration.AppConfigReadInt("System.Uri.AllowAllUriEncodingExpansion", 0);
			if (num == 1)
			{
				return true;
			}
			num = RegistryConfiguration.GlobalConfigReadInt("AllowAllUriEncodingExpansion", 0);
			return num == 1 || useFeature;
		}

		// Token: 0x06000CB2 RID: 3250 RVA: 0x000439B4 File Offset: 0x00041BB4
		private static bool LoadAllowFullDomainLiteralsConfiguration(bool useFeature)
		{
			int num = RegistryConfiguration.AppConfigReadInt("System.Net.AllowFullDomainLiterals", 0);
			if (num == 1)
			{
				return true;
			}
			num = RegistryConfiguration.GlobalConfigReadInt("AllowFullDomainLiterals", 0);
			return num == 1 || useFeature;
		}

		// Token: 0x06000CB3 RID: 3251 RVA: 0x000439E8 File Offset: 0x00041BE8
		private static bool LoadFinishProxyTunnelConnectionEarlyConfiguration(bool useFeature)
		{
			return RegistryConfiguration.AppConfigReadInt("System.Net.ServicePointManager.FinishProxyTunnelConnectionEarly", 1) != 0 && RegistryConfiguration.GlobalConfigReadInt("FinishProxyTunnelConnectionEarly", 1) != 0 && useFeature;
		}

		// Token: 0x06000CB4 RID: 3252 RVA: 0x00043A18 File Offset: 0x00041C18
		private static bool LoadAllowNewLineInFtpCommandConfiguration(bool useFeature)
		{
			int num = RegistryConfiguration.AppConfigReadInt("System.Net.AllowNewLineInFtpCommand", 0);
			if (num == 1)
			{
				return true;
			}
			num = RegistryConfiguration.GlobalConfigReadInt("AllowNewLineInFtpCommand", 0);
			return num == 1 || useFeature;
		}

		// Token: 0x06000CB5 RID: 3253 RVA: 0x00043A4C File Offset: 0x00041C4C
		private static bool LoadAllowSmtpFallbackToPlainTextConfiguration(bool useFeature)
		{
			int num = RegistryConfiguration.AppConfigReadInt("System.Net.AllowSmtpFallbackToPlainText", 0);
			if (num == 1)
			{
				return true;
			}
			num = RegistryConfiguration.GlobalConfigReadInt("AllowSmtpFallbackToPlainText", 0);
			return num == 1 || useFeature;
		}

		// Token: 0x06000CB6 RID: 3254 RVA: 0x00043A80 File Offset: 0x00041C80
		private static void EnsureConfigurationLoaded()
		{
			if (ServicePointManager.s_configurationLoaded)
			{
				return;
			}
			object obj = ServicePointManager.s_configurationLoadedLock;
			lock (obj)
			{
				if (!ServicePointManager.s_configurationLoaded)
				{
					ServicePointManager.LoadConfiguration();
					ServicePointManager.s_configurationLoaded = true;
				}
			}
		}

		// Token: 0x06000CB7 RID: 3255 RVA: 0x00043ADC File Offset: 0x00041CDC
		private static T TryInitialize<T>(Func<T, T> loadConfiguration, T fallbackDefault)
		{
			T t;
			try
			{
				t = loadConfiguration(fallbackDefault);
			}
			catch (Exception ex)
			{
				if (NclUtilities.IsFatal(ex))
				{
					throw;
				}
				t = fallbackDefault;
			}
			return t;
		}

		// Token: 0x0400116F RID: 4463
		public const int DefaultNonPersistentConnectionLimit = 4;

		// Token: 0x04001170 RID: 4464
		public const int DefaultPersistentConnectionLimit = 2;

		// Token: 0x04001171 RID: 4465
		private const int DefaultAspPersistentConnectionLimit = 10;

		// Token: 0x04001172 RID: 4466
		internal static readonly string SpecialConnectGroupName = "/.NET/NetClasses/HttpWebRequest/CONNECT__Group$$/";

		// Token: 0x04001173 RID: 4467
		internal static readonly TimerThread.Callback s_IdleServicePointTimeoutDelegate = new TimerThread.Callback(ServicePointManager.IdleServicePointTimeoutCallback);

		// Token: 0x04001174 RID: 4468
		private static Hashtable s_ServicePointTable = new Hashtable(10);

		// Token: 0x04001175 RID: 4469
		private static volatile TimerThread.Queue s_ServicePointIdlingQueue = TimerThread.GetOrCreateQueue(100000);

		// Token: 0x04001176 RID: 4470
		private static int s_MaxServicePoints = 0;

		// Token: 0x04001177 RID: 4471
		private static volatile CertPolicyValidationCallback s_CertPolicyValidationCallback = new CertPolicyValidationCallback();

		// Token: 0x04001178 RID: 4472
		private static volatile ServerCertValidationCallback s_ServerCertValidationCallback = null;

		// Token: 0x04001179 RID: 4473
		private static SecurityProtocolType s_SecurityProtocolType;

		// Token: 0x0400117A RID: 4474
		private static bool s_reusePort;

		// Token: 0x0400117B RID: 4475
		private static bool? s_reusePortSupported = null;

		// Token: 0x0400117C RID: 4476
		private static bool s_disableStrongCrypto;

		// Token: 0x0400117D RID: 4477
		private static bool s_disableSendAuxRecord;

		// Token: 0x0400117E RID: 4478
		private static bool s_disableSystemDefaultTlsVersions;

		// Token: 0x0400117F RID: 4479
		private static SslProtocols s_defaultSslProtocols;

		// Token: 0x04001180 RID: 4480
		private static bool s_disableCertificateEKUs;

		// Token: 0x04001181 RID: 4481
		private static bool s_useHttpPipeliningAndBufferPooling;

		// Token: 0x04001182 RID: 4482
		private static bool s_useSafeSynchronousClose;

		// Token: 0x04001183 RID: 4483
		private static bool s_useStrictRfcInterimResponseHandling;

		// Token: 0x04001184 RID: 4484
		private static bool s_allowDangerousUnicodeDecompositions;

		// Token: 0x04001185 RID: 4485
		private static bool s_useStrictIPv6AddressParsing;

		// Token: 0x04001186 RID: 4486
		private static bool s_allowAllUriEncodingExpansion;

		// Token: 0x04001187 RID: 4487
		private static bool s_allowFullDomainLiterals;

		// Token: 0x04001188 RID: 4488
		private static bool s_finishProxyTunnelConnectionEarly;

		// Token: 0x04001189 RID: 4489
		private static bool s_allowNewLineInFtpCommand;

		// Token: 0x0400118A RID: 4490
		private static bool s_allowSmtpFallbackToPlainText;

		// Token: 0x0400118B RID: 4491
		private static volatile Hashtable s_ConfigTable = null;

		// Token: 0x0400118C RID: 4492
		private static volatile int s_ConnectionLimit = ServicePointManager.PersistentConnectionLimit;

		// Token: 0x0400118D RID: 4493
		internal static volatile bool s_UseTcpKeepAlive = false;

		// Token: 0x0400118E RID: 4494
		internal static volatile int s_TcpKeepAliveTime;

		// Token: 0x0400118F RID: 4495
		internal static volatile int s_TcpKeepAliveInterval;

		// Token: 0x04001190 RID: 4496
		private static volatile bool s_UserChangedLimit;

		// Token: 0x04001191 RID: 4497
		private static object s_configurationLoadedLock = new object();

		// Token: 0x04001192 RID: 4498
		private static volatile bool s_configurationLoaded = false;

		// Token: 0x04001193 RID: 4499
		private const string RegistryGlobalStrongCryptoName = "SchUseStrongCrypto";

		// Token: 0x04001194 RID: 4500
		private const string RegistryGlobalReusePortName = "HWRPortReuseOnSocketBind";

		// Token: 0x04001195 RID: 4501
		private const string RegistryGlobalSendAuxRecordName = "SchSendAuxRecord";

		// Token: 0x04001196 RID: 4502
		private const string RegistryLocalSendAuxRecordName = "System.Net.ServicePointManager.SchSendAuxRecord";

		// Token: 0x04001197 RID: 4503
		private const string RegistryGlobalSystemDefaultTlsVersionsName = "SystemDefaultTlsVersions";

		// Token: 0x04001198 RID: 4504
		private const string RegistryLocalSystemDefaultTlsVersionsName = "System.Net.ServicePointManager.SystemDefaultTlsVersions";

		// Token: 0x04001199 RID: 4505
		private const string RegistryLocalSecureProtocolName = "System.Net.ServicePointManager.SecurityProtocol";

		// Token: 0x0400119A RID: 4506
		private const string RegistryGlobalRequireCertificateEKUs = "RequireCertificateEKUs";

		// Token: 0x0400119B RID: 4507
		private const string RegistryLocalRequireCertificateEKUs = "System.Net.ServicePointManager.RequireCertificateEKUs";

		// Token: 0x0400119C RID: 4508
		private const string RegistryGlobalUseHttpPipeliningAndBufferPooling = "UseHttpPipeliningAndBufferPooling";

		// Token: 0x0400119D RID: 4509
		private const string RegistryLocalUseHttpPipeliningAndBufferPooling = "System.Net.ServicePointManager.UseHttpPipeliningAndBufferPooling";

		// Token: 0x0400119E RID: 4510
		private const string RegistryGlobalUseSafeSynchronousClose = "UseSafeSynchronousClose";

		// Token: 0x0400119F RID: 4511
		private const string RegistryLocalUseSafeSynchronousClose = "System.Net.ServicePointManager.UseSafeSynchronousClose";

		// Token: 0x040011A0 RID: 4512
		private const string RegistryGlobalUseStrictRfcInterimResponseHandling = "UseStrictRfcInterimResponseHandling";

		// Token: 0x040011A1 RID: 4513
		private const string RegistryLocalUseStrictRfcInterimResponseHandling = "System.Net.ServicePointManager.UseStrictRfcInterimResponseHandling";

		// Token: 0x040011A2 RID: 4514
		private const string RegistryGlobalAllowDangerousUnicodeDecompositions = "AllowDangerousUnicodeDecompositions";

		// Token: 0x040011A3 RID: 4515
		private const string RegistryLocalAllowDangerousUnicodeDecompositions = "System.Uri.AllowDangerousUnicodeDecompositions";

		// Token: 0x040011A4 RID: 4516
		private const string RegistryGlobalUseStrictIPv6AddressParsing = "UseStrictIPv6AddressParsing";

		// Token: 0x040011A5 RID: 4517
		private const string RegistryLocalUseStrictIPv6AddressParsing = "System.Uri.UseStrictIPv6AddressParsing";

		// Token: 0x040011A6 RID: 4518
		private const string RegistryGlobalAllowAllUriEncodingExpansion = "AllowAllUriEncodingExpansion";

		// Token: 0x040011A7 RID: 4519
		private const string RegistryLocalAllowAllUriEncodingExpansion = "System.Uri.AllowAllUriEncodingExpansion";

		// Token: 0x040011A8 RID: 4520
		private const string RegistryGlobalAllowFullDomainLiterals = "AllowFullDomainLiterals";

		// Token: 0x040011A9 RID: 4521
		private const string RegistryLocalAllowFullDomainLiterals = "System.Net.AllowFullDomainLiterals";

		// Token: 0x040011AA RID: 4522
		private const string RegistryGlobalFinishProxyTunnelConnectionEarly = "FinishProxyTunnelConnectionEarly";

		// Token: 0x040011AB RID: 4523
		private const string RegistryLocalFinishProxyTunnelConnectionEarly = "System.Net.ServicePointManager.FinishProxyTunnelConnectionEarly";

		// Token: 0x040011AC RID: 4524
		private const string RegistryGlobalAllowNewLineInFtpCommand = "AllowNewLineInFtpCommand";

		// Token: 0x040011AD RID: 4525
		private const string RegistryLocalAllowNewLineInFtpCommand = "System.Net.AllowNewLineInFtpCommand";

		// Token: 0x040011AE RID: 4526
		private const string RegistryGlobalAllowSmtpFallbackToPlainText = "AllowSmtpFallbackToPlainText";

		// Token: 0x040011AF RID: 4527
		private const string RegistryLocalAllowSmtpFallbackToPlainText = "System.Net.AllowSmtpFallbackToPlainText";
	}
}
