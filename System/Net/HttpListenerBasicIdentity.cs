using System;
using System.Security.Principal;

namespace System.Net
{
	// Token: 0x020000F3 RID: 243
	public class HttpListenerBasicIdentity : GenericIdentity
	{
		// Token: 0x0600087D RID: 2173 RVA: 0x0002F571 File Offset: 0x0002D771
		public HttpListenerBasicIdentity(string username, string password)
			: base(username, "Basic")
		{
			this.m_Password = password;
		}

		// Token: 0x170001A8 RID: 424
		// (get) Token: 0x0600087E RID: 2174 RVA: 0x0002F586 File Offset: 0x0002D786
		public virtual string Password
		{
			get
			{
				return this.m_Password;
			}
		}

		// Token: 0x04000DD8 RID: 3544
		private string m_Password;
	}
}
