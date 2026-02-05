using System;

namespace Google.Protobuf.Reflection
{
	// Token: 0x02000074 RID: 116
	public interface IDescriptor
	{
		// Token: 0x17000216 RID: 534
		// (get) Token: 0x060007C8 RID: 1992
		string Name { get; }

		// Token: 0x17000217 RID: 535
		// (get) Token: 0x060007C9 RID: 1993
		string FullName { get; }

		// Token: 0x17000218 RID: 536
		// (get) Token: 0x060007CA RID: 1994
		FileDescriptor File { get; }
	}
}
