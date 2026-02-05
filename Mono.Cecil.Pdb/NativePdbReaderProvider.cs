using System;
using System.IO;
using Mono.Cecil.Cil;

namespace Mono.Cecil.Pdb
{
	// Token: 0x0200000B RID: 11
	public sealed class NativePdbReaderProvider : ISymbolReaderProvider
	{
		// Token: 0x06000125 RID: 293 RVA: 0x00003799 File Offset: 0x00001999
		public ISymbolReader GetSymbolReader(ModuleDefinition module, string fileName)
		{
			Mixin.CheckModule(module);
			Mixin.CheckFileName(fileName);
			return new NativePdbReader(Disposable.Owned<Stream>(File.OpenRead(Mixin.GetPdbFileName(fileName))));
		}

		// Token: 0x06000126 RID: 294 RVA: 0x000037BC File Offset: 0x000019BC
		public ISymbolReader GetSymbolReader(ModuleDefinition module, Stream symbolStream)
		{
			Mixin.CheckModule(module);
			Mixin.CheckStream(symbolStream);
			return new NativePdbReader(Disposable.NotOwned<Stream>(symbolStream));
		}
	}
}
