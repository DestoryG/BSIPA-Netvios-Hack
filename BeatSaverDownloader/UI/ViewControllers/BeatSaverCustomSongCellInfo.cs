using System;
using BeatSaberMarkupLanguage.Components;
using BeatSaverDownloader.Misc;
using BeatSaverSharp;
using UnityEngine;

namespace BeatSaverDownloader.UI.ViewControllers
{
	// Token: 0x02000017 RID: 23
	public class BeatSaverCustomSongCellInfo : CustomListTableData.CustomCellInfo
	{
		// Token: 0x06000108 RID: 264 RVA: 0x00004A2F File Offset: 0x00002C2F
		public BeatSaverCustomSongCellInfo(Beatmap song, Action<CustomListTableData.CustomCellInfo> callback, string text, string subtext = null)
			: base(text, subtext, null)
		{
			this._song = song;
			this._callback = callback;
			this.LoadImage();
		}

		// Token: 0x06000109 RID: 265 RVA: 0x00004A50 File Offset: 0x00002C50
		protected async void LoadImage()
		{
			Texture2D texture2D = Sprites.LoadTextureRaw(await this._song.FetchCoverImage(null));
			this.icon = texture2D;
			this._callback(this);
		}

		// Token: 0x04000051 RID: 81
		protected Beatmap _song;

		// Token: 0x04000052 RID: 82
		private Action<CustomListTableData.CustomCellInfo> _callback;
	}
}
