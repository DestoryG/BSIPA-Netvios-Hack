using System;

namespace Mono.Cecil.Cil
{
	// Token: 0x020001DA RID: 474
	internal sealed class ImageDebugHeaderEntry
	{
		// Token: 0x1700030F RID: 783
		// (get) Token: 0x06000EB9 RID: 3769 RVA: 0x00032C8E File Offset: 0x00030E8E
		// (set) Token: 0x06000EBA RID: 3770 RVA: 0x00032C96 File Offset: 0x00030E96
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

		// Token: 0x17000310 RID: 784
		// (get) Token: 0x06000EBB RID: 3771 RVA: 0x00032C9F File Offset: 0x00030E9F
		public byte[] Data
		{
			get
			{
				return this.data;
			}
		}

		// Token: 0x06000EBC RID: 3772 RVA: 0x00032CA7 File Offset: 0x00030EA7
		public ImageDebugHeaderEntry(ImageDebugDirectory directory, byte[] data)
		{
			this.directory = directory;
			this.data = data ?? Empty<byte>.Array;
		}

		// Token: 0x040008FB RID: 2299
		private ImageDebugDirectory directory;

		// Token: 0x040008FC RID: 2300
		private readonly byte[] data;
	}
}
