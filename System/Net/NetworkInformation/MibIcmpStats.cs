using System;

namespace System.Net.NetworkInformation
{
	// Token: 0x020002C7 RID: 711
	internal struct MibIcmpStats
	{
		// Token: 0x040019E2 RID: 6626
		internal uint messages;

		// Token: 0x040019E3 RID: 6627
		internal uint errors;

		// Token: 0x040019E4 RID: 6628
		internal uint destinationUnreachables;

		// Token: 0x040019E5 RID: 6629
		internal uint timeExceeds;

		// Token: 0x040019E6 RID: 6630
		internal uint parameterProblems;

		// Token: 0x040019E7 RID: 6631
		internal uint sourceQuenches;

		// Token: 0x040019E8 RID: 6632
		internal uint redirects;

		// Token: 0x040019E9 RID: 6633
		internal uint echoRequests;

		// Token: 0x040019EA RID: 6634
		internal uint echoReplies;

		// Token: 0x040019EB RID: 6635
		internal uint timestampRequests;

		// Token: 0x040019EC RID: 6636
		internal uint timestampReplies;

		// Token: 0x040019ED RID: 6637
		internal uint addressMaskRequests;

		// Token: 0x040019EE RID: 6638
		internal uint addressMaskReplies;
	}
}
