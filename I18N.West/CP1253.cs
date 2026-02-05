using System;
using System.Runtime.CompilerServices;
using System.Text;
using I18N.Common;

namespace I18N.West
{
	// Token: 0x0200000B RID: 11
	[Serializable]
	public class CP1253 : ByteEncoding
	{
		// Token: 0x0600001D RID: 29 RVA: 0x00004588 File Offset: 0x00002788
		public CP1253()
			: base(1253, CP1253.ToChars, "Greek (Windows)", "iso-8859-7", "windows-1253", "windows-1253", true, true, true, true, 1253)
		{
		}

		// Token: 0x0600001E RID: 30 RVA: 0x000045C2 File Offset: 0x000027C2
		public unsafe override int GetByteCountImpl(char* chars, int count)
		{
			if (base.EncoderFallback != null)
			{
				return this.GetBytesImpl(chars, count, null, 0);
			}
			return count;
		}

		// Token: 0x0600001F RID: 31 RVA: 0x000045DC File Offset: 0x000027DC
		public unsafe override int GetByteCount(string s)
		{
			if (base.EncoderFallback != null)
			{
				char* ptr = s;
				if (ptr != null)
				{
					ptr += RuntimeHelpers.OffsetToStringData / 2;
				}
				return this.GetBytesImpl(ptr, s.Length, null, 0);
			}
			return s.Length;
		}

		// Token: 0x06000020 RID: 32 RVA: 0x00004618 File Offset: 0x00002818
		protected unsafe override void ToBytes(char* chars, int charCount, byte* bytes, int byteCount)
		{
			if (bytes == null)
			{
				throw new ArgumentNullException("bytes");
			}
			this.GetBytesImpl(chars, charCount, bytes, byteCount);
		}

		// Token: 0x06000021 RID: 33 RVA: 0x00004638 File Offset: 0x00002838
		public unsafe override int GetBytesImpl(char* chars, int charCount, byte* bytes, int byteCount)
		{
			int num = 0;
			int num2 = 0;
			EncoderFallbackBuffer encoderFallbackBuffer = null;
			while (charCount > 0)
			{
				int num3 = (int)chars[num];
				if (num3 >= 128)
				{
					if (num3 <= 8230)
					{
						if (num3 <= 402)
						{
							switch (num3)
							{
							case 129:
							case 136:
							case 138:
							case 140:
							case 141:
							case 142:
							case 143:
							case 144:
							case 152:
							case 154:
							case 156:
							case 157:
							case 158:
							case 159:
							case 160:
							case 163:
							case 164:
							case 165:
							case 166:
							case 167:
							case 168:
							case 169:
							case 170:
							case 171:
							case 172:
							case 173:
							case 174:
							case 176:
							case 177:
							case 178:
							case 179:
							case 181:
							case 182:
							case 183:
							case 187:
							case 189:
								goto IL_049E;
							case 130:
							case 131:
							case 132:
							case 133:
							case 134:
							case 135:
							case 137:
							case 139:
							case 145:
							case 146:
							case 147:
							case 148:
							case 149:
							case 150:
							case 151:
							case 153:
							case 155:
							case 161:
							case 162:
							case 175:
							case 180:
							case 184:
							case 185:
							case 186:
							case 188:
								break;
							default:
								if (num3 == 402)
								{
									num3 = 131;
									goto IL_049E;
								}
								break;
							}
						}
						else
						{
							switch (num3)
							{
							case 900:
								num3 = 180;
								goto IL_049E;
							case 901:
								num3 = 161;
								goto IL_049E;
							case 902:
								num3 = 162;
								goto IL_049E;
							case 903:
							case 907:
							case 909:
							case 930:
							case 975:
							case 976:
							case 977:
							case 978:
							case 979:
							case 980:
								break;
							case 904:
								num3 = 184;
								goto IL_049E;
							case 905:
								num3 = 185;
								goto IL_049E;
							case 906:
								num3 = 186;
								goto IL_049E;
							case 908:
								num3 = 188;
								goto IL_049E;
							case 910:
							case 911:
							case 912:
							case 913:
							case 914:
							case 915:
							case 916:
							case 917:
							case 918:
							case 919:
							case 920:
							case 921:
							case 922:
							case 923:
							case 924:
							case 925:
							case 926:
							case 927:
							case 928:
							case 929:
								num3 -= 720;
								goto IL_049E;
							case 931:
							case 932:
							case 933:
							case 934:
							case 935:
							case 936:
							case 937:
							case 938:
							case 939:
							case 940:
							case 941:
							case 942:
							case 943:
							case 944:
							case 945:
							case 946:
							case 947:
							case 948:
							case 949:
							case 950:
							case 951:
							case 952:
							case 953:
							case 954:
							case 955:
							case 956:
							case 957:
							case 958:
							case 959:
							case 960:
							case 961:
							case 962:
							case 963:
							case 964:
							case 965:
							case 966:
							case 967:
							case 968:
							case 969:
							case 970:
							case 971:
							case 972:
							case 973:
							case 974:
								num3 -= 720;
								goto IL_049E;
							case 981:
								num3 = 246;
								goto IL_049E;
							default:
								switch (num3)
								{
								case 8211:
									num3 = 150;
									goto IL_049E;
								case 8212:
									num3 = 151;
									goto IL_049E;
								case 8213:
									num3 = 175;
									goto IL_049E;
								case 8216:
									num3 = 145;
									goto IL_049E;
								case 8217:
									num3 = 146;
									goto IL_049E;
								case 8218:
									num3 = 130;
									goto IL_049E;
								case 8220:
									num3 = 147;
									goto IL_049E;
								case 8221:
									num3 = 148;
									goto IL_049E;
								case 8222:
									num3 = 132;
									goto IL_049E;
								case 8224:
									num3 = 134;
									goto IL_049E;
								case 8225:
									num3 = 135;
									goto IL_049E;
								case 8226:
									num3 = 149;
									goto IL_049E;
								case 8230:
									num3 = 133;
									goto IL_049E;
								}
								break;
							}
						}
					}
					else if (num3 <= 8249)
					{
						if (num3 == 8240)
						{
							num3 = 137;
							goto IL_049E;
						}
						if (num3 == 8249)
						{
							num3 = 139;
							goto IL_049E;
						}
					}
					else
					{
						if (num3 == 8250)
						{
							num3 = 155;
							goto IL_049E;
						}
						if (num3 == 8364)
						{
							num3 = 128;
							goto IL_049E;
						}
						if (num3 == 8482)
						{
							num3 = 153;
							goto IL_049E;
						}
					}
					if (num3 < 65281 || num3 > 65374)
					{
						base.HandleFallback(ref encoderFallbackBuffer, chars, ref num, ref charCount, bytes, ref num2, ref byteCount);
						num++;
						charCount--;
						continue;
					}
					num3 -= 65248;
				}
				IL_049E:
				if (bytes != null)
				{
					bytes[num2] = (byte)num3;
				}
				num2++;
				byteCount--;
				num++;
				charCount--;
			}
			return num2;
		}

		// Token: 0x0400002E RID: 46
		private static readonly char[] ToChars = new char[]
		{
			'\0', '\u0001', '\u0002', '\u0003', '\u0004', '\u0005', '\u0006', '\a', '\b', '\t',
			'\n', '\v', '\f', '\r', '\u000e', '\u000f', '\u0010', '\u0011', '\u0012', '\u0013',
			'\u0014', '\u0015', '\u0016', '\u0017', '\u0018', '\u0019', '\u001a', '\u001b', '\u001c', '\u001d',
			'\u001e', '\u001f', ' ', '!', '"', '#', '$', '%', '&', '\'',
			'(', ')', '*', '+', ',', '-', '.', '/', '0', '1',
			'2', '3', '4', '5', '6', '7', '8', '9', ':', ';',
			'<', '=', '>', '?', '@', 'A', 'B', 'C', 'D', 'E',
			'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O',
			'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y',
			'Z', '[', '\\', ']', '^', '_', '`', 'a', 'b', 'c',
			'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm',
			'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w',
			'x', 'y', 'z', '{', '|', '}', '~', '\u007f', '€', '\u0081',
			'‚', 'ƒ', '„', '…', '†', '‡', '\u0088', '‰', '\u008a', '‹',
			'\u008c', '\u008d', '\u008e', '\u008f', '\u0090', '‘', '’', '“', '”', '•',
			'–', '—', '\u0098', '™', '\u009a', '›', '\u009c', '\u009d', '\u009e', '\u009f',
			'\u00a0', '\u0385', 'Ά', '£', '¤', '¥', '¦', '§', '\u00a8', '©',
			'ª', '«', '¬', '\u00ad', '®', '―', '°', '±', '²', '³',
			'\u0384', 'µ', '¶', '·', 'Έ', 'Ή', 'Ί', '»', 'Ό', '½',
			'Ύ', 'Ώ', 'ΐ', 'Α', 'Β', 'Γ', 'Δ', 'Ε', 'Ζ', 'Η',
			'Θ', 'Ι', 'Κ', 'Λ', 'Μ', 'Ν', 'Ξ', 'Ο', 'Π', 'Ρ',
			'?', 'Σ', 'Τ', 'Υ', 'Φ', 'Χ', 'Ψ', 'Ω', 'Ϊ', 'Ϋ',
			'ά', 'έ', 'ή', 'ί', 'ΰ', 'α', 'β', 'γ', 'δ', 'ε',
			'ζ', 'η', 'θ', 'ι', 'κ', 'λ', 'μ', 'ν', 'ξ', 'ο',
			'π', 'ρ', 'ς', 'σ', 'τ', 'υ', 'φ', 'χ', 'ψ', 'ω',
			'ϊ', 'ϋ', 'ό', 'ύ', 'ώ', '?'
		};
	}
}
