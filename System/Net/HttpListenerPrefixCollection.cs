using System;
using System.Collections;
using System.Collections.Generic;

namespace System.Net
{
	// Token: 0x020000FB RID: 251
	public class HttpListenerPrefixCollection : ICollection<string>, IEnumerable<string>, IEnumerable
	{
		// Token: 0x06000904 RID: 2308 RVA: 0x00032B64 File Offset: 0x00030D64
		internal HttpListenerPrefixCollection(HttpListener listener)
		{
			this.m_HttpListener = listener;
		}

		// Token: 0x06000905 RID: 2309 RVA: 0x00032B74 File Offset: 0x00030D74
		public void CopyTo(Array array, int offset)
		{
			this.m_HttpListener.CheckDisposed();
			if (this.Count > array.Length)
			{
				throw new ArgumentOutOfRangeException("array", SR.GetString("net_array_too_small"));
			}
			if (offset + this.Count > array.Length)
			{
				throw new ArgumentOutOfRangeException("offset");
			}
			int num = 0;
			foreach (object obj in this.m_HttpListener.m_UriPrefixes.Keys)
			{
				string text = (string)obj;
				array.SetValue(text, offset + num++);
			}
		}

		// Token: 0x06000906 RID: 2310 RVA: 0x00032C2C File Offset: 0x00030E2C
		public void CopyTo(string[] array, int offset)
		{
			this.m_HttpListener.CheckDisposed();
			if (this.Count > array.Length)
			{
				throw new ArgumentOutOfRangeException("array", SR.GetString("net_array_too_small"));
			}
			if (offset + this.Count > array.Length)
			{
				throw new ArgumentOutOfRangeException("offset");
			}
			int num = 0;
			foreach (object obj in this.m_HttpListener.m_UriPrefixes.Keys)
			{
				string text = (string)obj;
				array[offset + num++] = text;
			}
		}

		// Token: 0x170001C9 RID: 457
		// (get) Token: 0x06000907 RID: 2311 RVA: 0x00032CD8 File Offset: 0x00030ED8
		public int Count
		{
			get
			{
				return this.m_HttpListener.m_UriPrefixes.Count;
			}
		}

		// Token: 0x170001CA RID: 458
		// (get) Token: 0x06000908 RID: 2312 RVA: 0x00032CEA File Offset: 0x00030EEA
		public bool IsSynchronized
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170001CB RID: 459
		// (get) Token: 0x06000909 RID: 2313 RVA: 0x00032CED File Offset: 0x00030EED
		public bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600090A RID: 2314 RVA: 0x00032CF0 File Offset: 0x00030EF0
		public void Add(string uriPrefix)
		{
			this.m_HttpListener.AddPrefix(uriPrefix);
		}

		// Token: 0x0600090B RID: 2315 RVA: 0x00032CFE File Offset: 0x00030EFE
		public bool Contains(string uriPrefix)
		{
			return this.m_HttpListener.m_UriPrefixes.Contains(uriPrefix);
		}

		// Token: 0x0600090C RID: 2316 RVA: 0x00032D11 File Offset: 0x00030F11
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x0600090D RID: 2317 RVA: 0x00032D19 File Offset: 0x00030F19
		public IEnumerator<string> GetEnumerator()
		{
			return new ListenerPrefixEnumerator(this.m_HttpListener.m_UriPrefixes.Keys.GetEnumerator());
		}

		// Token: 0x0600090E RID: 2318 RVA: 0x00032D35 File Offset: 0x00030F35
		public bool Remove(string uriPrefix)
		{
			return this.m_HttpListener.RemovePrefix(uriPrefix);
		}

		// Token: 0x0600090F RID: 2319 RVA: 0x00032D43 File Offset: 0x00030F43
		public void Clear()
		{
			this.m_HttpListener.RemoveAll(true);
		}

		// Token: 0x04000E0B RID: 3595
		private HttpListener m_HttpListener;
	}
}
