using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Configuration;
using System.Reflection;
using System.Security.Authentication.ExtendedProtection;

namespace System.Net
{
	// Token: 0x020000C3 RID: 195
	internal class AuthenticationManager2 : AuthenticationManagerBase
	{
		// Token: 0x0600068C RID: 1676 RVA: 0x00024E78 File Offset: 0x00023078
		public AuthenticationManager2()
		{
			this.moduleBinding = new PrefixLookup();
			this.InitializeModuleList();
		}

		// Token: 0x0600068D RID: 1677 RVA: 0x00024E91 File Offset: 0x00023091
		public AuthenticationManager2(int maxPrefixLookupEntries)
		{
			this.moduleBinding = new PrefixLookup(maxPrefixLookupEntries);
			this.InitializeModuleList();
		}

		// Token: 0x0600068E RID: 1678 RVA: 0x00024EAC File Offset: 0x000230AC
		public override Authorization Authenticate(string challenge, WebRequest request, ICredentials credentials)
		{
			if (request == null)
			{
				throw new ArgumentNullException("request");
			}
			if (credentials == null)
			{
				throw new ArgumentNullException("credentials");
			}
			if (challenge == null)
			{
				throw new ArgumentNullException("challenge");
			}
			Authorization authorization = null;
			HttpWebRequest httpWebRequest = request as HttpWebRequest;
			if (httpWebRequest != null && httpWebRequest.CurrentAuthenticationState.Module != null)
			{
				authorization = httpWebRequest.CurrentAuthenticationState.Module.Authenticate(challenge, request, credentials);
			}
			else
			{
				foreach (IAuthenticationModule authenticationModule in this.moduleList.Values)
				{
					if (httpWebRequest != null)
					{
						httpWebRequest.CurrentAuthenticationState.Module = authenticationModule;
					}
					authorization = authenticationModule.Authenticate(challenge, request, credentials);
					if (authorization != null)
					{
						break;
					}
				}
			}
			return authorization;
		}

		// Token: 0x0600068F RID: 1679 RVA: 0x00024F70 File Offset: 0x00023170
		public override Authorization PreAuthenticate(WebRequest request, ICredentials credentials)
		{
			if (request == null)
			{
				throw new ArgumentNullException("request");
			}
			if (credentials == null)
			{
				return null;
			}
			HttpWebRequest httpWebRequest = request as HttpWebRequest;
			if (httpWebRequest == null)
			{
				return null;
			}
			string text = this.moduleBinding.Lookup(httpWebRequest.ChallengedUri.AbsoluteUri) as string;
			if (text == null)
			{
				return null;
			}
			IAuthenticationModule authenticationModule;
			if (!this.moduleList.TryGetValue(text.ToUpperInvariant(), out authenticationModule))
			{
				return null;
			}
			if (httpWebRequest.ChallengedUri.Scheme == Uri.UriSchemeHttps)
			{
				object cachedChannelBinding = httpWebRequest.ServicePoint.CachedChannelBinding;
				ChannelBinding channelBinding = cachedChannelBinding as ChannelBinding;
				if (channelBinding != null)
				{
					httpWebRequest.CurrentAuthenticationState.TransportContext = new CachedTransportContext(channelBinding);
				}
			}
			Authorization authorization = authenticationModule.PreAuthenticate(request, credentials);
			if (authorization != null && !authorization.Complete && httpWebRequest != null)
			{
				httpWebRequest.CurrentAuthenticationState.Module = authenticationModule;
			}
			return authorization;
		}

		// Token: 0x06000690 RID: 1680 RVA: 0x0002503C File Offset: 0x0002323C
		public override void Register(IAuthenticationModule authenticationModule)
		{
			if (authenticationModule == null)
			{
				throw new ArgumentNullException("authenticationModule");
			}
			string text = authenticationModule.AuthenticationType.ToUpperInvariant();
			this.moduleList.AddOrUpdate(text, authenticationModule, (string key, IAuthenticationModule value) => authenticationModule);
		}

		// Token: 0x06000691 RID: 1681 RVA: 0x0002509C File Offset: 0x0002329C
		public override void Unregister(IAuthenticationModule authenticationModule)
		{
			if (authenticationModule == null)
			{
				throw new ArgumentNullException("authenticationModule");
			}
			string text = authenticationModule.AuthenticationType.ToUpperInvariant();
			this.UnregisterInternal(text);
		}

		// Token: 0x06000692 RID: 1682 RVA: 0x000250CC File Offset: 0x000232CC
		public override void Unregister(string authenticationScheme)
		{
			if (authenticationScheme == null)
			{
				throw new ArgumentNullException("authenticationScheme");
			}
			string text = authenticationScheme.ToUpperInvariant();
			this.UnregisterInternal(text);
		}

		// Token: 0x1700011F RID: 287
		// (get) Token: 0x06000693 RID: 1683 RVA: 0x000250F5 File Offset: 0x000232F5
		public override IEnumerator RegisteredModules
		{
			get
			{
				return this.moduleList.Values.GetEnumerator();
			}
		}

		// Token: 0x06000694 RID: 1684 RVA: 0x00025108 File Offset: 0x00023308
		public override void BindModule(Uri uri, Authorization response, IAuthenticationModule module)
		{
			if (response.ProtectionRealm != null)
			{
				string[] protectionRealm = response.ProtectionRealm;
				for (int i = 0; i < protectionRealm.Length; i++)
				{
					this.moduleBinding.Add(protectionRealm[i], module.AuthenticationType.ToUpperInvariant());
				}
				return;
			}
			string text = AuthenticationManagerBase.generalize(uri);
			this.moduleBinding.Add(text, module.AuthenticationType);
		}

		// Token: 0x06000695 RID: 1685 RVA: 0x00025168 File Offset: 0x00023368
		private void InitializeModuleList()
		{
			List<Type> authenticationModules = AuthenticationModulesSectionInternal.GetSection().AuthenticationModules;
			this.moduleList = new ConcurrentDictionary<string, IAuthenticationModule>();
			IAuthenticationModule moduleToRegister;
			Func<string, IAuthenticationModule, IAuthenticationModule> <>9__0;
			foreach (Type type in authenticationModules)
			{
				try
				{
					moduleToRegister = Activator.CreateInstance(type, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.CreateInstance, null, new object[0], CultureInfo.InvariantCulture) as IAuthenticationModule;
					if (moduleToRegister != null)
					{
						string text = moduleToRegister.AuthenticationType.ToUpperInvariant();
						ConcurrentDictionary<string, IAuthenticationModule> concurrentDictionary = this.moduleList;
						string text2 = text;
						IAuthenticationModule moduleToRegister2 = moduleToRegister;
						Func<string, IAuthenticationModule, IAuthenticationModule> func;
						if ((func = <>9__0) == null)
						{
							func = (<>9__0 = (string key, IAuthenticationModule value) => moduleToRegister);
						}
						concurrentDictionary.AddOrUpdate(text2, moduleToRegister2, func);
					}
				}
				catch (Exception ex)
				{
				}
			}
		}

		// Token: 0x06000696 RID: 1686 RVA: 0x00025254 File Offset: 0x00023454
		private void UnregisterInternal(string normalizedAuthenticationType)
		{
			IAuthenticationModule authenticationModule;
			if (!this.moduleList.TryRemove(normalizedAuthenticationType, out authenticationModule))
			{
				throw new InvalidOperationException(SR.GetString("net_authmodulenotregistered"));
			}
		}

		// Token: 0x04000C7D RID: 3197
		private PrefixLookup moduleBinding;

		// Token: 0x04000C7E RID: 3198
		private ConcurrentDictionary<string, IAuthenticationModule> moduleList;
	}
}
