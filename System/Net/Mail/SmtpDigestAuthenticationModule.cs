using System;
using System.Collections;
using System.Security.Authentication.ExtendedProtection;
using System.Security.Permissions;

namespace System.Net.Mail
{
	// Token: 0x02000289 RID: 649
	internal class SmtpDigestAuthenticationModule : ISmtpAuthenticationModule
	{
		// Token: 0x06001844 RID: 6212 RVA: 0x0007BACB File Offset: 0x00079CCB
		internal SmtpDigestAuthenticationModule()
		{
		}

		// Token: 0x06001845 RID: 6213 RVA: 0x0007BAE0 File Offset: 0x00079CE0
		[EnvironmentPermission(SecurityAction.Assert, Unrestricted = true)]
		[SecurityPermission(SecurityAction.Assert, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public Authorization Authenticate(string challenge, NetworkCredential credential, object sessionCookie, string spn, ChannelBinding channelBindingToken)
		{
			Hashtable hashtable = this.sessions;
			Authorization authorization;
			lock (hashtable)
			{
				NTAuthentication ntauthentication = this.sessions[sessionCookie] as NTAuthentication;
				if (ntauthentication == null)
				{
					if (credential == null)
					{
						return null;
					}
					ntauthentication = (this.sessions[sessionCookie] = new NTAuthentication(false, "WDigest", credential, spn, ContextFlags.Connection, channelBindingToken));
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
			return authorization;
		}

		// Token: 0x17000509 RID: 1289
		// (get) Token: 0x06001846 RID: 6214 RVA: 0x0007BB8C File Offset: 0x00079D8C
		public string AuthenticationType
		{
			get
			{
				return "WDigest";
			}
		}

		// Token: 0x06001847 RID: 6215 RVA: 0x0007BB93 File Offset: 0x00079D93
		public void CloseContext(object sessionCookie)
		{
		}

		// Token: 0x0400184D RID: 6221
		private Hashtable sessions = new Hashtable();
	}
}
