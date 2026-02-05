using System;

namespace Valve.VR
{
	// Token: 0x02000082 RID: 130
	public struct VRVulkanTextureData_t
	{
		// Token: 0x040005CB RID: 1483
		public ulong m_nImage;

		// Token: 0x040005CC RID: 1484
		public IntPtr m_pDevice;

		// Token: 0x040005CD RID: 1485
		public IntPtr m_pPhysicalDevice;

		// Token: 0x040005CE RID: 1486
		public IntPtr m_pInstance;

		// Token: 0x040005CF RID: 1487
		public IntPtr m_pQueue;

		// Token: 0x040005D0 RID: 1488
		public uint m_nQueueFamilyIndex;

		// Token: 0x040005D1 RID: 1489
		public uint m_nWidth;

		// Token: 0x040005D2 RID: 1490
		public uint m_nHeight;

		// Token: 0x040005D3 RID: 1491
		public uint m_nFormat;

		// Token: 0x040005D4 RID: 1492
		public uint m_nSampleCount;
	}
}
