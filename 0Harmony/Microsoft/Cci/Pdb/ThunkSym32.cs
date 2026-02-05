using System;

namespace Microsoft.Cci.Pdb
{
	// Token: 0x020002CA RID: 714
	internal struct ThunkSym32
	{
		// Token: 0x04000DB0 RID: 3504
		internal uint parent;

		// Token: 0x04000DB1 RID: 3505
		internal uint end;

		// Token: 0x04000DB2 RID: 3506
		internal uint next;

		// Token: 0x04000DB3 RID: 3507
		internal uint off;

		// Token: 0x04000DB4 RID: 3508
		internal ushort seg;

		// Token: 0x04000DB5 RID: 3509
		internal ushort len;

		// Token: 0x04000DB6 RID: 3510
		internal byte ord;

		// Token: 0x04000DB7 RID: 3511
		internal string name;

		// Token: 0x04000DB8 RID: 3512
		internal byte[] variant;
	}
}
