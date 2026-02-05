using System;
using System.Runtime.InteropServices;
using IPA.Logging;

namespace IPA.Utilities
{
	/// <summary>
	/// Provides utilities for managing various critical sections.
	/// </summary>
	// Token: 0x02000018 RID: 24
	public static class CriticalSection
	{
		// Token: 0x06000066 RID: 102 RVA: 0x00003438 File Offset: 0x00001638
		internal static void Configure()
		{
			Logger.log.Debug("Configuring exit handlers");
			CriticalSection.ResetExitHandlers();
		}

		// Token: 0x06000067 RID: 103 RVA: 0x0000344E File Offset: 0x0000164E
		private static void Reset(object sender, EventArgs e)
		{
			Win32.SetConsoleCtrlHandler(CriticalSection.registeredHandler, false);
			CriticalSection.WinHttp.SetPeekMessageHook(null);
		}

		// Token: 0x06000068 RID: 104 RVA: 0x00003464 File Offset: 0x00001664
		internal static void ResetExitHandlers()
		{
			Win32.SetConsoleCtrlHandler(CriticalSection.registeredHandler, false);
			Win32.SetConsoleCtrlHandler(CriticalSection.registeredHandler, true);
			CriticalSection.WinHttp.SetPeekMessageHook(new CriticalSection.WinHttp.PeekMessageHook(CriticalSection.PeekMessageHook));
			AppDomain.CurrentDomain.ProcessExit -= CriticalSection.OnProcessExit;
			AppDomain.CurrentDomain.ProcessExit += CriticalSection.OnProcessExit;
		}

		// Token: 0x06000069 RID: 105 RVA: 0x000034C6 File Offset: 0x000016C6
		private static void OnProcessExit(object sender, EventArgs args)
		{
			CriticalSection.WinHttp.SetIgnoreUnhandledExceptions(true);
		}

		// Token: 0x0600006A RID: 106 RVA: 0x000034CE File Offset: 0x000016CE
		private static bool PeekMessageHook(bool isW, uint result, [MarshalAs(UnmanagedType.LPStruct)] in Win32.MSG message, IntPtr hwnd, uint filterMin, uint filterMax, ref Win32.PeekMessageParams removeMsg)
		{
			if (!CriticalSection.isInExecuteSection)
			{
				return false;
			}
			if (result == 0U)
			{
				return false;
			}
			if (message.message != Win32.WM.CLOSE)
			{
				return false;
			}
			if (removeMsg != Win32.PeekMessageParams.PM_REMOVE)
			{
				removeMsg = Win32.PeekMessageParams.PM_REMOVE;
				CriticalSection.exitRecieved = true;
				return true;
			}
			removeMsg = Win32.PeekMessageParams.PM_NOREMOVE;
			return true;
		}

		// Token: 0x0600006B RID: 107 RVA: 0x00003505 File Offset: 0x00001705
		private static bool HandleExit(Win32.CtrlTypes type)
		{
			return CriticalSection._handler != null && CriticalSection._handler(type);
		}

		/// <summary>
		/// Creates an <see cref="T:IPA.Utilities.CriticalSection.AutoExecuteSection" /> for automated management of an execute section.
		/// </summary>
		/// <returns>the new <see cref="T:IPA.Utilities.CriticalSection.AutoExecuteSection" /> that manages the section</returns>
		// Token: 0x0600006C RID: 108 RVA: 0x0000351B File Offset: 0x0000171B
		public static CriticalSection.AutoExecuteSection ExecuteSection()
		{
			return new CriticalSection.AutoExecuteSection(true);
		}

		/// <summary>
		/// Enters a critical execution section. Does not nest.
		/// </summary>
		/// <note>
		/// During a critical execution section, the program must execute until the end of the section before
		/// exiting. If an exit signal is recieved during the section, it will be canceled, and the process
		/// will terminate at the end of the section.
		/// </note>
		// Token: 0x0600006D RID: 109 RVA: 0x00003523 File Offset: 0x00001723
		public static void EnterExecuteSection()
		{
			CriticalSection.ResetExitHandlers();
			CriticalSection.exitRecieved = false;
			CriticalSection._handler = (Win32.CtrlTypes sig) => CriticalSection.exitRecieved = true;
			CriticalSection.isInExecuteSection = true;
		}

		/// <summary>
		/// Exits a critical execution section. Does not nest.
		/// </summary>
		/// <note>
		/// During a critical execution section, the program must execute until the end of the section before
		/// exiting. If an exit signal is recieved during the section, it will be canceled, and the process
		/// will terminate at the end of the section.
		/// </note>
		// Token: 0x0600006E RID: 110 RVA: 0x0000355E File Offset: 0x0000175E
		public static void ExitExecuteSection()
		{
			CriticalSection._handler = null;
			CriticalSection.isInExecuteSection = false;
			CriticalSection.Reset(null, null);
			if (CriticalSection.exitRecieved)
			{
				Environment.Exit(1);
			}
		}

		// Token: 0x04000022 RID: 34
		private static readonly Win32.ConsoleCtrlDelegate registeredHandler = new Win32.ConsoleCtrlDelegate(CriticalSection.HandleExit);

		// Token: 0x04000023 RID: 35
		private static Win32.ConsoleCtrlDelegate _handler = null;

		// Token: 0x04000024 RID: 36
		private static volatile bool isInExecuteSection = false;

		// Token: 0x04000025 RID: 37
		private static volatile bool exitRecieved = false;

		// Token: 0x020000BD RID: 189
		private static class WinHttp
		{
			// Token: 0x0600048E RID: 1166
			[DllImport("bsipa-doorstop")]
			public static extern void SetPeekMessageHook([MarshalAs(UnmanagedType.FunctionPtr)] CriticalSection.WinHttp.PeekMessageHook hook);

			// Token: 0x0600048F RID: 1167
			[DllImport("bsipa-doorstop")]
			public static extern void SetIgnoreUnhandledExceptions([MarshalAs(UnmanagedType.Bool)] bool ignore);

			// Token: 0x0200015D RID: 349
			// (Invoke) Token: 0x060006B3 RID: 1715
			public delegate bool PeekMessageHook(bool isW, uint result, [MarshalAs(UnmanagedType.LPStruct)] in Win32.MSG message, IntPtr hwnd, uint filterMin, uint filterMax, ref Win32.PeekMessageParams removeMsg);
		}

		/// <summary>
		/// A struct that allows <c>using</c> blocks to manage an execute section.
		/// </summary>
		// Token: 0x020000BE RID: 190
		public struct AutoExecuteSection : IDisposable
		{
			// Token: 0x06000490 RID: 1168 RVA: 0x00015C37 File Offset: 0x00013E37
			internal AutoExecuteSection(bool val)
			{
				this.constructed = val && !CriticalSection.isInExecuteSection;
				if (this.constructed)
				{
					CriticalSection.EnterExecuteSection();
				}
			}

			// Token: 0x06000491 RID: 1169 RVA: 0x00015C5C File Offset: 0x00013E5C
			void IDisposable.Dispose()
			{
				if (this.constructed)
				{
					CriticalSection.ExitExecuteSection();
				}
			}

			// Token: 0x040001A5 RID: 421
			private readonly bool constructed;
		}
	}
}
