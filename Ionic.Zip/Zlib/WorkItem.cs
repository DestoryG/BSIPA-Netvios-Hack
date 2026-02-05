using System;

namespace Ionic.Zlib
{
	// Token: 0x0200005D RID: 93
	internal class WorkItem
	{
		// Token: 0x06000452 RID: 1106 RVA: 0x0001EBCC File Offset: 0x0001CDCC
		public WorkItem(int size, CompressionLevel compressLevel, CompressionStrategy strategy, int ix)
		{
			this.buffer = new byte[size];
			int num = size + (size / 32768 + 1) * 5 * 2;
			this.compressed = new byte[num];
			this.compressor = new ZlibCodec();
			this.compressor.InitializeDeflate(compressLevel, false);
			this.compressor.OutputBuffer = this.compressed;
			this.compressor.InputBuffer = this.buffer;
			this.index = ix;
		}

		// Token: 0x04000328 RID: 808
		public byte[] buffer;

		// Token: 0x04000329 RID: 809
		public byte[] compressed;

		// Token: 0x0400032A RID: 810
		public int crc;

		// Token: 0x0400032B RID: 811
		public int index;

		// Token: 0x0400032C RID: 812
		public int ordinal;

		// Token: 0x0400032D RID: 813
		public int inputBytesAvailable;

		// Token: 0x0400032E RID: 814
		public int compressedBytesAvailable;

		// Token: 0x0400032F RID: 815
		public ZlibCodec compressor;
	}
}
