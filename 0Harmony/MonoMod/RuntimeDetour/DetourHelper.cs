using System;
using System.Linq.Expressions;
using System.Reflection;
using Mono.Cecil;
using Mono.Cecil.Cil;
using MonoMod.RuntimeDetour.Platforms;
using MonoMod.Utils;

namespace MonoMod.RuntimeDetour
{
	// Token: 0x02000349 RID: 841
	internal static class DetourHelper
	{
		// Token: 0x17000397 RID: 919
		// (get) Token: 0x0600138F RID: 5007 RVA: 0x00047134 File Offset: 0x00045334
		// (set) Token: 0x06001390 RID: 5008 RVA: 0x000471EC File Offset: 0x000453EC
		public static IDetourRuntimePlatform Runtime
		{
			get
			{
				if (DetourHelper._Runtime != null)
				{
					return DetourHelper._Runtime;
				}
				object runtimeLock = DetourHelper._RuntimeLock;
				IDetourRuntimePlatform detourRuntimePlatform;
				lock (runtimeLock)
				{
					if (DetourHelper._Runtime != null)
					{
						detourRuntimePlatform = DetourHelper._Runtime;
					}
					else
					{
						if (Type.GetType("Mono.Runtime") != null)
						{
							DetourHelper._Runtime = new DetourRuntimeMonoPlatform();
						}
						else if (typeof(object).Assembly.GetName().Name == "System.Private.CoreLib")
						{
							DetourHelper._Runtime = new DetourRuntimeNETCorePlatform();
						}
						else
						{
							DetourHelper._Runtime = new DetourRuntimeNETPlatform();
						}
						detourRuntimePlatform = DetourHelper._Runtime;
					}
				}
				return detourRuntimePlatform;
			}
			set
			{
				DetourHelper._Runtime = value;
			}
		}

		// Token: 0x17000398 RID: 920
		// (get) Token: 0x06001391 RID: 5009 RVA: 0x000471F4 File Offset: 0x000453F4
		// (set) Token: 0x06001392 RID: 5010 RVA: 0x00047318 File Offset: 0x00045518
		public static IDetourNativePlatform Native
		{
			get
			{
				if (DetourHelper._Native != null)
				{
					return DetourHelper._Native;
				}
				object nativeLock = DetourHelper._NativeLock;
				IDetourNativePlatform detourNativePlatform;
				lock (nativeLock)
				{
					if (DetourHelper._Native != null)
					{
						detourNativePlatform = DetourHelper._Native;
					}
					else
					{
						if (PlatformHelper.Is(Platform.ARM))
						{
							DetourHelper._Native = new DetourNativeARMPlatform();
						}
						else
						{
							DetourHelper._Native = new DetourNativeX86Platform();
						}
						if (PlatformHelper.Is(Platform.Windows))
						{
							detourNativePlatform = (DetourHelper._Native = new DetourNativeWindowsPlatform(DetourHelper._Native));
						}
						else
						{
							if (Type.GetType("Mono.Runtime") != null)
							{
								try
								{
									return DetourHelper._Native = new DetourNativeMonoPlatform(DetourHelper._Native, "libmonosgen-2.0." + PlatformHelper.LibrarySuffix);
								}
								catch
								{
								}
							}
							try
							{
								DetourHelper._Native = new DetourNativeMonoPosixPlatform(DetourHelper._Native);
							}
							catch
							{
							}
							try
							{
								DetourHelper._Native = new DetourNativeLibcPlatform(DetourHelper._Native);
							}
							catch
							{
							}
							detourNativePlatform = DetourHelper._Native;
						}
					}
				}
				return detourNativePlatform;
			}
			set
			{
				DetourHelper._Native = value;
			}
		}

		// Token: 0x06001393 RID: 5011 RVA: 0x00047320 File Offset: 0x00045520
		public static void MakeWritable(this IDetourNativePlatform plat, NativeDetourData detour)
		{
			plat.MakeWritable(detour.Method, detour.Size);
		}

		// Token: 0x06001394 RID: 5012 RVA: 0x00047334 File Offset: 0x00045534
		public static void MakeExecutable(this IDetourNativePlatform plat, NativeDetourData detour)
		{
			plat.MakeExecutable(detour.Method, detour.Size);
		}

		// Token: 0x06001395 RID: 5013 RVA: 0x00047348 File Offset: 0x00045548
		public static void FlushICache(this IDetourNativePlatform plat, NativeDetourData detour)
		{
			plat.FlushICache(detour.Method, detour.Size);
		}

		// Token: 0x06001396 RID: 5014 RVA: 0x0004735C File Offset: 0x0004555C
		public unsafe static void Write(this IntPtr to, ref int offs, byte value)
		{
			*(UIntPtr)((long)to + (long)offs) = value;
			offs++;
		}

		// Token: 0x06001397 RID: 5015 RVA: 0x00047371 File Offset: 0x00045571
		public unsafe static void Write(this IntPtr to, ref int offs, ushort value)
		{
			*(UIntPtr)((long)to + (long)offs) = (short)value;
			offs += 2;
		}

		// Token: 0x06001398 RID: 5016 RVA: 0x00047386 File Offset: 0x00045586
		public unsafe static void Write(this IntPtr to, ref int offs, uint value)
		{
			*(UIntPtr)((long)to + (long)offs) = (int)value;
			offs += 4;
		}

		// Token: 0x06001399 RID: 5017 RVA: 0x0004739B File Offset: 0x0004559B
		public unsafe static void Write(this IntPtr to, ref int offs, ulong value)
		{
			*(UIntPtr)((long)to + (long)offs) = (long)value;
			offs += 8;
		}

		// Token: 0x0600139A RID: 5018 RVA: 0x000473B0 File Offset: 0x000455B0
		public static IntPtr GetNativeStart(this MethodBase method)
		{
			return DetourHelper.Runtime.GetNativeStart(method);
		}

		// Token: 0x0600139B RID: 5019 RVA: 0x000473BD File Offset: 0x000455BD
		public static IntPtr GetNativeStart(this Delegate method)
		{
			return method.Method.GetNativeStart();
		}

		// Token: 0x0600139C RID: 5020 RVA: 0x000473CA File Offset: 0x000455CA
		public static IntPtr GetNativeStart(this Expression method)
		{
			return ((MethodCallExpression)method).Method.GetNativeStart();
		}

		// Token: 0x0600139D RID: 5021 RVA: 0x000473DC File Offset: 0x000455DC
		public static MethodInfo CreateILCopy(this MethodBase method)
		{
			return DetourHelper.Runtime.CreateCopy(method);
		}

		// Token: 0x0600139E RID: 5022 RVA: 0x000473E9 File Offset: 0x000455E9
		public static bool TryCreateILCopy(this MethodBase method, out MethodInfo dm)
		{
			return DetourHelper.Runtime.TryCreateCopy(method, out dm);
		}

		// Token: 0x0600139F RID: 5023 RVA: 0x000473F7 File Offset: 0x000455F7
		public static T Pin<T>(this T method) where T : MethodBase
		{
			DetourHelper.Runtime.Pin(method);
			return method;
		}

		// Token: 0x060013A0 RID: 5024 RVA: 0x0004740A File Offset: 0x0004560A
		public static T Unpin<T>(this T method) where T : MethodBase
		{
			DetourHelper.Runtime.Unpin(method);
			return method;
		}

		// Token: 0x060013A1 RID: 5025 RVA: 0x00047420 File Offset: 0x00045620
		public static MethodInfo GenerateNativeProxy(IntPtr target, MethodBase signature)
		{
			MethodInfo methodInfo = signature as MethodInfo;
			Type type = ((methodInfo != null) ? methodInfo.ReturnType : null) ?? typeof(void);
			ParameterInfo[] parameters = signature.GetParameters();
			Type[] array = new Type[parameters.Length];
			for (int i = 0; i < parameters.Length; i++)
			{
				array[i] = parameters[i].ParameterType;
			}
			MethodInfo methodInfo2;
			using (DynamicMethodDefinition dynamicMethodDefinition = new DynamicMethodDefinition("Native<" + ((long)target).ToString("X16") + ">", type, array))
			{
				methodInfo2 = dynamicMethodDefinition.StubCriticalDetour().Generate().Pin<MethodInfo>();
			}
			NativeDetourData nativeDetourData = DetourHelper.Native.Create(methodInfo2.GetNativeStart(), target, null);
			DetourHelper.Native.MakeWritable(nativeDetourData);
			DetourHelper.Native.Apply(nativeDetourData);
			DetourHelper.Native.MakeExecutable(nativeDetourData);
			DetourHelper.Native.FlushICache(nativeDetourData);
			DetourHelper.Native.Free(nativeDetourData);
			return methodInfo2;
		}

		// Token: 0x060013A2 RID: 5026 RVA: 0x00047534 File Offset: 0x00045734
		private static NativeDetourData ToNativeDetourData(IntPtr method, IntPtr target, uint size, byte type, IntPtr extra)
		{
			return new NativeDetourData
			{
				Method = method,
				Target = target,
				Size = size,
				Type = type,
				Extra = extra
			};
		}

		// Token: 0x060013A3 RID: 5027 RVA: 0x00047574 File Offset: 0x00045774
		public static DynamicMethodDefinition StubCriticalDetour(this DynamicMethodDefinition dm)
		{
			ILProcessor ilprocessor = dm.GetILProcessor();
			ModuleDefinition module = ilprocessor.Body.Method.Module;
			for (int i = 0; i < 32; i++)
			{
				ilprocessor.Emit(OpCodes.Nop);
			}
			ilprocessor.Emit(OpCodes.Ldstr, dm.Definition.Name + " should've been detoured!");
			ilprocessor.Emit(OpCodes.Newobj, module.ImportReference(DetourHelper._ctor_Exception));
			ilprocessor.Emit(OpCodes.Throw);
			return dm;
		}

		// Token: 0x060013A4 RID: 5028 RVA: 0x000475F4 File Offset: 0x000457F4
		public static void EmitDetourCopy(this ILProcessor il, IntPtr src, IntPtr dst, byte type)
		{
			ModuleDefinition module = il.Body.Method.Module;
			il.Emit(OpCodes.Ldsfld, module.ImportReference(DetourHelper._f_Native));
			il.Emit(OpCodes.Ldc_I8, (long)src);
			il.Emit(OpCodes.Conv_I);
			il.Emit(OpCodes.Ldc_I8, (long)dst);
			il.Emit(OpCodes.Conv_I);
			il.Emit(OpCodes.Ldc_I4, (int)type);
			il.Emit(OpCodes.Conv_U1);
			il.Emit(OpCodes.Callvirt, module.ImportReference(DetourHelper._m_Copy));
		}

		// Token: 0x060013A5 RID: 5029 RVA: 0x00047690 File Offset: 0x00045890
		public static void EmitDetourApply(this ILProcessor il, NativeDetourData data)
		{
			ModuleDefinition module = il.Body.Method.Module;
			il.Emit(OpCodes.Ldsfld, module.ImportReference(DetourHelper._f_Native));
			il.Emit(OpCodes.Ldc_I8, (long)data.Method);
			il.Emit(OpCodes.Conv_I);
			il.Emit(OpCodes.Ldc_I8, (long)data.Target);
			il.Emit(OpCodes.Conv_I);
			il.Emit(OpCodes.Ldc_I4, (int)data.Size);
			il.Emit(OpCodes.Ldc_I4, (int)data.Type);
			il.Emit(OpCodes.Conv_U1);
			il.Emit(OpCodes.Ldc_I8, (long)data.Extra);
			il.Emit(OpCodes.Conv_I);
			il.Emit(OpCodes.Call, module.ImportReference(DetourHelper._m_ToNativeDetourData));
			il.Emit(OpCodes.Callvirt, module.ImportReference(DetourHelper._m_Apply));
		}

		// Token: 0x04000FDF RID: 4063
		private static readonly object _RuntimeLock = new object();

		// Token: 0x04000FE0 RID: 4064
		private static IDetourRuntimePlatform _Runtime;

		// Token: 0x04000FE1 RID: 4065
		private static readonly object _NativeLock = new object();

		// Token: 0x04000FE2 RID: 4066
		private static IDetourNativePlatform _Native;

		// Token: 0x04000FE3 RID: 4067
		private static readonly FieldInfo _f_Native = typeof(DetourHelper).GetField("_Native", BindingFlags.Static | BindingFlags.NonPublic);

		// Token: 0x04000FE4 RID: 4068
		private static readonly MethodInfo _m_ToNativeDetourData = typeof(DetourHelper).GetMethod("ToNativeDetourData", BindingFlags.Static | BindingFlags.NonPublic);

		// Token: 0x04000FE5 RID: 4069
		private static readonly MethodInfo _m_Copy = typeof(IDetourNativePlatform).GetMethod("Copy");

		// Token: 0x04000FE6 RID: 4070
		private static readonly MethodInfo _m_Apply = typeof(IDetourNativePlatform).GetMethod("Apply");

		// Token: 0x04000FE7 RID: 4071
		private static readonly ConstructorInfo _ctor_Exception = typeof(Exception).GetConstructor(new Type[] { typeof(string) });
	}
}
