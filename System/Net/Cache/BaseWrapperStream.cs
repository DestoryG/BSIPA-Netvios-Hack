using System;
using System.IO;

namespace System.Net.Cache
{
	// Token: 0x0200031A RID: 794
	internal abstract class BaseWrapperStream : Stream, IRequestLifetimeTracker
	{
		// Token: 0x170006F3 RID: 1779
		// (get) Token: 0x06001C49 RID: 7241 RVA: 0x000867FE File Offset: 0x000849FE
		protected Stream WrappedStream
		{
			get
			{
				return this.m_WrappedStream;
			}
		}

		// Token: 0x06001C4A RID: 7242 RVA: 0x00086806 File Offset: 0x00084A06
		public BaseWrapperStream(Stream wrappedStream)
		{
			this.m_WrappedStream = wrappedStream;
		}

		// Token: 0x06001C4B RID: 7243 RVA: 0x00086818 File Offset: 0x00084A18
		public void TrackRequestLifetime(long requestStartTimestamp)
		{
			IRequestLifetimeTracker requestLifetimeTracker = this.m_WrappedStream as IRequestLifetimeTracker;
			requestLifetimeTracker.TrackRequestLifetime(requestStartTimestamp);
		}

		// Token: 0x04001B89 RID: 7049
		private Stream m_WrappedStream;
	}
}
