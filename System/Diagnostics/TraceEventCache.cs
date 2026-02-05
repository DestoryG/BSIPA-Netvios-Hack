using System;
using System.Collections;
using System.Globalization;
using System.Security.Permissions;
using System.Threading;

namespace System.Diagnostics
{
	// Token: 0x020004AE RID: 1198
	public class TraceEventCache
	{
		// Token: 0x17000AC5 RID: 2757
		// (get) Token: 0x06002C7B RID: 11387 RVA: 0x000C7BE7 File Offset: 0x000C5DE7
		internal Guid ActivityId
		{
			get
			{
				return Trace.CorrelationManager.ActivityId;
			}
		}

		// Token: 0x17000AC6 RID: 2758
		// (get) Token: 0x06002C7C RID: 11388 RVA: 0x000C7BF3 File Offset: 0x000C5DF3
		public string Callstack
		{
			get
			{
				if (this.stackTrace == null)
				{
					this.stackTrace = Environment.StackTrace;
				}
				else
				{
					new EnvironmentPermission(PermissionState.Unrestricted).Demand();
				}
				return this.stackTrace;
			}
		}

		// Token: 0x17000AC7 RID: 2759
		// (get) Token: 0x06002C7D RID: 11389 RVA: 0x000C7C1B File Offset: 0x000C5E1B
		public Stack LogicalOperationStack
		{
			get
			{
				return Trace.CorrelationManager.LogicalOperationStack;
			}
		}

		// Token: 0x17000AC8 RID: 2760
		// (get) Token: 0x06002C7E RID: 11390 RVA: 0x000C7C27 File Offset: 0x000C5E27
		public DateTime DateTime
		{
			get
			{
				if (this.dateTime == DateTime.MinValue)
				{
					this.dateTime = DateTime.UtcNow;
				}
				return this.dateTime;
			}
		}

		// Token: 0x17000AC9 RID: 2761
		// (get) Token: 0x06002C7F RID: 11391 RVA: 0x000C7C4C File Offset: 0x000C5E4C
		public int ProcessId
		{
			get
			{
				return TraceEventCache.GetProcessId();
			}
		}

		// Token: 0x17000ACA RID: 2762
		// (get) Token: 0x06002C80 RID: 11392 RVA: 0x000C7C54 File Offset: 0x000C5E54
		public string ThreadId
		{
			get
			{
				return TraceEventCache.GetThreadId().ToString(CultureInfo.InvariantCulture);
			}
		}

		// Token: 0x17000ACB RID: 2763
		// (get) Token: 0x06002C81 RID: 11393 RVA: 0x000C7C73 File Offset: 0x000C5E73
		public long Timestamp
		{
			get
			{
				if (this.timeStamp == -1L)
				{
					this.timeStamp = Stopwatch.GetTimestamp();
				}
				return this.timeStamp;
			}
		}

		// Token: 0x06002C82 RID: 11394 RVA: 0x000C7C90 File Offset: 0x000C5E90
		private static void InitProcessInfo()
		{
			new SecurityPermission(SecurityPermissionFlag.UnmanagedCode).Demand();
			if (TraceEventCache.processName == null)
			{
				Process currentProcess = Process.GetCurrentProcess();
				try
				{
					TraceEventCache.processId = currentProcess.Id;
					TraceEventCache.processName = currentProcess.ProcessName;
				}
				finally
				{
					currentProcess.Dispose();
				}
			}
		}

		// Token: 0x06002C83 RID: 11395 RVA: 0x000C7CEC File Offset: 0x000C5EEC
		internal static int GetProcessId()
		{
			TraceEventCache.InitProcessInfo();
			return TraceEventCache.processId;
		}

		// Token: 0x06002C84 RID: 11396 RVA: 0x000C7CFA File Offset: 0x000C5EFA
		internal static string GetProcessName()
		{
			TraceEventCache.InitProcessInfo();
			return TraceEventCache.processName;
		}

		// Token: 0x06002C85 RID: 11397 RVA: 0x000C7D08 File Offset: 0x000C5F08
		internal static int GetThreadId()
		{
			return Thread.CurrentThread.ManagedThreadId;
		}

		// Token: 0x040026C2 RID: 9922
		private static volatile int processId;

		// Token: 0x040026C3 RID: 9923
		private static volatile string processName;

		// Token: 0x040026C4 RID: 9924
		private long timeStamp = -1L;

		// Token: 0x040026C5 RID: 9925
		private DateTime dateTime = DateTime.MinValue;

		// Token: 0x040026C6 RID: 9926
		private string stackTrace;
	}
}
