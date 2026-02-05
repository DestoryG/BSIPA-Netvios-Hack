using System;
using System.Reflection;

namespace MonoMod.Utils
{
	// Token: 0x0200031C RID: 796
	internal interface _IDMDGenerator
	{
		// Token: 0x06001235 RID: 4661
		MethodInfo Generate(DynamicMethodDefinition dmd, object context);
	}
}
