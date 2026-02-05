using System;

namespace System.Net.Mail
{
	// Token: 0x0200027E RID: 638
	internal static class AuthCommand
	{
		// Token: 0x060017FA RID: 6138 RVA: 0x0007A750 File Offset: 0x00078950
		internal static IAsyncResult BeginSend(SmtpConnection conn, string type, string message, AsyncCallback callback, object state)
		{
			AuthCommand.PrepareCommand(conn, type, message);
			return ReadLinesCommand.BeginSend(conn, callback, state);
		}

		// Token: 0x060017FB RID: 6139 RVA: 0x0007A763 File Offset: 0x00078963
		internal static IAsyncResult BeginSend(SmtpConnection conn, string message, AsyncCallback callback, object state)
		{
			AuthCommand.PrepareCommand(conn, message);
			return ReadLinesCommand.BeginSend(conn, callback, state);
		}

		// Token: 0x060017FC RID: 6140 RVA: 0x0007A774 File Offset: 0x00078974
		private static LineInfo CheckResponse(LineInfo[] lines)
		{
			if (lines == null || lines.Length == 0)
			{
				throw new SmtpException(SR.GetString("SmtpAuthResponseInvalid"));
			}
			return lines[0];
		}

		// Token: 0x060017FD RID: 6141 RVA: 0x0007A794 File Offset: 0x00078994
		internal static LineInfo EndSend(IAsyncResult result)
		{
			return AuthCommand.CheckResponse(ReadLinesCommand.EndSend(result));
		}

		// Token: 0x060017FE RID: 6142 RVA: 0x0007A7A4 File Offset: 0x000789A4
		private static void PrepareCommand(SmtpConnection conn, string type, string message)
		{
			conn.BufferBuilder.Append(SmtpCommands.Auth);
			conn.BufferBuilder.Append(type);
			conn.BufferBuilder.Append(32);
			conn.BufferBuilder.Append(message);
			conn.BufferBuilder.Append(SmtpCommands.CRLF);
		}

		// Token: 0x060017FF RID: 6143 RVA: 0x0007A7F6 File Offset: 0x000789F6
		private static void PrepareCommand(SmtpConnection conn, string message)
		{
			conn.BufferBuilder.Append(message);
			conn.BufferBuilder.Append(SmtpCommands.CRLF);
		}

		// Token: 0x06001800 RID: 6144 RVA: 0x0007A814 File Offset: 0x00078A14
		internal static LineInfo Send(SmtpConnection conn, string type, string message)
		{
			AuthCommand.PrepareCommand(conn, type, message);
			return AuthCommand.CheckResponse(ReadLinesCommand.Send(conn));
		}

		// Token: 0x06001801 RID: 6145 RVA: 0x0007A829 File Offset: 0x00078A29
		internal static LineInfo Send(SmtpConnection conn, string message)
		{
			AuthCommand.PrepareCommand(conn, message);
			return AuthCommand.CheckResponse(ReadLinesCommand.Send(conn));
		}
	}
}
