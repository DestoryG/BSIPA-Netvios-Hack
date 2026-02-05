using System;
using System.Collections;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BeatSaberMarkupLanguage.Animations.APNG;
using BeatSaberMarkupLanguage.Animations.APNG.Chunks;
using UnityEngine;

namespace BeatSaberMarkupLanguage.Animations
{
	// Token: 0x020000C5 RID: 197
	public class APNGUnityDecoder
	{
		// Token: 0x06000411 RID: 1041 RVA: 0x00012B5C File Offset: 0x00010D5C
		public static IEnumerator Process(byte[] apngData, Action<AnimationInfo> callback)
		{
			AnimationInfo animationInfo = new AnimationInfo();
			Task.Run(delegate
			{
				APNGUnityDecoder.ProcessingThread(apngData, animationInfo);
			});
			yield return new WaitUntil(() => animationInfo.initialized);
			if (callback != null)
			{
				callback(animationInfo);
			}
			yield break;
		}

		// Token: 0x06000412 RID: 1042 RVA: 0x00012B74 File Offset: 0x00010D74
		private static void ProcessingThread(byte[] apngData, AnimationInfo animationInfo)
		{
			APNG apng = APNG.FromStream(new MemoryStream(apngData));
			int frameCount = apng.FrameCount;
			animationInfo.frameCount = frameCount;
			animationInfo.initialized = true;
			for (int i = 0; i < frameCount; i++)
			{
				Frame frame = apng.Frames[i];
				Bitmap bitmap = frame.ToBitmap();
				LockBitmap lockBitmap = new LockBitmap(bitmap);
				lockBitmap.LockBits();
				FrameInfo frameInfo = new FrameInfo(bitmap.Width, bitmap.Height);
				for (int j = 0; j < frameInfo.width; j++)
				{
					for (int k = 0; k < frameInfo.height; k++)
					{
						Color pixel = lockBitmap.GetPixel(j, k);
						Color32 color = default(Color32);
						if (i > 0)
						{
							color = animationInfo.frames.Last<FrameInfo>().colors[(frameInfo.height - k - 1) * frameInfo.width + j];
						}
						if (frame.fcTLChunk.BlendOp == BlendOps.APNGBlendOpSource)
						{
							frameInfo.colors[(frameInfo.height - k - 1) * frameInfo.width + j] = new Color32(pixel.R, pixel.G, pixel.B, pixel.A);
						}
						if (frame.fcTLChunk.BlendOp == BlendOps.APNGBlendOpOver)
						{
							float num = (float)pixel.A / 255f + (1f - (float)pixel.A / 255f) * ((float)color.a / 255f);
							float num2 = ((float)pixel.A / 255f * ((float)pixel.R / 255f) + (1f - (float)pixel.A / 255f) * ((float)color.a / 255f) * ((float)color.r / 255f)) / num;
							float num3 = ((float)pixel.A / 255f * ((float)pixel.G / 255f) + (1f - (float)pixel.A / 255f) * ((float)color.a / 255f) * ((float)color.g / 255f)) / num;
							float num4 = ((float)pixel.A / 255f * ((float)pixel.B / 255f) + (1f - (float)pixel.A / 255f) * ((float)color.a / 255f) * ((float)color.b / 255f)) / num;
							frameInfo.colors[(frameInfo.height - k - 1) * frameInfo.width + j] = new Color32((byte)(num2 * 255f), (byte)(num3 * 255f), (byte)(num4 * 255f), (byte)(num * 255f));
						}
					}
				}
				frameInfo.delay = frame.FrameRate;
				animationInfo.frames.Add(frameInfo);
			}
		}
	}
}
