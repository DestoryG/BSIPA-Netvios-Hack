using System;
using System.Security;
using System.Threading;
using Microsoft.Win32;

namespace System.Diagnostics
{
	// Token: 0x02000492 RID: 1170
	internal class MessageBoxPopup
	{
		// Token: 0x17000A79 RID: 2681
		// (get) Token: 0x06002B51 RID: 11089 RVA: 0x000C4D29 File Offset: 0x000C2F29
		// (set) Token: 0x06002B52 RID: 11090 RVA: 0x000C4D31 File Offset: 0x000C2F31
		public int ReturnValue { get; set; }

		// Token: 0x06002B53 RID: 11091 RVA: 0x000C4D3A File Offset: 0x000C2F3A
		[SecurityCritical]
		public MessageBoxPopup(string body, string title, int flags)
		{
			this.m_Event = new AutoResetEvent(false);
			this.m_Body = body;
			this.m_Title = title;
			this.m_Flags = flags;
		}

		// Token: 0x06002B54 RID: 11092 RVA: 0x000C4D64 File Offset: 0x000C2F64
		public int ShowMessageBox()
		{
			Thread thread = new Thread(new ThreadStart(this.DoPopup));
			thread.Start();
			this.m_Event.WaitOne();
			return this.ReturnValue;
		}

		// Token: 0x06002B55 RID: 11093 RVA: 0x000C4D9B File Offset: 0x000C2F9B
		[SecuritySafeCritical]
		public void DoPopup()
		{
			this.ReturnValue = SafeNativeMethods.MessageBox(IntPtr.Zero, this.m_Body, this.m_Title, this.m_Flags);
			this.m_Event.Set();
		}

		// Token: 0x04002677 RID: 9847
		private AutoResetEvent m_Event;

		// Token: 0x04002678 RID: 9848
		private string m_Body;

		// Token: 0x04002679 RID: 9849
		private string m_Title;

		// Token: 0x0400267A RID: 9850
		private int m_Flags;
	}
}
