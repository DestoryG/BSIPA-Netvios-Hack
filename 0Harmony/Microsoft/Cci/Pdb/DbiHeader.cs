using System;

namespace Microsoft.Cci.Pdb
{
	// Token: 0x020002FA RID: 762
	internal struct DbiHeader
	{
		// Token: 0x060011E0 RID: 4576 RVA: 0x0003A480 File Offset: 0x00038680
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

		// Token: 0x04000EA9 RID: 3753
		internal int sig;

		// Token: 0x04000EAA RID: 3754
		internal int ver;

		// Token: 0x04000EAB RID: 3755
		internal int age;

		// Token: 0x04000EAC RID: 3756
		internal short gssymStream;

		// Token: 0x04000EAD RID: 3757
		internal ushort vers;

		// Token: 0x04000EAE RID: 3758
		internal short pssymStream;

		// Token: 0x04000EAF RID: 3759
		internal ushort pdbver;

		// Token: 0x04000EB0 RID: 3760
		internal short symrecStream;

		// Token: 0x04000EB1 RID: 3761
		internal ushort pdbver2;

		// Token: 0x04000EB2 RID: 3762
		internal int gpmodiSize;

		// Token: 0x04000EB3 RID: 3763
		internal int secconSize;

		// Token: 0x04000EB4 RID: 3764
		internal int secmapSize;

		// Token: 0x04000EB5 RID: 3765
		internal int filinfSize;

		// Token: 0x04000EB6 RID: 3766
		internal int tsmapSize;

		// Token: 0x04000EB7 RID: 3767
		internal int mfcIndex;

		// Token: 0x04000EB8 RID: 3768
		internal int dbghdrSize;

		// Token: 0x04000EB9 RID: 3769
		internal int ecinfoSize;

		// Token: 0x04000EBA RID: 3770
		internal ushort flags;

		// Token: 0x04000EBB RID: 3771
		internal ushort machine;

		// Token: 0x04000EBC RID: 3772
		internal int reserved;
	}
}
