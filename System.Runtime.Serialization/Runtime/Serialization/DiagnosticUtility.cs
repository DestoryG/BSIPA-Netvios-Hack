using System;
using System.Diagnostics;

namespace System.Runtime.Serialization
{
	// Token: 0x020000F5 RID: 245
	internal static class DiagnosticUtility
	{
		// Token: 0x0400059B RID: 1435
		internal static bool ShouldTraceError = true;

		// Token: 0x0400059C RID: 1436
		internal static readonly bool ShouldTraceWarning = false;

		// Token: 0x0400059D RID: 1437
		internal static readonly bool ShouldTraceInformation = false;

		// Token: 0x0400059E RID: 1438
		internal static bool ShouldTraceVerbose = true;

		// Token: 0x02000182 RID: 386
		internal static class DiagnosticTrace
		{
			// Token: 0x060014FF RID: 5375 RVA: 0x00054AD0 File Offset: 0x00052CD0
			internal static void TraceEvent(params object[] args)
			{
			}
		}

		// Token: 0x02000183 RID: 387
		internal static class ExceptionUtility
		{
			// Token: 0x06001500 RID: 5376 RVA: 0x00054AD2 File Offset: 0x00052CD2
			internal static Exception ThrowHelperError(Exception e)
			{
				return DiagnosticUtility.ExceptionUtility.ThrowHelper(e, TraceEventType.Error);
			}

			// Token: 0x06001501 RID: 5377 RVA: 0x00054ADB File Offset: 0x00052CDB
			internal static Exception ThrowHelperCallback(string msg, Exception e)
			{
				return new CallbackException(msg, e);
			}

			// Token: 0x06001502 RID: 5378 RVA: 0x00054AE4 File Offset: 0x00052CE4
			internal static Exception ThrowHelperCallback(Exception e)
			{
				return new CallbackException("Callback exception", e);
			}

			// Token: 0x06001503 RID: 5379 RVA: 0x00054AF1 File Offset: 0x00052CF1
			internal static Exception ThrowHelper(Exception e, TraceEventType type)
			{
				return e;
			}

			// Token: 0x06001504 RID: 5380 RVA: 0x00054AF4 File Offset: 0x00052CF4
			internal static Exception ThrowHelperArgument(string arg)
			{
				return new ArgumentException(arg);
			}

			// Token: 0x06001505 RID: 5381 RVA: 0x00054AFC File Offset: 0x00052CFC
			internal static Exception ThrowHelperArgument(string arg, string message)
			{
				return new ArgumentException(message, arg);
			}

			// Token: 0x06001506 RID: 5382 RVA: 0x00054B05 File Offset: 0x00052D05
			internal static Exception ThrowHelperArgumentNull(string arg)
			{
				return new ArgumentNullException(arg);
			}

			// Token: 0x06001507 RID: 5383 RVA: 0x00054B0D File Offset: 0x00052D0D
			internal static Exception ThrowHelperFatal(string msg, Exception e)
			{
				return new FatalException(msg, e);
			}
		}
	}
}
