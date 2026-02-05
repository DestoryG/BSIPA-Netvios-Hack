using System;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;

namespace MonoMod.RuntimeDetour.Platforms
{
	// Token: 0x02000363 RID: 867
	internal class DetourRuntimeNETPlatform : DetourRuntimeILPlatform
	{
		// Token: 0x06001425 RID: 5157 RVA: 0x00048C00 File Offset: 0x00046E00
		protected override RuntimeMethodHandle GetMethodHandle(MethodBase method)
		{
			DynamicMethod dynamicMethod = method as DynamicMethod;
			if (dynamicMethod != null)
			{
				if (DetourRuntimeNETPlatform._RuntimeHelpers__CompileMethod_TakesIntPtr)
				{
					DetourRuntimeNETPlatform._RuntimeHelpers__CompileMethod.Invoke(null, new object[] { ((RuntimeMethodHandle)DetourRuntimeNETPlatform._DynamicMethod_GetMethodDescriptor.Invoke(dynamicMethod, DetourRuntimeNETPlatform._NoArgs)).Value });
				}
				else if (DetourRuntimeNETPlatform._RuntimeHelpers__CompileMethod_TakesIRuntimeMethodInfo)
				{
					DetourRuntimeNETPlatform._RuntimeHelpers__CompileMethod.Invoke(null, new object[] { DetourRuntimeNETPlatform._RuntimeMethodHandle_m_value.GetValue((RuntimeMethodHandle)DetourRuntimeNETPlatform._DynamicMethod_GetMethodDescriptor.Invoke(dynamicMethod, DetourRuntimeNETPlatform._NoArgs)) });
				}
				else
				{
					try
					{
						dynamicMethod.CreateDelegate(typeof(MulticastDelegate));
					}
					catch
					{
					}
				}
				if (DetourRuntimeNETPlatform._DynamicMethod_m_method != null)
				{
					return (RuntimeMethodHandle)DetourRuntimeNETPlatform._DynamicMethod_m_method.GetValue(method);
				}
				if (DetourRuntimeNETPlatform._DynamicMethod_GetMethodDescriptor != null)
				{
					return (RuntimeMethodHandle)DetourRuntimeNETPlatform._DynamicMethod_GetMethodDescriptor.Invoke(method, DetourRuntimeNETPlatform._NoArgs);
				}
			}
			return method.MethodHandle;
		}

		// Token: 0x06001426 RID: 5158 RVA: 0x00010C51 File Offset: 0x0000EE51
		protected override void DisableInlining(RuntimeMethodHandle handle)
		{
		}

		// Token: 0x04001194 RID: 4500
		private static readonly object[] _NoArgs = new object[0];

		// Token: 0x04001195 RID: 4501
		private static readonly FieldInfo _DynamicMethod_m_method = typeof(DynamicMethod).GetField("m_method", BindingFlags.Instance | BindingFlags.NonPublic);

		// Token: 0x04001196 RID: 4502
		private static readonly MethodInfo _DynamicMethod_GetMethodDescriptor = typeof(DynamicMethod).GetMethod("GetMethodDescriptor", BindingFlags.Instance | BindingFlags.NonPublic);

		// Token: 0x04001197 RID: 4503
		private static readonly FieldInfo _RuntimeMethodHandle_m_value = typeof(RuntimeMethodHandle).GetField("m_value", BindingFlags.Instance | BindingFlags.NonPublic);

		// Token: 0x04001198 RID: 4504
		private static readonly MethodInfo _RuntimeHelpers__CompileMethod = typeof(RuntimeHelpers).GetMethod("_CompileMethod", BindingFlags.Static | BindingFlags.NonPublic);

		// Token: 0x04001199 RID: 4505
		private static readonly bool _RuntimeHelpers__CompileMethod_TakesIntPtr = DetourRuntimeNETPlatform._RuntimeHelpers__CompileMethod != null && DetourRuntimeNETPlatform._RuntimeHelpers__CompileMethod.GetParameters()[0].ParameterType.FullName == "System.IntPtr";

		// Token: 0x0400119A RID: 4506
		private static readonly bool _RuntimeHelpers__CompileMethod_TakesIRuntimeMethodInfo = DetourRuntimeNETPlatform._RuntimeHelpers__CompileMethod != null && DetourRuntimeNETPlatform._RuntimeHelpers__CompileMethod.GetParameters()[0].ParameterType.FullName == "System.IRuntimeMethodInfo";
	}
}
