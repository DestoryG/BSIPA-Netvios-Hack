using System;
using System.Security.Cryptography.X509Certificates;

namespace System.Net.Security
{
	// Token: 0x0200035B RID: 859
	// (Invoke) Token: 0x06001EE4 RID: 7908
	public delegate X509Certificate LocalCertificateSelectionCallback(object sender, string targetHost, X509CertificateCollection localCertificates, X509Certificate remoteCertificate, string[] acceptableIssuers);
}
