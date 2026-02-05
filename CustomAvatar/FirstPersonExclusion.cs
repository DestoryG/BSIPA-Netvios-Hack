using System;
using UnityEngine;

namespace CustomAvatar
{
	// Token: 0x02000012 RID: 18
	public class FirstPersonExclusion : MonoBehaviour
	{
		// Token: 0x0600002D RID: 45 RVA: 0x000030F0 File Offset: 0x000012F0
		public void Awake()
		{
			bool flag = this.Exclude != null && this.Exclude.Length != 0;
			if (flag)
			{
				this.exclude = this.Exclude;
			}
		}

		// Token: 0x04000053 RID: 83
		[HideInInspector]
		public GameObject[] Exclude;

		// Token: 0x04000054 RID: 84
		public GameObject[] exclude;
	}
}
