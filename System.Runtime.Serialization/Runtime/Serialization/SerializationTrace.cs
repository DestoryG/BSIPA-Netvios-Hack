using System;
using System.Diagnostics;
using System.Security;

namespace System.Runtime.Serialization
{
	// Token: 0x020000D6 RID: 214
	internal static class SerializationTrace
	{
		// Token: 0x17000272 RID: 626
		// (get) Token: 0x06000C14 RID: 3092 RVA: 0x00034355 File Offset: 0x00032555
		internal static SourceSwitch CodeGenerationSwitch
		{
			get
			{
				return SerializationTrace.CodeGenerationTraceSource.Switch;
			}
		}

		// Token: 0x06000C15 RID: 3093 RVA: 0x00034361 File Offset: 0x00032561
		internal static void WriteInstruction(int lineNumber, string instruction)
		{
		}

		// Token: 0x06000C16 RID: 3094 RVA: 0x00034363 File Offset: 0x00032563
		internal static void TraceInstruction(string instruction)
		{
		}

		// Token: 0x17000273 RID: 627
		// (get) Token: 0x06000C17 RID: 3095 RVA: 0x00034365 File Offset: 0x00032565
		private static TraceSource CodeGenerationTraceSource
		{
			[SecuritySafeCritical]
			get
			{
				if (SerializationTrace.codeGen == null)
				{
					SerializationTrace.codeGen = new TraceSource("System.Runtime.Serialization.CodeGeneration");
				}
				return SerializationTrace.codeGen;
			}
		}

		// Token: 0x040004F3 RID: 1267
		[SecurityCritical]
		private static TraceSource codeGen;
	}
}
