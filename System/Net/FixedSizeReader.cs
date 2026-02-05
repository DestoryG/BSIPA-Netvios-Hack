using System;
using System.IO;

namespace System.Net
{
	// Token: 0x0200021F RID: 543
	internal class FixedSizeReader
	{
		// Token: 0x06001402 RID: 5122 RVA: 0x0006A571 File Offset: 0x00068771
		public FixedSizeReader(Stream transport)
		{
			this._Transport = transport;
		}

		// Token: 0x06001403 RID: 5123 RVA: 0x0006A580 File Offset: 0x00068780
		public int ReadPacket(byte[] buffer, int offset, int count)
		{
			int num = count;
			for (;;)
			{
				int num2 = this._Transport.Read(buffer, offset, num);
				if (num2 == 0)
				{
					break;
				}
				num -= num2;
				offset += num2;
				if (num == 0)
				{
					return count;
				}
			}
			if (num != count)
			{
				throw new IOException(SR.GetString("net_io_eof"));
			}
			return 0;
		}

		// Token: 0x06001404 RID: 5124 RVA: 0x0006A5C4 File Offset: 0x000687C4
		public void AsyncReadPacket(AsyncProtocolRequest request)
		{
			this._Request = request;
			this._TotalRead = 0;
			this.StartReading();
		}

		// Token: 0x06001405 RID: 5125 RVA: 0x0006A5DC File Offset: 0x000687DC
		private void StartReading()
		{
			int num;
			do
			{
				IAsyncResult asyncResult = this._Transport.BeginRead(this._Request.Buffer, this._Request.Offset + this._TotalRead, this._Request.Count - this._TotalRead, FixedSizeReader._ReadCallback, this);
				if (!asyncResult.CompletedSynchronously)
				{
					break;
				}
				num = this._Transport.EndRead(asyncResult);
			}
			while (!this.CheckCompletionBeforeNextRead(num));
		}

		// Token: 0x06001406 RID: 5126 RVA: 0x0006A648 File Offset: 0x00068848
		private bool CheckCompletionBeforeNextRead(int bytes)
		{
			if (bytes == 0)
			{
				if (this._TotalRead == 0)
				{
					this._Request.CompleteRequest(0);
					return true;
				}
				throw new IOException(SR.GetString("net_io_eof"));
			}
			else
			{
				if ((this._TotalRead += bytes) == this._Request.Count)
				{
					this._Request.CompleteRequest(this._Request.Count);
					return true;
				}
				return false;
			}
		}

		// Token: 0x06001407 RID: 5127 RVA: 0x0006A6B8 File Offset: 0x000688B8
		private static void ReadCallback(IAsyncResult transportResult)
		{
			if (transportResult.CompletedSynchronously)
			{
				return;
			}
			FixedSizeReader fixedSizeReader = (FixedSizeReader)transportResult.AsyncState;
			AsyncProtocolRequest request = fixedSizeReader._Request;
			try
			{
				int num = fixedSizeReader._Transport.EndRead(transportResult);
				if (!fixedSizeReader.CheckCompletionBeforeNextRead(num))
				{
					fixedSizeReader.StartReading();
				}
			}
			catch (Exception ex)
			{
				if (request.IsUserCompleted)
				{
					throw;
				}
				request.CompleteWithError(ex);
			}
		}

		// Token: 0x040015FE RID: 5630
		private static readonly AsyncCallback _ReadCallback = new AsyncCallback(FixedSizeReader.ReadCallback);

		// Token: 0x040015FF RID: 5631
		private readonly Stream _Transport;

		// Token: 0x04001600 RID: 5632
		private AsyncProtocolRequest _Request;

		// Token: 0x04001601 RID: 5633
		private int _TotalRead;
	}
}
