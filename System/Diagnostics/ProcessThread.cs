using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using Microsoft.Win32;
using Microsoft.Win32.SafeHandles;

namespace System.Diagnostics
{
	// Token: 0x02000500 RID: 1280
	[Designer("System.Diagnostics.Design.ProcessThreadDesigner, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	[HostProtection(SecurityAction.LinkDemand, SelfAffectingProcessMgmt = true, SelfAffectingThreading = true)]
	public class ProcessThread : Component
	{
		// Token: 0x06003090 RID: 12432 RVA: 0x000DBA7F File Offset: 0x000D9C7F
		internal ProcessThread(bool isRemoteMachine, ThreadInfo threadInfo)
		{
			this.isRemoteMachine = isRemoteMachine;
			this.threadInfo = threadInfo;
			GC.SuppressFinalize(this);
		}

		// Token: 0x17000BE6 RID: 3046
		// (get) Token: 0x06003091 RID: 12433 RVA: 0x000DBA9B File Offset: 0x000D9C9B
		[MonitoringDescription("ThreadBasePriority")]
		public int BasePriority
		{
			get
			{
				return this.threadInfo.basePriority;
			}
		}

		// Token: 0x17000BE7 RID: 3047
		// (get) Token: 0x06003092 RID: 12434 RVA: 0x000DBAA8 File Offset: 0x000D9CA8
		[MonitoringDescription("ThreadCurrentPriority")]
		public int CurrentPriority
		{
			get
			{
				return this.threadInfo.currentPriority;
			}
		}

		// Token: 0x17000BE8 RID: 3048
		// (get) Token: 0x06003093 RID: 12435 RVA: 0x000DBAB5 File Offset: 0x000D9CB5
		[MonitoringDescription("ThreadId")]
		public int Id
		{
			get
			{
				return this.threadInfo.threadId;
			}
		}

		// Token: 0x17000BE9 RID: 3049
		// (set) Token: 0x06003094 RID: 12436 RVA: 0x000DBAC4 File Offset: 0x000D9CC4
		[Browsable(false)]
		public int IdealProcessor
		{
			set
			{
				Microsoft.Win32.SafeHandles.SafeThreadHandle safeThreadHandle = null;
				try
				{
					safeThreadHandle = this.OpenThreadHandle(32);
					if (Microsoft.Win32.NativeMethods.SetThreadIdealProcessor(safeThreadHandle, value) < 0)
					{
						throw new Win32Exception();
					}
				}
				finally
				{
					ProcessThread.CloseThreadHandle(safeThreadHandle);
				}
			}
		}

		// Token: 0x17000BEA RID: 3050
		// (get) Token: 0x06003095 RID: 12437 RVA: 0x000DBB08 File Offset: 0x000D9D08
		// (set) Token: 0x06003096 RID: 12438 RVA: 0x000DBB6C File Offset: 0x000D9D6C
		[MonitoringDescription("ThreadPriorityBoostEnabled")]
		public bool PriorityBoostEnabled
		{
			get
			{
				if (!this.havePriorityBoostEnabled)
				{
					Microsoft.Win32.SafeHandles.SafeThreadHandle safeThreadHandle = null;
					try
					{
						safeThreadHandle = this.OpenThreadHandle(64);
						bool flag = false;
						if (!Microsoft.Win32.NativeMethods.GetThreadPriorityBoost(safeThreadHandle, out flag))
						{
							throw new Win32Exception();
						}
						this.priorityBoostEnabled = !flag;
						this.havePriorityBoostEnabled = true;
					}
					finally
					{
						ProcessThread.CloseThreadHandle(safeThreadHandle);
					}
				}
				return this.priorityBoostEnabled;
			}
			set
			{
				Microsoft.Win32.SafeHandles.SafeThreadHandle safeThreadHandle = null;
				try
				{
					safeThreadHandle = this.OpenThreadHandle(32);
					if (!Microsoft.Win32.NativeMethods.SetThreadPriorityBoost(safeThreadHandle, !value))
					{
						throw new Win32Exception();
					}
					this.priorityBoostEnabled = value;
					this.havePriorityBoostEnabled = true;
				}
				finally
				{
					ProcessThread.CloseThreadHandle(safeThreadHandle);
				}
			}
		}

		// Token: 0x17000BEB RID: 3051
		// (get) Token: 0x06003097 RID: 12439 RVA: 0x000DBBC0 File Offset: 0x000D9DC0
		// (set) Token: 0x06003098 RID: 12440 RVA: 0x000DBC24 File Offset: 0x000D9E24
		[MonitoringDescription("ThreadPriorityLevel")]
		public ThreadPriorityLevel PriorityLevel
		{
			get
			{
				if (!this.havePriorityLevel)
				{
					Microsoft.Win32.SafeHandles.SafeThreadHandle safeThreadHandle = null;
					try
					{
						safeThreadHandle = this.OpenThreadHandle(64);
						int threadPriority = Microsoft.Win32.NativeMethods.GetThreadPriority(safeThreadHandle);
						if (threadPriority == 2147483647)
						{
							throw new Win32Exception();
						}
						this.priorityLevel = (ThreadPriorityLevel)threadPriority;
						this.havePriorityLevel = true;
					}
					finally
					{
						ProcessThread.CloseThreadHandle(safeThreadHandle);
					}
				}
				return this.priorityLevel;
			}
			set
			{
				Microsoft.Win32.SafeHandles.SafeThreadHandle safeThreadHandle = null;
				try
				{
					safeThreadHandle = this.OpenThreadHandle(32);
					if (!Microsoft.Win32.NativeMethods.SetThreadPriority(safeThreadHandle, (int)value))
					{
						throw new Win32Exception();
					}
					this.priorityLevel = value;
				}
				finally
				{
					ProcessThread.CloseThreadHandle(safeThreadHandle);
				}
			}
		}

		// Token: 0x17000BEC RID: 3052
		// (get) Token: 0x06003099 RID: 12441 RVA: 0x000DBC6C File Offset: 0x000D9E6C
		[MonitoringDescription("ThreadPrivilegedProcessorTime")]
		public TimeSpan PrivilegedProcessorTime
		{
			get
			{
				this.EnsureState(ProcessThread.State.IsNt);
				return this.GetThreadTimes().PrivilegedProcessorTime;
			}
		}

		// Token: 0x17000BED RID: 3053
		// (get) Token: 0x0600309A RID: 12442 RVA: 0x000DBC80 File Offset: 0x000D9E80
		[MonitoringDescription("ThreadStartAddress")]
		public IntPtr StartAddress
		{
			get
			{
				this.EnsureState(ProcessThread.State.IsNt);
				return this.threadInfo.startAddress;
			}
		}

		// Token: 0x17000BEE RID: 3054
		// (get) Token: 0x0600309B RID: 12443 RVA: 0x000DBC94 File Offset: 0x000D9E94
		[MonitoringDescription("ThreadStartTime")]
		public DateTime StartTime
		{
			get
			{
				this.EnsureState(ProcessThread.State.IsNt);
				return this.GetThreadTimes().StartTime;
			}
		}

		// Token: 0x17000BEF RID: 3055
		// (get) Token: 0x0600309C RID: 12444 RVA: 0x000DBCA8 File Offset: 0x000D9EA8
		[MonitoringDescription("ThreadThreadState")]
		public ThreadState ThreadState
		{
			get
			{
				this.EnsureState(ProcessThread.State.IsNt);
				return this.threadInfo.threadState;
			}
		}

		// Token: 0x17000BF0 RID: 3056
		// (get) Token: 0x0600309D RID: 12445 RVA: 0x000DBCBC File Offset: 0x000D9EBC
		[MonitoringDescription("ThreadTotalProcessorTime")]
		public TimeSpan TotalProcessorTime
		{
			get
			{
				this.EnsureState(ProcessThread.State.IsNt);
				return this.GetThreadTimes().TotalProcessorTime;
			}
		}

		// Token: 0x17000BF1 RID: 3057
		// (get) Token: 0x0600309E RID: 12446 RVA: 0x000DBCD0 File Offset: 0x000D9ED0
		[MonitoringDescription("ThreadUserProcessorTime")]
		public TimeSpan UserProcessorTime
		{
			get
			{
				this.EnsureState(ProcessThread.State.IsNt);
				return this.GetThreadTimes().UserProcessorTime;
			}
		}

		// Token: 0x17000BF2 RID: 3058
		// (get) Token: 0x0600309F RID: 12447 RVA: 0x000DBCE4 File Offset: 0x000D9EE4
		[MonitoringDescription("ThreadWaitReason")]
		public ThreadWaitReason WaitReason
		{
			get
			{
				this.EnsureState(ProcessThread.State.IsNt);
				if (this.threadInfo.threadState != ThreadState.Wait)
				{
					throw new InvalidOperationException(SR.GetString("WaitReasonUnavailable"));
				}
				return this.threadInfo.threadWaitReason;
			}
		}

		// Token: 0x060030A0 RID: 12448 RVA: 0x000DBD16 File Offset: 0x000D9F16
		private static void CloseThreadHandle(Microsoft.Win32.SafeHandles.SafeThreadHandle handle)
		{
			if (handle != null)
			{
				handle.Close();
			}
		}

		// Token: 0x060030A1 RID: 12449 RVA: 0x000DBD24 File Offset: 0x000D9F24
		private void EnsureState(ProcessThread.State state)
		{
			if ((state & ProcessThread.State.IsLocal) != (ProcessThread.State)0 && this.isRemoteMachine)
			{
				throw new NotSupportedException(SR.GetString("NotSupportedRemoteThread"));
			}
			if ((state & ProcessThread.State.IsNt) != (ProcessThread.State)0 && Environment.OSVersion.Platform != PlatformID.Win32NT)
			{
				throw new PlatformNotSupportedException(SR.GetString("WinNTRequired"));
			}
		}

		// Token: 0x060030A2 RID: 12450 RVA: 0x000DBD70 File Offset: 0x000D9F70
		private Microsoft.Win32.SafeHandles.SafeThreadHandle OpenThreadHandle(int access)
		{
			this.EnsureState(ProcessThread.State.IsLocal);
			return ProcessManager.OpenThread(this.threadInfo.threadId, access);
		}

		// Token: 0x060030A3 RID: 12451 RVA: 0x000DBD8A File Offset: 0x000D9F8A
		public void ResetIdealProcessor()
		{
			this.IdealProcessor = 32;
		}

		// Token: 0x17000BF3 RID: 3059
		// (set) Token: 0x060030A4 RID: 12452 RVA: 0x000DBD94 File Offset: 0x000D9F94
		[Browsable(false)]
		public IntPtr ProcessorAffinity
		{
			set
			{
				Microsoft.Win32.SafeHandles.SafeThreadHandle safeThreadHandle = null;
				try
				{
					safeThreadHandle = this.OpenThreadHandle(96);
					if (Microsoft.Win32.NativeMethods.SetThreadAffinityMask(safeThreadHandle, new HandleRef(this, value)) == IntPtr.Zero)
					{
						throw new Win32Exception();
					}
				}
				finally
				{
					ProcessThread.CloseThreadHandle(safeThreadHandle);
				}
			}
		}

		// Token: 0x060030A5 RID: 12453 RVA: 0x000DBDE4 File Offset: 0x000D9FE4
		private ProcessThreadTimes GetThreadTimes()
		{
			ProcessThreadTimes processThreadTimes = new ProcessThreadTimes();
			Microsoft.Win32.SafeHandles.SafeThreadHandle safeThreadHandle = null;
			try
			{
				safeThreadHandle = this.OpenThreadHandle(64);
				if (!Microsoft.Win32.NativeMethods.GetThreadTimes(safeThreadHandle, out processThreadTimes.create, out processThreadTimes.exit, out processThreadTimes.kernel, out processThreadTimes.user))
				{
					throw new Win32Exception();
				}
			}
			finally
			{
				ProcessThread.CloseThreadHandle(safeThreadHandle);
			}
			return processThreadTimes;
		}

		// Token: 0x040028A6 RID: 10406
		private ThreadInfo threadInfo;

		// Token: 0x040028A7 RID: 10407
		private bool isRemoteMachine;

		// Token: 0x040028A8 RID: 10408
		private bool priorityBoostEnabled;

		// Token: 0x040028A9 RID: 10409
		private bool havePriorityBoostEnabled;

		// Token: 0x040028AA RID: 10410
		private ThreadPriorityLevel priorityLevel;

		// Token: 0x040028AB RID: 10411
		private bool havePriorityLevel;

		// Token: 0x02000886 RID: 2182
		private enum State
		{
			// Token: 0x0400378D RID: 14221
			IsLocal = 2,
			// Token: 0x0400378E RID: 14222
			IsNt = 4
		}
	}
}
