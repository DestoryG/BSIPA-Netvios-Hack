using System;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace System.Security
{
	// Token: 0x02000438 RID: 1080
	[EditorBrowsable(EditorBrowsableState.Never)]
	public static class SecureStringMarshal
	{
		// Token: 0x06002871 RID: 10353 RVA: 0x000B9DA8 File Offset: 0x000B7FA8
		[SecuritySafeCritical]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public static IntPtr SecureStringToCoTaskMemAnsi(SecureString s)
		{
			return Marshal.SecureStringToCoTaskMemAnsi(s);
		}

		// Token: 0x06002872 RID: 10354 RVA: 0x000B9DB0 File Offset: 0x000B7FB0
		[SecuritySafeCritical]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public static IntPtr SecureStringToGlobalAllocAnsi(SecureString s)
		{
			return Marshal.SecureStringToGlobalAllocAnsi(s);
		}

		// Token: 0x06002873 RID: 10355 RVA: 0x000B9DB8 File Offset: 0x000B7FB8
		[SecuritySafeCritical]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public static IntPtr SecureStringToCoTaskMemUnicode(SecureString s)
		{
			return Marshal.SecureStringToCoTaskMemUnicode(s);
		}

		// Token: 0x06002874 RID: 10356 RVA: 0x000B9DC0 File Offset: 0x000B7FC0
		[SecuritySafeCritical]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public static IntPtr SecureStringToGlobalAllocUnicode(SecureString s)
		{
			return Marshal.SecureStringToGlobalAllocUnicode(s);
		}
	}
}
