using System;
using System.Collections.Generic;

namespace System.Runtime.Serialization
{
	// Token: 0x0200008A RID: 138
	internal class ISerializableDataNode : DataNode<object>
	{
		// Token: 0x060009B3 RID: 2483 RVA: 0x0002A731 File Offset: 0x00028931
		internal ISerializableDataNode()
		{
			this.dataType = Globals.TypeOfISerializableDataNode;
		}

		// Token: 0x17000182 RID: 386
		// (get) Token: 0x060009B4 RID: 2484 RVA: 0x0002A744 File Offset: 0x00028944
		// (set) Token: 0x060009B5 RID: 2485 RVA: 0x0002A74C File Offset: 0x0002894C
		internal string FactoryTypeName
		{
			get
			{
				return this.factoryTypeName;
			}
			set
			{
				this.factoryTypeName = value;
			}
		}

		// Token: 0x17000183 RID: 387
		// (get) Token: 0x060009B6 RID: 2486 RVA: 0x0002A755 File Offset: 0x00028955
		// (set) Token: 0x060009B7 RID: 2487 RVA: 0x0002A75D File Offset: 0x0002895D
		internal string FactoryTypeNamespace
		{
			get
			{
				return this.factoryTypeNamespace;
			}
			set
			{
				this.factoryTypeNamespace = value;
			}
		}

		// Token: 0x17000184 RID: 388
		// (get) Token: 0x060009B8 RID: 2488 RVA: 0x0002A766 File Offset: 0x00028966
		// (set) Token: 0x060009B9 RID: 2489 RVA: 0x0002A76E File Offset: 0x0002896E
		internal IList<ISerializableDataMember> Members
		{
			get
			{
				return this.members;
			}
			set
			{
				this.members = value;
			}
		}

		// Token: 0x060009BA RID: 2490 RVA: 0x0002A777 File Offset: 0x00028977
		public override void GetData(ElementData element)
		{
			base.GetData(element);
			if (this.FactoryTypeName != null)
			{
				base.AddQualifiedNameAttribute(element, "z", "FactoryType", "http://schemas.microsoft.com/2003/10/Serialization/", this.FactoryTypeName, this.FactoryTypeNamespace);
			}
		}

		// Token: 0x060009BB RID: 2491 RVA: 0x0002A7AC File Offset: 0x000289AC
		public override void Clear()
		{
			base.Clear();
			this.members = null;
			this.factoryTypeName = (this.factoryTypeNamespace = null);
		}

		// Token: 0x040003AF RID: 943
		private string factoryTypeName;

		// Token: 0x040003B0 RID: 944
		private string factoryTypeNamespace;

		// Token: 0x040003B1 RID: 945
		private IList<ISerializableDataMember> members;
	}
}
