using System;
using System.Collections;
using UnityEngine;

namespace BeatSaberMarkupLanguage.Animations
{
	// Token: 0x020000C1 RID: 193
	public class AnimationLoader
	{
		// Token: 0x060003FD RID: 1021 RVA: 0x000126CC File Offset: 0x000108CC
		public static void Process(AnimationType type, byte[] data, Action<Texture2D, Rect[], float[], int, int> callback)
		{
			if (type == AnimationType.GIF)
			{
				PersistentSingleton<SharedCoroutineStarter>.instance.StartCoroutine(GIFUnityDecoder.Process(data, delegate(AnimationInfo animationInfo)
				{
					PersistentSingleton<SharedCoroutineStarter>.instance.StartCoroutine(AnimationLoader.ProcessAnimationInfo(animationInfo, callback));
				}));
				return;
			}
			if (type != AnimationType.APNG)
			{
				return;
			}
			PersistentSingleton<SharedCoroutineStarter>.instance.StartCoroutine(APNGUnityDecoder.Process(data, delegate(AnimationInfo animationInfo)
			{
				PersistentSingleton<SharedCoroutineStarter>.instance.StartCoroutine(AnimationLoader.ProcessAnimationInfo(animationInfo, callback));
			}));
		}

		// Token: 0x060003FE RID: 1022 RVA: 0x00012729 File Offset: 0x00010929
		public static IEnumerator ProcessAnimationInfo(AnimationInfo animationInfo, Action<Texture2D, Rect[], float[], int, int> callback)
		{
			int textureSize = 2048;
			int width = 0;
			int height = 0;
			Texture2D texture = null;
			Texture2D[] texList = new Texture2D[animationInfo.frameCount];
			float[] delays = new float[animationInfo.frameCount];
			int i;
			Func<bool> <>9__0;
			int j;
			for (i = 0; i < animationInfo.frameCount; i = j + 1)
			{
				if (animationInfo.frames.Count <= i)
				{
					Func<bool> func;
					if ((func = <>9__0) == null)
					{
						func = (<>9__0 = () => animationInfo.frames.Count > i);
					}
					yield return new WaitUntil(func);
				}
				if (texture == null)
				{
					textureSize = AnimationLoader.GetTextureSize(animationInfo, i);
					texture = new Texture2D(animationInfo.frames[i].width, animationInfo.frames[i].height);
				}
				FrameInfo frameInfo = animationInfo.frames[i];
				delays[i] = (float)frameInfo.delay;
				Texture2D frameTexture = new Texture2D(frameInfo.width, frameInfo.height, 4, false);
				frameTexture.wrapMode = 1;
				try
				{
					frameTexture.SetPixels32(frameInfo.colors);
					frameTexture.Apply(i == 0);
				}
				catch
				{
					yield break;
				}
				yield return null;
				texList[i] = frameTexture;
				if (i == 0)
				{
					width = animationInfo.frames[i].width;
					height = animationInfo.frames[i].height;
				}
				frameTexture = null;
				j = i;
			}
			Rect[] atlas = texture.PackTextures(texList, 2, textureSize, true);
			Texture2D[] array = texList;
			for (j = 0; j < array.Length; j++)
			{
				Object.Destroy(array[j]);
			}
			yield return null;
			if (callback != null)
			{
				callback(texture, atlas, delays, width, height);
			}
			yield break;
		}

		// Token: 0x060003FF RID: 1023 RVA: 0x00012740 File Offset: 0x00010940
		private static int GetTextureSize(AnimationInfo frameInfo, int i)
		{
			int num = 2;
			int num3;
			int num4;
			for (;;)
			{
				int num2 = frameInfo.frameCount;
				if (num2 % num == 0)
				{
					num2 += num2 % num;
				}
				num3 = num2 / num;
				num4 = num2 / num3;
				if (num3 <= num4)
				{
					break;
				}
				num += 2;
			}
			int num5 = Mathf.Clamp(num3 * frameInfo.frames[i].width, 0, 2048);
			int num6 = Mathf.Clamp(num4 * frameInfo.frames[i].height, 0, 2048);
			return Mathf.Max(num5, num6);
		}
	}
}
