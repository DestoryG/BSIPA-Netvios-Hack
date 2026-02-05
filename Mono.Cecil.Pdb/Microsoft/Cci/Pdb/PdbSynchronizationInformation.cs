using System;

namespace Microsoft.Cci.Pdb
{
	// Token: 0x020000DE RID: 222
	internal class PdbSynchronizationInformation
	{
		// Token: 0x060001A6 RID: 422 RVA: 0x00006660 File Offset: 0x00004860
		internal PdbSynchronizationInformation(BitAccess bits)
		{
			bits.ReadUInt32(out this.kickoffMethodToken);
			bits.ReadUInt32(out this.generatedCatchHandlerIlOffset);
			uint num;
			bits.ReadUInt32(out num);
			this.synchronizationPoints = new PdbSynchronizationPoint[num];
			for (uint num2 = 0U; num2 < num; num2 += 1U)
			{
				this.synchronizationPoints[(int)num2] = new PdbSynchronizationPoint(bits);
			}
		}

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x060001A7 RID: 423 RVA: 0x000066B9 File Offset: 0x000048B9
		public uint GeneratedCatchHandlerOffset
		{
			get
			{
				return this.generatedCatchHandlerIlOffset;
			}
		}

		// Token: 0x040004DD RID: 1245
		internal uint kickoffMethodToken;

		// Token: 0x040004DE RID: 1246
		internal uint generatedCatchHandlerIlOffset;

		// Token: 0x040004DF RID: 1247
		internal PdbSynchronizationPoint[] synchronizationPoints;
	}
}
