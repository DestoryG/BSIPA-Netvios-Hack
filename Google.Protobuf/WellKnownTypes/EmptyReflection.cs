using System;
using Google.Protobuf.Reflection;

namespace Google.Protobuf.WellKnownTypes
{
	// Token: 0x0200002F RID: 47
	public static class EmptyReflection
	{
		// Token: 0x17000062 RID: 98
		// (get) Token: 0x06000291 RID: 657 RVA: 0x0000C2C5 File Offset: 0x0000A4C5
		public static FileDescriptor Descriptor
		{
			get
			{
				return EmptyReflection.descriptor;
			}
		}

		// Token: 0x040000AE RID: 174
		private static FileDescriptor descriptor = FileDescriptor.FromGeneratedCode(Convert.FromBase64String(string.Concat(new string[] { "Chtnb29nbGUvcHJvdG9idWYvZW1wdHkucHJvdG8SD2dvb2dsZS5wcm90b2J1", "ZiIHCgVFbXB0eUJ2ChNjb20uZ29vZ2xlLnByb3RvYnVmQgpFbXB0eVByb3Rv", "UAFaJ2dpdGh1Yi5jb20vZ29sYW5nL3Byb3RvYnVmL3B0eXBlcy9lbXB0efgB", "AaICA0dQQqoCHkdvb2dsZS5Qcm90b2J1Zi5XZWxsS25vd25UeXBlc2IGcHJv", "dG8z" })), new FileDescriptor[0], new GeneratedClrTypeInfo(null, null, new GeneratedClrTypeInfo[]
		{
			new GeneratedClrTypeInfo(typeof(Empty), Empty.Parser, null, null, null, null, null)
		}));
	}
}
