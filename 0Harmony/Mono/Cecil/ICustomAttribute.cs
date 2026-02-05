using System;
using Mono.Collections.Generic;

namespace Mono.Cecil
{
	// Token: 0x02000102 RID: 258
	internal interface ICustomAttribute
	{
		// Token: 0x17000064 RID: 100
		// (get) Token: 0x060006AF RID: 1711
		TypeReference AttributeType { get; }

		// Token: 0x17000065 RID: 101
		// (get) Token: 0x060006B0 RID: 1712
		bool HasFields { get; }

		// Token: 0x17000066 RID: 102
		// (get) Token: 0x060006B1 RID: 1713
		bool HasProperties { get; }

		// Token: 0x17000067 RID: 103
		// (get) Token: 0x060006B2 RID: 1714
		bool HasConstructorArguments { get; }

		// Token: 0x17000068 RID: 104
		// (get) Token: 0x060006B3 RID: 1715
		Collection<CustomAttributeNamedArgument> Fields { get; }

		// Token: 0x17000069 RID: 105
		// (get) Token: 0x060006B4 RID: 1716
		Collection<CustomAttributeNamedArgument> Properties { get; }

		// Token: 0x1700006A RID: 106
		// (get) Token: 0x060006B5 RID: 1717
		Collection<CustomAttributeArgument> ConstructorArguments { get; }
	}
}
