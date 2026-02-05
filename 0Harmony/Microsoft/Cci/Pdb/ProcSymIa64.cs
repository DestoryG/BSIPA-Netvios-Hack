using System;

namespace Microsoft.Cci.Pdb
{
	// Token: 0x020002D6 RID: 726
	internal struct ProcSymIa64
	{
		// Token: 0x04000DF8 RID: 3576
		internal uint parent;

		// Token: 0x04000DF9 RID: 3577
		internal uint end;

		// Token: 0x04000DFA RID: 3578
		internal uint next;

		// Token: 0x04000DFB RID: 3579
		internal uint len;

		// Token: 0x04000DFC RID: 3580
		internal uint dbgStart;

		// Token: 0x04000DFD RID: 3581
		internal uint dbgEnd;

		// Token: 0x04000DFE RID: 3582
		internal uint typind;

		// Token: 0x04000DFF RID: 3583
		internal uint off;

		// Token: 0x04000E00 RID: 3584
		internal ushort seg;

		// Token: 0x04000E01 RID: 3585
		internal ushort retReg;

		// Token: 0x04000E02 RID: 3586
		internal byte flags;

		// Token: 0x04000E03 RID: 3587
		internal string name;
	}
}
