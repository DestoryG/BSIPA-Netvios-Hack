using System;
using System.IO;

namespace NSpeex
{
	// Token: 0x0200001A RID: 26
	public class RawWriter : AudioFileWriter
	{
		// Token: 0x060000B8 RID: 184 RVA: 0x00008D74 File Offset: 0x00006F74
		public override void Close()
		{
			this.xout.Close();
		}

		// Token: 0x060000B9 RID: 185 RVA: 0x00008D81 File Offset: 0x00006F81
		public override void Open(Stream stream)
		{
			this.xout = stream;
		}

		// Token: 0x060000BA RID: 186 RVA: 0x00008D8A File Offset: 0x00006F8A
		public override void WriteHeader(string comment)
		{
		}

		// Token: 0x060000BB RID: 187 RVA: 0x00008D8C File Offset: 0x00006F8C
		public override void WritePacket(byte[] data, int offset, int len)
		{
			this.xout.Write(data, offset, len);
		}

		// Token: 0x040000D9 RID: 217
		private Stream xout;
	}
}
