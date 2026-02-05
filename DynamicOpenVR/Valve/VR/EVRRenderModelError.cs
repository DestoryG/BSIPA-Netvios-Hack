using System;

namespace Valve.VR
{
	// Token: 0x0200005E RID: 94
	public enum EVRRenderModelError
	{
		// Token: 0x040004FB RID: 1275
		None,
		// Token: 0x040004FC RID: 1276
		Loading = 100,
		// Token: 0x040004FD RID: 1277
		NotSupported = 200,
		// Token: 0x040004FE RID: 1278
		InvalidArg = 300,
		// Token: 0x040004FF RID: 1279
		InvalidModel,
		// Token: 0x04000500 RID: 1280
		NoShapes,
		// Token: 0x04000501 RID: 1281
		MultipleShapes,
		// Token: 0x04000502 RID: 1282
		TooManyVertices,
		// Token: 0x04000503 RID: 1283
		MultipleTextures,
		// Token: 0x04000504 RID: 1284
		BufferTooSmall,
		// Token: 0x04000505 RID: 1285
		NotEnoughNormals,
		// Token: 0x04000506 RID: 1286
		NotEnoughTexCoords,
		// Token: 0x04000507 RID: 1287
		InvalidTexture = 400
	}
}
