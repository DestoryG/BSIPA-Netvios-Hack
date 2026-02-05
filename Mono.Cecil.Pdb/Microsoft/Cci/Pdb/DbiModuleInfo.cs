using System;

namespace Microsoft.Cci.Pdb
{
	// Token: 0x020000D4 RID: 212
	internal class DbiModuleInfo
	{
		// Token: 0x0600017A RID: 378 RVA: 0x000045FC File Offset: 0x000027FC
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

		// Token: 0x040004A1 RID: 1185
		internal int opened;

		// Token: 0x040004A2 RID: 1186
		internal ushort flags;

		// Token: 0x040004A3 RID: 1187
		internal short stream;

		// Token: 0x040004A4 RID: 1188
		internal int cbSyms;

		// Token: 0x040004A5 RID: 1189
		internal int cbOldLines;

		// Token: 0x040004A6 RID: 1190
		internal int cbLines;

		// Token: 0x040004A7 RID: 1191
		internal short files;

		// Token: 0x040004A8 RID: 1192
		internal short pad1;

		// Token: 0x040004A9 RID: 1193
		internal uint offsets;

		// Token: 0x040004AA RID: 1194
		internal int niSource;

		// Token: 0x040004AB RID: 1195
		internal int niCompiler;

		// Token: 0x040004AC RID: 1196
		internal string moduleName;

		// Token: 0x040004AD RID: 1197
		internal string objectName;
	}
}
