using System;
using System.Reflection;
using UnityEngine;

namespace NetViosCommon.Utility
{
	// Token: 0x02000008 RID: 8
	public static class ReflectionUtil
	{
		// Token: 0x06000017 RID: 23 RVA: 0x00002263 File Offset: 0x00000463
		public static void SetPrivateField(this object obj, string fieldName, object value)
		{
			obj.GetType().GetField(fieldName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).SetValue(obj, value);
		}

		// Token: 0x06000018 RID: 24 RVA: 0x0000227A File Offset: 0x0000047A
		public static T GetPrivateField<T>(this object obj, string fieldName)
		{
			return (T)((object)obj.GetType().GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic).GetValue(obj));
		}

		// Token: 0x06000019 RID: 25 RVA: 0x00002295 File Offset: 0x00000495
		public static void SetStaticPrivateField(this Type type, string fieldName, object value)
		{
			type.GetField(fieldName, BindingFlags.Static | BindingFlags.NonPublic).SetValue(null, value);
		}

		// Token: 0x0600001A RID: 26 RVA: 0x000022A7 File Offset: 0x000004A7
		public static T GetStaticPrivateField<T>(this Type type, string fieldName)
		{
			return (T)((object)type.GetField(fieldName, BindingFlags.Static | BindingFlags.NonPublic).GetValue(null));
		}

		// Token: 0x0600001B RID: 27 RVA: 0x000022BD File Offset: 0x000004BD
		public static T GetPrivateProperty<T>(this object obj, string propertyName)
		{
			return (T)((object)obj.GetType().GetProperty(propertyName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).GetValue(obj));
		}

		// Token: 0x0600001C RID: 28 RVA: 0x000022D8 File Offset: 0x000004D8
		public static void SetPrivateProperty(this object obj, string propertyName, object value)
		{
			obj.GetType().GetProperty(propertyName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).SetValue(obj, value, null);
		}

		// Token: 0x0600001D RID: 29 RVA: 0x000022F0 File Offset: 0x000004F0
		public static void InvokePrivateMethod(this object obj, string methodName, object[] methodParams)
		{
			obj.GetType().GetMethod(methodName, BindingFlags.Instance | BindingFlags.NonPublic).Invoke(obj, methodParams);
		}

		// Token: 0x0600001E RID: 30 RVA: 0x00002308 File Offset: 0x00000508
		public static void InvokePrivateStaticMethod(Type type, string methodName, object[] methodParams)
		{
			type.GetMethod(methodName, BindingFlags.Static | BindingFlags.NonPublic).Invoke(null, methodParams);
		}

		// Token: 0x0600001F RID: 31 RVA: 0x0000231C File Offset: 0x0000051C
		public static Component CopyComponent(Component original, Type originalType, Type overridingType, GameObject destination)
		{
			Component component = destination.AddComponent(overridingType);
			foreach (FieldInfo fieldInfo in originalType.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.GetField))
			{
				fieldInfo.SetValue(component, fieldInfo.GetValue(original));
			}
			return component;
		}
	}
}
