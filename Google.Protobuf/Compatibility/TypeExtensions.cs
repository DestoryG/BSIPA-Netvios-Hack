using System;
using System.Reflection;

namespace Google.Protobuf.Compatibility
{
	// Token: 0x02000083 RID: 131
	internal static class TypeExtensions
	{
		// Token: 0x06000840 RID: 2112 RVA: 0x0001D3CC File Offset: 0x0001B5CC
		internal static bool IsAssignableFrom(this Type target, Type c)
		{
			return target.GetTypeInfo().IsAssignableFrom(c.GetTypeInfo());
		}

		// Token: 0x06000841 RID: 2113 RVA: 0x0001D3E0 File Offset: 0x0001B5E0
		internal static PropertyInfo GetProperty(this Type target, string name)
		{
			while (target != null)
			{
				TypeInfo typeInfo = target.GetTypeInfo();
				PropertyInfo declaredProperty = typeInfo.GetDeclaredProperty(name);
				if (declaredProperty != null && ((declaredProperty.CanRead && declaredProperty.GetMethod.IsPublic) || (declaredProperty.CanWrite && declaredProperty.SetMethod.IsPublic)))
				{
					return declaredProperty;
				}
				target = typeInfo.BaseType;
			}
			return null;
		}

		// Token: 0x06000842 RID: 2114 RVA: 0x0001D448 File Offset: 0x0001B648
		internal static MethodInfo GetMethod(this Type target, string name)
		{
			while (target != null)
			{
				TypeInfo typeInfo = target.GetTypeInfo();
				MethodInfo declaredMethod = typeInfo.GetDeclaredMethod(name);
				if (declaredMethod != null && declaredMethod.IsPublic)
				{
					return declaredMethod;
				}
				target = typeInfo.BaseType;
			}
			return null;
		}
	}
}
