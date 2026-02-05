using System;

namespace System.Net
{
	// Token: 0x02000131 RID: 305
	internal struct SecurityBufferStruct
	{
		// Token: 0x04001035 RID: 4149
		public int count;

		// Token: 0x04001036 RID: 4150
		public BufferType type;

		// Token: 0x04001037 RID: 4151
		public IntPtr token;

		// Token: 0x04001038 RID: 4152
		public static readonly int Size = sizeof(SecurityBufferStruct);
	}
}
