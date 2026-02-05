using System;

namespace Mono.Cecil
{
	// Token: 0x02000082 RID: 130
	public interface IMetadataResolver
	{
		// Token: 0x06000507 RID: 1287
		TypeDefinition Resolve(TypeReference type);

		// Token: 0x06000508 RID: 1288
		FieldDefinition Resolve(FieldReference field);

		// Token: 0x06000509 RID: 1289
		MethodDefinition Resolve(MethodReference method);
	}
}
