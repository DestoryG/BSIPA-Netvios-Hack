using System;
using System.IO;

namespace BeatSaberMarkupLanguage.Animations.APNG.Chunks
{
	// Token: 0x020000D0 RID: 208
	internal class acTLChunk : Chunk
	{
		// Token: 0x0600049D RID: 1181 RVA: 0x00014606 File Offset: 0x00012806
		internal acTLChunk()
		{
			base.Length = 8U;
			base.ChunkType = "acTL";
			base.ChunkData = new byte[base.Length];
		}

		// Token: 0x0600049E RID: 1182 RVA: 0x00014631 File Offset: 0x00012831
		public acTLChunk(byte[] bytes)
			: base(bytes)
		{
		}

		// Token: 0x0600049F RID: 1183 RVA: 0x0001463A File Offset: 0x0001283A
		public acTLChunk(MemoryStream ms)
			: base(ms)
		{
		}

		// Token: 0x060004A0 RID: 1184 RVA: 0x00014643 File Offset: 0x00012843
		public acTLChunk(Chunk chunk)
			: base(chunk)
		{
		}

		// Token: 0x17000117 RID: 279
		// (get) Token: 0x060004A1 RID: 1185 RVA: 0x0001464C File Offset: 0x0001284C
		// (set) Token: 0x060004A2 RID: 1186 RVA: 0x00014654 File Offset: 0x00012854
		public uint FrameCount
		{
			get
			{
				return this.frameCount;
			}
			internal set
			{
				this.frameCount = value;
				base.ModifyChunkData(0, Helper.ConvertEndian(value));
			}
		}

		// Token: 0x17000118 RID: 280
		// (get) Token: 0x060004A3 RID: 1187 RVA: 0x0001466A File Offset: 0x0001286A
		// (set) Token: 0x060004A4 RID: 1188 RVA: 0x00014672 File Offset: 0x00012872
		public uint PlayCount
		{
			get
			{
				return this.playCount;
			}
			internal set
			{
				this.playCount = value;
				base.ModifyChunkData(4, Helper.ConvertEndian(value));
			}
		}

		// Token: 0x060004A5 RID: 1189 RVA: 0x00014688 File Offset: 0x00012888
		protected override void ParseData(MemoryStream ms)
		{
			this.frameCount = Helper.ConvertEndian(ms.ReadUInt32());
			this.playCount = Helper.ConvertEndian(ms.ReadUInt32());
		}

		// Token: 0x0400016C RID: 364
		private uint frameCount;

		// Token: 0x0400016D RID: 365
		private uint playCount;
	}
}
