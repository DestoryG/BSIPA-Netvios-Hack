using System;
using HMUI;
using UnityEngine;

namespace BeatSaberMarkupLanguage.Components
{
	// Token: 0x02000094 RID: 148
	public class BSMLScrollIndicator : VerticalScrollIndicator
	{
		// Token: 0x170000D1 RID: 209
		// (get) Token: 0x060002E2 RID: 738 RVA: 0x0000E87B File Offset: 0x0000CA7B
		// (set) Token: 0x060002E3 RID: 739 RVA: 0x0000E883 File Offset: 0x0000CA83
		public RectTransform Handle
		{
			get
			{
				return this._handle;
			}
			set
			{
				this._handle = value;
			}
		}
	}
}
