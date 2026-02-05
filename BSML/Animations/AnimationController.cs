using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;
using UnityEngine;

namespace BeatSaberMarkupLanguage.Animations
{
	// Token: 0x020000C3 RID: 195
	public class AnimationController : PersistentSingleton<AnimationController>
	{
		// Token: 0x06000407 RID: 1031 RVA: 0x00012825 File Offset: 0x00010A25
		private void Awake()
		{
			this.RegisteredAnimations = new ReadOnlyDictionary<string, AnimationControllerData>(this.registeredAnimations);
		}

		// Token: 0x06000408 RID: 1032 RVA: 0x00012838 File Offset: 0x00010A38
		public AnimationControllerData Register(string identifier, Texture2D tex, Rect[] uvs, float[] delays)
		{
			AnimationControllerData animationControllerData;
			if (!this.registeredAnimations.TryGetValue(identifier, out animationControllerData))
			{
				animationControllerData = new AnimationControllerData(tex, uvs, delays);
				this.registeredAnimations.Add(identifier, animationControllerData);
			}
			else
			{
				Object.Destroy(tex);
			}
			return animationControllerData;
		}

		// Token: 0x06000409 RID: 1033 RVA: 0x00012875 File Offset: 0x00010A75
		public void InitializeLoadingAnimation()
		{
			AnimationLoader.Process(AnimationType.APNG, Utilities.GetResource(Assembly.GetExecutingAssembly(), "BeatSaberMarkupLanguage.Resources.loading.apng"), delegate(Texture2D tex, Rect[] uvs, float[] delays, int width, int height)
			{
				this.loadingAnimation = new AnimationControllerData(tex, uvs, delays);
				this.registeredAnimations.Add("LOADING_ANIMATION", this.loadingAnimation);
			});
		}

		// Token: 0x0600040A RID: 1034 RVA: 0x00012898 File Offset: 0x00010A98
		public void Update()
		{
			DateTime utcNow = DateTime.UtcNow;
			foreach (AnimationControllerData animationControllerData in this.registeredAnimations.Values)
			{
				if (animationControllerData.IsPlaying)
				{
					animationControllerData.CheckFrame(utcNow);
				}
			}
		}

		// Token: 0x04000142 RID: 322
		private Dictionary<string, AnimationControllerData> registeredAnimations = new Dictionary<string, AnimationControllerData>();

		// Token: 0x04000143 RID: 323
		public ReadOnlyDictionary<string, AnimationControllerData> RegisteredAnimations;

		// Token: 0x04000144 RID: 324
		public AnimationControllerData loadingAnimation;
	}
}
