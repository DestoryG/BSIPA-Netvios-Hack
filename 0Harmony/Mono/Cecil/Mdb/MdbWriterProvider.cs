using System;
using System.IO;
using Mono.Cecil.Cil;

namespace Mono.Cecil.Mdb
{
	// Token: 0x02000223 RID: 547
	internal sealed class MdbWriterProvider : ISymbolWriterProvider
	{
		// Token: 0x0600104C RID: 4172 RVA: 0x00037A91 File Offset: 0x00035C91
		public ISymbolWriter GetSymbolWriter(ModuleDefinition module, string fileName)
		{
			Mixin.CheckModule(module);
			Mixin.CheckFileName(fileName);
			return new MdbWriter(module.Mvid, fileName);
		}

		// Token: 0x0600104D RID: 4173 RVA: 0x00037AAB File Offset: 0x00035CAB
		public ISymbolWriter GetSymbolWriter(ModuleDefinition module, Stream symbolStream)
		{
			throw new NotImplementedException();
		}
	}
}
