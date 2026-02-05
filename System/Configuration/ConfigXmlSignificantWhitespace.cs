using System;
using System.Configuration.Internal;
using System.Xml;

namespace System.Configuration
{
	// Token: 0x02000089 RID: 137
	internal sealed class ConfigXmlSignificantWhitespace : XmlSignificantWhitespace, IConfigErrorInfo
	{
		// Token: 0x06000546 RID: 1350 RVA: 0x0002181C File Offset: 0x0001FA1C
		public ConfigXmlSignificantWhitespace(string filename, int line, string strData, XmlDocument doc)
			: base(strData, doc)
		{
			this._line = line;
			this._filename = filename;
		}

		// Token: 0x170000C6 RID: 198
		// (get) Token: 0x06000547 RID: 1351 RVA: 0x00021835 File Offset: 0x0001FA35
		int IConfigErrorInfo.LineNumber
		{
			get
			{
				return this._line;
			}
		}

		// Token: 0x170000C7 RID: 199
		// (get) Token: 0x06000548 RID: 1352 RVA: 0x0002183D File Offset: 0x0001FA3D
		string IConfigErrorInfo.Filename
		{
			get
			{
				return this._filename;
			}
		}

		// Token: 0x06000549 RID: 1353 RVA: 0x00021848 File Offset: 0x0001FA48
		public override XmlNode CloneNode(bool deep)
		{
			XmlNode xmlNode = base.CloneNode(deep);
			ConfigXmlSignificantWhitespace configXmlSignificantWhitespace = xmlNode as ConfigXmlSignificantWhitespace;
			if (configXmlSignificantWhitespace != null)
			{
				configXmlSignificantWhitespace._line = this._line;
				configXmlSignificantWhitespace._filename = this._filename;
			}
			return xmlNode;
		}

		// Token: 0x04000C2A RID: 3114
		private int _line;

		// Token: 0x04000C2B RID: 3115
		private string _filename;
	}
}
