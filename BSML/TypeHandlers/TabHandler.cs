using System;
using System.Collections.Generic;
using BeatSaberMarkupLanguage.Components;

namespace BeatSaberMarkupLanguage.TypeHandlers
{
	// Token: 0x0200002F RID: 47
	[ComponentHandler(typeof(Tab))]
	public class TabHandler : TypeHandler<Tab>
	{
		// Token: 0x1700004D RID: 77
		// (get) Token: 0x0600010A RID: 266 RVA: 0x00007780 File Offset: 0x00005980
		public override Dictionary<string, string[]> Props
		{
			get
			{
				return new Dictionary<string, string[]> { 
				{
					"tabName",
					new string[] { "tab-name" }
				} };
			}
		}

		// Token: 0x1700004E RID: 78
		// (get) Token: 0x0600010B RID: 267 RVA: 0x000077AD File Offset: 0x000059AD
		public override Dictionary<string, Action<Tab, string>> Setters
		{
			get
			{
				Dictionary<string, Action<Tab, string>> dictionary = new Dictionary<string, Action<Tab, string>>();
				dictionary.Add("tabName", delegate(Tab component, string value)
				{
					if (string.IsNullOrEmpty(value))
					{
						throw new ArgumentNullException("tabName cannot be null or empty for Tab");
					}
					component.TabName = value;
				});
				return dictionary;
			}
		}
	}
}
