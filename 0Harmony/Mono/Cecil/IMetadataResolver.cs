using System;

namespace Mono.Cecil
{
	// Token: 0x02000139 RID: 313
	internal interface IMetadataResolver
	{
		// Token: 0x06000899 RID: 2201
		TypeDefinition Resolve(TypeReference type);

		// Token: 0x0600089A RID: 2202
		FieldDefinition Resolve(FieldReference field);

		// Token: 0x0600089B RID: 2203
		MethodDefinition Resolve(MethodReference method);
	}
}
