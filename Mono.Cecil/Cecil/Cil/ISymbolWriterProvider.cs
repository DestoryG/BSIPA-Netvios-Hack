using System;
using System.IO;

namespace Mono.Cecil.Cil
{
	// Token: 0x02000133 RID: 307
	public interface ISymbolWriterProvider
	{
		// Token: 0x06000B5C RID: 2908
		ISymbolWriter GetSymbolWriter(ModuleDefinition module, string fileName);

		// Token: 0x06000B5D RID: 2909
		ISymbolWriter GetSymbolWriter(ModuleDefinition module, Stream symbolStream);
	}
}
