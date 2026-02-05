using System;
using System.IO;
using Mono.Cecil.Cil;

namespace Mono.Cecil.Pdb
{
	// Token: 0x02000237 RID: 567
	internal sealed class PdbReaderProvider : ISymbolReaderProvider
	{
		// Token: 0x06001197 RID: 4503 RVA: 0x00039750 File Offset: 0x00037950
		public ISymbolReader GetSymbolReader(ModuleDefinition module, string fileName)
		{
			Mixin.CheckModule(module);
			if (module.HasDebugHeader && module.GetDebugHeader().GetEmbeddedPortablePdbEntry() != null)
			{
				return new EmbeddedPortablePdbReaderProvider().GetSymbolReader(module, fileName);
			}
			Mixin.CheckFileName(fileName);
			if (!Mixin.IsPortablePdb(Mixin.GetPdbFileName(fileName)))
			{
				return new NativePdbReaderProvider().GetSymbolReader(module, fileName);
			}
			return new PortablePdbReaderProvider().GetSymbolReader(module, fileName);
		}

		// Token: 0x06001198 RID: 4504 RVA: 0x000397B1 File Offset: 0x000379B1
		public ISymbolReader GetSymbolReader(ModuleDefinition module, Stream symbolStream)
		{
			Mixin.CheckModule(module);
			Mixin.CheckStream(symbolStream);
			Mixin.CheckReadSeek(symbolStream);
			if (!Mixin.IsPortablePdb(symbolStream))
			{
				return new NativePdbReaderProvider().GetSymbolReader(module, symbolStream);
			}
			return new PortablePdbReaderProvider().GetSymbolReader(module, symbolStream);
		}
	}
}
