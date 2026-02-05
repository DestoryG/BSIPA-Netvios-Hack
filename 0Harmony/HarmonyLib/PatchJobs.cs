using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace HarmonyLib
{
	// Token: 0x02000030 RID: 48
	internal class PatchJobs<T>
	{
		// Token: 0x06000105 RID: 261 RVA: 0x00008374 File Offset: 0x00006574
		internal PatchJobs<T>.Job GetJob(MethodBase method)
		{
			if (method == null)
			{
				return null;
			}
			PatchJobs<T>.Job job;
			if (!this.state.TryGetValue(method, out job))
			{
				job = new PatchJobs<T>.Job
				{
					original = method
				};
				this.state[method] = job;
			}
			return job;
		}

		// Token: 0x06000106 RID: 262 RVA: 0x000083B7 File Offset: 0x000065B7
		internal List<PatchJobs<T>.Job> GetJobs()
		{
			return this.state.Values.Where((PatchJobs<T>.Job job) => job.prefixes.Count + job.postfixes.Count + job.transpilers.Count + job.finalizers.Count > 0).ToList<PatchJobs<T>.Job>();
		}

		// Token: 0x06000107 RID: 263 RVA: 0x000083ED File Offset: 0x000065ED
		internal List<T> GetReplacements()
		{
			return this.state.Values.Select((PatchJobs<T>.Job job) => job.replacement).ToList<T>();
		}

		// Token: 0x040000A4 RID: 164
		internal Dictionary<MethodBase, PatchJobs<T>.Job> state = new Dictionary<MethodBase, PatchJobs<T>.Job>();

		// Token: 0x02000031 RID: 49
		internal class Job
		{
			// Token: 0x06000109 RID: 265 RVA: 0x00008438 File Offset: 0x00006638
			internal void AddPatch(AttributePatch patch)
			{
				HarmonyPatchType? type = patch.type;
				if (type != null)
				{
					switch (type.GetValueOrDefault())
					{
					case HarmonyPatchType.Prefix:
						this.prefixes.Add(patch.info);
						return;
					case HarmonyPatchType.Postfix:
						this.postfixes.Add(patch.info);
						return;
					case HarmonyPatchType.Transpiler:
						this.transpilers.Add(patch.info);
						return;
					case HarmonyPatchType.Finalizer:
						this.finalizers.Add(patch.info);
						break;
					default:
						return;
					}
				}
			}

			// Token: 0x040000A5 RID: 165
			internal MethodBase original;

			// Token: 0x040000A6 RID: 166
			internal T replacement;

			// Token: 0x040000A7 RID: 167
			internal List<HarmonyMethod> prefixes = new List<HarmonyMethod>();

			// Token: 0x040000A8 RID: 168
			internal List<HarmonyMethod> postfixes = new List<HarmonyMethod>();

			// Token: 0x040000A9 RID: 169
			internal List<HarmonyMethod> transpilers = new List<HarmonyMethod>();

			// Token: 0x040000AA RID: 170
			internal List<HarmonyMethod> finalizers = new List<HarmonyMethod>();
		}
	}
}
