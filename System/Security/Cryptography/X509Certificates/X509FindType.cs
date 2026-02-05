using System;

namespace System.Security.Cryptography.X509Certificates
{
	// Token: 0x02000468 RID: 1128
	public enum X509FindType
	{
		// Token: 0x040025C1 RID: 9665
		FindByThumbprint,
		// Token: 0x040025C2 RID: 9666
		FindBySubjectName,
		// Token: 0x040025C3 RID: 9667
		FindBySubjectDistinguishedName,
		// Token: 0x040025C4 RID: 9668
		FindByIssuerName,
		// Token: 0x040025C5 RID: 9669
		FindByIssuerDistinguishedName,
		// Token: 0x040025C6 RID: 9670
		FindBySerialNumber,
		// Token: 0x040025C7 RID: 9671
		FindByTimeValid,
		// Token: 0x040025C8 RID: 9672
		FindByTimeNotYetValid,
		// Token: 0x040025C9 RID: 9673
		FindByTimeExpired,
		// Token: 0x040025CA RID: 9674
		FindByTemplateName,
		// Token: 0x040025CB RID: 9675
		FindByApplicationPolicy,
		// Token: 0x040025CC RID: 9676
		FindByCertificatePolicy,
		// Token: 0x040025CD RID: 9677
		FindByExtension,
		// Token: 0x040025CE RID: 9678
		FindByKeyUsage,
		// Token: 0x040025CF RID: 9679
		FindBySubjectKeyIdentifier
	}
}
