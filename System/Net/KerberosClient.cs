using System;
using System.Globalization;
using System.Security.Authentication.ExtendedProtection;

namespace System.Net
{
	// Token: 0x020001BB RID: 443
	internal class KerberosClient : ISessionAuthenticationModule, IAuthenticationModule
	{
		// Token: 0x06001146 RID: 4422 RVA: 0x0005E291 File Offset: 0x0005C491
		public Authorization Authenticate(string challenge, WebRequest webRequest, ICredentials credentials)
		{
			return this.DoAuthenticate(challenge, webRequest, credentials, false);
		}

		// Token: 0x06001147 RID: 4423 RVA: 0x0005E2A0 File Offset: 0x0005C4A0
		private Authorization DoAuthenticate(string challenge, WebRequest webRequest, ICredentials credentials, bool preAuthenticate)
		{
			if (credentials == null)
			{
				return null;
			}
			HttpWebRequest httpWebRequest = webRequest as HttpWebRequest;
			NTAuthentication ntauthentication = null;
			string text = null;
			if (!preAuthenticate)
			{
				int num = AuthenticationManager.FindSubstringNotInQuotes(challenge, KerberosClient.Signature);
				if (num < 0)
				{
					return null;
				}
				int num2 = num + KerberosClient.SignatureSize;
				if (challenge.Length > num2 && challenge[num2] != ',')
				{
					num2++;
				}
				else
				{
					num = -1;
				}
				if (num >= 0 && challenge.Length > num2)
				{
					num = challenge.IndexOf(',', num2);
					if (num != -1)
					{
						text = challenge.Substring(num2, num - num2);
					}
					else
					{
						text = challenge.Substring(num2);
					}
				}
				ntauthentication = httpWebRequest.CurrentAuthenticationState.GetSecurityContext(this);
			}
			if (ntauthentication == null)
			{
				NetworkCredential credential = credentials.GetCredential(httpWebRequest.ChallengedUri, KerberosClient.Signature);
				if (credential == null || (!(credential is SystemNetworkCredential) && credential.InternalGetUserName().Length == 0))
				{
					return null;
				}
				ICredentialPolicy credentialPolicy = AuthenticationManager.CredentialPolicy;
				if (credentialPolicy != null && !credentialPolicy.ShouldSendCredential(httpWebRequest.ChallengedUri, httpWebRequest, credential, this))
				{
					return null;
				}
				SpnToken computeSpn = httpWebRequest.CurrentAuthenticationState.GetComputeSpn(httpWebRequest);
				ChannelBinding channelBinding = null;
				if (httpWebRequest.CurrentAuthenticationState.TransportContext != null)
				{
					channelBinding = httpWebRequest.CurrentAuthenticationState.TransportContext.GetChannelBinding(ChannelBindingKind.Endpoint);
				}
				ntauthentication = new NTAuthentication("Kerberos", credential, computeSpn, httpWebRequest, channelBinding);
				httpWebRequest.CurrentAuthenticationState.SetSecurityContext(ntauthentication, this);
			}
			string outgoingBlob = ntauthentication.GetOutgoingBlob(text);
			if (outgoingBlob == null)
			{
				return null;
			}
			return new Authorization("Kerberos " + outgoingBlob, ntauthentication.IsCompleted, string.Empty, ntauthentication.IsMutualAuthFlag);
		}

		// Token: 0x170003BE RID: 958
		// (get) Token: 0x06001148 RID: 4424 RVA: 0x0005E41E File Offset: 0x0005C61E
		public bool CanPreAuthenticate
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06001149 RID: 4425 RVA: 0x0005E421 File Offset: 0x0005C621
		public Authorization PreAuthenticate(WebRequest webRequest, ICredentials credentials)
		{
			return this.DoAuthenticate(null, webRequest, credentials, true);
		}

		// Token: 0x170003BF RID: 959
		// (get) Token: 0x0600114A RID: 4426 RVA: 0x0005E42D File Offset: 0x0005C62D
		public string AuthenticationType
		{
			get
			{
				return "Kerberos";
			}
		}

		// Token: 0x0600114B RID: 4427 RVA: 0x0005E434 File Offset: 0x0005C634
		public bool Update(string challenge, WebRequest webRequest)
		{
			HttpWebRequest httpWebRequest = webRequest as HttpWebRequest;
			NTAuthentication securityContext = httpWebRequest.CurrentAuthenticationState.GetSecurityContext(this);
			if (securityContext == null)
			{
				return true;
			}
			if (httpWebRequest.CurrentAuthenticationState.StatusCodeMatch == httpWebRequest.ResponseStatusCode)
			{
				return false;
			}
			int num = ((challenge == null) ? (-1) : AuthenticationManager.FindSubstringNotInQuotes(challenge, KerberosClient.Signature));
			if (num >= 0)
			{
				int num2 = num + KerberosClient.SignatureSize;
				string text = null;
				if (challenge.Length > num2 && challenge[num2] != ',')
				{
					num2++;
				}
				else
				{
					num = -1;
				}
				if (num >= 0 && challenge.Length > num2)
				{
					text = challenge.Substring(num2);
				}
				string outgoingBlob = securityContext.GetOutgoingBlob(text);
				httpWebRequest.CurrentAuthenticationState.Authorization.MutuallyAuthenticated = securityContext.IsMutualAuthFlag;
			}
			httpWebRequest.ServicePoint.SetCachedChannelBinding(httpWebRequest.ChallengedUri, securityContext.ChannelBinding);
			this.ClearSession(httpWebRequest);
			return true;
		}

		// Token: 0x0600114C RID: 4428 RVA: 0x0005E504 File Offset: 0x0005C704
		public void ClearSession(WebRequest webRequest)
		{
			HttpWebRequest httpWebRequest = webRequest as HttpWebRequest;
			httpWebRequest.CurrentAuthenticationState.ClearSession();
		}

		// Token: 0x170003C0 RID: 960
		// (get) Token: 0x0600114D RID: 4429 RVA: 0x0005E523 File Offset: 0x0005C723
		public bool CanUseDefaultCredentials
		{
			get
			{
				return true;
			}
		}

		// Token: 0x04001441 RID: 5185
		internal const string AuthType = "Kerberos";

		// Token: 0x04001442 RID: 5186
		internal static string Signature = "Kerberos".ToLower(CultureInfo.InvariantCulture);

		// Token: 0x04001443 RID: 5187
		internal static int SignatureSize = KerberosClient.Signature.Length;
	}
}
