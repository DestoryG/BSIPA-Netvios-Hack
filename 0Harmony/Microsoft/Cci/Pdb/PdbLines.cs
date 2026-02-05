using System;

namespace Microsoft.Cci.Pdb
{
	// Token: 0x02000310 RID: 784
	internal class PdbLines
	{
		// Token: 0x06001222 RID: 4642 RVA: 0x0003C940 File Offset: 0x0003AB40
		internal PdbLines(PdbSource file, uint count)
		{
			this.file = file;
			this.lines = new PdbLine[count];
		}

		// Token: 0x04000F11 RID: 3857
		internal PdbSource file;

		// Token: 0x04000F12 RID: 3858
		internal PdbLine[] lines;
	}
}
