using System;
using System.Threading;

namespace System.Net
{
	// Token: 0x020001BD RID: 445
	internal class ListenerAsyncResult : LazyAsyncResult
	{
		// Token: 0x170003CC RID: 972
		// (get) Token: 0x0600116C RID: 4460 RVA: 0x0005E9CD File Offset: 0x0005CBCD
		internal static IOCompletionCallback IOCallback
		{
			get
			{
				return ListenerAsyncResult.s_IOCallback;
			}
		}

		// Token: 0x0600116D RID: 4461 RVA: 0x0005E9D4 File Offset: 0x0005CBD4
		internal ListenerAsyncResult(object asyncObject, object userState, AsyncCallback callback)
			: base(asyncObject, userState, callback)
		{
			this.m_RequestContext = new AsyncRequestContext(this);
		}

		// Token: 0x0600116E RID: 4462 RVA: 0x0005E9EC File Offset: 0x0005CBEC
		private unsafe static void IOCompleted(ListenerAsyncResult asyncResult, uint errorCode, uint numBytes)
		{
			object obj = null;
			try
			{
				if (errorCode != 0U && errorCode != 234U)
				{
					asyncResult.ErrorCode = (int)errorCode;
					obj = new HttpListenerException((int)errorCode);
				}
				else
				{
					HttpListener httpListener = asyncResult.AsyncObject as HttpListener;
					if (errorCode == 0U)
					{
						bool flag = false;
						try
						{
							if (httpListener.ValidateRequest(asyncResult.m_RequestContext))
							{
								obj = httpListener.HandleAuthentication(asyncResult.m_RequestContext, out flag);
							}
							goto IL_0092;
						}
						finally
						{
							if (flag)
							{
								asyncResult.m_RequestContext = ((obj == null) ? new AsyncRequestContext(asyncResult) : null);
							}
							else
							{
								asyncResult.m_RequestContext.Reset(0UL, 0U);
							}
						}
					}
					asyncResult.m_RequestContext.Reset(asyncResult.m_RequestContext.RequestBlob->RequestId, numBytes);
					IL_0092:
					if (obj == null)
					{
						uint num = asyncResult.QueueBeginGetContext();
						if (num != 0U && num != 997U)
						{
							obj = new HttpListenerException((int)num);
						}
					}
					if (obj == null)
					{
						return;
					}
				}
			}
			catch (Exception ex)
			{
				if (NclUtilities.IsFatal(ex))
				{
					throw;
				}
				if (Logging.On)
				{
					Logging.PrintError(Logging.HttpListener, ValidationHelper.HashString(asyncResult), "IOCompleted", ex.ToString());
				}
				obj = ex;
			}
			asyncResult.InvokeCallback(obj);
		}

		// Token: 0x0600116F RID: 4463 RVA: 0x0005EB08 File Offset: 0x0005CD08
		private unsafe static void WaitCallback(uint errorCode, uint numBytes, NativeOverlapped* nativeOverlapped)
		{
			Overlapped overlapped = Overlapped.Unpack(nativeOverlapped);
			ListenerAsyncResult listenerAsyncResult = (ListenerAsyncResult)overlapped.AsyncResult;
			ListenerAsyncResult.IOCompleted(listenerAsyncResult, errorCode, numBytes);
		}

		// Token: 0x06001170 RID: 4464 RVA: 0x0005EB30 File Offset: 0x0005CD30
		internal unsafe uint QueueBeginGetContext()
		{
			uint num;
			uint num2;
			for (;;)
			{
				(base.AsyncObject as HttpListener).EnsureBoundHandle();
				num = 0U;
				num2 = UnsafeNclNativeMethods.HttpApi.HttpReceiveHttpRequest((base.AsyncObject as HttpListener).RequestQueueHandle, this.m_RequestContext.RequestBlob->RequestId, 1U, this.m_RequestContext.RequestBlob, this.m_RequestContext.Size, &num, this.m_RequestContext.NativeOverlapped);
				if (num2 == 87U && this.m_RequestContext.RequestBlob->RequestId != 0UL)
				{
					this.m_RequestContext.RequestBlob->RequestId = 0UL;
				}
				else
				{
					if (num2 != 234U)
					{
						break;
					}
					this.m_RequestContext.Reset(this.m_RequestContext.RequestBlob->RequestId, num);
				}
			}
			if (num2 == 0U && HttpListener.SkipIOCPCallbackOnSuccess)
			{
				ListenerAsyncResult.IOCompleted(this, num2, num);
			}
			return num2;
		}

		// Token: 0x06001171 RID: 4465 RVA: 0x0005EC06 File Offset: 0x0005CE06
		protected override void Cleanup()
		{
			if (this.m_RequestContext != null)
			{
				this.m_RequestContext.ReleasePins();
				this.m_RequestContext.Close();
			}
			base.Cleanup();
		}

		// Token: 0x04001450 RID: 5200
		private static readonly IOCompletionCallback s_IOCallback = new IOCompletionCallback(ListenerAsyncResult.WaitCallback);

		// Token: 0x04001451 RID: 5201
		private AsyncRequestContext m_RequestContext;
	}
}
