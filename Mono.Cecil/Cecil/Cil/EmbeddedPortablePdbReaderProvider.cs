using System;
using System.IO;
using System.IO.Compression;
using Mono.Cecil.PE;

namespace Mono.Cecil.Cil
{
	// Token: 0x0200010B RID: 267
	public sealed class EmbeddedPortablePdbReaderProvider : ISymbolReaderProvider
	{
		// Token: 0x06000A95 RID: 2709 RVA: 0x000230F0 File Offset: 0x000212F0
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

		// Token: 0x06000A96 RID: 2710 RVA: 0x00023134 File Offset: 0x00021334
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

		// Token: 0x06000A97 RID: 2711 RVA: 0x00011A5E File Offset: 0x0000FC5E
		public ISymbolReader GetSymbolReader(ModuleDefinition module, Stream symbolStream)
		{
			throw new NotSupportedException();
		}
	}
}
