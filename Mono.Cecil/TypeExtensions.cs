using System;
using System.Reflection;

namespace Mono
{
	// Token: 0x02000008 RID: 8
	internal static class TypeExtensions
	{
		// Token: 0x0600000D RID: 13 RVA: 0x000021CB File Offset: 0x000003CB
		public static TypeCode GetTypeCode(this Type type)
		{
			return Type.GetTypeCode(type);
		}

		// Token: 0x0600000E RID: 14 RVA: 0x000021D3 File Offset: 0x000003D3
		public static Assembly Assembly(this Type type)
		{
			return type.Assembly;
		}

		// Token: 0x0600000F RID: 15 RVA: 0x000021DB File Offset: 0x000003DB
		public static MethodBase DeclaringMethod(this Type type)
		{
			return type.DeclaringMethod;
		}

		// Token: 0x06000010 RID: 16 RVA: 0x000021E3 File Offset: 0x000003E3
		public static Type[] GetGenericArguments(this Type type)
		{
			return type.GetGenericArguments();
		}

		// Token: 0x06000011 RID: 17 RVA: 0x000021EB File Offset: 0x000003EB
		public static bool IsGenericType(this Type type)
		{
			return type.IsGenericType;
		}

		// Token: 0x06000012 RID: 18 RVA: 0x000021F3 File Offset: 0x000003F3
		public static bool IsGenericTypeDefinition(this Type type)
		{
			return type.IsGenericTypeDefinition;
		}

		// Token: 0x06000013 RID: 19 RVA: 0x000021FB File Offset: 0x000003FB
		public static bool IsValueType(this Type type)
		{
			return type.IsValueType;
		}
	}
}
