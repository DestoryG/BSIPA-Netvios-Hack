using System;
using System.Reflection;

namespace Mono.Cecil
{
	// Token: 0x02000129 RID: 297
	internal interface IReflectionImporter
	{
		// Token: 0x0600081C RID: 2076
		AssemblyNameReference ImportReference(AssemblyName reference);

		// Token: 0x0600081D RID: 2077
		TypeReference ImportReference(Type type, IGenericParameterProvider context);

		// Token: 0x0600081E RID: 2078
		FieldReference ImportReference(FieldInfo field, IGenericParameterProvider context);

		// Token: 0x0600081F RID: 2079
		MethodReference ImportReference(MethodBase method, IGenericParameterProvider context);
	}
}
