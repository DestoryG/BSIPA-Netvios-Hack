using System;
using System.Net.Mime;

namespace System.Net.Mail
{
	// Token: 0x0200027D RID: 637
	internal static class ReadLinesCommand
	{
		// Token: 0x060017F4 RID: 6132 RVA: 0x0007A57C File Offset: 0x0007877C
		internal static IAsyncResult BeginSend(SmtpConnection conn, AsyncCallback callback, object state)
		{
			MultiAsyncResult multiAsyncResult = new MultiAsyncResult(conn, callback, state);
			multiAsyncResult.Enter();
			IAsyncResult asyncResult = conn.BeginFlush(ReadLinesCommand.onWrite, multiAsyncResult);
			if (asyncResult.CompletedSynchronously)
			{
				conn.EndFlush(asyncResult);
				multiAsyncResult.Leave();
			}
			SmtpReplyReader nextReplyReader = conn.Reader.GetNextReplyReader();
			multiAsyncResult.Enter();
			IAsyncResult asyncResult2 = nextReplyReader.BeginReadLines(ReadLinesCommand.onReadLines, multiAsyncResult);
			if (asyncResult2.CompletedSynchronously)
			{
				LineInfo[] array = conn.Reader.CurrentReader.EndReadLines(asyncResult2);
				if (!(multiAsyncResult.Result is Exception))
				{
					multiAsyncResult.Result = array;
				}
				multiAsyncResult.Leave();
			}
			multiAsyncResult.CompleteSequence();
			return multiAsyncResult;
		}

		// Token: 0x060017F5 RID: 6133 RVA: 0x0007A618 File Offset: 0x00078818
		internal static LineInfo[] EndSend(IAsyncResult result)
		{
			object obj = MultiAsyncResult.End(result);
			if (obj is Exception)
			{
				throw (Exception)obj;
			}
			return (LineInfo[])obj;
		}

		// Token: 0x060017F6 RID: 6134 RVA: 0x0007A644 File Offset: 0x00078844
		private static void OnReadLines(IAsyncResult result)
		{
			if (!result.CompletedSynchronously)
			{
				MultiAsyncResult multiAsyncResult = (MultiAsyncResult)result.AsyncState;
				try
				{
					SmtpConnection smtpConnection = (SmtpConnection)multiAsyncResult.Context;
					LineInfo[] array = smtpConnection.Reader.CurrentReader.EndReadLines(result);
					if (!(multiAsyncResult.Result is Exception))
					{
						multiAsyncResult.Result = array;
					}
					multiAsyncResult.Leave();
				}
				catch (Exception ex)
				{
					multiAsyncResult.Leave(ex);
				}
			}
		}

		// Token: 0x060017F7 RID: 6135 RVA: 0x0007A6BC File Offset: 0x000788BC
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

		// Token: 0x060017F8 RID: 6136 RVA: 0x0007A714 File Offset: 0x00078914
		internal static LineInfo[] Send(SmtpConnection conn)
		{
			conn.Flush();
			return conn.Reader.GetNextReplyReader().ReadLines();
		}

		// Token: 0x04001819 RID: 6169
		private static AsyncCallback onReadLines = new AsyncCallback(ReadLinesCommand.OnReadLines);

		// Token: 0x0400181A RID: 6170
		private static AsyncCallback onWrite = new AsyncCallback(ReadLinesCommand.OnWrite);
	}
}
