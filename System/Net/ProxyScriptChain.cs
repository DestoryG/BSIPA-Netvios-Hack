using System;

namespace System.Net
{
	// Token: 0x020001E0 RID: 480
	internal class ProxyScriptChain : ProxyChain
	{
		// Token: 0x060012C4 RID: 4804 RVA: 0x0006366D File Offset: 0x0006186D
		internal ProxyScriptChain(WebProxy proxy, Uri destination)
			: base(destination)
		{
			this.m_Proxy = proxy;
		}

		// Token: 0x060012C5 RID: 4805 RVA: 0x00063680 File Offset: 0x00061880
		protected override bool GetNextProxy(out Uri proxy)
		{
			if (this.m_CurrentIndex < 0)
			{
				proxy = null;
				return false;
			}
			if (this.m_CurrentIndex == 0)
			{
				this.m_ScriptProxies = this.m_Proxy.GetProxiesAuto(base.Destination, ref this.m_SyncStatus);
			}
			if (this.m_ScriptProxies == null || this.m_CurrentIndex >= this.m_ScriptProxies.Length)
			{
				proxy = this.m_Proxy.GetProxyAutoFailover(base.Destination);
				this.m_CurrentIndex = -1;
				return true;
			}
			Uri[] scriptProxies = this.m_ScriptProxies;
			int currentIndex = this.m_CurrentIndex;
			this.m_CurrentIndex = currentIndex + 1;
			proxy = scriptProxies[currentIndex];
			return true;
		}

		// Token: 0x060012C6 RID: 4806 RVA: 0x0006370F File Offset: 0x0006190F
		internal override void Abort()
		{
			this.m_Proxy.AbortGetProxiesAuto(ref this.m_SyncStatus);
		}

		// Token: 0x04001514 RID: 5396
		private WebProxy m_Proxy;

		// Token: 0x04001515 RID: 5397
		private Uri[] m_ScriptProxies;

		// Token: 0x04001516 RID: 5398
		private int m_CurrentIndex;

		// Token: 0x04001517 RID: 5399
		private int m_SyncStatus;
	}
}
