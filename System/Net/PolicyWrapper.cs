using System;
using System.Collections;
using System.Net.Security;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;

namespace System.Net
{
	// Token: 0x02000144 RID: 324
	internal class PolicyWrapper
	{
		// Token: 0x06000B64 RID: 2916 RVA: 0x0003E1A0 File Offset: 0x0003C3A0
		internal PolicyWrapper(ICertificatePolicy policy, ServicePoint sp, WebRequest wr)
		{
			this.fwdPolicy = policy;
			this.srvPoint = sp;
			this.request = wr;
		}

		// Token: 0x06000B65 RID: 2917 RVA: 0x0003E1BD File Offset: 0x0003C3BD
		public bool Accept(X509Certificate Certificate, int CertificateProblem)
		{
			return this.fwdPolicy.CheckValidationResult(this.srvPoint, Certificate, this.request, CertificateProblem);
		}

		// Token: 0x06000B66 RID: 2918 RVA: 0x0003E1D8 File Offset: 0x0003C3D8
		internal static uint VerifyChainPolicy(SafeFreeCertChain chainContext, ref ChainPolicyParameter cpp)
		{
			ChainPolicyStatus chainPolicyStatus = default(ChainPolicyStatus);
			chainPolicyStatus.cbSize = ChainPolicyStatus.StructSize;
			int num = UnsafeNclNativeMethods.NativePKI.CertVerifyCertificateChainPolicy((IntPtr)4, chainContext, ref cpp, ref chainPolicyStatus);
			return chainPolicyStatus.dwError;
		}

		// Token: 0x06000B67 RID: 2919 RVA: 0x0003E210 File Offset: 0x0003C410
		private static IgnoreCertProblem MapErrorCode(uint errorCode)
		{
			if (errorCode - 2148081682U > 1U)
			{
				if (errorCode != 2148098073U)
				{
					switch (errorCode)
					{
					case 2148204801U:
						return (IgnoreCertProblem)3;
					case 2148204802U:
						return IgnoreCertProblem.not_time_nested;
					case 2148204803U:
						return IgnoreCertProblem.invalid_basic_constraints;
					case 2148204806U:
					case 2148204819U:
						return IgnoreCertProblem.invalid_policy;
					case 2148204809U:
					case 2148204810U:
					case 2148204818U:
						return IgnoreCertProblem.allow_unknown_ca;
					case 2148204812U:
					case 2148204814U:
						return IgnoreCertProblem.all_rev_unknown;
					case 2148204815U:
					case 2148204820U:
						return IgnoreCertProblem.invalid_name;
					case 2148204816U:
						return IgnoreCertProblem.wrong_usage;
					}
					return (IgnoreCertProblem)0;
				}
				return IgnoreCertProblem.invalid_basic_constraints;
			}
			return IgnoreCertProblem.all_rev_unknown;
		}

		// Token: 0x06000B68 RID: 2920 RVA: 0x0003E2AC File Offset: 0x0003C4AC
		private unsafe uint[] GetChainErrors(string hostName, X509Chain chain, ref bool fatalError)
		{
			fatalError = false;
			SafeFreeCertChain safeFreeCertChain = new SafeFreeCertChain(chain.ChainContext);
			ArrayList arrayList = new ArrayList();
			ChainPolicyParameter chainPolicyParameter = default(ChainPolicyParameter);
			chainPolicyParameter.cbSize = ChainPolicyParameter.StructSize;
			chainPolicyParameter.dwFlags = 0U;
			SSL_EXTRA_CERT_CHAIN_POLICY_PARA ssl_EXTRA_CERT_CHAIN_POLICY_PARA = new SSL_EXTRA_CERT_CHAIN_POLICY_PARA(false);
			chainPolicyParameter.pvExtraPolicyPara = &ssl_EXTRA_CERT_CHAIN_POLICY_PARA;
			fixed (string text = hostName)
			{
				char* ptr = text;
				if (ptr != null)
				{
					ptr += RuntimeHelpers.OffsetToStringData / 2;
				}
				if (ServicePointManager.CheckCertificateName)
				{
					ssl_EXTRA_CERT_CHAIN_POLICY_PARA.pwszServerName = ptr;
				}
				for (;;)
				{
					uint num = PolicyWrapper.VerifyChainPolicy(safeFreeCertChain, ref chainPolicyParameter);
					uint num2 = (uint)PolicyWrapper.MapErrorCode(num);
					arrayList.Add(num);
					if (num == 0U)
					{
						goto IL_00BF;
					}
					if (num2 == 0U)
					{
						break;
					}
					chainPolicyParameter.dwFlags |= num2;
					if (num == 2148204815U && ServicePointManager.CheckCertificateName)
					{
						ssl_EXTRA_CERT_CHAIN_POLICY_PARA.fdwChecks = 4096U;
					}
				}
				fatalError = true;
				IL_00BF:;
			}
			return (uint[])arrayList.ToArray(typeof(uint));
		}

		// Token: 0x06000B69 RID: 2921 RVA: 0x0003E390 File Offset: 0x0003C590
		internal bool CheckErrors(string hostName, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
		{
			if (sslPolicyErrors == SslPolicyErrors.None)
			{
				return this.Accept(certificate, 0);
			}
			if ((sslPolicyErrors & SslPolicyErrors.RemoteCertificateNotAvailable) != SslPolicyErrors.None)
			{
				return this.Accept(certificate, -2146762491);
			}
			if ((sslPolicyErrors & SslPolicyErrors.RemoteCertificateChainErrors) != SslPolicyErrors.None || (sslPolicyErrors & SslPolicyErrors.RemoteCertificateNameMismatch) != SslPolicyErrors.None)
			{
				bool flag = false;
				uint[] chainErrors = this.GetChainErrors(hostName, chain, ref flag);
				if (flag)
				{
					this.Accept(certificate, -2146893052);
					return false;
				}
				if (chainErrors.Length == 0)
				{
					return this.Accept(certificate, 0);
				}
				foreach (uint num in chainErrors)
				{
					if (!this.Accept(certificate, (int)num))
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x040010D4 RID: 4308
		private const uint IgnoreUnmatchedCN = 4096U;

		// Token: 0x040010D5 RID: 4309
		private ICertificatePolicy fwdPolicy;

		// Token: 0x040010D6 RID: 4310
		private ServicePoint srvPoint;

		// Token: 0x040010D7 RID: 4311
		private WebRequest request;
	}
}
