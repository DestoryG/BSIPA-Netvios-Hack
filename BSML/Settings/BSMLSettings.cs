using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using BeatSaberMarkupLanguage.Components;
using Polyglot;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace BeatSaberMarkupLanguage.Settings
{
	// Token: 0x0200006C RID: 108
	public class BSMLSettings : MonoBehaviour
	{
		// Token: 0x17000090 RID: 144
		// (get) Token: 0x060001C7 RID: 455 RVA: 0x0000B968 File Offset: 0x00009B68
		// (set) Token: 0x060001C8 RID: 456 RVA: 0x0000B98F File Offset: 0x00009B8F
		public static BSMLSettings instance
		{
			get
			{
				if (!BSMLSettings._instance)
				{
					BSMLSettings._instance = new GameObject("BSMLSettings").AddComponent<BSMLSettings>();
				}
				return BSMLSettings._instance;
			}
			private set
			{
				BSMLSettings._instance = value;
			}
		}

		// Token: 0x060001C9 RID: 457 RVA: 0x0000B998 File Offset: 0x00009B98
		internal void Setup()
		{
			base.StopAllCoroutines();
			base.StartCoroutine(this.AddButtonToMainScreen());
			foreach (CustomListTableData.CustomCellInfo customCellInfo in this.settingsMenus)
			{
				((SettingsMenu)customCellInfo).Setup();
			}
			this.isInitialized = true;
		}

		// Token: 0x060001CA RID: 458 RVA: 0x0000BA08 File Offset: 0x00009C08
		private void Awake()
		{
			Object.DontDestroyOnLoad(base.gameObject);
		}

		// Token: 0x060001CB RID: 459 RVA: 0x0000BA18 File Offset: 0x00009C18
		public void AddSettingsMenu(string name, string resource, object host)
		{
			if (this.settingsMenus.Any((CustomListTableData.CustomCellInfo x) => x.text == name))
			{
				return;
			}
			if (this.settingsMenus.Count == 0)
			{
				this.settingsMenus.Add(new SettingsMenu("About", "BeatSaberMarkupLanguage.Views.settings-about.bsml", this, Assembly.GetExecutingAssembly()));
			}
			SettingsMenu settingsMenu = new SettingsMenu(name, resource, host, Assembly.GetCallingAssembly());
			this.settingsMenus.Add(settingsMenu);
			if (this.isInitialized)
			{
				settingsMenu.Setup();
			}
			Button button = this.button;
			if (button == null)
			{
				return;
			}
			button.gameObject.SetActive(true);
		}

		// Token: 0x060001CC RID: 460 RVA: 0x0000BABC File Offset: 0x00009CBC
		public void RemoveSettingsMenu(object host)
		{
			IEnumerable<CustomListTableData.CustomCellInfo> enumerable = this.settingsMenus.Where((CustomListTableData.CustomCellInfo x) => (x as SettingsMenu).host == host);
			if (enumerable.Count<CustomListTableData.CustomCellInfo>() > 0)
			{
				this.settingsMenus.Remove(enumerable.FirstOrDefault<CustomListTableData.CustomCellInfo>());
			}
		}

		// Token: 0x060001CD RID: 461 RVA: 0x0000BB09 File Offset: 0x00009D09
		private IEnumerator AddButtonToMainScreen()
		{
			Transform transform = null;
			while (transform == null)
			{
				try
				{
					transform = GameObject.Find("MainMenuViewController/BottomPanel/Buttons").transform as RectTransform;
				}
				catch
				{
				}
				yield return new WaitForFixedUpdate();
			}
			this.button = Object.Instantiate<Transform>(transform.GetChild(0), transform).GetComponent<Button>();
			this.button.transform.GetChild(0).GetChild(1).GetComponentInChildren<LocalizedTextMeshProUGUI>()
				.Key = "Mod Settings";
			this.button.onClick.AddListener(new UnityAction(this.PresentSettings));
			this.button.transform.SetSiblingIndex(0);
			if (this.settingsMenus.Count == 0)
			{
				this.button.gameObject.SetActive(false);
			}
			yield break;
		}

		// Token: 0x060001CE RID: 462 RVA: 0x0000BB18 File Offset: 0x00009D18
		private void PresentSettings()
		{
			if (this.flowCoordinator == null)
			{
				this.flowCoordinator = BeatSaberUI.CreateFlowCoordinator<ModSettingsFlowCoordinator>();
			}
			this.flowCoordinator.isAnimating = true;
			BeatSaberUI.MainFlowCoordinator.PresentFlowCoordinator(this.flowCoordinator, delegate
			{
				this.flowCoordinator.ShowInitial();
				this.flowCoordinator.isAnimating = false;
			}, false, false);
		}

		// Token: 0x0400003B RID: 59
		private bool isInitialized;

		// Token: 0x0400003C RID: 60
		private Button button;

		// Token: 0x0400003D RID: 61
		private static BSMLSettings _instance;

		// Token: 0x0400003E RID: 62
		private ModSettingsFlowCoordinator flowCoordinator;

		// Token: 0x0400003F RID: 63
		public List<CustomListTableData.CustomCellInfo> settingsMenus = new List<CustomListTableData.CustomCellInfo>();
	}
}
