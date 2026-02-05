using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace System.Net
{
	// Token: 0x02000133 RID: 307
	[StructLayout(LayoutKind.Sequential)]
	internal class SecurityBufferDescriptor
	{
		// Token: 0x06000B44 RID: 2884 RVA: 0x0003DD96 File Offset: 0x0003BF96
		public SecurityBufferDescriptor(int count)
		{
			this.Version = 0;
			this.Count = count;
			this.UnmanagedPointer = null;
		}

		// Token: 0x06000B45 RID: 2885 RVA: 0x0003DDB4 File Offset: 0x0003BFB4
		[Conditional("TRAVE")]
		internal void DebugDump()
		{
		}

		// Token: 0x0400103E RID: 4158
		public readonly int Version;

		// Token: 0x0400103F RID: 4159
		public readonly int Count;

		// Token: 0x04001040 RID: 4160
		public unsafe void* UnmanagedPointer;
	}
}
