using System;
using System.Collections.Generic;
using System.Linq;
using BeatSaberMarkupLanguage.Components;
using BeatSaberMarkupLanguage.Parser;
using HMUI;
using IPA.Utilities;
using UnityEngine;

namespace BeatSaberMarkupLanguage.TypeHandlers
{
	// Token: 0x02000030 RID: 48
	[ComponentHandler(typeof(TabSelector))]
	public class TabSelectorHandler : TypeHandler<TabSelector>
	{
		// Token: 0x1700004F RID: 79
		// (get) Token: 0x0600010D RID: 269 RVA: 0x000077E8 File Offset: 0x000059E8
		public override Dictionary<string, string[]> Props
		{
			get
			{
				return new Dictionary<string, string[]>
				{
					{
						"tabTag",
						new string[] { "tab-tag" }
					},
					{
						"hasSeparator",
						new string[] { "has-separator" }
					},
					{
						"pageCount",
						new string[] { "page-count" }
					},
					{
						"leftButtonTag",
						new string[] { "left-button-tag" }
					},
					{
						"rightButtonTag",
						new string[] { "right-button-tag" }
					}
				};
			}
		}

		// Token: 0x17000050 RID: 80
		// (get) Token: 0x0600010E RID: 270 RVA: 0x0000787C File Offset: 0x00005A7C
		public override Dictionary<string, Action<TabSelector, string>> Setters
		{
			get
			{
				Dictionary<string, Action<TabSelector, string>> dictionary = new Dictionary<string, Action<TabSelector, string>>();
				dictionary.Add("hasSeparator", new Action<TabSelector, string>(this.SetSeparator));
				dictionary.Add("pageCount", delegate(TabSelector component, string value)
				{
					component.PageCount = Parse.Int(value);
				});
				dictionary.Add("leftButtonTag", delegate(TabSelector component, string value)
				{
					component.leftButtonTag = value;
				});
				dictionary.Add("rightButtonTag", delegate(TabSelector component, string value)
				{
					component.rightButtonTag = value;
				});
				return dictionary;
			}
		}

		// Token: 0x0600010F RID: 271 RVA: 0x00007924 File Offset: 0x00005B24
		public override void HandleType(BSMLParser.ComponentTypeWithData componentType, BSMLParserParams parserParams)
		{
			base.HandleType(componentType, parserParams);
			TabSelector tabSelector = componentType.component as TabSelector;
			tabSelector.parserParams = parserParams;
			string text;
			if (!componentType.data.TryGetValue("tabTag", out text))
			{
				throw new Exception("Tab Selector must have a tab-tag");
			}
			tabSelector.tabTag = text;
			parserParams.AddEvent("post-parse", new Action(tabSelector.Setup));
		}

		// Token: 0x06000110 RID: 272 RVA: 0x0000798C File Offset: 0x00005B8C
		private void SetSeparator(TabSelector tabSelector, string hasSeparator)
		{
			SegmentedControl textSegmentedControl = tabSelector.textSegmentedControl;
			string text = "_separatorPrefab";
			Transform transform;
			if (!Parse.Bool(hasSeparator))
			{
				transform = null;
			}
			else
			{
				transform = Resources.FindObjectsOfTypeAll<TextSegmentedControl>().First((TextSegmentedControl x) => x.GetField("_separatorPrefab") != null).GetField("_separatorPrefab");
			}
			textSegmentedControl.SetField(text, transform);
		}
	}
}
