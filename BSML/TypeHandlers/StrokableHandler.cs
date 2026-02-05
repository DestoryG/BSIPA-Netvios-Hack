using System;
using System.Collections.Generic;
using BeatSaberMarkupLanguage.Components;

namespace BeatSaberMarkupLanguage.TypeHandlers
{
	// Token: 0x0200002E RID: 46
	[ComponentHandler(typeof(Strokable))]
	public class StrokableHandler : TypeHandler<Strokable>
	{
		// Token: 0x1700004B RID: 75
		// (get) Token: 0x06000105 RID: 261 RVA: 0x000076D4 File Offset: 0x000058D4
		public override Dictionary<string, string[]> Props
		{
			get
			{
				return new Dictionary<string, string[]>
				{
					{
						"strokeColor",
						new string[] { "stroke-color" }
					},
					{
						"strokeType",
						new string[] { "stroke-type" }
					}
				};
			}
		}

		// Token: 0x1700004C RID: 76
		// (get) Token: 0x06000106 RID: 262 RVA: 0x0000771A File Offset: 0x0000591A
		public override Dictionary<string, Action<Strokable, string>> Setters
		{
			get
			{
				return new Dictionary<string, Action<Strokable, string>>
				{
					{
						"strokeColor",
						new Action<Strokable, string>(StrokableHandler.SetColor)
					},
					{
						"strokeType",
						new Action<Strokable, string>(StrokableHandler.SetType)
					}
				};
			}
		}

		// Token: 0x06000107 RID: 263 RVA: 0x0000774F File Offset: 0x0000594F
		public static void SetColor(Strokable strokable, string strokeColor)
		{
			strokable.SetColor(strokeColor);
		}

		// Token: 0x06000108 RID: 264 RVA: 0x00007758 File Offset: 0x00005958
		public static void SetType(Strokable strokable, string strokeType)
		{
			strokable.SetType((Strokable.StrokeType)Enum.Parse(typeof(Strokable.StrokeType), strokeType));
		}
	}
}
