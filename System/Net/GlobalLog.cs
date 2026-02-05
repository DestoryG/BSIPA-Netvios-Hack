using System;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.ConstrainedExecution;

namespace System.Net
{
	// Token: 0x020001C3 RID: 451
	internal static class GlobalLog
	{
		// Token: 0x060011BB RID: 4539 RVA: 0x000601A2 File Offset: 0x0005E3A2
		[ReliabilityContract(Consistency.MayCorruptAppDomain, Cer.None)]
		private static BaseLoggingObject LoggingInitialize()
		{
			return new BaseLoggingObject();
		}

		// Token: 0x170003DE RID: 990
		// (get) Token: 0x060011BC RID: 4540 RVA: 0x000601A9 File Offset: 0x0005E3A9
		internal static ThreadKinds CurrentThreadKind
		{
			get
			{
				return ThreadKinds.Unknown;
			}
		}

		// Token: 0x060011BD RID: 4541 RVA: 0x000601AC File Offset: 0x0005E3AC
		[Conditional("DEBUG")]
		[ReliabilityContract(Consistency.MayCorruptAppDomain, Cer.None)]
		internal static void SetThreadSource(ThreadKinds source)
		{
		}

		// Token: 0x060011BE RID: 4542 RVA: 0x000601AE File Offset: 0x0005E3AE
		[Conditional("DEBUG")]
		[ReliabilityContract(Consistency.MayCorruptAppDomain, Cer.None)]
		internal static void ThreadContract(ThreadKinds kind, string errorMsg)
		{
		}

		// Token: 0x060011BF RID: 4543 RVA: 0x000601B0 File Offset: 0x0005E3B0
		[Conditional("DEBUG")]
		[ReliabilityContract(Consistency.MayCorruptAppDomain, Cer.None)]
		internal static void ThreadContract(ThreadKinds kind, ThreadKinds allowedSources, string errorMsg)
		{
			if ((kind & ThreadKinds.SourceMask) != ThreadKinds.Unknown || (allowedSources & ThreadKinds.SourceMask) != allowedSources)
			{
				throw new InternalException();
			}
			ThreadKinds currentThreadKind = GlobalLog.CurrentThreadKind;
		}

		// Token: 0x060011C0 RID: 4544 RVA: 0x000601DC File Offset: 0x0005E3DC
		[Conditional("TRAVE")]
		public static void AddToArray(string msg)
		{
		}

		// Token: 0x060011C1 RID: 4545 RVA: 0x000601DE File Offset: 0x0005E3DE
		[Conditional("TRAVE")]
		public static void Ignore(object msg)
		{
		}

		// Token: 0x060011C2 RID: 4546 RVA: 0x000601E0 File Offset: 0x0005E3E0
		[Conditional("TRAVE")]
		[ReliabilityContract(Consistency.MayCorruptAppDomain, Cer.None)]
		public static void Print(string msg)
		{
		}

		// Token: 0x060011C3 RID: 4547 RVA: 0x000601E2 File Offset: 0x0005E3E2
		[Conditional("TRAVE")]
		public static void PrintHex(string msg, object value)
		{
		}

		// Token: 0x060011C4 RID: 4548 RVA: 0x000601E4 File Offset: 0x0005E3E4
		[Conditional("TRAVE")]
		public static void Enter(string func)
		{
		}

		// Token: 0x060011C5 RID: 4549 RVA: 0x000601E6 File Offset: 0x0005E3E6
		[Conditional("TRAVE")]
		public static void Enter(string func, string parms)
		{
		}

		// Token: 0x060011C6 RID: 4550 RVA: 0x000601E8 File Offset: 0x0005E3E8
		[Conditional("DEBUG")]
		[Conditional("_FORCE_ASSERTS")]
		[ReliabilityContract(Consistency.MayCorruptAppDomain, Cer.None)]
		public static void Assert(bool condition, string messageFormat, params object[] data)
		{
			if (!condition)
			{
				string text = string.Format(CultureInfo.InvariantCulture, messageFormat, data);
				int num = text.IndexOf('|');
				if (num != -1)
				{
					int num2 = text.Length - num - 1;
				}
			}
		}

		// Token: 0x060011C7 RID: 4551 RVA: 0x0006021D File Offset: 0x0005E41D
		[Conditional("DEBUG")]
		[Conditional("_FORCE_ASSERTS")]
		[ReliabilityContract(Consistency.MayCorruptAppDomain, Cer.None)]
		public static void Assert(string message)
		{
		}

		// Token: 0x060011C8 RID: 4552 RVA: 0x00060220 File Offset: 0x0005E420
		[Conditional("DEBUG")]
		[Conditional("_FORCE_ASSERTS")]
		[ReliabilityContract(Consistency.MayCorruptAppDomain, Cer.None)]
		public static void Assert(string message, string detailMessage)
		{
			try
			{
				GlobalLog.Logobject.DumpArray(false);
			}
			finally
			{
				UnsafeNclNativeMethods.DebugBreak();
				Debugger.Break();
			}
		}

		// Token: 0x060011C9 RID: 4553 RVA: 0x00060258 File Offset: 0x0005E458
		[Conditional("TRAVE")]
		public static void LeaveException(string func, Exception exception)
		{
		}

		// Token: 0x060011CA RID: 4554 RVA: 0x0006025A File Offset: 0x0005E45A
		[Conditional("TRAVE")]
		public static void Leave(string func)
		{
		}

		// Token: 0x060011CB RID: 4555 RVA: 0x0006025C File Offset: 0x0005E45C
		[Conditional("TRAVE")]
		public static void Leave(string func, string result)
		{
		}

		// Token: 0x060011CC RID: 4556 RVA: 0x0006025E File Offset: 0x0005E45E
		[Conditional("TRAVE")]
		public static void Leave(string func, int returnval)
		{
		}

		// Token: 0x060011CD RID: 4557 RVA: 0x00060260 File Offset: 0x0005E460
		[Conditional("TRAVE")]
		public static void Leave(string func, bool returnval)
		{
		}

		// Token: 0x060011CE RID: 4558 RVA: 0x00060262 File Offset: 0x0005E462
		[Conditional("TRAVE")]
		public static void DumpArray()
		{
		}

		// Token: 0x060011CF RID: 4559 RVA: 0x00060264 File Offset: 0x0005E464
		[Conditional("TRAVE")]
		public static void Dump(byte[] buffer)
		{
		}

		// Token: 0x060011D0 RID: 4560 RVA: 0x00060266 File Offset: 0x0005E466
		[Conditional("TRAVE")]
		public static void Dump(byte[] buffer, int length)
		{
		}

		// Token: 0x060011D1 RID: 4561 RVA: 0x00060268 File Offset: 0x0005E468
		[Conditional("TRAVE")]
		public static void Dump(byte[] buffer, int offset, int length)
		{
		}

		// Token: 0x060011D2 RID: 4562 RVA: 0x0006026A File Offset: 0x0005E46A
		[Conditional("TRAVE")]
		public static void Dump(IntPtr buffer, int offset, int length)
		{
		}

		// Token: 0x060011D3 RID: 4563 RVA: 0x0006026C File Offset: 0x0005E46C
		[Conditional("DEBUG")]
		internal static void DebugAddRequest(HttpWebRequest request, Connection connection, int flags)
		{
		}

		// Token: 0x060011D4 RID: 4564 RVA: 0x0006026E File Offset: 0x0005E46E
		[Conditional("DEBUG")]
		internal static void DebugRemoveRequest(HttpWebRequest request)
		{
		}

		// Token: 0x060011D5 RID: 4565 RVA: 0x00060270 File Offset: 0x0005E470
		[Conditional("DEBUG")]
		internal static void DebugUpdateRequest(HttpWebRequest request, Connection connection, int flags)
		{
		}

		// Token: 0x04001471 RID: 5233
		private static BaseLoggingObject Logobject = GlobalLog.LoggingInitialize();
	}
}
