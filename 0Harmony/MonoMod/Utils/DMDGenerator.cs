using System;
using System.Reflection;
using System.Reflection.Emit;

namespace MonoMod.Utils
{
	// Token: 0x0200031D RID: 797
	internal abstract class DMDGenerator<TSelf> : _IDMDGenerator where TSelf : DMDGenerator<TSelf>, new()
	{
		// Token: 0x06001236 RID: 4662
		protected abstract MethodInfo _Generate(DynamicMethodDefinition dmd, object context);

		// Token: 0x06001237 RID: 4663 RVA: 0x0003CDC1 File Offset: 0x0003AFC1
		MethodInfo _IDMDGenerator.Generate(DynamicMethodDefinition dmd, object context)
		{
			return DMDGenerator<TSelf>._Postbuild(this._Generate(dmd, context));
		}

		// Token: 0x06001238 RID: 4664 RVA: 0x0003CDD0 File Offset: 0x0003AFD0
		public static MethodInfo Generate(DynamicMethodDefinition dmd, object context = null)
		{
			TSelf tself;
			if ((tself = DMDGenerator<TSelf>._Instance) == null)
			{
				tself = (DMDGenerator<TSelf>._Instance = new TSelf());
			}
			return DMDGenerator<TSelf>._Postbuild(tself._Generate(dmd, context));
		}

		// Token: 0x06001239 RID: 4665 RVA: 0x0003CDFC File Offset: 0x0003AFFC
		internal static MethodInfo _Postbuild(MethodInfo mi)
		{
			if (mi == null)
			{
				return null;
			}
			if (DynamicMethodDefinition._IsMono && !(mi is DynamicMethod) && mi.DeclaringType != null)
			{
				Module module = ((mi != null) ? mi.Module : null);
				if (module == null)
				{
					return mi;
				}
				Assembly assembly = module.Assembly;
				if (((assembly != null) ? assembly.GetType() : null) == null)
				{
					return mi;
				}
				assembly.SetMonoCorlibInternal(true);
			}
			return mi;
		}

		// Token: 0x04000F34 RID: 3892
		private static TSelf _Instance;
	}
}
