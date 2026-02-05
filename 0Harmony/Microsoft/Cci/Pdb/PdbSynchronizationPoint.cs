using System;

namespace Microsoft.Cci.Pdb
{
	// Token: 0x0200030D RID: 781
	internal class PdbSynchronizationPoint
	{
		// Token: 0x0600121D RID: 4637 RVA: 0x0003C8DD File Offset: 0x0003AADD
		internal PdbSynchronizationPoint(BitAccess bits)
		{
			bits.ReadUInt32(out this.synchronizeOffset);
			bits.ReadUInt32(out this.continuationMethodToken);
			bits.ReadUInt32(out this.continuationOffset);
		}

		// Token: 0x17000389 RID: 905
		// (get) Token: 0x0600121E RID: 4638 RVA: 0x0003C909 File Offset: 0x0003AB09
		public uint SynchronizeOffset
		{
			get
			{
				return this.synchronizeOffset;
			}
		}

		// Token: 0x1700038A RID: 906
		// (get) Token: 0x0600121F RID: 4639 RVA: 0x0003C911 File Offset: 0x0003AB11
		public uint ContinuationOffset
		{
			get
			{
				return this.continuationOffset;
			}
		}

		// Token: 0x04000F03 RID: 3843
		internal uint synchronizeOffset;

		// Token: 0x04000F04 RID: 3844
		internal uint continuationMethodToken;

		// Token: 0x04000F05 RID: 3845
		internal uint continuationOffset;
	}
}
