using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace IPA.Logging
{
	// Token: 0x0200002F RID: 47
	internal static class WinConsole
	{
		// Token: 0x1700002B RID: 43
		// (get) Token: 0x06000103 RID: 259 RVA: 0x00004C77 File Offset: 0x00002E77
		// (set) Token: 0x06000104 RID: 260 RVA: 0x00004C7E File Offset: 0x00002E7E
		public static bool UseVTEscapes { get; private set; } = true;

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x06000105 RID: 261 RVA: 0x00004C86 File Offset: 0x00002E86
		internal static IntPtr OutHandle
		{
			get
			{
				return WinConsole.outHandle.DangerousGetHandle();
			}
		}

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x06000106 RID: 262 RVA: 0x00004C92 File Offset: 0x00002E92
		internal static IntPtr InHandle
		{
			get
			{
				return WinConsole.inHandle.DangerousGetHandle();
			}
		}

		// Token: 0x06000107 RID: 263 RVA: 0x00004CA0 File Offset: 0x00002EA0
		public static void Initialize(bool alwaysCreateNewConsole = true)
		{
			bool consoleAttached = true;
			if (alwaysCreateNewConsole || (WinConsole.AttachConsole(4294967295U) == 0U && (long)Marshal.GetLastWin32Error() != 5L))
			{
				consoleAttached = WinConsole.AllocConsole() != 0;
			}
			if (consoleAttached)
			{
				WinConsole.InitializeStreams();
				WinConsole.IsInitialized = true;
			}
		}

		// Token: 0x06000108 RID: 264 RVA: 0x00004CDB File Offset: 0x00002EDB
		public static void InitializeStreams()
		{
			WinConsole.InitializeOutStream();
			WinConsole.InitializeInStream();
		}

		// Token: 0x06000109 RID: 265 RVA: 0x00004CE8 File Offset: 0x00002EE8
		private static void InitializeOutStream()
		{
			FileStream fs = WinConsole.CreateFileStream("CONOUT$", 1073741824U, 2U, FileAccess.Write, out WinConsole.outHandle);
			if (fs != null)
			{
				StreamWriter streamWriter = new StreamWriter(fs);
				streamWriter.AutoFlush = true;
				WinConsole.ConOut = streamWriter;
				Console.SetOut(streamWriter);
				Console.SetError(streamWriter);
				IntPtr handle = WinConsole.GetStdHandle(-11);
				uint mode;
				if (WinConsole.GetConsoleMode(handle, out mode))
				{
					mode |= 4U;
					if (!WinConsole.SetConsoleMode(handle, mode))
					{
						WinConsole.UseVTEscapes = false;
						Console.Error.WriteLine("Could not enable VT100 escape code processing (maybe you're running an old Windows?): " + new Win32Exception(Marshal.GetLastWin32Error()).Message);
						return;
					}
				}
				else
				{
					WinConsole.UseVTEscapes = false;
					Console.Error.WriteLine("Could not enable VT100 escape code processing (maybe you're running an old Windows?): " + new Win32Exception(Marshal.GetLastWin32Error()).Message);
				}
			}
		}

		// Token: 0x0600010A RID: 266 RVA: 0x00004DA4 File Offset: 0x00002FA4
		private static void InitializeInStream()
		{
			FileStream fs = WinConsole.CreateFileStream("CONIN$", 2147483648U, 1U, FileAccess.Read, out WinConsole.inHandle);
			if (fs != null)
			{
				Console.SetIn(WinConsole.ConIn = new StreamReader(fs));
			}
		}

		// Token: 0x0600010B RID: 267 RVA: 0x00004DDC File Offset: 0x00002FDC
		private static FileStream CreateFileStream(string name, uint win32DesiredAccess, uint win32ShareMode, FileAccess dotNetFileAccess, out SafeFileHandle handle)
		{
			SafeFileHandle file = new SafeFileHandle(WinConsole.CreateFileW(name, win32DesiredAccess, win32ShareMode, IntPtr.Zero, 3U, 128U, IntPtr.Zero), true);
			if (!file.IsInvalid)
			{
				handle = file;
				return new FileStream(file, dotNetFileAccess);
			}
			handle = null;
			return null;
		}

		// Token: 0x0600010C RID: 268
		[DllImport("kernel32.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, SetLastError = true)]
		private static extern int AllocConsole();

		// Token: 0x0600010D RID: 269
		[DllImport("kernel32.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, SetLastError = true)]
		private static extern uint AttachConsole(uint dwProcessId);

		// Token: 0x0600010E RID: 270
		[DllImport("kernel32.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, SetLastError = true)]
		private static extern IntPtr CreateFileW(string lpFileName, uint dwDesiredAccess, uint dwShareMode, IntPtr lpSecurityAttributes, uint dwCreationDisposition, uint dwFlagsAndAttributes, IntPtr hTemplateFile);

		// Token: 0x0600010F RID: 271
		[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		private static extern bool GetConsoleMode(IntPtr hConsoleHandle, out uint lpMode);

		// Token: 0x06000110 RID: 272
		[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		private static extern bool SetConsoleMode(IntPtr hConsoleHandle, uint dwMode);

		// Token: 0x06000111 RID: 273
		[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		private static extern IntPtr GetStdHandle(int nStdHandle);

		// Token: 0x0400004C RID: 76
		internal static TextWriter ConOut;

		// Token: 0x0400004D RID: 77
		internal static TextReader ConIn;

		// Token: 0x0400004E RID: 78
		private static SafeFileHandle outHandle;

		// Token: 0x0400004F RID: 79
		private static SafeFileHandle inHandle;

		// Token: 0x04000051 RID: 81
		internal static bool IsInitialized;

		// Token: 0x04000052 RID: 82
		private const uint EnableVTProcessing = 4U;

		// Token: 0x04000053 RID: 83
		private const uint GenericWrite = 1073741824U;

		// Token: 0x04000054 RID: 84
		private const uint GenericRead = 2147483648U;

		// Token: 0x04000055 RID: 85
		private const uint FileShareRead = 1U;

		// Token: 0x04000056 RID: 86
		private const uint FileShareWrite = 2U;

		// Token: 0x04000057 RID: 87
		private const uint OpenExisting = 3U;

		// Token: 0x04000058 RID: 88
		private const uint FileAttributeNormal = 128U;

		// Token: 0x04000059 RID: 89
		private const uint ErrorAccessDenied = 5U;

		// Token: 0x0400005A RID: 90
		private const uint AttachParent = 4294967295U;
	}
}
