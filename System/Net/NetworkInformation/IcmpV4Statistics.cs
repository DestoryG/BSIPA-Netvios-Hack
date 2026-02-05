using System;

namespace System.Net.NetworkInformation
{
	// Token: 0x0200029C RID: 668
	[global::__DynamicallyInvokable]
	public abstract class IcmpV4Statistics
	{
		// Token: 0x1700051B RID: 1307
		// (get) Token: 0x060018C2 RID: 6338
		[global::__DynamicallyInvokable]
		public abstract long AddressMaskRepliesReceived
		{
			[global::__DynamicallyInvokable]
			get;
		}

		// Token: 0x1700051C RID: 1308
		// (get) Token: 0x060018C3 RID: 6339
		[global::__DynamicallyInvokable]
		public abstract long AddressMaskRepliesSent
		{
			[global::__DynamicallyInvokable]
			get;
		}

		// Token: 0x1700051D RID: 1309
		// (get) Token: 0x060018C4 RID: 6340
		[global::__DynamicallyInvokable]
		public abstract long AddressMaskRequestsReceived
		{
			[global::__DynamicallyInvokable]
			get;
		}

		// Token: 0x1700051E RID: 1310
		// (get) Token: 0x060018C5 RID: 6341
		[global::__DynamicallyInvokable]
		public abstract long AddressMaskRequestsSent
		{
			[global::__DynamicallyInvokable]
			get;
		}

		// Token: 0x1700051F RID: 1311
		// (get) Token: 0x060018C6 RID: 6342
		[global::__DynamicallyInvokable]
		public abstract long DestinationUnreachableMessagesReceived
		{
			[global::__DynamicallyInvokable]
			get;
		}

		// Token: 0x17000520 RID: 1312
		// (get) Token: 0x060018C7 RID: 6343
		[global::__DynamicallyInvokable]
		public abstract long DestinationUnreachableMessagesSent
		{
			[global::__DynamicallyInvokable]
			get;
		}

		// Token: 0x17000521 RID: 1313
		// (get) Token: 0x060018C8 RID: 6344
		[global::__DynamicallyInvokable]
		public abstract long EchoRepliesReceived
		{
			[global::__DynamicallyInvokable]
			get;
		}

		// Token: 0x17000522 RID: 1314
		// (get) Token: 0x060018C9 RID: 6345
		[global::__DynamicallyInvokable]
		public abstract long EchoRepliesSent
		{
			[global::__DynamicallyInvokable]
			get;
		}

		// Token: 0x17000523 RID: 1315
		// (get) Token: 0x060018CA RID: 6346
		[global::__DynamicallyInvokable]
		public abstract long EchoRequestsReceived
		{
			[global::__DynamicallyInvokable]
			get;
		}

		// Token: 0x17000524 RID: 1316
		// (get) Token: 0x060018CB RID: 6347
		[global::__DynamicallyInvokable]
		public abstract long EchoRequestsSent
		{
			[global::__DynamicallyInvokable]
			get;
		}

		// Token: 0x17000525 RID: 1317
		// (get) Token: 0x060018CC RID: 6348
		[global::__DynamicallyInvokable]
		public abstract long ErrorsReceived
		{
			[global::__DynamicallyInvokable]
			get;
		}

		// Token: 0x17000526 RID: 1318
		// (get) Token: 0x060018CD RID: 6349
		[global::__DynamicallyInvokable]
		public abstract long ErrorsSent
		{
			[global::__DynamicallyInvokable]
			get;
		}

		// Token: 0x17000527 RID: 1319
		// (get) Token: 0x060018CE RID: 6350
		[global::__DynamicallyInvokable]
		public abstract long MessagesReceived
		{
			[global::__DynamicallyInvokable]
			get;
		}

		// Token: 0x17000528 RID: 1320
		// (get) Token: 0x060018CF RID: 6351
		[global::__DynamicallyInvokable]
		public abstract long MessagesSent
		{
			[global::__DynamicallyInvokable]
			get;
		}

		// Token: 0x17000529 RID: 1321
		// (get) Token: 0x060018D0 RID: 6352
		[global::__DynamicallyInvokable]
		public abstract long ParameterProblemsReceived
		{
			[global::__DynamicallyInvokable]
			get;
		}

		// Token: 0x1700052A RID: 1322
		// (get) Token: 0x060018D1 RID: 6353
		[global::__DynamicallyInvokable]
		public abstract long ParameterProblemsSent
		{
			[global::__DynamicallyInvokable]
			get;
		}

		// Token: 0x1700052B RID: 1323
		// (get) Token: 0x060018D2 RID: 6354
		[global::__DynamicallyInvokable]
		public abstract long RedirectsReceived
		{
			[global::__DynamicallyInvokable]
			get;
		}

		// Token: 0x1700052C RID: 1324
		// (get) Token: 0x060018D3 RID: 6355
		[global::__DynamicallyInvokable]
		public abstract long RedirectsSent
		{
			[global::__DynamicallyInvokable]
			get;
		}

		// Token: 0x1700052D RID: 1325
		// (get) Token: 0x060018D4 RID: 6356
		[global::__DynamicallyInvokable]
		public abstract long SourceQuenchesReceived
		{
			[global::__DynamicallyInvokable]
			get;
		}

		// Token: 0x1700052E RID: 1326
		// (get) Token: 0x060018D5 RID: 6357
		[global::__DynamicallyInvokable]
		public abstract long SourceQuenchesSent
		{
			[global::__DynamicallyInvokable]
			get;
		}

		// Token: 0x1700052F RID: 1327
		// (get) Token: 0x060018D6 RID: 6358
		[global::__DynamicallyInvokable]
		public abstract long TimeExceededMessagesReceived
		{
			[global::__DynamicallyInvokable]
			get;
		}

		// Token: 0x17000530 RID: 1328
		// (get) Token: 0x060018D7 RID: 6359
		[global::__DynamicallyInvokable]
		public abstract long TimeExceededMessagesSent
		{
			[global::__DynamicallyInvokable]
			get;
		}

		// Token: 0x17000531 RID: 1329
		// (get) Token: 0x060018D8 RID: 6360
		[global::__DynamicallyInvokable]
		public abstract long TimestampRepliesReceived
		{
			[global::__DynamicallyInvokable]
			get;
		}

		// Token: 0x17000532 RID: 1330
		// (get) Token: 0x060018D9 RID: 6361
		[global::__DynamicallyInvokable]
		public abstract long TimestampRepliesSent
		{
			[global::__DynamicallyInvokable]
			get;
		}

		// Token: 0x17000533 RID: 1331
		// (get) Token: 0x060018DA RID: 6362
		[global::__DynamicallyInvokable]
		public abstract long TimestampRequestsReceived
		{
			[global::__DynamicallyInvokable]
			get;
		}

		// Token: 0x17000534 RID: 1332
		// (get) Token: 0x060018DB RID: 6363
		[global::__DynamicallyInvokable]
		public abstract long TimestampRequestsSent
		{
			[global::__DynamicallyInvokable]
			get;
		}

		// Token: 0x060018DC RID: 6364 RVA: 0x0007DBB9 File Offset: 0x0007BDB9
		[global::__DynamicallyInvokable]
		protected IcmpV4Statistics()
		{
		}
	}
}
