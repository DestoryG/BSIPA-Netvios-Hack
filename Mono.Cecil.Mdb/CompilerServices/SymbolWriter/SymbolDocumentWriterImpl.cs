using System;
using System.Diagnostics.SymbolStore;

namespace Mono.CompilerServices.SymbolWriter
{
	// Token: 0x02000019 RID: 25
	internal class SymbolDocumentWriterImpl : ISymbolDocumentWriter, ISourceFile, ICompileUnit
	{
		// Token: 0x060000D1 RID: 209 RVA: 0x000058AE File Offset: 0x00003AAE
		public SymbolDocumentWriterImpl(CompileUnitEntry comp_unit)
		{
			this.comp_unit = comp_unit;
		}

		// Token: 0x060000D2 RID: 210 RVA: 0x00004EEF File Offset: 0x000030EF
		public void SetCheckSum(Guid algorithmId, byte[] checkSum)
		{
		}

		// Token: 0x060000D3 RID: 211 RVA: 0x00004EEF File Offset: 0x000030EF
		public void SetSource(byte[] source)
		{
		}

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x060000D4 RID: 212 RVA: 0x000058BD File Offset: 0x00003ABD
		SourceFileEntry ISourceFile.Entry
		{
			get
			{
				return this.comp_unit.SourceFile;
			}
		}

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x060000D5 RID: 213 RVA: 0x000058CA File Offset: 0x00003ACA
		public CompileUnitEntry Entry
		{
			get
			{
				return this.comp_unit;
			}
		}

		// Token: 0x04000098 RID: 152
		private CompileUnitEntry comp_unit;
	}
}
