using System;
using System.IO;
using Mono.Cecil.Cil;

namespace Mono.Cecil.Pdb
{
	// Token: 0x02000239 RID: 569
	internal sealed class PdbWriterProvider : ISymbolWriterProvider
	{
		// Token: 0x0600119E RID: 4510 RVA: 0x00039829 File Offset: 0x00037A29
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

		// Token: 0x0600119F RID: 4511 RVA: 0x00039858 File Offset: 0x00037A58
		private static bool HasPortablePdbSymbols(ModuleDefinition module)
		{
			return module.symbol_reader != null && module.symbol_reader is PortablePdbReader;
		}

		// Token: 0x060011A0 RID: 4512 RVA: 0x00039872 File Offset: 0x00037A72
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
