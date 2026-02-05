using System;
using System.Reflection;

namespace Mono.Cecil
{
	// Token: 0x02000073 RID: 115
	public interface IReflectionImporter
	{
		// Token: 0x0600048A RID: 1162
		AssemblyNameReference ImportReference(AssemblyName reference);

		// Token: 0x0600048B RID: 1163
		TypeReference ImportReference(Type type, IGenericParameterProvider context);

		// Token: 0x0600048C RID: 1164
		FieldReference ImportReference(FieldInfo field, IGenericParameterProvider context);

		// Token: 0x0600048D RID: 1165
		MethodReference ImportReference(MethodBase method, IGenericParameterProvider context);
	}
}
