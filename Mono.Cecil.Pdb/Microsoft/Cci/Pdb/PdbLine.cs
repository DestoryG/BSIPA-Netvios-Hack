using System;

namespace Microsoft.Cci.Pdb
{
	// Token: 0x020000E0 RID: 224
	internal struct PdbLine
	{
		// Token: 0x060001AB RID: 427 RVA: 0x000066FD File Offset: 0x000048FD
		internal PdbLine(uint offset, uint lineBegin, ushort colBegin, uint lineEnd, ushort colEnd)
		{
			this.offset = offset;
			this.lineBegin = lineBegin;
			this.colBegin = colBegin;
			this.lineEnd = lineEnd;
			this.colEnd = colEnd;
		}

		// Token: 0x040004E3 RID: 1251
		internal uint offset;

		// Token: 0x040004E4 RID: 1252
		internal uint lineBegin;

		// Token: 0x040004E5 RID: 1253
		internal uint lineEnd;

		// Token: 0x040004E6 RID: 1254
		internal ushort colBegin;

		// Token: 0x040004E7 RID: 1255
		internal ushort colEnd;
	}
}
