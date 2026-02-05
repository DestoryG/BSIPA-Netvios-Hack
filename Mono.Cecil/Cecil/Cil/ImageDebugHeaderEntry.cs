using System;

namespace Mono.Cecil.Cil
{
	// Token: 0x02000116 RID: 278
	public sealed class ImageDebugHeaderEntry
	{
		// Token: 0x170002EF RID: 751
		// (get) Token: 0x06000AD2 RID: 2770 RVA: 0x00023A82 File Offset: 0x00021C82
		// (set) Token: 0x06000AD3 RID: 2771 RVA: 0x00023A8A File Offset: 0x00021C8A
		public ImageDebugDirectory Directory
		{
			get
			{
				return this.directory;
			}
			internal set
			{
				this.directory = value;
			}
		}

		// Token: 0x170002F0 RID: 752
		// (get) Token: 0x06000AD4 RID: 2772 RVA: 0x00023A93 File Offset: 0x00021C93
		public byte[] Data
		{
			get
			{
				return this.data;
			}
		}

		// Token: 0x06000AD5 RID: 2773 RVA: 0x00023A9B File Offset: 0x00021C9B
		public ImageDebugHeaderEntry(ImageDebugDirectory directory, byte[] data)
		{
			this.directory = directory;
			this.data = data ?? Empty<byte>.Array;
		}

		// Token: 0x0400069C RID: 1692
		private ImageDebugDirectory directory;

		// Token: 0x0400069D RID: 1693
		private readonly byte[] data;
	}
}
