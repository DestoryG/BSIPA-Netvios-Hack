using System;
using System.IO;
using Mono.Cecil.PE;

namespace Mono.Cecil.Cil
{
	// Token: 0x020001CD RID: 461
	internal sealed class PortablePdbReaderProvider : ISymbolReaderProvider
	{
		// Token: 0x06000E6C RID: 3692 RVA: 0x000320B4 File Offset: 0x000302B4
		public ISymbolReader GetSymbolReader(ModuleDefinition module, string fileName)
		{
			Mixin.CheckModule(module);
			Mixin.CheckFileName(fileName);
			FileStream fileStream = File.OpenRead(Mixin.GetPdbFileName(fileName));
			return this.GetSymbolReader(module, Disposable.Owned<Stream>(fileStream), fileStream.Name);
		}

		// Token: 0x06000E6D RID: 3693 RVA: 0x000320EC File Offset: 0x000302EC
		public ISymbolReader GetSymbolReader(ModuleDefinition module, Stream symbolStream)
		{
			Mixin.CheckModule(module);
			Mixin.CheckStream(symbolStream);
			return this.GetSymbolReader(module, Disposable.NotOwned<Stream>(symbolStream), symbolStream.GetFileName());
		}

		// Token: 0x06000E6E RID: 3694 RVA: 0x0003210D File Offset: 0x0003030D
		private ISymbolReader GetSymbolReader(ModuleDefinition module, Disposable<Stream> symbolStream, string fileName)
		{
			return new PortablePdbReader(ImageReader.ReadPortablePdb(symbolStream, fileName), module);
		}
	}
}
