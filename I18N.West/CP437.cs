using System;
using System.Runtime.CompilerServices;
using System.Text;
using I18N.Common;

namespace I18N.West
{
	// Token: 0x02000015 RID: 21
	[Serializable]
	public class CP437 : ByteEncoding
	{
		// Token: 0x06000040 RID: 64 RVA: 0x00006448 File Offset: 0x00004648
		public CP437()
			: base(437, CP437.ToChars, "OEM United States", "IBM437", "IBM437", "IBM437", false, false, false, false, 1252)
		{
		}

		// Token: 0x06000041 RID: 65 RVA: 0x00006482 File Offset: 0x00004682
		public unsafe override int GetByteCountImpl(char* chars, int count)
		{
			if (base.EncoderFallback != null)
			{
				return this.GetBytesImpl(chars, count, null, 0);
			}
			return count;
		}

		// Token: 0x06000042 RID: 66 RVA: 0x0000649C File Offset: 0x0000469C
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

		// Token: 0x06000043 RID: 67 RVA: 0x000064D8 File Offset: 0x000046D8
		protected unsafe override void ToBytes(char* chars, int charCount, byte* bytes, int byteCount)
		{
			if (bytes == null)
			{
				throw new ArgumentNullException("bytes");
			}
			this.GetBytesImpl(chars, charCount, bytes, byteCount);
		}

		// Token: 0x06000044 RID: 68 RVA: 0x000064F8 File Offset: 0x000046F8
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
					if (num3 <= 8962)
					{
						if (num3 <= 8240)
						{
							if (num3 <= 894)
							{
								if (num3 <= 732)
								{
									if (num3 <= 700)
									{
										switch (num3)
										{
										case 160:
											num3 = 255;
											goto IL_22F8;
										case 161:
											num3 = 173;
											goto IL_22F8;
										case 162:
											num3 = 155;
											goto IL_22F8;
										case 163:
											num3 = 156;
											goto IL_22F8;
										case 164:
											num3 = 15;
											goto IL_22F8;
										case 165:
											num3 = 157;
											goto IL_22F8;
										case 166:
											num3 = 221;
											goto IL_22F8;
										case 167:
											num3 = 21;
											goto IL_22F8;
										case 168:
											num3 = 34;
											goto IL_22F8;
										case 169:
											num3 = 99;
											goto IL_22F8;
										case 170:
											num3 = 166;
											goto IL_22F8;
										case 171:
											num3 = 174;
											goto IL_22F8;
										case 172:
											num3 = 170;
											goto IL_22F8;
										case 173:
											num3 = 45;
											goto IL_22F8;
										case 174:
											num3 = 114;
											goto IL_22F8;
										case 175:
											num3 = 95;
											goto IL_22F8;
										case 176:
											num3 = 248;
											goto IL_22F8;
										case 177:
											num3 = 241;
											goto IL_22F8;
										case 178:
											num3 = 253;
											goto IL_22F8;
										case 179:
											num3 = 51;
											goto IL_22F8;
										case 180:
											num3 = 39;
											goto IL_22F8;
										case 181:
											num3 = 230;
											goto IL_22F8;
										case 182:
											num3 = 20;
											goto IL_22F8;
										case 183:
											num3 = 250;
											goto IL_22F8;
										case 184:
											num3 = 44;
											goto IL_22F8;
										case 185:
											num3 = 49;
											goto IL_22F8;
										case 186:
											num3 = 167;
											goto IL_22F8;
										case 187:
											num3 = 175;
											goto IL_22F8;
										case 188:
											num3 = 172;
											goto IL_22F8;
										case 189:
											num3 = 171;
											goto IL_22F8;
										case 190:
											num3 = 95;
											goto IL_22F8;
										case 191:
											num3 = 168;
											goto IL_22F8;
										case 192:
											num3 = 65;
											goto IL_22F8;
										case 193:
											num3 = 65;
											goto IL_22F8;
										case 194:
											num3 = 65;
											goto IL_22F8;
										case 195:
											num3 = 65;
											goto IL_22F8;
										case 196:
											num3 = 142;
											goto IL_22F8;
										case 197:
											num3 = 143;
											goto IL_22F8;
										case 198:
											num3 = 146;
											goto IL_22F8;
										case 199:
											num3 = 128;
											goto IL_22F8;
										case 200:
											num3 = 69;
											goto IL_22F8;
										case 201:
											num3 = 144;
											goto IL_22F8;
										case 202:
											num3 = 69;
											goto IL_22F8;
										case 203:
											num3 = 69;
											goto IL_22F8;
										case 204:
											num3 = 73;
											goto IL_22F8;
										case 205:
											num3 = 73;
											goto IL_22F8;
										case 206:
											num3 = 73;
											goto IL_22F8;
										case 207:
											num3 = 73;
											goto IL_22F8;
										case 208:
											num3 = 68;
											goto IL_22F8;
										case 209:
											num3 = 165;
											goto IL_22F8;
										case 210:
											num3 = 79;
											goto IL_22F8;
										case 211:
											num3 = 79;
											goto IL_22F8;
										case 212:
											num3 = 79;
											goto IL_22F8;
										case 213:
											num3 = 79;
											goto IL_22F8;
										case 214:
											num3 = 153;
											goto IL_22F8;
										case 215:
											num3 = 120;
											goto IL_22F8;
										case 216:
											num3 = 79;
											goto IL_22F8;
										case 217:
											num3 = 85;
											goto IL_22F8;
										case 218:
											num3 = 85;
											goto IL_22F8;
										case 219:
											num3 = 85;
											goto IL_22F8;
										case 220:
											num3 = 154;
											goto IL_22F8;
										case 221:
											num3 = 89;
											goto IL_22F8;
										case 222:
											num3 = 95;
											goto IL_22F8;
										case 223:
											num3 = 225;
											goto IL_22F8;
										case 224:
											num3 = 133;
											goto IL_22F8;
										case 225:
											num3 = 160;
											goto IL_22F8;
										case 226:
											num3 = 131;
											goto IL_22F8;
										case 227:
											num3 = 97;
											goto IL_22F8;
										case 228:
											num3 = 132;
											goto IL_22F8;
										case 229:
											num3 = 134;
											goto IL_22F8;
										case 230:
											num3 = 145;
											goto IL_22F8;
										case 231:
											num3 = 135;
											goto IL_22F8;
										case 232:
											num3 = 138;
											goto IL_22F8;
										case 233:
											num3 = 130;
											goto IL_22F8;
										case 234:
											num3 = 136;
											goto IL_22F8;
										case 235:
											num3 = 137;
											goto IL_22F8;
										case 236:
											num3 = 141;
											goto IL_22F8;
										case 237:
											num3 = 161;
											goto IL_22F8;
										case 238:
											num3 = 140;
											goto IL_22F8;
										case 239:
											num3 = 139;
											goto IL_22F8;
										case 240:
											num3 = 100;
											goto IL_22F8;
										case 241:
											num3 = 164;
											goto IL_22F8;
										case 242:
											num3 = 149;
											goto IL_22F8;
										case 243:
											num3 = 162;
											goto IL_22F8;
										case 244:
											num3 = 147;
											goto IL_22F8;
										case 245:
											num3 = 111;
											goto IL_22F8;
										case 246:
											num3 = 148;
											goto IL_22F8;
										case 247:
											num3 = 246;
											goto IL_22F8;
										case 248:
											num3 = 111;
											goto IL_22F8;
										case 249:
											num3 = 151;
											goto IL_22F8;
										case 250:
											num3 = 163;
											goto IL_22F8;
										case 251:
											num3 = 150;
											goto IL_22F8;
										case 252:
											num3 = 129;
											goto IL_22F8;
										case 253:
											num3 = 121;
											goto IL_22F8;
										case 254:
											num3 = 95;
											goto IL_22F8;
										case 255:
											num3 = 152;
											goto IL_22F8;
										case 256:
											num3 = 65;
											goto IL_22F8;
										case 257:
											num3 = 97;
											goto IL_22F8;
										case 258:
											num3 = 65;
											goto IL_22F8;
										case 259:
											num3 = 97;
											goto IL_22F8;
										case 260:
											num3 = 65;
											goto IL_22F8;
										case 261:
											num3 = 97;
											goto IL_22F8;
										case 262:
											num3 = 67;
											goto IL_22F8;
										case 263:
											num3 = 99;
											goto IL_22F8;
										case 264:
											num3 = 67;
											goto IL_22F8;
										case 265:
											num3 = 99;
											goto IL_22F8;
										case 266:
											num3 = 67;
											goto IL_22F8;
										case 267:
											num3 = 99;
											goto IL_22F8;
										case 268:
											num3 = 67;
											goto IL_22F8;
										case 269:
											num3 = 99;
											goto IL_22F8;
										case 270:
											num3 = 68;
											goto IL_22F8;
										case 271:
											num3 = 100;
											goto IL_22F8;
										case 272:
											num3 = 68;
											goto IL_22F8;
										case 273:
											num3 = 100;
											goto IL_22F8;
										case 274:
											num3 = 69;
											goto IL_22F8;
										case 275:
											num3 = 101;
											goto IL_22F8;
										case 276:
											num3 = 69;
											goto IL_22F8;
										case 277:
											num3 = 101;
											goto IL_22F8;
										case 278:
											num3 = 69;
											goto IL_22F8;
										case 279:
											num3 = 101;
											goto IL_22F8;
										case 280:
											num3 = 69;
											goto IL_22F8;
										case 281:
											num3 = 101;
											goto IL_22F8;
										case 282:
											num3 = 69;
											goto IL_22F8;
										case 283:
											num3 = 101;
											goto IL_22F8;
										case 284:
											num3 = 71;
											goto IL_22F8;
										case 285:
											num3 = 103;
											goto IL_22F8;
										case 286:
											num3 = 71;
											goto IL_22F8;
										case 287:
											num3 = 103;
											goto IL_22F8;
										case 288:
											num3 = 71;
											goto IL_22F8;
										case 289:
											num3 = 103;
											goto IL_22F8;
										case 290:
											num3 = 71;
											goto IL_22F8;
										case 291:
											num3 = 103;
											goto IL_22F8;
										case 292:
											num3 = 72;
											goto IL_22F8;
										case 293:
											num3 = 104;
											goto IL_22F8;
										case 294:
											num3 = 72;
											goto IL_22F8;
										case 295:
											num3 = 104;
											goto IL_22F8;
										case 296:
											num3 = 73;
											goto IL_22F8;
										case 297:
											num3 = 105;
											goto IL_22F8;
										case 298:
											num3 = 73;
											goto IL_22F8;
										case 299:
											num3 = 105;
											goto IL_22F8;
										case 300:
											num3 = 73;
											goto IL_22F8;
										case 301:
											num3 = 105;
											goto IL_22F8;
										case 302:
											num3 = 73;
											goto IL_22F8;
										case 303:
											num3 = 105;
											goto IL_22F8;
										case 304:
											num3 = 73;
											goto IL_22F8;
										case 305:
											num3 = 105;
											goto IL_22F8;
										case 306:
										case 307:
										case 312:
										case 319:
										case 320:
										case 329:
										case 330:
										case 331:
										case 383:
										case 385:
										case 386:
										case 387:
										case 388:
										case 389:
										case 390:
										case 391:
										case 392:
										case 394:
										case 395:
										case 396:
										case 397:
										case 398:
										case 399:
										case 400:
										case 403:
										case 404:
										case 405:
										case 406:
										case 408:
										case 409:
										case 411:
										case 412:
										case 413:
										case 414:
										case 418:
										case 419:
										case 420:
										case 421:
										case 422:
										case 423:
										case 424:
										case 426:
										case 428:
										case 429:
										case 433:
										case 434:
										case 435:
										case 436:
										case 437:
										case 439:
										case 440:
										case 441:
										case 442:
										case 443:
										case 444:
										case 445:
										case 446:
										case 447:
										case 449:
										case 450:
										case 452:
										case 453:
										case 454:
										case 455:
										case 456:
										case 457:
										case 458:
										case 459:
										case 460:
										case 477:
										case 480:
										case 481:
										case 482:
										case 483:
										case 494:
										case 495:
										case 497:
										case 498:
										case 499:
										case 500:
										case 501:
										case 502:
										case 503:
										case 504:
										case 505:
										case 506:
										case 507:
										case 508:
										case 509:
										case 510:
										case 511:
										case 512:
										case 513:
										case 514:
										case 515:
										case 516:
										case 517:
										case 518:
										case 519:
										case 520:
										case 521:
										case 522:
										case 523:
										case 524:
										case 525:
										case 526:
										case 527:
										case 528:
										case 529:
										case 530:
										case 531:
										case 532:
										case 533:
										case 534:
										case 535:
										case 536:
										case 537:
										case 538:
										case 539:
										case 540:
										case 541:
										case 542:
										case 543:
										case 544:
										case 545:
										case 546:
										case 547:
										case 548:
										case 549:
										case 550:
										case 551:
										case 552:
										case 553:
										case 554:
										case 555:
										case 556:
										case 557:
										case 558:
										case 559:
										case 560:
										case 561:
										case 562:
										case 563:
										case 564:
										case 565:
										case 566:
										case 567:
										case 568:
										case 569:
										case 570:
										case 571:
										case 572:
										case 573:
										case 574:
										case 575:
										case 576:
										case 577:
										case 578:
										case 579:
										case 580:
										case 581:
										case 582:
										case 583:
										case 584:
										case 585:
										case 586:
										case 587:
										case 588:
										case 589:
										case 590:
										case 591:
										case 592:
										case 593:
										case 594:
										case 595:
										case 596:
										case 597:
										case 598:
										case 599:
										case 600:
										case 601:
										case 602:
										case 603:
										case 604:
										case 605:
										case 606:
										case 607:
										case 608:
										case 610:
										case 611:
										case 612:
										case 613:
										case 614:
										case 615:
										case 616:
										case 617:
										case 618:
										case 619:
										case 620:
										case 621:
										case 622:
										case 623:
										case 624:
										case 625:
										case 626:
										case 627:
										case 628:
										case 629:
										case 630:
										case 631:
											break;
										case 308:
											num3 = 74;
											goto IL_22F8;
										case 309:
											num3 = 106;
											goto IL_22F8;
										case 310:
											num3 = 75;
											goto IL_22F8;
										case 311:
											num3 = 107;
											goto IL_22F8;
										case 313:
											num3 = 76;
											goto IL_22F8;
										case 314:
											num3 = 108;
											goto IL_22F8;
										case 315:
											num3 = 76;
											goto IL_22F8;
										case 316:
											num3 = 108;
											goto IL_22F8;
										case 317:
											num3 = 76;
											goto IL_22F8;
										case 318:
											num3 = 108;
											goto IL_22F8;
										case 321:
											num3 = 76;
											goto IL_22F8;
										case 322:
											num3 = 108;
											goto IL_22F8;
										case 323:
											num3 = 78;
											goto IL_22F8;
										case 324:
											num3 = 110;
											goto IL_22F8;
										case 325:
											num3 = 78;
											goto IL_22F8;
										case 326:
											num3 = 110;
											goto IL_22F8;
										case 327:
											num3 = 78;
											goto IL_22F8;
										case 328:
											num3 = 110;
											goto IL_22F8;
										case 332:
											num3 = 79;
											goto IL_22F8;
										case 333:
											num3 = 111;
											goto IL_22F8;
										case 334:
											num3 = 79;
											goto IL_22F8;
										case 335:
											num3 = 111;
											goto IL_22F8;
										case 336:
											num3 = 79;
											goto IL_22F8;
										case 337:
											num3 = 111;
											goto IL_22F8;
										case 338:
											num3 = 79;
											goto IL_22F8;
										case 339:
											num3 = 111;
											goto IL_22F8;
										case 340:
											num3 = 82;
											goto IL_22F8;
										case 341:
											num3 = 114;
											goto IL_22F8;
										case 342:
											num3 = 82;
											goto IL_22F8;
										case 343:
											num3 = 114;
											goto IL_22F8;
										case 344:
											num3 = 82;
											goto IL_22F8;
										case 345:
											num3 = 114;
											goto IL_22F8;
										case 346:
											num3 = 83;
											goto IL_22F8;
										case 347:
											num3 = 115;
											goto IL_22F8;
										case 348:
											num3 = 83;
											goto IL_22F8;
										case 349:
											num3 = 115;
											goto IL_22F8;
										case 350:
											num3 = 83;
											goto IL_22F8;
										case 351:
											num3 = 115;
											goto IL_22F8;
										case 352:
											num3 = 83;
											goto IL_22F8;
										case 353:
											num3 = 115;
											goto IL_22F8;
										case 354:
											num3 = 84;
											goto IL_22F8;
										case 355:
											num3 = 116;
											goto IL_22F8;
										case 356:
											num3 = 84;
											goto IL_22F8;
										case 357:
											num3 = 116;
											goto IL_22F8;
										case 358:
											num3 = 84;
											goto IL_22F8;
										case 359:
											num3 = 116;
											goto IL_22F8;
										case 360:
											num3 = 85;
											goto IL_22F8;
										case 361:
											num3 = 117;
											goto IL_22F8;
										case 362:
											num3 = 85;
											goto IL_22F8;
										case 363:
											num3 = 117;
											goto IL_22F8;
										case 364:
											num3 = 85;
											goto IL_22F8;
										case 365:
											num3 = 117;
											goto IL_22F8;
										case 366:
											num3 = 85;
											goto IL_22F8;
										case 367:
											num3 = 117;
											goto IL_22F8;
										case 368:
											num3 = 85;
											goto IL_22F8;
										case 369:
											num3 = 117;
											goto IL_22F8;
										case 370:
											num3 = 85;
											goto IL_22F8;
										case 371:
											num3 = 117;
											goto IL_22F8;
										case 372:
											num3 = 87;
											goto IL_22F8;
										case 373:
											num3 = 119;
											goto IL_22F8;
										case 374:
											num3 = 89;
											goto IL_22F8;
										case 375:
											num3 = 121;
											goto IL_22F8;
										case 376:
											num3 = 89;
											goto IL_22F8;
										case 377:
											num3 = 90;
											goto IL_22F8;
										case 378:
											num3 = 122;
											goto IL_22F8;
										case 379:
											num3 = 90;
											goto IL_22F8;
										case 380:
											num3 = 122;
											goto IL_22F8;
										case 381:
											num3 = 90;
											goto IL_22F8;
										case 382:
											num3 = 122;
											goto IL_22F8;
										case 384:
											num3 = 98;
											goto IL_22F8;
										case 393:
											num3 = 68;
											goto IL_22F8;
										case 401:
											num3 = 159;
											goto IL_22F8;
										case 402:
											num3 = 159;
											goto IL_22F8;
										case 407:
											num3 = 73;
											goto IL_22F8;
										case 410:
											num3 = 108;
											goto IL_22F8;
										case 415:
											num3 = 79;
											goto IL_22F8;
										case 416:
											num3 = 79;
											goto IL_22F8;
										case 417:
											num3 = 111;
											goto IL_22F8;
										case 425:
											num3 = 228;
											goto IL_22F8;
										case 427:
											num3 = 116;
											goto IL_22F8;
										case 430:
											num3 = 84;
											goto IL_22F8;
										case 431:
											num3 = 85;
											goto IL_22F8;
										case 432:
											num3 = 117;
											goto IL_22F8;
										case 438:
											num3 = 122;
											goto IL_22F8;
										case 448:
											num3 = 124;
											goto IL_22F8;
										case 451:
											num3 = 33;
											goto IL_22F8;
										case 461:
											num3 = 65;
											goto IL_22F8;
										case 462:
											num3 = 97;
											goto IL_22F8;
										case 463:
											num3 = 73;
											goto IL_22F8;
										case 464:
											num3 = 105;
											goto IL_22F8;
										case 465:
											num3 = 79;
											goto IL_22F8;
										case 466:
											num3 = 111;
											goto IL_22F8;
										case 467:
											num3 = 85;
											goto IL_22F8;
										case 468:
											num3 = 117;
											goto IL_22F8;
										case 469:
											num3 = 85;
											goto IL_22F8;
										case 470:
											num3 = 117;
											goto IL_22F8;
										case 471:
											num3 = 85;
											goto IL_22F8;
										case 472:
											num3 = 117;
											goto IL_22F8;
										case 473:
											num3 = 85;
											goto IL_22F8;
										case 474:
											num3 = 117;
											goto IL_22F8;
										case 475:
											num3 = 85;
											goto IL_22F8;
										case 476:
											num3 = 117;
											goto IL_22F8;
										case 478:
											num3 = 65;
											goto IL_22F8;
										case 479:
											num3 = 97;
											goto IL_22F8;
										case 484:
											num3 = 71;
											goto IL_22F8;
										case 485:
											num3 = 103;
											goto IL_22F8;
										case 486:
											num3 = 71;
											goto IL_22F8;
										case 487:
											num3 = 103;
											goto IL_22F8;
										case 488:
											num3 = 75;
											goto IL_22F8;
										case 489:
											num3 = 107;
											goto IL_22F8;
										case 490:
											num3 = 79;
											goto IL_22F8;
										case 491:
											num3 = 111;
											goto IL_22F8;
										case 492:
											num3 = 79;
											goto IL_22F8;
										case 493:
											num3 = 111;
											goto IL_22F8;
										case 496:
											num3 = 106;
											goto IL_22F8;
										case 609:
											num3 = 103;
											goto IL_22F8;
										case 632:
											num3 = 237;
											goto IL_22F8;
										default:
											switch (num3)
											{
											case 697:
												num3 = 39;
												goto IL_22F8;
											case 698:
												num3 = 34;
												goto IL_22F8;
											case 700:
												num3 = 39;
												goto IL_22F8;
											}
											break;
										}
									}
									else
									{
										switch (num3)
										{
										case 708:
											num3 = 94;
											goto IL_22F8;
										case 709:
										case 711:
										case 716:
											break;
										case 710:
											num3 = 94;
											goto IL_22F8;
										case 712:
											num3 = 39;
											goto IL_22F8;
										case 713:
											num3 = 196;
											goto IL_22F8;
										case 714:
											num3 = 39;
											goto IL_22F8;
										case 715:
											num3 = 96;
											goto IL_22F8;
										case 717:
											num3 = 95;
											goto IL_22F8;
										default:
											if (num3 == 730)
											{
												num3 = 248;
												goto IL_22F8;
											}
											if (num3 == 732)
											{
												num3 = 126;
												goto IL_22F8;
											}
											break;
										}
									}
								}
								else if (num3 <= 807)
								{
									switch (num3)
									{
									case 768:
										num3 = 96;
										goto IL_22F8;
									case 769:
										num3 = 39;
										goto IL_22F8;
									case 770:
										num3 = 94;
										goto IL_22F8;
									case 771:
										num3 = 126;
										goto IL_22F8;
									case 772:
										num3 = 196;
										goto IL_22F8;
									case 773:
									case 774:
									case 775:
									case 777:
									case 779:
									case 780:
									case 781:
										break;
									case 776:
										num3 = 34;
										goto IL_22F8;
									case 778:
										num3 = 248;
										goto IL_22F8;
									case 782:
										num3 = 34;
										goto IL_22F8;
									default:
										if (num3 == 807)
										{
											num3 = 44;
											goto IL_22F8;
										}
										break;
									}
								}
								else
								{
									if (num3 == 817)
									{
										num3 = 95;
										goto IL_22F8;
									}
									if (num3 == 818)
									{
										num3 = 95;
										goto IL_22F8;
									}
									if (num3 == 894)
									{
										num3 = 59;
										goto IL_22F8;
									}
								}
							}
							else if (num3 <= 956)
							{
								if (num3 <= 934)
								{
									switch (num3)
									{
									case 913:
										num3 = 224;
										goto IL_22F8;
									case 914:
									case 918:
									case 919:
										break;
									case 915:
										num3 = 226;
										goto IL_22F8;
									case 916:
										num3 = 235;
										goto IL_22F8;
									case 917:
										num3 = 238;
										goto IL_22F8;
									case 920:
										num3 = 233;
										goto IL_22F8;
									default:
										switch (num3)
										{
										case 928:
											num3 = 227;
											goto IL_22F8;
										case 931:
											num3 = 228;
											goto IL_22F8;
										case 932:
											num3 = 231;
											goto IL_22F8;
										case 934:
											num3 = 232;
											goto IL_22F8;
										}
										break;
									}
								}
								else
								{
									if (num3 == 937)
									{
										num3 = 234;
										goto IL_22F8;
									}
									switch (num3)
									{
									case 945:
										num3 = 224;
										goto IL_22F8;
									case 946:
										num3 = 225;
										goto IL_22F8;
									case 947:
										break;
									case 948:
										num3 = 235;
										goto IL_22F8;
									case 949:
										num3 = 238;
										goto IL_22F8;
									default:
										if (num3 == 956)
										{
											num3 = 230;
											goto IL_22F8;
										}
										break;
									}
								}
							}
							else if (num3 <= 1417)
							{
								switch (num3)
								{
								case 960:
									num3 = 227;
									goto IL_22F8;
								case 961:
								case 962:
								case 965:
									break;
								case 963:
									num3 = 229;
									goto IL_22F8;
								case 964:
									num3 = 231;
									goto IL_22F8;
								case 966:
									num3 = 237;
									goto IL_22F8;
								default:
									if (num3 == 1211)
									{
										num3 = 104;
										goto IL_22F8;
									}
									if (num3 == 1417)
									{
										num3 = 58;
										goto IL_22F8;
									}
									break;
								}
							}
							else
							{
								if (num3 == 1642)
								{
									num3 = 37;
									goto IL_22F8;
								}
								switch (num3)
								{
								case 8192:
									num3 = 32;
									goto IL_22F8;
								case 8193:
									num3 = 32;
									goto IL_22F8;
								case 8194:
									num3 = 32;
									goto IL_22F8;
								case 8195:
									num3 = 32;
									goto IL_22F8;
								case 8196:
									num3 = 32;
									goto IL_22F8;
								case 8197:
									num3 = 32;
									goto IL_22F8;
								case 8198:
									num3 = 32;
									goto IL_22F8;
								case 8199:
								case 8200:
								case 8201:
								case 8202:
								case 8203:
								case 8204:
								case 8205:
								case 8206:
								case 8207:
								case 8210:
								case 8213:
								case 8214:
								case 8219:
								case 8223:
								case 8227:
								case 8229:
									break;
								case 8208:
									num3 = 45;
									goto IL_22F8;
								case 8209:
									num3 = 45;
									goto IL_22F8;
								case 8211:
									num3 = 45;
									goto IL_22F8;
								case 8212:
									num3 = 45;
									goto IL_22F8;
								case 8215:
									num3 = 95;
									goto IL_22F8;
								case 8216:
									num3 = 96;
									goto IL_22F8;
								case 8217:
									num3 = 39;
									goto IL_22F8;
								case 8218:
									num3 = 44;
									goto IL_22F8;
								case 8220:
									num3 = 34;
									goto IL_22F8;
								case 8221:
									num3 = 34;
									goto IL_22F8;
								case 8222:
									num3 = 44;
									goto IL_22F8;
								case 8224:
									num3 = 43;
									goto IL_22F8;
								case 8225:
									num3 = 216;
									goto IL_22F8;
								case 8226:
									num3 = 7;
									goto IL_22F8;
								case 8228:
									num3 = 250;
									goto IL_22F8;
								case 8230:
									num3 = 46;
									goto IL_22F8;
								default:
									if (num3 == 8240)
									{
										num3 = 37;
										goto IL_22F8;
									}
									break;
								}
							}
						}
						else if (num3 <= 8597)
						{
							if (num3 <= 8329)
							{
								if (num3 <= 8245)
								{
									if (num3 == 8242)
									{
										num3 = 39;
										goto IL_22F8;
									}
									if (num3 == 8245)
									{
										num3 = 96;
										goto IL_22F8;
									}
								}
								else
								{
									switch (num3)
									{
									case 8249:
										num3 = 60;
										goto IL_22F8;
									case 8250:
										num3 = 62;
										goto IL_22F8;
									case 8251:
										break;
									case 8252:
										num3 = 19;
										goto IL_22F8;
									default:
										if (num3 == 8260)
										{
											num3 = 47;
											goto IL_22F8;
										}
										switch (num3)
										{
										case 8304:
											num3 = 248;
											goto IL_22F8;
										case 8308:
										case 8309:
										case 8310:
										case 8311:
										case 8312:
											num3 -= 8256;
											goto IL_22F8;
										case 8319:
											num3 = 252;
											goto IL_22F8;
										case 8320:
										case 8321:
										case 8322:
										case 8323:
										case 8324:
										case 8325:
										case 8326:
										case 8327:
										case 8328:
										case 8329:
											num3 -= 8272;
											goto IL_22F8;
										}
										break;
									}
								}
							}
							else if (num3 <= 8359)
							{
								if (num3 == 8356)
								{
									num3 = 156;
									goto IL_22F8;
								}
								if (num3 == 8359)
								{
									num3 = 158;
									goto IL_22F8;
								}
							}
							else
							{
								if (num3 == 8413)
								{
									num3 = 9;
									goto IL_22F8;
								}
								switch (num3)
								{
								case 8450:
									num3 = 67;
									goto IL_22F8;
								case 8451:
								case 8452:
								case 8453:
								case 8454:
								case 8456:
								case 8457:
								case 8463:
								case 8468:
								case 8470:
								case 8471:
								case 8478:
								case 8479:
								case 8480:
								case 8481:
								case 8483:
								case 8485:
								case 8487:
								case 8489:
								case 8498:
									break;
								case 8455:
									num3 = 69;
									goto IL_22F8;
								case 8458:
									num3 = 103;
									goto IL_22F8;
								case 8459:
									num3 = 72;
									goto IL_22F8;
								case 8460:
									num3 = 72;
									goto IL_22F8;
								case 8461:
									num3 = 72;
									goto IL_22F8;
								case 8462:
									num3 = 104;
									goto IL_22F8;
								case 8464:
									num3 = 73;
									goto IL_22F8;
								case 8465:
									num3 = 73;
									goto IL_22F8;
								case 8466:
									num3 = 76;
									goto IL_22F8;
								case 8467:
									num3 = 108;
									goto IL_22F8;
								case 8469:
									num3 = 78;
									goto IL_22F8;
								case 8472:
									num3 = 80;
									goto IL_22F8;
								case 8473:
									num3 = 80;
									goto IL_22F8;
								case 8474:
									num3 = 81;
									goto IL_22F8;
								case 8475:
									num3 = 82;
									goto IL_22F8;
								case 8476:
									num3 = 82;
									goto IL_22F8;
								case 8477:
									num3 = 82;
									goto IL_22F8;
								case 8482:
									num3 = 84;
									goto IL_22F8;
								case 8484:
									num3 = 90;
									goto IL_22F8;
								case 8486:
									num3 = 234;
									goto IL_22F8;
								case 8488:
									num3 = 90;
									goto IL_22F8;
								case 8490:
									num3 = 75;
									goto IL_22F8;
								case 8491:
									num3 = 143;
									goto IL_22F8;
								case 8492:
									num3 = 66;
									goto IL_22F8;
								case 8493:
									num3 = 67;
									goto IL_22F8;
								case 8494:
									num3 = 101;
									goto IL_22F8;
								case 8495:
									num3 = 101;
									goto IL_22F8;
								case 8496:
									num3 = 69;
									goto IL_22F8;
								case 8497:
									num3 = 70;
									goto IL_22F8;
								case 8499:
									num3 = 77;
									goto IL_22F8;
								case 8500:
									num3 = 111;
									goto IL_22F8;
								default:
									switch (num3)
									{
									case 8592:
										num3 = 27;
										goto IL_22F8;
									case 8593:
										num3 = 24;
										goto IL_22F8;
									case 8594:
										num3 = 26;
										goto IL_22F8;
									case 8595:
										num3 = 25;
										goto IL_22F8;
									case 8596:
										num3 = 29;
										goto IL_22F8;
									case 8597:
										num3 = 18;
										goto IL_22F8;
									}
									break;
								}
							}
						}
						else if (num3 <= 8764)
						{
							if (num3 <= 8709)
							{
								if (num3 == 8616)
								{
									num3 = 23;
									goto IL_22F8;
								}
								if (num3 == 8709)
								{
									num3 = 237;
									goto IL_22F8;
								}
							}
							else
							{
								switch (num3)
								{
								case 8721:
									num3 = 228;
									goto IL_22F8;
								case 8722:
									num3 = 45;
									goto IL_22F8;
								case 8723:
									num3 = 241;
									goto IL_22F8;
								case 8724:
								case 8731:
								case 8732:
								case 8733:
								case 8736:
								case 8737:
								case 8738:
								case 8740:
								case 8741:
								case 8742:
								case 8743:
								case 8744:
									break;
								case 8725:
									num3 = 47;
									goto IL_22F8;
								case 8726:
									num3 = 92;
									goto IL_22F8;
								case 8727:
									num3 = 42;
									goto IL_22F8;
								case 8728:
									num3 = 248;
									goto IL_22F8;
								case 8729:
									num3 = 249;
									goto IL_22F8;
								case 8730:
									num3 = 251;
									goto IL_22F8;
								case 8734:
									num3 = 236;
									goto IL_22F8;
								case 8735:
									num3 = 28;
									goto IL_22F8;
								case 8739:
									num3 = 124;
									goto IL_22F8;
								case 8745:
									num3 = 239;
									goto IL_22F8;
								default:
									if (num3 == 8758)
									{
										num3 = 58;
										goto IL_22F8;
									}
									if (num3 == 8764)
									{
										num3 = 126;
										goto IL_22F8;
									}
									break;
								}
							}
						}
						else if (num3 <= 8810)
						{
							if (num3 == 8776)
							{
								num3 = 247;
								goto IL_22F8;
							}
							switch (num3)
							{
							case 8801:
								num3 = 240;
								goto IL_22F8;
							case 8802:
							case 8803:
								break;
							case 8804:
								num3 = 243;
								goto IL_22F8;
							case 8805:
								num3 = 242;
								goto IL_22F8;
							default:
								if (num3 == 8810)
								{
									num3 = 174;
									goto IL_22F8;
								}
								break;
							}
						}
						else
						{
							if (num3 == 8811)
							{
								num3 = 175;
								goto IL_22F8;
							}
							if (num3 == 8901)
							{
								num3 = 250;
								goto IL_22F8;
							}
							if (num3 == 8962)
							{
								num3 = 127;
								goto IL_22F8;
							}
						}
					}
					else if (num3 <= 9632)
					{
						if (num3 <= 9488)
						{
							if (num3 <= 9001)
							{
								if (num3 <= 8976)
								{
									if (num3 == 8963)
									{
										num3 = 94;
										goto IL_22F8;
									}
									if (num3 == 8976)
									{
										num3 = 169;
										goto IL_22F8;
									}
								}
								else
								{
									if (num3 == 8992)
									{
										num3 = 244;
										goto IL_22F8;
									}
									if (num3 == 8993)
									{
										num3 = 245;
										goto IL_22F8;
									}
									if (num3 == 9001)
									{
										num3 = 60;
										goto IL_22F8;
									}
								}
							}
							else if (num3 <= 9472)
							{
								if (num3 == 9002)
								{
									num3 = 62;
									goto IL_22F8;
								}
								if (num3 == 9472)
								{
									num3 = 196;
									goto IL_22F8;
								}
							}
							else
							{
								if (num3 == 9474)
								{
									num3 = 179;
									goto IL_22F8;
								}
								if (num3 == 9484)
								{
									num3 = 218;
									goto IL_22F8;
								}
								if (num3 == 9488)
								{
									num3 = 191;
									goto IL_22F8;
								}
							}
						}
						else if (num3 <= 9516)
						{
							if (num3 <= 9496)
							{
								if (num3 == 9492)
								{
									num3 = 192;
									goto IL_22F8;
								}
								if (num3 == 9496)
								{
									num3 = 217;
									goto IL_22F8;
								}
							}
							else
							{
								if (num3 == 9500)
								{
									num3 = 195;
									goto IL_22F8;
								}
								if (num3 == 9508)
								{
									num3 = 180;
									goto IL_22F8;
								}
								if (num3 == 9516)
								{
									num3 = 194;
									goto IL_22F8;
								}
							}
						}
						else if (num3 <= 9604)
						{
							switch (num3)
							{
							case 9524:
								num3 = 193;
								goto IL_22F8;
							case 9525:
							case 9526:
							case 9527:
							case 9528:
							case 9529:
							case 9530:
							case 9531:
							case 9533:
							case 9534:
							case 9535:
							case 9536:
							case 9537:
							case 9538:
							case 9539:
							case 9540:
							case 9541:
							case 9542:
							case 9543:
							case 9544:
							case 9545:
							case 9546:
							case 9547:
							case 9548:
							case 9549:
							case 9550:
							case 9551:
								break;
							case 9532:
								num3 = 197;
								goto IL_22F8;
							case 9552:
								num3 = 205;
								goto IL_22F8;
							case 9553:
								num3 = 186;
								goto IL_22F8;
							case 9554:
								num3 = 213;
								goto IL_22F8;
							case 9555:
								num3 = 214;
								goto IL_22F8;
							case 9556:
								num3 = 201;
								goto IL_22F8;
							case 9557:
								num3 = 184;
								goto IL_22F8;
							case 9558:
								num3 = 183;
								goto IL_22F8;
							case 9559:
								num3 = 187;
								goto IL_22F8;
							case 9560:
								num3 = 212;
								goto IL_22F8;
							case 9561:
								num3 = 211;
								goto IL_22F8;
							case 9562:
								num3 = 200;
								goto IL_22F8;
							case 9563:
								num3 = 190;
								goto IL_22F8;
							case 9564:
								num3 = 189;
								goto IL_22F8;
							case 9565:
								num3 = 188;
								goto IL_22F8;
							case 9566:
								num3 = 198;
								goto IL_22F8;
							case 9567:
								num3 = 199;
								goto IL_22F8;
							case 9568:
								num3 = 204;
								goto IL_22F8;
							case 9569:
								num3 = 181;
								goto IL_22F8;
							case 9570:
								num3 = 182;
								goto IL_22F8;
							case 9571:
								num3 = 185;
								goto IL_22F8;
							case 9572:
								num3 = 209;
								goto IL_22F8;
							case 9573:
								num3 = 210;
								goto IL_22F8;
							case 9574:
								num3 = 203;
								goto IL_22F8;
							case 9575:
								num3 = 207;
								goto IL_22F8;
							case 9576:
								num3 = 208;
								goto IL_22F8;
							case 9577:
								num3 = 202;
								goto IL_22F8;
							case 9578:
								num3 = 216;
								goto IL_22F8;
							case 9579:
								num3 = 215;
								goto IL_22F8;
							case 9580:
								num3 = 206;
								goto IL_22F8;
							default:
								if (num3 == 9600)
								{
									num3 = 223;
									goto IL_22F8;
								}
								if (num3 == 9604)
								{
									num3 = 220;
									goto IL_22F8;
								}
								break;
							}
						}
						else
						{
							if (num3 == 9608)
							{
								num3 = 219;
								goto IL_22F8;
							}
							switch (num3)
							{
							case 9612:
								num3 = 221;
								goto IL_22F8;
							case 9613:
							case 9614:
							case 9615:
								break;
							case 9616:
								num3 = 222;
								goto IL_22F8;
							case 9617:
								num3 = 176;
								goto IL_22F8;
							case 9618:
								num3 = 177;
								goto IL_22F8;
							case 9619:
								num3 = 178;
								goto IL_22F8;
							default:
								if (num3 == 9632)
								{
									num3 = 254;
									goto IL_22F8;
								}
								break;
							}
						}
					}
					else if (num3 <= 9830)
					{
						if (num3 <= 9668)
						{
							if (num3 <= 9650)
							{
								if (num3 == 9644)
								{
									num3 = 22;
									goto IL_22F8;
								}
								if (num3 == 9650)
								{
									num3 = 30;
									goto IL_22F8;
								}
							}
							else
							{
								if (num3 == 9658)
								{
									num3 = 16;
									goto IL_22F8;
								}
								if (num3 == 9660)
								{
									num3 = 31;
									goto IL_22F8;
								}
								if (num3 == 9668)
								{
									num3 = 17;
									goto IL_22F8;
								}
							}
						}
						else if (num3 <= 9688)
						{
							if (num3 == 9675)
							{
								num3 = 9;
								goto IL_22F8;
							}
							if (num3 == 9688)
							{
								num3 = 8;
								goto IL_22F8;
							}
						}
						else
						{
							if (num3 == 9689)
							{
								num3 = 10;
								goto IL_22F8;
							}
							switch (num3)
							{
							case 9786:
								num3 = 1;
								goto IL_22F8;
							case 9787:
								num3 = 2;
								goto IL_22F8;
							case 9788:
								num3 = 15;
								goto IL_22F8;
							case 9789:
							case 9790:
							case 9791:
							case 9793:
								break;
							case 9792:
								num3 = 12;
								goto IL_22F8;
							case 9794:
								num3 = 11;
								goto IL_22F8;
							default:
								switch (num3)
								{
								case 9824:
									num3 = 6;
									goto IL_22F8;
								case 9827:
									num3 = 5;
									goto IL_22F8;
								case 9829:
									num3 = 3;
									goto IL_22F8;
								case 9830:
									num3 = 4;
									goto IL_22F8;
								}
								break;
							}
						}
					}
					else if (num3 <= 12288)
					{
						if (num3 <= 9835)
						{
							if (num3 == 9834)
							{
								num3 = 13;
								goto IL_22F8;
							}
							if (num3 == 9835)
							{
								num3 = 14;
								goto IL_22F8;
							}
						}
						else
						{
							if (num3 == 10003)
							{
								num3 = 251;
								goto IL_22F8;
							}
							if (num3 == 10072)
							{
								num3 = 124;
								goto IL_22F8;
							}
							if (num3 == 12288)
							{
								num3 = 32;
								goto IL_22F8;
							}
						}
					}
					else if (num3 <= 12315)
					{
						switch (num3)
						{
						case 12295:
							num3 = 9;
							goto IL_22F8;
						case 12296:
							num3 = 60;
							goto IL_22F8;
						case 12297:
							num3 = 62;
							goto IL_22F8;
						case 12298:
							num3 = 174;
							goto IL_22F8;
						case 12299:
							num3 = 175;
							goto IL_22F8;
						default:
							if (num3 == 12314)
							{
								num3 = 91;
								goto IL_22F8;
							}
							if (num3 == 12315)
							{
								num3 = 93;
								goto IL_22F8;
							}
							break;
						}
					}
					else
					{
						if (num3 == 12539)
						{
							num3 = 250;
							goto IL_22F8;
						}
						if (num3 - 65281 <= 29)
						{
							num3 -= 65248;
							goto IL_22F8;
						}
						if (num3 - 65312 <= 62)
						{
							num3 -= 65248;
							goto IL_22F8;
						}
					}
					base.HandleFallback(ref encoderFallbackBuffer, chars, ref num, ref charCount, bytes, ref num2, ref byteCount);
					num++;
					charCount--;
					continue;
				}
				IL_22F8:
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

		// Token: 0x04000033 RID: 51
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
			'x', 'y', 'z', '{', '|', '}', '~', '\u007f', 'Ç', 'ü',
			'é', 'â', 'ä', 'à', 'å', 'ç', 'ê', 'ë', 'è', 'ï',
			'î', 'ì', 'Ä', 'Å', 'É', 'æ', 'Æ', 'ô', 'ö', 'ò',
			'û', 'ù', 'ÿ', 'Ö', 'Ü', '¢', '£', '¥', '₧', 'ƒ',
			'á', 'í', 'ó', 'ú', 'ñ', 'Ñ', 'ª', 'º', '¿', '⌐',
			'¬', '½', '¼', '¡', '«', '»', '░', '▒', '▓', '│',
			'┤', '╡', '╢', '╖', '╕', '╣', '║', '╗', '╝', '╜',
			'╛', '┐', '└', '┴', '┬', '├', '─', '┼', '╞', '╟',
			'╚', '╔', '╩', '╦', '╠', '═', '╬', '╧', '╨', '╤',
			'╥', '╙', '╘', '╒', '╓', '╫', '╪', '┘', '┌', '█',
			'▄', '▌', '▐', '▀', 'α', 'ß', 'Γ', 'π', 'Σ', 'σ',
			'µ', 'τ', 'Φ', 'Θ', 'Ω', 'δ', '∞', 'φ', 'ε', '∩',
			'≡', '±', '≥', '≤', '⌠', '⌡', '÷', '≈', '°', '∙',
			'·', '√', 'ⁿ', '²', '■', '\u00a0'
		};
	}
}
