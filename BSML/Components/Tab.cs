using System;
using UnityEngine;

namespace BeatSaberMarkupLanguage.Components
{
	// Token: 0x020000A7 RID: 167
	public class Tab : MonoBehaviour
	{
		// Token: 0x170000E4 RID: 228
		// (get) Token: 0x06000369 RID: 873 RVA: 0x000108DF File Offset: 0x0000EADF
		// (set) Token: 0x0600036A RID: 874 RVA: 0x000108E7 File Offset: 0x0000EAE7
		public string TabName
		{
			get
			{
				return this.tabName;
			}
			set
			{
				this.tabName = value;
				TabSelector tabSelector = this.selector;
				if (tabSelector == null)
				{
					return;
				}
				tabSelector.Refresh();
			}
		}

		// Token: 0x040000F8 RID: 248
		private string tabName;

		// Token: 0x040000F9 RID: 249
		public TabSelector selector;
	}
}
