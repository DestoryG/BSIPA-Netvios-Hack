using System;
using System.Runtime.InteropServices;
using System.Threading;

namespace System.Net
{
	// Token: 0x020001C0 RID: 448
	internal class HttpResponseStreamAsyncResult : LazyAsyncResult
	{
		// Token: 0x170003DC RID: 988
		// (get) Token: 0x060011A4 RID: 4516 RVA: 0x0005FDCC File Offset: 0x0005DFCC
		internal ushort dataChunkCount
		{
			get
			{
				if (this.m_DataChunks == null)
				{
					return 0;
				}
				return (ushort)this.m_DataChunks.Length;
			}
		}

		// Token: 0x170003DD RID: 989
		// (get) Token: 0x060011A5 RID: 4517 RVA: 0x0005FDE1 File Offset: 0x0005DFE1
		internal unsafe UnsafeNclNativeMethods.HttpApi.HTTP_DATA_CHUNK* pDataChunks
		{
			get
			{
				if (this.m_DataChunks == null)
				{
					return null;
				}
				return (UnsafeNclNativeMethods.HttpApi.HTTP_DATA_CHUNK*)(void*)Marshal.UnsafeAddrOfPinnedArrayElement(this.m_DataChunks, 0);
			}
		}

		// Token: 0x060011A6 RID: 4518 RVA: 0x0005FDFF File Offset: 0x0005DFFF
		internal HttpResponseStreamAsyncResult(object asyncObject, object userState, AsyncCallback callback)
			: base(asyncObject, userState, callback)
		{
		}

		// Token: 0x060011A7 RID: 4519 RVA: 0x0005FE0C File Offset: 0x0005E00C
		internal unsafe HttpResponseStreamAsyncResult(object asyncObject, object userState, AsyncCallback callback, byte[] buffer, int offset, int size, bool chunked, bool sentHeaders)
			: base(asyncObject, userState, callback)
		{
			this.m_SentHeaders = sentHeaders;
			Overlapped overlapped = new Overlapped();
			overlapped.AsyncResult = this;
			if (size == 0)
			{
				this.m_DataChunks = null;
				this.m_pOverlapped = overlapped.Pack(HttpResponseStreamAsyncResult.s_IOCallback, null);
				return;
			}
			this.m_DataChunks = new UnsafeNclNativeMethods.HttpApi.HTTP_DATA_CHUNK[chunked ? 3 : 1];
			object[] array = new object[1 + this.m_DataChunks.Length];
			array[this.m_DataChunks.Length] = this.m_DataChunks;
			int num = 0;
			byte[] array2 = null;
			if (chunked)
			{
				array2 = ConnectStream.GetChunkHeader(size, out num);
				this.m_DataChunks[0] = default(UnsafeNclNativeMethods.HttpApi.HTTP_DATA_CHUNK);
				this.m_DataChunks[0].DataChunkType = UnsafeNclNativeMethods.HttpApi.HTTP_DATA_CHUNK_TYPE.HttpDataChunkFromMemory;
				this.m_DataChunks[0].BufferLength = (uint)(array2.Length - num);
				array[0] = array2;
				this.m_DataChunks[1] = default(UnsafeNclNativeMethods.HttpApi.HTTP_DATA_CHUNK);
				this.m_DataChunks[1].DataChunkType = UnsafeNclNativeMethods.HttpApi.HTTP_DATA_CHUNK_TYPE.HttpDataChunkFromMemory;
				this.m_DataChunks[1].BufferLength = (uint)size;
				array[1] = buffer;
				this.m_DataChunks[2] = default(UnsafeNclNativeMethods.HttpApi.HTTP_DATA_CHUNK);
				this.m_DataChunks[2].DataChunkType = UnsafeNclNativeMethods.HttpApi.HTTP_DATA_CHUNK_TYPE.HttpDataChunkFromMemory;
				this.m_DataChunks[2].BufferLength = (uint)NclConstants.CRLF.Length;
				array[2] = NclConstants.CRLF;
			}
			else
			{
				this.m_DataChunks[0] = default(UnsafeNclNativeMethods.HttpApi.HTTP_DATA_CHUNK);
				this.m_DataChunks[0].DataChunkType = UnsafeNclNativeMethods.HttpApi.HTTP_DATA_CHUNK_TYPE.HttpDataChunkFromMemory;
				this.m_DataChunks[0].BufferLength = (uint)size;
				array[0] = buffer;
			}
			this.m_pOverlapped = overlapped.Pack(HttpResponseStreamAsyncResult.s_IOCallback, array);
			if (chunked)
			{
				this.m_DataChunks[0].pBuffer = (byte*)(void*)Marshal.UnsafeAddrOfPinnedArrayElement(array2, num);
				this.m_DataChunks[1].pBuffer = (byte*)(void*)Marshal.UnsafeAddrOfPinnedArrayElement(buffer, offset);
				this.m_DataChunks[2].pBuffer = (byte*)(void*)Marshal.UnsafeAddrOfPinnedArrayElement(NclConstants.CRLF, 0);
				return;
			}
			this.m_DataChunks[0].pBuffer = (byte*)(void*)Marshal.UnsafeAddrOfPinnedArrayElement(buffer, offset);
		}

		// Token: 0x060011A8 RID: 4520 RVA: 0x0006002C File Offset: 0x0005E22C
		internal void IOCompleted(uint errorCode, uint numBytes)
		{
			HttpResponseStreamAsyncResult.IOCompleted(this, errorCode, numBytes);
		}

		// Token: 0x060011A9 RID: 4521 RVA: 0x00060038 File Offset: 0x0005E238
		private unsafe static void IOCompleted(HttpResponseStreamAsyncResult asyncResult, uint errorCode, uint numBytes)
		{
			object obj = null;
			try
			{
				if (errorCode != 0U && errorCode != 38U)
				{
					asyncResult.ErrorCode = (int)errorCode;
					obj = new HttpListenerException((int)errorCode);
				}
				else if (asyncResult.m_DataChunks == null)
				{
					obj = 0U;
					if (Logging.On)
					{
						Logging.Dump(Logging.HttpListener, asyncResult, "Callback", IntPtr.Zero, 0);
					}
				}
				else
				{
					obj = ((asyncResult.m_DataChunks.Length == 1) ? asyncResult.m_DataChunks[0].BufferLength : 0U);
					if (Logging.On)
					{
						for (int i = 0; i < asyncResult.m_DataChunks.Length; i++)
						{
							Logging.Dump(Logging.HttpListener, asyncResult, "Callback", (IntPtr)((void*)asyncResult.m_DataChunks[0].pBuffer), (int)asyncResult.m_DataChunks[0].BufferLength);
						}
					}
				}
			}
			catch (Exception ex)
			{
				obj = ex;
			}
			asyncResult.InvokeCallback(obj);
		}

		// Token: 0x060011AA RID: 4522 RVA: 0x00060128 File Offset: 0x0005E328
		private unsafe static void Callback(uint errorCode, uint numBytes, NativeOverlapped* nativeOverlapped)
		{
			Overlapped overlapped = Overlapped.Unpack(nativeOverlapped);
			HttpResponseStreamAsyncResult httpResponseStreamAsyncResult = overlapped.AsyncResult as HttpResponseStreamAsyncResult;
			HttpResponseStreamAsyncResult.IOCompleted(httpResponseStreamAsyncResult, errorCode, numBytes);
		}

		// Token: 0x060011AB RID: 4523 RVA: 0x00060150 File Offset: 0x0005E350
		protected override void Cleanup()
		{
			base.Cleanup();
			if (this.m_pOverlapped != null)
			{
				Overlapped.Free(this.m_pOverlapped);
			}
		}

		// Token: 0x0400145D RID: 5213
		internal unsafe NativeOverlapped* m_pOverlapped;

		// Token: 0x0400145E RID: 5214
		private UnsafeNclNativeMethods.HttpApi.HTTP_DATA_CHUNK[] m_DataChunks;

		// Token: 0x0400145F RID: 5215
		internal bool m_SentHeaders;

		// Token: 0x04001460 RID: 5216
		private static readonly IOCompletionCallback s_IOCallback = new IOCompletionCallback(HttpResponseStreamAsyncResult.Callback);
	}
}
