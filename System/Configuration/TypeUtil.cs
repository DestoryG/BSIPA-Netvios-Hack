using System;
using System.Security.Permissions;

namespace System.Configuration
{
	// Token: 0x020000B4 RID: 180
	internal static class TypeUtil
	{
		// Token: 0x06000617 RID: 1559 RVA: 0x00023DFC File Offset: 0x00021FFC
		[ReflectionPermission(SecurityAction.Assert, Flags = ReflectionPermissionFlag.MemberAccess)]
		internal static object CreateInstanceWithReflectionPermission(string typeString)
		{
			Type type = Type.GetType(typeString, true);
			return Activator.CreateInstance(type, true);
		}
	}
}
