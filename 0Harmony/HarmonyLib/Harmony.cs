using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace HarmonyLib
{
	// Token: 0x0200005B RID: 91
	public class Harmony
	{
		// Token: 0x17000006 RID: 6
		// (get) Token: 0x06000185 RID: 389 RVA: 0x00009B0C File Offset: 0x00007D0C
		// (set) Token: 0x06000186 RID: 390 RVA: 0x00009B14 File Offset: 0x00007D14
		public string Id { get; private set; }

		// Token: 0x06000187 RID: 391 RVA: 0x00009B20 File Offset: 0x00007D20
		public Harmony(string id)
		{
			if (string.IsNullOrEmpty(id))
			{
				throw new ArgumentException("id cannot be null or empty");
			}
			try
			{
				string text = Environment.GetEnvironmentVariable("HARMONY_DEBUG");
				if (text != null && text.Length > 0)
				{
					text = text.Trim();
					Harmony.DEBUG = text == "1" || bool.Parse(text);
				}
			}
			catch
			{
			}
			if (Harmony.DEBUG)
			{
				Assembly assembly = typeof(Harmony).Assembly;
				Version version = assembly.GetName().Version;
				string text2 = assembly.Location;
				if (string.IsNullOrEmpty(text2))
				{
					text2 = new Uri(assembly.CodeBase).LocalPath;
				}
				FileLog.Log(string.Format("### Harmony id={0}, version={1}, location={2}", id, version, text2));
				MethodBase outsideCaller = AccessTools.GetOutsideCaller();
				if (outsideCaller.DeclaringType != null)
				{
					Assembly assembly2 = outsideCaller.DeclaringType.Assembly;
					text2 = assembly2.Location;
					if (string.IsNullOrEmpty(text2))
					{
						text2 = new Uri(assembly2.CodeBase).LocalPath;
					}
					FileLog.Log("### Started from " + outsideCaller.FullDescription() + ", location " + text2);
					FileLog.Log("### At " + DateTime.Now.ToString("yyyy-MM-dd hh.mm.ss"));
				}
			}
			this.Id = id;
		}

		// Token: 0x06000188 RID: 392 RVA: 0x00009C78 File Offset: 0x00007E78
		public void PatchAll()
		{
			Assembly assembly = new StackTrace().GetFrame(1).GetMethod().ReflectedType.Assembly;
			this.PatchAll(assembly);
		}

		// Token: 0x06000189 RID: 393 RVA: 0x00009CA7 File Offset: 0x00007EA7
		public PatchProcessor CreateProcessor(MethodBase original)
		{
			return new PatchProcessor(this, original);
		}

		// Token: 0x0600018A RID: 394 RVA: 0x00009CB0 File Offset: 0x00007EB0
		public PatchClassProcessor CreateClassProcessor(Type type)
		{
			return new PatchClassProcessor(this, type);
		}

		// Token: 0x0600018B RID: 395 RVA: 0x00009CB9 File Offset: 0x00007EB9
		public ReversePatcher CreateReversePatcher(MethodBase original, HarmonyMethod standin)
		{
			return new ReversePatcher(this, original, standin);
		}

		// Token: 0x0600018C RID: 396 RVA: 0x00009CC3 File Offset: 0x00007EC3
		public void PatchAll(Assembly assembly)
		{
			assembly.GetTypes().Do(delegate(Type type)
			{
				this.CreateClassProcessor(type).Patch();
			});
		}

		// Token: 0x0600018D RID: 397 RVA: 0x00009CDC File Offset: 0x00007EDC
		public MethodInfo Patch(MethodBase original, HarmonyMethod prefix = null, HarmonyMethod postfix = null, HarmonyMethod transpiler = null, HarmonyMethod finalizer = null)
		{
			PatchProcessor patchProcessor = this.CreateProcessor(original);
			patchProcessor.AddPrefix(prefix);
			patchProcessor.AddPostfix(postfix);
			patchProcessor.AddTranspiler(transpiler);
			patchProcessor.AddFinalizer(finalizer);
			return patchProcessor.Patch();
		}

		// Token: 0x0600018E RID: 398 RVA: 0x00009D0C File Offset: 0x00007F0C
		public static MethodInfo ReversePatch(MethodBase original, HarmonyMethod standin, MethodInfo transpiler = null)
		{
			return PatchFunctions.ReversePatch(standin, original, transpiler);
		}

		// Token: 0x0600018F RID: 399 RVA: 0x00009D18 File Offset: 0x00007F18
		public void UnpatchAll(string harmonyID = null)
		{
			Harmony.<>c__DisplayClass13_0 CS$<>8__locals1 = new Harmony.<>c__DisplayClass13_0();
			CS$<>8__locals1.harmonyID = harmonyID;
			CS$<>8__locals1.<>4__this = this;
			using (List<MethodBase>.Enumerator enumerator = Harmony.GetAllPatchedMethods().ToList<MethodBase>().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					MethodBase original = enumerator.Current;
					Patches patchInfo2 = Harmony.GetPatchInfo(original);
					patchInfo2.Prefixes.DoIf(new Func<Patch, bool>(CS$<>8__locals1.<UnpatchAll>g__IDCheck|0), delegate(Patch patchInfo)
					{
						CS$<>8__locals1.<>4__this.Unpatch(original, patchInfo.PatchMethod);
					});
					patchInfo2.Postfixes.DoIf(new Func<Patch, bool>(CS$<>8__locals1.<UnpatchAll>g__IDCheck|0), delegate(Patch patchInfo)
					{
						CS$<>8__locals1.<>4__this.Unpatch(original, patchInfo.PatchMethod);
					});
					patchInfo2.Transpilers.DoIf(new Func<Patch, bool>(CS$<>8__locals1.<UnpatchAll>g__IDCheck|0), delegate(Patch patchInfo)
					{
						CS$<>8__locals1.<>4__this.Unpatch(original, patchInfo.PatchMethod);
					});
					patchInfo2.Finalizers.DoIf(new Func<Patch, bool>(CS$<>8__locals1.<UnpatchAll>g__IDCheck|0), delegate(Patch patchInfo)
					{
						CS$<>8__locals1.<>4__this.Unpatch(original, patchInfo.PatchMethod);
					});
				}
			}
		}

		// Token: 0x06000190 RID: 400 RVA: 0x00009E40 File Offset: 0x00008040
		public void Unpatch(MethodBase original, HarmonyPatchType type, string harmonyID = null)
		{
			this.CreateProcessor(original).Unpatch(type, harmonyID);
		}

		// Token: 0x06000191 RID: 401 RVA: 0x00009E51 File Offset: 0x00008051
		public void Unpatch(MethodBase original, MethodInfo patch)
		{
			this.CreateProcessor(original).Unpatch(patch);
		}

		// Token: 0x06000192 RID: 402 RVA: 0x00009E64 File Offset: 0x00008064
		public static bool HasAnyPatches(string harmonyID)
		{
			return (from original in Harmony.GetAllPatchedMethods()
				select Harmony.GetPatchInfo(original)).Any((Patches info) => info.Owners.Contains(harmonyID));
		}

		// Token: 0x06000193 RID: 403 RVA: 0x00009EB8 File Offset: 0x000080B8
		public static Patches GetPatchInfo(MethodBase method)
		{
			return PatchProcessor.GetPatchInfo(method);
		}

		// Token: 0x06000194 RID: 404 RVA: 0x00009EC0 File Offset: 0x000080C0
		public IEnumerable<MethodBase> GetPatchedMethods()
		{
			return from original in Harmony.GetAllPatchedMethods()
				where Harmony.GetPatchInfo(original).Owners.Contains(this.Id)
				select original;
		}

		// Token: 0x06000195 RID: 405 RVA: 0x00009ED8 File Offset: 0x000080D8
		public static IEnumerable<MethodBase> GetAllPatchedMethods()
		{
			return PatchProcessor.GetAllPatchedMethods();
		}

		// Token: 0x06000196 RID: 406 RVA: 0x00009EDF File Offset: 0x000080DF
		public static Dictionary<string, Version> VersionInfo(out Version currentVersion)
		{
			return PatchProcessor.VersionInfo(out currentVersion);
		}

		// Token: 0x040000FE RID: 254
		public static bool DEBUG;
	}
}
