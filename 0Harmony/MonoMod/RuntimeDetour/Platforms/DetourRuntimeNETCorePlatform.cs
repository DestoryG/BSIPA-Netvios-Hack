using System;

namespace MonoMod.RuntimeDetour.Platforms
{
	// Token: 0x02000362 RID: 866
	internal class DetourRuntimeNETCorePlatform : DetourRuntimeNETPlatform
	{
		// Token: 0x06001423 RID: 5155 RVA: 0x00010C51 File Offset: 0x0000EE51
		protected override void DisableInlining(RuntimeMethodHandle handle)
		{
		}
	}
}
