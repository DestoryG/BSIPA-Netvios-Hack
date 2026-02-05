using System;

namespace Mono.Cecil.Cil
{
	// Token: 0x02000105 RID: 261
	public enum StackBehaviour
	{
		// Token: 0x04000573 RID: 1395
		Pop0,
		// Token: 0x04000574 RID: 1396
		Pop1,
		// Token: 0x04000575 RID: 1397
		Pop1_pop1,
		// Token: 0x04000576 RID: 1398
		Popi,
		// Token: 0x04000577 RID: 1399
		Popi_pop1,
		// Token: 0x04000578 RID: 1400
		Popi_popi,
		// Token: 0x04000579 RID: 1401
		Popi_popi8,
		// Token: 0x0400057A RID: 1402
		Popi_popi_popi,
		// Token: 0x0400057B RID: 1403
		Popi_popr4,
		// Token: 0x0400057C RID: 1404
		Popi_popr8,
		// Token: 0x0400057D RID: 1405
		Popref,
		// Token: 0x0400057E RID: 1406
		Popref_pop1,
		// Token: 0x0400057F RID: 1407
		Popref_popi,
		// Token: 0x04000580 RID: 1408
		Popref_popi_popi,
		// Token: 0x04000581 RID: 1409
		Popref_popi_popi8,
		// Token: 0x04000582 RID: 1410
		Popref_popi_popr4,
		// Token: 0x04000583 RID: 1411
		Popref_popi_popr8,
		// Token: 0x04000584 RID: 1412
		Popref_popi_popref,
		// Token: 0x04000585 RID: 1413
		PopAll,
		// Token: 0x04000586 RID: 1414
		Push0,
		// Token: 0x04000587 RID: 1415
		Push1,
		// Token: 0x04000588 RID: 1416
		Push1_push1,
		// Token: 0x04000589 RID: 1417
		Pushi,
		// Token: 0x0400058A RID: 1418
		Pushi8,
		// Token: 0x0400058B RID: 1419
		Pushr4,
		// Token: 0x0400058C RID: 1420
		Pushr8,
		// Token: 0x0400058D RID: 1421
		Pushref,
		// Token: 0x0400058E RID: 1422
		Varpop,
		// Token: 0x0400058F RID: 1423
		Varpush
	}
}
