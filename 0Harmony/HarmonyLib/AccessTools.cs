using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.ExceptionServices;
using System.Runtime.Serialization;
using MonoMod.Utils;

namespace HarmonyLib
{
	// Token: 0x02000087 RID: 135
	public static class AccessTools
	{
		// Token: 0x06000277 RID: 631 RVA: 0x0000CEF8 File Offset: 0x0000B0F8
		public static Type TypeByName(string name)
		{
			Type type = Type.GetType(name, false);
			IEnumerable<Assembly> enumerable = from a in AppDomain.CurrentDomain.GetAssemblies()
				where !a.FullName.StartsWith("Microsoft.VisualStudio")
				select a;
			if (type == null)
			{
				type = enumerable.SelectMany((Assembly a) => AccessTools.GetTypesFromAssembly(a)).FirstOrDefault((Type t) => t.FullName == name);
			}
			if (type == null)
			{
				type = enumerable.SelectMany((Assembly a) => AccessTools.GetTypesFromAssembly(a)).FirstOrDefault((Type t) => t.Name == name);
			}
			if (type == null && Harmony.DEBUG)
			{
				FileLog.Log("AccessTools.TypeByName: Could not find type named " + name);
			}
			return type;
		}

		// Token: 0x06000278 RID: 632 RVA: 0x0000CFF4 File Offset: 0x0000B1F4
		public static Type[] GetTypesFromAssembly(Assembly assembly)
		{
			return assembly.GetTypes();
		}

		// Token: 0x06000279 RID: 633 RVA: 0x0000CFFC File Offset: 0x0000B1FC
		public static T FindIncludingBaseTypes<T>(Type type, Func<Type, T> func) where T : class
		{
			T t;
			for (;;)
			{
				t = func(type);
				if (t != null)
				{
					break;
				}
				if (type == typeof(object))
				{
					goto Block_1;
				}
				type = type.BaseType;
			}
			return t;
			Block_1:
			return default(T);
		}

		// Token: 0x0600027A RID: 634 RVA: 0x0000D040 File Offset: 0x0000B240
		public static T FindIncludingInnerTypes<T>(Type type, Func<Type, T> func) where T : class
		{
			T t = func(type);
			if (t != null)
			{
				return t;
			}
			Type[] nestedTypes = type.GetNestedTypes(AccessTools.all);
			for (int i = 0; i < nestedTypes.Length; i++)
			{
				t = AccessTools.FindIncludingInnerTypes<T>(nestedTypes[i], func);
				if (t != null)
				{
					break;
				}
			}
			return t;
		}

		// Token: 0x0600027B RID: 635 RVA: 0x0000D08C File Offset: 0x0000B28C
		public static FieldInfo DeclaredField(Type type, string name)
		{
			if (type == null)
			{
				if (Harmony.DEBUG)
				{
					FileLog.Log("AccessTools.DeclaredField: type is null");
				}
				return null;
			}
			if (name == null)
			{
				if (Harmony.DEBUG)
				{
					FileLog.Log("AccessTools.DeclaredField: name is null");
				}
				return null;
			}
			FieldInfo field = type.GetField(name, AccessTools.allDeclared);
			if (field == null && Harmony.DEBUG)
			{
				FileLog.Log(string.Format("AccessTools.DeclaredField: Could not find field for type {0} and name {1}", type, name));
			}
			return field;
		}

		// Token: 0x0600027C RID: 636 RVA: 0x0000D0F8 File Offset: 0x0000B2F8
		public static FieldInfo Field(Type type, string name)
		{
			if (type == null)
			{
				if (Harmony.DEBUG)
				{
					FileLog.Log("AccessTools.Field: type is null");
				}
				return null;
			}
			if (name == null)
			{
				if (Harmony.DEBUG)
				{
					FileLog.Log("AccessTools.Field: name is null");
				}
				return null;
			}
			FieldInfo fieldInfo = AccessTools.FindIncludingBaseTypes<FieldInfo>(type, (Type t) => t.GetField(name, AccessTools.all));
			if (fieldInfo == null && Harmony.DEBUG)
			{
				FileLog.Log(string.Format("AccessTools.Field: Could not find field for type {0} and name {1}", type, name));
			}
			return fieldInfo;
		}

		// Token: 0x0600027D RID: 637 RVA: 0x0000D184 File Offset: 0x0000B384
		public static FieldInfo DeclaredField(Type type, int idx)
		{
			if (type == null)
			{
				if (Harmony.DEBUG)
				{
					FileLog.Log("AccessTools.DeclaredField: type is null");
				}
				return null;
			}
			FieldInfo fieldInfo = AccessTools.GetDeclaredFields(type).ElementAtOrDefault(idx);
			if (fieldInfo == null && Harmony.DEBUG)
			{
				FileLog.Log(string.Format("AccessTools.DeclaredField: Could not find field for type {0} and idx {1}", type, idx));
			}
			return fieldInfo;
		}

		// Token: 0x0600027E RID: 638 RVA: 0x0000D1E0 File Offset: 0x0000B3E0
		public static PropertyInfo DeclaredProperty(Type type, string name)
		{
			if (type == null)
			{
				if (Harmony.DEBUG)
				{
					FileLog.Log("AccessTools.DeclaredProperty: type is null");
				}
				return null;
			}
			if (name == null)
			{
				if (Harmony.DEBUG)
				{
					FileLog.Log("AccessTools.DeclaredProperty: name is null");
				}
				return null;
			}
			PropertyInfo property = type.GetProperty(name, AccessTools.allDeclared);
			if (property == null && Harmony.DEBUG)
			{
				FileLog.Log(string.Format("AccessTools.DeclaredProperty: Could not find property for type {0} and name {1}", type, name));
			}
			return property;
		}

		// Token: 0x0600027F RID: 639 RVA: 0x0000D24C File Offset: 0x0000B44C
		public static MethodInfo DeclaredPropertyGetter(Type type, string name)
		{
			PropertyInfo propertyInfo = AccessTools.DeclaredProperty(type, name);
			if (propertyInfo == null)
			{
				return null;
			}
			return propertyInfo.GetGetMethod(true);
		}

		// Token: 0x06000280 RID: 640 RVA: 0x0000D261 File Offset: 0x0000B461
		public static MethodInfo DeclaredPropertySetter(Type type, string name)
		{
			PropertyInfo propertyInfo = AccessTools.DeclaredProperty(type, name);
			if (propertyInfo == null)
			{
				return null;
			}
			return propertyInfo.GetSetMethod(true);
		}

		// Token: 0x06000281 RID: 641 RVA: 0x0000D278 File Offset: 0x0000B478
		public static PropertyInfo Property(Type type, string name)
		{
			if (type == null)
			{
				if (Harmony.DEBUG)
				{
					FileLog.Log("AccessTools.Property: type is null");
				}
				return null;
			}
			if (name == null)
			{
				if (Harmony.DEBUG)
				{
					FileLog.Log("AccessTools.Property: name is null");
				}
				return null;
			}
			PropertyInfo propertyInfo = AccessTools.FindIncludingBaseTypes<PropertyInfo>(type, (Type t) => t.GetProperty(name, AccessTools.all));
			if (propertyInfo == null && Harmony.DEBUG)
			{
				FileLog.Log(string.Format("AccessTools.Property: Could not find property for type {0} and name {1}", type, name));
			}
			return propertyInfo;
		}

		// Token: 0x06000282 RID: 642 RVA: 0x0000D301 File Offset: 0x0000B501
		public static MethodInfo PropertyGetter(Type type, string name)
		{
			PropertyInfo propertyInfo = AccessTools.Property(type, name);
			if (propertyInfo == null)
			{
				return null;
			}
			return propertyInfo.GetGetMethod(true);
		}

		// Token: 0x06000283 RID: 643 RVA: 0x0000D316 File Offset: 0x0000B516
		public static MethodInfo PropertySetter(Type type, string name)
		{
			PropertyInfo propertyInfo = AccessTools.Property(type, name);
			if (propertyInfo == null)
			{
				return null;
			}
			return propertyInfo.GetSetMethod(true);
		}

		// Token: 0x06000284 RID: 644 RVA: 0x0000D32C File Offset: 0x0000B52C
		public static MethodInfo DeclaredMethod(Type type, string name, Type[] parameters = null, Type[] generics = null)
		{
			if (type == null)
			{
				if (Harmony.DEBUG)
				{
					FileLog.Log("AccessTools.DeclaredMethod: type is null");
				}
				return null;
			}
			if (name == null)
			{
				if (Harmony.DEBUG)
				{
					FileLog.Log("AccessTools.DeclaredMethod: name is null");
				}
				return null;
			}
			ParameterModifier[] array = new ParameterModifier[0];
			MethodInfo methodInfo;
			if (parameters == null)
			{
				methodInfo = type.GetMethod(name, AccessTools.allDeclared);
			}
			else
			{
				methodInfo = type.GetMethod(name, AccessTools.allDeclared, null, parameters, array);
			}
			if (methodInfo == null)
			{
				if (Harmony.DEBUG)
				{
					FileLog.Log(string.Format("AccessTools.DeclaredMethod: Could not find method for type {0} and name {1} and parameters {2}", type, name, (parameters != null) ? parameters.Description() : null));
				}
				return null;
			}
			if (generics != null)
			{
				methodInfo = methodInfo.MakeGenericMethod(generics);
			}
			return methodInfo;
		}

		// Token: 0x06000285 RID: 645 RVA: 0x0000D3D0 File Offset: 0x0000B5D0
		public static MethodInfo Method(Type type, string name, Type[] parameters = null, Type[] generics = null)
		{
			if (type == null)
			{
				if (Harmony.DEBUG)
				{
					FileLog.Log("AccessTools.Method: type is null");
				}
				return null;
			}
			if (name == null)
			{
				if (Harmony.DEBUG)
				{
					FileLog.Log("AccessTools.Method: name is null");
				}
				return null;
			}
			ParameterModifier[] modifiers = new ParameterModifier[0];
			MethodInfo methodInfo;
			if (parameters == null)
			{
				try
				{
					methodInfo = AccessTools.FindIncludingBaseTypes<MethodInfo>(type, (Type t) => t.GetMethod(name, AccessTools.all));
					goto IL_00CF;
				}
				catch (AmbiguousMatchException ex)
				{
					methodInfo = AccessTools.FindIncludingBaseTypes<MethodInfo>(type, (Type t) => t.GetMethod(name, AccessTools.all, null, new Type[0], modifiers));
					if (methodInfo == null)
					{
						string text = string.Format("Ambiguous match in Harmony patch for {0}:{1}.", type, name);
						AmbiguousMatchException ex2 = ex;
						throw new AmbiguousMatchException(text + ((ex2 != null) ? ex2.ToString() : null));
					}
					goto IL_00CF;
				}
			}
			methodInfo = AccessTools.FindIncludingBaseTypes<MethodInfo>(type, (Type t) => t.GetMethod(name, AccessTools.all, null, parameters, modifiers));
			IL_00CF:
			if (methodInfo == null)
			{
				if (Harmony.DEBUG)
				{
					string text2 = "AccessTools.Method: Could not find method for type {0} and name {1} and parameters {2}";
					object name2 = name;
					Type[] parameters2 = parameters;
					FileLog.Log(string.Format(text2, type, name2, (parameters2 != null) ? parameters2.Description() : null));
				}
				return null;
			}
			if (generics != null)
			{
				methodInfo = methodInfo.MakeGenericMethod(generics);
			}
			return methodInfo;
		}

		// Token: 0x06000286 RID: 646 RVA: 0x0000D504 File Offset: 0x0000B704
		public static MethodInfo Method(string typeColonMethodname, Type[] parameters = null, Type[] generics = null)
		{
			if (typeColonMethodname == null)
			{
				if (Harmony.DEBUG)
				{
					FileLog.Log("AccessTools.Method: typeColonMethodname is null");
				}
				return null;
			}
			string[] array = typeColonMethodname.Split(new char[] { ':' });
			if (array.Length != 2)
			{
				throw new ArgumentException("Method must be specified as 'Namespace.Type1.Type2:MethodName", "typeColonMethodname");
			}
			return AccessTools.DeclaredMethod(AccessTools.TypeByName(array[0]), array[1], parameters, generics);
		}

		// Token: 0x06000287 RID: 647 RVA: 0x0000D564 File Offset: 0x0000B764
		public static List<string> GetMethodNames(Type type)
		{
			if (type == null)
			{
				if (Harmony.DEBUG)
				{
					FileLog.Log("AccessTools.GetMethodNames: type is null");
				}
				return new List<string>();
			}
			return (from m in AccessTools.GetDeclaredMethods(type)
				select m.Name).ToList<string>();
		}

		// Token: 0x06000288 RID: 648 RVA: 0x0000D5C0 File Offset: 0x0000B7C0
		public static List<string> GetMethodNames(object instance)
		{
			if (instance == null)
			{
				if (Harmony.DEBUG)
				{
					FileLog.Log("AccessTools.GetMethodNames: instance is null");
				}
				return new List<string>();
			}
			return AccessTools.GetMethodNames(instance.GetType());
		}

		// Token: 0x06000289 RID: 649 RVA: 0x0000D5E8 File Offset: 0x0000B7E8
		public static List<string> GetFieldNames(Type type)
		{
			if (type == null)
			{
				if (Harmony.DEBUG)
				{
					FileLog.Log("AccessTools.GetFieldNames: type is null");
				}
				return new List<string>();
			}
			return (from f in AccessTools.GetDeclaredFields(type)
				select f.Name).ToList<string>();
		}

		// Token: 0x0600028A RID: 650 RVA: 0x0000D644 File Offset: 0x0000B844
		public static List<string> GetFieldNames(object instance)
		{
			if (instance == null)
			{
				if (Harmony.DEBUG)
				{
					FileLog.Log("AccessTools.GetFieldNames: instance is null");
				}
				return new List<string>();
			}
			return AccessTools.GetFieldNames(instance.GetType());
		}

		// Token: 0x0600028B RID: 651 RVA: 0x0000D66C File Offset: 0x0000B86C
		public static List<string> GetPropertyNames(Type type)
		{
			if (type == null)
			{
				if (Harmony.DEBUG)
				{
					FileLog.Log("AccessTools.GetPropertyNames: type is null");
				}
				return new List<string>();
			}
			return (from f in AccessTools.GetDeclaredProperties(type)
				select f.Name).ToList<string>();
		}

		// Token: 0x0600028C RID: 652 RVA: 0x0000D6C8 File Offset: 0x0000B8C8
		public static List<string> GetPropertyNames(object instance)
		{
			if (instance == null)
			{
				if (Harmony.DEBUG)
				{
					FileLog.Log("AccessTools.GetPropertyNames: instance is null");
				}
				return new List<string>();
			}
			return AccessTools.GetPropertyNames(instance.GetType());
		}

		// Token: 0x0600028D RID: 653 RVA: 0x0000D6F0 File Offset: 0x0000B8F0
		public static Type GetUnderlyingType(this MemberInfo member)
		{
			MemberTypes memberType = member.MemberType;
			if (memberType <= MemberTypes.Field)
			{
				if (memberType == MemberTypes.Event)
				{
					return ((EventInfo)member).EventHandlerType;
				}
				if (memberType == MemberTypes.Field)
				{
					return ((FieldInfo)member).FieldType;
				}
			}
			else
			{
				if (memberType == MemberTypes.Method)
				{
					return ((MethodInfo)member).ReturnType;
				}
				if (memberType == MemberTypes.Property)
				{
					return ((PropertyInfo)member).PropertyType;
				}
			}
			throw new ArgumentException("Member must be of type EventInfo, FieldInfo, MethodInfo, or PropertyInfo");
		}

		// Token: 0x0600028E RID: 654 RVA: 0x0000D757 File Offset: 0x0000B957
		public static bool IsDeclaredMember<T>(this T member) where T : MemberInfo
		{
			return member.DeclaringType == member.ReflectedType;
		}

		// Token: 0x0600028F RID: 655 RVA: 0x0000D774 File Offset: 0x0000B974
		public static T GetDeclaredMember<T>(this T member) where T : MemberInfo
		{
			if (member.DeclaringType == null || member.IsDeclaredMember<T>())
			{
				return member;
			}
			int metadataToken = member.MetadataToken;
			foreach (MemberInfo memberInfo in member.DeclaringType.GetMembers(AccessTools.all))
			{
				if (memberInfo.MetadataToken == metadataToken)
				{
					return (T)((object)memberInfo);
				}
			}
			return member;
		}

		// Token: 0x06000290 RID: 656 RVA: 0x0000D7E4 File Offset: 0x0000B9E4
		public static ConstructorInfo DeclaredConstructor(Type type, Type[] parameters = null, bool searchForStatic = false)
		{
			if (type == null)
			{
				if (Harmony.DEBUG)
				{
					FileLog.Log("AccessTools.DeclaredConstructor: type is null");
				}
				return null;
			}
			if (parameters == null)
			{
				parameters = new Type[0];
			}
			BindingFlags bindingFlags = (searchForStatic ? (AccessTools.allDeclared & ~BindingFlags.Instance) : (AccessTools.allDeclared & ~BindingFlags.Static));
			return type.GetConstructor(bindingFlags, null, parameters, new ParameterModifier[0]);
		}

		// Token: 0x06000291 RID: 657 RVA: 0x0000D840 File Offset: 0x0000BA40
		public static ConstructorInfo Constructor(Type type, Type[] parameters = null, bool searchForStatic = false)
		{
			if (type == null)
			{
				if (Harmony.DEBUG)
				{
					FileLog.Log("AccessTools.ConstructorInfo: type is null");
				}
				return null;
			}
			if (parameters == null)
			{
				parameters = new Type[0];
			}
			BindingFlags flags = (searchForStatic ? (AccessTools.all & ~BindingFlags.Instance) : (AccessTools.all & ~BindingFlags.Static));
			return AccessTools.FindIncludingBaseTypes<ConstructorInfo>(type, (Type t) => t.GetConstructor(flags, null, parameters, new ParameterModifier[0]));
		}

		// Token: 0x06000292 RID: 658 RVA: 0x0000D8B8 File Offset: 0x0000BAB8
		public static List<ConstructorInfo> GetDeclaredConstructors(Type type, bool? searchForStatic = null)
		{
			if (type == null)
			{
				if (Harmony.DEBUG)
				{
					FileLog.Log("AccessTools.GetDeclaredConstructors: type is null");
				}
				return null;
			}
			BindingFlags bindingFlags = AccessTools.allDeclared;
			if (searchForStatic != null)
			{
				bindingFlags = (searchForStatic.Value ? (bindingFlags & ~BindingFlags.Instance) : (bindingFlags & ~BindingFlags.Static));
			}
			return (from method in type.GetConstructors(bindingFlags)
				where method.DeclaringType == type
				select method).ToList<ConstructorInfo>();
		}

		// Token: 0x06000293 RID: 659 RVA: 0x0000D938 File Offset: 0x0000BB38
		public static List<MethodInfo> GetDeclaredMethods(Type type)
		{
			if (type == null)
			{
				if (Harmony.DEBUG)
				{
					FileLog.Log("AccessTools.GetDeclaredMethods: type is null");
				}
				return null;
			}
			return type.GetMethods(AccessTools.allDeclared).ToList<MethodInfo>();
		}

		// Token: 0x06000294 RID: 660 RVA: 0x0000D966 File Offset: 0x0000BB66
		public static List<PropertyInfo> GetDeclaredProperties(Type type)
		{
			if (type == null)
			{
				if (Harmony.DEBUG)
				{
					FileLog.Log("AccessTools.GetDeclaredProperties: type is null");
				}
				return null;
			}
			return type.GetProperties(AccessTools.allDeclared).ToList<PropertyInfo>();
		}

		// Token: 0x06000295 RID: 661 RVA: 0x0000D994 File Offset: 0x0000BB94
		public static List<FieldInfo> GetDeclaredFields(Type type)
		{
			if (type == null)
			{
				if (Harmony.DEBUG)
				{
					FileLog.Log("AccessTools.GetDeclaredFields: type is null");
				}
				return null;
			}
			return type.GetFields(AccessTools.allDeclared).ToList<FieldInfo>();
		}

		// Token: 0x06000296 RID: 662 RVA: 0x0000D9C4 File Offset: 0x0000BBC4
		public static Type GetReturnedType(MethodBase methodOrConstructor)
		{
			if (methodOrConstructor == null)
			{
				if (Harmony.DEBUG)
				{
					FileLog.Log("AccessTools.GetReturnedType: methodOrConstructor is null");
				}
				return null;
			}
			if (methodOrConstructor as ConstructorInfo != null)
			{
				return typeof(void);
			}
			return ((MethodInfo)methodOrConstructor).ReturnType;
		}

		// Token: 0x06000297 RID: 663 RVA: 0x0000DA14 File Offset: 0x0000BC14
		public static Type Inner(Type type, string name)
		{
			if (type == null)
			{
				if (Harmony.DEBUG)
				{
					FileLog.Log("AccessTools.Inner: type is null");
				}
				return null;
			}
			if (name == null)
			{
				if (Harmony.DEBUG)
				{
					FileLog.Log("AccessTools.Inner: name is null");
				}
				return null;
			}
			return AccessTools.FindIncludingBaseTypes<Type>(type, (Type t) => t.GetNestedType(name, AccessTools.all));
		}

		// Token: 0x06000298 RID: 664 RVA: 0x0000DA78 File Offset: 0x0000BC78
		public static Type FirstInner(Type type, Func<Type, bool> predicate)
		{
			if (type == null)
			{
				if (Harmony.DEBUG)
				{
					FileLog.Log("AccessTools.FirstInner: type is null");
				}
				return null;
			}
			if (predicate == null)
			{
				if (Harmony.DEBUG)
				{
					FileLog.Log("AccessTools.FirstInner: predicate is null");
				}
				return null;
			}
			return type.GetNestedTypes(AccessTools.all).FirstOrDefault((Type subType) => predicate(subType));
		}

		// Token: 0x06000299 RID: 665 RVA: 0x0000DAE8 File Offset: 0x0000BCE8
		public static MethodInfo FirstMethod(Type type, Func<MethodInfo, bool> predicate)
		{
			if (type == null)
			{
				if (Harmony.DEBUG)
				{
					FileLog.Log("AccessTools.FirstMethod: type is null");
				}
				return null;
			}
			if (predicate == null)
			{
				if (Harmony.DEBUG)
				{
					FileLog.Log("AccessTools.FirstMethod: predicate is null");
				}
				return null;
			}
			return type.GetMethods(AccessTools.allDeclared).FirstOrDefault((MethodInfo method) => predicate(method));
		}

		// Token: 0x0600029A RID: 666 RVA: 0x0000DB58 File Offset: 0x0000BD58
		public static ConstructorInfo FirstConstructor(Type type, Func<ConstructorInfo, bool> predicate)
		{
			if (type == null)
			{
				if (Harmony.DEBUG)
				{
					FileLog.Log("AccessTools.FirstConstructor: type is null");
				}
				return null;
			}
			if (predicate == null)
			{
				if (Harmony.DEBUG)
				{
					FileLog.Log("AccessTools.FirstConstructor: predicate is null");
				}
				return null;
			}
			return type.GetConstructors(AccessTools.allDeclared).FirstOrDefault((ConstructorInfo constructor) => predicate(constructor));
		}

		// Token: 0x0600029B RID: 667 RVA: 0x0000DBC8 File Offset: 0x0000BDC8
		public static PropertyInfo FirstProperty(Type type, Func<PropertyInfo, bool> predicate)
		{
			if (type == null)
			{
				if (Harmony.DEBUG)
				{
					FileLog.Log("AccessTools.FirstProperty: type is null");
				}
				return null;
			}
			if (predicate == null)
			{
				if (Harmony.DEBUG)
				{
					FileLog.Log("AccessTools.FirstProperty: predicate is null");
				}
				return null;
			}
			return type.GetProperties(AccessTools.allDeclared).FirstOrDefault((PropertyInfo property) => predicate(property));
		}

		// Token: 0x0600029C RID: 668 RVA: 0x0000DC35 File Offset: 0x0000BE35
		public static Type[] GetTypes(object[] parameters)
		{
			if (parameters == null)
			{
				return new Type[0];
			}
			return parameters.Select(delegate(object p)
			{
				if (p != null)
				{
					return p.GetType();
				}
				return typeof(object);
			}).ToArray<Type>();
		}

		// Token: 0x0600029D RID: 669 RVA: 0x0000DC6C File Offset: 0x0000BE6C
		public static object[] ActualParameters(MethodBase method, object[] inputs)
		{
			List<Type> inputTypes = inputs.Select(delegate(object obj)
			{
				if (obj == null)
				{
					return null;
				}
				return obj.GetType();
			}).ToList<Type>();
			return (from p in method.GetParameters()
				select p.ParameterType).Select(delegate(Type pType)
			{
				int num = inputTypes.FindIndex((Type inType) => inType != null && pType.IsAssignableFrom(inType));
				if (num >= 0)
				{
					return inputs[num];
				}
				return AccessTools.GetDefaultValue(pType);
			}).ToArray<object>();
		}

		// Token: 0x0600029E RID: 670 RVA: 0x0000DCFB File Offset: 0x0000BEFB
		public static AccessTools.FieldRef<T, F> FieldRefAccess<T, F>(string fieldName)
		{
			return AccessTools.FieldRefAccess<T, F>(typeof(T).GetField(fieldName, BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.NonPublic));
		}

		// Token: 0x0600029F RID: 671 RVA: 0x0000DD14 File Offset: 0x0000BF14
		public static ref F FieldRefAccess<T, F>(T instance, string fieldName)
		{
			return AccessTools.FieldRefAccess<T, F>(fieldName)(instance);
		}

		// Token: 0x060002A0 RID: 672 RVA: 0x0000DD24 File Offset: 0x0000BF24
		public static AccessTools.FieldRef<T, F> FieldRefAccess<T, F>(FieldInfo fieldInfo)
		{
			if (fieldInfo == null)
			{
				throw new ArgumentNullException("fieldInfo");
			}
			if (!typeof(F).IsAssignableFrom(fieldInfo.FieldType))
			{
				throw new ArgumentException("FieldInfo type does not match FieldRefAccess return type.");
			}
			if (typeof(T) != typeof(object) && (fieldInfo.DeclaringType == null || !fieldInfo.DeclaringType.IsAssignableFrom(typeof(T))))
			{
				throw new MissingFieldException(typeof(T).Name, fieldInfo.Name);
			}
			DynamicMethodDefinition dynamicMethodDefinition = new DynamicMethodDefinition("__refget_" + typeof(T).Name + "_fi_" + fieldInfo.Name, typeof(F).MakeByRefType(), new Type[] { typeof(T) });
			ILGenerator ilgenerator = dynamicMethodDefinition.GetILGenerator();
			ilgenerator.Emit(OpCodes.Ldarg_0);
			ilgenerator.Emit(OpCodes.Ldflda, fieldInfo);
			ilgenerator.Emit(OpCodes.Ret);
			return (AccessTools.FieldRef<T, F>)dynamicMethodDefinition.Generate().CreateDelegate(typeof(AccessTools.FieldRef<T, F>));
		}

		// Token: 0x060002A1 RID: 673 RVA: 0x0000DE4D File Offset: 0x0000C04D
		public static ref F StaticFieldRefAccess<T, F>(string fieldName)
		{
			return AccessTools.StaticFieldRefAccess<F>(typeof(T).GetField(fieldName, BindingFlags.DeclaredOnly | BindingFlags.Static | BindingFlags.NonPublic))();
		}

		// Token: 0x060002A2 RID: 674 RVA: 0x0000DE6C File Offset: 0x0000C06C
		public static AccessTools.FieldRef<F> StaticFieldRefAccess<F>(FieldInfo fieldInfo)
		{
			if (fieldInfo == null)
			{
				throw new ArgumentNullException("fieldInfo");
			}
			Type declaringType = fieldInfo.DeclaringType;
			DynamicMethodDefinition dynamicMethodDefinition = new DynamicMethodDefinition("__refget_" + (((declaringType != null) ? declaringType.Name : null) ?? "null") + "_static_fi_" + fieldInfo.Name, typeof(F).MakeByRefType(), new Type[0]);
			ILGenerator ilgenerator = dynamicMethodDefinition.GetILGenerator();
			ilgenerator.Emit(OpCodes.Ldsflda, fieldInfo);
			ilgenerator.Emit(OpCodes.Ret);
			return (AccessTools.FieldRef<F>)dynamicMethodDefinition.Generate().CreateDelegate(typeof(AccessTools.FieldRef<F>));
		}

		// Token: 0x060002A3 RID: 675 RVA: 0x0000DF10 File Offset: 0x0000C110
		public static MethodBase GetOutsideCaller()
		{
			StackFrame[] frames = new StackTrace(true).GetFrames();
			for (int i = 0; i < frames.Length; i++)
			{
				MethodBase method = frames[i].GetMethod();
				Type declaringType = method.DeclaringType;
				if (((declaringType != null) ? declaringType.Namespace : null) != typeof(Harmony).Namespace)
				{
					return method;
				}
			}
			throw new Exception("Unexpected end of stack trace");
		}

		// Token: 0x060002A4 RID: 676 RVA: 0x0000DF74 File Offset: 0x0000C174
		public static void RethrowException(Exception exception)
		{
			ExceptionDispatchInfo.Capture(exception).Throw();
			throw exception;
		}

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x060002A5 RID: 677 RVA: 0x0000DF82 File Offset: 0x0000C182
		public static bool IsMonoRuntime { get; } = Type.GetType("Mono.Runtime") != null;

		// Token: 0x060002A6 RID: 678 RVA: 0x0000DF8C File Offset: 0x0000C18C
		public static void ThrowMissingMemberException(Type type, params string[] names)
		{
			string text = string.Join(",", AccessTools.GetFieldNames(type).ToArray());
			string text2 = string.Join(",", AccessTools.GetPropertyNames(type).ToArray());
			throw new MissingMemberException(string.Concat(new string[]
			{
				string.Join(",", names),
				"; available fields: ",
				text,
				"; available properties: ",
				text2
			}));
		}

		// Token: 0x060002A7 RID: 679 RVA: 0x0000DFFC File Offset: 0x0000C1FC
		public static object GetDefaultValue(Type type)
		{
			if (type == null)
			{
				if (Harmony.DEBUG)
				{
					FileLog.Log("AccessTools.GetDefaultValue: type is null");
				}
				return null;
			}
			if (type == typeof(void))
			{
				return null;
			}
			if (type.IsValueType)
			{
				return Activator.CreateInstance(type);
			}
			return null;
		}

		// Token: 0x060002A8 RID: 680 RVA: 0x0000E049 File Offset: 0x0000C249
		public static object CreateInstance(Type type)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			if (type.GetConstructor(BindingFlags.Instance | BindingFlags.Public, null, CallingConventions.Any, new Type[0], null) != null)
			{
				return Activator.CreateInstance(type);
			}
			return FormatterServices.GetUninitializedObject(type);
		}

		// Token: 0x060002A9 RID: 681 RVA: 0x0000E085 File Offset: 0x0000C285
		public static T MakeDeepCopy<T>(object source) where T : class
		{
			return AccessTools.MakeDeepCopy(source, typeof(T), null, "") as T;
		}

		// Token: 0x060002AA RID: 682 RVA: 0x0000E0A7 File Offset: 0x0000C2A7
		public static void MakeDeepCopy<T>(object source, out T result, Func<string, Traverse, Traverse, object> processor = null, string pathRoot = "")
		{
			result = (T)((object)AccessTools.MakeDeepCopy(source, typeof(T), processor, pathRoot));
		}

		// Token: 0x060002AB RID: 683 RVA: 0x0000E0C8 File Offset: 0x0000C2C8
		public static object MakeDeepCopy(object source, Type resultType, Func<string, Traverse, Traverse, object> processor = null, string pathRoot = "")
		{
			if (source == null || resultType == null)
			{
				return null;
			}
			resultType = Nullable.GetUnderlyingType(resultType) ?? resultType;
			Type type = source.GetType();
			if (type.IsPrimitive)
			{
				return source;
			}
			if (type.IsEnum)
			{
				return Enum.ToObject(resultType, (int)source);
			}
			if (type.IsGenericType && resultType.IsGenericType)
			{
				MethodInfo methodInfo = AccessTools.FirstMethod(resultType, (MethodInfo m) => m.Name == "Add" && m.GetParameters().Count<ParameterInfo>() == 1);
				if (methodInfo != null)
				{
					object obj = Activator.CreateInstance(resultType);
					FastInvokeHandler handler = MethodInvoker.GetHandler(methodInfo, false);
					Type type2 = resultType.GetGenericArguments()[0];
					int num = 0;
					foreach (object obj2 in (source as IEnumerable))
					{
						string text = num++.ToString();
						string text2 = ((pathRoot.Length > 0) ? (pathRoot + "." + text) : text);
						object obj3 = AccessTools.MakeDeepCopy(obj2, type2, processor, text2);
						handler(obj, new object[] { obj3 });
					}
					return obj;
				}
			}
			if (type.IsArray && resultType.IsArray)
			{
				Type elementType = resultType.GetElementType();
				int length = ((Array)source).Length;
				object[] array = Activator.CreateInstance(resultType, new object[] { length }) as object[];
				object[] array2 = source as object[];
				for (int i = 0; i < length; i++)
				{
					string text3 = i.ToString();
					string text4 = ((pathRoot.Length > 0) ? (pathRoot + "." + text3) : text3);
					array[i] = AccessTools.MakeDeepCopy(array2[i], elementType, processor, text4);
				}
				return array;
			}
			string @namespace = type.Namespace;
			if (@namespace == "System" || (@namespace != null && @namespace.StartsWith("System.")))
			{
				return source;
			}
			object obj4 = AccessTools.CreateInstance((resultType == typeof(object)) ? type : resultType);
			Traverse.IterateFields(source, obj4, delegate(string name, Traverse src, Traverse dst)
			{
				string text5 = ((pathRoot.Length > 0) ? (pathRoot + "." + name) : name);
				object obj5 = ((processor != null) ? processor(text5, src, dst) : src.GetValue());
				dst.SetValue(AccessTools.MakeDeepCopy(obj5, dst.GetValueType(), processor, text5));
			});
			return obj4;
		}

		// Token: 0x060002AC RID: 684 RVA: 0x0000E33C File Offset: 0x0000C53C
		public static bool IsStruct(Type type)
		{
			return type.IsValueType && !AccessTools.IsValue(type) && !AccessTools.IsVoid(type);
		}

		// Token: 0x060002AD RID: 685 RVA: 0x0000E359 File Offset: 0x0000C559
		public static bool IsClass(Type type)
		{
			return !type.IsValueType;
		}

		// Token: 0x060002AE RID: 686 RVA: 0x0000E364 File Offset: 0x0000C564
		public static bool IsValue(Type type)
		{
			return type.IsPrimitive || type.IsEnum;
		}

		// Token: 0x060002AF RID: 687 RVA: 0x0000E378 File Offset: 0x0000C578
		public static bool IsInteger(Type type)
		{
			TypeCode typeCode = Type.GetTypeCode(type);
			return typeCode - TypeCode.SByte <= 7;
		}

		// Token: 0x060002B0 RID: 688 RVA: 0x0000E398 File Offset: 0x0000C598
		public static bool IsFloatingPoint(Type type)
		{
			TypeCode typeCode = Type.GetTypeCode(type);
			return typeCode - TypeCode.Single <= 2;
		}

		// Token: 0x060002B1 RID: 689 RVA: 0x0000E3B6 File Offset: 0x0000C5B6
		public static bool IsNumber(Type type)
		{
			return AccessTools.IsInteger(type) || AccessTools.IsFloatingPoint(type);
		}

		// Token: 0x060002B2 RID: 690 RVA: 0x0000E3C8 File Offset: 0x0000C5C8
		public static bool IsVoid(Type type)
		{
			return type == typeof(void);
		}

		// Token: 0x060002B3 RID: 691 RVA: 0x0000E3DA File Offset: 0x0000C5DA
		public static bool IsOfNullableType<T>(T instance)
		{
			return Nullable.GetUnderlyingType(typeof(T)) != null;
		}

		// Token: 0x060002B4 RID: 692 RVA: 0x0000E3F4 File Offset: 0x0000C5F4
		public static int CombinedHashCode(IEnumerable<object> objects)
		{
			int num = 352654597;
			int num2 = num;
			int num3 = 0;
			foreach (object obj in objects)
			{
				if (num3 % 2 == 0)
				{
					num = ((num << 5) + num + (num >> 27)) ^ obj.GetHashCode();
				}
				else
				{
					num2 = ((num2 << 5) + num2 + (num2 >> 27)) ^ obj.GetHashCode();
				}
				num3++;
			}
			return num + num2 * 1566083941;
		}

		// Token: 0x04000197 RID: 407
		public static BindingFlags all = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.GetField | BindingFlags.SetField | BindingFlags.GetProperty | BindingFlags.SetProperty;

		// Token: 0x04000198 RID: 408
		public static BindingFlags allDeclared = AccessTools.all | BindingFlags.DeclaredOnly;

		// Token: 0x02000088 RID: 136
		// (Invoke) Token: 0x060002B7 RID: 695
		public delegate ref F FieldRef<T, F>(T obj = default(T));

		// Token: 0x02000089 RID: 137
		// (Invoke) Token: 0x060002BB RID: 699
		public delegate ref F FieldRef<F>();
	}
}
