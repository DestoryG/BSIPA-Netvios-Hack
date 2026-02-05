using System;

namespace System.Net
{
	// Token: 0x020001E1 RID: 481
	internal class DirectProxy : ProxyChain
	{
		// Token: 0x060012C7 RID: 4807 RVA: 0x00063722 File Offset: 0x00061922
		internal DirectProxy(Uri destination)
			: base(destination)
		{
		}

		// Token: 0x060012C8 RID: 4808 RVA: 0x0006372B File Offset: 0x0006192B
		protected override bool GetNextProxy(out Uri proxy)
		{
			proxy = null;
			if (this.m_ProxyRetrieved)
			{
				return false;
			}
			this.m_ProxyRetrieved = true;
			return true;
		}

		// Token: 0x04001518 RID: 5400
		private bool m_ProxyRetrieved;
	}
}
