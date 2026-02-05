using System;

namespace System.Net
{
	// Token: 0x020000F2 RID: 242
	[Obsolete("This class has been deprecated. Please use WebRequest.DefaultWebProxy instead to access and set the global default proxy. Use 'null' instead of GetEmptyWebProxy. http://go.microsoft.com/fwlink/?linkid=14202")]
	public class GlobalProxySelection
	{
		// Token: 0x170001A7 RID: 423
		// (get) Token: 0x06000879 RID: 2169 RVA: 0x0002F52C File Offset: 0x0002D72C
		// (set) Token: 0x0600087A RID: 2170 RVA: 0x0002F55A File Offset: 0x0002D75A
		public static IWebProxy Select
		{
			get
			{
				IWebProxy defaultWebProxy = WebRequest.DefaultWebProxy;
				if (defaultWebProxy == null)
				{
					return GlobalProxySelection.GetEmptyWebProxy();
				}
				WebRequest.WebProxyWrapper webProxyWrapper = defaultWebProxy as WebRequest.WebProxyWrapper;
				if (webProxyWrapper != null)
				{
					return webProxyWrapper.WebProxy;
				}
				return defaultWebProxy;
			}
			set
			{
				WebRequest.DefaultWebProxy = value;
			}
		}

		// Token: 0x0600087B RID: 2171 RVA: 0x0002F562 File Offset: 0x0002D762
		public static IWebProxy GetEmptyWebProxy()
		{
			return new EmptyWebProxy();
		}
	}
}
