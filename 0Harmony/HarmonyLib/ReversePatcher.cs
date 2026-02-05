using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace HarmonyLib
{
	// Token: 0x02000081 RID: 129
	public class ReversePatcher
	{
		// Token: 0x0600025B RID: 603 RVA: 0x0000C96B File Offset: 0x0000AB6B
		public ReversePatcher(Harmony instance, MethodBase original, HarmonyMethod standin)
		{
			this.instance = instance;
			this.original = original;
			this.standin = standin;
		}

		// Token: 0x0600025C RID: 604 RVA: 0x0000C988 File Offset: 0x0000AB88
		public MethodInfo Patch(HarmonyReversePatchType type = HarmonyReversePatchType.Original)
		{
			if (this.original == null)
			{
				throw new NullReferenceException("Null method for " + this.instance.Id);
			}
			MethodInfo transpiler = ReversePatcher.GetTranspiler(this.standin.method);
			return PatchFunctions.ReversePatch(this.standin, this.original, transpiler);
		}

		// Token: 0x0600025D RID: 605 RVA: 0x0000C9E4 File Offset: 0x0000ABE4
		internal static MethodInfo GetTranspiler(MethodInfo method)
		{
			string methodName = method.Name;
			IEnumerable<MethodInfo> declaredMethods = AccessTools.GetDeclaredMethods(method.DeclaringType);
			Type ici = typeof(IEnumerable<CodeInstruction>);
			return declaredMethods.FirstOrDefault((MethodInfo m) => !(m.ReturnType != ici) && m.Name.StartsWith("<" + methodName + ">"));
		}

		// Token: 0x0400017E RID: 382
		private readonly Harmony instance;

		// Token: 0x0400017F RID: 383
		private readonly MethodBase original;

		// Token: 0x04000180 RID: 384
		private readonly HarmonyMethod standin;
	}
}
