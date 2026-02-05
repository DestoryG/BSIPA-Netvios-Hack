using System;
using System.IO;
using Mono.Cecil.Cil;

namespace Mono.Cecil
{
	// Token: 0x0200014B RID: 331
	internal sealed class ReaderParameters
	{
		// Token: 0x170001B1 RID: 433
		// (get) Token: 0x060009BC RID: 2492 RVA: 0x00023D32 File Offset: 0x00021F32
		// (set) Token: 0x060009BD RID: 2493 RVA: 0x00023D3A File Offset: 0x00021F3A
		public ReadingMode ReadingMode
		{
			get
			{
				return this.reading_mode;
			}
			set
			{
				this.reading_mode = value;
			}
		}

		// Token: 0x170001B2 RID: 434
		// (get) Token: 0x060009BE RID: 2494 RVA: 0x00023D43 File Offset: 0x00021F43
		// (set) Token: 0x060009BF RID: 2495 RVA: 0x00023D4B File Offset: 0x00021F4B
		public bool InMemory
		{
			get
			{
				return this.in_memory;
			}
			set
			{
				this.in_memory = value;
			}
		}

		// Token: 0x170001B3 RID: 435
		// (get) Token: 0x060009C0 RID: 2496 RVA: 0x00023D54 File Offset: 0x00021F54
		// (set) Token: 0x060009C1 RID: 2497 RVA: 0x00023D5C File Offset: 0x00021F5C
		public IAssemblyResolver AssemblyResolver
		{
			get
			{
				return this.assembly_resolver;
			}
			set
			{
				this.assembly_resolver = value;
			}
		}

		// Token: 0x170001B4 RID: 436
		// (get) Token: 0x060009C2 RID: 2498 RVA: 0x00023D65 File Offset: 0x00021F65
		// (set) Token: 0x060009C3 RID: 2499 RVA: 0x00023D6D File Offset: 0x00021F6D
		public IMetadataResolver MetadataResolver
		{
			get
			{
				return this.metadata_resolver;
			}
			set
			{
				this.metadata_resolver = value;
			}
		}

		// Token: 0x170001B5 RID: 437
		// (get) Token: 0x060009C4 RID: 2500 RVA: 0x00023D76 File Offset: 0x00021F76
		// (set) Token: 0x060009C5 RID: 2501 RVA: 0x00023D7E File Offset: 0x00021F7E
		public IMetadataImporterProvider MetadataImporterProvider
		{
			get
			{
				return this.metadata_importer_provider;
			}
			set
			{
				this.metadata_importer_provider = value;
			}
		}

		// Token: 0x170001B6 RID: 438
		// (get) Token: 0x060009C6 RID: 2502 RVA: 0x00023D87 File Offset: 0x00021F87
		// (set) Token: 0x060009C7 RID: 2503 RVA: 0x00023D8F File Offset: 0x00021F8F
		public IReflectionImporterProvider ReflectionImporterProvider
		{
			get
			{
				return this.reflection_importer_provider;
			}
			set
			{
				this.reflection_importer_provider = value;
			}
		}

		// Token: 0x170001B7 RID: 439
		// (get) Token: 0x060009C8 RID: 2504 RVA: 0x00023D98 File Offset: 0x00021F98
		// (set) Token: 0x060009C9 RID: 2505 RVA: 0x00023DA0 File Offset: 0x00021FA0
		public Stream SymbolStream
		{
			get
			{
				return this.symbol_stream;
			}
			set
			{
				this.symbol_stream = value;
			}
		}

		// Token: 0x170001B8 RID: 440
		// (get) Token: 0x060009CA RID: 2506 RVA: 0x00023DA9 File Offset: 0x00021FA9
		// (set) Token: 0x060009CB RID: 2507 RVA: 0x00023DB1 File Offset: 0x00021FB1
		public ISymbolReaderProvider SymbolReaderProvider
		{
			get
			{
				return this.symbol_reader_provider;
			}
			set
			{
				this.symbol_reader_provider = value;
			}
		}

		// Token: 0x170001B9 RID: 441
		// (get) Token: 0x060009CC RID: 2508 RVA: 0x00023DBA File Offset: 0x00021FBA
		// (set) Token: 0x060009CD RID: 2509 RVA: 0x00023DC2 File Offset: 0x00021FC2
		public bool ReadSymbols
		{
			get
			{
				return this.read_symbols;
			}
			set
			{
				this.read_symbols = value;
			}
		}

		// Token: 0x170001BA RID: 442
		// (get) Token: 0x060009CE RID: 2510 RVA: 0x00023DCB File Offset: 0x00021FCB
		// (set) Token: 0x060009CF RID: 2511 RVA: 0x00023DD3 File Offset: 0x00021FD3
		public bool ThrowIfSymbolsAreNotMatching
		{
			get
			{
				return this.throw_symbols_mismatch;
			}
			set
			{
				this.throw_symbols_mismatch = value;
			}
		}

		// Token: 0x170001BB RID: 443
		// (get) Token: 0x060009D0 RID: 2512 RVA: 0x00023DDC File Offset: 0x00021FDC
		// (set) Token: 0x060009D1 RID: 2513 RVA: 0x00023DE4 File Offset: 0x00021FE4
		public bool ReadWrite
		{
			get
			{
				return this.read_write;
			}
			set
			{
				this.read_write = value;
			}
		}

		// Token: 0x170001BC RID: 444
		// (get) Token: 0x060009D2 RID: 2514 RVA: 0x00023DED File Offset: 0x00021FED
		// (set) Token: 0x060009D3 RID: 2515 RVA: 0x00023DF5 File Offset: 0x00021FF5
		public bool ApplyWindowsRuntimeProjections
		{
			get
			{
				return this.projections;
			}
			set
			{
				this.projections = value;
			}
		}

		// Token: 0x060009D4 RID: 2516 RVA: 0x00023DFE File Offset: 0x00021FFE
		public ReaderParameters()
			: this(ReadingMode.Deferred)
		{
		}

		// Token: 0x060009D5 RID: 2517 RVA: 0x00023E07 File Offset: 0x00022007
		public ReaderParameters(ReadingMode readingMode)
		{
			this.reading_mode = readingMode;
			this.throw_symbols_mismatch = true;
		}

		// Token: 0x04000393 RID: 915
		private ReadingMode reading_mode;

		// Token: 0x04000394 RID: 916
		internal IAssemblyResolver assembly_resolver;

		// Token: 0x04000395 RID: 917
		internal IMetadataResolver metadata_resolver;

		// Token: 0x04000396 RID: 918
		internal IMetadataImporterProvider metadata_importer_provider;

		// Token: 0x04000397 RID: 919
		internal IReflectionImporterProvider reflection_importer_provider;

		// Token: 0x04000398 RID: 920
		private Stream symbol_stream;

		// Token: 0x04000399 RID: 921
		private ISymbolReaderProvider symbol_reader_provider;

		// Token: 0x0400039A RID: 922
		private bool read_symbols;

		// Token: 0x0400039B RID: 923
		private bool throw_symbols_mismatch;

		// Token: 0x0400039C RID: 924
		private bool projections;

		// Token: 0x0400039D RID: 925
		private bool in_memory;

		// Token: 0x0400039E RID: 926
		private bool read_write;
	}
}
