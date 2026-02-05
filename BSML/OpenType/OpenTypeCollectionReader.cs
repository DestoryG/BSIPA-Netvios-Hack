using System;
using System.IO;
using System.Text;

namespace BeatSaberMarkupLanguage.OpenType
{
	// Token: 0x0200007A RID: 122
	public class OpenTypeCollectionReader : OpenTypeFontReader
	{
		// Token: 0x06000223 RID: 547 RVA: 0x0000C5A9 File Offset: 0x0000A7A9
		public OpenTypeCollectionReader(Stream input)
			: base(input)
		{
		}

		// Token: 0x06000224 RID: 548 RVA: 0x0000C5B2 File Offset: 0x0000A7B2
		public OpenTypeCollectionReader(Stream input, Encoding encoding)
			: base(input, encoding)
		{
		}

		// Token: 0x06000225 RID: 549 RVA: 0x0000C5BC File Offset: 0x0000A7BC
		public OpenTypeCollectionReader(Stream input, Encoding encoding, bool leaveOpen)
			: base(input, encoding, leaveOpen)
		{
		}

		// Token: 0x06000226 RID: 550 RVA: 0x0000C5C8 File Offset: 0x0000A7C8
		public CollectionHeader ReadCollectionHeader()
		{
			CollectionHeader collectionHeader = new CollectionHeader
			{
				TTCTag = base.ReadTag(),
				MajorVersion = base.ReadUInt16(),
				MinorVersion = base.ReadUInt16(),
				NumFonts = base.ReadUInt32()
			};
			collectionHeader.OffsetTable = new uint[collectionHeader.NumFonts];
			for (uint num = 0U; num < collectionHeader.NumFonts; num += 1U)
			{
				collectionHeader.OffsetTable[(int)num] = base.ReadOffset32();
			}
			if (collectionHeader.MajorVersion == 2)
			{
				collectionHeader.DSIGTag = base.ReadUInt32();
				collectionHeader.DSIGLength = base.ReadUInt32();
				collectionHeader.DSIGOffset = base.ReadUInt32();
			}
			return collectionHeader;
		}

		// Token: 0x06000227 RID: 551 RVA: 0x0000C67C File Offset: 0x0000A87C
		public OffsetTable[] ReadOffsetTables(CollectionHeader header)
		{
			OffsetTable[] array = new OffsetTable[header.NumFonts];
			for (uint num = 0U; num < header.NumFonts; num += 1U)
			{
				this.BaseStream.Position = (long)((ulong)header.OffsetTable[(int)num]);
				array[(int)num] = base.ReadOffsetTable();
			}
			return array;
		}

		// Token: 0x06000228 RID: 552 RVA: 0x0000C6CC File Offset: 0x0000A8CC
		public OpenTypeFont[] ReadFonts(CollectionHeader header, bool lazyLoad = true)
		{
			OpenTypeFont[] array = new OpenTypeFont[header.NumFonts];
			for (uint num = 0U; num < header.NumFonts; num += 1U)
			{
				this.BaseStream.Position = (long)((ulong)header.OffsetTable[(int)num]);
				array[(int)num] = new OpenTypeFont(this, lazyLoad);
			}
			return array;
		}
	}
}
