using System;
using System.Drawing;
using BeatSaberMarkupLanguage.Animations.APNG.Chunks;

namespace BeatSaberMarkupLanguage.Animations
{
	// Token: 0x020000C9 RID: 201
	public interface IAnimatedImage
	{
		// Token: 0x17000101 RID: 257
		Bitmap this[int index] { get; }

		// Token: 0x17000102 RID: 258
		// (get) Token: 0x06000431 RID: 1073
		// (set) Token: 0x06000432 RID: 1074
		int FrameRate { get; set; }

		// Token: 0x17000103 RID: 259
		// (get) Token: 0x06000433 RID: 1075
		int FrameCount { get; }

		// Token: 0x17000104 RID: 260
		// (get) Token: 0x06000434 RID: 1076
		// (set) Token: 0x06000435 RID: 1077
		int PlayCount { get; set; }

		// Token: 0x06000436 RID: 1078
		int GetFrameRate(int index);

		// Token: 0x06000437 RID: 1079
		void SetFrameRate(int index, int frameRate);

		// Token: 0x06000438 RID: 1080
		DisposeOps GetDisposeOperationFor(int index);

		// Token: 0x06000439 RID: 1081
		BlendOps GetBlendOperationFor(int index);

		// Token: 0x0600043A RID: 1082
		Bitmap GetDefaultImage();

		// Token: 0x17000105 RID: 261
		// (get) Token: 0x0600043B RID: 1083
		// (set) Token: 0x0600043C RID: 1084
		Size ViewSize { get; set; }

		// Token: 0x17000106 RID: 262
		// (get) Token: 0x0600043D RID: 1085
		// (set) Token: 0x0600043E RID: 1086
		Size ActualSize { get; set; }
	}
}
