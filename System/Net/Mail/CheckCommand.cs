using System;
using System.Net.Mime;

namespace System.Net.Mail
{
	// Token: 0x0200027C RID: 636
	internal static class CheckCommand
	{
		// Token: 0x060017EE RID: 6126 RVA: 0x0007A370 File Offset: 0x00078570
		internal static IAsyncResult BeginSend(SmtpConnection conn, AsyncCallback callback, object state)
		{
			MultiAsyncResult multiAsyncResult = new MultiAsyncResult(conn, callback, state);
			multiAsyncResult.Enter();
			IAsyncResult asyncResult = conn.BeginFlush(CheckCommand.onWrite, multiAsyncResult);
			if (asyncResult.CompletedSynchronously)
			{
				conn.EndFlush(asyncResult);
				multiAsyncResult.Leave();
			}
			SmtpReplyReader nextReplyReader = conn.Reader.GetNextReplyReader();
			multiAsyncResult.Enter();
			IAsyncResult asyncResult2 = nextReplyReader.BeginReadLine(CheckCommand.onReadLine, multiAsyncResult);
			if (asyncResult2.CompletedSynchronously)
			{
				LineInfo lineInfo = nextReplyReader.EndReadLine(asyncResult2);
				if (!(multiAsyncResult.Result is Exception))
				{
					multiAsyncResult.Result = lineInfo;
				}
				multiAsyncResult.Leave();
			}
			multiAsyncResult.CompleteSequence();
			return multiAsyncResult;
		}

		// Token: 0x060017EF RID: 6127 RVA: 0x0007A408 File Offset: 0x00078608
		internal static object EndSend(IAsyncResult result, out string response)
		{
			object obj = MultiAsyncResult.End(result);
			if (obj is Exception)
			{
				throw (Exception)obj;
			}
			LineInfo lineInfo = (LineInfo)obj;
			response = lineInfo.Line;
			return lineInfo.StatusCode;
		}

		// Token: 0x060017F0 RID: 6128 RVA: 0x0007A448 File Offset: 0x00078648
		private static void OnReadLine(IAsyncResult result)
		{
			if (!result.CompletedSynchronously)
			{
				MultiAsyncResult multiAsyncResult = (MultiAsyncResult)result.AsyncState;
				try
				{
					SmtpConnection smtpConnection = (SmtpConnection)multiAsyncResult.Context;
					LineInfo lineInfo = smtpConnection.Reader.CurrentReader.EndReadLine(result);
					if (!(multiAsyncResult.Result is Exception))
					{
						multiAsyncResult.Result = lineInfo;
					}
					multiAsyncResult.Leave();
				}
				catch (Exception ex)
				{
					multiAsyncResult.Leave(ex);
				}
			}
		}

		// Token: 0x060017F1 RID: 6129 RVA: 0x0007A4C4 File Offset: 0x000786C4
		private static void OnWrite(IAsyncResult result)
		{
			if (!result.CompletedSynchronously)
			{
				MultiAsyncResult multiAsyncResult = (MultiAsyncResult)result.AsyncState;
				try
				{
					SmtpConnection smtpConnection = (SmtpConnection)multiAsyncResult.Context;
					smtpConnection.EndFlush(result);
					multiAsyncResult.Leave();
				}
				catch (Exception ex)
				{
					multiAsyncResult.Leave(ex);
				}
			}
		}

		// Token: 0x060017F2 RID: 6130 RVA: 0x0007A51C File Offset: 0x0007871C
		internal static SmtpStatusCode Send(SmtpConnection conn, out string response)
		{
			conn.Flush();
			SmtpReplyReader nextReplyReader = conn.Reader.GetNextReplyReader();
			LineInfo lineInfo = nextReplyReader.ReadLine();
			response = lineInfo.Line;
			nextReplyReader.Close();
			return lineInfo.StatusCode;
		}

		// Token: 0x04001817 RID: 6167
		private static AsyncCallback onReadLine = new AsyncCallback(CheckCommand.OnReadLine);

		// Token: 0x04001818 RID: 6168
		private static AsyncCallback onWrite = new AsyncCallback(CheckCommand.OnWrite);
	}
}
