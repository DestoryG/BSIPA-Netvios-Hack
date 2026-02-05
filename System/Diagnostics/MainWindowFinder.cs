using System;
using System.Runtime.InteropServices;
using Microsoft.Win32;

namespace System.Diagnostics
{
	// Token: 0x020004F7 RID: 1271
	internal class MainWindowFinder
	{
		// Token: 0x06003028 RID: 12328 RVA: 0x000D98A8 File Offset: 0x000D7AA8
		public IntPtr FindMainWindow(int processId)
		{
			this.bestHandle = (IntPtr)0;
			this.processId = processId;
			Microsoft.Win32.NativeMethods.EnumThreadWindowsCallback enumThreadWindowsCallback = new Microsoft.Win32.NativeMethods.EnumThreadWindowsCallback(this.EnumWindowsCallback);
			Microsoft.Win32.NativeMethods.EnumWindows(enumThreadWindowsCallback, IntPtr.Zero);
			GC.KeepAlive(enumThreadWindowsCallback);
			return this.bestHandle;
		}

		// Token: 0x06003029 RID: 12329 RVA: 0x000D98ED File Offset: 0x000D7AED
		private bool IsMainWindow(IntPtr handle)
		{
			return !(Microsoft.Win32.NativeMethods.GetWindow(new HandleRef(this, handle), 4) != (IntPtr)0) && Microsoft.Win32.NativeMethods.IsWindowVisible(new HandleRef(this, handle));
		}

		// Token: 0x0600302A RID: 12330 RVA: 0x000D991C File Offset: 0x000D7B1C
		private bool EnumWindowsCallback(IntPtr handle, IntPtr extraParameter)
		{
			int num;
			Microsoft.Win32.NativeMethods.GetWindowThreadProcessId(new HandleRef(this, handle), out num);
			if (num == this.processId && this.IsMainWindow(handle))
			{
				this.bestHandle = handle;
				return false;
			}
			return true;
		}

		// Token: 0x0400287E RID: 10366
		private IntPtr bestHandle;

		// Token: 0x0400287F RID: 10367
		private int processId;
	}
}
