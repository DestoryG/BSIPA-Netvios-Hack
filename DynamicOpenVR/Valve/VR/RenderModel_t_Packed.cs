using System;
using System.Runtime.InteropServices;

namespace Valve.VR
{
	// Token: 0x020000BA RID: 186
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct RenderModel_t_Packed
	{
		// Token: 0x0600016A RID: 362 RVA: 0x00004329 File Offset: 0x00002529
		public RenderModel_t_Packed(RenderModel_t unpacked)
		{
			this.rVertexData = unpacked.rVertexData;
			this.unVertexCount = unpacked.unVertexCount;
			this.rIndexData = unpacked.rIndexData;
			this.unTriangleCount = unpacked.unTriangleCount;
			this.diffuseTextureId = unpacked.diffuseTextureId;
		}

		// Token: 0x0600016B RID: 363 RVA: 0x00004367 File Offset: 0x00002567
		public void Unpack(ref RenderModel_t unpacked)
		{
			unpacked.rVertexData = this.rVertexData;
			unpacked.unVertexCount = this.unVertexCount;
			unpacked.rIndexData = this.rIndexData;
			unpacked.unTriangleCount = this.unTriangleCount;
			unpacked.diffuseTextureId = this.diffuseTextureId;
		}

		// Token: 0x040006B3 RID: 1715
		public IntPtr rVertexData;

		// Token: 0x040006B4 RID: 1716
		public uint unVertexCount;

		// Token: 0x040006B5 RID: 1717
		public IntPtr rIndexData;

		// Token: 0x040006B6 RID: 1718
		public uint unTriangleCount;

		// Token: 0x040006B7 RID: 1719
		public int diffuseTextureId;
	}
}
