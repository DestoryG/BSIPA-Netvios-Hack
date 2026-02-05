using System;
using System.IO;
using System.Text;

namespace BeatSaberMarkupLanguage.OpenType
{
	// Token: 0x0200007D RID: 125
	public abstract class OpenTypeReader : BinaryReader
	{
		// Token: 0x06000241 RID: 577 RVA: 0x0000CB75 File Offset: 0x0000AD75
		protected OpenTypeReader(Stream input)
			: base(input)
		{
		}

		// Token: 0x06000242 RID: 578 RVA: 0x0000CB7E File Offset: 0x0000AD7E
		protected OpenTypeReader(Stream input, Encoding encoding)
			: base(input, encoding)
		{
		}

		// Token: 0x06000243 RID: 579 RVA: 0x0000CB88 File Offset: 0x0000AD88
		protected OpenTypeReader(Stream input, Encoding encoding, bool leaveOpen)
			: base(input, encoding, leaveOpen)
		{
		}

		// Token: 0x06000244 RID: 580 RVA: 0x0000CB93 File Offset: 0x0000AD93
		public static byte[] FromBigEndian(byte[] bytes)
		{
			if (BitConverter.IsLittleEndian)
			{
				Array.Reverse(bytes);
			}
			return bytes;
		}

		// Token: 0x06000245 RID: 581 RVA: 0x0000CBA3 File Offset: 0x0000ADA3
		public byte ReadUInt8()
		{
			return this.ReadByte();
		}

		// Token: 0x06000246 RID: 582 RVA: 0x0000CBAB File Offset: 0x0000ADAB
		public sbyte ReadInt8()
		{
			return this.ReadSByte();
		}

		// Token: 0x06000247 RID: 583 RVA: 0x0000CBB3 File Offset: 0x0000ADB3
		public new ushort ReadUInt16()
		{
			return BitConverter.ToUInt16(OpenTypeReader.FromBigEndian(this.ReadBytes(2)), 0);
		}

		// Token: 0x06000248 RID: 584 RVA: 0x0000CBC7 File Offset: 0x0000ADC7
		public new short ReadInt16()
		{
			return BitConverter.ToInt16(OpenTypeReader.FromBigEndian(this.ReadBytes(2)), 0);
		}

		// Token: 0x06000249 RID: 585 RVA: 0x0000CBDB File Offset: 0x0000ADDB
		public new uint ReadUInt32()
		{
			return BitConverter.ToUInt32(OpenTypeReader.FromBigEndian(this.ReadBytes(4)), 0);
		}

		// Token: 0x0600024A RID: 586 RVA: 0x0000CBEF File Offset: 0x0000ADEF
		public new int ReadInt32()
		{
			return BitConverter.ToInt32(OpenTypeReader.FromBigEndian(this.ReadBytes(4)), 0);
		}

		// Token: 0x0600024B RID: 587 RVA: 0x0000CC03 File Offset: 0x0000AE03
		public int ReadFixed()
		{
			return this.ReadInt32();
		}

		// Token: 0x0600024C RID: 588 RVA: 0x0000CC0B File Offset: 0x0000AE0B
		public short ReadFWord()
		{
			return this.ReadInt16();
		}

		// Token: 0x0600024D RID: 589 RVA: 0x0000CC13 File Offset: 0x0000AE13
		public ushort ReadUFWord()
		{
			return this.ReadUInt16();
		}

		// Token: 0x0600024E RID: 590 RVA: 0x0000CC0B File Offset: 0x0000AE0B
		public short ReadF2Dot14()
		{
			return this.ReadInt16();
		}

		// Token: 0x0600024F RID: 591 RVA: 0x0000CC1B File Offset: 0x0000AE1B
		public long ReadLongDateTime()
		{
			return BitConverter.ToInt64(OpenTypeReader.FromBigEndian(this.ReadBytes(8)), 0);
		}

		// Token: 0x06000250 RID: 592 RVA: 0x0000CC2F File Offset: 0x0000AE2F
		public OpenTypeTag ReadTag()
		{
			return new OpenTypeTag(this.ReadBytes(4));
		}

		// Token: 0x06000251 RID: 593 RVA: 0x0000CC13 File Offset: 0x0000AE13
		public ushort ReadOffset16()
		{
			return this.ReadUInt16();
		}

		// Token: 0x06000252 RID: 594 RVA: 0x0000CC3D File Offset: 0x0000AE3D
		public uint ReadOffset32()
		{
			return this.ReadUInt32();
		}

		// Token: 0x06000253 RID: 595 RVA: 0x0000CC48 File Offset: 0x0000AE48
		public static OpenTypeReader For(Stream stream, Encoding enc = null, bool leaveOpen = false)
		{
			if (enc == null)
			{
				enc = Encoding.Default;
			}
			long position = stream.Position;
			OpenTypeReader openTypeReader;
			using (BinaryReader binaryReader = new BinaryReader(stream, enc, true))
			{
				uint num = BitConverter.ToUInt32(OpenTypeReader.FromBigEndian(binaryReader.ReadBytes(4)), 0);
				stream.Position = position;
				OpenTypeFontReader openTypeFontReader;
				if (num != 65536U)
				{
					if (num != 1330926671U)
					{
						if (num == 1953784678U)
						{
							openTypeFontReader = new OpenTypeCollectionReader(stream, enc, leaveOpen);
						}
						else
						{
							openTypeFontReader = null;
						}
					}
					else
					{
						openTypeFontReader = new OpenTypeFontReader(stream, enc, leaveOpen);
					}
				}
				else
				{
					openTypeFontReader = new OpenTypeFontReader(stream, enc, leaveOpen);
				}
				openTypeReader = openTypeFontReader;
			}
			return openTypeReader;
		}
	}
}
