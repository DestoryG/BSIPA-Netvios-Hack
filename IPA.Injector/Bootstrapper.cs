using System;
using UnityEngine;

namespace IPA.Injector
{
	// Token: 0x02000003 RID: 3
	internal class Bootstrapper : MonoBehaviour
	{
		// Token: 0x14000001 RID: 1
		// (add) Token: 0x06000005 RID: 5 RVA: 0x0000217C File Offset: 0x0000037C
		// (remove) Token: 0x06000006 RID: 6 RVA: 0x000021B4 File Offset: 0x000003B4
		public event Action Destroyed = delegate
		{
		};

		// Token: 0x06000007 RID: 7 RVA: 0x000021E9 File Offset: 0x000003E9
		public void Awake()
		{
		}

		// Token: 0x06000008 RID: 8 RVA: 0x000021EB File Offset: 0x000003EB
		public void Start()
		{
			Object.Destroy(base.gameObject);
		}

		// Token: 0x06000009 RID: 9 RVA: 0x000021F8 File Offset: 0x000003F8
		public void OnDestroy()
		{
			this.Destroyed();
		}
	}
}
