using System;
using Google.Protobuf.Reflection;

namespace Com.Netvios.Proto
{
	// Token: 0x02000005 RID: 5
	public static class MessageReflection
	{
		// Token: 0x17000003 RID: 3
		// (get) Token: 0x0600000E RID: 14 RVA: 0x00002552 File Offset: 0x00000752
		public static FileDescriptor Descriptor
		{
			get
			{
				return MessageReflection.descriptor;
			}
		}

		// Token: 0x04000028 RID: 40
		private static FileDescriptor descriptor = FileDescriptor.FromGeneratedCode(Convert.FromBase64String("Cg1NZXNzYWdlLnByb3RvEhFjb20ubmV0dmlvcy5wcm90byplCghHYW1lVHlw" + "ZRINCglCZWF0U2FiZXIQABIJCgVDcmVlZBABEg4KCkJhdHRsZVdha2UQAhIL" + "CgdSYXdEYXRhEAMSEAoMU3ByaW5nVmVjdG9yEAQSEAoMR2xpbW1lckdyb3Zl" + "EAVCHgoVY29tLm5ldHZpb3MudGNwLnByb3RvQgVQcm90b2IGcHJvdG8z"), new FileDescriptor[0], new GeneratedClrTypeInfo(new Type[] { typeof(GameType) }, null, null));
	}
}
