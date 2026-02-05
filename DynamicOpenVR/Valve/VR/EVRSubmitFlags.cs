using System;

namespace Valve.VR
{
	// Token: 0x0200002F RID: 47
	public enum EVRSubmitFlags
	{
		// Token: 0x04000244 RID: 580
		Submit_Default,
		// Token: 0x04000245 RID: 581
		Submit_LensDistortionAlreadyApplied,
		// Token: 0x04000246 RID: 582
		Submit_GlRenderBuffer,
		// Token: 0x04000247 RID: 583
		Submit_Reserved = 4,
		// Token: 0x04000248 RID: 584
		Submit_TextureWithPose = 8,
		// Token: 0x04000249 RID: 585
		Submit_TextureWithDepth = 16
	}
}
