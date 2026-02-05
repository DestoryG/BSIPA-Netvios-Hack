using System;
using System.Collections.Specialized;
using System.IO;
using System.Threading;

namespace System.Net.Cache
{
	// Token: 0x0200031D RID: 797
	internal class MetadataUpdateStream : BaseWrapperStream, ICloseEx
	{
		// Token: 0x06001C7F RID: 7295 RVA: 0x00087704 File Offset: 0x00085904
		internal MetadataUpdateStream(Stream parentStream, RequestCache cache, string key, DateTime expiresGMT, DateTime lastModifiedGMT, DateTime lastSynchronizedGMT, TimeSpan maxStale, StringCollection entryMetadata, StringCollection systemMetadata, bool isStrictCacheErrors)
			: base(parentStream)
		{
			this.m_Cache = cache;
			this.m_Key = key;
			this.m_Expires = expiresGMT;
			this.m_LastModified = lastModifiedGMT;
			this.m_LastSynchronized = lastSynchronizedGMT;
			this.m_MaxStale = maxStale;
			this.m_EntryMetadata = entryMetadata;
			this.m_SystemMetadata = systemMetadata;
			this.m_IsStrictCacheErrors = isStrictCacheErrors;
		}

		// Token: 0x06001C80 RID: 7296 RVA: 0x0008775E File Offset: 0x0008595E
		private MetadataUpdateStream(Stream parentStream, RequestCache cache, string key, bool isStrictCacheErrors)
			: base(parentStream)
		{
			this.m_Cache = cache;
			this.m_Key = key;
			this.m_CacheDestroy = true;
			this.m_IsStrictCacheErrors = isStrictCacheErrors;
		}

		// Token: 0x17000704 RID: 1796
		// (get) Token: 0x06001C81 RID: 7297 RVA: 0x00087784 File Offset: 0x00085984
		public override bool CanRead
		{
			get
			{
				return base.WrappedStream.CanRead;
			}
		}

		// Token: 0x17000705 RID: 1797
		// (get) Token: 0x06001C82 RID: 7298 RVA: 0x00087791 File Offset: 0x00085991
		public override bool CanSeek
		{
			get
			{
				return base.WrappedStream.CanSeek;
			}
		}

		// Token: 0x17000706 RID: 1798
		// (get) Token: 0x06001C83 RID: 7299 RVA: 0x0008779E File Offset: 0x0008599E
		public override bool CanWrite
		{
			get
			{
				return base.WrappedStream.CanWrite;
			}
		}

		// Token: 0x17000707 RID: 1799
		// (get) Token: 0x06001C84 RID: 7300 RVA: 0x000877AB File Offset: 0x000859AB
		public override long Length
		{
			get
			{
				return base.WrappedStream.Length;
			}
		}

		// Token: 0x17000708 RID: 1800
		// (get) Token: 0x06001C85 RID: 7301 RVA: 0x000877B8 File Offset: 0x000859B8
		// (set) Token: 0x06001C86 RID: 7302 RVA: 0x000877C5 File Offset: 0x000859C5
		public override long Position
		{
			get
			{
				return base.WrappedStream.Position;
			}
			set
			{
				base.WrappedStream.Position = value;
			}
		}

		// Token: 0x06001C87 RID: 7303 RVA: 0x000877D3 File Offset: 0x000859D3
		public override long Seek(long offset, SeekOrigin origin)
		{
			return base.WrappedStream.Seek(offset, origin);
		}

		// Token: 0x06001C88 RID: 7304 RVA: 0x000877E2 File Offset: 0x000859E2
		public override void SetLength(long value)
		{
			base.WrappedStream.SetLength(value);
		}

		// Token: 0x06001C89 RID: 7305 RVA: 0x000877F0 File Offset: 0x000859F0
		public override void Write(byte[] buffer, int offset, int count)
		{
			base.WrappedStream.Write(buffer, offset, count);
		}

		// Token: 0x06001C8A RID: 7306 RVA: 0x00087800 File Offset: 0x00085A00
		public override IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
		{
			return base.WrappedStream.BeginWrite(buffer, offset, count, callback, state);
		}

		// Token: 0x06001C8B RID: 7307 RVA: 0x00087814 File Offset: 0x00085A14
		public override void EndWrite(IAsyncResult asyncResult)
		{
			base.WrappedStream.EndWrite(asyncResult);
		}

		// Token: 0x06001C8C RID: 7308 RVA: 0x00087822 File Offset: 0x00085A22
		public override void Flush()
		{
			base.WrappedStream.Flush();
		}

		// Token: 0x06001C8D RID: 7309 RVA: 0x0008782F File Offset: 0x00085A2F
		public override int Read(byte[] buffer, int offset, int count)
		{
			return base.WrappedStream.Read(buffer, offset, count);
		}

		// Token: 0x06001C8E RID: 7310 RVA: 0x0008783F File Offset: 0x00085A3F
		public override IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
		{
			return base.WrappedStream.BeginRead(buffer, offset, count, callback, state);
		}

		// Token: 0x06001C8F RID: 7311 RVA: 0x00087853 File Offset: 0x00085A53
		public override int EndRead(IAsyncResult asyncResult)
		{
			return base.WrappedStream.EndRead(asyncResult);
		}

		// Token: 0x06001C90 RID: 7312 RVA: 0x00087861 File Offset: 0x00085A61
		protected sealed override void Dispose(bool disposing)
		{
			this.Dispose(disposing, CloseExState.Normal);
		}

		// Token: 0x06001C91 RID: 7313 RVA: 0x0008786B File Offset: 0x00085A6B
		void ICloseEx.CloseEx(CloseExState closeState)
		{
			this.Dispose(true, closeState);
		}

		// Token: 0x17000709 RID: 1801
		// (get) Token: 0x06001C92 RID: 7314 RVA: 0x00087875 File Offset: 0x00085A75
		public override bool CanTimeout
		{
			get
			{
				return base.WrappedStream.CanTimeout;
			}
		}

		// Token: 0x1700070A RID: 1802
		// (get) Token: 0x06001C93 RID: 7315 RVA: 0x00087882 File Offset: 0x00085A82
		// (set) Token: 0x06001C94 RID: 7316 RVA: 0x0008788F File Offset: 0x00085A8F
		public override int ReadTimeout
		{
			get
			{
				return base.WrappedStream.ReadTimeout;
			}
			set
			{
				base.WrappedStream.ReadTimeout = value;
			}
		}

		// Token: 0x1700070B RID: 1803
		// (get) Token: 0x06001C95 RID: 7317 RVA: 0x0008789D File Offset: 0x00085A9D
		// (set) Token: 0x06001C96 RID: 7318 RVA: 0x000878AA File Offset: 0x00085AAA
		public override int WriteTimeout
		{
			get
			{
				return base.WrappedStream.WriteTimeout;
			}
			set
			{
				base.WrappedStream.WriteTimeout = value;
			}
		}

		// Token: 0x06001C97 RID: 7319 RVA: 0x000878B8 File Offset: 0x00085AB8
		protected virtual void Dispose(bool disposing, CloseExState closeState)
		{
			try
			{
				if (Interlocked.Increment(ref this._Disposed) == 1 && disposing)
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
					if (this.m_CacheDestroy)
					{
						if (this.m_IsStrictCacheErrors)
						{
							this.m_Cache.Remove(this.m_Key);
						}
						else
						{
							this.m_Cache.TryRemove(this.m_Key);
						}
					}
					else if (this.m_IsStrictCacheErrors)
					{
						this.m_Cache.Update(this.m_Key, this.m_Expires, this.m_LastModified, this.m_LastSynchronized, this.m_MaxStale, this.m_EntryMetadata, this.m_SystemMetadata);
					}
					else
					{
						this.m_Cache.TryUpdate(this.m_Key, this.m_Expires, this.m_LastModified, this.m_LastSynchronized, this.m_MaxStale, this.m_EntryMetadata, this.m_SystemMetadata);
					}
				}
			}
			finally
			{
				base.Dispose(disposing);
			}
		}

		// Token: 0x04001B97 RID: 7063
		private RequestCache m_Cache;

		// Token: 0x04001B98 RID: 7064
		private string m_Key;

		// Token: 0x04001B99 RID: 7065
		private DateTime m_Expires;

		// Token: 0x04001B9A RID: 7066
		private DateTime m_LastModified;

		// Token: 0x04001B9B RID: 7067
		private DateTime m_LastSynchronized;

		// Token: 0x04001B9C RID: 7068
		private TimeSpan m_MaxStale;

		// Token: 0x04001B9D RID: 7069
		private StringCollection m_EntryMetadata;

		// Token: 0x04001B9E RID: 7070
		private StringCollection m_SystemMetadata;

		// Token: 0x04001B9F RID: 7071
		private bool m_CacheDestroy;

		// Token: 0x04001BA0 RID: 7072
		private bool m_IsStrictCacheErrors;

		// Token: 0x04001BA1 RID: 7073
		private int _Disposed;
	}
}
