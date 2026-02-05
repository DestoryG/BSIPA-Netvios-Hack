using System;
using System.Collections.Generic;
using System.Linq;
using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.Components;
using BeatSaberMarkupLanguage.ViewControllers;
using BeatSaberMultiplayer.Data;
using BS_Utils.Utilities;
using HMUI;
using Polyglot;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BeatSaberMultiplayer.UI.ViewControllers.RoomView
{
	// Token: 0x02000054 RID: 84
	internal class LevelDetailViewController : BSMLResourceViewController
	{
		// Token: 0x14000017 RID: 23
		// (add) Token: 0x060006F1 RID: 1777 RVA: 0x0001CAC0 File Offset: 0x0001ACC0
		// (remove) Token: 0x060006F2 RID: 1778 RVA: 0x0001CAF8 File Offset: 0x0001ACF8
		public event Action<string, string> SongDetailSelectedEvent;

		// Token: 0x14000018 RID: 24
		// (add) Token: 0x060006F3 RID: 1779 RVA: 0x0001CB30 File Offset: 0x0001AD30
		// (remove) Token: 0x060006F4 RID: 1780 RVA: 0x0001CB68 File Offset: 0x0001AD68
		public event Action StartGameEvent;

		// Token: 0x1700020F RID: 527
		// (get) Token: 0x060006F5 RID: 1781 RVA: 0x0001C5DE File Offset: 0x0001A7DE
		public override string ResourceName
		{
			get
			{
				return string.Join(".", new string[]
				{
					base.GetType().Namespace,
					base.GetType().Name,
					"bsml"
				});
			}
		}

		// Token: 0x17000210 RID: 528
		// (get) Token: 0x060006F6 RID: 1782 RVA: 0x0001CB9D File Offset: 0x0001AD9D
		// (set) Token: 0x060006F7 RID: 1783 RVA: 0x0001CBA8 File Offset: 0x0001ADA8
		[UIValue("selectActionInteractable")]
		public bool SelectActionInteractable
		{
			get
			{
				return this._selectActionInteractable;
			}
			set
			{
				if (value)
				{
					if (Client.Instance.roomStatus == Room.RoomStatus.Playing)
					{
						return;
					}
					this._selectButtonGlow.SetGlow("#5DADE2");
					this.StartActionInteractable = false;
				}
				else
				{
					this._selectButtonGlow.SetGlow("none");
				}
				this._selectActionInteractable = value;
				base.NotifyPropertyChanged("SelectActionInteractable");
			}
		}

		// Token: 0x17000211 RID: 529
		// (get) Token: 0x060006F8 RID: 1784 RVA: 0x0001CC01 File Offset: 0x0001AE01
		// (set) Token: 0x060006F9 RID: 1785 RVA: 0x0001CC09 File Offset: 0x0001AE09
		[UIValue("startActionInteractable")]
		public bool StartActionInteractable
		{
			get
			{
				return this._startActionInteractable;
			}
			set
			{
				this._startActionInteractable = value;
				if (value)
				{
					this._startButtonGlow.SetGlow("#5DADE2");
					this.SelectActionInteractable = false;
				}
				else
				{
					this._startButtonGlow.SetGlow("none");
				}
				base.NotifyPropertyChanged("StartActionInteractable");
			}
		}

		// Token: 0x060006FA RID: 1786 RVA: 0x0001CC49 File Offset: 0x0001AE49
		[UIAction("selectActionButtonPressed")]
		private void SelectActionButtonPressed()
		{
			Action<string, string> songDetailSelectedEvent = this.SongDetailSelectedEvent;
			if (songDetailSelectedEvent != null)
			{
				songDetailSelectedEvent(this._selectedCharacteristicStr, this._selectedDifficultyStr);
			}
			this.SelectActionInteractable = false;
		}

		// Token: 0x060006FB RID: 1787 RVA: 0x0001CC6F File Offset: 0x0001AE6F
		[UIAction("startActionButtonPressed")]
		private void StartActionButtonPressed()
		{
			Action startGameEvent = this.StartGameEvent;
			if (startGameEvent != null)
			{
				startGameEvent();
			}
			this.StartActionInteractable = false;
			this._tipText.text = "游戏马上开始，请准备";
		}

		// Token: 0x060006FC RID: 1788 RVA: 0x0001CC9C File Offset: 0x0001AE9C
		[UIAction("#post-parse")]
		private void Setup()
		{
			(base.transform as RectTransform).anchorMin = new Vector2(0.5f, 0f);
			(base.transform as RectTransform).position = Vector2.zero;
			Vector3 localPosition = this.selectActionButton.transform.parent.parent.localPosition;
			this.selectActionButton.transform.parent.parent.localPosition = localPosition + new Vector3(-42f, -29f, 0f);
			this._selectButtonGlow = this.selectActionButton.GetComponent<Glowable>();
			this._startButtonGlow = this.startActionButton.GetComponent<Glowable>();
			this.SelectActionInteractable = Client.Instance.isHost;
			this.SetupDetailView();
			this._timeText.text = "";
		}

		// Token: 0x060006FD RID: 1789 RVA: 0x0001CD79 File Offset: 0x0001AF79
		public void DownloadLevelCompletedCallback(bool t)
		{
			this.StartActionInteractable = t;
		}

		// Token: 0x060006FE RID: 1790 RVA: 0x0001CD82 File Offset: 0x0001AF82
		public void SwitchTipText()
		{
			if (Client.Instance.roomStatus == Room.RoomStatus.Playing)
			{
				this._tipText.text = "游戏仍在进行中，请稍候";
				return;
			}
			this._tipText.text = "";
		}

		// Token: 0x060006FF RID: 1791 RVA: 0x0001CDB4 File Offset: 0x0001AFB4
		private void SetupDetailView()
		{
			this._levelDetails = Object.Instantiate<GameObject>(PluginUI.levelDetailClone, base.gameObject.transform);
			this._levelDetails.gameObject.SetActive(false);
			this._characteristicSegmentedControllerClone = this._levelDetails.GetComponentInChildren<BeatmapCharacteristicSegmentedControlController>();
			this._characteristicSegmentedControl = LevelDetailViewController.CreateIconSegmentedControl(this._characteristicSegmentedControllerClone.transform as RectTransform, new Vector2(0f, 0f), new Vector2(0f, 0f), delegate(int value)
			{
				if (!Client.Instance.isHost)
				{
					return;
				}
				this.SelectedCharacteristic(this._currentSong.previewDifficultyBeatmapSets[value]);
			});
			this._difficultiesSegmentedControllerClone = this._levelDetails.GetComponentInChildren<BeatmapDifficultySegmentedControlController>();
			this._diffSegmentedControl = LevelDetailViewController.CreateTextSegmentedControl(this._difficultiesSegmentedControllerClone.transform as RectTransform, new Vector2(0f, 0f), new Vector2(0f, 0f), delegate(int value)
			{
				if (!Client.Instance.isHost)
				{
					return;
				}
				this.SelectedDifficulty(this._currentDifficulties[value]);
			}, 3.5f, 1f);
			Vector3 localPosition = this._levelDetails.transform.localPosition;
			this._levelDetails.transform.localPosition = localPosition + new Vector3(-42f, 0f, 0f);
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

		// Token: 0x06000700 RID: 1792 RVA: 0x000196A0 File Offset: 0x000178A0
		private void Update()
		{
		}

		// Token: 0x06000701 RID: 1793 RVA: 0x0001D104 File Offset: 0x0001B304
		private void ClearData()
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
				this.StartActionInteractable = false;
				this._timeText.text = "";
			}
		}

		// Token: 0x06000702 RID: 1794 RVA: 0x0001D1D5 File Offset: 0x0001B3D5
		private void Initialize(IPreviewBeatmapLevel level, Texture2D coverImg)
		{
			this._currentSong = level;
			this._songNameText.text = level.songName;
			if (coverImg != null)
			{
				this._coverImage.texture = coverImg;
			}
			this.SelectActionInteractable = Client.Instance.isHost;
		}

		// Token: 0x06000703 RID: 1795 RVA: 0x0001D214 File Offset: 0x0001B414
		internal void SetContent(IPreviewBeatmapLevel IPlevel, Texture2D coverImg, IBeatmapLevel level, string selectedCharacteristic = "", string selectedDifficulty = "")
		{
			this.ClearData();
			this.Initialize(IPlevel, coverImg);
			this.SwitchTipText();
			this._beatmapLevel = level;
			int num = 0;
			PreviewDifficultyBeatmapSet previewDifficultyBeatmapSet = this._currentSong.previewDifficultyBeatmapSets[0];
			if (!string.IsNullOrWhiteSpace(selectedCharacteristic))
			{
				foreach (PreviewDifficultyBeatmapSet previewDifficultyBeatmapSet2 in this._currentSong.previewDifficultyBeatmapSets)
				{
					if (previewDifficultyBeatmapSet2.beatmapCharacteristic.serializedName == selectedCharacteristic)
					{
						previewDifficultyBeatmapSet = previewDifficultyBeatmapSet2;
						break;
					}
					num++;
				}
			}
			else
			{
				previewDifficultyBeatmapSet = this._currentSong.previewDifficultyBeatmapSets[0];
			}
			if (string.IsNullOrWhiteSpace(selectedDifficulty))
			{
				this._selectedDifficultyStr = previewDifficultyBeatmapSet.beatmapDifficulties[0].ToString();
			}
			else
			{
				this._selectedDifficultyStr = selectedDifficulty;
			}
			this._selectedCharacteristicStr = previewDifficultyBeatmapSet.beatmapCharacteristic.serializedName;
			this.SetupCharacteristicDisplay();
			this.SelectedCharacteristic(previewDifficultyBeatmapSet);
			this._characteristicSegmentedControl.SelectCellWithNumber(num);
		}

		// Token: 0x06000704 RID: 1796 RVA: 0x0001D2FC File Offset: 0x0001B4FC
		private void SelectedCharacteristic(PreviewDifficultyBeatmapSet characteristic)
		{
			this._selectedCharacteristic = characteristic;
			if (Client.Instance.isHost && this._selectedCharacteristicStr != characteristic.beatmapCharacteristic.serializedName)
			{
				this.SelectActionInteractable = true;
				this.StartActionInteractable = false;
			}
			this._selectedCharacteristicStr = characteristic.beatmapCharacteristic.serializedName;
			if (this._diffSegmentedControl != null)
			{
				this.SetupDifficultyDisplay();
			}
		}

		// Token: 0x06000705 RID: 1797 RVA: 0x0001D368 File Offset: 0x0001B568
		private void SetupCharacteristicDisplay()
		{
			IDifficultyBeatmapSet[] difficultyBeatmapSets = this._beatmapLevel.beatmapLevelData.difficultyBeatmapSets;
			List<IconSegmentedControl.DataItem> list = new List<IconSegmentedControl.DataItem>();
			foreach (IDifficultyBeatmapSet difficultyBeatmapSet in difficultyBeatmapSets)
			{
				if (Client.Instance.isHost)
				{
					BeatmapCharacteristicSO beatmapCharacteristic = difficultyBeatmapSet.beatmapCharacteristic;
					list.Add(new IconSegmentedControl.DataItem(beatmapCharacteristic.icon, Localization.Get(beatmapCharacteristic.descriptionLocalizationKey)));
				}
				else if (difficultyBeatmapSet.beatmapCharacteristic.serializedName == this._selectedCharacteristicStr)
				{
					BeatmapCharacteristicSO beatmapCharacteristic2 = difficultyBeatmapSet.beatmapCharacteristic;
					list.Add(new IconSegmentedControl.DataItem(beatmapCharacteristic2.icon, Localization.Get(beatmapCharacteristic2.descriptionLocalizationKey)));
					break;
				}
			}
			this._characteristicSegmentedControl.SetData(list.ToArray());
		}

		// Token: 0x06000706 RID: 1798 RVA: 0x0001D428 File Offset: 0x0001B628
		private void SetupDifficultyDisplay()
		{
			List<string> list = new List<string>();
			foreach (BeatmapDifficulty beatmapDifficulty in this._selectedCharacteristic.beatmapDifficulties)
			{
				if (Client.Instance.isHost)
				{
					list.Add(beatmapDifficulty.ToString());
				}
				else if (this._selectedDifficultyStr == beatmapDifficulty.ToString())
				{
					list.Add(beatmapDifficulty.ToString());
					break;
				}
			}
			for (int j = 0; j < list.Count; j++)
			{
				list[j] = LevelDetailViewController.ToDifficultyName(list[j].ToLower());
			}
			this._currentDifficulties = this._selectedCharacteristic.beatmapDifficulties;
			this._diffSegmentedControl.SetTexts(list.ToArray());
			TextMeshProUGUI[] componentsInChildren = this._diffSegmentedControl.GetComponentsInChildren<TextMeshProUGUI>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].enableWordWrapping = false;
			}
			if (this._currentDifficulties != null)
			{
				if (!string.IsNullOrWhiteSpace(this._selectedDifficultyStr))
				{
					foreach (BeatmapDifficulty beatmapDifficulty2 in this._currentDifficulties)
					{
						if (beatmapDifficulty2.ToString().ToLower() == this._selectedDifficultyStr.ToLower())
						{
							this.SelectedDifficulty(beatmapDifficulty2);
							return;
						}
					}
				}
				this.SelectedDifficulty(this._currentDifficulties[0]);
			}
		}

		// Token: 0x06000707 RID: 1799 RVA: 0x0001D58C File Offset: 0x0001B78C
		private void SelectedDifficulty(BeatmapDifficulty difficulty)
		{
			BeatmapData beatmapData = null;
			bool flag = false;
			foreach (IDifficultyBeatmapSet difficultyBeatmapSet in this._beatmapLevel.beatmapLevelData.difficultyBeatmapSets)
			{
				if (difficultyBeatmapSet.beatmapCharacteristic.descriptionLocalizationKey == this._selectedCharacteristic.beatmapCharacteristic.descriptionLocalizationKey)
				{
					int num = 0;
					foreach (IDifficultyBeatmap difficultyBeatmap in difficultyBeatmapSet.difficultyBeatmaps)
					{
						if (difficultyBeatmap.difficulty.ToString().ToLower() == difficulty.ToString().ToLower())
						{
							beatmapData = difficultyBeatmap.beatmapData;
							this.difficultyBeatmap = difficultyBeatmap;
							if (Client.Instance.isHost && this._selectedDifficultyStr != difficulty.ToString())
							{
								this.SelectActionInteractable = true;
								this.StartActionInteractable = false;
							}
							if (!Client.Instance.isHost)
							{
								this._diffSegmentedControl.SelectCellWithNumber(0);
							}
							else
							{
								this._diffSegmentedControl.SelectCellWithNumber(num);
							}
							this._selectedDifficultyStr = difficulty.ToString();
							flag = true;
							break;
						}
						num++;
					}
				}
				if (flag)
				{
					break;
				}
			}
			if (beatmapData == null)
			{
				return;
			}
			this._timeText.text = string.Format("{0:N0}:{1:00}", Math.Floor((double)this._currentSong.songDuration / 60.0), Math.Floor((double)this._currentSong.songDuration % 60.0));
			this._bpmText.text = this._currentSong.beatsPerMinute.ToString();
			this._npsText.text = ((float)beatmapData.notesCount / this._currentSong.songDuration).ToString("F2");
			this._notesText.text = beatmapData.notesCount.ToString();
			this._obstaclesText.text = beatmapData.obstaclesCount.ToString();
			this._bombsText.text = beatmapData.bombsCount.ToString();
		}

		// Token: 0x06000708 RID: 1800 RVA: 0x0001D7CC File Offset: 0x0001B9CC
		private static IconSegmentedControl CreateIconSegmentedControl(RectTransform parent, Vector2 anchoredPosition, Vector2 sizeDelta, Action<int> onValueChanged = null)
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

		// Token: 0x06000709 RID: 1801 RVA: 0x0001D9BC File Offset: 0x0001BBBC
		private static TextSegmentedControl CreateTextSegmentedControl(RectTransform parent, Vector2 anchoredPosition, Vector2 sizeDelta, Action<int> onValueChanged = null, float fontSize = 4f, float padding = 8f)
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

		// Token: 0x0600070A RID: 1802 RVA: 0x0001DBD0 File Offset: 0x0001BDD0
		private static string ToDifficultyName(string name)
		{
			name = name.ToLower();
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
			if (name == "expertplus")
			{
				return Localization.Get("DIFFICULTY_EXPERT_PLUS");
			}
			return Localization.Get("DIFFICULTY_UNKNOWN");
		}

		// Token: 0x04000355 RID: 853
		private GameObject _levelDetails;

		// Token: 0x04000356 RID: 854
		private IPreviewBeatmapLevel _currentSong;

		// Token: 0x04000357 RID: 855
		private IBeatmapLevel _beatmapLevel;

		// Token: 0x04000358 RID: 856
		private TextMeshProUGUI _songNameText;

		// Token: 0x04000359 RID: 857
		private RawImage _coverImage;

		// Token: 0x0400035A RID: 858
		private TextMeshProUGUI _timeText;

		// Token: 0x0400035B RID: 859
		private TextMeshProUGUI _bpmText;

		// Token: 0x0400035C RID: 860
		private TextMeshProUGUI _npsText;

		// Token: 0x0400035D RID: 861
		private TextMeshProUGUI _notesText;

		// Token: 0x0400035E RID: 862
		private TextMeshProUGUI _obstaclesText;

		// Token: 0x0400035F RID: 863
		private TextMeshProUGUI _bombsText;

		// Token: 0x04000360 RID: 864
		private bool _detailViewSetup;

		// Token: 0x04000361 RID: 865
		private BeatmapDifficultySegmentedControlController _difficultiesSegmentedControllerClone;

		// Token: 0x04000362 RID: 866
		private BeatmapCharacteristicSegmentedControlController _characteristicSegmentedControllerClone;

		// Token: 0x04000363 RID: 867
		private IconSegmentedControl _characteristicSegmentedControl;

		// Token: 0x04000364 RID: 868
		private TextSegmentedControl _diffSegmentedControl;

		// Token: 0x04000365 RID: 869
		private PreviewDifficultyBeatmapSet _selectedCharacteristic;

		// Token: 0x04000366 RID: 870
		private string _selectedCharacteristicStr = "";

		// Token: 0x04000367 RID: 871
		private BeatmapDifficulty[] _currentDifficulties;

		// Token: 0x04000368 RID: 872
		private string _selectedDifficultyStr = "";

		// Token: 0x04000369 RID: 873
		public IDifficultyBeatmap difficultyBeatmap;

		// Token: 0x0400036A RID: 874
		private float _selectButtonClickedInterval = 2f;

		// Token: 0x0400036B RID: 875
		private float _selectButtonClickedTimer;

		// Token: 0x0400036C RID: 876
		private bool _selectButtonClicked;

		// Token: 0x0400036D RID: 877
		private Glowable _selectButtonGlow;

		// Token: 0x0400036E RID: 878
		private bool _selectActionInteractable;

		// Token: 0x0400036F RID: 879
		private Glowable _startButtonGlow;

		// Token: 0x04000370 RID: 880
		private bool _startActionInteractable;

		// Token: 0x04000371 RID: 881
		[UIComponent("selectActionButton")]
		private Button selectActionButton;

		// Token: 0x04000372 RID: 882
		[UIComponent("startActionButton")]
		private Button startActionButton;

		// Token: 0x04000373 RID: 883
		[UIComponent("tip-text")]
		private TextMeshProUGUI _tipText;
	}
}
