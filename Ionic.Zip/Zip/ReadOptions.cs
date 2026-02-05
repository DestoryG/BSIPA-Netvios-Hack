using System;
using System.IO;
using System.Text;

namespace Ionic.Zip
{
	// Token: 0x0200003E RID: 62
	public class ReadOptions
	{
		// Token: 0x170000CE RID: 206
		// (get) Token: 0x0600030F RID: 783 RVA: 0x000113AA File Offset: 0x0000F5AA
		// (set) Token: 0x06000310 RID: 784 RVA: 0x000113B2 File Offset: 0x0000F5B2
		public EventHandler<ReadProgressEventArgs> ReadProgress { get; set; }

		// Token: 0x170000CF RID: 207
		// (get) Token: 0x06000311 RID: 785 RVA: 0x000113BB File Offset: 0x0000F5BB
		// (set) Token: 0x06000312 RID: 786 RVA: 0x000113C3 File Offset: 0x0000F5C3
		public TextWriter StatusMessageWriter { get; set; }

		// Token: 0x170000D0 RID: 208
		// (get) Token: 0x06000313 RID: 787 RVA: 0x000113CC File Offset: 0x0000F5CC
		// (set) Token: 0x06000314 RID: 788 RVA: 0x000113D4 File Offset: 0x0000F5D4
		public Encoding Encoding { get; set; }
	}
}
