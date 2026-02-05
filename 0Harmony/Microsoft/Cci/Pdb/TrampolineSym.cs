using System;

namespace Microsoft.Cci.Pdb
{
	// Token: 0x020002CC RID: 716
	internal struct TrampolineSym
	{
		// Token: 0x04000DBC RID: 3516
		internal ushort trampType;

		// Token: 0x04000DBD RID: 3517
		internal ushort cbThunk;

		// Token: 0x04000DBE RID: 3518
		internal uint offThunk;

		// Token: 0x04000DBF RID: 3519
		internal uint offTarget;

		// Token: 0x04000DC0 RID: 3520
		internal ushort sectThunk;

		// Token: 0x04000DC1 RID: 3521
		internal ushort sectTarget;
	}
}
