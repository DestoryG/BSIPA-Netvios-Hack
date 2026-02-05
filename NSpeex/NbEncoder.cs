using System;

namespace NSpeex
{
	// Token: 0x02000014 RID: 20
	internal class NbEncoder : NbCodec, IEncoder
	{
		// Token: 0x0600007D RID: 125 RVA: 0x00006090 File Offset: 0x00004290
		protected override void Init(int frameSize, int subframeSize, int lpcSize, int bufSize)
		{
			base.Init(frameSize, subframeSize, lpcSize, bufSize);
			this.complexity = 3;
			this.vbr_enabled = 0;
			this.vad_enabled = 0;
			this.abr_enabled = 0;
			this.vbr_quality = 8f;
			this.submodeSelect = 5;
			this.pre_mem2 = 0f;
			this.bounded_pitch = 1;
			this.exc2Buf = new float[bufSize];
			this.exc2Idx = bufSize - this.windowSize;
			this.swBuf = new float[bufSize];
			this.swIdx = bufSize - this.windowSize;
			this.window = Misc.Window(this.windowSize, subframeSize);
			this.lagWindow = Misc.LagWindow(lpcSize, this.lag_factor);
			this.autocorr = new float[lpcSize + 1];
			this.buf2 = new float[this.windowSize];
			this.interp_lpc = new float[lpcSize + 1];
			this.interp_qlpc = new float[lpcSize + 1];
			this.bw_lpc1 = new float[lpcSize + 1];
			this.bw_lpc2 = new float[lpcSize + 1];
			this.lsp = new float[lpcSize];
			this.qlsp = new float[lpcSize];
			this.old_lsp = new float[lpcSize];
			this.old_qlsp = new float[lpcSize];
			this.interp_lsp = new float[lpcSize];
			this.interp_qlsp = new float[lpcSize];
			this.rc = new float[lpcSize];
			this.mem_sp = new float[lpcSize];
			this.mem_sw = new float[lpcSize];
			this.mem_sw_whole = new float[lpcSize];
			this.mem_exc = new float[lpcSize];
			this.vbr = new Vbr();
			this.dtx_count = 0;
			this.abr_count = 0f;
			this.sampling_rate = 8000;
			this.awk1 = new float[lpcSize + 1];
			this.awk2 = new float[lpcSize + 1];
			this.awk3 = new float[lpcSize + 1];
			this.innov2 = new float[40];
			this.pitch = new int[this.nbSubframes];
		}

		// Token: 0x0600007E RID: 126 RVA: 0x00006290 File Offset: 0x00004490
		public virtual int Encode(Bits bits, float[] ins0)
		{
			Array.Copy(this.frmBuf, this.frameSize, this.frmBuf, 0, this.bufSize - this.frameSize);
			this.frmBuf[this.bufSize - this.frameSize] = ins0[0] - this.preemph * this.pre_mem;
			for (int i = 1; i < this.frameSize; i++)
			{
				this.frmBuf[this.bufSize - this.frameSize + i] = ins0[i] - this.preemph * ins0[i - 1];
			}
			this.pre_mem = ins0[this.frameSize - 1];
			Array.Copy(this.exc2Buf, this.frameSize, this.exc2Buf, 0, this.bufSize - this.frameSize);
			Array.Copy(this.excBuf, this.frameSize, this.excBuf, 0, this.bufSize - this.frameSize);
			Array.Copy(this.swBuf, this.frameSize, this.swBuf, 0, this.bufSize - this.frameSize);
			for (int i = 0; i < this.windowSize; i++)
			{
				this.buf2[i] = this.frmBuf[i + this.frmIdx] * this.window[i];
			}
			Lpc.Autocorr(this.buf2, this.autocorr, this.lpcSize + 1, this.windowSize);
			this.autocorr[0] += 10f;
			this.autocorr[0] *= this.lpc_floor;
			for (int i = 0; i < this.lpcSize + 1; i++)
			{
				this.autocorr[i] *= this.lagWindow[i];
			}
			Lpc.Wld(this.lpc, this.autocorr, this.rc, this.lpcSize);
			Array.Copy(this.lpc, 0, this.lpc, 1, this.lpcSize);
			this.lpc[0] = 1f;
			int num = Lsp.Lpc2lsp(this.lpc, this.lpcSize, this.lsp, 15, 0.2f);
			if (num == this.lpcSize)
			{
				for (int i = 0; i < this.lpcSize; i++)
				{
					this.lsp[i] = (float)Math.Acos((double)this.lsp[i]);
				}
			}
			else
			{
				if (this.complexity > 1)
				{
					num = Lsp.Lpc2lsp(this.lpc, this.lpcSize, this.lsp, 11, 0.05f);
				}
				if (num == this.lpcSize)
				{
					for (int i = 0; i < this.lpcSize; i++)
					{
						this.lsp[i] = (float)Math.Acos((double)this.lsp[i]);
					}
				}
				else
				{
					for (int i = 0; i < this.lpcSize; i++)
					{
						this.lsp[i] = this.old_lsp[i];
					}
				}
			}
			float num2 = 0f;
			for (int i = 0; i < this.lpcSize; i++)
			{
				num2 += (this.old_lsp[i] - this.lsp[i]) * (this.old_lsp[i] - this.lsp[i]);
			}
			if (this.first != 0)
			{
				for (int i = 0; i < this.lpcSize; i++)
				{
					this.interp_lsp[i] = this.lsp[i];
				}
			}
			else
			{
				for (int i = 0; i < this.lpcSize; i++)
				{
					this.interp_lsp[i] = 0.375f * this.old_lsp[i] + 0.625f * this.lsp[i];
				}
			}
			Lsp.Enforce_margin(this.interp_lsp, this.lpcSize, 0.002f);
			for (int i = 0; i < this.lpcSize; i++)
			{
				this.interp_lsp[i] = (float)Math.Cos((double)this.interp_lsp[i]);
			}
			this.m_lsp.Lsp2lpc(this.interp_lsp, this.interp_lpc, this.lpcSize);
			int num3;
			float num4;
			if (this.submodes[this.submodeID] == null || this.vbr_enabled != 0 || this.vad_enabled != 0 || this.submodes[this.submodeID].ForcedPitchGain != 0 || this.submodes[this.submodeID].LbrPitch != -1)
			{
				int[] array = new int[6];
				float[] array2 = new float[6];
				Filters.Bw_lpc(this.gamma1, this.interp_lpc, this.bw_lpc1, this.lpcSize);
				Filters.Bw_lpc(this.gamma2, this.interp_lpc, this.bw_lpc2, this.lpcSize);
				Filters.Filter_mem2(this.frmBuf, this.frmIdx, this.bw_lpc1, this.bw_lpc2, this.swBuf, this.swIdx, this.frameSize, this.lpcSize, this.mem_sw_whole, 0);
				Ltp.Open_loop_nbest_pitch(this.swBuf, this.swIdx, this.min_pitch, this.max_pitch, this.frameSize, array, array2, 6);
				num3 = array[0];
				num4 = array2[0];
				for (int i = 1; i < 6; i++)
				{
					if ((double)array2[i] > 0.85 * (double)num4 && (Math.Abs((double)array[i] - (double)num3 / 2.0) <= 1.0 || Math.Abs((double)array[i] - (double)num3 / 3.0) <= 1.0 || Math.Abs((double)array[i] - (double)num3 / 4.0) <= 1.0 || Math.Abs((double)array[i] - (double)num3 / 5.0) <= 1.0))
					{
						num3 = array[i];
					}
				}
			}
			else
			{
				num3 = 0;
				num4 = 0f;
			}
			Filters.Fir_mem2(this.frmBuf, this.frmIdx, this.interp_lpc, this.excBuf, this.excIdx, this.frameSize, this.lpcSize, this.mem_exc);
			float num5 = 0f;
			for (int i = 0; i < this.frameSize; i++)
			{
				num5 += this.excBuf[this.excIdx + i] * this.excBuf[this.excIdx + i];
			}
			num5 = (float)Math.Sqrt((double)(1f + num5 / (float)this.frameSize));
			if (this.vbr != null && (this.vbr_enabled != 0 || this.vad_enabled != 0))
			{
				if (this.abr_enabled != 0)
				{
					float num6 = 0f;
					if (this.abr_drift2 * this.abr_drift > 0f)
					{
						num6 = -1E-05f * this.abr_drift / (1f + this.abr_count);
						if (num6 > 0.05f)
						{
							num6 = 0.05f;
						}
						if (num6 < -0.05f)
						{
							num6 = -0.05f;
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
				this.relative_quality = this.vbr.Analysis(ins0, this.frameSize, num3, num4);
				if (this.vbr_enabled != 0)
				{
					int num7 = 0;
					float num8 = 100f;
					int j;
					for (j = 8; j > 0; j--)
					{
						int num9 = (int)Math.Floor((double)this.vbr_quality);
						float num10;
						if (num9 == 10)
						{
							num10 = NSpeex.Vbr.nb_thresh[j][num9];
						}
						else
						{
							num10 = (this.vbr_quality - (float)num9) * NSpeex.Vbr.nb_thresh[j][num9 + 1] + ((float)(1 + num9) - this.vbr_quality) * NSpeex.Vbr.nb_thresh[j][num9];
						}
						if (this.relative_quality > num10 && this.relative_quality - num10 < num8)
						{
							num7 = j;
							num8 = this.relative_quality - num10;
						}
					}
					j = num7;
					if (j == 0)
					{
						if (this.dtx_count == 0 || (double)num2 > 0.05 || this.dtx_enabled == 0 || this.dtx_count > 20)
						{
							j = 1;
							this.dtx_count = 1;
						}
						else
						{
							j = 0;
							this.dtx_count++;
						}
					}
					else
					{
						this.dtx_count = 0;
					}
					this.Mode = j;
					if (this.abr_enabled != 0)
					{
						int bitRate = this.BitRate;
						this.abr_drift += (float)(bitRate - this.abr_enabled);
						this.abr_drift2 = 0.95f * this.abr_drift2 + 0.05f * (float)(bitRate - this.abr_enabled);
						this.abr_count += new float?(1f).Value;
					}
				}
				else
				{
					int num11;
					if (this.relative_quality < 2f)
					{
						if (this.dtx_count == 0 || (double)num2 > 0.05 || this.dtx_enabled == 0 || this.dtx_count > 20)
						{
							this.dtx_count = 1;
							num11 = 1;
						}
						else
						{
							num11 = 0;
							this.dtx_count++;
						}
					}
					else
					{
						this.dtx_count = 0;
						num11 = this.submodeSelect;
					}
					this.submodeID = num11;
				}
			}
			else
			{
				this.relative_quality = -1f;
			}
			bits.Pack(0, 1);
			bits.Pack(this.submodeID, 4);
			if (this.submodes[this.submodeID] == null)
			{
				for (int i = 0; i < this.frameSize; i++)
				{
					this.excBuf[this.excIdx + i] = (this.exc2Buf[this.exc2Idx + i] = (this.swBuf[this.swIdx + i] = 0f));
				}
				for (int i = 0; i < this.lpcSize; i++)
				{
					this.mem_sw[i] = 0f;
				}
				this.first = 1;
				this.bounded_pitch = 1;
				Filters.Iir_mem2(this.excBuf, this.excIdx, this.interp_qlpc, this.frmBuf, this.frmIdx, this.frameSize, this.lpcSize, this.mem_sp);
				ins0[0] = this.frmBuf[this.frmIdx] + this.preemph * this.pre_mem2;
				for (int i = 1; i < this.frameSize; i++)
				{
					ins0[i] = this.frmBuf[this.frmIdx = i] + this.preemph * ins0[i - 1];
				}
				this.pre_mem2 = ins0[this.frameSize - 1];
				return 0;
			}
			if (this.first != 0)
			{
				for (int i = 0; i < this.lpcSize; i++)
				{
					this.old_lsp[i] = this.lsp[i];
				}
			}
			this.submodes[this.submodeID].LsqQuant.Quant(this.lsp, this.qlsp, this.lpcSize, bits);
			if (this.submodes[this.submodeID].LbrPitch != -1)
			{
				bits.Pack(num3 - this.min_pitch, 7);
			}
			if (this.submodes[this.submodeID].ForcedPitchGain != 0)
			{
				int num12 = (int)Math.Floor(0.5 + (double)(15f * num4));
				if (num12 > 15)
				{
					num12 = 15;
				}
				if (num12 < 0)
				{
					num12 = 0;
				}
				bits.Pack(num12, 4);
				num4 = 0.066667f * (float)num12;
			}
			int num13 = (int)Math.Floor(0.5 + 3.5 * Math.Log((double)num5));
			if (num13 < 0)
			{
				num13 = 0;
			}
			if (num13 > 31)
			{
				num13 = 31;
			}
			num5 = (float)Math.Exp((double)num13 / 3.5);
			bits.Pack(num13, 5);
			if (this.first != 0)
			{
				for (int i = 0; i < this.lpcSize; i++)
				{
					this.old_qlsp[i] = this.qlsp[i];
				}
			}
			float[] array3 = new float[this.subframeSize];
			float[] array4 = new float[this.subframeSize];
			float[] array5 = new float[this.subframeSize];
			float[] array6 = new float[this.lpcSize];
			float[] array7 = new float[this.frameSize];
			for (int i = 0; i < this.frameSize; i++)
			{
				array7[i] = this.frmBuf[this.frmIdx + i];
			}
			for (int k = 0; k < this.nbSubframes; k++)
			{
				int num14 = this.subframeSize * k;
				int num15 = this.frmIdx + num14;
				int num16 = this.excIdx + num14;
				int num17 = this.swIdx + num14;
				int num18 = this.exc2Idx + num14;
				float num19 = (float)(1.0 + (double)k) / (float)this.nbSubframes;
				for (int i = 0; i < this.lpcSize; i++)
				{
					this.interp_lsp[i] = (1f - num19) * this.old_lsp[i] + num19 * this.lsp[i];
				}
				for (int i = 0; i < this.lpcSize; i++)
				{
					this.interp_qlsp[i] = (1f - num19) * this.old_qlsp[i] + num19 * this.qlsp[i];
				}
				Lsp.Enforce_margin(this.interp_lsp, this.lpcSize, 0.002f);
				Lsp.Enforce_margin(this.interp_qlsp, this.lpcSize, 0.002f);
				for (int i = 0; i < this.lpcSize; i++)
				{
					this.interp_lsp[i] = (float)Math.Cos((double)this.interp_lsp[i]);
				}
				this.m_lsp.Lsp2lpc(this.interp_lsp, this.interp_lpc, this.lpcSize);
				for (int i = 0; i < this.lpcSize; i++)
				{
					this.interp_qlsp[i] = (float)Math.Cos((double)this.interp_qlsp[i]);
				}
				this.m_lsp.Lsp2lpc(this.interp_qlsp, this.interp_qlpc, this.lpcSize);
				num19 = 1f;
				this.pi_gain[k] = 0f;
				for (int i = 0; i <= this.lpcSize; i++)
				{
					this.pi_gain[k] += num19 * this.interp_qlpc[i];
					num19 = -num19;
				}
				Filters.Bw_lpc(this.gamma1, this.interp_lpc, this.bw_lpc1, this.lpcSize);
				if (this.gamma2 >= 0f)
				{
					Filters.Bw_lpc(this.gamma2, this.interp_lpc, this.bw_lpc2, this.lpcSize);
				}
				else
				{
					this.bw_lpc2[0] = 1f;
					this.bw_lpc2[1] = -this.preemph;
					for (int i = 2; i <= this.lpcSize; i++)
					{
						this.bw_lpc2[i] = 0f;
					}
				}
				for (int i = 0; i < this.subframeSize; i++)
				{
					this.excBuf[num16 + i] = 0f;
				}
				this.excBuf[num16] = 1f;
				Filters.Syn_percep_zero(this.excBuf, num16, this.interp_qlpc, this.bw_lpc1, this.bw_lpc2, array5, this.subframeSize, this.lpcSize);
				for (int i = 0; i < this.subframeSize; i++)
				{
					this.excBuf[num16 + i] = 0f;
				}
				for (int i = 0; i < this.subframeSize; i++)
				{
					this.exc2Buf[num18 + i] = 0f;
				}
				for (int i = 0; i < this.lpcSize; i++)
				{
					array6[i] = this.mem_sp[i];
				}
				Filters.Iir_mem2(this.excBuf, num16, this.interp_qlpc, this.excBuf, num16, this.subframeSize, this.lpcSize, array6);
				for (int i = 0; i < this.lpcSize; i++)
				{
					array6[i] = this.mem_sw[i];
				}
				Filters.Filter_mem2(this.excBuf, num16, this.bw_lpc1, this.bw_lpc2, array3, 0, this.subframeSize, this.lpcSize, array6, 0);
				for (int i = 0; i < this.lpcSize; i++)
				{
					array6[i] = this.mem_sw[i];
				}
				Filters.Filter_mem2(this.frmBuf, num15, this.bw_lpc1, this.bw_lpc2, this.swBuf, num17, this.subframeSize, this.lpcSize, array6, 0);
				for (int i = 0; i < this.subframeSize; i++)
				{
					array4[i] = this.swBuf[num17 + i] - array3[i];
				}
				for (int i = 0; i < this.subframeSize; i++)
				{
					this.excBuf[num16 + i] = (this.exc2Buf[num18 + i] = 0f);
				}
				int num20;
				int num21;
				if (this.submodes[this.submodeID].LbrPitch != -1)
				{
					int lbrPitch = this.submodes[this.submodeID].LbrPitch;
					if (lbrPitch != 0)
					{
						if (num3 < this.min_pitch + lbrPitch - 1)
						{
							num3 = this.min_pitch + lbrPitch - 1;
						}
						if (num3 > this.max_pitch - lbrPitch)
						{
							num3 = this.max_pitch - lbrPitch;
						}
						num20 = num3 - lbrPitch + 1;
						num21 = num3 + lbrPitch;
					}
					else
					{
						num21 = (num20 = num3);
					}
				}
				else
				{
					num20 = this.min_pitch;
					num21 = this.max_pitch;
				}
				if (this.bounded_pitch != 0 && num21 > num14)
				{
					num21 = num14;
				}
				int num22 = this.submodes[this.submodeID].Ltp.Quant(array4, this.swBuf, num17, this.interp_qlpc, this.bw_lpc1, this.bw_lpc2, this.excBuf, num16, num20, num21, num4, this.lpcSize, this.subframeSize, bits, this.exc2Buf, num18, array5, this.complexity);
				this.pitch[k] = num22;
				Filters.Syn_percep_zero(this.excBuf, num16, this.interp_qlpc, this.bw_lpc1, this.bw_lpc2, array3, this.subframeSize, this.lpcSize);
				for (int i = 0; i < this.subframeSize; i++)
				{
					array4[i] -= array3[i];
				}
				float num23 = 0f;
				int num24 = k * this.subframeSize;
				for (int i = 0; i < this.subframeSize; i++)
				{
					this.innov[num24 + i] = 0f;
				}
				Filters.Residue_percep_zero(array4, 0, this.interp_qlpc, this.bw_lpc1, this.bw_lpc2, this.buf2, this.subframeSize, this.lpcSize);
				for (int i = 0; i < this.subframeSize; i++)
				{
					num23 += this.buf2[i] * this.buf2[i];
				}
				num23 = (float)Math.Sqrt((double)(0.1f + num23 / (float)this.subframeSize));
				num23 /= num5;
				if (this.submodes[this.submodeID].HaveSubframeGain != 0)
				{
					num23 = (float)Math.Log((double)num23);
					if (this.submodes[this.submodeID].HaveSubframeGain == 3)
					{
						int num25 = VQ.Index(num23, NbCodec.exc_gain_quant_scal3, 8);
						bits.Pack(num25, 3);
						num23 = NbCodec.exc_gain_quant_scal3[num25];
					}
					else
					{
						int num25 = VQ.Index(num23, NbCodec.exc_gain_quant_scal1, 2);
						bits.Pack(num25, 1);
						num23 = NbCodec.exc_gain_quant_scal1[num25];
					}
					num23 = (float)Math.Exp((double)num23);
				}
				else
				{
					num23 = 1f;
				}
				num23 *= num5;
				float num26 = 1f / num23;
				for (int i = 0; i < this.subframeSize; i++)
				{
					array4[i] *= num26;
				}
				this.submodes[this.submodeID].Innovation.Quantify(array4, this.interp_qlpc, this.bw_lpc1, this.bw_lpc2, this.lpcSize, this.subframeSize, this.innov, num24, array5, bits, this.complexity);
				for (int i = 0; i < this.subframeSize; i++)
				{
					this.innov[num24 + i] *= num23;
				}
				for (int i = 0; i < this.subframeSize; i++)
				{
					this.excBuf[num16 + i] += this.innov[num24 + i];
				}
				if (this.submodes[this.submodeID].DoubleCodebook != 0)
				{
					float[] array8 = new float[this.subframeSize];
					for (int i = 0; i < this.subframeSize; i++)
					{
						array4[i] *= 2.2f;
					}
					this.submodes[this.submodeID].Innovation.Quantify(array4, this.interp_qlpc, this.bw_lpc1, this.bw_lpc2, this.lpcSize, this.subframeSize, array8, 0, array5, bits, this.complexity);
					for (int i = 0; i < this.subframeSize; i++)
					{
						array8[i] *= (float)((double)num23 * 0.45454545454545453);
					}
					for (int i = 0; i < this.subframeSize; i++)
					{
						this.excBuf[num16 + i] += array8[i];
					}
				}
				for (int i = 0; i < this.subframeSize; i++)
				{
					array4[i] *= num23;
				}
				for (int i = 0; i < this.lpcSize; i++)
				{
					array6[i] = this.mem_sp[i];
				}
				Filters.Iir_mem2(this.excBuf, num16, this.interp_qlpc, this.frmBuf, num15, this.subframeSize, this.lpcSize, this.mem_sp);
				Filters.Filter_mem2(this.frmBuf, num15, this.bw_lpc1, this.bw_lpc2, this.swBuf, num17, this.subframeSize, this.lpcSize, this.mem_sw, 0);
				for (int i = 0; i < this.subframeSize; i++)
				{
					this.exc2Buf[num18 + i] = this.excBuf[num16 + i];
				}
			}
			if (this.submodeID >= 1)
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
			if (this.submodeID == 1)
			{
				if (this.dtx_count != 0)
				{
					bits.Pack(15, 4);
				}
				else
				{
					bits.Pack(0, 4);
				}
			}
			this.first = 0;
			float num27 = 0f;
			float num28 = 0f;
			for (int i = 0; i < this.frameSize; i++)
			{
				num27 += this.frmBuf[this.frmIdx + i] * this.frmBuf[this.frmIdx + i];
				num28 += (this.frmBuf[this.frmIdx + i] - array7[i]) * (this.frmBuf[this.frmIdx + i] - array7[i]);
			}
			Math.Log((double)((num27 + 1f) / (num28 + 1f)));
			ins0[0] = this.frmBuf[this.frmIdx] + this.preemph * this.pre_mem2;
			for (int i = 1; i < this.frameSize; i++)
			{
				ins0[i] = this.frmBuf[this.frmIdx + i] + this.preemph * ins0[i - 1];
			}
			this.pre_mem2 = ins0[this.frameSize - 1];
			if (this.submodes[this.submodeID].Innovation is NoiseSearch || this.submodeID == 0)
			{
				this.bounded_pitch = 1;
			}
			else
			{
				this.bounded_pitch = 0;
			}
			return 1;
		}

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x0600007F RID: 127 RVA: 0x0000794F File Offset: 0x00005B4F
		public virtual int EncodedFrameSize
		{
			get
			{
				return NbCodec.NB_FRAME_SIZE[this.submodeID];
			}
		}

		// Token: 0x17000025 RID: 37
		// (set) Token: 0x06000080 RID: 128 RVA: 0x00007960 File Offset: 0x00005B60
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
				this.submodeID = (this.submodeSelect = NbEncoder.NB_QUALITY_MAP[value]);
			}
		}

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x06000081 RID: 129 RVA: 0x00007994 File Offset: 0x00005B94
		// (set) Token: 0x06000082 RID: 130 RVA: 0x000079E0 File Offset: 0x00005BE0
		public virtual int BitRate
		{
			get
			{
				if (this.submodes[this.submodeID] != null)
				{
					return this.sampling_rate * this.submodes[this.submodeID].BitsPerFrame / this.frameSize;
				}
				return this.sampling_rate * 5 / this.frameSize;
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

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x06000083 RID: 131 RVA: 0x00007A0B File Offset: 0x00005C0B
		// (set) Token: 0x06000084 RID: 132 RVA: 0x00007A14 File Offset: 0x00005C14
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

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x06000085 RID: 133 RVA: 0x00007A38 File Offset: 0x00005C38
		// (set) Token: 0x06000086 RID: 134 RVA: 0x00007A46 File Offset: 0x00005C46
		public virtual bool Vbr
		{
			get
			{
				return this.vbr_enabled != 0;
			}
			set
			{
				this.vbr_enabled = (value ? 1 : 0);
			}
		}

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x06000087 RID: 135 RVA: 0x00007A55 File Offset: 0x00005C55
		// (set) Token: 0x06000088 RID: 136 RVA: 0x00007A63 File Offset: 0x00005C63
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

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x06000089 RID: 137 RVA: 0x00007A72 File Offset: 0x00005C72
		// (set) Token: 0x0600008A RID: 138 RVA: 0x00007A7D File Offset: 0x00005C7D
		public virtual bool Dtx
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

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x0600008B RID: 139 RVA: 0x00007A8C File Offset: 0x00005C8C
		// (set) Token: 0x0600008C RID: 140 RVA: 0x00007A94 File Offset: 0x00005C94
		public virtual int Abr
		{
			get
			{
				return this.abr_enabled;
			}
			set
			{
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

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x0600008D RID: 141 RVA: 0x00007B0F File Offset: 0x00005D0F
		// (set) Token: 0x0600008E RID: 142 RVA: 0x00007B17 File Offset: 0x00005D17
		public virtual float VbrQuality
		{
			get
			{
				return this.vbr_quality;
			}
			set
			{
				if (value < 0f)
				{
					value = 0f;
				}
				if (value > 10f)
				{
					value = 10f;
				}
				this.vbr_quality = value;
			}
		}

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x0600008F RID: 143 RVA: 0x00007B3E File Offset: 0x00005D3E
		// (set) Token: 0x06000090 RID: 144 RVA: 0x00007B46 File Offset: 0x00005D46
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

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x06000091 RID: 145 RVA: 0x00007B5F File Offset: 0x00005D5F
		// (set) Token: 0x06000092 RID: 146 RVA: 0x00007B67 File Offset: 0x00005D67
		public virtual int SamplingRate
		{
			get
			{
				return this.sampling_rate;
			}
			set
			{
				this.sampling_rate = value;
			}
		}

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x06000093 RID: 147 RVA: 0x00007B70 File Offset: 0x00005D70
		public virtual int LookAhead
		{
			get
			{
				return this.windowSize - this.frameSize;
			}
		}

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x06000094 RID: 148 RVA: 0x00007B7F File Offset: 0x00005D7F
		public virtual float RelativeQuality
		{
			get
			{
				return this.relative_quality;
			}
		}

		// Token: 0x0400007C RID: 124
		public static readonly int[] NB_QUALITY_MAP = new int[]
		{
			1, 8, 2, 3, 3, 4, 4, 5, 5, 6,
			7
		};

		// Token: 0x0400007D RID: 125
		private int bounded_pitch;

		// Token: 0x0400007E RID: 126
		private int[] pitch;

		// Token: 0x0400007F RID: 127
		private float pre_mem2;

		// Token: 0x04000080 RID: 128
		private float[] exc2Buf;

		// Token: 0x04000081 RID: 129
		private int exc2Idx;

		// Token: 0x04000082 RID: 130
		private float[] swBuf;

		// Token: 0x04000083 RID: 131
		private int swIdx;

		// Token: 0x04000084 RID: 132
		private float[] window;

		// Token: 0x04000085 RID: 133
		private float[] buf2;

		// Token: 0x04000086 RID: 134
		private float[] autocorr;

		// Token: 0x04000087 RID: 135
		private float[] lagWindow;

		// Token: 0x04000088 RID: 136
		private float[] lsp;

		// Token: 0x04000089 RID: 137
		private float[] old_lsp;

		// Token: 0x0400008A RID: 138
		private float[] interp_lsp;

		// Token: 0x0400008B RID: 139
		private float[] interp_lpc;

		// Token: 0x0400008C RID: 140
		private float[] bw_lpc1;

		// Token: 0x0400008D RID: 141
		private float[] bw_lpc2;

		// Token: 0x0400008E RID: 142
		private float[] rc;

		// Token: 0x0400008F RID: 143
		private float[] mem_sw;

		// Token: 0x04000090 RID: 144
		private float[] mem_sw_whole;

		// Token: 0x04000091 RID: 145
		private float[] mem_exc;

		// Token: 0x04000092 RID: 146
		private Vbr vbr;

		// Token: 0x04000093 RID: 147
		private int dtx_count;

		// Token: 0x04000094 RID: 148
		private float[] innov2;

		// Token: 0x04000095 RID: 149
		protected internal int complexity;

		// Token: 0x04000096 RID: 150
		protected internal int vbr_enabled;

		// Token: 0x04000097 RID: 151
		protected internal int vad_enabled;

		// Token: 0x04000098 RID: 152
		protected internal int abr_enabled;

		// Token: 0x04000099 RID: 153
		protected internal float vbr_quality;

		// Token: 0x0400009A RID: 154
		protected internal float relative_quality;

		// Token: 0x0400009B RID: 155
		protected internal float abr_drift;

		// Token: 0x0400009C RID: 156
		protected internal float abr_drift2;

		// Token: 0x0400009D RID: 157
		protected internal float abr_count;

		// Token: 0x0400009E RID: 158
		protected internal int sampling_rate;

		// Token: 0x0400009F RID: 159
		protected internal int submodeSelect;
	}
}
