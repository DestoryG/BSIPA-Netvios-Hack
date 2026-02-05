using System;
using System.Runtime.InteropServices;

namespace System.Net
{
	// Token: 0x0200020F RID: 527
	[StructLayout(LayoutKind.Sequential)]
	internal class StreamSizes
	{
		// Token: 0x060013B9 RID: 5049 RVA: 0x000683C8 File Offset: 0x000665C8
		internal unsafe StreamSizes(byte[] memory)
		{
			fixed (byte[] array = memory)
			{
				void* ptr;
				if (memory == null || array.Length == 0)
				{
					ptr = null;
				}
				else
				{
					ptr = (void*)(&array[0]);
				}
				IntPtr intPtr = new IntPtr(ptr);
				checked
				{
					try
					{
						this.header = (int)((uint)Marshal.ReadInt32(intPtr));
						this.trailer = (int)((uint)Marshal.ReadInt32(intPtr, 4));
						this.maximumMessage = (int)((uint)Marshal.ReadInt32(intPtr, 8));
						this.buffersCount = (int)((uint)Marshal.ReadInt32(intPtr, 12));
						this.blockSize = (int)((uint)Marshal.ReadInt32(intPtr, 16));
					}
					catch (OverflowException)
					{
						throw;
					}
				}
			}
		}

		// Token: 0x04001567 RID: 5479
		public int header;

		// Token: 0x04001568 RID: 5480
		public int trailer;

		// Token: 0x04001569 RID: 5481
		public int maximumMessage;

		// Token: 0x0400156A RID: 5482
		public int buffersCount;

		// Token: 0x0400156B RID: 5483
		public int blockSize;

		// Token: 0x0400156C RID: 5484
		public static readonly int SizeOf = Marshal.SizeOf(typeof(StreamSizes));
	}
}
