using System;
using System.IO;
using System.Threading;

namespace System.Net.Cache
{
	// Token: 0x0200031B RID: 795
	internal class CombinedReadStream : BaseWrapperStream, ICloseEx
	{
		// Token: 0x06001C4C RID: 7244 RVA: 0x00086838 File Offset: 0x00084A38
		internal CombinedReadStream(Stream headStream, Stream tailStream)
			: base(tailStream)
		{
			this.m_HeadStream = headStream;
			this.m_HeadEOF = headStream == Stream.Null;
		}

		// Token: 0x170006F4 RID: 1780
		// (get) Token: 0x06001C4D RID: 7245 RVA: 0x00086856 File Offset: 0x00084A56
		public override bool CanRead
		{
			get
			{
				if (!this.m_HeadEOF)
				{
					return this.m_HeadStream.CanRead;
				}
				return base.WrappedStream.CanRead;
			}
		}

		// Token: 0x170006F5 RID: 1781
		// (get) Token: 0x06001C4E RID: 7246 RVA: 0x00086877 File Offset: 0x00084A77
		public override bool CanSeek
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170006F6 RID: 1782
		// (get) Token: 0x06001C4F RID: 7247 RVA: 0x0008687A File Offset: 0x00084A7A
		public override bool CanWrite
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170006F7 RID: 1783
		// (get) Token: 0x06001C50 RID: 7248 RVA: 0x0008687D File Offset: 0x00084A7D
		public override long Length
		{
			get
			{
				return base.WrappedStream.Length + (this.m_HeadEOF ? this.m_HeadLength : this.m_HeadStream.Length);
			}
		}

		// Token: 0x170006F8 RID: 1784
		// (get) Token: 0x06001C51 RID: 7249 RVA: 0x000868A6 File Offset: 0x00084AA6
		// (set) Token: 0x06001C52 RID: 7250 RVA: 0x000868CF File Offset: 0x00084ACF
		public override long Position
		{
			get
			{
				return base.WrappedStream.Position + (this.m_HeadEOF ? this.m_HeadLength : this.m_HeadStream.Position);
			}
			set
			{
				throw new NotSupportedException(SR.GetString("net_noseek"));
			}
		}

		// Token: 0x06001C53 RID: 7251 RVA: 0x000868E0 File Offset: 0x00084AE0
		public override long Seek(long offset, SeekOrigin origin)
		{
			throw new NotSupportedException(SR.GetString("net_noseek"));
		}

		// Token: 0x06001C54 RID: 7252 RVA: 0x000868F1 File Offset: 0x00084AF1
		public override void SetLength(long value)
		{
			throw new NotSupportedException(SR.GetString("net_noseek"));
		}

		// Token: 0x06001C55 RID: 7253 RVA: 0x00086902 File Offset: 0x00084B02
		public override void Write(byte[] buffer, int offset, int count)
		{
			throw new NotSupportedException(SR.GetString("net_noseek"));
		}

		// Token: 0x06001C56 RID: 7254 RVA: 0x00086913 File Offset: 0x00084B13
		public override IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
		{
			throw new NotSupportedException(SR.GetString("net_noseek"));
		}

		// Token: 0x06001C57 RID: 7255 RVA: 0x00086924 File Offset: 0x00084B24
		public override void EndWrite(IAsyncResult asyncResult)
		{
			throw new NotSupportedException(SR.GetString("net_noseek"));
		}

		// Token: 0x06001C58 RID: 7256 RVA: 0x00086935 File Offset: 0x00084B35
		public override void Flush()
		{
		}

		// Token: 0x06001C59 RID: 7257 RVA: 0x00086938 File Offset: 0x00084B38
		public override int Read(byte[] buffer, int offset, int count)
		{
			int num;
			try
			{
				if (Interlocked.Increment(ref this.m_ReadNesting) != 1)
				{
					throw new NotSupportedException(SR.GetString("net_io_invalidnestedcall", new object[] { "Read", "read" }));
				}
				if (this.m_HeadEOF)
				{
					num = base.WrappedStream.Read(buffer, offset, count);
				}
				else
				{
					int num2 = this.m_HeadStream.Read(buffer, offset, count);
					this.m_HeadLength += (long)num2;
					if (num2 == 0 && count != 0)
					{
						this.m_HeadEOF = true;
						this.m_HeadStream.Close();
						num2 = base.WrappedStream.Read(buffer, offset, count);
					}
					num = num2;
				}
			}
			finally
			{
				Interlocked.Decrement(ref this.m_ReadNesting);
			}
			return num;
		}

		// Token: 0x06001C5A RID: 7258 RVA: 0x000869FC File Offset: 0x00084BFC
		private void ReadCallback(IAsyncResult transportResult)
		{
			if (transportResult.CompletedSynchronously)
			{
				return;
			}
			CombinedReadStream.InnerAsyncResult innerAsyncResult = transportResult.AsyncState as CombinedReadStream.InnerAsyncResult;
			try
			{
				int num;
				if (!this.m_HeadEOF)
				{
					num = this.m_HeadStream.EndRead(transportResult);
					this.m_HeadLength += (long)num;
				}
				else
				{
					num = base.WrappedStream.EndRead(transportResult);
				}
				if (!this.m_HeadEOF && num == 0 && innerAsyncResult.Count != 0)
				{
					this.m_HeadEOF = true;
					this.m_HeadStream.Close();
					IAsyncResult asyncResult = base.WrappedStream.BeginRead(innerAsyncResult.Buffer, innerAsyncResult.Offset, innerAsyncResult.Count, this.m_ReadCallback, innerAsyncResult);
					if (!asyncResult.CompletedSynchronously)
					{
						return;
					}
					num = base.WrappedStream.EndRead(asyncResult);
				}
				innerAsyncResult.Buffer = null;
				innerAsyncResult.InvokeCallback(num);
			}
			catch (Exception ex)
			{
				if (innerAsyncResult.InternalPeekCompleted)
				{
					throw;
				}
				innerAsyncResult.InvokeCallback(ex);
			}
		}

		// Token: 0x06001C5B RID: 7259 RVA: 0x00086AEC File Offset: 0x00084CEC
		public override IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
		{
			IAsyncResult asyncResult;
			try
			{
				if (Interlocked.Increment(ref this.m_ReadNesting) != 1)
				{
					throw new NotSupportedException(SR.GetString("net_io_invalidnestedcall", new object[] { "BeginRead", "read" }));
				}
				if (this.m_ReadCallback == null)
				{
					this.m_ReadCallback = new AsyncCallback(this.ReadCallback);
				}
				if (this.m_HeadEOF)
				{
					asyncResult = base.WrappedStream.BeginRead(buffer, offset, count, callback, state);
				}
				else
				{
					CombinedReadStream.InnerAsyncResult innerAsyncResult = new CombinedReadStream.InnerAsyncResult(state, callback, buffer, offset, count);
					IAsyncResult asyncResult2 = this.m_HeadStream.BeginRead(buffer, offset, count, this.m_ReadCallback, innerAsyncResult);
					if (!asyncResult2.CompletedSynchronously)
					{
						asyncResult = innerAsyncResult;
					}
					else
					{
						int num = this.m_HeadStream.EndRead(asyncResult2);
						this.m_HeadLength += (long)num;
						if (num == 0 && innerAsyncResult.Count != 0)
						{
							this.m_HeadEOF = true;
							this.m_HeadStream.Close();
							asyncResult = base.WrappedStream.BeginRead(buffer, offset, count, callback, state);
						}
						else
						{
							innerAsyncResult.Buffer = null;
							innerAsyncResult.InvokeCallback(count);
							asyncResult = innerAsyncResult;
						}
					}
				}
			}
			catch
			{
				Interlocked.Decrement(ref this.m_ReadNesting);
				throw;
			}
			return asyncResult;
		}

		// Token: 0x06001C5C RID: 7260 RVA: 0x00086C28 File Offset: 0x00084E28
		public override int EndRead(IAsyncResult asyncResult)
		{
			if (Interlocked.Decrement(ref this.m_ReadNesting) != 0)
			{
				Interlocked.Increment(ref this.m_ReadNesting);
				throw new InvalidOperationException(SR.GetString("net_io_invalidendcall", new object[] { "EndRead" }));
			}
			if (asyncResult == null)
			{
				throw new ArgumentNullException("asyncResult");
			}
			CombinedReadStream.InnerAsyncResult innerAsyncResult = asyncResult as CombinedReadStream.InnerAsyncResult;
			if (innerAsyncResult == null)
			{
				if (!this.m_HeadEOF)
				{
					return this.m_HeadStream.EndRead(asyncResult);
				}
				return base.WrappedStream.EndRead(asyncResult);
			}
			else
			{
				innerAsyncResult.InternalWaitForCompletion();
				if (innerAsyncResult.Result is Exception)
				{
					throw (Exception)innerAsyncResult.Result;
				}
				return (int)innerAsyncResult.Result;
			}
		}

		// Token: 0x06001C5D RID: 7261 RVA: 0x00086CD1 File Offset: 0x00084ED1
		protected sealed override void Dispose(bool disposing)
		{
			this.Dispose(disposing, CloseExState.Normal);
		}

		// Token: 0x06001C5E RID: 7262 RVA: 0x00086CDB File Offset: 0x00084EDB
		void ICloseEx.CloseEx(CloseExState closeState)
		{
			this.Dispose(true, closeState);
		}

		// Token: 0x06001C5F RID: 7263 RVA: 0x00086CE8 File Offset: 0x00084EE8
		protected virtual void Dispose(bool disposing, CloseExState closeState)
		{
			try
			{
				if (disposing)
				{
					try
					{
						if (!this.m_HeadEOF)
						{
							ICloseEx closeEx = this.m_HeadStream as ICloseEx;
							if (closeEx != null)
							{
								closeEx.CloseEx(closeState);
							}
							else
							{
								this.m_HeadStream.Close();
							}
						}
					}
					finally
					{
						ICloseEx closeEx2 = base.WrappedStream as ICloseEx;
						if (closeEx2 != null)
						{
							closeEx2.CloseEx(closeState);
						}
						else
						{
							base.WrappedStream.Close();
						}
					}
				}
			}
			finally
			{
				base.Dispose(disposing);
			}
		}

		// Token: 0x170006F9 RID: 1785
		// (get) Token: 0x06001C60 RID: 7264 RVA: 0x00086D70 File Offset: 0x00084F70
		public override bool CanTimeout
		{
			get
			{
				return base.WrappedStream.CanTimeout && this.m_HeadStream.CanTimeout;
			}
		}

		// Token: 0x170006FA RID: 1786
		// (get) Token: 0x06001C61 RID: 7265 RVA: 0x00086D8C File Offset: 0x00084F8C
		// (set) Token: 0x06001C62 RID: 7266 RVA: 0x00086DB0 File Offset: 0x00084FB0
		public override int ReadTimeout
		{
			get
			{
				if (!this.m_HeadEOF)
				{
					return this.m_HeadStream.ReadTimeout;
				}
				return base.WrappedStream.ReadTimeout;
			}
			set
			{
				Stream wrappedStream = base.WrappedStream;
				this.m_HeadStream.ReadTimeout = value;
				wrappedStream.ReadTimeout = value;
			}
		}

		// Token: 0x170006FB RID: 1787
		// (get) Token: 0x06001C63 RID: 7267 RVA: 0x00086DD7 File Offset: 0x00084FD7
		// (set) Token: 0x06001C64 RID: 7268 RVA: 0x00086DF8 File Offset: 0x00084FF8
		public override int WriteTimeout
		{
			get
			{
				if (!this.m_HeadEOF)
				{
					return this.m_HeadStream.WriteTimeout;
				}
				return base.WrappedStream.WriteTimeout;
			}
			set
			{
				Stream wrappedStream = base.WrappedStream;
				this.m_HeadStream.WriteTimeout = value;
				wrappedStream.WriteTimeout = value;
			}
		}

		// Token: 0x04001B8A RID: 7050
		private Stream m_HeadStream;

		// Token: 0x04001B8B RID: 7051
		private bool m_HeadEOF;

		// Token: 0x04001B8C RID: 7052
		private long m_HeadLength;

		// Token: 0x04001B8D RID: 7053
		private int m_ReadNesting;

		// Token: 0x04001B8E RID: 7054
		private AsyncCallback m_ReadCallback;

		// Token: 0x020007B9 RID: 1977
		private class InnerAsyncResult : LazyAsyncResult
		{
			// Token: 0x06004333 RID: 17203 RVA: 0x00119B70 File Offset: 0x00117D70
			public InnerAsyncResult(object userState, AsyncCallback userCallback, byte[] buffer, int offset, int count)
				: base(null, userState, userCallback)
			{
				this.Buffer = buffer;
				this.Offset = offset;
				this.Count = count;
			}

			// Token: 0x04003449 RID: 13385
			public byte[] Buffer;

			// Token: 0x0400344A RID: 13386
			public int Offset;

			// Token: 0x0400344B RID: 13387
			public int Count;
		}
	}
}
