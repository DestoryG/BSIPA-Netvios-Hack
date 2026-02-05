using System;
using System.Globalization;

namespace System.Net
{
	// Token: 0x0200021B RID: 539
	internal class FrameHeader
	{
		// Token: 0x060013D2 RID: 5074 RVA: 0x00069114 File Offset: 0x00067314
		public FrameHeader()
		{
			this._MessageId = 22;
			this._MajorV = 1;
			this._MinorV = 0;
			this._PayloadSize = -1;
		}

		// Token: 0x060013D3 RID: 5075 RVA: 0x00069139 File Offset: 0x00067339
		public FrameHeader(int messageId, int majorV, int minorV)
		{
			this._MessageId = messageId;
			this._MajorV = majorV;
			this._MinorV = minorV;
			this._PayloadSize = -1;
		}

		// Token: 0x17000429 RID: 1065
		// (get) Token: 0x060013D4 RID: 5076 RVA: 0x0006915D File Offset: 0x0006735D
		public int Size
		{
			get
			{
				return 5;
			}
		}

		// Token: 0x1700042A RID: 1066
		// (get) Token: 0x060013D5 RID: 5077 RVA: 0x00069160 File Offset: 0x00067360
		public int MaxMessageSize
		{
			get
			{
				return 65535;
			}
		}

		// Token: 0x1700042B RID: 1067
		// (get) Token: 0x060013D6 RID: 5078 RVA: 0x00069167 File Offset: 0x00067367
		// (set) Token: 0x060013D7 RID: 5079 RVA: 0x0006916F File Offset: 0x0006736F
		public int MessageId
		{
			get
			{
				return this._MessageId;
			}
			set
			{
				this._MessageId = value;
			}
		}

		// Token: 0x1700042C RID: 1068
		// (get) Token: 0x060013D8 RID: 5080 RVA: 0x00069178 File Offset: 0x00067378
		public int MajorV
		{
			get
			{
				return this._MajorV;
			}
		}

		// Token: 0x1700042D RID: 1069
		// (get) Token: 0x060013D9 RID: 5081 RVA: 0x00069180 File Offset: 0x00067380
		public int MinorV
		{
			get
			{
				return this._MinorV;
			}
		}

		// Token: 0x1700042E RID: 1070
		// (get) Token: 0x060013DA RID: 5082 RVA: 0x00069188 File Offset: 0x00067388
		// (set) Token: 0x060013DB RID: 5083 RVA: 0x00069190 File Offset: 0x00067390
		public int PayloadSize
		{
			get
			{
				return this._PayloadSize;
			}
			set
			{
				if (value > this.MaxMessageSize)
				{
					throw new ArgumentException(SR.GetString("net_frame_max_size", new object[]
					{
						this.MaxMessageSize.ToString(NumberFormatInfo.InvariantInfo),
						value.ToString(NumberFormatInfo.InvariantInfo)
					}), "PayloadSize");
				}
				this._PayloadSize = value;
			}
		}

		// Token: 0x060013DC RID: 5084 RVA: 0x000691F0 File Offset: 0x000673F0
		public void CopyTo(byte[] dest, int start)
		{
			dest[start++] = (byte)this._MessageId;
			dest[start++] = (byte)this._MajorV;
			dest[start++] = (byte)this._MinorV;
			dest[start++] = (byte)((this._PayloadSize >> 8) & 255);
			dest[start] = (byte)(this._PayloadSize & 255);
		}

		// Token: 0x060013DD RID: 5085 RVA: 0x00069254 File Offset: 0x00067454
		public void CopyFrom(byte[] bytes, int start, FrameHeader verifier)
		{
			this._MessageId = (int)bytes[start++];
			this._MajorV = (int)bytes[start++];
			this._MinorV = (int)bytes[start++];
			this._PayloadSize = ((int)bytes[start++] << 8) | (int)bytes[start];
			if (verifier.MessageId != -1 && this.MessageId != verifier.MessageId)
			{
				throw new InvalidOperationException(SR.GetString("net_io_header_id", new object[] { "MessageId", this.MessageId, verifier.MessageId }));
			}
			if (verifier.MajorV != -1 && this.MajorV != verifier.MajorV)
			{
				throw new InvalidOperationException(SR.GetString("net_io_header_id", new object[] { "MajorV", this.MajorV, verifier.MajorV }));
			}
			if (verifier.MinorV != -1 && this.MinorV != verifier.MinorV)
			{
				throw new InvalidOperationException(SR.GetString("net_io_header_id", new object[] { "MinorV", this.MinorV, verifier.MinorV }));
			}
		}

		// Token: 0x040015D8 RID: 5592
		public const int IgnoreValue = -1;

		// Token: 0x040015D9 RID: 5593
		public const int HandshakeDoneId = 20;

		// Token: 0x040015DA RID: 5594
		public const int HandshakeErrId = 21;

		// Token: 0x040015DB RID: 5595
		public const int HandshakeId = 22;

		// Token: 0x040015DC RID: 5596
		public const int DefaultMajorV = 1;

		// Token: 0x040015DD RID: 5597
		public const int DefaultMinorV = 0;

		// Token: 0x040015DE RID: 5598
		private int _MessageId;

		// Token: 0x040015DF RID: 5599
		private int _MajorV;

		// Token: 0x040015E0 RID: 5600
		private int _MinorV;

		// Token: 0x040015E1 RID: 5601
		private int _PayloadSize;
	}
}
