using System;
using System.Collections.Generic;
using System.Globalization;

namespace System.Runtime.Serialization
{
	// Token: 0x02000088 RID: 136
	internal class CollectionDataNode : DataNode<Array>
	{
		// Token: 0x060009A0 RID: 2464 RVA: 0x0002A61B File Offset: 0x0002881B
		internal CollectionDataNode()
		{
			this.dataType = Globals.TypeOfCollectionDataNode;
		}

		// Token: 0x1700017B RID: 379
		// (get) Token: 0x060009A1 RID: 2465 RVA: 0x0002A635 File Offset: 0x00028835
		// (set) Token: 0x060009A2 RID: 2466 RVA: 0x0002A63D File Offset: 0x0002883D
		internal IList<IDataNode> Items
		{
			get
			{
				return this.items;
			}
			set
			{
				this.items = value;
			}
		}

		// Token: 0x1700017C RID: 380
		// (get) Token: 0x060009A3 RID: 2467 RVA: 0x0002A646 File Offset: 0x00028846
		// (set) Token: 0x060009A4 RID: 2468 RVA: 0x0002A64E File Offset: 0x0002884E
		internal string ItemName
		{
			get
			{
				return this.itemName;
			}
			set
			{
				this.itemName = value;
			}
		}

		// Token: 0x1700017D RID: 381
		// (get) Token: 0x060009A5 RID: 2469 RVA: 0x0002A657 File Offset: 0x00028857
		// (set) Token: 0x060009A6 RID: 2470 RVA: 0x0002A65F File Offset: 0x0002885F
		internal string ItemNamespace
		{
			get
			{
				return this.itemNamespace;
			}
			set
			{
				this.itemNamespace = value;
			}
		}

		// Token: 0x1700017E RID: 382
		// (get) Token: 0x060009A7 RID: 2471 RVA: 0x0002A668 File Offset: 0x00028868
		// (set) Token: 0x060009A8 RID: 2472 RVA: 0x0002A670 File Offset: 0x00028870
		internal int Size
		{
			get
			{
				return this.size;
			}
			set
			{
				this.size = value;
			}
		}

		// Token: 0x060009A9 RID: 2473 RVA: 0x0002A67C File Offset: 0x0002887C
		public override void GetData(ElementData element)
		{
			base.GetData(element);
			element.AddAttribute("z", "http://schemas.microsoft.com/2003/10/Serialization/", "Size", this.Size.ToString(NumberFormatInfo.InvariantInfo));
		}

		// Token: 0x060009AA RID: 2474 RVA: 0x0002A6B8 File Offset: 0x000288B8
		public override void Clear()
		{
			base.Clear();
			this.items = null;
			this.size = -1;
		}

		// Token: 0x040003A8 RID: 936
		private IList<IDataNode> items;

		// Token: 0x040003A9 RID: 937
		private string itemName;

		// Token: 0x040003AA RID: 938
		private string itemNamespace;

		// Token: 0x040003AB RID: 939
		private int size = -1;
	}
}
