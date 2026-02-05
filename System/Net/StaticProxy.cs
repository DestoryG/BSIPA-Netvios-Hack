using System;

namespace System.Net
{
	// Token: 0x020001E2 RID: 482
	internal class StaticProxy : ProxyChain
	{
		// Token: 0x060012C9 RID: 4809 RVA: 0x00063742 File Offset: 0x00061942
		internal StaticProxy(Uri destination, Uri proxy)
			: base(destination)
		{
			if (proxy == null)
			{
				throw new ArgumentNullException("proxy");
			}
			this.m_Proxy = proxy;
		}

		// Token: 0x060012CA RID: 4810 RVA: 0x00063766 File Offset: 0x00061966
		protected override bool GetNextProxy(out Uri proxy)
		{
			proxy = this.m_Proxy;
			if (proxy == null)
			{
				return false;
			}
			this.m_Proxy = null;
			return true;
		}

		// Token: 0x04001519 RID: 5401
		private Uri m_Proxy;
	}
}
