using System;
using System.Security;
using Microsoft.Win32;

namespace System.Diagnostics
{
	// Token: 0x02000491 RID: 1169
	internal static class AssertWrapper
	{
		// Token: 0x06002B4D RID: 11085 RVA: 0x000C4B2B File Offset: 0x000C2D2B
		public static void ShowAssert(string stackTrace, StackFrame frame, string message, string detailMessage)
		{
			AssertWrapper.ShowMessageBoxAssert(stackTrace, message, detailMessage);
		}

		// Token: 0x06002B4E RID: 11086 RVA: 0x000C4B38 File Offset: 0x000C2D38
		[SecuritySafeCritical]
		private static void ShowMessageBoxAssert(string stackTrace, string message, string detailMessage)
		{
			string text = string.Concat(new string[]
			{
				message,
				Environment.NewLine,
				detailMessage,
				Environment.NewLine,
				stackTrace
			});
			text = AssertWrapper.TruncateMessageToFitScreen(text);
			int num = 262674;
			if (!Environment.UserInteractive)
			{
				num |= 2097152;
			}
			if (AssertWrapper.IsRTLResources)
			{
				num = num | 524288 | 1048576;
			}
			int num2;
			if (!Microsoft.Win32.UnsafeNativeMethods.IsPackagedProcess.Value)
			{
				num2 = SafeNativeMethods.MessageBox(IntPtr.Zero, text, SR.GetString("DebugAssertTitle"), num);
			}
			else
			{
				num2 = new MessageBoxPopup(text, SR.GetString("DebugAssertTitle"), num).ShowMessageBox();
			}
			if (num2 == 3)
			{
				Environment.Exit(1);
				return;
			}
			if (num2 != 4)
			{
				return;
			}
			if (!Debugger.IsAttached)
			{
				Debugger.Launch();
			}
			Debugger.Break();
		}

		// Token: 0x17000A78 RID: 2680
		// (get) Token: 0x06002B4F RID: 11087 RVA: 0x000C4BFE File Offset: 0x000C2DFE
		private static bool IsRTLResources
		{
			get
			{
				return SR.GetString("RTL") != "RTL_False";
			}
		}

		// Token: 0x06002B50 RID: 11088 RVA: 0x000C4C14 File Offset: 0x000C2E14
		[SecuritySafeCritical]
		private static string TruncateMessageToFitScreen(string message)
		{
			IntPtr intPtr = SafeNativeMethods.GetStockObject(17);
			IntPtr intPtr2 = Microsoft.Win32.UnsafeNativeMethods.GetDC(IntPtr.Zero);
			NativeMethods.TEXTMETRIC textmetric = new NativeMethods.TEXTMETRIC();
			intPtr = Microsoft.Win32.UnsafeNativeMethods.SelectObject(intPtr2, intPtr);
			SafeNativeMethods.GetTextMetrics(intPtr2, textmetric);
			Microsoft.Win32.UnsafeNativeMethods.SelectObject(intPtr2, intPtr);
			Microsoft.Win32.UnsafeNativeMethods.ReleaseDC(IntPtr.Zero, intPtr2);
			intPtr2 = IntPtr.Zero;
			int systemMetrics = Microsoft.Win32.UnsafeNativeMethods.GetSystemMetrics(1);
			int num = systemMetrics / textmetric.tmHeight - 15;
			int num2 = 0;
			int num3 = 0;
			int num4 = 0;
			while (num2 < num && num4 < message.Length - 1)
			{
				char c = message[num4];
				num3++;
				if (c == '\n' || c == '\r' || num3 > 80)
				{
					num2++;
					num3 = 0;
				}
				if (c == '\r' && message[num4 + 1] == '\n')
				{
					num4 += 2;
				}
				else if (c == '\n' && message[num4 + 1] == '\r')
				{
					num4 += 2;
				}
				else
				{
					num4++;
				}
			}
			if (num4 < message.Length - 1)
			{
				message = SR.GetString("DebugMessageTruncated", new object[] { message.Substring(0, num4) });
			}
			return message;
		}
	}
}
