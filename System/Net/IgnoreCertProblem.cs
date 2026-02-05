using System;

namespace System.Net
{
	// Token: 0x02000124 RID: 292
	internal enum IgnoreCertProblem
	{
		// Token: 0x04000FE4 RID: 4068
		not_time_valid = 1,
		// Token: 0x04000FE5 RID: 4069
		ctl_not_time_valid,
		// Token: 0x04000FE6 RID: 4070
		not_time_nested = 4,
		// Token: 0x04000FE7 RID: 4071
		invalid_basic_constraints = 8,
		// Token: 0x04000FE8 RID: 4072
		all_not_time_valid = 7,
		// Token: 0x04000FE9 RID: 4073
		allow_unknown_ca = 16,
		// Token: 0x04000FEA RID: 4074
		wrong_usage = 32,
		// Token: 0x04000FEB RID: 4075
		invalid_name = 64,
		// Token: 0x04000FEC RID: 4076
		invalid_policy = 128,
		// Token: 0x04000FED RID: 4077
		end_rev_unknown = 256,
		// Token: 0x04000FEE RID: 4078
		ctl_signer_rev_unknown = 512,
		// Token: 0x04000FEF RID: 4079
		ca_rev_unknown = 1024,
		// Token: 0x04000FF0 RID: 4080
		root_rev_unknown = 2048,
		// Token: 0x04000FF1 RID: 4081
		all_rev_unknown = 3840,
		// Token: 0x04000FF2 RID: 4082
		none = 4095
	}
}
