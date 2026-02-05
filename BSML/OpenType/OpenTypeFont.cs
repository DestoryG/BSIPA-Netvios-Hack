using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace BeatSaberMarkupLanguage.OpenType
{
	// Token: 0x0200007B RID: 123
	public class OpenTypeFont : IDisposable
	{
		// Token: 0x170000A1 RID: 161
		// (get) Token: 0x06000229 RID: 553 RVA: 0x0000C718 File Offset: 0x0000A918
		public OpenTypeFontReader Reader { get; }

		// Token: 0x0600022A RID: 554 RVA: 0x0000C720 File Offset: 0x0000A920
		public OpenTypeFont(OpenTypeFontReader reader, bool lazyLoad = true)
			: this(reader.ReadOffsetTable(), reader, lazyLoad)
		{
		}

		// Token: 0x0600022B RID: 555 RVA: 0x0000C730 File Offset: 0x0000A930
		public OpenTypeFont(OffsetTable offsets, OpenTypeFontReader reader, bool lazyLoad = true)
		{
			this.offsetTable = offsets;
			this.tables = reader.ReadTableRecords(this.offsetTable);
			this.nameTableRecord = this.tables.Select((TableRecord t) => new TableRecord?(t)).FirstOrDefault((TableRecord? t) => t.Value.TableTag == OpenTypeTag.NAME);
			if (lazyLoad)
			{
				this.Reader = reader;
				return;
			}
			this.LoadAllTables(reader);
		}

		// Token: 0x0600022C RID: 556 RVA: 0x0000C7C2 File Offset: 0x0000A9C2
		private void LoadAllTables(OpenTypeFontReader reader)
		{
			this.nameTable = this.ReadNameTable(reader);
		}

		// Token: 0x170000A2 RID: 162
		// (get) Token: 0x0600022D RID: 557 RVA: 0x0000C7D4 File Offset: 0x0000A9D4
		public OpenTypeNameTable NameTable
		{
			get
			{
				OpenTypeNameTable openTypeNameTable;
				if ((openTypeNameTable = this.nameTable) == null)
				{
					openTypeNameTable = (this.nameTable = this.ReadNameTable(this.Reader));
				}
				return openTypeNameTable;
			}
		}

		// Token: 0x170000A3 RID: 163
		// (get) Token: 0x0600022E RID: 558 RVA: 0x0000C800 File Offset: 0x0000AA00
		public string UniqueId
		{
			get
			{
				string text;
				if ((text = this.uniqueId) == null)
				{
					OpenTypeNameTable.NameRecord nameRecord = this.FindBestNameRecord(OpenTypeNameTable.NameRecord.NameType.UniqueId);
					text = (this.uniqueId = ((nameRecord != null) ? nameRecord.Value : null));
				}
				return text;
			}
		}

		// Token: 0x170000A4 RID: 164
		// (get) Token: 0x0600022F RID: 559 RVA: 0x0000C834 File Offset: 0x0000AA34
		public string Family
		{
			get
			{
				string text;
				if ((text = this.family) == null)
				{
					OpenTypeNameTable.NameRecord nameRecord = this.FindBestNameRecord(OpenTypeNameTable.NameRecord.NameType.FontFamily);
					text = (this.family = ((nameRecord != null) ? nameRecord.Value : null));
				}
				return text;
			}
		}

		// Token: 0x170000A5 RID: 165
		// (get) Token: 0x06000230 RID: 560 RVA: 0x0000C868 File Offset: 0x0000AA68
		public string Subfamily
		{
			get
			{
				string text;
				if ((text = this.subfamily) == null)
				{
					OpenTypeNameTable.NameRecord nameRecord = this.FindBestNameRecord(OpenTypeNameTable.NameRecord.NameType.FontSubfamily);
					text = (this.subfamily = ((nameRecord != null) ? nameRecord.Value : null));
				}
				return text;
			}
		}

		// Token: 0x170000A6 RID: 166
		// (get) Token: 0x06000231 RID: 561 RVA: 0x0000C89C File Offset: 0x0000AA9C
		public string FullName
		{
			get
			{
				string text;
				if ((text = this.fullName) == null)
				{
					OpenTypeNameTable.NameRecord nameRecord = this.FindBestNameRecord(OpenTypeNameTable.NameRecord.NameType.FullFontName);
					text = (this.fullName = ((nameRecord != null) ? nameRecord.Value : null));
				}
				return text;
			}
		}

		// Token: 0x06000232 RID: 562 RVA: 0x0000C8CF File Offset: 0x0000AACF
		private OpenTypeNameTable ReadNameTable(OpenTypeFontReader reader)
		{
			return reader.TryReadTable(this.nameTableRecord.Value) as OpenTypeNameTable;
		}

		// Token: 0x06000233 RID: 563 RVA: 0x0000C8E8 File Offset: 0x0000AAE8
		private OpenTypeNameTable.NameRecord FindBestNameRecord(OpenTypeNameTable.NameRecord.NameType type)
		{
			return (from r in this.NameTable.NameRecords
				where r.NameID == type
				orderby OpenTypeFont.<FindBestNameRecord>g__RankPlatform|25_0(r) + OpenTypeFont.<FindBestNameRecord>g__RankLanguage|25_1(r) descending
				select r).FirstOrDefault<OpenTypeNameTable.NameRecord>();
		}

		// Token: 0x170000A7 RID: 167
		// (get) Token: 0x06000234 RID: 564 RVA: 0x0000C947 File Offset: 0x0000AB47
		public IEnumerable<TableRecord> Tables
		{
			get
			{
				return this.tables;
			}
		}

		// Token: 0x06000235 RID: 565 RVA: 0x0000C94F File Offset: 0x0000AB4F
		protected virtual void Dispose(bool disposing)
		{
			if (!this.disposedValue)
			{
				if (disposing)
				{
					OpenTypeFontReader reader = this.Reader;
					if (reader != null)
					{
						reader.Dispose();
					}
				}
				this.disposedValue = true;
			}
		}

		// Token: 0x06000236 RID: 566 RVA: 0x0000C974 File Offset: 0x0000AB74
		public void Dispose()
		{
			this.Dispose(true);
		}

		// Token: 0x06000237 RID: 567 RVA: 0x0000C980 File Offset: 0x0000AB80
		[CompilerGenerated]
		internal static int <FindBestNameRecord>g__RankPlatform|25_0(OpenTypeNameTable.NameRecord record)
		{
			switch (record.PlatformID)
			{
			case OpenTypeNameTable.NameRecord.Platform.Unicode:
				return 2000;
			case OpenTypeNameTable.NameRecord.Platform.Macintosh:
				return 1000;
			case OpenTypeNameTable.NameRecord.Platform.Windows:
				return 3000;
			}
			return 0;
		}

		// Token: 0x06000238 RID: 568 RVA: 0x0000C9C8 File Offset: 0x0000ABC8
		[CompilerGenerated]
		internal static int <FindBestNameRecord>g__RankLanguage|25_1(OpenTypeNameTable.NameRecord record)
		{
			int num;
			if (record != null && record.PlatformID == OpenTypeNameTable.NameRecord.Platform.Windows && record.LanguageID == 1033)
			{
				num = 100;
			}
			else
			{
				num = 0;
			}
			return num;
		}

		// Token: 0x04000071 RID: 113
		private readonly OffsetTable offsetTable;

		// Token: 0x04000072 RID: 114
		private readonly TableRecord[] tables;

		// Token: 0x04000073 RID: 115
		private readonly TableRecord? nameTableRecord;

		// Token: 0x04000075 RID: 117
		private OpenTypeNameTable nameTable;

		// Token: 0x04000076 RID: 118
		private string uniqueId;

		// Token: 0x04000077 RID: 119
		private string family;

		// Token: 0x04000078 RID: 120
		private string subfamily;

		// Token: 0x04000079 RID: 121
		private string fullName;

		// Token: 0x0400007A RID: 122
		private bool disposedValue;
	}
}
