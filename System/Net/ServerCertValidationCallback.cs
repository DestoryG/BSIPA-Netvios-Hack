using System;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Threading;

namespace System.Net
{
	// Token: 0x020000C9 RID: 201
	internal class ServerCertValidationCallback
	{
		// Token: 0x060006B8 RID: 1720 RVA: 0x00025497 File Offset: 0x00023697
		internal ServerCertValidationCallback(RemoteCertificateValidationCallback validationCallback)
		{
			this.m_ValidationCallback = validationCallback;
			this.m_Context = ExecutionContext.Capture();
		}

		// Token: 0x1700012B RID: 299
		// (get) Token: 0x060006B9 RID: 1721 RVA: 0x000254B1 File Offset: 0x000236B1
		internal RemoteCertificateValidationCallback ValidationCallback
		{
			get
			{
				return this.m_ValidationCallback;
			}
		}

		// Token: 0x060006BA RID: 1722 RVA: 0x000254BC File Offset: 0x000236BC
		internal void Callback(object state)
		{
			ServerCertValidationCallback.CallbackContext callbackContext = (ServerCertValidationCallback.CallbackContext)state;
			callbackContext.result = this.m_ValidationCallback(callbackContext.request, callbackContext.certificate, callbackContext.chain, callbackContext.sslPolicyErrors);
		}

		// Token: 0x060006BB RID: 1723 RVA: 0x000254FC File Offset: 0x000236FC
		internal bool Invoke(object request, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
		{
			if (this.m_Context == null)
			{
				return this.m_ValidationCallback(request, certificate, chain, sslPolicyErrors);
			}
			ExecutionContext executionContext = this.m_Context.CreateCopy();
			ServerCertValidationCallback.CallbackContext callbackContext = new ServerCertValidationCallback.CallbackContext(request, certificate, chain, sslPolicyErrors);
			ExecutionContext.Run(executionContext, new ContextCallback(this.Callback), callbackContext);
			return callbackContext.result;
		}

		// Token: 0x04000C90 RID: 3216
		private readonly RemoteCertificateValidationCallback m_ValidationCallback;

		// Token: 0x04000C91 RID: 3217
		private readonly ExecutionContext m_Context;

		// Token: 0x020006EF RID: 1775
		private class CallbackContext
		{
			// Token: 0x0600406A RID: 16490 RVA: 0x0010E409 File Offset: 0x0010C609
			internal CallbackContext(object request, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
			{
				this.request = request;
				this.certificate = certificate;
				this.chain = chain;
				this.sslPolicyErrors = sslPolicyErrors;
			}

			// Token: 0x0400307C RID: 12412
			internal readonly object request;

			// Token: 0x0400307D RID: 12413
			internal readonly X509Certificate certificate;

			// Token: 0x0400307E RID: 12414
			internal readonly X509Chain chain;

			// Token: 0x0400307F RID: 12415
			internal readonly SslPolicyErrors sslPolicyErrors;

			// Token: 0x04003080 RID: 12416
			internal bool result;
		}
	}
}
