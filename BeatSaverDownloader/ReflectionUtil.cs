using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace BeatSaverDownloader
{
	// Token: 0x0200000F RID: 15
	public static class ReflectionUtil
	{
		// Token: 0x060000B4 RID: 180 RVA: 0x000034BF File Offset: 0x000016BF
		public static void SetField(this object obj, string fieldName, object value)
		{
			((obj is Type) ? ((Type)obj) : obj.GetType()).GetField(fieldName, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic).SetValue(obj, value);
		}

		// Token: 0x060000B5 RID: 181 RVA: 0x000034E6 File Offset: 0x000016E6
		public static object GetField(this object obj, string fieldName)
		{
			return ((obj is Type) ? ((Type)obj) : obj.GetType()).GetField(fieldName, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic).GetValue(obj);
		}

		// Token: 0x060000B6 RID: 182 RVA: 0x0000350C File Offset: 0x0000170C
		public static T GetField<T>(this object obj, string fieldName)
		{
			return (T)((object)obj.GetField(fieldName));
		}

		// Token: 0x060000B7 RID: 183 RVA: 0x0000351A File Offset: 0x0000171A
		public static void SetProperty(this object obj, string propertyName, object value)
		{
			((obj is Type) ? ((Type)obj) : obj.GetType()).GetProperty(propertyName, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic).SetValue(obj, value, null);
		}

		// Token: 0x060000B8 RID: 184 RVA: 0x00003542 File Offset: 0x00001742
		public static object GetProperty(this object obj, string propertyName)
		{
			return ((obj is Type) ? ((Type)obj) : obj.GetType()).GetProperty(propertyName, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic).GetValue(obj);
		}

		// Token: 0x060000B9 RID: 185 RVA: 0x00003568 File Offset: 0x00001768
		public static T GetProperty<T>(this object obj, string propertyName)
		{
			return (T)((object)obj.GetProperty(propertyName));
		}

		// Token: 0x060000BA RID: 186 RVA: 0x00003576 File Offset: 0x00001776
		public static T InvokeMethod<T>(this object obj, string methodName, params object[] methodParams)
		{
			return (T)((object)obj.InvokeMethod(methodName, methodParams));
		}

		// Token: 0x060000BB RID: 187 RVA: 0x00003585 File Offset: 0x00001785
		public static object InvokeMethod(this object obj, string methodName, params object[] methodParams)
		{
			return ((obj is Type) ? ((Type)obj) : obj.GetType()).GetMethod(methodName, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic).Invoke(obj, methodParams);
		}

		// Token: 0x060000BC RID: 188 RVA: 0x000035AC File Offset: 0x000017AC
		public static object InvokeConstructor(this object obj, params object[] constructorParams)
		{
			Type[] array = new Type[constructorParams.Length];
			for (int i = 0; i < constructorParams.Length; i++)
			{
				array[i] = constructorParams[i].GetType();
			}
			return ((obj is Type) ? ((Type)obj) : obj.GetType()).GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, array, null).Invoke(constructorParams);
		}

		// Token: 0x060000BD RID: 189 RVA: 0x00003601 File Offset: 0x00001801
		public static Type GetStaticType(string clazz)
		{
			return Type.GetType(clazz);
		}

		// Token: 0x060000BE RID: 190 RVA: 0x00003609 File Offset: 0x00001809
		public static IEnumerable<Assembly> ListLoadedAssemblies()
		{
			return AppDomain.CurrentDomain.GetAssemblies();
		}

		// Token: 0x060000BF RID: 191 RVA: 0x00003618 File Offset: 0x00001818
		public static IEnumerable<string> ListNamespacesInAssembly(Assembly assembly)
		{
			return Enumerable.Empty<string>().Concat(from n in (from t in assembly.GetTypes()
					select t.Namespace).Distinct<string>()
				where n != null
				select n).Distinct<string>();
		}

		// Token: 0x060000C0 RID: 192 RVA: 0x00003688 File Offset: 0x00001888
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

		// Token: 0x060000C1 RID: 193 RVA: 0x00003748 File Offset: 0x00001948
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

		// Token: 0x060000C2 RID: 194 RVA: 0x000037B0 File Offset: 0x000019B0
		public static void SetPrivateField(this object obj, string fieldName, object value)
		{
			obj.GetType().GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic).SetValue(obj, value);
		}

		// Token: 0x060000C3 RID: 195 RVA: 0x000037C7 File Offset: 0x000019C7
		public static T GetPrivateField<T>(this object obj, string fieldName)
		{
			return (T)((object)obj.GetType().GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic).GetValue(obj));
		}

		// Token: 0x060000C4 RID: 196 RVA: 0x000037E2 File Offset: 0x000019E2
		public static object GetPrivateField(Type type, object obj, string fieldName)
		{
			return obj.GetType().GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic).GetValue(obj);
		}

		// Token: 0x060000C5 RID: 197 RVA: 0x000037F8 File Offset: 0x000019F8
		public static void InvokePrivateMethod(this object obj, string methodName, object[] methodParams)
		{
			obj.GetType().GetMethod(methodName, BindingFlags.Instance | BindingFlags.NonPublic).Invoke(obj, methodParams);
		}

		// Token: 0x060000C6 RID: 198 RVA: 0x00003810 File Offset: 0x00001A10
		private static void CopyForType(Type type, Component source, Component destination)
		{
			foreach (FieldInfo fieldInfo in type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.GetField | BindingFlags.SetField))
			{
				fieldInfo.SetValue(destination, fieldInfo.GetValue(source));
			}
		}

		// Token: 0x04000022 RID: 34
		private const BindingFlags _allBindingFlags = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;
	}
}
