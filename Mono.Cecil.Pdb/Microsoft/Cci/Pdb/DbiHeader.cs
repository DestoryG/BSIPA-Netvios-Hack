using System;

namespace Microsoft.Cci.Pdb
{
	// Token: 0x020000D3 RID: 211
	internal struct DbiHeader
	{
		// Token: 0x06000179 RID: 377 RVA: 0x000044FC File Offset: 0x000026FC
		internal DbiHeader(BitAccess bits)
		{
			bits.ReadInt32(out this.sig);
			bits.ReadInt32(out this.ver);
			bits.ReadInt32(out this.age);
			bits.ReadInt16(out this.gssymStream);
			bits.ReadUInt16(out this.vers);
			bits.ReadInt16(out this.pssymStream);
			bits.ReadUInt16(out this.pdbver);
			bits.ReadInt16(out this.symrecStream);
			bits.ReadUInt16(out this.pdbver2);
			bits.ReadInt32(out this.gpmodiSize);
			bits.ReadInt32(out this.secconSize);
			bits.ReadInt32(out this.secmapSize);
			bits.ReadInt32(out this.filinfSize);
			bits.ReadInt32(out this.tsmapSize);
			bits.ReadInt32(out this.mfcIndex);
			bits.ReadInt32(out this.dbghdrSize);
			bits.ReadInt32(out this.ecinfoSize);
			bits.ReadUInt16(out this.flags);
			bits.ReadUInt16(out this.machine);
			bits.ReadInt32(out this.reserved);
		}

		// Token: 0x0400048D RID: 1165
		internal int sig;

		// Token: 0x0400048E RID: 1166
		internal int ver;

		// Token: 0x0400048F RID: 1167
		internal int age;

		// Token: 0x04000490 RID: 1168
		internal short gssymStream;

		// Token: 0x04000491 RID: 1169
		internal ushort vers;

		// Token: 0x04000492 RID: 1170
		internal short pssymStream;

		// Token: 0x04000493 RID: 1171
		internal ushort pdbver;

		// Token: 0x04000494 RID: 1172
		internal short symrecStream;

		// Token: 0x04000495 RID: 1173
		internal ushort pdbver2;

		// Token: 0x04000496 RID: 1174
		internal int gpmodiSize;

		// Token: 0x04000497 RID: 1175
		internal int secconSize;

		// Token: 0x04000498 RID: 1176
		internal int secmapSize;

		// Token: 0x04000499 RID: 1177
		internal int filinfSize;

		// Token: 0x0400049A RID: 1178
		internal int tsmapSize;

		// Token: 0x0400049B RID: 1179
		internal int mfcIndex;

		// Token: 0x0400049C RID: 1180
		internal int dbghdrSize;

		// Token: 0x0400049D RID: 1181
		internal int ecinfoSize;

		// Token: 0x0400049E RID: 1182
		internal ushort flags;

		// Token: 0x0400049F RID: 1183
		internal ushort machine;

		// Token: 0x040004A0 RID: 1184
		internal int reserved;
	}
}
