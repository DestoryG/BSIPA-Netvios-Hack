using System;
using System.Xml;

namespace System.Configuration
{
	// Token: 0x0200008F RID: 143
	public interface IConfigurationSectionHandler
	{
		// Token: 0x0600056A RID: 1386
		object Create(object parent, object configContext, XmlNode section);
	}
}
