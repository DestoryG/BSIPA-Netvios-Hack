using System;
using System.IO;
using Mono.Cecil.Cil;

namespace Mono.Cecil
{
	// Token: 0x02000093 RID: 147
	public sealed class ReaderParameters
	{
		// Token: 0x17000197 RID: 407
		// (get) Token: 0x06000623 RID: 1571 RVA: 0x0001546E File Offset: 0x0001366E
		// (set) Token: 0x06000624 RID: 1572 RVA: 0x00015476 File Offset: 0x00013676
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

		// Token: 0x17000198 RID: 408
		// (get) Token: 0x06000625 RID: 1573 RVA: 0x0001547F File Offset: 0x0001367F
		// (set) Token: 0x06000626 RID: 1574 RVA: 0x00015487 File Offset: 0x00013687
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

		// Token: 0x17000199 RID: 409
		// (get) Token: 0x06000627 RID: 1575 RVA: 0x00015490 File Offset: 0x00013690
		// (set) Token: 0x06000628 RID: 1576 RVA: 0x00015498 File Offset: 0x00013698
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

		// Token: 0x1700019A RID: 410
		// (get) Token: 0x06000629 RID: 1577 RVA: 0x000154A1 File Offset: 0x000136A1
		// (set) Token: 0x0600062A RID: 1578 RVA: 0x000154A9 File Offset: 0x000136A9
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

		// Token: 0x1700019B RID: 411
		// (get) Token: 0x0600062B RID: 1579 RVA: 0x000154B2 File Offset: 0x000136B2
		// (set) Token: 0x0600062C RID: 1580 RVA: 0x000154BA File Offset: 0x000136BA
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

		// Token: 0x1700019C RID: 412
		// (get) Token: 0x0600062D RID: 1581 RVA: 0x000154C3 File Offset: 0x000136C3
		// (set) Token: 0x0600062E RID: 1582 RVA: 0x000154CB File Offset: 0x000136CB
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

		// Token: 0x1700019D RID: 413
		// (get) Token: 0x0600062F RID: 1583 RVA: 0x000154D4 File Offset: 0x000136D4
		// (set) Token: 0x06000630 RID: 1584 RVA: 0x000154DC File Offset: 0x000136DC
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

		// Token: 0x1700019E RID: 414
		// (get) Token: 0x06000631 RID: 1585 RVA: 0x000154E5 File Offset: 0x000136E5
		// (set) Token: 0x06000632 RID: 1586 RVA: 0x000154ED File Offset: 0x000136ED
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

		// Token: 0x1700019F RID: 415
		// (get) Token: 0x06000633 RID: 1587 RVA: 0x000154F6 File Offset: 0x000136F6
		// (set) Token: 0x06000634 RID: 1588 RVA: 0x000154FE File Offset: 0x000136FE
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

		// Token: 0x170001A0 RID: 416
		// (get) Token: 0x06000635 RID: 1589 RVA: 0x00015507 File Offset: 0x00013707
		// (set) Token: 0x06000636 RID: 1590 RVA: 0x0001550F File Offset: 0x0001370F
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

		// Token: 0x170001A1 RID: 417
		// (get) Token: 0x06000637 RID: 1591 RVA: 0x00015518 File Offset: 0x00013718
		// (set) Token: 0x06000638 RID: 1592 RVA: 0x00015520 File Offset: 0x00013720
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

		// Token: 0x170001A2 RID: 418
		// (get) Token: 0x06000639 RID: 1593 RVA: 0x00015529 File Offset: 0x00013729
		// (set) Token: 0x0600063A RID: 1594 RVA: 0x00015531 File Offset: 0x00013731
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

		// Token: 0x0600063B RID: 1595 RVA: 0x0001553A File Offset: 0x0001373A
		public ReaderParameters()
			: this(ReadingMode.Deferred)
		{
		}

		// Token: 0x0600063C RID: 1596 RVA: 0x00015543 File Offset: 0x00013743
		public ReaderParameters(ReadingMode readingMode)
		{
			this.reading_mode = readingMode;
			this.throw_symbols_mismatch = true;
		}

		// Token: 0x04000173 RID: 371
		private ReadingMode reading_mode;

		// Token: 0x04000174 RID: 372
		internal IAssemblyResolver assembly_resolver;

		// Token: 0x04000175 RID: 373
		internal IMetadataResolver metadata_resolver;

		// Token: 0x04000176 RID: 374
		internal IMetadataImporterProvider metadata_importer_provider;

		// Token: 0x04000177 RID: 375
		internal IReflectionImporterProvider reflection_importer_provider;

		// Token: 0x04000178 RID: 376
		private Stream symbol_stream;

		// Token: 0x04000179 RID: 377
		private ISymbolReaderProvider symbol_reader_provider;

		// Token: 0x0400017A RID: 378
		private bool read_symbols;

		// Token: 0x0400017B RID: 379
		private bool throw_symbols_mismatch;

		// Token: 0x0400017C RID: 380
		private bool projections;

		// Token: 0x0400017D RID: 381
		private bool in_memory;

		// Token: 0x0400017E RID: 382
		private bool read_write;
	}
}
