using System;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace HarmonyLib
{
	// Token: 0x0200002C RID: 44
	internal static class PatchArgumentExtensions
	{
		// Token: 0x060000EB RID: 235 RVA: 0x00007D20 File Offset: 0x00005F20
		private static HarmonyArgument[] AllHarmonyArguments(object[] attributes)
		{
			return (from harg in attributes.Select(delegate(object attr)
				{
					if (attr.GetType().Name != "HarmonyArgument")
					{
						return null;
					}
					return AccessTools.MakeDeepCopy<HarmonyArgument>(attr);
				})
				where harg != null
				select harg).ToArray<HarmonyArgument>();
		}

		// Token: 0x060000EC RID: 236 RVA: 0x00007D7B File Offset: 0x00005F7B
		private static HarmonyArgument GetArgumentAttribute(this ParameterInfo parameter)
		{
			return PatchArgumentExtensions.AllHarmonyArguments(parameter.GetCustomAttributes(false)).FirstOrDefault<HarmonyArgument>();
		}

		// Token: 0x060000ED RID: 237 RVA: 0x00007D8E File Offset: 0x00005F8E
		private static HarmonyArgument[] GetArgumentAttributes(this MethodInfo method)
		{
			if (method == null || method is DynamicMethod)
			{
				return null;
			}
			return PatchArgumentExtensions.AllHarmonyArguments(method.GetCustomAttributes(false));
		}

		// Token: 0x060000EE RID: 238 RVA: 0x00007DAF File Offset: 0x00005FAF
		private static HarmonyArgument[] GetArgumentAttributes(this Type type)
		{
			return PatchArgumentExtensions.AllHarmonyArguments(type.GetCustomAttributes(false));
		}

		// Token: 0x060000EF RID: 239 RVA: 0x00007DC0 File Offset: 0x00005FC0
		private static string GetOriginalArgumentName(this ParameterInfo parameter, string[] originalParameterNames)
		{
			HarmonyArgument argumentAttribute = parameter.GetArgumentAttribute();
			if (argumentAttribute == null)
			{
				return null;
			}
			if (!string.IsNullOrEmpty(argumentAttribute.OriginalName))
			{
				return argumentAttribute.OriginalName;
			}
			if (argumentAttribute.Index >= 0 && argumentAttribute.Index < originalParameterNames.Length)
			{
				return originalParameterNames[argumentAttribute.Index];
			}
			return null;
		}

		// Token: 0x060000F0 RID: 240 RVA: 0x00007E0C File Offset: 0x0000600C
		private static string GetOriginalArgumentName(HarmonyArgument[] attributes, string name, string[] originalParameterNames)
		{
			if (((attributes != null) ? attributes.Length : 0) <= 0)
			{
				return null;
			}
			HarmonyArgument harmonyArgument = attributes.SingleOrDefault((HarmonyArgument p) => p.NewName == name);
			if (harmonyArgument == null)
			{
				return null;
			}
			if (!string.IsNullOrEmpty(harmonyArgument.OriginalName))
			{
				return harmonyArgument.OriginalName;
			}
			if (originalParameterNames != null && harmonyArgument.Index >= 0 && harmonyArgument.Index < originalParameterNames.Length)
			{
				return originalParameterNames[harmonyArgument.Index];
			}
			return null;
		}

		// Token: 0x060000F1 RID: 241 RVA: 0x00007E84 File Offset: 0x00006084
		private static string GetOriginalArgumentName(this MethodInfo method, string[] originalParameterNames, string name)
		{
			string text = PatchArgumentExtensions.GetOriginalArgumentName((method != null) ? method.GetArgumentAttributes() : null, name, originalParameterNames);
			if (text != null)
			{
				return text;
			}
			HarmonyArgument[] array;
			if (method == null)
			{
				array = null;
			}
			else
			{
				Type declaringType = method.DeclaringType;
				array = ((declaringType != null) ? declaringType.GetArgumentAttributes() : null);
			}
			text = PatchArgumentExtensions.GetOriginalArgumentName(array, name, originalParameterNames);
			if (text != null)
			{
				return text;
			}
			return name;
		}

		// Token: 0x060000F2 RID: 242 RVA: 0x00007ED0 File Offset: 0x000060D0
		internal static int GetArgumentIndex(this MethodInfo patch, string[] originalParameterNames, ParameterInfo patchParam)
		{
			if (patch is DynamicMethod)
			{
				return Array.IndexOf<string>(originalParameterNames, patchParam.Name);
			}
			string text = patchParam.GetOriginalArgumentName(originalParameterNames);
			if (text != null)
			{
				return Array.IndexOf<string>(originalParameterNames, text);
			}
			text = patch.GetOriginalArgumentName(originalParameterNames, patchParam.Name);
			if (text != null)
			{
				return Array.IndexOf<string>(originalParameterNames, text);
			}
			return -1;
		}
	}
}
