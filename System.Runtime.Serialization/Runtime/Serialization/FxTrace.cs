using System;
using System.Runtime.Diagnostics;

namespace System.Runtime.Serialization
{
	// Token: 0x020000F6 RID: 246
	internal static class FxTrace
	{
		// Token: 0x17000328 RID: 808
		// (get) Token: 0x06000F35 RID: 3893 RVA: 0x0003E1FB File Offset: 0x0003C3FB
		public static EtwDiagnosticTrace Trace
		{
			get
			{
				return Fx.Trace;
			}
		}

		// Token: 0x17000329 RID: 809
		// (get) Token: 0x06000F36 RID: 3894 RVA: 0x0003E202 File Offset: 0x0003C402
		public static ExceptionTrace Exception
		{
			get
			{
				return new ExceptionTrace("System.Runtime.Serialization", FxTrace.Trace);
			}
		}

		// Token: 0x06000F37 RID: 3895 RVA: 0x0003E213 File Offset: 0x0003C413
		public static bool IsEventEnabled(int index)
		{
			return false;
		}

		// Token: 0x06000F38 RID: 3896 RVA: 0x0003E216 File Offset: 0x0003C416
		public static void UpdateEventDefinitions(EventDescriptor[] ed, ushort[] events)
		{
		}

		// Token: 0x0400059F RID: 1439
		public static bool ShouldTraceError = true;

		// Token: 0x040005A0 RID: 1440
		public static bool ShouldTraceVerbose = true;
	}
}
