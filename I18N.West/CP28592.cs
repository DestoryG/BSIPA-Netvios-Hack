using System;
using System.Runtime.CompilerServices;
using System.Text;
using I18N.Common;

namespace I18N.West
{
	// Token: 0x0200000D RID: 13
	[Serializable]
	public class CP28592 : ByteEncoding
	{
		// Token: 0x06000024 RID: 36 RVA: 0x00004B30 File Offset: 0x00002D30
		public CP28592()
			: base(28592, CP28592.ToChars, "Central European (ISO)", "iso-8859-2", "iso-8859-2", "iso-8859-2", true, true, true, true, 1250)
		{
		}

		// Token: 0x06000025 RID: 37 RVA: 0x00004B6A File Offset: 0x00002D6A
		public unsafe override int GetByteCountImpl(char* chars, int count)
		{
			if (base.EncoderFallback != null)
			{
				return this.GetBytesImpl(chars, count, null, 0);
			}
			return count;
		}

		// Token: 0x06000026 RID: 38 RVA: 0x00004B84 File Offset: 0x00002D84
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

		// Token: 0x06000027 RID: 39 RVA: 0x00004BC0 File Offset: 0x00002DC0
		protected unsafe override void ToBytes(char* chars, int charCount, byte* bytes, int byteCount)
		{
			if (bytes == null)
			{
				throw new ArgumentNullException("bytes");
			}
			this.GetBytesImpl(chars, charCount, bytes, byteCount);
		}

		// Token: 0x06000028 RID: 40 RVA: 0x00004BE0 File Offset: 0x00002DE0
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
					if (num3 <= 9559)
					{
						if (num3 <= 9472)
						{
							if (num3 <= 733)
							{
								if (num3 <= 283)
								{
									switch (num3)
									{
									case 162:
										num3 = 141;
										goto IL_0B88;
									case 163:
									case 166:
									case 170:
									case 171:
									case 172:
									case 175:
									case 177:
									case 178:
									case 179:
									case 181:
									case 183:
										break;
									case 164:
									case 167:
									case 168:
									case 173:
									case 176:
									case 180:
									case 184:
										goto IL_0B88;
									case 165:
										num3 = 142;
										goto IL_0B88;
									case 169:
										num3 = 136;
										goto IL_0B88;
									case 174:
										num3 = 159;
										goto IL_0B88;
									case 182:
										num3 = 20;
										goto IL_0B88;
									default:
										switch (num3)
										{
										case 193:
										case 194:
										case 196:
										case 199:
										case 201:
										case 203:
										case 205:
										case 206:
										case 208:
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
											goto IL_0B88;
										case 258:
											num3 = 195;
											goto IL_0B88;
										case 259:
											num3 = 227;
											goto IL_0B88;
										case 260:
											num3 = 161;
											goto IL_0B88;
										case 261:
											num3 = 177;
											goto IL_0B88;
										case 262:
											num3 = 198;
											goto IL_0B88;
										case 263:
											num3 = 230;
											goto IL_0B88;
										case 268:
											num3 = 200;
											goto IL_0B88;
										case 269:
											num3 = 232;
											goto IL_0B88;
										case 270:
											num3 = 207;
											goto IL_0B88;
										case 271:
											num3 = 239;
											goto IL_0B88;
										case 272:
											num3 = 208;
											goto IL_0B88;
										case 273:
											num3 = 240;
											goto IL_0B88;
										case 280:
											num3 = 202;
											goto IL_0B88;
										case 281:
											num3 = 234;
											goto IL_0B88;
										case 282:
											num3 = 204;
											goto IL_0B88;
										case 283:
											num3 = 236;
											goto IL_0B88;
										}
										break;
									}
								}
								else
								{
									switch (num3)
									{
									case 313:
										num3 = 197;
										goto IL_0B88;
									case 314:
										num3 = 229;
										goto IL_0B88;
									case 315:
									case 316:
									case 319:
									case 320:
									case 325:
									case 326:
									case 329:
									case 330:
									case 331:
									case 332:
									case 333:
									case 334:
									case 335:
									case 338:
									case 339:
									case 342:
									case 343:
									case 348:
									case 349:
									case 358:
									case 359:
									case 360:
									case 361:
									case 362:
									case 363:
									case 364:
									case 365:
									case 370:
									case 371:
									case 372:
									case 373:
									case 374:
									case 375:
									case 376:
										break;
									case 317:
										num3 = 165;
										goto IL_0B88;
									case 318:
										num3 = 181;
										goto IL_0B88;
									case 321:
										num3 = 163;
										goto IL_0B88;
									case 322:
										num3 = 179;
										goto IL_0B88;
									case 323:
										num3 = 209;
										goto IL_0B88;
									case 324:
										num3 = 241;
										goto IL_0B88;
									case 327:
										num3 = 210;
										goto IL_0B88;
									case 328:
										num3 = 242;
										goto IL_0B88;
									case 336:
										num3 = 213;
										goto IL_0B88;
									case 337:
										num3 = 245;
										goto IL_0B88;
									case 340:
										num3 = 192;
										goto IL_0B88;
									case 341:
										num3 = 224;
										goto IL_0B88;
									case 344:
										num3 = 216;
										goto IL_0B88;
									case 345:
										num3 = 248;
										goto IL_0B88;
									case 346:
										num3 = 166;
										goto IL_0B88;
									case 347:
										num3 = 182;
										goto IL_0B88;
									case 350:
										num3 = 170;
										goto IL_0B88;
									case 351:
										num3 = 186;
										goto IL_0B88;
									case 352:
										num3 = 169;
										goto IL_0B88;
									case 353:
										num3 = 185;
										goto IL_0B88;
									case 354:
										num3 = 222;
										goto IL_0B88;
									case 355:
										num3 = 254;
										goto IL_0B88;
									case 356:
										num3 = 171;
										goto IL_0B88;
									case 357:
										num3 = 187;
										goto IL_0B88;
									case 366:
										num3 = 217;
										goto IL_0B88;
									case 367:
										num3 = 249;
										goto IL_0B88;
									case 368:
										num3 = 219;
										goto IL_0B88;
									case 369:
										num3 = 251;
										goto IL_0B88;
									case 377:
										num3 = 172;
										goto IL_0B88;
									case 378:
										num3 = 188;
										goto IL_0B88;
									case 379:
										num3 = 175;
										goto IL_0B88;
									case 380:
										num3 = 191;
										goto IL_0B88;
									case 381:
										num3 = 174;
										goto IL_0B88;
									case 382:
										num3 = 190;
										goto IL_0B88;
									default:
										if (num3 == 711)
										{
											num3 = 183;
											goto IL_0B88;
										}
										switch (num3)
										{
										case 728:
											num3 = 162;
											goto IL_0B88;
										case 729:
											num3 = 255;
											goto IL_0B88;
										case 731:
											num3 = 178;
											goto IL_0B88;
										case 733:
											num3 = 189;
											goto IL_0B88;
										}
										break;
									}
								}
							}
							else if (num3 <= 8597)
							{
								if (num3 == 8226)
								{
									num3 = 7;
									goto IL_0B88;
								}
								if (num3 == 8252)
								{
									num3 = 19;
									goto IL_0B88;
								}
								switch (num3)
								{
								case 8592:
									num3 = 27;
									goto IL_0B88;
								case 8593:
									num3 = 24;
									goto IL_0B88;
								case 8594:
									num3 = 26;
									goto IL_0B88;
								case 8595:
									num3 = 25;
									goto IL_0B88;
								case 8596:
									num3 = 29;
									goto IL_0B88;
								case 8597:
									num3 = 18;
									goto IL_0B88;
								}
							}
							else
							{
								if (num3 == 8616)
								{
									num3 = 23;
									goto IL_0B88;
								}
								if (num3 == 8735)
								{
									num3 = 28;
									goto IL_0B88;
								}
								if (num3 == 9472)
								{
									num3 = 148;
									goto IL_0B88;
								}
							}
						}
						else if (num3 <= 9500)
						{
							if (num3 <= 9488)
							{
								if (num3 == 9474)
								{
									num3 = 131;
									goto IL_0B88;
								}
								if (num3 == 9484)
								{
									num3 = 134;
									goto IL_0B88;
								}
								if (num3 == 9488)
								{
									num3 = 143;
									goto IL_0B88;
								}
							}
							else
							{
								if (num3 == 9492)
								{
									num3 = 144;
									goto IL_0B88;
								}
								if (num3 == 9496)
								{
									num3 = 133;
									goto IL_0B88;
								}
								if (num3 == 9500)
								{
									num3 = 147;
									goto IL_0B88;
								}
							}
						}
						else if (num3 <= 9524)
						{
							if (num3 == 9508)
							{
								num3 = 132;
								goto IL_0B88;
							}
							if (num3 == 9516)
							{
								num3 = 146;
								goto IL_0B88;
							}
							if (num3 == 9524)
							{
								num3 = 145;
								goto IL_0B88;
							}
						}
						else
						{
							if (num3 == 9532)
							{
								num3 = 149;
								goto IL_0B88;
							}
							switch (num3)
							{
							case 9552:
								num3 = 157;
								goto IL_0B88;
							case 9553:
								num3 = 138;
								goto IL_0B88;
							case 9554:
							case 9555:
								break;
							case 9556:
								num3 = 153;
								goto IL_0B88;
							default:
								if (num3 == 9559)
								{
									num3 = 139;
									goto IL_0B88;
								}
								break;
							}
						}
					}
					else if (num3 <= 9644)
					{
						if (num3 <= 9577)
						{
							if (num3 <= 9568)
							{
								if (num3 == 9562)
								{
									num3 = 152;
									goto IL_0B88;
								}
								if (num3 == 9565)
								{
									num3 = 140;
									goto IL_0B88;
								}
								if (num3 == 9568)
								{
									num3 = 156;
									goto IL_0B88;
								}
							}
							else
							{
								if (num3 == 9571)
								{
									num3 = 137;
									goto IL_0B88;
								}
								if (num3 == 9574)
								{
									num3 = 155;
									goto IL_0B88;
								}
								if (num3 == 9577)
								{
									num3 = 154;
									goto IL_0B88;
								}
							}
						}
						else if (num3 <= 9604)
						{
							if (num3 == 9580)
							{
								num3 = 158;
								goto IL_0B88;
							}
							if (num3 == 9600)
							{
								num3 = 151;
								goto IL_0B88;
							}
							if (num3 == 9604)
							{
								num3 = 150;
								goto IL_0B88;
							}
						}
						else
						{
							if (num3 == 9608)
							{
								num3 = 135;
								goto IL_0B88;
							}
							switch (num3)
							{
							case 9617:
								num3 = 128;
								goto IL_0B88;
							case 9618:
								num3 = 129;
								goto IL_0B88;
							case 9619:
								num3 = 130;
								goto IL_0B88;
							default:
								if (num3 == 9644)
								{
									num3 = 22;
									goto IL_0B88;
								}
								break;
							}
						}
					}
					else if (num3 <= 9688)
					{
						if (num3 <= 9660)
						{
							if (num3 == 9650)
							{
								num3 = 30;
								goto IL_0B88;
							}
							if (num3 == 9658)
							{
								num3 = 16;
								goto IL_0B88;
							}
							if (num3 == 9660)
							{
								num3 = 31;
								goto IL_0B88;
							}
						}
						else
						{
							if (num3 == 9668)
							{
								num3 = 17;
								goto IL_0B88;
							}
							if (num3 == 9675)
							{
								num3 = 9;
								goto IL_0B88;
							}
							if (num3 == 9688)
							{
								num3 = 8;
								goto IL_0B88;
							}
						}
					}
					else if (num3 <= 9830)
					{
						if (num3 == 9689)
						{
							num3 = 10;
							goto IL_0B88;
						}
						switch (num3)
						{
						case 9786:
							num3 = 1;
							goto IL_0B88;
						case 9787:
							num3 = 2;
							goto IL_0B88;
						case 9788:
							num3 = 15;
							goto IL_0B88;
						case 9789:
						case 9790:
						case 9791:
						case 9793:
							break;
						case 9792:
							num3 = 12;
							goto IL_0B88;
						case 9794:
							num3 = 11;
							goto IL_0B88;
						default:
							switch (num3)
							{
							case 9824:
								num3 = 6;
								goto IL_0B88;
							case 9827:
								num3 = 5;
								goto IL_0B88;
							case 9829:
								num3 = 3;
								goto IL_0B88;
							case 9830:
								num3 = 4;
								goto IL_0B88;
							}
							break;
						}
					}
					else
					{
						if (num3 == 9834)
						{
							num3 = 13;
							goto IL_0B88;
						}
						if (num3 == 9836)
						{
							num3 = 14;
							goto IL_0B88;
						}
						switch (num3)
						{
						case 65512:
							num3 = 131;
							goto IL_0B88;
						case 65513:
							num3 = 27;
							goto IL_0B88;
						case 65514:
							num3 = 24;
							goto IL_0B88;
						case 65515:
							num3 = 26;
							goto IL_0B88;
						case 65516:
							num3 = 25;
							goto IL_0B88;
						case 65518:
							num3 = 9;
							goto IL_0B88;
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
				IL_0B88:
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

		// Token: 0x0400002F RID: 47
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
			'\u00a0', 'Ą', '\u02d8', 'Ł', '¤', 'Ľ', 'Ś', '§', '\u00a8', 'Š',
			'Ş', 'Ť', 'Ź', '\u00ad', 'Ž', 'Ż', '°', 'ą', '\u02db', 'ł',
			'\u00b4', 'ľ', 'ś', 'ˇ', '\u00b8', 'š', 'ş', 'ť', 'ź', '\u02dd',
			'ž', 'ż', 'Ŕ', 'Á', 'Â', 'Ă', 'Ä', 'Ĺ', 'Ć', 'Ç',
			'Č', 'É', 'Ę', 'Ë', 'Ě', 'Í', 'Î', 'Ď', 'Đ', 'Ń',
			'Ň', 'Ó', 'Ô', 'Ő', 'Ö', '×', 'Ř', 'Ů', 'Ú', 'Ű',
			'Ü', 'Ý', 'Ţ', 'ß', 'ŕ', 'á', 'â', 'ă', 'ä', 'ĺ',
			'ć', 'ç', 'č', 'é', 'ę', 'ë', 'ě', 'í', 'î', 'ď',
			'đ', 'ń', 'ň', 'ó', 'ô', 'ő', 'ö', '÷', 'ř', 'ů',
			'ú', 'ű', 'ü', 'ý', 'ţ', '\u02d9'
		};
	}
}
