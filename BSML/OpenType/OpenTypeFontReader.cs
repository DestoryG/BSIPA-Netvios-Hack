using System;
using System.IO;
using System.Text;

namespace BeatSaberMarkupLanguage.OpenType
{
	// Token: 0x0200007C RID: 124
	public class OpenTypeFontReader : OpenTypeReader
	{
		// Token: 0x06000239 RID: 569 RVA: 0x0000C9F6 File Offset: 0x0000ABF6
		public OpenTypeFontReader(Stream input)
			: base(input)
		{
		}

		// Token: 0x0600023A RID: 570 RVA: 0x0000C9FF File Offset: 0x0000ABFF
		public OpenTypeFontReader(Stream input, Encoding encoding)
			: base(input, encoding)
		{
		}

		// Token: 0x0600023B RID: 571 RVA: 0x0000CA09 File Offset: 0x0000AC09
		public OpenTypeFontReader(Stream input, Encoding encoding, bool leaveOpen)
			: base(input, encoding, leaveOpen)
		{
		}

		// Token: 0x0600023C RID: 572 RVA: 0x0000CA14 File Offset: 0x0000AC14
		public OffsetTable ReadOffsetTable()
		{
			return new OffsetTable
			{
				SFNTVersion = base.ReadUInt32(),
				NumTables = base.ReadUInt16(),
				SearchRange = base.ReadUInt16(),
				EntrySelector = base.ReadUInt16(),
				RangeShift = base.ReadUInt16(),
				TablesStart = this.BaseStream.Position
			};
		}

		// Token: 0x0600023D RID: 573 RVA: 0x0000CA80 File Offset: 0x0000AC80
		protected TableRecord ReadTableRecord()
		{
			return new TableRecord
			{
				TableTag = base.ReadTag(),
				Checksum = base.ReadUInt32(),
				Offset = base.ReadOffset32(),
				Length = base.ReadUInt32()
			};
		}

		// Token: 0x0600023E RID: 574 RVA: 0x0000CACC File Offset: 0x0000ACCC
		public TableRecord[] ReadTableRecords(OffsetTable offsets)
		{
			this.BaseStream.Position = offsets.TablesStart;
			TableRecord[] array = new TableRecord[(int)offsets.NumTables];
			for (int i = 0; i < (int)offsets.NumTables; i++)
			{
				array[i] = this.ReadTableRecord();
			}
			return array;
		}

		// Token: 0x0600023F RID: 575 RVA: 0x0000CB18 File Offset: 0x0000AD18
		public TableRecord[] ReadAllTables()
		{
			return this.ReadTableRecords(this.ReadOffsetTable());
		}

		// Token: 0x06000240 RID: 576 RVA: 0x0000CB28 File Offset: 0x0000AD28
		public OpenTypeTable TryReadTable(TableRecord table)
		{
			this.BaseStream.Position = (long)((ulong)table.Offset);
			OpenTypeTable openTypeTable = null;
			if (table.TableTag == OpenTypeTag.NAME)
			{
				openTypeTable = new OpenTypeNameTable();
			}
			if (openTypeTable != null)
			{
				openTypeTable.ReadFrom(this, table.Length);
			}
			return openTypeTable;
		}
	}
}
