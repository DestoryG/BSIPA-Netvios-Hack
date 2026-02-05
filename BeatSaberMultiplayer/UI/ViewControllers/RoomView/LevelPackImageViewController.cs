using System;
using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.ViewControllers;
using BeatSaberMultiplayer.Helper;
using HMUI;
using UnityEngine;
using UnityEngine.UI;

namespace BeatSaberMultiplayer.UI.ViewControllers.RoomView
{
	// Token: 0x02000055 RID: 85
	internal class LevelPackImageViewController : BSMLResourceViewController
	{
		// Token: 0x17000212 RID: 530
		// (get) Token: 0x0600070E RID: 1806 RVA: 0x0001C5DE File Offset: 0x0001A7DE
		public override string ResourceName
		{
			get
			{
				return string.Join(".", new string[]
				{
					base.GetType().Namespace,
					base.GetType().Name,
					"bsml"
				});
			}
		}

		// Token: 0x0600070F RID: 1807 RVA: 0x0001DCCF File Offset: 0x0001BECF
		protected override void DidActivate(bool firstActivation, ViewController.ActivationType type)
		{
			base.DidActivate(firstActivation, type);
		}

		// Token: 0x06000710 RID: 1808 RVA: 0x0001DCD9 File Offset: 0x0001BED9
		[UIAction("#post-parse")]
		public void SetupViewController()
		{
			this.levelPackImage.color = new Color(1f, 1f, 1f, 1f);
			this.levelPackImage.texture = Sprites.onlineIcon.texture;
		}

		// Token: 0x06000711 RID: 1809 RVA: 0x0001DD14 File Offset: 0x0001BF14
		public void SetPackImage(IAnnotatedBeatmapLevelCollection levelPack)
		{
			this.levelPackImage.texture = levelPack.coverImage.texture;
		}

		// Token: 0x04000374 RID: 884
		[UIComponent("level-pack-image")]
		public RawImage levelPackImage;
	}
}
