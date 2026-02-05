using System;
using System.Runtime.CompilerServices;
using System.Text;
using I18N.Common;

namespace I18N.West
{
	// Token: 0x02000017 RID: 23
	[Serializable]
	public class CP850 : ByteEncoding
	{
		// Token: 0x06000047 RID: 71 RVA: 0x00008848 File Offset: 0x00006A48
		public CP850()
			: base(850, CP850.ToChars, "Western European (DOS)", "ibm850", "ibm850", "ibm850", false, false, false, false, 1252)
		{
		}

		// Token: 0x06000048 RID: 72 RVA: 0x00008882 File Offset: 0x00006A82
		public unsafe override int GetByteCountImpl(char* chars, int count)
		{
			if (base.EncoderFallback != null)
			{
				return this.GetBytesImpl(chars, count, null, 0);
			}
			return count;
		}

		// Token: 0x06000049 RID: 73 RVA: 0x0000889C File Offset: 0x00006A9C
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

		// Token: 0x0600004A RID: 74 RVA: 0x000088D8 File Offset: 0x00006AD8
		protected unsafe override void ToBytes(char* chars, int charCount, byte* bytes, int byteCount)
		{
			if (bytes == null)
			{
				throw new ArgumentNullException("bytes");
			}
			this.GetBytesImpl(chars, charCount, bytes, byteCount);
		}

		// Token: 0x0600004B RID: 75 RVA: 0x000088F8 File Offset: 0x00006AF8
		public unsafe override int GetBytesImpl(char* chars, int charCount, byte* bytes, int byteCount)
		{
			int num = 0;
			int num2 = 0;
			EncoderFallbackBuffer encoderFallbackBuffer = null;
			while (charCount > 0)
			{
				int num3 = (int)chars[num];
				if (num3 >= 26)
				{
					if (num3 <= 9562)
					{
						if (num3 <= 9474)
						{
							if (num3 <= 8254)
							{
								if (num3 <= 8215)
								{
									switch (num3)
									{
									case 26:
										num3 = 127;
										goto IL_1051;
									case 27:
									case 29:
									case 30:
									case 31:
									case 32:
									case 33:
									case 34:
									case 35:
									case 36:
									case 37:
									case 38:
									case 39:
									case 40:
									case 41:
									case 42:
									case 43:
									case 44:
									case 45:
									case 46:
									case 47:
									case 48:
									case 49:
									case 50:
									case 51:
									case 52:
									case 53:
									case 54:
									case 55:
									case 56:
									case 57:
									case 58:
									case 59:
									case 60:
									case 61:
									case 62:
									case 63:
									case 64:
									case 65:
									case 66:
									case 67:
									case 68:
									case 69:
									case 70:
									case 71:
									case 72:
									case 73:
									case 74:
									case 75:
									case 76:
									case 77:
									case 78:
									case 79:
									case 80:
									case 81:
									case 82:
									case 83:
									case 84:
									case 85:
									case 86:
									case 87:
									case 88:
									case 89:
									case 90:
									case 91:
									case 92:
									case 93:
									case 94:
									case 95:
									case 96:
									case 97:
									case 98:
									case 99:
									case 100:
									case 101:
									case 102:
									case 103:
									case 104:
									case 105:
									case 106:
									case 107:
									case 108:
									case 109:
									case 110:
									case 111:
									case 112:
									case 113:
									case 114:
									case 115:
									case 116:
									case 117:
									case 118:
									case 119:
									case 120:
									case 121:
									case 122:
									case 123:
									case 124:
									case 125:
									case 126:
										goto IL_1051;
									case 28:
										num3 = 26;
										goto IL_1051;
									case 127:
										num3 = 28;
										goto IL_1051;
									case 128:
									case 129:
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
									case 141:
									case 142:
									case 143:
									case 144:
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
									case 157:
									case 158:
									case 159:
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
									case 306:
									case 307:
									case 308:
									case 309:
									case 310:
									case 311:
									case 312:
									case 313:
									case 314:
									case 315:
									case 316:
									case 317:
									case 318:
									case 319:
									case 320:
									case 321:
									case 322:
									case 323:
									case 324:
									case 325:
									case 326:
									case 327:
									case 328:
									case 329:
									case 330:
									case 331:
									case 332:
									case 333:
									case 334:
									case 335:
									case 336:
									case 337:
									case 338:
									case 339:
									case 340:
									case 341:
									case 342:
									case 343:
									case 344:
									case 345:
									case 346:
									case 347:
									case 348:
									case 349:
									case 350:
									case 351:
									case 352:
									case 353:
									case 354:
									case 355:
									case 356:
									case 357:
									case 358:
									case 359:
									case 360:
									case 361:
									case 362:
									case 363:
									case 364:
									case 365:
									case 366:
									case 367:
									case 368:
									case 369:
									case 370:
									case 371:
									case 372:
									case 373:
									case 374:
									case 375:
									case 376:
									case 377:
									case 378:
									case 379:
									case 380:
									case 381:
									case 382:
									case 383:
									case 384:
									case 385:
									case 386:
									case 387:
									case 388:
									case 389:
									case 390:
									case 391:
									case 392:
									case 393:
									case 394:
									case 395:
									case 396:
									case 397:
									case 398:
									case 399:
									case 400:
									case 401:
										break;
									case 160:
										num3 = 255;
										goto IL_1051;
									case 161:
										num3 = 173;
										goto IL_1051;
									case 162:
										num3 = 189;
										goto IL_1051;
									case 163:
										num3 = 156;
										goto IL_1051;
									case 164:
										num3 = 207;
										goto IL_1051;
									case 165:
										num3 = 190;
										goto IL_1051;
									case 166:
										num3 = 221;
										goto IL_1051;
									case 167:
										num3 = 245;
										goto IL_1051;
									case 168:
										num3 = 249;
										goto IL_1051;
									case 169:
										num3 = 184;
										goto IL_1051;
									case 170:
										num3 = 166;
										goto IL_1051;
									case 171:
										num3 = 174;
										goto IL_1051;
									case 172:
										num3 = 170;
										goto IL_1051;
									case 173:
										num3 = 240;
										goto IL_1051;
									case 174:
										num3 = 169;
										goto IL_1051;
									case 175:
										num3 = 238;
										goto IL_1051;
									case 176:
										num3 = 248;
										goto IL_1051;
									case 177:
										num3 = 241;
										goto IL_1051;
									case 178:
										num3 = 253;
										goto IL_1051;
									case 179:
										num3 = 252;
										goto IL_1051;
									case 180:
										num3 = 239;
										goto IL_1051;
									case 181:
										num3 = 230;
										goto IL_1051;
									case 182:
										num3 = 244;
										goto IL_1051;
									case 183:
										num3 = 250;
										goto IL_1051;
									case 184:
										num3 = 247;
										goto IL_1051;
									case 185:
										num3 = 251;
										goto IL_1051;
									case 186:
										num3 = 167;
										goto IL_1051;
									case 187:
										num3 = 175;
										goto IL_1051;
									case 188:
										num3 = 172;
										goto IL_1051;
									case 189:
										num3 = 171;
										goto IL_1051;
									case 190:
										num3 = 243;
										goto IL_1051;
									case 191:
										num3 = 168;
										goto IL_1051;
									case 192:
										num3 = 183;
										goto IL_1051;
									case 193:
										num3 = 181;
										goto IL_1051;
									case 194:
										num3 = 182;
										goto IL_1051;
									case 195:
										num3 = 199;
										goto IL_1051;
									case 196:
										num3 = 142;
										goto IL_1051;
									case 197:
										num3 = 143;
										goto IL_1051;
									case 198:
										num3 = 146;
										goto IL_1051;
									case 199:
										num3 = 128;
										goto IL_1051;
									case 200:
										num3 = 212;
										goto IL_1051;
									case 201:
										num3 = 144;
										goto IL_1051;
									case 202:
										num3 = 210;
										goto IL_1051;
									case 203:
										num3 = 211;
										goto IL_1051;
									case 204:
										num3 = 222;
										goto IL_1051;
									case 205:
										num3 = 214;
										goto IL_1051;
									case 206:
										num3 = 215;
										goto IL_1051;
									case 207:
										num3 = 216;
										goto IL_1051;
									case 208:
										num3 = 209;
										goto IL_1051;
									case 209:
										num3 = 165;
										goto IL_1051;
									case 210:
										num3 = 227;
										goto IL_1051;
									case 211:
										num3 = 224;
										goto IL_1051;
									case 212:
										num3 = 226;
										goto IL_1051;
									case 213:
										num3 = 229;
										goto IL_1051;
									case 214:
										num3 = 153;
										goto IL_1051;
									case 215:
										num3 = 158;
										goto IL_1051;
									case 216:
										num3 = 157;
										goto IL_1051;
									case 217:
										num3 = 235;
										goto IL_1051;
									case 218:
										num3 = 233;
										goto IL_1051;
									case 219:
										num3 = 234;
										goto IL_1051;
									case 220:
										num3 = 154;
										goto IL_1051;
									case 221:
										num3 = 237;
										goto IL_1051;
									case 222:
										num3 = 232;
										goto IL_1051;
									case 223:
										num3 = 225;
										goto IL_1051;
									case 224:
										num3 = 133;
										goto IL_1051;
									case 225:
										num3 = 160;
										goto IL_1051;
									case 226:
										num3 = 131;
										goto IL_1051;
									case 227:
										num3 = 198;
										goto IL_1051;
									case 228:
										num3 = 132;
										goto IL_1051;
									case 229:
										num3 = 134;
										goto IL_1051;
									case 230:
										num3 = 145;
										goto IL_1051;
									case 231:
										num3 = 135;
										goto IL_1051;
									case 232:
										num3 = 138;
										goto IL_1051;
									case 233:
										num3 = 130;
										goto IL_1051;
									case 234:
										num3 = 136;
										goto IL_1051;
									case 235:
										num3 = 137;
										goto IL_1051;
									case 236:
										num3 = 141;
										goto IL_1051;
									case 237:
										num3 = 161;
										goto IL_1051;
									case 238:
										num3 = 140;
										goto IL_1051;
									case 239:
										num3 = 139;
										goto IL_1051;
									case 240:
										num3 = 208;
										goto IL_1051;
									case 241:
										num3 = 164;
										goto IL_1051;
									case 242:
										num3 = 149;
										goto IL_1051;
									case 243:
										num3 = 162;
										goto IL_1051;
									case 244:
										num3 = 147;
										goto IL_1051;
									case 245:
										num3 = 228;
										goto IL_1051;
									case 246:
										num3 = 148;
										goto IL_1051;
									case 247:
										num3 = 246;
										goto IL_1051;
									case 248:
										num3 = 155;
										goto IL_1051;
									case 249:
										num3 = 151;
										goto IL_1051;
									case 250:
										num3 = 163;
										goto IL_1051;
									case 251:
										num3 = 150;
										goto IL_1051;
									case 252:
										num3 = 129;
										goto IL_1051;
									case 253:
										num3 = 236;
										goto IL_1051;
									case 254:
										num3 = 231;
										goto IL_1051;
									case 255:
										num3 = 152;
										goto IL_1051;
									case 272:
										num3 = 209;
										goto IL_1051;
									case 305:
										num3 = 213;
										goto IL_1051;
									case 402:
										num3 = 159;
										goto IL_1051;
									default:
										if (num3 == 8215)
										{
											num3 = 242;
											goto IL_1051;
										}
										break;
									}
								}
								else
								{
									if (num3 == 8226)
									{
										num3 = 7;
										goto IL_1051;
									}
									if (num3 == 8252)
									{
										num3 = 19;
										goto IL_1051;
									}
									if (num3 == 8254)
									{
										num3 = 238;
										goto IL_1051;
									}
								}
							}
							else if (num3 <= 8735)
							{
								switch (num3)
								{
								case 8592:
									num3 = 27;
									goto IL_1051;
								case 8593:
									num3 = 24;
									goto IL_1051;
								case 8594:
									num3 = 26;
									goto IL_1051;
								case 8595:
									num3 = 25;
									goto IL_1051;
								case 8596:
									num3 = 29;
									goto IL_1051;
								case 8597:
									num3 = 18;
									goto IL_1051;
								default:
									if (num3 == 8616)
									{
										num3 = 23;
										goto IL_1051;
									}
									if (num3 == 8735)
									{
										num3 = 28;
										goto IL_1051;
									}
									break;
								}
							}
							else
							{
								if (num3 == 8962)
								{
									num3 = 127;
									goto IL_1051;
								}
								if (num3 == 9472)
								{
									num3 = 196;
									goto IL_1051;
								}
								if (num3 == 9474)
								{
									num3 = 179;
									goto IL_1051;
								}
							}
						}
						else if (num3 <= 9508)
						{
							if (num3 <= 9492)
							{
								if (num3 == 9484)
								{
									num3 = 218;
									goto IL_1051;
								}
								if (num3 == 9488)
								{
									num3 = 191;
									goto IL_1051;
								}
								if (num3 == 9492)
								{
									num3 = 192;
									goto IL_1051;
								}
							}
							else
							{
								if (num3 == 9496)
								{
									num3 = 217;
									goto IL_1051;
								}
								if (num3 == 9500)
								{
									num3 = 195;
									goto IL_1051;
								}
								if (num3 == 9508)
								{
									num3 = 180;
									goto IL_1051;
								}
							}
						}
						else if (num3 <= 9532)
						{
							if (num3 == 9516)
							{
								num3 = 194;
								goto IL_1051;
							}
							if (num3 == 9524)
							{
								num3 = 193;
								goto IL_1051;
							}
							if (num3 == 9532)
							{
								num3 = 197;
								goto IL_1051;
							}
						}
						else
						{
							switch (num3)
							{
							case 9552:
								num3 = 205;
								goto IL_1051;
							case 9553:
								num3 = 186;
								goto IL_1051;
							case 9554:
							case 9555:
								break;
							case 9556:
								num3 = 201;
								goto IL_1051;
							default:
								if (num3 == 9559)
								{
									num3 = 187;
									goto IL_1051;
								}
								if (num3 == 9562)
								{
									num3 = 200;
									goto IL_1051;
								}
								break;
							}
						}
					}
					else if (num3 <= 9644)
					{
						if (num3 <= 9580)
						{
							if (num3 <= 9571)
							{
								if (num3 == 9565)
								{
									num3 = 188;
									goto IL_1051;
								}
								if (num3 == 9568)
								{
									num3 = 204;
									goto IL_1051;
								}
								if (num3 == 9571)
								{
									num3 = 185;
									goto IL_1051;
								}
							}
							else
							{
								if (num3 == 9574)
								{
									num3 = 203;
									goto IL_1051;
								}
								if (num3 == 9577)
								{
									num3 = 202;
									goto IL_1051;
								}
								if (num3 == 9580)
								{
									num3 = 206;
									goto IL_1051;
								}
							}
						}
						else if (num3 <= 9608)
						{
							if (num3 == 9600)
							{
								num3 = 223;
								goto IL_1051;
							}
							if (num3 == 9604)
							{
								num3 = 220;
								goto IL_1051;
							}
							if (num3 == 9608)
							{
								num3 = 219;
								goto IL_1051;
							}
						}
						else
						{
							switch (num3)
							{
							case 9617:
								num3 = 176;
								goto IL_1051;
							case 9618:
								num3 = 177;
								goto IL_1051;
							case 9619:
								num3 = 178;
								goto IL_1051;
							default:
								if (num3 == 9632)
								{
									num3 = 254;
									goto IL_1051;
								}
								if (num3 == 9644)
								{
									num3 = 22;
									goto IL_1051;
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
								goto IL_1051;
							}
							if (num3 == 9658)
							{
								num3 = 16;
								goto IL_1051;
							}
							if (num3 == 9660)
							{
								num3 = 31;
								goto IL_1051;
							}
						}
						else
						{
							if (num3 == 9668)
							{
								num3 = 17;
								goto IL_1051;
							}
							if (num3 == 9675)
							{
								num3 = 9;
								goto IL_1051;
							}
							if (num3 == 9688)
							{
								num3 = 8;
								goto IL_1051;
							}
						}
					}
					else if (num3 <= 9830)
					{
						if (num3 == 9689)
						{
							num3 = 10;
							goto IL_1051;
						}
						switch (num3)
						{
						case 9786:
							num3 = 1;
							goto IL_1051;
						case 9787:
							num3 = 2;
							goto IL_1051;
						case 9788:
							num3 = 15;
							goto IL_1051;
						case 9789:
						case 9790:
						case 9791:
						case 9793:
							break;
						case 9792:
							num3 = 12;
							goto IL_1051;
						case 9794:
							num3 = 11;
							goto IL_1051;
						default:
							switch (num3)
							{
							case 9824:
								num3 = 6;
								goto IL_1051;
							case 9827:
								num3 = 5;
								goto IL_1051;
							case 9829:
								num3 = 3;
								goto IL_1051;
							case 9830:
								num3 = 4;
								goto IL_1051;
							}
							break;
						}
					}
					else
					{
						if (num3 == 9834)
						{
							num3 = 13;
							goto IL_1051;
						}
						if (num3 == 9836)
						{
							num3 = 14;
							goto IL_1051;
						}
						switch (num3)
						{
						case 65512:
							num3 = 179;
							goto IL_1051;
						case 65513:
							num3 = 27;
							goto IL_1051;
						case 65514:
							num3 = 24;
							goto IL_1051;
						case 65515:
							num3 = 26;
							goto IL_1051;
						case 65516:
							num3 = 25;
							goto IL_1051;
						case 65517:
							num3 = 254;
							goto IL_1051;
						case 65518:
							num3 = 9;
							goto IL_1051;
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
				IL_1051:
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

		// Token: 0x04000034 RID: 52
		private static readonly char[] ToChars = new char[]
		{
			'\0', '\u0001', '\u0002', '\u0003', '\u0004', '\u0005', '\u0006', '\a', '\b', '\t',
			'\n', '\v', '\f', '\r', '\u000e', '\u000f', '\u0010', '\u0011', '\u0012', '\u0013',
			'\u0014', '\u0015', '\u0016', '\u0017', '\u0018', '\u0019', '\u001c', '\u001b', '\u007f', '\u001d',
			'\u001e', '\u001f', ' ', '!', '"', '#', '$', '%', '&', '\'',
			'(', ')', '*', '+', ',', '-', '.', '/', '0', '1',
			'2', '3', '4', '5', '6', '7', '8', '9', ':', ';',
			'<', '=', '>', '?', '@', 'A', 'B', 'C', 'D', 'E',
			'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O',
			'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y',
			'Z', '[', '\\', ']', '^', '_', '`', 'a', 'b', 'c',
			'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm',
			'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w',
			'x', 'y', 'z', '{', '|', '}', '~', '\u001a', 'Ç', 'ü',
			'é', 'â', 'ä', 'à', 'å', 'ç', 'ê', 'ë', 'è', 'ï',
			'î', 'ì', 'Ä', 'Å', 'É', 'æ', 'Æ', 'ô', 'ö', 'ò',
			'û', 'ù', 'ÿ', 'Ö', 'Ü', 'ø', '£', 'Ø', '×', 'ƒ',
			'á', 'í', 'ó', 'ú', 'ñ', 'Ñ', 'ª', 'º', '¿', '®',
			'¬', '½', '¼', '¡', '«', '»', '░', '▒', '▓', '│',
			'┤', 'Á', 'Â', 'À', '©', '╣', '║', '╗', '╝', '¢',
			'¥', '┐', '└', '┴', '┬', '├', '─', '┼', 'ã', 'Ã',
			'╚', '╔', '╩', '╦', '╠', '═', '╬', '¤', 'ð', 'Ð',
			'Ê', 'Ë', 'È', 'ı', 'Í', 'Î', 'Ï', '┘', '┌', '█',
			'▄', '¦', 'Ì', '▀', 'Ó', 'ß', 'Ô', 'Ò', 'õ', 'Õ',
			'µ', 'þ', 'Þ', 'Ú', 'Û', 'Ù', 'ý', 'Ý', '\u00af', '\u00b4',
			'\u00ad', '±', '‗', '¾', '¶', '§', '÷', '\u00b8', '°', '\u00a8',
			'·', '¹', '³', '²', '■', '\u00a0'
		};
	}
}
