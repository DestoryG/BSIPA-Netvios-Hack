using System;
using System.Collections;
using System.Security.Authentication.ExtendedProtection;
using System.Security.Permissions;
using System.Text;

namespace System.Net.Mail
{
	// Token: 0x0200028D RID: 653
	internal class SmtpLoginAuthenticationModule : ISmtpAuthenticationModule
	{
		// Token: 0x06001869 RID: 6249 RVA: 0x0007C14B File Offset: 0x0007A34B
		internal SmtpLoginAuthenticationModule()
		{
		}

		// Token: 0x0600186A RID: 6250 RVA: 0x0007C160 File Offset: 0x0007A360
		[EnvironmentPermission(SecurityAction.Assert, Unrestricted = true)]
		[SecurityPermission(SecurityAction.Assert, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public Authorization Authenticate(string challenge, NetworkCredential credential, object sessionCookie, string spn, ChannelBinding channelBindingToken)
		{
			if (Logging.On)
			{
				Logging.Enter(Logging.Web, this, "Authenticate", null);
			}
			Authorization authorization;
			try
			{
				Hashtable hashtable = this.sessions;
				lock (hashtable)
				{
					NetworkCredential networkCredential = this.sessions[sessionCookie] as NetworkCredential;
					if (networkCredential == null)
					{
						if (credential == null || credential is SystemNetworkCredential)
						{
							authorization = null;
						}
						else
						{
							this.sessions[sessionCookie] = credential;
							string text = credential.UserName;
							string domain = credential.Domain;
							if (domain != null && domain.Length > 0)
							{
								text = domain + "\\" + text;
							}
							authorization = new Authorization(Convert.ToBase64String(Encoding.UTF8.GetBytes(text)), false);
						}
					}
					else
					{
						this.sessions.Remove(sessionCookie);
						authorization = new Authorization(Convert.ToBase64String(Encoding.UTF8.GetBytes(networkCredential.Password)), true);
					}
				}
			}
			finally
			{
				if (Logging.On)
				{
					Logging.Exit(Logging.Web, this, "Authenticate", null);
				}
			}
			return authorization;
		}

		// Token: 0x1700050D RID: 1293
		// (get) Token: 0x0600186B RID: 6251 RVA: 0x0007C280 File Offset: 0x0007A480
		public string AuthenticationType
		{
			get
			{
				return "login";
			}
		}

		// Token: 0x0600186C RID: 6252 RVA: 0x0007C287 File Offset: 0x0007A487
		public void CloseContext(object sessionCookie)
		{
		}

		// Token: 0x04001852 RID: 6226
		private Hashtable sessions = new Hashtable();
	}
}
