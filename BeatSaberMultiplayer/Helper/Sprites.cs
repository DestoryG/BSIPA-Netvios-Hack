using System;
using System.Linq;
using BeatSaberMarkupLanguage;
using UnityEngine;

namespace BeatSaberMultiplayer.Helper
{
	// Token: 0x02000086 RID: 134
	internal class Sprites
	{
		// Token: 0x17000280 RID: 640
		// (get) Token: 0x0600094E RID: 2382 RVA: 0x00025EFC File Offset: 0x000240FC
		public static Material NoGlowMat
		{
			get
			{
				if (Sprites.noGlowMat == null)
				{
					Sprites.noGlowMat = new Material((from m in Resources.FindObjectsOfTypeAll<Material>()
						where m.name == "UINoGlow"
						select m).First<Material>());
					Sprites.noGlowMat.name = "UINoGlowCustom";
				}
				return Sprites.noGlowMat;
			}
		}

		// Token: 0x0600094F RID: 2383 RVA: 0x00025F64 File Offset: 0x00024164
		public static void ConvertSprites()
		{
			Sprites.onlineIcon = Utilities.FindSpriteInAssembly("BeatSaberMultiplayer.Assets.OnlineIcon.png");
			Sprites.whitePixel = Utilities.FindSpriteInAssembly("BeatSaberMultiplayer.Assets.WhitePixel.png");
			Sprites.speakerIcon = Utilities.FindSpriteInAssembly("BeatSaberMultiplayer.Assets.SpeakerIcon.png");
			Sprites.speakerIcon.texture.wrapMode = 1;
			Sprites.lockedRoomIcon = Utilities.FindSpriteInAssembly("BeatSaberMultiplayer.Assets.LockedRoom.png");
		}

		// Token: 0x04000471 RID: 1137
		public static Sprite onlineIcon;

		// Token: 0x04000472 RID: 1138
		public static Sprite whitePixel;

		// Token: 0x04000473 RID: 1139
		public static Sprite speakerIcon;

		// Token: 0x04000474 RID: 1140
		public static Sprite lockedRoomIcon;

		// Token: 0x04000475 RID: 1141
		private static Material noGlowMat;
	}
}
