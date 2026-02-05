using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace NetviosHelperPlugin
{
	// Token: 0x02000005 RID: 5
	public static class ReflectionUtil
	{
		// Token: 0x0600001D RID: 29 RVA: 0x00002704 File Offset: 0x00000904
		public static void SetField(this object obj, string fieldName, object value)
		{
			((obj is Type) ? ((Type)obj) : obj.GetType()).GetField(fieldName, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic).SetValue(obj, value);
		}

		// Token: 0x0600001E RID: 30 RVA: 0x00002730 File Offset: 0x00000930
		public static object GetField(this object obj, string fieldName)
		{
			return ((obj is Type) ? ((Type)obj) : obj.GetType()).GetField(fieldName, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic).GetValue(obj);
		}

		// Token: 0x0600001F RID: 31 RVA: 0x00002766 File Offset: 0x00000966
		public static T GetField<T>(this object obj, string fieldName)
		{
			return (T)((object)obj.GetField(fieldName));
		}

		// Token: 0x06000020 RID: 32 RVA: 0x00002774 File Offset: 0x00000974
		public static void SetProperty(this object obj, string propertyName, object value)
		{
			((obj is Type) ? ((Type)obj) : obj.GetType()).GetProperty(propertyName, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic).SetValue(obj, value, null);
		}

		// Token: 0x06000021 RID: 33 RVA: 0x000027A0 File Offset: 0x000009A0
		public static object GetProperty(this object obj, string propertyName)
		{
			return ((obj is Type) ? ((Type)obj) : obj.GetType()).GetProperty(propertyName, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic).GetValue(obj);
		}

		// Token: 0x06000022 RID: 34 RVA: 0x000027D6 File Offset: 0x000009D6
		public static T GetProperty<T>(this object obj, string propertyName)
		{
			return (T)((object)obj.GetProperty(propertyName));
		}

		// Token: 0x06000023 RID: 35 RVA: 0x000027E4 File Offset: 0x000009E4
		public static T InvokeMethod<T>(this object obj, string methodName, params object[] methodParams)
		{
			return (T)((object)obj.InvokeMethod(methodName, methodParams));
		}

		// Token: 0x06000024 RID: 36 RVA: 0x000027F4 File Offset: 0x000009F4
		public static object InvokeMethod(this object obj, string methodName, params object[] methodParams)
		{
			return ((obj is Type) ? ((Type)obj) : obj.GetType()).GetMethod(methodName, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic).Invoke(obj, methodParams);
		}

		// Token: 0x06000025 RID: 37 RVA: 0x0000282C File Offset: 0x00000A2C
		public static object InvokeConstructor(this object obj, params object[] constructorParams)
		{
			Type[] array = new Type[constructorParams.Length];
			for (int i = 0; i < constructorParams.Length; i++)
			{
				array[i] = constructorParams[i].GetType();
			}
			return ((obj is Type) ? ((Type)obj) : obj.GetType()).GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, array, null).Invoke(constructorParams);
		}

		// Token: 0x06000026 RID: 38 RVA: 0x0000288C File Offset: 0x00000A8C
		public static Type GetStaticType(string clazz)
		{
			return Type.GetType(clazz);
		}

		// Token: 0x06000027 RID: 39 RVA: 0x000028A4 File Offset: 0x00000AA4
		public static IEnumerable<Assembly> ListLoadedAssemblies()
		{
			return AppDomain.CurrentDomain.GetAssemblies();
		}

		// Token: 0x06000028 RID: 40 RVA: 0x000028C0 File Offset: 0x00000AC0
		public static IEnumerable<string> ListNamespacesInAssembly(Assembly assembly)
		{
			IEnumerable<string> enumerable = Enumerable.Empty<string>();
			enumerable = enumerable.Concat(from n in (from t in assembly.GetTypes()
					select t.Namespace).Distinct<string>()
				where n != null
				select n);
			return enumerable.Distinct<string>();
		}

		// Token: 0x06000029 RID: 41 RVA: 0x00002938 File Offset: 0x00000B38
		public static IEnumerable<string> ListClassesInNamespace(string ns)
		{
			Func<Type, bool> <>9__2;
			foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
			{
				IEnumerable<Type> types = assembly.GetTypes();
				Func<Type, bool> func;
				if ((func = <>9__2) == null)
				{
					func = (<>9__2 = (Type t) => t.Namespace == ns);
				}
				bool flag = types.Where(func).Any<Type>();
				if (flag)
				{
					return from t in assembly.GetTypes()
						where t.IsClass
						select t.Name;
				}
			}
			return null;
		}

		// Token: 0x0600002A RID: 42 RVA: 0x00002A0C File Offset: 0x00000C0C
		public static Behaviour CopyComponent(Behaviour original, Type originalType, Type overridingType, GameObject destination)
		{
			Behaviour behaviour = null;
			try
			{
				behaviour = destination.AddComponent(overridingType) as Behaviour;
			}
			catch (Exception)
			{
			}
			behaviour.enabled = false;
			Type type = originalType;
			while (type != typeof(MonoBehaviour))
			{
				ReflectionUtil.CopyForType(type, original, behaviour);
				type = type.BaseType;
			}
			behaviour.enabled = true;
			return behaviour;
		}

		// Token: 0x0600002B RID: 43 RVA: 0x00002A84 File Offset: 0x00000C84
		public static void SetPrivateField(this object obj, string fieldName, object value)
		{
			FieldInfo field = obj.GetType().GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic);
			field.SetValue(obj, value);
		}

		// Token: 0x0600002C RID: 44 RVA: 0x00002AAC File Offset: 0x00000CAC
		public static T GetPrivateField<T>(this object obj, string fieldName)
		{
			FieldInfo field = obj.GetType().GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic);
			object value = field.GetValue(obj);
			return (T)((object)value);
		}

		// Token: 0x0600002D RID: 45 RVA: 0x00002ADC File Offset: 0x00000CDC
		public static object GetPrivateField(Type type, object obj, string fieldName)
		{
			FieldInfo field = obj.GetType().GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic);
			return field.GetValue(obj);
		}

		// Token: 0x0600002E RID: 46 RVA: 0x00002B08 File Offset: 0x00000D08
		public static void SetParentPrivateField(this object obj, string fieldName, object value)
		{
			FieldInfo field = obj.GetType().BaseType.GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic);
			field.SetValue(obj, value);
		}

		// Token: 0x0600002F RID: 47 RVA: 0x00002B34 File Offset: 0x00000D34
		public static T GetParentPrivateField<T>(this object obj, string fieldName)
		{
			FieldInfo field = obj.GetType().BaseType.GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic);
			object value = field.GetValue(obj);
			return (T)((object)value);
		}

		// Token: 0x06000030 RID: 48 RVA: 0x00002B68 File Offset: 0x00000D68
		public static void InvokePrivateMethod(this object obj, string methodName, object[] methodParams)
		{
			MethodInfo method = obj.GetType().GetMethod(methodName, BindingFlags.Instance | BindingFlags.NonPublic);
			method.Invoke(obj, methodParams);
		}

		// Token: 0x06000031 RID: 49 RVA: 0x00002B90 File Offset: 0x00000D90
		private static void CopyForType(Type type, Component source, Component destination)
		{
			FieldInfo[] fields = type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.GetField | BindingFlags.SetField);
			foreach (FieldInfo fieldInfo in fields)
			{
				fieldInfo.SetValue(destination, fieldInfo.GetValue(source));
			}
		}

		// Token: 0x04000009 RID: 9
		private const BindingFlags _allBindingFlags = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;
	}
}
