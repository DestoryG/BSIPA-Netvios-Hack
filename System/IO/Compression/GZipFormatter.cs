using System;

namespace System.IO.Compression
{
	// Token: 0x02000430 RID: 1072
	internal class GZipFormatter : IFileFormatWriter
	{
		// Token: 0x0600282A RID: 10282 RVA: 0x000B853F File Offset: 0x000B673F
		internal GZipFormatter()
			: this(3)
		{
		}

		// Token: 0x0600282B RID: 10283 RVA: 0x000B8548 File Offset: 0x000B6748
		internal GZipFormatter(int compressionLevel)
		{
			if (compressionLevel == 10)
			{
				this.headerBytes[8] = 2;
			}
		}

		// Token: 0x0600282C RID: 10284 RVA: 0x000B8576 File Offset: 0x000B6776
		public byte[] GetHeader()
		{
			return this.headerBytes;
		}

		// Token: 0x0600282D RID: 10285 RVA: 0x000B8580 File Offset: 0x000B6780
		public void UpdateWithBytesRead(byte[] buffer, int offset, int bytesToCopy)
		{
			this._crc32 = Crc32Helper.UpdateCrc32(this._crc32, buffer, offset, bytesToCopy);
			long num = this._inputStreamSizeModulo + (long)((ulong)bytesToCopy);
			if (num >= 4294967296L)
			{
				num %= 4294967296L;
			}
			this._inputStreamSizeModulo = num;
		}

		// Token: 0x0600282E RID: 10286 RVA: 0x000B85CC File Offset: 0x000B67CC
		public byte[] GetFooter()
		{
			byte[] array = new byte[8];
			this.WriteUInt32(array, this._crc32, 0);
			this.WriteUInt32(array, (uint)this._inputStreamSizeModulo, 4);
			return array;
		}

		// Token: 0x0600282F RID: 10287 RVA: 0x000B85FE File Offset: 0x000B67FE
		internal void WriteUInt32(byte[] b, uint value, int startIndex)
		{
			b[startIndex] = (byte)value;
			b[startIndex + 1] = (byte)(value >> 8);
			b[startIndex + 2] = (byte)(value >> 16);
			b[startIndex + 3] = (byte)(value >> 24);
		}

		// Token: 0x040021DB RID: 8667
		private byte[] headerBytes = new byte[] { 31, 139, 8, 0, 0, 0, 0, 0, 4, 0 };

		// Token: 0x040021DC RID: 8668
		private uint _crc32;

		// Token: 0x040021DD RID: 8669
		private long _inputStreamSizeModulo;
	}
}
