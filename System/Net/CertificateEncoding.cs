using System;

namespace System.Net
{
	// Token: 0x02000134 RID: 308
	internal enum CertificateEncoding
	{
		// Token: 0x04001042 RID: 4162
		Zero,
		// Token: 0x04001043 RID: 4163
		X509AsnEncoding,
		// Token: 0x04001044 RID: 4164
		X509NdrEncoding,
		// Token: 0x04001045 RID: 4165
		Pkcs7AsnEncoding = 65536,
		// Token: 0x04001046 RID: 4166
		Pkcs7NdrEncoding = 131072,
		// Token: 0x04001047 RID: 4167
		AnyAsnEncoding = 65537
	}
}
