using System;
using System.IO;
using Mono.Cecil.Cil;

namespace Mono.Cecil.Pdb
{
	// Token: 0x02000238 RID: 568
	internal sealed class NativePdbWriterProvider : ISymbolWriterProvider
	{
		// Token: 0x0600119A RID: 4506 RVA: 0x000397E6 File Offset: 0x000379E6
		public ISymbolWriter GetSymbolWriter(ModuleDefinition module, string fileName)
		{
			Mixin.CheckModule(module);
			Mixin.CheckFileName(fileName);
			return new NativePdbWriter(module, NativePdbWriterProvider.CreateWriter(module, Mixin.GetPdbFileName(fileName)));
		}

		// Token: 0x0600119B RID: 4507 RVA: 0x00039806 File Offset: 0x00037A06
		private static SymWriter CreateWriter(ModuleDefinition module, string pdb)
		{
			SymWriter symWriter = new SymWriter();
			if (File.Exists(pdb))
			{
				File.Delete(pdb);
			}
			symWriter.Initialize(new ModuleMetadata(module), pdb, true);
			return symWriter;
		}

		// Token: 0x0600119C RID: 4508 RVA: 0x00037AAB File Offset: 0x00035CAB
		public ISymbolWriter GetSymbolWriter(ModuleDefinition module, Stream symbolStream)
		{
			throw new NotImplementedException();
		}
	}
}
