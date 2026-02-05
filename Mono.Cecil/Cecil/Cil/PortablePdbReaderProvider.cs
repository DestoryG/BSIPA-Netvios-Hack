using System;
using System.IO;
using Mono.Cecil.PE;

namespace Mono.Cecil.Cil
{
	// Token: 0x02000109 RID: 265
	public sealed class PortablePdbReaderProvider : ISymbolReaderProvider
	{
		// Token: 0x06000A85 RID: 2693 RVA: 0x00022EA8 File Offset: 0x000210A8
		public ISymbolReader GetSymbolReader(ModuleDefinition module, string fileName)
		{
			Mixin.CheckModule(module);
			Mixin.CheckFileName(fileName);
			FileStream fileStream = File.OpenRead(Mixin.GetPdbFileName(fileName));
			return this.GetSymbolReader(module, Disposable.Owned<Stream>(fileStream), fileStream.Name);
		}

		// Token: 0x06000A86 RID: 2694 RVA: 0x00022EE0 File Offset: 0x000210E0
		public ISymbolReader GetSymbolReader(ModuleDefinition module, Stream symbolStream)
		{
			Mixin.CheckModule(module);
			Mixin.CheckStream(symbolStream);
			return this.GetSymbolReader(module, Disposable.NotOwned<Stream>(symbolStream), symbolStream.GetFileName());
		}

		// Token: 0x06000A87 RID: 2695 RVA: 0x00022F01 File Offset: 0x00021101
		private ISymbolReader GetSymbolReader(ModuleDefinition module, Disposable<Stream> symbolStream, string fileName)
		{
			return new PortablePdbReader(ImageReader.ReadPortablePdb(symbolStream, fileName), module);
		}
	}
}
