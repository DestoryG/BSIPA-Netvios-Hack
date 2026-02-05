using System;
using System.Reflection;
using MonoMod.RuntimeDetour;

namespace HarmonyLib
{
	// Token: 0x0200001F RID: 31
	public static class Memory
	{
		// Token: 0x060000A6 RID: 166 RVA: 0x00005088 File Offset: 0x00003288
		public unsafe static void MarkForNoInlining(MethodBase method)
		{
			if (AccessTools.IsMonoRuntime)
			{
				byte* ptr = (byte*)(void*)method.MethodHandle.Value + 2;
				*(short*)ptr = (short)(*(ushort*)ptr | 8);
			}
		}

		// Token: 0x060000A7 RID: 167 RVA: 0x000050B8 File Offset: 0x000032B8
		public static string DetourMethod(MethodBase original, MethodBase replacement)
		{
			Exception ex;
			long methodStart = Memory.GetMethodStart(original, out ex);
			if (methodStart == 0L)
			{
				return ex.Message;
			}
			long methodStart2 = Memory.GetMethodStart(replacement, out ex);
			if (methodStart2 == 0L)
			{
				return ex.Message;
			}
			return Memory.WriteJump(methodStart, methodStart2);
		}

		// Token: 0x060000A8 RID: 168 RVA: 0x000050F4 File Offset: 0x000032F4
		internal static void DetourMethodAndPersist(MethodBase original, MethodBase replacement)
		{
			string text = Memory.DetourMethod(original, replacement);
			if (text != null)
			{
				throw new FormatException("Method " + original.FullDescription() + " cannot be patched. Reason: " + text);
			}
			PatchTools.RememberObject(original, replacement);
		}

		// Token: 0x060000A9 RID: 169 RVA: 0x00005130 File Offset: 0x00003330
		public static string WriteJump(long memory, long destination)
		{
			NativeDetourData nativeDetourData = DetourHelper.Native.Create((IntPtr)memory, (IntPtr)destination, null);
			DetourHelper.Native.MakeWritable(nativeDetourData);
			DetourHelper.Native.Apply(nativeDetourData);
			DetourHelper.Native.MakeExecutable(nativeDetourData);
			DetourHelper.Native.FlushICache(nativeDetourData);
			DetourHelper.Native.Free(nativeDetourData);
			return null;
		}

		// Token: 0x060000AA RID: 170 RVA: 0x00005198 File Offset: 0x00003398
		public static long GetMethodStart(MethodBase method, out Exception exception)
		{
			long num;
			try
			{
				exception = null;
				num = method.Pin<MethodBase>().GetNativeStart().ToInt64();
			}
			catch (Exception ex)
			{
				exception = ex;
				num = 0L;
			}
			return num;
		}
	}
}
