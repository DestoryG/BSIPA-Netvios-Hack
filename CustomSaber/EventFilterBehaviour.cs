using System;
using UnityEngine;

namespace CustomSaber
{
	// Token: 0x02000007 RID: 7
	[RequireComponent(typeof(EventManager))]
	public class EventFilterBehaviour : MonoBehaviour
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x0600000F RID: 15 RVA: 0x000024EC File Offset: 0x000006EC
		protected EventManager EventManager
		{
			get
			{
				bool flag = this.eventManager == null;
				if (flag)
				{
					this.eventManager = base.GetComponent<EventManager>();
				}
				return this.eventManager;
			}
		}

		// Token: 0x04000019 RID: 25
		private EventManager eventManager;
	}
}
