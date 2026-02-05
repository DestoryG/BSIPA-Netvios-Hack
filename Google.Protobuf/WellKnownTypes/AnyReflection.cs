using System;
using Google.Protobuf.Reflection;

namespace Google.Protobuf.WellKnownTypes
{
	// Token: 0x02000027 RID: 39
	public static class AnyReflection
	{
		// Token: 0x1700003C RID: 60
		// (get) Token: 0x0600020B RID: 523 RVA: 0x0000A53F File Offset: 0x0000873F
		public static FileDescriptor Descriptor
		{
			get
			{
				return AnyReflection.descriptor;
			}
		}

		// Token: 0x0400006E RID: 110
		private static FileDescriptor descriptor = FileDescriptor.FromGeneratedCode(Convert.FromBase64String(string.Concat(new string[] { "Chlnb29nbGUvcHJvdG9idWYvYW55LnByb3RvEg9nb29nbGUucHJvdG9idWYi", "JgoDQW55EhAKCHR5cGVfdXJsGAEgASgJEg0KBXZhbHVlGAIgASgMQm8KE2Nv", "bS5nb29nbGUucHJvdG9idWZCCEFueVByb3RvUAFaJWdpdGh1Yi5jb20vZ29s", "YW5nL3Byb3RvYnVmL3B0eXBlcy9hbnmiAgNHUEKqAh5Hb29nbGUuUHJvdG9i", "dWYuV2VsbEtub3duVHlwZXNiBnByb3RvMw==" })), new FileDescriptor[0], new GeneratedClrTypeInfo(null, null, new GeneratedClrTypeInfo[]
		{
			new GeneratedClrTypeInfo(typeof(Any), Any.Parser, new string[] { "TypeUrl", "Value" }, null, null, null, null)
		}));
	}
}
