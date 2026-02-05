using System;
using System.Linq;
using System.Reflection;
using BeatSaberMarkupLanguage;
using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.Components;
using BS_Utils.Utilities;
using SongCore.Data;
using SongCore.Utilities;
using UnityEngine;

namespace SongCore.UI
{
	// Token: 0x0200001C RID: 28
	public class RequirementsUI : NotifiableSingleton<RequirementsUI>
	{
		// Token: 0x17000053 RID: 83
		// (get) Token: 0x0600016E RID: 366 RVA: 0x00006FFE File Offset: 0x000051FE
		// (set) Token: 0x0600016F RID: 367 RVA: 0x00007006 File Offset: 0x00005206
		[UIValue("button-glow")]
		public string ButtonGlowColor
		{
			get
			{
				return this.buttonGlowColor;
			}
			set
			{
				this.buttonGlowColor = value;
				base.NotifyPropertyChanged("ButtonGlowColor");
			}
		}

		// Token: 0x17000054 RID: 84
		// (get) Token: 0x06000170 RID: 368 RVA: 0x0000701A File Offset: 0x0000521A
		// (set) Token: 0x06000171 RID: 369 RVA: 0x00007022 File Offset: 0x00005222
		[UIValue("button-interactable")]
		public bool ButtonInteractable
		{
			get
			{
				return this.buttonInteractable;
			}
			set
			{
				this.buttonInteractable = value;
				base.NotifyPropertyChanged("ButtonInteractable");
			}
		}

		// Token: 0x06000172 RID: 370 RVA: 0x00007038 File Offset: 0x00005238
		internal void Setup()
		{
			this.GetIcons();
			this.standardLevel = Resources.FindObjectsOfTypeAll<StandardLevelDetailViewController>().First<StandardLevelDetailViewController>();
			PersistentSingleton<BSMLParser>.instance.Parse(Utilities.GetResourceContent(Assembly.GetExecutingAssembly(), "SongCore.UI.requirements.bsml"), this.standardLevel.transform.Find("LevelDetail").gameObject, this);
			this.infoButtonTransform.localScale *= 0.7f;
		}

		// Token: 0x06000173 RID: 371 RVA: 0x000070AC File Offset: 0x000052AC
		internal void GetIcons()
		{
			if (!this.MissingReqIcon)
			{
				this.MissingReqIcon = Utils.LoadSpriteFromResources("SongCore.Icons.RedX.png", 100f);
			}
			if (!this.HaveReqIcon)
			{
				this.HaveReqIcon = Utils.LoadSpriteFromResources("SongCore.Icons.GreenCheck.png", 100f);
			}
			if (!this.HaveSuggestionIcon)
			{
				this.HaveSuggestionIcon = Utils.LoadSpriteFromResources("SongCore.Icons.YellowCheck.png", 100f);
			}
			if (!this.MissingSuggestionIcon)
			{
				this.MissingSuggestionIcon = Utils.LoadSpriteFromResources("SongCore.Icons.YellowX.png", 100f);
			}
			if (!this.WarningIcon)
			{
				this.WarningIcon = Utils.LoadSpriteFromResources("SongCore.Icons.Warning.png", 100f);
			}
			if (!this.InfoIcon)
			{
				this.InfoIcon = Utils.LoadSpriteFromResources("SongCore.Icons.Info.png", 100f);
			}
		}

		// Token: 0x06000174 RID: 372 RVA: 0x00007188 File Offset: 0x00005388
		[UIAction("button-click")]
		internal void ShowRequirements()
		{
			this.customListTableData.data.Clear();
			if (this.diffData != null && this.diffData.additionalDifficultyData._requirements.Count<string>() > 0)
			{
				foreach (string text in this.diffData.additionalDifficultyData._requirements)
				{
					if (!Collections.capabilities.Contains(text))
					{
						this.customListTableData.data.Add(new CustomListTableData.CustomCellInfo("<size=75%>" + text, "Missing Requirement", this.MissingReqIcon.texture));
					}
					else
					{
						this.customListTableData.data.Add(new CustomListTableData.CustomCellInfo("<size=75%>" + text, "Requirement", this.HaveReqIcon.texture));
					}
				}
			}
			if (this.songData.contributors.Count<ExtraSongData.Contributor>() > 0)
			{
				foreach (ExtraSongData.Contributor contributor in this.songData.contributors)
				{
					if (contributor.icon == null)
					{
						if (!string.IsNullOrWhiteSpace(contributor._iconPath))
						{
							contributor.icon = Utils.LoadSpriteFromFile(this.level.customLevelPath + "/" + contributor._iconPath, 100f);
							this.customListTableData.data.Add(new CustomListTableData.CustomCellInfo(contributor._name, contributor._role, contributor.icon.texture));
						}
						else
						{
							this.customListTableData.data.Add(new CustomListTableData.CustomCellInfo(contributor._name, contributor._role, this.InfoIcon.texture));
						}
					}
					else
					{
						this.customListTableData.data.Add(new CustomListTableData.CustomCellInfo(contributor._name, contributor._role, contributor.icon.texture));
					}
				}
			}
			if (this.wipFolder)
			{
				this.customListTableData.data.Add(new CustomListTableData.CustomCellInfo("<size=70%>WIP Song. Please Play in Practice Mode", "Warning", this.WarningIcon.texture));
			}
			if (this.diffData != null)
			{
				if (this.diffData.additionalDifficultyData._warnings.Count<string>() > 0)
				{
					foreach (string text2 in this.diffData.additionalDifficultyData._warnings)
					{
						this.customListTableData.data.Add(new CustomListTableData.CustomCellInfo("<size=75%>" + text2, "Warning", this.WarningIcon.texture));
					}
				}
				if (this.diffData.additionalDifficultyData._information.Count<string>() > 0)
				{
					foreach (string text3 in this.diffData.additionalDifficultyData._information)
					{
						this.customListTableData.data.Add(new CustomListTableData.CustomCellInfo("<size=75%>" + text3, "Info", this.InfoIcon.texture));
					}
				}
				if (this.diffData.additionalDifficultyData._suggestions.Count<string>() > 0)
				{
					foreach (string text4 in this.diffData.additionalDifficultyData._suggestions)
					{
						if (!Collections.capabilities.Contains(text4))
						{
							this.customListTableData.data.Add(new CustomListTableData.CustomCellInfo("<size=75%>" + text4, "Missing Suggestion", this.MissingSuggestionIcon.texture));
						}
						else
						{
							this.customListTableData.data.Add(new CustomListTableData.CustomCellInfo("<size=75%>" + text4, "Suggestion", this.HaveSuggestionIcon.texture));
						}
					}
				}
			}
			this.customListTableData.tableView.ReloadData();
			this.customListTableData.tableView.ScrollToCellWithIdx(0, 0, false);
		}

		// Token: 0x04000080 RID: 128
		private StandardLevelDetailViewController standardLevel;

		// Token: 0x04000081 RID: 129
		internal static Config ModPrefs = new Config("SongCore/SongCore");

		// Token: 0x04000082 RID: 130
		internal Sprite HaveReqIcon;

		// Token: 0x04000083 RID: 131
		internal Sprite MissingReqIcon;

		// Token: 0x04000084 RID: 132
		internal Sprite HaveSuggestionIcon;

		// Token: 0x04000085 RID: 133
		internal Sprite MissingSuggestionIcon;

		// Token: 0x04000086 RID: 134
		internal Sprite WarningIcon;

		// Token: 0x04000087 RID: 135
		internal Sprite InfoIcon;

		// Token: 0x04000088 RID: 136
		public CustomPreviewBeatmapLevel level;

		// Token: 0x04000089 RID: 137
		public ExtraSongData songData;

		// Token: 0x0400008A RID: 138
		public ExtraSongData.DifficultyData diffData;

		// Token: 0x0400008B RID: 139
		public bool wipFolder;

		// Token: 0x0400008C RID: 140
		[UIComponent("list")]
		public CustomListTableData customListTableData;

		// Token: 0x0400008D RID: 141
		private string buttonGlowColor = "none";

		// Token: 0x0400008E RID: 142
		private bool buttonInteractable;

		// Token: 0x0400008F RID: 143
		[UIComponent("info-button")]
		private Transform infoButtonTransform;
	}
}
