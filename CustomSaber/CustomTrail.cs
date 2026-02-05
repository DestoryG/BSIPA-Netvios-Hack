using System;
using System.Collections.Generic;
using System.Linq;
using CustomSaber.Settings;
using CustomSaber.Utilities;
using IPA.Utilities;
using UnityEngine;
using Xft;

namespace CustomSaber
{
	// Token: 0x02000006 RID: 6
	[AddComponentMenu("Custom Sabers/Custom Trail")]
	public class CustomTrail : MonoBehaviour
	{
		// Token: 0x0600000A RID: 10 RVA: 0x00002210 File Offset: 0x00000410
		public void Init(Saber saber, ColorManager colorManager)
		{
			Logger.log.Debug(string.Format("Replacing Trail for '{0}'", (saber != null) ? new SaberType?(saber.saberType) : null));
			bool flag = base.gameObject.name != "LeftSaber" && base.gameObject.name != "RightSaber";
			if (flag)
			{
				Logger.log.Warn("Parent not LeftSaber or RightSaber");
				Object.Destroy(this);
			}
			bool flag2 = !saber;
			if (flag2)
			{
				Logger.log.Warn("Saber not found");
				Object.Destroy(this);
			}
			IEnumerable<XWeaponTrail> enumerable = Resources.FindObjectsOfTypeAll<XWeaponTrail>();
			foreach (XWeaponTrail xweaponTrail in enumerable)
			{
				xweaponTrail.SetField("_trailWidth", 0f);
			}
			GameCoreSceneSetup gameCoreSceneSetup = Resources.FindObjectsOfTypeAll<GameCoreSceneSetup>().FirstOrDefault<GameCoreSceneSetup>();
			XWeaponTrail xweaponTrail2;
			if (gameCoreSceneSetup == null)
			{
				xweaponTrail2 = null;
			}
			else
			{
				BasicSaberModelController field = gameCoreSceneSetup.GetField("_basicSaberModelControllerPrefab");
				xweaponTrail2 = ((field != null) ? field.GetField("_saberWeaponTrail") : null);
			}
			XWeaponTrail xweaponTrail3 = xweaponTrail2;
			bool flag3 = xweaponTrail3;
			if (flag3)
			{
				try
				{
					this.oldTrailRendererPrefab = xweaponTrail3.GetField("_trailRendererPrefab");
				}
				catch (Exception ex)
				{
					Logger.log.Error(ex);
					throw;
				}
				bool overrideTrailLength = Configuration.OverrideTrailLength;
				if (overrideTrailLength)
				{
					this.Length = (int)((float)this.Length * Configuration.TrailLength);
				}
				bool flag4 = this.Length > 1;
				if (flag4)
				{
					this.trail = base.gameObject.AddComponent<CustomWeaponTrail>();
					this.trail.Init(this.oldTrailRendererPrefab, colorManager, this.PointStart, this.PointEnd, this.TrailMaterial, this.TrailColor, this.Length, this.MultiplierColor, this.colorType);
				}
			}
			else
			{
				Logger.log.Debug(string.Format("Trail not found for '{0}'", (saber != null) ? new SaberType?(saber.saberType) : null));
				Object.Destroy(this);
			}
		}

		// Token: 0x0600000B RID: 11 RVA: 0x00002448 File Offset: 0x00000648
		public void EnableTrail(bool enable)
		{
			this.trail.enabled = enable;
		}

		// Token: 0x0600000C RID: 12 RVA: 0x00002458 File Offset: 0x00000658
		public void SetMaterial(Material newMat)
		{
			this.TrailMaterial = newMat;
			this.trail.SetMaterial(newMat);
		}

		// Token: 0x0600000D RID: 13 RVA: 0x0000246F File Offset: 0x0000066F
		public void SetColor(Color newColor)
		{
			this.TrailColor = newColor;
			this.trail.SetColor(newColor);
		}

		// Token: 0x04000010 RID: 16
		public Transform PointStart;

		// Token: 0x04000011 RID: 17
		public Transform PointEnd;

		// Token: 0x04000012 RID: 18
		public Material TrailMaterial;

		// Token: 0x04000013 RID: 19
		public ColorType colorType = ColorType.CustomColor;

		// Token: 0x04000014 RID: 20
		public Color TrailColor = new Color(1f, 1f, 1f, 1f);

		// Token: 0x04000015 RID: 21
		public Color MultiplierColor = new Color(1f, 1f, 1f, 1f);

		// Token: 0x04000016 RID: 22
		public int Length = 20;

		// Token: 0x04000017 RID: 23
		private CustomWeaponTrail trail;

		// Token: 0x04000018 RID: 24
		private XWeaponTrailRenderer oldTrailRendererPrefab;
	}
}
