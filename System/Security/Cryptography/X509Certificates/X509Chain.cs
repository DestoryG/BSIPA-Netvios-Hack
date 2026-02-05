using System;
using System.Globalization;
using System.Net;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Security.Permissions;
using Microsoft.Win32;
using Microsoft.Win32.SafeHandles;

namespace System.Security.Cryptography.X509Certificates
{
	// Token: 0x0200046D RID: 1133
	public class X509Chain : IDisposable
	{
		// Token: 0x06002A1F RID: 10783 RVA: 0x000C02D9 File Offset: 0x000BE4D9
		public static X509Chain Create()
		{
			return (X509Chain)CryptoConfig.CreateFromName("X509Chain");
		}

		// Token: 0x06002A20 RID: 10784 RVA: 0x000C02EA File Offset: 0x000BE4EA
		[SecurityCritical]
		public X509Chain()
			: this(false)
		{
		}

		// Token: 0x06002A21 RID: 10785 RVA: 0x000C02F4 File Offset: 0x000BE4F4
		[SecurityCritical]
		public X509Chain(bool useMachineContext)
		{
			this.m_syncRoot = new object();
			base..ctor();
			this.m_status = 0U;
			this.m_chainPolicy = null;
			this.m_chainStatus = null;
			this.m_chainElementCollection = new X509ChainElementCollection();
			this.m_safeCertChainHandle = SafeX509ChainHandle.InvalidHandle;
			this.m_useMachineContext = useMachineContext;
		}

		// Token: 0x06002A22 RID: 10786 RVA: 0x000C0344 File Offset: 0x000BE544
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		[SecurityPermission(SecurityAction.InheritanceDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public X509Chain(IntPtr chainContext)
		{
			this.m_syncRoot = new object();
			base..ctor();
			if (chainContext == IntPtr.Zero)
			{
				throw new ArgumentNullException("chainContext");
			}
			this.m_safeCertChainHandle = CAPISafe.CertDuplicateCertificateChain(chainContext);
			if (this.m_safeCertChainHandle == null || this.m_safeCertChainHandle == SafeX509ChainHandle.InvalidHandle)
			{
				throw new CryptographicException(SR.GetString("Cryptography_InvalidContextHandle"), "chainContext");
			}
			this.Init();
		}

		// Token: 0x17000A35 RID: 2613
		// (get) Token: 0x06002A23 RID: 10787 RVA: 0x000C03B6 File Offset: 0x000BE5B6
		public IntPtr ChainContext
		{
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			[SecurityPermission(SecurityAction.InheritanceDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			get
			{
				return this.m_safeCertChainHandle.DangerousGetHandle();
			}
		}

		// Token: 0x17000A36 RID: 2614
		// (get) Token: 0x06002A24 RID: 10788 RVA: 0x000C03C3 File Offset: 0x000BE5C3
		public SafeX509ChainHandle SafeHandle
		{
			[SecurityCritical]
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			[SecurityPermission(SecurityAction.InheritanceDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			get
			{
				return this.m_safeCertChainHandle;
			}
		}

		// Token: 0x17000A37 RID: 2615
		// (get) Token: 0x06002A25 RID: 10789 RVA: 0x000C03CB File Offset: 0x000BE5CB
		// (set) Token: 0x06002A26 RID: 10790 RVA: 0x000C03E6 File Offset: 0x000BE5E6
		public X509ChainPolicy ChainPolicy
		{
			get
			{
				if (this.m_chainPolicy == null)
				{
					this.m_chainPolicy = new X509ChainPolicy();
				}
				return this.m_chainPolicy;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				this.m_chainPolicy = value;
			}
		}

		// Token: 0x17000A38 RID: 2616
		// (get) Token: 0x06002A27 RID: 10791 RVA: 0x000C03FD File Offset: 0x000BE5FD
		public X509ChainStatus[] ChainStatus
		{
			get
			{
				if (this.m_chainStatus == null)
				{
					if (this.m_status == 0U)
					{
						this.m_chainStatus = new X509ChainStatus[0];
					}
					else
					{
						this.m_chainStatus = X509Chain.GetChainStatusInformation(this.m_status);
					}
				}
				return this.m_chainStatus;
			}
		}

		// Token: 0x17000A39 RID: 2617
		// (get) Token: 0x06002A28 RID: 10792 RVA: 0x000C0434 File Offset: 0x000BE634
		public X509ChainElementCollection ChainElements
		{
			get
			{
				return this.m_chainElementCollection;
			}
		}

		// Token: 0x06002A29 RID: 10793 RVA: 0x000C043C File Offset: 0x000BE63C
		[PermissionSet(SecurityAction.LinkDemand, Unrestricted = true)]
		[PermissionSet(SecurityAction.InheritanceDemand, Unrestricted = true)]
		public bool Build(X509Certificate2 certificate)
		{
			object syncRoot = this.m_syncRoot;
			bool flag2;
			lock (syncRoot)
			{
				if (certificate == null || certificate.CertContext.IsInvalid)
				{
					throw new ArgumentException(SR.GetString("Cryptography_InvalidContextHandle"), "certificate");
				}
				StorePermission storePermission = new StorePermission(StorePermissionFlags.OpenStore | StorePermissionFlags.EnumerateCertificates);
				storePermission.Demand();
				X509ChainPolicy chainPolicy = this.ChainPolicy;
				if (chainPolicy.RevocationMode == X509RevocationMode.Online && (certificate.Extensions["2.5.29.31"] != null || certificate.Extensions["1.3.6.1.5.5.7.1.1"] != null))
				{
					PermissionSet permissionSet = new PermissionSet(PermissionState.None);
					permissionSet.AddPermission(new WebPermission(PermissionState.Unrestricted));
					permissionSet.AddPermission(new StorePermission(StorePermissionFlags.AddToStore));
					permissionSet.Demand();
				}
				this.Reset();
				int num = X509Chain.BuildChain(this.m_useMachineContext ? new IntPtr(1L) : new IntPtr(0L), certificate.CertContext, chainPolicy.ExtraStore, chainPolicy.ApplicationPolicy, chainPolicy.CertificatePolicy, chainPolicy.RevocationMode, chainPolicy.RevocationFlag, chainPolicy.VerificationTime, chainPolicy.UrlRetrievalTimeout, ref this.m_safeCertChainHandle);
				if (num != 0)
				{
					if (X509Chain.CompatSwitches.ShouldThrowOnChainBuildingFailure)
					{
						throw new CryptographicException(num);
					}
					flag2 = false;
				}
				else
				{
					this.Init();
					CAPIBase.CERT_CHAIN_POLICY_PARA cert_CHAIN_POLICY_PARA = new CAPIBase.CERT_CHAIN_POLICY_PARA(Marshal.SizeOf(typeof(CAPIBase.CERT_CHAIN_POLICY_PARA)));
					CAPIBase.CERT_CHAIN_POLICY_STATUS cert_CHAIN_POLICY_STATUS = new CAPIBase.CERT_CHAIN_POLICY_STATUS(Marshal.SizeOf(typeof(CAPIBase.CERT_CHAIN_POLICY_STATUS)));
					cert_CHAIN_POLICY_PARA.dwFlags = (uint)chainPolicy.VerificationFlags;
					if (!CAPISafe.CertVerifyCertificateChainPolicy(new IntPtr(1L), this.m_safeCertChainHandle, ref cert_CHAIN_POLICY_PARA, ref cert_CHAIN_POLICY_STATUS))
					{
						throw new CryptographicException(Marshal.GetLastWin32Error());
					}
					CAPISafe.SetLastError(cert_CHAIN_POLICY_STATUS.dwError);
					flag2 = cert_CHAIN_POLICY_STATUS.dwError == 0U;
				}
			}
			return flag2;
		}

		// Token: 0x06002A2A RID: 10794 RVA: 0x000C060C File Offset: 0x000BE80C
		[SecurityCritical]
		[PermissionSet(SecurityAction.LinkDemand, Unrestricted = true)]
		[PermissionSet(SecurityAction.InheritanceDemand, Unrestricted = true)]
		public void Reset()
		{
			this.m_status = 0U;
			this.m_chainStatus = null;
			this.m_chainElementCollection = new X509ChainElementCollection();
			if (!this.m_safeCertChainHandle.IsInvalid)
			{
				this.m_safeCertChainHandle.Dispose();
				this.m_safeCertChainHandle = SafeX509ChainHandle.InvalidHandle;
			}
		}

		// Token: 0x06002A2B RID: 10795 RVA: 0x000C064A File Offset: 0x000BE84A
		[SecuritySafeCritical]
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06002A2C RID: 10796 RVA: 0x000C0659 File Offset: 0x000BE859
		[SecuritySafeCritical]
		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				this.Reset();
			}
		}

		// Token: 0x06002A2D RID: 10797 RVA: 0x000C0664 File Offset: 0x000BE864
		[SecurityCritical]
		private unsafe void Init()
		{
			using (SafeX509ChainHandle safeX509ChainHandle = CAPISafe.CertDuplicateCertificateChain(this.m_safeCertChainHandle))
			{
				CAPIBase.CERT_CHAIN_CONTEXT cert_CHAIN_CONTEXT = new CAPIBase.CERT_CHAIN_CONTEXT(Marshal.SizeOf(typeof(CAPIBase.CERT_CHAIN_CONTEXT)));
				uint num = (uint)Marshal.ReadInt32(safeX509ChainHandle.DangerousGetHandle());
				if ((ulong)num > (ulong)((long)Marshal.SizeOf(cert_CHAIN_CONTEXT)))
				{
					num = (uint)Marshal.SizeOf(cert_CHAIN_CONTEXT);
				}
				X509Utils.memcpy(this.m_safeCertChainHandle.DangerousGetHandle(), new IntPtr((void*)(&cert_CHAIN_CONTEXT)), num);
				this.m_status = cert_CHAIN_CONTEXT.dwErrorStatus;
				this.m_chainElementCollection = new X509ChainElementCollection(Marshal.ReadIntPtr(cert_CHAIN_CONTEXT.rgpChain));
			}
		}

		// Token: 0x06002A2E RID: 10798 RVA: 0x000C0714 File Offset: 0x000BE914
		internal static X509ChainStatus[] GetChainStatusInformation(uint dwStatus)
		{
			if (dwStatus == 0U)
			{
				return new X509ChainStatus[0];
			}
			int num = 0;
			for (uint num2 = dwStatus; num2 != 0U; num2 >>= 1)
			{
				if ((num2 & 1U) != 0U)
				{
					num++;
				}
			}
			X509ChainStatus[] array = new X509ChainStatus[num];
			int num3 = 0;
			foreach (X509Chain.X509ChainErrorMapping x509ChainErrorMapping in X509Chain.s_x509ChainErrorMappings)
			{
				if ((dwStatus & x509ChainErrorMapping.Win32Flag) != 0U)
				{
					array[num3].StatusInformation = X509Utils.GetSystemErrorString(x509ChainErrorMapping.Win32ErrorCode);
					array[num3].Status = x509ChainErrorMapping.ChainStatusFlag;
					num3++;
					dwStatus &= ~x509ChainErrorMapping.Win32Flag;
				}
			}
			int num4 = 0;
			for (uint num5 = dwStatus; num5 != 0U; num5 >>= 1)
			{
				if ((num5 & 1U) != 0U)
				{
					array[num3].Status = (X509ChainStatusFlags)(1 << num4);
					array[num3].StatusInformation = SR.GetString("Unknown_Error");
					num3++;
				}
				num4++;
			}
			return array;
		}

		// Token: 0x06002A2F RID: 10799 RVA: 0x000C0804 File Offset: 0x000BEA04
		[SecurityCritical]
		internal unsafe static int BuildChain(IntPtr hChainEngine, SafeCertContextHandle pCertContext, X509Certificate2Collection extraStore, OidCollection applicationPolicy, OidCollection certificatePolicy, X509RevocationMode revocationMode, X509RevocationFlag revocationFlag, DateTime verificationTime, TimeSpan timeout, ref SafeX509ChainHandle ppChainContext)
		{
			if (pCertContext == null || pCertContext.IsInvalid)
			{
				throw new ArgumentException(SR.GetString("Cryptography_InvalidContextHandle"), "pCertContext");
			}
			SafeCertStoreHandle safeCertStoreHandle = SafeCertStoreHandle.InvalidHandle;
			if (extraStore != null && extraStore.Count > 0)
			{
				safeCertStoreHandle = X509Utils.ExportToMemoryStore(extraStore);
			}
			CAPIBase.CERT_CHAIN_PARA cert_CHAIN_PARA = default(CAPIBase.CERT_CHAIN_PARA);
			cert_CHAIN_PARA.cbSize = (uint)Marshal.SizeOf(cert_CHAIN_PARA);
			SafeLocalAllocHandle safeLocalAllocHandle = SafeLocalAllocHandle.InvalidHandle;
			SafeLocalAllocHandle safeLocalAllocHandle2 = SafeLocalAllocHandle.InvalidHandle;
			try
			{
				if (applicationPolicy != null && applicationPolicy.Count > 0)
				{
					cert_CHAIN_PARA.RequestedUsage.dwType = 0U;
					cert_CHAIN_PARA.RequestedUsage.Usage.cUsageIdentifier = (uint)applicationPolicy.Count;
					safeLocalAllocHandle = X509Utils.CopyOidsToUnmanagedMemory(applicationPolicy);
					cert_CHAIN_PARA.RequestedUsage.Usage.rgpszUsageIdentifier = safeLocalAllocHandle.DangerousGetHandle();
				}
				if (certificatePolicy != null && certificatePolicy.Count > 0)
				{
					cert_CHAIN_PARA.RequestedIssuancePolicy.dwType = 0U;
					cert_CHAIN_PARA.RequestedIssuancePolicy.Usage.cUsageIdentifier = (uint)certificatePolicy.Count;
					safeLocalAllocHandle2 = X509Utils.CopyOidsToUnmanagedMemory(certificatePolicy);
					cert_CHAIN_PARA.RequestedIssuancePolicy.Usage.rgpszUsageIdentifier = safeLocalAllocHandle2.DangerousGetHandle();
				}
				cert_CHAIN_PARA.dwUrlRetrievalTimeout = (uint)Math.Floor(timeout.TotalMilliseconds);
				global::System.Runtime.InteropServices.ComTypes.FILETIME filetime = default(global::System.Runtime.InteropServices.ComTypes.FILETIME);
				*(long*)(&filetime) = verificationTime.ToFileTime();
				uint num = X509Utils.MapRevocationFlags(revocationMode, revocationFlag);
				if (!CAPISafe.CertGetCertificateChain(hChainEngine, pCertContext, ref filetime, safeCertStoreHandle, ref cert_CHAIN_PARA, num, IntPtr.Zero, ref ppChainContext))
				{
					return Marshal.GetHRForLastWin32Error();
				}
			}
			finally
			{
				safeLocalAllocHandle.Dispose();
				safeLocalAllocHandle2.Dispose();
			}
			return 0;
		}

		// Token: 0x040025F1 RID: 9713
		private uint m_status;

		// Token: 0x040025F2 RID: 9714
		private X509ChainPolicy m_chainPolicy;

		// Token: 0x040025F3 RID: 9715
		private X509ChainStatus[] m_chainStatus;

		// Token: 0x040025F4 RID: 9716
		private X509ChainElementCollection m_chainElementCollection;

		// Token: 0x040025F5 RID: 9717
		[SecurityCritical]
		private SafeX509ChainHandle m_safeCertChainHandle;

		// Token: 0x040025F6 RID: 9718
		private bool m_useMachineContext;

		// Token: 0x040025F7 RID: 9719
		private readonly object m_syncRoot;

		// Token: 0x040025F8 RID: 9720
		private static readonly X509Chain.X509ChainErrorMapping[] s_x509ChainErrorMappings = new X509Chain.X509ChainErrorMapping[]
		{
			new X509Chain.X509ChainErrorMapping(8U, -2146869244, X509ChainStatusFlags.NotSignatureValid),
			new X509Chain.X509ChainErrorMapping(262144U, -2146869244, X509ChainStatusFlags.CtlNotSignatureValid),
			new X509Chain.X509ChainErrorMapping(32U, -2146762487, X509ChainStatusFlags.UntrustedRoot),
			new X509Chain.X509ChainErrorMapping(65536U, -2146762486, X509ChainStatusFlags.PartialChain),
			new X509Chain.X509ChainErrorMapping(4U, -2146885616, X509ChainStatusFlags.Revoked),
			new X509Chain.X509ChainErrorMapping(16U, -2146762480, X509ChainStatusFlags.NotValidForUsage),
			new X509Chain.X509ChainErrorMapping(524288U, -2146762480, X509ChainStatusFlags.CtlNotValidForUsage),
			new X509Chain.X509ChainErrorMapping(1U, -2146762495, X509ChainStatusFlags.NotTimeValid),
			new X509Chain.X509ChainErrorMapping(131072U, -2146762495, X509ChainStatusFlags.CtlNotTimeValid),
			new X509Chain.X509ChainErrorMapping(2048U, -2146762476, X509ChainStatusFlags.InvalidNameConstraints),
			new X509Chain.X509ChainErrorMapping(4096U, -2146762476, X509ChainStatusFlags.HasNotSupportedNameConstraint),
			new X509Chain.X509ChainErrorMapping(8192U, -2146762476, X509ChainStatusFlags.HasNotDefinedNameConstraint),
			new X509Chain.X509ChainErrorMapping(16384U, -2146762476, X509ChainStatusFlags.HasNotPermittedNameConstraint),
			new X509Chain.X509ChainErrorMapping(32768U, -2146762476, X509ChainStatusFlags.HasExcludedNameConstraint),
			new X509Chain.X509ChainErrorMapping(512U, -2146762477, X509ChainStatusFlags.InvalidPolicyConstraints),
			new X509Chain.X509ChainErrorMapping(33554432U, -2146762477, X509ChainStatusFlags.NoIssuanceChainPolicy),
			new X509Chain.X509ChainErrorMapping(1024U, -2146869223, X509ChainStatusFlags.InvalidBasicConstraints),
			new X509Chain.X509ChainErrorMapping(2U, -2146762494, X509ChainStatusFlags.NotTimeNested),
			new X509Chain.X509ChainErrorMapping(64U, -2146885614, X509ChainStatusFlags.RevocationStatusUnknown),
			new X509Chain.X509ChainErrorMapping(16777216U, -2146885613, X509ChainStatusFlags.OfflineRevocation),
			new X509Chain.X509ChainErrorMapping(67108864U, -2146762479, X509ChainStatusFlags.ExplicitDistrust),
			new X509Chain.X509ChainErrorMapping(134217728U, -2146762491, X509ChainStatusFlags.HasNotSupportedCriticalExtension),
			new X509Chain.X509ChainErrorMapping(1048576U, -2146877418, X509ChainStatusFlags.HasWeakSignature)
		};

		// Token: 0x02000879 RID: 2169
		private struct X509ChainErrorMapping
		{
			// Token: 0x06004566 RID: 17766 RVA: 0x001217C4 File Offset: 0x0011F9C4
			public X509ChainErrorMapping(uint win32Flag, int win32ErrorCode, X509ChainStatusFlags chainStatusFlag)
			{
				this.Win32Flag = win32Flag;
				this.Win32ErrorCode = win32ErrorCode;
				this.ChainStatusFlag = chainStatusFlag;
			}

			// Token: 0x0400371B RID: 14107
			public readonly uint Win32Flag;

			// Token: 0x0400371C RID: 14108
			public readonly int Win32ErrorCode;

			// Token: 0x0400371D RID: 14109
			public readonly X509ChainStatusFlags ChainStatusFlag;
		}

		// Token: 0x0200087A RID: 2170
		private static class CompatSwitches
		{
			// Token: 0x06004567 RID: 17767 RVA: 0x001217DC File Offset: 0x0011F9DC
			[SecuritySafeCritical]
			[EnvironmentPermission(SecurityAction.Assert, Unrestricted = true)]
			private static int ReadInt32CompatSwitch(string switchName, int defaultValue)
			{
				string environmentVariable = Environment.GetEnvironmentVariable("COMPlus_" + switchName);
				int num;
				if (environmentVariable != null && int.TryParse(environmentVariable, NumberStyles.Integer, CultureInfo.InvariantCulture, out num))
				{
					return num;
				}
				int? num2 = X509Chain.CompatSwitches.ReadInt32CompatSwitchFromRegistry(RegistryHive.CurrentUser, switchName);
				if (num2 != null)
				{
					return num2.GetValueOrDefault();
				}
				int? num3 = X509Chain.CompatSwitches.ReadInt32CompatSwitchFromRegistry(RegistryHive.LocalMachine, switchName);
				if (num3 == null)
				{
					return defaultValue;
				}
				return num3.GetValueOrDefault();
			}

			// Token: 0x06004568 RID: 17768 RVA: 0x0012184C File Offset: 0x0011FA4C
			[SecuritySafeCritical]
			[SecurityPermission(SecurityAction.Assert, UnmanagedCode = true)]
			[RegistryPermission(SecurityAction.Assert, Unrestricted = true)]
			private static int? ReadInt32CompatSwitchFromRegistry(RegistryHive hive, string switchName)
			{
				try
				{
					using (RegistryKey registryKey = RegistryKey.OpenBaseKey(hive, RegistryView.Registry64))
					{
						using (RegistryKey registryKey2 = registryKey.OpenSubKey("SOFTWARE\\Microsoft\\.NETFramework", false))
						{
							return ((registryKey2 != null) ? registryKey2.GetValue(switchName) : null) as int?;
						}
					}
				}
				catch
				{
				}
				return null;
			}

			// Token: 0x0400371E RID: 14110
			internal static readonly bool ShouldThrowOnChainBuildingFailure = X509Chain.CompatSwitches.ReadInt32CompatSwitch("X509Chain_ThrowOnBuildFailure", 1) != 0;
		}
	}
}
