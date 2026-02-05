using System;
using System.IO;
using Mono.Cecil.Cil;

namespace Mono.Cecil.Pdb
{
	// Token: 0x0200000C RID: 12
	public sealed class PdbReaderProvider : ISymbolReaderProvider
	{
		// Token: 0x06000128 RID: 296 RVA: 0x000037E0 File Offset: 0x000019E0
		public ISymbolReader GetSymbolReader(ModuleDefinition module, string fileName)
		{
			Mixin.CheckModule(module);
			Mixin.CheckFileName(fileName);
			if (module.HasDebugHeader && module.GetDebugHeader().GetEmbeddedPortablePdbEntry() != null)
			{
				return new EmbeddedPortablePdbReaderProvider().GetSymbolReader(module, fileName);
			}
			if (!Mixin.IsPortablePdb(Mixin.GetPdbFileName(fileName)))
			{
				return new NativePdbReaderProvider().GetSymbolReader(module, fileName);
			}
			return new PortablePdbReaderProvider().GetSymbolReader(module, fileName);
		}

		// Token: 0x06000129 RID: 297 RVA: 0x00003841 File Offset: 0x00001A41
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
