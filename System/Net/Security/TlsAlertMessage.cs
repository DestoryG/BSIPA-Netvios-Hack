using System;

namespace System.Net.Security
{
	// Token: 0x02000362 RID: 866
	internal enum TlsAlertMessage
	{
		// Token: 0x04001D52 RID: 7506
		CloseNotify,
		// Token: 0x04001D53 RID: 7507
		UnexpectedMessage = 10,
		// Token: 0x04001D54 RID: 7508
		BadRecordMac = 20,
		// Token: 0x04001D55 RID: 7509
		DecryptionFailed,
		// Token: 0x04001D56 RID: 7510
		RecordOverflow,
		// Token: 0x04001D57 RID: 7511
		DecompressionFail = 30,
		// Token: 0x04001D58 RID: 7512
		HandshakeFailure = 40,
		// Token: 0x04001D59 RID: 7513
		BadCertificate = 42,
		// Token: 0x04001D5A RID: 7514
		UnsupportedCert,
		// Token: 0x04001D5B RID: 7515
		CertificateRevoked,
		// Token: 0x04001D5C RID: 7516
		CertificateExpired,
		// Token: 0x04001D5D RID: 7517
		CertificateUnknown,
		// Token: 0x04001D5E RID: 7518
		IllegalParameter,
		// Token: 0x04001D5F RID: 7519
		UnknownCA,
		// Token: 0x04001D60 RID: 7520
		AccessDenied,
		// Token: 0x04001D61 RID: 7521
		DecodeError,
		// Token: 0x04001D62 RID: 7522
		DecryptError,
		// Token: 0x04001D63 RID: 7523
		ExportRestriction = 60,
		// Token: 0x04001D64 RID: 7524
		ProtocolVersion = 70,
		// Token: 0x04001D65 RID: 7525
		InsuffientSecurity,
		// Token: 0x04001D66 RID: 7526
		InternalError = 80,
		// Token: 0x04001D67 RID: 7527
		UserCanceled = 90,
		// Token: 0x04001D68 RID: 7528
		NoRenegotiation = 100,
		// Token: 0x04001D69 RID: 7529
		UnsupportedExt = 110
	}
}
