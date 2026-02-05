using System;

namespace System.Net.Configuration
{
	// Token: 0x02000345 RID: 837
	internal sealed class SmtpNetworkElementInternal
	{
		// Token: 0x06001E12 RID: 7698 RVA: 0x0008D4CC File Offset: 0x0008B6CC
		internal SmtpNetworkElementInternal(SmtpNetworkElement element)
		{
			this.host = element.Host;
			this.port = element.Port;
			this.targetname = element.TargetName;
			this.clientDomain = element.ClientDomain;
			this.enableSsl = element.EnableSsl;
			if (element.DefaultCredentials)
			{
				this.credential = (NetworkCredential)CredentialCache.DefaultCredentials;
				return;
			}
			if (element.UserName != null && element.UserName.Length > 0)
			{
				this.credential = new NetworkCredential(element.UserName, element.Password);
			}
		}

		// Token: 0x170007C0 RID: 1984
		// (get) Token: 0x06001E13 RID: 7699 RVA: 0x0008D561 File Offset: 0x0008B761
		internal NetworkCredential Credential
		{
			get
			{
				return this.credential;
			}
		}

		// Token: 0x170007C1 RID: 1985
		// (get) Token: 0x06001E14 RID: 7700 RVA: 0x0008D569 File Offset: 0x0008B769
		internal string Host
		{
			get
			{
				return this.host;
			}
		}

		// Token: 0x170007C2 RID: 1986
		// (get) Token: 0x06001E15 RID: 7701 RVA: 0x0008D571 File Offset: 0x0008B771
		internal string ClientDomain
		{
			get
			{
				return this.clientDomain;
			}
		}

		// Token: 0x170007C3 RID: 1987
		// (get) Token: 0x06001E16 RID: 7702 RVA: 0x0008D579 File Offset: 0x0008B779
		internal int Port
		{
			get
			{
				return this.port;
			}
		}

		// Token: 0x170007C4 RID: 1988
		// (get) Token: 0x06001E17 RID: 7703 RVA: 0x0008D581 File Offset: 0x0008B781
		internal string TargetName
		{
			get
			{
				return this.targetname;
			}
		}

		// Token: 0x170007C5 RID: 1989
		// (get) Token: 0x06001E18 RID: 7704 RVA: 0x0008D589 File Offset: 0x0008B789
		internal bool EnableSsl
		{
			get
			{
				return this.enableSsl;
			}
		}

		// Token: 0x04001C9F RID: 7327
		private string targetname;

		// Token: 0x04001CA0 RID: 7328
		private string host;

		// Token: 0x04001CA1 RID: 7329
		private string clientDomain;

		// Token: 0x04001CA2 RID: 7330
		private int port;

		// Token: 0x04001CA3 RID: 7331
		private NetworkCredential credential;

		// Token: 0x04001CA4 RID: 7332
		private bool enableSsl;
	}
}
