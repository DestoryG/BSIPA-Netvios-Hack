using System;
using System.IO;

namespace Mono.Cecil.PE
{
	// Token: 0x020000CD RID: 205
	internal class BinaryStreamReader : BinaryReader
	{
		// Token: 0x170002A8 RID: 680
		// (get) Token: 0x060008A7 RID: 2215 RVA: 0x0001B014 File Offset: 0x00019214
		// (set) Token: 0x060008A8 RID: 2216 RVA: 0x0001B022 File Offset: 0x00019222
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

		// Token: 0x170002A9 RID: 681
		// (get) Token: 0x060008A9 RID: 2217 RVA: 0x0001B031 File Offset: 0x00019231
		public int Length
		{
			get
			{
				return (int)this.BaseStream.Length;
			}
		}

		// Token: 0x060008AA RID: 2218 RVA: 0x0001B03F File Offset: 0x0001923F
		public BinaryStreamReader(Stream stream)
			: base(stream)
		{
		}

		// Token: 0x060008AB RID: 2219 RVA: 0x0001B048 File Offset: 0x00019248
		public void Advance(int bytes)
		{
			this.BaseStream.Seek((long)bytes, SeekOrigin.Current);
		}

		// Token: 0x060008AC RID: 2220 RVA: 0x0001B059 File Offset: 0x00019259
		public void MoveTo(uint position)
		{
			this.BaseStream.Seek((long)((ulong)position), SeekOrigin.Begin);
		}

		// Token: 0x060008AD RID: 2221 RVA: 0x0001B06C File Offset: 0x0001926C
		public void Align(int align)
		{
			align--;
			int position = this.Position;
			this.Advance(((position + align) & ~align) - position);
		}

		// Token: 0x060008AE RID: 2222 RVA: 0x0001B093 File Offset: 0x00019293
		public DataDirectory ReadDataDirectory()
		{
			return new DataDirectory(this.ReadUInt32(), this.ReadUInt32());
		}
	}
}
