using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using MonoMod.Utils;

namespace HarmonyLib
{
	// Token: 0x0200007D RID: 125
	public class PatchProcessor
	{
		// Token: 0x0600023B RID: 571 RVA: 0x0000C299 File Offset: 0x0000A499
		public PatchProcessor(Harmony instance, MethodBase original)
		{
			this.instance = instance;
			this.original = original;
		}

		// Token: 0x0600023C RID: 572 RVA: 0x0000C2AF File Offset: 0x0000A4AF
		public PatchProcessor AddPrefix(HarmonyMethod prefix)
		{
			this.prefix = prefix;
			return this;
		}

		// Token: 0x0600023D RID: 573 RVA: 0x0000C2B9 File Offset: 0x0000A4B9
		public PatchProcessor AddPrefix(MethodInfo fixMethod)
		{
			this.prefix = new HarmonyMethod(fixMethod);
			return this;
		}

		// Token: 0x0600023E RID: 574 RVA: 0x0000C2C8 File Offset: 0x0000A4C8
		public PatchProcessor AddPostfix(HarmonyMethod postfix)
		{
			this.postfix = postfix;
			return this;
		}

		// Token: 0x0600023F RID: 575 RVA: 0x0000C2D2 File Offset: 0x0000A4D2
		public PatchProcessor AddPostfix(MethodInfo fixMethod)
		{
			this.postfix = new HarmonyMethod(fixMethod);
			return this;
		}

		// Token: 0x06000240 RID: 576 RVA: 0x0000C2E1 File Offset: 0x0000A4E1
		public PatchProcessor AddTranspiler(HarmonyMethod transpiler)
		{
			this.transpiler = transpiler;
			return this;
		}

		// Token: 0x06000241 RID: 577 RVA: 0x0000C2EB File Offset: 0x0000A4EB
		public PatchProcessor AddTranspiler(MethodInfo fixMethod)
		{
			this.transpiler = new HarmonyMethod(fixMethod);
			return this;
		}

		// Token: 0x06000242 RID: 578 RVA: 0x0000C2FA File Offset: 0x0000A4FA
		public PatchProcessor AddFinalizer(HarmonyMethod finalizer)
		{
			this.finalizer = finalizer;
			return this;
		}

		// Token: 0x06000243 RID: 579 RVA: 0x0000C304 File Offset: 0x0000A504
		public PatchProcessor AddFinalizer(MethodInfo fixMethod)
		{
			this.finalizer = new HarmonyMethod(fixMethod);
			return this;
		}

		// Token: 0x06000244 RID: 580 RVA: 0x0000C314 File Offset: 0x0000A514
		public static IEnumerable<MethodBase> GetAllPatchedMethods()
		{
			object obj = PatchProcessor.locker;
			IEnumerable<MethodBase> patchedMethods;
			lock (obj)
			{
				patchedMethods = HarmonySharedState.GetPatchedMethods();
			}
			return patchedMethods;
		}

		// Token: 0x06000245 RID: 581 RVA: 0x0000C354 File Offset: 0x0000A554
		public MethodInfo Patch()
		{
			if (this.original == null)
			{
				throw new NullReferenceException("Null method for " + this.instance.Id);
			}
			if (!this.original.IsDeclaredMember<MethodBase>())
			{
				MethodBase declaredMember = this.original.GetDeclaredMember<MethodBase>();
				throw new ArgumentException("You can only patch implemented methods/constructors. Path the declared method " + declaredMember.FullDescription() + " instead.");
			}
			object obj = PatchProcessor.locker;
			MethodInfo methodInfo2;
			lock (obj)
			{
				PatchInfo patchInfo = HarmonySharedState.GetPatchInfo(this.original);
				if (patchInfo == null)
				{
					patchInfo = new PatchInfo();
				}
				PatchFunctions.AddPrefix(patchInfo, this.instance.Id, this.prefix);
				PatchFunctions.AddPostfix(patchInfo, this.instance.Id, this.postfix);
				PatchFunctions.AddTranspiler(patchInfo, this.instance.Id, this.transpiler);
				PatchFunctions.AddFinalizer(patchInfo, this.instance.Id, this.finalizer);
				MethodInfo methodInfo = PatchFunctions.UpdateWrapper(this.original, patchInfo);
				HarmonySharedState.UpdatePatchInfo(this.original, patchInfo);
				methodInfo2 = methodInfo;
			}
			return methodInfo2;
		}

		// Token: 0x06000246 RID: 582 RVA: 0x0000C478 File Offset: 0x0000A678
		public PatchProcessor Unpatch(HarmonyPatchType type, string harmonyID)
		{
			object obj = PatchProcessor.locker;
			lock (obj)
			{
				PatchInfo patchInfo = HarmonySharedState.GetPatchInfo(this.original);
				if (patchInfo == null)
				{
					patchInfo = new PatchInfo();
				}
				if (type == HarmonyPatchType.All || type == HarmonyPatchType.Prefix)
				{
					PatchFunctions.RemovePrefix(patchInfo, harmonyID);
				}
				if (type == HarmonyPatchType.All || type == HarmonyPatchType.Postfix)
				{
					PatchFunctions.RemovePostfix(patchInfo, harmonyID);
				}
				if (type == HarmonyPatchType.All || type == HarmonyPatchType.Transpiler)
				{
					PatchFunctions.RemoveTranspiler(patchInfo, harmonyID);
				}
				if (type == HarmonyPatchType.All || type == HarmonyPatchType.Finalizer)
				{
					PatchFunctions.RemoveFinalizer(patchInfo, harmonyID);
				}
				PatchFunctions.UpdateWrapper(this.original, patchInfo);
				HarmonySharedState.UpdatePatchInfo(this.original, patchInfo);
			}
			return this;
		}

		// Token: 0x06000247 RID: 583 RVA: 0x0000C51C File Offset: 0x0000A71C
		public PatchProcessor Unpatch(MethodInfo patch)
		{
			object obj = PatchProcessor.locker;
			lock (obj)
			{
				PatchInfo patchInfo = HarmonySharedState.GetPatchInfo(this.original);
				if (patchInfo == null)
				{
					patchInfo = new PatchInfo();
				}
				PatchFunctions.RemovePatch(patchInfo, patch);
				PatchFunctions.UpdateWrapper(this.original, patchInfo);
				HarmonySharedState.UpdatePatchInfo(this.original, patchInfo);
			}
			return this;
		}

		// Token: 0x06000248 RID: 584 RVA: 0x0000C590 File Offset: 0x0000A790
		public static Patches GetPatchInfo(MethodBase method)
		{
			object obj = PatchProcessor.locker;
			PatchInfo patchInfo;
			lock (obj)
			{
				patchInfo = HarmonySharedState.GetPatchInfo(method);
			}
			if (patchInfo == null)
			{
				return null;
			}
			return new Patches(patchInfo.prefixes, patchInfo.postfixes, patchInfo.transpilers, patchInfo.finalizers);
		}

		// Token: 0x06000249 RID: 585 RVA: 0x0000C5F4 File Offset: 0x0000A7F4
		public static Dictionary<string, Version> VersionInfo(out Version currentVersion)
		{
			currentVersion = typeof(Harmony).Assembly.GetName().Version;
			Dictionary<string, Assembly> assemblies = new Dictionary<string, Assembly>();
			Action<Patch> <>9__2;
			Action<Patch> <>9__3;
			Action<Patch> <>9__4;
			Action<Patch> <>9__5;
			PatchProcessor.GetAllPatchedMethods().Do(delegate(MethodBase method)
			{
				object obj = PatchProcessor.locker;
				PatchInfo patchInfo;
				lock (obj)
				{
					patchInfo = HarmonySharedState.GetPatchInfo(method);
				}
				IEnumerable<Patch> prefixes = patchInfo.prefixes;
				Action<Patch> action;
				if ((action = <>9__2) == null)
				{
					action = (<>9__2 = delegate(Patch fix)
					{
						assemblies[fix.owner] = fix.PatchMethod.DeclaringType.Assembly;
					});
				}
				prefixes.Do(action);
				IEnumerable<Patch> postfixes = patchInfo.postfixes;
				Action<Patch> action2;
				if ((action2 = <>9__3) == null)
				{
					action2 = (<>9__3 = delegate(Patch fix)
					{
						assemblies[fix.owner] = fix.PatchMethod.DeclaringType.Assembly;
					});
				}
				postfixes.Do(action2);
				IEnumerable<Patch> transpilers = patchInfo.transpilers;
				Action<Patch> action3;
				if ((action3 = <>9__4) == null)
				{
					action3 = (<>9__4 = delegate(Patch fix)
					{
						assemblies[fix.owner] = fix.PatchMethod.DeclaringType.Assembly;
					});
				}
				transpilers.Do(action3);
				IEnumerable<Patch> finalizers = patchInfo.finalizers;
				Action<Patch> action4;
				if ((action4 = <>9__5) == null)
				{
					action4 = (<>9__5 = delegate(Patch fix)
					{
						assemblies[fix.owner] = fix.PatchMethod.DeclaringType.Assembly;
					});
				}
				finalizers.Do(action4);
			});
			Dictionary<string, Version> result = new Dictionary<string, Version>();
			assemblies.Do(delegate(KeyValuePair<string, Assembly> info)
			{
				AssemblyName assemblyName = info.Value.GetReferencedAssemblies().FirstOrDefault((AssemblyName a) => a.FullName.StartsWith("0Harmony, Version", StringComparison.Ordinal));
				if (assemblyName != null)
				{
					result[info.Key] = assemblyName.Version;
				}
			});
			return result;
		}

		// Token: 0x0600024A RID: 586 RVA: 0x0000C66C File Offset: 0x0000A86C
		public static List<CodeInstruction> GetOriginalInstructions(MethodBase original, ILGenerator generator = null)
		{
			if (generator == null)
			{
				generator = new DynamicMethodDefinition(string.Format("{0}_Copy{1}", original.Name, Guid.NewGuid()), typeof(void), new Type[0]).GetILGenerator();
			}
			return (from ins in MethodBodyReader.GetInstructions(generator, original)
				select ins.GetCodeInstruction()).ToList<CodeInstruction>();
		}

		// Token: 0x0600024B RID: 587 RVA: 0x0000C6E4 File Offset: 0x0000A8E4
		public static List<CodeInstruction> GetOriginalInstructions(MethodBase original, out ILGenerator generator)
		{
			DynamicMethodDefinition dynamicMethodDefinition = new DynamicMethodDefinition(string.Format("{0}_Dummy{1}", original.Name, Guid.NewGuid()), typeof(void), new Type[0]);
			generator = dynamicMethodDefinition.GetILGenerator();
			return (from ins in MethodBodyReader.GetInstructions(generator, original)
				select ins.GetCodeInstruction()).ToList<CodeInstruction>();
		}

		// Token: 0x0600024C RID: 588 RVA: 0x0000C75C File Offset: 0x0000A95C
		public static IEnumerable<KeyValuePair<OpCode, object>> ReadMethodBody(MethodBase method)
		{
			return from instr in MethodBodyReader.GetInstructions(new DynamicMethodDefinition(string.Format("{0}_Dummy{1}", method.Name, Guid.NewGuid()), typeof(void), new Type[0]).GetILGenerator(), method)
				select new KeyValuePair<OpCode, object>(instr.opcode, instr.operand);
		}

		// Token: 0x04000163 RID: 355
		private readonly Harmony instance;

		// Token: 0x04000164 RID: 356
		private readonly MethodBase original;

		// Token: 0x04000165 RID: 357
		private HarmonyMethod prefix;

		// Token: 0x04000166 RID: 358
		private HarmonyMethod postfix;

		// Token: 0x04000167 RID: 359
		private HarmonyMethod transpiler;

		// Token: 0x04000168 RID: 360
		private HarmonyMethod finalizer;

		// Token: 0x04000169 RID: 361
		internal static readonly object locker = new object();
	}
}
