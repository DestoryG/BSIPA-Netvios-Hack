using System;
using System.Runtime.InteropServices;

namespace Valve.VR
{
	// Token: 0x020000A7 RID: 167
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct VRControllerState_t_Packed
	{
		// Token: 0x06000166 RID: 358 RVA: 0x00004200 File Offset: 0x00002400
		public VRControllerState_t_Packed(VRControllerState_t unpacked)
		{
			this.unPacketNum = unpacked.unPacketNum;
			this.ulButtonPressed = unpacked.ulButtonPressed;
			this.ulButtonTouched = unpacked.ulButtonTouched;
			this.rAxis0 = unpacked.rAxis0;
			this.rAxis1 = unpacked.rAxis1;
			this.rAxis2 = unpacked.rAxis2;
			this.rAxis3 = unpacked.rAxis3;
			this.rAxis4 = unpacked.rAxis4;
		}

		// Token: 0x06000167 RID: 359 RVA: 0x00004270 File Offset: 0x00002470
		public void Unpack(ref VRControllerState_t unpacked)
		{
			unpacked.unPacketNum = this.unPacketNum;
			unpacked.ulButtonPressed = this.ulButtonPressed;
			unpacked.ulButtonTouched = this.ulButtonTouched;
			unpacked.rAxis0 = this.rAxis0;
			unpacked.rAxis1 = this.rAxis1;
			unpacked.rAxis2 = this.rAxis2;
			unpacked.rAxis3 = this.rAxis3;
			unpacked.rAxis4 = this.rAxis4;
		}

		// Token: 0x0400063E RID: 1598
		public uint unPacketNum;

		// Token: 0x0400063F RID: 1599
		public ulong ulButtonPressed;

		// Token: 0x04000640 RID: 1600
		public ulong ulButtonTouched;

		// Token: 0x04000641 RID: 1601
		public VRControllerAxis_t rAxis0;

		// Token: 0x04000642 RID: 1602
		public VRControllerAxis_t rAxis1;

		// Token: 0x04000643 RID: 1603
		public VRControllerAxis_t rAxis2;

		// Token: 0x04000644 RID: 1604
		public VRControllerAxis_t rAxis3;

		// Token: 0x04000645 RID: 1605
		public VRControllerAxis_t rAxis4;
	}
}
