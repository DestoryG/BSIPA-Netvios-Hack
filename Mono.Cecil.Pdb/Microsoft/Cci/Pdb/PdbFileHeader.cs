using System;
using System.IO;
using System.Text;

namespace Microsoft.Cci.Pdb
{
	// Token: 0x020000DC RID: 220
	internal class PdbFileHeader
	{
		// Token: 0x06000197 RID: 407 RVA: 0x00005C80 File Offset: 0x00003E80
		internal PdbFileHeader(Stream reader, BitAccess bits)
		{
			bits.MinCapacity(56);
			reader.Seek(0L, SeekOrigin.Begin);
			bits.FillBuffer(reader, 52);
			this.magic = new byte[32];
			bits.ReadBytes(this.magic);
			bits.ReadInt32(out this.pageSize);
			bits.ReadInt32(out this.freePageMap);
			bits.ReadInt32(out this.pagesUsed);
			bits.ReadInt32(out this.directorySize);
			bits.ReadInt32(out this.zero);
			if (this.Magic != "Microsoft C/C++ MSF 7.00")
			{
				throw new InvalidOperationException("Magic is wrong.");
			}
			int num = ((this.directorySize + this.pageSize - 1) / this.pageSize * 4 + this.pageSize - 1) / this.pageSize;
			this.directoryRoot = new int[num];
			bits.FillBuffer(reader, num * 4);
			bits.ReadInt32(this.directoryRoot);
		}

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x06000198 RID: 408 RVA: 0x00005D6B File Offset: 0x00003F6B
		private string Magic
		{
			get
			{
				return PdbFileHeader.StringFromBytesUTF8(this.magic, 0, "Microsoft C/C++ MSF 7.00".Length);
			}
		}

		// Token: 0x06000199 RID: 409 RVA: 0x00005D83 File Offset: 0x00003F83
		private static string StringFromBytesUTF8(byte[] bytes)
		{
			return PdbFileHeader.StringFromBytesUTF8(bytes, 0, bytes.Length);
		}

		// Token: 0x0600019A RID: 410 RVA: 0x00005D90 File Offset: 0x00003F90
		private static string StringFromBytesUTF8(byte[] bytes, int offset, int length)
		{
			for (int i = 0; i < length; i++)
			{
				if (bytes[offset + i] < 32)
				{
					length = i;
				}
			}
			return Encoding.UTF8.GetString(bytes, offset, length);
		}

		// Token: 0x040004C2 RID: 1218
		private const string MAGIC = "Microsoft C/C++ MSF 7.00";

		// Token: 0x040004C3 RID: 1219
		internal readonly byte[] magic;

		// Token: 0x040004C4 RID: 1220
		internal readonly int pageSize;

		// Token: 0x040004C5 RID: 1221
		internal int freePageMap;

		// Token: 0x040004C6 RID: 1222
		internal int pagesUsed;

		// Token: 0x040004C7 RID: 1223
		internal int directorySize;

		// Token: 0x040004C8 RID: 1224
		internal readonly int zero;

		// Token: 0x040004C9 RID: 1225
		internal int[] directoryRoot;
	}
}
