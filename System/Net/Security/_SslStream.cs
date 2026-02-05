using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Threading;

namespace System.Net.Security
{
	// Token: 0x02000361 RID: 865
	internal class _SslStream
	{
		// Token: 0x06001F9F RID: 8095 RVA: 0x0009399C File Offset: 0x00091B9C
		internal _SslStream(SslState sslState)
		{
			if (global::System.PinnableBufferCacheEventSource.Log.IsEnabled())
			{
				global::System.PinnableBufferCacheEventSource.Log.DebugMessage1("CTOR: In System.Net._SslStream.SslStream", (long)this.GetHashCode());
			}
			this._SslState = sslState;
			this._Reader = new FixedSizeReader(this._SslState.InnerStream);
		}

		// Token: 0x06001FA0 RID: 8096 RVA: 0x000939EE File Offset: 0x00091BEE
		private void FreeReadBuffer()
		{
			if (this._InternalBufferFromPinnableCache)
			{
				_SslStream.s_PinnableReadBufferCache.FreeBuffer(this._InternalBuffer);
				this._InternalBufferFromPinnableCache = false;
			}
			this._InternalBuffer = null;
		}

		// Token: 0x06001FA1 RID: 8097 RVA: 0x00093A18 File Offset: 0x00091C18
		~_SslStream()
		{
			if (this._InternalBufferFromPinnableCache)
			{
				if (global::System.PinnableBufferCacheEventSource.Log.IsEnabled())
				{
					global::System.PinnableBufferCacheEventSource.Log.DebugMessage2("DTOR: In System.Net._SslStream.~SslStream Freeing Read Buffer", (long)this.GetHashCode(), global::System.PinnableBufferCacheEventSource.AddressOfByteArray(this._InternalBuffer));
				}
				this.FreeReadBuffer();
			}
			if (this._PinnableOutputBuffer != null)
			{
				if (global::System.PinnableBufferCacheEventSource.Log.IsEnabled())
				{
					global::System.PinnableBufferCacheEventSource.Log.DebugMessage2("DTOR: In System.Net._SslStream.~SslStream Freeing Write Buffer", (long)this.GetHashCode(), global::System.PinnableBufferCacheEventSource.AddressOfByteArray(this._PinnableOutputBuffer));
				}
				_SslStream.s_PinnableWriteBufferCache.FreeBuffer(this._PinnableOutputBuffer);
			}
		}

		// Token: 0x06001FA2 RID: 8098 RVA: 0x00093AC0 File Offset: 0x00091CC0
		internal int Read(byte[] buffer, int offset, int count)
		{
			return this.ProcessRead(buffer, offset, count, null);
		}

		// Token: 0x06001FA3 RID: 8099 RVA: 0x00093ACC File Offset: 0x00091CCC
		internal void Write(byte[] buffer, int offset, int count)
		{
			this.ProcessWrite(buffer, offset, count, null);
		}

		// Token: 0x06001FA4 RID: 8100 RVA: 0x00093AD8 File Offset: 0x00091CD8
		internal void Write(BufferOffsetSize[] buffers)
		{
			this.ProcessWrite(buffers, null);
		}

		// Token: 0x06001FA5 RID: 8101 RVA: 0x00093AE4 File Offset: 0x00091CE4
		internal IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback asyncCallback, object asyncState)
		{
			BufferAsyncResult bufferAsyncResult = new BufferAsyncResult(this, buffer, offset, count, asyncState, asyncCallback);
			AsyncProtocolRequest asyncProtocolRequest = new AsyncProtocolRequest(bufferAsyncResult);
			this.ProcessRead(buffer, offset, count, asyncProtocolRequest);
			return bufferAsyncResult;
		}

		// Token: 0x06001FA6 RID: 8102 RVA: 0x00093B14 File Offset: 0x00091D14
		internal int EndRead(IAsyncResult asyncResult)
		{
			if (asyncResult == null)
			{
				throw new ArgumentNullException("asyncResult");
			}
			BufferAsyncResult bufferAsyncResult = asyncResult as BufferAsyncResult;
			if (bufferAsyncResult == null)
			{
				throw new ArgumentException(SR.GetString("net_io_async_result", new object[] { asyncResult.GetType().FullName }), "asyncResult");
			}
			if (Interlocked.Exchange(ref this._NestedRead, 0) == 0)
			{
				throw new InvalidOperationException(SR.GetString("net_io_invalidendcall", new object[] { "EndRead" }));
			}
			bufferAsyncResult.InternalWaitForCompletion();
			if (!(bufferAsyncResult.Result is Exception))
			{
				return (int)bufferAsyncResult.Result;
			}
			if (bufferAsyncResult.Result is IOException)
			{
				throw (Exception)bufferAsyncResult.Result;
			}
			throw new IOException(SR.GetString("net_io_read"), (Exception)bufferAsyncResult.Result);
		}

		// Token: 0x06001FA7 RID: 8103 RVA: 0x00093BE4 File Offset: 0x00091DE4
		internal IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback asyncCallback, object asyncState)
		{
			LazyAsyncResult lazyAsyncResult = new LazyAsyncResult(this, asyncState, asyncCallback);
			AsyncProtocolRequest asyncProtocolRequest = new AsyncProtocolRequest(lazyAsyncResult);
			this.ProcessWrite(buffer, offset, count, asyncProtocolRequest);
			return lazyAsyncResult;
		}

		// Token: 0x06001FA8 RID: 8104 RVA: 0x00093C10 File Offset: 0x00091E10
		internal IAsyncResult BeginWrite(BufferOffsetSize[] buffers, AsyncCallback asyncCallback, object asyncState)
		{
			LazyAsyncResult lazyAsyncResult = new LazyAsyncResult(this, asyncState, asyncCallback);
			_SslStream.SplitWriteAsyncProtocolRequest splitWriteAsyncProtocolRequest = new _SslStream.SplitWriteAsyncProtocolRequest(lazyAsyncResult);
			this.ProcessWrite(buffers, splitWriteAsyncProtocolRequest);
			return lazyAsyncResult;
		}

		// Token: 0x06001FA9 RID: 8105 RVA: 0x00093C38 File Offset: 0x00091E38
		internal void EndWrite(IAsyncResult asyncResult)
		{
			if (asyncResult == null)
			{
				throw new ArgumentNullException("asyncResult");
			}
			LazyAsyncResult lazyAsyncResult = asyncResult as LazyAsyncResult;
			if (lazyAsyncResult == null)
			{
				throw new ArgumentException(SR.GetString("net_io_async_result", new object[] { asyncResult.GetType().FullName }), "asyncResult");
			}
			if (Interlocked.Exchange(ref this._NestedWrite, 0) == 0)
			{
				throw new InvalidOperationException(SR.GetString("net_io_invalidendcall", new object[] { "EndWrite" }));
			}
			lazyAsyncResult.InternalWaitForCompletion();
			if (!(lazyAsyncResult.Result is Exception))
			{
				return;
			}
			if (lazyAsyncResult.Result is IOException)
			{
				throw (Exception)lazyAsyncResult.Result;
			}
			throw new IOException(SR.GetString("net_io_write"), (Exception)lazyAsyncResult.Result);
		}

		// Token: 0x17000842 RID: 2114
		// (get) Token: 0x06001FAA RID: 8106 RVA: 0x00093CFA File Offset: 0x00091EFA
		internal bool DataAvailable
		{
			get
			{
				return this.InternalBufferCount != 0;
			}
		}

		// Token: 0x17000843 RID: 2115
		// (get) Token: 0x06001FAB RID: 8107 RVA: 0x00093D05 File Offset: 0x00091F05
		private byte[] InternalBuffer
		{
			get
			{
				return this._InternalBuffer;
			}
		}

		// Token: 0x17000844 RID: 2116
		// (get) Token: 0x06001FAC RID: 8108 RVA: 0x00093D0D File Offset: 0x00091F0D
		private int InternalOffset
		{
			get
			{
				return this._InternalOffset;
			}
		}

		// Token: 0x17000845 RID: 2117
		// (get) Token: 0x06001FAD RID: 8109 RVA: 0x00093D15 File Offset: 0x00091F15
		private int InternalBufferCount
		{
			get
			{
				return this._InternalBufferCount;
			}
		}

		// Token: 0x06001FAE RID: 8110 RVA: 0x00093D1D File Offset: 0x00091F1D
		private void DecrementInternalBufferCount(int decrCount)
		{
			this._InternalOffset += decrCount;
			this._InternalBufferCount -= decrCount;
		}

		// Token: 0x06001FAF RID: 8111 RVA: 0x00093D3C File Offset: 0x00091F3C
		private void EnsureInternalBufferSize(int curOffset, int addSize)
		{
			if (this._InternalBuffer == null || this._InternalBuffer.Length < addSize + curOffset)
			{
				bool internalBufferFromPinnableCache = this._InternalBufferFromPinnableCache;
				byte[] internalBuffer = this._InternalBuffer;
				int num = addSize + curOffset;
				if (num <= 16416)
				{
					if (global::System.PinnableBufferCacheEventSource.Log.IsEnabled())
					{
						global::System.PinnableBufferCacheEventSource.Log.DebugMessage2("In System.Net._SslStream.EnsureInternalBufferSize IS pinnable", (long)this.GetHashCode(), (long)num);
					}
					this._InternalBufferFromPinnableCache = true;
					this._InternalBuffer = _SslStream.s_PinnableReadBufferCache.AllocateBuffer();
				}
				else
				{
					if (global::System.PinnableBufferCacheEventSource.Log.IsEnabled())
					{
						global::System.PinnableBufferCacheEventSource.Log.DebugMessage2("In System.Net._SslStream.EnsureInternalBufferSize NOT pinnable", (long)this.GetHashCode(), (long)num);
					}
					this._InternalBufferFromPinnableCache = false;
					this._InternalBuffer = new byte[num];
				}
				if (internalBuffer != null && curOffset != 0)
				{
					Buffer.BlockCopy(internalBuffer, 0, this._InternalBuffer, 0, curOffset);
				}
				if (internalBufferFromPinnableCache)
				{
					_SslStream.s_PinnableReadBufferCache.FreeBuffer(internalBuffer);
				}
			}
			this._InternalOffset = curOffset;
			this._InternalBufferCount = curOffset + addSize;
		}

		// Token: 0x06001FB0 RID: 8112 RVA: 0x00093E24 File Offset: 0x00092024
		private void ValidateParameters(byte[] buffer, int offset, int count)
		{
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			if (offset < 0)
			{
				throw new ArgumentOutOfRangeException("offset");
			}
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count");
			}
			if (count > buffer.Length - offset)
			{
				throw new ArgumentOutOfRangeException("count", SR.GetString("net_offset_plus_count"));
			}
		}

		// Token: 0x06001FB1 RID: 8113 RVA: 0x00093E7C File Offset: 0x0009207C
		private void ProcessWrite(BufferOffsetSize[] buffers, _SslStream.SplitWriteAsyncProtocolRequest asyncRequest)
		{
			this._SslState.CheckThrow(true, true);
			foreach (BufferOffsetSize bufferOffsetSize in buffers)
			{
				this.ValidateParameters(bufferOffsetSize.Buffer, bufferOffsetSize.Offset, bufferOffsetSize.Size);
			}
			if (Interlocked.Exchange(ref this._NestedWrite, 1) == 1)
			{
				throw new NotSupportedException(SR.GetString("net_io_invalidnestedcall", new object[]
				{
					(asyncRequest != null) ? "BeginWrite" : "Write",
					"write"
				}));
			}
			bool flag = false;
			try
			{
				SplitWritesState splitWritesState = new SplitWritesState(buffers);
				if (asyncRequest != null)
				{
					asyncRequest.SetNextRequest(splitWritesState, _SslStream._ResumeAsyncWriteCallback);
				}
				this.StartWriting(splitWritesState, asyncRequest);
			}
			catch (Exception ex)
			{
				this._SslState.FinishWrite();
				flag = true;
				if (ex is IOException)
				{
					throw;
				}
				throw new IOException(SR.GetString("net_io_write"), ex);
			}
			finally
			{
				if (asyncRequest == null || flag)
				{
					this._NestedWrite = 0;
				}
			}
		}

		// Token: 0x06001FB2 RID: 8114 RVA: 0x00093F80 File Offset: 0x00092180
		private void ProcessWrite(byte[] buffer, int offset, int count, AsyncProtocolRequest asyncRequest)
		{
			if (this._SslState.LastPayload != null)
			{
				BufferOffsetSize[] array = new BufferOffsetSize[]
				{
					new BufferOffsetSize(buffer, offset, count, false)
				};
				if (asyncRequest != null)
				{
					this.ProcessWrite(array, new _SslStream.SplitWriteAsyncProtocolRequest(asyncRequest.UserAsyncResult));
					return;
				}
				this.ProcessWrite(array, null);
				return;
			}
			else
			{
				this.ValidateParameters(buffer, offset, count);
				this._SslState.CheckThrow(true, true);
				if (Interlocked.Exchange(ref this._NestedWrite, 1) == 1)
				{
					throw new NotSupportedException(SR.GetString("net_io_invalidnestedcall", new object[]
					{
						(asyncRequest != null) ? "BeginWrite" : "Write",
						"write"
					}));
				}
				bool flag = false;
				try
				{
					this.StartWriting(buffer, offset, count, asyncRequest);
				}
				catch (Exception ex)
				{
					this._SslState.FinishWrite();
					flag = true;
					if (ex is IOException)
					{
						throw;
					}
					throw new IOException(SR.GetString("net_io_write"), ex);
				}
				finally
				{
					if (asyncRequest == null || flag)
					{
						this._NestedWrite = 0;
					}
				}
				return;
			}
		}

		// Token: 0x06001FB3 RID: 8115 RVA: 0x0009408C File Offset: 0x0009228C
		private void StartWriting(SplitWritesState splitWrite, _SslStream.SplitWriteAsyncProtocolRequest asyncRequest)
		{
			while (!splitWrite.IsDone)
			{
				if (this._SslState.CheckEnqueueWrite(asyncRequest))
				{
					return;
				}
				byte[] array = null;
				if (this._SslState.LastPayload != null)
				{
					array = this._SslState.LastPayload;
					this._SslState.LastPayloadConsumed();
				}
				BufferOffsetSize[] array2 = splitWrite.GetNextBuffers();
				array2 = this.EncryptBuffers(array2, array);
				if (asyncRequest != null)
				{
					IAsyncResult asyncResult = ((NetworkStream)this._SslState.InnerStream).BeginMultipleWrite(array2, _SslStream._MulitpleWriteCallback, asyncRequest);
					if (!asyncResult.CompletedSynchronously)
					{
						return;
					}
					((NetworkStream)this._SslState.InnerStream).EndMultipleWrite(asyncResult);
				}
				else
				{
					((NetworkStream)this._SslState.InnerStream).MultipleWrite(array2);
				}
				this._SslState.FinishWrite();
			}
			if (asyncRequest != null)
			{
				asyncRequest.CompleteUser();
			}
		}

		// Token: 0x06001FB4 RID: 8116 RVA: 0x0009415C File Offset: 0x0009235C
		private BufferOffsetSize[] EncryptBuffers(BufferOffsetSize[] buffers, byte[] lastHandshakePayload)
		{
			List<BufferOffsetSize> list = null;
			SecurityStatus securityStatus = SecurityStatus.OK;
			foreach (BufferOffsetSize bufferOffsetSize in buffers)
			{
				int num = Math.Min(bufferOffsetSize.Size, this._SslState.MaxDataSize);
				byte[] array2 = null;
				int num2;
				securityStatus = this._SslState.EncryptData(bufferOffsetSize.Buffer, bufferOffsetSize.Offset, num, ref array2, out num2);
				if (securityStatus != SecurityStatus.OK)
				{
					break;
				}
				if (num != bufferOffsetSize.Size || list != null)
				{
					if (list == null)
					{
						list = new List<BufferOffsetSize>(buffers.Length * (bufferOffsetSize.Size / num + 1));
						if (lastHandshakePayload != null)
						{
							list.Add(new BufferOffsetSize(lastHandshakePayload, false));
						}
						foreach (BufferOffsetSize bufferOffsetSize2 in buffers)
						{
							if (bufferOffsetSize2 == bufferOffsetSize)
							{
								break;
							}
							list.Add(bufferOffsetSize2);
						}
					}
					list.Add(new BufferOffsetSize(array2, 0, num2, false));
					while ((bufferOffsetSize.Size -= num) != 0)
					{
						bufferOffsetSize.Offset += num;
						num = Math.Min(bufferOffsetSize.Size, this._SslState.MaxDataSize);
						array2 = null;
						securityStatus = this._SslState.EncryptData(bufferOffsetSize.Buffer, bufferOffsetSize.Offset, num, ref array2, out num2);
						if (securityStatus != SecurityStatus.OK)
						{
							break;
						}
						list.Add(new BufferOffsetSize(array2, 0, num2, false));
					}
				}
				else
				{
					bufferOffsetSize.Buffer = array2;
					bufferOffsetSize.Offset = 0;
					bufferOffsetSize.Size = num2;
				}
				if (securityStatus != SecurityStatus.OK)
				{
					break;
				}
			}
			if (securityStatus != SecurityStatus.OK)
			{
				ProtocolToken protocolToken = new ProtocolToken(null, securityStatus);
				throw new IOException(SR.GetString("net_io_encrypt"), protocolToken.GetException());
			}
			if (list != null)
			{
				buffers = list.ToArray();
			}
			else if (lastHandshakePayload != null)
			{
				BufferOffsetSize[] array4 = new BufferOffsetSize[buffers.Length + 1];
				Array.Copy(buffers, 0, array4, 1, buffers.Length);
				array4[0] = new BufferOffsetSize(lastHandshakePayload, false);
				buffers = array4;
			}
			return buffers;
		}

		// Token: 0x06001FB5 RID: 8117 RVA: 0x00094334 File Offset: 0x00092534
		private void StartWriting(byte[] buffer, int offset, int count, AsyncProtocolRequest asyncRequest)
		{
			if (asyncRequest != null)
			{
				asyncRequest.SetNextRequest(buffer, offset, count, _SslStream._ResumeAsyncWriteCallback);
			}
			if (count >= 0)
			{
				byte[] array = null;
				if (this._PinnableOutputBufferInUse == null)
				{
					if (this._PinnableOutputBuffer == null)
					{
						this._PinnableOutputBuffer = _SslStream.s_PinnableWriteBufferCache.AllocateBuffer();
					}
					this._PinnableOutputBufferInUse = buffer;
					array = this._PinnableOutputBuffer;
					if (global::System.PinnableBufferCacheEventSource.Log.IsEnabled())
					{
						global::System.PinnableBufferCacheEventSource.Log.DebugMessage3("In System.Net._SslStream.StartWriting Trying Pinnable", (long)this.GetHashCode(), (long)count, global::System.PinnableBufferCacheEventSource.AddressOfByteArray(array));
					}
				}
				else if (global::System.PinnableBufferCacheEventSource.Log.IsEnabled())
				{
					global::System.PinnableBufferCacheEventSource.Log.DebugMessage2("In System.Net._SslStream.StartWriting BufferInUse", (long)this.GetHashCode(), (long)count);
				}
				while (!this._SslState.CheckEnqueueWrite(asyncRequest))
				{
					int num = Math.Min(count, this._SslState.MaxDataSize);
					int num2;
					SecurityStatus securityStatus = this._SslState.EncryptData(buffer, offset, num, ref array, out num2);
					if (securityStatus != SecurityStatus.OK)
					{
						ProtocolToken protocolToken = new ProtocolToken(null, securityStatus);
						throw new IOException(SR.GetString("net_io_encrypt"), protocolToken.GetException());
					}
					if (global::System.PinnableBufferCacheEventSource.Log.IsEnabled())
					{
						global::System.PinnableBufferCacheEventSource.Log.DebugMessage3("In System.Net._SslStream.StartWriting Got Encrypted Buffer", (long)this.GetHashCode(), (long)num2, global::System.PinnableBufferCacheEventSource.AddressOfByteArray(array));
					}
					if (asyncRequest != null)
					{
						asyncRequest.SetNextRequest(buffer, offset + num, count - num, _SslStream._ResumeAsyncWriteCallback);
						IAsyncResult asyncResult = this._SslState.InnerStream.BeginWrite(array, 0, num2, _SslStream._WriteCallback, asyncRequest);
						if (!asyncResult.CompletedSynchronously)
						{
							return;
						}
						this._SslState.InnerStream.EndWrite(asyncResult);
					}
					else
					{
						this._SslState.InnerStream.Write(array, 0, num2);
					}
					offset += num;
					count -= num;
					this._SslState.FinishWrite();
					if (count == 0)
					{
						goto IL_019B;
					}
				}
				return;
			}
			IL_019B:
			if (asyncRequest != null)
			{
				asyncRequest.CompleteUser();
			}
			if (buffer == this._PinnableOutputBufferInUse)
			{
				this._PinnableOutputBufferInUse = null;
				if (global::System.PinnableBufferCacheEventSource.Log.IsEnabled())
				{
					global::System.PinnableBufferCacheEventSource.Log.DebugMessage1("In System.Net._SslStream.StartWriting Freeing buffer.", (long)this.GetHashCode());
				}
			}
		}

		// Token: 0x06001FB6 RID: 8118 RVA: 0x0009451C File Offset: 0x0009271C
		private int ProcessRead(byte[] buffer, int offset, int count, AsyncProtocolRequest asyncRequest)
		{
			this.ValidateParameters(buffer, offset, count);
			if (Interlocked.Exchange(ref this._NestedRead, 1) == 1)
			{
				throw new NotSupportedException(SR.GetString("net_io_invalidnestedcall", new object[]
				{
					(asyncRequest != null) ? "BeginRead" : "Read",
					"read"
				}));
			}
			bool flag = false;
			int num2;
			try
			{
				if (this.InternalBufferCount != 0)
				{
					int num = ((this.InternalBufferCount > count) ? count : this.InternalBufferCount);
					if (num != 0)
					{
						Buffer.BlockCopy(this.InternalBuffer, this.InternalOffset, buffer, offset, num);
						this.DecrementInternalBufferCount(num);
					}
					if (asyncRequest != null)
					{
						asyncRequest.CompleteUser(num);
					}
					num2 = num;
				}
				else
				{
					num2 = this.StartReading(buffer, offset, count, asyncRequest);
				}
			}
			catch (Exception ex)
			{
				this._SslState.FinishRead(null);
				flag = true;
				if (ex is IOException)
				{
					throw;
				}
				throw new IOException(SR.GetString("net_io_read"), ex);
			}
			finally
			{
				if (asyncRequest == null || flag)
				{
					this._NestedRead = 0;
				}
			}
			return num2;
		}

		// Token: 0x06001FB7 RID: 8119 RVA: 0x00094628 File Offset: 0x00092828
		private int StartReading(byte[] buffer, int offset, int count, AsyncProtocolRequest asyncRequest)
		{
			int num;
			for (;;)
			{
				if (asyncRequest != null)
				{
					asyncRequest.SetNextRequest(buffer, offset, count, _SslStream._ResumeAsyncReadCallback);
				}
				num = this._SslState.CheckEnqueueRead(buffer, offset, count, asyncRequest);
				if (num == 0)
				{
					break;
				}
				if (num != -1)
				{
					goto Block_2;
				}
				int num2;
				if ((num2 = this.StartFrameHeader(buffer, offset, count, asyncRequest)) != -1)
				{
					return num2;
				}
			}
			return 0;
			Block_2:
			if (asyncRequest != null)
			{
				asyncRequest.CompleteUser(num);
			}
			return num;
		}

		// Token: 0x06001FB8 RID: 8120 RVA: 0x00094688 File Offset: 0x00092888
		private int StartFrameHeader(byte[] buffer, int offset, int count, AsyncProtocolRequest asyncRequest)
		{
			this.EnsureInternalBufferSize(0, 5);
			int num;
			if (asyncRequest != null)
			{
				asyncRequest.SetNextRequest(this.InternalBuffer, 0, 5, _SslStream._ReadHeaderCallback);
				this._Reader.AsyncReadPacket(asyncRequest);
				if (!asyncRequest.MustCompleteSynchronously)
				{
					return 0;
				}
				num = asyncRequest.Result;
			}
			else
			{
				num = this._Reader.ReadPacket(this.InternalBuffer, 0, 5);
			}
			return this.StartFrameBody(num, buffer, offset, count, asyncRequest);
		}

		// Token: 0x06001FB9 RID: 8121 RVA: 0x000946FC File Offset: 0x000928FC
		private int StartFrameBody(int readBytes, byte[] buffer, int offset, int count, AsyncProtocolRequest asyncRequest)
		{
			if (readBytes == 0)
			{
				this.DecrementInternalBufferCount(this.InternalBufferCount);
				if (asyncRequest != null)
				{
					asyncRequest.CompleteUser(0);
				}
				return 0;
			}
			readBytes = this._SslState.GetRemainingFrameSize(this.InternalBuffer, readBytes);
			if (readBytes < 0)
			{
				throw new IOException(SR.GetString("net_frame_read_size"));
			}
			this.EnsureInternalBufferSize(5, readBytes);
			if (asyncRequest != null)
			{
				asyncRequest.SetNextRequest(this.InternalBuffer, 5, readBytes, _SslStream._ReadFrameCallback);
				this._Reader.AsyncReadPacket(asyncRequest);
				if (!asyncRequest.MustCompleteSynchronously)
				{
					return 0;
				}
				readBytes = asyncRequest.Result;
			}
			else
			{
				readBytes = this._Reader.ReadPacket(this.InternalBuffer, 5, readBytes);
			}
			return this.ProcessFrameBody(readBytes, buffer, offset, count, asyncRequest);
		}

		// Token: 0x06001FBA RID: 8122 RVA: 0x000947B8 File Offset: 0x000929B8
		private int ProcessFrameBody(int readBytes, byte[] buffer, int offset, int count, AsyncProtocolRequest asyncRequest)
		{
			if (readBytes == 0)
			{
				throw new IOException(SR.GetString("net_io_eof"));
			}
			readBytes += 5;
			int num = 0;
			SecurityStatus securityStatus = this._SslState.DecryptData(this.InternalBuffer, ref num, ref readBytes);
			if (securityStatus != SecurityStatus.OK)
			{
				byte[] array = null;
				if (readBytes != 0)
				{
					array = new byte[readBytes];
					Buffer.BlockCopy(this.InternalBuffer, num, array, 0, readBytes);
				}
				this.DecrementInternalBufferCount(this.InternalBufferCount);
				return this.ProcessReadErrorCode(securityStatus, buffer, offset, count, asyncRequest, array);
			}
			if (readBytes == 0 && count != 0)
			{
				this.DecrementInternalBufferCount(this.InternalBufferCount);
				return -1;
			}
			this.EnsureInternalBufferSize(0, num + readBytes);
			this.DecrementInternalBufferCount(num);
			if (readBytes > count)
			{
				readBytes = count;
			}
			Buffer.BlockCopy(this.InternalBuffer, this.InternalOffset, buffer, offset, readBytes);
			this.DecrementInternalBufferCount(readBytes);
			this._SslState.FinishRead(null);
			if (asyncRequest != null)
			{
				asyncRequest.CompleteUser(readBytes);
			}
			return readBytes;
		}

		// Token: 0x06001FBB RID: 8123 RVA: 0x00094898 File Offset: 0x00092A98
		private int ProcessReadErrorCode(SecurityStatus errorCode, byte[] buffer, int offset, int count, AsyncProtocolRequest asyncRequest, byte[] extraBuffer)
		{
			ProtocolToken protocolToken = new ProtocolToken(null, errorCode);
			if (protocolToken.Renegotiate)
			{
				this._SslState.ReplyOnReAuthentication(extraBuffer);
				return -1;
			}
			if (protocolToken.CloseConnection)
			{
				this._SslState.FinishRead(null);
				if (asyncRequest != null)
				{
					asyncRequest.CompleteUser(0);
				}
				return 0;
			}
			throw new IOException(SR.GetString("net_io_decrypt"), protocolToken.GetException());
		}

		// Token: 0x06001FBC RID: 8124 RVA: 0x00094900 File Offset: 0x00092B00
		private static void WriteCallback(IAsyncResult transportResult)
		{
			if (transportResult.CompletedSynchronously)
			{
				return;
			}
			AsyncProtocolRequest asyncProtocolRequest = (AsyncProtocolRequest)transportResult.AsyncState;
			_SslStream sslStream = (_SslStream)asyncProtocolRequest.AsyncObject;
			try
			{
				sslStream._SslState.InnerStream.EndWrite(transportResult);
				sslStream._SslState.FinishWrite();
				if (asyncProtocolRequest.Count == 0)
				{
					asyncProtocolRequest.Count = -1;
				}
				sslStream.StartWriting(asyncProtocolRequest.Buffer, asyncProtocolRequest.Offset, asyncProtocolRequest.Count, asyncProtocolRequest);
			}
			catch (Exception ex)
			{
				if (asyncProtocolRequest.IsUserCompleted)
				{
					throw;
				}
				sslStream._SslState.FinishWrite();
				asyncProtocolRequest.CompleteWithError(ex);
			}
		}

		// Token: 0x06001FBD RID: 8125 RVA: 0x000949A4 File Offset: 0x00092BA4
		private static void MulitpleWriteCallback(IAsyncResult transportResult)
		{
			if (transportResult.CompletedSynchronously)
			{
				return;
			}
			_SslStream.SplitWriteAsyncProtocolRequest splitWriteAsyncProtocolRequest = (_SslStream.SplitWriteAsyncProtocolRequest)transportResult.AsyncState;
			_SslStream sslStream = (_SslStream)splitWriteAsyncProtocolRequest.AsyncObject;
			try
			{
				((NetworkStream)sslStream._SslState.InnerStream).EndMultipleWrite(transportResult);
				sslStream._SslState.FinishWrite();
				sslStream.StartWriting(splitWriteAsyncProtocolRequest.SplitWritesState, splitWriteAsyncProtocolRequest);
			}
			catch (Exception ex)
			{
				if (splitWriteAsyncProtocolRequest.IsUserCompleted)
				{
					throw;
				}
				sslStream._SslState.FinishWrite();
				splitWriteAsyncProtocolRequest.CompleteWithError(ex);
			}
		}

		// Token: 0x06001FBE RID: 8126 RVA: 0x00094A34 File Offset: 0x00092C34
		private static void ResumeAsyncReadCallback(AsyncProtocolRequest request)
		{
			try
			{
				((_SslStream)request.AsyncObject).StartReading(request.Buffer, request.Offset, request.Count, request);
			}
			catch (Exception ex)
			{
				if (request.IsUserCompleted)
				{
					throw;
				}
				((_SslStream)request.AsyncObject)._SslState.FinishRead(null);
				request.CompleteWithError(ex);
			}
		}

		// Token: 0x06001FBF RID: 8127 RVA: 0x00094AA4 File Offset: 0x00092CA4
		private static void ResumeAsyncWriteCallback(AsyncProtocolRequest asyncRequest)
		{
			try
			{
				_SslStream.SplitWriteAsyncProtocolRequest splitWriteAsyncProtocolRequest = asyncRequest as _SslStream.SplitWriteAsyncProtocolRequest;
				if (splitWriteAsyncProtocolRequest != null)
				{
					((_SslStream)asyncRequest.AsyncObject).StartWriting(splitWriteAsyncProtocolRequest.SplitWritesState, splitWriteAsyncProtocolRequest);
				}
				else
				{
					((_SslStream)asyncRequest.AsyncObject).StartWriting(asyncRequest.Buffer, asyncRequest.Offset, asyncRequest.Count, asyncRequest);
				}
			}
			catch (Exception ex)
			{
				if (asyncRequest.IsUserCompleted)
				{
					throw;
				}
				((_SslStream)asyncRequest.AsyncObject)._SslState.FinishWrite();
				asyncRequest.CompleteWithError(ex);
			}
		}

		// Token: 0x06001FC0 RID: 8128 RVA: 0x00094B34 File Offset: 0x00092D34
		private static void ReadHeaderCallback(AsyncProtocolRequest asyncRequest)
		{
			try
			{
				_SslStream sslStream = (_SslStream)asyncRequest.AsyncObject;
				BufferAsyncResult bufferAsyncResult = (BufferAsyncResult)asyncRequest.UserAsyncResult;
				if (-1 == sslStream.StartFrameBody(asyncRequest.Result, bufferAsyncResult.Buffer, bufferAsyncResult.Offset, bufferAsyncResult.Count, asyncRequest))
				{
					sslStream.StartReading(bufferAsyncResult.Buffer, bufferAsyncResult.Offset, bufferAsyncResult.Count, asyncRequest);
				}
			}
			catch (Exception ex)
			{
				if (asyncRequest.IsUserCompleted)
				{
					throw;
				}
				asyncRequest.CompleteWithError(ex);
			}
		}

		// Token: 0x06001FC1 RID: 8129 RVA: 0x00094BBC File Offset: 0x00092DBC
		private static void ReadFrameCallback(AsyncProtocolRequest asyncRequest)
		{
			try
			{
				_SslStream sslStream = (_SslStream)asyncRequest.AsyncObject;
				BufferAsyncResult bufferAsyncResult = (BufferAsyncResult)asyncRequest.UserAsyncResult;
				if (-1 == sslStream.ProcessFrameBody(asyncRequest.Result, bufferAsyncResult.Buffer, bufferAsyncResult.Offset, bufferAsyncResult.Count, asyncRequest))
				{
					sslStream.StartReading(bufferAsyncResult.Buffer, bufferAsyncResult.Offset, bufferAsyncResult.Count, asyncRequest);
				}
			}
			catch (Exception ex)
			{
				if (asyncRequest.IsUserCompleted)
				{
					throw;
				}
				asyncRequest.CompleteWithError(ex);
			}
		}

		// Token: 0x04001D3D RID: 7485
		private static AsyncCallback _WriteCallback = new AsyncCallback(_SslStream.WriteCallback);

		// Token: 0x04001D3E RID: 7486
		private static AsyncCallback _MulitpleWriteCallback = new AsyncCallback(_SslStream.MulitpleWriteCallback);

		// Token: 0x04001D3F RID: 7487
		private static AsyncProtocolCallback _ResumeAsyncWriteCallback = new AsyncProtocolCallback(_SslStream.ResumeAsyncWriteCallback);

		// Token: 0x04001D40 RID: 7488
		private static AsyncProtocolCallback _ResumeAsyncReadCallback = new AsyncProtocolCallback(_SslStream.ResumeAsyncReadCallback);

		// Token: 0x04001D41 RID: 7489
		private static AsyncProtocolCallback _ReadHeaderCallback = new AsyncProtocolCallback(_SslStream.ReadHeaderCallback);

		// Token: 0x04001D42 RID: 7490
		private static AsyncProtocolCallback _ReadFrameCallback = new AsyncProtocolCallback(_SslStream.ReadFrameCallback);

		// Token: 0x04001D43 RID: 7491
		private const int PinnableReadBufferSize = 16416;

		// Token: 0x04001D44 RID: 7492
		private static global::System.PinnableBufferCache s_PinnableReadBufferCache = new global::System.PinnableBufferCache("System.Net.SslStream", 16416);

		// Token: 0x04001D45 RID: 7493
		private const int PinnableWriteBufferSize = 5120;

		// Token: 0x04001D46 RID: 7494
		private static global::System.PinnableBufferCache s_PinnableWriteBufferCache = new global::System.PinnableBufferCache("System.Net.SslStream", 5120);

		// Token: 0x04001D47 RID: 7495
		private SslState _SslState;

		// Token: 0x04001D48 RID: 7496
		private int _NestedWrite;

		// Token: 0x04001D49 RID: 7497
		private int _NestedRead;

		// Token: 0x04001D4A RID: 7498
		private byte[] _InternalBuffer;

		// Token: 0x04001D4B RID: 7499
		private bool _InternalBufferFromPinnableCache;

		// Token: 0x04001D4C RID: 7500
		private byte[] _PinnableOutputBuffer;

		// Token: 0x04001D4D RID: 7501
		private byte[] _PinnableOutputBufferInUse;

		// Token: 0x04001D4E RID: 7502
		private int _InternalOffset;

		// Token: 0x04001D4F RID: 7503
		private int _InternalBufferCount;

		// Token: 0x04001D50 RID: 7504
		private FixedSizeReader _Reader;

		// Token: 0x020007D6 RID: 2006
		private class SplitWriteAsyncProtocolRequest : AsyncProtocolRequest
		{
			// Token: 0x060043AB RID: 17323 RVA: 0x0011D21E File Offset: 0x0011B41E
			internal SplitWriteAsyncProtocolRequest(LazyAsyncResult userAsyncResult)
				: base(userAsyncResult)
			{
			}

			// Token: 0x060043AC RID: 17324 RVA: 0x0011D227 File Offset: 0x0011B427
			internal void SetNextRequest(SplitWritesState splitWritesState, AsyncProtocolCallback callback)
			{
				this.SplitWritesState = splitWritesState;
				base.SetNextRequest(null, 0, 0, callback);
			}

			// Token: 0x040034A4 RID: 13476
			internal SplitWritesState SplitWritesState;
		}
	}
}
