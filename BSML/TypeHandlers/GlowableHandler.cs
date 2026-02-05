using System;
using System.Collections.Generic;
using BeatSaberMarkupLanguage.Components;

namespace BeatSaberMarkupLanguage.TypeHandlers
{
	// Token: 0x0200001E RID: 30
	[ComponentHandler(typeof(Glowable))]
	public class GlowableHandler : TypeHandler<Glowable>
	{
		// Token: 0x17000031 RID: 49
		// (get) Token: 0x060000CC RID: 204 RVA: 0x00005F58 File Offset: 0x00004158
		public override Dictionary<string, string[]> Props
		{
			get
			{
				return new Dictionary<string, string[]> { 
				{
					"glowColor",
					new string[] { "glow-color" }
				} };
			}
		}

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x060000CD RID: 205 RVA: 0x00005F85 File Offset: 0x00004185
		public override Dictionary<string, Action<Glowable, string>> Setters
		{
			get
			{
				return new Dictionary<string, Action<Glowable, string>> { 
				{
					"glowColor",
					new Action<Glowable, string>(GlowableHandler.SetGlow)
				} };
			}
		}

		// Token: 0x060000CE RID: 206 RVA: 0x00005FA3 File Offset: 0x000041A3
		public static void SetGlow(Glowable glowable, string glowColor)
		{
			glowable.SetGlow(glowColor);
		}
	}
}
