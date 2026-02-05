using System;

namespace System.Net
{
	// Token: 0x020000EA RID: 234
	public enum FtpStatusCode
	{
		// Token: 0x04000D5C RID: 3420
		Undefined,
		// Token: 0x04000D5D RID: 3421
		RestartMarker = 110,
		// Token: 0x04000D5E RID: 3422
		ServiceTemporarilyNotAvailable = 120,
		// Token: 0x04000D5F RID: 3423
		DataAlreadyOpen = 125,
		// Token: 0x04000D60 RID: 3424
		OpeningData = 150,
		// Token: 0x04000D61 RID: 3425
		CommandOK = 200,
		// Token: 0x04000D62 RID: 3426
		CommandExtraneous = 202,
		// Token: 0x04000D63 RID: 3427
		DirectoryStatus = 212,
		// Token: 0x04000D64 RID: 3428
		FileStatus,
		// Token: 0x04000D65 RID: 3429
		SystemType = 215,
		// Token: 0x04000D66 RID: 3430
		SendUserCommand = 220,
		// Token: 0x04000D67 RID: 3431
		ClosingControl,
		// Token: 0x04000D68 RID: 3432
		ClosingData = 226,
		// Token: 0x04000D69 RID: 3433
		EnteringPassive,
		// Token: 0x04000D6A RID: 3434
		LoggedInProceed = 230,
		// Token: 0x04000D6B RID: 3435
		ServerWantsSecureSession = 234,
		// Token: 0x04000D6C RID: 3436
		FileActionOK = 250,
		// Token: 0x04000D6D RID: 3437
		PathnameCreated = 257,
		// Token: 0x04000D6E RID: 3438
		SendPasswordCommand = 331,
		// Token: 0x04000D6F RID: 3439
		NeedLoginAccount,
		// Token: 0x04000D70 RID: 3440
		FileCommandPending = 350,
		// Token: 0x04000D71 RID: 3441
		ServiceNotAvailable = 421,
		// Token: 0x04000D72 RID: 3442
		CantOpenData = 425,
		// Token: 0x04000D73 RID: 3443
		ConnectionClosed,
		// Token: 0x04000D74 RID: 3444
		ActionNotTakenFileUnavailableOrBusy = 450,
		// Token: 0x04000D75 RID: 3445
		ActionAbortedLocalProcessingError,
		// Token: 0x04000D76 RID: 3446
		ActionNotTakenInsufficientSpace,
		// Token: 0x04000D77 RID: 3447
		CommandSyntaxError = 500,
		// Token: 0x04000D78 RID: 3448
		ArgumentSyntaxError,
		// Token: 0x04000D79 RID: 3449
		CommandNotImplemented,
		// Token: 0x04000D7A RID: 3450
		BadCommandSequence,
		// Token: 0x04000D7B RID: 3451
		NotLoggedIn = 530,
		// Token: 0x04000D7C RID: 3452
		AccountNeeded = 532,
		// Token: 0x04000D7D RID: 3453
		ActionNotTakenFileUnavailable = 550,
		// Token: 0x04000D7E RID: 3454
		ActionAbortedUnknownPageType,
		// Token: 0x04000D7F RID: 3455
		FileActionAborted,
		// Token: 0x04000D80 RID: 3456
		ActionNotTakenFilenameNotAllowed
	}
}
