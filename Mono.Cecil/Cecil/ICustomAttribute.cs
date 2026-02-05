using System;
using Mono.Collections.Generic;

namespace Mono.Cecil
{
	// Token: 0x02000052 RID: 82
	public interface ICustomAttribute
	{
		// Token: 0x1700004E RID: 78
		// (get) Token: 0x0600033B RID: 827
		TypeReference AttributeType { get; }

		// Token: 0x1700004F RID: 79
		// (get) Token: 0x0600033C RID: 828
		bool HasFields { get; }

		// Token: 0x17000050 RID: 80
		// (get) Token: 0x0600033D RID: 829
		bool HasProperties { get; }

		// Token: 0x17000051 RID: 81
		// (get) Token: 0x0600033E RID: 830
		bool HasConstructorArguments { get; }

		// Token: 0x17000052 RID: 82
		// (get) Token: 0x0600033F RID: 831
		Collection<CustomAttributeNamedArgument> Fields { get; }

		// Token: 0x17000053 RID: 83
		// (get) Token: 0x06000340 RID: 832
		Collection<CustomAttributeNamedArgument> Properties { get; }

		// Token: 0x17000054 RID: 84
		// (get) Token: 0x06000341 RID: 833
		Collection<CustomAttributeArgument> ConstructorArguments { get; }
	}
}
