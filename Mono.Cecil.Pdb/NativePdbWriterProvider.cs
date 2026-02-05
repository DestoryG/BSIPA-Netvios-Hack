using System;
using System.IO;
using Mono.Cecil.Cil;

namespace Mono.Cecil.Pdb
{
	// Token: 0x0200000D RID: 13
	public sealed class NativePdbWriterProvider : ISymbolWriterProvider
	{
		// Token: 0x0600012B RID: 299 RVA: 0x00003876 File Offset: 0x00001A76
		public ISymbolWriter GetSymbolWriter(ModuleDefinition module, string fileName)
		{
			Mixin.CheckModule(module);
			Mixin.CheckFileName(fileName);
			return new NativePdbWriter(module, NativePdbWriterProvider.CreateWriter(module, Mixin.GetPdbFileName(fileName)));
		}

		// Token: 0x0600012C RID: 300 RVA: 0x00003896 File Offset: 0x00001A96
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

		// Token: 0x0600012D RID: 301 RVA: 0x00002190 File Offset: 0x00000390
		public ISymbolWriter GetSymbolWriter(ModuleDefinition module, Stream symbolStream)
		{
			throw new NotImplementedException();
		}
	}
}
