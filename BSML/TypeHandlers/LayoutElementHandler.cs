using System;
using System.Collections.Generic;
using UnityEngine.UI;

namespace BeatSaberMarkupLanguage.TypeHandlers
{
	// Token: 0x02000025 RID: 37
	[ComponentHandler(typeof(LayoutElement))]
	public class LayoutElementHandler : TypeHandler<LayoutElement>
	{
		// Token: 0x1700003D RID: 61
		// (get) Token: 0x060000E4 RID: 228 RVA: 0x0000677C File Offset: 0x0000497C
		public override Dictionary<string, string[]> Props
		{
			get
			{
				return new Dictionary<string, string[]>
				{
					{
						"ignoreLayout",
						new string[] { "ignore-layout" }
					},
					{
						"preferredWidth",
						new string[] { "preferred-width", "pref-width" }
					},
					{
						"preferredHeight",
						new string[] { "preferred-height", "pref-height" }
					},
					{
						"minHeight",
						new string[] { "min-height" }
					},
					{
						"minWidth",
						new string[] { "min-width" }
					}
				};
			}
		}

		// Token: 0x1700003E RID: 62
		// (get) Token: 0x060000E5 RID: 229 RVA: 0x00006820 File Offset: 0x00004A20
		public override Dictionary<string, Action<LayoutElement, string>> Setters
		{
			get
			{
				Dictionary<string, Action<LayoutElement, string>> dictionary = new Dictionary<string, Action<LayoutElement, string>>();
				dictionary.Add("ignoreLayout", delegate(LayoutElement layoutElement, string value)
				{
					layoutElement.ignoreLayout = Parse.Bool(value);
				});
				dictionary.Add("preferredWidth", delegate(LayoutElement layoutElement, string value)
				{
					layoutElement.preferredWidth = Parse.Float(value);
				});
				dictionary.Add("preferredHeight", delegate(LayoutElement layoutElement, string value)
				{
					layoutElement.preferredHeight = Parse.Float(value);
				});
				dictionary.Add("minHeight", delegate(LayoutElement layoutElement, string value)
				{
					layoutElement.minHeight = Parse.Float(value);
				});
				dictionary.Add("minWidth", delegate(LayoutElement layoutElement, string value)
				{
					layoutElement.minWidth = Parse.Float(value);
				});
				return dictionary;
			}
		}
	}
}
