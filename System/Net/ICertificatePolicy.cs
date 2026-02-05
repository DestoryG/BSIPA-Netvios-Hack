using System;
using System.Security.Cryptography.X509Certificates;

namespace System.Net
{
	// Token: 0x02000110 RID: 272
	public interface ICertificatePolicy
	{
		// Token: 0x06000B00 RID: 2816
		bool CheckValidationResult(ServicePoint srvPoint, X509Certificate certificate, WebRequest request, int certificateProblem);
	}
}
