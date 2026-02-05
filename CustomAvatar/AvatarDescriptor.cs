using System;
using UnityEngine;

namespace CustomAvatar
{
	// Token: 0x02000013 RID: 19
	public class AvatarDescriptor : MonoBehaviour, ISerializationCallbackReceiver
	{
		// Token: 0x0600002F RID: 47 RVA: 0x00003123 File Offset: 0x00001323
		public void OnBeforeSerialize()
		{
		}

		// Token: 0x06000030 RID: 48 RVA: 0x00003128 File Offset: 0x00001328
		public void OnAfterDeserialize()
		{
			string text;
			if ((text = this.name) == null)
			{
				text = this.Name ?? this.AvatarName;
			}
			this.name = text;
			string text2;
			if ((text2 = this.author) == null)
			{
				text2 = this.Author ?? this.AuthorName;
			}
			this.author = text2;
			Sprite sprite;
			if ((sprite = this.cover) == null)
			{
				sprite = this.Cover ?? this.CoverImage;
			}
			this.cover = sprite;
		}

		// Token: 0x04000055 RID: 85
		public string name;

		// Token: 0x04000056 RID: 86
		public string author;

		// Token: 0x04000057 RID: 87
		public bool allowHeightCalibration = true;

		// Token: 0x04000058 RID: 88
		public Sprite cover;

		// Token: 0x04000059 RID: 89
		[SerializeField]
		[HideInInspector]
		private string AvatarName;

		// Token: 0x0400005A RID: 90
		[SerializeField]
		[HideInInspector]
		private string AuthorName;

		// Token: 0x0400005B RID: 91
		[SerializeField]
		[HideInInspector]
		private Sprite CoverImage;

		// Token: 0x0400005C RID: 92
		[SerializeField]
		[HideInInspector]
		private string Name;

		// Token: 0x0400005D RID: 93
		[SerializeField]
		[HideInInspector]
		private string Author;

		// Token: 0x0400005E RID: 94
		[SerializeField]
		[HideInInspector]
		private Sprite Cover;
	}
}
