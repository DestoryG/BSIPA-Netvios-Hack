using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Microsoft.CSharp.RuntimeBinder
{
	// Token: 0x02000020 RID: 32
	internal static class RuntimeBinderExtensions
	{
		// Token: 0x06000118 RID: 280 RVA: 0x000063F4 File Offset: 0x000045F4
		public static bool IsNullableType(this Type type)
		{
			return type.IsConstructedGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);
		}

		// Token: 0x06000119 RID: 281 RVA: 0x00006418 File Offset: 0x00004618
		public static bool IsEquivalentTo(this MemberInfo mi1, MemberInfo mi2)
		{
			if (mi1 == null || mi2 == null)
			{
				return mi1 == null && mi2 == null;
			}
			if (mi1.Equals(mi2))
			{
				return true;
			}
			if (mi1 is MethodInfo && mi2 is MethodInfo)
			{
				MethodInfo method1 = mi1 as MethodInfo;
				MethodInfo method2 = mi2 as MethodInfo;
				if (method1.IsGenericMethod != method2.IsGenericMethod)
				{
					return false;
				}
				if (method1.IsGenericMethod)
				{
					method1 = method1.GetGenericMethodDefinition();
					method2 = method2.GetGenericMethodDefinition();
					if (method1.GetGenericArguments().Length != method2.GetGenericArguments().Length)
					{
						return false;
					}
				}
				ParameterInfo[] parameters;
				ParameterInfo[] parameters2;
				if (method1 != method2 && method1.Name == method2.Name && method1.DeclaringType.IsGenericallyEqual(method2.DeclaringType) && method1.ReturnType.IsGenericallyEquivalentTo(method2.ReturnType, method1, method2) && (parameters = method1.GetParameters()).Length == (parameters2 = method2.GetParameters()).Length)
				{
					return parameters.Zip(parameters2, (ParameterInfo pi1, ParameterInfo pi2) => pi1.IsEquivalentTo(pi2, method1, method2)).All((bool x) => x);
				}
				return false;
			}
			else if (mi1 is ConstructorInfo && mi2 is ConstructorInfo)
			{
				ConstructorInfo ctor1 = mi1 as ConstructorInfo;
				ConstructorInfo ctor2 = mi2 as ConstructorInfo;
				ParameterInfo[] parameters3;
				ParameterInfo[] parameters4;
				if (ctor1 != ctor2 && ctor1.DeclaringType.IsGenericallyEqual(ctor2.DeclaringType) && (parameters3 = ctor1.GetParameters()).Length == (parameters4 = ctor2.GetParameters()).Length)
				{
					return parameters3.Zip(parameters4, (ParameterInfo pi1, ParameterInfo pi2) => pi1.IsEquivalentTo(pi2, ctor1, ctor2)).All((bool x) => x);
				}
				return false;
			}
			else
			{
				if (mi1 is PropertyInfo && mi2 is PropertyInfo)
				{
					PropertyInfo propertyInfo = mi1 as PropertyInfo;
					PropertyInfo propertyInfo2 = mi2 as PropertyInfo;
					return propertyInfo != propertyInfo2 && propertyInfo.Name == propertyInfo2.Name && propertyInfo.DeclaringType.IsGenericallyEqual(propertyInfo2.DeclaringType) && propertyInfo.PropertyType.IsGenericallyEquivalentTo(propertyInfo2.PropertyType, propertyInfo, propertyInfo2) && propertyInfo.GetGetMethod(true).IsEquivalentTo(propertyInfo2.GetGetMethod(true)) && propertyInfo.GetSetMethod(true).IsEquivalentTo(propertyInfo2.GetSetMethod(true));
				}
				return false;
			}
		}

		// Token: 0x0600011A RID: 282 RVA: 0x0000672F File Offset: 0x0000492F
		private static bool IsEquivalentTo(this ParameterInfo pi1, ParameterInfo pi2, MethodBase method1, MethodBase method2)
		{
			if (pi1 == null || pi2 == null)
			{
				return pi1 == null && pi2 == null;
			}
			return pi1.Equals(pi2) || pi1.ParameterType.IsGenericallyEquivalentTo(pi2.ParameterType, method1, method2);
		}

		// Token: 0x0600011B RID: 283 RVA: 0x00006760 File Offset: 0x00004960
		private static bool IsGenericallyEqual(this Type t1, Type t2)
		{
			if (t1 == null || t2 == null)
			{
				return t1 == null && t2 == null;
			}
			if (t1.Equals(t2))
			{
				return true;
			}
			if (t1.IsConstructedGenericType || t2.IsConstructedGenericType)
			{
				Type type = (t1.IsConstructedGenericType ? t1.GetGenericTypeDefinition() : t1);
				Type type2 = (t2.IsConstructedGenericType ? t2.GetGenericTypeDefinition() : t2);
				return type.Equals(type2);
			}
			return false;
		}

		// Token: 0x0600011C RID: 284 RVA: 0x000067D8 File Offset: 0x000049D8
		private static bool IsGenericallyEquivalentTo(this Type t1, Type t2, MemberInfo member1, MemberInfo member2)
		{
			if (t1.Equals(t2))
			{
				return true;
			}
			if (t1.IsGenericParameter)
			{
				if (t2.IsGenericParameter)
				{
					if (t1.DeclaringMethod == null && member1.DeclaringType.Equals(t1.DeclaringType))
					{
						if (!(t2.DeclaringMethod == null) || !member2.DeclaringType.Equals(t2.DeclaringType))
						{
							return t1.IsTypeParameterEquivalentToTypeInst(t2, member2);
						}
					}
					else if (t2.DeclaringMethod == null && member2.DeclaringType.Equals(t2.DeclaringType))
					{
						return t2.IsTypeParameterEquivalentToTypeInst(t1, member1);
					}
					return false;
				}
				return t1.IsTypeParameterEquivalentToTypeInst(t2, member2);
			}
			else
			{
				if (t2.IsGenericParameter)
				{
					return t2.IsTypeParameterEquivalentToTypeInst(t1, member1);
				}
				if (t1.IsGenericType && t2.IsGenericType)
				{
					Type[] genericArguments = t1.GetGenericArguments();
					Type[] genericArguments2 = t2.GetGenericArguments();
					if (genericArguments.Length == genericArguments2.Length)
					{
						if (t1.IsGenericallyEqual(t2))
						{
							return genericArguments.Zip(genericArguments2, (Type ta1, Type ta2) => ta1.IsGenericallyEquivalentTo(ta2, member1, member2)).All((bool x) => x);
						}
						return false;
					}
				}
				if (t1.IsArray && t2.IsArray)
				{
					return t1.GetArrayRank() == t2.GetArrayRank() && t1.GetElementType().IsGenericallyEquivalentTo(t2.GetElementType(), member1, member2);
				}
				return ((t1.IsByRef && t2.IsByRef) || (t1.IsPointer && t2.IsPointer)) && t1.GetElementType().IsGenericallyEquivalentTo(t2.GetElementType(), member1, member2);
			}
		}

		// Token: 0x0600011D RID: 285 RVA: 0x000069B4 File Offset: 0x00004BB4
		private static bool IsTypeParameterEquivalentToTypeInst(this Type typeParam, Type typeInst, MemberInfo member)
		{
			if (!(typeParam.DeclaringMethod != null))
			{
				return member.DeclaringType.GetGenericArguments()[typeParam.GenericParameterPosition].Equals(typeInst);
			}
			if (!(member is MethodBase))
			{
				return false;
			}
			MethodBase methodBase = (MethodBase)member;
			int genericParameterPosition = typeParam.GenericParameterPosition;
			Type[] array = (methodBase.IsGenericMethod ? methodBase.GetGenericArguments() : null);
			return array != null && array.Length > genericParameterPosition && array[genericParameterPosition].Equals(typeInst);
		}

		// Token: 0x0600011E RID: 286 RVA: 0x00006A26 File Offset: 0x00004C26
		public static bool HasSameMetadataDefinitionAs(this MemberInfo mi1, MemberInfo mi2)
		{
			return mi1.Module.Equals(mi2.Module) && RuntimeBinderExtensions.s_MemberEquivalence(mi1, mi2);
		}

		// Token: 0x0600011F RID: 287 RVA: 0x00006A4C File Offset: 0x00004C4C
		public static string GetIndexerName(this Type type)
		{
			string text = RuntimeBinderExtensions.GetTypeIndexerName(type);
			if (text == null && type.IsInterface)
			{
				Type[] interfaces = type.GetInterfaces();
				for (int i = 0; i < interfaces.Length; i++)
				{
					text = RuntimeBinderExtensions.GetTypeIndexerName(interfaces[i]);
					if (text != null)
					{
						break;
					}
				}
			}
			return text;
		}

		// Token: 0x06000120 RID: 288 RVA: 0x00006A90 File Offset: 0x00004C90
		private static string GetTypeIndexerName(Type type)
		{
			RuntimeBinderExtensions.<>c__DisplayClass9_0 CS$<>8__locals1 = new RuntimeBinderExtensions.<>c__DisplayClass9_0();
			RuntimeBinderExtensions.<>c__DisplayClass9_0 CS$<>8__locals2 = CS$<>8__locals1;
			DefaultMemberAttribute customAttribute = type.GetCustomAttribute<DefaultMemberAttribute>();
			CS$<>8__locals2.name = ((customAttribute != null) ? customAttribute.MemberName : null);
			if (CS$<>8__locals1.name != null && type.GetProperties(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic).Any((PropertyInfo p) => p.Name == CS$<>8__locals1.name && p.GetIndexParameters().Length != 0))
			{
				return CS$<>8__locals1.name;
			}
			return null;
		}

		// Token: 0x040000EC RID: 236
		private static Func<MemberInfo, MemberInfo, bool> s_MemberEquivalence = delegate(MemberInfo m1, MemberInfo m2)
		{
			try
			{
				Type typeFromHandle = typeof(MemberInfo);
				MethodInfo method = typeFromHandle.GetMethod("HasSameMetadataDefinitionAs", BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.ExactBinding, null, new Type[] { typeof(MemberInfo) }, null);
				if (method != null)
				{
					Func<MemberInfo, MemberInfo, bool> func = (Func<MemberInfo, MemberInfo, bool>)method.CreateDelegate(typeof(Func<MemberInfo, MemberInfo, bool>));
					try
					{
						bool flag = func(m1, m2);
						RuntimeBinderExtensions.s_MemberEquivalence = func;
						return flag;
					}
					catch
					{
					}
				}
				PropertyInfo property = typeFromHandle.GetProperty("MetadataToken", typeof(int), Array.Empty<Type>());
				if (property != null && property.CanRead)
				{
					ParameterExpression parameterExpression = Expression.Parameter(typeFromHandle);
					ParameterExpression parameterExpression2 = Expression.Parameter(typeFromHandle);
					Func<MemberInfo, MemberInfo, bool> func2 = Expression.Lambda<Func<MemberInfo, MemberInfo, bool>>(Expression.Equal(Expression.Property(parameterExpression, property), Expression.Property(parameterExpression2, property)), new ParameterExpression[] { parameterExpression, parameterExpression2 }).Compile();
					bool flag2 = func2(m1, m2);
					RuntimeBinderExtensions.s_MemberEquivalence = func2;
					return flag2;
				}
			}
			catch
			{
			}
			return (RuntimeBinderExtensions.s_MemberEquivalence = (MemberInfo m1param, MemberInfo m2param) => m1param.IsEquivalentTo(m2param))(m1, m2);
		};
	}
}
