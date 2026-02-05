using System;

namespace Mono.Cecil.Metadata
{
	// Token: 0x020000EF RID: 239
	internal enum Table : byte
	{
		// Token: 0x040003EF RID: 1007
		Module,
		// Token: 0x040003F0 RID: 1008
		TypeRef,
		// Token: 0x040003F1 RID: 1009
		TypeDef,
		// Token: 0x040003F2 RID: 1010
		FieldPtr,
		// Token: 0x040003F3 RID: 1011
		Field,
		// Token: 0x040003F4 RID: 1012
		MethodPtr,
		// Token: 0x040003F5 RID: 1013
		Method,
		// Token: 0x040003F6 RID: 1014
		ParamPtr,
		// Token: 0x040003F7 RID: 1015
		Param,
		// Token: 0x040003F8 RID: 1016
		InterfaceImpl,
		// Token: 0x040003F9 RID: 1017
		MemberRef,
		// Token: 0x040003FA RID: 1018
		Constant,
		// Token: 0x040003FB RID: 1019
		CustomAttribute,
		// Token: 0x040003FC RID: 1020
		FieldMarshal,
		// Token: 0x040003FD RID: 1021
		DeclSecurity,
		// Token: 0x040003FE RID: 1022
		ClassLayout,
		// Token: 0x040003FF RID: 1023
		FieldLayout,
		// Token: 0x04000400 RID: 1024
		StandAloneSig,
		// Token: 0x04000401 RID: 1025
		EventMap,
		// Token: 0x04000402 RID: 1026
		EventPtr,
		// Token: 0x04000403 RID: 1027
		Event,
		// Token: 0x04000404 RID: 1028
		PropertyMap,
		// Token: 0x04000405 RID: 1029
		PropertyPtr,
		// Token: 0x04000406 RID: 1030
		Property,
		// Token: 0x04000407 RID: 1031
		MethodSemantics,
		// Token: 0x04000408 RID: 1032
		MethodImpl,
		// Token: 0x04000409 RID: 1033
		ModuleRef,
		// Token: 0x0400040A RID: 1034
		TypeSpec,
		// Token: 0x0400040B RID: 1035
		ImplMap,
		// Token: 0x0400040C RID: 1036
		FieldRVA,
		// Token: 0x0400040D RID: 1037
		EncLog,
		// Token: 0x0400040E RID: 1038
		EncMap,
		// Token: 0x0400040F RID: 1039
		Assembly,
		// Token: 0x04000410 RID: 1040
		AssemblyProcessor,
		// Token: 0x04000411 RID: 1041
		AssemblyOS,
		// Token: 0x04000412 RID: 1042
		AssemblyRef,
		// Token: 0x04000413 RID: 1043
		AssemblyRefProcessor,
		// Token: 0x04000414 RID: 1044
		AssemblyRefOS,
		// Token: 0x04000415 RID: 1045
		File,
		// Token: 0x04000416 RID: 1046
		ExportedType,
		// Token: 0x04000417 RID: 1047
		ManifestResource,
		// Token: 0x04000418 RID: 1048
		NestedClass,
		// Token: 0x04000419 RID: 1049
		GenericParam,
		// Token: 0x0400041A RID: 1050
		MethodSpec,
		// Token: 0x0400041B RID: 1051
		GenericParamConstraint,
		// Token: 0x0400041C RID: 1052
		Document = 48,
		// Token: 0x0400041D RID: 1053
		MethodDebugInformation,
		// Token: 0x0400041E RID: 1054
		LocalScope,
		// Token: 0x0400041F RID: 1055
		LocalVariable,
		// Token: 0x04000420 RID: 1056
		LocalConstant,
		// Token: 0x04000421 RID: 1057
		ImportScope,
		// Token: 0x04000422 RID: 1058
		StateMachineMethod,
		// Token: 0x04000423 RID: 1059
		CustomDebugInformation
	}
}
