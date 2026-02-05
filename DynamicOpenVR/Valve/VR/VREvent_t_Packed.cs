using System;
using System.Runtime.InteropServices;

namespace Valve.VR
{
	// Token: 0x020000A3 RID: 163
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct VREvent_t_Packed
	{
		// Token: 0x06000164 RID: 356 RVA: 0x0000419B File Offset: 0x0000239B
		public VREvent_t_Packed(VREvent_t unpacked)
		{
			this.eventType = unpacked.eventType;
			this.trackedDeviceIndex = unpacked.trackedDeviceIndex;
			this.eventAgeSeconds = unpacked.eventAgeSeconds;
			this.data = unpacked.data;
		}

		// Token: 0x06000165 RID: 357 RVA: 0x000041CD File Offset: 0x000023CD
		public void Unpack(ref VREvent_t unpacked)
		{
			unpacked.eventType = this.eventType;
			unpacked.trackedDeviceIndex = this.trackedDeviceIndex;
			unpacked.eventAgeSeconds = this.eventAgeSeconds;
			unpacked.data = this.data;
		}

		// Token: 0x0400062E RID: 1582
		public uint eventType;

		// Token: 0x0400062F RID: 1583
		public uint trackedDeviceIndex;

		// Token: 0x04000630 RID: 1584
		public float eventAgeSeconds;

		// Token: 0x04000631 RID: 1585
		public VREvent_Data_t data;
	}
}
