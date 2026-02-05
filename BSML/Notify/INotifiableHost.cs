using System;

namespace BeatSaberMarkupLanguage.Notify
{
	// Token: 0x02000082 RID: 130
	public interface INotifiableHost
	{
		// Token: 0x14000002 RID: 2
		// (add) Token: 0x0600027B RID: 635
		// (remove) Token: 0x0600027C RID: 636
		event PropertyChangedEventHandler PropertyChanged;
	}
}
