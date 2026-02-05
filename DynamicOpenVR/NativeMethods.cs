using System;
using System.Runtime.InteropServices;
using System.Text;

namespace DynamicOpenVR
{
	// Token: 0x020000C8 RID: 200
	internal static class NativeMethods
	{
		// Token: 0x0600018D RID: 397
		[DllImport("kernel32.dll", SetLastError = true)]
		public static extern IntPtr LoadLibrary(string dllToLoad);

		// Token: 0x0600018E RID: 398
		[DllImport("kernel32.dll", SetLastError = true)]
		internal static extern int QueryFullProcessImageName([In] IntPtr hProcess, [In] int dwFlags, [Out] StringBuilder lpExeName, ref int lpdwSize);
	}
}
