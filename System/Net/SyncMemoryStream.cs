using System;
using System.IO;

namespace System.Net
{
	// Token: 0x020001A7 RID: 423
	internal sealed class SyncMemoryStream : MemoryStream, IRequestLifetimeTracker
	{
		// Token: 0x060010A7 RID: 4263 RVA: 0x000599DC File Offset: 0x00057BDC
		internal SyncMemoryStream(byte[] bytes)
			: base(bytes, false)
		{
			this.m_ReadTimeout = (this.m_WriteTimeout = -1);
		}

		// Token: 0x060010A8 RID: 4264 RVA: 0x00059A04 File Offset: 0x00057C04
		internal SyncMemoryStream(int initialCapacity)
			: base(initialCapacity)
		{
			this.m_ReadTimeout = (this.m_WriteTimeout = -1);
		}

		// Token: 0x060010A9 RID: 4265 RVA: 0x00059A28 File Offset: 0x00057C28
		public override IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
		{
			int num = this.Read(buffer, offset, count);
			return new LazyAsyncResult(null, state, callback, num);
		}

		// Token: 0x060010AA RID: 4266 RVA: 0x00059A50 File Offset: 0x00057C50
		public override int EndRead(IAsyncResult asyncResult)
		{
			LazyAsyncResult lazyAsyncResult = (LazyAsyncResult)asyncResult;
			return (int)lazyAsyncResult.InternalWaitForCompletion();
		}

		// Token: 0x060010AB RID: 4267 RVA: 0x00059A6F File Offset: 0x00057C6F
		public override IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
		{
			this.Write(buffer, offset, count);
			return new LazyAsyncResult(null, state, callback, null);
		}

		// Token: 0x060010AC RID: 4268 RVA: 0x00059A88 File Offset: 0x00057C88
		public override void EndWrite(IAsyncResult asyncResult)
		{
			LazyAsyncResult lazyAsyncResult = (LazyAsyncResult)asyncResult;
			lazyAsyncResult.InternalWaitForCompletion();
		}

		// Token: 0x170003A1 RID: 929
		// (get) Token: 0x060010AD RID: 4269 RVA: 0x00059AA3 File Offset: 0x00057CA3
		public override bool CanTimeout
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170003A2 RID: 930
		// (get) Token: 0x060010AE RID: 4270 RVA: 0x00059AA6 File Offset: 0x00057CA6
		// (set) Token: 0x060010AF RID: 4271 RVA: 0x00059AAE File Offset: 0x00057CAE
		public override int ReadTimeout
		{
			get
			{
				return this.m_ReadTimeout;
			}
			set
			{
				this.m_ReadTimeout = value;
			}
		}

		// Token: 0x170003A3 RID: 931
		// (get) Token: 0x060010B0 RID: 4272 RVA: 0x00059AB7 File Offset: 0x00057CB7
		// (set) Token: 0x060010B1 RID: 4273 RVA: 0x00059ABF File Offset: 0x00057CBF
		public override int WriteTimeout
		{
			get
			{
				return this.m_WriteTimeout;
			}
			set
			{
				this.m_WriteTimeout = value;
			}
		}

		// Token: 0x060010B2 RID: 4274 RVA: 0x00059AC8 File Offset: 0x00057CC8
		public void TrackRequestLifetime(long requestStartTimestamp)
		{
			this.m_RequestLifetimeSetter = new RequestLifetimeSetter(requestStartTimestamp);
		}

		// Token: 0x060010B3 RID: 4275 RVA: 0x00059AD6 File Offset: 0x00057CD6
		protected override void Dispose(bool disposing)
		{
			if (this.m_Disposed)
			{
				return;
			}
			this.m_Disposed = true;
			if (disposing)
			{
				RequestLifetimeSetter.Report(this.m_RequestLifetimeSetter);
			}
			base.Dispose(disposing);
		}

		// Token: 0x0400139C RID: 5020
		private int m_ReadTimeout;

		// Token: 0x0400139D RID: 5021
		private int m_WriteTimeout;

		// Token: 0x0400139E RID: 5022
		private RequestLifetimeSetter m_RequestLifetimeSetter;

		// Token: 0x0400139F RID: 5023
		private bool m_Disposed;
	}
}
