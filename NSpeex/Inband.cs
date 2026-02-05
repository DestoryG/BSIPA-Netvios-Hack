using System;

namespace NSpeex
{
	// Token: 0x02000028 RID: 40
	internal class Inband
	{
		// Token: 0x06000111 RID: 273 RVA: 0x00014766 File Offset: 0x00012966
		public Inband(Stereo stereo)
		{
			this.stereo = stereo;
		}

		// Token: 0x06000112 RID: 274 RVA: 0x00014778 File Offset: 0x00012978
		public void SpeexInbandRequest(Bits bits)
		{
			switch (bits.Unpack(4))
			{
			case 0:
				bits.Advance(1);
				return;
			case 1:
				bits.Advance(1);
				return;
			case 2:
				bits.Advance(4);
				return;
			case 3:
				bits.Advance(4);
				return;
			case 4:
				bits.Advance(4);
				return;
			case 5:
				bits.Advance(4);
				return;
			case 6:
				bits.Advance(4);
				return;
			case 7:
				bits.Advance(4);
				return;
			case 8:
				bits.Advance(8);
				return;
			case 9:
				this.stereo.Init(bits);
				return;
			case 10:
				bits.Advance(16);
				return;
			case 11:
				bits.Advance(16);
				return;
			case 12:
				bits.Advance(32);
				return;
			case 13:
				bits.Advance(32);
				return;
			case 14:
				bits.Advance(64);
				return;
			case 15:
				bits.Advance(64);
				return;
			default:
				return;
			}
		}

		// Token: 0x06000113 RID: 275 RVA: 0x00014860 File Offset: 0x00012A60
		public void UserInbandRequest(Bits bits)
		{
			int num = bits.Unpack(4);
			bits.Advance(5 + 8 * num);
		}

		// Token: 0x04000134 RID: 308
		private Stereo stereo;
	}
}
