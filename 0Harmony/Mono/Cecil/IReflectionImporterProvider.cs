using System;

namespace Mono.Cecil
{
	// Token: 0x02000128 RID: 296
	internal interface IReflectionImporterProvider
	{
		// Token: 0x0600081B RID: 2075
		IReflectionImporter GetReflectionImporter(ModuleDefinition module);
	}
}
