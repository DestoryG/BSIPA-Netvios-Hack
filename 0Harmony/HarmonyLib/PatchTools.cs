using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace HarmonyLib
{
	// Token: 0x0200003D RID: 61
	internal static class PatchTools
	{
		// Token: 0x06000139 RID: 313 RVA: 0x00008EBA File Offset: 0x000070BA
		internal static void RememberObject(object key, object value)
		{
			PatchTools.objectReferences[key] = value;
		}

		// Token: 0x0600013A RID: 314 RVA: 0x00008EC8 File Offset: 0x000070C8
		internal static MethodInfo GetPatchMethod(Type patchType, string attributeName)
		{
			Func<object, bool> <>9__1;
			MethodInfo methodInfo = patchType.GetMethods(AccessTools.all).FirstOrDefault(delegate(MethodInfo m)
			{
				IEnumerable<object> customAttributes = m.GetCustomAttributes(true);
				Func<object, bool> func;
				if ((func = <>9__1) == null)
				{
					func = (<>9__1 = (object a) => a.GetType().FullName == attributeName);
				}
				return customAttributes.Any(func);
			});
			if (methodInfo == null)
			{
				string text = attributeName.Replace("HarmonyLib.Harmony", "");
				methodInfo = patchType.GetMethod(text, AccessTools.all);
			}
			return methodInfo;
		}

		// Token: 0x0600013B RID: 315 RVA: 0x00008F2C File Offset: 0x0000712C
		internal static AssemblyBuilder DefineDynamicAssembly(string name)
		{
			AssemblyName assemblyName = new AssemblyName(name);
			return AppDomain.CurrentDomain.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run);
		}

		// Token: 0x0600013C RID: 316 RVA: 0x00008F4C File Offset: 0x0000714C
		internal static List<AttributePatch> GetPatchMethods(Type type)
		{
			string fullName = typeof(HarmonyPatch).FullName;
			return (from method in AccessTools.GetDeclaredMethods(type)
				select AttributePatch.Create(method) into attributePatch
				where attributePatch != null
				select attributePatch).ToList<AttributePatch>();
		}

		// Token: 0x0600013D RID: 317 RVA: 0x00008FBC File Offset: 0x000071BC
		internal static MethodBase GetOriginalMethod(this HarmonyMethod attr)
		{
			try
			{
				MethodType? methodType = attr.methodType;
				if (methodType != null)
				{
					switch (methodType.GetValueOrDefault())
					{
					case MethodType.Normal:
						if (attr.methodName == null)
						{
							return null;
						}
						return AccessTools.DeclaredMethod(attr.declaringType, attr.methodName, attr.argumentTypes, null);
					case MethodType.Getter:
						if (attr.methodName == null)
						{
							return null;
						}
						return AccessTools.DeclaredProperty(attr.declaringType, attr.methodName).GetGetMethod(true);
					case MethodType.Setter:
						if (attr.methodName == null)
						{
							return null;
						}
						return AccessTools.DeclaredProperty(attr.declaringType, attr.methodName).GetSetMethod(true);
					case MethodType.Constructor:
						return AccessTools.DeclaredConstructor(attr.declaringType, attr.argumentTypes, false);
					case MethodType.StaticConstructor:
						return (from c in AccessTools.GetDeclaredConstructors(attr.declaringType, null)
							where c.IsStatic
							select c).FirstOrDefault<ConstructorInfo>();
					}
				}
			}
			catch (AmbiguousMatchException ex)
			{
				throw new HarmonyException("Ambiguous match for HarmonyMethod[" + attr.Description() + "]", ex.InnerException ?? ex);
			}
			return null;
		}

		// Token: 0x040000C8 RID: 200
		private static readonly Dictionary<object, object> objectReferences = new Dictionary<object, object>();
	}
}
