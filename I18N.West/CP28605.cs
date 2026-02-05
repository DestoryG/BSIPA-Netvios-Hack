using System;
using System.Runtime.CompilerServices;
using System.Text;
using I18N.Common;

namespace I18N.West
{
	// Token: 0x02000013 RID: 19
	[Serializable]
	public class CP28605 : ByteEncoding
	{
		// Token: 0x06000039 RID: 57 RVA: 0x000060C8 File Offset: 0x000042C8
		public CP28605()
			: base(28605, CP28605.ToChars, "Latin 9 (ISO)", "iso-8859-15", "iso-8859-15", "iso-8859-15", false, true, true, true, 1252)
		{
		}

		// Token: 0x0600003A RID: 58 RVA: 0x00006102 File Offset: 0x00004302
		public unsafe override int GetByteCountImpl(char* chars, int count)
		{
			if (base.EncoderFallback != null)
			{
				return this.GetBytesImpl(chars, count, null, 0);
			}
			return count;
		}

		// Token: 0x0600003B RID: 59 RVA: 0x0000611C File Offset: 0x0000431C
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

		// Token: 0x0600003C RID: 60 RVA: 0x00006158 File Offset: 0x00004358
		protected unsafe override void ToBytes(char* chars, int charCount, byte* bytes, int byteCount)
		{
			if (bytes == null)
			{
				throw new ArgumentNullException("bytes");
			}
			this.GetBytesImpl(chars, charCount, bytes, byteCount);
		}

		// Token: 0x0600003D RID: 61 RVA: 0x00006178 File Offset: 0x00004378
		public unsafe override int GetBytesImpl(char* chars, int charCount, byte* bytes, int byteCount)
		{
			int num = 0;
			int num2 = 0;
			EncoderFallbackBuffer encoderFallbackBuffer = null;
			while (charCount > 0)
			{
				int num3 = (int)chars[num];
				if (num3 >= 164)
				{
					if (num3 <= 352)
					{
						if (num3 <= 338)
						{
							switch (num3)
							{
							case 165:
							case 167:
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
							case 181:
							case 182:
							case 183:
							case 185:
							case 186:
							case 187:
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
								goto IL_0276;
							case 166:
							case 168:
							case 180:
							case 184:
							case 188:
							case 189:
							case 190:
								break;
							default:
								if (num3 == 338)
								{
									num3 = 188;
									goto IL_0276;
								}
								break;
							}
						}
						else
						{
							if (num3 == 339)
							{
								num3 = 189;
								goto IL_0276;
							}
							if (num3 == 352)
							{
								num3 = 166;
								goto IL_0276;
							}
						}
					}
					else if (num3 <= 376)
					{
						if (num3 == 353)
						{
							num3 = 168;
							goto IL_0276;
						}
						if (num3 == 376)
						{
							num3 = 190;
							goto IL_0276;
						}
					}
					else
					{
						if (num3 == 381)
						{
							num3 = 180;
							goto IL_0276;
						}
						if (num3 == 382)
						{
							num3 = 184;
							goto IL_0276;
						}
						if (num3 == 8364)
						{
							num3 = 164;
							goto IL_0276;
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
				IL_0276:
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

		// Token: 0x04000032 RID: 50
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
			'x', 'y', 'z', '{', '|', '}', '~', '\u007f', '\u0080', '\u0081',
			'\u0082', '\u0083', '\u0084', '\u0085', '\u0086', '\u0087', '\u0088', '\u0089', '\u008a', '\u008b',
			'\u008c', '\u008d', '\u008e', '\u008f', '\u0090', '\u0091', '\u0092', '\u0093', '\u0094', '\u0095',
			'\u0096', '\u0097', '\u0098', '\u0099', '\u009a', '\u009b', '\u009c', '\u009d', '\u009e', '\u009f',
			'\u00a0', '¡', '¢', '£', '€', '¥', 'Š', '§', 'š', '©',
			'ª', '«', '¬', '\u00ad', '®', '\u00af', '°', '±', '²', '³',
			'Ž', 'µ', '¶', '·', 'ž', '¹', 'º', '»', 'Œ', 'œ',
			'Ÿ', '¿', 'À', 'Á', 'Â', 'Ã', 'Ä', 'Å', 'Æ', 'Ç',
			'È', 'É', 'Ê', 'Ë', 'Ì', 'Í', 'Î', 'Ï', 'Ð', 'Ñ',
			'Ò', 'Ó', 'Ô', 'Õ', 'Ö', '×', 'Ø', 'Ù', 'Ú', 'Û',
			'Ü', 'Ý', 'Þ', 'ß', 'à', 'á', 'â', 'ã', 'ä', 'å',
			'æ', 'ç', 'è', 'é', 'ê', 'ë', 'ì', 'í', 'î', 'ï',
			'ð', 'ñ', 'ò', 'ó', 'ô', 'õ', 'ö', '÷', 'ø', 'ù',
			'ú', 'û', 'ü', 'ý', 'þ', 'ÿ'
		};
	}
}
