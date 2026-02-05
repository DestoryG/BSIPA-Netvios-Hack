using System;
using System.IO;
using System.Text;

namespace NSpeex
{
	// Token: 0x02000009 RID: 9
	public class OggSpeexWriter : AudioFileWriter
	{
		// Token: 0x06000023 RID: 35 RVA: 0x00002D6C File Offset: 0x00000F6C
		public OggSpeexWriter(int mode, int sampleRate, int channels, int nframes, bool vbr)
		{
			this.streamSerialNumber = new Random().Next();
			this.dataBuffer = new byte[65565];
			this.dataBufferPtr = 0;
			this.headerBuffer = new byte[255];
			this.headerBufferPtr = 0;
			this.pageCount = 0;
			this.packetCount = 0;
			this.granulepos = 0L;
			this.mode = mode;
			this.sampleRate = sampleRate;
			this.channels = channels;
			this.nframes = nframes;
			this.vbr = vbr;
		}

		// Token: 0x17000003 RID: 3
		// (set) Token: 0x06000024 RID: 36 RVA: 0x00002DF8 File Offset: 0x00000FF8
		public int SerialNumber
		{
			set
			{
				this.streamSerialNumber = value;
			}
		}

		// Token: 0x06000025 RID: 37 RVA: 0x00002E01 File Offset: 0x00001001
		public override void Close()
		{
			this.Flush(true);
			this.xout.Close();
		}

		// Token: 0x06000026 RID: 38 RVA: 0x00002E15 File Offset: 0x00001015
		public override void Open(Stream stream)
		{
			this.xout = new BinaryWriter(stream);
		}

		// Token: 0x06000027 RID: 39 RVA: 0x00002E24 File Offset: 0x00001024
		private static int WriteOggPageHeader(BinaryWriter buf, int headerType, long granulepos, int streamSerialNumber, int pageCount, int packetCount, byte[] packetSizes)
		{
			buf.Write(Encoding.UTF8.GetBytes("OggS"));
			buf.Write(0);
			buf.Write((byte)headerType);
			buf.Write(granulepos);
			buf.Write(streamSerialNumber);
			buf.Write(pageCount);
			buf.Write(0);
			buf.Write((byte)packetCount);
			buf.Write(packetSizes, 0, packetCount);
			return packetCount + 27;
		}

		// Token: 0x06000028 RID: 40 RVA: 0x00002E8C File Offset: 0x0000108C
		private static byte[] BuildOggPageHeader(int headerType, long granulepos, int streamSerialNumber, int pageCount, int packetCount, byte[] packetSizes)
		{
			byte[] array = new byte[packetCount + 27];
			OggSpeexWriter.WriteOggPageHeader(new BinaryWriter(new MemoryStream(array)), headerType, granulepos, streamSerialNumber, pageCount, packetCount, packetSizes);
			return array;
		}

		// Token: 0x06000029 RID: 41 RVA: 0x00002EC0 File Offset: 0x000010C0
		public override void WriteHeader(string comment)
		{
			OggCrc oggCrc = new OggCrc();
			byte[] array = OggSpeexWriter.BuildOggPageHeader(2, 0L, this.streamSerialNumber, this.pageCount++, 1, new byte[] { 80 });
			byte[] array2 = AudioFileWriter.BuildSpeexHeader(this.sampleRate, this.mode, this.channels, this.vbr, this.nframes);
			oggCrc.Initialize();
			oggCrc.TransformBlock(array, 0, array.Length, array, 0);
			oggCrc.TransformFinalBlock(array2, 0, array2.Length);
			this.xout.Write(array, 0, 22);
			this.xout.Write(oggCrc.Hash, 0, oggCrc.HashSize / 8);
			this.xout.Write(array, 26, array.Length - 26);
			this.xout.Write(array2, 0, array2.Length);
			array = OggSpeexWriter.BuildOggPageHeader(0, 0L, this.streamSerialNumber, this.pageCount++, 1, new byte[] { (byte)(comment.Length + 8) });
			array2 = AudioFileWriter.BuildSpeexComment(comment);
			oggCrc.Initialize();
			oggCrc.TransformBlock(array, 0, array.Length, array, 0);
			oggCrc.TransformFinalBlock(array2, 0, array2.Length);
			this.xout.Write(array, 0, 22);
			this.xout.Write(oggCrc.Hash, 0, oggCrc.HashSize / 8);
			this.xout.Write(array, 26, array.Length - 26);
			this.xout.Write(array2, 0, array2.Length);
		}

		// Token: 0x0600002A RID: 42 RVA: 0x00003044 File Offset: 0x00001244
		public override void WritePacket(byte[] data, int offset, int len)
		{
			if (len <= 0)
			{
				return;
			}
			if (this.packetCount > 250)
			{
				this.Flush(false);
			}
			Array.Copy(data, offset, this.dataBuffer, this.dataBufferPtr, len);
			this.dataBufferPtr += len;
			this.headerBuffer[this.headerBufferPtr++] = (byte)len;
			this.packetCount++;
			this.granulepos += (long)(this.nframes * ((this.mode == 2) ? 640 : ((this.mode == 1) ? 320 : 160)));
		}

		// Token: 0x0600002B RID: 43 RVA: 0x000030F0 File Offset: 0x000012F0
		private void Flush(bool eos)
		{
			OggCrc oggCrc = new OggCrc();
			byte[] array = OggSpeexWriter.BuildOggPageHeader(eos ? 4 : 0, this.granulepos, this.streamSerialNumber, this.pageCount++, this.packetCount, this.headerBuffer);
			oggCrc.Initialize();
			oggCrc.TransformBlock(array, 0, array.Length, array, 0);
			oggCrc.TransformFinalBlock(this.dataBuffer, 0, this.dataBufferPtr);
			this.xout.Write(array, 0, 22);
			this.xout.Write(oggCrc.Hash, 0, oggCrc.HashSize / 8);
			this.xout.Write(array, 26, array.Length - 26);
			this.xout.Write(this.dataBuffer, 0, this.dataBufferPtr);
			this.dataBufferPtr = 0;
			this.headerBufferPtr = 0;
			this.packetCount = 0;
		}

		// Token: 0x04000007 RID: 7
		public const int PACKETS_PER_OGG_PAGE = 250;

		// Token: 0x04000008 RID: 8
		private BinaryWriter xout;

		// Token: 0x04000009 RID: 9
		private readonly int mode;

		// Token: 0x0400000A RID: 10
		private readonly int sampleRate;

		// Token: 0x0400000B RID: 11
		private readonly int channels;

		// Token: 0x0400000C RID: 12
		private readonly int nframes;

		// Token: 0x0400000D RID: 13
		private readonly bool vbr;

		// Token: 0x0400000E RID: 14
		private int streamSerialNumber;

		// Token: 0x0400000F RID: 15
		private byte[] dataBuffer;

		// Token: 0x04000010 RID: 16
		private int dataBufferPtr;

		// Token: 0x04000011 RID: 17
		private byte[] headerBuffer;

		// Token: 0x04000012 RID: 18
		private int headerBufferPtr;

		// Token: 0x04000013 RID: 19
		private int pageCount;

		// Token: 0x04000014 RID: 20
		private int packetCount;

		// Token: 0x04000015 RID: 21
		private long granulepos;
	}
}
