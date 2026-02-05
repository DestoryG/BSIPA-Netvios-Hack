using System;
using System.Xml;

namespace System.Configuration
{
	// Token: 0x02000096 RID: 150
	public class NameValueSectionHandler : IConfigurationSectionHandler
	{
		// Token: 0x0600058C RID: 1420 RVA: 0x00022878 File Offset: 0x00020A78
		public object Create(object parent, object context, XmlNode section)
		{
			return NameValueSectionHandler.CreateStatic(parent, section, this.KeyAttributeName, this.ValueAttributeName);
		}

		// Token: 0x0600058D RID: 1421 RVA: 0x0002288D File Offset: 0x00020A8D
		internal static object CreateStatic(object parent, XmlNode section)
		{
			return NameValueSectionHandler.CreateStatic(parent, section, "key", "value");
		}

		// Token: 0x0600058E RID: 1422 RVA: 0x000228A0 File Offset: 0x00020AA0
		internal static object CreateStatic(object parent, XmlNode section, string keyAttriuteName, string valueAttributeName)
		{
			ReadOnlyNameValueCollection readOnlyNameValueCollection;
			if (parent == null)
			{
				readOnlyNameValueCollection = new ReadOnlyNameValueCollection(StringComparer.OrdinalIgnoreCase);
			}
			else
			{
				ReadOnlyNameValueCollection readOnlyNameValueCollection2 = (ReadOnlyNameValueCollection)parent;
				readOnlyNameValueCollection = new ReadOnlyNameValueCollection(readOnlyNameValueCollection2);
			}
			HandlerBase.CheckForUnrecognizedAttributes(section);
			foreach (object obj in section.ChildNodes)
			{
				XmlNode xmlNode = (XmlNode)obj;
				if (!HandlerBase.IsIgnorableAlsoCheckForNonElement(xmlNode))
				{
					if (xmlNode.Name == "add")
					{
						string text = HandlerBase.RemoveRequiredAttribute(xmlNode, keyAttriuteName);
						string text2 = HandlerBase.RemoveRequiredAttribute(xmlNode, valueAttributeName, true);
						HandlerBase.CheckForUnrecognizedAttributes(xmlNode);
						readOnlyNameValueCollection[text] = text2;
					}
					else if (xmlNode.Name == "remove")
					{
						string text3 = HandlerBase.RemoveRequiredAttribute(xmlNode, keyAttriuteName);
						HandlerBase.CheckForUnrecognizedAttributes(xmlNode);
						readOnlyNameValueCollection.Remove(text3);
					}
					else if (xmlNode.Name.Equals("clear"))
					{
						HandlerBase.CheckForUnrecognizedAttributes(xmlNode);
						readOnlyNameValueCollection.Clear();
					}
					else
					{
						HandlerBase.ThrowUnrecognizedElement(xmlNode);
					}
				}
			}
			readOnlyNameValueCollection.SetReadOnly();
			return readOnlyNameValueCollection;
		}

		// Token: 0x170000D4 RID: 212
		// (get) Token: 0x0600058F RID: 1423 RVA: 0x000229BC File Offset: 0x00020BBC
		protected virtual string KeyAttributeName
		{
			get
			{
				return "key";
			}
		}

		// Token: 0x170000D5 RID: 213
		// (get) Token: 0x06000590 RID: 1424 RVA: 0x000229C3 File Offset: 0x00020BC3
		protected virtual string ValueAttributeName
		{
			get
			{
				return "value";
			}
		}

		// Token: 0x04000C35 RID: 3125
		private const string defaultKeyAttribute = "key";

		// Token: 0x04000C36 RID: 3126
		private const string defaultValueAttribute = "value";
	}
}
