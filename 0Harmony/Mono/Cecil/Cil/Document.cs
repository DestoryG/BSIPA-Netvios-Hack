using System;

namespace Mono.Cecil.Cil
{
	// Token: 0x020001BE RID: 446
	internal sealed class Document : DebugInformation
	{
		// Token: 0x170002D6 RID: 726
		// (get) Token: 0x06000DCB RID: 3531 RVA: 0x0002FE85 File Offset: 0x0002E085
		// (set) Token: 0x06000DCC RID: 3532 RVA: 0x0002FE8D File Offset: 0x0002E08D
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

		// Token: 0x170002D7 RID: 727
		// (get) Token: 0x06000DCD RID: 3533 RVA: 0x0002FE96 File Offset: 0x0002E096
		// (set) Token: 0x06000DCE RID: 3534 RVA: 0x0002FEA3 File Offset: 0x0002E0A3
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

		// Token: 0x170002D8 RID: 728
		// (get) Token: 0x06000DCF RID: 3535 RVA: 0x0002FEB1 File Offset: 0x0002E0B1
		// (set) Token: 0x06000DD0 RID: 3536 RVA: 0x0002FEB9 File Offset: 0x0002E0B9
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

		// Token: 0x170002D9 RID: 729
		// (get) Token: 0x06000DD1 RID: 3537 RVA: 0x0002FEC2 File Offset: 0x0002E0C2
		// (set) Token: 0x06000DD2 RID: 3538 RVA: 0x0002FECF File Offset: 0x0002E0CF
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

		// Token: 0x170002DA RID: 730
		// (get) Token: 0x06000DD3 RID: 3539 RVA: 0x0002FEDD File Offset: 0x0002E0DD
		// (set) Token: 0x06000DD4 RID: 3540 RVA: 0x0002FEE5 File Offset: 0x0002E0E5
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

		// Token: 0x170002DB RID: 731
		// (get) Token: 0x06000DD5 RID: 3541 RVA: 0x0002FEEE File Offset: 0x0002E0EE
		// (set) Token: 0x06000DD6 RID: 3542 RVA: 0x0002FEFB File Offset: 0x0002E0FB
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

		// Token: 0x170002DC RID: 732
		// (get) Token: 0x06000DD7 RID: 3543 RVA: 0x0002FF09 File Offset: 0x0002E109
		// (set) Token: 0x06000DD8 RID: 3544 RVA: 0x0002FF11 File Offset: 0x0002E111
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

		// Token: 0x170002DD RID: 733
		// (get) Token: 0x06000DD9 RID: 3545 RVA: 0x0002FF1A File Offset: 0x0002E11A
		// (set) Token: 0x06000DDA RID: 3546 RVA: 0x0002FF27 File Offset: 0x0002E127
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

		// Token: 0x170002DE RID: 734
		// (get) Token: 0x06000DDB RID: 3547 RVA: 0x0002FF35 File Offset: 0x0002E135
		// (set) Token: 0x06000DDC RID: 3548 RVA: 0x0002FF3D File Offset: 0x0002E13D
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

		// Token: 0x170002DF RID: 735
		// (get) Token: 0x06000DDD RID: 3549 RVA: 0x0002FF46 File Offset: 0x0002E146
		// (set) Token: 0x06000DDE RID: 3550 RVA: 0x0002FF4E File Offset: 0x0002E14E
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

		// Token: 0x170002E0 RID: 736
		// (get) Token: 0x06000DDF RID: 3551 RVA: 0x0002FF57 File Offset: 0x0002E157
		// (set) Token: 0x06000DE0 RID: 3552 RVA: 0x0002FF5F File Offset: 0x0002E15F
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

		// Token: 0x06000DE1 RID: 3553 RVA: 0x0002FF68 File Offset: 0x0002E168
		public Document(string url)
		{
			this.url = url;
			this.hash = Empty<byte>.Array;
			this.embedded_source = Empty<byte>.Array;
			this.token = new MetadataToken(TokenType.Document);
		}

		// Token: 0x04000787 RID: 1927
		private string url;

		// Token: 0x04000788 RID: 1928
		private Guid type;

		// Token: 0x04000789 RID: 1929
		private Guid hash_algorithm;

		// Token: 0x0400078A RID: 1930
		private Guid language;

		// Token: 0x0400078B RID: 1931
		private Guid language_vendor;

		// Token: 0x0400078C RID: 1932
		private byte[] hash;

		// Token: 0x0400078D RID: 1933
		private byte[] embedded_source;
	}
}
