using System;
using UnityEngine;

namespace CustomAvatar
{
	// Token: 0x02000014 RID: 20
	[RequireComponent(typeof(EventManager))]
	public class EventFilterBehaviour : MonoBehaviour
	{
		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000032 RID: 50 RVA: 0x000031A8 File Offset: 0x000013A8
		protected EventManager EventManager
		{
			get
			{
				bool flag = this._eventManager == null;
				if (flag)
				{
					this._eventManager = base.GetComponent<EventManager>();
				}
				return this._eventManager;
			}
		}

		// Token: 0x0400005F RID: 95
		private EventManager _eventManager;
	}
}
