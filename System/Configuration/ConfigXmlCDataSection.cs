using System;
using System.Configuration.Internal;
using System.Xml;

namespace System.Configuration
{
	// Token: 0x02000085 RID: 133
	internal sealed class ConfigXmlCDataSection : XmlCDataSection, IConfigErrorInfo
	{
		// Token: 0x0600052C RID: 1324 RVA: 0x0002151C File Offset: 0x0001F71C
		public ConfigXmlCDataSection(string filename, int line, string data, XmlDocument doc)
			: base(data, doc)
		{
			this._line = line;
			this._filename = filename;
		}

		// Token: 0x170000BC RID: 188
		// (get) Token: 0x0600052D RID: 1325 RVA: 0x00021535 File Offset: 0x0001F735
		int IConfigErrorInfo.LineNumber
		{
			get
			{
				return this._line;
			}
		}

		// Token: 0x170000BD RID: 189
		// (get) Token: 0x0600052E RID: 1326 RVA: 0x0002153D File Offset: 0x0001F73D
		string IConfigErrorInfo.Filename
		{
			get
			{
				return this._filename;
			}
		}

		// Token: 0x0600052F RID: 1327 RVA: 0x00021548 File Offset: 0x0001F748
		public override XmlNode CloneNode(bool deep)
		{
			XmlNode xmlNode = base.CloneNode(deep);
			ConfigXmlCDataSection configXmlCDataSection = xmlNode as ConfigXmlCDataSection;
			if (configXmlCDataSection != null)
			{
				configXmlCDataSection._line = this._line;
				configXmlCDataSection._filename = this._filename;
			}
			return xmlNode;
		}

		// Token: 0x04000C21 RID: 3105
		private int _line;

		// Token: 0x04000C22 RID: 3106
		private string _filename;
	}
}
