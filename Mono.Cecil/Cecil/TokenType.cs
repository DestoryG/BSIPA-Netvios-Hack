using System;

namespace Mono.Cecil
{
	// Token: 0x020000CB RID: 203
	public enum TokenType : uint
	{
		// Token: 0x04000318 RID: 792
		Module,
		// Token: 0x04000319 RID: 793
		TypeRef = 16777216U,
		// Token: 0x0400031A RID: 794
		TypeDef = 33554432U,
		// Token: 0x0400031B RID: 795
		Field = 67108864U,
		// Token: 0x0400031C RID: 796
		Method = 100663296U,
		// Token: 0x0400031D RID: 797
		Param = 134217728U,
		// Token: 0x0400031E RID: 798
		InterfaceImpl = 150994944U,
		// Token: 0x0400031F RID: 799
		MemberRef = 167772160U,
		// Token: 0x04000320 RID: 800
		CustomAttribute = 201326592U,
		// Token: 0x04000321 RID: 801
		Permission = 234881024U,
		// Token: 0x04000322 RID: 802
		Signature = 285212672U,
		// Token: 0x04000323 RID: 803
		Event = 335544320U,
		// Token: 0x04000324 RID: 804
		Property = 385875968U,
		// Token: 0x04000325 RID: 805
		ModuleRef = 436207616U,
		// Token: 0x04000326 RID: 806
		TypeSpec = 452984832U,
		// Token: 0x04000327 RID: 807
		Assembly = 536870912U,
		// Token: 0x04000328 RID: 808
		AssemblyRef = 587202560U,
		// Token: 0x04000329 RID: 809
		File = 637534208U,
		// Token: 0x0400032A RID: 810
		ExportedType = 654311424U,
		// Token: 0x0400032B RID: 811
		ManifestResource = 671088640U,
		// Token: 0x0400032C RID: 812
		GenericParam = 704643072U,
		// Token: 0x0400032D RID: 813
		MethodSpec = 721420288U,
		// Token: 0x0400032E RID: 814
		GenericParamConstraint = 738197504U,
		// Token: 0x0400032F RID: 815
		Document = 805306368U,
		// Token: 0x04000330 RID: 816
		MethodDebugInformation = 822083584U,
		// Token: 0x04000331 RID: 817
		LocalScope = 838860800U,
		// Token: 0x04000332 RID: 818
		LocalVariable = 855638016U,
		// Token: 0x04000333 RID: 819
		LocalConstant = 872415232U,
		// Token: 0x04000334 RID: 820
		ImportScope = 889192448U,
		// Token: 0x04000335 RID: 821
		StateMachineMethod = 905969664U,
		// Token: 0x04000336 RID: 822
		CustomDebugInformation = 922746880U,
		// Token: 0x04000337 RID: 823
		String = 1879048192U
	}
}
