using System;
using System.Configuration.Internal;
using System.Xml;

namespace System.Configuration
{
	// Token: 0x0200008A RID: 138
	internal sealed class ConfigXmlText : XmlText, IConfigErrorInfo
	{
		// Token: 0x0600054A RID: 1354 RVA: 0x00021880 File Offset: 0x0001FA80
		public ConfigXmlText(string filename, int line, string strData, XmlDocument doc)
			: base(strData, doc)
		{
			this._line = line;
			this._filename = filename;
		}

		// Token: 0x170000C8 RID: 200
		// (get) Token: 0x0600054B RID: 1355 RVA: 0x00021899 File Offset: 0x0001FA99
		int IConfigErrorInfo.LineNumber
		{
			get
			{
				return this._line;
			}
		}

		// Token: 0x170000C9 RID: 201
		// (get) Token: 0x0600054C RID: 1356 RVA: 0x000218A1 File Offset: 0x0001FAA1
		string IConfigErrorInfo.Filename
		{
			get
			{
				return this._filename;
			}
		}

		// Token: 0x0600054D RID: 1357 RVA: 0x000218AC File Offset: 0x0001FAAC
		public override XmlNode CloneNode(bool deep)
		{
			XmlNode xmlNode = base.CloneNode(deep);
			ConfigXmlText configXmlText = xmlNode as ConfigXmlText;
			if (configXmlText != null)
			{
				configXmlText._line = this._line;
				configXmlText._filename = this._filename;
			}
			return xmlNode;
		}

		// Token: 0x04000C2C RID: 3116
		private int _line;

		// Token: 0x04000C2D RID: 3117
		private string _filename;
	}
}
