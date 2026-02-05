using System;
using System.Diagnostics;
using System.Threading;

namespace System.Net
{
	// Token: 0x020001BC RID: 444
	internal class LazyAsyncResult : IAsyncResult
	{
		// Token: 0x170003C1 RID: 961
		// (get) Token: 0x06001150 RID: 4432 RVA: 0x0005E554 File Offset: 0x0005C754
		private static LazyAsyncResult.ThreadContext CurrentThreadContext
		{
			get
			{
				LazyAsyncResult.ThreadContext threadContext = LazyAsyncResult.t_ThreadContext;
				if (threadContext == null)
				{
					threadContext = new LazyAsyncResult.ThreadContext();
					LazyAsyncResult.t_ThreadContext = threadContext;
				}
				return threadContext;
			}
		}

		// Token: 0x06001151 RID: 4433 RVA: 0x0005E577 File Offset: 0x0005C777
		internal LazyAsyncResult(object myObject, object myState, AsyncCallback myCallBack)
		{
			this.m_AsyncObject = myObject;
			this.m_AsyncState = myState;
			this.m_AsyncCallback = myCallBack;
			this.m_Result = DBNull.Value;
		}

		// Token: 0x06001152 RID: 4434 RVA: 0x0005E59F File Offset: 0x0005C79F
		internal LazyAsyncResult(object myObject, object myState, AsyncCallback myCallBack, object result)
		{
			this.m_AsyncObject = myObject;
			this.m_AsyncState = myState;
			this.m_AsyncCallback = myCallBack;
			this.m_Result = result;
			this.m_IntCompleted = 1;
			if (this.m_AsyncCallback != null)
			{
				this.m_AsyncCallback(this);
			}
		}

		// Token: 0x170003C2 RID: 962
		// (get) Token: 0x06001153 RID: 4435 RVA: 0x0005E5DF File Offset: 0x0005C7DF
		internal object AsyncObject
		{
			get
			{
				return this.m_AsyncObject;
			}
		}

		// Token: 0x170003C3 RID: 963
		// (get) Token: 0x06001154 RID: 4436 RVA: 0x0005E5E7 File Offset: 0x0005C7E7
		public object AsyncState
		{
			get
			{
				return this.m_AsyncState;
			}
		}

		// Token: 0x170003C4 RID: 964
		// (get) Token: 0x06001155 RID: 4437 RVA: 0x0005E5EF File Offset: 0x0005C7EF
		// (set) Token: 0x06001156 RID: 4438 RVA: 0x0005E5F7 File Offset: 0x0005C7F7
		protected AsyncCallback AsyncCallback
		{
			get
			{
				return this.m_AsyncCallback;
			}
			set
			{
				this.m_AsyncCallback = value;
			}
		}

		// Token: 0x170003C5 RID: 965
		// (get) Token: 0x06001157 RID: 4439 RVA: 0x0005E600 File Offset: 0x0005C800
		public WaitHandle AsyncWaitHandle
		{
			get
			{
				this.m_UserEvent = true;
				if (this.m_IntCompleted == 0)
				{
					Interlocked.CompareExchange(ref this.m_IntCompleted, int.MinValue, 0);
				}
				ManualResetEvent manualResetEvent = (ManualResetEvent)this.m_Event;
				while (manualResetEvent == null)
				{
					this.LazilyCreateEvent(out manualResetEvent);
				}
				return manualResetEvent;
			}
		}

		// Token: 0x06001158 RID: 4440 RVA: 0x0005E64C File Offset: 0x0005C84C
		private bool LazilyCreateEvent(out ManualResetEvent waitHandle)
		{
			waitHandle = new ManualResetEvent(false);
			bool flag;
			try
			{
				if (Interlocked.CompareExchange(ref this.m_Event, waitHandle, null) == null)
				{
					if (this.InternalPeekCompleted)
					{
						waitHandle.Set();
					}
					flag = true;
				}
				else
				{
					waitHandle.Close();
					waitHandle = (ManualResetEvent)this.m_Event;
					flag = false;
				}
			}
			catch
			{
				this.m_Event = null;
				if (waitHandle != null)
				{
					waitHandle.Close();
				}
				throw;
			}
			return flag;
		}

		// Token: 0x06001159 RID: 4441 RVA: 0x0005E6C4 File Offset: 0x0005C8C4
		[Conditional("DEBUG")]
		protected void DebugProtectState(bool protect)
		{
		}

		// Token: 0x170003C6 RID: 966
		// (get) Token: 0x0600115A RID: 4442 RVA: 0x0005E6C8 File Offset: 0x0005C8C8
		public bool CompletedSynchronously
		{
			get
			{
				int num = this.m_IntCompleted;
				if (num == 0)
				{
					num = Interlocked.CompareExchange(ref this.m_IntCompleted, int.MinValue, 0);
				}
				return num > 0;
			}
		}

		// Token: 0x170003C7 RID: 967
		// (get) Token: 0x0600115B RID: 4443 RVA: 0x0005E6F8 File Offset: 0x0005C8F8
		public bool IsCompleted
		{
			get
			{
				int num = this.m_IntCompleted;
				if (num == 0)
				{
					num = Interlocked.CompareExchange(ref this.m_IntCompleted, int.MinValue, 0);
				}
				return (num & int.MaxValue) != 0;
			}
		}

		// Token: 0x170003C8 RID: 968
		// (get) Token: 0x0600115C RID: 4444 RVA: 0x0005E72B File Offset: 0x0005C92B
		internal bool InternalPeekCompleted
		{
			get
			{
				return (this.m_IntCompleted & int.MaxValue) != 0;
			}
		}

		// Token: 0x170003C9 RID: 969
		// (get) Token: 0x0600115D RID: 4445 RVA: 0x0005E73C File Offset: 0x0005C93C
		// (set) Token: 0x0600115E RID: 4446 RVA: 0x0005E753 File Offset: 0x0005C953
		internal object Result
		{
			get
			{
				if (this.m_Result != DBNull.Value)
				{
					return this.m_Result;
				}
				return null;
			}
			set
			{
				this.m_Result = value;
			}
		}

		// Token: 0x170003CA RID: 970
		// (get) Token: 0x0600115F RID: 4447 RVA: 0x0005E75C File Offset: 0x0005C95C
		// (set) Token: 0x06001160 RID: 4448 RVA: 0x0005E764 File Offset: 0x0005C964
		internal bool EndCalled
		{
			get
			{
				return this.m_EndCalled;
			}
			set
			{
				this.m_EndCalled = value;
			}
		}

		// Token: 0x170003CB RID: 971
		// (get) Token: 0x06001161 RID: 4449 RVA: 0x0005E76D File Offset: 0x0005C96D
		// (set) Token: 0x06001162 RID: 4450 RVA: 0x0005E775 File Offset: 0x0005C975
		internal int ErrorCode
		{
			get
			{
				return this.m_ErrorCode;
			}
			set
			{
				this.m_ErrorCode = value;
			}
		}

		// Token: 0x06001163 RID: 4451 RVA: 0x0005E780 File Offset: 0x0005C980
		protected void ProtectedInvokeCallback(object result, IntPtr userToken)
		{
			if (result == DBNull.Value)
			{
				throw new ArgumentNullException("result");
			}
			if ((this.m_IntCompleted & 2147483647) == 0 && (Interlocked.Increment(ref this.m_IntCompleted) & 2147483647) == 1)
			{
				if (this.m_Result == DBNull.Value)
				{
					this.m_Result = result;
				}
				ManualResetEvent manualResetEvent = (ManualResetEvent)this.m_Event;
				if (manualResetEvent != null)
				{
					try
					{
						manualResetEvent.Set();
					}
					catch (ObjectDisposedException)
					{
					}
				}
				this.Complete(userToken);
			}
		}

		// Token: 0x06001164 RID: 4452 RVA: 0x0005E808 File Offset: 0x0005CA08
		internal void InvokeCallback(object result)
		{
			this.ProtectedInvokeCallback(result, IntPtr.Zero);
		}

		// Token: 0x06001165 RID: 4453 RVA: 0x0005E816 File Offset: 0x0005CA16
		internal void InvokeCallback()
		{
			this.ProtectedInvokeCallback(null, IntPtr.Zero);
		}

		// Token: 0x06001166 RID: 4454 RVA: 0x0005E824 File Offset: 0x0005CA24
		protected virtual void Complete(IntPtr userToken)
		{
			bool flag = false;
			LazyAsyncResult.ThreadContext currentThreadContext = LazyAsyncResult.CurrentThreadContext;
			try
			{
				currentThreadContext.m_NestedIOCount++;
				if (this.m_AsyncCallback != null)
				{
					if (currentThreadContext.m_NestedIOCount >= 50)
					{
						ThreadPool.QueueUserWorkItem(new WaitCallback(this.WorkerThreadComplete));
						flag = true;
					}
					else
					{
						this.m_AsyncCallback(this);
					}
				}
			}
			finally
			{
				currentThreadContext.m_NestedIOCount--;
				if (!flag)
				{
					this.Cleanup();
				}
			}
		}

		// Token: 0x06001167 RID: 4455 RVA: 0x0005E8A8 File Offset: 0x0005CAA8
		private void WorkerThreadComplete(object state)
		{
			try
			{
				this.m_AsyncCallback(this);
			}
			finally
			{
				this.Cleanup();
			}
		}

		// Token: 0x06001168 RID: 4456 RVA: 0x0005E8DC File Offset: 0x0005CADC
		protected virtual void Cleanup()
		{
		}

		// Token: 0x06001169 RID: 4457 RVA: 0x0005E8DE File Offset: 0x0005CADE
		internal object InternalWaitForCompletion()
		{
			return this.WaitForCompletion(true);
		}

		// Token: 0x0600116A RID: 4458 RVA: 0x0005E8E8 File Offset: 0x0005CAE8
		private object WaitForCompletion(bool snap)
		{
			ManualResetEvent manualResetEvent = null;
			bool flag = false;
			if (!(snap ? this.IsCompleted : this.InternalPeekCompleted))
			{
				manualResetEvent = (ManualResetEvent)this.m_Event;
				if (manualResetEvent == null)
				{
					flag = this.LazilyCreateEvent(out manualResetEvent);
				}
			}
			if (manualResetEvent == null)
			{
				goto IL_0075;
			}
			try
			{
				manualResetEvent.WaitOne(-1, false);
				goto IL_0075;
			}
			catch (ObjectDisposedException)
			{
				goto IL_0075;
			}
			finally
			{
				if (flag && !this.m_UserEvent)
				{
					ManualResetEvent manualResetEvent2 = (ManualResetEvent)this.m_Event;
					this.m_Event = null;
					if (!this.m_UserEvent)
					{
						manualResetEvent2.Close();
					}
				}
			}
			IL_006F:
			Thread.SpinWait(1);
			IL_0075:
			if (this.m_Result != DBNull.Value)
			{
				return this.m_Result;
			}
			goto IL_006F;
		}

		// Token: 0x0600116B RID: 4459 RVA: 0x0005E99C File Offset: 0x0005CB9C
		internal void InternalCleanup()
		{
			if ((this.m_IntCompleted & 2147483647) == 0 && (Interlocked.Increment(ref this.m_IntCompleted) & 2147483647) == 1)
			{
				this.m_Result = null;
				this.Cleanup();
			}
		}

		// Token: 0x04001444 RID: 5188
		private const int c_HighBit = -2147483648;

		// Token: 0x04001445 RID: 5189
		private const int c_ForceAsyncCount = 50;

		// Token: 0x04001446 RID: 5190
		[ThreadStatic]
		private static LazyAsyncResult.ThreadContext t_ThreadContext;

		// Token: 0x04001447 RID: 5191
		private object m_AsyncObject;

		// Token: 0x04001448 RID: 5192
		private object m_AsyncState;

		// Token: 0x04001449 RID: 5193
		private AsyncCallback m_AsyncCallback;

		// Token: 0x0400144A RID: 5194
		private object m_Result;

		// Token: 0x0400144B RID: 5195
		private int m_ErrorCode;

		// Token: 0x0400144C RID: 5196
		private int m_IntCompleted;

		// Token: 0x0400144D RID: 5197
		private bool m_EndCalled;

		// Token: 0x0400144E RID: 5198
		private bool m_UserEvent;

		// Token: 0x0400144F RID: 5199
		private object m_Event;

		// Token: 0x02000750 RID: 1872
		private class ThreadContext
		{
			// Token: 0x040031F9 RID: 12793
			internal int m_NestedIOCount;
		}
	}
}
