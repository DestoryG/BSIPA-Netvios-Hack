using System;
using System.IO;
using System.Reflection;
using Mono.Cecil.Cil;

namespace Mono.Cecil
{
	// Token: 0x0200014D RID: 333
	internal sealed class WriterParameters
	{
		// Token: 0x170001C5 RID: 453
		// (get) Token: 0x060009E8 RID: 2536 RVA: 0x00023EE5 File Offset: 0x000220E5
		// (set) Token: 0x060009E9 RID: 2537 RVA: 0x00023EED File Offset: 0x000220ED
		public uint? Timestamp
		{
			get
			{
				return this.timestamp;
			}
			set
			{
				this.timestamp = value;
			}
		}

		// Token: 0x170001C6 RID: 454
		// (get) Token: 0x060009EA RID: 2538 RVA: 0x00023EF6 File Offset: 0x000220F6
		// (set) Token: 0x060009EB RID: 2539 RVA: 0x00023EFE File Offset: 0x000220FE
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

		// Token: 0x170001C7 RID: 455
		// (get) Token: 0x060009EC RID: 2540 RVA: 0x00023F07 File Offset: 0x00022107
		// (set) Token: 0x060009ED RID: 2541 RVA: 0x00023F0F File Offset: 0x0002210F
		public ISymbolWriterProvider SymbolWriterProvider
		{
			get
			{
				return this.symbol_writer_provider;
			}
			set
			{
				this.symbol_writer_provider = value;
			}
		}

		// Token: 0x170001C8 RID: 456
		// (get) Token: 0x060009EE RID: 2542 RVA: 0x00023F18 File Offset: 0x00022118
		// (set) Token: 0x060009EF RID: 2543 RVA: 0x00023F20 File Offset: 0x00022120
		public bool WriteSymbols
		{
			get
			{
				return this.write_symbols;
			}
			set
			{
				this.write_symbols = value;
			}
		}

		// Token: 0x170001C9 RID: 457
		// (get) Token: 0x060009F0 RID: 2544 RVA: 0x00023F29 File Offset: 0x00022129
		public bool HasStrongNameKey
		{
			get
			{
				return this.key_pair != null || this.key_blob != null || this.key_container != null;
			}
		}

		// Token: 0x170001CA RID: 458
		// (get) Token: 0x060009F1 RID: 2545 RVA: 0x00023F46 File Offset: 0x00022146
		// (set) Token: 0x060009F2 RID: 2546 RVA: 0x00023F4E File Offset: 0x0002214E
		public byte[] StrongNameKeyBlob
		{
			get
			{
				return this.key_blob;
			}
			set
			{
				this.key_blob = value;
			}
		}

		// Token: 0x170001CB RID: 459
		// (get) Token: 0x060009F3 RID: 2547 RVA: 0x00023F57 File Offset: 0x00022157
		// (set) Token: 0x060009F4 RID: 2548 RVA: 0x00023F5F File Offset: 0x0002215F
		public string StrongNameKeyContainer
		{
			get
			{
				return this.key_container;
			}
			set
			{
				this.key_container = value;
			}
		}

		// Token: 0x170001CC RID: 460
		// (get) Token: 0x060009F5 RID: 2549 RVA: 0x00023F68 File Offset: 0x00022168
		// (set) Token: 0x060009F6 RID: 2550 RVA: 0x00023F70 File Offset: 0x00022170
		public StrongNameKeyPair StrongNameKeyPair
		{
			get
			{
				return this.key_pair;
			}
			set
			{
				this.key_pair = value;
			}
		}

		// Token: 0x170001CD RID: 461
		// (get) Token: 0x060009F7 RID: 2551 RVA: 0x00023F79 File Offset: 0x00022179
		// (set) Token: 0x060009F8 RID: 2552 RVA: 0x00023F81 File Offset: 0x00022181
		public bool DeterministicMvid { get; set; }

		// Token: 0x040003A7 RID: 935
		private uint? timestamp;

		// Token: 0x040003A8 RID: 936
		private Stream symbol_stream;

		// Token: 0x040003A9 RID: 937
		private ISymbolWriterProvider symbol_writer_provider;

		// Token: 0x040003AA RID: 938
		private bool write_symbols;

		// Token: 0x040003AB RID: 939
		private byte[] key_blob;

		// Token: 0x040003AC RID: 940
		private string key_container;

		// Token: 0x040003AD RID: 941
		private StrongNameKeyPair key_pair;
	}
}
