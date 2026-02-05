using System;
using System.Collections.Generic;
using System.Reflection;

namespace HarmonyLib
{
	// Token: 0x02000009 RID: 9
	internal class AccessCache
	{
		// Token: 0x06000022 RID: 34 RVA: 0x000027F8 File Offset: 0x000009F8
		private static T Get<T>(Dictionary<Type, Dictionary<string, T>> dict, Type type, string name, Func<T> fetcher)
		{
			Dictionary<string, T> dictionary;
			if (!dict.TryGetValue(type, out dictionary))
			{
				dictionary = new Dictionary<string, T>();
				dict[type] = dictionary;
			}
			T t;
			if (!dictionary.TryGetValue(name, out t))
			{
				t = fetcher();
				dictionary[name] = t;
			}
			return t;
		}

		// Token: 0x06000023 RID: 35 RVA: 0x0000283C File Offset: 0x00000A3C
		private static T Get<T>(Dictionary<Type, Dictionary<string, Dictionary<int, T>>> dict, Type type, string name, Type[] arguments, Func<T> fetcher)
		{
			Dictionary<string, Dictionary<int, T>> dictionary;
			if (!dict.TryGetValue(type, out dictionary))
			{
				dictionary = new Dictionary<string, Dictionary<int, T>>();
				dict[type] = dictionary;
			}
			Dictionary<int, T> dictionary2;
			if (!dictionary.TryGetValue(name, out dictionary2))
			{
				dictionary2 = new Dictionary<int, T>();
				dictionary[name] = dictionary2;
			}
			int num = AccessTools.CombinedHashCode(arguments);
			T t;
			if (!dictionary2.TryGetValue(num, out t))
			{
				t = fetcher();
				dictionary2[num] = t;
			}
			return t;
		}

		// Token: 0x06000024 RID: 36 RVA: 0x000028A0 File Offset: 0x00000AA0
		internal FieldInfo GetFieldInfo(Type type, string name, AccessCache.MemberType memberType = AccessCache.MemberType.Any, bool declaredOnly = false)
		{
			FieldInfo fieldInfo = AccessCache.Get<FieldInfo>(this.declaredFields, type, name, () => type.GetField(name, AccessCache.declaredOnlyBindingFlags[memberType]));
			if (fieldInfo == null && !declaredOnly)
			{
				Func<Type, FieldInfo> <>9__2;
				fieldInfo = AccessCache.Get<FieldInfo>(this.inheritedFields, type, name, delegate
				{
					Type type2 = type;
					Func<Type, FieldInfo> func;
					if ((func = <>9__2) == null)
					{
						func = (<>9__2 = (Type t) => t.GetField(name, AccessTools.all));
					}
					return AccessTools.FindIncludingBaseTypes<FieldInfo>(type2, func);
				});
			}
			return fieldInfo;
		}

		// Token: 0x06000025 RID: 37 RVA: 0x00002920 File Offset: 0x00000B20
		internal PropertyInfo GetPropertyInfo(Type type, string name, AccessCache.MemberType memberType = AccessCache.MemberType.Any, bool declaredOnly = false)
		{
			PropertyInfo propertyInfo = AccessCache.Get<PropertyInfo>(this.declaredProperties, type, name, () => type.GetProperty(name, AccessCache.declaredOnlyBindingFlags[memberType]));
			if (propertyInfo == null && !declaredOnly)
			{
				Func<Type, PropertyInfo> <>9__2;
				propertyInfo = AccessCache.Get<PropertyInfo>(this.inheritedProperties, type, name, delegate
				{
					Type type2 = type;
					Func<Type, PropertyInfo> func;
					if ((func = <>9__2) == null)
					{
						func = (<>9__2 = (Type t) => t.GetProperty(name, AccessTools.all));
					}
					return AccessTools.FindIncludingBaseTypes<PropertyInfo>(type2, func);
				});
			}
			return propertyInfo;
		}

		// Token: 0x06000026 RID: 38 RVA: 0x000029A0 File Offset: 0x00000BA0
		internal MethodBase GetMethodInfo(Type type, string name, Type[] arguments, AccessCache.MemberType memberType = AccessCache.MemberType.Any, bool declaredOnly = false)
		{
			MethodBase methodBase = AccessCache.Get<MethodBase>(this.declaredMethods, type, name, arguments, () => type.GetMethod(name, AccessCache.declaredOnlyBindingFlags[memberType]));
			if (methodBase == null && !declaredOnly)
			{
				methodBase = AccessCache.Get<MethodBase>(this.inheritedMethods, type, name, arguments, () => AccessTools.Method(type, name, arguments, null));
			}
			return methodBase;
		}

		// Token: 0x04000003 RID: 3
		private const BindingFlags BasicFlags = BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.GetField | BindingFlags.SetField | BindingFlags.GetProperty | BindingFlags.SetProperty;

		// Token: 0x04000004 RID: 4
		private static readonly Dictionary<AccessCache.MemberType, BindingFlags> declaredOnlyBindingFlags = new Dictionary<AccessCache.MemberType, BindingFlags>
		{
			{
				AccessCache.MemberType.Any,
				BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.GetField | BindingFlags.SetField | BindingFlags.GetProperty | BindingFlags.SetProperty
			},
			{
				AccessCache.MemberType.Instance,
				BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.GetField | BindingFlags.SetField | BindingFlags.GetProperty | BindingFlags.SetProperty
			},
			{
				AccessCache.MemberType.Static,
				BindingFlags.DeclaredOnly | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.GetField | BindingFlags.SetField | BindingFlags.GetProperty | BindingFlags.SetProperty
			}
		};

		// Token: 0x04000005 RID: 5
		private readonly Dictionary<Type, Dictionary<string, FieldInfo>> declaredFields = new Dictionary<Type, Dictionary<string, FieldInfo>>();

		// Token: 0x04000006 RID: 6
		private readonly Dictionary<Type, Dictionary<string, PropertyInfo>> declaredProperties = new Dictionary<Type, Dictionary<string, PropertyInfo>>();

		// Token: 0x04000007 RID: 7
		private readonly Dictionary<Type, Dictionary<string, Dictionary<int, MethodBase>>> declaredMethods = new Dictionary<Type, Dictionary<string, Dictionary<int, MethodBase>>>();

		// Token: 0x04000008 RID: 8
		private readonly Dictionary<Type, Dictionary<string, FieldInfo>> inheritedFields = new Dictionary<Type, Dictionary<string, FieldInfo>>();

		// Token: 0x04000009 RID: 9
		private readonly Dictionary<Type, Dictionary<string, PropertyInfo>> inheritedProperties = new Dictionary<Type, Dictionary<string, PropertyInfo>>();

		// Token: 0x0400000A RID: 10
		private readonly Dictionary<Type, Dictionary<string, Dictionary<int, MethodBase>>> inheritedMethods = new Dictionary<Type, Dictionary<string, Dictionary<int, MethodBase>>>();

		// Token: 0x0200000A RID: 10
		internal enum MemberType
		{
			// Token: 0x0400000C RID: 12
			Any,
			// Token: 0x0400000D RID: 13
			Static,
			// Token: 0x0400000E RID: 14
			Instance
		}
	}
}
