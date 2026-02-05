using System;
using System.Reflection;
using System.Reflection.Emit;

namespace MonoMod.RuntimeDetour.Platforms
{
	// Token: 0x02000361 RID: 865
	internal class DetourRuntimeMonoPlatform : DetourRuntimeILPlatform
	{
		// Token: 0x0600141F RID: 5151 RVA: 0x00048B28 File Offset: 0x00046D28
		protected override RuntimeMethodHandle GetMethodHandle(MethodBase method)
		{
			if (method is DynamicMethod)
			{
				MethodInfo dynamicMethod_CreateDynMethod = DetourRuntimeMonoPlatform._DynamicMethod_CreateDynMethod;
				if (dynamicMethod_CreateDynMethod != null)
				{
					dynamicMethod_CreateDynMethod.Invoke(method, DetourRuntimeMonoPlatform._NoArgs);
				}
				if (DetourRuntimeMonoPlatform._DynamicMethod_mhandle != null)
				{
					return (RuntimeMethodHandle)DetourRuntimeMonoPlatform._DynamicMethod_mhandle.GetValue(method);
				}
			}
			return method.MethodHandle;
		}

		// Token: 0x06001420 RID: 5152 RVA: 0x00048B78 File Offset: 0x00046D78
		protected unsafe override void DisableInlining(RuntimeMethodHandle handle)
		{
			ushort* ptr = (long)handle.Value / 2L + 2L;
			ushort* ptr2 = ptr;
			*ptr2 |= 8;
		}

		// Token: 0x04001191 RID: 4497
		private static readonly object[] _NoArgs = new object[0];

		// Token: 0x04001192 RID: 4498
		private static readonly MethodInfo _DynamicMethod_CreateDynMethod = typeof(DynamicMethod).GetMethod("CreateDynMethod", BindingFlags.Instance | BindingFlags.NonPublic);

		// Token: 0x04001193 RID: 4499
		private static readonly FieldInfo _DynamicMethod_mhandle = typeof(DynamicMethod).GetField("mhandle", BindingFlags.Instance | BindingFlags.NonPublic);
	}
}
