using System;
using System.Security.Cryptography.X509Certificates;

namespace System.Net.Security
{
	// Token: 0x0200035D RID: 861
	// (Invoke) Token: 0x06001EEC RID: 7916
	internal delegate X509Certificate LocalCertSelectionCallback(string targetHost, X509CertificateCollection localCertificates, X509Certificate remoteCertificate, string[] acceptableIssuers);
}
