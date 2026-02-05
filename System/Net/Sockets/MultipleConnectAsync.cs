using System;
using System.Threading;

namespace System.Net.Sockets
{
	// Token: 0x02000399 RID: 921
	internal abstract class MultipleConnectAsync
	{
		// Token: 0x0600225F RID: 8799 RVA: 0x000A3E68 File Offset: 0x000A2068
		public bool StartConnectAsync(SocketAsyncEventArgs args, DnsEndPoint endPoint)
		{
			object obj = this.lockObject;
			bool flag2;
			lock (obj)
			{
				this.userArgs = args;
				this.endPoint = endPoint;
				if (this.state == MultipleConnectAsync.State.Canceled)
				{
					this.SyncFail(new SocketException(SocketError.OperationAborted));
					flag2 = false;
				}
				else
				{
					this.state = MultipleConnectAsync.State.DnsQuery;
					IAsyncResult asyncResult = Dns.BeginGetHostAddresses(endPoint.Host, new AsyncCallback(this.DnsCallback), null);
					if (asyncResult.CompletedSynchronously)
					{
						flag2 = this.DoDnsCallback(asyncResult, true);
					}
					else
					{
						flag2 = true;
					}
				}
			}
			return flag2;
		}

		// Token: 0x06002260 RID: 8800 RVA: 0x000A3F04 File Offset: 0x000A2104
		private void DnsCallback(IAsyncResult result)
		{
			if (!result.CompletedSynchronously)
			{
				this.DoDnsCallback(result, false);
			}
		}

		// Token: 0x06002261 RID: 8801 RVA: 0x000A3F18 File Offset: 0x000A2118
		private bool DoDnsCallback(IAsyncResult result, bool sync)
		{
			Exception ex = null;
			object obj = this.lockObject;
			lock (obj)
			{
				if (this.state == MultipleConnectAsync.State.Canceled)
				{
					return true;
				}
				try
				{
					this.addressList = Dns.EndGetHostAddresses(result);
				}
				catch (Exception ex2)
				{
					this.state = MultipleConnectAsync.State.Completed;
					ex = ex2;
				}
				if (ex == null)
				{
					this.state = MultipleConnectAsync.State.ConnectAttempt;
					this.internalArgs = new SocketAsyncEventArgs();
					this.internalArgs.Completed += this.InternalConnectCallback;
					this.internalArgs.SetBuffer(this.userArgs.Buffer, this.userArgs.Offset, this.userArgs.Count);
					ex = this.AttemptConnection();
					if (ex != null)
					{
						this.state = MultipleConnectAsync.State.Completed;
					}
				}
			}
			return ex == null || this.Fail(sync, ex);
		}

		// Token: 0x06002262 RID: 8802 RVA: 0x000A4008 File Offset: 0x000A2208
		private void InternalConnectCallback(object sender, SocketAsyncEventArgs args)
		{
			Exception ex = null;
			object obj = this.lockObject;
			lock (obj)
			{
				if (this.state == MultipleConnectAsync.State.Canceled)
				{
					ex = new SocketException(SocketError.OperationAborted);
				}
				else if (args.SocketError == SocketError.Success)
				{
					this.state = MultipleConnectAsync.State.Completed;
				}
				else if (args.SocketError == SocketError.OperationAborted)
				{
					ex = new SocketException(SocketError.OperationAborted);
					this.state = MultipleConnectAsync.State.Canceled;
				}
				else
				{
					SocketError socketError = args.SocketError;
					Exception ex2 = this.AttemptConnection();
					if (ex2 == null)
					{
						return;
					}
					SocketException ex3 = ex2 as SocketException;
					if (ex3 != null && ex3.SocketErrorCode == SocketError.NoData)
					{
						ex = new SocketException(socketError);
					}
					else
					{
						ex = ex2;
					}
					this.state = MultipleConnectAsync.State.Completed;
				}
			}
			if (ex == null)
			{
				this.Succeed();
				return;
			}
			this.AsyncFail(ex);
		}

		// Token: 0x06002263 RID: 8803 RVA: 0x000A40E4 File Offset: 0x000A22E4
		private Exception AttemptConnection()
		{
			try
			{
				Socket socket = null;
				IPAddress ipaddress = this.GetNextAddress(out socket);
				if (ipaddress == null)
				{
					return new SocketException(SocketError.NoData);
				}
				this.internalArgs.RemoteEndPoint = new IPEndPoint(ipaddress, this.endPoint.Port);
				if (!socket.ConnectAsync(this.internalArgs))
				{
					return new SocketException(this.internalArgs.SocketError);
				}
			}
			catch (ObjectDisposedException)
			{
				return new SocketException(SocketError.OperationAborted);
			}
			catch (Exception ex)
			{
				return ex;
			}
			return null;
		}

		// Token: 0x06002264 RID: 8804
		protected abstract void OnSucceed();

		// Token: 0x06002265 RID: 8805 RVA: 0x000A4180 File Offset: 0x000A2380
		protected void Succeed()
		{
			this.OnSucceed();
			this.userArgs.FinishWrapperConnectSuccess(this.internalArgs.ConnectSocket, this.internalArgs.BytesTransferred, this.internalArgs.SocketFlags);
			this.internalArgs.Dispose();
		}

		// Token: 0x06002266 RID: 8806
		protected abstract void OnFail(bool abortive);

		// Token: 0x06002267 RID: 8807 RVA: 0x000A41BF File Offset: 0x000A23BF
		private bool Fail(bool sync, Exception e)
		{
			if (sync)
			{
				this.SyncFail(e);
				return false;
			}
			this.AsyncFail(e);
			return true;
		}

		// Token: 0x06002268 RID: 8808 RVA: 0x000A41D8 File Offset: 0x000A23D8
		private void SyncFail(Exception e)
		{
			this.OnFail(false);
			if (this.internalArgs != null)
			{
				this.internalArgs.Dispose();
			}
			SocketException ex = e as SocketException;
			if (ex != null)
			{
				this.userArgs.FinishConnectByNameSyncFailure(ex, 0, SocketFlags.None);
				return;
			}
			throw e;
		}

		// Token: 0x06002269 RID: 8809 RVA: 0x000A4219 File Offset: 0x000A2419
		private void AsyncFail(Exception e)
		{
			this.OnFail(false);
			if (this.internalArgs != null)
			{
				this.internalArgs.Dispose();
			}
			this.userArgs.FinishOperationAsyncFailure(e, 0, SocketFlags.None);
		}

		// Token: 0x0600226A RID: 8810 RVA: 0x000A4244 File Offset: 0x000A2444
		public void Cancel()
		{
			bool flag = false;
			object obj = this.lockObject;
			lock (obj)
			{
				switch (this.state)
				{
				case MultipleConnectAsync.State.NotStarted:
					flag = true;
					break;
				case MultipleConnectAsync.State.DnsQuery:
					ThreadPool.QueueUserWorkItem(new WaitCallback(this.CallAsyncFail));
					flag = true;
					break;
				case MultipleConnectAsync.State.ConnectAttempt:
					flag = true;
					break;
				}
				this.state = MultipleConnectAsync.State.Canceled;
			}
			if (flag)
			{
				this.OnFail(true);
			}
		}

		// Token: 0x0600226B RID: 8811 RVA: 0x000A42CC File Offset: 0x000A24CC
		private void CallAsyncFail(object ignored)
		{
			this.AsyncFail(new SocketException(SocketError.OperationAborted));
		}

		// Token: 0x0600226C RID: 8812
		protected abstract IPAddress GetNextAddress(out Socket attemptSocket);

		// Token: 0x04001F6E RID: 8046
		protected SocketAsyncEventArgs userArgs;

		// Token: 0x04001F6F RID: 8047
		protected SocketAsyncEventArgs internalArgs;

		// Token: 0x04001F70 RID: 8048
		protected DnsEndPoint endPoint;

		// Token: 0x04001F71 RID: 8049
		protected IPAddress[] addressList;

		// Token: 0x04001F72 RID: 8050
		protected int nextAddress;

		// Token: 0x04001F73 RID: 8051
		private MultipleConnectAsync.State state;

		// Token: 0x04001F74 RID: 8052
		private object lockObject = new object();

		// Token: 0x020007E1 RID: 2017
		private enum State
		{
			// Token: 0x040034D3 RID: 13523
			NotStarted,
			// Token: 0x040034D4 RID: 13524
			DnsQuery,
			// Token: 0x040034D5 RID: 13525
			ConnectAttempt,
			// Token: 0x040034D6 RID: 13526
			Completed,
			// Token: 0x040034D7 RID: 13527
			Canceled
		}
	}
}
