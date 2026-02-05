using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using BeatSaberMarkupLanguage;
using BeatSaberMultiplayer.Helper;
using BeatSaberMultiplayer.UI.FlowCoordinators;
using BS_Utils.Utilities;
using HMUI;
using Newtonsoft.Json;
using Polyglot;
using SongCore;
using UnityEngine;
using UnityEngine.UI;

namespace BeatSaberMultiplayer.UI
{
	// Token: 0x02000052 RID: 82
	internal class PluginUI : MonoBehaviour
	{
		// Token: 0x060006CF RID: 1743
		[DllImport("NetViosSDK.dll", CharSet = CharSet.Ansi)]
		[return: MarshalAs(UnmanagedType.LPStr)]
		public static extern IntPtr NetViosSDK_GetSDKData();

		// Token: 0x060006D0 RID: 1744 RVA: 0x0001BF50 File Offset: 0x0001A150
		public static void OnLoad()
		{
			if (PluginUI.instance == null)
			{
				new GameObject("Multiplayer Plugin").AddComponent<PluginUI>().SetUp();
			}
		}

		// Token: 0x060006D1 RID: 1745 RVA: 0x0001BF74 File Offset: 0x0001A174
		public void SetUp()
		{
			PluginUI.instance = this;
			PluginUI.RefreshSdkUserInfo();
			if (Loader.AreSongsLoading)
			{
				Loader.SongsLoadedEvent += this.SongsLoaded;
			}
			else
			{
				this.SongsLoaded(null, null);
			}
			this.CreatUI();
			this.CloneLevelDetailView();
			this.InitPlayerData();
		}

		// Token: 0x060006D2 RID: 1746 RVA: 0x0001BFC0 File Offset: 0x0001A1C0
		public static void RefreshSdkUserInfo()
		{
			PluginUI.sdkUserInfo = JsonConvert.DeserializeObject<Dictionary<string, string>>(Marshal.PtrToStringAnsi(PluginUI.NetViosSDK_GetSDKData()));
		}

		// Token: 0x060006D3 RID: 1747 RVA: 0x0001BFD6 File Offset: 0x0001A1D6
		public void CreatUI()
		{
			if (this._mainFlow == null)
			{
				this._mainFlow = Resources.FindObjectsOfTypeAll<MainFlowCoordinator>().First<MainFlowCoordinator>();
			}
			this.CreateFlowCoordinator();
			this.CreateSimpleDialogPromptViewController();
			this.CreateOnlineButton();
		}

		// Token: 0x060006D4 RID: 1748 RVA: 0x0001C008 File Offset: 0x0001A208
		private void CreateFlowCoordinator()
		{
			try
			{
				if (this.multiPlayerFlowCoordinator == null)
				{
					this.multiPlayerFlowCoordinator = BeatSaberUI.CreateFlowCoordinator<MultiPlayerFlowCoordinator>();
					this.multiPlayerFlowCoordinator.didFinishEvent += delegate
					{
						MainFlowCoordinator mainFlow = this._mainFlow;
						if (mainFlow == null)
						{
							return;
						}
						mainFlow.InvokeMethod("DismissFlowCoordinator", new object[] { this.multiPlayerFlowCoordinator, null, false });
					};
					this.multiPlayerFlowCoordinator.showTcpErrorEvent += this.ConnectedAndLoggedServerCallback;
				}
			}
			catch (Exception ex)
			{
				Logger.log.Error("creat multiplayer flowCoordinator error: " + ex.Message);
			}
		}

		// Token: 0x060006D5 RID: 1749 RVA: 0x0001C08C File Offset: 0x0001A28C
		private void CreateSimpleDialogPromptViewController()
		{
			PluginUI.simpleDialog = Object.Instantiate<GameObject>(this._mainFlow.GetPrivateField("_simpleDialogPromptViewController").gameObject).GetComponent<SimpleDialogPromptViewController>();
		}

		// Token: 0x060006D6 RID: 1750 RVA: 0x0001C0B4 File Offset: 0x0001A2B4
		private void CreateOnlineButton()
		{
			try
			{
				Button[] componentsInChildren = Resources.FindObjectsOfTypeAll<RectTransform>().First((RectTransform x) => x.name == "MainButtons" && x.parent.name == "MainMenuViewController").GetComponentsInChildren<Button>();
				this._multiplayerButton = Object.Instantiate<Button>(Resources.FindObjectsOfTypeAll<Button>().Last((Button x) => x.name == "SoloFreePlayButton"), componentsInChildren.First((Button x) => x.name == "SoloFreePlayButton").transform.parent, false);
				this._multiplayerButton.name = "BSMultiPlayerButton";
				Object.Destroy(this._multiplayerButton.GetComponentInChildren<LocalizedTextMeshProUGUI>());
				Object.Destroy(this._multiplayerButton.GetComponentInChildren<HoverHint>());
				this._multiplayerButton.transform.SetAsLastSibling();
				this._multiplayerButton.SetButtonText(PluginSetting.Title);
				this._multiplayerButton.SetButtonIcon(Sprites.onlineIcon);
				this._multiplayerButton.interactable = true;
				this._multiplayerButton.onClick = new Button.ButtonClickedEvent();
				this._multiplayerButton.onClick.AddListener(delegate
				{
					try
					{
						Client.Instance.ConnectedAndLoggedServerEvent -= this.ConnectedAndLoggedServerCallback;
						Client.Instance.ConnectedAndLoggedServerEvent += this.ConnectedAndLoggedServerCallback;
						Client.Instance.Connect();
					}
					catch (Exception ex2)
					{
						Logger.log.Error(string.Format("click online button error: {0}", ex2));
					}
				});
			}
			catch (Exception ex)
			{
				Logger.log.Error(string.Format("CreatOnlineButton Error: {0}", ex));
			}
		}

		// Token: 0x060006D7 RID: 1751 RVA: 0x0001C224 File Offset: 0x0001A424
		private void ConnectedAndLoggedServerCallback(int code, string msg)
		{
			Client.Instance.ConnectedAndLoggedServerEvent -= this.ConnectedAndLoggedServerCallback;
			if (code != 0)
			{
				if (PluginUI.simpleDialog == null)
				{
					PluginUI.simpleDialog = Object.Instantiate<GameObject>(this._mainFlow.GetPrivateField("_simpleDialogPromptViewController").gameObject).GetComponent<SimpleDialogPromptViewController>();
				}
				PluginUI.simpleDialog.Init("提示", msg, "知道了", delegate(int selectedButton)
				{
					this._mainFlow.InvokeMethod("DismissViewController", new object[]
					{
						PluginUI.simpleDialog,
						null,
						false
					});
				});
				this._mainFlow.InvokeMethod("PresentViewController", new object[]
				{
					PluginUI.simpleDialog,
					null,
					true
				});
				return;
			}
			MainFlowCoordinator mainFlow = this._mainFlow;
			if (mainFlow == null)
			{
				return;
			}
			mainFlow.InvokeMethod("PresentFlowCoordinator", new object[] { this.multiPlayerFlowCoordinator, null, false, false });
		}

		// Token: 0x060006D8 RID: 1752 RVA: 0x0001C2FC File Offset: 0x0001A4FC
		public void SongsLoaded(Loader sender, Dictionary<string, CustomPreviewBeatmapLevel> levels)
		{
			Action action = this.didDownloadSongEvent;
			if (action == null)
			{
				return;
			}
			action();
		}

		// Token: 0x060006D9 RID: 1753 RVA: 0x0001C310 File Offset: 0x0001A510
		private void CloneLevelDetailView()
		{
			PluginUI.levelDetailClone = Object.Instantiate<GameObject>(Resources.FindObjectsOfTypeAll<StandardLevelDetailView>().First((StandardLevelDetailView x) => x.gameObject.name == "LevelDetail").gameObject);
			PluginUI.levelDetailClone.gameObject.SetActive(false);
			Object.Destroy(PluginUI.levelDetailClone.GetComponent<StandardLevelDetailView>());
			IEnumerable<RectTransform> enumerable = from x in PluginUI.levelDetailClone.GetComponentsInChildren<RectTransform>()
				where x.gameObject.name.StartsWith("BSML")
				select x;
			HoverHint[] componentsInChildren = PluginUI.levelDetailClone.GetComponentsInChildren<HoverHint>();
			LocalizedHoverHint[] componentsInChildren2 = PluginUI.levelDetailClone.GetComponentsInChildren<LocalizedHoverHint>();
			foreach (RectTransform rectTransform in enumerable)
			{
				Object.Destroy(rectTransform.gameObject);
			}
			HoverHint[] array = componentsInChildren;
			for (int i = 0; i < array.Length; i++)
			{
				Object.Destroy(array[i]);
			}
			LocalizedHoverHint[] array2 = componentsInChildren2;
			for (int i = 0; i < array2.Length; i++)
			{
				Object.Destroy(array2[i]);
			}
			Object.Destroy(PluginUI.levelDetailClone.transform.Find("LevelInfo").Find("FavoritesToggle").gameObject);
			Object.Destroy(PluginUI.levelDetailClone.transform.Find("PlayContainer").Find("PlayButtons").gameObject);
			Object.Destroy(PluginUI.levelDetailClone.transform.Find("Stats").Find("MaxCombo").gameObject);
			Object.Destroy(PluginUI.levelDetailClone.transform.Find("Stats").Find("Highscore").gameObject);
			Object.Destroy(PluginUI.levelDetailClone.transform.Find("Stats").Find("MaxRank").gameObject);
		}

		// Token: 0x060006DA RID: 1754 RVA: 0x0001C4FC File Offset: 0x0001A6FC
		private void InitPlayerData()
		{
			this.playerData = Resources.FindObjectsOfTypeAll<PlayerDataModel>().FirstOrDefault<PlayerDataModel>().playerData;
		}

		// Token: 0x04000338 RID: 824
		public static PluginUI instance;

		// Token: 0x04000339 RID: 825
		public static GameObject levelDetailClone;

		// Token: 0x0400033A RID: 826
		public static SimpleDialogPromptViewController simpleDialog;

		// Token: 0x0400033B RID: 827
		public MultiPlayerFlowCoordinator multiPlayerFlowCoordinator;

		// Token: 0x0400033C RID: 828
		public PlayerData playerData;

		// Token: 0x0400033D RID: 829
		public Action didDownloadSongEvent;

		// Token: 0x0400033E RID: 830
		private MainFlowCoordinator _mainFlow;

		// Token: 0x0400033F RID: 831
		private Button _multiplayerButton;

		// Token: 0x04000340 RID: 832
		public static Dictionary<string, string> sdkUserInfo;
	}
}
