using System;

namespace Mono.Cecil.Metadata
{
	// Token: 0x020001A7 RID: 423
	internal enum ElementType : byte
	{
		// Token: 0x04000603 RID: 1539
		None,
		// Token: 0x04000604 RID: 1540
		Void,
		// Token: 0x04000605 RID: 1541
		Boolean,
		// Token: 0x04000606 RID: 1542
		Char,
		// Token: 0x04000607 RID: 1543
		I1,
		// Token: 0x04000608 RID: 1544
		U1,
		// Token: 0x04000609 RID: 1545
		I2,
		// Token: 0x0400060A RID: 1546
		U2,
		// Token: 0x0400060B RID: 1547
		I4,
		// Token: 0x0400060C RID: 1548
		U4,
		// Token: 0x0400060D RID: 1549
		I8,
		// Token: 0x0400060E RID: 1550
		U8,
		// Token: 0x0400060F RID: 1551
		R4,
		// Token: 0x04000610 RID: 1552
		R8,
		// Token: 0x04000611 RID: 1553
		String,
		// Token: 0x04000612 RID: 1554
		Ptr,
		// Token: 0x04000613 RID: 1555
		ByRef,
		// Token: 0x04000614 RID: 1556
		ValueType,
		// Token: 0x04000615 RID: 1557
		Class,
		// Token: 0x04000616 RID: 1558
		Var,
		// Token: 0x04000617 RID: 1559
		Array,
		// Token: 0x04000618 RID: 1560
		GenericInst,
		// Token: 0x04000619 RID: 1561
		TypedByRef,
		// Token: 0x0400061A RID: 1562
		I = 24,
		// Token: 0x0400061B RID: 1563
		U,
		// Token: 0x0400061C RID: 1564
		FnPtr = 27,
		// Token: 0x0400061D RID: 1565
		Object,
		// Token: 0x0400061E RID: 1566
		SzArray,
		// Token: 0x0400061F RID: 1567
		MVar,
		// Token: 0x04000620 RID: 1568
		CModReqD,
		// Token: 0x04000621 RID: 1569
		CModOpt,
		// Token: 0x04000622 RID: 1570
		Internal,
		// Token: 0x04000623 RID: 1571
		Modifier = 64,
		// Token: 0x04000624 RID: 1572
		Sentinel,
		// Token: 0x04000625 RID: 1573
		Pinned = 69,
		// Token: 0x04000626 RID: 1574
		Type = 80,
		// Token: 0x04000627 RID: 1575
		Boxed,
		// Token: 0x04000628 RID: 1576
		Enum = 85
	}
}
