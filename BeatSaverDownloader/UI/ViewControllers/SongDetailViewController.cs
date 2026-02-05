using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.Notify;
using BeatSaberMarkupLanguage.ViewControllers;
using BeatSaverDownloader.Misc;
using BeatSaverSharp;
using BeatSaverSharp.Exceptions;
using HMUI;
using Polyglot;
using SongCore;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BeatSaverDownloader.UI.ViewControllers
{
	// Token: 0x0200001C RID: 28
	public class SongDetailViewController : BSMLResourceViewController, INotifiableHost
	{
		// Token: 0x1700004A RID: 74
		// (get) Token: 0x06000129 RID: 297 RVA: 0x000051D9 File Offset: 0x000033D9
		public override string ResourceName
		{
			get
			{
				return "BeatSaverDownloader.UI.BSML.songDetail.bsml";
			}
		}

		// Token: 0x1700004B RID: 75
		// (get) Token: 0x0600012A RID: 298 RVA: 0x000051E0 File Offset: 0x000033E0
		// (set) Token: 0x0600012B RID: 299 RVA: 0x000051E8 File Offset: 0x000033E8
		[UIValue("downloadInteractable")]
		public bool DownloadInteractable
		{
			get
			{
				return this._downloadInteractable;
			}
			set
			{
				this._downloadInteractable = value;
				base.NotifyPropertyChanged("DownloadInteractable");
			}
		}

		// Token: 0x0600012C RID: 300 RVA: 0x000051FC File Offset: 0x000033FC
		[UIAction("#post-parse")]
		internal void Setup()
		{
			(base.transform as RectTransform).sizeDelta = new Vector2(70f, 0f);
			(base.transform as RectTransform).anchorMin = new Vector2(0.5f, 0f);
			(base.transform as RectTransform).anchorMax = new Vector2(0.5f, 1f);
			this.SetupDetailView();
		}

		// Token: 0x0600012D RID: 301 RVA: 0x0000526C File Offset: 0x0000346C
		[UIAction("downloadPressed")]
		internal void DownloadPressed()
		{
			Action<Beatmap, Texture2D> action = this.didPressDownload;
			if (action != null)
			{
				action(this._currentSong, this._coverImage.texture as Texture2D);
			}
			this.DownloadInteractable = false;
		}

		// Token: 0x0600012E RID: 302 RVA: 0x0000529C File Offset: 0x0000349C
		internal void ClearData()
		{
			if (this._detailViewSetup)
			{
				this._timeText.text = "--";
				this._bpmText.text = "--";
				this._npsText.text = "--";
				this._notesText.text = "--";
				this._obstaclesText.text = "--";
				this._bombsText.text = "--";
				this._songNameText.text = "--";
				this._coverImage.texture = Texture2D.blackTexture;
				this._diffSegmentedControl.SetTexts(new string[0]);
				this._characteristicSegmentedControl.SetData(new IconSegmentedControl.DataItem[0]);
				this.DownloadInteractable = false;
			}
		}

		// Token: 0x0600012F RID: 303 RVA: 0x00005360 File Offset: 0x00003560
		internal async void Initialize(StrongBox<Beatmap> song, Texture2D cover)
		{
			if (song.Value.Partial)
			{
				try
				{
					await song.Value.Populate();
				}
				catch (InvalidPartialException)
				{
					Plugin.log.Warn("Map not found on BeatSaver");
					this._songNameText.text = "Song Not Found";
					return;
				}
			}
			this._currentSong = song.Value;
			this._songNameText.text = this._currentSong.Metadata.SongName;
			if (cover != null)
			{
				this._coverImage.texture = cover;
			}
			this.UpdateDownloadButtonStatus();
			this.SetupCharacteristicDisplay();
			this.SelectedCharacteristic(this._currentSong.Metadata.Characteristics[0]);
			Action<string> action = this.setDescription;
			if (action != null)
			{
				action(this._currentSong.Description);
			}
		}

		// Token: 0x06000130 RID: 304 RVA: 0x000053A7 File Offset: 0x000035A7
		internal void UpdateDownloadButtonStatus()
		{
			this.DownloadInteractable = !SongDownloader.Instance.IsSongDownloaded(this._currentSong.Hash);
		}

		// Token: 0x06000131 RID: 305 RVA: 0x000043AD File Offset: 0x000025AD
		protected override void DidDeactivate(ViewController.DeactivationType deactivationType)
		{
			base.DidDeactivate(deactivationType);
		}

		// Token: 0x06000132 RID: 306 RVA: 0x000053C8 File Offset: 0x000035C8
		internal void SetupDetailView()
		{
			this._levelDetails = Object.Instantiate<GameObject>(PluginUI._levelDetailClone, base.gameObject.transform);
			this._levelDetails.gameObject.SetActive(false);
			this._characteristicSegmentedControllerClone = this._levelDetails.GetComponentInChildren<BeatmapCharacteristicSegmentedControlController>();
			this._characteristicSegmentedControl = SongDetailViewController.CreateIconSegmentedControl(this._characteristicSegmentedControllerClone.transform as RectTransform, new Vector2(0f, 0f), new Vector2(0f, 0f), delegate(int value)
			{
				this.SelectedCharacteristic(this._currentSong.Metadata.Characteristics[value]);
			});
			this._difficultiesSegmentedControllerClone = this._levelDetails.GetComponentInChildren<BeatmapDifficultySegmentedControlController>();
			this._diffSegmentedControl = SongDetailViewController.CreateTextSegmentedControl(this._difficultiesSegmentedControllerClone.transform as RectTransform, new Vector2(0f, 0f), new Vector2(0f, 0f), delegate(int value)
			{
				this.SelectedDifficulty(this._currentDifficulties[value]);
			}, 3.5f, 1f);
			this._songNameText = this._levelDetails.GetComponentsInChildren<TextMeshProUGUI>().First((TextMeshProUGUI x) => x.gameObject.name == "SongNameText");
			this._coverImage = this._levelDetails.transform.Find("LevelInfo").Find("CoverImage").GetComponent<RawImage>();
			this._timeText = this._levelDetails.GetComponentsInChildren<TextMeshProUGUI>().First((TextMeshProUGUI x) => x.gameObject.transform.parent.name == "Time");
			this._bpmText = this._levelDetails.GetComponentsInChildren<TextMeshProUGUI>().First((TextMeshProUGUI x) => x.gameObject.transform.parent.name == "BPM");
			this._npsText = this._levelDetails.GetComponentsInChildren<TextMeshProUGUI>().First((TextMeshProUGUI x) => x.gameObject.transform.parent.name == "NPS");
			this._notesText = this._levelDetails.GetComponentsInChildren<TextMeshProUGUI>().First((TextMeshProUGUI x) => x.gameObject.transform.parent.name == "NotesCount");
			this._obstaclesText = this._levelDetails.GetComponentsInChildren<TextMeshProUGUI>().First((TextMeshProUGUI x) => x.gameObject.transform.parent.name == "ObstaclesCount");
			this._bombsText = this._levelDetails.GetComponentsInChildren<TextMeshProUGUI>().First((TextMeshProUGUI x) => x.gameObject.transform.parent.name == "BombsCount");
			this._timeText.text = "--";
			this._bpmText.text = "--";
			this._npsText.text = "--";
			this._notesText.text = "--";
			this._obstaclesText.text = "--";
			this._bombsText.text = "--";
			this._songNameText.text = "--";
			this._detailViewSetup = true;
			this._levelDetails.gameObject.SetActive(true);
		}

		// Token: 0x06000133 RID: 307 RVA: 0x000056DC File Offset: 0x000038DC
		public void SelectedDifficulty(BeatmapCharacteristicDifficulty difficulty)
		{
			this._timeText.text = string.Format("{0:N0}:{1:00}", Math.Floor((double)difficulty.Length.Value / 60.0), Math.Floor((double)difficulty.Length.Value % 60.0));
			this._bpmText.text = this._currentSong.Metadata.BPM.ToString();
			this._npsText.text = ((float)difficulty.Notes.Value / (float)difficulty.Length.Value).ToString("F2");
			this._notesText.text = difficulty.Notes.ToString();
			this._obstaclesText.text = difficulty.Obstacles.ToString();
			this._bombsText.text = difficulty.Bombs.ToString();
		}

		// Token: 0x06000134 RID: 308 RVA: 0x00005805 File Offset: 0x00003A05
		public void SelectedCharacteristic(BeatmapCharacteristic characteristic)
		{
			this._selectedCharacteristic = characteristic;
			if (this._diffSegmentedControl != null)
			{
				this.SetupDifficultyDisplay();
			}
		}

		// Token: 0x06000135 RID: 309 RVA: 0x00005824 File Offset: 0x00003A24
		internal void SetupDifficultyDisplay()
		{
			List<BeatmapCharacteristicDifficulty> list = new List<BeatmapCharacteristicDifficulty>();
			List<string> list2 = new List<string>(this._selectedCharacteristic.Difficulties.Keys.Where((string x) => this._selectedCharacteristic.Difficulties[x] != null)).OrderBy((string x) => SongDetailViewController.DiffOrder(x)).ToList<string>();
			foreach (string text in list2)
			{
				if (this._selectedCharacteristic.Difficulties[text] != null)
				{
					list.Add(this._selectedCharacteristic.Difficulties[text].Value);
				}
			}
			for (int i = 0; i < list2.Count; i++)
			{
				list2[i] = SongDetailViewController.ToDifficultyName(list2[i]);
			}
			this._currentDifficulties = list.ToArray();
			this._diffSegmentedControl.SetTexts(list2.ToArray());
			TextMeshProUGUI[] componentsInChildren = this._diffSegmentedControl.GetComponentsInChildren<TextMeshProUGUI>();
			for (int j = 0; j < componentsInChildren.Length; j++)
			{
				componentsInChildren[j].enableWordWrapping = false;
			}
			if (list.Count > 0)
			{
				this._diffSegmentedControl.SelectCellWithNumber(0);
			}
			if (this._currentDifficulties != null)
			{
				this.SelectedDifficulty(this._currentDifficulties[0]);
			}
		}

		// Token: 0x06000136 RID: 310 RVA: 0x000059A0 File Offset: 0x00003BA0
		private void SetupCharacteristicDisplay()
		{
			List<IconSegmentedControl.DataItem> list = new List<IconSegmentedControl.DataItem>();
			foreach (BeatmapCharacteristic beatmapCharacteristic in this._currentSong.Metadata.Characteristics)
			{
				BeatmapCharacteristicSO beatmapCharacteristicBySerializedName = Loader.beatmapCharacteristicCollection.GetBeatmapCharacteristicBySerializedName(beatmapCharacteristic.Name);
				if (beatmapCharacteristicBySerializedName.characteristicNameLocalizationKey == "Missing Characteristic")
				{
					list.Add(new IconSegmentedControl.DataItem(beatmapCharacteristicBySerializedName.icon, "Missing Characteristic: " + beatmapCharacteristic.Name));
				}
				else
				{
					list.Add(new IconSegmentedControl.DataItem(beatmapCharacteristicBySerializedName.icon, Localization.Get(beatmapCharacteristicBySerializedName.descriptionLocalizationKey)));
				}
			}
			this._characteristicSegmentedControl.SetData(list.ToArray());
		}

		// Token: 0x06000137 RID: 311 RVA: 0x00005A74 File Offset: 0x00003C74
		internal static string ToDifficultyName(string name)
		{
			if (name == "easy")
			{
				return Localization.Get("DIFFICULTY_EASY");
			}
			if (name == "normal")
			{
				return Localization.Get("DIFFICULTY_NORMAL");
			}
			if (name == "hard")
			{
				return Localization.Get("DIFFICULTY_HARD");
			}
			if (name == "expert")
			{
				return Localization.Get("DIFFICULTY_EXPERT");
			}
			if (name == "expertPlus")
			{
				return Localization.Get("DIFFICULTY_EXPERT_PLUS");
			}
			return Localization.Get("DIFFICULTY_UNKNOWN");
		}

		// Token: 0x06000138 RID: 312 RVA: 0x00005B04 File Offset: 0x00003D04
		internal static int DiffOrder(string name)
		{
			if (name != null)
			{
				if (name == "easy")
				{
					return 0;
				}
				if (name == "normal")
				{
					return 1;
				}
				if (name == "hard")
				{
					return 2;
				}
				if (name == "expert")
				{
					return 3;
				}
				if (name == "expertPlus")
				{
					return 4;
				}
			}
			return 5;
		}

		// Token: 0x06000139 RID: 313 RVA: 0x00005B64 File Offset: 0x00003D64
		public static TextSegmentedControl CreateTextSegmentedControl(RectTransform parent, Vector2 anchoredPosition, Vector2 sizeDelta, Action<int> onValueChanged = null, float fontSize = 4f, float padding = 8f)
		{
			TextSegmentedControl textSegmentedControl = new GameObject("CustomTextSegmentedControl", new Type[] { typeof(RectTransform) }).AddComponent<TextSegmentedControl>();
			textSegmentedControl.gameObject.AddComponent<HorizontalLayoutGroup>();
			TextSegmentedControlCellNew[] array = Resources.FindObjectsOfTypeAll<TextSegmentedControlCellNew>();
			textSegmentedControl.SetPrivateField("_singleCellPrefab", array.First((TextSegmentedControlCellNew x) => x.name == "HSingleTextSegmentedControlCell"));
			textSegmentedControl.SetPrivateField("_firstCellPrefab", array.First((TextSegmentedControlCellNew x) => x.name == "LeftTextSegmentedControlCell"));
			textSegmentedControl.SetPrivateField("_middleCellPrefab", array.Last((TextSegmentedControlCellNew x) => x.name == "HMiddleTextSegmentedControlCell"));
			textSegmentedControl.SetPrivateField("_lastCellPrefab", array.Last((TextSegmentedControlCellNew x) => x.name == "RightTextSegmentedControlCell"));
			textSegmentedControl.SetPrivateField("_container", (from x in Resources.FindObjectsOfTypeAll<TextSegmentedControl>()
				select x.GetPrivateField("_container")).First((object x) => x != null));
			textSegmentedControl.transform.SetParent(parent, false);
			(textSegmentedControl.transform as RectTransform).anchorMax = new Vector2(0.5f, 0.5f);
			(textSegmentedControl.transform as RectTransform).anchorMin = new Vector2(0.5f, 0.5f);
			(textSegmentedControl.transform as RectTransform).anchoredPosition = anchoredPosition;
			(textSegmentedControl.transform as RectTransform).sizeDelta = sizeDelta;
			textSegmentedControl.SetPrivateField("_fontSize", fontSize);
			textSegmentedControl.SetPrivateField("_padding", padding);
			if (onValueChanged != null)
			{
				textSegmentedControl.didSelectCellEvent += delegate(SegmentedControl sender, int index)
				{
					onValueChanged(index);
				};
			}
			return textSegmentedControl;
		}

		// Token: 0x0600013A RID: 314 RVA: 0x00005D78 File Offset: 0x00003F78
		public static IconSegmentedControl CreateIconSegmentedControl(RectTransform parent, Vector2 anchoredPosition, Vector2 sizeDelta, Action<int> onValueChanged = null)
		{
			IconSegmentedControl iconSegmentedControl = new GameObject("CustomIconSegmentedControl", new Type[] { typeof(RectTransform) }).AddComponent<IconSegmentedControl>();
			iconSegmentedControl.gameObject.AddComponent<HorizontalLayoutGroup>();
			IconSegmentedControlCell[] array = Resources.FindObjectsOfTypeAll<IconSegmentedControlCell>();
			iconSegmentedControl.SetPrivateField("_singleCellPrefab", array.First((IconSegmentedControlCell x) => x.name == "SingleIconSegmentedControlCell"));
			iconSegmentedControl.SetPrivateField("_firstCellPrefab", array.First((IconSegmentedControlCell x) => x.name == "LeftIconSegmentedControlCell"));
			iconSegmentedControl.SetPrivateField("_middleCellPrefab", array.First((IconSegmentedControlCell x) => x.name == "HMiddleIconSegmentedControlCell"));
			iconSegmentedControl.SetPrivateField("_lastCellPrefab", array.First((IconSegmentedControlCell x) => x.name == "RightIconSegmentedControlCell"));
			iconSegmentedControl.SetPrivateField("_container", (from x in Resources.FindObjectsOfTypeAll<IconSegmentedControl>()
				select x.GetPrivateField("_container")).First((object x) => x != null));
			iconSegmentedControl.transform.SetParent(parent, false);
			(iconSegmentedControl.transform as RectTransform).anchorMax = new Vector2(0.5f, 0.5f);
			(iconSegmentedControl.transform as RectTransform).anchorMin = new Vector2(0.5f, 0.5f);
			(iconSegmentedControl.transform as RectTransform).anchoredPosition = anchoredPosition;
			(iconSegmentedControl.transform as RectTransform).sizeDelta = sizeDelta;
			if (onValueChanged != null)
			{
				iconSegmentedControl.didSelectCellEvent += delegate(SegmentedControl sender, int index)
				{
					onValueChanged(index);
				};
			}
			return iconSegmentedControl;
		}

		// Token: 0x0400006B RID: 107
		private GameObject _levelDetails;

		// Token: 0x0400006C RID: 108
		private bool _detailViewSetup;

		// Token: 0x0400006D RID: 109
		private Beatmap _currentSong;

		// Token: 0x0400006E RID: 110
		private BeatmapDifficultySegmentedControlController _difficultiesSegmentedControllerClone;

		// Token: 0x0400006F RID: 111
		private BeatmapCharacteristicSegmentedControlController _characteristicSegmentedControllerClone;

		// Token: 0x04000070 RID: 112
		private TextSegmentedControl _diffSegmentedControl;

		// Token: 0x04000071 RID: 113
		private IconSegmentedControl _characteristicSegmentedControl;

		// Token: 0x04000072 RID: 114
		private BeatmapCharacteristic _selectedCharacteristic;

		// Token: 0x04000073 RID: 115
		private BeatmapCharacteristicDifficulty[] _currentDifficulties;

		// Token: 0x04000074 RID: 116
		private TextMeshProUGUI _songNameText;

		// Token: 0x04000075 RID: 117
		private RawImage _coverImage;

		// Token: 0x04000076 RID: 118
		private TextMeshProUGUI _timeText;

		// Token: 0x04000077 RID: 119
		private TextMeshProUGUI _bpmText;

		// Token: 0x04000078 RID: 120
		private TextMeshProUGUI _npsText;

		// Token: 0x04000079 RID: 121
		private TextMeshProUGUI _notesText;

		// Token: 0x0400007A RID: 122
		private TextMeshProUGUI _obstaclesText;

		// Token: 0x0400007B RID: 123
		private TextMeshProUGUI _bombsText;

		// Token: 0x0400007C RID: 124
		private bool _downloadInteractable;

		// Token: 0x0400007D RID: 125
		public Action<Beatmap, Texture2D> didPressDownload;

		// Token: 0x0400007E RID: 126
		public Action<User> didPressUploader;

		// Token: 0x0400007F RID: 127
		public Action<string> setDescription;
	}
}
