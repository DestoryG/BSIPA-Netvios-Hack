using System;
using System.Collections.Generic;
using UnityEngine;

namespace BeatSaberMarkupLanguage.Components
{
	// Token: 0x0200009E RID: 158
	public class ExternalComponents : MonoBehaviour
	{
		// Token: 0x0600032F RID: 815 RVA: 0x0000F598 File Offset: 0x0000D798
		public T Get<T>() where T : Component
		{
			foreach (Component component in this.components)
			{
				T t = component as T;
				if (t != null)
				{
					return t;
				}
			}
			return default(T);
		}

		// Token: 0x040000CE RID: 206
		public List<Component> components = new List<Component>();
	}
}
