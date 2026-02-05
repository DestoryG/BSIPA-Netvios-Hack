using System;
using System.Collections.Generic;

namespace Microsoft.Cci.Pdb
{
	// Token: 0x0200030E RID: 782
	internal class PdbInfo
	{
		// Token: 0x04000F06 RID: 3846
		public PdbFunction[] Functions;

		// Token: 0x04000F07 RID: 3847
		public Dictionary<uint, PdbTokenLine> TokenToSourceMapping;

		// Token: 0x04000F08 RID: 3848
		public string SourceServerData;

		// Token: 0x04000F09 RID: 3849
		public int Age;

		// Token: 0x04000F0A RID: 3850
		public Guid Guid;

		// Token: 0x04000F0B RID: 3851
		public byte[] SourceLinkData;
	}
}
