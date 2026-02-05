using System;
using System.IO;
using System.IO.Compression;
using Mono.Cecil.PE;

namespace Mono.Cecil.Cil
{
	// Token: 0x020001CF RID: 463
	internal sealed class EmbeddedPortablePdbReaderProvider : ISymbolReaderProvider
	{
		// Token: 0x06000E7C RID: 3708 RVA: 0x000322FC File Offset: 0x000304FC
		public ISymbolReader GetSymbolReader(ModuleDefinition module, string fileName)
		{
			Mixin.CheckModule(module);
			ImageDebugHeaderEntry embeddedPortablePdbEntry = module.GetDebugHeader().GetEmbeddedPortablePdbEntry();
			if (embeddedPortablePdbEntry == null)
			{
				throw new InvalidOperationException();
			}
			return new EmbeddedPortablePdbReader((PortablePdbReader)new PortablePdbReaderProvider().GetSymbolReader(module, EmbeddedPortablePdbReaderProvider.GetPortablePdbStream(embeddedPortablePdbEntry)));
		}

		// Token: 0x06000E7D RID: 3709 RVA: 0x00032340 File Offset: 0x00030540
		private static Stream GetPortablePdbStream(ImageDebugHeaderEntry entry)
		{
			MemoryStream memoryStream = new MemoryStream(entry.Data);
			BinaryStreamReader binaryStreamReader = new BinaryStreamReader(memoryStream);
			binaryStreamReader.ReadInt32();
			MemoryStream memoryStream2 = new MemoryStream(binaryStreamReader.ReadInt32());
			using (DeflateStream deflateStream = new DeflateStream(memoryStream, CompressionMode.Decompress, true))
			{
				deflateStream.CopyTo(memoryStream2);
			}
			return memoryStream2;
		}

		// Token: 0x06000E7E RID: 3710 RVA: 0x000039BA File Offset: 0x00001BBA
		public ISymbolReader GetSymbolReader(ModuleDefinition module, Stream symbolStream)
		{
			throw new NotSupportedException();
		}
	}
}
