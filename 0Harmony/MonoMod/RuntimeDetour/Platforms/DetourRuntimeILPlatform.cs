using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using Mono.Cecil.Cil;
using MonoMod.Utils;

namespace MonoMod.RuntimeDetour.Platforms
{
	// Token: 0x0200035C RID: 860
	internal abstract class DetourRuntimeILPlatform : IDetourRuntimePlatform
	{
		// Token: 0x06001409 RID: 5129
		protected abstract RuntimeMethodHandle GetMethodHandle(MethodBase method);

		// Token: 0x0600140A RID: 5130 RVA: 0x000484A0 File Offset: 0x000466A0
		public unsafe DetourRuntimeILPlatform()
		{
			MethodInfo method = typeof(DetourRuntimeILPlatform).GetMethod("_SelftestGetRefPtr", BindingFlags.Instance | BindingFlags.NonPublic);
			MethodInfo method2 = typeof(DetourRuntimeILPlatform).GetMethod("_SelftestGetRefPtrHook", BindingFlags.Static | BindingFlags.NonPublic);
			this._HookSelftest(method, method2);
			IntPtr intPtr = ((Func<IntPtr>)Delegate.CreateDelegate(typeof(Func<IntPtr>), this, method))();
			MethodInfo method3 = typeof(DetourRuntimeILPlatform).GetMethod("_SelftestGetStruct", BindingFlags.Instance | BindingFlags.NonPublic);
			MethodInfo method4 = typeof(DetourRuntimeILPlatform).GetMethod("_SelftestGetStructHook", BindingFlags.Static | BindingFlags.NonPublic);
			this._HookSelftest(method3, method4);
			fixed (DetourRuntimeILPlatform.GlueThiscallStructRetPtrOrder* ptr = &this.GlueThiscallStructRetPtr)
			{
				DetourRuntimeILPlatform.GlueThiscallStructRetPtrOrder* ptr2 = ptr;
				((Func<IntPtr, IntPtr, IntPtr, DetourRuntimeILPlatform._SelftestStruct>)Delegate.CreateDelegate(typeof(Func<IntPtr, IntPtr, IntPtr, DetourRuntimeILPlatform._SelftestStruct>), this, method3))((IntPtr)((void*)ptr2), (IntPtr)((void*)ptr2), intPtr);
			}
		}

		// Token: 0x0600140B RID: 5131 RVA: 0x00048584 File Offset: 0x00046784
		private void _HookSelftest(MethodInfo from, MethodInfo to)
		{
			this.Pin(from);
			this.Pin(to);
			NativeDetourData nativeDetourData = DetourHelper.Native.Create(this.GetNativeStart(from), this.GetNativeStart(to), null);
			DetourHelper.Native.MakeWritable(nativeDetourData);
			DetourHelper.Native.Apply(nativeDetourData);
			DetourHelper.Native.MakeExecutable(nativeDetourData);
			DetourHelper.Native.FlushICache(nativeDetourData);
			DetourHelper.Native.Free(nativeDetourData);
		}

		// Token: 0x0600140C RID: 5132 RVA: 0x000485F8 File Offset: 0x000467F8
		[MethodImpl(MethodImplOptions.NoInlining)]
		private IntPtr _SelftestGetRefPtr()
		{
			Console.Error.WriteLine("If you're reading this, the MonoMod.RuntimeDetour selftest failed.");
			throw new Exception("This method should've been detoured!");
		}

		// Token: 0x0600140D RID: 5133 RVA: 0x00010978 File Offset: 0x0000EB78
		private static IntPtr _SelftestGetRefPtrHook(IntPtr self)
		{
			return self;
		}

		// Token: 0x0600140E RID: 5134 RVA: 0x000485F8 File Offset: 0x000467F8
		[MethodImpl(MethodImplOptions.NoInlining)]
		private DetourRuntimeILPlatform._SelftestStruct _SelftestGetStruct(IntPtr x, IntPtr y, IntPtr thisPtr)
		{
			Console.Error.WriteLine("If you're reading this, the MonoMod.RuntimeDetour selftest failed.");
			throw new Exception("This method should've been detoured!");
		}

		// Token: 0x0600140F RID: 5135 RVA: 0x00048613 File Offset: 0x00046813
		private unsafe static void _SelftestGetStructHook(IntPtr a, IntPtr b, IntPtr c, IntPtr d, IntPtr e)
		{
			if (b == c)
			{
				*(int*)(void*)b = 0;
				return;
			}
			if (b == e)
			{
				*(int*)(void*)c = 2;
				return;
			}
			*(int*)(void*)c = 1;
		}

		// Token: 0x06001410 RID: 5136 RVA: 0x00048642 File Offset: 0x00046842
		protected virtual IntPtr GetFunctionPointer(RuntimeMethodHandle handle)
		{
			return handle.GetFunctionPointer();
		}

		// Token: 0x06001411 RID: 5137 RVA: 0x0004864B File Offset: 0x0004684B
		protected virtual void PrepareMethod(RuntimeMethodHandle handle)
		{
			RuntimeHelpers.PrepareMethod(handle);
		}

		// Token: 0x06001412 RID: 5138 RVA: 0x00048653 File Offset: 0x00046853
		protected virtual void PrepareMethod(RuntimeMethodHandle handle, RuntimeTypeHandle[] instantiation)
		{
			RuntimeHelpers.PrepareMethod(handle, instantiation);
		}

		// Token: 0x06001413 RID: 5139 RVA: 0x00010C51 File Offset: 0x0000EE51
		protected virtual void DisableInlining(RuntimeMethodHandle handle)
		{
		}

		// Token: 0x06001414 RID: 5140 RVA: 0x0004865C File Offset: 0x0004685C
		public IntPtr GetNativeStart(MethodBase method)
		{
			Dictionary<MethodBase, DetourRuntimeILPlatform.MethodPin> pinnedMethods = this.PinnedMethods;
			DetourRuntimeILPlatform.MethodPin methodPin;
			bool flag2;
			lock (pinnedMethods)
			{
				flag2 = this.PinnedMethods.TryGetValue(method, out methodPin);
			}
			if (flag2)
			{
				return this.GetFunctionPointer(methodPin.Handle);
			}
			return this.GetFunctionPointer(this.GetMethodHandle(method));
		}

		// Token: 0x06001415 RID: 5141 RVA: 0x000486C4 File Offset: 0x000468C4
		public void Pin(MethodBase method)
		{
			Dictionary<MethodBase, DetourRuntimeILPlatform.MethodPin> pinnedMethods = this.PinnedMethods;
			lock (pinnedMethods)
			{
				DetourRuntimeILPlatform.MethodPin methodPin;
				if (this.PinnedMethods.TryGetValue(method, out methodPin))
				{
					methodPin.Count++;
				}
				else
				{
					methodPin = new DetourRuntimeILPlatform.MethodPin();
					methodPin.Count = 1;
					RuntimeMethodHandle runtimeMethodHandle = (methodPin.Handle = this.GetMethodHandle(method));
					Type declaringType = method.DeclaringType;
					if (declaringType != null && declaringType.IsGenericType)
					{
						this.PrepareMethod(runtimeMethodHandle, (from type in method.DeclaringType.GetGenericArguments()
							select type.TypeHandle).ToArray<RuntimeTypeHandle>());
					}
					else
					{
						this.PrepareMethod(runtimeMethodHandle);
					}
					this.DisableInlining(runtimeMethodHandle);
					this.PinnedMethods[method] = methodPin;
				}
			}
		}

		// Token: 0x06001416 RID: 5142 RVA: 0x000487B0 File Offset: 0x000469B0
		public void Unpin(MethodBase method)
		{
			Dictionary<MethodBase, DetourRuntimeILPlatform.MethodPin> pinnedMethods = this.PinnedMethods;
			lock (pinnedMethods)
			{
				DetourRuntimeILPlatform.MethodPin methodPin;
				if (this.PinnedMethods.TryGetValue(method, out methodPin))
				{
					if (methodPin.Count <= 1)
					{
						this.PinnedMethods.Remove(method);
					}
					else
					{
						methodPin.Count--;
					}
				}
			}
		}

		// Token: 0x06001417 RID: 5143 RVA: 0x00048824 File Offset: 0x00046A24
		public MethodInfo CreateCopy(MethodBase method)
		{
			if (method == null || (method.GetMethodImplementationFlags() & MethodImplAttributes.CodeTypeMask) != MethodImplAttributes.IL)
			{
				throw new InvalidOperationException("Uncopyable method: " + (((method != null) ? method.ToString() : null) ?? "NULL"));
			}
			MethodInfo methodInfo;
			using (DynamicMethodDefinition dynamicMethodDefinition = new DynamicMethodDefinition(method))
			{
				methodInfo = dynamicMethodDefinition.Generate();
			}
			return methodInfo;
		}

		// Token: 0x06001418 RID: 5144 RVA: 0x00048894 File Offset: 0x00046A94
		public bool TryCreateCopy(MethodBase method, out MethodInfo dm)
		{
			if (method == null || (method.GetMethodImplementationFlags() & MethodImplAttributes.CodeTypeMask) != MethodImplAttributes.IL)
			{
				dm = null;
				return false;
			}
			bool flag;
			try
			{
				dm = this.CreateCopy(method);
				flag = true;
			}
			catch
			{
				dm = null;
				flag = false;
			}
			return flag;
		}

		// Token: 0x06001419 RID: 5145 RVA: 0x000488E0 File Offset: 0x00046AE0
		public MethodBase GetDetourTarget(MethodBase from, MethodBase to)
		{
			Type declaringType = to.DeclaringType;
			MethodInfo methodInfo = null;
			if (this.GlueThiscallStructRetPtr != DetourRuntimeILPlatform.GlueThiscallStructRetPtrOrder.Original)
			{
				MethodInfo methodInfo2 = from as MethodInfo;
				if (methodInfo2 != null && !from.IsStatic)
				{
					MethodInfo methodInfo3 = to as MethodInfo;
					if (methodInfo3 != null && to.IsStatic && methodInfo2.ReturnType == methodInfo3.ReturnType && methodInfo2.ReturnType.IsValueType)
					{
						int managedSize = methodInfo2.ReturnType.GetManagedSize();
						if (managedSize == 3 || managedSize == 5 || managedSize == 6 || managedSize == 7 || managedSize >= 9)
						{
							Type thisParamType = from.GetThisParamType();
							Type type = methodInfo2.ReturnType.MakeByRefType();
							int num = 0;
							int num2 = 1;
							if (this.GlueThiscallStructRetPtr == DetourRuntimeILPlatform.GlueThiscallStructRetPtrOrder.RetThisArgs)
							{
								num = 1;
								num2 = 0;
							}
							List<Type> list = new List<Type> { thisParamType };
							list.Insert(num2, type);
							list.AddRange(from p in @from.GetParameters()
								select p.ParameterType);
							using (DynamicMethodDefinition dynamicMethodDefinition = new DynamicMethodDefinition(string.Concat(new string[]
							{
								"Glue:ThiscallStructRetPtr<",
								from.GetID(null, null, true, false, true),
								",",
								to.GetID(null, null, true, false, true),
								">"
							}), typeof(void), list.ToArray()))
							{
								ILProcessor ilprocessor = dynamicMethodDefinition.GetILProcessor();
								ilprocessor.Emit(OpCodes.Ldarg, num2);
								ilprocessor.Emit(OpCodes.Ldarg, num);
								for (int i = 2; i < list.Count; i++)
								{
									ilprocessor.Emit(OpCodes.Ldarg, i);
								}
								ilprocessor.Emit(OpCodes.Call, ilprocessor.Body.Method.Module.ImportReference(to));
								ilprocessor.Emit(OpCodes.Stobj, ilprocessor.Body.Method.Module.ImportReference(methodInfo2.ReturnType));
								ilprocessor.Emit(OpCodes.Ret);
								methodInfo = dynamicMethodDefinition.Generate();
							}
						}
					}
				}
			}
			return methodInfo ?? to;
		}

		// Token: 0x04001183 RID: 4483
		protected Dictionary<MethodBase, DetourRuntimeILPlatform.MethodPin> PinnedMethods = new Dictionary<MethodBase, DetourRuntimeILPlatform.MethodPin>();

		// Token: 0x04001184 RID: 4484
		private readonly DetourRuntimeILPlatform.GlueThiscallStructRetPtrOrder GlueThiscallStructRetPtr;

		// Token: 0x0200035D RID: 861
		private struct _SelftestStruct
		{
			// Token: 0x04001185 RID: 4485
			private readonly byte A;

			// Token: 0x04001186 RID: 4486
			private readonly byte B;

			// Token: 0x04001187 RID: 4487
			private readonly byte C;
		}

		// Token: 0x0200035E RID: 862
		protected class MethodPin
		{
			// Token: 0x04001188 RID: 4488
			public int Count;

			// Token: 0x04001189 RID: 4489
			public RuntimeMethodHandle Handle;
		}

		// Token: 0x0200035F RID: 863
		private enum GlueThiscallStructRetPtrOrder
		{
			// Token: 0x0400118B RID: 4491
			Original,
			// Token: 0x0400118C RID: 4492
			ThisRetArgs,
			// Token: 0x0400118D RID: 4493
			RetThisArgs
		}
	}
}
