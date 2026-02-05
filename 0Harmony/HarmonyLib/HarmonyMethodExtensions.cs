using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace HarmonyLib
{
	// Token: 0x02000067 RID: 103
	public static class HarmonyMethodExtensions
	{
		// Token: 0x060001C6 RID: 454 RVA: 0x0000A4D8 File Offset: 0x000086D8
		internal static void SetValue(Traverse trv, string name, object val)
		{
			if (val == null)
			{
				return;
			}
			Traverse traverse = trv.Field(name);
			if (name == "methodType" || name == "reversePatchType")
			{
				val = Enum.ToObject(Nullable.GetUnderlyingType(traverse.GetValueType()), (int)val);
			}
			traverse.SetValue(val);
		}

		// Token: 0x060001C7 RID: 455 RVA: 0x0000A52C File Offset: 0x0000872C
		public static void CopyTo(this HarmonyMethod from, HarmonyMethod to)
		{
			if (to == null)
			{
				return;
			}
			Traverse fromTrv = Traverse.Create(from);
			Traverse toTrv = Traverse.Create(to);
			HarmonyMethod.HarmonyFields().ForEach(delegate(string f)
			{
				object value = fromTrv.Field(f).GetValue();
				if (value != null)
				{
					HarmonyMethodExtensions.SetValue(toTrv, f, value);
				}
			});
		}

		// Token: 0x060001C8 RID: 456 RVA: 0x0000A574 File Offset: 0x00008774
		public static HarmonyMethod Clone(this HarmonyMethod original)
		{
			HarmonyMethod harmonyMethod = new HarmonyMethod();
			original.CopyTo(harmonyMethod);
			return harmonyMethod;
		}

		// Token: 0x060001C9 RID: 457 RVA: 0x0000A590 File Offset: 0x00008790
		public static HarmonyMethod Merge(this HarmonyMethod master, HarmonyMethod detail)
		{
			if (detail == null)
			{
				return master;
			}
			HarmonyMethod harmonyMethod = new HarmonyMethod();
			Traverse resultTrv = Traverse.Create(harmonyMethod);
			Traverse masterTrv = Traverse.Create(master);
			Traverse detailTrv = Traverse.Create(detail);
			HarmonyMethod.HarmonyFields().ForEach(delegate(string f)
			{
				object value = masterTrv.Field(f).GetValue();
				object value2 = detailTrv.Field(f).GetValue();
				HarmonyMethodExtensions.SetValue(resultTrv, f, value2 ?? value);
			});
			return harmonyMethod;
		}

		// Token: 0x060001CA RID: 458 RVA: 0x0000A5EC File Offset: 0x000087EC
		private static HarmonyMethod GetHarmonyMethodInfo(object attribute)
		{
			FieldInfo field = attribute.GetType().GetField("info", AccessTools.all);
			if (field == null)
			{
				return null;
			}
			if (field.FieldType.FullName != typeof(HarmonyMethod).FullName)
			{
				return null;
			}
			return AccessTools.MakeDeepCopy<HarmonyMethod>(field.GetValue(attribute));
		}

		// Token: 0x060001CB RID: 459 RVA: 0x0000A64C File Offset: 0x0000884C
		public static List<HarmonyMethod> GetFromType(Type type)
		{
			return (from attr in type.GetCustomAttributes(true)
				select HarmonyMethodExtensions.GetHarmonyMethodInfo(attr) into info
				where info != null
				select info).ToList<HarmonyMethod>();
		}

		// Token: 0x060001CC RID: 460 RVA: 0x0000A6AD File Offset: 0x000088AD
		public static HarmonyMethod GetMergedFromType(Type type)
		{
			return HarmonyMethod.Merge(HarmonyMethodExtensions.GetFromType(type));
		}

		// Token: 0x060001CD RID: 461 RVA: 0x0000A6BC File Offset: 0x000088BC
		public static List<HarmonyMethod> GetFromMethod(MethodBase method)
		{
			return (from attr in method.GetCustomAttributes(true)
				select HarmonyMethodExtensions.GetHarmonyMethodInfo(attr) into info
				where info != null
				select info).ToList<HarmonyMethod>();
		}

		// Token: 0x060001CE RID: 462 RVA: 0x0000A71D File Offset: 0x0000891D
		public static HarmonyMethod GetMergedFromMethod(MethodBase method)
		{
			return HarmonyMethod.Merge(HarmonyMethodExtensions.GetFromMethod(method));
		}
	}
}
