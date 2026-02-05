using System;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Security.Principal;
using System.Text;
using Microsoft.Win32;
using Microsoft.Win32.SafeHandles;

namespace System.CodeDom.Compiler
{
	// Token: 0x0200067A RID: 1658
	[PermissionSet(SecurityAction.LinkDemand, Unrestricted = true)]
	public static class Executor
	{
		// Token: 0x06003D1D RID: 15645 RVA: 0x000FB500 File Offset: 0x000F9700
		internal static string GetRuntimeInstallDirectory()
		{
			return RuntimeEnvironment.GetRuntimeDirectory();
		}

		// Token: 0x06003D1E RID: 15646 RVA: 0x000FB507 File Offset: 0x000F9707
		private static FileStream CreateInheritedFile(string file)
		{
			return new FileStream(file, FileMode.CreateNew, FileAccess.Write, FileShare.Read | FileShare.Inheritable);
		}

		// Token: 0x06003D1F RID: 15647 RVA: 0x000FB514 File Offset: 0x000F9714
		public static void ExecWait(string cmd, TempFileCollection tempFiles)
		{
			string text = null;
			string text2 = null;
			Executor.ExecWaitWithCapture(cmd, tempFiles, ref text, ref text2);
		}

		// Token: 0x06003D20 RID: 15648 RVA: 0x000FB531 File Offset: 0x000F9731
		public static int ExecWaitWithCapture(string cmd, TempFileCollection tempFiles, ref string outputName, ref string errorName)
		{
			return Executor.ExecWaitWithCapture(null, cmd, Environment.CurrentDirectory, tempFiles, ref outputName, ref errorName, null);
		}

		// Token: 0x06003D21 RID: 15649 RVA: 0x000FB543 File Offset: 0x000F9743
		public static int ExecWaitWithCapture(string cmd, string currentDir, TempFileCollection tempFiles, ref string outputName, ref string errorName)
		{
			return Executor.ExecWaitWithCapture(null, cmd, currentDir, tempFiles, ref outputName, ref errorName, null);
		}

		// Token: 0x06003D22 RID: 15650 RVA: 0x000FB552 File Offset: 0x000F9752
		public static int ExecWaitWithCapture(IntPtr userToken, string cmd, TempFileCollection tempFiles, ref string outputName, ref string errorName)
		{
			return Executor.ExecWaitWithCapture(new SafeUserTokenHandle(userToken, false), cmd, Environment.CurrentDirectory, tempFiles, ref outputName, ref errorName, null);
		}

		// Token: 0x06003D23 RID: 15651 RVA: 0x000FB56B File Offset: 0x000F976B
		public static int ExecWaitWithCapture(IntPtr userToken, string cmd, string currentDir, TempFileCollection tempFiles, ref string outputName, ref string errorName)
		{
			return Executor.ExecWaitWithCapture(new SafeUserTokenHandle(userToken, false), cmd, Environment.CurrentDirectory, tempFiles, ref outputName, ref errorName, null);
		}

		// Token: 0x06003D24 RID: 15652 RVA: 0x000FB588 File Offset: 0x000F9788
		internal static int ExecWaitWithCapture(SafeUserTokenHandle userToken, string cmd, string currentDir, TempFileCollection tempFiles, ref string outputName, ref string errorName, string trueCmdLine)
		{
			int num = 0;
			try
			{
				WindowsImpersonationContext windowsImpersonationContext = Executor.RevertImpersonation();
				try
				{
					num = Executor.ExecWaitWithCaptureUnimpersonated(userToken, cmd, currentDir, tempFiles, ref outputName, ref errorName, trueCmdLine);
				}
				finally
				{
					Executor.ReImpersonate(windowsImpersonationContext);
				}
			}
			catch
			{
				throw;
			}
			return num;
		}

		// Token: 0x06003D25 RID: 15653 RVA: 0x000FB5D8 File Offset: 0x000F97D8
		private unsafe static int ExecWaitWithCaptureUnimpersonated(SafeUserTokenHandle userToken, string cmd, string currentDir, TempFileCollection tempFiles, ref string outputName, ref string errorName, string trueCmdLine)
		{
			IntSecurity.UnmanagedCode.Demand();
			if (outputName == null || outputName.Length == 0)
			{
				outputName = tempFiles.AddExtension("out");
			}
			if (errorName == null || errorName.Length == 0)
			{
				errorName = tempFiles.AddExtension("err");
			}
			FileStream fileStream = Executor.CreateInheritedFile(outputName);
			FileStream fileStream2 = Executor.CreateInheritedFile(errorName);
			bool flag = false;
			SafeNativeMethods.PROCESS_INFORMATION process_INFORMATION = new SafeNativeMethods.PROCESS_INFORMATION();
			Microsoft.Win32.SafeHandles.SafeProcessHandle safeProcessHandle = new Microsoft.Win32.SafeHandles.SafeProcessHandle();
			Microsoft.Win32.SafeHandles.SafeThreadHandle safeThreadHandle = new Microsoft.Win32.SafeHandles.SafeThreadHandle();
			SafeUserTokenHandle safeUserTokenHandle = null;
			try
			{
				StreamWriter streamWriter = new StreamWriter(fileStream, Encoding.UTF8);
				streamWriter.Write(currentDir);
				streamWriter.Write("> ");
				streamWriter.WriteLine((trueCmdLine != null) ? trueCmdLine : cmd);
				streamWriter.WriteLine();
				streamWriter.WriteLine();
				streamWriter.Flush();
				Microsoft.Win32.NativeMethods.STARTUPINFO startupinfo = new Microsoft.Win32.NativeMethods.STARTUPINFO();
				startupinfo.cb = Marshal.SizeOf(startupinfo);
				startupinfo.dwFlags = 257;
				startupinfo.wShowWindow = 0;
				startupinfo.hStdOutput = fileStream.SafeFileHandle;
				startupinfo.hStdError = fileStream2.SafeFileHandle;
				startupinfo.hStdInput = new SafeFileHandle(Microsoft.Win32.UnsafeNativeMethods.GetStdHandle(-10), false);
				StringDictionary stringDictionary = new StringDictionary();
				foreach (object obj in Environment.GetEnvironmentVariables())
				{
					DictionaryEntry dictionaryEntry = (DictionaryEntry)obj;
					stringDictionary[(string)dictionaryEntry.Key] = (string)dictionaryEntry.Value;
				}
				stringDictionary["_ClrRestrictSecAttributes"] = "1";
				byte[] array = EnvironmentBlock.ToByteArray(stringDictionary, false);
				try
				{
					byte[] array2;
					byte* ptr;
					if ((array2 = array) == null || array2.Length == 0)
					{
						ptr = null;
					}
					else
					{
						ptr = &array2[0];
					}
					IntPtr intPtr = new IntPtr((void*)ptr);
					if (userToken == null || userToken.IsInvalid)
					{
						RuntimeHelpers.PrepareConstrainedRegions();
						try
						{
							goto IL_0322;
						}
						finally
						{
							flag = Microsoft.Win32.NativeMethods.CreateProcess(null, new StringBuilder(cmd), null, null, true, 0, intPtr, currentDir, startupinfo, process_INFORMATION);
							if (process_INFORMATION.hProcess != (IntPtr)0 && process_INFORMATION.hProcess != Microsoft.Win32.NativeMethods.INVALID_HANDLE_VALUE)
							{
								safeProcessHandle.InitialSetHandle(process_INFORMATION.hProcess);
							}
							if (process_INFORMATION.hThread != (IntPtr)0 && process_INFORMATION.hThread != Microsoft.Win32.NativeMethods.INVALID_HANDLE_VALUE)
							{
								safeThreadHandle.InitialSetHandle(process_INFORMATION.hThread);
							}
						}
					}
					flag = SafeUserTokenHandle.DuplicateTokenEx(userToken, 983551, null, 2, 1, out safeUserTokenHandle);
					if (flag)
					{
						RuntimeHelpers.PrepareConstrainedRegions();
						try
						{
						}
						finally
						{
							flag = Microsoft.Win32.NativeMethods.CreateProcessAsUser(safeUserTokenHandle, null, cmd, null, null, true, 0, new HandleRef(null, intPtr), currentDir, startupinfo, process_INFORMATION);
							if (process_INFORMATION.hProcess != (IntPtr)0 && process_INFORMATION.hProcess != Microsoft.Win32.NativeMethods.INVALID_HANDLE_VALUE)
							{
								safeProcessHandle.InitialSetHandle(process_INFORMATION.hProcess);
							}
							if (process_INFORMATION.hThread != (IntPtr)0 && process_INFORMATION.hThread != Microsoft.Win32.NativeMethods.INVALID_HANDLE_VALUE)
							{
								safeThreadHandle.InitialSetHandle(process_INFORMATION.hThread);
							}
						}
					}
				}
				finally
				{
					byte[] array2 = null;
				}
			}
			finally
			{
				if (!flag && safeUserTokenHandle != null && !safeUserTokenHandle.IsInvalid)
				{
					safeUserTokenHandle.Close();
					safeUserTokenHandle = null;
				}
				fileStream.Close();
				fileStream2.Close();
			}
			IL_0322:
			if (flag)
			{
				try
				{
					ProcessWaitHandle processWaitHandle = null;
					bool flag2;
					try
					{
						processWaitHandle = new ProcessWaitHandle(safeProcessHandle);
						flag2 = processWaitHandle.WaitOne(600000, false);
					}
					finally
					{
						if (processWaitHandle != null)
						{
							processWaitHandle.Close();
						}
					}
					if (!flag2)
					{
						throw new ExternalException(SR.GetString("ExecTimeout", new object[] { cmd }), 258);
					}
					int num = 259;
					if (!Microsoft.Win32.NativeMethods.GetExitCodeProcess(safeProcessHandle, out num))
					{
						throw new ExternalException(SR.GetString("ExecCantGetRetCode", new object[] { cmd }), Marshal.GetLastWin32Error());
					}
					return num;
				}
				finally
				{
					safeProcessHandle.Close();
					safeThreadHandle.Close();
					if (safeUserTokenHandle != null && !safeUserTokenHandle.IsInvalid)
					{
						safeUserTokenHandle.Close();
					}
				}
			}
			int lastWin32Error = Marshal.GetLastWin32Error();
			if (lastWin32Error == 8)
			{
				throw new OutOfMemoryException();
			}
			Win32Exception ex = new Win32Exception(lastWin32Error);
			ExternalException ex2 = new ExternalException(SR.GetString("ExecCantExec", new object[] { cmd }), ex);
			throw ex2;
		}

		// Token: 0x06003D26 RID: 15654 RVA: 0x000FBA9C File Offset: 0x000F9C9C
		[PermissionSet(SecurityAction.LinkDemand, Unrestricted = true)]
		[SecurityPermission(SecurityAction.Assert, ControlPrincipal = true, UnmanagedCode = true)]
		internal static WindowsImpersonationContext RevertImpersonation()
		{
			return WindowsIdentity.Impersonate(new IntPtr(0));
		}

		// Token: 0x06003D27 RID: 15655 RVA: 0x000FBAA9 File Offset: 0x000F9CA9
		internal static void ReImpersonate(WindowsImpersonationContext impersonation)
		{
			impersonation.Undo();
		}

		// Token: 0x04002C8F RID: 11407
		private const int ProcessTimeOut = 600000;
	}
}
