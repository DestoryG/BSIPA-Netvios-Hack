using System;
using System.Reflection;
using UnityEngine;

namespace CameraPlus
{
	// Token: 0x0200000C RID: 12
	public static class ReflectionUtil
	{
		// Token: 0x06000067 RID: 103 RVA: 0x000072AD File Offset: 0x000054AD
		public static void SetPrivateField(this object obj, string fieldName, object value)
		{
			obj.GetType().GetField(fieldName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).SetValue(obj, value);
		}

		// Token: 0x06000068 RID: 104 RVA: 0x000072C4 File Offset: 0x000054C4
		public static T GetPrivateField<T>(this object obj, string fieldName)
		{
			return (T)((object)obj.GetType().GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic).GetValue(obj));
		}

		// Token: 0x06000069 RID: 105 RVA: 0x000072DF File Offset: 0x000054DF
		public static void SetPrivateProperty(this object obj, string propertyName, object value)
		{
			obj.GetType().GetProperty(propertyName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).SetValue(obj, value, null);
		}

		// Token: 0x0600006A RID: 106 RVA: 0x000072F7 File Offset: 0x000054F7
		public static void InvokePrivateMethod(this object obj, string methodName, object[] methodParams)
		{
			obj.GetType().GetMethod(methodName, BindingFlags.Instance | BindingFlags.NonPublic).Invoke(obj, methodParams);
		}

		// Token: 0x0600006B RID: 107 RVA: 0x00007310 File Offset: 0x00005510
		public static Component CopyComponent(Component original, Type overridingType, GameObject destination)
		{
			Component component = destination.AddComponent(overridingType);
			foreach (FieldInfo fieldInfo in original.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.GetField))
			{
				fieldInfo.SetValue(component, fieldInfo.GetValue(original));
			}
			return component;
		}
	}
}
