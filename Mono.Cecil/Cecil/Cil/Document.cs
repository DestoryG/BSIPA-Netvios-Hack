using System;

namespace Mono.Cecil.Cil
{
	// Token: 0x020000FA RID: 250
	public sealed class Document : DebugInformation
	{
		// Token: 0x170002B6 RID: 694
		// (get) Token: 0x060009E7 RID: 2535 RVA: 0x00020CF9 File Offset: 0x0001EEF9
		// (set) Token: 0x060009E8 RID: 2536 RVA: 0x00020D01 File Offset: 0x0001EF01
		public string Url
		{
			get
			{
				return this.url;
			}
			set
			{
				this.url = value;
			}
		}

		// Token: 0x170002B7 RID: 695
		// (get) Token: 0x060009E9 RID: 2537 RVA: 0x00020D0A File Offset: 0x0001EF0A
		// (set) Token: 0x060009EA RID: 2538 RVA: 0x00020D17 File Offset: 0x0001EF17
		public DocumentType Type
		{
			get
			{
				return this.type.ToType();
			}
			set
			{
				this.type = value.ToGuid();
			}
		}

		// Token: 0x170002B8 RID: 696
		// (get) Token: 0x060009EB RID: 2539 RVA: 0x00020D25 File Offset: 0x0001EF25
		// (set) Token: 0x060009EC RID: 2540 RVA: 0x00020D2D File Offset: 0x0001EF2D
		public Guid TypeGuid
		{
			get
			{
				return this.type;
			}
			set
			{
				this.type = value;
			}
		}

		// Token: 0x170002B9 RID: 697
		// (get) Token: 0x060009ED RID: 2541 RVA: 0x00020D36 File Offset: 0x0001EF36
		// (set) Token: 0x060009EE RID: 2542 RVA: 0x00020D43 File Offset: 0x0001EF43
		public DocumentHashAlgorithm HashAlgorithm
		{
			get
			{
				return this.hash_algorithm.ToHashAlgorithm();
			}
			set
			{
				this.hash_algorithm = value.ToGuid();
			}
		}

		// Token: 0x170002BA RID: 698
		// (get) Token: 0x060009EF RID: 2543 RVA: 0x00020D51 File Offset: 0x0001EF51
		// (set) Token: 0x060009F0 RID: 2544 RVA: 0x00020D59 File Offset: 0x0001EF59
		public Guid HashAlgorithmGuid
		{
			get
			{
				return this.hash_algorithm;
			}
			set
			{
				this.hash_algorithm = value;
			}
		}

		// Token: 0x170002BB RID: 699
		// (get) Token: 0x060009F1 RID: 2545 RVA: 0x00020D62 File Offset: 0x0001EF62
		// (set) Token: 0x060009F2 RID: 2546 RVA: 0x00020D6F File Offset: 0x0001EF6F
		public DocumentLanguage Language
		{
			get
			{
				return this.language.ToLanguage();
			}
			set
			{
				this.language = value.ToGuid();
			}
		}

		// Token: 0x170002BC RID: 700
		// (get) Token: 0x060009F3 RID: 2547 RVA: 0x00020D7D File Offset: 0x0001EF7D
		// (set) Token: 0x060009F4 RID: 2548 RVA: 0x00020D85 File Offset: 0x0001EF85
		public Guid LanguageGuid
		{
			get
			{
				return this.language;
			}
			set
			{
				this.language = value;
			}
		}

		// Token: 0x170002BD RID: 701
		// (get) Token: 0x060009F5 RID: 2549 RVA: 0x00020D8E File Offset: 0x0001EF8E
		// (set) Token: 0x060009F6 RID: 2550 RVA: 0x00020D9B File Offset: 0x0001EF9B
		public DocumentLanguageVendor LanguageVendor
		{
			get
			{
				return this.language_vendor.ToVendor();
			}
			set
			{
				this.language_vendor = value.ToGuid();
			}
		}

		// Token: 0x170002BE RID: 702
		// (get) Token: 0x060009F7 RID: 2551 RVA: 0x00020DA9 File Offset: 0x0001EFA9
		// (set) Token: 0x060009F8 RID: 2552 RVA: 0x00020DB1 File Offset: 0x0001EFB1
		public Guid LanguageVendorGuid
		{
			get
			{
				return this.language_vendor;
			}
			set
			{
				this.language_vendor = value;
			}
		}

		// Token: 0x170002BF RID: 703
		// (get) Token: 0x060009F9 RID: 2553 RVA: 0x00020DBA File Offset: 0x0001EFBA
		// (set) Token: 0x060009FA RID: 2554 RVA: 0x00020DC2 File Offset: 0x0001EFC2
		public byte[] Hash
		{
			get
			{
				return this.hash;
			}
			set
			{
				this.hash = value;
			}
		}

		// Token: 0x170002C0 RID: 704
		// (get) Token: 0x060009FB RID: 2555 RVA: 0x00020DCB File Offset: 0x0001EFCB
		// (set) Token: 0x060009FC RID: 2556 RVA: 0x00020DD3 File Offset: 0x0001EFD3
		public byte[] EmbeddedSource
		{
			get
			{
				return this.embedded_source;
			}
			set
			{
				this.embedded_source = value;
			}
		}

		// Token: 0x060009FD RID: 2557 RVA: 0x00020DDC File Offset: 0x0001EFDC
		public Document(string url)
		{
			this.url = url;
			this.hash = Empty<byte>.Array;
			this.embedded_source = Empty<byte>.Array;
			this.token = new MetadataToken(TokenType.Document);
		}

		// Token: 0x04000528 RID: 1320
		private string url;

		// Token: 0x04000529 RID: 1321
		private Guid type;

		// Token: 0x0400052A RID: 1322
		private Guid hash_algorithm;

		// Token: 0x0400052B RID: 1323
		private Guid language;

		// Token: 0x0400052C RID: 1324
		private Guid language_vendor;

		// Token: 0x0400052D RID: 1325
		private byte[] hash;

		// Token: 0x0400052E RID: 1326
		private byte[] embedded_source;
	}
}
