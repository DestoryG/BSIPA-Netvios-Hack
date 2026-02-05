using System;
using System.Globalization;
using System.Text;
using System.Threading;

namespace System
{
	// Token: 0x0200003E RID: 62
	[global::__DynamicallyInvokable]
	public class UriBuilder
	{
		// Token: 0x060003A1 RID: 929 RVA: 0x00019FE4 File Offset: 0x000181E4
		[global::__DynamicallyInvokable]
		public UriBuilder()
		{
		}

		// Token: 0x060003A2 RID: 930 RVA: 0x0001A060 File Offset: 0x00018260
		[global::__DynamicallyInvokable]
		public UriBuilder(string uri)
		{
			Uri uri2 = new Uri(uri, UriKind.RelativeOrAbsolute);
			if (uri2.IsAbsoluteUri)
			{
				this.Init(uri2);
				return;
			}
			uri = Uri.UriSchemeHttp + Uri.SchemeDelimiter + uri;
			this.Init(new Uri(uri));
		}

		// Token: 0x060003A3 RID: 931 RVA: 0x0001A110 File Offset: 0x00018310
		[global::__DynamicallyInvokable]
		public UriBuilder(Uri uri)
		{
			if (uri == null)
			{
				throw new ArgumentNullException("uri");
			}
			this.Init(uri);
		}

		// Token: 0x060003A4 RID: 932 RVA: 0x0001A1A0 File Offset: 0x000183A0
		private void Init(Uri uri)
		{
			this.m_fragment = uri.Fragment;
			this.m_query = uri.Query;
			this.m_host = uri.Host;
			this.m_path = uri.AbsolutePath;
			this.m_port = uri.Port;
			this.m_scheme = uri.Scheme;
			this.m_schemeDelimiter = (uri.HasAuthority ? Uri.SchemeDelimiter : ":");
			string userInfo = uri.UserInfo;
			if (!string.IsNullOrEmpty(userInfo))
			{
				int num = userInfo.IndexOf(':');
				if (num != -1)
				{
					this.m_password = userInfo.Substring(num + 1);
					this.m_username = userInfo.Substring(0, num);
				}
				else
				{
					this.m_username = userInfo;
				}
			}
			this.SetFieldsFromUri(uri);
		}

		// Token: 0x060003A5 RID: 933 RVA: 0x0001A258 File Offset: 0x00018458
		[global::__DynamicallyInvokable]
		public UriBuilder(string schemeName, string hostName)
		{
			this.Scheme = schemeName;
			this.Host = hostName;
		}

		// Token: 0x060003A6 RID: 934 RVA: 0x0001A2DF File Offset: 0x000184DF
		[global::__DynamicallyInvokable]
		public UriBuilder(string scheme, string host, int portNumber)
			: this(scheme, host)
		{
			this.Port = portNumber;
		}

		// Token: 0x060003A7 RID: 935 RVA: 0x0001A2F0 File Offset: 0x000184F0
		[global::__DynamicallyInvokable]
		public UriBuilder(string scheme, string host, int port, string pathValue)
			: this(scheme, host, port)
		{
			this.Path = pathValue;
		}

		// Token: 0x060003A8 RID: 936 RVA: 0x0001A304 File Offset: 0x00018504
		[global::__DynamicallyInvokable]
		public UriBuilder(string scheme, string host, int port, string path, string extraValue)
			: this(scheme, host, port, path)
		{
			try
			{
				this.Extra = extraValue;
			}
			catch (Exception ex)
			{
				if (ex is ThreadAbortException || ex is StackOverflowException || ex is OutOfMemoryException)
				{
					throw;
				}
				throw new ArgumentException("extraValue");
			}
		}

		// Token: 0x17000079 RID: 121
		// (set) Token: 0x060003A9 RID: 937 RVA: 0x0001A35C File Offset: 0x0001855C
		private string Extra
		{
			set
			{
				if (value == null)
				{
					value = string.Empty;
				}
				if (value.Length <= 0)
				{
					this.Fragment = string.Empty;
					this.Query = string.Empty;
					return;
				}
				if (value[0] == '#')
				{
					this.Fragment = value.Substring(1);
					return;
				}
				if (value[0] == '?')
				{
					int num = value.IndexOf('#');
					if (num == -1)
					{
						num = value.Length;
					}
					else
					{
						this.Fragment = value.Substring(num + 1);
					}
					this.Query = value.Substring(1, num - 1);
					return;
				}
				throw new ArgumentException("value");
			}
		}

		// Token: 0x1700007A RID: 122
		// (get) Token: 0x060003AA RID: 938 RVA: 0x0001A3F7 File Offset: 0x000185F7
		// (set) Token: 0x060003AB RID: 939 RVA: 0x0001A3FF File Offset: 0x000185FF
		[global::__DynamicallyInvokable]
		public string Fragment
		{
			[global::__DynamicallyInvokable]
			get
			{
				return this.m_fragment;
			}
			[global::__DynamicallyInvokable]
			set
			{
				if (value == null)
				{
					value = string.Empty;
				}
				if (value.Length > 0)
				{
					value = "#" + value;
				}
				this.m_fragment = value;
				this.m_changed = true;
			}
		}

		// Token: 0x1700007B RID: 123
		// (get) Token: 0x060003AC RID: 940 RVA: 0x0001A42F File Offset: 0x0001862F
		// (set) Token: 0x060003AD RID: 941 RVA: 0x0001A438 File Offset: 0x00018638
		[global::__DynamicallyInvokable]
		public string Host
		{
			[global::__DynamicallyInvokable]
			get
			{
				return this.m_host;
			}
			[global::__DynamicallyInvokable]
			set
			{
				if (value == null)
				{
					value = string.Empty;
				}
				this.m_host = value;
				if (this.m_host.IndexOf(':') >= 0 && this.m_host[0] != '[')
				{
					this.m_host = "[" + this.m_host + "]";
				}
				this.m_changed = true;
			}
		}

		// Token: 0x1700007C RID: 124
		// (get) Token: 0x060003AE RID: 942 RVA: 0x0001A498 File Offset: 0x00018698
		// (set) Token: 0x060003AF RID: 943 RVA: 0x0001A4A0 File Offset: 0x000186A0
		[global::__DynamicallyInvokable]
		public string Password
		{
			[global::__DynamicallyInvokable]
			get
			{
				return this.m_password;
			}
			[global::__DynamicallyInvokable]
			set
			{
				if (value == null)
				{
					value = string.Empty;
				}
				this.m_password = value;
				this.m_changed = true;
			}
		}

		// Token: 0x1700007D RID: 125
		// (get) Token: 0x060003B0 RID: 944 RVA: 0x0001A4BA File Offset: 0x000186BA
		// (set) Token: 0x060003B1 RID: 945 RVA: 0x0001A4C2 File Offset: 0x000186C2
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
				if (value == null || value.Length == 0)
				{
					value = "/";
				}
				this.m_path = Uri.InternalEscapeString(this.ConvertSlashes(value));
				this.m_changed = true;
			}
		}

		// Token: 0x1700007E RID: 126
		// (get) Token: 0x060003B2 RID: 946 RVA: 0x0001A4EF File Offset: 0x000186EF
		// (set) Token: 0x060003B3 RID: 947 RVA: 0x0001A4F7 File Offset: 0x000186F7
		[global::__DynamicallyInvokable]
		public int Port
		{
			[global::__DynamicallyInvokable]
			get
			{
				return this.m_port;
			}
			[global::__DynamicallyInvokable]
			set
			{
				if (value < -1 || value > 65535)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this.m_port = value;
				this.m_changed = true;
			}
		}

		// Token: 0x1700007F RID: 127
		// (get) Token: 0x060003B4 RID: 948 RVA: 0x0001A51E File Offset: 0x0001871E
		// (set) Token: 0x060003B5 RID: 949 RVA: 0x0001A526 File Offset: 0x00018726
		[global::__DynamicallyInvokable]
		public string Query
		{
			[global::__DynamicallyInvokable]
			get
			{
				return this.m_query;
			}
			[global::__DynamicallyInvokable]
			set
			{
				if (value == null)
				{
					value = string.Empty;
				}
				if (value.Length > 0)
				{
					value = "?" + value;
				}
				this.m_query = value;
				this.m_changed = true;
			}
		}

		// Token: 0x17000080 RID: 128
		// (get) Token: 0x060003B6 RID: 950 RVA: 0x0001A556 File Offset: 0x00018756
		// (set) Token: 0x060003B7 RID: 951 RVA: 0x0001A560 File Offset: 0x00018760
		[global::__DynamicallyInvokable]
		public string Scheme
		{
			[global::__DynamicallyInvokable]
			get
			{
				return this.m_scheme;
			}
			[global::__DynamicallyInvokable]
			set
			{
				if (value == null)
				{
					value = string.Empty;
				}
				int num = value.IndexOf(':');
				if (num != -1)
				{
					value = value.Substring(0, num);
				}
				if (value.Length != 0)
				{
					if (!Uri.CheckSchemeName(value))
					{
						throw new ArgumentException("value");
					}
					value = value.ToLower(CultureInfo.InvariantCulture);
				}
				this.m_scheme = value;
				this.m_changed = true;
			}
		}

		// Token: 0x17000081 RID: 129
		// (get) Token: 0x060003B8 RID: 952 RVA: 0x0001A5C4 File Offset: 0x000187C4
		[global::__DynamicallyInvokable]
		public Uri Uri
		{
			[global::__DynamicallyInvokable]
			get
			{
				if (this.m_changed)
				{
					this.m_uri = new Uri(this.ToString());
					this.SetFieldsFromUri(this.m_uri);
					this.m_changed = false;
				}
				return this.m_uri;
			}
		}

		// Token: 0x17000082 RID: 130
		// (get) Token: 0x060003B9 RID: 953 RVA: 0x0001A5F8 File Offset: 0x000187F8
		// (set) Token: 0x060003BA RID: 954 RVA: 0x0001A600 File Offset: 0x00018800
		[global::__DynamicallyInvokable]
		public string UserName
		{
			[global::__DynamicallyInvokable]
			get
			{
				return this.m_username;
			}
			[global::__DynamicallyInvokable]
			set
			{
				if (value == null)
				{
					value = string.Empty;
				}
				this.m_username = value;
				this.m_changed = true;
			}
		}

		// Token: 0x060003BB RID: 955 RVA: 0x0001A61C File Offset: 0x0001881C
		private string ConvertSlashes(string path)
		{
			StringBuilder stringBuilder = new StringBuilder(path.Length);
			foreach (char c in path)
			{
				if (c == '\\')
				{
					c = '/';
				}
				stringBuilder.Append(c);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x060003BC RID: 956 RVA: 0x0001A664 File Offset: 0x00018864
		[global::__DynamicallyInvokable]
		public override bool Equals(object rparam)
		{
			return rparam != null && this.Uri.Equals(rparam.ToString());
		}

		// Token: 0x060003BD RID: 957 RVA: 0x0001A67C File Offset: 0x0001887C
		[global::__DynamicallyInvokable]
		public override int GetHashCode()
		{
			return this.Uri.GetHashCode();
		}

		// Token: 0x060003BE RID: 958 RVA: 0x0001A68C File Offset: 0x0001888C
		private void SetFieldsFromUri(Uri uri)
		{
			this.m_fragment = uri.Fragment;
			this.m_query = uri.Query;
			this.m_host = uri.Host;
			this.m_path = uri.AbsolutePath;
			this.m_port = uri.Port;
			this.m_scheme = uri.Scheme;
			this.m_schemeDelimiter = (uri.HasAuthority ? Uri.SchemeDelimiter : ":");
			string userInfo = uri.UserInfo;
			if (userInfo.Length > 0)
			{
				int num = userInfo.IndexOf(':');
				if (num != -1)
				{
					this.m_password = userInfo.Substring(num + 1);
					this.m_username = userInfo.Substring(0, num);
					return;
				}
				this.m_username = userInfo;
			}
		}

		// Token: 0x060003BF RID: 959 RVA: 0x0001A740 File Offset: 0x00018940
		[global::__DynamicallyInvokable]
		public override string ToString()
		{
			if (this.m_username.Length == 0 && this.m_password.Length > 0)
			{
				throw new UriFormatException(SR.GetString("net_uri_BadUserPassword"));
			}
			if (this.m_scheme.Length != 0)
			{
				UriParser syntax = UriParser.GetSyntax(this.m_scheme);
				if (syntax != null)
				{
					this.m_schemeDelimiter = ((syntax.InFact(UriSyntaxFlags.MustHaveAuthority) || (this.m_host.Length != 0 && syntax.NotAny(UriSyntaxFlags.MailToLikeUri) && syntax.InFact(UriSyntaxFlags.OptionalAuthority))) ? Uri.SchemeDelimiter : ":");
				}
				else
				{
					this.m_schemeDelimiter = ((this.m_host.Length != 0) ? Uri.SchemeDelimiter : ":");
				}
			}
			string text = ((this.m_scheme.Length != 0) ? (this.m_scheme + this.m_schemeDelimiter) : string.Empty);
			return string.Concat(new string[]
			{
				text,
				this.m_username,
				(this.m_password.Length > 0) ? (":" + this.m_password) : string.Empty,
				(this.m_username.Length > 0) ? "@" : string.Empty,
				this.m_host,
				(this.m_port != -1 && this.m_host.Length > 0) ? (":" + this.m_port.ToString()) : string.Empty,
				(this.m_host.Length > 0 && this.m_path.Length != 0 && this.m_path[0] != '/') ? "/" : string.Empty,
				this.m_path,
				this.m_query,
				this.m_fragment
			});
		}

		// Token: 0x04000436 RID: 1078
		private bool m_changed = true;

		// Token: 0x04000437 RID: 1079
		private string m_fragment = string.Empty;

		// Token: 0x04000438 RID: 1080
		private string m_host = "localhost";

		// Token: 0x04000439 RID: 1081
		private string m_password = string.Empty;

		// Token: 0x0400043A RID: 1082
		private string m_path = "/";

		// Token: 0x0400043B RID: 1083
		private int m_port = -1;

		// Token: 0x0400043C RID: 1084
		private string m_query = string.Empty;

		// Token: 0x0400043D RID: 1085
		private string m_scheme = "http";

		// Token: 0x0400043E RID: 1086
		private string m_schemeDelimiter = Uri.SchemeDelimiter;

		// Token: 0x0400043F RID: 1087
		private Uri m_uri;

		// Token: 0x04000440 RID: 1088
		private string m_username = string.Empty;
	}
}
