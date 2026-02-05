using System;
using System.Collections.Specialized;
using System.IO;

namespace System.Net.Cache
{
	// Token: 0x0200030D RID: 781
	internal abstract class RequestCache
	{
		// Token: 0x06001BDC RID: 7132 RVA: 0x00085379 File Offset: 0x00083579
		protected RequestCache(bool isPrivateCache, bool canWrite)
		{
			this._IsPrivateCache = isPrivateCache;
			this._CanWrite = canWrite;
		}

		// Token: 0x170006CC RID: 1740
		// (get) Token: 0x06001BDD RID: 7133 RVA: 0x0008538F File Offset: 0x0008358F
		internal bool IsPrivateCache
		{
			get
			{
				return this._IsPrivateCache;
			}
		}

		// Token: 0x170006CD RID: 1741
		// (get) Token: 0x06001BDE RID: 7134 RVA: 0x00085397 File Offset: 0x00083597
		internal bool CanWrite
		{
			get
			{
				return this._CanWrite;
			}
		}

		// Token: 0x06001BDF RID: 7135
		internal abstract Stream Retrieve(string key, out RequestCacheEntry cacheEntry);

		// Token: 0x06001BE0 RID: 7136
		internal abstract Stream Store(string key, long contentLength, DateTime expiresUtc, DateTime lastModifiedUtc, TimeSpan maxStale, StringCollection entryMetadata, StringCollection systemMetadata);

		// Token: 0x06001BE1 RID: 7137
		internal abstract void Remove(string key);

		// Token: 0x06001BE2 RID: 7138
		internal abstract void Update(string key, DateTime expiresUtc, DateTime lastModifiedUtc, DateTime lastSynchronizedUtc, TimeSpan maxStale, StringCollection entryMetadata, StringCollection systemMetadata);

		// Token: 0x06001BE3 RID: 7139
		internal abstract bool TryRetrieve(string key, out RequestCacheEntry cacheEntry, out Stream readStream);

		// Token: 0x06001BE4 RID: 7140
		internal abstract bool TryStore(string key, long contentLength, DateTime expiresUtc, DateTime lastModifiedUtc, TimeSpan maxStale, StringCollection entryMetadata, StringCollection systemMetadata, out Stream writeStream);

		// Token: 0x06001BE5 RID: 7141
		internal abstract bool TryRemove(string key);

		// Token: 0x06001BE6 RID: 7142
		internal abstract bool TryUpdate(string key, DateTime expiresUtc, DateTime lastModifiedUtc, DateTime lastSynchronizedUtc, TimeSpan maxStale, StringCollection entryMetadata, StringCollection systemMetadata);

		// Token: 0x06001BE7 RID: 7143
		internal abstract void UnlockEntry(Stream retrieveStream);

		// Token: 0x04001B32 RID: 6962
		internal static readonly char[] LineSplits = new char[] { '\r', '\n' };

		// Token: 0x04001B33 RID: 6963
		private bool _IsPrivateCache;

		// Token: 0x04001B34 RID: 6964
		private bool _CanWrite;
	}
}
