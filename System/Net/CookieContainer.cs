using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;

namespace System.Net
{
	// Token: 0x020000D8 RID: 216
	[global::__DynamicallyInvokable]
	[Serializable]
	public class CookieContainer
	{
		// Token: 0x0600074E RID: 1870 RVA: 0x000282C8 File Offset: 0x000264C8
		[global::__DynamicallyInvokable]
		public CookieContainer()
		{
			string domainName = IPGlobalProperties.InternalGetIPGlobalProperties().DomainName;
			if (domainName != null && domainName.Length > 1)
			{
				this.m_fqdnMyDomain = "." + domainName;
			}
		}

		// Token: 0x0600074F RID: 1871 RVA: 0x00028337 File Offset: 0x00026537
		public CookieContainer(int capacity)
			: this()
		{
			if (capacity <= 0)
			{
				throw new ArgumentException(SR.GetString("net_toosmall"), "Capacity");
			}
			this.m_maxCookies = capacity;
		}

		// Token: 0x06000750 RID: 1872 RVA: 0x00028360 File Offset: 0x00026560
		public CookieContainer(int capacity, int perDomainCapacity, int maxCookieSize)
			: this(capacity)
		{
			if (perDomainCapacity != 2147483647 && (perDomainCapacity <= 0 || perDomainCapacity > capacity))
			{
				throw new ArgumentOutOfRangeException("perDomainCapacity", SR.GetString("net_cookie_capacity_range", new object[] { "PerDomainCapacity", 0, capacity }));
			}
			this.m_maxCookiesPerDomain = perDomainCapacity;
			if (maxCookieSize <= 0)
			{
				throw new ArgumentException(SR.GetString("net_toosmall"), "MaxCookieSize");
			}
			this.m_maxCookieSize = maxCookieSize;
		}

		// Token: 0x1700015B RID: 347
		// (get) Token: 0x06000751 RID: 1873 RVA: 0x000283E0 File Offset: 0x000265E0
		// (set) Token: 0x06000752 RID: 1874 RVA: 0x000283E8 File Offset: 0x000265E8
		[global::__DynamicallyInvokable]
		public int Capacity
		{
			[global::__DynamicallyInvokable]
			get
			{
				return this.m_maxCookies;
			}
			[global::__DynamicallyInvokable]
			set
			{
				if (value <= 0 || (value < this.m_maxCookiesPerDomain && this.m_maxCookiesPerDomain != 2147483647))
				{
					throw new ArgumentOutOfRangeException("value", SR.GetString("net_cookie_capacity_range", new object[] { "Capacity", 0, this.m_maxCookiesPerDomain }));
				}
				if (value < this.m_maxCookies)
				{
					this.m_maxCookies = value;
					this.AgeCookies(null);
				}
				this.m_maxCookies = value;
			}
		}

		// Token: 0x1700015C RID: 348
		// (get) Token: 0x06000753 RID: 1875 RVA: 0x00028468 File Offset: 0x00026668
		[global::__DynamicallyInvokable]
		public int Count
		{
			[global::__DynamicallyInvokable]
			get
			{
				return this.m_count;
			}
		}

		// Token: 0x1700015D RID: 349
		// (get) Token: 0x06000754 RID: 1876 RVA: 0x00028470 File Offset: 0x00026670
		// (set) Token: 0x06000755 RID: 1877 RVA: 0x00028478 File Offset: 0x00026678
		[global::__DynamicallyInvokable]
		public int MaxCookieSize
		{
			[global::__DynamicallyInvokable]
			get
			{
				return this.m_maxCookieSize;
			}
			[global::__DynamicallyInvokable]
			set
			{
				if (value <= 0)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this.m_maxCookieSize = value;
			}
		}

		// Token: 0x1700015E RID: 350
		// (get) Token: 0x06000756 RID: 1878 RVA: 0x00028490 File Offset: 0x00026690
		// (set) Token: 0x06000757 RID: 1879 RVA: 0x00028498 File Offset: 0x00026698
		[global::__DynamicallyInvokable]
		public int PerDomainCapacity
		{
			[global::__DynamicallyInvokable]
			get
			{
				return this.m_maxCookiesPerDomain;
			}
			[global::__DynamicallyInvokable]
			set
			{
				if (value <= 0 || (value > this.m_maxCookies && value != 2147483647))
				{
					throw new ArgumentOutOfRangeException("value");
				}
				if (value < this.m_maxCookiesPerDomain)
				{
					this.m_maxCookiesPerDomain = value;
					this.AgeCookies(null);
				}
				this.m_maxCookiesPerDomain = value;
			}
		}

		// Token: 0x06000758 RID: 1880 RVA: 0x000284E4 File Offset: 0x000266E4
		public void Add(Cookie cookie)
		{
			if (cookie == null)
			{
				throw new ArgumentNullException("cookie");
			}
			if (cookie.Domain.Length == 0)
			{
				throw new ArgumentException(SR.GetString("net_emptystringcall"), "cookie.Domain");
			}
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(cookie.Secure ? Uri.UriSchemeHttps : Uri.UriSchemeHttp).Append(Uri.SchemeDelimiter);
			if (!cookie.DomainImplicit && cookie.Domain[0] == '.')
			{
				stringBuilder.Append("0");
			}
			stringBuilder.Append(cookie.Domain);
			if (cookie.PortList != null)
			{
				stringBuilder.Append(":").Append(cookie.PortList[0]);
			}
			stringBuilder.Append(cookie.Path);
			Uri uri;
			if (!Uri.TryCreate(stringBuilder.ToString(), UriKind.Absolute, out uri))
			{
				throw new CookieException(SR.GetString("net_cookie_attribute", new object[] { "Domain", cookie.Domain }));
			}
			Cookie cookie2 = cookie.Clone();
			cookie2.VerifySetDefaults(cookie2.Variant, uri, this.IsLocalDomain(uri.Host), this.m_fqdnMyDomain, true, true);
			this.Add(cookie2, true);
		}

		// Token: 0x06000759 RID: 1881 RVA: 0x00028614 File Offset: 0x00026814
		private void AddRemoveDomain(string key, PathList value)
		{
			object syncRoot = this.m_domainTable.SyncRoot;
			lock (syncRoot)
			{
				if (value == null)
				{
					this.m_domainTable.Remove(key);
				}
				else
				{
					this.m_domainTable[key] = value;
				}
			}
		}

		// Token: 0x0600075A RID: 1882 RVA: 0x00028674 File Offset: 0x00026874
		internal void Add(Cookie cookie, bool throwOnError)
		{
			if (cookie.Value.Length <= this.m_maxCookieSize)
			{
				try
				{
					object syncRoot = this.m_domainTable.SyncRoot;
					PathList pathList;
					lock (syncRoot)
					{
						pathList = (PathList)this.m_domainTable[cookie.DomainKey];
						if (pathList == null)
						{
							pathList = new PathList();
							this.AddRemoveDomain(cookie.DomainKey, pathList);
						}
					}
					int cookiesCount = pathList.GetCookiesCount();
					object syncRoot2 = pathList.SyncRoot;
					CookieCollection cookieCollection;
					lock (syncRoot2)
					{
						cookieCollection = (CookieCollection)pathList[cookie.Path];
						if (cookieCollection == null)
						{
							cookieCollection = new CookieCollection();
							pathList[cookie.Path] = cookieCollection;
						}
					}
					if (cookie.Expired)
					{
						CookieCollection cookieCollection2 = cookieCollection;
						lock (cookieCollection2)
						{
							int num = cookieCollection.IndexOf(cookie);
							if (num != -1)
							{
								cookieCollection.RemoveAt(num);
								this.m_count--;
							}
							goto IL_0197;
						}
					}
					if (cookiesCount < this.m_maxCookiesPerDomain || this.AgeCookies(cookie.DomainKey))
					{
						if (this.m_count < this.m_maxCookies || this.AgeCookies(null))
						{
							CookieCollection cookieCollection3 = cookieCollection;
							lock (cookieCollection3)
							{
								this.m_count += cookieCollection.InternalAdd(cookie, true);
							}
						}
					}
					IL_0197:;
				}
				catch (Exception ex)
				{
					if (ex is ThreadAbortException || ex is StackOverflowException || ex is OutOfMemoryException)
					{
						throw;
					}
					if (throwOnError)
					{
						throw new CookieException(SR.GetString("net_container_add_cookie"), ex);
					}
				}
				return;
			}
			if (throwOnError)
			{
				throw new CookieException(SR.GetString("net_cookie_size", new object[]
				{
					cookie.ToString(),
					this.m_maxCookieSize
				}));
			}
		}

		// Token: 0x0600075B RID: 1883 RVA: 0x000288CC File Offset: 0x00026ACC
		private bool AgeCookies(string domain)
		{
			if (this.m_maxCookies == 0 || this.m_maxCookiesPerDomain == 0)
			{
				this.m_domainTable = new Hashtable();
				this.m_count = 0;
				return false;
			}
			int num = 0;
			DateTime dateTime = DateTime.MaxValue;
			CookieCollection cookieCollection = null;
			int num2 = 0;
			int num3 = 0;
			float num4 = 1f;
			if (this.m_count > this.m_maxCookies)
			{
				num4 = (float)this.m_maxCookies / (float)this.m_count;
			}
			object syncRoot = this.m_domainTable.SyncRoot;
			lock (syncRoot)
			{
				foreach (object obj in this.m_domainTable)
				{
					DictionaryEntry dictionaryEntry = (DictionaryEntry)obj;
					PathList pathList;
					if (domain == null)
					{
						string text = (string)dictionaryEntry.Key;
						pathList = (PathList)dictionaryEntry.Value;
					}
					else
					{
						pathList = (PathList)this.m_domainTable[domain];
					}
					num2 = 0;
					object syncRoot2 = pathList.SyncRoot;
					lock (syncRoot2)
					{
						foreach (object obj2 in pathList.Values)
						{
							CookieCollection cookieCollection2 = (CookieCollection)obj2;
							num3 = this.ExpireCollection(cookieCollection2);
							num += num3;
							this.m_count -= num3;
							num2 += cookieCollection2.Count;
							DateTime dateTime2;
							if (cookieCollection2.Count > 0 && (dateTime2 = cookieCollection2.TimeStamp(CookieCollection.Stamp.Check)) < dateTime)
							{
								cookieCollection = cookieCollection2;
								dateTime = dateTime2;
							}
						}
					}
					int num5 = Math.Min((int)((float)num2 * num4), Math.Min(this.m_maxCookiesPerDomain, this.m_maxCookies) - 1);
					if (num2 > num5)
					{
						object syncRoot3 = pathList.SyncRoot;
						Array array;
						Array array2;
						lock (syncRoot3)
						{
							array = Array.CreateInstance(typeof(CookieCollection), pathList.Count);
							array2 = Array.CreateInstance(typeof(DateTime), pathList.Count);
							foreach (object obj3 in pathList.Values)
							{
								CookieCollection cookieCollection3 = (CookieCollection)obj3;
								array2.SetValue(cookieCollection3.TimeStamp(CookieCollection.Stamp.Check), num3);
								array.SetValue(cookieCollection3, num3);
								num3++;
							}
						}
						Array.Sort(array2, array);
						num3 = 0;
						for (int i = 0; i < array.Length; i++)
						{
							CookieCollection cookieCollection4 = (CookieCollection)array.GetValue(i);
							CookieCollection cookieCollection5 = cookieCollection4;
							lock (cookieCollection5)
							{
								while (num2 > num5 && cookieCollection4.Count > 0)
								{
									cookieCollection4.RemoveAt(0);
									num2--;
									this.m_count--;
									num++;
								}
							}
							if (num2 <= num5)
							{
								break;
							}
						}
						if (num2 > num5 && domain != null)
						{
							return false;
						}
					}
				}
			}
			if (domain != null)
			{
				return true;
			}
			if (num != 0)
			{
				return true;
			}
			if (dateTime == DateTime.MaxValue)
			{
				return false;
			}
			CookieCollection cookieCollection6 = cookieCollection;
			lock (cookieCollection6)
			{
				while (this.m_count >= this.m_maxCookies && cookieCollection.Count > 0)
				{
					cookieCollection.RemoveAt(0);
					this.m_count--;
				}
			}
			return true;
		}

		// Token: 0x0600075C RID: 1884 RVA: 0x00028D30 File Offset: 0x00026F30
		private int ExpireCollection(CookieCollection cc)
		{
			int num;
			lock (cc)
			{
				int count = cc.Count;
				for (int i = count - 1; i >= 0; i--)
				{
					Cookie cookie = cc[i];
					if (cookie.Expired)
					{
						cc.RemoveAt(i);
					}
				}
				num = count - cc.Count;
			}
			return num;
		}

		// Token: 0x0600075D RID: 1885 RVA: 0x00028DA0 File Offset: 0x00026FA0
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

		// Token: 0x0600075E RID: 1886 RVA: 0x00028E04 File Offset: 0x00027004
		internal bool IsLocalDomain(string host)
		{
			int num = host.IndexOf('.');
			if (num == -1)
			{
				return true;
			}
			if (host == "127.0.0.1" || host == "::1" || host == "0:0:0:0:0:0:0:1")
			{
				return true;
			}
			if (string.Compare(this.m_fqdnMyDomain, 0, host, num, this.m_fqdnMyDomain.Length, StringComparison.OrdinalIgnoreCase) == 0)
			{
				return true;
			}
			string[] array = host.Split(new char[] { '.' });
			if (array != null && array.Length == 4 && array[0] == "127")
			{
				int i = 1;
				while (i < 4)
				{
					switch (array[i].Length)
					{
					case 1:
						break;
					case 2:
						goto IL_00C3;
					case 3:
						if (array[i][2] >= '0' && array[i][2] <= '9')
						{
							goto IL_00C3;
						}
						goto IL_00FF;
					default:
						goto IL_00FF;
					}
					IL_00DD:
					if (array[i][0] >= '0' && array[i][0] <= '9')
					{
						i++;
						continue;
					}
					break;
					IL_00C3:
					if (array[i][1] >= '0' && array[i][1] <= '9')
					{
						goto IL_00DD;
					}
					break;
				}
				IL_00FF:
				if (i == 4)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600075F RID: 1887 RVA: 0x00028F18 File Offset: 0x00027118
		[global::__DynamicallyInvokable]
		public void Add(Uri uri, Cookie cookie)
		{
			if (uri == null)
			{
				throw new ArgumentNullException("uri");
			}
			if (cookie == null)
			{
				throw new ArgumentNullException("cookie");
			}
			Cookie cookie2 = cookie.Clone();
			cookie2.VerifySetDefaults(cookie2.Variant, uri, this.IsLocalDomain(uri.Host), this.m_fqdnMyDomain, true, true);
			this.Add(cookie2, true);
		}

		// Token: 0x06000760 RID: 1888 RVA: 0x00028F78 File Offset: 0x00027178
		[global::__DynamicallyInvokable]
		public void Add(Uri uri, CookieCollection cookies)
		{
			if (uri == null)
			{
				throw new ArgumentNullException("uri");
			}
			if (cookies == null)
			{
				throw new ArgumentNullException("cookies");
			}
			bool flag = this.IsLocalDomain(uri.Host);
			foreach (object obj in cookies)
			{
				Cookie cookie = (Cookie)obj;
				Cookie cookie2 = cookie.Clone();
				cookie2.VerifySetDefaults(cookie2.Variant, uri, flag, this.m_fqdnMyDomain, true, true);
				this.Add(cookie2, true);
			}
		}

		// Token: 0x06000761 RID: 1889 RVA: 0x00029020 File Offset: 0x00027220
		internal CookieCollection CookieCutter(Uri uri, string headerName, string setCookieHeader, bool isThrow)
		{
			CookieCollection cookieCollection = new CookieCollection();
			CookieVariant cookieVariant = CookieVariant.Unknown;
			if (headerName == null)
			{
				cookieVariant = CookieVariant.Rfc2109;
			}
			else
			{
				for (int i = 0; i < CookieContainer.HeaderInfo.Length; i++)
				{
					if (string.Compare(headerName, CookieContainer.HeaderInfo[i].Name, StringComparison.OrdinalIgnoreCase) == 0)
					{
						cookieVariant = CookieContainer.HeaderInfo[i].Variant;
					}
				}
			}
			bool flag = this.IsLocalDomain(uri.Host);
			try
			{
				CookieParser cookieParser = new CookieParser(setCookieHeader);
				for (;;)
				{
					Cookie cookie = cookieParser.Get();
					if (cookie == null)
					{
						if (cookieParser.EndofHeader())
						{
							break;
						}
					}
					else if (ValidationHelper.IsBlankString(cookie.Name))
					{
						if (isThrow)
						{
							goto Block_9;
						}
					}
					else if (cookie.VerifySetDefaults(cookieVariant, uri, flag, this.m_fqdnMyDomain, true, isThrow))
					{
						cookieCollection.InternalAdd(cookie, true);
					}
				}
				goto IL_0103;
				Block_9:
				throw new CookieException(SR.GetString("net_cookie_format"));
			}
			catch (Exception ex)
			{
				if (ex is ThreadAbortException || ex is StackOverflowException || ex is OutOfMemoryException)
				{
					throw;
				}
				if (isThrow)
				{
					throw new CookieException(SR.GetString("net_cookie_parse_header", new object[] { uri.AbsoluteUri }), ex);
				}
			}
			IL_0103:
			foreach (object obj in cookieCollection)
			{
				Cookie cookie2 = (Cookie)obj;
				this.Add(cookie2, isThrow);
			}
			return cookieCollection;
		}

		// Token: 0x06000762 RID: 1890 RVA: 0x00029190 File Offset: 0x00027390
		[global::__DynamicallyInvokable]
		public CookieCollection GetCookies(Uri uri)
		{
			if (uri == null)
			{
				throw new ArgumentNullException("uri");
			}
			return this.InternalGetCookies(uri);
		}

		// Token: 0x06000763 RID: 1891 RVA: 0x000291B0 File Offset: 0x000273B0
		internal CookieCollection InternalGetCookies(Uri uri)
		{
			bool flag = uri.Scheme == Uri.UriSchemeHttps;
			int port = uri.Port;
			CookieCollection cookieCollection = new CookieCollection();
			List<string> list = new List<string>();
			List<string> list2 = new List<string>();
			string host = uri.Host;
			list.Add(host);
			list.Add("." + host);
			int num = host.IndexOf('.');
			if (num == -1)
			{
				if (this.m_fqdnMyDomain != null && this.m_fqdnMyDomain.Length != 0)
				{
					list.Add(host + this.m_fqdnMyDomain);
					list.Add(this.m_fqdnMyDomain);
				}
			}
			else
			{
				list.Add(host.Substring(num));
				if (host.Length > 2)
				{
					int num2 = host.LastIndexOf('.', host.Length - 2);
					if (num2 > 0)
					{
						num2 = host.LastIndexOf('.', num2 - 1);
					}
					if (num2 != -1)
					{
						while (num < num2 && (num = host.IndexOf('.', num + 1)) != -1)
						{
							list2.Add(host.Substring(num));
						}
					}
				}
			}
			this.BuildCookieCollectionFromDomainMatches(uri, flag, port, cookieCollection, list, false);
			this.BuildCookieCollectionFromDomainMatches(uri, flag, port, cookieCollection, list2, true);
			return cookieCollection;
		}

		// Token: 0x06000764 RID: 1892 RVA: 0x000292E4 File Offset: 0x000274E4
		private void BuildCookieCollectionFromDomainMatches(Uri uri, bool isSecure, int port, CookieCollection cookies, List<string> domainAttribute, bool matchOnlyPlainCookie)
		{
			for (int i = 0; i < domainAttribute.Count; i++)
			{
				bool flag = false;
				bool flag2 = false;
				object syncRoot = this.m_domainTable.SyncRoot;
				PathList pathList;
				lock (syncRoot)
				{
					pathList = (PathList)this.m_domainTable[domainAttribute[i]];
				}
				if (pathList != null)
				{
					object syncRoot2 = pathList.SyncRoot;
					lock (syncRoot2)
					{
						foreach (object obj in pathList)
						{
							DictionaryEntry dictionaryEntry = (DictionaryEntry)obj;
							string text = (string)dictionaryEntry.Key;
							if (uri.AbsolutePath.StartsWith(CookieParser.CheckQuoted(text)))
							{
								flag = true;
								CookieCollection cookieCollection = (CookieCollection)dictionaryEntry.Value;
								cookieCollection.TimeStamp(CookieCollection.Stamp.Set);
								this.MergeUpdateCollections(cookies, cookieCollection, port, isSecure, matchOnlyPlainCookie);
								if (text == "/")
								{
									flag2 = true;
								}
							}
							else if (flag)
							{
								break;
							}
						}
					}
					if (!flag2)
					{
						CookieCollection cookieCollection2 = (CookieCollection)pathList["/"];
						if (cookieCollection2 != null)
						{
							cookieCollection2.TimeStamp(CookieCollection.Stamp.Set);
							this.MergeUpdateCollections(cookies, cookieCollection2, port, isSecure, matchOnlyPlainCookie);
						}
					}
					if (pathList.Count == 0)
					{
						this.AddRemoveDomain(domainAttribute[i], null);
					}
				}
			}
		}

		// Token: 0x06000765 RID: 1893 RVA: 0x0002947C File Offset: 0x0002767C
		private void MergeUpdateCollections(CookieCollection destination, CookieCollection source, int port, bool isSecure, bool isPlainOnly)
		{
			lock (source)
			{
				for (int i = 0; i < source.Count; i++)
				{
					bool flag2 = false;
					Cookie cookie = source[i];
					if (cookie.Expired)
					{
						source.RemoveAt(i);
						this.m_count--;
						i--;
					}
					else
					{
						if (!isPlainOnly || cookie.Variant == CookieVariant.Plain)
						{
							if (cookie.PortList != null)
							{
								foreach (int num in cookie.PortList)
								{
									if (num == port)
									{
										flag2 = true;
										break;
									}
								}
							}
							else
							{
								flag2 = true;
							}
						}
						if (cookie.Secure && !isSecure)
						{
							flag2 = false;
						}
						if (flag2)
						{
							destination.InternalAdd(cookie, false);
						}
					}
				}
			}
		}

		// Token: 0x06000766 RID: 1894 RVA: 0x0002955C File Offset: 0x0002775C
		[global::__DynamicallyInvokable]
		public string GetCookieHeader(Uri uri)
		{
			if (uri == null)
			{
				throw new ArgumentNullException("uri");
			}
			string text;
			return this.GetCookieHeader(uri, out text);
		}

		// Token: 0x06000767 RID: 1895 RVA: 0x00029588 File Offset: 0x00027788
		internal string GetCookieHeader(Uri uri, out string optCookie2)
		{
			CookieCollection cookieCollection = this.InternalGetCookies(uri);
			string text = string.Empty;
			string text2 = string.Empty;
			foreach (object obj in cookieCollection)
			{
				Cookie cookie = (Cookie)obj;
				text = text + text2 + cookie.ToString();
				text2 = "; ";
			}
			optCookie2 = (cookieCollection.IsOtherVersionSeen ? ("$Version=" + 1.ToString(NumberFormatInfo.InvariantInfo)) : string.Empty);
			return text;
		}

		// Token: 0x06000768 RID: 1896 RVA: 0x00029630 File Offset: 0x00027830
		[global::__DynamicallyInvokable]
		public void SetCookies(Uri uri, string cookieHeader)
		{
			if (uri == null)
			{
				throw new ArgumentNullException("uri");
			}
			if (cookieHeader == null)
			{
				throw new ArgumentNullException("cookieHeader");
			}
			this.CookieCutter(uri, null, cookieHeader, true);
		}

		// Token: 0x04000D0F RID: 3343
		[global::__DynamicallyInvokable]
		public const int DefaultCookieLimit = 300;

		// Token: 0x04000D10 RID: 3344
		[global::__DynamicallyInvokable]
		public const int DefaultPerDomainCookieLimit = 20;

		// Token: 0x04000D11 RID: 3345
		[global::__DynamicallyInvokable]
		public const int DefaultCookieLengthLimit = 4096;

		// Token: 0x04000D12 RID: 3346
		private static readonly HeaderVariantInfo[] HeaderInfo = new HeaderVariantInfo[]
		{
			new HeaderVariantInfo("Set-Cookie", CookieVariant.Rfc2109),
			new HeaderVariantInfo("Set-Cookie2", CookieVariant.Rfc2965)
		};

		// Token: 0x04000D13 RID: 3347
		private Hashtable m_domainTable = new Hashtable();

		// Token: 0x04000D14 RID: 3348
		private int m_maxCookieSize = 4096;

		// Token: 0x04000D15 RID: 3349
		private int m_maxCookies = 300;

		// Token: 0x04000D16 RID: 3350
		private int m_maxCookiesPerDomain = 20;

		// Token: 0x04000D17 RID: 3351
		private int m_count;

		// Token: 0x04000D18 RID: 3352
		private string m_fqdnMyDomain = string.Empty;
	}
}
