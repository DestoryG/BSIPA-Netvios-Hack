using System;

namespace System.Net
{
	// Token: 0x0200017F RID: 383
	[global::__DynamicallyInvokable]
	public enum WebExceptionStatus
	{
		// Token: 0x04001229 RID: 4649
		[global::__DynamicallyInvokable]
		Success,
		// Token: 0x0400122A RID: 4650
		NameResolutionFailure,
		// Token: 0x0400122B RID: 4651
		[global::__DynamicallyInvokable]
		ConnectFailure,
		// Token: 0x0400122C RID: 4652
		ReceiveFailure,
		// Token: 0x0400122D RID: 4653
		[global::__DynamicallyInvokable]
		SendFailure,
		// Token: 0x0400122E RID: 4654
		PipelineFailure,
		// Token: 0x0400122F RID: 4655
		[global::__DynamicallyInvokable]
		RequestCanceled,
		// Token: 0x04001230 RID: 4656
		ProtocolError,
		// Token: 0x04001231 RID: 4657
		ConnectionClosed,
		// Token: 0x04001232 RID: 4658
		TrustFailure,
		// Token: 0x04001233 RID: 4659
		SecureChannelFailure,
		// Token: 0x04001234 RID: 4660
		ServerProtocolViolation,
		// Token: 0x04001235 RID: 4661
		KeepAliveFailure,
		// Token: 0x04001236 RID: 4662
		[global::__DynamicallyInvokable]
		Pending,
		// Token: 0x04001237 RID: 4663
		Timeout,
		// Token: 0x04001238 RID: 4664
		ProxyNameResolutionFailure,
		// Token: 0x04001239 RID: 4665
		[global::__DynamicallyInvokable]
		UnknownError,
		// Token: 0x0400123A RID: 4666
		[global::__DynamicallyInvokable]
		MessageLengthLimitExceeded,
		// Token: 0x0400123B RID: 4667
		CacheEntryNotFound,
		// Token: 0x0400123C RID: 4668
		RequestProhibitedByCachePolicy,
		// Token: 0x0400123D RID: 4669
		RequestProhibitedByProxy
	}
}
