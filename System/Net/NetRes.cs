using System;
using System.Globalization;

namespace System.Net
{
	// Token: 0x020001CB RID: 459
	internal class NetRes
	{
		// Token: 0x0600122A RID: 4650 RVA: 0x00060DEE File Offset: 0x0005EFEE
		private NetRes()
		{
		}

		// Token: 0x0600122B RID: 4651 RVA: 0x00060DF8 File Offset: 0x0005EFF8
		public static string GetWebStatusString(string Res, WebExceptionStatus Status)
		{
			string @string = SR.GetString(WebExceptionMapping.GetWebStatusString(Status));
			string string2 = SR.GetString(Res);
			return string.Format(CultureInfo.CurrentCulture, string2, new object[] { @string });
		}

		// Token: 0x0600122C RID: 4652 RVA: 0x00060E2D File Offset: 0x0005F02D
		public static string GetWebStatusString(WebExceptionStatus Status)
		{
			return SR.GetString(WebExceptionMapping.GetWebStatusString(Status));
		}

		// Token: 0x0600122D RID: 4653 RVA: 0x00060E3C File Offset: 0x0005F03C
		public static string GetWebStatusCodeString(HttpStatusCode statusCode, string statusDescription)
		{
			string text = "(";
			int num = (int)statusCode;
			string text2 = text + num.ToString(NumberFormatInfo.InvariantInfo) + ")";
			string text3 = null;
			try
			{
				text3 = SR.GetString("net_httpstatuscode_" + statusCode.ToString(), null);
			}
			catch
			{
			}
			if (text3 != null && text3.Length > 0)
			{
				text2 = text2 + " " + text3;
			}
			else if (statusDescription != null && statusDescription.Length > 0)
			{
				text2 = text2 + " " + statusDescription;
			}
			return text2;
		}

		// Token: 0x0600122E RID: 4654 RVA: 0x00060ED4 File Offset: 0x0005F0D4
		public static string GetWebStatusCodeString(FtpStatusCode statusCode, string statusDescription)
		{
			string text = "(";
			int num = (int)statusCode;
			string text2 = text + num.ToString(NumberFormatInfo.InvariantInfo) + ")";
			string text3 = null;
			try
			{
				text3 = SR.GetString("net_ftpstatuscode_" + statusCode.ToString(), null);
			}
			catch
			{
			}
			if (text3 != null && text3.Length > 0)
			{
				text2 = text2 + " " + text3;
			}
			else if (statusDescription != null && statusDescription.Length > 0)
			{
				text2 = text2 + " " + statusDescription;
			}
			return text2;
		}
	}
}
