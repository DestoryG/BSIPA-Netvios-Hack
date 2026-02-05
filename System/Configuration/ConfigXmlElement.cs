using System;
using System.Configuration.Internal;
using System.Xml;

namespace System.Configuration
{
	// Token: 0x02000088 RID: 136
	internal sealed class ConfigXmlElement : XmlElement, IConfigErrorInfo
	{
		// Token: 0x06000542 RID: 1346 RVA: 0x000217B7 File Offset: 0x0001F9B7
		public ConfigXmlElement(string filename, int line, string prefix, string localName, string namespaceUri, XmlDocument doc)
			: base(prefix, localName, namespaceUri, doc)
		{
			this._line = line;
			this._filename = filename;
		}

		// Token: 0x170000C4 RID: 196
		// (get) Token: 0x06000543 RID: 1347 RVA: 0x000217D4 File Offset: 0x0001F9D4
		int IConfigErrorInfo.LineNumber
		{
			get
			{
				return this._line;
			}
		}

		// Token: 0x170000C5 RID: 197
		// (get) Token: 0x06000544 RID: 1348 RVA: 0x000217DC File Offset: 0x0001F9DC
		string IConfigErrorInfo.Filename
		{
			get
			{
				return this._filename;
			}
		}

		// Token: 0x06000545 RID: 1349 RVA: 0x000217E4 File Offset: 0x0001F9E4
		public override XmlNode CloneNode(bool deep)
		{
			XmlNode xmlNode = base.CloneNode(deep);
			ConfigXmlElement configXmlElement = xmlNode as ConfigXmlElement;
			if (configXmlElement != null)
			{
				configXmlElement._line = this._line;
				configXmlElement._filename = this._filename;
			}
			return xmlNode;
		}

		// Token: 0x04000C28 RID: 3112
		private int _line;

		// Token: 0x04000C29 RID: 3113
		private string _filename;
	}
}
