using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace HarmonyLib
{
	// Token: 0x02000036 RID: 54
	internal class PatchSorter
	{
		// Token: 0x0600011B RID: 283 RVA: 0x00008780 File Offset: 0x00006980
		internal PatchSorter(Patch[] patches, bool debug)
		{
			this.patches = patches.Select((Patch x) => new PatchSorter.PatchSortingWrapper(x)).ToList<PatchSorter.PatchSortingWrapper>();
			this.debug = debug;
			using (List<PatchSorter.PatchSortingWrapper>.Enumerator enumerator = this.patches.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					PatchSorter.PatchSortingWrapper node = enumerator.Current;
					node.AddBeforeDependency(this.patches.Where((PatchSorter.PatchSortingWrapper x) => node.innerPatch.before.Contains(x.innerPatch.owner)));
					node.AddAfterDependency(this.patches.Where((PatchSorter.PatchSortingWrapper x) => node.innerPatch.after.Contains(x.innerPatch.owner)));
				}
			}
			this.patches.Sort();
		}

		// Token: 0x0600011C RID: 284 RVA: 0x00008864 File Offset: 0x00006A64
		internal List<MethodInfo> Sort(MethodBase original)
		{
			if (this.sortedPatchArray != null)
			{
				return this.sortedPatchArray.Select((Patch x) => x.GetMethod(original)).ToList<MethodInfo>();
			}
			this.handledPatches = new HashSet<PatchSorter.PatchSortingWrapper>();
			this.waitingList = new List<PatchSorter.PatchSortingWrapper>();
			this.result = new List<PatchSorter.PatchSortingWrapper>(this.patches.Count);
			Queue<PatchSorter.PatchSortingWrapper> queue = new Queue<PatchSorter.PatchSortingWrapper>(this.patches);
			Func<PatchSorter.PatchSortingWrapper, bool> <>9__3;
			while (queue.Count != 0)
			{
				foreach (PatchSorter.PatchSortingWrapper patchSortingWrapper in queue)
				{
					IEnumerable<PatchSorter.PatchSortingWrapper> after = patchSortingWrapper.after;
					Func<PatchSorter.PatchSortingWrapper, bool> func;
					if ((func = <>9__3) == null)
					{
						func = (<>9__3 = (PatchSorter.PatchSortingWrapper x) => this.handledPatches.Contains(x));
					}
					if (after.All(func))
					{
						this.AddNodeToResult(patchSortingWrapper);
						if (patchSortingWrapper.before.Count != 0)
						{
							this.ProcessWaitingList();
						}
					}
					else
					{
						this.waitingList.Add(patchSortingWrapper);
					}
				}
				this.CullDependency();
				queue = new Queue<PatchSorter.PatchSortingWrapper>(this.waitingList);
				this.waitingList.Clear();
			}
			this.sortedPatchArray = this.result.Select((PatchSorter.PatchSortingWrapper x) => x.innerPatch).ToArray<Patch>();
			this.handledPatches = null;
			this.waitingList = null;
			this.patches = null;
			return this.sortedPatchArray.Select((Patch x) => x.GetMethod(original)).ToList<MethodInfo>();
		}

		// Token: 0x0600011D RID: 285 RVA: 0x00008A08 File Offset: 0x00006C08
		internal bool ComparePatchLists(Patch[] patches)
		{
			if (this.sortedPatchArray == null)
			{
				this.Sort(null);
			}
			return patches != null && this.sortedPatchArray.Length == patches.Length && this.sortedPatchArray.All((Patch x) => patches.Contains(x, new PatchSorter.PatchDetailedComparer()));
		}

		// Token: 0x0600011E RID: 286 RVA: 0x00008A68 File Offset: 0x00006C68
		private void CullDependency()
		{
			for (int i = this.waitingList.Count - 1; i >= 0; i--)
			{
				foreach (PatchSorter.PatchSortingWrapper patchSortingWrapper in this.waitingList[i].after)
				{
					if (!this.handledPatches.Contains(patchSortingWrapper))
					{
						this.waitingList[i].RemoveAfterDependency(patchSortingWrapper);
						if (this.debug)
						{
							string text = patchSortingWrapper.innerPatch.PatchMethod.FullDescription();
							string text2 = this.waitingList[i].innerPatch.PatchMethod.FullDescription();
							FileLog.LogBuffered("Breaking dependance between " + text + " and " + text2);
						}
						return;
					}
				}
			}
		}

		// Token: 0x0600011F RID: 287 RVA: 0x00008B4C File Offset: 0x00006D4C
		private void ProcessWaitingList()
		{
			int num = this.waitingList.Count;
			int i = 0;
			while (i < num)
			{
				PatchSorter.PatchSortingWrapper patchSortingWrapper = this.waitingList[i];
				if (patchSortingWrapper.after.All(new Func<PatchSorter.PatchSortingWrapper, bool>(this.handledPatches.Contains)))
				{
					this.waitingList.Remove(patchSortingWrapper);
					this.AddNodeToResult(patchSortingWrapper);
					num--;
					i = 0;
				}
				else
				{
					i++;
				}
			}
		}

		// Token: 0x06000120 RID: 288 RVA: 0x00008BB9 File Offset: 0x00006DB9
		private void AddNodeToResult(PatchSorter.PatchSortingWrapper node)
		{
			this.result.Add(node);
			this.handledPatches.Add(node);
		}

		// Token: 0x040000B7 RID: 183
		private List<PatchSorter.PatchSortingWrapper> patches;

		// Token: 0x040000B8 RID: 184
		private HashSet<PatchSorter.PatchSortingWrapper> handledPatches;

		// Token: 0x040000B9 RID: 185
		private List<PatchSorter.PatchSortingWrapper> result;

		// Token: 0x040000BA RID: 186
		private List<PatchSorter.PatchSortingWrapper> waitingList;

		// Token: 0x040000BB RID: 187
		internal Patch[] sortedPatchArray;

		// Token: 0x040000BC RID: 188
		private readonly bool debug;

		// Token: 0x02000037 RID: 55
		private class PatchSortingWrapper : IComparable
		{
			// Token: 0x06000121 RID: 289 RVA: 0x00008BD4 File Offset: 0x00006DD4
			internal PatchSortingWrapper(Patch patch)
			{
				this.innerPatch = patch;
				this.before = new HashSet<PatchSorter.PatchSortingWrapper>();
				this.after = new HashSet<PatchSorter.PatchSortingWrapper>();
			}

			// Token: 0x06000122 RID: 290 RVA: 0x00008BF9 File Offset: 0x00006DF9
			public int CompareTo(object obj)
			{
				PatchSorter.PatchSortingWrapper patchSortingWrapper = obj as PatchSorter.PatchSortingWrapper;
				return PatchInfoSerialization.PriorityComparer((patchSortingWrapper != null) ? patchSortingWrapper.innerPatch : null, this.innerPatch.index, this.innerPatch.priority);
			}

			// Token: 0x06000123 RID: 291 RVA: 0x00008C28 File Offset: 0x00006E28
			public override bool Equals(object obj)
			{
				PatchSorter.PatchSortingWrapper patchSortingWrapper = obj as PatchSorter.PatchSortingWrapper;
				return patchSortingWrapper != null && this.innerPatch.PatchMethod == patchSortingWrapper.innerPatch.PatchMethod;
			}

			// Token: 0x06000124 RID: 292 RVA: 0x00008C5C File Offset: 0x00006E5C
			public override int GetHashCode()
			{
				return this.innerPatch.PatchMethod.GetHashCode();
			}

			// Token: 0x06000125 RID: 293 RVA: 0x00008C70 File Offset: 0x00006E70
			internal void AddBeforeDependency(IEnumerable<PatchSorter.PatchSortingWrapper> dependencies)
			{
				foreach (PatchSorter.PatchSortingWrapper patchSortingWrapper in dependencies)
				{
					this.before.Add(patchSortingWrapper);
					patchSortingWrapper.after.Add(this);
				}
			}

			// Token: 0x06000126 RID: 294 RVA: 0x00008CCC File Offset: 0x00006ECC
			internal void AddAfterDependency(IEnumerable<PatchSorter.PatchSortingWrapper> dependencies)
			{
				foreach (PatchSorter.PatchSortingWrapper patchSortingWrapper in dependencies)
				{
					this.after.Add(patchSortingWrapper);
					patchSortingWrapper.before.Add(this);
				}
			}

			// Token: 0x06000127 RID: 295 RVA: 0x00008D28 File Offset: 0x00006F28
			internal void RemoveAfterDependency(PatchSorter.PatchSortingWrapper afterNode)
			{
				this.after.Remove(afterNode);
				afterNode.before.Remove(this);
			}

			// Token: 0x06000128 RID: 296 RVA: 0x00008D44 File Offset: 0x00006F44
			internal void RemoveBeforeDependency(PatchSorter.PatchSortingWrapper beforeNode)
			{
				this.before.Remove(beforeNode);
				beforeNode.after.Remove(this);
			}

			// Token: 0x040000BD RID: 189
			internal readonly HashSet<PatchSorter.PatchSortingWrapper> after;

			// Token: 0x040000BE RID: 190
			internal readonly HashSet<PatchSorter.PatchSortingWrapper> before;

			// Token: 0x040000BF RID: 191
			internal readonly Patch innerPatch;
		}

		// Token: 0x02000038 RID: 56
		internal class PatchDetailedComparer : IEqualityComparer<Patch>
		{
			// Token: 0x06000129 RID: 297 RVA: 0x00008D60 File Offset: 0x00006F60
			public bool Equals(Patch x, Patch y)
			{
				return y != null && x != null && x.owner == y.owner && x.PatchMethod == y.PatchMethod && x.index == y.index && x.priority == y.priority && x.before.Length == y.before.Length && x.after.Length == y.after.Length && x.before.All(new Func<string, bool>(y.before.Contains<string>)) && x.after.All(new Func<string, bool>(y.after.Contains<string>));
			}

			// Token: 0x0600012A RID: 298 RVA: 0x00008E1E File Offset: 0x0000701E
			public int GetHashCode(Patch obj)
			{
				return obj.GetHashCode();
			}
		}
	}
}
