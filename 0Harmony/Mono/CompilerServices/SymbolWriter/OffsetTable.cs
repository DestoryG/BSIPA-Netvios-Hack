using System;
using System.IO;

namespace Mono.CompilerServices.SymbolWriter
{
	// Token: 0x02000207 RID: 519
	internal class OffsetTable
	{
		// Token: 0x06000F8E RID: 3982 RVA: 0x00034BB0 File Offset: 0x00032DB0
		internal OffsetTable()
		{
			int platform = (int)Environment.OSVersion.Platform;
			if (platform != 4 && platform != 128)
			{
				this.FileFlags |= OffsetTable.Flags.WindowsFileNames;
			}
		}

		// Token: 0x06000F8F RID: 3983 RVA: 0x00034C00 File Offset: 0x00032E00
		internal OffsetTable(BinaryReader reader, int major_version, int minor_version)
		{
			this.TotalFileSize = reader.ReadInt32();
			this.DataSectionOffset = reader.ReadInt32();
			this.DataSectionSize = reader.ReadInt32();
			this.CompileUnitCount = reader.ReadInt32();
			this.CompileUnitTableOffset = reader.ReadInt32();
			this.CompileUnitTableSize = reader.ReadInt32();
			this.SourceCount = reader.ReadInt32();
			this.SourceTableOffset = reader.ReadInt32();
			this.SourceTableSize = reader.ReadInt32();
			this.MethodCount = reader.ReadInt32();
			this.MethodTableOffset = reader.ReadInt32();
			this.MethodTableSize = reader.ReadInt32();
			this.TypeCount = reader.ReadInt32();
			this.AnonymousScopeCount = reader.ReadInt32();
			this.AnonymousScopeTableOffset = reader.ReadInt32();
			this.AnonymousScopeTableSize = reader.ReadInt32();
			this.LineNumberTable_LineBase = reader.ReadInt32();
			this.LineNumberTable_LineRange = reader.ReadInt32();
			this.LineNumberTable_OpcodeBase = reader.ReadInt32();
			this.FileFlags = (OffsetTable.Flags)reader.ReadInt32();
		}

		// Token: 0x06000F90 RID: 3984 RVA: 0x00034D1C File Offset: 0x00032F1C
		internal void Write(BinaryWriter bw, int major_version, int minor_version)
		{
			bw.Write(this.TotalFileSize);
			bw.Write(this.DataSectionOffset);
			bw.Write(this.DataSectionSize);
			bw.Write(this.CompileUnitCount);
			bw.Write(this.CompileUnitTableOffset);
			bw.Write(this.CompileUnitTableSize);
			bw.Write(this.SourceCount);
			bw.Write(this.SourceTableOffset);
			bw.Write(this.SourceTableSize);
			bw.Write(this.MethodCount);
			bw.Write(this.MethodTableOffset);
			bw.Write(this.MethodTableSize);
			bw.Write(this.TypeCount);
			bw.Write(this.AnonymousScopeCount);
			bw.Write(this.AnonymousScopeTableOffset);
			bw.Write(this.AnonymousScopeTableSize);
			bw.Write(this.LineNumberTable_LineBase);
			bw.Write(this.LineNumberTable_LineRange);
			bw.Write(this.LineNumberTable_OpcodeBase);
			bw.Write((int)this.FileFlags);
		}

		// Token: 0x06000F91 RID: 3985 RVA: 0x00034E1C File Offset: 0x0003301C
		public override string ToString()
		{
			return string.Format("OffsetTable [{0} - {1}:{2} - {3}:{4}:{5} - {6}:{7}:{8} - {9}]", new object[] { this.TotalFileSize, this.DataSectionOffset, this.DataSectionSize, this.SourceCount, this.SourceTableOffset, this.SourceTableSize, this.MethodCount, this.MethodTableOffset, this.MethodTableSize, this.TypeCount });
		}

		// Token: 0x0400096D RID: 2413
		public const int MajorVersion = 50;

		// Token: 0x0400096E RID: 2414
		public const int MinorVersion = 0;

		// Token: 0x0400096F RID: 2415
		public const long Magic = 5037318119232611860L;

		// Token: 0x04000970 RID: 2416
		public int TotalFileSize;

		// Token: 0x04000971 RID: 2417
		public int DataSectionOffset;

		// Token: 0x04000972 RID: 2418
		public int DataSectionSize;

		// Token: 0x04000973 RID: 2419
		public int CompileUnitCount;

		// Token: 0x04000974 RID: 2420
		public int CompileUnitTableOffset;

		// Token: 0x04000975 RID: 2421
		public int CompileUnitTableSize;

		// Token: 0x04000976 RID: 2422
		public int SourceCount;

		// Token: 0x04000977 RID: 2423
		public int SourceTableOffset;

		// Token: 0x04000978 RID: 2424
		public int SourceTableSize;

		// Token: 0x04000979 RID: 2425
		public int MethodCount;

		// Token: 0x0400097A RID: 2426
		public int MethodTableOffset;

		// Token: 0x0400097B RID: 2427
		public int MethodTableSize;

		// Token: 0x0400097C RID: 2428
		public int TypeCount;

		// Token: 0x0400097D RID: 2429
		public int AnonymousScopeCount;

		// Token: 0x0400097E RID: 2430
		public int AnonymousScopeTableOffset;

		// Token: 0x0400097F RID: 2431
		public int AnonymousScopeTableSize;

		// Token: 0x04000980 RID: 2432
		public OffsetTable.Flags FileFlags;

		// Token: 0x04000981 RID: 2433
		public int LineNumberTable_LineBase = -1;

		// Token: 0x04000982 RID: 2434
		public int LineNumberTable_LineRange = 8;

		// Token: 0x04000983 RID: 2435
		public int LineNumberTable_OpcodeBase = 9;

		// Token: 0x02000208 RID: 520
		[Flags]
		public enum Flags
		{
			// Token: 0x04000985 RID: 2437
			IsAspxSource = 1,
			// Token: 0x04000986 RID: 2438
			WindowsFileNames = 2
		}
	}
}
