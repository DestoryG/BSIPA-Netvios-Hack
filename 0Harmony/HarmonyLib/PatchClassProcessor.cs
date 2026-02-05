using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace HarmonyLib
{
	// Token: 0x02000079 RID: 121
	public class PatchClassProcessor
	{
		// Token: 0x0600021D RID: 541 RVA: 0x0000B374 File Offset: 0x00009574
		public PatchClassProcessor(Harmony instance, Type type)
		{
			if (instance == null)
			{
				throw new ArgumentNullException("instance");
			}
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			this.instance = instance;
			this.containerType = type;
			List<HarmonyMethod> fromType = HarmonyMethodExtensions.GetFromType(type);
			if (fromType == null || fromType.Count == 0)
			{
				return;
			}
			this.containerAttributes = HarmonyMethod.Merge(fromType);
			if (this.containerAttributes.methodType == null)
			{
				this.containerAttributes.methodType = new MethodType?(MethodType.Normal);
			}
			this.auxilaryMethods = new Dictionary<Type, MethodInfo>();
			foreach (Type type2 in PatchClassProcessor.auxilaryTypes)
			{
				MethodInfo patchMethod = PatchTools.GetPatchMethod(this.containerType, type2.FullName);
				if (patchMethod != null)
				{
					this.auxilaryMethods[type2] = patchMethod;
				}
			}
			this.patchMethods = PatchTools.GetPatchMethods(this.containerType);
			foreach (AttributePatch attributePatch in this.patchMethods)
			{
				MethodInfo method = attributePatch.info.method;
				attributePatch.info = this.containerAttributes.Merge(attributePatch.info);
				attributePatch.info.method = method;
			}
		}

		// Token: 0x0600021E RID: 542 RVA: 0x0000B4F0 File Offset: 0x000096F0
		public List<MethodInfo> Patch()
		{
			if (this.containerAttributes == null)
			{
				return null;
			}
			Exception ex = null;
			if (!this.RunMethod<HarmonyPrepare, bool>(true, false, null, new object[0]))
			{
				this.RunMethod<HarmonyCleanup>(ref ex, new object[0]);
				this.ReportException(ex, null);
				return new List<MethodInfo>();
			}
			List<MethodInfo> list = new List<MethodInfo>();
			MethodBase methodBase = null;
			try
			{
				this.ReversePatch(ref methodBase);
				List<MethodBase> bulkMethods = this.GetBulkMethods();
				list = ((bulkMethods.Count > 0) ? this.BulkPatch(bulkMethods, ref methodBase) : this.PatchWithAttributes(ref methodBase));
			}
			catch (Exception ex)
			{
			}
			this.RunMethod<HarmonyCleanup>(ref ex, new object[] { ex });
			this.ReportException(ex, methodBase);
			return list;
		}

		// Token: 0x0600021F RID: 543 RVA: 0x0000B59C File Offset: 0x0000979C
		private void ReversePatch(ref MethodBase lastOriginal)
		{
			for (int i = 0; i < this.patchMethods.Count; i++)
			{
				AttributePatch attributePatch = this.patchMethods[i];
				HarmonyPatchType? type = attributePatch.type;
				HarmonyPatchType harmonyPatchType = HarmonyPatchType.ReversePatch;
				if ((type.GetValueOrDefault() == harmonyPatchType) & (type != null))
				{
					lastOriginal = attributePatch.info.GetOriginalMethod();
					ReversePatcher reversePatcher = this.instance.CreateReversePatcher(lastOriginal, attributePatch.info);
					object locker = PatchProcessor.locker;
					lock (locker)
					{
						reversePatcher.Patch(HarmonyReversePatchType.Original);
					}
				}
			}
		}

		// Token: 0x06000220 RID: 544 RVA: 0x0000B648 File Offset: 0x00009848
		private List<MethodInfo> BulkPatch(List<MethodBase> originals, ref MethodBase lastOriginal)
		{
			PatchJobs<MethodInfo> patchJobs = new PatchJobs<MethodInfo>();
			for (int i = 0; i < originals.Count; i++)
			{
				lastOriginal = originals[i];
				PatchJobs<MethodInfo>.Job job = patchJobs.GetJob(lastOriginal);
				foreach (AttributePatch attributePatch in this.patchMethods)
				{
					string text = "You cannot combine TargetMethod, TargetMethods or PatchAll with individual annotations";
					HarmonyMethod info = attributePatch.info;
					if (info.declaringType != null)
					{
						throw new ArgumentException(text + " [" + info.declaringType.FullDescription() + "]");
					}
					if (info.methodName != null)
					{
						throw new ArgumentException(text + " [" + info.methodName + "]");
					}
					if (info.methodType != null && info.methodType.Value != MethodType.Normal)
					{
						throw new ArgumentException(string.Format("{0} [{1}]", text, info.methodType));
					}
					if (info.argumentTypes != null)
					{
						throw new ArgumentException(text + " [" + info.argumentTypes.Description() + "]");
					}
					job.AddPatch(attributePatch);
				}
			}
			foreach (PatchJobs<MethodInfo>.Job job2 in patchJobs.GetJobs())
			{
				lastOriginal = job2.original;
				this.ProcessPatchJob(job2);
			}
			return patchJobs.GetReplacements();
		}

		// Token: 0x06000221 RID: 545 RVA: 0x0000B7F8 File Offset: 0x000099F8
		private List<MethodInfo> PatchWithAttributes(ref MethodBase lastOriginal)
		{
			PatchJobs<MethodInfo> patchJobs = new PatchJobs<MethodInfo>();
			foreach (AttributePatch attributePatch in this.patchMethods)
			{
				lastOriginal = attributePatch.info.GetOriginalMethod();
				if (lastOriginal == null)
				{
					throw new ArgumentException("Undefined target method for patch method " + attributePatch.info.method.FullDescription());
				}
				patchJobs.GetJob(lastOriginal).AddPatch(attributePatch);
			}
			foreach (PatchJobs<MethodInfo>.Job job in patchJobs.GetJobs())
			{
				lastOriginal = job.original;
				this.ProcessPatchJob(job);
			}
			return patchJobs.GetReplacements();
		}

		// Token: 0x06000222 RID: 546 RVA: 0x0000B8E4 File Offset: 0x00009AE4
		private void ProcessPatchJob(PatchJobs<MethodInfo>.Job job)
		{
			MethodInfo methodInfo = null;
			bool flag = this.RunMethod<HarmonyPrepare, bool>(true, false, null, new object[] { job.original });
			Exception ex = null;
			if (flag)
			{
				object locker = PatchProcessor.locker;
				lock (locker)
				{
					try
					{
						PatchInfo patchInfo = HarmonySharedState.GetPatchInfo(job.original);
						if (patchInfo == null)
						{
							patchInfo = new PatchInfo();
						}
						foreach (HarmonyMethod harmonyMethod in job.prefixes)
						{
							PatchFunctions.AddPrefix(patchInfo, this.instance.Id, harmonyMethod);
						}
						foreach (HarmonyMethod harmonyMethod2 in job.postfixes)
						{
							PatchFunctions.AddPostfix(patchInfo, this.instance.Id, harmonyMethod2);
						}
						foreach (HarmonyMethod harmonyMethod3 in job.transpilers)
						{
							PatchFunctions.AddTranspiler(patchInfo, this.instance.Id, harmonyMethod3);
						}
						foreach (HarmonyMethod harmonyMethod4 in job.finalizers)
						{
							PatchFunctions.AddFinalizer(patchInfo, this.instance.Id, harmonyMethod4);
						}
						methodInfo = PatchFunctions.UpdateWrapper(job.original, patchInfo);
						HarmonySharedState.UpdatePatchInfo(job.original, patchInfo);
					}
					catch (Exception ex)
					{
					}
				}
			}
			this.RunMethod<HarmonyCleanup>(ref ex, new object[] { job.original, ex });
			this.ReportException(ex, job.original);
			job.replacement = methodInfo;
		}

		// Token: 0x06000223 RID: 547 RVA: 0x0000BB3C File Offset: 0x00009D3C
		private List<MethodBase> GetBulkMethods()
		{
			if (this.containerType.GetCustomAttributes(true).Any((object a) => a.GetType().FullName == typeof(HarmonyPatchAll).FullName))
			{
				Type declaringType = this.containerAttributes.declaringType;
				if (declaringType == null)
				{
					throw new ArgumentException("Using " + typeof(HarmonyPatchAll).FullName + " requires an additional attribute for specifying the Class/Type");
				}
				List<MethodBase> list = new List<MethodBase>();
				list.AddRange(AccessTools.GetDeclaredConstructors(declaringType, null).Cast<MethodBase>());
				list.AddRange(AccessTools.GetDeclaredMethods(declaringType).Cast<MethodBase>());
				List<PropertyInfo> declaredProperties = AccessTools.GetDeclaredProperties(declaringType);
				list.AddRange((from prop in declaredProperties
					select prop.GetGetMethod(true) into method
					where method != null
					select method).Cast<MethodBase>());
				list.AddRange((from prop in declaredProperties
					select prop.GetSetMethod(true) into method
					where method != null
					select method).Cast<MethodBase>());
				return list;
			}
			else
			{
				IEnumerable<MethodBase> enumerable = this.RunMethod<HarmonyTargetMethods, IEnumerable<MethodBase>>(null, null, new Func<IEnumerable<MethodBase>, string>(PatchClassProcessor.<GetBulkMethods>g__FailOnResult|12_1), new object[0]);
				if (enumerable != null)
				{
					return enumerable.ToList<MethodBase>();
				}
				List<MethodBase> list2 = new List<MethodBase>();
				MethodBase methodBase = this.RunMethod<HarmonyTargetMethod, MethodBase>(null, null, delegate(MethodBase method)
				{
					if (!(method == null))
					{
						return null;
					}
					return "null";
				}, new object[0]);
				if (methodBase != null)
				{
					list2.Add(methodBase);
				}
				return list2;
			}
		}

		// Token: 0x06000224 RID: 548 RVA: 0x0000BD08 File Offset: 0x00009F08
		private void ReportException(Exception exception, MethodBase original)
		{
			if (exception == null)
			{
				return;
			}
			if (this.containerAttributes.debug.GetValueOrDefault() || Harmony.DEBUG)
			{
				Version version;
				Harmony.VersionInfo(out version);
				FileLog.indentLevel = 0;
				FileLog.Log(string.Format("### Exception from user \"{0}\", Harmony v{1}", this.instance.Id, version));
				FileLog.Log("### Original: " + (((original != null) ? original.FullDescription() : null) ?? "NULL"));
				FileLog.Log("### Patch class: " + this.containerType.FullDescription());
				Exception ex = exception;
				HarmonyException ex2 = ex as HarmonyException;
				if (ex2 != null)
				{
					ex = ex2.InnerException;
				}
				string text = ex.ToString();
				while (text.Contains("\n\n"))
				{
					text = text.Replace("\n\n", "\n");
				}
				text = text.Split(new char[] { '\n' }).Join((string line) => "### " + line, "\n");
				FileLog.Log(text.Trim());
			}
			if (exception is HarmonyException)
			{
				throw exception;
			}
			throw new HarmonyException("Patching exception in method " + original.FullDescription(), exception);
		}

		// Token: 0x06000225 RID: 549 RVA: 0x0000BE3C File Offset: 0x0000A03C
		private T RunMethod<S, T>(T defaultIfNotExisting, T defaultIfFailing, Func<T, string> failOnResult = null, params object[] parameters)
		{
			MethodInfo methodInfo;
			if (!this.auxilaryMethods.TryGetValue(typeof(S), out methodInfo))
			{
				return defaultIfNotExisting;
			}
			object[] array = (parameters ?? new object[0]).Union(new object[] { this.instance }).ToArray<object>();
			object[] array2 = AccessTools.ActualParameters(methodInfo, array);
			if (methodInfo.ReturnType != typeof(void) && !typeof(T).IsAssignableFrom(methodInfo.ReturnType))
			{
				throw new Exception(string.Concat(new string[]
				{
					"Method ",
					methodInfo.FullDescription(),
					" has wrong return type (should be assignable to ",
					typeof(T).FullName,
					")"
				}));
			}
			T t = defaultIfFailing;
			try
			{
				if (methodInfo.ReturnType == typeof(void))
				{
					methodInfo.Invoke(null, array2);
					t = defaultIfNotExisting;
				}
				else
				{
					t = (T)((object)methodInfo.Invoke(null, array2));
				}
				if (failOnResult != null)
				{
					string text = failOnResult(t);
					if (text != null)
					{
						throw new Exception("Method " + methodInfo.FullDescription() + " returned an unexpected result: " + text);
					}
				}
			}
			catch (Exception ex)
			{
				this.ReportException(ex, methodInfo);
			}
			return t;
		}

		// Token: 0x06000226 RID: 550 RVA: 0x0000BF88 File Offset: 0x0000A188
		private void RunMethod<S>(ref Exception exception, params object[] parameters)
		{
			MethodInfo methodInfo;
			if (this.auxilaryMethods.TryGetValue(typeof(S), out methodInfo))
			{
				object[] array = (parameters ?? new object[0]).Union(new object[] { this.instance }).ToArray<object>();
				object[] array2 = AccessTools.ActualParameters(methodInfo, array);
				try
				{
					object obj = methodInfo.Invoke(null, array2);
					if (methodInfo.ReturnType == typeof(Exception))
					{
						exception = obj as Exception;
					}
				}
				catch (Exception ex)
				{
					this.ReportException(ex, methodInfo);
				}
			}
		}

		// Token: 0x06000228 RID: 552 RVA: 0x0000C07B File Offset: 0x0000A27B
		[CompilerGenerated]
		internal static string <GetBulkMethods>g__FailOnResult|12_1(IEnumerable<MethodBase> res)
		{
			if (res == null)
			{
				return "null";
			}
			if (res.Any((MethodBase m) => m == null))
			{
				return "some element was null";
			}
			return null;
		}

		// Token: 0x0400014B RID: 331
		private readonly Harmony instance;

		// Token: 0x0400014C RID: 332
		private readonly Type containerType;

		// Token: 0x0400014D RID: 333
		private readonly HarmonyMethod containerAttributes;

		// Token: 0x0400014E RID: 334
		private readonly Dictionary<Type, MethodInfo> auxilaryMethods;

		// Token: 0x0400014F RID: 335
		private readonly List<AttributePatch> patchMethods;

		// Token: 0x04000150 RID: 336
		private static readonly List<Type> auxilaryTypes = new List<Type>
		{
			typeof(HarmonyPrepare),
			typeof(HarmonyCleanup),
			typeof(HarmonyTargetMethod),
			typeof(HarmonyTargetMethods)
		};
	}
}
