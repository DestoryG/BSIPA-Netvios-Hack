using System;
using System.IO;

namespace Mono.Cecil.Cil
{
	// Token: 0x0200010F RID: 271
	public sealed class EmbeddedPortablePdbWriterProvider : ISymbolWriterProvider
	{
		// Token: 0x06000AAD RID: 2733 RVA: 0x000235A0 File Offset: 0x000217A0
		public ISymbolWriter GetSymbolWriter(ModuleDefinition module, string fileName)
		{
			Mixin.CheckModule(module);
			Mixin.CheckFileName(fileName);
			MemoryStream memoryStream = new MemoryStream();
			PortablePdbWriter portablePdbWriter = (PortablePdbWriter)new PortablePdbWriterProvider().GetSymbolWriter(module, memoryStream);
			return new EmbeddedPortablePdbWriter(memoryStream, portablePdbWriter);
		}

		// Token: 0x06000AAE RID: 2734 RVA: 0x00011A5E File Offset: 0x0000FC5E
		public ISymbolWriter GetSymbolWriter(ModuleDefinition module, Stream symbolStream)
		{
			throw new NotSupportedException();
		}
	}
}
