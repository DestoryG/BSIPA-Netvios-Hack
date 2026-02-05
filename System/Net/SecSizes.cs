using System;
using System.Runtime.InteropServices;

namespace System.Net
{
	// Token: 0x02000210 RID: 528
	[StructLayout(LayoutKind.Sequential)]
	internal class SecSizes
	{
		// Token: 0x060013BB RID: 5051 RVA: 0x00068474 File Offset: 0x00066674
		internal unsafe SecSizes(byte[] memory)
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
						this.MaxToken = (int)((uint)Marshal.ReadInt32(intPtr));
						this.MaxSignature = (int)((uint)Marshal.ReadInt32(intPtr, 4));
						this.BlockSize = (int)((uint)Marshal.ReadInt32(intPtr, 8));
						this.SecurityTrailer = (int)((uint)Marshal.ReadInt32(intPtr, 12));
					}
					catch (OverflowException)
					{
						throw;
					}
				}
			}
		}

		// Token: 0x0400156D RID: 5485
		public readonly int MaxToken;

		// Token: 0x0400156E RID: 5486
		public readonly int MaxSignature;

		// Token: 0x0400156F RID: 5487
		public readonly int BlockSize;

		// Token: 0x04001570 RID: 5488
		public readonly int SecurityTrailer;

		// Token: 0x04001571 RID: 5489
		public static readonly int SizeOf = Marshal.SizeOf(typeof(SecSizes));
	}
}
