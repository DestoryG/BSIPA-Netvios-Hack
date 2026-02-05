using System;
using System.Threading;

namespace System.IO.Compression
{
	// Token: 0x02000427 RID: 1063
	internal class DeflateStreamAsyncResult : IAsyncResult
	{
		// Token: 0x060027DA RID: 10202 RVA: 0x000B7322 File Offset: 0x000B5522
		public DeflateStreamAsyncResult(object asyncObject, object asyncState, AsyncCallback asyncCallback, byte[] buffer, int offset, int count)
		{
			this.buffer = buffer;
			this.offset = offset;
			this.count = count;
			this.m_CompletedSynchronously = true;
			this.m_AsyncObject = asyncObject;
			this.m_AsyncState = asyncState;
			this.m_AsyncCallback = asyncCallback;
		}

		// Token: 0x170009D2 RID: 2514
		// (get) Token: 0x060027DB RID: 10203 RVA: 0x000B735E File Offset: 0x000B555E
		public object AsyncState
		{
			get
			{
				return this.m_AsyncState;
			}
		}

		// Token: 0x170009D3 RID: 2515
		// (get) Token: 0x060027DC RID: 10204 RVA: 0x000B7368 File Offset: 0x000B5568
		public WaitHandle AsyncWaitHandle
		{
			get
			{
				int completed = this.m_Completed;
				if (this.m_Event == null)
				{
					Interlocked.CompareExchange(ref this.m_Event, new ManualResetEvent(completed != 0), null);
				}
				ManualResetEvent manualResetEvent = (ManualResetEvent)this.m_Event;
				if (completed == 0 && this.m_Completed != 0)
				{
					manualResetEvent.Set();
				}
				return manualResetEvent;
			}
		}

		// Token: 0x170009D4 RID: 2516
		// (get) Token: 0x060027DD RID: 10205 RVA: 0x000B73B9 File Offset: 0x000B55B9
		public bool CompletedSynchronously
		{
			get
			{
				return this.m_CompletedSynchronously;
			}
		}

		// Token: 0x170009D5 RID: 2517
		// (get) Token: 0x060027DE RID: 10206 RVA: 0x000B73C1 File Offset: 0x000B55C1
		public bool IsCompleted
		{
			get
			{
				return this.m_Completed != 0;
			}
		}

		// Token: 0x170009D6 RID: 2518
		// (get) Token: 0x060027DF RID: 10207 RVA: 0x000B73CC File Offset: 0x000B55CC
		internal object Result
		{
			get
			{
				return this.m_Result;
			}
		}

		// Token: 0x060027E0 RID: 10208 RVA: 0x000B73D4 File Offset: 0x000B55D4
		internal void Close()
		{
			if (this.m_Event != null)
			{
				((ManualResetEvent)this.m_Event).Close();
			}
		}

		// Token: 0x060027E1 RID: 10209 RVA: 0x000B73EE File Offset: 0x000B55EE
		internal void InvokeCallback(bool completedSynchronously, object result)
		{
			this.Complete(completedSynchronously, result);
		}

		// Token: 0x060027E2 RID: 10210 RVA: 0x000B73F8 File Offset: 0x000B55F8
		internal void InvokeCallback(object result)
		{
			this.Complete(result);
		}

		// Token: 0x060027E3 RID: 10211 RVA: 0x000B7401 File Offset: 0x000B5601
		private void Complete(bool completedSynchronously, object result)
		{
			this.m_CompletedSynchronously = completedSynchronously;
			this.Complete(result);
		}

		// Token: 0x060027E4 RID: 10212 RVA: 0x000B7414 File Offset: 0x000B5614
		private void Complete(object result)
		{
			this.m_Result = result;
			Interlocked.Increment(ref this.m_Completed);
			if (this.m_Event != null)
			{
				((ManualResetEvent)this.m_Event).Set();
			}
			if (Interlocked.Increment(ref this.m_InvokedCallback) == 1 && this.m_AsyncCallback != null)
			{
				this.m_AsyncCallback(this);
			}
		}

		// Token: 0x04002196 RID: 8598
		public byte[] buffer;

		// Token: 0x04002197 RID: 8599
		public int offset;

		// Token: 0x04002198 RID: 8600
		public int count;

		// Token: 0x04002199 RID: 8601
		public bool isWrite;

		// Token: 0x0400219A RID: 8602
		private object m_AsyncObject;

		// Token: 0x0400219B RID: 8603
		private object m_AsyncState;

		// Token: 0x0400219C RID: 8604
		private AsyncCallback m_AsyncCallback;

		// Token: 0x0400219D RID: 8605
		private object m_Result;

		// Token: 0x0400219E RID: 8606
		internal bool m_CompletedSynchronously;

		// Token: 0x0400219F RID: 8607
		private int m_InvokedCallback;

		// Token: 0x040021A0 RID: 8608
		private int m_Completed;

		// Token: 0x040021A1 RID: 8609
		private object m_Event;
	}
}
