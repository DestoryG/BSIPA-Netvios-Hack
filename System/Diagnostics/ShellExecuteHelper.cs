using System;
using System.Runtime.InteropServices;
using System.Threading;
using Microsoft.Win32;

namespace System.Diagnostics
{
	// Token: 0x020004F6 RID: 1270
	internal class ShellExecuteHelper
	{
		// Token: 0x06003024 RID: 12324 RVA: 0x000D980E File Offset: 0x000D7A0E
		public ShellExecuteHelper(Microsoft.Win32.NativeMethods.ShellExecuteInfo executeInfo)
		{
			this._executeInfo = executeInfo;
		}

		// Token: 0x06003025 RID: 12325 RVA: 0x000D9820 File Offset: 0x000D7A20
		public void ShellExecuteFunction()
		{
			if (!(this._succeeded = Microsoft.Win32.NativeMethods.ShellExecuteEx(this._executeInfo)))
			{
				this._errorCode = Marshal.GetLastWin32Error();
			}
		}

		// Token: 0x06003026 RID: 12326 RVA: 0x000D9850 File Offset: 0x000D7A50
		public bool ShellExecuteOnSTAThread()
		{
			if (Thread.CurrentThread.GetApartmentState() != ApartmentState.STA)
			{
				ThreadStart threadStart = new ThreadStart(this.ShellExecuteFunction);
				Thread thread = new Thread(threadStart);
				thread.SetApartmentState(ApartmentState.STA);
				thread.Start();
				thread.Join();
			}
			else
			{
				this.ShellExecuteFunction();
			}
			return this._succeeded;
		}

		// Token: 0x17000BC5 RID: 3013
		// (get) Token: 0x06003027 RID: 12327 RVA: 0x000D989E File Offset: 0x000D7A9E
		public int ErrorCode
		{
			get
			{
				return this._errorCode;
			}
		}

		// Token: 0x0400287B RID: 10363
		private Microsoft.Win32.NativeMethods.ShellExecuteInfo _executeInfo;

		// Token: 0x0400287C RID: 10364
		private int _errorCode;

		// Token: 0x0400287D RID: 10365
		private bool _succeeded;
	}
}
