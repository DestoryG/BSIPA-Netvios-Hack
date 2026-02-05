using System;
using System.IO;
using Mono.Cecil.Cil;
using Mono.CompilerServices.SymbolWriter;

namespace Mono.Cecil.Mdb
{
	// Token: 0x0200001C RID: 28
	public sealed class MdbReaderProvider : ISymbolReaderProvider
	{
		// Token: 0x060000DB RID: 219 RVA: 0x0000591A File Offset: 0x00003B1A
		public ISymbolReader GetSymbolReader(ModuleDefinition module, string fileName)
		{
			Mixin.CheckModule(module);
			Mixin.CheckFileName(fileName);
			return new MdbReader(module, MonoSymbolFile.ReadSymbolFile(Mixin.GetMdbFileName(fileName)));
		}

		// Token: 0x060000DC RID: 220 RVA: 0x00005939 File Offset: 0x00003B39
		public ISymbolReader GetSymbolReader(ModuleDefinition module, Stream symbolStream)
		{
			Mixin.CheckModule(module);
			Mixin.CheckStream(symbolStream);
			return new MdbReader(module, MonoSymbolFile.ReadSymbolFile(symbolStream));
		}
	}
}
