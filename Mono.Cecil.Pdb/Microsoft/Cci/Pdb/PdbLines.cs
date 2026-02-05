using System;

namespace Microsoft.Cci.Pdb
{
	// Token: 0x020000E1 RID: 225
	internal class PdbLines
	{
		// Token: 0x060001AC RID: 428 RVA: 0x00006724 File Offset: 0x00004924
		internal PdbLines(PdbSource file, uint count)
		{
			this.file = file;
			this.lines = new PdbLine[count];
		}

		// Token: 0x040004E8 RID: 1256
		internal PdbSource file;

		// Token: 0x040004E9 RID: 1257
		internal PdbLine[] lines;
	}
}
