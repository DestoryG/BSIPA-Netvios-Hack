using System;

namespace Microsoft.Cci.Pdb
{
	// Token: 0x020000C3 RID: 195
	internal struct DiscardedSym
	{
		// Token: 0x04000447 RID: 1095
		internal CV_DISCARDED iscarded;

		// Token: 0x04000448 RID: 1096
		internal uint fileid;

		// Token: 0x04000449 RID: 1097
		internal uint linenum;

		// Token: 0x0400044A RID: 1098
		internal byte[] data;
	}
}
