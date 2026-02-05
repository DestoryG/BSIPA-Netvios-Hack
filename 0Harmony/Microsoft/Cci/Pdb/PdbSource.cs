using System;

namespace Microsoft.Cci.Pdb
{
	// Token: 0x02000314 RID: 788
	internal class PdbSource
	{
		// Token: 0x0600122C RID: 4652 RVA: 0x0003CD05 File Offset: 0x0003AF05
		internal PdbSource(string name, Guid doctype, Guid language, Guid vendor, Guid checksumAlgorithm, byte[] checksum)
		{
			this.name = name;
			this.doctype = doctype;
			this.language = language;
			this.vendor = vendor;
			this.checksumAlgorithm = checksumAlgorithm;
			this.checksum = checksum;
		}

		// Token: 0x04000F20 RID: 3872
		internal string name;

		// Token: 0x04000F21 RID: 3873
		internal Guid doctype;

		// Token: 0x04000F22 RID: 3874
		internal Guid language;

		// Token: 0x04000F23 RID: 3875
		internal Guid vendor;

		// Token: 0x04000F24 RID: 3876
		internal Guid checksumAlgorithm;

		// Token: 0x04000F25 RID: 3877
		internal byte[] checksum;
	}
}
