using System;

namespace NSpeex
{
	// Token: 0x02000013 RID: 19
	internal class SbDecoder : SbCodec, IDecoder
	{
		// Token: 0x06000074 RID: 116 RVA: 0x00005560 File Offset: 0x00003760
		public SbDecoder(bool ultraWide)
			: base(ultraWide)
		{
			this.stereo = new Stereo();
			this.enhanced = true;
			if (ultraWide)
			{
				this.Uwbinit();
				return;
			}
			this.Wbinit();
		}

		// Token: 0x06000075 RID: 117 RVA: 0x0000558B File Offset: 0x0000378B
		private void Wbinit()
		{
			this.lowdec = new NbDecoder();
			this.lowdec.PerceptualEnhancement = this.enhanced;
			this.Init(160, 40, 8, 640, 0.7f);
		}

		// Token: 0x06000076 RID: 118 RVA: 0x000055C1 File Offset: 0x000037C1
		private void Uwbinit()
		{
			this.lowdec = new SbDecoder(false);
			this.lowdec.PerceptualEnhancement = this.enhanced;
			this.Init(320, 80, 8, 1280, 0.5f);
		}

		// Token: 0x06000077 RID: 119 RVA: 0x000055F8 File Offset: 0x000037F8
		protected override void Init(int frameSize, int subframeSize, int lpcSize, int bufSize, float foldingGain)
		{
			base.Init(frameSize, subframeSize, lpcSize, bufSize, foldingGain);
			this.excIdx = 0;
			this.innov2 = new float[subframeSize];
		}

		// Token: 0x06000078 RID: 120 RVA: 0x0000561C File Offset: 0x0000381C
		public virtual int Decode(Bits bits, float[] xout)
		{
			int num = this.lowdec.Decode(bits, this.x0d);
			if (num != 0)
			{
				return num;
			}
			bool dtx = this.lowdec.Dtx;
			if (bits == null)
			{
				this.DecodeLost(xout, dtx);
				return 0;
			}
			int num2 = bits.Peek();
			if (num2 != 0)
			{
				num2 = bits.Unpack(1);
				this.submodeID = bits.Unpack(3);
			}
			else
			{
				this.submodeID = 0;
			}
			for (int i = 0; i < this.frameSize; i++)
			{
				this.excBuf[i] = 0f;
			}
			if (this.submodes[this.submodeID] != null)
			{
				float[] piGain = this.lowdec.PiGain;
				float[] exc = this.lowdec.Exc;
				float[] innov = this.lowdec.Innov;
				this.submodes[this.submodeID].LsqQuant.Unquant(this.qlsp, this.lpcSize, bits);
				if (this.first != 0)
				{
					for (int i = 0; i < this.lpcSize; i++)
					{
						this.old_qlsp[i] = this.qlsp[i];
					}
				}
				for (int j = 0; j < this.nbSubframes; j++)
				{
					float num3 = 0f;
					float num4 = 0f;
					int num5 = this.subframeSize * j;
					float num6 = (1f + (float)j) / (float)this.nbSubframes;
					for (int i = 0; i < this.lpcSize; i++)
					{
						this.interp_qlsp[i] = (1f - num6) * this.old_qlsp[i] + num6 * this.qlsp[i];
					}
					Lsp.Enforce_margin(this.interp_qlsp, this.lpcSize, 0.05f);
					for (int i = 0; i < this.lpcSize; i++)
					{
						this.interp_qlsp[i] = (float)Math.Cos((double)this.interp_qlsp[i]);
					}
					this.m_lsp.Lsp2lpc(this.interp_qlsp, this.interp_qlpc, this.lpcSize);
					if (this.enhanced)
					{
						float lpcEnhK = this.submodes[this.submodeID].LpcEnhK1;
						float lpcEnhK2 = this.submodes[this.submodeID].LpcEnhK2;
						float num7 = lpcEnhK - lpcEnhK2;
						Filters.Bw_lpc(lpcEnhK, this.interp_qlpc, this.awk1, this.lpcSize);
						Filters.Bw_lpc(lpcEnhK2, this.interp_qlpc, this.awk2, this.lpcSize);
						Filters.Bw_lpc(num7, this.interp_qlpc, this.awk3, this.lpcSize);
					}
					num6 = 1f;
					this.pi_gain[j] = 0f;
					for (int i = 0; i <= this.lpcSize; i++)
					{
						num4 += num6 * this.interp_qlpc[i];
						num6 = -num6;
						this.pi_gain[j] += this.interp_qlpc[i];
					}
					float num8 = piGain[j];
					num8 = 1f / (Math.Abs(num8) + 0.01f);
					num4 = 1f / (Math.Abs(num4) + 0.01f);
					float num9 = Math.Abs(0.01f + num4) / (0.01f + Math.Abs(num8));
					for (int i = num5; i < num5 + this.subframeSize; i++)
					{
						this.excBuf[i] = 0f;
					}
					if (this.submodes[this.submodeID].Innovation == null)
					{
						int num10 = bits.Unpack(5);
						float num11 = (float)Math.Exp(((double)num10 - 10.0) / 8.0);
						num11 /= num9;
						for (int i = num5; i < num5 + this.subframeSize; i++)
						{
							this.excBuf[i] = this.foldingGain * num11 * innov[i];
						}
					}
					else
					{
						int num12 = bits.Unpack(4);
						for (int i = num5; i < num5 + this.subframeSize; i++)
						{
							num3 += exc[i] * exc[i];
						}
						float num13 = (float)Math.Exp((double)(0.27027026f * (float)num12 - 2f));
						float num14 = num13 * (float)Math.Sqrt((double)(1f + num3)) / num9;
						this.submodes[this.submodeID].Innovation.Unquantify(this.excBuf, num5, this.subframeSize, bits);
						for (int i = num5; i < num5 + this.subframeSize; i++)
						{
							this.excBuf[i] *= num14;
						}
						if (this.submodes[this.submodeID].DoubleCodebook != 0)
						{
							for (int i = 0; i < this.subframeSize; i++)
							{
								this.innov2[i] = 0f;
							}
							this.submodes[this.submodeID].Innovation.Unquantify(this.innov2, 0, this.subframeSize, bits);
							for (int i = 0; i < this.subframeSize; i++)
							{
								this.innov2[i] *= num14 * 0.4f;
							}
							for (int i = 0; i < this.subframeSize; i++)
							{
								this.excBuf[num5 + i] += this.innov2[i];
							}
						}
					}
					for (int i = num5; i < num5 + this.subframeSize; i++)
					{
						this.high[i] = this.excBuf[i];
					}
					if (this.enhanced)
					{
						Filters.Filter_mem2(this.high, num5, this.awk2, this.awk1, this.subframeSize, this.lpcSize, this.mem_sp, this.lpcSize);
						Filters.Filter_mem2(this.high, num5, this.awk3, this.interp_qlpc, this.subframeSize, this.lpcSize, this.mem_sp, 0);
					}
					else
					{
						for (int i = 0; i < this.lpcSize; i++)
						{
							this.mem_sp[this.lpcSize + i] = 0f;
						}
						Filters.Iir_mem2(this.high, num5, this.interp_qlpc, this.high, num5, this.subframeSize, this.lpcSize, this.mem_sp);
					}
				}
				this.filters.Fir_mem_up(this.x0d, Codebook_Constants.h0, this.y0, this.fullFrameSize, 64, this.g0_mem);
				this.filters.Fir_mem_up(this.high, Codebook_Constants.h1, this.y1, this.fullFrameSize, 64, this.g1_mem);
				for (int i = 0; i < this.fullFrameSize; i++)
				{
					xout[i] = 2f * (this.y0[i] - this.y1[i]);
				}
				for (int i = 0; i < this.lpcSize; i++)
				{
					this.old_qlsp[i] = this.qlsp[i];
				}
				this.first = 0;
				return 0;
			}
			if (dtx)
			{
				this.DecodeLost(xout, true);
				return 0;
			}
			for (int i = 0; i < this.frameSize; i++)
			{
				this.excBuf[i] = 0f;
			}
			this.first = 1;
			Filters.Iir_mem2(this.excBuf, this.excIdx, this.interp_qlpc, this.high, 0, this.frameSize, this.lpcSize, this.mem_sp);
			this.filters.Fir_mem_up(this.x0d, Codebook_Constants.h0, this.y0, this.fullFrameSize, 64, this.g0_mem);
			this.filters.Fir_mem_up(this.high, Codebook_Constants.h1, this.y1, this.fullFrameSize, 64, this.g1_mem);
			for (int i = 0; i < this.fullFrameSize; i++)
			{
				xout[i] = 2f * (this.y0[i] - this.y1[i]);
			}
			return 0;
		}

		// Token: 0x06000079 RID: 121 RVA: 0x00005D9C File Offset: 0x00003F9C
		public int DecodeLost(float[] xout, bool dtx)
		{
			int num = 0;
			if (dtx)
			{
				num = this.submodeID;
				this.submodeID = 1;
			}
			else
			{
				Filters.Bw_lpc(0.99f, this.interp_qlpc, this.interp_qlpc, this.lpcSize);
			}
			this.first = 1;
			this.awk1 = new float[this.lpcSize + 1];
			this.awk2 = new float[this.lpcSize + 1];
			this.awk3 = new float[this.lpcSize + 1];
			if (this.enhanced)
			{
				float num2;
				float num3;
				if (this.submodes[this.submodeID] != null)
				{
					num2 = this.submodes[this.submodeID].LpcEnhK1;
					num3 = this.submodes[this.submodeID].LpcEnhK2;
				}
				else
				{
					num3 = (num2 = 0.7f);
				}
				float num4 = num2 - num3;
				Filters.Bw_lpc(num2, this.interp_qlpc, this.awk1, this.lpcSize);
				Filters.Bw_lpc(num3, this.interp_qlpc, this.awk2, this.lpcSize);
				Filters.Bw_lpc(num4, this.interp_qlpc, this.awk3, this.lpcSize);
			}
			if (!dtx)
			{
				for (int i = 0; i < this.frameSize; i++)
				{
					this.excBuf[this.excIdx + i] *= new float?(0.9f).Value;
				}
			}
			for (int i = 0; i < this.frameSize; i++)
			{
				this.high[i] = this.excBuf[this.excIdx + i];
			}
			if (this.enhanced)
			{
				Filters.Filter_mem2(this.high, 0, this.awk2, this.awk1, this.high, 0, this.frameSize, this.lpcSize, this.mem_sp, this.lpcSize);
				Filters.Filter_mem2(this.high, 0, this.awk3, this.interp_qlpc, this.high, 0, this.frameSize, this.lpcSize, this.mem_sp, 0);
			}
			else
			{
				for (int i = 0; i < this.lpcSize; i++)
				{
					this.mem_sp[this.lpcSize + i] = 0f;
				}
				Filters.Iir_mem2(this.high, 0, this.interp_qlpc, this.high, 0, this.frameSize, this.lpcSize, this.mem_sp);
			}
			this.filters.Fir_mem_up(this.x0d, Codebook_Constants.h0, this.y0, this.fullFrameSize, 64, this.g0_mem);
			this.filters.Fir_mem_up(this.high, Codebook_Constants.h1, this.y1, this.fullFrameSize, 64, this.g1_mem);
			for (int i = 0; i < this.fullFrameSize; i++)
			{
				xout[i] = 2f * (this.y0[i] - this.y1[i]);
			}
			if (dtx)
			{
				this.submodeID = num;
			}
			return 0;
		}

		// Token: 0x0600007A RID: 122 RVA: 0x0000606D File Offset: 0x0000426D
		public virtual void DecodeStereo(float[] data, int frameSize)
		{
			this.stereo.Decode(data, frameSize);
		}

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x0600007B RID: 123 RVA: 0x0000607C File Offset: 0x0000427C
		// (set) Token: 0x0600007C RID: 124 RVA: 0x00006084 File Offset: 0x00004284
		public virtual bool PerceptualEnhancement
		{
			get
			{
				return this.enhanced;
			}
			set
			{
				this.enhanced = value;
			}
		}

		// Token: 0x04000078 RID: 120
		protected internal IDecoder lowdec;

		// Token: 0x04000079 RID: 121
		protected internal Stereo stereo;

		// Token: 0x0400007A RID: 122
		protected internal bool enhanced;

		// Token: 0x0400007B RID: 123
		private float[] innov2;
	}
}
