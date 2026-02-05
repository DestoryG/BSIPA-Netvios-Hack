using System;
using UnityEngine;

namespace NetViosCommon.Utility
{
	// Token: 0x02000009 RID: 9
	public class SDKDataComponent : MonoBehaviour
	{
		// Token: 0x14000001 RID: 1
		// (add) Token: 0x06000020 RID: 32 RVA: 0x00002360 File Offset: 0x00000560
		// (remove) Token: 0x06000021 RID: 33 RVA: 0x00002398 File Offset: 0x00000598
		public event SDKDataComponent.SDKDataHandler OnSDKData;

		// Token: 0x06000022 RID: 34 RVA: 0x000023CD File Offset: 0x000005CD
		protected void triggerEvent(string sdkData)
		{
			if (this.OnSDKData != null)
			{
				this.OnSDKData(sdkData);
			}
		}

		// Token: 0x0200000B RID: 11
		// (Invoke) Token: 0x0600002B RID: 43
		public delegate void SDKDataHandler(string sdkData);
	}
}
