using System;
using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.ViewControllers;
using HMUI;
using UnityEngine.UI;

namespace BeatSaberMultiplayer.UI.ViewControllers.MultiPlayerView
{
	// Token: 0x02000063 RID: 99
	internal class MultiPlayerViewController : BSMLResourceViewController
	{
		// Token: 0x17000223 RID: 547
		// (get) Token: 0x0600079D RID: 1949 RVA: 0x0001C5DE File Offset: 0x0001A7DE
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

		// Token: 0x1400002B RID: 43
		// (add) Token: 0x0600079E RID: 1950 RVA: 0x0001FDC8 File Offset: 0x0001DFC8
		// (remove) Token: 0x0600079F RID: 1951 RVA: 0x0001FE00 File Offset: 0x0001E000
		public event Action didSelectRoomsEvent;

		// Token: 0x1400002C RID: 44
		// (add) Token: 0x060007A0 RID: 1952 RVA: 0x0001FE38 File Offset: 0x0001E038
		// (remove) Token: 0x060007A1 RID: 1953 RVA: 0x0001FE70 File Offset: 0x0001E070
		public event Action didSelectMasterEvent;

		// Token: 0x060007A2 RID: 1954 RVA: 0x0001FEA5 File Offset: 0x0001E0A5
		protected override void DidActivate(bool firstActivation, ViewController.ActivationType activationType)
		{
			base.DidActivate(firstActivation, activationType);
			if (firstActivation)
			{
				this._masterButton.interactable = false;
			}
		}

		// Token: 0x060007A3 RID: 1955 RVA: 0x0001FEBE File Offset: 0x0001E0BE
		[UIAction("rooms-btn-pressed")]
		private void RoomsBtnPressed()
		{
			Action action = this.didSelectRoomsEvent;
			if (action == null)
			{
				return;
			}
			action();
		}

		// Token: 0x060007A4 RID: 1956 RVA: 0x0001FED0 File Offset: 0x0001E0D0
		[UIAction("master-btn-pressed")]
		private void MasterBtnPressed()
		{
			Action action = this.didSelectMasterEvent;
			if (action == null)
			{
				return;
			}
			action();
		}

		// Token: 0x040003C5 RID: 965
		[UIComponent("rooms-button")]
		private Button _roomsButton;

		// Token: 0x040003C6 RID: 966
		[UIComponent("master-button")]
		private Button _masterButton;
	}
}
