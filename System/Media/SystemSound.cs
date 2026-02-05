using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;

namespace System.Media
{
	// Token: 0x020003A6 RID: 934
	[HostProtection(SecurityAction.LinkDemand, UI = true)]
	public class SystemSound
	{
		// Token: 0x060022DB RID: 8923 RVA: 0x000A5DDC File Offset: 0x000A3FDC
		internal SystemSound(int soundType)
		{
			this.soundType = soundType;
		}

		// Token: 0x060022DC RID: 8924 RVA: 0x000A5DEC File Offset: 0x000A3FEC
		public void Play()
		{
			IntSecurity.UnmanagedCode.Assert();
			try
			{
				SystemSound.SafeNativeMethods.MessageBeep(this.soundType);
			}
			finally
			{
				CodeAccessPermission.RevertAssert();
			}
		}

		// Token: 0x04001FAB RID: 8107
		private int soundType;

		// Token: 0x020007E6 RID: 2022
		private class SafeNativeMethods
		{
			// Token: 0x060043D5 RID: 17365 RVA: 0x0011D839 File Offset: 0x0011BA39
			private SafeNativeMethods()
			{
			}

			// Token: 0x060043D6 RID: 17366
			[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
			internal static extern bool MessageBeep(int type);
		}
	}
}
