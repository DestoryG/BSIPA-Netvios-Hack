using System;
using Google.Protobuf.Reflection;

namespace Google.Protobuf.WellKnownTypes
{
	// Token: 0x0200002D RID: 45
	public static class DurationReflection
	{
		// Token: 0x1700005C RID: 92
		// (get) Token: 0x06000272 RID: 626 RVA: 0x0000BC99 File Offset: 0x00009E99
		public static FileDescriptor Descriptor
		{
			get
			{
				return DurationReflection.descriptor;
			}
		}

		// Token: 0x040000A1 RID: 161
		private static FileDescriptor descriptor = FileDescriptor.FromGeneratedCode(Convert.FromBase64String(string.Concat(new string[] { "Ch5nb29nbGUvcHJvdG9idWYvZHVyYXRpb24ucHJvdG8SD2dvb2dsZS5wcm90", "b2J1ZiIqCghEdXJhdGlvbhIPCgdzZWNvbmRzGAEgASgDEg0KBW5hbm9zGAIg", "ASgFQnwKE2NvbS5nb29nbGUucHJvdG9idWZCDUR1cmF0aW9uUHJvdG9QAVoq", "Z2l0aHViLmNvbS9nb2xhbmcvcHJvdG9idWYvcHR5cGVzL2R1cmF0aW9u+AEB", "ogIDR1BCqgIeR29vZ2xlLlByb3RvYnVmLldlbGxLbm93blR5cGVzYgZwcm90", "bzM=" })), new FileDescriptor[0], new GeneratedClrTypeInfo(null, null, new GeneratedClrTypeInfo[]
		{
			new GeneratedClrTypeInfo(typeof(Duration), Duration.Parser, new string[] { "Seconds", "Nanos" }, null, null, null, null)
		}));
	}
}
