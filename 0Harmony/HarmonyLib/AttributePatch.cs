using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace HarmonyLib
{
	// Token: 0x02000033 RID: 51
	internal class AttributePatch
	{
		// Token: 0x0600010F RID: 271 RVA: 0x0000853C File Offset: 0x0000673C
		internal static AttributePatch Create(MethodInfo patch)
		{
			if (patch == null)
			{
				throw new NullReferenceException("Patch method cannot be null");
			}
			object[] customAttributes = patch.GetCustomAttributes(true);
			HarmonyPatchType? patchType = AttributePatch.GetPatchType(patch.Name, customAttributes);
			HarmonyPatchType? harmonyPatchType = patchType;
			HarmonyPatchType harmonyPatchType2 = HarmonyPatchType.ReversePatch;
			if (!((harmonyPatchType.GetValueOrDefault() == harmonyPatchType2) & (harmonyPatchType != null)) && !patch.IsStatic)
			{
				throw new ArgumentException("Patch method " + patch.FullDescription() + " must be static");
			}
			string harmonyAttributeName = typeof(HarmonyAttribute).FullName;
			HarmonyMethod harmonyMethod = HarmonyMethod.Merge((from attr in customAttributes
				where attr.GetType().BaseType.FullName == harmonyAttributeName
				select AccessTools.Field(attr.GetType(), "info").GetValue(attr) into harmonyInfo
				select AccessTools.MakeDeepCopy<HarmonyMethod>(harmonyInfo)).ToList<HarmonyMethod>());
			harmonyMethod.method = patch;
			return new AttributePatch
			{
				info = harmonyMethod,
				type = patchType
			};
		}

		// Token: 0x06000110 RID: 272 RVA: 0x0000864C File Offset: 0x0000684C
		private static HarmonyPatchType? GetPatchType(string methodName, object[] allAttributes)
		{
			HashSet<string> hashSet = new HashSet<string>(from attr in allAttributes
				select attr.GetType().FullName into name
				where name.StartsWith("Harmony")
				select name);
			HarmonyPatchType? harmonyPatchType = null;
			foreach (HarmonyPatchType harmonyPatchType2 in AttributePatch.allPatchTypes)
			{
				string text = harmonyPatchType2.ToString();
				if (text == methodName || hashSet.Contains("HarmonyLib.Harmony" + text))
				{
					harmonyPatchType = new HarmonyPatchType?(harmonyPatchType2);
					break;
				}
			}
			return harmonyPatchType;
		}

		// Token: 0x040000AE RID: 174
		private static readonly HarmonyPatchType[] allPatchTypes = new HarmonyPatchType[]
		{
			HarmonyPatchType.Prefix,
			HarmonyPatchType.Postfix,
			HarmonyPatchType.Transpiler,
			HarmonyPatchType.Finalizer,
			HarmonyPatchType.ReversePatch
		};

		// Token: 0x040000AF RID: 175
		internal HarmonyMethod info;

		// Token: 0x040000B0 RID: 176
		internal HarmonyPatchType? type;
	}
}
