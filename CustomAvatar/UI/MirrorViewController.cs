using System;
using CustomAvatar.StereoRendering;
using HMUI;
using UnityEngine;

namespace CustomAvatar.UI
{
	// Token: 0x02000033 RID: 51
	internal class MirrorViewController : ViewController
	{
		// Token: 0x06000104 RID: 260 RVA: 0x00007398 File Offset: 0x00005598
		protected override void DidActivate(bool firstActivation, ViewController.ActivationType activationType)
		{
			this._mirrorContainer = new GameObject();
			base.DidActivate(firstActivation, activationType);
			if (firstActivation)
			{
				base.StartCoroutine(MirrorHelper.SpawnMirror(MirrorViewController.kMirrorPosition, MirrorViewController.kMirrorRotation, MirrorViewController.kMirrorScale, this._mirrorContainer.transform));
			}
		}

		// Token: 0x06000105 RID: 261 RVA: 0x000073E7 File Offset: 0x000055E7
		protected override void DidDeactivate(ViewController.DeactivationType deactivationType)
		{
			base.DidDeactivate(deactivationType);
			Object.Destroy(this._mirrorContainer);
		}

		// Token: 0x0400017F RID: 383
		private static readonly Vector3 kMirrorPosition = new Vector3(0f, 0f, 1.5f);

		// Token: 0x04000180 RID: 384
		private static readonly Quaternion kMirrorRotation = Quaternion.Euler(-90f, 0f, 0f);

		// Token: 0x04000181 RID: 385
		private static readonly Vector3 kMirrorScale = new Vector3(0.5f, 1f, 0.25f);

		// Token: 0x04000182 RID: 386
		private GameObject _mirrorContainer;
	}
}
