using System;

namespace Microsoft.Cci.Pdb
{
	// Token: 0x020002FB RID: 763
	internal class DbiModuleInfo
	{
		// Token: 0x060011E1 RID: 4577 RVA: 0x0003A580 File Offset: 0x00038780
		internal DbiModuleInfo(BitAccess bits, bool readStrings)
		{
			bits.ReadInt32(out this.opened);
			new DbiSecCon(bits);
			bits.ReadUInt16(out this.flags);
			bits.ReadInt16(out this.stream);
			bits.ReadInt32(out this.cbSyms);
			bits.ReadInt32(out this.cbOldLines);
			bits.ReadInt32(out this.cbLines);
			bits.ReadInt16(out this.files);
			bits.ReadInt16(out this.pad1);
			bits.ReadUInt32(out this.offsets);
			bits.ReadInt32(out this.niSource);
			bits.ReadInt32(out this.niCompiler);
			if (readStrings)
			{
				bits.ReadCString(out this.moduleName);
				bits.ReadCString(out this.objectName);
			}
			else
			{
				bits.SkipCString(out this.moduleName);
				bits.SkipCString(out this.objectName);
			}
			bits.Align(4);
		}

		// Token: 0x04000EBD RID: 3773
		internal int opened;

		// Token: 0x04000EBE RID: 3774
		internal ushort flags;

		// Token: 0x04000EBF RID: 3775
		internal short stream;

		// Token: 0x04000EC0 RID: 3776
		internal int cbSyms;

		// Token: 0x04000EC1 RID: 3777
		internal int cbOldLines;

		// Token: 0x04000EC2 RID: 3778
		internal int cbLines;

		// Token: 0x04000EC3 RID: 3779
		internal short files;

		// Token: 0x04000EC4 RID: 3780
		internal short pad1;

		// Token: 0x04000EC5 RID: 3781
		internal uint offsets;

		// Token: 0x04000EC6 RID: 3782
		internal int niSource;

		// Token: 0x04000EC7 RID: 3783
		internal int niCompiler;

		// Token: 0x04000EC8 RID: 3784
		internal string moduleName;

		// Token: 0x04000EC9 RID: 3785
		internal string objectName;
	}
}
