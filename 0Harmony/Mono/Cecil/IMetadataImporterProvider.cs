using System;

namespace Mono.Cecil
{
	// Token: 0x02000126 RID: 294
	internal interface IMetadataImporterProvider
	{
		// Token: 0x06000816 RID: 2070
		IMetadataImporter GetMetadataImporter(ModuleDefinition module);
	}
}
