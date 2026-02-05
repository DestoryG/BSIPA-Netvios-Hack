using System;

namespace NSpeex
{
	// Token: 0x02000018 RID: 24
	public class SpeexJitterBuffer
	{
		// Token: 0x060000A8 RID: 168 RVA: 0x00008AF8 File Offset: 0x00006CF8
		public SpeexJitterBuffer(SpeexDecoder decoder)
		{
			if (decoder == null)
			{
				throw new ArgumentNullException("decoder");
			}
			this.decoder = decoder;
			this.inPacket.sequence = 0L;
			this.inPacket.span = 1L;
			this.inPacket.timestamp = 1L;
			this.buffer.DestroyBufferCallback = delegate(byte[] x)
			{
			};
			this.buffer.Init(1);
		}

		// Token: 0x060000A9 RID: 169 RVA: 0x00008B88 File Offset: 0x00006D88
		public void Get(short[] decodedFrame)
		{
			if (decodedFrame == null)
			{
				throw new ArgumentNullException("decodedFrame");
			}
			if (this.outPacket.data == null)
			{
				this.outPacket.data = new byte[decodedFrame.Length * 2];
			}
			else
			{
				Array.Clear(this.outPacket.data, 0, this.outPacket.data.Length);
			}
			this.outPacket.len = this.outPacket.data.Length;
			int num;
			if (this.buffer.Get(ref this.outPacket, 1, out num) != 0)
			{
				this.decoder.Decode(null, 0, 0, decodedFrame, 0, true);
			}
			else
			{
				this.decoder.Decode(this.outPacket.data, 0, this.outPacket.len, decodedFrame, 0, false);
			}
			this.buffer.Tick();
		}

		// Token: 0x060000AA RID: 170 RVA: 0x00008C58 File Offset: 0x00006E58
		public void Put(byte[] frameData)
		{
			if (frameData == null)
			{
				throw new ArgumentNullException("frameData");
			}
			this.inPacket.data = frameData;
			this.inPacket.len = frameData.Length;
			this.inPacket.timestamp = this.inPacket.timestamp + 1L;
			this.buffer.Put(this.inPacket);
		}

		// Token: 0x040000C9 RID: 201
		private readonly SpeexDecoder decoder;

		// Token: 0x040000CA RID: 202
		private readonly JitterBuffer buffer = new JitterBuffer();

		// Token: 0x040000CB RID: 203
		private JitterBuffer.JitterBufferPacket outPacket;

		// Token: 0x040000CC RID: 204
		private JitterBuffer.JitterBufferPacket inPacket;
	}
}
