using System;
using System.Collections;
using System.Security.Authentication.ExtendedProtection;
using System.Security.Permissions;

namespace System.Net.Mail
{
	// Token: 0x0200028F RID: 655
	internal class SmtpNtlmAuthenticationModule : ISmtpAuthenticationModule
	{
		// Token: 0x06001872 RID: 6258 RVA: 0x0007C490 File Offset: 0x0007A690
		internal SmtpNtlmAuthenticationModule()
		{
		}

		// Token: 0x06001873 RID: 6259 RVA: 0x0007C4A4 File Offset: 0x0007A6A4
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
					NTAuthentication ntauthentication = this.sessions[sessionCookie] as NTAuthentication;
					if (ntauthentication == null)
					{
						if (credential == null)
						{
							return null;
						}
						ntauthentication = (this.sessions[sessionCookie] = new NTAuthentication(false, "Ntlm", credential, spn, ContextFlags.Connection, channelBindingToken));
					}
					string outgoingBlob = ntauthentication.GetOutgoingBlob(challenge);
					if (!ntauthentication.IsCompleted)
					{
						authorization = new Authorization(outgoingBlob, false);
					}
					else
					{
						this.sessions.Remove(sessionCookie);
						authorization = new Authorization(outgoingBlob, true);
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

		// Token: 0x1700050F RID: 1295
		// (get) Token: 0x06001874 RID: 6260 RVA: 0x0007C590 File Offset: 0x0007A790
		public string AuthenticationType
		{
			get
			{
				return "ntlm";
			}
		}

		// Token: 0x06001875 RID: 6261 RVA: 0x0007C597 File Offset: 0x0007A797
		public void CloseContext(object sessionCookie)
		{
		}

		// Token: 0x04001854 RID: 6228
		private Hashtable sessions = new Hashtable();
	}
}
