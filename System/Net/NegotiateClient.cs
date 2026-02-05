using System;
using System.Security.Authentication.ExtendedProtection;

namespace System.Net
{
	// Token: 0x020001C8 RID: 456
	internal class NegotiateClient : ISessionAuthenticationModule, IAuthenticationModule
	{
		// Token: 0x0600121D RID: 4637 RVA: 0x000609F8 File Offset: 0x0005EBF8
		public Authorization Authenticate(string challenge, WebRequest webRequest, ICredentials credentials)
		{
			return this.DoAuthenticate(challenge, webRequest, credentials, false);
		}

		// Token: 0x0600121E RID: 4638 RVA: 0x00060A04 File Offset: 0x0005EC04
		private Authorization DoAuthenticate(string challenge, WebRequest webRequest, ICredentials credentials, bool preAuthenticate)
		{
			if (credentials == null)
			{
				return null;
			}
			HttpWebRequest httpWebRequest = webRequest as HttpWebRequest;
			NTAuthentication ntauthentication = null;
			string text = null;
			bool flag = false;
			if (!preAuthenticate)
			{
				int num = NegotiateClient.GetSignatureIndex(challenge, out flag);
				if (num < 0)
				{
					return null;
				}
				int num2 = num + (flag ? "nego2".Length : "negotiate".Length);
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
				NetworkCredential credential = credentials.GetCredential(httpWebRequest.ChallengedUri, "negotiate");
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
				ntauthentication = new NTAuthentication("Negotiate", credential, computeSpn, httpWebRequest, channelBinding);
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
			httpWebRequest.NtlmKeepAlive = text == null && ntauthentication.IsValidContext && !ntauthentication.IsKerberos;
			return AuthenticationManager.GetGroupAuthorization(this, (flag ? "Nego2" : "Negotiate") + " " + outgoingBlob, ntauthentication.IsCompleted, ntauthentication, unsafeOrProxyAuthenticatedConnectionSharing, ntauthentication.IsKerberos);
		}

		// Token: 0x170003E2 RID: 994
		// (get) Token: 0x0600121F RID: 4639 RVA: 0x00060BE0 File Offset: 0x0005EDE0
		public bool CanPreAuthenticate
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06001220 RID: 4640 RVA: 0x00060BE3 File Offset: 0x0005EDE3
		public Authorization PreAuthenticate(WebRequest webRequest, ICredentials credentials)
		{
			return this.DoAuthenticate(null, webRequest, credentials, true);
		}

		// Token: 0x170003E3 RID: 995
		// (get) Token: 0x06001221 RID: 4641 RVA: 0x00060BEF File Offset: 0x0005EDEF
		public string AuthenticationType
		{
			get
			{
				return "Negotiate";
			}
		}

		// Token: 0x06001222 RID: 4642 RVA: 0x00060BF8 File Offset: 0x0005EDF8
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
			if (!httpWebRequest.UnsafeOrProxyAuthenticatedConnectionSharing)
			{
				httpWebRequest.ServicePoint.ReleaseConnectionGroup(httpWebRequest.GetConnectionGroupLine());
			}
			bool flag = true;
			int num = ((challenge == null) ? (-1) : NegotiateClient.GetSignatureIndex(challenge, out flag));
			if (num >= 0)
			{
				int num2 = num + (flag ? "nego2".Length : "negotiate".Length);
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

		// Token: 0x06001223 RID: 4643 RVA: 0x00060D04 File Offset: 0x0005EF04
		public void ClearSession(WebRequest webRequest)
		{
			HttpWebRequest httpWebRequest = webRequest as HttpWebRequest;
			httpWebRequest.CurrentAuthenticationState.ClearSession();
		}

		// Token: 0x170003E4 RID: 996
		// (get) Token: 0x06001224 RID: 4644 RVA: 0x00060D23 File Offset: 0x0005EF23
		public bool CanUseDefaultCredentials
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06001225 RID: 4645 RVA: 0x00060D28 File Offset: 0x0005EF28
		private static int GetSignatureIndex(string challenge, out bool useNego2)
		{
			useNego2 = true;
			int num = -1;
			if (ComNetOS.IsWin7orLater)
			{
				num = AuthenticationManager.FindSubstringNotInQuotes(challenge, "nego2");
			}
			if (num < 0)
			{
				useNego2 = false;
				num = AuthenticationManager.FindSubstringNotInQuotes(challenge, "negotiate");
			}
			return num;
		}

		// Token: 0x04001478 RID: 5240
		internal const string AuthType = "Negotiate";

		// Token: 0x04001479 RID: 5241
		private const string negotiateHeader = "Negotiate";

		// Token: 0x0400147A RID: 5242
		private const string negotiateSignature = "negotiate";

		// Token: 0x0400147B RID: 5243
		private const string nego2Header = "Nego2";

		// Token: 0x0400147C RID: 5244
		private const string nego2Signature = "nego2";
	}
}
