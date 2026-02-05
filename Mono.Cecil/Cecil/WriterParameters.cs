using System;
using System.IO;
using System.Reflection;
using Mono.Cecil.Cil;

namespace Mono.Cecil
{
	// Token: 0x02000095 RID: 149
	public sealed class WriterParameters
	{
		// Token: 0x170001AB RID: 427
		// (get) Token: 0x0600064F RID: 1615 RVA: 0x00015621 File Offset: 0x00013821
		// (set) Token: 0x06000650 RID: 1616 RVA: 0x00015629 File Offset: 0x00013829
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

		// Token: 0x170001AC RID: 428
		// (get) Token: 0x06000651 RID: 1617 RVA: 0x00015632 File Offset: 0x00013832
		// (set) Token: 0x06000652 RID: 1618 RVA: 0x0001563A File Offset: 0x0001383A
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

		// Token: 0x170001AD RID: 429
		// (get) Token: 0x06000653 RID: 1619 RVA: 0x00015643 File Offset: 0x00013843
		// (set) Token: 0x06000654 RID: 1620 RVA: 0x0001564B File Offset: 0x0001384B
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

		// Token: 0x170001AE RID: 430
		// (get) Token: 0x06000655 RID: 1621 RVA: 0x00015654 File Offset: 0x00013854
		// (set) Token: 0x06000656 RID: 1622 RVA: 0x0001565C File Offset: 0x0001385C
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

		// Token: 0x170001AF RID: 431
		// (get) Token: 0x06000657 RID: 1623 RVA: 0x00015665 File Offset: 0x00013865
		// (set) Token: 0x06000658 RID: 1624 RVA: 0x0001566D File Offset: 0x0001386D
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

		// Token: 0x04000187 RID: 391
		private uint? timestamp;

		// Token: 0x04000188 RID: 392
		private Stream symbol_stream;

		// Token: 0x04000189 RID: 393
		private ISymbolWriterProvider symbol_writer_provider;

		// Token: 0x0400018A RID: 394
		private bool write_symbols;

		// Token: 0x0400018B RID: 395
		private StrongNameKeyPair key_pair;
	}
}
