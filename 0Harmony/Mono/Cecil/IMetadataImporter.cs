using System;

namespace Mono.Cecil
{
	// Token: 0x02000127 RID: 295
	internal interface IMetadataImporter
	{
		// Token: 0x06000817 RID: 2071
		AssemblyNameReference ImportReference(AssemblyNameReference reference);

		// Token: 0x06000818 RID: 2072
		TypeReference ImportReference(TypeReference type, IGenericParameterProvider context);

		// Token: 0x06000819 RID: 2073
		FieldReference ImportReference(FieldReference field, IGenericParameterProvider context);

		// Token: 0x0600081A RID: 2074
		MethodReference ImportReference(MethodReference method, IGenericParameterProvider context);
	}
}
