using System;

namespace Microsoft.Cci.Pdb
{
	// Token: 0x020000A3 RID: 163
	internal struct ThunkSym32
	{
		// Token: 0x04000394 RID: 916
		internal uint parent;

		// Token: 0x04000395 RID: 917
		internal uint end;

		// Token: 0x04000396 RID: 918
		internal uint next;

		// Token: 0x04000397 RID: 919
		internal uint off;

		// Token: 0x04000398 RID: 920
		internal ushort seg;

		// Token: 0x04000399 RID: 921
		internal ushort len;

		// Token: 0x0400039A RID: 922
		internal byte ord;

		// Token: 0x0400039B RID: 923
		internal string name;

		// Token: 0x0400039C RID: 924
		internal byte[] variant;
	}
}
