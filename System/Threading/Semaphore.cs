using System;
using System.IO.Ports;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.AccessControl;
using System.Security.Permissions;
using Microsoft.Win32;
using Microsoft.Win32.SafeHandles;

namespace System.Threading
{
	// Token: 0x020003D4 RID: 980
	[ComVisible(false)]
	[global::__DynamicallyInvokable]
	[HostProtection(SecurityAction.LinkDemand, Synchronization = true, ExternalThreading = true)]
	public sealed class Semaphore : WaitHandle
	{
		// Token: 0x060025AC RID: 9644 RVA: 0x000AF066 File Offset: 0x000AD266
		[SecuritySafeCritical]
		[global::__DynamicallyInvokable]
		public Semaphore(int initialCount, int maximumCount)
			: this(initialCount, maximumCount, null)
		{
		}

		// Token: 0x060025AD RID: 9645 RVA: 0x000AF074 File Offset: 0x000AD274
		[global::__DynamicallyInvokable]
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public Semaphore(int initialCount, int maximumCount, string name)
		{
			if (initialCount < 0)
			{
				throw new ArgumentOutOfRangeException("initialCount", SR.GetString("ArgumentOutOfRange_NeedNonNegNumRequired"));
			}
			if (maximumCount < 1)
			{
				throw new ArgumentOutOfRangeException("maximumCount", SR.GetString("ArgumentOutOfRange_NeedPosNum"));
			}
			if (initialCount > maximumCount)
			{
				throw new ArgumentException(SR.GetString("Argument_SemaphoreInitialMaximum"));
			}
			if (name != null && 260 < name.Length)
			{
				throw new ArgumentException(SR.GetString("Argument_WaitHandleNameTooLong"));
			}
			SafeWaitHandle safeWaitHandle = SafeNativeMethods.CreateSemaphore(null, initialCount, maximumCount, name);
			if (safeWaitHandle.IsInvalid)
			{
				int lastWin32Error = Marshal.GetLastWin32Error();
				if (name != null && name.Length != 0 && 6 == lastWin32Error)
				{
					throw new WaitHandleCannotBeOpenedException(SR.GetString("WaitHandleCannotBeOpenedException_InvalidHandle", new object[] { name }));
				}
				InternalResources.WinIOError();
			}
			base.SafeWaitHandle = safeWaitHandle;
		}

		// Token: 0x060025AE RID: 9646 RVA: 0x000AF13A File Offset: 0x000AD33A
		[global::__DynamicallyInvokable]
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public Semaphore(int initialCount, int maximumCount, string name, out bool createdNew)
			: this(initialCount, maximumCount, name, out createdNew, null)
		{
		}

		// Token: 0x060025AF RID: 9647 RVA: 0x000AF148 File Offset: 0x000AD348
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public unsafe Semaphore(int initialCount, int maximumCount, string name, out bool createdNew, SemaphoreSecurity semaphoreSecurity)
		{
			if (initialCount < 0)
			{
				throw new ArgumentOutOfRangeException("initialCount", SR.GetString("ArgumentOutOfRange_NeedNonNegNumRequired"));
			}
			if (maximumCount < 1)
			{
				throw new ArgumentOutOfRangeException("maximumCount", SR.GetString("ArgumentOutOfRange_NeedNonNegNumRequired"));
			}
			if (initialCount > maximumCount)
			{
				throw new ArgumentException(SR.GetString("Argument_SemaphoreInitialMaximum"));
			}
			if (name != null && 260 < name.Length)
			{
				throw new ArgumentException(SR.GetString("Argument_WaitHandleNameTooLong"));
			}
			SafeWaitHandle safeWaitHandle;
			if (semaphoreSecurity != null)
			{
				Microsoft.Win32.NativeMethods.SECURITY_ATTRIBUTES security_ATTRIBUTES = new Microsoft.Win32.NativeMethods.SECURITY_ATTRIBUTES();
				security_ATTRIBUTES.nLength = Marshal.SizeOf(security_ATTRIBUTES);
				byte[] securityDescriptorBinaryForm = semaphoreSecurity.GetSecurityDescriptorBinaryForm();
				byte[] array;
				byte* ptr;
				if ((array = securityDescriptorBinaryForm) == null || array.Length == 0)
				{
					ptr = null;
				}
				else
				{
					ptr = &array[0];
				}
				security_ATTRIBUTES.lpSecurityDescriptor = new SafeLocalMemHandle((IntPtr)((void*)ptr), false);
				safeWaitHandle = SafeNativeMethods.CreateSemaphore(security_ATTRIBUTES, initialCount, maximumCount, name);
				array = null;
			}
			else
			{
				safeWaitHandle = SafeNativeMethods.CreateSemaphore(null, initialCount, maximumCount, name);
			}
			int lastWin32Error = Marshal.GetLastWin32Error();
			if (safeWaitHandle.IsInvalid)
			{
				if (name != null && name.Length != 0 && 6 == lastWin32Error)
				{
					throw new WaitHandleCannotBeOpenedException(SR.GetString("WaitHandleCannotBeOpenedException_InvalidHandle", new object[] { name }));
				}
				InternalResources.WinIOError();
			}
			createdNew = lastWin32Error != 183;
			base.SafeWaitHandle = safeWaitHandle;
		}

		// Token: 0x060025B0 RID: 9648 RVA: 0x000AF27B File Offset: 0x000AD47B
		private Semaphore(SafeWaitHandle handle)
		{
			base.SafeWaitHandle = handle;
		}

		// Token: 0x060025B1 RID: 9649 RVA: 0x000AF28A File Offset: 0x000AD48A
		[global::__DynamicallyInvokable]
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public static Semaphore OpenExisting(string name)
		{
			return Semaphore.OpenExisting(name, SemaphoreRights.Modify | SemaphoreRights.Synchronize);
		}

		// Token: 0x060025B2 RID: 9650 RVA: 0x000AF298 File Offset: 0x000AD498
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public static Semaphore OpenExisting(string name, SemaphoreRights rights)
		{
			Semaphore semaphore;
			switch (Semaphore.OpenExistingWorker(name, rights, out semaphore))
			{
			case Semaphore.OpenExistingResult.NameNotFound:
				throw new WaitHandleCannotBeOpenedException();
			case Semaphore.OpenExistingResult.PathNotFound:
				InternalResources.WinIOError(3, string.Empty);
				return semaphore;
			case Semaphore.OpenExistingResult.NameInvalid:
				throw new WaitHandleCannotBeOpenedException(SR.GetString("WaitHandleCannotBeOpenedException_InvalidHandle", new object[] { name }));
			default:
				return semaphore;
			}
		}

		// Token: 0x060025B3 RID: 9651 RVA: 0x000AF2F3 File Offset: 0x000AD4F3
		[global::__DynamicallyInvokable]
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public static bool TryOpenExisting(string name, out Semaphore result)
		{
			return Semaphore.OpenExistingWorker(name, SemaphoreRights.Modify | SemaphoreRights.Synchronize, out result) == Semaphore.OpenExistingResult.Success;
		}

		// Token: 0x060025B4 RID: 9652 RVA: 0x000AF304 File Offset: 0x000AD504
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public static bool TryOpenExisting(string name, SemaphoreRights rights, out Semaphore result)
		{
			return Semaphore.OpenExistingWorker(name, rights, out result) == Semaphore.OpenExistingResult.Success;
		}

		// Token: 0x060025B5 RID: 9653 RVA: 0x000AF314 File Offset: 0x000AD514
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		private static Semaphore.OpenExistingResult OpenExistingWorker(string name, SemaphoreRights rights, out Semaphore result)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			if (name.Length == 0)
			{
				throw new ArgumentException(SR.GetString("InvalidNullEmptyArgument", new object[] { "name" }), "name");
			}
			if (name != null && 260 < name.Length)
			{
				throw new ArgumentException(SR.GetString("Argument_WaitHandleNameTooLong"));
			}
			result = null;
			SafeWaitHandle safeWaitHandle = SafeNativeMethods.OpenSemaphore((int)rights, false, name);
			if (safeWaitHandle.IsInvalid)
			{
				int lastWin32Error = Marshal.GetLastWin32Error();
				if (2 == lastWin32Error || 123 == lastWin32Error)
				{
					return Semaphore.OpenExistingResult.NameNotFound;
				}
				if (3 == lastWin32Error)
				{
					return Semaphore.OpenExistingResult.PathNotFound;
				}
				if (name != null && name.Length != 0 && 6 == lastWin32Error)
				{
					return Semaphore.OpenExistingResult.NameInvalid;
				}
				InternalResources.WinIOError();
			}
			result = new Semaphore(safeWaitHandle);
			return Semaphore.OpenExistingResult.Success;
		}

		// Token: 0x060025B6 RID: 9654 RVA: 0x000AF3C4 File Offset: 0x000AD5C4
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		[PrePrepareMethod]
		[global::__DynamicallyInvokable]
		public int Release()
		{
			return this.Release(1);
		}

		// Token: 0x060025B7 RID: 9655 RVA: 0x000AF3D0 File Offset: 0x000AD5D0
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		[global::__DynamicallyInvokable]
		public int Release(int releaseCount)
		{
			if (releaseCount < 1)
			{
				throw new ArgumentOutOfRangeException("releaseCount", SR.GetString("ArgumentOutOfRange_NeedNonNegNumRequired"));
			}
			int num;
			if (!SafeNativeMethods.ReleaseSemaphore(base.SafeWaitHandle, releaseCount, out num))
			{
				throw new SemaphoreFullException();
			}
			return num;
		}

		// Token: 0x060025B8 RID: 9656 RVA: 0x000AF40D File Offset: 0x000AD60D
		public SemaphoreSecurity GetAccessControl()
		{
			return new SemaphoreSecurity(base.SafeWaitHandle, AccessControlSections.Access | AccessControlSections.Owner | AccessControlSections.Group);
		}

		// Token: 0x060025B9 RID: 9657 RVA: 0x000AF41C File Offset: 0x000AD61C
		public void SetAccessControl(SemaphoreSecurity semaphoreSecurity)
		{
			if (semaphoreSecurity == null)
			{
				throw new ArgumentNullException("semaphoreSecurity");
			}
			semaphoreSecurity.Persist(base.SafeWaitHandle);
		}

		// Token: 0x04002057 RID: 8279
		private const int MAX_PATH = 260;

		// Token: 0x0200080E RID: 2062
		private new enum OpenExistingResult
		{
			// Token: 0x0400356E RID: 13678
			Success,
			// Token: 0x0400356F RID: 13679
			NameNotFound,
			// Token: 0x04003570 RID: 13680
			PathNotFound,
			// Token: 0x04003571 RID: 13681
			NameInvalid
		}
	}
}
