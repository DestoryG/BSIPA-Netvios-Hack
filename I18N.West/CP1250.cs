using System;
using System.Runtime.CompilerServices;
using System.Text;
using I18N.Common;

namespace I18N.West
{
	// Token: 0x02000007 RID: 7
	[Serializable]
	public class CP1250 : ByteEncoding
	{
		// Token: 0x0600000F RID: 15 RVA: 0x000036C0 File Offset: 0x000018C0
		public CP1250()
			: base(1250, CP1250.ToChars, "Central European (Windows)", "iso-8859-2", "windows-1250", "windows-1250", true, true, true, true, 1250)
		{
		}

		// Token: 0x06000010 RID: 16 RVA: 0x000036FA File Offset: 0x000018FA
		public unsafe override int GetByteCountImpl(char* chars, int count)
		{
			if (base.EncoderFallback != null)
			{
				return this.GetBytesImpl(chars, count, null, 0);
			}
			return count;
		}

		// Token: 0x06000011 RID: 17 RVA: 0x00003714 File Offset: 0x00001914
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

		// Token: 0x06000012 RID: 18 RVA: 0x00003750 File Offset: 0x00001950
		protected unsafe override void ToBytes(char* chars, int charCount, byte* bytes, int byteCount)
		{
			if (bytes == null)
			{
				throw new ArgumentNullException("bytes");
			}
			this.GetBytesImpl(chars, charCount, bytes, byteCount);
		}

		// Token: 0x06000013 RID: 19 RVA: 0x00003770 File Offset: 0x00001970
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
						if (num3 <= 136)
						{
							if (num3 == 129 || num3 == 131 || num3 == 136)
							{
								goto IL_07E3;
							}
						}
						else if (num3 <= 273)
						{
							if (num3 == 144)
							{
								goto IL_07E3;
							}
							switch (num3)
							{
							case 152:
							case 160:
							case 164:
							case 166:
							case 167:
							case 168:
							case 169:
							case 171:
							case 172:
							case 173:
							case 174:
							case 176:
							case 177:
							case 180:
							case 181:
							case 182:
							case 183:
							case 184:
							case 187:
							case 193:
							case 194:
							case 196:
							case 199:
							case 201:
							case 203:
							case 205:
							case 206:
							case 211:
							case 212:
							case 214:
							case 215:
							case 218:
							case 220:
							case 221:
							case 223:
							case 225:
							case 226:
							case 228:
							case 231:
							case 233:
							case 235:
							case 237:
							case 238:
							case 243:
							case 244:
							case 246:
							case 247:
							case 250:
							case 252:
							case 253:
								goto IL_07E3;
							case 258:
								num3 = 195;
								goto IL_07E3;
							case 259:
								num3 = 227;
								goto IL_07E3;
							case 260:
								num3 = 165;
								goto IL_07E3;
							case 261:
								num3 = 185;
								goto IL_07E3;
							case 262:
								num3 = 198;
								goto IL_07E3;
							case 263:
								num3 = 230;
								goto IL_07E3;
							case 268:
								num3 = 200;
								goto IL_07E3;
							case 269:
								num3 = 232;
								goto IL_07E3;
							case 270:
								num3 = 207;
								goto IL_07E3;
							case 271:
								num3 = 239;
								goto IL_07E3;
							case 272:
								num3 = 208;
								goto IL_07E3;
							case 273:
								num3 = 240;
								goto IL_07E3;
							}
						}
						else
						{
							switch (num3)
							{
							case 280:
								num3 = 202;
								goto IL_07E3;
							case 281:
								num3 = 234;
								goto IL_07E3;
							case 282:
								num3 = 204;
								goto IL_07E3;
							case 283:
								num3 = 236;
								goto IL_07E3;
							default:
								switch (num3)
								{
								case 313:
									num3 = 197;
									goto IL_07E3;
								case 314:
									num3 = 229;
									goto IL_07E3;
								case 317:
									num3 = 188;
									goto IL_07E3;
								case 318:
									num3 = 190;
									goto IL_07E3;
								case 321:
									num3 = 163;
									goto IL_07E3;
								case 322:
									num3 = 179;
									goto IL_07E3;
								case 323:
									num3 = 209;
									goto IL_07E3;
								case 324:
									num3 = 241;
									goto IL_07E3;
								case 327:
									num3 = 210;
									goto IL_07E3;
								case 328:
									num3 = 242;
									goto IL_07E3;
								case 336:
									num3 = 213;
									goto IL_07E3;
								case 337:
									num3 = 245;
									goto IL_07E3;
								case 340:
									num3 = 192;
									goto IL_07E3;
								case 341:
									num3 = 224;
									goto IL_07E3;
								case 344:
									num3 = 216;
									goto IL_07E3;
								case 345:
									num3 = 248;
									goto IL_07E3;
								case 346:
									num3 = 140;
									goto IL_07E3;
								case 347:
									num3 = 156;
									goto IL_07E3;
								case 350:
									num3 = 170;
									goto IL_07E3;
								case 351:
									num3 = 186;
									goto IL_07E3;
								case 352:
									num3 = 138;
									goto IL_07E3;
								case 353:
									num3 = 154;
									goto IL_07E3;
								case 354:
									num3 = 222;
									goto IL_07E3;
								case 355:
									num3 = 254;
									goto IL_07E3;
								case 356:
									num3 = 141;
									goto IL_07E3;
								case 357:
									num3 = 157;
									goto IL_07E3;
								case 366:
									num3 = 217;
									goto IL_07E3;
								case 367:
									num3 = 249;
									goto IL_07E3;
								case 368:
									num3 = 219;
									goto IL_07E3;
								case 369:
									num3 = 251;
									goto IL_07E3;
								case 377:
									num3 = 143;
									goto IL_07E3;
								case 378:
									num3 = 159;
									goto IL_07E3;
								case 379:
									num3 = 175;
									goto IL_07E3;
								case 380:
									num3 = 191;
									goto IL_07E3;
								case 381:
									num3 = 142;
									goto IL_07E3;
								case 382:
									num3 = 158;
									goto IL_07E3;
								}
								break;
							}
						}
					}
					else if (num3 <= 8240)
					{
						if (num3 <= 733)
						{
							if (num3 == 711)
							{
								num3 = 161;
								goto IL_07E3;
							}
							switch (num3)
							{
							case 728:
								num3 = 162;
								goto IL_07E3;
							case 729:
								num3 = 255;
								goto IL_07E3;
							case 731:
								num3 = 178;
								goto IL_07E3;
							case 733:
								num3 = 189;
								goto IL_07E3;
							}
						}
						else
						{
							switch (num3)
							{
							case 8211:
								num3 = 150;
								goto IL_07E3;
							case 8212:
								num3 = 151;
								goto IL_07E3;
							case 8213:
							case 8214:
							case 8215:
							case 8219:
							case 8223:
							case 8227:
							case 8228:
							case 8229:
								break;
							case 8216:
								num3 = 145;
								goto IL_07E3;
							case 8217:
								num3 = 146;
								goto IL_07E3;
							case 8218:
								num3 = 130;
								goto IL_07E3;
							case 8220:
								num3 = 147;
								goto IL_07E3;
							case 8221:
								num3 = 148;
								goto IL_07E3;
							case 8222:
								num3 = 132;
								goto IL_07E3;
							case 8224:
								num3 = 134;
								goto IL_07E3;
							case 8225:
								num3 = 135;
								goto IL_07E3;
							case 8226:
								num3 = 149;
								goto IL_07E3;
							case 8230:
								num3 = 133;
								goto IL_07E3;
							default:
								if (num3 == 8240)
								{
									num3 = 137;
									goto IL_07E3;
								}
								break;
							}
						}
					}
					else if (num3 <= 8250)
					{
						if (num3 == 8249)
						{
							num3 = 139;
							goto IL_07E3;
						}
						if (num3 == 8250)
						{
							num3 = 155;
							goto IL_07E3;
						}
					}
					else
					{
						if (num3 == 8364)
						{
							num3 = 128;
							goto IL_07E3;
						}
						if (num3 == 8482)
						{
							num3 = 153;
							goto IL_07E3;
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
				IL_07E3:
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

		// Token: 0x0400002C RID: 44
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
			'‚', '\u0083', '„', '…', '†', '‡', '\u0088', '‰', 'Š', '‹',
			'Ś', 'Ť', 'Ž', 'Ź', '\u0090', '‘', '’', '“', '”', '•',
			'–', '—', '\u0098', '™', 'š', '›', 'ś', 'ť', 'ž', 'ź',
			'\u00a0', 'ˇ', '\u02d8', 'Ł', '¤', 'Ą', '¦', '§', '\u00a8', '©',
			'Ş', '«', '¬', '\u00ad', '®', 'Ż', '°', '±', '\u02db', 'ł',
			'\u00b4', 'µ', '¶', '·', '\u00b8', 'ą', 'ş', '»', 'Ľ', '\u02dd',
			'ľ', 'ż', 'Ŕ', 'Á', 'Â', 'Ă', 'Ä', 'Ĺ', 'Ć', 'Ç',
			'Č', 'É', 'Ę', 'Ë', 'Ě', 'Í', 'Î', 'Ď', 'Đ', 'Ń',
			'Ň', 'Ó', 'Ô', 'Ő', 'Ö', '×', 'Ř', 'Ů', 'Ú', 'Ű',
			'Ü', 'Ý', 'Ţ', 'ß', 'ŕ', 'á', 'â', 'ă', 'ä', 'ĺ',
			'ć', 'ç', 'č', 'é', 'ę', 'ë', 'ě', 'í', 'î', 'ď',
			'đ', 'ń', 'ň', 'ó', 'ô', 'ő', 'ö', '÷', 'ř', 'ů',
			'ú', 'ű', 'ü', 'ý', 'ţ', '\u02d9'
		};
	}
}
