using System;
using System.IO;

namespace BeatSaberMarkupLanguage.Animations.APNG.Chunks
{
	// Token: 0x020000D5 RID: 213
	internal class fdATChunk : Chunk
	{
		// Token: 0x060004CF RID: 1231 RVA: 0x00014631 File Offset: 0x00012831
		public fdATChunk(byte[] bytes)
			: base(bytes)
		{
		}

		// Token: 0x060004D0 RID: 1232 RVA: 0x0001463A File Offset: 0x0001283A
		public fdATChunk(MemoryStream ms)
			: base(ms)
		{
		}

		// Token: 0x060004D1 RID: 1233 RVA: 0x00014643 File Offset: 0x00012843
		public fdATChunk(Chunk chunk)
			: base(chunk)
		{
		}

		// Token: 0x17000127 RID: 295
		// (get) Token: 0x060004D2 RID: 1234 RVA: 0x00014BB4 File Offset: 0x00012DB4
		// (set) Token: 0x060004D3 RID: 1235 RVA: 0x00014BBC File Offset: 0x00012DBC
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

		// Token: 0x17000128 RID: 296
		// (get) Token: 0x060004D4 RID: 1236 RVA: 0x00014BD2 File Offset: 0x00012DD2
		// (set) Token: 0x060004D5 RID: 1237 RVA: 0x00014BDA File Offset: 0x00012DDA
		public byte[] FrameData
		{
			get
			{
				return this.frameData;
			}
			internal set
			{
				this.frameData = value;
				base.ModifyChunkData(4, value);
			}
		}

		// Token: 0x060004D6 RID: 1238 RVA: 0x00014BEB File Offset: 0x00012DEB
		protected override void ParseData(MemoryStream ms)
		{
			this.sequenceNumber = Helper.ConvertEndian(ms.ReadUInt32());
			this.frameData = ms.ReadBytes((int)(base.Length - 4U));
		}

		// Token: 0x060004D7 RID: 1239 RVA: 0x00014C14 File Offset: 0x00012E14
		public IDATChunk ToIDATChunk()
		{
			uint num;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				memoryStream.WriteBytes(new byte[] { 73, 68, 65, 84 });
				memoryStream.WriteBytes(this.FrameData);
				num = CrcHelper.Calculate(memoryStream.ToArray());
			}
			IDATChunk idatchunk;
			using (MemoryStream memoryStream2 = new MemoryStream())
			{
				memoryStream2.WriteUInt32(Helper.ConvertEndian(base.Length - 4U));
				memoryStream2.WriteBytes(new byte[] { 73, 68, 65, 84 });
				memoryStream2.WriteBytes(this.FrameData);
				memoryStream2.WriteUInt32(Helper.ConvertEndian(num));
				memoryStream2.Position = 0L;
				idatchunk = new IDATChunk(memoryStream2);
			}
			return idatchunk;
		}

		// Token: 0x060004D8 RID: 1240 RVA: 0x00014CE4 File Offset: 0x00012EE4
		public static fdATChunk FromIDATChunk(IDATChunk idatChunk, uint sequenceNumber)
		{
			fdATChunk fdATChunk = null;
			byte[] array;
			uint num;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				memoryStream.WriteBytes(new byte[] { 102, 100, 65, 84 });
				byte[] bytes = BitConverter.GetBytes(Helper.ConvertEndian(sequenceNumber));
				array = new byte[bytes.Length + idatChunk.ChunkData.Length];
				Buffer.BlockCopy(bytes, 0, array, 0, bytes.Length);
				Buffer.BlockCopy(idatChunk.ChunkData, 0, array, bytes.Length, idatChunk.ChunkData.Length);
				memoryStream.WriteBytes(array);
				num = CrcHelper.Calculate(memoryStream.ToArray());
			}
			using (MemoryStream memoryStream2 = new MemoryStream())
			{
				memoryStream2.WriteUInt32((uint)Helper.ConvertEndian(idatChunk.ChunkData.Length + 4));
				memoryStream2.WriteBytes(new byte[] { 102, 100, 65, 84 });
				memoryStream2.WriteBytes(array);
				memoryStream2.WriteUInt32(Helper.ConvertEndian(num));
				memoryStream2.Position = 0L;
				fdATChunk = new fdATChunk(memoryStream2);
			}
			return fdATChunk;
		}

		// Token: 0x04000182 RID: 386
		private uint sequenceNumber;

		// Token: 0x04000183 RID: 387
		private byte[] frameData;
	}
}
