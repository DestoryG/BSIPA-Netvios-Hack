using System;
using System.Runtime.CompilerServices;
using System.Text;
using I18N.Common;

namespace I18N.West
{
	// Token: 0x02000009 RID: 9
	[Serializable]
	public class CP1252 : ByteEncoding
	{
		// Token: 0x06000016 RID: 22 RVA: 0x00003FAC File Offset: 0x000021AC
		public CP1252()
			: base(1252, CP1252.ToChars, "Western European (Windows)", "iso-8859-1", "Windows-1252", "Windows-1252", true, true, true, true, 1252)
		{
		}

		// Token: 0x06000017 RID: 23 RVA: 0x00003FE6 File Offset: 0x000021E6
		public unsafe override int GetByteCountImpl(char* chars, int count)
		{
			if (base.EncoderFallback != null)
			{
				return this.GetBytesImpl(chars, count, null, 0);
			}
			return count;
		}

		// Token: 0x06000018 RID: 24 RVA: 0x00004000 File Offset: 0x00002200
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

		// Token: 0x06000019 RID: 25 RVA: 0x0000403C File Offset: 0x0000223C
		protected unsafe override void ToBytes(char* chars, int charCount, byte* bytes, int byteCount)
		{
			if (bytes == null)
			{
				throw new ArgumentNullException("bytes");
			}
			this.GetBytesImpl(chars, charCount, bytes, byteCount);
		}

		// Token: 0x0600001A RID: 26 RVA: 0x0000405C File Offset: 0x0000225C
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
					if (num3 <= 382)
					{
						if (num3 <= 352)
						{
							if (num3 <= 338)
							{
								switch (num3)
								{
								case 129:
								case 141:
								case 143:
								case 144:
								case 157:
								case 160:
								case 161:
								case 162:
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
								case 175:
								case 176:
								case 177:
								case 178:
								case 179:
								case 180:
								case 181:
								case 182:
								case 183:
								case 184:
								case 185:
								case 186:
								case 187:
								case 188:
								case 189:
								case 190:
								case 191:
								case 192:
								case 193:
								case 194:
								case 195:
								case 196:
								case 197:
								case 198:
								case 199:
								case 200:
								case 201:
								case 202:
								case 203:
								case 204:
								case 205:
								case 206:
								case 207:
								case 208:
								case 209:
								case 210:
								case 211:
								case 212:
								case 213:
								case 214:
								case 215:
								case 216:
								case 217:
								case 218:
								case 219:
								case 220:
								case 221:
								case 222:
								case 223:
								case 224:
								case 225:
								case 226:
								case 227:
								case 228:
								case 229:
								case 230:
								case 231:
								case 232:
								case 233:
								case 234:
								case 235:
								case 236:
								case 237:
								case 238:
								case 239:
								case 240:
								case 241:
								case 242:
								case 243:
								case 244:
								case 245:
								case 246:
								case 247:
								case 248:
								case 249:
								case 250:
								case 251:
								case 252:
								case 253:
								case 254:
								case 255:
									goto IL_04D5;
								case 130:
								case 131:
								case 132:
								case 133:
								case 134:
								case 135:
								case 136:
								case 137:
								case 138:
								case 139:
								case 140:
								case 142:
								case 145:
								case 146:
								case 147:
								case 148:
								case 149:
								case 150:
								case 151:
								case 152:
								case 153:
								case 154:
								case 155:
								case 156:
								case 158:
								case 159:
									break;
								default:
									if (num3 == 338)
									{
										num3 = 140;
										goto IL_04D5;
									}
									break;
								}
							}
							else
							{
								if (num3 == 339)
								{
									num3 = 156;
									goto IL_04D5;
								}
								if (num3 == 352)
								{
									num3 = 138;
									goto IL_04D5;
								}
							}
						}
						else if (num3 <= 376)
						{
							if (num3 == 353)
							{
								num3 = 154;
								goto IL_04D5;
							}
							if (num3 == 376)
							{
								num3 = 159;
								goto IL_04D5;
							}
						}
						else
						{
							if (num3 == 381)
							{
								num3 = 142;
								goto IL_04D5;
							}
							if (num3 == 382)
							{
								num3 = 158;
								goto IL_04D5;
							}
						}
					}
					else if (num3 <= 8230)
					{
						if (num3 <= 710)
						{
							if (num3 == 402)
							{
								num3 = 131;
								goto IL_04D5;
							}
							if (num3 == 710)
							{
								num3 = 136;
								goto IL_04D5;
							}
						}
						else
						{
							if (num3 == 732)
							{
								num3 = 152;
								goto IL_04D5;
							}
							switch (num3)
							{
							case 8211:
								num3 = 150;
								goto IL_04D5;
							case 8212:
								num3 = 151;
								goto IL_04D5;
							case 8216:
								num3 = 145;
								goto IL_04D5;
							case 8217:
								num3 = 146;
								goto IL_04D5;
							case 8218:
								num3 = 130;
								goto IL_04D5;
							case 8220:
								num3 = 147;
								goto IL_04D5;
							case 8221:
								num3 = 148;
								goto IL_04D5;
							case 8222:
								num3 = 132;
								goto IL_04D5;
							case 8224:
								num3 = 134;
								goto IL_04D5;
							case 8225:
								num3 = 135;
								goto IL_04D5;
							case 8226:
								num3 = 149;
								goto IL_04D5;
							case 8230:
								num3 = 133;
								goto IL_04D5;
							}
						}
					}
					else if (num3 <= 8249)
					{
						if (num3 == 8240)
						{
							num3 = 137;
							goto IL_04D5;
						}
						if (num3 == 8249)
						{
							num3 = 139;
							goto IL_04D5;
						}
					}
					else
					{
						if (num3 == 8250)
						{
							num3 = 155;
							goto IL_04D5;
						}
						if (num3 == 8364)
						{
							num3 = 128;
							goto IL_04D5;
						}
						if (num3 == 8482)
						{
							num3 = 153;
							goto IL_04D5;
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
				IL_04D5:
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

		// Token: 0x0400002D RID: 45
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
			'‚', 'ƒ', '„', '…', '†', '‡', 'ˆ', '‰', 'Š', '‹',
			'Œ', '\u008d', 'Ž', '\u008f', '\u0090', '‘', '’', '“', '”', '•',
			'–', '—', '\u02dc', '™', 'š', '›', 'œ', '\u009d', 'ž', 'Ÿ',
			'\u00a0', '¡', '¢', '£', '¤', '¥', '¦', '§', '\u00a8', '©',
			'ª', '«', '¬', '\u00ad', '®', '\u00af', '°', '±', '²', '³',
			'\u00b4', 'µ', '¶', '·', '\u00b8', '¹', 'º', '»', '¼', '½',
			'¾', '¿', 'À', 'Á', 'Â', 'Ã', 'Ä', 'Å', 'Æ', 'Ç',
			'È', 'É', 'Ê', 'Ë', 'Ì', 'Í', 'Î', 'Ï', 'Ð', 'Ñ',
			'Ò', 'Ó', 'Ô', 'Õ', 'Ö', '×', 'Ø', 'Ù', 'Ú', 'Û',
			'Ü', 'Ý', 'Þ', 'ß', 'à', 'á', 'â', 'ã', 'ä', 'å',
			'æ', 'ç', 'è', 'é', 'ê', 'ë', 'ì', 'í', 'î', 'ï',
			'ð', 'ñ', 'ò', 'ó', 'ô', 'õ', 'ö', '÷', 'ø', 'ù',
			'ú', 'û', 'ü', 'ý', 'þ', 'ÿ'
		};
	}
}
