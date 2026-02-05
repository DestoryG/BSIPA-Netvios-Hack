using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace BeatSaberMarkupLanguage.OpenType
{
	// Token: 0x0200007F RID: 127
	public class OpenTypeNameTable : OpenTypeTable
	{
		// Token: 0x170000A8 RID: 168
		// (get) Token: 0x06000256 RID: 598 RVA: 0x0000CCE8 File Offset: 0x0000AEE8
		// (set) Token: 0x06000257 RID: 599 RVA: 0x0000CCF0 File Offset: 0x0000AEF0
		public OpenTypeNameTable.FormatEnum Format { get; private set; }

		// Token: 0x170000A9 RID: 169
		// (get) Token: 0x06000258 RID: 600 RVA: 0x0000CCF9 File Offset: 0x0000AEF9
		// (set) Token: 0x06000259 RID: 601 RVA: 0x0000CD01 File Offset: 0x0000AF01
		public ushort Count { get; private set; }

		// Token: 0x170000AA RID: 170
		// (get) Token: 0x0600025A RID: 602 RVA: 0x0000CD0A File Offset: 0x0000AF0A
		// (set) Token: 0x0600025B RID: 603 RVA: 0x0000CD12 File Offset: 0x0000AF12
		public ushort StringOffset { get; private set; }

		// Token: 0x170000AB RID: 171
		// (get) Token: 0x0600025C RID: 604 RVA: 0x0000CD1B File Offset: 0x0000AF1B
		// (set) Token: 0x0600025D RID: 605 RVA: 0x0000CD23 File Offset: 0x0000AF23
		public IReadOnlyList<OpenTypeNameTable.NameRecord> NameRecords { get; private set; }

		// Token: 0x170000AC RID: 172
		// (get) Token: 0x0600025E RID: 606 RVA: 0x0000CD2C File Offset: 0x0000AF2C
		// (set) Token: 0x0600025F RID: 607 RVA: 0x0000CD34 File Offset: 0x0000AF34
		public ushort LangTagCount { get; private set; }

		// Token: 0x170000AD RID: 173
		// (get) Token: 0x06000260 RID: 608 RVA: 0x0000CD3D File Offset: 0x0000AF3D
		// (set) Token: 0x06000261 RID: 609 RVA: 0x0000CD45 File Offset: 0x0000AF45
		public IReadOnlyList<OpenTypeNameTable.LangTagRecord> LangTagRecords { get; private set; } = new List<OpenTypeNameTable.LangTagRecord>();

		// Token: 0x06000262 RID: 610 RVA: 0x0000CD50 File Offset: 0x0000AF50
		public override void ReadFrom(OpenTypeReader reader, uint length)
		{
			this.Format = (OpenTypeNameTable.FormatEnum)reader.ReadUInt16();
			this.Count = reader.ReadUInt16();
			this.StringOffset = reader.ReadUInt16();
			uint num = (uint)(this.StringOffset - 6);
			this.NameRecords = this.ReadNameRecords(reader);
			num -= (uint)(this.Count * 12);
			if (this.Format == OpenTypeNameTable.FormatEnum.LangTagged)
			{
				this.LangTagCount = reader.ReadUInt16();
				num -= 2U;
				this.LangTagRecords = this.ReadLangTagRecords(reader);
				num -= (uint)(this.LangTagCount * 4);
			}
			reader.BaseStream.Seek((long)((ulong)num), SeekOrigin.Current);
			long position = reader.BaseStream.Position;
			foreach (OpenTypeNameTable.NameRecord nameRecord in this.NameRecords)
			{
				reader.BaseStream.Position = position + (long)((ulong)nameRecord.Offset);
				byte[] array = reader.ReadBytes((int)nameRecord.Length);
				if (nameRecord.PlatformID == OpenTypeNameTable.NameRecord.Platform.Windows || nameRecord.PlatformID == OpenTypeNameTable.NameRecord.Platform.Unicode)
				{
					nameRecord.Value = Encoding.BigEndianUnicode.GetString(array);
				}
				else
				{
					nameRecord.Value = Encoding.UTF8.GetString(array);
				}
			}
			foreach (OpenTypeNameTable.LangTagRecord langTagRecord in this.LangTagRecords)
			{
				reader.BaseStream.Position = position + (long)((ulong)langTagRecord.Offset);
				byte[] array2 = reader.ReadBytes((int)langTagRecord.Length);
				langTagRecord.Value = Encoding.UTF8.GetString(array2);
			}
		}

		// Token: 0x06000263 RID: 611 RVA: 0x0000CEF4 File Offset: 0x0000B0F4
		private IReadOnlyList<OpenTypeNameTable.NameRecord> ReadNameRecords(OpenTypeReader reader)
		{
			List<OpenTypeNameTable.NameRecord> list = new List<OpenTypeNameTable.NameRecord>();
			for (int i = 0; i < (int)this.Count; i++)
			{
				list.Add(new OpenTypeNameTable.NameRecord
				{
					PlatformID = (OpenTypeNameTable.NameRecord.Platform)reader.ReadUInt16(),
					EncodingID = reader.ReadUInt16(),
					LanguageID = reader.ReadUInt16(),
					NameID = (OpenTypeNameTable.NameRecord.NameType)reader.ReadUInt16(),
					Length = reader.ReadUInt16(),
					Offset = reader.ReadOffset16()
				});
			}
			return list;
		}

		// Token: 0x06000264 RID: 612 RVA: 0x0000CF6C File Offset: 0x0000B16C
		private IReadOnlyList<OpenTypeNameTable.LangTagRecord> ReadLangTagRecords(OpenTypeReader reader)
		{
			List<OpenTypeNameTable.LangTagRecord> list = new List<OpenTypeNameTable.LangTagRecord>();
			for (int i = 0; i < (int)this.LangTagCount; i++)
			{
				list.Add(new OpenTypeNameTable.LangTagRecord
				{
					Length = reader.ReadUInt16(),
					Offset = reader.ReadOffset16()
				});
			}
			return list;
		}

		// Token: 0x02000132 RID: 306
		public enum FormatEnum : ushort
		{
			// Token: 0x0400029B RID: 667
			Default,
			// Token: 0x0400029C RID: 668
			LangTagged
		}

		// Token: 0x02000133 RID: 307
		public class NameRecord
		{
			// Token: 0x1700013E RID: 318
			// (get) Token: 0x0600063A RID: 1594 RVA: 0x000168A1 File Offset: 0x00014AA1
			// (set) Token: 0x0600063B RID: 1595 RVA: 0x000168A9 File Offset: 0x00014AA9
			public OpenTypeNameTable.NameRecord.Platform PlatformID { get; set; }

			// Token: 0x1700013F RID: 319
			// (get) Token: 0x0600063C RID: 1596 RVA: 0x000168B2 File Offset: 0x00014AB2
			// (set) Token: 0x0600063D RID: 1597 RVA: 0x000168BA File Offset: 0x00014ABA
			public ushort EncodingID { get; set; }

			// Token: 0x17000140 RID: 320
			// (get) Token: 0x0600063E RID: 1598 RVA: 0x000168C3 File Offset: 0x00014AC3
			// (set) Token: 0x0600063F RID: 1599 RVA: 0x000168CB File Offset: 0x00014ACB
			public ushort LanguageID { get; set; }

			// Token: 0x17000141 RID: 321
			// (get) Token: 0x06000640 RID: 1600 RVA: 0x000168D4 File Offset: 0x00014AD4
			// (set) Token: 0x06000641 RID: 1601 RVA: 0x000168DC File Offset: 0x00014ADC
			public OpenTypeNameTable.NameRecord.NameType NameID { get; set; }

			// Token: 0x17000142 RID: 322
			// (get) Token: 0x06000642 RID: 1602 RVA: 0x000168E5 File Offset: 0x00014AE5
			// (set) Token: 0x06000643 RID: 1603 RVA: 0x000168ED File Offset: 0x00014AED
			public ushort Length { get; set; }

			// Token: 0x17000143 RID: 323
			// (get) Token: 0x06000644 RID: 1604 RVA: 0x000168F6 File Offset: 0x00014AF6
			// (set) Token: 0x06000645 RID: 1605 RVA: 0x000168FE File Offset: 0x00014AFE
			public ushort Offset { get; set; }

			// Token: 0x17000144 RID: 324
			// (get) Token: 0x06000646 RID: 1606 RVA: 0x00016907 File Offset: 0x00014B07
			// (set) Token: 0x06000647 RID: 1607 RVA: 0x0001690F File Offset: 0x00014B0F
			public string Value { get; set; }

			// Token: 0x0400029D RID: 669
			public const uint Size = 12U;

			// Token: 0x0400029E RID: 670
			public const ushort USEnglishLangID = 1033;

			// Token: 0x0200015E RID: 350
			public enum Platform : ushort
			{
				// Token: 0x0400031B RID: 795
				Unicode,
				// Token: 0x0400031C RID: 796
				Macintosh,
				// Token: 0x0400031D RID: 797
				ISO,
				// Token: 0x0400031E RID: 798
				Windows,
				// Token: 0x0400031F RID: 799
				Custom
			}

			// Token: 0x0200015F RID: 351
			public enum NameType : ushort
			{
				// Token: 0x04000321 RID: 801
				Copyright,
				// Token: 0x04000322 RID: 802
				FontFamily,
				// Token: 0x04000323 RID: 803
				FontSubfamily,
				// Token: 0x04000324 RID: 804
				UniqueId,
				// Token: 0x04000325 RID: 805
				FullFontName,
				// Token: 0x04000326 RID: 806
				Version,
				// Token: 0x04000327 RID: 807
				PostScriptName,
				// Token: 0x04000328 RID: 808
				Trademark,
				// Token: 0x04000329 RID: 809
				Manufacturer,
				// Token: 0x0400032A RID: 810
				Designer,
				// Token: 0x0400032B RID: 811
				Description,
				// Token: 0x0400032C RID: 812
				VendorURL,
				// Token: 0x0400032D RID: 813
				DesignerURL,
				// Token: 0x0400032E RID: 814
				LicenseDescription,
				// Token: 0x0400032F RID: 815
				LicenseInfoURL,
				// Token: 0x04000330 RID: 816
				Reserved1,
				// Token: 0x04000331 RID: 817
				TypographicFamily,
				// Token: 0x04000332 RID: 818
				TypographicSubfamily,
				// Token: 0x04000333 RID: 819
				CompatibleFull,
				// Token: 0x04000334 RID: 820
				SampleText,
				// Token: 0x04000335 RID: 821
				PostScriptCID,
				// Token: 0x04000336 RID: 822
				WWSFamily,
				// Token: 0x04000337 RID: 823
				WWSSubfamily,
				// Token: 0x04000338 RID: 824
				LightBackgroundPalette,
				// Token: 0x04000339 RID: 825
				DarkBackgroundPalette,
				// Token: 0x0400033A RID: 826
				VariationsPostScriptPrefix
			}
		}

		// Token: 0x02000134 RID: 308
		public class LangTagRecord
		{
			// Token: 0x17000145 RID: 325
			// (get) Token: 0x06000649 RID: 1609 RVA: 0x00016918 File Offset: 0x00014B18
			// (set) Token: 0x0600064A RID: 1610 RVA: 0x00016920 File Offset: 0x00014B20
			public ushort Length { get; set; }

			// Token: 0x17000146 RID: 326
			// (get) Token: 0x0600064B RID: 1611 RVA: 0x00016929 File Offset: 0x00014B29
			// (set) Token: 0x0600064C RID: 1612 RVA: 0x00016931 File Offset: 0x00014B31
			public ushort Offset { get; set; }

			// Token: 0x17000147 RID: 327
			// (get) Token: 0x0600064D RID: 1613 RVA: 0x0001693A File Offset: 0x00014B3A
			// (set) Token: 0x0600064E RID: 1614 RVA: 0x00016942 File Offset: 0x00014B42
			public string Value { get; set; }

			// Token: 0x040002A6 RID: 678
			public const uint Size = 4U;
		}
	}
}
