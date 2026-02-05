using System;
using System.Globalization;
using System.IO;
using System.Net.Sockets;
using System.Threading;

namespace System.Net
{
	// Token: 0x02000219 RID: 537
	internal class StreamFramer
	{
		// Token: 0x060013C3 RID: 5059 RVA: 0x000688C4 File Offset: 0x00066AC4
		public StreamFramer(Stream Transport)
		{
			if (Transport == null || Transport == Stream.Null)
			{
				throw new ArgumentNullException("Transport");
			}
			this.m_Transport = Transport;
			if (this.m_Transport.GetType() == typeof(NetworkStream))
			{
				this.m_NetworkStream = Transport as NetworkStream;
			}
			this.m_ReadHeaderBuffer = new byte[this.m_CurReadHeader.Size];
			this.m_WriteHeaderBuffer = new byte[this.m_WriteHeader.Size];
			this.m_ReadFrameCallback = new AsyncCallback(this.ReadFrameCallback);
			this.m_BeginWriteCallback = new AsyncCallback(this.BeginWriteCallback);
		}

		// Token: 0x17000426 RID: 1062
		// (get) Token: 0x060013C4 RID: 5060 RVA: 0x00068990 File Offset: 0x00066B90
		public FrameHeader ReadHeader
		{
			get
			{
				return this.m_CurReadHeader;
			}
		}

		// Token: 0x17000427 RID: 1063
		// (get) Token: 0x060013C5 RID: 5061 RVA: 0x00068998 File Offset: 0x00066B98
		public FrameHeader WriteHeader
		{
			get
			{
				return this.m_WriteHeader;
			}
		}

		// Token: 0x17000428 RID: 1064
		// (get) Token: 0x060013C6 RID: 5062 RVA: 0x000689A0 File Offset: 0x00066BA0
		public Stream Transport
		{
			get
			{
				return this.m_Transport;
			}
		}

		// Token: 0x060013C7 RID: 5063 RVA: 0x000689A8 File Offset: 0x00066BA8
		public byte[] ReadMessage()
		{
			if (this.m_Eof)
			{
				return null;
			}
			int i = 0;
			byte[] array = this.m_ReadHeaderBuffer;
			int num;
			while (i < array.Length)
			{
				num = this.Transport.Read(array, i, array.Length - i);
				if (num == 0)
				{
					if (i == 0)
					{
						this.m_Eof = true;
						return null;
					}
					throw new IOException(SR.GetString("net_io_readfailure", new object[] { SR.GetString("net_io_connectionclosed") }));
				}
				else
				{
					i += num;
				}
			}
			this.m_CurReadHeader.CopyFrom(array, 0, this.m_ReadVerifier);
			if (this.m_CurReadHeader.PayloadSize > this.m_CurReadHeader.MaxMessageSize)
			{
				throw new InvalidOperationException(SR.GetString("net_frame_size", new object[]
				{
					this.m_CurReadHeader.MaxMessageSize.ToString(NumberFormatInfo.InvariantInfo),
					this.m_CurReadHeader.PayloadSize.ToString(NumberFormatInfo.InvariantInfo)
				}));
			}
			array = new byte[this.m_CurReadHeader.PayloadSize];
			for (i = 0; i < array.Length; i += num)
			{
				num = this.Transport.Read(array, i, array.Length - i);
				if (num == 0)
				{
					throw new IOException(SR.GetString("net_io_readfailure", new object[] { SR.GetString("net_io_connectionclosed") }));
				}
			}
			return array;
		}

		// Token: 0x060013C8 RID: 5064 RVA: 0x00068AEC File Offset: 0x00066CEC
		public IAsyncResult BeginReadMessage(AsyncCallback asyncCallback, object stateObject)
		{
			WorkerAsyncResult workerAsyncResult;
			if (this.m_Eof)
			{
				workerAsyncResult = new WorkerAsyncResult(this, stateObject, asyncCallback, null, 0, 0);
				workerAsyncResult.InvokeCallback(-1);
				return workerAsyncResult;
			}
			workerAsyncResult = new WorkerAsyncResult(this, stateObject, asyncCallback, this.m_ReadHeaderBuffer, 0, this.m_ReadHeaderBuffer.Length);
			IAsyncResult asyncResult = this.Transport.BeginRead(this.m_ReadHeaderBuffer, 0, this.m_ReadHeaderBuffer.Length, this.m_ReadFrameCallback, workerAsyncResult);
			if (asyncResult.CompletedSynchronously)
			{
				this.ReadFrameComplete(asyncResult);
			}
			return workerAsyncResult;
		}

		// Token: 0x060013C9 RID: 5065 RVA: 0x00068B68 File Offset: 0x00066D68
		private void ReadFrameCallback(IAsyncResult transportResult)
		{
			if (transportResult.CompletedSynchronously)
			{
				return;
			}
			WorkerAsyncResult workerAsyncResult = (WorkerAsyncResult)transportResult.AsyncState;
			try
			{
				this.ReadFrameComplete(transportResult);
			}
			catch (Exception ex)
			{
				if (ex is ThreadAbortException || ex is StackOverflowException || ex is OutOfMemoryException)
				{
					throw;
				}
				if (!(ex is IOException))
				{
					ex = new IOException(SR.GetString("net_io_readfailure", new object[] { ex.Message }), ex);
				}
				workerAsyncResult.InvokeCallback(ex);
			}
		}

		// Token: 0x060013CA RID: 5066 RVA: 0x00068BF0 File Offset: 0x00066DF0
		private void ReadFrameComplete(IAsyncResult transportResult)
		{
			WorkerAsyncResult workerAsyncResult;
			int payloadSize;
			for (;;)
			{
				workerAsyncResult = (WorkerAsyncResult)transportResult.AsyncState;
				int num = this.Transport.EndRead(transportResult);
				workerAsyncResult.Offset += num;
				if (num <= 0)
				{
					break;
				}
				if (workerAsyncResult.Offset >= workerAsyncResult.End)
				{
					if (workerAsyncResult.HeaderDone)
					{
						goto IL_0140;
					}
					workerAsyncResult.HeaderDone = true;
					this.m_CurReadHeader.CopyFrom(workerAsyncResult.Buffer, 0, this.m_ReadVerifier);
					payloadSize = this.m_CurReadHeader.PayloadSize;
					if (payloadSize < 0)
					{
						workerAsyncResult.InvokeCallback(new IOException(SR.GetString("net_frame_read_size")));
					}
					if (payloadSize == 0)
					{
						goto Block_6;
					}
					if (payloadSize > this.m_CurReadHeader.MaxMessageSize)
					{
						goto Block_7;
					}
					byte[] array = new byte[payloadSize];
					workerAsyncResult.Buffer = array;
					workerAsyncResult.End = array.Length;
					workerAsyncResult.Offset = 0;
				}
				transportResult = this.Transport.BeginRead(workerAsyncResult.Buffer, workerAsyncResult.Offset, workerAsyncResult.End - workerAsyncResult.Offset, this.m_ReadFrameCallback, workerAsyncResult);
				if (!transportResult.CompletedSynchronously)
				{
					return;
				}
			}
			object obj;
			if (!workerAsyncResult.HeaderDone && workerAsyncResult.Offset == 0)
			{
				obj = -1;
			}
			else
			{
				obj = new IOException(SR.GetString("net_frame_read_io"));
			}
			workerAsyncResult.InvokeCallback(obj);
			return;
			Block_6:
			workerAsyncResult.InvokeCallback(0);
			return;
			Block_7:
			throw new InvalidOperationException(SR.GetString("net_frame_size", new object[]
			{
				this.m_CurReadHeader.MaxMessageSize.ToString(NumberFormatInfo.InvariantInfo),
				payloadSize.ToString(NumberFormatInfo.InvariantInfo)
			}));
			IL_0140:
			workerAsyncResult.HeaderDone = false;
			workerAsyncResult.InvokeCallback(workerAsyncResult.End);
		}

		// Token: 0x060013CB RID: 5067 RVA: 0x00068D90 File Offset: 0x00066F90
		public byte[] EndReadMessage(IAsyncResult asyncResult)
		{
			if (asyncResult == null)
			{
				throw new ArgumentNullException("asyncResult");
			}
			WorkerAsyncResult workerAsyncResult = asyncResult as WorkerAsyncResult;
			if (workerAsyncResult == null)
			{
				throw new ArgumentException(SR.GetString("net_io_async_result", new object[] { typeof(WorkerAsyncResult).FullName }), "asyncResult");
			}
			if (!workerAsyncResult.InternalPeekCompleted)
			{
				workerAsyncResult.InternalWaitForCompletion();
			}
			if (workerAsyncResult.Result is Exception)
			{
				throw (Exception)workerAsyncResult.Result;
			}
			int num = (int)workerAsyncResult.Result;
			if (num == -1)
			{
				this.m_Eof = true;
				return null;
			}
			if (num == 0)
			{
				return new byte[0];
			}
			return workerAsyncResult.Buffer;
		}

		// Token: 0x060013CC RID: 5068 RVA: 0x00068E34 File Offset: 0x00067034
		public void WriteMessage(byte[] message)
		{
			if (message == null)
			{
				throw new ArgumentNullException("message");
			}
			this.m_WriteHeader.PayloadSize = message.Length;
			this.m_WriteHeader.CopyTo(this.m_WriteHeaderBuffer, 0);
			if (this.m_NetworkStream != null && message.Length != 0)
			{
				BufferOffsetSize[] array = new BufferOffsetSize[]
				{
					new BufferOffsetSize(this.m_WriteHeaderBuffer, 0, this.m_WriteHeaderBuffer.Length, false),
					new BufferOffsetSize(message, 0, message.Length, false)
				};
				this.m_NetworkStream.MultipleWrite(array);
				return;
			}
			this.Transport.Write(this.m_WriteHeaderBuffer, 0, this.m_WriteHeaderBuffer.Length);
			if (message.Length == 0)
			{
				return;
			}
			this.Transport.Write(message, 0, message.Length);
		}

		// Token: 0x060013CD RID: 5069 RVA: 0x00068EE4 File Offset: 0x000670E4
		public IAsyncResult BeginWriteMessage(byte[] message, AsyncCallback asyncCallback, object stateObject)
		{
			if (message == null)
			{
				throw new ArgumentNullException("message");
			}
			this.m_WriteHeader.PayloadSize = message.Length;
			this.m_WriteHeader.CopyTo(this.m_WriteHeaderBuffer, 0);
			if (this.m_NetworkStream != null && message.Length != 0)
			{
				BufferOffsetSize[] array = new BufferOffsetSize[]
				{
					new BufferOffsetSize(this.m_WriteHeaderBuffer, 0, this.m_WriteHeaderBuffer.Length, false),
					new BufferOffsetSize(message, 0, message.Length, false)
				};
				return this.m_NetworkStream.BeginMultipleWrite(array, asyncCallback, stateObject);
			}
			if (message.Length == 0)
			{
				return this.Transport.BeginWrite(this.m_WriteHeaderBuffer, 0, this.m_WriteHeaderBuffer.Length, asyncCallback, stateObject);
			}
			WorkerAsyncResult workerAsyncResult = new WorkerAsyncResult(this, stateObject, asyncCallback, message, 0, message.Length);
			IAsyncResult asyncResult = this.Transport.BeginWrite(this.m_WriteHeaderBuffer, 0, this.m_WriteHeaderBuffer.Length, this.m_BeginWriteCallback, workerAsyncResult);
			if (asyncResult.CompletedSynchronously)
			{
				this.BeginWriteComplete(asyncResult);
			}
			return workerAsyncResult;
		}

		// Token: 0x060013CE RID: 5070 RVA: 0x00068FC8 File Offset: 0x000671C8
		private void BeginWriteCallback(IAsyncResult transportResult)
		{
			if (transportResult.CompletedSynchronously)
			{
				return;
			}
			WorkerAsyncResult workerAsyncResult = (WorkerAsyncResult)transportResult.AsyncState;
			try
			{
				this.BeginWriteComplete(transportResult);
			}
			catch (Exception ex)
			{
				if (ex is ThreadAbortException || ex is StackOverflowException || ex is OutOfMemoryException)
				{
					throw;
				}
				workerAsyncResult.InvokeCallback(ex);
			}
		}

		// Token: 0x060013CF RID: 5071 RVA: 0x00069028 File Offset: 0x00067228
		private void BeginWriteComplete(IAsyncResult transportResult)
		{
			WorkerAsyncResult workerAsyncResult;
			for (;;)
			{
				workerAsyncResult = (WorkerAsyncResult)transportResult.AsyncState;
				this.Transport.EndWrite(transportResult);
				if (workerAsyncResult.Offset == workerAsyncResult.End)
				{
					break;
				}
				workerAsyncResult.Offset = workerAsyncResult.End;
				transportResult = this.Transport.BeginWrite(workerAsyncResult.Buffer, 0, workerAsyncResult.End, this.m_BeginWriteCallback, workerAsyncResult);
				if (!transportResult.CompletedSynchronously)
				{
					return;
				}
			}
			workerAsyncResult.InvokeCallback();
		}

		// Token: 0x060013D0 RID: 5072 RVA: 0x00069098 File Offset: 0x00067298
		public void EndWriteMessage(IAsyncResult asyncResult)
		{
			if (asyncResult == null)
			{
				throw new ArgumentNullException("asyncResult");
			}
			WorkerAsyncResult workerAsyncResult = asyncResult as WorkerAsyncResult;
			if (workerAsyncResult != null)
			{
				if (!workerAsyncResult.InternalPeekCompleted)
				{
					workerAsyncResult.InternalWaitForCompletion();
				}
				if (workerAsyncResult.Result is Exception)
				{
					throw (Exception)workerAsyncResult.Result;
				}
			}
			else
			{
				this.Transport.EndWrite(asyncResult);
			}
		}

		// Token: 0x040015C7 RID: 5575
		private Stream m_Transport;

		// Token: 0x040015C8 RID: 5576
		private bool m_Eof;

		// Token: 0x040015C9 RID: 5577
		private FrameHeader m_WriteHeader = new FrameHeader();

		// Token: 0x040015CA RID: 5578
		private FrameHeader m_CurReadHeader = new FrameHeader();

		// Token: 0x040015CB RID: 5579
		private FrameHeader m_ReadVerifier = new FrameHeader(-1, -1, -1);

		// Token: 0x040015CC RID: 5580
		private byte[] m_ReadHeaderBuffer;

		// Token: 0x040015CD RID: 5581
		private byte[] m_WriteHeaderBuffer;

		// Token: 0x040015CE RID: 5582
		private readonly AsyncCallback m_ReadFrameCallback;

		// Token: 0x040015CF RID: 5583
		private readonly AsyncCallback m_BeginWriteCallback;

		// Token: 0x040015D0 RID: 5584
		private NetworkStream m_NetworkStream;
	}
}
