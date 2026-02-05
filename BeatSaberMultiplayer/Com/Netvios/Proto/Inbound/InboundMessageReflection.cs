using System;
using Google.Protobuf.Reflection;

namespace Com.Netvios.Proto.Inbound
{
	// Token: 0x0200004A RID: 74
	public static class InboundMessageReflection
	{
		// Token: 0x170001F9 RID: 505
		// (get) Token: 0x06000630 RID: 1584 RVA: 0x0001920A File Offset: 0x0001740A
		public static FileDescriptor Descriptor
		{
			get
			{
				return InboundMessageReflection.descriptor;
			}
		}

		// Token: 0x040002EF RID: 751
		private static FileDescriptor descriptor = FileDescriptor.FromGeneratedCode(Convert.FromBase64String(string.Concat(new string[] { "ChRJbmJvdW5kTWVzc2FnZS5wcm90bxIZY29tLm5ldHZpb3MucHJvdG8uaW5i", "b3VuZBoNTWVzc2FnZS5wcm90bxodQmVhdFNhYmVySW5ib3VuZE1lc3NhZ2Uu", "cHJvdG8ifAoEQm9keRIpCgRnYW1lGAEgASgOMhsuY29tLm5ldHZpb3MucHJv", "dG8uR2FtZVR5cGUSQQoNYmVhdFNhYmVyQm9keRgFIAEoCzIoLmNvbS5uZXR2", "aW9zLnByb3RvLmluYm91bmQuQmVhdFNhYmVyQm9keUgAQgYKBGRhdGFCKAoY", "Y29tLm5ldHZpb3MudGNwLnByb3RvLmluQgxJbmJvdW5kUHJvdG9iBnByb3Rv", "Mw==" })), new FileDescriptor[]
		{
			MessageReflection.Descriptor,
			BeatSaberInboundMessageReflection.Descriptor
		}, new GeneratedClrTypeInfo(null, null, new GeneratedClrTypeInfo[]
		{
			new GeneratedClrTypeInfo(typeof(Body), Body.Parser, new string[] { "Game", "BeatSaberBody" }, new string[] { "Data" }, null, null, null)
		}));
	}
}
