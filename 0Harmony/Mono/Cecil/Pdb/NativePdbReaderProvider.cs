using System;
using System.IO;
using Mono.Cecil.Cil;

namespace Mono.Cecil.Pdb
{
	// Token: 0x02000236 RID: 566
	internal sealed class NativePdbReaderProvider : ISymbolReaderProvider
	{
		// Token: 0x06001194 RID: 4500 RVA: 0x00039713 File Offset: 0x00037913
		public ISymbolReader GetSymbolReader(ModuleDefinition module, string fileName)
		{
			Mixin.CheckModule(module);
			Mixin.CheckFileName(fileName);
			return new NativePdbReader(Disposable.Owned<Stream>(File.OpenRead(Mixin.GetPdbFileName(fileName))));
		}

		// Token: 0x06001195 RID: 4501 RVA: 0x00039736 File Offset: 0x00037936
		public ISymbolReader GetSymbolReader(ModuleDefinition module, Stream symbolStream)
		{
			Mixin.CheckModule(module);
			Mixin.CheckStream(symbolStream);
			return new NativePdbReader(Disposable.NotOwned<Stream>(symbolStream));
		}
	}
}
