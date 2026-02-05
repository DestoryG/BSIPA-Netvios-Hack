using System;

namespace Microsoft.Cci.Pdb
{
	// Token: 0x0200030C RID: 780
	internal class PdbSynchronizationInformation
	{
		// Token: 0x0600121B RID: 4635 RVA: 0x0003C87C File Offset: 0x0003AA7C
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

		// Token: 0x17000388 RID: 904
		// (get) Token: 0x0600121C RID: 4636 RVA: 0x0003C8D5 File Offset: 0x0003AAD5
		public uint GeneratedCatchHandlerOffset
		{
			get
			{
				return this.generatedCatchHandlerIlOffset;
			}
		}

		// Token: 0x04000F00 RID: 3840
		internal uint kickoffMethodToken;

		// Token: 0x04000F01 RID: 3841
		internal uint generatedCatchHandlerIlOffset;

		// Token: 0x04000F02 RID: 3842
		internal PdbSynchronizationPoint[] synchronizationPoints;
	}
}
