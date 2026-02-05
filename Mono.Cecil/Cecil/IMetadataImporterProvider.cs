using System;

namespace Mono.Cecil
{
	// Token: 0x02000070 RID: 112
	public interface IMetadataImporterProvider
	{
		// Token: 0x06000484 RID: 1156
		IMetadataImporter GetMetadataImporter(ModuleDefinition module);
	}
}
