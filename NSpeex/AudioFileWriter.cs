using System;
using System.IO;
using System.Text;

namespace NSpeex
{
	// Token: 0x02000008 RID: 8
	public abstract class AudioFileWriter
	{
		// Token: 0x06000019 RID: 25
		public abstract void Close();

		// Token: 0x0600001A RID: 26
		public abstract void Open(Stream stream);

		// Token: 0x0600001B RID: 27 RVA: 0x00002C01 File Offset: 0x00000E01
		public void Open(string filename)
		{
			this.Open(new FileStream(filename, FileMode.Create));
		}

		// Token: 0x0600001C RID: 28
		public abstract void WriteHeader(string comment);

		// Token: 0x0600001D RID: 29
		public abstract void WritePacket(byte[] data, int offset, int len);

		// Token: 0x0600001E RID: 30 RVA: 0x00002C10 File Offset: 0x00000E10
		protected static int WriteSpeexHeader(BinaryWriter buf, int sampleRate, int mode, int channels, bool vbr, int nframes)
		{
			buf.Write(Encoding.UTF8.GetBytes("Speex   "));
			buf.Write(Encoding.UTF8.GetBytes("speex-1.0"));
			for (int i = 0; i < 11; i++)
			{
				buf.Write(0);
			}
			buf.Write(1);
			buf.Write(80);
			buf.Write(sampleRate);
			buf.Write(mode);
			buf.Write(4);
			buf.Write(channels);
			buf.Write(-1);
			buf.Write(160 << mode);
			buf.Write(vbr ? 1 : 0);
			buf.Write(nframes);
			buf.Write(0);
			buf.Write(0);
			buf.Write(0);
			return 80;
		}

		// Token: 0x0600001F RID: 31 RVA: 0x00002CCC File Offset: 0x00000ECC
		protected static byte[] BuildSpeexHeader(int sampleRate, int mode, int channels, bool vbr, int nframes)
		{
			byte[] array = new byte[80];
			AudioFileWriter.WriteSpeexHeader(new BinaryWriter(new MemoryStream(array)), sampleRate, mode, channels, vbr, nframes);
			return array;
		}

		// Token: 0x06000020 RID: 32 RVA: 0x00002CFC File Offset: 0x00000EFC
		protected static int WriteSpeexComment(BinaryWriter buf, string comment)
		{
			int length = comment.Length;
			buf.Write(length);
			buf.Write(Encoding.UTF8.GetBytes(comment), 0, length);
			buf.Write(0);
			return length + 8;
		}

		// Token: 0x06000021 RID: 33 RVA: 0x00002D34 File Offset: 0x00000F34
		protected static byte[] BuildSpeexComment(string comment)
		{
			byte[] array = new byte[comment.Length + 8];
			AudioFileWriter.WriteSpeexComment(new BinaryWriter(new MemoryStream(array)), comment);
			return array;
		}
	}
}
