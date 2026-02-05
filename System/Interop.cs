using System;

// Token: 0x02000003 RID: 3
internal static class Interop
{
	// Token: 0x020006AF RID: 1711
	internal static class SChannel
	{
		// Token: 0x04002EB4 RID: 11956
		public const int SCHANNEL_RENEGOTIATE = 0;

		// Token: 0x04002EB5 RID: 11957
		public const int SCHANNEL_SHUTDOWN = 1;

		// Token: 0x04002EB6 RID: 11958
		public const int SCHANNEL_ALERT = 2;

		// Token: 0x04002EB7 RID: 11959
		public const int SCHANNEL_SESSION = 3;

		// Token: 0x04002EB8 RID: 11960
		public const int TLS1_ALERT_WARNING = 1;

		// Token: 0x04002EB9 RID: 11961
		public const int TLS1_ALERT_FATAL = 2;

		// Token: 0x04002EBA RID: 11962
		public const int TLS1_ALERT_CLOSE_NOTIFY = 0;

		// Token: 0x04002EBB RID: 11963
		public const int TLS1_ALERT_UNEXPECTED_MESSAGE = 10;

		// Token: 0x04002EBC RID: 11964
		public const int TLS1_ALERT_BAD_RECORD_MAC = 20;

		// Token: 0x04002EBD RID: 11965
		public const int TLS1_ALERT_DECRYPTION_FAILED = 21;

		// Token: 0x04002EBE RID: 11966
		public const int TLS1_ALERT_RECORD_OVERFLOW = 22;

		// Token: 0x04002EBF RID: 11967
		public const int TLS1_ALERT_DECOMPRESSION_FAIL = 30;

		// Token: 0x04002EC0 RID: 11968
		public const int TLS1_ALERT_HANDSHAKE_FAILURE = 40;

		// Token: 0x04002EC1 RID: 11969
		public const int TLS1_ALERT_BAD_CERTIFICATE = 42;

		// Token: 0x04002EC2 RID: 11970
		public const int TLS1_ALERT_UNSUPPORTED_CERT = 43;

		// Token: 0x04002EC3 RID: 11971
		public const int TLS1_ALERT_CERTIFICATE_REVOKED = 44;

		// Token: 0x04002EC4 RID: 11972
		public const int TLS1_ALERT_CERTIFICATE_EXPIRED = 45;

		// Token: 0x04002EC5 RID: 11973
		public const int TLS1_ALERT_CERTIFICATE_UNKNOWN = 46;

		// Token: 0x04002EC6 RID: 11974
		public const int TLS1_ALERT_ILLEGAL_PARAMETER = 47;

		// Token: 0x04002EC7 RID: 11975
		public const int TLS1_ALERT_UNKNOWN_CA = 48;

		// Token: 0x04002EC8 RID: 11976
		public const int TLS1_ALERT_ACCESS_DENIED = 49;

		// Token: 0x04002EC9 RID: 11977
		public const int TLS1_ALERT_DECODE_ERROR = 50;

		// Token: 0x04002ECA RID: 11978
		public const int TLS1_ALERT_DECRYPT_ERROR = 51;

		// Token: 0x04002ECB RID: 11979
		public const int TLS1_ALERT_EXPORT_RESTRICTION = 60;

		// Token: 0x04002ECC RID: 11980
		public const int TLS1_ALERT_PROTOCOL_VERSION = 70;

		// Token: 0x04002ECD RID: 11981
		public const int TLS1_ALERT_INSUFFIENT_SECURITY = 71;

		// Token: 0x04002ECE RID: 11982
		public const int TLS1_ALERT_INTERNAL_ERROR = 80;

		// Token: 0x04002ECF RID: 11983
		public const int TLS1_ALERT_USER_CANCELED = 90;

		// Token: 0x04002ED0 RID: 11984
		public const int TLS1_ALERT_NO_RENEGOTIATION = 100;

		// Token: 0x04002ED1 RID: 11985
		public const int TLS1_ALERT_UNSUPPORTED_EXT = 110;

		// Token: 0x020008CC RID: 2252
		public struct SCHANNEL_ALERT_TOKEN
		{
			// Token: 0x04003B5D RID: 15197
			public uint dwTokenType;

			// Token: 0x04003B5E RID: 15198
			public uint dwAlertType;

			// Token: 0x04003B5F RID: 15199
			public uint dwAlertNumber;
		}
	}
}
