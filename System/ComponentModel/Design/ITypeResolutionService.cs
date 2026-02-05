using System;
using System.Reflection;

namespace System.ComponentModel.Design
{
	// Token: 0x020005FB RID: 1531
	public interface ITypeResolutionService
	{
		// Token: 0x06003858 RID: 14424
		Assembly GetAssembly(AssemblyName name);

		// Token: 0x06003859 RID: 14425
		Assembly GetAssembly(AssemblyName name, bool throwOnError);

		// Token: 0x0600385A RID: 14426
		Type GetType(string name);

		// Token: 0x0600385B RID: 14427
		Type GetType(string name, bool throwOnError);

		// Token: 0x0600385C RID: 14428
		Type GetType(string name, bool throwOnError, bool ignoreCase);

		// Token: 0x0600385D RID: 14429
		void ReferenceAssembly(AssemblyName name);

		// Token: 0x0600385E RID: 14430
		string GetPathOfAssembly(AssemblyName name);
	}
}
