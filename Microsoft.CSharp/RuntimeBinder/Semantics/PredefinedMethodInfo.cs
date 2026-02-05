using System;
using Microsoft.CSharp.RuntimeBinder.Syntax;

namespace Microsoft.CSharp.RuntimeBinder.Semantics
{
	// Token: 0x02000059 RID: 89
	internal sealed class PredefinedMethodInfo
	{
		// Token: 0x06000306 RID: 774 RVA: 0x00014DD1 File Offset: 0x00012FD1
		public PredefinedMethodInfo(PREDEFMETH method, PredefinedType type, PredefinedName name, MethodCallingConventionEnum callingConvention, ACCESS access, int cTypeVars, int[] signature)
		{
			this.method = method;
			this.type = type;
			this.name = name;
			this.callingConvention = callingConvention;
			this.access = access;
			this.cTypeVars = cTypeVars;
			this.signature = signature;
		}

		// Token: 0x04000466 RID: 1126
		public PREDEFMETH method;

		// Token: 0x04000467 RID: 1127
		public PredefinedType type;

		// Token: 0x04000468 RID: 1128
		public PredefinedName name;

		// Token: 0x04000469 RID: 1129
		public MethodCallingConventionEnum callingConvention;

		// Token: 0x0400046A RID: 1130
		public ACCESS access;

		// Token: 0x0400046B RID: 1131
		public int cTypeVars;

		// Token: 0x0400046C RID: 1132
		public int[] signature;
	}
}
