using System;
using System.Configuration;
using System.Net.Security;
using System.Net.Sockets;
using System.Threading;

namespace System.Net.Configuration
{
	// Token: 0x02000340 RID: 832
	internal sealed class SettingsSectionInternal
	{
		// Token: 0x06001DB7 RID: 7607 RVA: 0x0008C7E0 File Offset: 0x0008A9E0
		internal SettingsSectionInternal(SettingsSection section)
		{
			if (section == null)
			{
				section = new SettingsSection();
			}
			this.alwaysUseCompletionPortsForConnect = section.Socket.AlwaysUseCompletionPortsForConnect;
			this.alwaysUseCompletionPortsForAccept = section.Socket.AlwaysUseCompletionPortsForAccept;
			this.checkCertificateName = section.ServicePointManager.CheckCertificateName;
			this.checkCertificateRevocationList = section.ServicePointManager.CheckCertificateRevocationList;
			this.dnsRefreshTimeout = section.ServicePointManager.DnsRefreshTimeout;
			this.ipProtectionLevel = section.Socket.IPProtectionLevel;
			this.ipv6Enabled = section.Ipv6.Enabled;
			this.enableDnsRoundRobin = section.ServicePointManager.EnableDnsRoundRobin;
			this.encryptionPolicy = section.ServicePointManager.EncryptionPolicy;
			this.expect100Continue = section.ServicePointManager.Expect100Continue;
			this.maximumUnauthorizedUploadLength = section.HttpWebRequest.MaximumUnauthorizedUploadLength;
			this.maximumResponseHeadersLength = section.HttpWebRequest.MaximumResponseHeadersLength;
			this.maximumErrorResponseLength = section.HttpWebRequest.MaximumErrorResponseLength;
			this.useUnsafeHeaderParsing = section.HttpWebRequest.UseUnsafeHeaderParsing;
			this.useNagleAlgorithm = section.ServicePointManager.UseNagleAlgorithm;
			this.autoConfigUrlRetryInterval = section.WebProxyScript.AutoConfigUrlRetryInterval;
			TimeSpan timeSpan = section.WebProxyScript.DownloadTimeout;
			this.downloadTimeout = ((timeSpan == TimeSpan.MaxValue || timeSpan == TimeSpan.Zero) ? (-1) : ((int)timeSpan.TotalMilliseconds));
			this.performanceCountersEnabled = section.PerformanceCounters.Enabled;
			this.httpListenerUnescapeRequestUrl = section.HttpListener.UnescapeRequestUrl;
			this.httpListenerTimeouts = section.HttpListener.Timeouts.GetTimeouts();
			this.defaultCredentialsHandleCacheSize = section.WindowsAuthentication.DefaultCredentialsHandleCacheSize;
			WebUtilityElement webUtility = section.WebUtility;
			this.WebUtilityUnicodeDecodingConformance = webUtility.UnicodeDecodingConformance;
			this.WebUtilityUnicodeEncodingConformance = webUtility.UnicodeEncodingConformance;
		}

		// Token: 0x1700078A RID: 1930
		// (get) Token: 0x06001DB8 RID: 7608 RVA: 0x0008C9AC File Offset: 0x0008ABAC
		internal static SettingsSectionInternal Section
		{
			get
			{
				if (SettingsSectionInternal.s_settings == null)
				{
					object internalSyncObject = SettingsSectionInternal.InternalSyncObject;
					lock (internalSyncObject)
					{
						if (SettingsSectionInternal.s_settings == null)
						{
							SettingsSectionInternal.s_settings = new SettingsSectionInternal((SettingsSection)PrivilegedConfigurationManager.GetSection(ConfigurationStrings.SettingsSectionPath));
						}
					}
				}
				return SettingsSectionInternal.s_settings;
			}
		}

		// Token: 0x1700078B RID: 1931
		// (get) Token: 0x06001DB9 RID: 7609 RVA: 0x0008CA1C File Offset: 0x0008AC1C
		private static object InternalSyncObject
		{
			get
			{
				if (SettingsSectionInternal.s_InternalSyncObject == null)
				{
					object obj = new object();
					Interlocked.CompareExchange(ref SettingsSectionInternal.s_InternalSyncObject, obj, null);
				}
				return SettingsSectionInternal.s_InternalSyncObject;
			}
		}

		// Token: 0x06001DBA RID: 7610 RVA: 0x0008CA48 File Offset: 0x0008AC48
		internal static SettingsSectionInternal GetSection()
		{
			return new SettingsSectionInternal((SettingsSection)PrivilegedConfigurationManager.GetSection(ConfigurationStrings.SettingsSectionPath));
		}

		// Token: 0x1700078C RID: 1932
		// (get) Token: 0x06001DBB RID: 7611 RVA: 0x0008CA5E File Offset: 0x0008AC5E
		internal bool AlwaysUseCompletionPortsForAccept
		{
			get
			{
				return this.alwaysUseCompletionPortsForAccept;
			}
		}

		// Token: 0x1700078D RID: 1933
		// (get) Token: 0x06001DBC RID: 7612 RVA: 0x0008CA66 File Offset: 0x0008AC66
		internal bool AlwaysUseCompletionPortsForConnect
		{
			get
			{
				return this.alwaysUseCompletionPortsForConnect;
			}
		}

		// Token: 0x1700078E RID: 1934
		// (get) Token: 0x06001DBD RID: 7613 RVA: 0x0008CA6E File Offset: 0x0008AC6E
		internal int AutoConfigUrlRetryInterval
		{
			get
			{
				return this.autoConfigUrlRetryInterval;
			}
		}

		// Token: 0x1700078F RID: 1935
		// (get) Token: 0x06001DBE RID: 7614 RVA: 0x0008CA76 File Offset: 0x0008AC76
		internal bool CheckCertificateName
		{
			get
			{
				return this.checkCertificateName;
			}
		}

		// Token: 0x17000790 RID: 1936
		// (get) Token: 0x06001DBF RID: 7615 RVA: 0x0008CA7E File Offset: 0x0008AC7E
		// (set) Token: 0x06001DC0 RID: 7616 RVA: 0x0008CA86 File Offset: 0x0008AC86
		internal bool CheckCertificateRevocationList
		{
			get
			{
				return this.checkCertificateRevocationList;
			}
			set
			{
				this.checkCertificateRevocationList = value;
			}
		}

		// Token: 0x17000791 RID: 1937
		// (get) Token: 0x06001DC1 RID: 7617 RVA: 0x0008CA8F File Offset: 0x0008AC8F
		// (set) Token: 0x06001DC2 RID: 7618 RVA: 0x0008CA97 File Offset: 0x0008AC97
		internal int DefaultCredentialsHandleCacheSize
		{
			get
			{
				return this.defaultCredentialsHandleCacheSize;
			}
			set
			{
				this.defaultCredentialsHandleCacheSize = value;
			}
		}

		// Token: 0x17000792 RID: 1938
		// (get) Token: 0x06001DC3 RID: 7619 RVA: 0x0008CAA0 File Offset: 0x0008ACA0
		// (set) Token: 0x06001DC4 RID: 7620 RVA: 0x0008CAA8 File Offset: 0x0008ACA8
		internal int DnsRefreshTimeout
		{
			get
			{
				return this.dnsRefreshTimeout;
			}
			set
			{
				this.dnsRefreshTimeout = value;
			}
		}

		// Token: 0x17000793 RID: 1939
		// (get) Token: 0x06001DC5 RID: 7621 RVA: 0x0008CAB1 File Offset: 0x0008ACB1
		internal int DownloadTimeout
		{
			get
			{
				return this.downloadTimeout;
			}
		}

		// Token: 0x17000794 RID: 1940
		// (get) Token: 0x06001DC6 RID: 7622 RVA: 0x0008CAB9 File Offset: 0x0008ACB9
		// (set) Token: 0x06001DC7 RID: 7623 RVA: 0x0008CAC1 File Offset: 0x0008ACC1
		internal bool EnableDnsRoundRobin
		{
			get
			{
				return this.enableDnsRoundRobin;
			}
			set
			{
				this.enableDnsRoundRobin = value;
			}
		}

		// Token: 0x17000795 RID: 1941
		// (get) Token: 0x06001DC8 RID: 7624 RVA: 0x0008CACA File Offset: 0x0008ACCA
		internal EncryptionPolicy EncryptionPolicy
		{
			get
			{
				return this.encryptionPolicy;
			}
		}

		// Token: 0x17000796 RID: 1942
		// (get) Token: 0x06001DC9 RID: 7625 RVA: 0x0008CAD2 File Offset: 0x0008ACD2
		// (set) Token: 0x06001DCA RID: 7626 RVA: 0x0008CADA File Offset: 0x0008ACDA
		internal bool Expect100Continue
		{
			get
			{
				return this.expect100Continue;
			}
			set
			{
				this.expect100Continue = value;
			}
		}

		// Token: 0x17000797 RID: 1943
		// (get) Token: 0x06001DCB RID: 7627 RVA: 0x0008CAE3 File Offset: 0x0008ACE3
		internal IPProtectionLevel IPProtectionLevel
		{
			get
			{
				return this.ipProtectionLevel;
			}
		}

		// Token: 0x17000798 RID: 1944
		// (get) Token: 0x06001DCC RID: 7628 RVA: 0x0008CAEB File Offset: 0x0008ACEB
		internal bool Ipv6Enabled
		{
			get
			{
				return this.ipv6Enabled;
			}
		}

		// Token: 0x17000799 RID: 1945
		// (get) Token: 0x06001DCD RID: 7629 RVA: 0x0008CAF3 File Offset: 0x0008ACF3
		// (set) Token: 0x06001DCE RID: 7630 RVA: 0x0008CAFB File Offset: 0x0008ACFB
		internal int MaximumResponseHeadersLength
		{
			get
			{
				return this.maximumResponseHeadersLength;
			}
			set
			{
				this.maximumResponseHeadersLength = value;
			}
		}

		// Token: 0x1700079A RID: 1946
		// (get) Token: 0x06001DCF RID: 7631 RVA: 0x0008CB04 File Offset: 0x0008AD04
		internal int MaximumUnauthorizedUploadLength
		{
			get
			{
				return this.maximumUnauthorizedUploadLength;
			}
		}

		// Token: 0x1700079B RID: 1947
		// (get) Token: 0x06001DD0 RID: 7632 RVA: 0x0008CB0C File Offset: 0x0008AD0C
		// (set) Token: 0x06001DD1 RID: 7633 RVA: 0x0008CB14 File Offset: 0x0008AD14
		internal int MaximumErrorResponseLength
		{
			get
			{
				return this.maximumErrorResponseLength;
			}
			set
			{
				this.maximumErrorResponseLength = value;
			}
		}

		// Token: 0x1700079C RID: 1948
		// (get) Token: 0x06001DD2 RID: 7634 RVA: 0x0008CB1D File Offset: 0x0008AD1D
		internal bool UseUnsafeHeaderParsing
		{
			get
			{
				return this.useUnsafeHeaderParsing;
			}
		}

		// Token: 0x1700079D RID: 1949
		// (get) Token: 0x06001DD3 RID: 7635 RVA: 0x0008CB25 File Offset: 0x0008AD25
		// (set) Token: 0x06001DD4 RID: 7636 RVA: 0x0008CB2D File Offset: 0x0008AD2D
		internal bool UseNagleAlgorithm
		{
			get
			{
				return this.useNagleAlgorithm;
			}
			set
			{
				this.useNagleAlgorithm = value;
			}
		}

		// Token: 0x1700079E RID: 1950
		// (get) Token: 0x06001DD5 RID: 7637 RVA: 0x0008CB36 File Offset: 0x0008AD36
		internal bool PerformanceCountersEnabled
		{
			get
			{
				return this.performanceCountersEnabled;
			}
		}

		// Token: 0x1700079F RID: 1951
		// (get) Token: 0x06001DD6 RID: 7638 RVA: 0x0008CB3E File Offset: 0x0008AD3E
		internal bool HttpListenerUnescapeRequestUrl
		{
			get
			{
				return this.httpListenerUnescapeRequestUrl;
			}
		}

		// Token: 0x170007A0 RID: 1952
		// (get) Token: 0x06001DD7 RID: 7639 RVA: 0x0008CB46 File Offset: 0x0008AD46
		internal long[] HttpListenerTimeouts
		{
			get
			{
				return this.httpListenerTimeouts;
			}
		}

		// Token: 0x170007A1 RID: 1953
		// (get) Token: 0x06001DD8 RID: 7640 RVA: 0x0008CB4E File Offset: 0x0008AD4E
		// (set) Token: 0x06001DD9 RID: 7641 RVA: 0x0008CB56 File Offset: 0x0008AD56
		internal UnicodeDecodingConformance WebUtilityUnicodeDecodingConformance { get; private set; }

		// Token: 0x170007A2 RID: 1954
		// (get) Token: 0x06001DDA RID: 7642 RVA: 0x0008CB5F File Offset: 0x0008AD5F
		// (set) Token: 0x06001DDB RID: 7643 RVA: 0x0008CB67 File Offset: 0x0008AD67
		internal UnicodeEncodingConformance WebUtilityUnicodeEncodingConformance { get; private set; }

		// Token: 0x04001C69 RID: 7273
		private static object s_InternalSyncObject;

		// Token: 0x04001C6C RID: 7276
		private static volatile SettingsSectionInternal s_settings;

		// Token: 0x04001C6D RID: 7277
		private bool alwaysUseCompletionPortsForAccept;

		// Token: 0x04001C6E RID: 7278
		private bool alwaysUseCompletionPortsForConnect;

		// Token: 0x04001C6F RID: 7279
		private bool checkCertificateName;

		// Token: 0x04001C70 RID: 7280
		private bool checkCertificateRevocationList;

		// Token: 0x04001C71 RID: 7281
		private int defaultCredentialsHandleCacheSize;

		// Token: 0x04001C72 RID: 7282
		private int autoConfigUrlRetryInterval;

		// Token: 0x04001C73 RID: 7283
		private int downloadTimeout;

		// Token: 0x04001C74 RID: 7284
		private int dnsRefreshTimeout;

		// Token: 0x04001C75 RID: 7285
		private bool enableDnsRoundRobin;

		// Token: 0x04001C76 RID: 7286
		private EncryptionPolicy encryptionPolicy;

		// Token: 0x04001C77 RID: 7287
		private bool expect100Continue;

		// Token: 0x04001C78 RID: 7288
		private IPProtectionLevel ipProtectionLevel;

		// Token: 0x04001C79 RID: 7289
		private bool ipv6Enabled;

		// Token: 0x04001C7A RID: 7290
		private int maximumResponseHeadersLength;

		// Token: 0x04001C7B RID: 7291
		private int maximumErrorResponseLength;

		// Token: 0x04001C7C RID: 7292
		private int maximumUnauthorizedUploadLength;

		// Token: 0x04001C7D RID: 7293
		private bool useUnsafeHeaderParsing;

		// Token: 0x04001C7E RID: 7294
		private bool useNagleAlgorithm;

		// Token: 0x04001C7F RID: 7295
		private bool performanceCountersEnabled;

		// Token: 0x04001C80 RID: 7296
		private bool httpListenerUnescapeRequestUrl;

		// Token: 0x04001C81 RID: 7297
		private long[] httpListenerTimeouts;
	}
}
