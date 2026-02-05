using System;
using System.IO;
using Mono.Cecil.PE;

namespace Mono.Cecil.Cil
{
	// Token: 0x020001D1 RID: 465
	internal sealed class PortablePdbWriterProvider : ISymbolWriterProvider
	{
		// Token: 0x06000E85 RID: 3717 RVA: 0x000323E4 File Offset: 0x000305E4
		public ISymbolWriter GetSymbolWriter(ModuleDefinition module, string fileName)
		{
			Mixin.CheckModule(module);
			Mixin.CheckFileName(fileName);
			FileStream fileStream = File.OpenWrite(Mixin.GetPdbFileName(fileName));
			return this.GetSymbolWriter(module, Disposable.Owned<Stream>(fileStream));
		}

		// Token: 0x06000E86 RID: 3718 RVA: 0x00032416 File Offset: 0x00030616
		public ISymbolWriter GetSymbolWriter(ModuleDefinition module, Stream symbolStream)
		{
			Mixin.CheckModule(module);
			Mixin.CheckStream(symbolStream);
			return this.GetSymbolWriter(module, Disposable.NotOwned<Stream>(symbolStream));
		}

		// Token: 0x06000E87 RID: 3719 RVA: 0x00032434 File Offset: 0x00030634
		private ISymbolWriter GetSymbolWriter(ModuleDefinition module, Disposable<Stream> stream)
		{
			MetadataBuilder metadataBuilder = new MetadataBuilder(module, this);
			ImageWriter imageWriter = ImageWriter.CreateDebugWriter(module, metadataBuilder, stream);
			return new PortablePdbWriter(metadataBuilder, module, imageWriter);
		}
	}
}
