using System;

namespace Mono.Cecil
{
	// Token: 0x020000A4 RID: 164
	public sealed class PInvokeInfo
	{
		// Token: 0x170001F0 RID: 496
		// (get) Token: 0x0600070A RID: 1802 RVA: 0x00016AE4 File Offset: 0x00014CE4
		// (set) Token: 0x0600070B RID: 1803 RVA: 0x00016AEC File Offset: 0x00014CEC
		public PInvokeAttributes Attributes
		{
			get
			{
				return (PInvokeAttributes)this.attributes;
			}
			set
			{
				this.attributes = (ushort)value;
			}
		}

		// Token: 0x170001F1 RID: 497
		// (get) Token: 0x0600070C RID: 1804 RVA: 0x00016AF5 File Offset: 0x00014CF5
		// (set) Token: 0x0600070D RID: 1805 RVA: 0x00016AFD File Offset: 0x00014CFD
		public string EntryPoint
		{
			get
			{
				return this.entry_point;
			}
			set
			{
				this.entry_point = value;
			}
		}

		// Token: 0x170001F2 RID: 498
		// (get) Token: 0x0600070E RID: 1806 RVA: 0x00016B06 File Offset: 0x00014D06
		// (set) Token: 0x0600070F RID: 1807 RVA: 0x00016B0E File Offset: 0x00014D0E
		public ModuleReference Module
		{
			get
			{
				return this.module;
			}
			set
			{
				this.module = value;
			}
		}

		// Token: 0x170001F3 RID: 499
		// (get) Token: 0x06000710 RID: 1808 RVA: 0x00016B17 File Offset: 0x00014D17
		// (set) Token: 0x06000711 RID: 1809 RVA: 0x00016B25 File Offset: 0x00014D25
		public bool IsNoMangle
		{
			get
			{
				return this.attributes.GetAttributes(1);
			}
			set
			{
				this.attributes = this.attributes.SetAttributes(1, value);
			}
		}

		// Token: 0x170001F4 RID: 500
		// (get) Token: 0x06000712 RID: 1810 RVA: 0x00016B3A File Offset: 0x00014D3A
		// (set) Token: 0x06000713 RID: 1811 RVA: 0x00016B49 File Offset: 0x00014D49
		public bool IsCharSetNotSpec
		{
			get
			{
				return this.attributes.GetMaskedAttributes(6, 0U);
			}
			set
			{
				this.attributes = this.attributes.SetMaskedAttributes(6, 0U, value);
			}
		}

		// Token: 0x170001F5 RID: 501
		// (get) Token: 0x06000714 RID: 1812 RVA: 0x00016B5F File Offset: 0x00014D5F
		// (set) Token: 0x06000715 RID: 1813 RVA: 0x00016B6E File Offset: 0x00014D6E
		public bool IsCharSetAnsi
		{
			get
			{
				return this.attributes.GetMaskedAttributes(6, 2U);
			}
			set
			{
				this.attributes = this.attributes.SetMaskedAttributes(6, 2U, value);
			}
		}

		// Token: 0x170001F6 RID: 502
		// (get) Token: 0x06000716 RID: 1814 RVA: 0x00016B84 File Offset: 0x00014D84
		// (set) Token: 0x06000717 RID: 1815 RVA: 0x00016B93 File Offset: 0x00014D93
		public bool IsCharSetUnicode
		{
			get
			{
				return this.attributes.GetMaskedAttributes(6, 4U);
			}
			set
			{
				this.attributes = this.attributes.SetMaskedAttributes(6, 4U, value);
			}
		}

		// Token: 0x170001F7 RID: 503
		// (get) Token: 0x06000718 RID: 1816 RVA: 0x00016BA9 File Offset: 0x00014DA9
		// (set) Token: 0x06000719 RID: 1817 RVA: 0x00016BB8 File Offset: 0x00014DB8
		public bool IsCharSetAuto
		{
			get
			{
				return this.attributes.GetMaskedAttributes(6, 6U);
			}
			set
			{
				this.attributes = this.attributes.SetMaskedAttributes(6, 6U, value);
			}
		}

		// Token: 0x170001F8 RID: 504
		// (get) Token: 0x0600071A RID: 1818 RVA: 0x00016BCE File Offset: 0x00014DCE
		// (set) Token: 0x0600071B RID: 1819 RVA: 0x00016BDD File Offset: 0x00014DDD
		public bool SupportsLastError
		{
			get
			{
				return this.attributes.GetAttributes(64);
			}
			set
			{
				this.attributes = this.attributes.SetAttributes(64, value);
			}
		}

		// Token: 0x170001F9 RID: 505
		// (get) Token: 0x0600071C RID: 1820 RVA: 0x00016BF3 File Offset: 0x00014DF3
		// (set) Token: 0x0600071D RID: 1821 RVA: 0x00016C0A File Offset: 0x00014E0A
		public bool IsCallConvWinapi
		{
			get
			{
				return this.attributes.GetMaskedAttributes(1792, 256U);
			}
			set
			{
				this.attributes = this.attributes.SetMaskedAttributes(1792, 256U, value);
			}
		}

		// Token: 0x170001FA RID: 506
		// (get) Token: 0x0600071E RID: 1822 RVA: 0x00016C28 File Offset: 0x00014E28
		// (set) Token: 0x0600071F RID: 1823 RVA: 0x00016C3F File Offset: 0x00014E3F
		public bool IsCallConvCdecl
		{
			get
			{
				return this.attributes.GetMaskedAttributes(1792, 512U);
			}
			set
			{
				this.attributes = this.attributes.SetMaskedAttributes(1792, 512U, value);
			}
		}

		// Token: 0x170001FB RID: 507
		// (get) Token: 0x06000720 RID: 1824 RVA: 0x00016C5D File Offset: 0x00014E5D
		// (set) Token: 0x06000721 RID: 1825 RVA: 0x00016C74 File Offset: 0x00014E74
		public bool IsCallConvStdCall
		{
			get
			{
				return this.attributes.GetMaskedAttributes(1792, 768U);
			}
			set
			{
				this.attributes = this.attributes.SetMaskedAttributes(1792, 768U, value);
			}
		}

		// Token: 0x170001FC RID: 508
		// (get) Token: 0x06000722 RID: 1826 RVA: 0x00016C92 File Offset: 0x00014E92
		// (set) Token: 0x06000723 RID: 1827 RVA: 0x00016CA9 File Offset: 0x00014EA9
		public bool IsCallConvThiscall
		{
			get
			{
				return this.attributes.GetMaskedAttributes(1792, 1024U);
			}
			set
			{
				this.attributes = this.attributes.SetMaskedAttributes(1792, 1024U, value);
			}
		}

		// Token: 0x170001FD RID: 509
		// (get) Token: 0x06000724 RID: 1828 RVA: 0x00016CC7 File Offset: 0x00014EC7
		// (set) Token: 0x06000725 RID: 1829 RVA: 0x00016CDE File Offset: 0x00014EDE
		public bool IsCallConvFastcall
		{
			get
			{
				return this.attributes.GetMaskedAttributes(1792, 1280U);
			}
			set
			{
				this.attributes = this.attributes.SetMaskedAttributes(1792, 1280U, value);
			}
		}

		// Token: 0x170001FE RID: 510
		// (get) Token: 0x06000726 RID: 1830 RVA: 0x00016CFC File Offset: 0x00014EFC
		// (set) Token: 0x06000727 RID: 1831 RVA: 0x00016D0D File Offset: 0x00014F0D
		public bool IsBestFitEnabled
		{
			get
			{
				return this.attributes.GetMaskedAttributes(48, 16U);
			}
			set
			{
				this.attributes = this.attributes.SetMaskedAttributes(48, 16U, value);
			}
		}

		// Token: 0x170001FF RID: 511
		// (get) Token: 0x06000728 RID: 1832 RVA: 0x00016D25 File Offset: 0x00014F25
		// (set) Token: 0x06000729 RID: 1833 RVA: 0x00016D36 File Offset: 0x00014F36
		public bool IsBestFitDisabled
		{
			get
			{
				return this.attributes.GetMaskedAttributes(48, 32U);
			}
			set
			{
				this.attributes = this.attributes.SetMaskedAttributes(48, 32U, value);
			}
		}

		// Token: 0x17000200 RID: 512
		// (get) Token: 0x0600072A RID: 1834 RVA: 0x00016D4E File Offset: 0x00014F4E
		// (set) Token: 0x0600072B RID: 1835 RVA: 0x00016D65 File Offset: 0x00014F65
		public bool IsThrowOnUnmappableCharEnabled
		{
			get
			{
				return this.attributes.GetMaskedAttributes(12288, 4096U);
			}
			set
			{
				this.attributes = this.attributes.SetMaskedAttributes(12288, 4096U, value);
			}
		}

		// Token: 0x17000201 RID: 513
		// (get) Token: 0x0600072C RID: 1836 RVA: 0x00016D83 File Offset: 0x00014F83
		// (set) Token: 0x0600072D RID: 1837 RVA: 0x00016D9A File Offset: 0x00014F9A
		public bool IsThrowOnUnmappableCharDisabled
		{
			get
			{
				return this.attributes.GetMaskedAttributes(12288, 8192U);
			}
			set
			{
				this.attributes = this.attributes.SetMaskedAttributes(12288, 8192U, value);
			}
		}

		// Token: 0x0600072E RID: 1838 RVA: 0x00016DB8 File Offset: 0x00014FB8
		public PInvokeInfo(PInvokeAttributes attributes, string entryPoint, ModuleReference module)
		{
			this.attributes = (ushort)attributes;
			this.entry_point = entryPoint;
			this.module = module;
		}

		// Token: 0x0400021D RID: 541
		private ushort attributes;

		// Token: 0x0400021E RID: 542
		private string entry_point;

		// Token: 0x0400021F RID: 543
		private ModuleReference module;
	}
}
