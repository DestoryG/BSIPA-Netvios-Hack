using System;
using System.Collections.Generic;

namespace System.Runtime.Serialization
{
	// Token: 0x02000087 RID: 135
	internal class ClassDataNode : DataNode<object>
	{
		// Token: 0x0600099C RID: 2460 RVA: 0x0002A5E8 File Offset: 0x000287E8
		internal ClassDataNode()
		{
			this.dataType = Globals.TypeOfClassDataNode;
		}

		// Token: 0x1700017A RID: 378
		// (get) Token: 0x0600099D RID: 2461 RVA: 0x0002A5FB File Offset: 0x000287FB
		// (set) Token: 0x0600099E RID: 2462 RVA: 0x0002A603 File Offset: 0x00028803
		internal IList<ExtensionDataMember> Members
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

		// Token: 0x0600099F RID: 2463 RVA: 0x0002A60C File Offset: 0x0002880C
		public override void Clear()
		{
			base.Clear();
			this.members = null;
		}

		// Token: 0x040003A7 RID: 935
		private IList<ExtensionDataMember> members;
	}
}
