using System;
using System.Xml.Linq;

namespace System.Xml.XPath
{
	// Token: 0x02000002 RID: 2
	public static class XDocumentExtensions
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		public static IXPathNavigable ToXPathNavigable(this XNode node)
		{
			return new XDocumentExtensions.XDocumentNavigable(node);
		}

		// Token: 0x02000003 RID: 3
		private class XDocumentNavigable : IXPathNavigable
		{
			// Token: 0x06000002 RID: 2 RVA: 0x00002058 File Offset: 0x00000258
			public XDocumentNavigable(XNode n)
			{
				this._node = n;
			}

			// Token: 0x06000003 RID: 3 RVA: 0x00002067 File Offset: 0x00000267
			public XPathNavigator CreateNavigator()
			{
				return this._node.CreateNavigator();
			}

			// Token: 0x04000001 RID: 1
			private XNode _node;
		}
	}
}
