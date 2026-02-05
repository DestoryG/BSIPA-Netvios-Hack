using System;
using System.Diagnostics;
using System.Reflection;

namespace IPA.Utilities
{
	// Token: 0x0200001C RID: 28
	internal static class ExceptionUtilities
	{
		// Token: 0x06000081 RID: 129 RVA: 0x00003764 File Offset: 0x00001964
		public static Exception SetStackTrace(this Exception target, StackTrace stack)
		{
			object getStackTraceString = ExceptionUtilities.TraceToStringMi.Invoke(stack, new object[] { Enum.GetValues(ExceptionUtilities.TraceFormatTi).GetValue(0) });
			ExceptionUtilities.StackTraceStringFi.SetValue(target, getStackTraceString);
			return target;
		}

		// Token: 0x06000082 RID: 130 RVA: 0x000037A4 File Offset: 0x000019A4
		// Note: this type is marked as 'beforefieldinit'.
		static ExceptionUtilities()
		{
			Type type = Type.GetType("System.Diagnostics.StackTrace");
			ExceptionUtilities.TraceFormatTi = ((type != null) ? type.GetNestedType("TraceFormat", BindingFlags.NonPublic) : null);
			ExceptionUtilities.TraceToStringMi = typeof(StackTrace).GetMethod("ToString", BindingFlags.Instance | BindingFlags.NonPublic, null, new Type[] { ExceptionUtilities.TraceFormatTi }, null);
		}

		// Token: 0x04000028 RID: 40
		private static readonly FieldInfo StackTraceStringFi = typeof(Exception).GetField("_stackTraceString", BindingFlags.Instance | BindingFlags.NonPublic);

		// Token: 0x04000029 RID: 41
		private static readonly Type TraceFormatTi;

		// Token: 0x0400002A RID: 42
		private static readonly MethodInfo TraceToStringMi;
	}
}
