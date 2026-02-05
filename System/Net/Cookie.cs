using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.Serialization;

namespace System.Net
{
	// Token: 0x020000D1 RID: 209
	[global::__DynamicallyInvokable]
	[Serializable]
	public sealed class Cookie
	{
		// Token: 0x060006EB RID: 1771 RVA: 0x000262D8 File Offset: 0x000244D8
		[global::__DynamicallyInvokable]
		public Cookie()
		{
		}

		// Token: 0x060006EC RID: 1772 RVA: 0x0002636C File Offset: 0x0002456C
		[global::__DynamicallyInvokable]
		public Cookie(string name, string value)
		{
			this.Name = name;
			this.m_value = value;
		}

		// Token: 0x060006ED RID: 1773 RVA: 0x0002640C File Offset: 0x0002460C
		[global::__DynamicallyInvokable]
		public Cookie(string name, string value, string path)
			: this(name, value)
		{
			this.Path = path;
		}

		// Token: 0x060006EE RID: 1774 RVA: 0x0002641D File Offset: 0x0002461D
		[global::__DynamicallyInvokable]
		public Cookie(string name, string value, string path, string domain)
			: this(name, value, path)
		{
			this.Domain = domain;
		}

		// Token: 0x17000135 RID: 309
		// (get) Token: 0x060006EF RID: 1775 RVA: 0x00026430 File Offset: 0x00024630
		// (set) Token: 0x060006F0 RID: 1776 RVA: 0x00026438 File Offset: 0x00024638
		[global::__DynamicallyInvokable]
		public string Comment
		{
			[global::__DynamicallyInvokable]
			get
			{
				return this.m_comment;
			}
			[global::__DynamicallyInvokable]
			set
			{
				if (value == null)
				{
					value = string.Empty;
				}
				this.m_comment = value;
			}
		}

		// Token: 0x17000136 RID: 310
		// (get) Token: 0x060006F1 RID: 1777 RVA: 0x0002644B File Offset: 0x0002464B
		// (set) Token: 0x060006F2 RID: 1778 RVA: 0x00026453 File Offset: 0x00024653
		[global::__DynamicallyInvokable]
		public Uri CommentUri
		{
			[global::__DynamicallyInvokable]
			get
			{
				return this.m_commentUri;
			}
			[global::__DynamicallyInvokable]
			set
			{
				this.m_commentUri = value;
			}
		}

		// Token: 0x17000137 RID: 311
		// (get) Token: 0x060006F3 RID: 1779 RVA: 0x0002645C File Offset: 0x0002465C
		// (set) Token: 0x060006F4 RID: 1780 RVA: 0x00026464 File Offset: 0x00024664
		[global::__DynamicallyInvokable]
		public bool HttpOnly
		{
			[global::__DynamicallyInvokable]
			get
			{
				return this.m_httpOnly;
			}
			[global::__DynamicallyInvokable]
			set
			{
				this.m_httpOnly = value;
			}
		}

		// Token: 0x17000138 RID: 312
		// (get) Token: 0x060006F5 RID: 1781 RVA: 0x0002646D File Offset: 0x0002466D
		// (set) Token: 0x060006F6 RID: 1782 RVA: 0x00026475 File Offset: 0x00024675
		[global::__DynamicallyInvokable]
		public bool Discard
		{
			[global::__DynamicallyInvokable]
			get
			{
				return this.m_discard;
			}
			[global::__DynamicallyInvokable]
			set
			{
				this.m_discard = value;
			}
		}

		// Token: 0x17000139 RID: 313
		// (get) Token: 0x060006F7 RID: 1783 RVA: 0x0002647E File Offset: 0x0002467E
		// (set) Token: 0x060006F8 RID: 1784 RVA: 0x00026486 File Offset: 0x00024686
		[global::__DynamicallyInvokable]
		public string Domain
		{
			[global::__DynamicallyInvokable]
			get
			{
				return this.m_domain;
			}
			[global::__DynamicallyInvokable]
			set
			{
				this.m_domain = ((value == null) ? string.Empty : value);
				this.m_domain_implicit = false;
				this.m_domainKey = string.Empty;
			}
		}

		// Token: 0x1700013A RID: 314
		// (get) Token: 0x060006F9 RID: 1785 RVA: 0x000264AC File Offset: 0x000246AC
		private string _Domain
		{
			get
			{
				if (!this.Plain && !this.m_domain_implicit && this.m_domain.Length != 0)
				{
					return "$Domain=" + (this.IsQuotedDomain ? "\"" : string.Empty) + this.m_domain + (this.IsQuotedDomain ? "\"" : string.Empty);
				}
				return string.Empty;
			}
		}

		// Token: 0x1700013B RID: 315
		// (get) Token: 0x060006FA RID: 1786 RVA: 0x00026514 File Offset: 0x00024714
		// (set) Token: 0x060006FB RID: 1787 RVA: 0x0002651C File Offset: 0x0002471C
		internal bool DomainImplicit
		{
			get
			{
				return this.m_domain_implicit;
			}
			set
			{
				this.m_domain_implicit = value;
			}
		}

		// Token: 0x1700013C RID: 316
		// (get) Token: 0x060006FC RID: 1788 RVA: 0x00026525 File Offset: 0x00024725
		// (set) Token: 0x060006FD RID: 1789 RVA: 0x00026550 File Offset: 0x00024750
		[global::__DynamicallyInvokable]
		public bool Expired
		{
			[global::__DynamicallyInvokable]
			get
			{
				return this.m_expires != DateTime.MinValue && this.m_expires.ToLocalTime() <= DateTime.Now;
			}
			[global::__DynamicallyInvokable]
			set
			{
				if (value)
				{
					this.m_expires = DateTime.Now;
				}
			}
		}

		// Token: 0x1700013D RID: 317
		// (get) Token: 0x060006FE RID: 1790 RVA: 0x00026560 File Offset: 0x00024760
		// (set) Token: 0x060006FF RID: 1791 RVA: 0x00026568 File Offset: 0x00024768
		[global::__DynamicallyInvokable]
		public DateTime Expires
		{
			[global::__DynamicallyInvokable]
			get
			{
				return this.m_expires;
			}
			[global::__DynamicallyInvokable]
			set
			{
				this.m_expires = value;
			}
		}

		// Token: 0x1700013E RID: 318
		// (get) Token: 0x06000700 RID: 1792 RVA: 0x00026571 File Offset: 0x00024771
		// (set) Token: 0x06000701 RID: 1793 RVA: 0x00026579 File Offset: 0x00024779
		[global::__DynamicallyInvokable]
		public string Name
		{
			[global::__DynamicallyInvokable]
			get
			{
				return this.m_name;
			}
			[global::__DynamicallyInvokable]
			set
			{
				if (ValidationHelper.IsBlankString(value) || !this.InternalSetName(value))
				{
					throw new CookieException(SR.GetString("net_cookie_attribute", new object[]
					{
						"Name",
						(value == null) ? "<null>" : value
					}));
				}
			}
		}

		// Token: 0x06000702 RID: 1794 RVA: 0x000265B8 File Offset: 0x000247B8
		internal bool InternalSetName(string value)
		{
			if (ValidationHelper.IsBlankString(value) || value[0] == '$' || value.IndexOfAny(Cookie.Reserved2Name) != -1)
			{
				this.m_name = string.Empty;
				return false;
			}
			this.m_name = value;
			return true;
		}

		// Token: 0x1700013F RID: 319
		// (get) Token: 0x06000703 RID: 1795 RVA: 0x000265F0 File Offset: 0x000247F0
		// (set) Token: 0x06000704 RID: 1796 RVA: 0x000265F8 File Offset: 0x000247F8
		[global::__DynamicallyInvokable]
		public string Path
		{
			[global::__DynamicallyInvokable]
			get
			{
				return this.m_path;
			}
			[global::__DynamicallyInvokable]
			set
			{
				this.m_path = ((value == null) ? string.Empty : value);
				this.m_path_implicit = false;
			}
		}

		// Token: 0x17000140 RID: 320
		// (get) Token: 0x06000705 RID: 1797 RVA: 0x00026612 File Offset: 0x00024812
		private string _Path
		{
			get
			{
				if (!this.Plain && !this.m_path_implicit && this.m_path.Length != 0)
				{
					return "$Path=" + this.m_path;
				}
				return string.Empty;
			}
		}

		// Token: 0x17000141 RID: 321
		// (get) Token: 0x06000706 RID: 1798 RVA: 0x00026647 File Offset: 0x00024847
		internal bool Plain
		{
			get
			{
				return this.Variant == CookieVariant.Plain;
			}
		}

		// Token: 0x06000707 RID: 1799 RVA: 0x00026654 File Offset: 0x00024854
		internal Cookie Clone()
		{
			Cookie cookie = new Cookie(this.m_name, this.m_value);
			if (!this.m_port_implicit)
			{
				cookie.Port = this.m_port;
			}
			if (!this.m_path_implicit)
			{
				cookie.Path = this.m_path;
			}
			cookie.Domain = this.m_domain;
			cookie.DomainImplicit = this.m_domain_implicit;
			cookie.m_timeStamp = this.m_timeStamp;
			cookie.Comment = this.m_comment;
			cookie.CommentUri = this.m_commentUri;
			cookie.HttpOnly = this.m_httpOnly;
			cookie.Discard = this.m_discard;
			cookie.Expires = this.m_expires;
			cookie.Version = this.m_version;
			cookie.Secure = this.m_secure;
			cookie.m_cookieVariant = this.m_cookieVariant;
			return cookie;
		}

		// Token: 0x06000708 RID: 1800 RVA: 0x00026720 File Offset: 0x00024920
		private static bool IsDomainEqualToHost(string domain, string host)
		{
			return host.Length + 1 == domain.Length && string.Compare(host, 0, domain, 1, host.Length, StringComparison.OrdinalIgnoreCase) == 0;
		}

		// Token: 0x06000709 RID: 1801 RVA: 0x00026748 File Offset: 0x00024948
		internal bool VerifySetDefaults(CookieVariant variant, Uri uri, bool isLocalDomain, string localDomain, bool set_default, bool isThrow)
		{
			string host = uri.Host;
			int port = uri.Port;
			string absolutePath = uri.AbsolutePath;
			bool flag = true;
			if (set_default)
			{
				if (this.Version == 0)
				{
					variant = CookieVariant.Plain;
				}
				else if (this.Version == 1 && variant == CookieVariant.Unknown)
				{
					variant = CookieVariant.Rfc2109;
				}
				this.m_cookieVariant = variant;
			}
			if (this.m_name == null || this.m_name.Length == 0 || this.m_name[0] == '$' || this.m_name.IndexOfAny(Cookie.Reserved2Name) != -1)
			{
				if (isThrow)
				{
					throw new CookieException(SR.GetString("net_cookie_attribute", new object[]
					{
						"Name",
						(this.m_name == null) ? "<null>" : this.m_name
					}));
				}
				return false;
			}
			else if (this.m_value == null || ((this.m_value.Length <= 2 || this.m_value[0] != '"' || this.m_value[this.m_value.Length - 1] != '"') && this.m_value.IndexOfAny(Cookie.Reserved2Value) != -1))
			{
				if (isThrow)
				{
					throw new CookieException(SR.GetString("net_cookie_attribute", new object[]
					{
						"Value",
						(this.m_value == null) ? "<null>" : this.m_value
					}));
				}
				return false;
			}
			else if (this.Comment != null && (this.Comment.Length <= 2 || this.Comment[0] != '"' || this.Comment[this.Comment.Length - 1] != '"') && this.Comment.IndexOfAny(Cookie.Reserved2Value) != -1)
			{
				if (isThrow)
				{
					throw new CookieException(SR.GetString("net_cookie_attribute", new object[] { "Comment", this.Comment }));
				}
				return false;
			}
			else
			{
				if (this.Path == null || (this.Path.Length > 2 && this.Path[0] == '"' && this.Path[this.Path.Length - 1] == '"') || this.Path.IndexOfAny(Cookie.Reserved2Value) == -1)
				{
					if (set_default && this.m_domain_implicit)
					{
						this.m_domain = host;
					}
					else
					{
						if (!this.m_domain_implicit)
						{
							string text = this.m_domain;
							if (!Cookie.DomainCharsTest(text))
							{
								if (isThrow)
								{
									throw new CookieException(SR.GetString("net_cookie_attribute", new object[]
									{
										"Domain",
										(text == null) ? "<null>" : text
									}));
								}
								return false;
							}
							else
							{
								if (text[0] != '.')
								{
									if (variant != CookieVariant.Rfc2965 && variant != CookieVariant.Plain)
									{
										if (isThrow)
										{
											throw new CookieException(SR.GetString("net_cookie_attribute", new object[] { "Domain", this.m_domain }));
										}
										return false;
									}
									else
									{
										text = "." + text;
									}
								}
								int num = host.IndexOf('.');
								if (isLocalDomain && string.Compare(localDomain, text, StringComparison.OrdinalIgnoreCase) == 0)
								{
									flag = true;
								}
								else if (text.IndexOf('.', 1, text.Length - 2) == -1)
								{
									if (!Cookie.IsDomainEqualToHost(text, host))
									{
										flag = false;
									}
								}
								else if (variant == CookieVariant.Plain)
								{
									if (!Cookie.IsDomainEqualToHost(text, host) && (host.Length <= text.Length || string.Compare(host, host.Length - text.Length, text, 0, text.Length, StringComparison.OrdinalIgnoreCase) != 0))
									{
										flag = false;
									}
								}
								else if ((num == -1 || text.Length != host.Length - num || string.Compare(host, num, text, 0, text.Length, StringComparison.OrdinalIgnoreCase) != 0) && !Cookie.IsDomainEqualToHost(text, host))
								{
									flag = false;
								}
								if (flag)
								{
									this.m_domainKey = text.ToLower(CultureInfo.InvariantCulture);
								}
							}
						}
						else if (string.Compare(host, this.m_domain, StringComparison.OrdinalIgnoreCase) != 0)
						{
							flag = false;
						}
						if (!flag)
						{
							if (isThrow)
							{
								throw new CookieException(SR.GetString("net_cookie_attribute", new object[] { "Domain", this.m_domain }));
							}
							return false;
						}
					}
					if (set_default && this.m_path_implicit)
					{
						switch (this.m_cookieVariant)
						{
						case CookieVariant.Plain:
							this.m_path = absolutePath;
							goto IL_04B8;
						case CookieVariant.Rfc2109:
							this.m_path = absolutePath.Substring(0, absolutePath.LastIndexOf('/'));
							goto IL_04B8;
						}
						this.m_path = absolutePath.Substring(0, absolutePath.LastIndexOf('/') + 1);
					}
					else if (!absolutePath.StartsWith(CookieParser.CheckQuoted(this.m_path)))
					{
						if (isThrow)
						{
							throw new CookieException(SR.GetString("net_cookie_attribute", new object[] { "Path", this.m_path }));
						}
						return false;
					}
					IL_04B8:
					if (set_default && !this.m_port_implicit && this.m_port.Length == 0)
					{
						this.m_port_list = new int[] { port };
					}
					if (!this.m_port_implicit)
					{
						flag = false;
						foreach (int num2 in this.m_port_list)
						{
							if (num2 == port)
							{
								flag = true;
								break;
							}
						}
						if (!flag)
						{
							if (isThrow)
							{
								throw new CookieException(SR.GetString("net_cookie_attribute", new object[] { "Port", this.m_port }));
							}
							return false;
						}
					}
					return true;
				}
				if (isThrow)
				{
					throw new CookieException(SR.GetString("net_cookie_attribute", new object[] { "Path", this.Path }));
				}
				return false;
			}
		}

		// Token: 0x0600070A RID: 1802 RVA: 0x00026C9C File Offset: 0x00024E9C
		private static bool DomainCharsTest(string name)
		{
			if (name == null || name.Length == 0)
			{
				return false;
			}
			foreach (char c in name)
			{
				if ((c < '0' || c > '9') && c != '.' && c != '-' && (c < 'a' || c > 'z') && (c < 'A' || c > 'Z') && c != '_')
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x17000142 RID: 322
		// (get) Token: 0x0600070B RID: 1803 RVA: 0x00026CFF File Offset: 0x00024EFF
		// (set) Token: 0x0600070C RID: 1804 RVA: 0x00026D08 File Offset: 0x00024F08
		[global::__DynamicallyInvokable]
		public string Port
		{
			[global::__DynamicallyInvokable]
			get
			{
				return this.m_port;
			}
			[global::__DynamicallyInvokable]
			set
			{
				this.m_port_implicit = false;
				if (value == null || value.Length == 0)
				{
					this.m_port = string.Empty;
					return;
				}
				if (value[0] != '"' || value[value.Length - 1] != '"')
				{
					throw new CookieException(SR.GetString("net_cookie_attribute", new object[] { "Port", value }));
				}
				string[] array = value.Split(Cookie.PortSplitDelimiters);
				List<int> list = new List<int>();
				for (int i = 0; i < array.Length; i++)
				{
					if (array[i] != string.Empty)
					{
						int num;
						if (!int.TryParse(array[i], out num))
						{
							throw new CookieException(SR.GetString("net_cookie_attribute", new object[] { "Port", value }));
						}
						if (num < 0 || num > 65535)
						{
							throw new CookieException(SR.GetString("net_cookie_attribute", new object[] { "Port", value }));
						}
						list.Add(num);
					}
				}
				this.m_port_list = list.ToArray();
				this.m_port = value;
				this.m_version = 1;
				this.m_cookieVariant = CookieVariant.Rfc2965;
			}
		}

		// Token: 0x17000143 RID: 323
		// (get) Token: 0x0600070D RID: 1805 RVA: 0x00026E25 File Offset: 0x00025025
		internal int[] PortList
		{
			get
			{
				return this.m_port_list;
			}
		}

		// Token: 0x17000144 RID: 324
		// (get) Token: 0x0600070E RID: 1806 RVA: 0x00026E2D File Offset: 0x0002502D
		private string _Port
		{
			get
			{
				if (!this.m_port_implicit)
				{
					return "$Port" + ((this.m_port.Length == 0) ? string.Empty : ("=" + this.m_port));
				}
				return string.Empty;
			}
		}

		// Token: 0x17000145 RID: 325
		// (get) Token: 0x0600070F RID: 1807 RVA: 0x00026E6B File Offset: 0x0002506B
		// (set) Token: 0x06000710 RID: 1808 RVA: 0x00026E73 File Offset: 0x00025073
		[global::__DynamicallyInvokable]
		public bool Secure
		{
			[global::__DynamicallyInvokable]
			get
			{
				return this.m_secure;
			}
			[global::__DynamicallyInvokable]
			set
			{
				this.m_secure = value;
			}
		}

		// Token: 0x17000146 RID: 326
		// (get) Token: 0x06000711 RID: 1809 RVA: 0x00026E7C File Offset: 0x0002507C
		[global::__DynamicallyInvokable]
		public DateTime TimeStamp
		{
			[global::__DynamicallyInvokable]
			get
			{
				return this.m_timeStamp;
			}
		}

		// Token: 0x17000147 RID: 327
		// (get) Token: 0x06000712 RID: 1810 RVA: 0x00026E84 File Offset: 0x00025084
		// (set) Token: 0x06000713 RID: 1811 RVA: 0x00026E8C File Offset: 0x0002508C
		[global::__DynamicallyInvokable]
		public string Value
		{
			[global::__DynamicallyInvokable]
			get
			{
				return this.m_value;
			}
			[global::__DynamicallyInvokable]
			set
			{
				this.m_value = ((value == null) ? string.Empty : value);
			}
		}

		// Token: 0x17000148 RID: 328
		// (get) Token: 0x06000714 RID: 1812 RVA: 0x00026E9F File Offset: 0x0002509F
		// (set) Token: 0x06000715 RID: 1813 RVA: 0x00026EA7 File Offset: 0x000250A7
		internal CookieVariant Variant
		{
			get
			{
				return this.m_cookieVariant;
			}
			set
			{
				this.m_cookieVariant = value;
			}
		}

		// Token: 0x17000149 RID: 329
		// (get) Token: 0x06000716 RID: 1814 RVA: 0x00026EB0 File Offset: 0x000250B0
		internal string DomainKey
		{
			get
			{
				if (!this.m_domain_implicit)
				{
					return this.m_domainKey;
				}
				return this.Domain;
			}
		}

		// Token: 0x1700014A RID: 330
		// (get) Token: 0x06000717 RID: 1815 RVA: 0x00026EC7 File Offset: 0x000250C7
		// (set) Token: 0x06000718 RID: 1816 RVA: 0x00026ECF File Offset: 0x000250CF
		[global::__DynamicallyInvokable]
		public int Version
		{
			[global::__DynamicallyInvokable]
			get
			{
				return this.m_version;
			}
			[global::__DynamicallyInvokable]
			set
			{
				if (value < 0)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this.m_version = value;
				if (value > 0 && this.m_cookieVariant < CookieVariant.Rfc2109)
				{
					this.m_cookieVariant = CookieVariant.Rfc2109;
				}
			}
		}

		// Token: 0x1700014B RID: 331
		// (get) Token: 0x06000719 RID: 1817 RVA: 0x00026EFC File Offset: 0x000250FC
		private string _Version
		{
			get
			{
				if (this.Version != 0)
				{
					return "$Version=" + (this.IsQuotedVersion ? "\"" : string.Empty) + this.m_version.ToString(NumberFormatInfo.InvariantInfo) + (this.IsQuotedVersion ? "\"" : string.Empty);
				}
				return string.Empty;
			}
		}

		// Token: 0x0600071A RID: 1818 RVA: 0x00026F59 File Offset: 0x00025159
		internal static IComparer GetComparer()
		{
			return Cookie.staticComparer;
		}

		// Token: 0x0600071B RID: 1819 RVA: 0x00026F60 File Offset: 0x00025160
		[global::__DynamicallyInvokable]
		public override bool Equals(object comparand)
		{
			if (!(comparand is Cookie))
			{
				return false;
			}
			Cookie cookie = (Cookie)comparand;
			return string.Compare(this.Name, cookie.Name, StringComparison.OrdinalIgnoreCase) == 0 && string.Compare(this.Value, cookie.Value, StringComparison.Ordinal) == 0 && string.Compare(this.Path, cookie.Path, StringComparison.Ordinal) == 0 && string.Compare(this.Domain, cookie.Domain, StringComparison.OrdinalIgnoreCase) == 0 && this.Version == cookie.Version;
		}

		// Token: 0x0600071C RID: 1820 RVA: 0x00026FE0 File Offset: 0x000251E0
		[global::__DynamicallyInvokable]
		public override int GetHashCode()
		{
			return string.Concat(new string[]
			{
				this.Name,
				"=",
				this.Value,
				";",
				this.Path,
				"; ",
				this.Domain,
				"; ",
				this.Version.ToString()
			}).GetHashCode();
		}

		// Token: 0x0600071D RID: 1821 RVA: 0x00027054 File Offset: 0x00025254
		[global::__DynamicallyInvokable]
		public override string ToString()
		{
			string domain = this._Domain;
			string path = this._Path;
			string port = this._Port;
			string version = this._Version;
			string text = string.Concat(new string[]
			{
				(version.Length == 0) ? string.Empty : (version + "; "),
				this.Name,
				"=",
				this.Value,
				(path.Length == 0) ? string.Empty : ("; " + path),
				(domain.Length == 0) ? string.Empty : ("; " + domain),
				(port.Length == 0) ? string.Empty : ("; " + port)
			});
			if (text == "=")
			{
				return string.Empty;
			}
			return text;
		}

		// Token: 0x0600071E RID: 1822 RVA: 0x00027130 File Offset: 0x00025330
		internal string ToServerString()
		{
			string text = this.Name + "=" + this.Value;
			if (this.m_comment != null && this.m_comment.Length > 0)
			{
				text = text + "; Comment=" + this.m_comment;
			}
			if (this.m_commentUri != null)
			{
				text = text + "; CommentURL=\"" + this.m_commentUri.ToString() + "\"";
			}
			if (this.m_discard)
			{
				text += "; Discard";
			}
			if (!this.m_domain_implicit && this.m_domain != null && this.m_domain.Length > 0)
			{
				text = text + "; Domain=" + this.m_domain;
			}
			if (this.Expires != DateTime.MinValue)
			{
				int num = (int)(this.Expires.ToLocalTime() - DateTime.Now).TotalSeconds;
				if (num < 0)
				{
					num = 0;
				}
				text = text + "; Max-Age=" + num.ToString(NumberFormatInfo.InvariantInfo);
			}
			if (!this.m_path_implicit && this.m_path != null && this.m_path.Length > 0)
			{
				text = text + "; Path=" + this.m_path;
			}
			if (!this.Plain && !this.m_port_implicit && this.m_port != null && this.m_port.Length > 0)
			{
				text = text + "; Port=" + this.m_port;
			}
			if (this.m_version > 0)
			{
				text = text + "; Version=" + this.m_version.ToString(NumberFormatInfo.InvariantInfo);
			}
			if (!(text == "="))
			{
				return text;
			}
			return null;
		}

		// Token: 0x04000CBC RID: 3260
		internal const int MaxSupportedVersion = 1;

		// Token: 0x04000CBD RID: 3261
		internal const string CommentAttributeName = "Comment";

		// Token: 0x04000CBE RID: 3262
		internal const string CommentUrlAttributeName = "CommentURL";

		// Token: 0x04000CBF RID: 3263
		internal const string DiscardAttributeName = "Discard";

		// Token: 0x04000CC0 RID: 3264
		internal const string DomainAttributeName = "Domain";

		// Token: 0x04000CC1 RID: 3265
		internal const string ExpiresAttributeName = "Expires";

		// Token: 0x04000CC2 RID: 3266
		internal const string MaxAgeAttributeName = "Max-Age";

		// Token: 0x04000CC3 RID: 3267
		internal const string PathAttributeName = "Path";

		// Token: 0x04000CC4 RID: 3268
		internal const string PortAttributeName = "Port";

		// Token: 0x04000CC5 RID: 3269
		internal const string SecureAttributeName = "Secure";

		// Token: 0x04000CC6 RID: 3270
		internal const string VersionAttributeName = "Version";

		// Token: 0x04000CC7 RID: 3271
		internal const string HttpOnlyAttributeName = "HttpOnly";

		// Token: 0x04000CC8 RID: 3272
		internal const string SeparatorLiteral = "; ";

		// Token: 0x04000CC9 RID: 3273
		internal const string EqualsLiteral = "=";

		// Token: 0x04000CCA RID: 3274
		internal const string QuotesLiteral = "\"";

		// Token: 0x04000CCB RID: 3275
		internal const string SpecialAttributeLiteral = "$";

		// Token: 0x04000CCC RID: 3276
		internal static readonly char[] PortSplitDelimiters = new char[] { ' ', ',', '"' };

		// Token: 0x04000CCD RID: 3277
		internal static readonly char[] Reserved2Name = new char[] { ' ', '\t', '\r', '\n', '=', ';', ',' };

		// Token: 0x04000CCE RID: 3278
		internal static readonly char[] Reserved2Value = new char[] { ';', ',' };

		// Token: 0x04000CCF RID: 3279
		private static Comparer staticComparer = new Comparer();

		// Token: 0x04000CD0 RID: 3280
		private string m_comment = string.Empty;

		// Token: 0x04000CD1 RID: 3281
		private Uri m_commentUri;

		// Token: 0x04000CD2 RID: 3282
		private CookieVariant m_cookieVariant = CookieVariant.Plain;

		// Token: 0x04000CD3 RID: 3283
		private bool m_discard;

		// Token: 0x04000CD4 RID: 3284
		private string m_domain = string.Empty;

		// Token: 0x04000CD5 RID: 3285
		private bool m_domain_implicit = true;

		// Token: 0x04000CD6 RID: 3286
		private DateTime m_expires = DateTime.MinValue;

		// Token: 0x04000CD7 RID: 3287
		private string m_name = string.Empty;

		// Token: 0x04000CD8 RID: 3288
		private string m_path = string.Empty;

		// Token: 0x04000CD9 RID: 3289
		private bool m_path_implicit = true;

		// Token: 0x04000CDA RID: 3290
		private string m_port = string.Empty;

		// Token: 0x04000CDB RID: 3291
		private bool m_port_implicit = true;

		// Token: 0x04000CDC RID: 3292
		private int[] m_port_list;

		// Token: 0x04000CDD RID: 3293
		private bool m_secure;

		// Token: 0x04000CDE RID: 3294
		[OptionalField]
		private bool m_httpOnly;

		// Token: 0x04000CDF RID: 3295
		private DateTime m_timeStamp = DateTime.Now;

		// Token: 0x04000CE0 RID: 3296
		private string m_value = string.Empty;

		// Token: 0x04000CE1 RID: 3297
		private int m_version;

		// Token: 0x04000CE2 RID: 3298
		private string m_domainKey = string.Empty;

		// Token: 0x04000CE3 RID: 3299
		internal bool IsQuotedVersion;

		// Token: 0x04000CE4 RID: 3300
		internal bool IsQuotedDomain;
	}
}
