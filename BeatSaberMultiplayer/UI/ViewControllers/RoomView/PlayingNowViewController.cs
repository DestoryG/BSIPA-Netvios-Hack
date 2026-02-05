using System;
using BeatSaberMarkupLanguage.ViewControllers;

namespace BeatSaberMultiplayer.UI.ViewControllers.RoomView
{
	// Token: 0x0200005A RID: 90
	internal class PlayingNowViewController : BSMLResourceViewController
	{
		// Token: 0x17000218 RID: 536
		// (get) Token: 0x06000752 RID: 1874 RVA: 0x0001C5DE File Offset: 0x0001A7DE
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
	}
}
