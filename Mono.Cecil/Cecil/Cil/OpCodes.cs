using System;

namespace Mono.Cecil.Cil
{
	// Token: 0x02000108 RID: 264
	public static class OpCodes
	{
		// Token: 0x04000599 RID: 1433
		internal static readonly OpCode[] OneByteOpCode = new OpCode[225];

		// Token: 0x0400059A RID: 1434
		internal static readonly OpCode[] TwoBytesOpCode = new OpCode[31];

		// Token: 0x0400059B RID: 1435
		public static readonly OpCode Nop = new OpCode(83886335, 318768389);

		// Token: 0x0400059C RID: 1436
		public static readonly OpCode Break = new OpCode(16843263, 318768389);

		// Token: 0x0400059D RID: 1437
		public static readonly OpCode Ldarg_0 = new OpCode(84017919, 335545601);

		// Token: 0x0400059E RID: 1438
		public static readonly OpCode Ldarg_1 = new OpCode(84083711, 335545601);

		// Token: 0x0400059F RID: 1439
		public static readonly OpCode Ldarg_2 = new OpCode(84149503, 335545601);

		// Token: 0x040005A0 RID: 1440
		public static readonly OpCode Ldarg_3 = new OpCode(84215295, 335545601);

		// Token: 0x040005A1 RID: 1441
		public static readonly OpCode Ldloc_0 = new OpCode(84281087, 335545601);

		// Token: 0x040005A2 RID: 1442
		public static readonly OpCode Ldloc_1 = new OpCode(84346879, 335545601);

		// Token: 0x040005A3 RID: 1443
		public static readonly OpCode Ldloc_2 = new OpCode(84412671, 335545601);

		// Token: 0x040005A4 RID: 1444
		public static readonly OpCode Ldloc_3 = new OpCode(84478463, 335545601);

		// Token: 0x040005A5 RID: 1445
		public static readonly OpCode Stloc_0 = new OpCode(84544255, 318833921);

		// Token: 0x040005A6 RID: 1446
		public static readonly OpCode Stloc_1 = new OpCode(84610047, 318833921);

		// Token: 0x040005A7 RID: 1447
		public static readonly OpCode Stloc_2 = new OpCode(84675839, 318833921);

		// Token: 0x040005A8 RID: 1448
		public static readonly OpCode Stloc_3 = new OpCode(84741631, 318833921);

		// Token: 0x040005A9 RID: 1449
		public static readonly OpCode Ldarg_S = new OpCode(84807423, 335549185);

		// Token: 0x040005AA RID: 1450
		public static readonly OpCode Ldarga_S = new OpCode(84873215, 369103617);

		// Token: 0x040005AB RID: 1451
		public static readonly OpCode Starg_S = new OpCode(84939007, 318837505);

		// Token: 0x040005AC RID: 1452
		public static readonly OpCode Ldloc_S = new OpCode(85004799, 335548929);

		// Token: 0x040005AD RID: 1453
		public static readonly OpCode Ldloca_S = new OpCode(85070591, 369103361);

		// Token: 0x040005AE RID: 1454
		public static readonly OpCode Stloc_S = new OpCode(85136383, 318837249);

		// Token: 0x040005AF RID: 1455
		public static readonly OpCode Ldnull = new OpCode(85202175, 436208901);

		// Token: 0x040005B0 RID: 1456
		public static readonly OpCode Ldc_I4_M1 = new OpCode(85267967, 369100033);

		// Token: 0x040005B1 RID: 1457
		public static readonly OpCode Ldc_I4_0 = new OpCode(85333759, 369100033);

		// Token: 0x040005B2 RID: 1458
		public static readonly OpCode Ldc_I4_1 = new OpCode(85399551, 369100033);

		// Token: 0x040005B3 RID: 1459
		public static readonly OpCode Ldc_I4_2 = new OpCode(85465343, 369100033);

		// Token: 0x040005B4 RID: 1460
		public static readonly OpCode Ldc_I4_3 = new OpCode(85531135, 369100033);

		// Token: 0x040005B5 RID: 1461
		public static readonly OpCode Ldc_I4_4 = new OpCode(85596927, 369100033);

		// Token: 0x040005B6 RID: 1462
		public static readonly OpCode Ldc_I4_5 = new OpCode(85662719, 369100033);

		// Token: 0x040005B7 RID: 1463
		public static readonly OpCode Ldc_I4_6 = new OpCode(85728511, 369100033);

		// Token: 0x040005B8 RID: 1464
		public static readonly OpCode Ldc_I4_7 = new OpCode(85794303, 369100033);

		// Token: 0x040005B9 RID: 1465
		public static readonly OpCode Ldc_I4_8 = new OpCode(85860095, 369100033);

		// Token: 0x040005BA RID: 1466
		public static readonly OpCode Ldc_I4_S = new OpCode(85925887, 369102849);

		// Token: 0x040005BB RID: 1467
		public static readonly OpCode Ldc_I4 = new OpCode(85991679, 369099269);

		// Token: 0x040005BC RID: 1468
		public static readonly OpCode Ldc_I8 = new OpCode(86057471, 385876741);

		// Token: 0x040005BD RID: 1469
		public static readonly OpCode Ldc_R4 = new OpCode(86123263, 402657541);

		// Token: 0x040005BE RID: 1470
		public static readonly OpCode Ldc_R8 = new OpCode(86189055, 419432197);

		// Token: 0x040005BF RID: 1471
		public static readonly OpCode Dup = new OpCode(86255103, 352388357);

		// Token: 0x040005C0 RID: 1472
		public static readonly OpCode Pop = new OpCode(86320895, 318833925);

		// Token: 0x040005C1 RID: 1473
		public static readonly OpCode Jmp = new OpCode(36055039, 318768133);

		// Token: 0x040005C2 RID: 1474
		public static readonly OpCode Call = new OpCode(36120831, 471532549);

		// Token: 0x040005C3 RID: 1475
		public static readonly OpCode Calli = new OpCode(36186623, 471533573);

		// Token: 0x040005C4 RID: 1476
		public static readonly OpCode Ret = new OpCode(120138495, 320537861);

		// Token: 0x040005C5 RID: 1477
		public static readonly OpCode Br_S = new OpCode(2763775, 318770945);

		// Token: 0x040005C6 RID: 1478
		public static readonly OpCode Brfalse_S = new OpCode(53161215, 318967553);

		// Token: 0x040005C7 RID: 1479
		public static readonly OpCode Brtrue_S = new OpCode(53227007, 318967553);

		// Token: 0x040005C8 RID: 1480
		public static readonly OpCode Beq_S = new OpCode(53292799, 318902017);

		// Token: 0x040005C9 RID: 1481
		public static readonly OpCode Bge_S = new OpCode(53358591, 318902017);

		// Token: 0x040005CA RID: 1482
		public static readonly OpCode Bgt_S = new OpCode(53424383, 318902017);

		// Token: 0x040005CB RID: 1483
		public static readonly OpCode Ble_S = new OpCode(53490175, 318902017);

		// Token: 0x040005CC RID: 1484
		public static readonly OpCode Blt_S = new OpCode(53555967, 318902017);

		// Token: 0x040005CD RID: 1485
		public static readonly OpCode Bne_Un_S = new OpCode(53621759, 318902017);

		// Token: 0x040005CE RID: 1486
		public static readonly OpCode Bge_Un_S = new OpCode(53687551, 318902017);

		// Token: 0x040005CF RID: 1487
		public static readonly OpCode Bgt_Un_S = new OpCode(53753343, 318902017);

		// Token: 0x040005D0 RID: 1488
		public static readonly OpCode Ble_Un_S = new OpCode(53819135, 318902017);

		// Token: 0x040005D1 RID: 1489
		public static readonly OpCode Blt_Un_S = new OpCode(53884927, 318902017);

		// Token: 0x040005D2 RID: 1490
		public static readonly OpCode Br = new OpCode(3619071, 318767109);

		// Token: 0x040005D3 RID: 1491
		public static readonly OpCode Brfalse = new OpCode(54016511, 318963717);

		// Token: 0x040005D4 RID: 1492
		public static readonly OpCode Brtrue = new OpCode(54082303, 318963717);

		// Token: 0x040005D5 RID: 1493
		public static readonly OpCode Beq = new OpCode(54148095, 318898177);

		// Token: 0x040005D6 RID: 1494
		public static readonly OpCode Bge = new OpCode(54213887, 318898177);

		// Token: 0x040005D7 RID: 1495
		public static readonly OpCode Bgt = new OpCode(54279679, 318898177);

		// Token: 0x040005D8 RID: 1496
		public static readonly OpCode Ble = new OpCode(54345471, 318898177);

		// Token: 0x040005D9 RID: 1497
		public static readonly OpCode Blt = new OpCode(54411263, 318898177);

		// Token: 0x040005DA RID: 1498
		public static readonly OpCode Bne_Un = new OpCode(54477055, 318898177);

		// Token: 0x040005DB RID: 1499
		public static readonly OpCode Bge_Un = new OpCode(54542847, 318898177);

		// Token: 0x040005DC RID: 1500
		public static readonly OpCode Bgt_Un = new OpCode(54608639, 318898177);

		// Token: 0x040005DD RID: 1501
		public static readonly OpCode Ble_Un = new OpCode(54674431, 318898177);

		// Token: 0x040005DE RID: 1502
		public static readonly OpCode Blt_Un = new OpCode(54740223, 318898177);

		// Token: 0x040005DF RID: 1503
		public static readonly OpCode Switch = new OpCode(54806015, 318966277);

		// Token: 0x040005E0 RID: 1504
		public static readonly OpCode Ldind_I1 = new OpCode(88426239, 369296645);

		// Token: 0x040005E1 RID: 1505
		public static readonly OpCode Ldind_U1 = new OpCode(88492031, 369296645);

		// Token: 0x040005E2 RID: 1506
		public static readonly OpCode Ldind_I2 = new OpCode(88557823, 369296645);

		// Token: 0x040005E3 RID: 1507
		public static readonly OpCode Ldind_U2 = new OpCode(88623615, 369296645);

		// Token: 0x040005E4 RID: 1508
		public static readonly OpCode Ldind_I4 = new OpCode(88689407, 369296645);

		// Token: 0x040005E5 RID: 1509
		public static readonly OpCode Ldind_U4 = new OpCode(88755199, 369296645);

		// Token: 0x040005E6 RID: 1510
		public static readonly OpCode Ldind_I8 = new OpCode(88820991, 386073861);

		// Token: 0x040005E7 RID: 1511
		public static readonly OpCode Ldind_I = new OpCode(88886783, 369296645);

		// Token: 0x040005E8 RID: 1512
		public static readonly OpCode Ldind_R4 = new OpCode(88952575, 402851077);

		// Token: 0x040005E9 RID: 1513
		public static readonly OpCode Ldind_R8 = new OpCode(89018367, 419628293);

		// Token: 0x040005EA RID: 1514
		public static readonly OpCode Ldind_Ref = new OpCode(89084159, 436405509);

		// Token: 0x040005EB RID: 1515
		public static readonly OpCode Stind_Ref = new OpCode(89149951, 319096069);

		// Token: 0x040005EC RID: 1516
		public static readonly OpCode Stind_I1 = new OpCode(89215743, 319096069);

		// Token: 0x040005ED RID: 1517
		public static readonly OpCode Stind_I2 = new OpCode(89281535, 319096069);

		// Token: 0x040005EE RID: 1518
		public static readonly OpCode Stind_I4 = new OpCode(89347327, 319096069);

		// Token: 0x040005EF RID: 1519
		public static readonly OpCode Stind_I8 = new OpCode(89413119, 319161605);

		// Token: 0x040005F0 RID: 1520
		public static readonly OpCode Stind_R4 = new OpCode(89478911, 319292677);

		// Token: 0x040005F1 RID: 1521
		public static readonly OpCode Stind_R8 = new OpCode(89544703, 319358213);

		// Token: 0x040005F2 RID: 1522
		public static readonly OpCode Add = new OpCode(89610495, 335676677);

		// Token: 0x040005F3 RID: 1523
		public static readonly OpCode Sub = new OpCode(89676287, 335676677);

		// Token: 0x040005F4 RID: 1524
		public static readonly OpCode Mul = new OpCode(89742079, 335676677);

		// Token: 0x040005F5 RID: 1525
		public static readonly OpCode Div = new OpCode(89807871, 335676677);

		// Token: 0x040005F6 RID: 1526
		public static readonly OpCode Div_Un = new OpCode(89873663, 335676677);

		// Token: 0x040005F7 RID: 1527
		public static readonly OpCode Rem = new OpCode(89939455, 335676677);

		// Token: 0x040005F8 RID: 1528
		public static readonly OpCode Rem_Un = new OpCode(90005247, 335676677);

		// Token: 0x040005F9 RID: 1529
		public static readonly OpCode And = new OpCode(90071039, 335676677);

		// Token: 0x040005FA RID: 1530
		public static readonly OpCode Or = new OpCode(90136831, 335676677);

		// Token: 0x040005FB RID: 1531
		public static readonly OpCode Xor = new OpCode(90202623, 335676677);

		// Token: 0x040005FC RID: 1532
		public static readonly OpCode Shl = new OpCode(90268415, 335676677);

		// Token: 0x040005FD RID: 1533
		public static readonly OpCode Shr = new OpCode(90334207, 335676677);

		// Token: 0x040005FE RID: 1534
		public static readonly OpCode Shr_Un = new OpCode(90399999, 335676677);

		// Token: 0x040005FF RID: 1535
		public static readonly OpCode Neg = new OpCode(90465791, 335611141);

		// Token: 0x04000600 RID: 1536
		public static readonly OpCode Not = new OpCode(90531583, 335611141);

		// Token: 0x04000601 RID: 1537
		public static readonly OpCode Conv_I1 = new OpCode(90597375, 369165573);

		// Token: 0x04000602 RID: 1538
		public static readonly OpCode Conv_I2 = new OpCode(90663167, 369165573);

		// Token: 0x04000603 RID: 1539
		public static readonly OpCode Conv_I4 = new OpCode(90728959, 369165573);

		// Token: 0x04000604 RID: 1540
		public static readonly OpCode Conv_I8 = new OpCode(90794751, 385942789);

		// Token: 0x04000605 RID: 1541
		public static readonly OpCode Conv_R4 = new OpCode(90860543, 402720005);

		// Token: 0x04000606 RID: 1542
		public static readonly OpCode Conv_R8 = new OpCode(90926335, 419497221);

		// Token: 0x04000607 RID: 1543
		public static readonly OpCode Conv_U4 = new OpCode(90992127, 369165573);

		// Token: 0x04000608 RID: 1544
		public static readonly OpCode Conv_U8 = new OpCode(91057919, 385942789);

		// Token: 0x04000609 RID: 1545
		public static readonly OpCode Callvirt = new OpCode(40792063, 471532547);

		// Token: 0x0400060A RID: 1546
		public static readonly OpCode Cpobj = new OpCode(91189503, 319097859);

		// Token: 0x0400060B RID: 1547
		public static readonly OpCode Ldobj = new OpCode(91255295, 335744003);

		// Token: 0x0400060C RID: 1548
		public static readonly OpCode Ldstr = new OpCode(91321087, 436209923);

		// Token: 0x0400060D RID: 1549
		public static readonly OpCode Newobj = new OpCode(41055231, 437978115);

		// Token: 0x0400060E RID: 1550
		public static readonly OpCode Castclass = new OpCode(91452671, 436866051);

		// Token: 0x0400060F RID: 1551
		public static readonly OpCode Isinst = new OpCode(91518463, 369757187);

		// Token: 0x04000610 RID: 1552
		public static readonly OpCode Conv_R_Un = new OpCode(91584255, 419497221);

		// Token: 0x04000611 RID: 1553
		public static readonly OpCode Unbox = new OpCode(91650559, 369757189);

		// Token: 0x04000612 RID: 1554
		public static readonly OpCode Throw = new OpCode(142047999, 319423747);

		// Token: 0x04000613 RID: 1555
		public static readonly OpCode Ldfld = new OpCode(91782143, 336199939);

		// Token: 0x04000614 RID: 1556
		public static readonly OpCode Ldflda = new OpCode(91847935, 369754371);

		// Token: 0x04000615 RID: 1557
		public static readonly OpCode Stfld = new OpCode(91913727, 319488259);

		// Token: 0x04000616 RID: 1558
		public static readonly OpCode Ldsfld = new OpCode(91979519, 335544579);

		// Token: 0x04000617 RID: 1559
		public static readonly OpCode Ldsflda = new OpCode(92045311, 369099011);

		// Token: 0x04000618 RID: 1560
		public static readonly OpCode Stsfld = new OpCode(92111103, 318832899);

		// Token: 0x04000619 RID: 1561
		public static readonly OpCode Stobj = new OpCode(92176895, 319032323);

		// Token: 0x0400061A RID: 1562
		public static readonly OpCode Conv_Ovf_I1_Un = new OpCode(92242687, 369165573);

		// Token: 0x0400061B RID: 1563
		public static readonly OpCode Conv_Ovf_I2_Un = new OpCode(92308479, 369165573);

		// Token: 0x0400061C RID: 1564
		public static readonly OpCode Conv_Ovf_I4_Un = new OpCode(92374271, 369165573);

		// Token: 0x0400061D RID: 1565
		public static readonly OpCode Conv_Ovf_I8_Un = new OpCode(92440063, 385942789);

		// Token: 0x0400061E RID: 1566
		public static readonly OpCode Conv_Ovf_U1_Un = new OpCode(92505855, 369165573);

		// Token: 0x0400061F RID: 1567
		public static readonly OpCode Conv_Ovf_U2_Un = new OpCode(92571647, 369165573);

		// Token: 0x04000620 RID: 1568
		public static readonly OpCode Conv_Ovf_U4_Un = new OpCode(92637439, 369165573);

		// Token: 0x04000621 RID: 1569
		public static readonly OpCode Conv_Ovf_U8_Un = new OpCode(92703231, 385942789);

		// Token: 0x04000622 RID: 1570
		public static readonly OpCode Conv_Ovf_I_Un = new OpCode(92769023, 369165573);

		// Token: 0x04000623 RID: 1571
		public static readonly OpCode Conv_Ovf_U_Un = new OpCode(92834815, 369165573);

		// Token: 0x04000624 RID: 1572
		public static readonly OpCode Box = new OpCode(92900607, 436276229);

		// Token: 0x04000625 RID: 1573
		public static readonly OpCode Newarr = new OpCode(92966399, 436407299);

		// Token: 0x04000626 RID: 1574
		public static readonly OpCode Ldlen = new OpCode(93032191, 369755395);

		// Token: 0x04000627 RID: 1575
		public static readonly OpCode Ldelema = new OpCode(93097983, 369888259);

		// Token: 0x04000628 RID: 1576
		public static readonly OpCode Ldelem_I1 = new OpCode(93163775, 369886467);

		// Token: 0x04000629 RID: 1577
		public static readonly OpCode Ldelem_U1 = new OpCode(93229567, 369886467);

		// Token: 0x0400062A RID: 1578
		public static readonly OpCode Ldelem_I2 = new OpCode(93295359, 369886467);

		// Token: 0x0400062B RID: 1579
		public static readonly OpCode Ldelem_U2 = new OpCode(93361151, 369886467);

		// Token: 0x0400062C RID: 1580
		public static readonly OpCode Ldelem_I4 = new OpCode(93426943, 369886467);

		// Token: 0x0400062D RID: 1581
		public static readonly OpCode Ldelem_U4 = new OpCode(93492735, 369886467);

		// Token: 0x0400062E RID: 1582
		public static readonly OpCode Ldelem_I8 = new OpCode(93558527, 386663683);

		// Token: 0x0400062F RID: 1583
		public static readonly OpCode Ldelem_I = new OpCode(93624319, 369886467);

		// Token: 0x04000630 RID: 1584
		public static readonly OpCode Ldelem_R4 = new OpCode(93690111, 403440899);

		// Token: 0x04000631 RID: 1585
		public static readonly OpCode Ldelem_R8 = new OpCode(93755903, 420218115);

		// Token: 0x04000632 RID: 1586
		public static readonly OpCode Ldelem_Ref = new OpCode(93821695, 436995331);

		// Token: 0x04000633 RID: 1587
		public static readonly OpCode Stelem_I = new OpCode(93887487, 319620355);

		// Token: 0x04000634 RID: 1588
		public static readonly OpCode Stelem_I1 = new OpCode(93953279, 319620355);

		// Token: 0x04000635 RID: 1589
		public static readonly OpCode Stelem_I2 = new OpCode(94019071, 319620355);

		// Token: 0x04000636 RID: 1590
		public static readonly OpCode Stelem_I4 = new OpCode(94084863, 319620355);

		// Token: 0x04000637 RID: 1591
		public static readonly OpCode Stelem_I8 = new OpCode(94150655, 319685891);

		// Token: 0x04000638 RID: 1592
		public static readonly OpCode Stelem_R4 = new OpCode(94216447, 319751427);

		// Token: 0x04000639 RID: 1593
		public static readonly OpCode Stelem_R8 = new OpCode(94282239, 319816963);

		// Token: 0x0400063A RID: 1594
		public static readonly OpCode Stelem_Ref = new OpCode(94348031, 319882499);

		// Token: 0x0400063B RID: 1595
		public static readonly OpCode Ldelem_Any = new OpCode(94413823, 336333827);

		// Token: 0x0400063C RID: 1596
		public static readonly OpCode Stelem_Any = new OpCode(94479615, 319884291);

		// Token: 0x0400063D RID: 1597
		public static readonly OpCode Unbox_Any = new OpCode(94545407, 336202755);

		// Token: 0x0400063E RID: 1598
		public static readonly OpCode Conv_Ovf_I1 = new OpCode(94614527, 369165573);

		// Token: 0x0400063F RID: 1599
		public static readonly OpCode Conv_Ovf_U1 = new OpCode(94680319, 369165573);

		// Token: 0x04000640 RID: 1600
		public static readonly OpCode Conv_Ovf_I2 = new OpCode(94746111, 369165573);

		// Token: 0x04000641 RID: 1601
		public static readonly OpCode Conv_Ovf_U2 = new OpCode(94811903, 369165573);

		// Token: 0x04000642 RID: 1602
		public static readonly OpCode Conv_Ovf_I4 = new OpCode(94877695, 369165573);

		// Token: 0x04000643 RID: 1603
		public static readonly OpCode Conv_Ovf_U4 = new OpCode(94943487, 369165573);

		// Token: 0x04000644 RID: 1604
		public static readonly OpCode Conv_Ovf_I8 = new OpCode(95009279, 385942789);

		// Token: 0x04000645 RID: 1605
		public static readonly OpCode Conv_Ovf_U8 = new OpCode(95075071, 385942789);

		// Token: 0x04000646 RID: 1606
		public static readonly OpCode Refanyval = new OpCode(95142655, 369167365);

		// Token: 0x04000647 RID: 1607
		public static readonly OpCode Ckfinite = new OpCode(95208447, 419497221);

		// Token: 0x04000648 RID: 1608
		public static readonly OpCode Mkrefany = new OpCode(95274751, 335744005);

		// Token: 0x04000649 RID: 1609
		public static readonly OpCode Ldtoken = new OpCode(95342847, 369101573);

		// Token: 0x0400064A RID: 1610
		public static readonly OpCode Conv_U2 = new OpCode(95408639, 369165573);

		// Token: 0x0400064B RID: 1611
		public static readonly OpCode Conv_U1 = new OpCode(95474431, 369165573);

		// Token: 0x0400064C RID: 1612
		public static readonly OpCode Conv_I = new OpCode(95540223, 369165573);

		// Token: 0x0400064D RID: 1613
		public static readonly OpCode Conv_Ovf_I = new OpCode(95606015, 369165573);

		// Token: 0x0400064E RID: 1614
		public static readonly OpCode Conv_Ovf_U = new OpCode(95671807, 369165573);

		// Token: 0x0400064F RID: 1615
		public static readonly OpCode Add_Ovf = new OpCode(95737599, 335676677);

		// Token: 0x04000650 RID: 1616
		public static readonly OpCode Add_Ovf_Un = new OpCode(95803391, 335676677);

		// Token: 0x04000651 RID: 1617
		public static readonly OpCode Mul_Ovf = new OpCode(95869183, 335676677);

		// Token: 0x04000652 RID: 1618
		public static readonly OpCode Mul_Ovf_Un = new OpCode(95934975, 335676677);

		// Token: 0x04000653 RID: 1619
		public static readonly OpCode Sub_Ovf = new OpCode(96000767, 335676677);

		// Token: 0x04000654 RID: 1620
		public static readonly OpCode Sub_Ovf_Un = new OpCode(96066559, 335676677);

		// Token: 0x04000655 RID: 1621
		public static readonly OpCode Endfinally = new OpCode(129686783, 318768389);

		// Token: 0x04000656 RID: 1622
		public static readonly OpCode Leave = new OpCode(12312063, 319946757);

		// Token: 0x04000657 RID: 1623
		public static readonly OpCode Leave_S = new OpCode(12377855, 319950593);

		// Token: 0x04000658 RID: 1624
		public static readonly OpCode Stind_I = new OpCode(96329727, 319096069);

		// Token: 0x04000659 RID: 1625
		public static readonly OpCode Conv_U = new OpCode(96395519, 369165573);

		// Token: 0x0400065A RID: 1626
		public static readonly OpCode Arglist = new OpCode(96403710, 369100037);

		// Token: 0x0400065B RID: 1627
		public static readonly OpCode Ceq = new OpCode(96469502, 369231109);

		// Token: 0x0400065C RID: 1628
		public static readonly OpCode Cgt = new OpCode(96535294, 369231109);

		// Token: 0x0400065D RID: 1629
		public static readonly OpCode Cgt_Un = new OpCode(96601086, 369231109);

		// Token: 0x0400065E RID: 1630
		public static readonly OpCode Clt = new OpCode(96666878, 369231109);

		// Token: 0x0400065F RID: 1631
		public static readonly OpCode Clt_Un = new OpCode(96732670, 369231109);

		// Token: 0x04000660 RID: 1632
		public static readonly OpCode Ldftn = new OpCode(96798462, 369099781);

		// Token: 0x04000661 RID: 1633
		public static readonly OpCode Ldvirtftn = new OpCode(96864254, 369755141);

		// Token: 0x04000662 RID: 1634
		public static readonly OpCode Ldarg = new OpCode(96930302, 335547909);

		// Token: 0x04000663 RID: 1635
		public static readonly OpCode Ldarga = new OpCode(96996094, 369102341);

		// Token: 0x04000664 RID: 1636
		public static readonly OpCode Starg = new OpCode(97061886, 318836229);

		// Token: 0x04000665 RID: 1637
		public static readonly OpCode Ldloc = new OpCode(97127678, 335547653);

		// Token: 0x04000666 RID: 1638
		public static readonly OpCode Ldloca = new OpCode(97193470, 369102085);

		// Token: 0x04000667 RID: 1639
		public static readonly OpCode Stloc = new OpCode(97259262, 318835973);

		// Token: 0x04000668 RID: 1640
		public static readonly OpCode Localloc = new OpCode(97325054, 369296645);

		// Token: 0x04000669 RID: 1641
		public static readonly OpCode Endfilter = new OpCode(130945534, 318964997);

		// Token: 0x0400066A RID: 1642
		public static readonly OpCode Unaligned = new OpCode(80679678, 318771204);

		// Token: 0x0400066B RID: 1643
		public static readonly OpCode Volatile = new OpCode(80745470, 318768388);

		// Token: 0x0400066C RID: 1644
		public static readonly OpCode Tail = new OpCode(80811262, 318768388);

		// Token: 0x0400066D RID: 1645
		public static readonly OpCode Initobj = new OpCode(97654270, 318966787);

		// Token: 0x0400066E RID: 1646
		public static readonly OpCode Constrained = new OpCode(97720062, 318770180);

		// Token: 0x0400066F RID: 1647
		public static readonly OpCode Cpblk = new OpCode(97785854, 319227141);

		// Token: 0x04000670 RID: 1648
		public static readonly OpCode Initblk = new OpCode(97851646, 319227141);

		// Token: 0x04000671 RID: 1649
		public static readonly OpCode No = new OpCode(97917438, 318771204);

		// Token: 0x04000672 RID: 1650
		public static readonly OpCode Rethrow = new OpCode(148314878, 318768387);

		// Token: 0x04000673 RID: 1651
		public static readonly OpCode Sizeof = new OpCode(98049278, 369101829);

		// Token: 0x04000674 RID: 1652
		public static readonly OpCode Refanytype = new OpCode(98115070, 369165573);

		// Token: 0x04000675 RID: 1653
		public static readonly OpCode Readonly = new OpCode(98180862, 318768388);
	}
}
