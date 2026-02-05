using System;

namespace Mono.Cecil
{
	// Token: 0x0200018D RID: 397
	internal enum TokenType : uint
	{
		// Token: 0x04000575 RID: 1397
		Module,
		// Token: 0x04000576 RID: 1398
		TypeRef = 16777216U,
		// Token: 0x04000577 RID: 1399
		TypeDef = 33554432U,
		// Token: 0x04000578 RID: 1400
		Field = 67108864U,
		// Token: 0x04000579 RID: 1401
		Method = 100663296U,
		// Token: 0x0400057A RID: 1402
		Param = 134217728U,
		// Token: 0x0400057B RID: 1403
		InterfaceImpl = 150994944U,
		// Token: 0x0400057C RID: 1404
		MemberRef = 167772160U,
		// Token: 0x0400057D RID: 1405
		CustomAttribute = 201326592U,
		// Token: 0x0400057E RID: 1406
		Permission = 234881024U,
		// Token: 0x0400057F RID: 1407
		Signature = 285212672U,
		// Token: 0x04000580 RID: 1408
		Event = 335544320U,
		// Token: 0x04000581 RID: 1409
		Property = 385875968U,
		// Token: 0x04000582 RID: 1410
		ModuleRef = 436207616U,
		// Token: 0x04000583 RID: 1411
		TypeSpec = 452984832U,
		// Token: 0x04000584 RID: 1412
		Assembly = 536870912U,
		// Token: 0x04000585 RID: 1413
		AssemblyRef = 587202560U,
		// Token: 0x04000586 RID: 1414
		File = 637534208U,
		// Token: 0x04000587 RID: 1415
		ExportedType = 654311424U,
		// Token: 0x04000588 RID: 1416
		ManifestResource = 671088640U,
		// Token: 0x04000589 RID: 1417
		GenericParam = 704643072U,
		// Token: 0x0400058A RID: 1418
		MethodSpec = 721420288U,
		// Token: 0x0400058B RID: 1419
		GenericParamConstraint = 738197504U,
		// Token: 0x0400058C RID: 1420
		Document = 805306368U,
		// Token: 0x0400058D RID: 1421
		MethodDebugInformation = 822083584U,
		// Token: 0x0400058E RID: 1422
		LocalScope = 838860800U,
		// Token: 0x0400058F RID: 1423
		LocalVariable = 855638016U,
		// Token: 0x04000590 RID: 1424
		LocalConstant = 872415232U,
		// Token: 0x04000591 RID: 1425
		ImportScope = 889192448U,
		// Token: 0x04000592 RID: 1426
		StateMachineMethod = 905969664U,
		// Token: 0x04000593 RID: 1427
		CustomDebugInformation = 922746880U,
		// Token: 0x04000594 RID: 1428
		String = 1879048192U
	}
}
