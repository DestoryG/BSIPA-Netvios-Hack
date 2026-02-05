using System;
using System.Collections;
using System.Configuration;
using System.Threading;

namespace System.Net.Configuration
{
	// Token: 0x0200034D RID: 845
	internal sealed class WebRequestModulesSectionInternal
	{
		// Token: 0x06001E4A RID: 7754 RVA: 0x0008DC24 File Offset: 0x0008BE24
		internal WebRequestModulesSectionInternal(WebRequestModulesSection section)
		{
			if (section.WebRequestModules.Count > 0)
			{
				this.webRequestModules = new ArrayList(section.WebRequestModules.Count);
				foreach (object obj in section.WebRequestModules)
				{
					WebRequestModuleElement webRequestModuleElement = (WebRequestModuleElement)obj;
					try
					{
						this.webRequestModules.Add(new WebRequestPrefixElement(webRequestModuleElement.Prefix, webRequestModuleElement.Type));
					}
					catch (Exception ex)
					{
						if (NclUtilities.IsFatal(ex))
						{
							throw;
						}
						throw new ConfigurationErrorsException(SR.GetString("net_config_webrequestmodules"), ex);
					}
				}
			}
		}

		// Token: 0x170007D8 RID: 2008
		// (get) Token: 0x06001E4B RID: 7755 RVA: 0x0008DCEC File Offset: 0x0008BEEC
		internal static object ClassSyncObject
		{
			get
			{
				if (WebRequestModulesSectionInternal.classSyncObject == null)
				{
					object obj = new object();
					Interlocked.CompareExchange(ref WebRequestModulesSectionInternal.classSyncObject, obj, null);
				}
				return WebRequestModulesSectionInternal.classSyncObject;
			}
		}

		// Token: 0x06001E4C RID: 7756 RVA: 0x0008DD18 File Offset: 0x0008BF18
		internal static WebRequestModulesSectionInternal GetSection()
		{
			object obj = WebRequestModulesSectionInternal.ClassSyncObject;
			WebRequestModulesSectionInternal webRequestModulesSectionInternal;
			lock (obj)
			{
				WebRequestModulesSection webRequestModulesSection = PrivilegedConfigurationManager.GetSection(ConfigurationStrings.WebRequestModulesSectionPath) as WebRequestModulesSection;
				if (webRequestModulesSection == null)
				{
					webRequestModulesSectionInternal = null;
				}
				else
				{
					webRequestModulesSectionInternal = new WebRequestModulesSectionInternal(webRequestModulesSection);
				}
			}
			return webRequestModulesSectionInternal;
		}

		// Token: 0x170007D9 RID: 2009
		// (get) Token: 0x06001E4D RID: 7757 RVA: 0x0008DD70 File Offset: 0x0008BF70
		internal ArrayList WebRequestModules
		{
			get
			{
				ArrayList arrayList = this.webRequestModules;
				if (arrayList == null)
				{
					arrayList = new ArrayList(0);
				}
				return arrayList;
			}
		}

		// Token: 0x04001CB4 RID: 7348
		private static object classSyncObject;

		// Token: 0x04001CB5 RID: 7349
		private ArrayList webRequestModules;
	}
}
