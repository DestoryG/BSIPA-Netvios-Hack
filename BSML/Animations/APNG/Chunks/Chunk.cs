using System;
using System.IO;
using System.Text;

namespace BeatSaberMarkupLanguage.Animations.APNG.Chunks
{
	// Token: 0x020000D1 RID: 209
	public class Chunk
	{
		// Token: 0x060004A6 RID: 1190 RVA: 0x000146AC File Offset: 0x000128AC
		internal Chunk()
		{
			this.Length = 0U;
			this.ChunkType = string.Empty;
			this.ChunkData = null;
			this.Crc = 0U;
		}

		// Token: 0x060004A7 RID: 1191 RVA: 0x000146D4 File Offset: 0x000128D4
		internal Chunk(byte[] bytes)
		{
			MemoryStream memoryStream = new MemoryStream(bytes);
			this.Length = Helper.ConvertEndian(memoryStream.ReadUInt32());
			this.ChunkType = Encoding.ASCII.GetString(memoryStream.ReadBytes(4));
			this.ChunkData = memoryStream.ReadBytes((int)this.Length);
			this.Crc = Helper.ConvertEndian(memoryStream.ReadUInt32());
			if (memoryStream.Position != memoryStream.Length)
			{
				throw new Exception("Chunk length not correct.");
			}
			if ((ulong)this.Length != (ulong)((long)this.ChunkData.Length))
			{
				throw new Exception("Chunk data length not correct.");
			}
			this.ParseData(new MemoryStream(this.ChunkData));
		}

		// Token: 0x060004A8 RID: 1192 RVA: 0x00014780 File Offset: 0x00012980
		internal Chunk(MemoryStream ms)
		{
			this.Length = Helper.ConvertEndian(ms.ReadUInt32());
			this.ChunkType = Encoding.ASCII.GetString(ms.ReadBytes(4));
			this.ChunkData = ms.ReadBytes((int)this.Length);
			this.Crc = Helper.ConvertEndian(ms.ReadUInt32());
			this.ParseData(new MemoryStream(this.ChunkData));
		}

		// Token: 0x060004A9 RID: 1193 RVA: 0x000147F0 File Offset: 0x000129F0
		internal Chunk(Chunk chunk)
		{
			this.Length = chunk.Length;
			this.ChunkType = chunk.ChunkType;
			this.ChunkData = chunk.ChunkData;
			this.Crc = chunk.Crc;
			this.ParseData(new MemoryStream(this.ChunkData));
		}

		// Token: 0x17000119 RID: 281
		// (get) Token: 0x060004AA RID: 1194 RVA: 0x00014844 File Offset: 0x00012A44
		// (set) Token: 0x060004AB RID: 1195 RVA: 0x0001484C File Offset: 0x00012A4C
		public uint Length { get; set; }

		// Token: 0x1700011A RID: 282
		// (get) Token: 0x060004AC RID: 1196 RVA: 0x00014855 File Offset: 0x00012A55
		// (set) Token: 0x060004AD RID: 1197 RVA: 0x0001485D File Offset: 0x00012A5D
		public string ChunkType { get; set; }

		// Token: 0x1700011B RID: 283
		// (get) Token: 0x060004AE RID: 1198 RVA: 0x00014866 File Offset: 0x00012A66
		// (set) Token: 0x060004AF RID: 1199 RVA: 0x0001486E File Offset: 0x00012A6E
		public byte[] ChunkData { get; set; }

		// Token: 0x1700011C RID: 284
		// (get) Token: 0x060004B0 RID: 1200 RVA: 0x00014877 File Offset: 0x00012A77
		// (set) Token: 0x060004B1 RID: 1201 RVA: 0x0001487F File Offset: 0x00012A7F
		public uint Crc { get; set; }

		// Token: 0x1700011D RID: 285
		// (get) Token: 0x060004B2 RID: 1202 RVA: 0x00014888 File Offset: 0x00012A88
		public byte[] RawData
		{
			get
			{
				MemoryStream memoryStream = new MemoryStream();
				memoryStream.WriteUInt32(Helper.ConvertEndian(this.Length));
				memoryStream.WriteBytes(Encoding.ASCII.GetBytes(this.ChunkType));
				memoryStream.WriteBytes(this.ChunkData);
				memoryStream.WriteUInt32(Helper.ConvertEndian(this.Crc));
				return memoryStream.ToArray();
			}
		}

		// Token: 0x060004B3 RID: 1203 RVA: 0x000148E4 File Offset: 0x00012AE4
		public void ModifyChunkData(int postion, byte[] newData)
		{
			Array.Copy(newData, 0, this.ChunkData, postion, newData.Length);
			using (MemoryStream memoryStream = new MemoryStream())
			{
				memoryStream.WriteBytes(Encoding.ASCII.GetBytes(this.ChunkType));
				memoryStream.WriteBytes(this.ChunkData);
				this.Crc = CrcHelper.Calculate(memoryStream.ToArray());
			}
		}

		// Token: 0x060004B4 RID: 1204 RVA: 0x00014958 File Offset: 0x00012B58
		public void ModifyChunkData(int postion, uint newData)
		{
			this.ModifyChunkData(postion, BitConverter.GetBytes(newData));
		}

		// Token: 0x060004B5 RID: 1205 RVA: 0x0000263A File Offset: 0x0000083A
		protected virtual void ParseData(MemoryStream ms)
		{
		}

		// Token: 0x060004B6 RID: 1206 RVA: 0x00014968 File Offset: 0x00012B68
		public override bool Equals(object obj)
		{
			bool flag = false;
			if (obj == null)
			{
				flag = false;
			}
			else
			{
				Chunk chunk = obj as Chunk;
				if (chunk != null)
				{
					flag = this.Length == chunk.Length && this.ChunkType == chunk.ChunkType && this.Crc == chunk.Crc;
				}
			}
			return flag;
		}

		// Token: 0x060004B7 RID: 1207 RVA: 0x000149BC File Offset: 0x00012BBC
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}
	}
}
