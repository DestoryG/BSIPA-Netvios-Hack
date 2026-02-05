using System;
using System.IO;
using Mono.Cecil.Cil;

namespace Mono.Cecil.Pdb
{
	// Token: 0x0200000E RID: 14
	public sealed class PdbWriterProvider : ISymbolWriterProvider
	{
		// Token: 0x0600012F RID: 303 RVA: 0x000038B9 File Offset: 0x00001AB9
		public ISymbolWriter GetSymbolWriter(ModuleDefinition module, string fileName)
		{
			Mixin.CheckModule(module);
			Mixin.CheckFileName(fileName);
			if (PdbWriterProvider.HasPortablePdbSymbols(module))
			{
				return new PortablePdbWriterProvider().GetSymbolWriter(module, fileName);
			}
			return new NativePdbWriterProvider().GetSymbolWriter(module, fileName);
		}

		// Token: 0x06000130 RID: 304 RVA: 0x000038E8 File Offset: 0x00001AE8
		private static bool HasPortablePdbSymbols(ModuleDefinition module)
		{
			return module.symbol_reader != null && module.symbol_reader is PortablePdbReader;
		}

		// Token: 0x06000131 RID: 305 RVA: 0x00003902 File Offset: 0x00001B02
		public ISymbolWriter GetSymbolWriter(ModuleDefinition module, Stream symbolStream)
		{
			Mixin.CheckModule(module);
			Mixin.CheckStream(symbolStream);
			Mixin.CheckReadSeek(symbolStream);
			if (PdbWriterProvider.HasPortablePdbSymbols(module))
			{
				return new PortablePdbWriterProvider().GetSymbolWriter(module, symbolStream);
			}
			return new NativePdbWriterProvider().GetSymbolWriter(module, symbolStream);
		}
	}
}
