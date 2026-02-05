using System;
using System.Reflection;
using System.Reflection.Emit;
using MonoMod.Utils;

namespace HarmonyLib
{
	// Token: 0x02000006 RID: 6
	public static class FastAccess
	{
		// Token: 0x0600000F RID: 15 RVA: 0x00002174 File Offset: 0x00000374
		public static InstantiationHandler<T> CreateInstantiationHandler<T>()
		{
			ConstructorInfo constructor = typeof(T).GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, new Type[0], null);
			if (constructor == null)
			{
				throw new ApplicationException(string.Format("The type {0} must declare an empty constructor (the constructor may be private, internal, protected, protected internal, or public).", typeof(T)));
			}
			DynamicMethodDefinition dynamicMethodDefinition = new DynamicMethodDefinition("InstantiateObject_" + typeof(T).Name, typeof(T), null);
			ILGenerator ilgenerator = dynamicMethodDefinition.GetILGenerator();
			ilgenerator.Emit(OpCodes.Newobj, constructor);
			ilgenerator.Emit(OpCodes.Ret);
			return (InstantiationHandler<T>)dynamicMethodDefinition.Generate().CreateDelegate(typeof(InstantiationHandler<T>));
		}

		// Token: 0x06000010 RID: 16 RVA: 0x0000221C File Offset: 0x0000041C
		public static GetterHandler<T, S> CreateGetterHandler<T, S>(PropertyInfo propertyInfo)
		{
			MethodInfo getMethod = propertyInfo.GetGetMethod(true);
			DynamicMethodDefinition dynamicMethodDefinition = FastAccess.CreateGetDynamicMethod<T, S>(propertyInfo.DeclaringType);
			ILGenerator ilgenerator = dynamicMethodDefinition.GetILGenerator();
			ilgenerator.Emit(OpCodes.Ldarg_0);
			ilgenerator.Emit(OpCodes.Call, getMethod);
			ilgenerator.Emit(OpCodes.Ret);
			return (GetterHandler<T, S>)dynamicMethodDefinition.Generate().CreateDelegate(typeof(GetterHandler<T, S>));
		}

		// Token: 0x06000011 RID: 17 RVA: 0x0000227C File Offset: 0x0000047C
		public static GetterHandler<T, S> CreateGetterHandler<T, S>(FieldInfo fieldInfo)
		{
			DynamicMethodDefinition dynamicMethodDefinition = FastAccess.CreateGetDynamicMethod<T, S>(fieldInfo.DeclaringType);
			ILGenerator ilgenerator = dynamicMethodDefinition.GetILGenerator();
			ilgenerator.Emit(OpCodes.Ldarg_0);
			ilgenerator.Emit(OpCodes.Ldfld, fieldInfo);
			ilgenerator.Emit(OpCodes.Ret);
			return (GetterHandler<T, S>)dynamicMethodDefinition.Generate().CreateDelegate(typeof(GetterHandler<T, S>));
		}

		// Token: 0x06000012 RID: 18 RVA: 0x000022D4 File Offset: 0x000004D4
		public static GetterHandler<T, S> CreateFieldGetter<T, S>(params string[] names)
		{
			foreach (string text in names)
			{
				FieldInfo field = typeof(T).GetField(text, AccessTools.all);
				if (field != null)
				{
					return FastAccess.CreateGetterHandler<T, S>(field);
				}
				PropertyInfo property = typeof(T).GetProperty(text, AccessTools.all);
				if (property != null)
				{
					return FastAccess.CreateGetterHandler<T, S>(property);
				}
			}
			return null;
		}

		// Token: 0x06000013 RID: 19 RVA: 0x00002348 File Offset: 0x00000548
		public static SetterHandler<T, S> CreateSetterHandler<T, S>(PropertyInfo propertyInfo)
		{
			MethodInfo setMethod = propertyInfo.GetSetMethod(true);
			DynamicMethodDefinition dynamicMethodDefinition = FastAccess.CreateSetDynamicMethod<T, S>(propertyInfo.DeclaringType);
			ILGenerator ilgenerator = dynamicMethodDefinition.GetILGenerator();
			ilgenerator.Emit(OpCodes.Ldarg_0);
			ilgenerator.Emit(OpCodes.Ldarg_1);
			ilgenerator.Emit(OpCodes.Call, setMethod);
			ilgenerator.Emit(OpCodes.Ret);
			return (SetterHandler<T, S>)dynamicMethodDefinition.Generate().CreateDelegate(typeof(SetterHandler<T, S>));
		}

		// Token: 0x06000014 RID: 20 RVA: 0x000023B4 File Offset: 0x000005B4
		public static SetterHandler<T, S> CreateSetterHandler<T, S>(FieldInfo fieldInfo)
		{
			DynamicMethodDefinition dynamicMethodDefinition = FastAccess.CreateSetDynamicMethod<T, S>(fieldInfo.DeclaringType);
			ILGenerator ilgenerator = dynamicMethodDefinition.GetILGenerator();
			ilgenerator.Emit(OpCodes.Ldarg_0);
			ilgenerator.Emit(OpCodes.Ldarg_1);
			ilgenerator.Emit(OpCodes.Stfld, fieldInfo);
			ilgenerator.Emit(OpCodes.Ret);
			return (SetterHandler<T, S>)dynamicMethodDefinition.Generate().CreateDelegate(typeof(SetterHandler<T, S>));
		}

		// Token: 0x06000015 RID: 21 RVA: 0x00002417 File Offset: 0x00000617
		private static DynamicMethodDefinition CreateGetDynamicMethod<T, S>(Type type)
		{
			return new DynamicMethodDefinition("DynamicGet_" + type.Name, typeof(S), new Type[] { typeof(T) });
		}

		// Token: 0x06000016 RID: 22 RVA: 0x0000244C File Offset: 0x0000064C
		private static DynamicMethodDefinition CreateSetDynamicMethod<T, S>(Type type)
		{
			return new DynamicMethodDefinition("DynamicSet_" + type.Name, typeof(void), new Type[]
			{
				typeof(T),
				typeof(S)
			});
		}
	}
}
