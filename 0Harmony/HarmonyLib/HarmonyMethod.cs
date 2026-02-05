using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace HarmonyLib
{
	// Token: 0x02000062 RID: 98
	public class HarmonyMethod
	{
		// Token: 0x060001B4 RID: 436 RVA: 0x0000A12E File Offset: 0x0000832E
		public HarmonyMethod()
		{
		}

		// Token: 0x060001B5 RID: 437 RVA: 0x0000A140 File Offset: 0x00008340
		private void ImportMethod(MethodInfo theMethod)
		{
			this.method = theMethod;
			if (this.method != null)
			{
				List<HarmonyMethod> fromMethod = HarmonyMethodExtensions.GetFromMethod(this.method);
				if (fromMethod != null)
				{
					HarmonyMethod.Merge(fromMethod).CopyTo(this);
				}
			}
		}

		// Token: 0x060001B6 RID: 438 RVA: 0x0000A17D File Offset: 0x0000837D
		public HarmonyMethod(MethodInfo method)
		{
			if (method == null)
			{
				throw new ArgumentNullException("method");
			}
			this.ImportMethod(method);
		}

		// Token: 0x060001B7 RID: 439 RVA: 0x0000A1A8 File Offset: 0x000083A8
		public HarmonyMethod(MethodInfo method, int priority = -1, string[] before = null, string[] after = null, bool? debug = null)
		{
			if (method == null)
			{
				throw new ArgumentNullException("method");
			}
			this.ImportMethod(method);
			this.priority = priority;
			this.before = before;
			this.after = after;
			this.debug = debug;
		}

		// Token: 0x060001B8 RID: 440 RVA: 0x0000A1FC File Offset: 0x000083FC
		public HarmonyMethod(Type methodType, string methodName, Type[] argumentTypes = null)
		{
			MethodInfo methodInfo = AccessTools.Method(methodType, methodName, argumentTypes, null);
			if (methodInfo == null)
			{
				throw new ArgumentException(string.Format("Cannot not find method for type {0} and name {1} and parameters {2}", methodType, methodName, (argumentTypes != null) ? argumentTypes.Description() : null));
			}
			this.ImportMethod(methodInfo);
		}

		// Token: 0x060001B9 RID: 441 RVA: 0x0000A24E File Offset: 0x0000844E
		public static List<string> HarmonyFields()
		{
			return (from s in AccessTools.GetFieldNames(typeof(HarmonyMethod))
				where s != "method"
				select s).ToList<string>();
		}

		// Token: 0x060001BA RID: 442 RVA: 0x0000A288 File Offset: 0x00008488
		public static HarmonyMethod Merge(List<HarmonyMethod> attributes)
		{
			HarmonyMethod harmonyMethod = new HarmonyMethod();
			if (attributes == null)
			{
				return harmonyMethod;
			}
			Traverse resultTrv = Traverse.Create(harmonyMethod);
			attributes.ForEach(delegate(HarmonyMethod attribute)
			{
				Traverse trv = Traverse.Create(attribute);
				HarmonyMethod.HarmonyFields().ForEach(delegate(string f)
				{
					object value = trv.Field(f).GetValue();
					if (value != null)
					{
						HarmonyMethodExtensions.SetValue(resultTrv, f, value);
					}
				});
			});
			return harmonyMethod;
		}

		// Token: 0x060001BB RID: 443 RVA: 0x0000A2C8 File Offset: 0x000084C8
		public override string ToString()
		{
			string result = "";
			Traverse trv = Traverse.Create(this);
			HarmonyMethod.HarmonyFields().ForEach(delegate(string f)
			{
				if (result.Length > 0)
				{
					result += ", ";
				}
				result += string.Format("{0}={1}", f, trv.Field(f).GetValue());
			});
			return "HarmonyMethod[" + result + "]";
		}

		// Token: 0x060001BC RID: 444 RVA: 0x0000A320 File Offset: 0x00008520
		internal string Description()
		{
			string text = ((this.declaringType != null) ? this.declaringType.FullName : "undefined");
			string text2 = this.methodName ?? "undefined";
			string text3 = ((this.methodType != null) ? this.methodType.Value.ToString() : "undefined");
			string text4 = ((this.argumentTypes != null) ? this.argumentTypes.Description() : "undefined");
			return string.Concat(new string[] { "(class=", text, ", methodname=", text2, ", type=", text3, ", args=", text4, ")" });
		}

		// Token: 0x0400010C RID: 268
		public MethodInfo method;

		// Token: 0x0400010D RID: 269
		public Type declaringType;

		// Token: 0x0400010E RID: 270
		public string methodName;

		// Token: 0x0400010F RID: 271
		public MethodType? methodType;

		// Token: 0x04000110 RID: 272
		public Type[] argumentTypes;

		// Token: 0x04000111 RID: 273
		public int priority = -1;

		// Token: 0x04000112 RID: 274
		public string[] before;

		// Token: 0x04000113 RID: 275
		public string[] after;

		// Token: 0x04000114 RID: 276
		public HarmonyReversePatchType? reversePatchType;

		// Token: 0x04000115 RID: 277
		public bool? debug;
	}
}
