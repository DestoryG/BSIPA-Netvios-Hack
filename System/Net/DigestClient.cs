using System;
using System.Globalization;
using System.Security.Authentication.ExtendedProtection;

namespace System.Net
{
	// Token: 0x020001AB RID: 427
	internal class DigestClient : ISessionAuthenticationModule, IAuthenticationModule
	{
		// Token: 0x060010C9 RID: 4297 RVA: 0x0005A0B1 File Offset: 0x000582B1
		public Authorization Authenticate(string challenge, WebRequest webRequest, ICredentials credentials)
		{
			return this.DoAuthenticate(challenge, webRequest, credentials, false);
		}

		// Token: 0x060010CA RID: 4298 RVA: 0x0005A0C0 File Offset: 0x000582C0
		private Authorization DoAuthenticate(string challenge, WebRequest webRequest, ICredentials credentials, bool preAuthenticate)
		{
			if (credentials == null)
			{
				return null;
			}
			HttpWebRequest httpWebRequest = webRequest as HttpWebRequest;
			NetworkCredential credential = credentials.GetCredential(httpWebRequest.ChallengedUri, DigestClient.Signature);
			if (credential is SystemNetworkCredential)
			{
				if (DigestClient.WDigestAvailable)
				{
					return this.XPDoAuthenticate(challenge, httpWebRequest, credentials, preAuthenticate);
				}
				return null;
			}
			else
			{
				HttpDigestChallenge httpDigestChallenge;
				if (!preAuthenticate)
				{
					int num = AuthenticationManager.FindSubstringNotInQuotes(challenge, DigestClient.Signature);
					if (num < 0)
					{
						return null;
					}
					httpDigestChallenge = HttpDigest.Interpret(challenge, num, httpWebRequest);
				}
				else
				{
					httpDigestChallenge = DigestClient.challengeCache.Lookup(httpWebRequest.ChallengedUri.AbsoluteUri) as HttpDigestChallenge;
				}
				if (httpDigestChallenge == null)
				{
					return null;
				}
				if (!DigestClient.CheckQOP(httpDigestChallenge))
				{
					if (Logging.On)
					{
						Logging.PrintError(Logging.Web, SR.GetString("net_log_digest_qop_not_supported", new object[] { httpDigestChallenge.QualityOfProtection }));
					}
					return null;
				}
				if (preAuthenticate)
				{
					httpDigestChallenge = httpDigestChallenge.CopyAndIncrementNonce();
					httpDigestChallenge.SetFromRequest(httpWebRequest);
				}
				if (credential == null)
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
				Authorization authorization = HttpDigest.Authenticate(httpDigestChallenge, credential, computeSpn.Spn, channelBinding);
				if (!preAuthenticate && webRequest.PreAuthenticate && authorization != null)
				{
					string[] array;
					if (httpDigestChallenge.Domain != null)
					{
						array = httpDigestChallenge.Domain.Split(DigestClient.singleSpaceArray);
					}
					else
					{
						(array = new string[1])[0] = httpWebRequest.ChallengedUri.GetParts(UriComponents.SchemeAndServer, UriFormat.UriEscaped);
					}
					string[] array2 = array;
					authorization.ProtectionRealm = ((httpDigestChallenge.Domain == null) ? null : array2);
					for (int i = 0; i < array2.Length; i++)
					{
						DigestClient.challengeCache.Add(array2[i], httpDigestChallenge);
					}
				}
				return authorization;
			}
		}

		// Token: 0x060010CB RID: 4299 RVA: 0x0005A278 File Offset: 0x00058478
		public Authorization PreAuthenticate(WebRequest webRequest, ICredentials credentials)
		{
			return this.DoAuthenticate(null, webRequest, credentials, true);
		}

		// Token: 0x170003A8 RID: 936
		// (get) Token: 0x060010CC RID: 4300 RVA: 0x0005A284 File Offset: 0x00058484
		public bool CanPreAuthenticate
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170003A9 RID: 937
		// (get) Token: 0x060010CD RID: 4301 RVA: 0x0005A287 File Offset: 0x00058487
		public string AuthenticationType
		{
			get
			{
				return "Digest";
			}
		}

		// Token: 0x060010CE RID: 4302 RVA: 0x0005A290 File Offset: 0x00058490
		internal static bool CheckQOP(HttpDigestChallenge challenge)
		{
			if (challenge.QopPresent)
			{
				for (int i = 0; i >= 0; i += "auth".Length)
				{
					i = challenge.QualityOfProtection.IndexOf("auth", i);
					if (i < 0)
					{
						return false;
					}
					if ((i == 0 || ", \"'\t\r\n".IndexOf(challenge.QualityOfProtection[i - 1]) >= 0) && (i + "auth".Length == challenge.QualityOfProtection.Length || ", \"'\t\r\n".IndexOf(challenge.QualityOfProtection[i + "auth".Length]) >= 0))
					{
						break;
					}
				}
			}
			return true;
		}

		// Token: 0x060010CF RID: 4303 RVA: 0x0005A338 File Offset: 0x00058538
		public bool Update(string challenge, WebRequest webRequest)
		{
			HttpWebRequest httpWebRequest = webRequest as HttpWebRequest;
			if (httpWebRequest.CurrentAuthenticationState.GetSecurityContext(this) != null)
			{
				return this.XPUpdate(challenge, httpWebRequest);
			}
			if (httpWebRequest.ResponseStatusCode != httpWebRequest.CurrentAuthenticationState.StatusCodeMatch)
			{
				ChannelBinding channelBinding = null;
				if (httpWebRequest.CurrentAuthenticationState.TransportContext != null)
				{
					channelBinding = httpWebRequest.CurrentAuthenticationState.TransportContext.GetChannelBinding(ChannelBindingKind.Endpoint);
				}
				httpWebRequest.ServicePoint.SetCachedChannelBinding(httpWebRequest.ChallengedUri, channelBinding);
				return true;
			}
			int num = ((challenge == null) ? (-1) : AuthenticationManager.FindSubstringNotInQuotes(challenge, DigestClient.Signature));
			if (num < 0)
			{
				return true;
			}
			int num2 = num + DigestClient.SignatureSize;
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
				string text = challenge.Substring(num2);
			}
			HttpDigestChallenge httpDigestChallenge = HttpDigest.Interpret(challenge, num, httpWebRequest);
			return httpDigestChallenge == null || !httpDigestChallenge.Stale;
		}

		// Token: 0x170003AA RID: 938
		// (get) Token: 0x060010D0 RID: 4304 RVA: 0x0005A41C File Offset: 0x0005861C
		public bool CanUseDefaultCredentials
		{
			get
			{
				return DigestClient.WDigestAvailable;
			}
		}

		// Token: 0x170003AB RID: 939
		// (get) Token: 0x060010D1 RID: 4305 RVA: 0x0005A423 File Offset: 0x00058623
		internal static bool WDigestAvailable
		{
			get
			{
				return DigestClient._WDigestAvailable;
			}
		}

		// Token: 0x060010D2 RID: 4306 RVA: 0x0005A42C File Offset: 0x0005862C
		public void ClearSession(WebRequest webRequest)
		{
			HttpWebRequest httpWebRequest = webRequest as HttpWebRequest;
			httpWebRequest.CurrentAuthenticationState.ClearSession();
		}

		// Token: 0x060010D3 RID: 4307 RVA: 0x0005A44C File Offset: 0x0005864C
		private Authorization XPDoAuthenticate(string challenge, HttpWebRequest httpWebRequest, ICredentials credentials, bool preAuthenticate)
		{
			NTAuthentication ntauthentication = null;
			string text;
			if (!preAuthenticate)
			{
				int num = AuthenticationManager.FindSubstringNotInQuotes(challenge, DigestClient.Signature);
				if (num < 0)
				{
					return null;
				}
				ntauthentication = httpWebRequest.CurrentAuthenticationState.GetSecurityContext(this);
				text = DigestClient.RefineDigestChallenge(challenge, num);
			}
			else
			{
				HttpDigestChallenge httpDigestChallenge = DigestClient.challengeCache.Lookup(httpWebRequest.ChallengedUri.AbsoluteUri) as HttpDigestChallenge;
				if (httpDigestChallenge == null)
				{
					return null;
				}
				httpDigestChallenge = httpDigestChallenge.CopyAndIncrementNonce();
				httpDigestChallenge.SetFromRequest(httpWebRequest);
				text = httpDigestChallenge.ToBlob();
			}
			Uri uri = httpWebRequest.GetRemoteResourceUri();
			UriComponents uriComponents;
			if (httpWebRequest.CurrentMethod.ConnectRequest)
			{
				uriComponents = UriComponents.HostAndPort;
				uri = httpWebRequest.RequestUri;
			}
			else
			{
				uriComponents = UriComponents.PathAndQuery;
			}
			string parts = uri.GetParts(uriComponents, UriFormat.UriEscaped);
			if (ntauthentication == null)
			{
				NetworkCredential credential = credentials.GetCredential(httpWebRequest.ChallengedUri, DigestClient.Signature);
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
				ntauthentication = new NTAuthentication("WDigest", credential, computeSpn, httpWebRequest, channelBinding);
				httpWebRequest.CurrentAuthenticationState.SetSecurityContext(ntauthentication, this);
			}
			SecurityStatus securityStatus;
			string outgoingDigestBlob = ntauthentication.GetOutgoingDigestBlob(text, httpWebRequest.CurrentMethod.Name, parts, null, false, false, out securityStatus);
			if (outgoingDigestBlob == null)
			{
				return null;
			}
			Authorization authorization = new Authorization("Digest " + outgoingDigestBlob, ntauthentication.IsCompleted, string.Empty, ntauthentication.IsMutualAuthFlag);
			if (!preAuthenticate && httpWebRequest.PreAuthenticate)
			{
				HttpDigestChallenge httpDigestChallenge2 = HttpDigest.Interpret(text, -1, httpWebRequest);
				string[] array;
				if (httpDigestChallenge2.Domain != null)
				{
					array = httpDigestChallenge2.Domain.Split(DigestClient.singleSpaceArray);
				}
				else
				{
					(array = new string[1])[0] = httpWebRequest.ChallengedUri.GetParts(UriComponents.SchemeAndServer, UriFormat.UriEscaped);
				}
				string[] array2 = array;
				authorization.ProtectionRealm = ((httpDigestChallenge2.Domain == null) ? null : array2);
				for (int i = 0; i < array2.Length; i++)
				{
					DigestClient.challengeCache.Add(array2[i], httpDigestChallenge2);
				}
			}
			return authorization;
		}

		// Token: 0x060010D4 RID: 4308 RVA: 0x0005A670 File Offset: 0x00058870
		private bool XPUpdate(string challenge, HttpWebRequest httpWebRequest)
		{
			NTAuthentication securityContext = httpWebRequest.CurrentAuthenticationState.GetSecurityContext(this);
			if (securityContext == null)
			{
				return false;
			}
			int num = ((challenge == null) ? (-1) : AuthenticationManager.FindSubstringNotInQuotes(challenge, DigestClient.Signature));
			if (num < 0)
			{
				httpWebRequest.ServicePoint.SetCachedChannelBinding(httpWebRequest.ChallengedUri, securityContext.ChannelBinding);
				this.ClearSession(httpWebRequest);
				return true;
			}
			if (httpWebRequest.ResponseStatusCode != httpWebRequest.CurrentAuthenticationState.StatusCodeMatch)
			{
				httpWebRequest.ServicePoint.SetCachedChannelBinding(httpWebRequest.ChallengedUri, securityContext.ChannelBinding);
				this.ClearSession(httpWebRequest);
				return true;
			}
			string text = DigestClient.RefineDigestChallenge(challenge, num);
			SecurityStatus securityStatus;
			string outgoingDigestBlob = securityContext.GetOutgoingDigestBlob(text, httpWebRequest.CurrentMethod.Name, null, null, false, true, out securityStatus);
			httpWebRequest.CurrentAuthenticationState.Authorization.MutuallyAuthenticated = securityContext.IsMutualAuthFlag;
			return securityContext.IsCompleted;
		}

		// Token: 0x060010D5 RID: 4309 RVA: 0x0005A738 File Offset: 0x00058938
		private static string RefineDigestChallenge(string challenge, int index)
		{
			int num = index + DigestClient.SignatureSize;
			if (challenge.Length > num && challenge[num] != ',')
			{
				num++;
			}
			else
			{
				index = -1;
			}
			if (index >= 0 && challenge.Length > num)
			{
				string text = challenge.Substring(num);
				int num2 = 0;
				int num3 = num2;
				bool flag = true;
				HttpDigestChallenge httpDigestChallenge = new HttpDigestChallenge();
				int num4;
				for (;;)
				{
					num4 = num3;
					index = AuthenticationManager.SplitNoQuotes(text, ref num4);
					if (num4 < 0)
					{
						break;
					}
					string text2 = text.Substring(num3, num4 - num3);
					string text3;
					if (index < 0)
					{
						text3 = HttpDigest.unquote(text.Substring(num4 + 1));
					}
					else
					{
						text3 = HttpDigest.unquote(text.Substring(num4 + 1, index - num4 - 1));
					}
					flag = httpDigestChallenge.defineAttribute(text2, text3);
					if (index < 0 || !flag)
					{
						break;
					}
					index = (num3 = index + 1);
				}
				if ((!flag || num4 < 0) && num3 < text.Length)
				{
					text = ((num3 > 0) ? text.Substring(0, num3 - 1) : "");
				}
				return text;
			}
			Logging.PrintError(Logging.Web, SR.GetString("net_log_auth_invalid_challenge", new object[] { "Digest" }));
			return string.Empty;
		}

		// Token: 0x040013A6 RID: 5030
		internal const string AuthType = "Digest";

		// Token: 0x040013A7 RID: 5031
		internal static string Signature = "Digest".ToLower(CultureInfo.InvariantCulture);

		// Token: 0x040013A8 RID: 5032
		internal static int SignatureSize = DigestClient.Signature.Length;

		// Token: 0x040013A9 RID: 5033
		private static PrefixLookup challengeCache = new PrefixLookup();

		// Token: 0x040013AA RID: 5034
		private static readonly char[] singleSpaceArray = new char[] { ' ' };

		// Token: 0x040013AB RID: 5035
		private static bool _WDigestAvailable = SSPIWrapper.GetVerifyPackageInfo(GlobalSSPI.SSPIAuth, "WDigest") != null;
	}
}
