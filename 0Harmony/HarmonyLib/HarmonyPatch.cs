using System;
using System.Collections.Generic;

namespace HarmonyLib
{
	// Token: 0x02000048 RID: 72
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
	public class HarmonyPatch : HarmonyAttribute
	{
		// Token: 0x06000152 RID: 338 RVA: 0x000094DD File Offset: 0x000076DD
		public HarmonyPatch()
		{
		}

		// Token: 0x06000153 RID: 339 RVA: 0x000094E5 File Offset: 0x000076E5
		public HarmonyPatch(Type declaringType)
		{
			this.info.declaringType = declaringType;
		}

		// Token: 0x06000154 RID: 340 RVA: 0x000094F9 File Offset: 0x000076F9
		public HarmonyPatch(Type declaringType, Type[] argumentTypes)
		{
			this.info.declaringType = declaringType;
			this.info.argumentTypes = argumentTypes;
		}

		// Token: 0x06000155 RID: 341 RVA: 0x00009519 File Offset: 0x00007719
		public HarmonyPatch(Type declaringType, string methodName)
		{
			this.info.declaringType = declaringType;
			this.info.methodName = methodName;
		}

		// Token: 0x06000156 RID: 342 RVA: 0x00009539 File Offset: 0x00007739
		public HarmonyPatch(Type declaringType, string methodName, params Type[] argumentTypes)
		{
			this.info.declaringType = declaringType;
			this.info.methodName = methodName;
			this.info.argumentTypes = argumentTypes;
		}

		// Token: 0x06000157 RID: 343 RVA: 0x00009565 File Offset: 0x00007765
		public HarmonyPatch(Type declaringType, string methodName, Type[] argumentTypes, ArgumentType[] argumentVariations)
		{
			this.info.declaringType = declaringType;
			this.info.methodName = methodName;
			this.ParseSpecialArguments(argumentTypes, argumentVariations);
		}

		// Token: 0x06000158 RID: 344 RVA: 0x0000958E File Offset: 0x0000778E
		public HarmonyPatch(Type declaringType, MethodType methodType)
		{
			this.info.declaringType = declaringType;
			this.info.methodType = new MethodType?(methodType);
		}

		// Token: 0x06000159 RID: 345 RVA: 0x000095B3 File Offset: 0x000077B3
		public HarmonyPatch(Type declaringType, MethodType methodType, params Type[] argumentTypes)
		{
			this.info.declaringType = declaringType;
			this.info.methodType = new MethodType?(methodType);
			this.info.argumentTypes = argumentTypes;
		}

		// Token: 0x0600015A RID: 346 RVA: 0x000095E4 File Offset: 0x000077E4
		public HarmonyPatch(Type declaringType, MethodType methodType, Type[] argumentTypes, ArgumentType[] argumentVariations)
		{
			this.info.declaringType = declaringType;
			this.info.methodType = new MethodType?(methodType);
			this.ParseSpecialArguments(argumentTypes, argumentVariations);
		}

		// Token: 0x0600015B RID: 347 RVA: 0x00009612 File Offset: 0x00007812
		public HarmonyPatch(Type declaringType, string methodName, MethodType methodType)
		{
			this.info.declaringType = declaringType;
			this.info.methodName = methodName;
			this.info.methodType = new MethodType?(methodType);
		}

		// Token: 0x0600015C RID: 348 RVA: 0x00009643 File Offset: 0x00007843
		public HarmonyPatch(string methodName)
		{
			this.info.methodName = methodName;
		}

		// Token: 0x0600015D RID: 349 RVA: 0x00009657 File Offset: 0x00007857
		public HarmonyPatch(string methodName, params Type[] argumentTypes)
		{
			this.info.methodName = methodName;
			this.info.argumentTypes = argumentTypes;
		}

		// Token: 0x0600015E RID: 350 RVA: 0x00009677 File Offset: 0x00007877
		public HarmonyPatch(string methodName, Type[] argumentTypes, ArgumentType[] argumentVariations)
		{
			this.info.methodName = methodName;
			this.ParseSpecialArguments(argumentTypes, argumentVariations);
		}

		// Token: 0x0600015F RID: 351 RVA: 0x00009693 File Offset: 0x00007893
		public HarmonyPatch(string methodName, MethodType methodType)
		{
			this.info.methodName = methodName;
			this.info.methodType = new MethodType?(methodType);
		}

		// Token: 0x06000160 RID: 352 RVA: 0x000096B8 File Offset: 0x000078B8
		public HarmonyPatch(MethodType methodType)
		{
			this.info.methodType = new MethodType?(methodType);
		}

		// Token: 0x06000161 RID: 353 RVA: 0x000096D1 File Offset: 0x000078D1
		public HarmonyPatch(MethodType methodType, params Type[] argumentTypes)
		{
			this.info.methodType = new MethodType?(methodType);
			this.info.argumentTypes = argumentTypes;
		}

		// Token: 0x06000162 RID: 354 RVA: 0x000096F6 File Offset: 0x000078F6
		public HarmonyPatch(MethodType methodType, Type[] argumentTypes, ArgumentType[] argumentVariations)
		{
			this.info.methodType = new MethodType?(methodType);
			this.ParseSpecialArguments(argumentTypes, argumentVariations);
		}

		// Token: 0x06000163 RID: 355 RVA: 0x00009717 File Offset: 0x00007917
		public HarmonyPatch(Type[] argumentTypes)
		{
			this.info.argumentTypes = argumentTypes;
		}

		// Token: 0x06000164 RID: 356 RVA: 0x0000972B File Offset: 0x0000792B
		public HarmonyPatch(Type[] argumentTypes, ArgumentType[] argumentVariations)
		{
			this.ParseSpecialArguments(argumentTypes, argumentVariations);
		}

		// Token: 0x06000165 RID: 357 RVA: 0x0000973C File Offset: 0x0000793C
		private void ParseSpecialArguments(Type[] argumentTypes, ArgumentType[] argumentVariations)
		{
			if (argumentVariations == null || argumentVariations.Length == 0)
			{
				this.info.argumentTypes = argumentTypes;
				return;
			}
			if (argumentTypes.Length < argumentVariations.Length)
			{
				throw new ArgumentException("argumentVariations contains more elements than argumentTypes", "argumentVariations");
			}
			List<Type> list = new List<Type>();
			for (int i = 0; i < argumentTypes.Length; i++)
			{
				Type type = argumentTypes[i];
				switch (argumentVariations[i])
				{
				case ArgumentType.Ref:
				case ArgumentType.Out:
					type = type.MakeByRefType();
					break;
				case ArgumentType.Pointer:
					type = type.MakePointerType();
					break;
				}
				list.Add(type);
			}
			this.info.argumentTypes = list.ToArray();
		}
	}
}
