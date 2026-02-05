using System;
using System.Collections.Generic;

namespace System.Runtime.Serialization
{
	// Token: 0x02000083 RID: 131
	public sealed class ExtensionDataObject
	{
		// Token: 0x06000968 RID: 2408 RVA: 0x0002A35C File Offset: 0x0002855C
		internal ExtensionDataObject()
		{
		}

		// Token: 0x17000163 RID: 355
		// (get) Token: 0x06000969 RID: 2409 RVA: 0x0002A364 File Offset: 0x00028564
		// (set) Token: 0x0600096A RID: 2410 RVA: 0x0002A36C File Offset: 0x0002856C
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

		// Token: 0x0400039A RID: 922
		private IList<ExtensionDataMember> members;
	}
}
