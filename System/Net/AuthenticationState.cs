using System;
using System.Net.Security;

namespace System.Net
{
	// Token: 0x02000190 RID: 400
	internal class AuthenticationState
	{
		// Token: 0x06000F59 RID: 3929 RVA: 0x0004F6F2 File Offset: 0x0004D8F2
		internal NTAuthentication GetSecurityContext(IAuthenticationModule module)
		{
			if (module != this.Module)
			{
				return null;
			}
			return this.SecurityContext;
		}

		// Token: 0x06000F5A RID: 3930 RVA: 0x0004F705 File Offset: 0x0004D905
		internal void SetSecurityContext(NTAuthentication securityContext, IAuthenticationModule module)
		{
			this.SecurityContext = securityContext;
		}

		// Token: 0x17000367 RID: 871
		// (get) Token: 0x06000F5B RID: 3931 RVA: 0x0004F70E File Offset: 0x0004D90E
		// (set) Token: 0x06000F5C RID: 3932 RVA: 0x0004F716 File Offset: 0x0004D916
		internal TransportContext TransportContext
		{
			get
			{
				return this._TransportContext;
			}
			set
			{
				this._TransportContext = value;
			}
		}

		// Token: 0x17000368 RID: 872
		// (get) Token: 0x06000F5D RID: 3933 RVA: 0x0004F71F File Offset: 0x0004D91F
		internal HttpResponseHeader AuthenticateHeader
		{
			get
			{
				if (!this.IsProxyAuth)
				{
					return HttpResponseHeader.WwwAuthenticate;
				}
				return HttpResponseHeader.ProxyAuthenticate;
			}
		}

		// Token: 0x17000369 RID: 873
		// (get) Token: 0x06000F5E RID: 3934 RVA: 0x0004F72E File Offset: 0x0004D92E
		internal string AuthorizationHeader
		{
			get
			{
				if (!this.IsProxyAuth)
				{
					return "Authorization";
				}
				return "Proxy-Authorization";
			}
		}

		// Token: 0x1700036A RID: 874
		// (get) Token: 0x06000F5F RID: 3935 RVA: 0x0004F743 File Offset: 0x0004D943
		internal HttpStatusCode StatusCodeMatch
		{
			get
			{
				if (!this.IsProxyAuth)
				{
					return HttpStatusCode.Unauthorized;
				}
				return HttpStatusCode.ProxyAuthenticationRequired;
			}
		}

		// Token: 0x06000F60 RID: 3936 RVA: 0x0004F758 File Offset: 0x0004D958
		internal AuthenticationState(bool isProxyAuth)
		{
			this.IsProxyAuth = isProxyAuth;
		}

		// Token: 0x06000F61 RID: 3937 RVA: 0x0004F768 File Offset: 0x0004D968
		private void PrepareState(HttpWebRequest httpWebRequest)
		{
			Uri uri = (this.IsProxyAuth ? httpWebRequest.ServicePoint.InternalAddress : httpWebRequest.GetRemoteResourceUri());
			if (this.ChallengedUri != uri)
			{
				if (this.ChallengedUri == null || this.ChallengedUri.Scheme != uri.Scheme || this.ChallengedUri.Host != uri.Host || this.ChallengedUri.Port != uri.Port)
				{
					this.ChallengedSpn = null;
				}
				this.ChallengedUri = uri;
			}
			httpWebRequest.CurrentAuthenticationState = this;
		}

		// Token: 0x06000F62 RID: 3938 RVA: 0x0004F7F8 File Offset: 0x0004D9F8
		internal SpnToken GetComputeSpn(HttpWebRequest httpWebRequest)
		{
			if (this.ChallengedSpn != null)
			{
				return this.ChallengedSpn;
			}
			bool flag = true;
			string text = httpWebRequest.ChallengedUri.GetParts(UriComponents.Scheme | UriComponents.Host | UriComponents.Port | UriComponents.Path, UriFormat.SafeUnescaped);
			SpnToken spnToken = AuthenticationManager.SpnDictionary.InternalGet(text);
			if (spnToken == null || spnToken.Spn == null)
			{
				string text2;
				if (!this.IsProxyAuth && (httpWebRequest.ServicePoint.InternalProxyServicePoint || httpWebRequest.UseCustomHost))
				{
					text2 = httpWebRequest.ChallengedUri.Host;
					if (httpWebRequest.ChallengedUri.HostNameType == UriHostNameType.IPv6 || httpWebRequest.ChallengedUri.HostNameType == UriHostNameType.IPv4 || text2.IndexOf('.') != -1)
					{
						goto IL_00D1;
					}
					try
					{
						IPHostEntry iphostEntry;
						if (Dns.TryInternalResolve(text2, out iphostEntry))
						{
							text2 = iphostEntry.HostName;
							flag &= iphostEntry.isTrustedHost;
						}
						goto IL_00D1;
					}
					catch (Exception ex)
					{
						if (NclUtilities.IsFatal(ex))
						{
							throw;
						}
						goto IL_00D1;
					}
				}
				text2 = httpWebRequest.ServicePoint.Hostname;
				flag &= httpWebRequest.ServicePoint.IsTrustedHost;
				IL_00D1:
				string text3 = "HTTP/" + text2;
				text = httpWebRequest.ChallengedUri.GetParts(UriComponents.SchemeAndServer, UriFormat.SafeUnescaped) + "/";
				spnToken = new SpnToken(text3, flag);
				AuthenticationManager.SpnDictionary.InternalSet(text, spnToken);
			}
			this.ChallengedSpn = spnToken;
			return this.ChallengedSpn;
		}

		// Token: 0x06000F63 RID: 3939 RVA: 0x0004F930 File Offset: 0x0004DB30
		internal void PreAuthIfNeeded(HttpWebRequest httpWebRequest, ICredentials authInfo)
		{
			if (!this.TriedPreAuth)
			{
				this.TriedPreAuth = true;
				if (authInfo != null)
				{
					this.PrepareState(httpWebRequest);
					try
					{
						Authorization authorization = AuthenticationManager.PreAuthenticate(httpWebRequest, authInfo);
						if (authorization != null && authorization.Message != null)
						{
							this.UniqueGroupId = authorization.ConnectionGroupId;
							httpWebRequest.Headers.Set(this.AuthorizationHeader, authorization.Message);
						}
					}
					catch (Exception ex)
					{
						this.ClearSession(httpWebRequest);
					}
				}
			}
		}

		// Token: 0x06000F64 RID: 3940 RVA: 0x0004F9AC File Offset: 0x0004DBAC
		internal bool AttemptAuthenticate(HttpWebRequest httpWebRequest, ICredentials authInfo)
		{
			if (this.Authorization != null && this.Authorization.Complete)
			{
				if (this.IsProxyAuth)
				{
					this.ClearAuthReq(httpWebRequest);
				}
				return false;
			}
			if (authInfo == null)
			{
				return false;
			}
			string text = httpWebRequest.AuthHeader(this.AuthenticateHeader);
			if (text == null)
			{
				if (!this.IsProxyAuth && this.Authorization != null && httpWebRequest.ProxyAuthenticationState.Authorization != null)
				{
					httpWebRequest.Headers.Set(this.AuthorizationHeader, this.Authorization.Message);
				}
				return false;
			}
			this.PrepareState(httpWebRequest);
			try
			{
				this.Authorization = AuthenticationManager.Authenticate(text, httpWebRequest, authInfo);
			}
			catch (Exception ex)
			{
				this.Authorization = null;
				this.ClearSession(httpWebRequest);
				throw;
			}
			if (this.Authorization == null)
			{
				return false;
			}
			if (this.Authorization.Message == null)
			{
				this.Authorization = null;
				return false;
			}
			this.UniqueGroupId = this.Authorization.ConnectionGroupId;
			try
			{
				httpWebRequest.Headers.Set(this.AuthorizationHeader, this.Authorization.Message);
			}
			catch
			{
				this.Authorization = null;
				this.ClearSession(httpWebRequest);
				throw;
			}
			return true;
		}

		// Token: 0x06000F65 RID: 3941 RVA: 0x0004FAD4 File Offset: 0x0004DCD4
		internal void ClearAuthReq(HttpWebRequest httpWebRequest)
		{
			this.TriedPreAuth = false;
			this.Authorization = null;
			this.UniqueGroupId = null;
			httpWebRequest.Headers.Remove(this.AuthorizationHeader);
		}

		// Token: 0x06000F66 RID: 3942 RVA: 0x0004FAFC File Offset: 0x0004DCFC
		internal void Update(HttpWebRequest httpWebRequest)
		{
			if (this.Authorization != null)
			{
				this.PrepareState(httpWebRequest);
				ISessionAuthenticationModule sessionAuthenticationModule = this.Module as ISessionAuthenticationModule;
				if (sessionAuthenticationModule != null)
				{
					string text = httpWebRequest.AuthHeader(this.AuthenticateHeader);
					if (this.IsProxyAuth || httpWebRequest.ResponseStatusCode != HttpStatusCode.ProxyAuthenticationRequired)
					{
						bool flag = true;
						try
						{
							flag = sessionAuthenticationModule.Update(text, httpWebRequest);
						}
						catch (Exception ex)
						{
							this.ClearSession(httpWebRequest);
							if (httpWebRequest.AuthenticationLevel == AuthenticationLevel.MutualAuthRequired && (httpWebRequest.CurrentAuthenticationState == null || httpWebRequest.CurrentAuthenticationState.Authorization == null || !httpWebRequest.CurrentAuthenticationState.Authorization.MutuallyAuthenticated))
							{
								throw;
							}
						}
						this.Authorization.SetComplete(flag);
					}
				}
				if (httpWebRequest.PreAuthenticate && this.Module != null && this.Authorization.Complete && this.Module.CanPreAuthenticate && httpWebRequest.ResponseStatusCode != this.StatusCodeMatch)
				{
					AuthenticationManager.BindModule(this.ChallengedUri, this.Authorization, this.Module);
				}
			}
		}

		// Token: 0x06000F67 RID: 3943 RVA: 0x0004FC00 File Offset: 0x0004DE00
		internal void ClearSession()
		{
			if (this.SecurityContext != null)
			{
				this.SecurityContext.CloseContext();
				this.SecurityContext = null;
			}
		}

		// Token: 0x06000F68 RID: 3944 RVA: 0x0004FC1C File Offset: 0x0004DE1C
		internal void ClearSession(HttpWebRequest httpWebRequest)
		{
			this.PrepareState(httpWebRequest);
			ISessionAuthenticationModule sessionAuthenticationModule = this.Module as ISessionAuthenticationModule;
			this.Module = null;
			if (sessionAuthenticationModule != null)
			{
				try
				{
					sessionAuthenticationModule.ClearSession(httpWebRequest);
				}
				catch (Exception ex)
				{
					if (NclUtilities.IsFatal(ex))
					{
						throw;
					}
				}
			}
		}

		// Token: 0x040012A6 RID: 4774
		private bool TriedPreAuth;

		// Token: 0x040012A7 RID: 4775
		internal Authorization Authorization;

		// Token: 0x040012A8 RID: 4776
		internal IAuthenticationModule Module;

		// Token: 0x040012A9 RID: 4777
		internal string UniqueGroupId;

		// Token: 0x040012AA RID: 4778
		private bool IsProxyAuth;

		// Token: 0x040012AB RID: 4779
		internal Uri ChallengedUri;

		// Token: 0x040012AC RID: 4780
		private SpnToken ChallengedSpn;

		// Token: 0x040012AD RID: 4781
		private NTAuthentication SecurityContext;

		// Token: 0x040012AE RID: 4782
		private TransportContext _TransportContext;
	}
}
