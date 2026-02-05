using System;
using HMUI;

namespace BeatSaberMultiplayer.Interop
{
	// Token: 0x02000071 RID: 113
	internal interface IDismissable
	{
		// Token: 0x1700023A RID: 570
		// (get) Token: 0x0600085D RID: 2141
		// (set) Token: 0x0600085E RID: 2142
		FlowCoordinator ParentFlowCoordinator { get; set; }

		// Token: 0x0600085F RID: 2143
		void Dismiss(bool immediately);
	}
}
