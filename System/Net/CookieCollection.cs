using System;
using System.Collections;
using System.Runtime.Serialization;

namespace System.Net
{
	// Token: 0x020000D6 RID: 214
	[global::__DynamicallyInvokable]
	[Serializable]
	public class CookieCollection : ICollection, IEnumerable
	{
		// Token: 0x06000739 RID: 1849 RVA: 0x00027F20 File Offset: 0x00026120
		[global::__DynamicallyInvokable]
		public CookieCollection()
		{
			this.m_IsReadOnly = true;
		}

		// Token: 0x0600073A RID: 1850 RVA: 0x00027F45 File Offset: 0x00026145
		internal CookieCollection(bool IsReadOnly)
		{
			this.m_IsReadOnly = IsReadOnly;
		}

		// Token: 0x17000152 RID: 338
		// (get) Token: 0x0600073B RID: 1851 RVA: 0x00027F6A File Offset: 0x0002616A
		public bool IsReadOnly
		{
			get
			{
				return this.m_IsReadOnly;
			}
		}

		// Token: 0x17000153 RID: 339
		public Cookie this[int index]
		{
			get
			{
				if (index < 0 || index >= this.m_list.Count)
				{
					throw new ArgumentOutOfRangeException("index");
				}
				return (Cookie)this.m_list[index];
			}
		}

		// Token: 0x17000154 RID: 340
		[global::__DynamicallyInvokable]
		public Cookie this[string name]
		{
			[global::__DynamicallyInvokable]
			get
			{
				foreach (object obj in this.m_list)
				{
					Cookie cookie = (Cookie)obj;
					if (string.Compare(cookie.Name, name, StringComparison.OrdinalIgnoreCase) == 0)
					{
						return cookie;
					}
				}
				return null;
			}
		}

		// Token: 0x0600073E RID: 1854 RVA: 0x0002800C File Offset: 0x0002620C
		[global::__DynamicallyInvokable]
		public void Add(Cookie cookie)
		{
			if (cookie == null)
			{
				throw new ArgumentNullException("cookie");
			}
			this.m_version++;
			int num = this.IndexOf(cookie);
			if (num == -1)
			{
				this.m_list.Add(cookie);
				return;
			}
			this.m_list[num] = cookie;
		}

		// Token: 0x0600073F RID: 1855 RVA: 0x0002805C File Offset: 0x0002625C
		[global::__DynamicallyInvokable]
		public void Add(CookieCollection cookies)
		{
			if (cookies == null)
			{
				throw new ArgumentNullException("cookies");
			}
			foreach (object obj in cookies)
			{
				Cookie cookie = (Cookie)obj;
				this.Add(cookie);
			}
		}

		// Token: 0x17000155 RID: 341
		// (get) Token: 0x06000740 RID: 1856 RVA: 0x000280C0 File Offset: 0x000262C0
		[global::__DynamicallyInvokable]
		public int Count
		{
			[global::__DynamicallyInvokable]
			get
			{
				return this.m_list.Count;
			}
		}

		// Token: 0x17000156 RID: 342
		// (get) Token: 0x06000741 RID: 1857 RVA: 0x000280CD File Offset: 0x000262CD
		public bool IsSynchronized
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000157 RID: 343
		// (get) Token: 0x06000742 RID: 1858 RVA: 0x000280D0 File Offset: 0x000262D0
		public object SyncRoot
		{
			get
			{
				return this;
			}
		}

		// Token: 0x06000743 RID: 1859 RVA: 0x000280D3 File Offset: 0x000262D3
		public void CopyTo(Array array, int index)
		{
			this.m_list.CopyTo(array, index);
		}

		// Token: 0x06000744 RID: 1860 RVA: 0x000280E2 File Offset: 0x000262E2
		public void CopyTo(Cookie[] array, int index)
		{
			this.m_list.CopyTo(array, index);
		}

		// Token: 0x06000745 RID: 1861 RVA: 0x000280F4 File Offset: 0x000262F4
		internal DateTime TimeStamp(CookieCollection.Stamp how)
		{
			switch (how)
			{
			case CookieCollection.Stamp.Set:
				this.m_TimeStamp = DateTime.Now;
				break;
			case CookieCollection.Stamp.SetToUnused:
				this.m_TimeStamp = DateTime.MinValue;
				break;
			case CookieCollection.Stamp.SetToMaxUsed:
				this.m_TimeStamp = DateTime.MaxValue;
				break;
			}
			return this.m_TimeStamp;
		}

		// Token: 0x17000158 RID: 344
		// (get) Token: 0x06000746 RID: 1862 RVA: 0x00028144 File Offset: 0x00026344
		internal bool IsOtherVersionSeen
		{
			get
			{
				return this.m_has_other_versions;
			}
		}

		// Token: 0x06000747 RID: 1863 RVA: 0x0002814C File Offset: 0x0002634C
		internal int InternalAdd(Cookie cookie, bool isStrict)
		{
			int num = 1;
			if (isStrict)
			{
				IComparer comparer = Cookie.GetComparer();
				int num2 = 0;
				foreach (object obj in this.m_list)
				{
					Cookie cookie2 = (Cookie)obj;
					if (comparer.Compare(cookie, cookie2) == 0)
					{
						num = 0;
						if (cookie2.Variant <= cookie.Variant)
						{
							this.m_list[num2] = cookie;
							break;
						}
						break;
					}
					else
					{
						num2++;
					}
				}
				if (num2 == this.m_list.Count)
				{
					this.m_list.Add(cookie);
				}
			}
			else
			{
				this.m_list.Add(cookie);
			}
			if (cookie.Version != 1)
			{
				this.m_has_other_versions = true;
			}
			return num;
		}

		// Token: 0x06000748 RID: 1864 RVA: 0x0002821C File Offset: 0x0002641C
		internal int IndexOf(Cookie cookie)
		{
			IComparer comparer = Cookie.GetComparer();
			int num = 0;
			foreach (object obj in this.m_list)
			{
				Cookie cookie2 = (Cookie)obj;
				if (comparer.Compare(cookie, cookie2) == 0)
				{
					return num;
				}
				num++;
			}
			return -1;
		}

		// Token: 0x06000749 RID: 1865 RVA: 0x00028290 File Offset: 0x00026490
		internal void RemoveAt(int idx)
		{
			this.m_list.RemoveAt(idx);
		}

		// Token: 0x0600074A RID: 1866 RVA: 0x0002829E File Offset: 0x0002649E
		[global::__DynamicallyInvokable]
		public IEnumerator GetEnumerator()
		{
			return new CookieCollection.CookieCollectionEnumerator(this);
		}

		// Token: 0x04000D08 RID: 3336
		internal int m_version;

		// Token: 0x04000D09 RID: 3337
		private ArrayList m_list = new ArrayList();

		// Token: 0x04000D0A RID: 3338
		private DateTime m_TimeStamp = DateTime.MinValue;

		// Token: 0x04000D0B RID: 3339
		private bool m_has_other_versions;

		// Token: 0x04000D0C RID: 3340
		[OptionalField]
		private bool m_IsReadOnly;

		// Token: 0x020006F3 RID: 1779
		internal enum Stamp
		{
			// Token: 0x0400308D RID: 12429
			Check,
			// Token: 0x0400308E RID: 12430
			Set,
			// Token: 0x0400308F RID: 12431
			SetToUnused,
			// Token: 0x04003090 RID: 12432
			SetToMaxUsed
		}

		// Token: 0x020006F4 RID: 1780
		private class CookieCollectionEnumerator : IEnumerator
		{
			// Token: 0x0600406F RID: 16495 RVA: 0x0010E47D File Offset: 0x0010C67D
			internal CookieCollectionEnumerator(CookieCollection cookies)
			{
				this.m_cookies = cookies;
				this.m_count = cookies.Count;
				this.m_version = cookies.m_version;
			}

			// Token: 0x17000EE5 RID: 3813
			// (get) Token: 0x06004070 RID: 16496 RVA: 0x0010E4AC File Offset: 0x0010C6AC
			object IEnumerator.Current
			{
				get
				{
					if (this.m_index < 0 || this.m_index >= this.m_count)
					{
						throw new InvalidOperationException(SR.GetString("InvalidOperation_EnumOpCantHappen"));
					}
					if (this.m_version != this.m_cookies.m_version)
					{
						throw new InvalidOperationException(SR.GetString("InvalidOperation_EnumFailedVersion"));
					}
					return this.m_cookies[this.m_index];
				}
			}

			// Token: 0x06004071 RID: 16497 RVA: 0x0010E514 File Offset: 0x0010C714
			bool IEnumerator.MoveNext()
			{
				if (this.m_version != this.m_cookies.m_version)
				{
					throw new InvalidOperationException(SR.GetString("InvalidOperation_EnumFailedVersion"));
				}
				int num = this.m_index + 1;
				this.m_index = num;
				if (num < this.m_count)
				{
					return true;
				}
				this.m_index = this.m_count;
				return false;
			}

			// Token: 0x06004072 RID: 16498 RVA: 0x0010E56C File Offset: 0x0010C76C
			void IEnumerator.Reset()
			{
				this.m_index = -1;
			}

			// Token: 0x04003091 RID: 12433
			private CookieCollection m_cookies;

			// Token: 0x04003092 RID: 12434
			private int m_count;

			// Token: 0x04003093 RID: 12435
			private int m_index = -1;

			// Token: 0x04003094 RID: 12436
			private int m_version;
		}
	}
}
