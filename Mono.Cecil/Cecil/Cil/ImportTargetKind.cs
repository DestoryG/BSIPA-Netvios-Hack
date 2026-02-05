using System;

namespace Mono.Cecil.Cil
{
	// Token: 0x0200011E RID: 286
	public enum ImportTargetKind : byte
	{
		// Token: 0x040006B4 RID: 1716
		ImportNamespace = 1,
		// Token: 0x040006B5 RID: 1717
		ImportNamespaceInAssembly,
		// Token: 0x040006B6 RID: 1718
		ImportType,
		// Token: 0x040006B7 RID: 1719
		ImportXmlNamespaceWithAlias,
		// Token: 0x040006B8 RID: 1720
		ImportAlias,
		// Token: 0x040006B9 RID: 1721
		DefineAssemblyAlias,
		// Token: 0x040006BA RID: 1722
		DefineNamespaceAlias,
		// Token: 0x040006BB RID: 1723
		DefineNamespaceInAssemblyAlias,
		// Token: 0x040006BC RID: 1724
		DefineTypeAlias
	}
}
