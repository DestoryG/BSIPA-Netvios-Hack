using System;
using System.Configuration.Internal;
using System.Xml;

namespace System.Configuration
{
	// Token: 0x02000086 RID: 134
	internal sealed class ConfigXmlComment : XmlComment, IConfigErrorInfo
	{
		// Token: 0x06000530 RID: 1328 RVA: 0x00021580 File Offset: 0x0001F780
		public ConfigXmlComment(string filename, int line, string comment, XmlDocument doc)
			: base(comment, doc)
		{
			this._line = line;
			this._filename = filename;
		}

		// Token: 0x170000BE RID: 190
		// (get) Token: 0x06000531 RID: 1329 RVA: 0x00021599 File Offset: 0x0001F799
		int IConfigErrorInfo.LineNumber
		{
			get
			{
				return this._line;
			}
		}

		// Token: 0x170000BF RID: 191
		// (get) Token: 0x06000532 RID: 1330 RVA: 0x000215A1 File Offset: 0x0001F7A1
		string IConfigErrorInfo.Filename
		{
			get
			{
				return this._filename;
			}
		}

		// Token: 0x06000533 RID: 1331 RVA: 0x000215AC File Offset: 0x0001F7AC
		public override XmlNode CloneNode(bool deep)
		{
			XmlNode xmlNode = base.CloneNode(deep);
			ConfigXmlComment configXmlComment = xmlNode as ConfigXmlComment;
			if (configXmlComment != null)
			{
				configXmlComment._line = this._line;
				configXmlComment._filename = this._filename;
			}
			return xmlNode;
		}

		// Token: 0x04000C23 RID: 3107
		private int _line;

		// Token: 0x04000C24 RID: 3108
		private string _filename;
	}
}
