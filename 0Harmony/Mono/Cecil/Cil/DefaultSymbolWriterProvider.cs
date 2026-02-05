using System;
using System.IO;

namespace Mono.Cecil.Cil
{
	// Token: 0x020001F9 RID: 505
	internal class DefaultSymbolWriterProvider : ISymbolWriterProvider
	{
		// Token: 0x06000F4E RID: 3918 RVA: 0x00033C78 File Offset: 0x00031E78
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

		// Token: 0x06000F4F RID: 3919 RVA: 0x000039BA File Offset: 0x00001BBA
		public ISymbolWriter GetSymbolWriter(ModuleDefinition module, Stream symbolStream)
		{
			throw new NotSupportedException();
		}
	}
}
