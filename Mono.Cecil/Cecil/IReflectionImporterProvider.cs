using System;

namespace Mono.Cecil
{
	// Token: 0x02000072 RID: 114
	public interface IReflectionImporterProvider
	{
		// Token: 0x06000489 RID: 1161
		IReflectionImporter GetReflectionImporter(ModuleDefinition module);
	}
}
