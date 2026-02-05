using System;

namespace Microsoft.Cci.Pdb
{
	// Token: 0x0200030F RID: 783
	internal struct PdbLine
	{
		// Token: 0x06001221 RID: 4641 RVA: 0x0003C919 File Offset: 0x0003AB19
		internal PdbLine(uint offset, uint lineBegin, ushort colBegin, uint lineEnd, ushort colEnd)
		{
			this.offset = offset;
			this.lineBegin = lineBegin;
			this.colBegin = colBegin;
			this.lineEnd = lineEnd;
			this.colEnd = colEnd;
		}

		// Token: 0x04000F0C RID: 3852
		internal uint offset;

		// Token: 0x04000F0D RID: 3853
		internal uint lineBegin;

		// Token: 0x04000F0E RID: 3854
		internal uint lineEnd;

		// Token: 0x04000F0F RID: 3855
		internal ushort colBegin;

		// Token: 0x04000F10 RID: 3856
		internal ushort colEnd;
	}
}
