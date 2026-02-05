using System;

namespace System.Net.WebSockets
{
	// Token: 0x02000232 RID: 562
	public enum WebSocketCloseStatus
	{
		// Token: 0x04001681 RID: 5761
		NormalClosure = 1000,
		// Token: 0x04001682 RID: 5762
		EndpointUnavailable,
		// Token: 0x04001683 RID: 5763
		ProtocolError,
		// Token: 0x04001684 RID: 5764
		InvalidMessageType,
		// Token: 0x04001685 RID: 5765
		Empty = 1005,
		// Token: 0x04001686 RID: 5766
		InvalidPayloadData = 1007,
		// Token: 0x04001687 RID: 5767
		PolicyViolation,
		// Token: 0x04001688 RID: 5768
		MessageTooBig,
		// Token: 0x04001689 RID: 5769
		MandatoryExtension,
		// Token: 0x0400168A RID: 5770
		InternalServerError
	}
}
