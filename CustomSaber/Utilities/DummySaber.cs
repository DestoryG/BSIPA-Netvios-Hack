using System;
using System.Linq;
using CustomSaber.Settings;
using IPA.Utilities;
using UnityEngine;
using Xft;

namespace CustomSaber.Utilities
{
	// Token: 0x0200000F RID: 15
	internal class DummySaber : MonoBehaviour
	{
		// Token: 0x06000032 RID: 50 RVA: 0x000028D0 File Offset: 0x00000AD0
		private void Start()
		{
			PlayerDataModel playerDataModel = Resources.FindObjectsOfTypeAll<PlayerDataModel>().FirstOrDefault<PlayerDataModel>();
			PlayerData field = playerDataModel.GetField("_playerData");
			ColorSchemesSettings colorSchemesSettings = field.colorSchemesSettings;
			ColorScheme colorScheme = (colorSchemesSettings.overrideDefaultColors ? colorSchemesSettings.GetColorSchemeForId(colorSchemesSettings.selectedColorSchemeId) : this.GetDefaultColorScheme());
			Color color = ((base.gameObject.name == "LeftSaber") ? colorScheme.saberAColor : colorScheme.saberBColor);
			bool flag = SaberAssetLoader.SelectedSaber == 0;
			if (flag)
			{
				foreach (Renderer renderer in base.gameObject.GetComponentsInChildren<Renderer>())
				{
					foreach (Material material in renderer.materials)
					{
						material.color = color;
						bool flag2 = material.HasProperty("_Color");
						if (flag2)
						{
							material.SetColor("_Color", color);
						}
						bool flag3 = material.HasProperty("_TintColor");
						if (flag3)
						{
							material.SetColor("_TintColor", color);
						}
						bool flag4 = material.HasProperty("_AddColor");
						if (flag4)
						{
							material.SetColor("_AddColor", ColorExtensions.ColorWithAlpha(color * 0.5f, 0f));
						}
					}
				}
			}
			else
			{
				SaberScript.ApplyColorsToSaber(base.gameObject, color);
			}
			XWeaponTrail[] componentsInChildren2 = base.gameObject.GetComponentsInChildren<XWeaponTrail>();
			foreach (XWeaponTrail xweaponTrail in componentsInChildren2)
			{
				xweaponTrail.color = color;
			}
			base.transform.localScale = new Vector3(Configuration.SaberWidthAdjust, Configuration.SaberWidthAdjust, 1f);
		}

		// Token: 0x06000033 RID: 51 RVA: 0x00002A98 File Offset: 0x00000C98
		private ColorScheme GetDefaultColorScheme()
		{
			ColorManager colorManager = Resources.FindObjectsOfTypeAll<ColorManager>().FirstOrDefault<ColorManager>();
			return colorManager.GetField("_colorScheme");
		}
	}
}
