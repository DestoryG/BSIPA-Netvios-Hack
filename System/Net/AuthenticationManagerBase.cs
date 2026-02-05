using System;
using System.Collections;
using System.Collections.Specialized;

namespace System.Net
{
	// Token: 0x020000C1 RID: 193
	internal abstract class AuthenticationManagerBase : IAuthenticationManager
	{
		// Token: 0x17000117 RID: 279
		// (get) Token: 0x0600066F RID: 1647 RVA: 0x00024780 File Offset: 0x00022980
		// (set) Token: 0x06000670 RID: 1648 RVA: 0x00024789 File Offset: 0x00022989
		public ICredentialPolicy CredentialPolicy
		{
			get
			{
				return AuthenticationManagerBase.s_ICredentialPolicy;
			}
			set
			{
				AuthenticationManagerBase.s_ICredentialPolicy = value;
			}
		}

		// Token: 0x06000671 RID: 1649 RVA: 0x00024793 File Offset: 0x00022993
		public virtual void EnsureConfigLoaded()
		{
		}

		// Token: 0x17000118 RID: 280
		// (get) Token: 0x06000672 RID: 1650 RVA: 0x00024795 File Offset: 0x00022995
		public StringDictionary CustomTargetNameDictionary
		{
			get
			{
				return AuthenticationManagerBase.m_SpnDictionary;
			}
		}

		// Token: 0x17000119 RID: 281
		// (get) Token: 0x06000673 RID: 1651 RVA: 0x0002479C File Offset: 0x0002299C
		public SpnDictionary SpnDictionary
		{
			get
			{
				return AuthenticationManagerBase.m_SpnDictionary;
			}
		}

		// Token: 0x1700011A RID: 282
		// (get) Token: 0x06000674 RID: 1652 RVA: 0x000247A4 File Offset: 0x000229A4
		public bool OSSupportsExtendedProtection
		{
			get
			{
				if (AuthenticationManagerBase.s_OSSupportsExtendedProtection == TriState.Unspecified)
				{
					if (ComNetOS.IsWin7orLater)
					{
						AuthenticationManagerBase.s_OSSupportsExtendedProtection = TriState.True;
					}
					else if (this.SspSupportsExtendedProtection)
					{
						if (UnsafeNclNativeMethods.HttpApi.ExtendedProtectionSupported)
						{
							AuthenticationManagerBase.s_OSSupportsExtendedProtection = TriState.True;
						}
						else
						{
							AuthenticationManagerBase.s_OSSupportsExtendedProtection = TriState.False;
						}
					}
					else
					{
						AuthenticationManagerBase.s_OSSupportsExtendedProtection = TriState.False;
					}
				}
				return AuthenticationManagerBase.s_OSSupportsExtendedProtection == TriState.True;
			}
		}

		// Token: 0x1700011B RID: 283
		// (get) Token: 0x06000675 RID: 1653 RVA: 0x000247F8 File Offset: 0x000229F8
		public bool SspSupportsExtendedProtection
		{
			get
			{
				if (AuthenticationManagerBase.s_SspSupportsExtendedProtection == TriState.Unspecified)
				{
					if (ComNetOS.IsWin7orLater)
					{
						AuthenticationManagerBase.s_SspSupportsExtendedProtection = TriState.True;
					}
					else
					{
						ContextFlags contextFlags = ContextFlags.Connection | ContextFlags.AcceptIntegrity;
						NTAuthentication ntauthentication = new NTAuthentication(false, "NTLM", SystemNetworkCredential.defaultCredential, "http/localhost", contextFlags, null);
						try
						{
							NTAuthentication ntauthentication2 = new NTAuthentication(true, "NTLM", SystemNetworkCredential.defaultCredential, null, ContextFlags.Connection, null);
							try
							{
								byte[] array = null;
								while (!ntauthentication2.IsCompleted)
								{
									SecurityStatus securityStatus;
									array = ntauthentication.GetOutgoingBlob(array, true, out securityStatus);
									array = ntauthentication2.GetOutgoingBlob(array, true, out securityStatus);
								}
								if (ntauthentication2.OSSupportsExtendedProtection)
								{
									AuthenticationManagerBase.s_SspSupportsExtendedProtection = TriState.True;
								}
								else
								{
									if (Logging.On)
									{
										Logging.PrintWarning(Logging.Web, SR.GetString("net_ssp_dont_support_cbt"));
									}
									AuthenticationManagerBase.s_SspSupportsExtendedProtection = TriState.False;
								}
							}
							finally
							{
								ntauthentication2.CloseContext();
							}
						}
						finally
						{
							ntauthentication.CloseContext();
						}
					}
				}
				return AuthenticationManagerBase.s_SspSupportsExtendedProtection == TriState.True;
			}
		}

		// Token: 0x06000676 RID: 1654
		public abstract Authorization Authenticate(string challenge, WebRequest request, ICredentials credentials);

		// Token: 0x06000677 RID: 1655
		public abstract Authorization PreAuthenticate(WebRequest request, ICredentials credentials);

		// Token: 0x06000678 RID: 1656
		public abstract void Register(IAuthenticationModule authenticationModule);

		// Token: 0x06000679 RID: 1657
		public abstract void Unregister(IAuthenticationModule authenticationModule);

		// Token: 0x0600067A RID: 1658
		public abstract void Unregister(string authenticationScheme);

		// Token: 0x1700011C RID: 284
		// (get) Token: 0x0600067B RID: 1659
		public abstract IEnumerator RegisteredModules { get; }

		// Token: 0x0600067C RID: 1660
		public abstract void BindModule(Uri uri, Authorization response, IAuthenticationModule module);

		// Token: 0x0600067D RID: 1661 RVA: 0x000248E4 File Offset: 0x00022AE4
		protected static string generalize(Uri location)
		{
			string components = location.GetComponents(UriComponents.Scheme | UriComponents.UserInfo | UriComponents.Host | UriComponents.Port | UriComponents.Path, UriFormat.UriEscaped);
			int num = components.LastIndexOf('/');
			if (num < 0)
			{
				return components;
			}
			return components.Substring(0, num + 1);
		}

		// Token: 0x04000C77 RID: 3191
		private static volatile ICredentialPolicy s_ICredentialPolicy;

		// Token: 0x04000C78 RID: 3192
		private static SpnDictionary m_SpnDictionary = new SpnDictionary();

		// Token: 0x04000C79 RID: 3193
		private static TriState s_OSSupportsExtendedProtection = TriState.Unspecified;

		// Token: 0x04000C7A RID: 3194
		private static TriState s_SspSupportsExtendedProtection = TriState.Unspecified;
	}
}
