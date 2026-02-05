using System;
using BeatSaberMarkupLanguage.Parser;
using UnityEngine;

namespace BeatSaberMarkupLanguage.Components.Settings
{
	// Token: 0x020000B1 RID: 177
	public abstract class GenericSetting : MonoBehaviour
	{
		// Token: 0x060003AF RID: 943
		public abstract void Setup();

		// Token: 0x060003B0 RID: 944
		public abstract void ApplyValue();

		// Token: 0x060003B1 RID: 945
		public abstract void ReceiveValue();

		// Token: 0x0400011D RID: 285
		public BSMLAction formatter;

		// Token: 0x0400011E RID: 286
		public BSMLAction onChange;

		// Token: 0x0400011F RID: 287
		public BSMLValue associatedValue;

		// Token: 0x04000120 RID: 288
		public bool updateOnChange;
	}
}
