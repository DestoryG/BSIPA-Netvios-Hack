using System;
using System.IO;

namespace Mono.Cecil.Cil
{
	// Token: 0x020001D3 RID: 467
	internal sealed class EmbeddedPortablePdbWriterProvider : ISymbolWriterProvider
	{
		// Token: 0x06000E94 RID: 3732 RVA: 0x000327AC File Offset: 0x000309AC
		public ISymbolWriter GetSymbolWriter(ModuleDefinition module, string fileName)
		{
			Mixin.CheckModule(module);
			Mixin.CheckFileName(fileName);
			MemoryStream memoryStream = new MemoryStream();
			PortablePdbWriter portablePdbWriter = (PortablePdbWriter)new PortablePdbWriterProvider().GetSymbolWriter(module, memoryStream);
			return new EmbeddedPortablePdbWriter(memoryStream, portablePdbWriter);
		}

		// Token: 0x06000E95 RID: 3733 RVA: 0x000039BA File Offset: 0x00001BBA
		public ISymbolWriter GetSymbolWriter(ModuleDefinition module, Stream symbolStream)
		{
			throw new NotSupportedException();
		}
	}
}
