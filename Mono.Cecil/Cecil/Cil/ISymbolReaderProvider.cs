using System;
using System.IO;

namespace Mono.Cecil.Cil
{
	// Token: 0x0200012C RID: 300
	public interface ISymbolReaderProvider
	{
		// Token: 0x06000B4A RID: 2890
		ISymbolReader GetSymbolReader(ModuleDefinition module, string fileName);

		// Token: 0x06000B4B RID: 2891
		ISymbolReader GetSymbolReader(ModuleDefinition module, Stream symbolStream);
	}
}
