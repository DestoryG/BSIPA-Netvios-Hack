using System;

namespace NSpeex
{
	// Token: 0x02000027 RID: 39
	internal class SbEncoder : SbCodec, IEncoder
	{
		// Token: 0x060000F5 RID: 245 RVA: 0x00013112 File Offset: 0x00011312
		public SbEncoder(bool ultraWide)
			: base(ultraWide)
		{
			if (ultraWide)
			{
				this.Uwbinit();
				return;
			}
			this.Wbinit();
		}

		// Token: 0x060000F6 RID: 246 RVA: 0x0001312B File Offset: 0x0001132B
		private void Wbinit()
		{
			this.lowenc = new NbEncoder();
			this.Init(160, 40, 8, 640, 0.9f);
			this.uwb = false;
			this.nb_modes = 5;
			this.sampling_rate = 16000;
		}

		// Token: 0x060000F7 RID: 247 RVA: 0x00013169 File Offset: 0x00011369
		private void Uwbinit()
		{
			this.lowenc = new SbEncoder(false);
			this.Init(320, 80, 8, 1280, 0.7f);
			this.uwb = true;
			this.nb_modes = 2;
			this.sampling_rate = 32000;
		}

		// Token: 0x060000F8 RID: 248 RVA: 0x000131A8 File Offset: 0x000113A8
		protected override void Init(int frameSize, int subframeSize, int lpcSize, int bufSize, float foldingGain)
		{
			base.Init(frameSize, subframeSize, lpcSize, bufSize, foldingGain);
			this.complexity = 3;
			this.vbr_enabled = 0;
			this.vad_enabled = 0;
			this.abr_enabled = 0;
			this.vbr_quality = 8f;
			this.submodeSelect = this.submodeID;
			this.x1d = new float[frameSize];
			this.h0_mem = new float[64];
			this.buf = new float[this.windowSize];
			this.swBuf = new float[frameSize];
			this.res = new float[frameSize];
			this.target = new float[subframeSize];
			this.window = Misc.Window(this.windowSize, subframeSize);
			this.lagWindow = Misc.LagWindow(lpcSize, this.lag_factor);
			this.rc = new float[lpcSize];
			this.autocorr = new float[lpcSize + 1];
			this.lsp = new float[lpcSize];
			this.old_lsp = new float[lpcSize];
			this.interp_lsp = new float[lpcSize];
			this.interp_lpc = new float[lpcSize + 1];
			this.bw_lpc1 = new float[lpcSize + 1];
			this.bw_lpc2 = new float[lpcSize + 1];
			this.mem_sp2 = new float[lpcSize];
			this.mem_sw = new float[lpcSize];
			this.abr_count = 0f;
		}

		// Token: 0x060000F9 RID: 249 RVA: 0x000132F4 File Offset: 0x000114F4
		public virtual int Encode(Bits bits, float[] ins0)
		{
			Filters.Qmf_decomp(ins0, Codebook_Constants.h0, this.x0d, this.x1d, this.fullFrameSize, 64, this.h0_mem);
			this.lowenc.Encode(bits, this.x0d);
			for (int i = 0; i < this.windowSize - this.frameSize; i++)
			{
				this.high[i] = this.high[this.frameSize + i];
			}
			for (int i = 0; i < this.frameSize; i++)
			{
				this.high[this.windowSize - this.frameSize + i] = this.x1d[i];
			}
			Array.Copy(this.excBuf, this.frameSize, this.excBuf, 0, this.bufSize - this.frameSize);
			float[] piGain = this.lowenc.PiGain;
			float[] exc = this.lowenc.Exc;
			float[] innov = this.lowenc.Innov;
			int num;
			if (this.lowenc.Mode == 0)
			{
				num = 1;
			}
			else
			{
				num = 0;
			}
			for (int i = 0; i < this.windowSize; i++)
			{
				this.buf[i] = this.high[i] * this.window[i];
			}
			Lpc.Autocorr(this.buf, this.autocorr, this.lpcSize + 1, this.windowSize);
			this.autocorr[0] += 1f;
			this.autocorr[0] *= this.lpc_floor;
			for (int i = 0; i < this.lpcSize + 1; i++)
			{
				this.autocorr[i] *= this.lagWindow[i];
			}
			Lpc.Wld(this.lpc, this.autocorr, this.rc, this.lpcSize);
			Array.Copy(this.lpc, 0, this.lpc, 1, this.lpcSize);
			this.lpc[0] = 1f;
			int num2 = Lsp.Lpc2lsp(this.lpc, this.lpcSize, this.lsp, 15, 0.2f);
			if (num2 != this.lpcSize)
			{
				num2 = Lsp.Lpc2lsp(this.lpc, this.lpcSize, this.lsp, 11, 0.02f);
				if (num2 != this.lpcSize)
				{
					for (int i = 0; i < this.lpcSize; i++)
					{
						this.lsp[i] = (float)Math.Cos(3.141592653589793 * (double)((float)(i + 1)) / (double)(this.lpcSize + 1));
					}
				}
			}
			for (int i = 0; i < this.lpcSize; i++)
			{
				this.lsp[i] = (float)Math.Acos((double)this.lsp[i]);
			}
			float num3 = 0f;
			for (int i = 0; i < this.lpcSize; i++)
			{
				num3 += (this.old_lsp[i] - this.lsp[i]) * (this.old_lsp[i] - this.lsp[i]);
			}
			if ((this.vbr_enabled != 0 || this.vad_enabled != 0) && num == 0)
			{
				float num4 = 0f;
				float num5 = 0f;
				if (this.abr_enabled != 0)
				{
					float num6 = 0f;
					if (this.abr_drift2 * this.abr_drift > 0f)
					{
						num6 = -1E-05f * this.abr_drift / (1f + this.abr_count);
						if (num6 > 0.1f)
						{
							num6 = 0.1f;
						}
						if (num6 < -0.1f)
						{
							num6 = -0.1f;
						}
					}
					this.vbr_quality += num6;
					if (this.vbr_quality > 10f)
					{
						this.vbr_quality = 10f;
					}
					if (this.vbr_quality < 0f)
					{
						this.vbr_quality = 0f;
					}
				}
				for (int i = 0; i < this.frameSize; i++)
				{
					num4 += this.x0d[i] * this.x0d[i];
					num5 += this.high[i] * this.high[i];
				}
				float num7 = (float)Math.Log((double)((1f + num5) / (1f + num4)));
				this.relative_quality = this.lowenc.RelativeQuality;
				if (num7 < -4f)
				{
					num7 = -4f;
				}
				if (num7 > 2f)
				{
					num7 = 2f;
				}
				if (this.vbr_enabled != 0)
				{
					int num8 = this.nb_modes - 1;
					this.relative_quality += 1f * (num7 + 2f);
					if (this.relative_quality < -1f)
					{
						this.relative_quality = -1f;
					}
					while (num8 != 0)
					{
						int num9 = (int)Math.Floor((double)this.vbr_quality);
						float num10;
						if (num9 == 10)
						{
							num10 = NSpeex.Vbr.hb_thresh[num8][num9];
						}
						else
						{
							num10 = (this.vbr_quality - (float)num9) * NSpeex.Vbr.hb_thresh[num8][num9 + 1] + ((float)(1 + num9) - this.vbr_quality) * NSpeex.Vbr.hb_thresh[num8][num9];
						}
						if (this.relative_quality >= num10)
						{
							break;
						}
						num8--;
					}
					this.Mode = num8;
					if (this.abr_enabled != 0)
					{
						int bitRate = this.BitRate;
						this.abr_drift += (float)(bitRate - this.abr_enabled);
						this.abr_drift2 = 0.95f * this.abr_drift2 + 0.05f * (float)(bitRate - this.abr_enabled);
						this.abr_count += 1f;
					}
				}
				else
				{
					int num11;
					if ((double)this.relative_quality < 2.0)
					{
						num11 = 1;
					}
					else
					{
						num11 = this.submodeSelect;
					}
					this.submodeID = num11;
				}
			}
			bits.Pack(1, 1);
			if (num != 0)
			{
				bits.Pack(0, 3);
			}
			else
			{
				bits.Pack(this.submodeID, 3);
			}
			if (num == 0 && this.submodes[this.submodeID] != null)
			{
				this.submodes[this.submodeID].LsqQuant.Quant(this.lsp, this.qlsp, this.lpcSize, bits);
				if (this.first != 0)
				{
					for (int i = 0; i < this.lpcSize; i++)
					{
						this.old_lsp[i] = this.lsp[i];
					}
					for (int i = 0; i < this.lpcSize; i++)
					{
						this.old_qlsp[i] = this.qlsp[i];
					}
				}
				float[] array = new float[this.lpcSize];
				float[] array2 = new float[this.subframeSize];
				float[] array3 = new float[this.subframeSize];
				for (int j = 0; j < this.nbSubframes; j++)
				{
					float num12 = 0f;
					float num13 = 0f;
					int num14 = this.subframeSize * j;
					int num15 = num14;
					int num16 = this.excIdx + num14;
					int num17 = num14;
					int num18 = num14;
					float num19 = (1f + (float)j) / (float)this.nbSubframes;
					for (int i = 0; i < this.lpcSize; i++)
					{
						this.interp_lsp[i] = (1f - num19) * this.old_lsp[i] + num19 * this.lsp[i];
					}
					for (int i = 0; i < this.lpcSize; i++)
					{
						this.interp_qlsp[i] = (1f - num19) * this.old_qlsp[i] + num19 * this.qlsp[i];
					}
					Lsp.Enforce_margin(this.interp_lsp, this.lpcSize, 0.05f);
					Lsp.Enforce_margin(this.interp_qlsp, this.lpcSize, 0.05f);
					for (int i = 0; i < this.lpcSize; i++)
					{
						this.interp_lsp[i] = (float)Math.Cos((double)this.interp_lsp[i]);
					}
					for (int i = 0; i < this.lpcSize; i++)
					{
						this.interp_qlsp[i] = (float)Math.Cos((double)this.interp_qlsp[i]);
					}
					this.m_lsp.Lsp2lpc(this.interp_lsp, this.interp_lpc, this.lpcSize);
					this.m_lsp.Lsp2lpc(this.interp_qlsp, this.interp_qlpc, this.lpcSize);
					Filters.Bw_lpc(this.gamma1, this.interp_lpc, this.bw_lpc1, this.lpcSize);
					Filters.Bw_lpc(this.gamma2, this.interp_lpc, this.bw_lpc2, this.lpcSize);
					float num20 = 0f;
					num19 = 1f;
					this.pi_gain[j] = 0f;
					for (int i = 0; i <= this.lpcSize; i++)
					{
						num20 += num19 * this.interp_qlpc[i];
						num19 = -num19;
						this.pi_gain[j] += this.interp_qlpc[i];
					}
					float num21 = piGain[j];
					num21 = 1f / (Math.Abs(num21) + 0.01f);
					num20 = 1f / (Math.Abs(num20) + 0.01f);
					float num22 = Math.Abs(0.01f + num20) / (0.01f + Math.Abs(num21));
					Filters.Fir_mem2(this.high, num15, this.interp_qlpc, this.excBuf, num16, this.subframeSize, this.lpcSize, this.mem_sp2);
					for (int i = 0; i < this.subframeSize; i++)
					{
						num12 += this.excBuf[num16 + i] * this.excBuf[num16 + i];
					}
					if (this.submodes[this.submodeID].Innovation == null)
					{
						for (int i = 0; i < this.subframeSize; i++)
						{
							num13 += innov[num14 + i] * innov[num14 + i];
						}
						float num23 = num12 / (0.01f + num13);
						num23 = (float)Math.Sqrt((double)num23);
						num23 *= num22;
						int num24 = (int)Math.Floor(10.5 + 8.0 * Math.Log((double)num23 + 0.0001));
						if (num24 < 0)
						{
							num24 = 0;
						}
						if (num24 > 31)
						{
							num24 = 31;
						}
						bits.Pack(num24, 5);
						num23 = (float)(0.1 * Math.Exp((double)num24 / 9.4));
						num23 /= num22;
					}
					else
					{
						for (int i = 0; i < this.subframeSize; i++)
						{
							num13 += exc[num14 + i] * exc[num14 + i];
						}
						float num25 = (float)(Math.Sqrt((double)(1f + num12)) * (double)num22 / Math.Sqrt((double)((1f + num13) * (float)this.subframeSize)));
						int num26 = (int)Math.Floor(0.5 + 3.7 * (Math.Log((double)num25) + 2.0));
						if (num26 < 0)
						{
							num26 = 0;
						}
						if (num26 > 15)
						{
							num26 = 15;
						}
						bits.Pack(num26, 4);
						num25 = (float)Math.Exp(0.27027027027027023 * (double)num26 - 2.0);
						float num27 = num25 * (float)Math.Sqrt((double)(1f + num13)) / num22;
						float num28 = 1f / num27;
						for (int i = 0; i < this.subframeSize; i++)
						{
							this.excBuf[num16 + i] = 0f;
						}
						this.excBuf[num16] = 1f;
						Filters.Syn_percep_zero(this.excBuf, num16, this.interp_qlpc, this.bw_lpc1, this.bw_lpc2, array2, this.subframeSize, this.lpcSize);
						for (int i = 0; i < this.subframeSize; i++)
						{
							this.excBuf[num16 + i] = 0f;
						}
						for (int i = 0; i < this.lpcSize; i++)
						{
							array[i] = this.mem_sp[i];
						}
						Filters.Iir_mem2(this.excBuf, num16, this.interp_qlpc, this.excBuf, num16, this.subframeSize, this.lpcSize, array);
						for (int i = 0; i < this.lpcSize; i++)
						{
							array[i] = this.mem_sw[i];
						}
						Filters.Filter_mem2(this.excBuf, num16, this.bw_lpc1, this.bw_lpc2, this.res, num17, this.subframeSize, this.lpcSize, array, 0);
						for (int i = 0; i < this.lpcSize; i++)
						{
							array[i] = this.mem_sw[i];
						}
						Filters.Filter_mem2(this.high, num15, this.bw_lpc1, this.bw_lpc2, this.swBuf, num18, this.subframeSize, this.lpcSize, array, 0);
						for (int i = 0; i < this.subframeSize; i++)
						{
							this.target[i] = this.swBuf[num18 + i] - this.res[num17 + i];
						}
						for (int i = 0; i < this.subframeSize; i++)
						{
							this.excBuf[num16 + i] = 0f;
						}
						for (int i = 0; i < this.subframeSize; i++)
						{
							this.target[i] *= num28;
						}
						for (int i = 0; i < this.subframeSize; i++)
						{
							array3[i] = 0f;
						}
						this.submodes[this.submodeID].Innovation.Quantify(this.target, this.interp_qlpc, this.bw_lpc1, this.bw_lpc2, this.lpcSize, this.subframeSize, array3, 0, array2, bits, this.complexity + 1 >> 1);
						for (int i = 0; i < this.subframeSize; i++)
						{
							this.excBuf[num16 + i] += array3[i] * num27;
						}
						if (this.submodes[this.submodeID].DoubleCodebook != 0)
						{
							float[] array4 = new float[this.subframeSize];
							for (int i = 0; i < this.subframeSize; i++)
							{
								array4[i] = 0f;
							}
							for (int i = 0; i < this.subframeSize; i++)
							{
								this.target[i] *= 2.5f;
							}
							this.submodes[this.submodeID].Innovation.Quantify(this.target, this.interp_qlpc, this.bw_lpc1, this.bw_lpc2, this.lpcSize, this.subframeSize, array4, 0, array2, bits, this.complexity + 1 >> 1);
							for (int i = 0; i < this.subframeSize; i++)
							{
								array4[i] *= (float)((double)num27 * 0.4);
							}
							for (int i = 0; i < this.subframeSize; i++)
							{
								this.excBuf[num16 + i] += array4[i];
							}
						}
					}
					for (int i = 0; i < this.lpcSize; i++)
					{
						array[i] = this.mem_sp[i];
					}
					Filters.Iir_mem2(this.excBuf, num16, this.interp_qlpc, this.high, num15, this.subframeSize, this.lpcSize, this.mem_sp);
					Filters.Filter_mem2(this.high, num15, this.bw_lpc1, this.bw_lpc2, this.swBuf, num18, this.subframeSize, this.lpcSize, this.mem_sw, 0);
				}
				this.filters.Fir_mem_up(this.x0d, Codebook_Constants.h0, this.y0, this.fullFrameSize, 64, this.g0_mem);
				this.filters.Fir_mem_up(this.high, Codebook_Constants.h1, this.y1, this.fullFrameSize, 64, this.g1_mem);
				for (int i = 0; i < this.fullFrameSize; i++)
				{
					ins0[i] = 2f * (this.y0[i] - this.y1[i]);
				}
				for (int i = 0; i < this.lpcSize; i++)
				{
					this.old_lsp[i] = this.lsp[i];
				}
				for (int i = 0; i < this.lpcSize; i++)
				{
					this.old_qlsp[i] = this.qlsp[i];
				}
				this.first = 0;
				return 1;
			}
			for (int i = 0; i < this.frameSize; i++)
			{
				this.excBuf[this.excIdx + i] = (this.swBuf[i] = 0f);
			}
			for (int i = 0; i < this.lpcSize; i++)
			{
				this.mem_sw[i] = 0f;
			}
			this.first = 1;
			Filters.Iir_mem2(this.excBuf, this.excIdx, this.interp_qlpc, this.high, 0, this.subframeSize, this.lpcSize, this.mem_sp);
			this.filters.Fir_mem_up(this.x0d, Codebook_Constants.h0, this.y0, this.fullFrameSize, 64, this.g0_mem);
			this.filters.Fir_mem_up(this.high, Codebook_Constants.h1, this.y1, this.fullFrameSize, 64, this.g1_mem);
			for (int i = 0; i < this.fullFrameSize; i++)
			{
				ins0[i] = 2f * (this.y0[i] - this.y1[i]);
			}
			if (num != 0)
			{
				return 0;
			}
			return 1;
		}

		// Token: 0x17000041 RID: 65
		// (get) Token: 0x060000FA RID: 250 RVA: 0x00014394 File Offset: 0x00012594
		public virtual int EncodedFrameSize
		{
			get
			{
				int num = SbCodec.SB_FRAME_SIZE[this.submodeID];
				return num + this.lowenc.EncodedFrameSize;
			}
		}

		// Token: 0x17000042 RID: 66
		// (set) Token: 0x060000FB RID: 251 RVA: 0x000143C0 File Offset: 0x000125C0
		public virtual int Quality
		{
			set
			{
				if (value < 0)
				{
					value = 0;
				}
				if (value > 10)
				{
					value = 10;
				}
				if (this.uwb)
				{
					this.lowenc.Quality = value;
					this.Mode = SbEncoder.UWB_QUALITY_MAP[value];
					return;
				}
				this.lowenc.Mode = SbEncoder.NB_QUALITY_MAP[value];
				this.Mode = SbEncoder.WB_QUALITY_MAP[value];
			}
		}

		// Token: 0x17000043 RID: 67
		// (get) Token: 0x060000FC RID: 252 RVA: 0x00014420 File Offset: 0x00012620
		// (set) Token: 0x060000FD RID: 253 RVA: 0x00014484 File Offset: 0x00012684
		public virtual int BitRate
		{
			get
			{
				if (this.submodes[this.submodeID] != null)
				{
					return this.lowenc.BitRate + this.sampling_rate * this.submodes[this.submodeID].BitsPerFrame / this.frameSize;
				}
				return this.lowenc.BitRate + this.sampling_rate * 4 / this.frameSize;
			}
			set
			{
				for (int i = 10; i >= 0; i--)
				{
					this.Quality = i;
					if (this.BitRate <= value)
					{
						return;
					}
				}
			}
		}

		// Token: 0x17000044 RID: 68
		// (get) Token: 0x060000FE RID: 254 RVA: 0x000144AF File Offset: 0x000126AF
		public virtual int LookAhead
		{
			get
			{
				return 2 * this.lowenc.LookAhead + 64 - 1;
			}
		}

		// Token: 0x17000045 RID: 69
		// (get) Token: 0x060000FF RID: 255 RVA: 0x000144C3 File Offset: 0x000126C3
		// (set) Token: 0x06000100 RID: 256 RVA: 0x000144CC File Offset: 0x000126CC
		public virtual int Mode
		{
			get
			{
				return this.submodeID;
			}
			set
			{
				if (value < 0)
				{
					value = 0;
				}
				this.submodeID = (this.submodeSelect = value);
			}
		}

		// Token: 0x17000046 RID: 70
		// (get) Token: 0x06000101 RID: 257 RVA: 0x000144F0 File Offset: 0x000126F0
		// (set) Token: 0x06000102 RID: 258 RVA: 0x000144FE File Offset: 0x000126FE
		public virtual bool Vbr
		{
			get
			{
				return this.vbr_enabled != 0;
			}
			set
			{
				this.vbr_enabled = (value ? 1 : 0);
				this.lowenc.Vbr = value;
			}
		}

		// Token: 0x17000047 RID: 71
		// (get) Token: 0x06000103 RID: 259 RVA: 0x00014519 File Offset: 0x00012719
		// (set) Token: 0x06000104 RID: 260 RVA: 0x00014527 File Offset: 0x00012727
		public virtual bool Vad
		{
			get
			{
				return this.vad_enabled != 0;
			}
			set
			{
				this.vad_enabled = (value ? 1 : 0);
			}
		}

		// Token: 0x17000048 RID: 72
		// (get) Token: 0x06000105 RID: 261 RVA: 0x00014536 File Offset: 0x00012736
		// (set) Token: 0x06000106 RID: 262 RVA: 0x00014541 File Offset: 0x00012741
		public new bool Dtx
		{
			get
			{
				return this.dtx_enabled == 1;
			}
			set
			{
				this.dtx_enabled = (value ? 1 : 0);
			}
		}

		// Token: 0x17000049 RID: 73
		// (get) Token: 0x06000107 RID: 263 RVA: 0x00014550 File Offset: 0x00012750
		// (set) Token: 0x06000108 RID: 264 RVA: 0x00014558 File Offset: 0x00012758
		public virtual int Abr
		{
			get
			{
				return this.abr_enabled;
			}
			set
			{
				this.lowenc.Vbr = true;
				this.abr_enabled = ((value != 0) ? 1 : 0);
				this.vbr_enabled = 1;
				int i;
				for (i = 10; i >= 0; i--)
				{
					this.Quality = i;
					int bitRate = this.BitRate;
					if (bitRate <= value)
					{
						break;
					}
				}
				float num = (float)i;
				if (num < 0f)
				{
					num = 0f;
				}
				this.VbrQuality = num;
				this.abr_count = 0f;
				this.abr_drift = 0f;
				this.abr_drift2 = 0f;
			}
		}

		// Token: 0x1700004A RID: 74
		// (get) Token: 0x06000109 RID: 265 RVA: 0x000145DF File Offset: 0x000127DF
		// (set) Token: 0x0600010A RID: 266 RVA: 0x000145E8 File Offset: 0x000127E8
		public virtual float VbrQuality
		{
			get
			{
				return this.vbr_quality;
			}
			set
			{
				this.vbr_quality = value;
				float num = value + 0.6f;
				if (num > 10f)
				{
					num = 10f;
				}
				this.lowenc.VbrQuality = num;
				int num2 = (int)Math.Floor(0.5 + (double)value);
				if (num2 > 10)
				{
					num2 = 10;
				}
				this.Quality = num2;
			}
		}

		// Token: 0x1700004B RID: 75
		// (get) Token: 0x0600010B RID: 267 RVA: 0x00014640 File Offset: 0x00012840
		// (set) Token: 0x0600010C RID: 268 RVA: 0x00014648 File Offset: 0x00012848
		public virtual int Complexity
		{
			get
			{
				return this.complexity;
			}
			set
			{
				if (value < 0)
				{
					value = 0;
				}
				if (value > 10)
				{
					value = 10;
				}
				this.complexity = value;
			}
		}

		// Token: 0x1700004C RID: 76
		// (get) Token: 0x0600010D RID: 269 RVA: 0x00014661 File Offset: 0x00012861
		// (set) Token: 0x0600010E RID: 270 RVA: 0x00014669 File Offset: 0x00012869
		public virtual int SamplingRate
		{
			get
			{
				return this.sampling_rate;
			}
			set
			{
				this.sampling_rate = value;
				this.lowenc.SamplingRate = value;
			}
		}

		// Token: 0x1700004D RID: 77
		// (get) Token: 0x0600010F RID: 271 RVA: 0x0001467E File Offset: 0x0001287E
		public virtual float RelativeQuality
		{
			get
			{
				return this.relative_quality;
			}
		}

		// Token: 0x04000111 RID: 273
		private static readonly int[] NB_QUALITY_MAP = new int[]
		{
			1, 8, 2, 3, 4, 5, 5, 6, 6, 7,
			7
		};

		// Token: 0x04000112 RID: 274
		private static readonly int[] WB_QUALITY_MAP = new int[]
		{
			1, 1, 1, 1, 1, 1, 2, 2, 3, 3,
			4
		};

		// Token: 0x04000113 RID: 275
		private static readonly int[] UWB_QUALITY_MAP = new int[]
		{
			0, 1, 1, 1, 1, 1, 1, 1, 1, 1,
			1
		};

		// Token: 0x04000114 RID: 276
		protected internal IEncoder lowenc;

		// Token: 0x04000115 RID: 277
		private float[] x1d;

		// Token: 0x04000116 RID: 278
		private float[] h0_mem;

		// Token: 0x04000117 RID: 279
		private float[] buf;

		// Token: 0x04000118 RID: 280
		private float[] swBuf;

		// Token: 0x04000119 RID: 281
		private float[] res;

		// Token: 0x0400011A RID: 282
		private float[] target;

		// Token: 0x0400011B RID: 283
		private float[] window;

		// Token: 0x0400011C RID: 284
		private float[] lagWindow;

		// Token: 0x0400011D RID: 285
		private float[] rc;

		// Token: 0x0400011E RID: 286
		private float[] autocorr;

		// Token: 0x0400011F RID: 287
		private float[] lsp;

		// Token: 0x04000120 RID: 288
		private float[] old_lsp;

		// Token: 0x04000121 RID: 289
		private float[] interp_lsp;

		// Token: 0x04000122 RID: 290
		private float[] interp_lpc;

		// Token: 0x04000123 RID: 291
		private float[] bw_lpc1;

		// Token: 0x04000124 RID: 292
		private float[] bw_lpc2;

		// Token: 0x04000125 RID: 293
		private float[] mem_sp2;

		// Token: 0x04000126 RID: 294
		private float[] mem_sw;

		// Token: 0x04000127 RID: 295
		protected internal int nb_modes;

		// Token: 0x04000128 RID: 296
		private bool uwb;

		// Token: 0x04000129 RID: 297
		protected internal int complexity;

		// Token: 0x0400012A RID: 298
		protected internal int vbr_enabled;

		// Token: 0x0400012B RID: 299
		protected internal int vad_enabled;

		// Token: 0x0400012C RID: 300
		protected internal int abr_enabled;

		// Token: 0x0400012D RID: 301
		protected internal float vbr_quality;

		// Token: 0x0400012E RID: 302
		protected internal float relative_quality;

		// Token: 0x0400012F RID: 303
		protected internal float abr_drift;

		// Token: 0x04000130 RID: 304
		protected internal float abr_drift2;

		// Token: 0x04000131 RID: 305
		protected internal float abr_count;

		// Token: 0x04000132 RID: 306
		protected internal int sampling_rate;

		// Token: 0x04000133 RID: 307
		protected internal int submodeSelect;
	}
}
