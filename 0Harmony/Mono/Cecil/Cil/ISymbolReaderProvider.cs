using System;
using System.IO;

namespace Mono.Cecil.Cil
{
	// Token: 0x020001F1 RID: 497
	internal interface ISymbolReaderProvider
	{
		// Token: 0x06000F3A RID: 3898
		ISymbolReader GetSymbolReader(ModuleDefinition module, string fileName);

		// Token: 0x06000F3B RID: 3899
		ISymbolReader GetSymbolReader(ModuleDefinition module, Stream symbolStream);
	}
}
