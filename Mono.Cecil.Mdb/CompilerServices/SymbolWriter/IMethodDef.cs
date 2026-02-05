using System;

namespace Mono.CompilerServices.SymbolWriter
{
	// Token: 0x02000007 RID: 7
	public interface IMethodDef
	{
		// Token: 0x17000003 RID: 3
		// (get) Token: 0x0600000B RID: 11
		string Name { get; }

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x0600000C RID: 12
		int Token { get; }
	}
}
