using System;

namespace Microsoft.Cci.Pdb
{
	// Token: 0x020002D5 RID: 725
	internal struct ProcSymMips
	{
		// Token: 0x04000DE8 RID: 3560
		internal uint parent;

		// Token: 0x04000DE9 RID: 3561
		internal uint end;

		// Token: 0x04000DEA RID: 3562
		internal uint next;

		// Token: 0x04000DEB RID: 3563
		internal uint len;

		// Token: 0x04000DEC RID: 3564
		internal uint dbgStart;

		// Token: 0x04000DED RID: 3565
		internal uint dbgEnd;

		// Token: 0x04000DEE RID: 3566
		internal uint regSave;

		// Token: 0x04000DEF RID: 3567
		internal uint fpSave;

		// Token: 0x04000DF0 RID: 3568
		internal uint intOff;

		// Token: 0x04000DF1 RID: 3569
		internal uint fpOff;

		// Token: 0x04000DF2 RID: 3570
		internal uint typind;

		// Token: 0x04000DF3 RID: 3571
		internal uint off;

		// Token: 0x04000DF4 RID: 3572
		internal ushort seg;

		// Token: 0x04000DF5 RID: 3573
		internal byte retReg;

		// Token: 0x04000DF6 RID: 3574
		internal byte frameReg;

		// Token: 0x04000DF7 RID: 3575
		internal string name;
	}
}
