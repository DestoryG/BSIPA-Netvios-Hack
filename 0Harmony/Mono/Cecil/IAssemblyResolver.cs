using System;

namespace Mono.Cecil
{
	// Token: 0x02000138 RID: 312
	internal interface IAssemblyResolver : IDisposable
	{
		// Token: 0x06000897 RID: 2199
		AssemblyDefinition Resolve(AssemblyNameReference name);

		// Token: 0x06000898 RID: 2200
		AssemblyDefinition Resolve(AssemblyNameReference name, ReaderParameters parameters);
	}
}
