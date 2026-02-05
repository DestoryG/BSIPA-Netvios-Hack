using System;
using System.IO;
using Mono.Cecil.Cil;
using Mono.CompilerServices.SymbolWriter;

namespace Mono.Cecil.Mdb
{
	// Token: 0x0200021F RID: 543
	internal sealed class MdbReaderProvider : ISymbolReaderProvider
	{
		// Token: 0x06001038 RID: 4152 RVA: 0x0003764A File Offset: 0x0003584A
		public ISymbolReader GetSymbolReader(ModuleDefinition module, string fileName)
		{
			Mixin.CheckModule(module);
			Mixin.CheckFileName(fileName);
			return new MdbReader(module, MonoSymbolFile.ReadSymbolFile(Mixin.GetMdbFileName(fileName)));
		}

		// Token: 0x06001039 RID: 4153 RVA: 0x00037669 File Offset: 0x00035869
		public ISymbolReader GetSymbolReader(ModuleDefinition module, Stream symbolStream)
		{
			Mixin.CheckModule(module);
			Mixin.CheckStream(symbolStream);
			return new MdbReader(module, MonoSymbolFile.ReadSymbolFile(symbolStream));
		}
	}
}
