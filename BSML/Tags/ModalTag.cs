using System;
using System.Linq;
using BeatSaberMarkupLanguage.Components;
using HMUI;
using IPA.Utilities;
using UnityEngine;

namespace BeatSaberMarkupLanguage.Tags
{
	// Token: 0x02000052 RID: 82
	public class ModalTag : BSMLTag
	{
		// Token: 0x17000077 RID: 119
		// (get) Token: 0x0600017D RID: 381 RVA: 0x00009D87 File Offset: 0x00007F87
		public override string[] Aliases
		{
			get
			{
				return new string[] { "modal" };
			}
		}

		// Token: 0x0600017E RID: 382 RVA: 0x00009D98 File Offset: 0x00007F98
		public override GameObject CreateObject(Transform parent)
		{
			GameObject gameObject = new GameObject();
			gameObject.SetActive(false);
			gameObject.name = "BSMLModalView";
			RectTransform rectTransform = gameObject.AddComponent<RectTransform>();
			rectTransform.SetParent(parent, false);
			rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
			rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
			rectTransform.sizeDelta = new Vector2(0f, 0f);
			ModalView modalView = gameObject.AddComponent<ModalView>();
			ModalView modalView2 = Resources.FindObjectsOfTypeAll<ModalView>().First((ModalView x) => x.name == "TableView");
			modalView.SetField("_presentPanelAnimations", modalView2.GetField("_presentPanelAnimations"));
			modalView.SetField("_dismissPanelAnimation", modalView2.GetField("_dismissPanelAnimation"));
			GameObject gameObject2 = new GameObject();
			gameObject2.transform.SetParent(rectTransform, false);
			gameObject2.name = "Shadow";
			RectTransform rectTransform2 = gameObject2.gameObject.AddComponent<RectTransform>();
			rectTransform2.anchorMin = new Vector2(0f, 0f);
			rectTransform2.anchorMax = new Vector2(1f, 1f);
			rectTransform2.sizeDelta = new Vector2(10f, 10f);
			gameObject2.gameObject.AddComponent<Backgroundable>().ApplyBackground("round-rect-panel-shadow");
			GameObject gameObject3 = new GameObject();
			gameObject3.transform.SetParent(rectTransform, false);
			gameObject3.name = "Content";
			RectTransform rectTransform3 = gameObject3.gameObject.AddComponent<RectTransform>();
			rectTransform3.anchorMin = new Vector2(0f, 0f);
			rectTransform3.anchorMax = new Vector2(1f, 1f);
			rectTransform3.sizeDelta = new Vector2(0f, 0f);
			Backgroundable backgroundable = gameObject3.gameObject.AddComponent<Backgroundable>();
			backgroundable.ApplyBackground("round-rect-panel");
			backgroundable.background.color = new Color(0.706f, 0.706f, 0.706f, 1f);
			backgroundable.background.material = Resources.FindObjectsOfTypeAll<Material>().First((Material x) => x.name == "UIFogBG");
			ExternalComponents externalComponents = gameObject3.AddComponent<ExternalComponents>();
			externalComponents.components.Add(modalView);
			externalComponents.components.Add(rectTransform);
			return gameObject3;
		}
	}
}
