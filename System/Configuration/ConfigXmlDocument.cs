using System;
using System.Configuration.Internal;
using System.IO;
using System.Security.Permissions;
using System.Xml;

namespace System.Configuration
{
	// Token: 0x02000087 RID: 135
	[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
	public sealed class ConfigXmlDocument : XmlDocument, IConfigErrorInfo
	{
		// Token: 0x170000C0 RID: 192
		// (get) Token: 0x06000534 RID: 1332 RVA: 0x000215E4 File Offset: 0x0001F7E4
		int IConfigErrorInfo.LineNumber
		{
			get
			{
				if (this._reader == null)
				{
					return 0;
				}
				if (this._lineOffset > 0)
				{
					return this._reader.LineNumber + this._lineOffset - 1;
				}
				return this._reader.LineNumber;
			}
		}

		// Token: 0x170000C1 RID: 193
		// (get) Token: 0x06000535 RID: 1333 RVA: 0x00021619 File Offset: 0x0001F819
		public int LineNumber
		{
			get
			{
				return ((IConfigErrorInfo)this).LineNumber;
			}
		}

		// Token: 0x170000C2 RID: 194
		// (get) Token: 0x06000536 RID: 1334 RVA: 0x00021621 File Offset: 0x0001F821
		public string Filename
		{
			get
			{
				return ConfigurationException.SafeFilename(this._filename);
			}
		}

		// Token: 0x170000C3 RID: 195
		// (get) Token: 0x06000537 RID: 1335 RVA: 0x0002162E File Offset: 0x0001F82E
		string IConfigErrorInfo.Filename
		{
			get
			{
				return this._filename;
			}
		}

		// Token: 0x06000538 RID: 1336 RVA: 0x00021638 File Offset: 0x0001F838
		public override void Load(string filename)
		{
			this._filename = filename;
			try
			{
				this._reader = new XmlTextReader(filename);
				this._reader.XmlResolver = null;
				base.Load(this._reader);
			}
			finally
			{
				if (this._reader != null)
				{
					this._reader.Close();
					this._reader = null;
				}
			}
		}

		// Token: 0x06000539 RID: 1337 RVA: 0x000216A0 File Offset: 0x0001F8A0
		public void LoadSingleElement(string filename, XmlTextReader sourceReader)
		{
			this._filename = filename;
			this._lineOffset = sourceReader.LineNumber;
			string text = sourceReader.ReadOuterXml();
			try
			{
				this._reader = new XmlTextReader(new StringReader(text), sourceReader.NameTable);
				base.Load(this._reader);
			}
			finally
			{
				if (this._reader != null)
				{
					this._reader.Close();
					this._reader = null;
				}
			}
		}

		// Token: 0x0600053A RID: 1338 RVA: 0x00021718 File Offset: 0x0001F918
		public override XmlAttribute CreateAttribute(string prefix, string localName, string namespaceUri)
		{
			return new ConfigXmlAttribute(this._filename, this.LineNumber, prefix, localName, namespaceUri, this);
		}

		// Token: 0x0600053B RID: 1339 RVA: 0x0002172F File Offset: 0x0001F92F
		public override XmlElement CreateElement(string prefix, string localName, string namespaceUri)
		{
			return new ConfigXmlElement(this._filename, this.LineNumber, prefix, localName, namespaceUri, this);
		}

		// Token: 0x0600053C RID: 1340 RVA: 0x00021746 File Offset: 0x0001F946
		public override XmlText CreateTextNode(string text)
		{
			return new ConfigXmlText(this._filename, this.LineNumber, text, this);
		}

		// Token: 0x0600053D RID: 1341 RVA: 0x0002175B File Offset: 0x0001F95B
		public override XmlCDataSection CreateCDataSection(string data)
		{
			return new ConfigXmlCDataSection(this._filename, this.LineNumber, data, this);
		}

		// Token: 0x0600053E RID: 1342 RVA: 0x00021770 File Offset: 0x0001F970
		public override XmlComment CreateComment(string data)
		{
			return new ConfigXmlComment(this._filename, this.LineNumber, data, this);
		}

		// Token: 0x0600053F RID: 1343 RVA: 0x00021785 File Offset: 0x0001F985
		public override XmlSignificantWhitespace CreateSignificantWhitespace(string data)
		{
			return new ConfigXmlSignificantWhitespace(this._filename, this.LineNumber, data, this);
		}

		// Token: 0x06000540 RID: 1344 RVA: 0x0002179A File Offset: 0x0001F99A
		public override XmlWhitespace CreateWhitespace(string data)
		{
			return new ConfigXmlWhitespace(this._filename, this.LineNumber, data, this);
		}

		// Token: 0x04000C25 RID: 3109
		private XmlTextReader _reader;

		// Token: 0x04000C26 RID: 3110
		private int _lineOffset;

		// Token: 0x04000C27 RID: 3111
		private string _filename;
	}
}
