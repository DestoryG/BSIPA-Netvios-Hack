using System;
using System.Security.Permissions;

namespace System.Media
{
	// Token: 0x020003A5 RID: 933
	[HostProtection(SecurityAction.LinkDemand, UI = true)]
	public sealed class SystemSounds
	{
		// Token: 0x060022D5 RID: 8917 RVA: 0x000A5D35 File Offset: 0x000A3F35
		private SystemSounds()
		{
		}

		// Token: 0x170008D1 RID: 2257
		// (get) Token: 0x060022D6 RID: 8918 RVA: 0x000A5D3D File Offset: 0x000A3F3D
		public static SystemSound Asterisk
		{
			get
			{
				if (SystemSounds.asterisk == null)
				{
					SystemSounds.asterisk = new SystemSound(64);
				}
				return SystemSounds.asterisk;
			}
		}

		// Token: 0x170008D2 RID: 2258
		// (get) Token: 0x060022D7 RID: 8919 RVA: 0x000A5D5D File Offset: 0x000A3F5D
		public static SystemSound Beep
		{
			get
			{
				if (SystemSounds.beep == null)
				{
					SystemSounds.beep = new SystemSound(0);
				}
				return SystemSounds.beep;
			}
		}

		// Token: 0x170008D3 RID: 2259
		// (get) Token: 0x060022D8 RID: 8920 RVA: 0x000A5D7C File Offset: 0x000A3F7C
		public static SystemSound Exclamation
		{
			get
			{
				if (SystemSounds.exclamation == null)
				{
					SystemSounds.exclamation = new SystemSound(48);
				}
				return SystemSounds.exclamation;
			}
		}

		// Token: 0x170008D4 RID: 2260
		// (get) Token: 0x060022D9 RID: 8921 RVA: 0x000A5D9C File Offset: 0x000A3F9C
		public static SystemSound Hand
		{
			get
			{
				if (SystemSounds.hand == null)
				{
					SystemSounds.hand = new SystemSound(16);
				}
				return SystemSounds.hand;
			}
		}

		// Token: 0x170008D5 RID: 2261
		// (get) Token: 0x060022DA RID: 8922 RVA: 0x000A5DBC File Offset: 0x000A3FBC
		public static SystemSound Question
		{
			get
			{
				if (SystemSounds.question == null)
				{
					SystemSounds.question = new SystemSound(32);
				}
				return SystemSounds.question;
			}
		}

		// Token: 0x04001FA6 RID: 8102
		private static volatile SystemSound asterisk;

		// Token: 0x04001FA7 RID: 8103
		private static volatile SystemSound beep;

		// Token: 0x04001FA8 RID: 8104
		private static volatile SystemSound exclamation;

		// Token: 0x04001FA9 RID: 8105
		private static volatile SystemSound hand;

		// Token: 0x04001FAA RID: 8106
		private static volatile SystemSound question;

		// Token: 0x020007E5 RID: 2021
		private class NativeMethods
		{
			// Token: 0x060043D4 RID: 17364 RVA: 0x0011D831 File Offset: 0x0011BA31
			private NativeMethods()
			{
			}

			// Token: 0x040034E7 RID: 13543
			internal const int MB_ICONHAND = 16;

			// Token: 0x040034E8 RID: 13544
			internal const int MB_ICONQUESTION = 32;

			// Token: 0x040034E9 RID: 13545
			internal const int MB_ICONEXCLAMATION = 48;

			// Token: 0x040034EA RID: 13546
			internal const int MB_ICONASTERISK = 64;
		}
	}
}
