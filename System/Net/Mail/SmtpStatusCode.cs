using System;

namespace System.Net.Mail
{
	// Token: 0x02000295 RID: 661
	public enum SmtpStatusCode
	{
		// Token: 0x04001865 RID: 6245
		SystemStatus = 211,
		// Token: 0x04001866 RID: 6246
		HelpMessage = 214,
		// Token: 0x04001867 RID: 6247
		ServiceReady = 220,
		// Token: 0x04001868 RID: 6248
		ServiceClosingTransmissionChannel,
		// Token: 0x04001869 RID: 6249
		Ok = 250,
		// Token: 0x0400186A RID: 6250
		UserNotLocalWillForward,
		// Token: 0x0400186B RID: 6251
		CannotVerifyUserWillAttemptDelivery,
		// Token: 0x0400186C RID: 6252
		StartMailInput = 354,
		// Token: 0x0400186D RID: 6253
		ServiceNotAvailable = 421,
		// Token: 0x0400186E RID: 6254
		MailboxBusy = 450,
		// Token: 0x0400186F RID: 6255
		LocalErrorInProcessing,
		// Token: 0x04001870 RID: 6256
		InsufficientStorage,
		// Token: 0x04001871 RID: 6257
		ClientNotPermitted = 454,
		// Token: 0x04001872 RID: 6258
		CommandUnrecognized = 500,
		// Token: 0x04001873 RID: 6259
		SyntaxError,
		// Token: 0x04001874 RID: 6260
		CommandNotImplemented,
		// Token: 0x04001875 RID: 6261
		BadCommandSequence,
		// Token: 0x04001876 RID: 6262
		MustIssueStartTlsFirst = 530,
		// Token: 0x04001877 RID: 6263
		CommandParameterNotImplemented = 504,
		// Token: 0x04001878 RID: 6264
		MailboxUnavailable = 550,
		// Token: 0x04001879 RID: 6265
		UserNotLocalTryAlternatePath,
		// Token: 0x0400187A RID: 6266
		ExceededStorageAllocation,
		// Token: 0x0400187B RID: 6267
		MailboxNameNotAllowed,
		// Token: 0x0400187C RID: 6268
		TransactionFailed,
		// Token: 0x0400187D RID: 6269
		GeneralFailure = -1
	}
}
