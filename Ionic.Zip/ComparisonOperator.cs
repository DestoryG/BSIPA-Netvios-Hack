using System;
using System.ComponentModel;

namespace Ionic
{
	// Token: 0x0200000F RID: 15
	internal enum ComparisonOperator
	{
		// Token: 0x04000065 RID: 101
		[Description(">")]
		GreaterThan,
		// Token: 0x04000066 RID: 102
		[Description(">=")]
		GreaterThanOrEqualTo,
		// Token: 0x04000067 RID: 103
		[Description("<")]
		LesserThan,
		// Token: 0x04000068 RID: 104
		[Description("<=")]
		LesserThanOrEqualTo,
		// Token: 0x04000069 RID: 105
		[Description("=")]
		EqualTo,
		// Token: 0x0400006A RID: 106
		[Description("!=")]
		NotEqualTo
	}
}
