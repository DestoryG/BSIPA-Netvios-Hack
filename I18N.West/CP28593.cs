using System;
using System.Runtime.CompilerServices;
using System.Text;
using I18N.Common;

namespace I18N.West
{
	// Token: 0x0200000F RID: 15
	[Serializable]
	public class CP28593 : ByteEncoding
	{
		// Token: 0x0600002B RID: 43 RVA: 0x000057C0 File Offset: 0x000039C0
		public CP28593()
			: base(28593, CP28593.ToChars, "Latin 3 (ISO)", "iso-8859-3", "iso-8859-3", "iso-8859-3", true, true, true, true, 28593)
		{
		}

		// Token: 0x0600002C RID: 44 RVA: 0x000057FA File Offset: 0x000039FA
		public unsafe override int GetByteCountImpl(char* chars, int count)
		{
			if (base.EncoderFallback != null)
			{
				return this.GetBytesImpl(chars, count, null, 0);
			}
			return count;
		}

		// Token: 0x0600002D RID: 45 RVA: 0x00005814 File Offset: 0x00003A14
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

		// Token: 0x0600002E RID: 46 RVA: 0x00005850 File Offset: 0x00003A50
		protected unsafe override void ToBytes(char* chars, int charCount, byte* bytes, int byteCount)
		{
			if (bytes == null)
			{
				throw new ArgumentNullException("bytes");
			}
			this.GetBytesImpl(chars, charCount, bytes, byteCount);
		}

		// Token: 0x0600002F RID: 47 RVA: 0x00005870 File Offset: 0x00003A70
		public unsafe override int GetBytesImpl(char* chars, int charCount, byte* bytes, int byteCount)
		{
			int num = 0;
			int num2 = 0;
			EncoderFallbackBuffer encoderFallbackBuffer = null;
			while (charCount > 0)
			{
				int num3 = (int)chars[num];
				if (num3 >= 161)
				{
					if (num3 <= 365)
					{
						if (num3 <= 351)
						{
							switch (num3)
							{
							case 163:
							case 164:
							case 167:
							case 168:
							case 173:
							case 176:
							case 178:
							case 179:
							case 180:
							case 181:
							case 183:
							case 184:
							case 189:
							case 192:
							case 193:
							case 194:
							case 196:
							case 199:
							case 200:
							case 201:
							case 202:
							case 203:
							case 204:
							case 205:
							case 206:
							case 207:
							case 209:
							case 210:
							case 211:
							case 212:
							case 214:
							case 215:
							case 217:
							case 218:
							case 219:
							case 220:
							case 223:
							case 224:
							case 225:
							case 226:
							case 228:
							case 231:
							case 232:
							case 233:
							case 234:
							case 235:
							case 236:
							case 237:
							case 238:
							case 239:
							case 241:
							case 242:
							case 243:
							case 244:
							case 246:
							case 247:
							case 249:
							case 250:
							case 251:
							case 252:
								goto IL_0453;
							case 165:
							case 166:
							case 169:
							case 170:
							case 171:
							case 172:
							case 174:
							case 175:
							case 177:
							case 182:
							case 185:
							case 186:
							case 187:
							case 188:
							case 190:
							case 191:
							case 195:
							case 197:
							case 198:
							case 208:
							case 213:
							case 216:
							case 221:
							case 222:
							case 227:
							case 229:
							case 230:
							case 240:
							case 245:
							case 248:
							case 253:
							case 254:
							case 255:
							case 256:
							case 257:
							case 258:
							case 259:
							case 260:
							case 261:
							case 262:
							case 263:
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
							case 290:
							case 291:
							case 296:
							case 297:
							case 298:
							case 299:
							case 300:
							case 301:
							case 302:
							case 303:
							case 306:
							case 307:
								break;
							case 264:
								num3 = 198;
								goto IL_0453;
							case 265:
								num3 = 230;
								goto IL_0453;
							case 266:
								num3 = 197;
								goto IL_0453;
							case 267:
								num3 = 229;
								goto IL_0453;
							case 284:
								num3 = 216;
								goto IL_0453;
							case 285:
								num3 = 248;
								goto IL_0453;
							case 286:
								num3 = 171;
								goto IL_0453;
							case 287:
								num3 = 187;
								goto IL_0453;
							case 288:
								num3 = 213;
								goto IL_0453;
							case 289:
								num3 = 245;
								goto IL_0453;
							case 292:
								num3 = 166;
								goto IL_0453;
							case 293:
								num3 = 182;
								goto IL_0453;
							case 294:
								num3 = 161;
								goto IL_0453;
							case 295:
								num3 = 177;
								goto IL_0453;
							case 304:
								num3 = 169;
								goto IL_0453;
							case 305:
								num3 = 185;
								goto IL_0453;
							case 308:
								num3 = 172;
								goto IL_0453;
							case 309:
								num3 = 188;
								goto IL_0453;
							default:
								switch (num3)
								{
								case 348:
									num3 = 222;
									goto IL_0453;
								case 349:
									num3 = 254;
									goto IL_0453;
								case 350:
									num3 = 170;
									goto IL_0453;
								case 351:
									num3 = 186;
									goto IL_0453;
								}
								break;
							}
						}
						else
						{
							if (num3 == 364)
							{
								num3 = 221;
								goto IL_0453;
							}
							if (num3 == 365)
							{
								num3 = 253;
								goto IL_0453;
							}
						}
					}
					else if (num3 <= 380)
					{
						if (num3 == 379)
						{
							num3 = 175;
							goto IL_0453;
						}
						if (num3 == 380)
						{
							num3 = 191;
							goto IL_0453;
						}
					}
					else
					{
						if (num3 == 728)
						{
							num3 = 162;
							goto IL_0453;
						}
						if (num3 == 729)
						{
							num3 = 255;
							goto IL_0453;
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
				IL_0453:
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

		// Token: 0x04000030 RID: 48
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
			'\u00a0', 'Ħ', '\u02d8', '£', '¤', '?', 'Ĥ', '§', '\u00a8', 'İ',
			'Ş', 'Ğ', 'Ĵ', '\u00ad', '?', 'Ż', '°', 'ħ', '²', '³',
			'\u00b4', 'µ', 'ĥ', '·', '\u00b8', 'ı', 'ş', 'ğ', 'ĵ', '½',
			'?', 'ż', 'À', 'Á', 'Â', '?', 'Ä', 'Ċ', 'Ĉ', 'Ç',
			'È', 'É', 'Ê', 'Ë', 'Ì', 'Í', 'Î', 'Ï', '?', 'Ñ',
			'Ò', 'Ó', 'Ô', 'Ġ', 'Ö', '×', 'Ĝ', 'Ù', 'Ú', 'Û',
			'Ü', 'Ŭ', 'Ŝ', 'ß', 'à', 'á', 'â', '?', 'ä', 'ċ',
			'ĉ', 'ç', 'è', 'é', 'ê', 'ë', 'ì', 'í', 'î', 'ï',
			'?', 'ñ', 'ò', 'ó', 'ô', 'ġ', 'ö', '÷', 'ĝ', 'ù',
			'ú', 'û', 'ü', 'ŭ', 'ŝ', '\u02d9'
		};
	}
}
