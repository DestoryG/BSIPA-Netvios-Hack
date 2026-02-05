using System;
using System.Configuration.Internal;
using System.Xml;

namespace System.Configuration
{
	// Token: 0x02000084 RID: 132
	internal sealed class ConfigXmlAttribute : XmlAttribute, IConfigErrorInfo
	{
		// Token: 0x06000528 RID: 1320 RVA: 0x000214B6 File Offset: 0x0001F6B6
		public ConfigXmlAttribute(string filename, int line, string prefix, string localName, string namespaceUri, XmlDocument doc)
			: base(prefix, localName, namespaceUri, doc)
		{
			this._line = line;
			this._filename = filename;
		}

		// Token: 0x170000BA RID: 186
		// (get) Token: 0x06000529 RID: 1321 RVA: 0x000214D3 File Offset: 0x0001F6D3
		int IConfigErrorInfo.LineNumber
		{
			get
			{
				return this._line;
			}
		}

		// Token: 0x170000BB RID: 187
		// (get) Token: 0x0600052A RID: 1322 RVA: 0x000214DB File Offset: 0x0001F6DB
		string IConfigErrorInfo.Filename
		{
			get
			{
				return this._filename;
			}
		}

		// Token: 0x0600052B RID: 1323 RVA: 0x000214E4 File Offset: 0x0001F6E4
		public override XmlNode CloneNode(bool deep)
		{
			XmlNode xmlNode = base.CloneNode(deep);
			ConfigXmlAttribute configXmlAttribute = xmlNode as ConfigXmlAttribute;
			if (configXmlAttribute != null)
			{
				configXmlAttribute._line = this._line;
				configXmlAttribute._filename = this._filename;
			}
			return xmlNode;
		}

		// Token: 0x04000C1F RID: 3103
		private int _line;

		// Token: 0x04000C20 RID: 3104
		private string _filename;
	}
}
