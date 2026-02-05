using System;
using System.Collections.Generic;

namespace Mono.CompilerServices.SymbolWriter
{
	// Token: 0x02000209 RID: 521
	internal class LineNumberEntry
	{
		// Token: 0x06000F92 RID: 3986 RVA: 0x00034EC7 File Offset: 0x000330C7
		public LineNumberEntry(int file, int row, int column, int offset)
			: this(file, row, column, offset, false)
		{
		}

		// Token: 0x06000F93 RID: 3987 RVA: 0x00034ED5 File Offset: 0x000330D5
		public LineNumberEntry(int file, int row, int offset)
			: this(file, row, -1, offset, false)
		{
		}

		// Token: 0x06000F94 RID: 3988 RVA: 0x00034EE2 File Offset: 0x000330E2
		public LineNumberEntry(int file, int row, int column, int offset, bool is_hidden)
			: this(file, row, column, -1, -1, offset, is_hidden)
		{
		}

		// Token: 0x06000F95 RID: 3989 RVA: 0x00034EF3 File Offset: 0x000330F3
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

		// Token: 0x06000F96 RID: 3990 RVA: 0x00034F30 File Offset: 0x00033130
		public override string ToString()
		{
			return string.Format("[Line {0}:{1},{2}-{3},{4}:{5}]", new object[] { this.File, this.Row, this.Column, this.EndRow, this.EndColumn, this.Offset });
		}

		// Token: 0x04000987 RID: 2439
		public readonly int Row;

		// Token: 0x04000988 RID: 2440
		public int Column;

		// Token: 0x04000989 RID: 2441
		public int EndRow;

		// Token: 0x0400098A RID: 2442
		public int EndColumn;

		// Token: 0x0400098B RID: 2443
		public readonly int File;

		// Token: 0x0400098C RID: 2444
		public readonly int Offset;

		// Token: 0x0400098D RID: 2445
		public readonly bool IsHidden;

		// Token: 0x0400098E RID: 2446
		public static readonly LineNumberEntry Null = new LineNumberEntry(0, 0, 0, 0);

		// Token: 0x0200020A RID: 522
		public sealed class LocationComparer : IComparer<LineNumberEntry>
		{
			// Token: 0x06000F98 RID: 3992 RVA: 0x00034FB4 File Offset: 0x000331B4
			public int Compare(LineNumberEntry l1, LineNumberEntry l2)
			{
				if (l1.Row != l2.Row)
				{
					return l1.Row.CompareTo(l2.Row);
				}
				return l1.Column.CompareTo(l2.Column);
			}

			// Token: 0x0400098F RID: 2447
			public static readonly LineNumberEntry.LocationComparer Default = new LineNumberEntry.LocationComparer();
		}
	}
}
