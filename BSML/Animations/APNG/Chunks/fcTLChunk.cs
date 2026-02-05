using System;
using System.IO;

namespace BeatSaberMarkupLanguage.Animations.APNG.Chunks
{
	// Token: 0x020000D4 RID: 212
	internal class fcTLChunk : Chunk
	{
		// Token: 0x060004B8 RID: 1208 RVA: 0x000149C4 File Offset: 0x00012BC4
		internal fcTLChunk()
		{
			base.Length = 26U;
			base.ChunkType = "fcTL";
			base.ChunkData = new byte[base.Length];
		}

		// Token: 0x060004B9 RID: 1209 RVA: 0x00014631 File Offset: 0x00012831
		public fcTLChunk(byte[] bytes)
			: base(bytes)
		{
		}

		// Token: 0x060004BA RID: 1210 RVA: 0x0001463A File Offset: 0x0001283A
		public fcTLChunk(MemoryStream ms)
			: base(ms)
		{
		}

		// Token: 0x060004BB RID: 1211 RVA: 0x00014643 File Offset: 0x00012843
		public fcTLChunk(Chunk chunk)
			: base(chunk)
		{
		}

		// Token: 0x1700011E RID: 286
		// (get) Token: 0x060004BC RID: 1212 RVA: 0x000149F0 File Offset: 0x00012BF0
		// (set) Token: 0x060004BD RID: 1213 RVA: 0x000149F8 File Offset: 0x00012BF8
		public uint SequenceNumber
		{
			get
			{
				return this.sequenceNumber;
			}
			internal set
			{
				this.sequenceNumber = value;
				base.ModifyChunkData(0, Helper.ConvertEndian(value));
			}
		}

		// Token: 0x1700011F RID: 287
		// (get) Token: 0x060004BE RID: 1214 RVA: 0x00014A0E File Offset: 0x00012C0E
		// (set) Token: 0x060004BF RID: 1215 RVA: 0x00014A16 File Offset: 0x00012C16
		public uint Width
		{
			get
			{
				return this.width;
			}
			internal set
			{
				this.width = value;
				base.ModifyChunkData(4, Helper.ConvertEndian(value));
			}
		}

		// Token: 0x17000120 RID: 288
		// (get) Token: 0x060004C0 RID: 1216 RVA: 0x00014A2C File Offset: 0x00012C2C
		// (set) Token: 0x060004C1 RID: 1217 RVA: 0x00014A34 File Offset: 0x00012C34
		public uint Height
		{
			get
			{
				return this.height;
			}
			internal set
			{
				this.height = value;
				base.ModifyChunkData(8, Helper.ConvertEndian(value));
			}
		}

		// Token: 0x17000121 RID: 289
		// (get) Token: 0x060004C2 RID: 1218 RVA: 0x00014A4A File Offset: 0x00012C4A
		// (set) Token: 0x060004C3 RID: 1219 RVA: 0x00014A52 File Offset: 0x00012C52
		public uint XOffset
		{
			get
			{
				return this.xOffset;
			}
			internal set
			{
				this.xOffset = value;
				base.ModifyChunkData(12, Helper.ConvertEndian(value));
			}
		}

		// Token: 0x17000122 RID: 290
		// (get) Token: 0x060004C4 RID: 1220 RVA: 0x00014A69 File Offset: 0x00012C69
		// (set) Token: 0x060004C5 RID: 1221 RVA: 0x00014A71 File Offset: 0x00012C71
		public uint YOffset
		{
			get
			{
				return this.yOffset;
			}
			internal set
			{
				this.yOffset = value;
				base.ModifyChunkData(16, Helper.ConvertEndian(value));
			}
		}

		// Token: 0x17000123 RID: 291
		// (get) Token: 0x060004C6 RID: 1222 RVA: 0x00014A88 File Offset: 0x00012C88
		// (set) Token: 0x060004C7 RID: 1223 RVA: 0x00014A90 File Offset: 0x00012C90
		public ushort DelayNumerator
		{
			get
			{
				return this.delayNumerator;
			}
			internal set
			{
				this.delayNumerator = value;
				base.ModifyChunkData(20, BitConverter.GetBytes(Helper.ConvertEndian(value)));
			}
		}

		// Token: 0x17000124 RID: 292
		// (get) Token: 0x060004C8 RID: 1224 RVA: 0x00014AAC File Offset: 0x00012CAC
		// (set) Token: 0x060004C9 RID: 1225 RVA: 0x00014AB4 File Offset: 0x00012CB4
		public ushort DelayDenominator
		{
			get
			{
				return this.delayDenominator;
			}
			internal set
			{
				this.delayDenominator = value;
				base.ModifyChunkData(22, BitConverter.GetBytes(Helper.ConvertEndian(value)));
			}
		}

		// Token: 0x17000125 RID: 293
		// (get) Token: 0x060004CA RID: 1226 RVA: 0x00014AD0 File Offset: 0x00012CD0
		// (set) Token: 0x060004CB RID: 1227 RVA: 0x00014AD8 File Offset: 0x00012CD8
		public DisposeOps DisposeOp
		{
			get
			{
				return this.disposeOp;
			}
			internal set
			{
				this.disposeOp = value;
				base.ModifyChunkData(24, new byte[] { (byte)value });
			}
		}

		// Token: 0x17000126 RID: 294
		// (get) Token: 0x060004CC RID: 1228 RVA: 0x00014AF4 File Offset: 0x00012CF4
		// (set) Token: 0x060004CD RID: 1229 RVA: 0x00014AFC File Offset: 0x00012CFC
		public BlendOps BlendOp
		{
			get
			{
				return this.blendOp;
			}
			internal set
			{
				this.blendOp = value;
				base.ModifyChunkData(25, new byte[] { (byte)value });
			}
		}

		// Token: 0x060004CE RID: 1230 RVA: 0x00014B18 File Offset: 0x00012D18
		protected override void ParseData(MemoryStream ms)
		{
			this.sequenceNumber = Helper.ConvertEndian(ms.ReadUInt32());
			this.width = Helper.ConvertEndian(ms.ReadUInt32());
			this.height = Helper.ConvertEndian(ms.ReadUInt32());
			this.xOffset = Helper.ConvertEndian(ms.ReadUInt32());
			this.yOffset = Helper.ConvertEndian(ms.ReadUInt32());
			this.delayNumerator = Helper.ConvertEndian(ms.ReadUInt16());
			this.delayDenominator = Helper.ConvertEndian(ms.ReadUInt16());
			this.disposeOp = (DisposeOps)ms.ReadByte();
			this.blendOp = (BlendOps)ms.ReadByte();
		}

		// Token: 0x04000179 RID: 377
		private uint sequenceNumber;

		// Token: 0x0400017A RID: 378
		private uint width;

		// Token: 0x0400017B RID: 379
		private uint height;

		// Token: 0x0400017C RID: 380
		private uint xOffset;

		// Token: 0x0400017D RID: 381
		private uint yOffset;

		// Token: 0x0400017E RID: 382
		private ushort delayNumerator;

		// Token: 0x0400017F RID: 383
		private ushort delayDenominator;

		// Token: 0x04000180 RID: 384
		private DisposeOps disposeOp;

		// Token: 0x04000181 RID: 385
		private BlendOps blendOp;
	}
}
