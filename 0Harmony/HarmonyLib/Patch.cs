using System;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace HarmonyLib
{
	// Token: 0x02000077 RID: 119
	[Serializable]
	public class Patch : IComparable
	{
		// Token: 0x1700000D RID: 13
		// (get) Token: 0x06000211 RID: 529 RVA: 0x0000B13C File Offset: 0x0000933C
		// (set) Token: 0x06000212 RID: 530 RVA: 0x0000B1D8 File Offset: 0x000093D8
		public MethodInfo PatchMethod
		{
			get
			{
				if (this.patchMethod == null)
				{
					Module module = (from a in AppDomain.CurrentDomain.GetAssemblies()
						where !a.FullName.StartsWith("Microsoft.VisualStudio")
						select a).SelectMany((Assembly a) => a.GetLoadedModules()).First((Module a) => a.FullyQualifiedName == this.module);
					this.patchMethod = (MethodInfo)module.ResolveMethod(this.token);
				}
				return this.patchMethod;
			}
			set
			{
				this.patchMethod = value;
				this.token = this.patchMethod.MetadataToken;
				this.module = this.patchMethod.Module.FullyQualifiedName;
			}
		}

		// Token: 0x06000213 RID: 531 RVA: 0x0000B208 File Offset: 0x00009408
		public Patch(MethodInfo patch, int index, string owner, int priority, string[] before, string[] after, bool debug)
		{
			if (patch is DynamicMethod)
			{
				throw new Exception("Cannot directly reference dynamic method \"" + patch.FullDescription() + "\" in Harmony. Use a factory method instead that will return the dynamic method.");
			}
			this.index = index;
			this.owner = owner;
			this.priority = priority;
			this.before = before;
			this.after = after;
			this.debug = debug;
			this.PatchMethod = patch;
		}

		// Token: 0x06000214 RID: 532 RVA: 0x0000B274 File Offset: 0x00009474
		public MethodInfo GetMethod(MethodBase original)
		{
			MethodInfo methodInfo = this.PatchMethod;
			if (methodInfo.ReturnType != typeof(DynamicMethod) && methodInfo.ReturnType != typeof(MethodInfo))
			{
				return methodInfo;
			}
			if (!methodInfo.IsStatic)
			{
				return methodInfo;
			}
			ParameterInfo[] parameters = methodInfo.GetParameters();
			if (parameters.Count<ParameterInfo>() != 1)
			{
				return methodInfo;
			}
			if (parameters[0].ParameterType != typeof(MethodBase))
			{
				return methodInfo;
			}
			return methodInfo.Invoke(null, new object[] { original }) as MethodInfo;
		}

		// Token: 0x06000215 RID: 533 RVA: 0x0000B305 File Offset: 0x00009505
		public override bool Equals(object obj)
		{
			return obj != null && obj is Patch && this.PatchMethod == ((Patch)obj).PatchMethod;
		}

		// Token: 0x06000216 RID: 534 RVA: 0x0000B32A File Offset: 0x0000952A
		public int CompareTo(object obj)
		{
			return PatchInfoSerialization.PriorityComparer(obj, this.index, this.priority);
		}

		// Token: 0x06000217 RID: 535 RVA: 0x0000B33E File Offset: 0x0000953E
		public override int GetHashCode()
		{
			return this.PatchMethod.GetHashCode();
		}

		// Token: 0x0400013F RID: 319
		public readonly int index;

		// Token: 0x04000140 RID: 320
		public readonly string owner;

		// Token: 0x04000141 RID: 321
		public readonly int priority;

		// Token: 0x04000142 RID: 322
		public readonly string[] before;

		// Token: 0x04000143 RID: 323
		public readonly string[] after;

		// Token: 0x04000144 RID: 324
		public readonly bool debug;

		// Token: 0x04000145 RID: 325
		[NonSerialized]
		private MethodInfo patchMethod;

		// Token: 0x04000146 RID: 326
		private int token;

		// Token: 0x04000147 RID: 327
		private string module;
	}
}
