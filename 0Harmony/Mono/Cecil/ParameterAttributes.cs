using System;

namespace Mono.Cecil
{
	// Token: 0x02000158 RID: 344
	[Flags]
	internal enum ParameterAttributes : ushort
	{
		// Token: 0x0400042E RID: 1070
		None = 0,
		// Token: 0x0400042F RID: 1071
		In = 1,
		// Token: 0x04000430 RID: 1072
		Out = 2,
		// Token: 0x04000431 RID: 1073
		Lcid = 4,
		// Token: 0x04000432 RID: 1074
		Retval = 8,
		// Token: 0x04000433 RID: 1075
		Optional = 16,
		// Token: 0x04000434 RID: 1076
		HasDefault = 4096,
		// Token: 0x04000435 RID: 1077
		HasFieldMarshal = 8192,
		// Token: 0x04000436 RID: 1078
		Unused = 53216
	}
}
