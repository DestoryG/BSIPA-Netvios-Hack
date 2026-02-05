using System;
using Google.Protobuf.Reflection;

namespace Google.Protobuf.WellKnownTypes
{
	// Token: 0x0200003B RID: 59
	public static class TimestampReflection
	{
		// Token: 0x17000083 RID: 131
		// (get) Token: 0x06000320 RID: 800 RVA: 0x0000DB50 File Offset: 0x0000BD50
		public static FileDescriptor Descriptor
		{
			get
			{
				return TimestampReflection.descriptor;
			}
		}

		// Token: 0x040000D5 RID: 213
		private static FileDescriptor descriptor = FileDescriptor.FromGeneratedCode(Convert.FromBase64String(string.Concat(new string[] { "Ch9nb29nbGUvcHJvdG9idWYvdGltZXN0YW1wLnByb3RvEg9nb29nbGUucHJv", "dG9idWYiKwoJVGltZXN0YW1wEg8KB3NlY29uZHMYASABKAMSDQoFbmFub3MY", "AiABKAVCfgoTY29tLmdvb2dsZS5wcm90b2J1ZkIOVGltZXN0YW1wUHJvdG9Q", "AVorZ2l0aHViLmNvbS9nb2xhbmcvcHJvdG9idWYvcHR5cGVzL3RpbWVzdGFt", "cPgBAaICA0dQQqoCHkdvb2dsZS5Qcm90b2J1Zi5XZWxsS25vd25UeXBlc2IG", "cHJvdG8z" })), new FileDescriptor[0], new GeneratedClrTypeInfo(null, null, new GeneratedClrTypeInfo[]
		{
			new GeneratedClrTypeInfo(typeof(Timestamp), Timestamp.Parser, new string[] { "Seconds", "Nanos" }, null, null, null, null)
		}));
	}
}
