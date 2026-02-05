using System;

namespace Mono.Cecil
{
	// Token: 0x02000081 RID: 129
	public interface IAssemblyResolver : IDisposable
	{
		// Token: 0x06000505 RID: 1285
		AssemblyDefinition Resolve(AssemblyNameReference name);

		// Token: 0x06000506 RID: 1286
		AssemblyDefinition Resolve(AssemblyNameReference name, ReaderParameters parameters);
	}
}
