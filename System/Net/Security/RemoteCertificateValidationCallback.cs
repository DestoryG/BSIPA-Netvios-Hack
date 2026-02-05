using System;
using System.Security.Cryptography.X509Certificates;

namespace System.Net.Security
{
	// Token: 0x0200035A RID: 858
	// (Invoke) Token: 0x06001EE0 RID: 7904
	public delegate bool RemoteCertificateValidationCallback(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors);
}
