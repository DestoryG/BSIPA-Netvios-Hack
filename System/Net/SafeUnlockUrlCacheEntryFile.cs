using System;
using System.Net.Cache;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace System.Net
{
	// Token: 0x02000205 RID: 517
	internal sealed class SafeUnlockUrlCacheEntryFile : SafeHandleZeroOrMinusOneIsInvalid
	{
		// Token: 0x06001360 RID: 4960 RVA: 0x00065FEC File Offset: 0x000641EC
		private SafeUnlockUrlCacheEntryFile(string keyString)
			: base(true)
		{
			this.m_KeyString = keyString;
		}

		// Token: 0x06001361 RID: 4961 RVA: 0x00065FFC File Offset: 0x000641FC
		protected unsafe override bool ReleaseHandle()
		{
			fixed (string keyString = this.m_KeyString)
			{
				char* ptr = keyString;
				if (ptr != null)
				{
					ptr += RuntimeHelpers.OffsetToStringData / 2;
				}
				UnsafeNclNativeMethods.SafeNetHandles.UnlockUrlCacheEntryFileW(ptr, 0);
			}
			base.SetHandle(IntPtr.Zero);
			this.m_KeyString = null;
			return true;
		}

		// Token: 0x06001362 RID: 4962 RVA: 0x0006603C File Offset: 0x0006423C
		internal unsafe static _WinInetCache.Status GetAndLockFile(string key, byte* entryPtr, ref int entryBufSize, out SafeUnlockUrlCacheEntryFile handle)
		{
			if (ValidationHelper.IsBlankString(key))
			{
				throw new ArgumentNullException("key");
			}
			handle = new SafeUnlockUrlCacheEntryFile(key);
			char* ptr = key;
			if (ptr != null)
			{
				ptr += RuntimeHelpers.OffsetToStringData / 2;
			}
			return SafeUnlockUrlCacheEntryFile.MustRunGetAndLockFile(ptr, entryPtr, ref entryBufSize, handle);
		}

		// Token: 0x06001363 RID: 4963 RVA: 0x00066080 File Offset: 0x00064280
		private unsafe static _WinInetCache.Status MustRunGetAndLockFile(char* key, byte* entryPtr, ref int entryBufSize, SafeUnlockUrlCacheEntryFile handle)
		{
			_WinInetCache.Status status = _WinInetCache.Status.Success;
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
			}
			finally
			{
				if (!UnsafeNclNativeMethods.SafeNetHandles.RetrieveUrlCacheEntryFileW(key, entryPtr, ref entryBufSize, 0))
				{
					status = (_WinInetCache.Status)Marshal.GetLastWin32Error();
					handle.SetHandleAsInvalid();
				}
				else
				{
					handle.SetHandle((IntPtr)1);
				}
			}
			return status;
		}

		// Token: 0x04001555 RID: 5461
		private string m_KeyString;
	}
}
