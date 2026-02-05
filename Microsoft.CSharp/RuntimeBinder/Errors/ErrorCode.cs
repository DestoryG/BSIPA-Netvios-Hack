using System;

namespace Microsoft.CSharp.RuntimeBinder.Errors
{
	// Token: 0x020000BF RID: 191
	internal enum ErrorCode
	{
		// Token: 0x040005B7 RID: 1463
		ERR_BadBinaryOps = 19,
		// Token: 0x040005B8 RID: 1464
		ERR_BadIndexLHS = 21,
		// Token: 0x040005B9 RID: 1465
		ERR_BadIndexCount,
		// Token: 0x040005BA RID: 1466
		ERR_BadUnaryOp,
		// Token: 0x040005BB RID: 1467
		ERR_NoImplicitConv = 29,
		// Token: 0x040005BC RID: 1468
		ERR_NoExplicitConv,
		// Token: 0x040005BD RID: 1469
		ERR_ConstOutOfRange,
		// Token: 0x040005BE RID: 1470
		ERR_AmbigBinaryOps = 34,
		// Token: 0x040005BF RID: 1471
		ERR_AmbigUnaryOp,
		// Token: 0x040005C0 RID: 1472
		ERR_ValueCantBeNull = 37,
		// Token: 0x040005C1 RID: 1473
		ERR_WrongNestedThis,
		// Token: 0x040005C2 RID: 1474
		ERR_NoSuchMember = 117,
		// Token: 0x040005C3 RID: 1475
		ERR_ObjectRequired = 120,
		// Token: 0x040005C4 RID: 1476
		ERR_AmbigCall,
		// Token: 0x040005C5 RID: 1477
		ERR_BadAccess,
		// Token: 0x040005C6 RID: 1478
		ERR_MethDelegateMismatch,
		// Token: 0x040005C7 RID: 1479
		ERR_AssgLvalueExpected = 131,
		// Token: 0x040005C8 RID: 1480
		ERR_NoConstructors = 143,
		// Token: 0x040005C9 RID: 1481
		ERR_PropertyLacksGet = 154,
		// Token: 0x040005CA RID: 1482
		ERR_ObjectProhibited = 176,
		// Token: 0x040005CB RID: 1483
		ERR_AssgReadonly = 191,
		// Token: 0x040005CC RID: 1484
		ERR_RefReadonly,
		// Token: 0x040005CD RID: 1485
		ERR_AssgReadonlyStatic = 198,
		// Token: 0x040005CE RID: 1486
		ERR_RefReadonlyStatic,
		// Token: 0x040005CF RID: 1487
		ERR_AssgReadonlyProp,
		// Token: 0x040005D0 RID: 1488
		ERR_RefProperty = 206,
		// Token: 0x040005D1 RID: 1489
		ERR_UnsafeNeeded = 214,
		// Token: 0x040005D2 RID: 1490
		ERR_BadBoolOp = 217,
		// Token: 0x040005D3 RID: 1491
		ERR_MustHaveOpTF,
		// Token: 0x040005D4 RID: 1492
		ERR_ConstOutOfRangeChecked = 221,
		// Token: 0x040005D5 RID: 1493
		ERR_AmbigMember = 229,
		// Token: 0x040005D6 RID: 1494
		ERR_NoImplicitConvCast = 266,
		// Token: 0x040005D7 RID: 1495
		ERR_InaccessibleGetter = 271,
		// Token: 0x040005D8 RID: 1496
		ERR_InaccessibleSetter,
		// Token: 0x040005D9 RID: 1497
		ERR_BadArity = 305,
		// Token: 0x040005DA RID: 1498
		ERR_TypeArgsNotAllowed = 307,
		// Token: 0x040005DB RID: 1499
		ERR_HasNoTypeVars,
		// Token: 0x040005DC RID: 1500
		ERR_NewConstraintNotSatisfied = 310,
		// Token: 0x040005DD RID: 1501
		ERR_GenericConstraintNotSatisfiedRefType,
		// Token: 0x040005DE RID: 1502
		ERR_GenericConstraintNotSatisfiedNullableEnum,
		// Token: 0x040005DF RID: 1503
		ERR_GenericConstraintNotSatisfiedNullableInterface,
		// Token: 0x040005E0 RID: 1504
		ERR_GenericConstraintNotSatisfiedValType = 315,
		// Token: 0x040005E1 RID: 1505
		ERR_CantInferMethTypeArgs = 411,
		// Token: 0x040005E2 RID: 1506
		ERR_RefConstraintNotSatisfied = 452,
		// Token: 0x040005E3 RID: 1507
		ERR_ValConstraintNotSatisfied,
		// Token: 0x040005E4 RID: 1508
		ERR_AmbigUDConv = 457,
		// Token: 0x040005E5 RID: 1509
		ERR_BindToBogus = 570,
		// Token: 0x040005E6 RID: 1510
		ERR_CantCallSpecialMethod,
		// Token: 0x040005E7 RID: 1511
		ERR_ConvertToStaticClass = 716,
		// Token: 0x040005E8 RID: 1512
		ERR_IncrementLvalueExpected = 1059,
		// Token: 0x040005E9 RID: 1513
		ERR_BadArgCount = 1501,
		// Token: 0x040005EA RID: 1514
		ERR_BadArgTypes,
		// Token: 0x040005EB RID: 1515
		ERR_RefLvalueExpected = 1510,
		// Token: 0x040005EC RID: 1516
		ERR_BadProtectedAccess = 1540,
		// Token: 0x040005ED RID: 1517
		ERR_BindToBogusProp2 = 1545,
		// Token: 0x040005EE RID: 1518
		ERR_BindToBogusProp1,
		// Token: 0x040005EF RID: 1519
		ERR_BadDelArgCount = 1593,
		// Token: 0x040005F0 RID: 1520
		ERR_BadDelArgTypes,
		// Token: 0x040005F1 RID: 1521
		ERR_AssgReadonlyLocal = 1604,
		// Token: 0x040005F2 RID: 1522
		ERR_RefReadonlyLocal,
		// Token: 0x040005F3 RID: 1523
		ERR_ReturnNotLValue = 1612,
		// Token: 0x040005F4 RID: 1524
		ERR_AssgReadonly2 = 1648,
		// Token: 0x040005F5 RID: 1525
		ERR_RefReadonly2,
		// Token: 0x040005F6 RID: 1526
		ERR_AssgReadonlyStatic2,
		// Token: 0x040005F7 RID: 1527
		ERR_RefReadonlyStatic2,
		// Token: 0x040005F8 RID: 1528
		ERR_AssgReadonlyLocalCause = 1656,
		// Token: 0x040005F9 RID: 1529
		ERR_RefReadonlyLocalCause,
		// Token: 0x040005FA RID: 1530
		ERR_BadCtorArgCount = 1729,
		// Token: 0x040005FB RID: 1531
		ERR_NonInvocableMemberCalled = 1955,
		// Token: 0x040005FC RID: 1532
		ERR_NamedArgumentSpecificationBeforeFixedArgument = 5002,
		// Token: 0x040005FD RID: 1533
		ERR_BadNamedArgument,
		// Token: 0x040005FE RID: 1534
		ERR_BadNamedArgumentForDelegateInvoke,
		// Token: 0x040005FF RID: 1535
		ERR_DuplicateNamedArgument,
		// Token: 0x04000600 RID: 1536
		ERR_NamedArgumentUsedInPositional
	}
}
