using System;
using System.Runtime.InteropServices;

namespace System.Net.Mail
{
	// Token: 0x02000260 RID: 608
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
	internal struct MetadataRecord
	{
		// Token: 0x04001786 RID: 6022
		internal uint Identifier;

		// Token: 0x04001787 RID: 6023
		internal uint Attributes;

		// Token: 0x04001788 RID: 6024
		internal uint UserType;

		// Token: 0x04001789 RID: 6025
		internal uint DataType;

		// Token: 0x0400178A RID: 6026
		internal uint DataLen;

		// Token: 0x0400178B RID: 6027
		internal IntPtr DataBuf;

		// Token: 0x0400178C RID: 6028
		internal uint DataTag;
	}
}
