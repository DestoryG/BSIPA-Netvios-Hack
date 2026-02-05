using System;
using System.IO;

namespace Mono.Cecil.PE
{
	// Token: 0x0200018F RID: 399
	internal class BinaryStreamReader : BinaryReader
	{
		// Token: 0x170002C8 RID: 712
		// (get) Token: 0x06000C85 RID: 3205 RVA: 0x0002A113 File Offset: 0x00028313
		// (set) Token: 0x06000C86 RID: 3206 RVA: 0x0002A121 File Offset: 0x00028321
		public int Position
		{
			get
			{
				return (int)this.BaseStream.Position;
			}
			set
			{
				this.BaseStream.Position = (long)value;
			}
		}

		// Token: 0x170002C9 RID: 713
		// (get) Token: 0x06000C87 RID: 3207 RVA: 0x0002A130 File Offset: 0x00028330
		public int Length
		{
			get
			{
				return (int)this.BaseStream.Length;
			}
		}

		// Token: 0x06000C88 RID: 3208 RVA: 0x0002A13E File Offset: 0x0002833E
		public BinaryStreamReader(Stream stream)
			: base(stream)
		{
		}

		// Token: 0x06000C89 RID: 3209 RVA: 0x0002A147 File Offset: 0x00028347
		public void Advance(int bytes)
		{
			this.BaseStream.Seek((long)bytes, SeekOrigin.Current);
		}

		// Token: 0x06000C8A RID: 3210 RVA: 0x0002A158 File Offset: 0x00028358
		public void MoveTo(uint position)
		{
			this.BaseStream.Seek((long)((ulong)position), SeekOrigin.Begin);
		}

		// Token: 0x06000C8B RID: 3211 RVA: 0x0002A16C File Offset: 0x0002836C
		public void Align(int align)
		{
			align--;
			int position = this.Position;
			this.Advance(((position + align) & ~align) - position);
		}

		// Token: 0x06000C8C RID: 3212 RVA: 0x0002A193 File Offset: 0x00028393
		public DataDirectory ReadDataDirectory()
		{
			return new DataDirectory(this.ReadUInt32(), this.ReadUInt32());
		}
	}
}
