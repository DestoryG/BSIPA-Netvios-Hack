using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace SongCore.Utilities
{
	// Token: 0x02000018 RID: 24
	public static class ReflectionUtil
	{
		// Token: 0x06000133 RID: 307 RVA: 0x0000629D File Offset: 0x0000449D
		public static void SetField(this object obj, string fieldName, object value)
		{
			((obj is Type) ? ((Type)obj) : obj.GetType()).GetField(fieldName, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic).SetValue(obj, value);
		}

		// Token: 0x06000134 RID: 308 RVA: 0x000062C4 File Offset: 0x000044C4
		public static object GetField(this object obj, string fieldName)
		{
			return ((obj is Type) ? ((Type)obj) : obj.GetType()).GetField(fieldName, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic).GetValue(obj);
		}

		// Token: 0x06000135 RID: 309 RVA: 0x000062EA File Offset: 0x000044EA
		public static T GetField<T>(this object obj, string fieldName)
		{
			return (T)((object)obj.GetField(fieldName));
		}

		// Token: 0x06000136 RID: 310 RVA: 0x000062F8 File Offset: 0x000044F8
		public static void SetProperty(this object obj, string propertyName, object value)
		{
			((obj is Type) ? ((Type)obj) : obj.GetType()).GetProperty(propertyName, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic).SetValue(obj, value, null);
		}

		// Token: 0x06000137 RID: 311 RVA: 0x00006320 File Offset: 0x00004520
		public static object GetProperty(this object obj, string propertyName)
		{
			return ((obj is Type) ? ((Type)obj) : obj.GetType()).GetProperty(propertyName, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic).GetValue(obj);
		}

		// Token: 0x06000138 RID: 312 RVA: 0x00006346 File Offset: 0x00004546
		public static T GetProperty<T>(this object obj, string propertyName)
		{
			return (T)((object)obj.GetProperty(propertyName));
		}

		// Token: 0x06000139 RID: 313 RVA: 0x00006354 File Offset: 0x00004554
		public static T InvokeMethod<T>(this object obj, string methodName, params object[] methodParams)
		{
			return (T)((object)obj.InvokeMethod(methodName, methodParams));
		}

		// Token: 0x0600013A RID: 314 RVA: 0x00006363 File Offset: 0x00004563
		public static object InvokeMethod(this object obj, string methodName, params object[] methodParams)
		{
			return ((obj is Type) ? ((Type)obj) : obj.GetType()).GetMethod(methodName, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic).Invoke(obj, methodParams);
		}

		// Token: 0x0600013B RID: 315 RVA: 0x0000638C File Offset: 0x0000458C
		public static object InvokeConstructor(this object obj, params object[] constructorParams)
		{
			Type[] array = new Type[constructorParams.Length];
			for (int i = 0; i < constructorParams.Length; i++)
			{
				array[i] = constructorParams[i].GetType();
			}
			return ((obj is Type) ? ((Type)obj) : obj.GetType()).GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, array, null).Invoke(constructorParams);
		}

		// Token: 0x0600013C RID: 316 RVA: 0x000063E1 File Offset: 0x000045E1
		public static Type GetStaticType(string clazz)
		{
			return Type.GetType(clazz);
		}

		// Token: 0x0600013D RID: 317 RVA: 0x000063E9 File Offset: 0x000045E9
		public static IEnumerable<Assembly> ListLoadedAssemblies()
		{
			return AppDomain.CurrentDomain.GetAssemblies();
		}

		// Token: 0x0600013E RID: 318 RVA: 0x000063F8 File Offset: 0x000045F8
		public static IEnumerable<string> ListNamespacesInAssembly(Assembly assembly)
		{
			return Enumerable.Empty<string>().Concat(from n in (from t in assembly.GetTypes()
					select t.Namespace).Distinct<string>()
				where n != null
				select n).Distinct<string>();
		}

		// Token: 0x0600013F RID: 319 RVA: 0x00006468 File Offset: 0x00004668
		public static IEnumerable<string> ListClassesInNamespace(string ns)
		{
			Func<Type, bool> <>9__0;
			foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
			{
				IEnumerable<Type> types = assembly.GetTypes();
				Func<Type, bool> func;
				if ((func = <>9__0) == null)
				{
					func = (<>9__0 = (Type t) => t.Namespace == ns);
				}
				if (types.Where(func).Any<Type>())
				{
					return from t in assembly.GetTypes()
						where t.IsClass
						select t.Name;
				}
			}
			return null;
		}

		// Token: 0x06000140 RID: 320 RVA: 0x00006528 File Offset: 0x00004728
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

		// Token: 0x06000141 RID: 321 RVA: 0x00006590 File Offset: 0x00004790
		public static void SetPrivateField(this object obj, string fieldName, object value)
		{
			obj.GetType().GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic).SetValue(obj, value);
		}

		// Token: 0x06000142 RID: 322 RVA: 0x000065A7 File Offset: 0x000047A7
		public static T GetPrivateField<T>(this object obj, string fieldName)
		{
			return (T)((object)obj.GetType().GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic).GetValue(obj));
		}

		// Token: 0x06000143 RID: 323 RVA: 0x000065C2 File Offset: 0x000047C2
		public static object GetPrivateField(Type type, object obj, string fieldName)
		{
			return obj.GetType().GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic).GetValue(obj);
		}

		// Token: 0x06000144 RID: 324 RVA: 0x000065D8 File Offset: 0x000047D8
		public static void SetParentPrivateField(this object obj, string fieldName, object value)
		{
			obj.GetType().BaseType.GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic).SetValue(obj, value);
		}

		// Token: 0x06000145 RID: 325 RVA: 0x000065F4 File Offset: 0x000047F4
		public static T GetParentPrivateField<T>(this object obj, string fieldName)
		{
			return (T)((object)obj.GetType().BaseType.GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic).GetValue(obj));
		}

		// Token: 0x06000146 RID: 326 RVA: 0x00006614 File Offset: 0x00004814
		public static void InvokePrivateMethod(this object obj, string methodName, object[] methodParams)
		{
			obj.GetType().GetMethod(methodName, BindingFlags.Instance | BindingFlags.NonPublic).Invoke(obj, methodParams);
		}

		// Token: 0x06000147 RID: 327 RVA: 0x0000662C File Offset: 0x0000482C
		private static void CopyForType(Type type, Component source, Component destination)
		{
			foreach (FieldInfo fieldInfo in type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.GetField | BindingFlags.SetField))
			{
				fieldInfo.SetValue(destination, fieldInfo.GetValue(source));
			}
		}

		// Token: 0x04000070 RID: 112
		private const BindingFlags _allBindingFlags = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;
	}
}
