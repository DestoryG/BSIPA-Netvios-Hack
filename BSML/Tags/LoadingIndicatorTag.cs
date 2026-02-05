using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace BeatSaberMarkupLanguage.Tags
{
	// Token: 0x02000050 RID: 80
	internal class LoadingIndicatorTag : BSMLTag
	{
		// Token: 0x17000075 RID: 117
		// (get) Token: 0x06000177 RID: 375 RVA: 0x00009C3D File Offset: 0x00007E3D
		public override string[] Aliases
		{
			get
			{
				return new string[] { "loading", "loading-indicator" };
			}
		}

		// Token: 0x06000178 RID: 376 RVA: 0x00009C58 File Offset: 0x00007E58
		public override GameObject CreateObject(Transform parent)
		{
			GameObject gameObject = Object.Instantiate<GameObject>((from x in Resources.FindObjectsOfTypeAll<GameObject>()
				where x.name == "LoadingIndicator"
				select x).First<GameObject>(), parent, false);
			gameObject.name = "BSMLLoadingIndicator";
			gameObject.AddComponent<LayoutElement>();
			return gameObject;
		}
	}
}
