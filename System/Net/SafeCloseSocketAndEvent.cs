using System;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Security;
using System.Threading;
using Microsoft.Win32.SafeHandles;

namespace System.Net
{
	// Token: 0x02000201 RID: 513
	[SuppressUnmanagedCodeSecurity]
	internal sealed class SafeCloseSocketAndEvent : SafeCloseSocket
	{
		// Token: 0x0600134E RID: 4942 RVA: 0x00065D60 File Offset: 0x00063F60
		internal SafeCloseSocketAndEvent()
		{
		}

		// Token: 0x0600134F RID: 4943 RVA: 0x00065D68 File Offset: 0x00063F68
		protected override bool ReleaseHandle()
		{
			bool flag = base.ReleaseHandle();
			this.DeleteEvent();
			return flag;
		}

		// Token: 0x06001350 RID: 4944 RVA: 0x00065D84 File Offset: 0x00063F84
		internal static SafeCloseSocketAndEvent CreateWSASocketWithEvent(AddressFamily addressFamily, SocketType socketType, ProtocolType protocolType, bool autoReset, bool signaled)
		{
			SafeCloseSocketAndEvent safeCloseSocketAndEvent = new SafeCloseSocketAndEvent();
			SafeCloseSocket.CreateSocket(SafeCloseSocket.InnerSafeCloseSocket.CreateWSASocket(addressFamily, socketType, protocolType), safeCloseSocketAndEvent);
			if (safeCloseSocketAndEvent.IsInvalid)
			{
				throw new SocketException();
			}
			safeCloseSocketAndEvent.waitHandle = new AutoResetEvent(false);
			SafeCloseSocketAndEvent.CompleteInitialization(safeCloseSocketAndEvent);
			return safeCloseSocketAndEvent;
		}

		// Token: 0x06001351 RID: 4945 RVA: 0x00065DC8 File Offset: 0x00063FC8
		internal static void CompleteInitialization(SafeCloseSocketAndEvent socketAndEventHandle)
		{
			SafeWaitHandle safeWaitHandle = socketAndEventHandle.waitHandle.SafeWaitHandle;
			bool flag = false;
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
				safeWaitHandle.DangerousAddRef(ref flag);
			}
			catch
			{
				if (flag)
				{
					safeWaitHandle.DangerousRelease();
					socketAndEventHandle.waitHandle = null;
					flag = false;
				}
			}
			finally
			{
				if (flag)
				{
					safeWaitHandle.Dispose();
				}
			}
		}

		// Token: 0x06001352 RID: 4946 RVA: 0x00065E30 File Offset: 0x00064030
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		private void DeleteEvent()
		{
			try
			{
				if (this.waitHandle != null)
				{
					this.waitHandle.SafeWaitHandle.DangerousRelease();
				}
			}
			catch
			{
			}
		}

		// Token: 0x06001353 RID: 4947 RVA: 0x00065E6C File Offset: 0x0006406C
		internal WaitHandle GetEventHandle()
		{
			return this.waitHandle;
		}

		// Token: 0x04001551 RID: 5457
		private AutoResetEvent waitHandle;
	}
}
