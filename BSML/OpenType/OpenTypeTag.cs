using System;
using System.Linq;

namespace BeatSaberMarkupLanguage.OpenType
{
	// Token: 0x02000080 RID: 128
	public struct OpenTypeTag
	{
		// Token: 0x170000AE RID: 174
		// (get) Token: 0x06000266 RID: 614 RVA: 0x0000CFC7 File Offset: 0x0000B1C7
		// (set) Token: 0x06000267 RID: 615 RVA: 0x0000CFCF File Offset: 0x0000B1CF
		public byte[] Value { readonly get; set; }

		// Token: 0x170000AF RID: 175
		// (get) Token: 0x06000268 RID: 616 RVA: 0x0000CFD8 File Offset: 0x0000B1D8
		public uint IntValue
		{
			get
			{
				return BitConverter.ToUInt32(this.Value, 0);
			}
		}

		// Token: 0x06000269 RID: 617 RVA: 0x0000CFE6 File Offset: 0x0000B1E6
		public bool Validate()
		{
			if (this.Value.Length == 4)
			{
				return this.Value.All((byte b) => b >= 32 && b <= 126);
			}
			return false;
		}

		// Token: 0x0600026A RID: 618 RVA: 0x0000D020 File Offset: 0x0000B220
		public override bool Equals(object obj)
		{
			if (obj is OpenTypeTag)
			{
				OpenTypeTag openTypeTag = (OpenTypeTag)obj;
				return this.IntValue == openTypeTag.IntValue;
			}
			return false;
		}

		// Token: 0x0600026B RID: 619 RVA: 0x0000D050 File Offset: 0x0000B250
		public override int GetHashCode()
		{
			return 1637310455 + this.IntValue.GetHashCode();
		}

		// Token: 0x0600026C RID: 620 RVA: 0x0000D071 File Offset: 0x0000B271
		public OpenTypeTag(byte[] value)
		{
			this.Value = value;
		}

		// Token: 0x0600026D RID: 621 RVA: 0x0000D07A File Offset: 0x0000B27A
		public static OpenTypeTag FromChars(char[] chrs)
		{
			return new OpenTypeTag(chrs.Select((char c) => (byte)c).ToArray<byte>());
		}

		// Token: 0x0600026E RID: 622 RVA: 0x0000D0AB File Offset: 0x0000B2AB
		public static OpenTypeTag FromString(string str)
		{
			return OpenTypeTag.FromChars(str.ToCharArray(0, 4));
		}

		// Token: 0x0600026F RID: 623 RVA: 0x0000D0BA File Offset: 0x0000B2BA
		public static bool operator ==(OpenTypeTag left, OpenTypeTag right)
		{
			return left.Equals(right);
		}

		// Token: 0x06000270 RID: 624 RVA: 0x0000D0CF File Offset: 0x0000B2CF
		public static bool operator !=(OpenTypeTag left, OpenTypeTag right)
		{
			return !(left == right);
		}

		// Token: 0x04000081 RID: 129
		public static readonly OpenTypeTag NAME = OpenTypeTag.FromString("name");
	}
}
