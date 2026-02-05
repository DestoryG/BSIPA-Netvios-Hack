using System;

namespace Mono.Cecil
{
	// Token: 0x0200015E RID: 350
	internal sealed class PInvokeInfo
	{
		// Token: 0x17000210 RID: 528
		// (get) Token: 0x06000AC1 RID: 2753 RVA: 0x000256B4 File Offset: 0x000238B4
		// (set) Token: 0x06000AC2 RID: 2754 RVA: 0x000256BC File Offset: 0x000238BC
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

		// Token: 0x17000211 RID: 529
		// (get) Token: 0x06000AC3 RID: 2755 RVA: 0x000256C5 File Offset: 0x000238C5
		// (set) Token: 0x06000AC4 RID: 2756 RVA: 0x000256CD File Offset: 0x000238CD
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

		// Token: 0x17000212 RID: 530
		// (get) Token: 0x06000AC5 RID: 2757 RVA: 0x000256D6 File Offset: 0x000238D6
		// (set) Token: 0x06000AC6 RID: 2758 RVA: 0x000256DE File Offset: 0x000238DE
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

		// Token: 0x17000213 RID: 531
		// (get) Token: 0x06000AC7 RID: 2759 RVA: 0x000256E7 File Offset: 0x000238E7
		// (set) Token: 0x06000AC8 RID: 2760 RVA: 0x000256F5 File Offset: 0x000238F5
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

		// Token: 0x17000214 RID: 532
		// (get) Token: 0x06000AC9 RID: 2761 RVA: 0x0002570A File Offset: 0x0002390A
		// (set) Token: 0x06000ACA RID: 2762 RVA: 0x00025719 File Offset: 0x00023919
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

		// Token: 0x17000215 RID: 533
		// (get) Token: 0x06000ACB RID: 2763 RVA: 0x0002572F File Offset: 0x0002392F
		// (set) Token: 0x06000ACC RID: 2764 RVA: 0x0002573E File Offset: 0x0002393E
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

		// Token: 0x17000216 RID: 534
		// (get) Token: 0x06000ACD RID: 2765 RVA: 0x00025754 File Offset: 0x00023954
		// (set) Token: 0x06000ACE RID: 2766 RVA: 0x00025763 File Offset: 0x00023963
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

		// Token: 0x17000217 RID: 535
		// (get) Token: 0x06000ACF RID: 2767 RVA: 0x00025779 File Offset: 0x00023979
		// (set) Token: 0x06000AD0 RID: 2768 RVA: 0x00025788 File Offset: 0x00023988
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

		// Token: 0x17000218 RID: 536
		// (get) Token: 0x06000AD1 RID: 2769 RVA: 0x0002579E File Offset: 0x0002399E
		// (set) Token: 0x06000AD2 RID: 2770 RVA: 0x000257AD File Offset: 0x000239AD
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

		// Token: 0x17000219 RID: 537
		// (get) Token: 0x06000AD3 RID: 2771 RVA: 0x000257C3 File Offset: 0x000239C3
		// (set) Token: 0x06000AD4 RID: 2772 RVA: 0x000257DA File Offset: 0x000239DA
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

		// Token: 0x1700021A RID: 538
		// (get) Token: 0x06000AD5 RID: 2773 RVA: 0x000257F8 File Offset: 0x000239F8
		// (set) Token: 0x06000AD6 RID: 2774 RVA: 0x0002580F File Offset: 0x00023A0F
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

		// Token: 0x1700021B RID: 539
		// (get) Token: 0x06000AD7 RID: 2775 RVA: 0x0002582D File Offset: 0x00023A2D
		// (set) Token: 0x06000AD8 RID: 2776 RVA: 0x00025844 File Offset: 0x00023A44
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

		// Token: 0x1700021C RID: 540
		// (get) Token: 0x06000AD9 RID: 2777 RVA: 0x00025862 File Offset: 0x00023A62
		// (set) Token: 0x06000ADA RID: 2778 RVA: 0x00025879 File Offset: 0x00023A79
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

		// Token: 0x1700021D RID: 541
		// (get) Token: 0x06000ADB RID: 2779 RVA: 0x00025897 File Offset: 0x00023A97
		// (set) Token: 0x06000ADC RID: 2780 RVA: 0x000258AE File Offset: 0x00023AAE
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

		// Token: 0x1700021E RID: 542
		// (get) Token: 0x06000ADD RID: 2781 RVA: 0x000258CC File Offset: 0x00023ACC
		// (set) Token: 0x06000ADE RID: 2782 RVA: 0x000258DD File Offset: 0x00023ADD
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

		// Token: 0x1700021F RID: 543
		// (get) Token: 0x06000ADF RID: 2783 RVA: 0x000258F5 File Offset: 0x00023AF5
		// (set) Token: 0x06000AE0 RID: 2784 RVA: 0x00025906 File Offset: 0x00023B06
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

		// Token: 0x17000220 RID: 544
		// (get) Token: 0x06000AE1 RID: 2785 RVA: 0x0002591E File Offset: 0x00023B1E
		// (set) Token: 0x06000AE2 RID: 2786 RVA: 0x00025935 File Offset: 0x00023B35
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

		// Token: 0x17000221 RID: 545
		// (get) Token: 0x06000AE3 RID: 2787 RVA: 0x00025953 File Offset: 0x00023B53
		// (set) Token: 0x06000AE4 RID: 2788 RVA: 0x0002596A File Offset: 0x00023B6A
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

		// Token: 0x06000AE5 RID: 2789 RVA: 0x00025988 File Offset: 0x00023B88
		public PInvokeInfo(PInvokeAttributes attributes, string entryPoint, ModuleReference module)
		{
			this.attributes = (ushort)attributes;
			this.entry_point = entryPoint;
			this.module = module;
		}

		// Token: 0x04000455 RID: 1109
		private ushort attributes;

		// Token: 0x04000456 RID: 1110
		private string entry_point;

		// Token: 0x04000457 RID: 1111
		private ModuleReference module;
	}
}
