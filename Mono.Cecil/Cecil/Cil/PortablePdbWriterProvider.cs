using System;
using System.IO;
using Mono.Cecil.PE;

namespace Mono.Cecil.Cil
{
	// Token: 0x0200010D RID: 269
	public sealed class PortablePdbWriterProvider : ISymbolWriterProvider
	{
		// Token: 0x06000A9E RID: 2718 RVA: 0x000231D8 File Offset: 0x000213D8
		public ISymbolWriter GetSymbolWriter(ModuleDefinition module, string fileName)
		{
			Mixin.CheckModule(module);
			Mixin.CheckFileName(fileName);
			FileStream fileStream = File.OpenWrite(Mixin.GetPdbFileName(fileName));
			return this.GetSymbolWriter(module, Disposable.Owned<Stream>(fileStream));
		}

		// Token: 0x06000A9F RID: 2719 RVA: 0x0002320A File Offset: 0x0002140A
		public ISymbolWriter GetSymbolWriter(ModuleDefinition module, Stream symbolStream)
		{
			Mixin.CheckModule(module);
			Mixin.CheckStream(symbolStream);
			return this.GetSymbolWriter(module, Disposable.NotOwned<Stream>(symbolStream));
		}

		// Token: 0x06000AA0 RID: 2720 RVA: 0x00023228 File Offset: 0x00021428
		private ISymbolWriter GetSymbolWriter(ModuleDefinition module, Disposable<Stream> stream)
		{
			MetadataBuilder metadataBuilder = new MetadataBuilder(module, this);
			ImageWriter imageWriter = ImageWriter.CreateDebugWriter(module, metadataBuilder, stream);
			return new PortablePdbWriter(metadataBuilder, module, imageWriter);
		}
	}
}
