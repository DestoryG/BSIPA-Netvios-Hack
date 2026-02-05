using System;
using System.Collections;
using System.Collections.Specialized;
using System.IO;
using Microsoft.Win32;

namespace System.Net.Cache
{
	// Token: 0x02000321 RID: 801
	internal class SingleItemRequestCache : WinInetCache
	{
		// Token: 0x06001CCC RID: 7372 RVA: 0x0008A15F File Offset: 0x0008835F
		internal SingleItemRequestCache(bool useWinInet)
			: base(true, true, false)
		{
			this._UseWinInet = useWinInet;
		}

		// Token: 0x06001CCD RID: 7373 RVA: 0x0008A174 File Offset: 0x00088374
		internal override Stream Retrieve(string key, out RequestCacheEntry cacheEntry)
		{
			Stream stream;
			if (!this.TryRetrieve(key, out cacheEntry, out stream))
			{
				FileNotFoundException ex = new FileNotFoundException(null, key);
				throw new IOException(SR.GetString("net_cache_retrieve_failure", new object[] { ex.Message }), ex);
			}
			return stream;
		}

		// Token: 0x06001CCE RID: 7374 RVA: 0x0008A1B8 File Offset: 0x000883B8
		internal override Stream Store(string key, long contentLength, DateTime expiresUtc, DateTime lastModifiedUtc, TimeSpan maxStale, StringCollection entryMetadata, StringCollection systemMetadata)
		{
			Stream stream;
			if (!this.TryStore(key, contentLength, expiresUtc, lastModifiedUtc, maxStale, entryMetadata, systemMetadata, out stream))
			{
				FileNotFoundException ex = new FileNotFoundException(null, key);
				throw new IOException(SR.GetString("net_cache_retrieve_failure", new object[] { ex.Message }), ex);
			}
			return stream;
		}

		// Token: 0x06001CCF RID: 7375 RVA: 0x0008A204 File Offset: 0x00088404
		internal override void Remove(string key)
		{
			if (!this.TryRemove(key))
			{
				FileNotFoundException ex = new FileNotFoundException(null, key);
				throw new IOException(SR.GetString("net_cache_retrieve_failure", new object[] { ex.Message }), ex);
			}
		}

		// Token: 0x06001CD0 RID: 7376 RVA: 0x0008A244 File Offset: 0x00088444
		internal override void Update(string key, DateTime expiresUtc, DateTime lastModifiedUtc, DateTime lastSynchronizedUtc, TimeSpan maxStale, StringCollection entryMetadata, StringCollection systemMetadata)
		{
			if (!this.TryUpdate(key, expiresUtc, lastModifiedUtc, lastSynchronizedUtc, maxStale, entryMetadata, systemMetadata))
			{
				FileNotFoundException ex = new FileNotFoundException(null, key);
				throw new IOException(SR.GetString("net_cache_retrieve_failure", new object[] { ex.Message }), ex);
			}
		}

		// Token: 0x06001CD1 RID: 7377 RVA: 0x0008A28C File Offset: 0x0008848C
		internal override bool TryRetrieve(string key, out RequestCacheEntry cacheEntry, out Stream readStream)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key");
			}
			SingleItemRequestCache.FrozenCacheEntry frozenCacheEntry = this._Entry;
			cacheEntry = null;
			readStream = null;
			if (frozenCacheEntry == null || frozenCacheEntry.Key != key)
			{
				RequestCacheEntry requestCacheEntry;
				Stream stream;
				if (!this._UseWinInet || !base.TryRetrieve(key, out requestCacheEntry, out stream))
				{
					return false;
				}
				frozenCacheEntry = new SingleItemRequestCache.FrozenCacheEntry(key, requestCacheEntry, stream);
				stream.Close();
				this._Entry = frozenCacheEntry;
			}
			cacheEntry = SingleItemRequestCache.FrozenCacheEntry.Create(frozenCacheEntry);
			readStream = new SingleItemRequestCache.ReadOnlyStream(frozenCacheEntry.StreamBytes);
			return true;
		}

		// Token: 0x06001CD2 RID: 7378 RVA: 0x0008A308 File Offset: 0x00088508
		internal override bool TryStore(string key, long contentLength, DateTime expiresUtc, DateTime lastModifiedUtc, TimeSpan maxStale, StringCollection entryMetadata, StringCollection systemMetadata, out Stream writeStream)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key");
			}
			RequestCacheEntry requestCacheEntry = new RequestCacheEntry();
			requestCacheEntry.IsPrivateEntry = base.IsPrivateCache;
			requestCacheEntry.StreamSize = contentLength;
			requestCacheEntry.ExpiresUtc = expiresUtc;
			requestCacheEntry.LastModifiedUtc = lastModifiedUtc;
			requestCacheEntry.LastAccessedUtc = DateTime.UtcNow;
			requestCacheEntry.LastSynchronizedUtc = DateTime.UtcNow;
			requestCacheEntry.MaxStale = maxStale;
			requestCacheEntry.HitCount = 0;
			requestCacheEntry.UsageCount = 0;
			requestCacheEntry.IsPartialEntry = false;
			requestCacheEntry.EntryMetadata = entryMetadata;
			requestCacheEntry.SystemMetadata = systemMetadata;
			writeStream = null;
			Stream stream = null;
			if (this._UseWinInet)
			{
				base.TryStore(key, contentLength, expiresUtc, lastModifiedUtc, maxStale, entryMetadata, systemMetadata, out stream);
			}
			writeStream = new SingleItemRequestCache.WriteOnlyStream(key, this, requestCacheEntry, stream);
			return true;
		}

		// Token: 0x06001CD3 RID: 7379 RVA: 0x0008A3C0 File Offset: 0x000885C0
		private void Commit(string key, RequestCacheEntry tempEntry, byte[] allBytes)
		{
			SingleItemRequestCache.FrozenCacheEntry frozenCacheEntry = new SingleItemRequestCache.FrozenCacheEntry(key, tempEntry, allBytes);
			this._Entry = frozenCacheEntry;
		}

		// Token: 0x06001CD4 RID: 7380 RVA: 0x0008A3E0 File Offset: 0x000885E0
		internal override bool TryRemove(string key)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key");
			}
			if (this._UseWinInet)
			{
				base.TryRemove(key);
			}
			SingleItemRequestCache.FrozenCacheEntry entry = this._Entry;
			if (entry != null && entry.Key == key)
			{
				this._Entry = null;
			}
			return true;
		}

		// Token: 0x06001CD5 RID: 7381 RVA: 0x0008A42C File Offset: 0x0008862C
		internal override bool TryUpdate(string key, DateTime expiresUtc, DateTime lastModifiedUtc, DateTime lastSynchronizedUtc, TimeSpan maxStale, StringCollection entryMetadata, StringCollection systemMetadata)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key");
			}
			SingleItemRequestCache.FrozenCacheEntry frozenCacheEntry = SingleItemRequestCache.FrozenCacheEntry.Create(this._Entry);
			if (frozenCacheEntry == null || frozenCacheEntry.Key != key)
			{
				return true;
			}
			frozenCacheEntry.ExpiresUtc = expiresUtc;
			frozenCacheEntry.LastModifiedUtc = lastModifiedUtc;
			frozenCacheEntry.LastSynchronizedUtc = lastSynchronizedUtc;
			frozenCacheEntry.MaxStale = maxStale;
			frozenCacheEntry.EntryMetadata = entryMetadata;
			frozenCacheEntry.SystemMetadata = systemMetadata;
			this._Entry = frozenCacheEntry;
			return true;
		}

		// Token: 0x06001CD6 RID: 7382 RVA: 0x0008A49C File Offset: 0x0008869C
		internal override void UnlockEntry(Stream stream)
		{
		}

		// Token: 0x04001BAD RID: 7085
		private bool _UseWinInet;

		// Token: 0x04001BAE RID: 7086
		private SingleItemRequestCache.FrozenCacheEntry _Entry;

		// Token: 0x020007BD RID: 1981
		private sealed class FrozenCacheEntry : RequestCacheEntry
		{
			// Token: 0x06004349 RID: 17225 RVA: 0x0011C31D File Offset: 0x0011A51D
			public FrozenCacheEntry(string key, RequestCacheEntry entry, Stream stream)
				: this(key, entry, SingleItemRequestCache.FrozenCacheEntry.GetBytes(stream))
			{
			}

			// Token: 0x0600434A RID: 17226 RVA: 0x0011C330 File Offset: 0x0011A530
			public FrozenCacheEntry(string key, RequestCacheEntry entry, byte[] streamBytes)
			{
				this._Key = key;
				this._StreamBytes = streamBytes;
				base.IsPrivateEntry = entry.IsPrivateEntry;
				base.StreamSize = entry.StreamSize;
				base.ExpiresUtc = entry.ExpiresUtc;
				base.HitCount = entry.HitCount;
				base.LastAccessedUtc = entry.LastAccessedUtc;
				entry.LastModifiedUtc = entry.LastModifiedUtc;
				base.LastSynchronizedUtc = entry.LastSynchronizedUtc;
				base.MaxStale = entry.MaxStale;
				base.UsageCount = entry.UsageCount;
				base.IsPartialEntry = entry.IsPartialEntry;
				base.EntryMetadata = entry.EntryMetadata;
				base.SystemMetadata = entry.SystemMetadata;
			}

			// Token: 0x0600434B RID: 17227 RVA: 0x0011C3E4 File Offset: 0x0011A5E4
			private static byte[] GetBytes(Stream stream)
			{
				bool flag = false;
				byte[] array;
				if (stream.CanSeek)
				{
					array = new byte[stream.Length];
				}
				else
				{
					flag = true;
					array = new byte[8192];
				}
				int num = 0;
				for (;;)
				{
					int num2 = stream.Read(array, num, array.Length - num);
					if (num2 == 0)
					{
						break;
					}
					if ((num += num2) == array.Length && flag)
					{
						byte[] array2 = new byte[array.Length + 8192];
						Buffer.BlockCopy(array, 0, array2, 0, num);
						array = array2;
					}
				}
				if (flag)
				{
					byte[] array3 = new byte[num];
					Buffer.BlockCopy(array, 0, array3, 0, num);
					array = array3;
				}
				return array;
			}

			// Token: 0x0600434C RID: 17228 RVA: 0x0011C472 File Offset: 0x0011A672
			public static SingleItemRequestCache.FrozenCacheEntry Create(SingleItemRequestCache.FrozenCacheEntry clonedObject)
			{
				if (clonedObject != null)
				{
					return (SingleItemRequestCache.FrozenCacheEntry)clonedObject.MemberwiseClone();
				}
				return null;
			}

			// Token: 0x17000F41 RID: 3905
			// (get) Token: 0x0600434D RID: 17229 RVA: 0x0011C484 File Offset: 0x0011A684
			public byte[] StreamBytes
			{
				get
				{
					return this._StreamBytes;
				}
			}

			// Token: 0x17000F42 RID: 3906
			// (get) Token: 0x0600434E RID: 17230 RVA: 0x0011C48C File Offset: 0x0011A68C
			public string Key
			{
				get
				{
					return this._Key;
				}
			}

			// Token: 0x04003456 RID: 13398
			private byte[] _StreamBytes;

			// Token: 0x04003457 RID: 13399
			private string _Key;
		}

		// Token: 0x020007BE RID: 1982
		internal class ReadOnlyStream : Stream, IRequestLifetimeTracker
		{
			// Token: 0x0600434F RID: 17231 RVA: 0x0011C494 File Offset: 0x0011A694
			internal ReadOnlyStream(byte[] bytes)
			{
				this._Bytes = bytes;
				this._Offset = 0;
				this._Disposed = false;
				this._ReadTimeout = (this._WriteTimeout = -1);
			}

			// Token: 0x17000F43 RID: 3907
			// (get) Token: 0x06004350 RID: 17232 RVA: 0x0011C4CC File Offset: 0x0011A6CC
			public override bool CanRead
			{
				get
				{
					return true;
				}
			}

			// Token: 0x17000F44 RID: 3908
			// (get) Token: 0x06004351 RID: 17233 RVA: 0x0011C4CF File Offset: 0x0011A6CF
			public override bool CanSeek
			{
				get
				{
					return true;
				}
			}

			// Token: 0x17000F45 RID: 3909
			// (get) Token: 0x06004352 RID: 17234 RVA: 0x0011C4D2 File Offset: 0x0011A6D2
			public override bool CanTimeout
			{
				get
				{
					return true;
				}
			}

			// Token: 0x17000F46 RID: 3910
			// (get) Token: 0x06004353 RID: 17235 RVA: 0x0011C4D5 File Offset: 0x0011A6D5
			public override bool CanWrite
			{
				get
				{
					return false;
				}
			}

			// Token: 0x17000F47 RID: 3911
			// (get) Token: 0x06004354 RID: 17236 RVA: 0x0011C4D8 File Offset: 0x0011A6D8
			public override long Length
			{
				get
				{
					return (long)this._Bytes.Length;
				}
			}

			// Token: 0x17000F48 RID: 3912
			// (get) Token: 0x06004355 RID: 17237 RVA: 0x0011C4E3 File Offset: 0x0011A6E3
			// (set) Token: 0x06004356 RID: 17238 RVA: 0x0011C4EC File Offset: 0x0011A6EC
			public override long Position
			{
				get
				{
					return (long)this._Offset;
				}
				set
				{
					if (value < 0L || value > (long)this._Bytes.Length)
					{
						throw new ArgumentOutOfRangeException("value");
					}
					this._Offset = (int)value;
				}
			}

			// Token: 0x17000F49 RID: 3913
			// (get) Token: 0x06004357 RID: 17239 RVA: 0x0011C512 File Offset: 0x0011A712
			// (set) Token: 0x06004358 RID: 17240 RVA: 0x0011C51A File Offset: 0x0011A71A
			public override int ReadTimeout
			{
				get
				{
					return this._ReadTimeout;
				}
				set
				{
					if (value <= 0 && value != -1)
					{
						throw new ArgumentOutOfRangeException("value", SR.GetString("net_io_timeout_use_gt_zero"));
					}
					this._ReadTimeout = value;
				}
			}

			// Token: 0x17000F4A RID: 3914
			// (get) Token: 0x06004359 RID: 17241 RVA: 0x0011C540 File Offset: 0x0011A740
			// (set) Token: 0x0600435A RID: 17242 RVA: 0x0011C548 File Offset: 0x0011A748
			public override int WriteTimeout
			{
				get
				{
					return this._WriteTimeout;
				}
				set
				{
					if (value <= 0 && value != -1)
					{
						throw new ArgumentOutOfRangeException("value", SR.GetString("net_io_timeout_use_gt_zero"));
					}
					this._WriteTimeout = value;
				}
			}

			// Token: 0x0600435B RID: 17243 RVA: 0x0011C56E File Offset: 0x0011A76E
			public override void Flush()
			{
			}

			// Token: 0x0600435C RID: 17244 RVA: 0x0011C570 File Offset: 0x0011A770
			public override IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
			{
				int num = this.Read(buffer, offset, count);
				LazyAsyncResult lazyAsyncResult = new LazyAsyncResult(null, state, callback);
				lazyAsyncResult.InvokeCallback(num);
				return lazyAsyncResult;
			}

			// Token: 0x0600435D RID: 17245 RVA: 0x0011C5A0 File Offset: 0x0011A7A0
			public override int EndRead(IAsyncResult asyncResult)
			{
				if (asyncResult == null)
				{
					throw new ArgumentNullException("asyncResult");
				}
				LazyAsyncResult lazyAsyncResult = (LazyAsyncResult)asyncResult;
				if (lazyAsyncResult.EndCalled)
				{
					throw new InvalidOperationException(SR.GetString("net_io_invalidendcall", new object[] { "EndRead" }));
				}
				lazyAsyncResult.EndCalled = true;
				return (int)lazyAsyncResult.InternalWaitForCompletion();
			}

			// Token: 0x0600435E RID: 17246 RVA: 0x0011C5FC File Offset: 0x0011A7FC
			public override int Read(byte[] buffer, int offset, int count)
			{
				if (this._Disposed)
				{
					throw new ObjectDisposedException(base.GetType().Name);
				}
				if (buffer == null)
				{
					throw new ArgumentNullException("buffer");
				}
				if (offset < 0 || offset > buffer.Length)
				{
					throw new ArgumentOutOfRangeException("offset");
				}
				if (count < 0 || count > buffer.Length - offset)
				{
					throw new ArgumentOutOfRangeException("count");
				}
				if (this._Offset == this._Bytes.Length)
				{
					return 0;
				}
				int num = this._Offset;
				count = Math.Min(count, this._Bytes.Length - num);
				global::System.Buffer.BlockCopy(this._Bytes, num, buffer, offset, count);
				num += count;
				this._Offset = num;
				return count;
			}

			// Token: 0x0600435F RID: 17247 RVA: 0x0011C6A2 File Offset: 0x0011A8A2
			public override IAsyncResult BeginWrite(byte[] buffer, int offset, int size, AsyncCallback callback, object state)
			{
				throw new NotSupportedException(SR.GetString("net_readonlystream"));
			}

			// Token: 0x06004360 RID: 17248 RVA: 0x0011C6B3 File Offset: 0x0011A8B3
			public override void EndWrite(IAsyncResult asyncResult)
			{
				throw new NotSupportedException(SR.GetString("net_readonlystream"));
			}

			// Token: 0x06004361 RID: 17249 RVA: 0x0011C6C4 File Offset: 0x0011A8C4
			public override void Write(byte[] buffer, int offset, int count)
			{
				throw new NotSupportedException(SR.GetString("net_readonlystream"));
			}

			// Token: 0x06004362 RID: 17250 RVA: 0x0011C6D8 File Offset: 0x0011A8D8
			public override long Seek(long offset, SeekOrigin origin)
			{
				switch (origin)
				{
				case SeekOrigin.Begin:
					this.Position = offset;
					return offset;
				case SeekOrigin.Current:
					return this.Position += offset;
				case SeekOrigin.End:
					return this.Position = (long)this._Bytes.Length - offset;
				default:
					throw new ArgumentException(SR.GetString("net_invalid_enum", new object[] { "SeekOrigin" }), "origin");
				}
			}

			// Token: 0x06004363 RID: 17251 RVA: 0x0011C74D File Offset: 0x0011A94D
			public override void SetLength(long length)
			{
				throw new NotSupportedException(SR.GetString("net_readonlystream"));
			}

			// Token: 0x06004364 RID: 17252 RVA: 0x0011C760 File Offset: 0x0011A960
			protected override void Dispose(bool disposing)
			{
				try
				{
					if (!this._Disposed)
					{
						this._Disposed = true;
						if (disposing)
						{
							RequestLifetimeSetter.Report(this.m_RequestLifetimeSetter);
						}
					}
				}
				finally
				{
					base.Dispose(disposing);
				}
			}

			// Token: 0x17000F4B RID: 3915
			// (get) Token: 0x06004365 RID: 17253 RVA: 0x0011C7A4 File Offset: 0x0011A9A4
			internal byte[] Buffer
			{
				get
				{
					return this._Bytes;
				}
			}

			// Token: 0x06004366 RID: 17254 RVA: 0x0011C7AC File Offset: 0x0011A9AC
			void IRequestLifetimeTracker.TrackRequestLifetime(long requestStartTimestamp)
			{
				this.m_RequestLifetimeSetter = new RequestLifetimeSetter(requestStartTimestamp);
			}

			// Token: 0x04003458 RID: 13400
			private byte[] _Bytes;

			// Token: 0x04003459 RID: 13401
			private int _Offset;

			// Token: 0x0400345A RID: 13402
			private bool _Disposed;

			// Token: 0x0400345B RID: 13403
			private int _ReadTimeout;

			// Token: 0x0400345C RID: 13404
			private int _WriteTimeout;

			// Token: 0x0400345D RID: 13405
			private RequestLifetimeSetter m_RequestLifetimeSetter;
		}

		// Token: 0x020007BF RID: 1983
		private class WriteOnlyStream : Stream
		{
			// Token: 0x06004367 RID: 17255 RVA: 0x0011C7BA File Offset: 0x0011A9BA
			public WriteOnlyStream(string key, SingleItemRequestCache cache, RequestCacheEntry cacheEntry, Stream realWriteStream)
			{
				this._Key = key;
				this._Cache = cache;
				this._TempEntry = cacheEntry;
				this._RealStream = realWriteStream;
				this._Buffers = new ArrayList();
			}

			// Token: 0x17000F4C RID: 3916
			// (get) Token: 0x06004368 RID: 17256 RVA: 0x0011C7EA File Offset: 0x0011A9EA
			public override bool CanRead
			{
				get
				{
					return false;
				}
			}

			// Token: 0x17000F4D RID: 3917
			// (get) Token: 0x06004369 RID: 17257 RVA: 0x0011C7ED File Offset: 0x0011A9ED
			public override bool CanSeek
			{
				get
				{
					return false;
				}
			}

			// Token: 0x17000F4E RID: 3918
			// (get) Token: 0x0600436A RID: 17258 RVA: 0x0011C7F0 File Offset: 0x0011A9F0
			public override bool CanTimeout
			{
				get
				{
					return true;
				}
			}

			// Token: 0x17000F4F RID: 3919
			// (get) Token: 0x0600436B RID: 17259 RVA: 0x0011C7F3 File Offset: 0x0011A9F3
			public override bool CanWrite
			{
				get
				{
					return true;
				}
			}

			// Token: 0x17000F50 RID: 3920
			// (get) Token: 0x0600436C RID: 17260 RVA: 0x0011C7F6 File Offset: 0x0011A9F6
			public override long Length
			{
				get
				{
					throw new NotSupportedException(SR.GetString("net_writeonlystream"));
				}
			}

			// Token: 0x17000F51 RID: 3921
			// (get) Token: 0x0600436D RID: 17261 RVA: 0x0011C807 File Offset: 0x0011AA07
			// (set) Token: 0x0600436E RID: 17262 RVA: 0x0011C818 File Offset: 0x0011AA18
			public override long Position
			{
				get
				{
					throw new NotSupportedException(SR.GetString("net_writeonlystream"));
				}
				set
				{
					throw new NotSupportedException(SR.GetString("net_writeonlystream"));
				}
			}

			// Token: 0x17000F52 RID: 3922
			// (get) Token: 0x0600436F RID: 17263 RVA: 0x0011C829 File Offset: 0x0011AA29
			// (set) Token: 0x06004370 RID: 17264 RVA: 0x0011C831 File Offset: 0x0011AA31
			public override int ReadTimeout
			{
				get
				{
					return this._ReadTimeout;
				}
				set
				{
					if (value <= 0 && value != -1)
					{
						throw new ArgumentOutOfRangeException("value", SR.GetString("net_io_timeout_use_gt_zero"));
					}
					this._ReadTimeout = value;
				}
			}

			// Token: 0x17000F53 RID: 3923
			// (get) Token: 0x06004371 RID: 17265 RVA: 0x0011C857 File Offset: 0x0011AA57
			// (set) Token: 0x06004372 RID: 17266 RVA: 0x0011C85F File Offset: 0x0011AA5F
			public override int WriteTimeout
			{
				get
				{
					return this._WriteTimeout;
				}
				set
				{
					if (value <= 0 && value != -1)
					{
						throw new ArgumentOutOfRangeException("value", SR.GetString("net_io_timeout_use_gt_zero"));
					}
					this._WriteTimeout = value;
				}
			}

			// Token: 0x06004373 RID: 17267 RVA: 0x0011C885 File Offset: 0x0011AA85
			public override void Flush()
			{
			}

			// Token: 0x06004374 RID: 17268 RVA: 0x0011C887 File Offset: 0x0011AA87
			public override IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
			{
				throw new NotSupportedException(SR.GetString("net_writeonlystream"));
			}

			// Token: 0x06004375 RID: 17269 RVA: 0x0011C898 File Offset: 0x0011AA98
			public override int EndRead(IAsyncResult asyncResult)
			{
				throw new NotSupportedException(SR.GetString("net_writeonlystream"));
			}

			// Token: 0x06004376 RID: 17270 RVA: 0x0011C8A9 File Offset: 0x0011AAA9
			public override int Read(byte[] buffer, int offset, int count)
			{
				throw new NotSupportedException(SR.GetString("net_writeonlystream"));
			}

			// Token: 0x06004377 RID: 17271 RVA: 0x0011C8BA File Offset: 0x0011AABA
			public override long Seek(long offset, SeekOrigin origin)
			{
				throw new NotSupportedException(SR.GetString("net_writeonlystream"));
			}

			// Token: 0x06004378 RID: 17272 RVA: 0x0011C8CB File Offset: 0x0011AACB
			public override void SetLength(long length)
			{
				throw new NotSupportedException(SR.GetString("net_writeonlystream"));
			}

			// Token: 0x06004379 RID: 17273 RVA: 0x0011C8DC File Offset: 0x0011AADC
			public override IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
			{
				this.Write(buffer, offset, count);
				LazyAsyncResult lazyAsyncResult = new LazyAsyncResult(null, state, callback);
				lazyAsyncResult.InvokeCallback(null);
				return lazyAsyncResult;
			}

			// Token: 0x0600437A RID: 17274 RVA: 0x0011C908 File Offset: 0x0011AB08
			public override void EndWrite(IAsyncResult asyncResult)
			{
				if (asyncResult == null)
				{
					throw new ArgumentNullException("asyncResult");
				}
				LazyAsyncResult lazyAsyncResult = (LazyAsyncResult)asyncResult;
				if (lazyAsyncResult.EndCalled)
				{
					throw new InvalidOperationException(SR.GetString("net_io_invalidendcall", new object[] { "EndWrite" }));
				}
				lazyAsyncResult.EndCalled = true;
				lazyAsyncResult.InternalWaitForCompletion();
			}

			// Token: 0x0600437B RID: 17275 RVA: 0x0011C960 File Offset: 0x0011AB60
			public override void Write(byte[] buffer, int offset, int count)
			{
				if (this._Disposed)
				{
					throw new ObjectDisposedException(base.GetType().Name);
				}
				if (buffer == null)
				{
					throw new ArgumentNullException("buffer");
				}
				if (offset < 0 || offset > buffer.Length)
				{
					throw new ArgumentOutOfRangeException("offset");
				}
				if (count < 0 || count > buffer.Length - offset)
				{
					throw new ArgumentOutOfRangeException("count");
				}
				if (this._RealStream != null)
				{
					try
					{
						this._RealStream.Write(buffer, offset, count);
					}
					catch
					{
						this._RealStream.Close();
						this._RealStream = null;
					}
				}
				byte[] array = new byte[count];
				Buffer.BlockCopy(buffer, offset, array, 0, count);
				this._Buffers.Add(array);
				this._TotalSize += (long)count;
			}

			// Token: 0x0600437C RID: 17276 RVA: 0x0011CA2C File Offset: 0x0011AC2C
			protected override void Dispose(bool disposing)
			{
				this._Disposed = true;
				base.Dispose(disposing);
				if (disposing)
				{
					if (this._RealStream != null)
					{
						try
						{
							this._RealStream.Close();
						}
						catch
						{
						}
					}
					byte[] array = new byte[this._TotalSize];
					int num = 0;
					for (int i = 0; i < this._Buffers.Count; i++)
					{
						byte[] array2 = (byte[])this._Buffers[i];
						Buffer.BlockCopy(array2, 0, array, num, array2.Length);
						num += array2.Length;
					}
					this._Cache.Commit(this._Key, this._TempEntry, array);
				}
			}

			// Token: 0x0400345E RID: 13406
			private string _Key;

			// Token: 0x0400345F RID: 13407
			private SingleItemRequestCache _Cache;

			// Token: 0x04003460 RID: 13408
			private RequestCacheEntry _TempEntry;

			// Token: 0x04003461 RID: 13409
			private Stream _RealStream;

			// Token: 0x04003462 RID: 13410
			private long _TotalSize;

			// Token: 0x04003463 RID: 13411
			private ArrayList _Buffers;

			// Token: 0x04003464 RID: 13412
			private bool _Disposed;

			// Token: 0x04003465 RID: 13413
			private int _ReadTimeout;

			// Token: 0x04003466 RID: 13414
			private int _WriteTimeout;
		}
	}
}
