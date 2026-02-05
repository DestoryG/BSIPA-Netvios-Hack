using System;

namespace System.Net
{
	// Token: 0x020001AE RID: 430
	[Serializable]
	internal sealed class EmptyWebProxy : IAutoWebProxy, IWebProxy
	{
		// Token: 0x060010EE RID: 4334 RVA: 0x0005BA3A File Offset: 0x00059C3A
		public Uri GetProxy(Uri uri)
		{
			return uri;
		}

		// Token: 0x060010EF RID: 4335 RVA: 0x0005BA3D File Offset: 0x00059C3D
		public bool IsBypassed(Uri uri)
		{
			return true;
		}

		// Token: 0x170003AC RID: 940
		// (get) Token: 0x060010F0 RID: 4336 RVA: 0x0005BA40 File Offset: 0x00059C40
		// (set) Token: 0x060010F1 RID: 4337 RVA: 0x0005BA48 File Offset: 0x00059C48
		public ICredentials Credentials
		{
			get
			{
				return this.m_credentials;
			}
			set
			{
				this.m_credentials = value;
			}
		}

		// Token: 0x060010F2 RID: 4338 RVA: 0x0005BA51 File Offset: 0x00059C51
		ProxyChain IAutoWebProxy.GetProxies(Uri destination)
		{
			return new DirectProxy(destination);
		}

		// Token: 0x040013E5 RID: 5093
		[NonSerialized]
		private ICredentials m_credentials;
	}
}
