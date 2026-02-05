using System;
using System.IO;

namespace BeatSaberMarkupLanguage.Animations.APNG.Chunks
{
	// Token: 0x020000D8 RID: 216
	internal class IHDRChunk : Chunk
	{
		// Token: 0x060004DF RID: 1247 RVA: 0x00014631 File Offset: 0x00012831
		public IHDRChunk(byte[] chunkBytes)
			: base(chunkBytes)
		{
		}

		// Token: 0x060004E0 RID: 1248 RVA: 0x0001463A File Offset: 0x0001283A
		public IHDRChunk(MemoryStream ms)
			: base(ms)
		{
		}

		// Token: 0x060004E1 RID: 1249 RVA: 0x00014643 File Offset: 0x00012843
		public IHDRChunk(Chunk chunk)
			: base(chunk)
		{
		}

		// Token: 0x17000129 RID: 297
		// (get) Token: 0x060004E2 RID: 1250 RVA: 0x00014DFC File Offset: 0x00012FFC
		// (set) Token: 0x060004E3 RID: 1251 RVA: 0x00014E04 File Offset: 0x00013004
		public int Width
		{
			get
			{
				return this.width;
			}
			internal set
			{
				this.width = value;
				base.ModifyChunkData(0, BitConverter.GetBytes(Helper.ConvertEndian(value)));
			}
		}

		// Token: 0x1700012A RID: 298
		// (get) Token: 0x060004E4 RID: 1252 RVA: 0x00014E1F File Offset: 0x0001301F
		// (set) Token: 0x060004E5 RID: 1253 RVA: 0x00014E27 File Offset: 0x00013027
		public int Height
		{
			get
			{
				return this.height;
			}
			internal set
			{
				this.height = value;
				base.ModifyChunkData(4, BitConverter.GetBytes(Helper.ConvertEndian(value)));
			}
		}

		// Token: 0x1700012B RID: 299
		// (get) Token: 0x060004E6 RID: 1254 RVA: 0x00014E42 File Offset: 0x00013042
		// (set) Token: 0x060004E7 RID: 1255 RVA: 0x00014E4A File Offset: 0x0001304A
		public byte BitDepth
		{
			get
			{
				return this.bitDepth;
			}
			internal set
			{
				this.bitDepth = value;
				base.ModifyChunkData(5, new byte[] { value });
			}
		}

		// Token: 0x1700012C RID: 300
		// (get) Token: 0x060004E8 RID: 1256 RVA: 0x00014E64 File Offset: 0x00013064
		// (set) Token: 0x060004E9 RID: 1257 RVA: 0x00014E6C File Offset: 0x0001306C
		public byte ColorType
		{
			get
			{
				return this.colorType;
			}
			internal set
			{
				this.colorType = value;
				base.ModifyChunkData(6, new byte[] { value });
			}
		}

		// Token: 0x1700012D RID: 301
		// (get) Token: 0x060004EA RID: 1258 RVA: 0x00014E86 File Offset: 0x00013086
		// (set) Token: 0x060004EB RID: 1259 RVA: 0x00014E8E File Offset: 0x0001308E
		public byte CompressionMethod
		{
			get
			{
				return this.compressionMethod;
			}
			internal set
			{
				this.compressionMethod = value;
				base.ModifyChunkData(7, new byte[] { value });
			}
		}

		// Token: 0x1700012E RID: 302
		// (get) Token: 0x060004EC RID: 1260 RVA: 0x00014EA8 File Offset: 0x000130A8
		// (set) Token: 0x060004ED RID: 1261 RVA: 0x00014EB0 File Offset: 0x000130B0
		public byte FilterMethod
		{
			get
			{
				return this.filterMethod;
			}
			internal set
			{
				this.filterMethod = value;
				base.ModifyChunkData(8, new byte[] { value });
			}
		}

		// Token: 0x1700012F RID: 303
		// (get) Token: 0x060004EE RID: 1262 RVA: 0x00014ECA File Offset: 0x000130CA
		// (set) Token: 0x060004EF RID: 1263 RVA: 0x00014ED2 File Offset: 0x000130D2
		public byte InterlaceMethod
		{
			get
			{
				return this.interlaceMethod;
			}
			internal set
			{
				this.interlaceMethod = value;
				base.ModifyChunkData(9, new byte[] { value });
			}
		}

		// Token: 0x060004F0 RID: 1264 RVA: 0x00014EF0 File Offset: 0x000130F0
		protected override void ParseData(MemoryStream ms)
		{
			this.width = Helper.ConvertEndian(ms.ReadInt32());
			this.height = Helper.ConvertEndian(ms.ReadInt32());
			this.bitDepth = Convert.ToByte(ms.ReadByte());
			this.colorType = Convert.ToByte(ms.ReadByte());
			this.compressionMethod = Convert.ToByte(ms.ReadByte());
			this.filterMethod = Convert.ToByte(ms.ReadByte());
			this.interlaceMethod = Convert.ToByte(ms.ReadByte());
		}

		// Token: 0x04000184 RID: 388
		private int width;

		// Token: 0x04000185 RID: 389
		private int height;

		// Token: 0x04000186 RID: 390
		private byte bitDepth;

		// Token: 0x04000187 RID: 391
		private byte colorType;

		// Token: 0x04000188 RID: 392
		private byte compressionMethod;

		// Token: 0x04000189 RID: 393
		private byte filterMethod;

		// Token: 0x0400018A RID: 394
		private byte interlaceMethod;
	}
}
