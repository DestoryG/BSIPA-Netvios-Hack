using System;
using System.Collections.Generic;
using System.Linq;
using HMUI;
using IPA.Utilities;
using UnityEngine;

namespace BeatSaberMarkupLanguage.TypeHandlers
{
	// Token: 0x0200002B RID: 43
	[ComponentHandler(typeof(RectTransform))]
	public class RectTransformHandler : TypeHandler<RectTransform>
	{
		// Token: 0x17000046 RID: 70
		// (get) Token: 0x060000F7 RID: 247 RVA: 0x00006F90 File Offset: 0x00005190
		public override Dictionary<string, string[]> Props
		{
			get
			{
				return new Dictionary<string, string[]>
				{
					{
						"anchorMinX",
						new string[] { "anchor-min-x" }
					},
					{
						"anchorMinY",
						new string[] { "anchor-min-y" }
					},
					{
						"anchorMaxX",
						new string[] { "anchor-max-x" }
					},
					{
						"anchorMaxY",
						new string[] { "anchor-max-y" }
					},
					{
						"anchorPosX",
						new string[] { "anchor-pos-x" }
					},
					{
						"anchorPosY",
						new string[] { "anchor-pos-y" }
					},
					{
						"sizeDeltaX",
						new string[] { "size-delta-x" }
					},
					{
						"sizeDeltaY",
						new string[] { "size-delta-y" }
					},
					{
						"pivotX",
						new string[] { "pivot-x" }
					},
					{
						"pivotY",
						new string[] { "pivot-y" }
					},
					{
						"hoverHint",
						new string[] { "hover-hint" }
					},
					{
						"active",
						new string[] { "active" }
					}
				};
			}
		}

		// Token: 0x17000047 RID: 71
		// (get) Token: 0x060000F8 RID: 248 RVA: 0x000070D0 File Offset: 0x000052D0
		public override Dictionary<string, Action<RectTransform, string>> Setters
		{
			get
			{
				Dictionary<string, Action<RectTransform, string>> dictionary = new Dictionary<string, Action<RectTransform, string>>();
				dictionary.Add("anchorMinX", delegate(RectTransform component, string value)
				{
					component.anchorMin = new Vector2(Parse.Float(value), component.anchorMin.y);
				});
				dictionary.Add("anchorMinY", delegate(RectTransform component, string value)
				{
					component.anchorMin = new Vector2(component.anchorMin.x, Parse.Float(value));
				});
				dictionary.Add("anchorMaxX", delegate(RectTransform component, string value)
				{
					component.anchorMax = new Vector2(Parse.Float(value), component.anchorMax.y);
				});
				dictionary.Add("anchorMaxY", delegate(RectTransform component, string value)
				{
					component.anchorMax = new Vector2(component.anchorMax.x, Parse.Float(value));
				});
				dictionary.Add("anchorPosX", delegate(RectTransform component, string value)
				{
					component.anchoredPosition = new Vector2(Parse.Float(value), component.anchoredPosition.y);
				});
				dictionary.Add("anchorPosY", delegate(RectTransform component, string value)
				{
					component.anchoredPosition = new Vector2(component.anchoredPosition.x, Parse.Float(value));
				});
				dictionary.Add("sizeDeltaX", delegate(RectTransform component, string value)
				{
					component.sizeDelta = new Vector2(Parse.Float(value), component.sizeDelta.y);
				});
				dictionary.Add("sizeDeltaY", delegate(RectTransform component, string value)
				{
					component.sizeDelta = new Vector2(component.sizeDelta.x, Parse.Float(value));
				});
				dictionary.Add("pivotX", delegate(RectTransform component, string value)
				{
					component.pivot = new Vector2(Parse.Float(value), component.pivot.y);
				});
				dictionary.Add("pivotY", delegate(RectTransform component, string value)
				{
					component.pivot = new Vector2(component.pivot.x, Parse.Float(value));
				});
				dictionary.Add("hoverHint", new Action<RectTransform, string>(this.AddHoverHint));
				dictionary.Add("active", delegate(RectTransform component, string value)
				{
					component.gameObject.SetActive(Parse.Bool(value));
				});
				return dictionary;
			}
		}

		// Token: 0x060000F9 RID: 249 RVA: 0x000072C7 File Offset: 0x000054C7
		private void AddHoverHint(RectTransform rectTransform, string text)
		{
			HoverHint hoverHint = rectTransform.gameObject.AddComponent<HoverHint>();
			hoverHint.text = text;
			hoverHint.SetField("_hoverHintController", Resources.FindObjectsOfTypeAll<HoverHintController>().First<HoverHintController>());
		}
	}
}
