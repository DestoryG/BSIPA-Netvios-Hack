using System;

namespace SongCore.Data
{
	// Token: 0x0200002F RID: 47
	[Serializable]
	public class SongFolderEntry
	{
		// Token: 0x060001A5 RID: 421 RVA: 0x000084D0 File Offset: 0x000066D0
		public SongFolderEntry(string name, string path, FolderLevelPack pack, string imagePath = "", bool wip = false)
		{
			this.Name = name;
			this.Path = path;
			this.Pack = pack;
			this.ImagePath = imagePath;
			this.WIP = wip;
		}

		// Token: 0x0400009D RID: 157
		public string Name;

		// Token: 0x0400009E RID: 158
		public string Path;

		// Token: 0x0400009F RID: 159
		public FolderLevelPack Pack;

		// Token: 0x040000A0 RID: 160
		public string ImagePath;

		// Token: 0x040000A1 RID: 161
		public bool WIP;
	}
}
