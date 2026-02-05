using System;
using System.Collections.Generic;
using UnityEngine.UI;

namespace BeatSaberMarkupLanguage.TypeHandlers
{
	// Token: 0x02000020 RID: 32
	[ComponentHandler(typeof(HorizontalOrVerticalLayoutGroup))]
	public class HorizontalOrVerticalLayoutGroupHandler : TypeHandler<HorizontalOrVerticalLayoutGroup>
	{
		// Token: 0x17000035 RID: 53
		// (get) Token: 0x060000D3 RID: 211 RVA: 0x000060F0 File Offset: 0x000042F0
		public override Dictionary<string, string[]> Props
		{
			get
			{
				return new Dictionary<string, string[]>
				{
					{
						"spacing",
						new string[] { "spacing" }
					},
					{
						"childForceExpandWidth",
						new string[] { "child-expand-width" }
					},
					{
						"childForceExpandHeight",
						new string[] { "child-expand-height" }
					},
					{
						"childControlWidth",
						new string[] { "child-control-width" }
					},
					{
						"childControlHeight",
						new string[] { "child-control-height" }
					}
				};
			}
		}

		// Token: 0x17000036 RID: 54
		// (get) Token: 0x060000D4 RID: 212 RVA: 0x00006184 File Offset: 0x00004384
		public override Dictionary<string, Action<HorizontalOrVerticalLayoutGroup, string>> Setters
		{
			get
			{
				Dictionary<string, Action<HorizontalOrVerticalLayoutGroup, string>> dictionary = new Dictionary<string, Action<HorizontalOrVerticalLayoutGroup, string>>();
				dictionary.Add("spacing", delegate(HorizontalOrVerticalLayoutGroup component, string value)
				{
					component.spacing = Parse.Float(value);
				});
				dictionary.Add("childForceExpandWidth", delegate(HorizontalOrVerticalLayoutGroup component, string value)
				{
					component.childForceExpandWidth = Parse.Bool(value);
				});
				dictionary.Add("childForceExpandHeight", delegate(HorizontalOrVerticalLayoutGroup component, string value)
				{
					component.childForceExpandHeight = Parse.Bool(value);
				});
				dictionary.Add("childControlWidth", delegate(HorizontalOrVerticalLayoutGroup component, string value)
				{
					component.childControlWidth = Parse.Bool(value);
				});
				dictionary.Add("childControlHeight", delegate(HorizontalOrVerticalLayoutGroup component, string value)
				{
					component.childControlHeight = Parse.Bool(value);
				});
				return dictionary;
			}
		}
	}
}
