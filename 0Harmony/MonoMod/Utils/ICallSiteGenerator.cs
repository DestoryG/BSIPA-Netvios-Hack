using System;
using Mono.Cecil;

namespace MonoMod.Utils
{
	// Token: 0x02000331 RID: 817
	internal interface ICallSiteGenerator
	{
		// Token: 0x060012D6 RID: 4822
		CallSite ToCallSite(ModuleDefinition module);
	}
}
