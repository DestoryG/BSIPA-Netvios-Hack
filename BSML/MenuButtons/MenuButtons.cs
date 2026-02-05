using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.Parser;
using HMUI;
using IPA.Utilities;
using UnityEngine;

namespace BeatSaberMarkupLanguage.MenuButtons
{
	// Token: 0x02000086 RID: 134
	public class MenuButtons : PersistentSingleton<MenuButtons>
	{
		// Token: 0x170000B9 RID: 185
		// (get) Token: 0x06000292 RID: 658 RVA: 0x0000D2F4 File Offset: 0x0000B4F4
		[UIValue("any-buttons")]
		public bool AnyButtons
		{
			get
			{
				return this.buttons.Count > 0;
			}
		}

		// Token: 0x06000293 RID: 659 RVA: 0x0000D304 File Offset: 0x0000B504
		internal void Setup()
		{
			this.releaseInfoViewController = Resources.FindObjectsOfTypeAll<ReleaseInfoViewController>().First<ReleaseInfoViewController>();
			for (int i = 0; i < this.releaseInfoViewController.transform.childCount; i++)
			{
				this.releaseInfoViewController.transform.GetChild(i).gameObject.SetActive(false);
			}
			this.releaseInfoViewController.didDeactivateEvent -= new ViewController.DidDeactivateDelegate(this.OnDeactivate);
			this.releaseInfoViewController.didDeactivateEvent += new ViewController.DidDeactivateDelegate(this.OnDeactivate);
			this.releaseNotesScrollView = this.releaseInfoViewController.GetField("_textPageScrollView").transform;
			PersistentSingleton<BSMLParser>.instance.Parse(Utilities.GetResourceContent(Assembly.GetExecutingAssembly(), "BeatSaberMarkupLanguage.Views.main-left-screen.bsml"), this.releaseInfoViewController.gameObject, this);
			if (PersistentSingleton<MenuPins>.instance.rootObject == null)
			{
				PersistentSingleton<MenuPins>.instance.Setup();
				return;
			}
			PersistentSingleton<MenuPins>.instance.Refresh();
		}

		// Token: 0x06000294 RID: 660 RVA: 0x0000D3F0 File Offset: 0x0000B5F0
		internal void Refresh()
		{
			if (this.rootObject != null)
			{
				this.releaseNotesScrollView.transform.SetParent(null, false);
				Object.Destroy(this.rootObject);
				base.StopAllCoroutines();
				this.Setup();
			}
			Transform transform = this.releaseNotesScrollView;
			if (transform == null)
			{
				return;
			}
			transform.gameObject.SetActive(false);
		}

		// Token: 0x06000295 RID: 661 RVA: 0x0000D44C File Offset: 0x0000B64C
		public void RegisterButton(MenuButton menuButton)
		{
			if (this.buttons.Any((object x) => (x as MenuButton).Text == menuButton.Text))
			{
				return;
			}
			this.buttons.Add(menuButton);
			this.pinButtons.Add(new PinnedMod(menuButton));
			this.Refresh();
		}

		// Token: 0x06000296 RID: 662 RVA: 0x0000D4B0 File Offset: 0x0000B6B0
		public void UnregisterButton(MenuButton menuButton)
		{
			this.buttons.Remove(menuButton);
			this.pinButtons.RemoveAll((object x) => (x as PinnedMod).menuButton == menuButton);
			this.Refresh();
		}

		// Token: 0x06000297 RID: 663 RVA: 0x0000D4FA File Offset: 0x0000B6FA
		[UIAction("#post-parse")]
		private void PostParse()
		{
			if (this.AnyButtons && !Plugin.config.GetBool("New", "seenMenuButton", false, false))
			{
				base.StartCoroutine(this.ShowNew());
			}
		}

		// Token: 0x06000298 RID: 664 RVA: 0x0000D529 File Offset: 0x0000B729
		private IEnumerator ShowNew()
		{
			yield return new WaitForSeconds(1f);
			this.parserParams.EmitEvent("show-new");
			Plugin.config.SetBool("New", "seenMenuButton", true);
			yield break;
		}

		// Token: 0x06000299 RID: 665 RVA: 0x0000D538 File Offset: 0x0000B738
		private void OnDeactivate(ViewController.DeactivationType deactivationType)
		{
			this.parserParams.EmitEvent("close-modals");
		}

		// Token: 0x0400008D RID: 141
		private ReleaseInfoViewController releaseInfoViewController;

		// Token: 0x0400008E RID: 142
		[UIValue("release-notes")]
		private Transform releaseNotesScrollView;

		// Token: 0x0400008F RID: 143
		[UIValue("buttons")]
		private List<object> buttons = new List<object>();

		// Token: 0x04000090 RID: 144
		[UIValue("pin-buttons")]
		internal List<object> pinButtons = new List<object>();

		// Token: 0x04000091 RID: 145
		[UIObject("root-object")]
		private GameObject rootObject;

		// Token: 0x04000092 RID: 146
		[UIParams]
		private BSMLParserParams parserParams;
	}
}
