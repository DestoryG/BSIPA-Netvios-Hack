using System;
using System.Reflection;

namespace MonoMod.RuntimeDetour
{
	// Token: 0x0200034B RID: 843
	internal interface IDetourRuntimePlatform
	{
		// Token: 0x060013B0 RID: 5040
		IntPtr GetNativeStart(MethodBase method);

		// Token: 0x060013B1 RID: 5041
		MethodInfo CreateCopy(MethodBase method);

		// Token: 0x060013B2 RID: 5042
		bool TryCreateCopy(MethodBase method, out MethodInfo dm);

		// Token: 0x060013B3 RID: 5043
		void Pin(MethodBase method);

		// Token: 0x060013B4 RID: 5044
		void Unpin(MethodBase method);

		// Token: 0x060013B5 RID: 5045
		MethodBase GetDetourTarget(MethodBase from, MethodBase to);
	}
}
