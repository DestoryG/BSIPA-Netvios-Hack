using System;

namespace System.Runtime.CompilerServices
{
	// Token: 0x0200000C RID: 12
	public interface ITuple
	{
		// Token: 0x17000013 RID: 19
		// (get) Token: 0x06000099 RID: 153
		int Length { get; }

		// Token: 0x17000014 RID: 20
		object this[int index] { get; }
	}
}
