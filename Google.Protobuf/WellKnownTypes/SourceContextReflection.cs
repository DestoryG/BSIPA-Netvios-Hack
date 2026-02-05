using System;
using Google.Protobuf.Reflection;

namespace Google.Protobuf.WellKnownTypes
{
	// Token: 0x02000033 RID: 51
	public static class SourceContextReflection
	{
		// Token: 0x1700006B RID: 107
		// (get) Token: 0x060002C5 RID: 709 RVA: 0x0000CB83 File Offset: 0x0000AD83
		public static FileDescriptor Descriptor
		{
			get
			{
				return SourceContextReflection.descriptor;
			}
		}

		// Token: 0x040000B9 RID: 185
		private static FileDescriptor descriptor = FileDescriptor.FromGeneratedCode(Convert.FromBase64String(string.Concat(new string[] { "CiRnb29nbGUvcHJvdG9idWYvc291cmNlX2NvbnRleHQucHJvdG8SD2dvb2ds", "ZS5wcm90b2J1ZiIiCg1Tb3VyY2VDb250ZXh0EhEKCWZpbGVfbmFtZRgBIAEo", "CUKVAQoTY29tLmdvb2dsZS5wcm90b2J1ZkISU291cmNlQ29udGV4dFByb3Rv", "UAFaQWdvb2dsZS5nb2xhbmcub3JnL2dlbnByb3RvL3Byb3RvYnVmL3NvdXJj", "ZV9jb250ZXh0O3NvdXJjZV9jb250ZXh0ogIDR1BCqgIeR29vZ2xlLlByb3Rv", "YnVmLldlbGxLbm93blR5cGVzYgZwcm90bzM=" })), new FileDescriptor[0], new GeneratedClrTypeInfo(null, null, new GeneratedClrTypeInfo[]
		{
			new GeneratedClrTypeInfo(typeof(SourceContext), SourceContext.Parser, new string[] { "FileName" }, null, null, null, null)
		}));
	}
}
