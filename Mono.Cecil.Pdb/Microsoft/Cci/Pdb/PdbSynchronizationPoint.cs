using System;

namespace Microsoft.Cci.Pdb
{
	// Token: 0x020000DF RID: 223
	internal class PdbSynchronizationPoint
	{
		// Token: 0x060001A8 RID: 424 RVA: 0x000066C1 File Offset: 0x000048C1
		internal PdbSynchronizationPoint(BitAccess bits)
		{
			bits.ReadUInt32(out this.synchronizeOffset);
			bits.ReadUInt32(out this.continuationMethodToken);
			bits.ReadUInt32(out this.continuationOffset);
		}

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x060001A9 RID: 425 RVA: 0x000066ED File Offset: 0x000048ED
		public uint SynchronizeOffset
		{
			get
			{
				return this.synchronizeOffset;
			}
		}

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x060001AA RID: 426 RVA: 0x000066F5 File Offset: 0x000048F5
		public uint ContinuationOffset
		{
			get
			{
				return this.continuationOffset;
			}
		}

		// Token: 0x040004E0 RID: 1248
		internal uint synchronizeOffset;

		// Token: 0x040004E1 RID: 1249
		internal uint continuationMethodToken;

		// Token: 0x040004E2 RID: 1250
		internal uint continuationOffset;
	}
}
