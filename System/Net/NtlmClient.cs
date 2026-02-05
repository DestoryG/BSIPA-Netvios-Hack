using System;
using System.Globalization;
using System.Security.Authentication.ExtendedProtection;

namespace System.Net
{
	// Token: 0x020001D1 RID: 465
	internal class NtlmClient : ISessionAuthenticationModule, IAuthenticationModule
	{
		// Token: 0x0600126C RID: 4716 RVA: 0x000626C9 File Offset: 0x000608C9
		public Authorization Authenticate(string challenge, WebRequest webRequest, ICredentials credentials)
		{
			return this.DoAuthenticate(challenge, webRequest, credentials, false);
		}

		// Token: 0x0600126D RID: 4717 RVA: 0x000626D8 File Offset: 0x000608D8
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
				int num = AuthenticationManager.FindSubstringNotInQuotes(challenge, NtlmClient.Signature);
				if (num < 0)
				{
					return null;
				}
				int num2 = num + NtlmClient.SignatureSize;
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
				NetworkCredential credential = credentials.GetCredential(httpWebRequest.ChallengedUri, NtlmClient.Signature);
				string empty = string.Empty;
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
				ntauthentication = new NTAuthentication("NTLM", credential, computeSpn, httpWebRequest, channelBinding);
				httpWebRequest.CurrentAuthenticationState.SetSecurityContext(ntauthentication, this);
			}
			string outgoingBlob = ntauthentication.GetOutgoingBlob(text);
			if (outgoingBlob == null)
			{
				return null;
			}
			bool unsafeOrProxyAuthenticatedConnectionSharing = httpWebRequest.UnsafeOrProxyAuthenticatedConnectionSharing;
			if (unsafeOrProxyAuthenticatedConnectionSharing)
			{
				httpWebRequest.LockConnection = true;
			}
			httpWebRequest.NtlmKeepAlive = text == null;
			return AuthenticationManager.GetGroupAuthorization(this, "NTLM " + outgoingBlob, ntauthentication.IsCompleted, ntauthentication, unsafeOrProxyAuthenticatedConnectionSharing, false);
		}

		// Token: 0x170003FA RID: 1018
		// (get) Token: 0x0600126E RID: 4718 RVA: 0x00062877 File Offset: 0x00060A77
		public bool CanPreAuthenticate
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600126F RID: 4719 RVA: 0x0006287A File Offset: 0x00060A7A
		public Authorization PreAuthenticate(WebRequest webRequest, ICredentials credentials)
		{
			return this.DoAuthenticate(null, webRequest, credentials, true);
		}

		// Token: 0x170003FB RID: 1019
		// (get) Token: 0x06001270 RID: 4720 RVA: 0x00062886 File Offset: 0x00060A86
		public string AuthenticationType
		{
			get
			{
				return "NTLM";
			}
		}

		// Token: 0x06001271 RID: 4721 RVA: 0x00062890 File Offset: 0x00060A90
		public bool Update(string challenge, WebRequest webRequest)
		{
			HttpWebRequest httpWebRequest = webRequest as HttpWebRequest;
			NTAuthentication securityContext = httpWebRequest.CurrentAuthenticationState.GetSecurityContext(this);
			if (securityContext == null)
			{
				return true;
			}
			if (!securityContext.IsCompleted && httpWebRequest.CurrentAuthenticationState.StatusCodeMatch == httpWebRequest.ResponseStatusCode)
			{
				return false;
			}
			this.ClearSession(httpWebRequest);
			if (!httpWebRequest.UnsafeOrProxyAuthenticatedConnectionSharing)
			{
				httpWebRequest.ServicePoint.ReleaseConnectionGroup(httpWebRequest.GetConnectionGroupLine());
			}
			httpWebRequest.ServicePoint.SetCachedChannelBinding(httpWebRequest.ChallengedUri, securityContext.ChannelBinding);
			return true;
		}

		// Token: 0x06001272 RID: 4722 RVA: 0x0006290C File Offset: 0x00060B0C
		public void ClearSession(WebRequest webRequest)
		{
			HttpWebRequest httpWebRequest = webRequest as HttpWebRequest;
			httpWebRequest.CurrentAuthenticationState.ClearSession();
		}

		// Token: 0x170003FC RID: 1020
		// (get) Token: 0x06001273 RID: 4723 RVA: 0x0006292B File Offset: 0x00060B2B
		public bool CanUseDefaultCredentials
		{
			get
			{
				return true;
			}
		}

		// Token: 0x040014CA RID: 5322
		internal const string AuthType = "NTLM";

		// Token: 0x040014CB RID: 5323
		internal static string Signature = "NTLM".ToLower(CultureInfo.InvariantCulture);

		// Token: 0x040014CC RID: 5324
		internal static int SignatureSize = NtlmClient.Signature.Length;
	}
}
