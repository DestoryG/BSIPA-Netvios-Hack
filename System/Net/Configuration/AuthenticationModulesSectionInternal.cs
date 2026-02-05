using System;
using System.Collections.Generic;
using System.Configuration;
using System.Threading;

namespace System.Net.Configuration
{
	// Token: 0x02000327 RID: 807
	internal sealed class AuthenticationModulesSectionInternal
	{
		// Token: 0x06001CEF RID: 7407 RVA: 0x0008A750 File Offset: 0x00088950
		internal AuthenticationModulesSectionInternal(AuthenticationModulesSection section)
		{
			if (section.AuthenticationModules.Count > 0)
			{
				this.authenticationModules = new List<Type>(section.AuthenticationModules.Count);
				foreach (object obj in section.AuthenticationModules)
				{
					AuthenticationModuleElement authenticationModuleElement = (AuthenticationModuleElement)obj;
					Type type = null;
					try
					{
						type = Type.GetType(authenticationModuleElement.Type, true, true);
						if (!typeof(IAuthenticationModule).IsAssignableFrom(type))
						{
							throw new InvalidCastException(SR.GetString("net_invalid_cast", new object[] { type.FullName, "IAuthenticationModule" }));
						}
					}
					catch (Exception ex)
					{
						if (NclUtilities.IsFatal(ex))
						{
							throw;
						}
						throw new ConfigurationErrorsException(SR.GetString("net_config_authenticationmodules"), ex);
					}
					this.authenticationModules.Add(type);
				}
			}
		}

		// Token: 0x17000721 RID: 1825
		// (get) Token: 0x06001CF0 RID: 7408 RVA: 0x0008A854 File Offset: 0x00088A54
		internal List<Type> AuthenticationModules
		{
			get
			{
				List<Type> list = this.authenticationModules;
				if (list == null)
				{
					list = new List<Type>(0);
				}
				return list;
			}
		}

		// Token: 0x17000722 RID: 1826
		// (get) Token: 0x06001CF1 RID: 7409 RVA: 0x0008A874 File Offset: 0x00088A74
		internal static object ClassSyncObject
		{
			get
			{
				if (AuthenticationModulesSectionInternal.classSyncObject == null)
				{
					object obj = new object();
					Interlocked.CompareExchange(ref AuthenticationModulesSectionInternal.classSyncObject, obj, null);
				}
				return AuthenticationModulesSectionInternal.classSyncObject;
			}
		}

		// Token: 0x06001CF2 RID: 7410 RVA: 0x0008A8A0 File Offset: 0x00088AA0
		internal static AuthenticationModulesSectionInternal GetSection()
		{
			object obj = AuthenticationModulesSectionInternal.ClassSyncObject;
			AuthenticationModulesSectionInternal authenticationModulesSectionInternal;
			lock (obj)
			{
				AuthenticationModulesSection authenticationModulesSection = PrivilegedConfigurationManager.GetSection(ConfigurationStrings.AuthenticationModulesSectionPath) as AuthenticationModulesSection;
				if (authenticationModulesSection == null)
				{
					authenticationModulesSectionInternal = null;
				}
				else
				{
					authenticationModulesSectionInternal = new AuthenticationModulesSectionInternal(authenticationModulesSection);
				}
			}
			return authenticationModulesSectionInternal;
		}

		// Token: 0x04001BBC RID: 7100
		private List<Type> authenticationModules;

		// Token: 0x04001BBD RID: 7101
		private static object classSyncObject;
	}
}
