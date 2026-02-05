using System;
using System.Collections.Generic;
using System.Xml;

namespace System.Runtime.Serialization
{
	// Token: 0x02000089 RID: 137
	internal class XmlDataNode : DataNode<object>
	{
		// Token: 0x060009AB RID: 2475 RVA: 0x0002A6CE File Offset: 0x000288CE
		internal XmlDataNode()
		{
			this.dataType = Globals.TypeOfXmlDataNode;
		}

		// Token: 0x1700017F RID: 383
		// (get) Token: 0x060009AC RID: 2476 RVA: 0x0002A6E1 File Offset: 0x000288E1
		// (set) Token: 0x060009AD RID: 2477 RVA: 0x0002A6E9 File Offset: 0x000288E9
		internal IList<XmlAttribute> XmlAttributes
		{
			get
			{
				return this.xmlAttributes;
			}
			set
			{
				this.xmlAttributes = value;
			}
		}

		// Token: 0x17000180 RID: 384
		// (get) Token: 0x060009AE RID: 2478 RVA: 0x0002A6F2 File Offset: 0x000288F2
		// (set) Token: 0x060009AF RID: 2479 RVA: 0x0002A6FA File Offset: 0x000288FA
		internal IList<XmlNode> XmlChildNodes
		{
			get
			{
				return this.xmlChildNodes;
			}
			set
			{
				this.xmlChildNodes = value;
			}
		}

		// Token: 0x17000181 RID: 385
		// (get) Token: 0x060009B0 RID: 2480 RVA: 0x0002A703 File Offset: 0x00028903
		// (set) Token: 0x060009B1 RID: 2481 RVA: 0x0002A70B File Offset: 0x0002890B
		internal XmlDocument OwnerDocument
		{
			get
			{
				return this.ownerDocument;
			}
			set
			{
				this.ownerDocument = value;
			}
		}

		// Token: 0x060009B2 RID: 2482 RVA: 0x0002A714 File Offset: 0x00028914
		public override void Clear()
		{
			base.Clear();
			this.xmlAttributes = null;
			this.xmlChildNodes = null;
			this.ownerDocument = null;
		}

		// Token: 0x040003AC RID: 940
		private IList<XmlAttribute> xmlAttributes;

		// Token: 0x040003AD RID: 941
		private IList<XmlNode> xmlChildNodes;

		// Token: 0x040003AE RID: 942
		private XmlDocument ownerDocument;
	}
}
