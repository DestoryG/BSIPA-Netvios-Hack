using System;
using System.Reflection;
using BeatSaberMarkupLanguage.Attributes;
using UnityEngine;

namespace BeatSaberMarkupLanguage.GameplaySetup
{
	// Token: 0x02000091 RID: 145
	internal class GameplaySetupMenu
	{
		// Token: 0x060002CE RID: 718 RVA: 0x0000DF67 File Offset: 0x0000C167
		public GameplaySetupMenu(string name, string resource, object host, Assembly assembly)
		{
			this.name = name;
			this.resource = resource;
			this.host = host;
			this.assembly = assembly;
		}

		// Token: 0x060002CF RID: 719 RVA: 0x0000DF8C File Offset: 0x0000C18C
		[UIAction("#post-parse")]
		public void Setup()
		{
			PersistentSingleton<BSMLParser>.instance.Parse(Utilities.GetResourceContent(this.assembly, this.resource), this.tabObject, this.host);
		}

		// Token: 0x0400009D RID: 157
		public string resource;

		// Token: 0x0400009E RID: 158
		public object host;

		// Token: 0x0400009F RID: 159
		public Assembly assembly;

		// Token: 0x040000A0 RID: 160
		[UIValue("tab-name")]
		public string name;

		// Token: 0x040000A1 RID: 161
		[UIObject("root-tab")]
		private GameObject tabObject;
	}
}
