using System;
using System.Reflection;

namespace IPA.Utilities
{
	// Token: 0x02000013 RID: 19
	internal class AccessorDelegateInfo<TDelegate> where TDelegate : Delegate
	{
		// Token: 0x04000015 RID: 21
		public static readonly Type Type = typeof(TDelegate);

		// Token: 0x04000016 RID: 22
		public static readonly MethodInfo Invoke = AccessorDelegateInfo<TDelegate>.Type.GetMethod("Invoke");

		// Token: 0x04000017 RID: 23
		public static readonly ParameterInfo[] Parameters = AccessorDelegateInfo<TDelegate>.Invoke.GetParameters();
	}
}
