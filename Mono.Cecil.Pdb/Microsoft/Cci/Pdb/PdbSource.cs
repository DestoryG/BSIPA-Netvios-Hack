using System;

namespace Microsoft.Cci.Pdb
{
	// Token: 0x020000E5 RID: 229
	internal class PdbSource
	{
		// Token: 0x060001B4 RID: 436 RVA: 0x00006AA5 File Offset: 0x00004CA5
		internal PdbSource(string name, Guid doctype, Guid language, Guid vendor)
		{
			this.name = name;
			this.doctype = doctype;
			this.language = language;
			this.vendor = vendor;
		}

		// Token: 0x040004F7 RID: 1271
		internal string name;

		// Token: 0x040004F8 RID: 1272
		internal Guid doctype;

		// Token: 0x040004F9 RID: 1273
		internal Guid language;

		// Token: 0x040004FA RID: 1274
		internal Guid vendor;
	}
}
