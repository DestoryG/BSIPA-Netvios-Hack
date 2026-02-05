using System;
using BeatSaberMarkupLanguage.Components;
using UnityEngine;

namespace BeatSaberMarkupLanguage.Tags
{
	// Token: 0x0200005B RID: 91
	internal class TabTag : BackgroundTag
	{
		// Token: 0x17000081 RID: 129
		// (get) Token: 0x0600019A RID: 410 RVA: 0x0000AAB0 File Offset: 0x00008CB0
		public override string[] Aliases
		{
			get
			{
				return new string[] { "tab" };
			}
		}

		// Token: 0x0600019B RID: 411 RVA: 0x0000AAC0 File Offset: 0x00008CC0
		public override GameObject CreateObject(Transform parent)
		{
			GameObject gameObject = base.CreateObject(parent);
			gameObject.name = "BSMLTab";
			gameObject.AddComponent<Tab>();
			return gameObject;
		}
	}
}
