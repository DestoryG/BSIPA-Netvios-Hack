using System;

namespace NSpeex
{
	// Token: 0x02000015 RID: 21
	public class JitterBuffer
	{
		// Token: 0x06000097 RID: 151 RVA: 0x00007BD5 File Offset: 0x00005DD5
		private static int RoundDown(int x, int step)
		{
			if (x < 0)
			{
				return (x - step + 1) / step * step;
			}
			return x / step * step;
		}

		// Token: 0x06000098 RID: 152 RVA: 0x00007BEA File Offset: 0x00005DEA
		private void FreeBuffer(byte[] buffer)
		{
		}

		// Token: 0x06000099 RID: 153 RVA: 0x00007BEC File Offset: 0x00005DEC
		private byte[] AllocBuffer(long size)
		{
			return new byte[size];
		}

		// Token: 0x0600009A RID: 154 RVA: 0x00007BF8 File Offset: 0x00005DF8
		public void Init(int step_size)
		{
			if (step_size <= 0)
			{
				throw new ArgumentOutOfRangeException("step_size");
			}
			for (int i = 0; i < 200; i++)
			{
				this.packets[i].data = null;
			}
			for (int i = 0; i < 3; i++)
			{
				this._tb[i] = new JitterBuffer.TimingBuffer();
			}
			this.delay_step = step_size;
			this.concealment_size = step_size;
			this.buffer_margin = 0;
			this.late_cutoff = 50;
			this.DestroyBufferCallback = null;
			this.latency_tradeoff = 0;
			this.auto_adjust = true;
			int num = 4;
			this.SetMaxLateRate(num);
			this.Reset();
		}

		// Token: 0x0600009B RID: 155 RVA: 0x00007C90 File Offset: 0x00005E90
		private void SetMaxLateRate(int maxLateRate)
		{
			this.max_late_rate = maxLateRate;
			this.window_size = 4000 / this.max_late_rate;
			this.subwindow_size = this.window_size / 3;
		}

		// Token: 0x0600009C RID: 156 RVA: 0x00007CBC File Offset: 0x00005EBC
		private void Reset()
		{
			for (int i = 0; i < 200; i++)
			{
				if (this.packets[i].data != null)
				{
					if (this.DestroyBufferCallback != null)
					{
						this.DestroyBufferCallback(this.packets[i].data);
					}
					else
					{
						this.FreeBuffer(this.packets[i].data);
					}
					this.packets[i].data = null;
				}
			}
			this.pointer_timestamp = 0L;
			this.next_stop = 0L;
			this.reset_state = true;
			this.lost_count = 0;
			this.buffered = 0L;
			this.auto_tradeoff = 32000;
			for (int i = 0; i < 3; i++)
			{
				this._tb[i].Init();
				this.timeBuffers[i] = this._tb[i];
			}
		}

		// Token: 0x0600009D RID: 157 RVA: 0x00007D98 File Offset: 0x00005F98
		private short ComputeOptDelay()
		{
			short num = 0;
			int num2 = int.MaxValue;
			int num3 = 0;
			int[] array = new int[3];
			bool flag = false;
			int num4 = 0;
			int num5 = 0;
			JitterBuffer.TimingBuffer[] tb = this._tb;
			int num6 = 0;
			for (int i = 0; i < 3; i++)
			{
				num6 += tb[i].curr_count;
			}
			if (num6 == 0)
			{
				return 0;
			}
			float num7;
			if (this.latency_tradeoff != 0)
			{
				num7 = (float)this.latency_tradeoff * 100f / (float)num6;
			}
			else
			{
				num7 = (float)(this.auto_tradeoff * this.window_size / num6);
			}
			for (int i = 0; i < 3; i++)
			{
				array[i] = 0;
			}
			for (int i = 0; i < 40; i++)
			{
				int num8 = -1;
				int num9 = 32767;
				for (int j = 0; j < 3; j++)
				{
					if (array[j] < tb[j].filled && tb[j].timing[array[j]] < num9)
					{
						num8 = j;
						num9 = tb[j].timing[array[j]];
					}
				}
				if (num8 == -1)
				{
					break;
				}
				if (i == 0)
				{
					num5 = num9;
				}
				num4 = num9;
				num9 = JitterBuffer.RoundDown(num9, this.delay_step);
				array[num8]++;
				int num10 = (int)((float)(-(float)num9) + num7 * (float)num3);
				if (num10 < num2)
				{
					num2 = num10;
					num = (short)num9;
				}
				num3++;
				if (num9 >= 0 && !flag)
				{
					flag = true;
					num3 += 4;
				}
			}
			int num11 = num4 - num5;
			this.auto_tradeoff = 1 + num11 / 40;
			if (num6 < 40 && num > 0)
			{
				return 0;
			}
			return num;
		}

		// Token: 0x0600009E RID: 158 RVA: 0x00007F18 File Offset: 0x00006118
		private void UpdateTimings(int timing)
		{
			if (timing < -32768)
			{
				timing = -32768;
			}
			if (timing > 32767)
			{
				timing = 32767;
			}
			short num = (short)timing;
			if (this.timeBuffers[0].curr_count >= this.subwindow_size)
			{
				JitterBuffer.TimingBuffer timingBuffer = this.timeBuffers[2];
				for (int i = 2; i >= 1; i--)
				{
					this.timeBuffers[i] = this.timeBuffers[i - 1];
				}
				this.timeBuffers[0] = timingBuffer;
				this.timeBuffers[0].Init();
			}
			this.timeBuffers[0].Add(num);
		}

		// Token: 0x0600009F RID: 159 RVA: 0x00007FA8 File Offset: 0x000061A8
		public void Put(JitterBuffer.JitterBufferPacket packet)
		{
			if (!this.reset_state)
			{
				for (int i = 0; i < 200; i++)
				{
					if (this.packets[i].data != null && this.packets[i].timestamp + this.packets[i].span <= this.pointer_timestamp)
					{
						if (this.DestroyBufferCallback != null)
						{
							this.DestroyBufferCallback(this.packets[i].data);
						}
						else
						{
							this.FreeBuffer(this.packets[i].data);
						}
						this.packets[i].data = null;
					}
				}
			}
			bool flag;
			if (!this.reset_state && packet.timestamp < this.next_stop)
			{
				this.UpdateTimings((int)packet.timestamp - (int)this.next_stop - this.buffer_margin);
				flag = true;
			}
			else
			{
				flag = false;
			}
			if (this.lost_count > 20)
			{
				this.Reset();
			}
			if (this.reset_state || packet.timestamp + packet.span + (long)this.delay_step >= this.pointer_timestamp)
			{
				int i = 0;
				while (i < 200 && this.packets[i].data != null)
				{
					i++;
				}
				if (i == 200)
				{
					long num = this.packets[0].timestamp;
					i = 0;
					for (int j = 1; j < 200; j++)
					{
						if (this.packets[i].data == null || this.packets[j].timestamp < num)
						{
							num = this.packets[j].timestamp;
							i = j;
						}
					}
					if (this.DestroyBufferCallback != null)
					{
						this.DestroyBufferCallback(this.packets[i].data);
					}
					else
					{
						this.FreeBuffer(this.packets[i].data);
					}
					this.packets[i].data = null;
				}
				if (this.DestroyBufferCallback != null)
				{
					this.packets[i].data = packet.data;
				}
				else
				{
					this.packets[i].data = this.AllocBuffer((long)packet.len);
					for (int j = 0; j < packet.len; j++)
					{
						this.packets[i].data[j] = packet.data[j];
					}
				}
				this.packets[i].timestamp = packet.timestamp;
				this.packets[i].span = packet.span;
				this.packets[i].len = packet.len;
				this.packets[i].sequence = packet.sequence;
				this.packets[i].user_data = packet.user_data;
				if (this.reset_state || flag)
				{
					this.arrival[i] = 0L;
					return;
				}
				this.arrival[i] = this.next_stop;
			}
		}

		// Token: 0x060000A0 RID: 160 RVA: 0x000082C0 File Offset: 0x000064C0
		public int Get(ref JitterBuffer.JitterBufferPacket packet, int desired_span, out int start_offset)
		{
			if (desired_span <= 0)
			{
				throw new ArgumentOutOfRangeException("desired_span");
			}
			start_offset = 0;
			int i;
			if (this.reset_state)
			{
				bool flag = false;
				long num = 0L;
				for (i = 0; i < 200; i++)
				{
					if (this.packets[i].data != null && (!flag || this.packets[i].timestamp < num))
					{
						num = this.packets[i].timestamp;
						flag = true;
					}
				}
				if (!flag)
				{
					packet.timestamp = 0L;
					packet.span = (long)this.interp_requested;
					return 1;
				}
				this.reset_state = false;
				this.pointer_timestamp = num;
				this.next_stop = num;
			}
			this.last_returned_timestamp = this.pointer_timestamp;
			if (this.interp_requested != 0)
			{
				packet.timestamp = this.pointer_timestamp;
				packet.span = (long)this.interp_requested;
				this.pointer_timestamp += (long)this.interp_requested;
				packet.len = 0;
				this.interp_requested = 0;
				this.buffered = packet.span - (long)desired_span;
				return 2;
			}
			i = 0;
			while (i < 200 && (this.packets[i].data == null || this.packets[i].timestamp != this.pointer_timestamp || this.packets[i].timestamp + this.packets[i].span < this.pointer_timestamp + (long)desired_span))
			{
				i++;
			}
			if (i == 200)
			{
				i = 0;
				while (i < 200 && (this.packets[i].data == null || this.packets[i].timestamp > this.pointer_timestamp || this.packets[i].timestamp + this.packets[i].span < this.pointer_timestamp + (long)desired_span))
				{
					i++;
				}
			}
			if (i == 200)
			{
				i = 0;
				while (i < 200 && (this.packets[i].data == null || this.packets[i].timestamp > this.pointer_timestamp || this.packets[i].timestamp + this.packets[i].span <= this.pointer_timestamp))
				{
					i++;
				}
			}
			if (i == 200)
			{
				bool flag2 = false;
				long num2 = 0L;
				long num3 = 0L;
				int num4 = 0;
				for (i = 0; i < 200; i++)
				{
					if (this.packets[i].data != null && this.packets[i].timestamp < this.pointer_timestamp + (long)desired_span && this.packets[i].timestamp >= this.pointer_timestamp && (!flag2 || this.packets[i].timestamp < num2 || (this.packets[i].timestamp == num2 && this.packets[i].span > num3)))
					{
						num2 = this.packets[i].timestamp;
						num3 = this.packets[i].span;
						num4 = i;
						flag2 = true;
					}
				}
				if (flag2)
				{
					i = num4;
				}
			}
			if (i != 200)
			{
				this.lost_count = 0;
				if (this.arrival[i] != 0L)
				{
					this.UpdateTimings((int)this.packets[i].timestamp - (int)this.arrival[i] - this.buffer_margin);
				}
				if (this.DestroyBufferCallback != null)
				{
					packet.data = this.packets[i].data;
					packet.len = this.packets[i].len;
				}
				else
				{
					if (this.packets[i].len <= packet.len)
					{
						packet.len = this.packets[i].len;
					}
					for (long num5 = 0L; num5 < (long)packet.len; num5 += 1L)
					{
						checked
						{
							packet.data[(int)((IntPtr)num5)] = this.packets[i].data[(int)((IntPtr)num5)];
						}
					}
					this.FreeBuffer(this.packets[i].data);
				}
				this.packets[i].data = null;
				int num6 = (int)this.packets[i].timestamp - (int)this.pointer_timestamp;
				if (start_offset != 0)
				{
					start_offset = num6;
				}
				packet.timestamp = this.packets[i].timestamp;
				this.last_returned_timestamp = packet.timestamp;
				packet.span = this.packets[i].span;
				packet.sequence = this.packets[i].sequence;
				packet.user_data = this.packets[i].user_data;
				packet.len = this.packets[i].len;
				this.pointer_timestamp = this.packets[i].timestamp + this.packets[i].span;
				this.buffered = packet.span - (long)desired_span;
				if (start_offset != 0)
				{
					this.buffered += (long)start_offset;
				}
				return 0;
			}
			this.lost_count++;
			short num7 = this.ComputeOptDelay();
			if (num7 < 0)
			{
				this.ShiftTimings(-num7);
				packet.timestamp = this.pointer_timestamp;
				packet.span = (long)(-(long)num7);
				packet.len = 0;
				this.buffered = packet.span - (long)desired_span;
				return 2;
			}
			packet.timestamp = this.pointer_timestamp;
			desired_span = JitterBuffer.RoundDown(desired_span, this.concealment_size);
			packet.span = (long)desired_span;
			this.pointer_timestamp += (long)desired_span;
			packet.len = 0;
			this.buffered = packet.span - (long)desired_span;
			return 1;
		}

		// Token: 0x060000A1 RID: 161 RVA: 0x000088A8 File Offset: 0x00006AA8
		private void ShiftTimings(short amount)
		{
			for (int i = 0; i < 3; i++)
			{
				for (int j = 0; j < this.timeBuffers[i].filled; j++)
				{
					this.timeBuffers[i].timing[j] += (int)amount;
				}
			}
		}

		// Token: 0x060000A2 RID: 162 RVA: 0x000088FC File Offset: 0x00006AFC
		private int UpdateDelay()
		{
			short num = this.ComputeOptDelay();
			if (num < 0)
			{
				this.ShiftTimings(-num);
				this.pointer_timestamp += (long)num;
				this.interp_requested = (int)(-(int)num);
			}
			else if (num > 0)
			{
				this.ShiftTimings(-num);
				this.pointer_timestamp += (long)num;
			}
			return (int)num;
		}

		// Token: 0x060000A3 RID: 163 RVA: 0x00008954 File Offset: 0x00006B54
		public void Tick()
		{
			if (this.auto_adjust)
			{
				this.UpdateDelay();
			}
			if (this.buffered >= 0L)
			{
				this.next_stop = this.pointer_timestamp - this.buffered;
			}
			else
			{
				this.next_stop = this.pointer_timestamp;
			}
			this.buffered = 0L;
		}

		// Token: 0x040000A0 RID: 160
		private const int MAX_BUFFER_SIZE = 200;

		// Token: 0x040000A1 RID: 161
		private const int MAX_TIMINGS = 40;

		// Token: 0x040000A2 RID: 162
		private const int MAX_BUFFERS = 3;

		// Token: 0x040000A3 RID: 163
		private const int TOP_DELAY = 40;

		// Token: 0x040000A4 RID: 164
		public const int JITTER_BUFFER_OK = 0;

		// Token: 0x040000A5 RID: 165
		public const int JITTER_BUFFER_MISSING = 1;

		// Token: 0x040000A6 RID: 166
		public const int JITTER_BUFFER_INSERTION = 2;

		// Token: 0x040000A7 RID: 167
		public const int JITTER_BUFFER_INTERNAL_ERROR = -1;

		// Token: 0x040000A8 RID: 168
		public const int JITTER_BUFFER_BAD_ARGUMENT = -2;

		// Token: 0x040000A9 RID: 169
		private long pointer_timestamp;

		// Token: 0x040000AA RID: 170
		private long last_returned_timestamp;

		// Token: 0x040000AB RID: 171
		private long next_stop;

		// Token: 0x040000AC RID: 172
		private long buffered;

		// Token: 0x040000AD RID: 173
		private JitterBuffer.JitterBufferPacket[] packets = new JitterBuffer.JitterBufferPacket[200];

		// Token: 0x040000AE RID: 174
		private long[] arrival = new long[200];

		// Token: 0x040000AF RID: 175
		public Action<byte[]> DestroyBufferCallback;

		// Token: 0x040000B0 RID: 176
		public int delay_step;

		// Token: 0x040000B1 RID: 177
		private int concealment_size;

		// Token: 0x040000B2 RID: 178
		private bool reset_state;

		// Token: 0x040000B3 RID: 179
		private int buffer_margin;

		// Token: 0x040000B4 RID: 180
		private int late_cutoff;

		// Token: 0x040000B5 RID: 181
		private int interp_requested;

		// Token: 0x040000B6 RID: 182
		private bool auto_adjust;

		// Token: 0x040000B7 RID: 183
		private JitterBuffer.TimingBuffer[] _tb = new JitterBuffer.TimingBuffer[3];

		// Token: 0x040000B8 RID: 184
		private JitterBuffer.TimingBuffer[] timeBuffers = new JitterBuffer.TimingBuffer[3];

		// Token: 0x040000B9 RID: 185
		public int window_size;

		// Token: 0x040000BA RID: 186
		private int subwindow_size;

		// Token: 0x040000BB RID: 187
		private int max_late_rate;

		// Token: 0x040000BC RID: 188
		public int latency_tradeoff;

		// Token: 0x040000BD RID: 189
		public int auto_tradeoff;

		// Token: 0x040000BE RID: 190
		private int lost_count;

		// Token: 0x02000016 RID: 22
		private class TimingBuffer
		{
			// Token: 0x060000A5 RID: 165 RVA: 0x000089E3 File Offset: 0x00006BE3
			internal void Init()
			{
				this.filled = 0;
				this.curr_count = 0;
			}

			// Token: 0x060000A6 RID: 166 RVA: 0x000089F4 File Offset: 0x00006BF4
			internal void Add(short timing)
			{
				if (this.filled >= 40 && (int)timing >= this.timing[this.filled - 1])
				{
					this.curr_count++;
					return;
				}
				int num = 0;
				while (num < this.filled && (int)timing >= this.timing[num])
				{
					num++;
				}
				if (num < this.filled)
				{
					int num2 = this.filled - num;
					if (this.filled == 40)
					{
						num2--;
					}
					Array.Copy(this.timing, num, this.timing, num + 1, num2);
					Array.Copy(this.counts, num, this.counts, num + 1, num2);
				}
				this.timing[num] = (int)timing;
				this.counts[num] = (short)this.curr_count;
				this.curr_count++;
				if (this.filled < 40)
				{
					this.filled++;
				}
			}

			// Token: 0x040000BF RID: 191
			public int filled;

			// Token: 0x040000C0 RID: 192
			public int curr_count;

			// Token: 0x040000C1 RID: 193
			public int[] timing = new int[40];

			// Token: 0x040000C2 RID: 194
			public short[] counts = new short[40];
		}

		// Token: 0x02000017 RID: 23
		public struct JitterBufferPacket
		{
			// Token: 0x040000C3 RID: 195
			public byte[] data;

			// Token: 0x040000C4 RID: 196
			public int len;

			// Token: 0x040000C5 RID: 197
			public long timestamp;

			// Token: 0x040000C6 RID: 198
			public long span;

			// Token: 0x040000C7 RID: 199
			public long sequence;

			// Token: 0x040000C8 RID: 200
			public long user_data;
		}
	}
}
