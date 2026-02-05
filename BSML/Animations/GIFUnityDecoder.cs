using System;
using System.Collections;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace BeatSaberMarkupLanguage.Animations
{
	// Token: 0x020000CB RID: 203
	public class GIFUnityDecoder
	{
		// Token: 0x06000465 RID: 1125 RVA: 0x00013478 File Offset: 0x00011678
		public static IEnumerator Process(byte[] gifData, Action<AnimationInfo> callback)
		{
			AnimationInfo animationInfo = new AnimationInfo();
			Task.Run(delegate
			{
				GIFUnityDecoder.ProcessingThread(gifData, animationInfo);
			});
			yield return new WaitUntil(() => animationInfo.initialized);
			if (callback != null)
			{
				callback(animationInfo);
			}
			yield break;
		}

		// Token: 0x06000466 RID: 1126 RVA: 0x00013490 File Offset: 0x00011690
		private static void ProcessingThread(byte[] gifData, AnimationInfo animationInfo)
		{
			Image image = Image.FromStream(new MemoryStream(gifData));
			FrameDimension frameDimension = new FrameDimension(image.FrameDimensionsList[0]);
			int frameCount = image.GetFrameCount(frameDimension);
			animationInfo.frameCount = frameCount;
			animationInfo.initialized = true;
			int num = 0;
			int num2 = -1;
			for (int i = 0; i < frameCount; i++)
			{
				image.SelectActiveFrame(frameDimension, i);
				Bitmap bitmap = new Bitmap(image.Width, image.Height);
				Graphics.FromImage(bitmap).DrawImage(image, Point.Empty);
				LockBitmap lockBitmap = new LockBitmap(bitmap);
				lockBitmap.LockBits();
				FrameInfo frameInfo = new FrameInfo(bitmap.Width, bitmap.Height);
				if (frameInfo.colors == null)
				{
					frameInfo.colors = new Color32[lockBitmap.Height * lockBitmap.Width];
				}
				for (int j = 0; j < lockBitmap.Width; j++)
				{
					for (int k = 0; k < lockBitmap.Height; k++)
					{
						Color pixel = lockBitmap.GetPixel(j, k);
						frameInfo.colors[(lockBitmap.Height - k - 1) * lockBitmap.Width + j] = new Color32(pixel.R, pixel.G, pixel.B, pixel.A);
					}
				}
				int num3 = BitConverter.ToInt32(image.GetPropertyItem(20736).Value, num);
				if (num2 == -1)
				{
					num2 = num3;
				}
				frameInfo.delay = num3 * 10;
				animationInfo.frames.Add(frameInfo);
				num += 4;
				Thread.Sleep(0);
			}
		}
	}
}
