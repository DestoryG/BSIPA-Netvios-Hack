using System;

namespace Microsoft.Cci.Pdb
{
	// Token: 0x02000315 RID: 789
	internal class PdbTokenLine
	{
		// Token: 0x0600122D RID: 4653 RVA: 0x0003CD3A File Offset: 0x0003AF3A
		internal PdbTokenLine(uint token, uint file_id, uint line, uint column, uint endLine, uint endColumn)
		{
			this.token = token;
			this.file_id = file_id;
			this.line = line;
			this.column = column;
			this.endLine = endLine;
			this.endColumn = endColumn;
		}

		// Token: 0x04000F26 RID: 3878
		internal uint token;

		// Token: 0x04000F27 RID: 3879
		internal uint file_id;

		// Token: 0x04000F28 RID: 3880
		internal uint line;

		// Token: 0x04000F29 RID: 3881
		internal uint column;

		// Token: 0x04000F2A RID: 3882
		internal uint endLine;

		// Token: 0x04000F2B RID: 3883
		internal uint endColumn;

		// Token: 0x04000F2C RID: 3884
		internal PdbSource sourceFile;

		// Token: 0x04000F2D RID: 3885
		internal PdbTokenLine nextLine;
	}
}
