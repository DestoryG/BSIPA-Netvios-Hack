using System;
using System.Collections;
using System.Xml;

namespace System.Configuration
{
	// Token: 0x020000B3 RID: 179
	public class SingleTagSectionHandler : IConfigurationSectionHandler
	{
		// Token: 0x06000615 RID: 1557 RVA: 0x00023D70 File Offset: 0x00021F70
		public virtual object Create(object parent, object context, XmlNode section)
		{
			Hashtable hashtable;
			if (parent == null)
			{
				hashtable = new Hashtable();
			}
			else
			{
				hashtable = new Hashtable((IDictionary)parent);
			}
			HandlerBase.CheckForChildNodes(section);
			foreach (object obj in section.Attributes)
			{
				XmlAttribute xmlAttribute = (XmlAttribute)obj;
				hashtable[xmlAttribute.Name] = xmlAttribute.Value;
			}
			return hashtable;
		}
	}
}
