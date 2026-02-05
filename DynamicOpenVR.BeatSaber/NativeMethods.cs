using System;
using System.Runtime.InteropServices;

namespace DynamicOpenVR.BeatSaber
{
	// Token: 0x02000006 RID: 6
	internal static class NativeMethods
	{
		// Token: 0x0600000B RID: 11
		[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		internal static extern int MessageBox(IntPtr hWnd, string text, string caption, uint type);
	}
}
