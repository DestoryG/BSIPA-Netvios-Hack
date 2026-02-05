using System;
using System.Collections.Generic;
using System.Linq;
using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.ViewControllers;
using CustomSaber.Utilities;

namespace CustomSaber.Settings.UI
{
	// Token: 0x02000015 RID: 21
	internal class SaberSettingsViewController : BSMLResourceViewController
	{
		// Token: 0x17000015 RID: 21
		// (get) Token: 0x06000089 RID: 137 RVA: 0x00004501 File Offset: 0x00002701
		public override string ResourceName
		{
			get
			{
				return "CustomSaber.Settings.UI.Views.saberSettings.bsml";
			}
		}

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x0600008A RID: 138 RVA: 0x00004508 File Offset: 0x00002708
		// (set) Token: 0x0600008B RID: 139 RVA: 0x00004528 File Offset: 0x00002728
		[UIValue("trail-type")]
		public string TrailType
		{
			get
			{
				return Configuration.TrailType.ToString();
			}
			set
			{
				TrailType trailType;
				Configuration.TrailType = (Enum.TryParse<TrailType>(value, out trailType) ? trailType : Configuration.TrailType);
			}
		}

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x0600008C RID: 140 RVA: 0x0000454D File Offset: 0x0000274D
		// (set) Token: 0x0600008D RID: 141 RVA: 0x00004554 File Offset: 0x00002754
		[UIValue("custom-events-enabled")]
		public bool CustomEventsEnabled
		{
			get
			{
				return Configuration.CustomEventsEnabled;
			}
			set
			{
				Configuration.CustomEventsEnabled = value;
			}
		}

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x0600008E RID: 142 RVA: 0x0000455D File Offset: 0x0000275D
		// (set) Token: 0x0600008F RID: 143 RVA: 0x00004564 File Offset: 0x00002764
		[UIValue("random-sabers-enabled")]
		public bool RandomSabersEnabled
		{
			get
			{
				return Configuration.RandomSabersEnabled;
			}
			set
			{
				Configuration.RandomSabersEnabled = value;
			}
		}

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x06000090 RID: 144 RVA: 0x0000456D File Offset: 0x0000276D
		// (set) Token: 0x06000091 RID: 145 RVA: 0x00004574 File Offset: 0x00002774
		[UIValue("sabers-in-menu")]
		public bool ShowSabersInSaberMenu
		{
			get
			{
				return Configuration.ShowSabersInSaberMenu;
			}
			set
			{
				Configuration.ShowSabersInSaberMenu = value;
			}
		}

		// Token: 0x06000092 RID: 146 RVA: 0x00004580 File Offset: 0x00002780
		[UIAction("sabers-in-menu-changed")]
		public void OnSabersInMenuChanged(bool value)
		{
			if (value)
			{
				SaberListViewController instance = SaberListViewController.Instance;
				if (instance != null)
				{
					instance.GenerateHandheldSaberPreview();
				}
			}
			else
			{
				SaberListViewController instance2 = SaberListViewController.Instance;
				if (instance2 != null)
				{
					instance2.ClearHandheldSabers();
				}
				SaberListViewController instance3 = SaberListViewController.Instance;
				if (instance3 != null)
				{
					instance3.ShowMenuHandles();
				}
			}
		}

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x06000093 RID: 147 RVA: 0x000045CC File Offset: 0x000027CC
		// (set) Token: 0x06000094 RID: 148 RVA: 0x000045D3 File Offset: 0x000027D3
		[UIValue("disable-whitestep")]
		public bool DisableWhitestep
		{
			get
			{
				return Configuration.DisableWhitestep;
			}
			set
			{
				Configuration.DisableWhitestep = value;
			}
		}

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x06000095 RID: 149 RVA: 0x000045DC File Offset: 0x000027DC
		// (set) Token: 0x06000096 RID: 150 RVA: 0x000045E3 File Offset: 0x000027E3
		[UIValue("override-trail-length")]
		public bool OverrideTrailLength
		{
			get
			{
				return Configuration.OverrideTrailLength;
			}
			set
			{
				Configuration.OverrideTrailLength = value;
			}
		}

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x06000097 RID: 151 RVA: 0x000045EC File Offset: 0x000027EC
		// (set) Token: 0x06000098 RID: 152 RVA: 0x000045F3 File Offset: 0x000027F3
		[UIValue("trail-length")]
		public float TrailLength
		{
			get
			{
				return Configuration.TrailLength;
			}
			set
			{
				Configuration.TrailLength = value;
			}
		}

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x06000099 RID: 153 RVA: 0x000045FC File Offset: 0x000027FC
		// (set) Token: 0x0600009A RID: 154 RVA: 0x00004603 File Offset: 0x00002803
		[UIValue("saber-width-adjust")]
		public float SaberWidthAdjust
		{
			get
			{
				return Configuration.SaberWidthAdjust;
			}
			set
			{
				Configuration.SaberWidthAdjust = value;
			}
		}

		// Token: 0x0600009B RID: 155 RVA: 0x0000460C File Offset: 0x0000280C
		[UIAction("percent-formatter")]
		public string OnFormatPercent(float obj)
		{
			return string.Format("{0}%", obj * 100f);
		}

		// Token: 0x0600009C RID: 156 RVA: 0x00004624 File Offset: 0x00002824
		[UIAction("refreshPreview")]
		public void RefreshPreview()
		{
			base.StartCoroutine(SaberListViewController.Instance.GenerateSaberPreview(SaberAssetLoader.SelectedSaber));
		}

		// Token: 0x04000061 RID: 97
		[UIValue("trail-type-list")]
		public List<object> trailType = Enum.GetNames(typeof(TrailType)).ToList<object>();
	}
}
