using System;
using System.Reflection;

namespace Google.Protobuf.Compatibility
{
	// Token: 0x02000082 RID: 130
	internal static class PropertyInfoExtensions
	{
		// Token: 0x0600083E RID: 2110 RVA: 0x0001D37C File Offset: 0x0001B57C
		internal static MethodInfo GetGetMethod(this PropertyInfo target)
		{
			MethodInfo getMethod = target.GetMethod;
			if (!(getMethod != null) || !getMethod.IsPublic)
			{
				return null;
			}
			return getMethod;
		}

		// Token: 0x0600083F RID: 2111 RVA: 0x0001D3A4 File Offset: 0x0001B5A4
		internal static MethodInfo GetSetMethod(this PropertyInfo target)
		{
			MethodInfo setMethod = target.SetMethod;
			if (!(setMethod != null) || !setMethod.IsPublic)
			{
				return null;
			}
			return setMethod;
		}
	}
}
