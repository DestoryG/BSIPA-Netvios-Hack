using System;
using System.Collections;
using System.ComponentModel;
using System.Security.Authentication.ExtendedProtection;
using System.Security.Permissions;

namespace System.Net.Mail
{
	// Token: 0x0200028E RID: 654
	internal class SmtpNegotiateAuthenticationModule : ISmtpAuthenticationModule
	{
		// Token: 0x0600186D RID: 6253 RVA: 0x0007C289 File Offset: 0x0007A489
		internal SmtpNegotiateAuthenticationModule()
		{
		}

		// Token: 0x0600186E RID: 6254 RVA: 0x0007C29C File Offset: 0x0007A49C
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
						ntauthentication = (this.sessions[sessionCookie] = new NTAuthentication(false, "Negotiate", credential, spn, ContextFlags.Connection | ContextFlags.AcceptStream, channelBindingToken));
					}
					string text = null;
					if (!ntauthentication.IsCompleted)
					{
						byte[] array = null;
						if (challenge != null)
						{
							array = Convert.FromBase64String(challenge);
						}
						SecurityStatus securityStatus;
						byte[] outgoingBlob = ntauthentication.GetOutgoingBlob(array, false, out securityStatus);
						if (outgoingBlob != null)
						{
							text = Convert.ToBase64String(outgoingBlob);
						}
					}
					else
					{
						text = this.GetSecurityLayerOutgoingBlob(challenge, ntauthentication);
					}
					authorization = new Authorization(text, ntauthentication.IsCompleted);
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

		// Token: 0x1700050E RID: 1294
		// (get) Token: 0x0600186F RID: 6255 RVA: 0x0007C3A4 File Offset: 0x0007A5A4
		public string AuthenticationType
		{
			get
			{
				return "gssapi";
			}
		}

		// Token: 0x06001870 RID: 6256 RVA: 0x0007C3AC File Offset: 0x0007A5AC
		public void CloseContext(object sessionCookie)
		{
			NTAuthentication ntauthentication = null;
			Hashtable hashtable = this.sessions;
			lock (hashtable)
			{
				ntauthentication = this.sessions[sessionCookie] as NTAuthentication;
				if (ntauthentication != null)
				{
					this.sessions.Remove(sessionCookie);
				}
			}
			if (ntauthentication != null)
			{
				ntauthentication.CloseContext();
			}
		}

		// Token: 0x06001871 RID: 6257 RVA: 0x0007C414 File Offset: 0x0007A614
		private string GetSecurityLayerOutgoingBlob(string challenge, NTAuthentication clientContext)
		{
			if (challenge == null)
			{
				return null;
			}
			byte[] array = Convert.FromBase64String(challenge);
			int num;
			try
			{
				num = clientContext.VerifySignature(array, 0, array.Length);
			}
			catch (Win32Exception)
			{
				return null;
			}
			if (num < 4 || (array[0] & 1) != 1)
			{
				return null;
			}
			array[0] = 1;
			byte[] array2 = null;
			try
			{
				num = clientContext.MakeSignature(array, 0, 4, ref array2);
			}
			catch (Win32Exception)
			{
				return null;
			}
			return Convert.ToBase64String(array2, 0, num);
		}

		// Token: 0x04001853 RID: 6227
		private Hashtable sessions = new Hashtable();
	}
}
