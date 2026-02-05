using System;
using System.Runtime.InteropServices;

namespace Valve.VR
{
	// Token: 0x020000B8 RID: 184
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct RenderModel_TextureMap_t_Packed
	{
		// Token: 0x06000168 RID: 360 RVA: 0x000042DD File Offset: 0x000024DD
		public RenderModel_TextureMap_t_Packed(RenderModel_TextureMap_t unpacked)
		{
			this.unWidth = unpacked.unWidth;
			this.unHeight = unpacked.unHeight;
			this.rubTextureMapData = unpacked.rubTextureMapData;
		}

		// Token: 0x06000169 RID: 361 RVA: 0x00004303 File Offset: 0x00002503
		public void Unpack(ref RenderModel_TextureMap_t unpacked)
		{
			unpacked.unWidth = this.unWidth;
			unpacked.unHeight = this.unHeight;
			unpacked.rubTextureMapData = this.rubTextureMapData;
		}

		// Token: 0x040006AB RID: 1707
		public ushort unWidth;

		// Token: 0x040006AC RID: 1708
		public ushort unHeight;

		// Token: 0x040006AD RID: 1709
		public IntPtr rubTextureMapData;
	}
}
