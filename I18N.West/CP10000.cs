using System;
using System.Runtime.CompilerServices;
using System.Text;
using I18N.Common;

namespace I18N.West
{
	// Token: 0x02000003 RID: 3
	[Serializable]
	public class CP10000 : ByteEncoding
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		public CP10000()
			: base(10000, CP10000.ToChars, "Western European (Mac)", "macintosh", "macintosh", "macintosh", false, false, false, false, 1252)
		{
		}

		// Token: 0x06000002 RID: 2 RVA: 0x0000208A File Offset: 0x0000028A
		public unsafe override int GetByteCountImpl(char* chars, int count)
		{
			if (base.EncoderFallback != null)
			{
				return this.GetBytesImpl(chars, count, null, 0);
			}
			return count;
		}

		// Token: 0x06000003 RID: 3 RVA: 0x000020A4 File Offset: 0x000002A4
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

		// Token: 0x06000004 RID: 4 RVA: 0x000020E0 File Offset: 0x000002E0
		protected unsafe override void ToBytes(char* chars, int charCount, byte* bytes, int byteCount)
		{
			if (bytes == null)
			{
				throw new ArgumentNullException("bytes");
			}
			this.GetBytesImpl(chars, charCount, bytes, byteCount);
		}

		// Token: 0x06000005 RID: 5 RVA: 0x00002100 File Offset: 0x00000300
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
					if (num3 <= 8706)
					{
						if (num3 <= 733)
						{
							if (num3 <= 376)
							{
								if (num3 <= 338)
								{
									switch (num3)
									{
									case 160:
										num3 = 202;
										goto IL_0A98;
									case 161:
										num3 = 193;
										goto IL_0A98;
									case 162:
									case 163:
									case 169:
									case 177:
									case 181:
										goto IL_0A98;
									case 164:
										num3 = 219;
										goto IL_0A98;
									case 165:
										num3 = 180;
										goto IL_0A98;
									case 166:
									case 173:
									case 178:
									case 179:
									case 185:
									case 188:
									case 189:
									case 190:
									case 208:
									case 215:
									case 221:
									case 222:
									case 240:
									case 253:
									case 254:
									case 256:
									case 257:
									case 258:
									case 259:
									case 260:
									case 261:
									case 262:
									case 263:
									case 264:
									case 265:
									case 266:
									case 267:
									case 268:
									case 269:
									case 270:
									case 271:
									case 272:
									case 273:
									case 274:
									case 275:
									case 276:
									case 277:
									case 278:
									case 279:
									case 280:
									case 281:
									case 282:
									case 283:
									case 284:
									case 285:
									case 286:
									case 287:
									case 288:
									case 289:
									case 290:
									case 291:
									case 292:
									case 293:
									case 294:
									case 295:
									case 296:
									case 297:
									case 298:
									case 299:
									case 300:
									case 301:
									case 302:
									case 303:
									case 304:
										break;
									case 167:
										num3 = 164;
										goto IL_0A98;
									case 168:
										num3 = 172;
										goto IL_0A98;
									case 170:
										num3 = 187;
										goto IL_0A98;
									case 171:
										num3 = 199;
										goto IL_0A98;
									case 172:
										num3 = 194;
										goto IL_0A98;
									case 174:
										num3 = 168;
										goto IL_0A98;
									case 175:
										num3 = 248;
										goto IL_0A98;
									case 176:
										num3 = 161;
										goto IL_0A98;
									case 180:
										num3 = 171;
										goto IL_0A98;
									case 182:
										num3 = 166;
										goto IL_0A98;
									case 183:
										num3 = 225;
										goto IL_0A98;
									case 184:
										num3 = 252;
										goto IL_0A98;
									case 186:
										num3 = 188;
										goto IL_0A98;
									case 187:
										num3 = 200;
										goto IL_0A98;
									case 191:
										num3 = 192;
										goto IL_0A98;
									case 192:
										num3 = 203;
										goto IL_0A98;
									case 193:
										num3 = 231;
										goto IL_0A98;
									case 194:
										num3 = 229;
										goto IL_0A98;
									case 195:
										num3 = 204;
										goto IL_0A98;
									case 196:
										num3 = 128;
										goto IL_0A98;
									case 197:
										num3 = 129;
										goto IL_0A98;
									case 198:
										num3 = 174;
										goto IL_0A98;
									case 199:
										num3 = 130;
										goto IL_0A98;
									case 200:
										num3 = 233;
										goto IL_0A98;
									case 201:
										num3 = 131;
										goto IL_0A98;
									case 202:
										num3 = 230;
										goto IL_0A98;
									case 203:
										num3 = 232;
										goto IL_0A98;
									case 204:
										num3 = 237;
										goto IL_0A98;
									case 205:
										num3 = 234;
										goto IL_0A98;
									case 206:
										num3 = 235;
										goto IL_0A98;
									case 207:
										num3 = 236;
										goto IL_0A98;
									case 209:
										num3 = 132;
										goto IL_0A98;
									case 210:
										num3 = 241;
										goto IL_0A98;
									case 211:
										num3 = 238;
										goto IL_0A98;
									case 212:
										num3 = 239;
										goto IL_0A98;
									case 213:
										num3 = 205;
										goto IL_0A98;
									case 214:
										num3 = 133;
										goto IL_0A98;
									case 216:
										num3 = 175;
										goto IL_0A98;
									case 217:
										num3 = 244;
										goto IL_0A98;
									case 218:
										num3 = 242;
										goto IL_0A98;
									case 219:
										num3 = 243;
										goto IL_0A98;
									case 220:
										num3 = 134;
										goto IL_0A98;
									case 223:
										num3 = 167;
										goto IL_0A98;
									case 224:
										num3 = 136;
										goto IL_0A98;
									case 225:
										num3 = 135;
										goto IL_0A98;
									case 226:
										num3 = 137;
										goto IL_0A98;
									case 227:
										num3 = 139;
										goto IL_0A98;
									case 228:
										num3 = 138;
										goto IL_0A98;
									case 229:
										num3 = 140;
										goto IL_0A98;
									case 230:
										num3 = 190;
										goto IL_0A98;
									case 231:
										num3 = 141;
										goto IL_0A98;
									case 232:
										num3 = 143;
										goto IL_0A98;
									case 233:
										num3 = 142;
										goto IL_0A98;
									case 234:
										num3 = 144;
										goto IL_0A98;
									case 235:
										num3 = 145;
										goto IL_0A98;
									case 236:
										num3 = 147;
										goto IL_0A98;
									case 237:
										num3 = 146;
										goto IL_0A98;
									case 238:
										num3 = 148;
										goto IL_0A98;
									case 239:
										num3 = 149;
										goto IL_0A98;
									case 241:
										num3 = 150;
										goto IL_0A98;
									case 242:
										num3 = 152;
										goto IL_0A98;
									case 243:
										num3 = 151;
										goto IL_0A98;
									case 244:
										num3 = 153;
										goto IL_0A98;
									case 245:
										num3 = 155;
										goto IL_0A98;
									case 246:
										num3 = 154;
										goto IL_0A98;
									case 247:
										num3 = 214;
										goto IL_0A98;
									case 248:
										num3 = 191;
										goto IL_0A98;
									case 249:
										num3 = 157;
										goto IL_0A98;
									case 250:
										num3 = 156;
										goto IL_0A98;
									case 251:
										num3 = 158;
										goto IL_0A98;
									case 252:
										num3 = 159;
										goto IL_0A98;
									case 255:
										num3 = 216;
										goto IL_0A98;
									case 305:
										num3 = 245;
										goto IL_0A98;
									default:
										if (num3 == 338)
										{
											num3 = 206;
											goto IL_0A98;
										}
										break;
									}
								}
								else
								{
									if (num3 == 339)
									{
										num3 = 207;
										goto IL_0A98;
									}
									if (num3 == 376)
									{
										num3 = 217;
										goto IL_0A98;
									}
								}
							}
							else if (num3 <= 710)
							{
								if (num3 == 402)
								{
									num3 = 196;
									goto IL_0A98;
								}
								if (num3 == 710)
								{
									num3 = 246;
									goto IL_0A98;
								}
							}
							else
							{
								if (num3 == 711)
								{
									num3 = 255;
									goto IL_0A98;
								}
								switch (num3)
								{
								case 728:
									num3 = 249;
									goto IL_0A98;
								case 729:
									num3 = 250;
									goto IL_0A98;
								case 730:
									num3 = 251;
									goto IL_0A98;
								case 731:
									num3 = 254;
									goto IL_0A98;
								case 732:
									num3 = 247;
									goto IL_0A98;
								case 733:
									num3 = 253;
									goto IL_0A98;
								}
							}
						}
						else if (num3 <= 8249)
						{
							if (num3 <= 8230)
							{
								if (num3 == 960)
								{
									num3 = 185;
									goto IL_0A98;
								}
								switch (num3)
								{
								case 8211:
									num3 = 208;
									goto IL_0A98;
								case 8212:
									num3 = 209;
									goto IL_0A98;
								case 8216:
									num3 = 212;
									goto IL_0A98;
								case 8217:
									num3 = 213;
									goto IL_0A98;
								case 8218:
									num3 = 226;
									goto IL_0A98;
								case 8220:
									num3 = 210;
									goto IL_0A98;
								case 8221:
									num3 = 211;
									goto IL_0A98;
								case 8222:
									num3 = 227;
									goto IL_0A98;
								case 8224:
									num3 = 160;
									goto IL_0A98;
								case 8225:
									num3 = 224;
									goto IL_0A98;
								case 8226:
									num3 = 165;
									goto IL_0A98;
								case 8230:
									num3 = 201;
									goto IL_0A98;
								}
							}
							else
							{
								if (num3 == 8240)
								{
									num3 = 228;
									goto IL_0A98;
								}
								if (num3 == 8249)
								{
									num3 = 220;
									goto IL_0A98;
								}
							}
						}
						else if (num3 <= 8260)
						{
							if (num3 == 8250)
							{
								num3 = 221;
								goto IL_0A98;
							}
							if (num3 == 8260)
							{
								num3 = 218;
								goto IL_0A98;
							}
						}
						else
						{
							if (num3 == 8482)
							{
								num3 = 170;
								goto IL_0A98;
							}
							if (num3 == 8486)
							{
								num3 = 189;
								goto IL_0A98;
							}
							if (num3 == 8706)
							{
								num3 = 182;
								goto IL_0A98;
							}
						}
					}
					else if (num3 <= 8800)
					{
						if (num3 <= 8730)
						{
							if (num3 <= 8719)
							{
								if (num3 == 8710)
								{
									num3 = 198;
									goto IL_0A98;
								}
								if (num3 == 8719)
								{
									num3 = 184;
									goto IL_0A98;
								}
							}
							else
							{
								if (num3 == 8721)
								{
									num3 = 183;
									goto IL_0A98;
								}
								if (num3 == 8730)
								{
									num3 = 195;
									goto IL_0A98;
								}
							}
						}
						else if (num3 <= 8747)
						{
							if (num3 == 8734)
							{
								num3 = 176;
								goto IL_0A98;
							}
							if (num3 == 8747)
							{
								num3 = 186;
								goto IL_0A98;
							}
						}
						else
						{
							if (num3 == 8776)
							{
								num3 = 197;
								goto IL_0A98;
							}
							if (num3 == 8800)
							{
								num3 = 173;
								goto IL_0A98;
							}
						}
					}
					else if (num3 <= 9674)
					{
						if (num3 <= 8805)
						{
							if (num3 == 8804)
							{
								num3 = 178;
								goto IL_0A98;
							}
							if (num3 == 8805)
							{
								num3 = 179;
								goto IL_0A98;
							}
						}
						else
						{
							if (num3 == 8984)
							{
								num3 = 17;
								goto IL_0A98;
							}
							if (num3 == 9674)
							{
								num3 = 215;
								goto IL_0A98;
							}
						}
					}
					else if (num3 <= 10003)
					{
						if (num3 == 9830)
						{
							num3 = 19;
							goto IL_0A98;
						}
						if (num3 == 10003)
						{
							num3 = 18;
							goto IL_0A98;
						}
					}
					else
					{
						if (num3 == 63743)
						{
							num3 = 240;
							goto IL_0A98;
						}
						if (num3 == 64257)
						{
							num3 = 222;
							goto IL_0A98;
						}
						if (num3 == 64258)
						{
							num3 = 223;
							goto IL_0A98;
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
				IL_0A98:
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

		// Token: 0x0400002A RID: 42
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
			'x', 'y', 'z', '{', '|', '}', '~', '\u007f', 'Ä', 'Å',
			'Ç', 'É', 'Ñ', 'Ö', 'Ü', 'á', 'à', 'â', 'ä', 'ã',
			'å', 'ç', 'é', 'è', 'ê', 'ë', 'í', 'ì', 'î', 'ï',
			'ñ', 'ó', 'ò', 'ô', 'ö', 'õ', 'ú', 'ù', 'û', 'ü',
			'†', '°', '¢', '£', '§', '•', '¶', 'ß', '®', '©',
			'™', '\u00b4', '\u00a8', '≠', 'Æ', 'Ø', '∞', '±', '≤', '≥',
			'¥', 'µ', '∂', '∑', '∏', 'π', '∫', 'ª', 'º', 'Ω',
			'æ', 'ø', '¿', '¡', '¬', '√', 'ƒ', '≈', '∆', '«',
			'»', '…', '\u00a0', 'À', 'Ã', 'Õ', 'Œ', 'œ', '–', '—',
			'“', '”', '‘', '’', '÷', '◊', 'ÿ', 'Ÿ', '⁄', '¤',
			'‹', '›', 'ﬁ', 'ﬂ', '‡', '·', '‚', '„', '‰', 'Â',
			'Ê', 'Á', 'Ë', 'È', 'Í', 'Î', 'Ï', 'Ì', 'Ó', 'Ô',
			'\uf8ff', 'Ò', 'Ú', 'Û', 'Ù', 'ı', 'ˆ', '\u02dc', '\u00af', '\u02d8',
			'\u02d9', '\u02da', '\u00b8', '\u02dd', '\u02db', 'ˇ'
		};
	}
}
