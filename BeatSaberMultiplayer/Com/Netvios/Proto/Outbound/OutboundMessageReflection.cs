using System;
using Google.Protobuf.Reflection;

namespace Com.Netvios.Proto.Outbound
{
	// Token: 0x0200002D RID: 45
	public static class OutboundMessageReflection
	{
		// Token: 0x1700013D RID: 317
		// (get) Token: 0x060003C6 RID: 966 RVA: 0x00010D06 File Offset: 0x0000EF06
		public static FileDescriptor Descriptor
		{
			get
			{
				return OutboundMessageReflection.descriptor;
			}
		}

		// Token: 0x040001F8 RID: 504
		private static FileDescriptor descriptor = FileDescriptor.FromGeneratedCode(Convert.FromBase64String(string.Concat(new string[] { "ChVPdXRib3VuZE1lc3NhZ2UucHJvdG8SGmNvbS5uZXR2aW9zLnByb3RvLm91", "dGJvdW5kGg1NZXNzYWdlLnByb3RvGh5CZWF0U2FiZXJPdXRib3VuZE1lc3Nh", "Z2UucHJvdG8iqgEKBEJvZHkSKQoEZ2FtZRgBIAEoDjIbLmNvbS5uZXR2aW9z", "LnByb3RvLkdhbWVUeXBlEgwKBGNvZGUYAiABKAUSDwoHbWVzc2FnZRgDIAEo", "CRIMCgR0b29rGAQgASgFEkIKDWJlYXRTYWJlckJvZHkYBSABKAsyKS5jb20u", "bmV0dmlvcy5wcm90by5vdXRib3VuZC5CZWF0U2FiZXJCb2R5SABCBgoEZGF0", "YUIqChljb20ubmV0dmlvcy50Y3AucHJvdG8ub3V0Qg1PdXRib3VuZFByb3Rv", "YgZwcm90bzM=" })), new FileDescriptor[]
		{
			MessageReflection.Descriptor,
			BeatSaberOutboundMessageReflection.Descriptor
		}, new GeneratedClrTypeInfo(null, null, new GeneratedClrTypeInfo[]
		{
			new GeneratedClrTypeInfo(typeof(Body), Body.Parser, new string[] { "Game", "Code", "Message", "Took", "BeatSaberBody" }, new string[] { "Data" }, null, null, null)
		}));
	}
}
