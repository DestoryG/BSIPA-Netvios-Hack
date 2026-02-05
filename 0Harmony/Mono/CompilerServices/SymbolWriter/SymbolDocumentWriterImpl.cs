using System;
using System.Diagnostics.SymbolStore;

namespace Mono.CompilerServices.SymbolWriter
{
	// Token: 0x0200021C RID: 540
	internal class SymbolDocumentWriterImpl : ISymbolDocumentWriter, ISourceFile, ICompileUnit
	{
		// Token: 0x0600102E RID: 4142 RVA: 0x000375DE File Offset: 0x000357DE
		public SymbolDocumentWriterImpl(CompileUnitEntry comp_unit)
		{
			this.comp_unit = comp_unit;
		}

		// Token: 0x0600102F RID: 4143 RVA: 0x00010C51 File Offset: 0x0000EE51
		public void SetCheckSum(Guid algorithmId, byte[] checkSum)
		{
		}

		// Token: 0x06001030 RID: 4144 RVA: 0x00010C51 File Offset: 0x0000EE51
		public void SetSource(byte[] source)
		{
		}

		// Token: 0x17000371 RID: 881
		// (get) Token: 0x06001031 RID: 4145 RVA: 0x000375ED File Offset: 0x000357ED
		SourceFileEntry ISourceFile.Entry
		{
			get
			{
				return this.comp_unit.SourceFile;
			}
		}

		// Token: 0x17000372 RID: 882
		// (get) Token: 0x06001032 RID: 4146 RVA: 0x000375FA File Offset: 0x000357FA
		public CompileUnitEntry Entry
		{
			get
			{
				return this.comp_unit;
			}
		}

		// Token: 0x040009FE RID: 2558
		private CompileUnitEntry comp_unit;
	}
}
