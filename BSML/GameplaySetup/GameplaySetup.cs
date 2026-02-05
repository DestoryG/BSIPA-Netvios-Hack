using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using BeatSaberMarkupLanguage.Attributes;
using TMPro;
using UnityEngine;

namespace BeatSaberMarkupLanguage.GameplaySetup
{
	// Token: 0x02000090 RID: 144
	public class GameplaySetup : PersistentSingleton<GameplaySetup>
	{
		// Token: 0x060002CA RID: 714 RVA: 0x0000DD90 File Offset: 0x0000BF90
		internal void Setup()
		{
			if (this.menus.Count == 0)
			{
				return;
			}
			this.gameplaySetupViewController = Resources.FindObjectsOfTypeAll<GameplaySetupViewController>().First<GameplaySetupViewController>();
			this.gameplaySetupViewController.transform.Find("HeaderPanel").GetComponentInChildren<TextMeshProUGUI>().fontSize = 4f;
			this.vanillaItems.Clear();
			foreach (object obj in this.gameplaySetupViewController.transform)
			{
				Transform transform = (Transform)obj;
				if (transform.name != "HeaderPanel")
				{
					this.vanillaItems.Add(transform);
				}
			}
			(this.gameplaySetupViewController.transform.Find("HeaderPanel") as RectTransform).sizeDelta = new Vector2(90f, 6f);
			PersistentSingleton<BSMLParser>.instance.Parse(Utilities.GetResourceContent(Assembly.GetExecutingAssembly(), "BeatSaberMarkupLanguage.Views.gameplay-setup.bsml"), this.gameplaySetupViewController.gameObject, this);
		}

		// Token: 0x060002CB RID: 715 RVA: 0x0000DEA8 File Offset: 0x0000C0A8
		public void AddTab(string name, string resource, object host)
		{
			if (this.menus.Any((object x) => (x as GameplaySetupMenu).name == name))
			{
				return;
			}
			this.menus.Add(new GameplaySetupMenu(name, resource, host, Assembly.GetCallingAssembly()));
		}

		// Token: 0x060002CC RID: 716 RVA: 0x0000DEFC File Offset: 0x0000C0FC
		public void RemoveTab(string name)
		{
			IEnumerable<object> enumerable = this.menus.Where((object x) => (x as GameplaySetupMenu).name == name);
			if (enumerable.Count<object>() > 0)
			{
				this.menus.Remove(enumerable.FirstOrDefault<object>());
			}
		}

		// Token: 0x0400009A RID: 154
		private GameplaySetupViewController gameplaySetupViewController;

		// Token: 0x0400009B RID: 155
		[UIValue("vanilla-items")]
		private List<Transform> vanillaItems = new List<Transform>();

		// Token: 0x0400009C RID: 156
		[UIValue("mod-menus")]
		private List<object> menus = new List<object>();
	}
}
