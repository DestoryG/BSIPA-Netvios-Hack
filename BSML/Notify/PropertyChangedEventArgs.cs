using System;

namespace BeatSaberMarkupLanguage.Notify
{
	// Token: 0x02000084 RID: 132
	public class PropertyChangedEventArgs : EventArgs
	{
		// Token: 0x170000B4 RID: 180
		// (get) Token: 0x06000281 RID: 641 RVA: 0x0000D14B File Offset: 0x0000B34B
		public string PropertyName { get; }

		// Token: 0x06000282 RID: 642 RVA: 0x0000D153 File Offset: 0x0000B353
		public PropertyChangedEventArgs(string propertyName)
		{
			this.PropertyName = propertyName;
		}
	}
}
