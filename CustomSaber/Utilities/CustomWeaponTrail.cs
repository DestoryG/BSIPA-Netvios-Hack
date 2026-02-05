using System;
using CustomSaber.Settings;
using IPA.Utilities;
using UnityEngine;
using Xft;

namespace CustomSaber.Utilities
{
	// Token: 0x0200000D RID: 13
	internal class CustomWeaponTrail : XWeaponTrail
	{
		// Token: 0x17000006 RID: 6
		// (get) Token: 0x06000026 RID: 38 RVA: 0x000026EC File Offset: 0x000008EC
		public override Color color
		{
			get
			{
				Color color = this._customColor * this._multiplierSaberColor;
				bool flag = this._colorManager;
				if (flag)
				{
					bool flag2 = this._saberType.Equals(ColorType.LeftSaber);
					if (flag2)
					{
						color = this._colorManager.ColorForSaberType(0) * this._multiplierSaberColor;
					}
					else
					{
						bool flag3 = this._saberType.Equals(ColorType.RightSaber);
						if (flag3)
						{
							color = this._colorManager.ColorForSaberType(1) * this._multiplierSaberColor;
						}
					}
				}
				return color;
			}
		}

		// Token: 0x06000027 RID: 39 RVA: 0x00002790 File Offset: 0x00000990
		public void Init(XWeaponTrailRenderer TrailRendererPrefab, ColorManager colorManager, Transform PointStart, Transform PointEnd, Material TrailMaterial, Color TrailColor, int Length, Color multiplierSaberColor, ColorType colorType)
		{
			this._colorManager = colorManager;
			this._multiplierSaberColor = multiplierSaberColor;
			this._customColor = TrailColor;
			this._customMaterial = TrailMaterial;
			this._saberType = colorType;
			this._pointStart = PointStart;
			this._pointEnd = PointEnd;
			this._maxFrame = Length;
			this._trailRendererPrefab = TrailRendererPrefab;
		}

		// Token: 0x06000028 RID: 40 RVA: 0x000027E4 File Offset: 0x000009E4
		public override void Start()
		{
			base.Start();
			this._trailRenderer.GetField("_meshRenderer").material = this._customMaterial;
			bool disableWhitestep = Configuration.DisableWhitestep;
			if (disableWhitestep)
			{
				this.SetField("_whiteSteps", 0);
			}
		}

		// Token: 0x06000029 RID: 41 RVA: 0x0000282B File Offset: 0x00000A2B
		public void SetColor(Color newColor)
		{
			this._customColor = newColor;
		}

		// Token: 0x0600002A RID: 42 RVA: 0x00002835 File Offset: 0x00000A35
		public void SetMaterial(Material newMaterial)
		{
			this._customMaterial = newMaterial;
			this._trailRenderer.GetField("_meshRenderer").material = this._customMaterial;
		}

		// Token: 0x0400002E RID: 46
		public ColorType _saberType;

		// Token: 0x0400002F RID: 47
		public ColorManager _colorManager;

		// Token: 0x04000030 RID: 48
		public Color _multiplierSaberColor;

		// Token: 0x04000031 RID: 49
		public Color _customColor;

		// Token: 0x04000032 RID: 50
		public Material _customMaterial;
	}
}
