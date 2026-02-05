using System;
using BeatSaberMarkupLanguage;
using BeatSaberMultiplayer.OverriddenClasses;
using HMUI;

namespace BeatSaberMultiplayer.Interop
{
	// Token: 0x02000070 RID: 112
	internal class BeatSaverDownloaderInterop
	{
		// Token: 0x17000239 RID: 569
		// (get) Token: 0x0600085A RID: 2138 RVA: 0x00023786 File Offset: 0x00021986
		public bool CanCreate
		{
			get
			{
				return CustomMoreSongsFlowCoordinator.CanCreate;
			}
		}

		// Token: 0x0600085B RID: 2139 RVA: 0x00023790 File Offset: 0x00021990
		public FlowCoordinator PresentDownloaderFlowCoordinator(FlowCoordinator parent, Action dismissedCallback)
		{
			FlowCoordinator flowCoordinator;
			try
			{
				if (this._coordinator == null)
				{
					CustomMoreSongsFlowCoordinator customMoreSongsFlowCoordinator = BeatSaberUI.CreateFlowCoordinator<CustomMoreSongsFlowCoordinator>();
					customMoreSongsFlowCoordinator.ParentFlowCoordinator = parent;
					this._coordinator = customMoreSongsFlowCoordinator;
				}
				parent.PresentFlowCoordinator(this._coordinator, dismissedCallback, false, false);
				flowCoordinator = this._coordinator;
			}
			catch (Exception ex)
			{
				Logger.log.Error(string.Format("Error creating MoreSongsFlowCoordinator: {0}", ex));
				flowCoordinator = null;
			}
			return flowCoordinator;
		}

		// Token: 0x04000423 RID: 1059
		private FlowCoordinator _coordinator;
	}
}
