using System;
using System.Collections;
using System.Runtime.CompilerServices;

namespace System
{
	// Token: 0x02000002 RID: 2
	internal interface IValueTupleInternal : global::System.Runtime.CompilerServices.ITuple
	{
		// Token: 0x06000001 RID: 1
		int GetHashCode(IEqualityComparer comparer);

		// Token: 0x06000002 RID: 2
		string ToStringEnd();
	}
}
