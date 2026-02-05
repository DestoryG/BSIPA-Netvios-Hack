using System;
using System.Collections.Generic;

namespace System.Runtime.Serialization
{
	// Token: 0x02000072 RID: 114
	internal class TypeHandleRefEqualityComparer : IEqualityComparer<TypeHandleRef>
	{
		// Token: 0x0600088F RID: 2191 RVA: 0x0002803C File Offset: 0x0002623C
		public bool Equals(TypeHandleRef x, TypeHandleRef y)
		{
			return x.Value.Equals(y.Value);
		}

		// Token: 0x06000890 RID: 2192 RVA: 0x00028060 File Offset: 0x00026260
		public int GetHashCode(TypeHandleRef obj)
		{
			return obj.Value.GetHashCode();
		}
	}
}
