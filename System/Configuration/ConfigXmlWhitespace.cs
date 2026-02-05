using System;
using System.Configuration.Internal;
using System.Xml;

namespace System.Configuration
{
	// Token: 0x0200008B RID: 139
	internal sealed class ConfigXmlWhitespace : XmlWhitespace, IConfigErrorInfo
	{
		// Token: 0x0600054E RID: 1358 RVA: 0x000218E4 File Offset: 0x0001FAE4
		public ConfigXmlWhitespace(string filename, int line, string comment, XmlDocument doc)
			: base(comment, doc)
		{
			this._line = line;
			this._filename = filename;
		}

		// Token: 0x170000CA RID: 202
		// (get) Token: 0x0600054F RID: 1359 RVA: 0x000218FD File Offset: 0x0001FAFD
		int IConfigErrorInfo.LineNumber
		{
			get
			{
				return this._line;
			}
		}

		// Token: 0x170000CB RID: 203
		// (get) Token: 0x06000550 RID: 1360 RVA: 0x00021905 File Offset: 0x0001FB05
		string IConfigErrorInfo.Filename
		{
			get
			{
				return this._filename;
			}
		}

		// Token: 0x06000551 RID: 1361 RVA: 0x00021910 File Offset: 0x0001FB10
		public override XmlNode CloneNode(bool deep)
		{
			XmlNode xmlNode = base.CloneNode(deep);
			ConfigXmlWhitespace configXmlWhitespace = xmlNode as ConfigXmlWhitespace;
			if (configXmlWhitespace != null)
			{
				configXmlWhitespace._line = this._line;
				configXmlWhitespace._filename = this._filename;
			}
			return xmlNode;
		}

		// Token: 0x04000C2E RID: 3118
		private int _line;

		// Token: 0x04000C2F RID: 3119
		private string _filename;
	}
}
