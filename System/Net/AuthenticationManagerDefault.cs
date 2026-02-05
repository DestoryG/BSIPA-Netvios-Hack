using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Configuration;
using System.Reflection;
using System.Security.Authentication.ExtendedProtection;
using System.Threading;

namespace System.Net
{
	// Token: 0x020000C2 RID: 194
	internal class AuthenticationManagerDefault : AuthenticationManagerBase
	{
		// Token: 0x06000681 RID: 1665 RVA: 0x00024948 File Offset: 0x00022B48
		public override void EnsureConfigLoaded()
		{
			try
			{
				object obj = this.ModuleList;
			}
			catch (Exception ex)
			{
				if (ex is ThreadAbortException || ex is OutOfMemoryException || ex is StackOverflowException)
				{
					throw;
				}
			}
		}

		// Token: 0x1700011D RID: 285
		// (get) Token: 0x06000682 RID: 1666 RVA: 0x0002498C File Offset: 0x00022B8C
		private ArrayList ModuleList
		{
			get
			{
				if (this.moduleList == null)
				{
					PrefixLookup prefixLookup = this.moduleBinding;
					lock (prefixLookup)
					{
						if (this.moduleList == null)
						{
							List<Type> authenticationModules = AuthenticationModulesSectionInternal.GetSection().AuthenticationModules;
							ArrayList arrayList = new ArrayList();
							foreach (Type type in authenticationModules)
							{
								try
								{
									IAuthenticationModule authenticationModule = Activator.CreateInstance(type, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.CreateInstance, null, new object[0], CultureInfo.InvariantCulture) as IAuthenticationModule;
									if (authenticationModule != null)
									{
										AuthenticationManagerDefault.RemoveAuthenticationType(arrayList, authenticationModule.AuthenticationType);
										arrayList.Add(authenticationModule);
									}
								}
								catch (Exception ex)
								{
								}
							}
							this.moduleList = arrayList;
						}
					}
				}
				return this.moduleList;
			}
		}

		// Token: 0x06000683 RID: 1667 RVA: 0x00024A88 File Offset: 0x00022C88
		private static void RemoveAuthenticationType(ArrayList list, string typeToRemove)
		{
			for (int i = 0; i < list.Count; i++)
			{
				if (string.Compare(((IAuthenticationModule)list[i]).AuthenticationType, typeToRemove, StringComparison.OrdinalIgnoreCase) == 0)
				{
					list.RemoveAt(i);
					return;
				}
			}
		}

		// Token: 0x06000684 RID: 1668 RVA: 0x00024AC8 File Offset: 0x00022CC8
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
				PrefixLookup prefixLookup = this.moduleBinding;
				lock (prefixLookup)
				{
					for (int i = 0; i < this.ModuleList.Count; i++)
					{
						IAuthenticationModule authenticationModule = (IAuthenticationModule)this.ModuleList[i];
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
			}
			return authorization;
		}

		// Token: 0x06000685 RID: 1669 RVA: 0x00024BAC File Offset: 0x00022DAC
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
			IAuthenticationModule authenticationModule = this.findModule(text);
			if (authenticationModule == null)
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

		// Token: 0x06000686 RID: 1670 RVA: 0x00024C70 File Offset: 0x00022E70
		public override void Register(IAuthenticationModule authenticationModule)
		{
			if (authenticationModule == null)
			{
				throw new ArgumentNullException("authenticationModule");
			}
			PrefixLookup prefixLookup = this.moduleBinding;
			lock (prefixLookup)
			{
				IAuthenticationModule authenticationModule2 = this.findModule(authenticationModule.AuthenticationType);
				if (authenticationModule2 != null)
				{
					this.ModuleList.Remove(authenticationModule2);
				}
				this.ModuleList.Add(authenticationModule);
			}
		}

		// Token: 0x06000687 RID: 1671 RVA: 0x00024CE4 File Offset: 0x00022EE4
		public override void Unregister(IAuthenticationModule authenticationModule)
		{
			if (authenticationModule == null)
			{
				throw new ArgumentNullException("authenticationModule");
			}
			PrefixLookup prefixLookup = this.moduleBinding;
			lock (prefixLookup)
			{
				if (!this.ModuleList.Contains(authenticationModule))
				{
					throw new InvalidOperationException(SR.GetString("net_authmodulenotregistered"));
				}
				this.ModuleList.Remove(authenticationModule);
			}
		}

		// Token: 0x06000688 RID: 1672 RVA: 0x00024D58 File Offset: 0x00022F58
		public override void Unregister(string authenticationScheme)
		{
			if (authenticationScheme == null)
			{
				throw new ArgumentNullException("authenticationScheme");
			}
			PrefixLookup prefixLookup = this.moduleBinding;
			lock (prefixLookup)
			{
				IAuthenticationModule authenticationModule = this.findModule(authenticationScheme);
				if (authenticationModule == null)
				{
					throw new InvalidOperationException(SR.GetString("net_authschemenotregistered"));
				}
				this.ModuleList.Remove(authenticationModule);
			}
		}

		// Token: 0x1700011E RID: 286
		// (get) Token: 0x06000689 RID: 1673 RVA: 0x00024DC8 File Offset: 0x00022FC8
		public override IEnumerator RegisteredModules
		{
			get
			{
				return this.ModuleList.GetEnumerator();
			}
		}

		// Token: 0x0600068A RID: 1674 RVA: 0x00024DD8 File Offset: 0x00022FD8
		public override void BindModule(Uri uri, Authorization response, IAuthenticationModule module)
		{
			if (response.ProtectionRealm != null)
			{
				string[] protectionRealm = response.ProtectionRealm;
				for (int i = 0; i < protectionRealm.Length; i++)
				{
					this.moduleBinding.Add(protectionRealm[i], module.AuthenticationType);
				}
				return;
			}
			string text = AuthenticationManagerBase.generalize(uri);
			this.moduleBinding.Add(text, module.AuthenticationType);
		}

		// Token: 0x0600068B RID: 1675 RVA: 0x00024E30 File Offset: 0x00023030
		private IAuthenticationModule findModule(string authenticationType)
		{
			IAuthenticationModule authenticationModule = null;
			ArrayList arrayList = this.ModuleList;
			for (int i = 0; i < arrayList.Count; i++)
			{
				IAuthenticationModule authenticationModule2 = (IAuthenticationModule)arrayList[i];
				if (string.Compare(authenticationModule2.AuthenticationType, authenticationType, StringComparison.OrdinalIgnoreCase) == 0)
				{
					authenticationModule = authenticationModule2;
					break;
				}
			}
			return authenticationModule;
		}

		// Token: 0x04000C7B RID: 3195
		private PrefixLookup moduleBinding = new PrefixLookup();

		// Token: 0x04000C7C RID: 3196
		private volatile ArrayList moduleList;
	}
}
