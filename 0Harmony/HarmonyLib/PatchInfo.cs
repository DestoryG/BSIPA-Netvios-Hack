using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace HarmonyLib
{
	// Token: 0x02000070 RID: 112
	[Serializable]
	public class PatchInfo
	{
		// Token: 0x060001F3 RID: 499 RVA: 0x0000AC9A File Offset: 0x00008E9A
		public PatchInfo()
		{
			this.prefixes = new Patch[0];
			this.postfixes = new Patch[0];
			this.transpilers = new Patch[0];
			this.finalizers = new Patch[0];
		}

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x060001F4 RID: 500 RVA: 0x0000ACD4 File Offset: 0x00008ED4
		public bool Debugging
		{
			get
			{
				if (!this.prefixes.Any((Patch p) => p.debug))
				{
					if (!this.postfixes.Any((Patch p) => p.debug))
					{
						if (!this.transpilers.Any((Patch p) => p.debug))
						{
							return this.finalizers.Any((Patch p) => p.debug);
						}
					}
				}
				return true;
			}
		}

		// Token: 0x060001F5 RID: 501 RVA: 0x0000AD94 File Offset: 0x00008F94
		public void AddPrefix(MethodInfo patch, string owner, int priority, string[] before, string[] after, bool debug)
		{
			List<Patch> list = this.prefixes.ToList<Patch>();
			list.Add(new Patch(patch, this.prefixes.Count<Patch>() + 1, owner, priority, before, after, debug));
			this.prefixes = list.ToArray();
		}

		// Token: 0x060001F6 RID: 502 RVA: 0x0000ADDC File Offset: 0x00008FDC
		public void RemovePrefix(string owner)
		{
			if (owner == "*")
			{
				this.prefixes = new Patch[0];
				return;
			}
			this.prefixes = this.prefixes.Where((Patch patch) => patch.owner != owner).ToArray<Patch>();
		}

		// Token: 0x060001F7 RID: 503 RVA: 0x0000AE38 File Offset: 0x00009038
		public void AddPostfix(MethodInfo patch, string owner, int priority, string[] before, string[] after, bool debug)
		{
			List<Patch> list = this.postfixes.ToList<Patch>();
			list.Add(new Patch(patch, this.postfixes.Count<Patch>() + 1, owner, priority, before, after, debug));
			this.postfixes = list.ToArray();
		}

		// Token: 0x060001F8 RID: 504 RVA: 0x0000AE80 File Offset: 0x00009080
		public void RemovePostfix(string owner)
		{
			if (owner == "*")
			{
				this.postfixes = new Patch[0];
				return;
			}
			this.postfixes = this.postfixes.Where((Patch patch) => patch.owner != owner).ToArray<Patch>();
		}

		// Token: 0x060001F9 RID: 505 RVA: 0x0000AEDC File Offset: 0x000090DC
		public void AddTranspiler(MethodInfo patch, string owner, int priority, string[] before, string[] after, bool debug)
		{
			List<Patch> list = this.transpilers.ToList<Patch>();
			list.Add(new Patch(patch, this.transpilers.Count<Patch>() + 1, owner, priority, before, after, debug));
			this.transpilers = list.ToArray();
		}

		// Token: 0x060001FA RID: 506 RVA: 0x0000AF24 File Offset: 0x00009124
		public void RemoveTranspiler(string owner)
		{
			if (owner == "*")
			{
				this.transpilers = new Patch[0];
				return;
			}
			this.transpilers = this.transpilers.Where((Patch patch) => patch.owner != owner).ToArray<Patch>();
		}

		// Token: 0x060001FB RID: 507 RVA: 0x0000AF80 File Offset: 0x00009180
		public void AddFinalizer(MethodInfo patch, string owner, int priority, string[] before, string[] after, bool debug)
		{
			List<Patch> list = this.finalizers.ToList<Patch>();
			list.Add(new Patch(patch, this.finalizers.Count<Patch>() + 1, owner, priority, before, after, debug));
			this.finalizers = list.ToArray();
		}

		// Token: 0x060001FC RID: 508 RVA: 0x0000AFC8 File Offset: 0x000091C8
		public void RemoveFinalizer(string owner)
		{
			if (owner == "*")
			{
				this.finalizers = new Patch[0];
				return;
			}
			this.finalizers = this.finalizers.Where((Patch patch) => patch.owner != owner).ToArray<Patch>();
		}

		// Token: 0x060001FD RID: 509 RVA: 0x0000B024 File Offset: 0x00009224
		public void RemovePatch(MethodInfo patch)
		{
			this.prefixes = this.prefixes.Where((Patch p) => p.PatchMethod != patch).ToArray<Patch>();
			this.postfixes = this.postfixes.Where((Patch p) => p.PatchMethod != patch).ToArray<Patch>();
			this.transpilers = this.transpilers.Where((Patch p) => p.PatchMethod != patch).ToArray<Patch>();
			this.finalizers = this.finalizers.Where((Patch p) => p.PatchMethod != patch).ToArray<Patch>();
		}

		// Token: 0x04000131 RID: 305
		public Patch[] prefixes;

		// Token: 0x04000132 RID: 306
		public Patch[] postfixes;

		// Token: 0x04000133 RID: 307
		public Patch[] transpilers;

		// Token: 0x04000134 RID: 308
		public Patch[] finalizers;
	}
}
