using System;
using System.Collections.Generic;
using BeatSaberMarkupLanguage.Components;

namespace BeatSaberMarkupLanguage.TypeHandlers
{
	// Token: 0x02000015 RID: 21
	[ComponentHandler(typeof(BSMLScrollView))]
	public class BSMLScrollViewHandler : TypeHandler<BSMLScrollView>
	{
		// Token: 0x17000021 RID: 33
		// (get) Token: 0x060000AB RID: 171 RVA: 0x00004E54 File Offset: 0x00003054
		public override Dictionary<string, string[]> Props
		{
			get
			{
				return new Dictionary<string, string[]> { 
				{
					"reserveButtonSpace",
					new string[] { "reserve-button-space" }
				} };
			}
		}

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x060000AC RID: 172 RVA: 0x00004E81 File Offset: 0x00003081
		public override Dictionary<string, Action<BSMLScrollView, string>> Setters
		{
			get
			{
				Dictionary<string, Action<BSMLScrollView, string>> dictionary = new Dictionary<string, Action<BSMLScrollView, string>>();
				dictionary.Add("reserveButtonSpace", delegate(BSMLScrollView component, string value)
				{
					component.ReserveButtonSpace = Parse.Bool(value);
				});
				return dictionary;
			}
		}
	}
}
