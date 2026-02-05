using System;

namespace Mono.Cecil.Cil
{
	// Token: 0x020001E2 RID: 482
	internal enum ImportTargetKind : byte
	{
		// Token: 0x04000913 RID: 2323
		ImportNamespace = 1,
		// Token: 0x04000914 RID: 2324
		ImportNamespaceInAssembly,
		// Token: 0x04000915 RID: 2325
		ImportType,
		// Token: 0x04000916 RID: 2326
		ImportXmlNamespaceWithAlias,
		// Token: 0x04000917 RID: 2327
		ImportAlias,
		// Token: 0x04000918 RID: 2328
		DefineAssemblyAlias,
		// Token: 0x04000919 RID: 2329
		DefineNamespaceAlias,
		// Token: 0x0400091A RID: 2330
		DefineNamespaceInAssemblyAlias,
		// Token: 0x0400091B RID: 2331
		DefineTypeAlias
	}
}
