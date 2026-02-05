using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace BS_Utils.Utilities
{
	// Token: 0x02000007 RID: 7
	public static class ReflectionUtil
	{
		// Token: 0x0600007D RID: 125 RVA: 0x00003C48 File Offset: 0x00001E48
		public static void SetField(this object obj, string fieldName, object value, Type targetType)
		{
			FieldInfo field = targetType.GetField(fieldName, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
			if (field == null)
			{
				throw new InvalidOperationException(fieldName + " is not a member of " + targetType.Name);
			}
			field.SetValue(obj, value);
		}

		// Token: 0x0600007E RID: 126 RVA: 0x00003C88 File Offset: 0x00001E88
		public static object GetField(this object obj, string fieldName, Type targetType)
		{
			FieldInfo field = targetType.GetField(fieldName, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
			if (field == null)
			{
				throw new InvalidOperationException(fieldName + " is not a member of " + targetType.Name);
			}
			return field.GetValue(obj);
		}

		// Token: 0x0600007F RID: 127 RVA: 0x00003CC8 File Offset: 0x00001EC8
		public static void SetPrivateField(this object obj, string fieldName, object value, Type targetType)
		{
			FieldInfo field = targetType.GetField(fieldName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			if (field == null)
			{
				throw new InvalidOperationException(fieldName + " is not a member of " + targetType.Name);
			}
			field.SetValue(obj, value);
		}

		// Token: 0x06000080 RID: 128 RVA: 0x00003D08 File Offset: 0x00001F08
		public static T GetPrivateField<T>(this object obj, string fieldName, Type targetType)
		{
			FieldInfo field = targetType.GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic);
			if (field == null)
			{
				throw new InvalidOperationException(fieldName + " is not a member of " + targetType.Name);
			}
			object value = field.GetValue(obj);
			return (T)((object)value);
		}

		// Token: 0x06000081 RID: 129 RVA: 0x00003D50 File Offset: 0x00001F50
		public static void SetProperty(this object obj, string propertyName, object value, Type targetType)
		{
			PropertyInfo property = targetType.GetProperty(propertyName, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
			if (property == null)
			{
				throw new InvalidOperationException(propertyName + " is not a member of " + targetType.Name);
			}
			property.SetValue(obj, value);
		}

		// Token: 0x06000082 RID: 130 RVA: 0x00003D90 File Offset: 0x00001F90
		public static object GetProperty(this object obj, string propertyName, Type targetType)
		{
			PropertyInfo property = targetType.GetProperty(propertyName, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
			if (property == null)
			{
				throw new InvalidOperationException(propertyName + " is not a member of " + targetType.Name);
			}
			return property.GetValue(obj);
		}

		// Token: 0x06000083 RID: 131 RVA: 0x00003DD0 File Offset: 0x00001FD0
		public static void SetPrivateField(this object obj, string fieldName, object value)
		{
			obj.SetPrivateField(fieldName, value, obj.GetType());
		}

		// Token: 0x06000084 RID: 132 RVA: 0x00003DE0 File Offset: 0x00001FE0
		public static T GetPrivateField<T>(this object obj, string fieldName)
		{
			return obj.GetPrivateField(fieldName, obj.GetType());
		}

		// Token: 0x06000085 RID: 133 RVA: 0x00003DEF File Offset: 0x00001FEF
		public static void SetField(this object obj, string fieldName, object value)
		{
			obj.SetField(fieldName, value, obj.GetType());
		}

		// Token: 0x06000086 RID: 134 RVA: 0x00003DFF File Offset: 0x00001FFF
		public static object GetField(this object obj, string fieldName)
		{
			return obj.GetField(fieldName, obj.GetType());
		}

		// Token: 0x06000087 RID: 135 RVA: 0x00003E0E File Offset: 0x0000200E
		public static T GetField<T>(this object obj, string fieldName, Type targetType)
		{
			return (T)((object)obj.GetField(fieldName, targetType));
		}

		// Token: 0x06000088 RID: 136 RVA: 0x00003E1D File Offset: 0x0000201D
		public static T GetField<T>(this object obj, string fieldName)
		{
			return (T)((object)obj.GetField(fieldName, obj.GetType()));
		}

		// Token: 0x06000089 RID: 137 RVA: 0x00003E31 File Offset: 0x00002031
		public static void SetProperty(this object obj, string propertyName, object value)
		{
			obj.SetProperty(propertyName, value, obj.GetType());
		}

		// Token: 0x0600008A RID: 138 RVA: 0x00003E41 File Offset: 0x00002041
		public static object GetProperty(this object obj, string propertyName)
		{
			return obj.GetProperty(propertyName, obj.GetType());
		}

		// Token: 0x0600008B RID: 139 RVA: 0x00003E50 File Offset: 0x00002050
		public static T GetProperty<T>(this object obj, string propertyName)
		{
			return (T)((object)obj.GetProperty(propertyName));
		}

		// Token: 0x0600008C RID: 140 RVA: 0x00003E5E File Offset: 0x0000205E
		public static T InvokeMethod<T>(this object obj, string methodName, params object[] methodParams)
		{
			return (T)((object)obj.InvokeMethod(methodName, methodParams));
		}

		// Token: 0x0600008D RID: 141 RVA: 0x00003E70 File Offset: 0x00002070
		public static object InvokeMethod(this object obj, string methodName, params object[] methodParams)
		{
			MethodInfo method = obj.GetType().GetMethod(methodName, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
			if (method == null)
			{
				throw new InvalidOperationException(methodName + " is not a member of " + obj.GetType().Name);
			}
			return method.Invoke(obj, methodParams);
		}

		// Token: 0x0600008E RID: 142 RVA: 0x00003EBC File Offset: 0x000020BC
		public static object InvokeConstructor(this object obj, params object[] constructorParams)
		{
			Type[] array = new Type[constructorParams.Length];
			for (int i = 0; i < constructorParams.Length; i++)
			{
				array[i] = constructorParams[i].GetType();
			}
			return ((obj is Type) ? ((Type)obj) : obj.GetType()).GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, array, null).Invoke(constructorParams);
		}

		// Token: 0x0600008F RID: 143 RVA: 0x00003F11 File Offset: 0x00002111
		public static Type GetStaticType(string clazz)
		{
			return Type.GetType(clazz);
		}

		// Token: 0x06000090 RID: 144 RVA: 0x00003F19 File Offset: 0x00002119
		public static IEnumerable<Assembly> ListLoadedAssemblies()
		{
			return AppDomain.CurrentDomain.GetAssemblies();
		}

		// Token: 0x06000091 RID: 145 RVA: 0x00003F28 File Offset: 0x00002128
		public static IEnumerable<string> ListNamespacesInAssembly(Assembly assembly)
		{
			IEnumerable<string> enumerable = Enumerable.Empty<string>();
			enumerable = enumerable.Concat(from n in (from t in assembly.GetTypes()
					select t.Namespace).Distinct<string>()
				where n != null
				select n);
			return enumerable.Distinct<string>();
		}

		// Token: 0x06000092 RID: 146 RVA: 0x00003F9C File Offset: 0x0000219C
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

		// Token: 0x06000093 RID: 147 RVA: 0x0000405C File Offset: 0x0000225C
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

		// Token: 0x06000094 RID: 148 RVA: 0x000040C4 File Offset: 0x000022C4
		private static void CopyForType(Type type, Component source, Component destination)
		{
			FieldInfo[] fields = type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.GetField | BindingFlags.SetField);
			foreach (FieldInfo fieldInfo in fields)
			{
				fieldInfo.SetValue(destination, fieldInfo.GetValue(source));
			}
		}

		// Token: 0x04000031 RID: 49
		private const BindingFlags _allBindingFlags = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;
	}
}
