using System;
using System.IO;

namespace Mono.CompilerServices.SymbolWriter
{
	// Token: 0x02000009 RID: 9
	public class OffsetTable
	{
		// Token: 0x06000034 RID: 52 RVA: 0x00002EC8 File Offset: 0x000010C8
		internal OffsetTable()
		{
			int platform = (int)Environment.OSVersion.Platform;
			if (platform != 4 && platform != 128)
			{
				this.FileFlags |= OffsetTable.Flags.WindowsFileNames;
			}
		}

		// Token: 0x06000035 RID: 53 RVA: 0x00002F18 File Offset: 0x00001118
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

		// Token: 0x06000036 RID: 54 RVA: 0x00003034 File Offset: 0x00001234
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

		// Token: 0x06000037 RID: 55 RVA: 0x00003134 File Offset: 0x00001334
		public override string ToString()
		{
			return string.Format("OffsetTable [{0} - {1}:{2} - {3}:{4}:{5} - {6}:{7}:{8} - {9}]", new object[] { this.TotalFileSize, this.DataSectionOffset, this.DataSectionSize, this.SourceCount, this.SourceTableOffset, this.SourceTableSize, this.MethodCount, this.MethodTableOffset, this.MethodTableSize, this.TypeCount });
		}

		// Token: 0x04000018 RID: 24
		public const int MajorVersion = 50;

		// Token: 0x04000019 RID: 25
		public const int MinorVersion = 0;

		// Token: 0x0400001A RID: 26
		public const long Magic = 5037318119232611860L;

		// Token: 0x0400001B RID: 27
		public int TotalFileSize;

		// Token: 0x0400001C RID: 28
		public int DataSectionOffset;

		// Token: 0x0400001D RID: 29
		public int DataSectionSize;

		// Token: 0x0400001E RID: 30
		public int CompileUnitCount;

		// Token: 0x0400001F RID: 31
		public int CompileUnitTableOffset;

		// Token: 0x04000020 RID: 32
		public int CompileUnitTableSize;

		// Token: 0x04000021 RID: 33
		public int SourceCount;

		// Token: 0x04000022 RID: 34
		public int SourceTableOffset;

		// Token: 0x04000023 RID: 35
		public int SourceTableSize;

		// Token: 0x04000024 RID: 36
		public int MethodCount;

		// Token: 0x04000025 RID: 37
		public int MethodTableOffset;

		// Token: 0x04000026 RID: 38
		public int MethodTableSize;

		// Token: 0x04000027 RID: 39
		public int TypeCount;

		// Token: 0x04000028 RID: 40
		public int AnonymousScopeCount;

		// Token: 0x04000029 RID: 41
		public int AnonymousScopeTableOffset;

		// Token: 0x0400002A RID: 42
		public int AnonymousScopeTableSize;

		// Token: 0x0400002B RID: 43
		public OffsetTable.Flags FileFlags;

		// Token: 0x0400002C RID: 44
		public int LineNumberTable_LineBase = -1;

		// Token: 0x0400002D RID: 45
		public int LineNumberTable_LineRange = 8;

		// Token: 0x0400002E RID: 46
		public int LineNumberTable_OpcodeBase = 9;

		// Token: 0x02000021 RID: 33
		[Flags]
		public enum Flags
		{
			// Token: 0x040000A6 RID: 166
			IsAspxSource = 1,
			// Token: 0x040000A7 RID: 167
			WindowsFileNames = 2
		}
	}
}
