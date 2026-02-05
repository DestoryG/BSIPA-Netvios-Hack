using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace HarmonyLib
{
	// Token: 0x0200002F RID: 47
	internal static class PatchFunctions
	{
		// Token: 0x060000F9 RID: 249 RVA: 0x00007F68 File Offset: 0x00006168
		internal static void AddPrefix(PatchInfo patchInfo, string owner, HarmonyMethod info)
		{
			if (info == null || info.method == null)
			{
				return;
			}
			int num = ((info.priority == -1) ? 400 : info.priority);
			string[] array = info.before ?? new string[0];
			string[] array2 = info.after ?? new string[0];
			bool valueOrDefault = info.debug.GetValueOrDefault();
			patchInfo.AddPrefix(info.method, owner, num, array, array2, valueOrDefault);
		}

		// Token: 0x060000FA RID: 250 RVA: 0x00007FDD File Offset: 0x000061DD
		internal static void RemovePrefix(PatchInfo patchInfo, string owner)
		{
			patchInfo.RemovePrefix(owner);
		}

		// Token: 0x060000FB RID: 251 RVA: 0x00007FE8 File Offset: 0x000061E8
		internal static void AddPostfix(PatchInfo patchInfo, string owner, HarmonyMethod info)
		{
			if (info == null || info.method == null)
			{
				return;
			}
			int num = ((info.priority == -1) ? 400 : info.priority);
			string[] array = info.before ?? new string[0];
			string[] array2 = info.after ?? new string[0];
			bool valueOrDefault = info.debug.GetValueOrDefault();
			patchInfo.AddPostfix(info.method, owner, num, array, array2, valueOrDefault);
		}

		// Token: 0x060000FC RID: 252 RVA: 0x0000805D File Offset: 0x0000625D
		internal static void RemovePostfix(PatchInfo patchInfo, string owner)
		{
			patchInfo.RemovePostfix(owner);
		}

		// Token: 0x060000FD RID: 253 RVA: 0x00008068 File Offset: 0x00006268
		internal static void AddTranspiler(PatchInfo patchInfo, string owner, HarmonyMethod info)
		{
			if (info == null || info.method == null)
			{
				return;
			}
			int num = ((info.priority == -1) ? 400 : info.priority);
			string[] array = info.before ?? new string[0];
			string[] array2 = info.after ?? new string[0];
			bool valueOrDefault = info.debug.GetValueOrDefault();
			patchInfo.AddTranspiler(info.method, owner, num, array, array2, valueOrDefault);
		}

		// Token: 0x060000FE RID: 254 RVA: 0x000080DD File Offset: 0x000062DD
		internal static void RemoveTranspiler(PatchInfo patchInfo, string owner)
		{
			patchInfo.RemoveTranspiler(owner);
		}

		// Token: 0x060000FF RID: 255 RVA: 0x000080E8 File Offset: 0x000062E8
		internal static void AddFinalizer(PatchInfo patchInfo, string owner, HarmonyMethod info)
		{
			if (info == null || info.method == null)
			{
				return;
			}
			int num = ((info.priority == -1) ? 400 : info.priority);
			string[] array = info.before ?? new string[0];
			string[] array2 = info.after ?? new string[0];
			bool valueOrDefault = info.debug.GetValueOrDefault();
			patchInfo.AddFinalizer(info.method, owner, num, array, array2, valueOrDefault);
		}

		// Token: 0x06000100 RID: 256 RVA: 0x0000815D File Offset: 0x0000635D
		internal static void RemoveFinalizer(PatchInfo patchInfo, string owner)
		{
			patchInfo.RemoveFinalizer(owner);
		}

		// Token: 0x06000101 RID: 257 RVA: 0x00008166 File Offset: 0x00006366
		internal static void RemovePatch(PatchInfo patchInfo, MethodInfo patch)
		{
			patchInfo.RemovePatch(patch);
		}

		// Token: 0x06000102 RID: 258 RVA: 0x0000816F File Offset: 0x0000636F
		internal static List<MethodInfo> GetSortedPatchMethods(MethodBase original, Patch[] patches, bool debug)
		{
			return new PatchSorter(patches, debug).Sort(original);
		}

		// Token: 0x06000103 RID: 259 RVA: 0x00008180 File Offset: 0x00006380
		internal static MethodInfo UpdateWrapper(MethodBase original, PatchInfo patchInfo)
		{
			bool flag = patchInfo.Debugging || Harmony.DEBUG;
			List<MethodInfo> sortedPatchMethods = PatchFunctions.GetSortedPatchMethods(original, patchInfo.prefixes, flag);
			List<MethodInfo> sortedPatchMethods2 = PatchFunctions.GetSortedPatchMethods(original, patchInfo.postfixes, flag);
			List<MethodInfo> sortedPatchMethods3 = PatchFunctions.GetSortedPatchMethods(original, patchInfo.transpilers, flag);
			List<MethodInfo> sortedPatchMethods4 = PatchFunctions.GetSortedPatchMethods(original, patchInfo.finalizers, flag);
			Dictionary<int, CodeInstruction> dictionary;
			MethodInfo methodInfo = new MethodPatcher(original, null, sortedPatchMethods, sortedPatchMethods2, sortedPatchMethods3, sortedPatchMethods4, flag).CreateReplacement(out dictionary);
			if (methodInfo == null)
			{
				throw new MissingMethodException("Cannot create replacement for " + original.FullDescription());
			}
			try
			{
				Memory.DetourMethodAndPersist(original, methodInfo);
			}
			catch (Exception ex)
			{
				throw HarmonyException.Create(ex, dictionary);
			}
			return methodInfo;
		}

		// Token: 0x06000104 RID: 260 RVA: 0x00008234 File Offset: 0x00006434
		internal static MethodInfo ReversePatch(HarmonyMethod standin, MethodBase original, MethodInfo postTranspiler)
		{
			if (standin == null)
			{
				throw new ArgumentNullException("standin");
			}
			if (standin.method == null)
			{
				throw new ArgumentNullException("standin.method");
			}
			bool flag = standin.debug.GetValueOrDefault() || Harmony.DEBUG;
			List<MethodInfo> list = new List<MethodInfo>();
			HarmonyReversePatchType? reversePatchType = standin.reversePatchType;
			HarmonyReversePatchType harmonyReversePatchType = HarmonyReversePatchType.Snapshot;
			if ((reversePatchType.GetValueOrDefault() == harmonyReversePatchType) & (reversePatchType != null))
			{
				Patches patchInfo = Harmony.GetPatchInfo(original);
				list.AddRange(PatchFunctions.GetSortedPatchMethods(original, patchInfo.Transpilers.ToArray<Patch>(), flag));
			}
			if (postTranspiler != null)
			{
				list.Add(postTranspiler);
			}
			List<MethodInfo> list2 = new List<MethodInfo>();
			Dictionary<int, CodeInstruction> dictionary;
			MethodInfo methodInfo = new MethodPatcher(standin.method, original, list2, list2, list, list2, flag).CreateReplacement(out dictionary);
			if (methodInfo == null)
			{
				throw new MissingMethodException("Cannot create replacement for " + standin.method.FullDescription());
			}
			try
			{
				string text = Memory.DetourMethod(standin.method, methodInfo);
				if (text != null)
				{
					throw new FormatException("Method " + standin.method.FullDescription() + " cannot be patched. Reason: " + text);
				}
			}
			catch (Exception ex)
			{
				throw HarmonyException.Create(ex, dictionary);
			}
			PatchTools.RememberObject(standin.method, methodInfo);
			return methodInfo;
		}
	}
}
