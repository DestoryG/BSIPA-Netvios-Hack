using System;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.Win32;

namespace System.IO.Ports
{
	// Token: 0x02000409 RID: 1033
	internal static class InternalResources
	{
		// Token: 0x060026AD RID: 9901 RVA: 0x000B1E2D File Offset: 0x000B002D
		internal static void EndOfFile()
		{
			throw new EndOfStreamException(SR.GetString("IO_EOF_ReadBeyondEOF"));
		}

		// Token: 0x060026AE RID: 9902 RVA: 0x000B1E40 File Offset: 0x000B0040
		internal static string GetMessage(int errorCode)
		{
			StringBuilder stringBuilder = new StringBuilder(512);
			int num = SafeNativeMethods.FormatMessage(12800, IntPtr.Zero, (uint)errorCode, 0, stringBuilder, stringBuilder.Capacity, null);
			if (num != 0)
			{
				return stringBuilder.ToString();
			}
			return SR.GetString("IO_UnknownError", new object[] { errorCode });
		}

		// Token: 0x060026AF RID: 9903 RVA: 0x000B1E97 File Offset: 0x000B0097
		internal static void FileNotOpen()
		{
			throw new ObjectDisposedException(null, SR.GetString("Port_not_open"));
		}

		// Token: 0x060026B0 RID: 9904 RVA: 0x000B1EA9 File Offset: 0x000B00A9
		internal static void WrongAsyncResult()
		{
			throw new ArgumentException(SR.GetString("Arg_WrongAsyncResult"));
		}

		// Token: 0x060026B1 RID: 9905 RVA: 0x000B1EBA File Offset: 0x000B00BA
		internal static void EndReadCalledTwice()
		{
			throw new ArgumentException(SR.GetString("InvalidOperation_EndReadCalledMultiple"));
		}

		// Token: 0x060026B2 RID: 9906 RVA: 0x000B1ECB File Offset: 0x000B00CB
		internal static void EndWriteCalledTwice()
		{
			throw new ArgumentException(SR.GetString("InvalidOperation_EndWriteCalledMultiple"));
		}

		// Token: 0x060026B3 RID: 9907 RVA: 0x000B1EDC File Offset: 0x000B00DC
		internal static void WinIOError()
		{
			int lastWin32Error = Marshal.GetLastWin32Error();
			InternalResources.WinIOError(lastWin32Error, string.Empty);
		}

		// Token: 0x060026B4 RID: 9908 RVA: 0x000B1EFC File Offset: 0x000B00FC
		internal static void WinIOError(string str)
		{
			int lastWin32Error = Marshal.GetLastWin32Error();
			InternalResources.WinIOError(lastWin32Error, str);
		}

		// Token: 0x060026B5 RID: 9909 RVA: 0x000B1F18 File Offset: 0x000B0118
		internal static void WinIOError(int errorCode, string str)
		{
			if (errorCode <= 5)
			{
				if (errorCode - 2 > 1)
				{
					if (errorCode == 5)
					{
						if (str.Length == 0)
						{
							throw new UnauthorizedAccessException(SR.GetString("UnauthorizedAccess_IODenied_NoPathName"));
						}
						throw new UnauthorizedAccessException(SR.GetString("UnauthorizedAccess_IODenied_Path", new object[] { str }));
					}
				}
				else
				{
					if (str.Length == 0)
					{
						throw new IOException(SR.GetString("IO_PortNotFound"));
					}
					throw new IOException(SR.GetString("IO_PortNotFoundFileName", new object[] { str }));
				}
			}
			else if (errorCode != 32)
			{
				if (errorCode == 206)
				{
					throw new PathTooLongException(SR.GetString("IO_PathTooLong"));
				}
			}
			else
			{
				if (str.Length == 0)
				{
					throw new IOException(SR.GetString("IO_SharingViolation_NoFileName"));
				}
				throw new IOException(SR.GetString("IO_SharingViolation_File", new object[] { str }));
			}
			throw new IOException(InternalResources.GetMessage(errorCode), InternalResources.MakeHRFromErrorCode(errorCode));
		}

		// Token: 0x060026B6 RID: 9910 RVA: 0x000B2004 File Offset: 0x000B0204
		internal static int MakeHRFromErrorCode(int errorCode)
		{
			return -2147024896 | errorCode;
		}
	}
}
