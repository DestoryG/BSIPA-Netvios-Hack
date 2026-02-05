using System;
using System.Collections.Generic;

namespace Mono.CompilerServices.SymbolWriter
{
	// Token: 0x0200000A RID: 10
	public class LineNumberEntry
	{
		// Token: 0x06000038 RID: 56 RVA: 0x000031DF File Offset: 0x000013DF
		public LineNumberEntry(int file, int row, int column, int offset)
			: this(file, row, column, offset, false)
		{
		}

		// Token: 0x06000039 RID: 57 RVA: 0x000031ED File Offset: 0x000013ED
		public LineNumberEntry(int file, int row, int offset)
			: this(file, row, -1, offset, false)
		{
		}

		// Token: 0x0600003A RID: 58 RVA: 0x000031FA File Offset: 0x000013FA
		public LineNumberEntry(int file, int row, int column, int offset, bool is_hidden)
			: this(file, row, column, -1, -1, offset, is_hidden)
		{
		}

		// Token: 0x0600003B RID: 59 RVA: 0x0000320B File Offset: 0x0000140B
		public LineNumberEntry(int file, int row, int column, int end_row, int end_column, int offset, bool is_hidden)
		{
			this.File = file;
			this.Row = row;
			this.Column = column;
			this.EndRow = end_row;
			this.EndColumn = end_column;
			this.Offset = offset;
			this.IsHidden = is_hidden;
		}

		// Token: 0x0600003C RID: 60 RVA: 0x00003248 File Offset: 0x00001448
		public override string ToString()
		{
			return string.Format("[Line {0}:{1},{2}-{3},{4}:{5}]", new object[] { this.File, this.Row, this.Column, this.EndRow, this.EndColumn, this.Offset });
		}

		// Token: 0x0400002F RID: 47
		public readonly int Row;

		// Token: 0x04000030 RID: 48
		public int Column;

		// Token: 0x04000031 RID: 49
		public int EndRow;

		// Token: 0x04000032 RID: 50
		public int EndColumn;

		// Token: 0x04000033 RID: 51
		public readonly int File;

		// Token: 0x04000034 RID: 52
		public readonly int Offset;

		// Token: 0x04000035 RID: 53
		public readonly bool IsHidden;

		// Token: 0x04000036 RID: 54
		public static readonly LineNumberEntry Null = new LineNumberEntry(0, 0, 0, 0);

		// Token: 0x02000022 RID: 34
		public sealed class LocationComparer : IComparer<LineNumberEntry>
		{
			// Token: 0x060000FA RID: 250 RVA: 0x000060B0 File Offset: 0x000042B0
			public int Compare(LineNumberEntry l1, LineNumberEntry l2)
			{
				if (l1.Row != l2.Row)
				{
					return l1.Row.CompareTo(l2.Row);
				}
				return l1.Column.CompareTo(l2.Column);
			}

			// Token: 0x040000A8 RID: 168
			public static readonly LineNumberEntry.LocationComparer Default = new LineNumberEntry.LocationComparer();
		}
	}
}
