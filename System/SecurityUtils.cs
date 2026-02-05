using System;
using System.Reflection;
using System.Security;
using System.Security.Permissions;

namespace System
{
	// Token: 0x0200005C RID: 92
	internal static class SecurityUtils
	{
		// Token: 0x17000083 RID: 131
		// (get) Token: 0x06000410 RID: 1040 RVA: 0x0001D654 File Offset: 0x0001B854
		private static ReflectionPermission MemberAccessPermission
		{
			get
			{
				if (SecurityUtils.memberAccessPermission == null)
				{
					SecurityUtils.memberAccessPermission = new ReflectionPermission(ReflectionPermissionFlag.MemberAccess);
				}
				return SecurityUtils.memberAccessPermission;
			}
		}

		// Token: 0x17000084 RID: 132
		// (get) Token: 0x06000411 RID: 1041 RVA: 0x0001D673 File Offset: 0x0001B873
		private static ReflectionPermission RestrictedMemberAccessPermission
		{
			get
			{
				if (SecurityUtils.restrictedMemberAccessPermission == null)
				{
					SecurityUtils.restrictedMemberAccessPermission = new ReflectionPermission(ReflectionPermissionFlag.RestrictedMemberAccess);
				}
				return SecurityUtils.restrictedMemberAccessPermission;
			}
		}

		// Token: 0x06000412 RID: 1042 RVA: 0x0001D694 File Offset: 0x0001B894
		private static void DemandReflectionAccess(Type type)
		{
			try
			{
				SecurityUtils.MemberAccessPermission.Demand();
			}
			catch (SecurityException)
			{
				SecurityUtils.DemandGrantSet(type.Assembly);
			}
		}

		// Token: 0x06000413 RID: 1043 RVA: 0x0001D6CC File Offset: 0x0001B8CC
		[SecuritySafeCritical]
		private static void DemandGrantSet(Assembly assembly)
		{
			PermissionSet permissionSet = assembly.PermissionSet;
			permissionSet.AddPermission(SecurityUtils.RestrictedMemberAccessPermission);
			permissionSet.Demand();
		}

		// Token: 0x06000414 RID: 1044 RVA: 0x0001D6F4 File Offset: 0x0001B8F4
		private static bool HasReflectionPermission(Type type)
		{
			try
			{
				SecurityUtils.DemandReflectionAccess(type);
				return true;
			}
			catch (SecurityException)
			{
			}
			return false;
		}

		// Token: 0x06000415 RID: 1045 RVA: 0x0001D724 File Offset: 0x0001B924
		internal static object SecureCreateInstance(Type type)
		{
			return SecurityUtils.SecureCreateInstance(type, null, false);
		}

		// Token: 0x06000416 RID: 1046 RVA: 0x0001D730 File Offset: 0x0001B930
		internal static object SecureCreateInstance(Type type, object[] args, bool allowNonPublic)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.CreateInstance;
			if (!type.IsVisible)
			{
				SecurityUtils.DemandReflectionAccess(type);
			}
			else if (allowNonPublic && !SecurityUtils.HasReflectionPermission(type))
			{
				allowNonPublic = false;
			}
			if (allowNonPublic)
			{
				bindingFlags |= BindingFlags.NonPublic;
			}
			return Activator.CreateInstance(type, bindingFlags, null, args, null);
		}

		// Token: 0x06000417 RID: 1047 RVA: 0x0001D787 File Offset: 0x0001B987
		internal static object SecureCreateInstance(Type type, object[] args)
		{
			return SecurityUtils.SecureCreateInstance(type, args, false);
		}

		// Token: 0x06000418 RID: 1048 RVA: 0x0001D791 File Offset: 0x0001B991
		internal static object SecureConstructorInvoke(Type type, Type[] argTypes, object[] args, bool allowNonPublic)
		{
			return SecurityUtils.SecureConstructorInvoke(type, argTypes, args, allowNonPublic, BindingFlags.Default);
		}

		// Token: 0x06000419 RID: 1049 RVA: 0x0001D7A0 File Offset: 0x0001B9A0
		internal static object SecureConstructorInvoke(Type type, Type[] argTypes, object[] args, bool allowNonPublic, BindingFlags extraFlags)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			if (!type.IsVisible)
			{
				SecurityUtils.DemandReflectionAccess(type);
			}
			else if (allowNonPublic && !SecurityUtils.HasReflectionPermission(type))
			{
				allowNonPublic = false;
			}
			BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | extraFlags;
			if (!allowNonPublic)
			{
				bindingFlags &= ~BindingFlags.NonPublic;
			}
			ConstructorInfo constructor = type.GetConstructor(bindingFlags, null, argTypes, null);
			if (constructor != null)
			{
				return constructor.Invoke(args);
			}
			return null;
		}

		// Token: 0x0600041A RID: 1050 RVA: 0x0001D80C File Offset: 0x0001BA0C
		private static bool GenericArgumentsAreVisible(MethodInfo method)
		{
			if (method.IsGenericMethod)
			{
				Type[] genericArguments = method.GetGenericArguments();
				foreach (Type type in genericArguments)
				{
					if (!type.IsVisible)
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x0600041B RID: 1051 RVA: 0x0001D848 File Offset: 0x0001BA48
		internal static object FieldInfoGetValue(FieldInfo field, object target)
		{
			Type declaringType = field.DeclaringType;
			if (declaringType == null)
			{
				if (!field.IsPublic)
				{
					SecurityUtils.DemandGrantSet(field.Module.Assembly);
				}
			}
			else if (!(declaringType != null) || !declaringType.IsVisible || !field.IsPublic)
			{
				SecurityUtils.DemandReflectionAccess(declaringType);
			}
			return field.GetValue(target);
		}

		// Token: 0x0600041C RID: 1052 RVA: 0x0001D8A8 File Offset: 0x0001BAA8
		internal static object MethodInfoInvoke(MethodInfo method, object target, object[] args)
		{
			Type declaringType = method.DeclaringType;
			if (declaringType == null)
			{
				if (!method.IsPublic || !SecurityUtils.GenericArgumentsAreVisible(method))
				{
					SecurityUtils.DemandGrantSet(method.Module.Assembly);
				}
			}
			else if (!declaringType.IsVisible || !method.IsPublic || !SecurityUtils.GenericArgumentsAreVisible(method))
			{
				SecurityUtils.DemandReflectionAccess(declaringType);
			}
			return method.Invoke(target, args);
		}

		// Token: 0x0600041D RID: 1053 RVA: 0x0001D910 File Offset: 0x0001BB10
		internal static object ConstructorInfoInvoke(ConstructorInfo ctor, object[] args)
		{
			Type declaringType = ctor.DeclaringType;
			if (declaringType != null && (!declaringType.IsVisible || !ctor.IsPublic))
			{
				SecurityUtils.DemandReflectionAccess(declaringType);
			}
			return ctor.Invoke(args);
		}

		// Token: 0x0600041E RID: 1054 RVA: 0x0001D94A File Offset: 0x0001BB4A
		internal static object ArrayCreateInstance(Type type, int length)
		{
			if (!type.IsVisible)
			{
				SecurityUtils.DemandReflectionAccess(type);
			}
			return Array.CreateInstance(type, length);
		}

		// Token: 0x040004F0 RID: 1264
		private static volatile ReflectionPermission memberAccessPermission;

		// Token: 0x040004F1 RID: 1265
		private static volatile ReflectionPermission restrictedMemberAccessPermission;
	}
}
