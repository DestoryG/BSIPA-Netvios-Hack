using System;
using System.Threading;

namespace System.Net
{
	// Token: 0x02000222 RID: 546
	internal class AsyncProtocolRequest
	{
		// Token: 0x06001410 RID: 5136 RVA: 0x0006A792 File Offset: 0x00068992
		public AsyncProtocolRequest(LazyAsyncResult userAsyncResult)
		{
			this.UserAsyncResult = userAsyncResult;
		}

		// Token: 0x06001411 RID: 5137 RVA: 0x0006A7A1 File Offset: 0x000689A1
		public void SetNextRequest(byte[] buffer, int offset, int count, AsyncProtocolCallback callback)
		{
			if (this._CompletionStatus != 0)
			{
				throw new InternalException();
			}
			this.Buffer = buffer;
			this.Offset = offset;
			this.Count = count;
			this._Callback = callback;
		}

		// Token: 0x17000432 RID: 1074
		// (get) Token: 0x06001412 RID: 5138 RVA: 0x0006A7CE File Offset: 0x000689CE
		internal object AsyncObject
		{
			get
			{
				return this.UserAsyncResult.AsyncObject;
			}
		}

		// Token: 0x06001413 RID: 5139 RVA: 0x0006A7DC File Offset: 0x000689DC
		internal void CompleteRequest(int result)
		{
			this.Result = result;
			int num = Interlocked.Exchange(ref this._CompletionStatus, 1);
			if (num == 1)
			{
				throw new InternalException();
			}
			if (num == 2)
			{
				this._CompletionStatus = 0;
				this._Callback(this);
			}
		}

		// Token: 0x17000433 RID: 1075
		// (get) Token: 0x06001414 RID: 5140 RVA: 0x0006A820 File Offset: 0x00068A20
		public bool MustCompleteSynchronously
		{
			get
			{
				int num = Interlocked.Exchange(ref this._CompletionStatus, 2);
				if (num == 2)
				{
					throw new InternalException();
				}
				if (num == 1)
				{
					this._CompletionStatus = 0;
					return true;
				}
				return false;
			}
		}

		// Token: 0x06001415 RID: 5141 RVA: 0x0006A852 File Offset: 0x00068A52
		internal void CompleteWithError(Exception e)
		{
			this.UserAsyncResult.InvokeCallback(e);
		}

		// Token: 0x06001416 RID: 5142 RVA: 0x0006A860 File Offset: 0x00068A60
		internal void CompleteUser()
		{
			this.UserAsyncResult.InvokeCallback();
		}

		// Token: 0x06001417 RID: 5143 RVA: 0x0006A86D File Offset: 0x00068A6D
		internal void CompleteUser(object userResult)
		{
			this.UserAsyncResult.InvokeCallback(userResult);
		}

		// Token: 0x17000434 RID: 1076
		// (get) Token: 0x06001418 RID: 5144 RVA: 0x0006A87B File Offset: 0x00068A7B
		internal bool IsUserCompleted
		{
			get
			{
				return this.UserAsyncResult.InternalPeekCompleted;
			}
		}

		// Token: 0x04001607 RID: 5639
		private AsyncProtocolCallback _Callback;

		// Token: 0x04001608 RID: 5640
		private int _CompletionStatus;

		// Token: 0x04001609 RID: 5641
		private const int StatusNotStarted = 0;

		// Token: 0x0400160A RID: 5642
		private const int StatusCompleted = 1;

		// Token: 0x0400160B RID: 5643
		private const int StatusCheckedOnSyncCompletion = 2;

		// Token: 0x0400160C RID: 5644
		public LazyAsyncResult UserAsyncResult;

		// Token: 0x0400160D RID: 5645
		public int Result;

		// Token: 0x0400160E RID: 5646
		public object AsyncState;

		// Token: 0x0400160F RID: 5647
		public byte[] Buffer;

		// Token: 0x04001610 RID: 5648
		public int Offset;

		// Token: 0x04001611 RID: 5649
		public int Count;
	}
}
