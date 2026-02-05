using System;
using System.Collections.Generic;
using System.Linq;
using BeatSaberMarkupLanguage.Components;
using BeatSaberMarkupLanguage.Parser;
using HMUI;
using UnityEngine;
using UnityEngine.UI;

namespace BeatSaberMarkupLanguage.TypeHandlers
{
	// Token: 0x0200002D RID: 45
	[ComponentHandler(typeof(BSMLScrollableContainer))]
	public class ScrollableContainerHandler : TypeHandler
	{
		// Token: 0x1700004A RID: 74
		// (get) Token: 0x06000100 RID: 256 RVA: 0x000073F2 File Offset: 0x000055F2
		public override Dictionary<string, string[]> Props { get; } = new Dictionary<string, string[]>
		{
			{
				"id",
				new string[] { "id" }
			},
			{
				"maskOverflow",
				new string[] { "mask-overflow" }
			},
			{
				"alignBottom",
				new string[] { "align-bottom" }
			}
		};

		// Token: 0x06000101 RID: 257 RVA: 0x000073FC File Offset: 0x000055FC
		public override void HandleType(BSMLParser.ComponentTypeWithData componentType, BSMLParserParams parserParams)
		{
			BSMLScrollableContainer bsmlscrollableContainer = componentType.component as BSMLScrollableContainer;
			string text;
			if (componentType.data.TryGetValue("id", out text))
			{
				parserParams.AddEvent(text + "#PageUp", new Action(bsmlscrollableContainer.PageUpButtonPressed));
				parserParams.AddEvent(text + "#PageDown", new Action(bsmlscrollableContainer.PageDownButtonPressed));
			}
			string text2;
			if (componentType.data.TryGetValue("maskOverflow", out text2))
			{
				bool flag;
				bsmlscrollableContainer.MaskOverflow = !bool.TryParse(text2, out flag) || flag;
			}
			if (componentType.data.TryGetValue("alignBottom", out text2))
			{
				bool flag2;
				bsmlscrollableContainer.AlignBottom = bool.TryParse(text2, out flag2) && flag2;
			}
		}

		// Token: 0x06000102 RID: 258 RVA: 0x000074B4 File Offset: 0x000056B4
		public override void HandleTypeAfterChildren(BSMLParser.ComponentTypeWithData componentType, BSMLParserParams parserParams)
		{
			foreach (GameObject gameObject in parserParams.GetObjectsWithTag("ScrollFocus"))
			{
				gameObject.AddComponent<ItemForFocussedScrolling>();
			}
		}

		// Token: 0x06000103 RID: 259 RVA: 0x0000750C File Offset: 0x0000570C
		public override void HandleTypeAfterParse(BSMLParser.ComponentTypeWithData componentType, BSMLParserParams parserParams)
		{
			BSMLScrollableContainer bsmlscrollableContainer = componentType.component as BSMLScrollableContainer;
			string text;
			if (componentType.data.TryGetValue("id", out text))
			{
				bsmlscrollableContainer.PageUpButton = (from o in parserParams.GetObjectsWithTag("PageUpFor:" + text)
					select o.GetComponent<Button>()).FirstOrDefault((Button b) => b != null);
				bsmlscrollableContainer.PageDownButton = (from o in parserParams.GetObjectsWithTag("PageDownFor:" + text)
					select o.GetComponent<Button>()).FirstOrDefault((Button b) => b != null);
				bsmlscrollableContainer.ScrollIndicator = (from o in parserParams.GetObjectsWithTag("IndicatorFor:" + text)
					select o.GetComponent<VerticalScrollIndicator>() ?? o.GetComponent<BSMLScrollIndicator>()).FirstOrDefault((VerticalScrollIndicator i) => i != null);
			}
			bsmlscrollableContainer.Setup();
			bsmlscrollableContainer.RefreshButtonsInteractibility();
		}
	}
}
