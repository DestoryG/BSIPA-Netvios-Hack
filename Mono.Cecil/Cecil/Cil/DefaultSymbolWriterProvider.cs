using System;
using System.IO;

namespace Mono.Cecil.Cil
{
	// Token: 0x02000134 RID: 308
	public class DefaultSymbolWriterProvider : ISymbolWriterProvider
	{
		// Token: 0x06000B5E RID: 2910 RVA: 0x000248B8 File Offset: 0x00022AB8
		public ISymbolWriter GetSymbolWriter(ModuleDefinition module, string fileName)
		{
			ISymbolReader symbolReader = module.SymbolReader;
			if (symbolReader == null)
			{
				throw new InvalidOperationException();
			}
			if (module.Image != null && module.Image.HasDebugTables())
			{
				return null;
			}
			return symbolReader.GetWriterProvider().GetSymbolWriter(module, fileName);
		}

		// Token: 0x06000B5F RID: 2911 RVA: 0x00011A5E File Offset: 0x0000FC5E
		public ISymbolWriter GetSymbolWriter(ModuleDefinition module, Stream symbolStream)
		{
			throw new NotSupportedException();
		}
	}
}
