using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BeatSaberMarkupLanguage.TypeHandlers
{
	// Token: 0x0200001F RID: 31
	[ComponentHandler(typeof(GridLayoutGroup))]
	public class GridLayoutGroupHandler : TypeHandler<GridLayoutGroup>
	{
		// Token: 0x17000033 RID: 51
		// (get) Token: 0x060000D0 RID: 208 RVA: 0x00005FB4 File Offset: 0x000041B4
		public override Dictionary<string, string[]> Props
		{
			get
			{
				return new Dictionary<string, string[]>
				{
					{
						"cellSizeX",
						new string[] { "cell-size-x" }
					},
					{
						"cellSizeY",
						new string[] { "cell-size-y" }
					},
					{
						"spacingX",
						new string[] { "spacing-x" }
					},
					{
						"spacingY",
						new string[] { "spacing-y" }
					}
				};
			}
		}

		// Token: 0x17000034 RID: 52
		// (get) Token: 0x060000D1 RID: 209 RVA: 0x0000602C File Offset: 0x0000422C
		public override Dictionary<string, Action<GridLayoutGroup, string>> Setters
		{
			get
			{
				Dictionary<string, Action<GridLayoutGroup, string>> dictionary = new Dictionary<string, Action<GridLayoutGroup, string>>();
				dictionary.Add("cellSizeX", delegate(GridLayoutGroup component, string value)
				{
					component.cellSize = new Vector2(Parse.Float(value), component.cellSize.y);
				});
				dictionary.Add("cellSizeY", delegate(GridLayoutGroup component, string value)
				{
					component.cellSize = new Vector2(component.cellSize.x, Parse.Float(value));
				});
				dictionary.Add("spacingX", delegate(GridLayoutGroup component, string value)
				{
					component.spacing = new Vector2(Parse.Float(value), component.spacing.y);
				});
				dictionary.Add("spacingY", delegate(GridLayoutGroup component, string value)
				{
					component.spacing = new Vector2(component.spacing.x, Parse.Float(value));
				});
				return dictionary;
			}
		}
	}
}
