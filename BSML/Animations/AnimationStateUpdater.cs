using System;
using UnityEngine;
using UnityEngine.UI;

namespace BeatSaberMarkupLanguage.Animations
{
	// Token: 0x020000C2 RID: 194
	public class AnimationStateUpdater : MonoBehaviour
	{
		// Token: 0x170000F9 RID: 249
		// (get) Token: 0x06000401 RID: 1025 RVA: 0x000127BD File Offset: 0x000109BD
		// (set) Token: 0x06000402 RID: 1026 RVA: 0x000127C5 File Offset: 0x000109C5
		public AnimationControllerData controllerData
		{
			get
			{
				return this._controllerData;
			}
			set
			{
				if (this._controllerData != null)
				{
					this.OnDisable();
				}
				this._controllerData = value;
				if (base.isActiveAndEnabled)
				{
					this.OnEnable();
				}
			}
		}

		// Token: 0x06000403 RID: 1027 RVA: 0x000127EA File Offset: 0x000109EA
		private void OnEnable()
		{
			AnimationControllerData controllerData = this.controllerData;
			if (controllerData == null)
			{
				return;
			}
			controllerData.activeImages.Add(this.image);
		}

		// Token: 0x06000404 RID: 1028 RVA: 0x00012807 File Offset: 0x00010A07
		private void OnDisable()
		{
			AnimationControllerData controllerData = this.controllerData;
			if (controllerData == null)
			{
				return;
			}
			controllerData.activeImages.Remove(this.image);
		}

		// Token: 0x06000405 RID: 1029 RVA: 0x00012807 File Offset: 0x00010A07
		private void OnDestroy()
		{
			AnimationControllerData controllerData = this.controllerData;
			if (controllerData == null)
			{
				return;
			}
			controllerData.activeImages.Remove(this.image);
		}

		// Token: 0x04000140 RID: 320
		private AnimationControllerData _controllerData;

		// Token: 0x04000141 RID: 321
		public Image image;
	}
}
