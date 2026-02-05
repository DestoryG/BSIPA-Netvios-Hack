using System;
using System.IO;
using System.Runtime.Serialization;
using System.Text;

namespace System.Xml
{
	// Token: 0x0200005A RID: 90
	internal class XmlUTF8TextWriter : XmlBaseWriter, IXmlTextWriterInitializer
	{
		// Token: 0x1700009D RID: 157
		// (get) Token: 0x06000666 RID: 1638 RVA: 0x0001DE17 File Offset: 0x0001C017
		internal override bool FastAsync
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06000667 RID: 1639 RVA: 0x0001DE1C File Offset: 0x0001C01C
		public void SetOutput(Stream stream, Encoding encoding, bool ownsStream)
		{
			if (stream == null)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("stream");
			}
			if (encoding == null)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("encoding");
			}
			if (encoding.WebName != Encoding.UTF8.WebName)
			{
				stream = new EncodingStreamWrapper(stream, encoding, true);
			}
			if (this.writer == null)
			{
				this.writer = new XmlUTF8NodeWriter();
			}
			this.writer.SetOutput(stream, ownsStream, encoding);
			base.SetOutput(this.writer);
		}

		// Token: 0x1700009E RID: 158
		// (get) Token: 0x06000668 RID: 1640 RVA: 0x0001DE93 File Offset: 0x0001C093
		public override bool CanFragment
		{
			get
			{
				return this.writer.Encoding == null;
			}
		}

		// Token: 0x06000669 RID: 1641 RVA: 0x0001DEA3 File Offset: 0x0001C0A3
		protected override XmlSigningNodeWriter CreateSigningNodeWriter()
		{
			return new XmlSigningNodeWriter(true);
		}

		// Token: 0x040002A8 RID: 680
		private XmlUTF8NodeWriter writer;
	}
}
