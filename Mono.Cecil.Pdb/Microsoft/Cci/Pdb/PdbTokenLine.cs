using System;

namespace Microsoft.Cci.Pdb
{
	// Token: 0x020000E6 RID: 230
	internal class PdbTokenLine
	{
		// Token: 0x060001B5 RID: 437 RVA: 0x00006ACA File Offset: 0x00004CCA
		internal PdbTokenLine(uint token, uint file_id, uint line, uint column, uint endLine, uint endColumn)
		{
			this.token = token;
			this.file_id = file_id;
			this.line = line;
			this.column = column;
			this.endLine = endLine;
			this.endColumn = endColumn;
		}

		// Token: 0x040004FB RID: 1275
		internal uint token;

		// Token: 0x040004FC RID: 1276
		internal uint file_id;

		// Token: 0x040004FD RID: 1277
		internal uint line;

		// Token: 0x040004FE RID: 1278
		internal uint column;

		// Token: 0x040004FF RID: 1279
		internal uint endLine;

		// Token: 0x04000500 RID: 1280
		internal uint endColumn;

		// Token: 0x04000501 RID: 1281
		internal PdbSource sourceFile;

		// Token: 0x04000502 RID: 1282
		internal PdbTokenLine nextLine;
	}
}
