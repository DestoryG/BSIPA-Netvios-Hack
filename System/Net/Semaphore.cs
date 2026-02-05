using System;
using System.Threading;

namespace System.Net
{
	// Token: 0x0200020A RID: 522
	internal sealed class Semaphore : WaitHandle
	{
		// Token: 0x0600137F RID: 4991 RVA: 0x00066788 File Offset: 0x00064988
		internal Semaphore(int initialCount, int maxCount)
		{
			lock (this)
			{
				this.Handle = UnsafeNclNativeMethods.CreateSemaphore(IntPtr.Zero, initialCount, maxCount, IntPtr.Zero);
			}
		}

		// Token: 0x06001380 RID: 4992 RVA: 0x000667DC File Offset: 0x000649DC
		internal bool ReleaseSemaphore()
		{
			return UnsafeNclNativeMethods.ReleaseSemaphore(this.Handle, 1, IntPtr.Zero);
		}
	}
}
