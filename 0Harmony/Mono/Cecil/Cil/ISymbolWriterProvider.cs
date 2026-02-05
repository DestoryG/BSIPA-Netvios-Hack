using System;
using System.IO;

namespace Mono.Cecil.Cil
{
	// Token: 0x020001F8 RID: 504
	internal interface ISymbolWriterProvider
	{
		// Token: 0x06000F4C RID: 3916
		ISymbolWriter GetSymbolWriter(ModuleDefinition module, string fileName);

		// Token: 0x06000F4D RID: 3917
		ISymbolWriter GetSymbolWriter(ModuleDefinition module, Stream symbolStream);
	}
}
