using System;
using UnityEngine;

namespace SongCore.Data
{
	// Token: 0x02000031 RID: 49
	public class ModSeperateSongFolder : SeperateSongFolder
	{
		// Token: 0x1700005A RID: 90
		// (get) Token: 0x060001AF RID: 431 RVA: 0x000087F4 File Offset: 0x000069F4
		// (set) Token: 0x060001B0 RID: 432 RVA: 0x000087FC File Offset: 0x000069FC
		public bool AlwaysShow { get; set; } = true;

		// Token: 0x060001B1 RID: 433 RVA: 0x00008805 File Offset: 0x00006A05
		public ModSeperateSongFolder(SongFolderEntry folderEntry)
			: base(folderEntry)
		{
		}

		// Token: 0x060001B2 RID: 434 RVA: 0x00008815 File Offset: 0x00006A15
		public ModSeperateSongFolder(SongFolderEntry folderEntry, Sprite Image)
			: base(folderEntry, Image)
		{
		}
	}
}
