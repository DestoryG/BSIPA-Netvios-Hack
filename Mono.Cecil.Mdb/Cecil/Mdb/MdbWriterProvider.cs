using System;
using System.IO;
using Mono.Cecil.Cil;

namespace Mono.Cecil.Mdb
{
	// Token: 0x0200001F RID: 31
	public sealed class MdbWriterProvider : ISymbolWriterProvider
	{
		// Token: 0x060000EC RID: 236 RVA: 0x00005D54 File Offset: 0x00003F54
		public ISymbolWriter GetSymbolWriter(ModuleDefinition module, string fileName)
		{
			Mixin.CheckModule(module);
			Mixin.CheckFileName(fileName);
			return new MdbWriter(module.Mvid, fileName);
		}

		// Token: 0x060000ED RID: 237 RVA: 0x00005D6E File Offset: 0x00003F6E
		public ISymbolWriter GetSymbolWriter(ModuleDefinition module, Stream symbolStream)
		{
			throw new NotImplementedException();
		}
	}
}
