using System;
using BeatSaberMarkupLanguage.ViewControllers;

namespace BeatSaberMultiplayer.UI.ViewControllers.MasterView
{
	// Token: 0x02000065 RID: 101
	internal class MasterViewController : BSMLResourceViewController
	{
		// Token: 0x17000226 RID: 550
		// (get) Token: 0x060007B0 RID: 1968 RVA: 0x0001C5DE File Offset: 0x0001A7DE
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
