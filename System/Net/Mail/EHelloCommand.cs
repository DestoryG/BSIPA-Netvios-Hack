using System;

namespace System.Net.Mail
{
	// Token: 0x02000281 RID: 641
	internal static class EHelloCommand
	{
		// Token: 0x0600180A RID: 6154 RVA: 0x0007A9A7 File Offset: 0x00078BA7
		internal static IAsyncResult BeginSend(SmtpConnection conn, string domain, AsyncCallback callback, object state)
		{
			EHelloCommand.PrepareCommand(conn, domain);
			return ReadLinesCommand.BeginSend(conn, callback, state);
		}

		// Token: 0x0600180B RID: 6155 RVA: 0x0007A9B8 File Offset: 0x00078BB8
		private static string[] CheckResponse(LineInfo[] lines)
		{
			if (lines == null || lines.Length == 0)
			{
				throw new SmtpException(SR.GetString("SmtpEhloResponseInvalid"));
			}
			if (lines[0].StatusCode == SmtpStatusCode.Ok)
			{
				string[] array = new string[lines.Length - 1];
				for (int i = 1; i < lines.Length; i++)
				{
					array[i - 1] = lines[i].Line;
				}
				return array;
			}
			if (lines[0].StatusCode < (SmtpStatusCode)400)
			{
				throw new SmtpException(SR.GetString("net_webstatus_ServerProtocolViolation"), lines[0].Line);
			}
			throw new SmtpException(lines[0].StatusCode, lines[0].Line, true);
		}

		// Token: 0x0600180C RID: 6156 RVA: 0x0007AA68 File Offset: 0x00078C68
		internal static string[] EndSend(IAsyncResult result)
		{
			return EHelloCommand.CheckResponse(ReadLinesCommand.EndSend(result));
		}

		// Token: 0x0600180D RID: 6157 RVA: 0x0007AA78 File Offset: 0x00078C78
		private static void PrepareCommand(SmtpConnection conn, string domain)
		{
			if (conn.IsStreamOpen)
			{
				throw new InvalidOperationException(SR.GetString("SmtpDataStreamOpen"));
			}
			conn.BufferBuilder.Append(SmtpCommands.EHello);
			conn.BufferBuilder.Append(domain);
			conn.BufferBuilder.Append(SmtpCommands.CRLF);
		}

		// Token: 0x0600180E RID: 6158 RVA: 0x0007AAC9 File Offset: 0x00078CC9
		internal static string[] Send(SmtpConnection conn, string domain)
		{
			EHelloCommand.PrepareCommand(conn, domain);
			return EHelloCommand.CheckResponse(ReadLinesCommand.Send(conn));
		}
	}
}
