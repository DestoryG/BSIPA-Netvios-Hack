using System;
using System.Collections.Generic;
using BeatSaberMarkupLanguage.Parser;
using UnityEngine;
using UnityEngine.UI;

namespace BeatSaberMarkupLanguage.TypeHandlers
{
	// Token: 0x02000026 RID: 38
	[ComponentHandler(typeof(LayoutGroup))]
	public class LayoutGroupHandler : TypeHandler
	{
		// Token: 0x1700003F RID: 63
		// (get) Token: 0x060000E7 RID: 231 RVA: 0x0000690C File Offset: 0x00004B0C
		public override Dictionary<string, string[]> Props
		{
			get
			{
				return new Dictionary<string, string[]>
				{
					{
						"padTop",
						new string[] { "pad-top" }
					},
					{
						"padBottom",
						new string[] { "pad-bottom" }
					},
					{
						"padLeft",
						new string[] { "pad-left" }
					},
					{
						"padRight",
						new string[] { "pad-right" }
					},
					{
						"pad",
						new string[] { "pad" }
					},
					{
						"childAlign",
						new string[] { "child-align" }
					}
				};
			}
		}

		// Token: 0x060000E8 RID: 232 RVA: 0x000069B8 File Offset: 0x00004BB8
		public override void HandleType(BSMLParser.ComponentTypeWithData componentType, BSMLParserParams parserParams)
		{
			LayoutGroup layoutGroup = componentType.component as LayoutGroup;
			string text;
			if (componentType.data.TryGetValue("pad", out text))
			{
				int num = Parse.Int(text);
				layoutGroup.padding = new RectOffset(num, num, num, num);
			}
			string text2;
			string text3;
			string text4;
			string text5;
			layoutGroup.padding = new RectOffset(componentType.data.TryGetValue("padLeft", out text2) ? Parse.Int(text2) : layoutGroup.padding.left, componentType.data.TryGetValue("padRight", out text3) ? Parse.Int(text3) : layoutGroup.padding.right, componentType.data.TryGetValue("padTop", out text4) ? Parse.Int(text4) : layoutGroup.padding.top, componentType.data.TryGetValue("padBottom", out text5) ? Parse.Int(text5) : layoutGroup.padding.bottom);
			string text6;
			if (componentType.data.TryGetValue("childAlign", out text6))
			{
				layoutGroup.childAlignment = (TextAnchor)Enum.Parse(typeof(TextAnchor), text6);
			}
		}
	}
}
