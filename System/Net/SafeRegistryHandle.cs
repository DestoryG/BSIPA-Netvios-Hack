using System;
using System.Security;
using Microsoft.Win32.SafeHandles;

namespace System.Net
{
	// Token: 0x02000206 RID: 518
	[SuppressUnmanagedCodeSecurity]
	internal sealed class SafeRegistryHandle : SafeHandleZeroOrMinusOneIsInvalid
	{
		// Token: 0x06001364 RID: 4964 RVA: 0x000660D0 File Offset: 0x000642D0
		private SafeRegistryHandle()
			: base(true)
		{
		}

		// Token: 0x06001365 RID: 4965 RVA: 0x000660D9 File Offset: 0x000642D9
		internal static uint RegOpenKeyEx(IntPtr key, string subKey, uint ulOptions, uint samDesired, out SafeRegistryHandle resultSubKey)
		{
			return UnsafeNclNativeMethods.RegistryHelper.RegOpenKeyEx(key, subKey, ulOptions, samDesired, out resultSubKey);
		}

		// Token: 0x06001366 RID: 4966 RVA: 0x000660E6 File Offset: 0x000642E6
		internal uint RegOpenKeyEx(string subKey, uint ulOptions, uint samDesired, out SafeRegistryHandle resultSubKey)
		{
			return UnsafeNclNativeMethods.RegistryHelper.RegOpenKeyEx(this, subKey, ulOptions, samDesired, out resultSubKey);
		}

		// Token: 0x06001367 RID: 4967 RVA: 0x000660F3 File Offset: 0x000642F3
		internal uint RegCloseKey()
		{
			base.Close();
			return this.resClose;
		}

		// Token: 0x06001368 RID: 4968 RVA: 0x00066104 File Offset: 0x00064304
		internal uint QueryValue(string name, out object data)
		{
			data = null;
			byte[] array = null;
			uint num = 0U;
			uint num3;
			uint num2;
			for (;;)
			{
				num2 = UnsafeNclNativeMethods.RegistryHelper.RegQueryValueEx(this, name, IntPtr.Zero, out num3, array, ref num);
				if (num2 != 234U && (array != null || num2 != 0U))
				{
					break;
				}
				array = new byte[num];
			}
			if (num2 != 0U)
			{
				return num2;
			}
			if (num3 == 3U)
			{
				if ((ulong)num != (ulong)((long)array.Length))
				{
					byte[] array2 = array;
					array = new byte[num];
					Buffer.BlockCopy(array2, 0, array, 0, (int)num);
				}
				data = array;
				return 0U;
			}
			return 50U;
		}

		// Token: 0x06001369 RID: 4969 RVA: 0x0006616E File Offset: 0x0006436E
		internal uint RegNotifyChangeKeyValue(bool watchSubTree, uint notifyFilter, SafeWaitHandle regEvent, bool async)
		{
			return UnsafeNclNativeMethods.RegistryHelper.RegNotifyChangeKeyValue(this, watchSubTree, notifyFilter, regEvent, async);
		}

		// Token: 0x0600136A RID: 4970 RVA: 0x0006617B File Offset: 0x0006437B
		internal static uint RegOpenCurrentUser(uint samDesired, out SafeRegistryHandle resultKey)
		{
			return UnsafeNclNativeMethods.RegistryHelper.RegOpenCurrentUser(samDesired, out resultKey);
		}

		// Token: 0x0600136B RID: 4971 RVA: 0x00066184 File Offset: 0x00064384
		protected override bool ReleaseHandle()
		{
			if (!this.IsInvalid)
			{
				this.resClose = UnsafeNclNativeMethods.RegistryHelper.RegCloseKey(this.handle);
			}
			base.SetHandleAsInvalid();
			return true;
		}

		// Token: 0x04001556 RID: 5462
		private uint resClose;
	}
}
