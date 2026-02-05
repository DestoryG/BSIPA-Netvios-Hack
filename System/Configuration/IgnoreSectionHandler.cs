using System;
using System.Xml;

namespace System.Configuration
{
	// Token: 0x02000091 RID: 145
	public class IgnoreSectionHandler : IConfigurationSectionHandler
	{
		// Token: 0x0600056D RID: 1389 RVA: 0x00021D91 File Offset: 0x0001FF91
		public virtual object Create(object parent, object configContext, XmlNode section)
		{
			return null;
		}
	}
}
