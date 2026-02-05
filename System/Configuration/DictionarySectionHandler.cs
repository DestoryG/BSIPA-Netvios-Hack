using System;
using System.Collections;
using System.Xml;

namespace System.Configuration
{
	// Token: 0x0200008C RID: 140
	public class DictionarySectionHandler : IConfigurationSectionHandler
	{
		// Token: 0x06000552 RID: 1362 RVA: 0x00021948 File Offset: 0x0001FB48
		public virtual object Create(object parent, object context, XmlNode section)
		{
			Hashtable hashtable;
			if (parent == null)
			{
				hashtable = new Hashtable(StringComparer.OrdinalIgnoreCase);
			}
			else
			{
				hashtable = (Hashtable)((Hashtable)parent).Clone();
			}
			HandlerBase.CheckForUnrecognizedAttributes(section);
			foreach (object obj in section.ChildNodes)
			{
				XmlNode xmlNode = (XmlNode)obj;
				if (!HandlerBase.IsIgnorableAlsoCheckForNonElement(xmlNode))
				{
					if (xmlNode.Name == "add")
					{
						HandlerBase.CheckForChildNodes(xmlNode);
						string text = HandlerBase.RemoveRequiredAttribute(xmlNode, this.KeyAttributeName);
						string text2;
						if (this.ValueRequired)
						{
							text2 = HandlerBase.RemoveRequiredAttribute(xmlNode, this.ValueAttributeName);
						}
						else
						{
							text2 = HandlerBase.RemoveAttribute(xmlNode, this.ValueAttributeName);
						}
						HandlerBase.CheckForUnrecognizedAttributes(xmlNode);
						if (text2 == null)
						{
							text2 = "";
						}
						hashtable[text] = text2;
					}
					else if (xmlNode.Name == "remove")
					{
						HandlerBase.CheckForChildNodes(xmlNode);
						string text3 = HandlerBase.RemoveRequiredAttribute(xmlNode, this.KeyAttributeName);
						HandlerBase.CheckForUnrecognizedAttributes(xmlNode);
						hashtable.Remove(text3);
					}
					else if (xmlNode.Name.Equals("clear"))
					{
						HandlerBase.CheckForChildNodes(xmlNode);
						HandlerBase.CheckForUnrecognizedAttributes(xmlNode);
						hashtable.Clear();
					}
					else
					{
						HandlerBase.ThrowUnrecognizedElement(xmlNode);
					}
				}
			}
			return hashtable;
		}

		// Token: 0x170000CC RID: 204
		// (get) Token: 0x06000553 RID: 1363 RVA: 0x00021AA0 File Offset: 0x0001FCA0
		protected virtual string KeyAttributeName
		{
			get
			{
				return "key";
			}
		}

		// Token: 0x170000CD RID: 205
		// (get) Token: 0x06000554 RID: 1364 RVA: 0x00021AA7 File Offset: 0x0001FCA7
		protected virtual string ValueAttributeName
		{
			get
			{
				return "value";
			}
		}

		// Token: 0x170000CE RID: 206
		// (get) Token: 0x06000555 RID: 1365 RVA: 0x00021AAE File Offset: 0x0001FCAE
		internal virtual bool ValueRequired
		{
			get
			{
				return false;
			}
		}
	}
}
