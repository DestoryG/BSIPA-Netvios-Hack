using System;
using CustomAvatar.Avatar;
using UnityEngine;

namespace CustomAvatar.UI
{
	// Token: 0x02000030 RID: 48
	internal class AvatarListItem
	{
		// Token: 0x060000F2 RID: 242 RVA: 0x00006DE4 File Offset: 0x00004FE4
		internal AvatarListItem(LoadedAvatar avatar)
		{
			this.name = avatar.descriptor.name;
			this.author = avatar.descriptor.author;
			Sprite cover = avatar.descriptor.cover;
			this.icon = ((cover != null) ? cover.texture : null);
			this.avatar = avatar;
		}

		// Token: 0x060000F3 RID: 243 RVA: 0x00006E3F File Offset: 0x0000503F
		internal AvatarListItem(string name, Texture2D icon)
		{
			this.name = name;
			this.icon = icon;
		}

		// Token: 0x04000171 RID: 369
		public string name;

		// Token: 0x04000172 RID: 370
		public string author;

		// Token: 0x04000173 RID: 371
		public Texture2D icon;

		// Token: 0x04000174 RID: 372
		public LoadedAvatar avatar;
	}
}
