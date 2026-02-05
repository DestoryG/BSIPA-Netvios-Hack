using System;
using UnityEngine;

namespace BeatSaberMarkupLanguage.Tags
{
	// Token: 0x02000040 RID: 64
	public abstract class BSMLTag
	{
		// Token: 0x17000063 RID: 99
		// (get) Token: 0x06000144 RID: 324
		public abstract string[] Aliases { get; }

		// Token: 0x17000064 RID: 100
		// (get) Token: 0x06000145 RID: 325 RVA: 0x00008B80 File Offset: 0x00006D80
		public virtual bool AddChildren
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06000146 RID: 326
		public abstract GameObject CreateObject(Transform parent);

		// Token: 0x06000147 RID: 327 RVA: 0x0000263A File Offset: 0x0000083A
		public virtual void Setup()
		{
		}

		// Token: 0x04000036 RID: 54
		public bool isInitialized;
	}
}
