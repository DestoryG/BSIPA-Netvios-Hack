using System;

namespace Mono.Cecil
{
	// Token: 0x02000071 RID: 113
	public interface IMetadataImporter
	{
		// Token: 0x06000485 RID: 1157
		AssemblyNameReference ImportReference(AssemblyNameReference reference);

		// Token: 0x06000486 RID: 1158
		TypeReference ImportReference(TypeReference type, IGenericParameterProvider context);

		// Token: 0x06000487 RID: 1159
		FieldReference ImportReference(FieldReference field, IGenericParameterProvider context);

		// Token: 0x06000488 RID: 1160
		MethodReference ImportReference(MethodReference method, IGenericParameterProvider context);
	}
}
