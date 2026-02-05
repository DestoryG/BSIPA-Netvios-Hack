using System;
using System.IO;
using System.Threading;

namespace System.Net.Cache
{
	// Token: 0x0200031C RID: 796
	internal class ForwardingReadStream : BaseWrapperStream, ICloseEx
	{
		// Token: 0x06001C65 RID: 7269 RVA: 0x00086E1F File Offset: 0x0008501F
		internal ForwardingReadStream(Stream originalStream, Stream shadowStream, long bytesToSkip, bool throwOnWriteError)
			: base(originalStream)
		{
			if (!shadowStream.CanWrite)
			{
				throw new ArgumentException(SR.GetString("net_cache_shadowstream_not_writable"), "shadowStream");
			}
			this.m_ShadowStream = shadowStream;
			this.m_BytesToSkip = bytesToSkip;
			this.m_ThrowOnWriteError = throwOnWriteError;
		}

		// Token: 0x170006FC RID: 1788
		// (get) Token: 0x06001C66 RID: 7270 RVA: 0x00086E5B File Offset: 0x0008505B
		public override bool CanRead
		{
			get
			{
				return base.WrappedStream.CanRead;
			}
		}

		// Token: 0x170006FD RID: 1789
		// (get) Token: 0x06001C67 RID: 7271 RVA: 0x00086E68 File Offset: 0x00085068
		public override bool CanSeek
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170006FE RID: 1790
		// (get) Token: 0x06001C68 RID: 7272 RVA: 0x00086E6B File Offset: 0x0008506B
		public override bool CanWrite
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170006FF RID: 1791
		// (get) Token: 0x06001C69 RID: 7273 RVA: 0x00086E6E File Offset: 0x0008506E
		public override long Length
		{
			get
			{
				return base.WrappedStream.Length - this.m_BytesToSkip;
			}
		}

		// Token: 0x17000700 RID: 1792
		// (get) Token: 0x06001C6A RID: 7274 RVA: 0x00086E82 File Offset: 0x00085082
		// (set) Token: 0x06001C6B RID: 7275 RVA: 0x00086E96 File Offset: 0x00085096
		public override long Position
		{
			get
			{
				return base.WrappedStream.Position - this.m_BytesToSkip;
			}
			set
			{
				throw new NotSupportedException(SR.GetString("net_noseek"));
			}
		}

		// Token: 0x06001C6C RID: 7276 RVA: 0x00086EA7 File Offset: 0x000850A7
		public override long Seek(long offset, SeekOrigin origin)
		{
			throw new NotSupportedException(SR.GetString("net_noseek"));
		}

		// Token: 0x06001C6D RID: 7277 RVA: 0x00086EB8 File Offset: 0x000850B8
		public override void SetLength(long value)
		{
			throw new NotSupportedException(SR.GetString("net_noseek"));
		}

		// Token: 0x06001C6E RID: 7278 RVA: 0x00086EC9 File Offset: 0x000850C9
		public override void Write(byte[] buffer, int offset, int count)
		{
			throw new NotSupportedException(SR.GetString("net_noseek"));
		}

		// Token: 0x06001C6F RID: 7279 RVA: 0x00086EDA File Offset: 0x000850DA
		public override IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
		{
			throw new NotSupportedException(SR.GetString("net_noseek"));
		}

		// Token: 0x06001C70 RID: 7280 RVA: 0x00086EEB File Offset: 0x000850EB
		public override void EndWrite(IAsyncResult asyncResult)
		{
			throw new NotSupportedException(SR.GetString("net_noseek"));
		}

		// Token: 0x06001C71 RID: 7281 RVA: 0x00086EFC File Offset: 0x000850FC
		public override void Flush()
		{
		}

		// Token: 0x06001C72 RID: 7282 RVA: 0x00086F00 File Offset: 0x00085100
		public override int Read(byte[] buffer, int offset, int count)
		{
			bool flag = false;
			int num = -1;
			if (Interlocked.Increment(ref this.m_ReadNesting) != 1)
			{
				throw new NotSupportedException(SR.GetString("net_io_invalidnestedcall", new object[] { "Read", "read" }));
			}
			int num3;
			try
			{
				if (this.m_BytesToSkip != 0L)
				{
					byte[] array = new byte[4096];
					while (this.m_BytesToSkip != 0L)
					{
						int num2 = base.WrappedStream.Read(array, 0, (this.m_BytesToSkip < (long)array.Length) ? ((int)this.m_BytesToSkip) : array.Length);
						if (num2 == 0)
						{
							this.m_SeenReadEOF = true;
						}
						this.m_BytesToSkip -= (long)num2;
						if (!this.m_ShadowStreamIsDead)
						{
							this.m_ShadowStream.Write(array, 0, num2);
						}
					}
				}
				num = base.WrappedStream.Read(buffer, offset, count);
				if (num == 0)
				{
					this.m_SeenReadEOF = true;
				}
				if (this.m_ShadowStreamIsDead)
				{
					num3 = num;
				}
				else
				{
					flag = true;
					this.m_ShadowStream.Write(buffer, offset, num);
					num3 = num;
				}
			}
			catch (Exception ex)
			{
				if (ex is ThreadAbortException || ex is StackOverflowException || ex is OutOfMemoryException)
				{
					throw;
				}
				if (!this.m_ShadowStreamIsDead)
				{
					this.m_ShadowStreamIsDead = true;
					try
					{
						if (this.m_ShadowStream is ICloseEx)
						{
							((ICloseEx)this.m_ShadowStream).CloseEx(CloseExState.Abort | CloseExState.Silent);
						}
						else
						{
							this.m_ShadowStream.Close();
						}
					}
					catch (Exception ex2)
					{
						if (ex is ThreadAbortException || ex is StackOverflowException || ex is OutOfMemoryException)
						{
							throw;
						}
					}
				}
				if (!flag || this.m_ThrowOnWriteError)
				{
					throw;
				}
				num3 = num;
			}
			finally
			{
				Interlocked.Decrement(ref this.m_ReadNesting);
			}
			return num3;
		}

		// Token: 0x06001C73 RID: 7283 RVA: 0x000870E4 File Offset: 0x000852E4
		private void ReadCallback(IAsyncResult transportResult)
		{
			if (transportResult.CompletedSynchronously)
			{
				return;
			}
			ForwardingReadStream.InnerAsyncResult innerAsyncResult = transportResult.AsyncState as ForwardingReadStream.InnerAsyncResult;
			this.ReadComplete(transportResult);
		}

		// Token: 0x06001C74 RID: 7284 RVA: 0x00087110 File Offset: 0x00085310
		private void ReadComplete(IAsyncResult transportResult)
		{
			for (;;)
			{
				ForwardingReadStream.InnerAsyncResult innerAsyncResult = transportResult.AsyncState as ForwardingReadStream.InnerAsyncResult;
				try
				{
					if (!innerAsyncResult.IsWriteCompletion)
					{
						innerAsyncResult.Count = base.WrappedStream.EndRead(transportResult);
						if (innerAsyncResult.Count == 0)
						{
							this.m_SeenReadEOF = true;
						}
						if (!this.m_ShadowStreamIsDead)
						{
							innerAsyncResult.IsWriteCompletion = true;
							transportResult = this.m_ShadowStream.BeginWrite(innerAsyncResult.Buffer, innerAsyncResult.Offset, innerAsyncResult.Count, this.m_ReadCallback, innerAsyncResult);
							if (transportResult.CompletedSynchronously)
							{
								continue;
							}
							break;
						}
					}
					else
					{
						this.m_ShadowStream.EndWrite(transportResult);
						innerAsyncResult.IsWriteCompletion = false;
					}
				}
				catch (Exception ex)
				{
					if (innerAsyncResult.InternalPeekCompleted)
					{
						throw;
					}
					try
					{
						this.m_ShadowStreamIsDead = true;
						if (this.m_ShadowStream is ICloseEx)
						{
							((ICloseEx)this.m_ShadowStream).CloseEx(CloseExState.Abort | CloseExState.Silent);
						}
						else
						{
							this.m_ShadowStream.Close();
						}
					}
					catch (Exception ex2)
					{
					}
					if (!innerAsyncResult.IsWriteCompletion || this.m_ThrowOnWriteError)
					{
						if (transportResult.CompletedSynchronously)
						{
							throw;
						}
						innerAsyncResult.InvokeCallback(ex);
						break;
					}
				}
				try
				{
					if (this.m_BytesToSkip != 0L)
					{
						this.m_BytesToSkip -= (long)innerAsyncResult.Count;
						innerAsyncResult.Count = ((this.m_BytesToSkip < (long)innerAsyncResult.Buffer.Length) ? ((int)this.m_BytesToSkip) : innerAsyncResult.Buffer.Length);
						if (this.m_BytesToSkip == 0L)
						{
							transportResult = innerAsyncResult;
							innerAsyncResult = innerAsyncResult.AsyncState as ForwardingReadStream.InnerAsyncResult;
						}
						transportResult = base.WrappedStream.BeginRead(innerAsyncResult.Buffer, innerAsyncResult.Offset, innerAsyncResult.Count, this.m_ReadCallback, innerAsyncResult);
						if (transportResult.CompletedSynchronously)
						{
							continue;
						}
					}
					else
					{
						innerAsyncResult.InvokeCallback(innerAsyncResult.Count);
					}
				}
				catch (Exception ex3)
				{
					if (innerAsyncResult.InternalPeekCompleted)
					{
						throw;
					}
					try
					{
						this.m_ShadowStreamIsDead = true;
						if (this.m_ShadowStream is ICloseEx)
						{
							((ICloseEx)this.m_ShadowStream).CloseEx(CloseExState.Abort | CloseExState.Silent);
						}
						else
						{
							this.m_ShadowStream.Close();
						}
					}
					catch (Exception ex4)
					{
					}
					if (transportResult.CompletedSynchronously)
					{
						throw;
					}
					innerAsyncResult.InvokeCallback(ex3);
				}
				break;
			}
		}

		// Token: 0x06001C75 RID: 7285 RVA: 0x00087348 File Offset: 0x00085548
		public override IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
		{
			if (Interlocked.Increment(ref this.m_ReadNesting) != 1)
			{
				throw new NotSupportedException(SR.GetString("net_io_invalidnestedcall", new object[] { "BeginRead", "read" }));
			}
			IAsyncResult asyncResult;
			try
			{
				if (this.m_ReadCallback == null)
				{
					this.m_ReadCallback = new AsyncCallback(this.ReadCallback);
				}
				if (this.m_ShadowStreamIsDead && this.m_BytesToSkip == 0L)
				{
					asyncResult = base.WrappedStream.BeginRead(buffer, offset, count, callback, state);
				}
				else
				{
					ForwardingReadStream.InnerAsyncResult innerAsyncResult = new ForwardingReadStream.InnerAsyncResult(state, callback, buffer, offset, count);
					if (this.m_BytesToSkip != 0L)
					{
						ForwardingReadStream.InnerAsyncResult innerAsyncResult2 = innerAsyncResult;
						innerAsyncResult = new ForwardingReadStream.InnerAsyncResult(innerAsyncResult2, null, new byte[4096], 0, (this.m_BytesToSkip < (long)buffer.Length) ? ((int)this.m_BytesToSkip) : buffer.Length);
					}
					IAsyncResult asyncResult2 = base.WrappedStream.BeginRead(innerAsyncResult.Buffer, innerAsyncResult.Offset, innerAsyncResult.Count, this.m_ReadCallback, innerAsyncResult);
					if (asyncResult2.CompletedSynchronously)
					{
						this.ReadComplete(asyncResult2);
					}
					asyncResult = innerAsyncResult;
				}
			}
			catch
			{
				Interlocked.Decrement(ref this.m_ReadNesting);
				throw;
			}
			return asyncResult;
		}

		// Token: 0x06001C76 RID: 7286 RVA: 0x00087468 File Offset: 0x00085668
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
			ForwardingReadStream.InnerAsyncResult innerAsyncResult = asyncResult as ForwardingReadStream.InnerAsyncResult;
			if (innerAsyncResult == null && base.WrappedStream.EndRead(asyncResult) == 0)
			{
				this.m_SeenReadEOF = true;
			}
			bool flag = false;
			try
			{
				innerAsyncResult.InternalWaitForCompletion();
				if (innerAsyncResult.Result is Exception)
				{
					throw (Exception)innerAsyncResult.Result;
				}
				flag = true;
			}
			finally
			{
				if (!flag && !this.m_ShadowStreamIsDead)
				{
					this.m_ShadowStreamIsDead = true;
					if (this.m_ShadowStream is ICloseEx)
					{
						((ICloseEx)this.m_ShadowStream).CloseEx(CloseExState.Abort | CloseExState.Silent);
					}
					else
					{
						this.m_ShadowStream.Close();
					}
				}
			}
			return (int)innerAsyncResult.Result;
		}

		// Token: 0x06001C77 RID: 7287 RVA: 0x0008755C File Offset: 0x0008575C
		protected sealed override void Dispose(bool disposing)
		{
			this.Dispose(disposing, CloseExState.Normal);
		}

		// Token: 0x06001C78 RID: 7288 RVA: 0x00087568 File Offset: 0x00085768
		void ICloseEx.CloseEx(CloseExState closeState)
		{
			if (Interlocked.Increment(ref this._Disposed) == 1)
			{
				if (closeState == CloseExState.Silent)
				{
					try
					{
						int num = 0;
						int num2;
						while (num < ConnectStream.s_DrainingBuffer.Length && (num2 = this.Read(ConnectStream.s_DrainingBuffer, 0, ConnectStream.s_DrainingBuffer.Length)) > 0)
						{
							num += num2;
						}
					}
					catch (Exception ex)
					{
						if (ex is ThreadAbortException || ex is StackOverflowException || ex is OutOfMemoryException)
						{
							throw;
						}
					}
				}
				this.Dispose(true, closeState);
			}
		}

		// Token: 0x06001C79 RID: 7289 RVA: 0x000875E8 File Offset: 0x000857E8
		protected virtual void Dispose(bool disposing, CloseExState closeState)
		{
			try
			{
				if (disposing)
				{
					try
					{
						ICloseEx closeEx = base.WrappedStream as ICloseEx;
						if (closeEx != null)
						{
							closeEx.CloseEx(closeState);
						}
						else
						{
							base.WrappedStream.Close();
						}
					}
					finally
					{
						if (!this.m_SeenReadEOF)
						{
							closeState |= CloseExState.Abort;
						}
						if (this.m_ShadowStream is ICloseEx)
						{
							((ICloseEx)this.m_ShadowStream).CloseEx(closeState);
						}
						else
						{
							this.m_ShadowStream.Close();
						}
					}
				}
			}
			finally
			{
				base.Dispose(disposing);
			}
		}

		// Token: 0x17000701 RID: 1793
		// (get) Token: 0x06001C7A RID: 7290 RVA: 0x0008767C File Offset: 0x0008587C
		public override bool CanTimeout
		{
			get
			{
				return base.WrappedStream.CanTimeout && this.m_ShadowStream.CanTimeout;
			}
		}

		// Token: 0x17000702 RID: 1794
		// (get) Token: 0x06001C7B RID: 7291 RVA: 0x00087698 File Offset: 0x00085898
		// (set) Token: 0x06001C7C RID: 7292 RVA: 0x000876A8 File Offset: 0x000858A8
		public override int ReadTimeout
		{
			get
			{
				return base.WrappedStream.ReadTimeout;
			}
			set
			{
				Stream wrappedStream = base.WrappedStream;
				this.m_ShadowStream.ReadTimeout = value;
				wrappedStream.ReadTimeout = value;
			}
		}

		// Token: 0x17000703 RID: 1795
		// (get) Token: 0x06001C7D RID: 7293 RVA: 0x000876CF File Offset: 0x000858CF
		// (set) Token: 0x06001C7E RID: 7294 RVA: 0x000876DC File Offset: 0x000858DC
		public override int WriteTimeout
		{
			get
			{
				return this.m_ShadowStream.WriteTimeout;
			}
			set
			{
				Stream wrappedStream = base.WrappedStream;
				this.m_ShadowStream.WriteTimeout = value;
				wrappedStream.WriteTimeout = value;
			}
		}

		// Token: 0x04001B8F RID: 7055
		private Stream m_ShadowStream;

		// Token: 0x04001B90 RID: 7056
		private int m_ReadNesting;

		// Token: 0x04001B91 RID: 7057
		private bool m_ShadowStreamIsDead;

		// Token: 0x04001B92 RID: 7058
		private AsyncCallback m_ReadCallback;

		// Token: 0x04001B93 RID: 7059
		private long m_BytesToSkip;

		// Token: 0x04001B94 RID: 7060
		private bool m_ThrowOnWriteError;

		// Token: 0x04001B95 RID: 7061
		private bool m_SeenReadEOF;

		// Token: 0x04001B96 RID: 7062
		private int _Disposed;

		// Token: 0x020007BA RID: 1978
		private class InnerAsyncResult : LazyAsyncResult
		{
			// Token: 0x06004334 RID: 17204 RVA: 0x00119B92 File Offset: 0x00117D92
			public InnerAsyncResult(object userState, AsyncCallback userCallback, byte[] buffer, int offset, int count)
				: base(null, userState, userCallback)
			{
				this.Buffer = buffer;
				this.Offset = offset;
				this.Count = count;
			}

			// Token: 0x0400344C RID: 13388
			public byte[] Buffer;

			// Token: 0x0400344D RID: 13389
			public int Offset;

			// Token: 0x0400344E RID: 13390
			public int Count;

			// Token: 0x0400344F RID: 13391
			public bool IsWriteCompletion;
		}
	}
}
