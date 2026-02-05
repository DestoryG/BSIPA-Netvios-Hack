using System;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Threading;

namespace System.Net
{
	// Token: 0x020000C8 RID: 200
	internal class CertPolicyValidationCallback
	{
		// Token: 0x060006B2 RID: 1714 RVA: 0x000253AD File Offset: 0x000235AD
		internal CertPolicyValidationCallback()
		{
			this.m_CertificatePolicy = new DefaultCertPolicy();
			this.m_Context = null;
		}

		// Token: 0x060006B3 RID: 1715 RVA: 0x000253C7 File Offset: 0x000235C7
		internal CertPolicyValidationCallback(ICertificatePolicy certificatePolicy)
		{
			this.m_CertificatePolicy = certificatePolicy;
			this.m_Context = ExecutionContext.Capture();
		}

		// Token: 0x17000129 RID: 297
		// (get) Token: 0x060006B4 RID: 1716 RVA: 0x000253E1 File Offset: 0x000235E1
		internal ICertificatePolicy CertificatePolicy
		{
			get
			{
				return this.m_CertificatePolicy;
			}
		}

		// Token: 0x1700012A RID: 298
		// (get) Token: 0x060006B5 RID: 1717 RVA: 0x000253E9 File Offset: 0x000235E9
		internal bool UsesDefault
		{
			get
			{
				return this.m_Context == null;
			}
		}

		// Token: 0x060006B6 RID: 1718 RVA: 0x000253F4 File Offset: 0x000235F4
		internal void Callback(object state)
		{
			CertPolicyValidationCallback.CallbackContext callbackContext = (CertPolicyValidationCallback.CallbackContext)state;
			callbackContext.result = callbackContext.policyWrapper.CheckErrors(callbackContext.hostName, callbackContext.certificate, callbackContext.chain, callbackContext.sslPolicyErrors);
		}

		// Token: 0x060006B7 RID: 1719 RVA: 0x00025434 File Offset: 0x00023634
		internal bool Invoke(string hostName, ServicePoint servicePoint, X509Certificate certificate, WebRequest request, X509Chain chain, SslPolicyErrors sslPolicyErrors)
		{
			PolicyWrapper policyWrapper = new PolicyWrapper(this.m_CertificatePolicy, servicePoint, request);
			if (this.m_Context == null)
			{
				return policyWrapper.CheckErrors(hostName, certificate, chain, sslPolicyErrors);
			}
			ExecutionContext executionContext = this.m_Context.CreateCopy();
			CertPolicyValidationCallback.CallbackContext callbackContext = new CertPolicyValidationCallback.CallbackContext(policyWrapper, hostName, certificate, chain, sslPolicyErrors);
			ExecutionContext.Run(executionContext, new ContextCallback(this.Callback), callbackContext);
			return callbackContext.result;
		}

		// Token: 0x04000C8E RID: 3214
		private readonly ICertificatePolicy m_CertificatePolicy;

		// Token: 0x04000C8F RID: 3215
		private readonly ExecutionContext m_Context;

		// Token: 0x020006EE RID: 1774
		private class CallbackContext
		{
			// Token: 0x06004069 RID: 16489 RVA: 0x0010E3DC File Offset: 0x0010C5DC
			internal CallbackContext(PolicyWrapper policyWrapper, string hostName, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
			{
				this.policyWrapper = policyWrapper;
				this.hostName = hostName;
				this.certificate = certificate;
				this.chain = chain;
				this.sslPolicyErrors = sslPolicyErrors;
			}

			// Token: 0x04003076 RID: 12406
			internal readonly PolicyWrapper policyWrapper;

			// Token: 0x04003077 RID: 12407
			internal readonly string hostName;

			// Token: 0x04003078 RID: 12408
			internal readonly X509Certificate certificate;

			// Token: 0x04003079 RID: 12409
			internal readonly X509Chain chain;

			// Token: 0x0400307A RID: 12410
			internal readonly SslPolicyErrors sslPolicyErrors;

			// Token: 0x0400307B RID: 12411
			internal bool result;
		}
	}
}
