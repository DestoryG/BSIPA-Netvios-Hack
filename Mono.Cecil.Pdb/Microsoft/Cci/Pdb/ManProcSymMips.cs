using System;

namespace Microsoft.Cci.Pdb
{
	// Token: 0x020000A2 RID: 162
	internal struct ManProcSymMips
	{
		// Token: 0x04000384 RID: 900
		internal uint parent;

		// Token: 0x04000385 RID: 901
		internal uint end;

		// Token: 0x04000386 RID: 902
		internal uint next;

		// Token: 0x04000387 RID: 903
		internal uint len;

		// Token: 0x04000388 RID: 904
		internal uint dbgStart;

		// Token: 0x04000389 RID: 905
		internal uint dbgEnd;

		// Token: 0x0400038A RID: 906
		internal uint regSave;

		// Token: 0x0400038B RID: 907
		internal uint fpSave;

		// Token: 0x0400038C RID: 908
		internal uint intOff;

		// Token: 0x0400038D RID: 909
		internal uint fpOff;

		// Token: 0x0400038E RID: 910
		internal uint token;

		// Token: 0x0400038F RID: 911
		internal uint off;

		// Token: 0x04000390 RID: 912
		internal ushort seg;

		// Token: 0x04000391 RID: 913
		internal byte retReg;

		// Token: 0x04000392 RID: 914
		internal byte frameReg;

		// Token: 0x04000393 RID: 915
		internal string name;
	}
}
