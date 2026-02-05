using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics.Tracing;
using System.IO;
using System.Net.Cache;
using System.Net.Configuration;
using System.Net.Security;
using System.Reflection;
using System.Runtime.Serialization;
using System.Security;
using System.Security.Permissions;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;

namespace System.Net
{
	// Token: 0x0200018A RID: 394
	[global::__DynamicallyInvokable]
	[Serializable]
	public abstract class WebRequest : MarshalByRefObject, ISerializable
	{
		// Token: 0x17000344 RID: 836
		// (get) Token: 0x06000ECE RID: 3790 RVA: 0x0004DC23 File Offset: 0x0004BE23
		[Obsolete("This API supports the .NET Framework infrastructure and is not intended to be used directly from your code.", true)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public virtual IWebRequestCreate CreatorInstance
		{
			get
			{
				return WebRequest.webRequestCreate;
			}
		}

		// Token: 0x06000ECF RID: 3791 RVA: 0x0004DC2A File Offset: 0x0004BE2A
		[Obsolete("This API supports the .NET Framework infrastructure and is not intended to be used directly from your code.", true)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public static void RegisterPortableWebRequestCreator(IWebRequestCreate creator)
		{
		}

		// Token: 0x17000345 RID: 837
		// (get) Token: 0x06000ED0 RID: 3792 RVA: 0x0004DC2C File Offset: 0x0004BE2C
		private static object InternalSyncObject
		{
			get
			{
				if (WebRequest.s_InternalSyncObject == null)
				{
					object obj = new object();
					Interlocked.CompareExchange(ref WebRequest.s_InternalSyncObject, obj, null);
				}
				return WebRequest.s_InternalSyncObject;
			}
		}

		// Token: 0x17000346 RID: 838
		// (get) Token: 0x06000ED1 RID: 3793 RVA: 0x0004DC58 File Offset: 0x0004BE58
		internal static TimerThread.Queue DefaultTimerQueue
		{
			get
			{
				return WebRequest.s_DefaultTimerQueue;
			}
		}

		// Token: 0x06000ED2 RID: 3794 RVA: 0x0004DC60 File Offset: 0x0004BE60
		private static WebRequest Create(Uri requestUri, bool useUriBase)
		{
			if (Logging.On)
			{
				Logging.Enter(Logging.Web, "WebRequest", "Create", requestUri.ToString());
			}
			WebRequestPrefixElement webRequestPrefixElement = null;
			bool flag = false;
			string text;
			if (!useUriBase)
			{
				text = requestUri.AbsoluteUri;
			}
			else
			{
				text = requestUri.Scheme + ":";
			}
			int length = text.Length;
			ArrayList prefixList = WebRequest.PrefixList;
			for (int i = 0; i < prefixList.Count; i++)
			{
				webRequestPrefixElement = (WebRequestPrefixElement)prefixList[i];
				if (length >= webRequestPrefixElement.Prefix.Length && string.Compare(webRequestPrefixElement.Prefix, 0, text, 0, webRequestPrefixElement.Prefix.Length, StringComparison.OrdinalIgnoreCase) == 0)
				{
					flag = true;
					break;
				}
			}
			if (flag)
			{
				WebRequest webRequest = webRequestPrefixElement.Creator.Create(requestUri);
				if (Logging.On)
				{
					Logging.Exit(Logging.Web, "WebRequest", "Create", webRequest);
				}
				return webRequest;
			}
			if (Logging.On)
			{
				Logging.Exit(Logging.Web, "WebRequest", "Create", null);
			}
			throw new NotSupportedException(SR.GetString("net_unknown_prefix"));
		}

		// Token: 0x06000ED3 RID: 3795 RVA: 0x0004DD6F File Offset: 0x0004BF6F
		[global::__DynamicallyInvokable]
		public static WebRequest Create(string requestUriString)
		{
			if (requestUriString == null)
			{
				throw new ArgumentNullException("requestUriString");
			}
			return WebRequest.Create(new Uri(requestUriString), false);
		}

		// Token: 0x06000ED4 RID: 3796 RVA: 0x0004DD8B File Offset: 0x0004BF8B
		[global::__DynamicallyInvokable]
		public static WebRequest Create(Uri requestUri)
		{
			if (requestUri == null)
			{
				throw new ArgumentNullException("requestUri");
			}
			return WebRequest.Create(requestUri, false);
		}

		// Token: 0x06000ED5 RID: 3797 RVA: 0x0004DDA8 File Offset: 0x0004BFA8
		public static WebRequest CreateDefault(Uri requestUri)
		{
			if (requestUri == null)
			{
				throw new ArgumentNullException("requestUri");
			}
			return WebRequest.Create(requestUri, true);
		}

		// Token: 0x06000ED6 RID: 3798 RVA: 0x0004DDC5 File Offset: 0x0004BFC5
		[global::__DynamicallyInvokable]
		public static HttpWebRequest CreateHttp(string requestUriString)
		{
			if (requestUriString == null)
			{
				throw new ArgumentNullException("requestUriString");
			}
			return WebRequest.CreateHttp(new Uri(requestUriString));
		}

		// Token: 0x06000ED7 RID: 3799 RVA: 0x0004DDE0 File Offset: 0x0004BFE0
		[global::__DynamicallyInvokable]
		public static HttpWebRequest CreateHttp(Uri requestUri)
		{
			if (requestUri == null)
			{
				throw new ArgumentNullException("requestUri");
			}
			if (requestUri.Scheme != Uri.UriSchemeHttp && requestUri.Scheme != Uri.UriSchemeHttps)
			{
				throw new NotSupportedException(SR.GetString("net_unknown_prefix"));
			}
			return (HttpWebRequest)WebRequest.CreateDefault(requestUri);
		}

		// Token: 0x06000ED8 RID: 3800 RVA: 0x0004DE40 File Offset: 0x0004C040
		[global::__DynamicallyInvokable]
		public static bool RegisterPrefix(string prefix, IWebRequestCreate creator)
		{
			bool flag = false;
			if (prefix == null)
			{
				throw new ArgumentNullException("prefix");
			}
			if (creator == null)
			{
				throw new ArgumentNullException("creator");
			}
			ExceptionHelper.WebPermissionUnrestricted.Demand();
			object internalSyncObject = WebRequest.InternalSyncObject;
			lock (internalSyncObject)
			{
				ArrayList arrayList = (ArrayList)WebRequest.PrefixList.Clone();
				Uri uri;
				if (Uri.TryCreate(prefix, UriKind.Absolute, out uri))
				{
					string text = uri.AbsoluteUri;
					if (!prefix.EndsWith("/", StringComparison.Ordinal) && uri.GetComponents(UriComponents.Path | UriComponents.Query | UriComponents.Fragment, UriFormat.UriEscaped).Equals("/"))
					{
						text = text.Substring(0, text.Length - 1);
					}
					prefix = text;
				}
				int i;
				for (i = 0; i < arrayList.Count; i++)
				{
					WebRequestPrefixElement webRequestPrefixElement = (WebRequestPrefixElement)arrayList[i];
					if (prefix.Length > webRequestPrefixElement.Prefix.Length)
					{
						break;
					}
					if (prefix.Length == webRequestPrefixElement.Prefix.Length && string.Compare(webRequestPrefixElement.Prefix, prefix, StringComparison.OrdinalIgnoreCase) == 0)
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					arrayList.Insert(i, new WebRequestPrefixElement(prefix, creator));
					WebRequest.PrefixList = arrayList;
				}
			}
			return !flag;
		}

		// Token: 0x17000347 RID: 839
		// (get) Token: 0x06000ED9 RID: 3801 RVA: 0x0004DF7C File Offset: 0x0004C17C
		// (set) Token: 0x06000EDA RID: 3802 RVA: 0x0004DFE0 File Offset: 0x0004C1E0
		internal static ArrayList PrefixList
		{
			get
			{
				if (WebRequest.s_PrefixList == null)
				{
					object internalSyncObject = WebRequest.InternalSyncObject;
					lock (internalSyncObject)
					{
						if (WebRequest.s_PrefixList == null)
						{
							WebRequest.s_PrefixList = WebRequestModulesSectionInternal.GetSection().WebRequestModules;
						}
					}
				}
				return WebRequest.s_PrefixList;
			}
			set
			{
				WebRequest.s_PrefixList = value;
			}
		}

		// Token: 0x06000EDB RID: 3803 RVA: 0x0004DFEA File Offset: 0x0004C1EA
		[global::__DynamicallyInvokable]
		protected WebRequest()
		{
			this.m_ImpersonationLevel = TokenImpersonationLevel.Delegation;
			this.m_AuthenticationLevel = AuthenticationLevel.MutualAuthRequested;
		}

		// Token: 0x06000EDC RID: 3804 RVA: 0x0004E000 File Offset: 0x0004C200
		protected WebRequest(SerializationInfo serializationInfo, StreamingContext streamingContext)
		{
		}

		// Token: 0x06000EDD RID: 3805 RVA: 0x0004E008 File Offset: 0x0004C208
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter, SerializationFormatter = true)]
		void ISerializable.GetObjectData(SerializationInfo serializationInfo, StreamingContext streamingContext)
		{
			this.GetObjectData(serializationInfo, streamingContext);
		}

		// Token: 0x06000EDE RID: 3806 RVA: 0x0004E012 File Offset: 0x0004C212
		[SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
		protected virtual void GetObjectData(SerializationInfo serializationInfo, StreamingContext streamingContext)
		{
		}

		// Token: 0x17000348 RID: 840
		// (get) Token: 0x06000EDF RID: 3807 RVA: 0x0004E014 File Offset: 0x0004C214
		// (set) Token: 0x06000EE0 RID: 3808 RVA: 0x0004E028 File Offset: 0x0004C228
		public static RequestCachePolicy DefaultCachePolicy
		{
			get
			{
				return RequestCacheManager.GetBinding(string.Empty).Policy;
			}
			set
			{
				ExceptionHelper.WebPermissionUnrestricted.Demand();
				RequestCacheBinding binding = RequestCacheManager.GetBinding(string.Empty);
				RequestCacheManager.SetBinding(string.Empty, new RequestCacheBinding(binding.Cache, binding.Validator, value));
			}
		}

		// Token: 0x17000349 RID: 841
		// (get) Token: 0x06000EE1 RID: 3809 RVA: 0x0004E066 File Offset: 0x0004C266
		// (set) Token: 0x06000EE2 RID: 3810 RVA: 0x0004E06E File Offset: 0x0004C26E
		public virtual RequestCachePolicy CachePolicy
		{
			get
			{
				return this.m_CachePolicy;
			}
			set
			{
				this.InternalSetCachePolicy(value);
			}
		}

		// Token: 0x06000EE3 RID: 3811 RVA: 0x0004E078 File Offset: 0x0004C278
		private void InternalSetCachePolicy(RequestCachePolicy policy)
		{
			if (this.m_CacheBinding != null && this.m_CacheBinding.Cache != null && this.m_CacheBinding.Validator != null && this.CacheProtocol == null && policy != null && policy.Level != RequestCacheLevel.BypassCache)
			{
				this.CacheProtocol = new RequestCacheProtocol(this.m_CacheBinding.Cache, this.m_CacheBinding.Validator.CreateValidator());
			}
			this.m_CachePolicy = policy;
		}

		// Token: 0x1700034A RID: 842
		// (get) Token: 0x06000EE4 RID: 3812 RVA: 0x0004E0E8 File Offset: 0x0004C2E8
		// (set) Token: 0x06000EE5 RID: 3813 RVA: 0x0004E0EF File Offset: 0x0004C2EF
		[global::__DynamicallyInvokable]
		public virtual string Method
		{
			[global::__DynamicallyInvokable]
			get
			{
				throw ExceptionHelper.PropertyNotImplementedException;
			}
			[global::__DynamicallyInvokable]
			set
			{
				throw ExceptionHelper.PropertyNotImplementedException;
			}
		}

		// Token: 0x1700034B RID: 843
		// (get) Token: 0x06000EE6 RID: 3814 RVA: 0x0004E0F6 File Offset: 0x0004C2F6
		[global::__DynamicallyInvokable]
		public virtual Uri RequestUri
		{
			[global::__DynamicallyInvokable]
			get
			{
				throw ExceptionHelper.PropertyNotImplementedException;
			}
		}

		// Token: 0x1700034C RID: 844
		// (get) Token: 0x06000EE7 RID: 3815 RVA: 0x0004E0FD File Offset: 0x0004C2FD
		// (set) Token: 0x06000EE8 RID: 3816 RVA: 0x0004E104 File Offset: 0x0004C304
		public virtual string ConnectionGroupName
		{
			get
			{
				throw ExceptionHelper.PropertyNotImplementedException;
			}
			set
			{
				throw ExceptionHelper.PropertyNotImplementedException;
			}
		}

		// Token: 0x1700034D RID: 845
		// (get) Token: 0x06000EE9 RID: 3817 RVA: 0x0004E10B File Offset: 0x0004C30B
		// (set) Token: 0x06000EEA RID: 3818 RVA: 0x0004E112 File Offset: 0x0004C312
		[global::__DynamicallyInvokable]
		public virtual WebHeaderCollection Headers
		{
			[global::__DynamicallyInvokable]
			get
			{
				throw ExceptionHelper.PropertyNotImplementedException;
			}
			[global::__DynamicallyInvokable]
			set
			{
				throw ExceptionHelper.PropertyNotImplementedException;
			}
		}

		// Token: 0x1700034E RID: 846
		// (get) Token: 0x06000EEB RID: 3819 RVA: 0x0004E119 File Offset: 0x0004C319
		// (set) Token: 0x06000EEC RID: 3820 RVA: 0x0004E120 File Offset: 0x0004C320
		public virtual long ContentLength
		{
			get
			{
				throw ExceptionHelper.PropertyNotImplementedException;
			}
			set
			{
				throw ExceptionHelper.PropertyNotImplementedException;
			}
		}

		// Token: 0x1700034F RID: 847
		// (get) Token: 0x06000EED RID: 3821 RVA: 0x0004E127 File Offset: 0x0004C327
		// (set) Token: 0x06000EEE RID: 3822 RVA: 0x0004E12E File Offset: 0x0004C32E
		[global::__DynamicallyInvokable]
		public virtual string ContentType
		{
			[global::__DynamicallyInvokable]
			get
			{
				throw ExceptionHelper.PropertyNotImplementedException;
			}
			[global::__DynamicallyInvokable]
			set
			{
				throw ExceptionHelper.PropertyNotImplementedException;
			}
		}

		// Token: 0x17000350 RID: 848
		// (get) Token: 0x06000EEF RID: 3823 RVA: 0x0004E135 File Offset: 0x0004C335
		// (set) Token: 0x06000EF0 RID: 3824 RVA: 0x0004E13C File Offset: 0x0004C33C
		[global::__DynamicallyInvokable]
		public virtual ICredentials Credentials
		{
			[global::__DynamicallyInvokable]
			get
			{
				throw ExceptionHelper.PropertyNotImplementedException;
			}
			[global::__DynamicallyInvokable]
			set
			{
				throw ExceptionHelper.PropertyNotImplementedException;
			}
		}

		// Token: 0x17000351 RID: 849
		// (get) Token: 0x06000EF1 RID: 3825 RVA: 0x0004E143 File Offset: 0x0004C343
		// (set) Token: 0x06000EF2 RID: 3826 RVA: 0x0004E14A File Offset: 0x0004C34A
		[global::__DynamicallyInvokable]
		public virtual bool UseDefaultCredentials
		{
			[global::__DynamicallyInvokable]
			get
			{
				throw ExceptionHelper.PropertyNotImplementedException;
			}
			[global::__DynamicallyInvokable]
			set
			{
				throw ExceptionHelper.PropertyNotImplementedException;
			}
		}

		// Token: 0x17000352 RID: 850
		// (get) Token: 0x06000EF3 RID: 3827 RVA: 0x0004E151 File Offset: 0x0004C351
		// (set) Token: 0x06000EF4 RID: 3828 RVA: 0x0004E158 File Offset: 0x0004C358
		[global::__DynamicallyInvokable]
		public virtual IWebProxy Proxy
		{
			[global::__DynamicallyInvokable]
			get
			{
				throw ExceptionHelper.PropertyNotImplementedException;
			}
			[global::__DynamicallyInvokable]
			set
			{
				throw ExceptionHelper.PropertyNotImplementedException;
			}
		}

		// Token: 0x17000353 RID: 851
		// (get) Token: 0x06000EF5 RID: 3829 RVA: 0x0004E15F File Offset: 0x0004C35F
		// (set) Token: 0x06000EF6 RID: 3830 RVA: 0x0004E166 File Offset: 0x0004C366
		public virtual bool PreAuthenticate
		{
			get
			{
				throw ExceptionHelper.PropertyNotImplementedException;
			}
			set
			{
				throw ExceptionHelper.PropertyNotImplementedException;
			}
		}

		// Token: 0x17000354 RID: 852
		// (get) Token: 0x06000EF7 RID: 3831 RVA: 0x0004E16D File Offset: 0x0004C36D
		// (set) Token: 0x06000EF8 RID: 3832 RVA: 0x0004E174 File Offset: 0x0004C374
		public virtual int Timeout
		{
			get
			{
				throw ExceptionHelper.PropertyNotImplementedException;
			}
			set
			{
				throw ExceptionHelper.PropertyNotImplementedException;
			}
		}

		// Token: 0x06000EF9 RID: 3833 RVA: 0x0004E17B File Offset: 0x0004C37B
		public virtual Stream GetRequestStream()
		{
			throw ExceptionHelper.MethodNotImplementedException;
		}

		// Token: 0x06000EFA RID: 3834 RVA: 0x0004E182 File Offset: 0x0004C382
		public virtual WebResponse GetResponse()
		{
			throw ExceptionHelper.MethodNotImplementedException;
		}

		// Token: 0x06000EFB RID: 3835 RVA: 0x0004E189 File Offset: 0x0004C389
		[global::__DynamicallyInvokable]
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public virtual IAsyncResult BeginGetResponse(AsyncCallback callback, object state)
		{
			throw ExceptionHelper.MethodNotImplementedException;
		}

		// Token: 0x06000EFC RID: 3836 RVA: 0x0004E190 File Offset: 0x0004C390
		[global::__DynamicallyInvokable]
		public virtual WebResponse EndGetResponse(IAsyncResult asyncResult)
		{
			throw ExceptionHelper.MethodNotImplementedException;
		}

		// Token: 0x06000EFD RID: 3837 RVA: 0x0004E197 File Offset: 0x0004C397
		[global::__DynamicallyInvokable]
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public virtual IAsyncResult BeginGetRequestStream(AsyncCallback callback, object state)
		{
			throw ExceptionHelper.MethodNotImplementedException;
		}

		// Token: 0x06000EFE RID: 3838 RVA: 0x0004E19E File Offset: 0x0004C39E
		[global::__DynamicallyInvokable]
		public virtual Stream EndGetRequestStream(IAsyncResult asyncResult)
		{
			throw ExceptionHelper.MethodNotImplementedException;
		}

		// Token: 0x06000EFF RID: 3839 RVA: 0x0004E1A8 File Offset: 0x0004C3A8
		[global::__DynamicallyInvokable]
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public virtual Task<Stream> GetRequestStreamAsync()
		{
			IWebProxy webProxy = null;
			try
			{
				webProxy = this.Proxy;
			}
			catch (NotImplementedException)
			{
			}
			if (ExecutionContext.IsFlowSuppressed() && (this.UseDefaultCredentials || this.Credentials != null || (webProxy != null && webProxy.Credentials != null)))
			{
				WindowsIdentity currentUser = this.SafeCaptureIdenity();
				return Task.Run<Stream>(delegate
				{
					Task<Stream> task;
					using (currentUser)
					{
						using (currentUser.Impersonate())
						{
							task = Task<Stream>.Factory.FromAsync(new Func<AsyncCallback, object, IAsyncResult>(this.BeginGetRequestStream), new Func<IAsyncResult, Stream>(this.EndGetRequestStream), null);
						}
					}
					return task;
				});
			}
			return Task.Run<Stream>(() => Task<Stream>.Factory.FromAsync(new Func<AsyncCallback, object, IAsyncResult>(this.BeginGetRequestStream), new Func<IAsyncResult, Stream>(this.EndGetRequestStream), null));
		}

		// Token: 0x06000F00 RID: 3840 RVA: 0x0004E234 File Offset: 0x0004C434
		[global::__DynamicallyInvokable]
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public virtual Task<WebResponse> GetResponseAsync()
		{
			IWebProxy webProxy = null;
			try
			{
				webProxy = this.Proxy;
			}
			catch (NotImplementedException)
			{
			}
			if (ExecutionContext.IsFlowSuppressed() && (this.UseDefaultCredentials || this.Credentials != null || (webProxy != null && webProxy.Credentials != null)))
			{
				WindowsIdentity currentUser = this.SafeCaptureIdenity();
				return Task.Run<WebResponse>(delegate
				{
					Task<WebResponse> task;
					using (currentUser)
					{
						using (currentUser.Impersonate())
						{
							task = Task<WebResponse>.Factory.FromAsync(new Func<AsyncCallback, object, IAsyncResult>(this.BeginGetResponse), new Func<IAsyncResult, WebResponse>(this.EndGetResponse), null);
						}
					}
					return task;
				});
			}
			return Task.Run<WebResponse>(() => Task<WebResponse>.Factory.FromAsync(new Func<AsyncCallback, object, IAsyncResult>(this.BeginGetResponse), new Func<IAsyncResult, WebResponse>(this.EndGetResponse), null));
		}

		// Token: 0x06000F01 RID: 3841 RVA: 0x0004E2C0 File Offset: 0x0004C4C0
		[SecuritySafeCritical]
		[SecurityPermission(SecurityAction.Assert, Flags = SecurityPermissionFlag.ControlPrincipal)]
		private WindowsIdentity SafeCaptureIdenity()
		{
			return WindowsIdentity.GetCurrent();
		}

		// Token: 0x06000F02 RID: 3842 RVA: 0x0004E2C7 File Offset: 0x0004C4C7
		[global::__DynamicallyInvokable]
		public virtual void Abort()
		{
			throw ExceptionHelper.MethodNotImplementedException;
		}

		// Token: 0x17000355 RID: 853
		// (get) Token: 0x06000F03 RID: 3843 RVA: 0x0004E2CE File Offset: 0x0004C4CE
		// (set) Token: 0x06000F04 RID: 3844 RVA: 0x0004E2D6 File Offset: 0x0004C4D6
		internal RequestCacheProtocol CacheProtocol
		{
			get
			{
				return this.m_CacheProtocol;
			}
			set
			{
				this.m_CacheProtocol = value;
			}
		}

		// Token: 0x17000356 RID: 854
		// (get) Token: 0x06000F05 RID: 3845 RVA: 0x0004E2DF File Offset: 0x0004C4DF
		// (set) Token: 0x06000F06 RID: 3846 RVA: 0x0004E2E7 File Offset: 0x0004C4E7
		public AuthenticationLevel AuthenticationLevel
		{
			get
			{
				return this.m_AuthenticationLevel;
			}
			set
			{
				this.m_AuthenticationLevel = value;
			}
		}

		// Token: 0x06000F07 RID: 3847 RVA: 0x0004E2F0 File Offset: 0x0004C4F0
		internal virtual ContextAwareResult GetConnectingContext()
		{
			throw ExceptionHelper.MethodNotImplementedException;
		}

		// Token: 0x06000F08 RID: 3848 RVA: 0x0004E2F7 File Offset: 0x0004C4F7
		internal virtual ContextAwareResult GetWritingContext()
		{
			throw ExceptionHelper.MethodNotImplementedException;
		}

		// Token: 0x06000F09 RID: 3849 RVA: 0x0004E2FE File Offset: 0x0004C4FE
		internal virtual ContextAwareResult GetReadingContext()
		{
			throw ExceptionHelper.MethodNotImplementedException;
		}

		// Token: 0x17000357 RID: 855
		// (get) Token: 0x06000F0A RID: 3850 RVA: 0x0004E305 File Offset: 0x0004C505
		// (set) Token: 0x06000F0B RID: 3851 RVA: 0x0004E30D File Offset: 0x0004C50D
		public TokenImpersonationLevel ImpersonationLevel
		{
			get
			{
				return this.m_ImpersonationLevel;
			}
			set
			{
				this.m_ImpersonationLevel = value;
			}
		}

		// Token: 0x06000F0C RID: 3852 RVA: 0x0004E316 File Offset: 0x0004C516
		internal virtual void RequestCallback(object obj)
		{
			throw ExceptionHelper.MethodNotImplementedException;
		}

		// Token: 0x17000358 RID: 856
		// (get) Token: 0x06000F0D RID: 3853 RVA: 0x0004E320 File Offset: 0x0004C520
		// (set) Token: 0x06000F0E RID: 3854 RVA: 0x0004E390 File Offset: 0x0004C590
		internal static IWebProxy InternalDefaultWebProxy
		{
			get
			{
				if (!WebRequest.s_DefaultWebProxyInitialized)
				{
					object internalSyncObject = WebRequest.InternalSyncObject;
					lock (internalSyncObject)
					{
						if (!WebRequest.s_DefaultWebProxyInitialized)
						{
							DefaultProxySectionInternal section = DefaultProxySectionInternal.GetSection();
							if (section != null)
							{
								WebRequest.s_DefaultWebProxy = section.WebProxy;
							}
							WebRequest.s_DefaultWebProxyInitialized = true;
						}
					}
				}
				return WebRequest.s_DefaultWebProxy;
			}
			set
			{
				if (!WebRequest.s_DefaultWebProxyInitialized)
				{
					object internalSyncObject = WebRequest.InternalSyncObject;
					lock (internalSyncObject)
					{
						WebRequest.s_DefaultWebProxy = value;
						WebRequest.s_DefaultWebProxyInitialized = true;
						return;
					}
				}
				WebRequest.s_DefaultWebProxy = value;
			}
		}

		// Token: 0x17000359 RID: 857
		// (get) Token: 0x06000F0F RID: 3855 RVA: 0x0004E3EC File Offset: 0x0004C5EC
		// (set) Token: 0x06000F10 RID: 3856 RVA: 0x0004E3FD File Offset: 0x0004C5FD
		[global::__DynamicallyInvokable]
		public static IWebProxy DefaultWebProxy
		{
			[global::__DynamicallyInvokable]
			get
			{
				ExceptionHelper.WebPermissionUnrestricted.Demand();
				return WebRequest.InternalDefaultWebProxy;
			}
			[global::__DynamicallyInvokable]
			set
			{
				ExceptionHelper.WebPermissionUnrestricted.Demand();
				WebRequest.InternalDefaultWebProxy = value;
			}
		}

		// Token: 0x06000F11 RID: 3857 RVA: 0x0004E40F File Offset: 0x0004C60F
		public static IWebProxy GetSystemWebProxy()
		{
			ExceptionHelper.WebPermissionUnrestricted.Demand();
			return WebRequest.InternalGetSystemWebProxy();
		}

		// Token: 0x06000F12 RID: 3858 RVA: 0x0004E420 File Offset: 0x0004C620
		internal static IWebProxy InternalGetSystemWebProxy()
		{
			return new WebRequest.WebProxyWrapperOpaque(new WebProxy(true));
		}

		// Token: 0x06000F13 RID: 3859 RVA: 0x0004E42D File Offset: 0x0004C62D
		internal void SetupCacheProtocol(Uri uri)
		{
			this.m_CacheBinding = RequestCacheManager.GetBinding(uri.Scheme);
			this.InternalSetCachePolicy(this.m_CacheBinding.Policy);
			if (this.m_CachePolicy == null)
			{
				this.InternalSetCachePolicy(WebRequest.DefaultCachePolicy);
			}
		}

		// Token: 0x06000F14 RID: 3860 RVA: 0x0004E464 File Offset: 0x0004C664
		private static void InitEtwMethods()
		{
			Type typeFromHandle = typeof(FrameworkEventSource);
			Type[] array = new Type[]
			{
				typeof(object),
				typeof(string),
				typeof(bool),
				typeof(bool)
			};
			BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
			MethodInfo method = typeFromHandle.GetMethod("BeginGetResponse", bindingFlags, null, array, null);
			MethodInfo method2 = typeFromHandle.GetMethod("EndGetResponse", bindingFlags, null, new Type[]
			{
				typeof(object),
				typeof(bool),
				typeof(bool),
				typeof(int)
			}, null);
			MethodInfo method3 = typeFromHandle.GetMethod("BeginGetRequestStream", bindingFlags, null, array, null);
			MethodInfo method4 = typeFromHandle.GetMethod("EndGetRequestStream", bindingFlags, null, new Type[]
			{
				typeof(object),
				typeof(bool),
				typeof(bool)
			}, null);
			if (method != null && method2 != null && method3 != null && method4 != null)
			{
				WebRequest.s_EtwFireBeginGetResponse = (WebRequest.DelEtwFireBeginWRGet)method.CreateDelegate(typeof(WebRequest.DelEtwFireBeginWRGet), FrameworkEventSource.Log);
				WebRequest.s_EtwFireEndGetResponse = (WebRequest.DelEtwFireEndWRespGet)method2.CreateDelegate(typeof(WebRequest.DelEtwFireEndWRespGet), FrameworkEventSource.Log);
				WebRequest.s_EtwFireBeginGetRequestStream = (WebRequest.DelEtwFireBeginWRGet)method3.CreateDelegate(typeof(WebRequest.DelEtwFireBeginWRGet), FrameworkEventSource.Log);
				WebRequest.s_EtwFireEndGetRequestStream = (WebRequest.DelEtwFireEndWRGet)method4.CreateDelegate(typeof(WebRequest.DelEtwFireEndWRGet), FrameworkEventSource.Log);
			}
			WebRequest.s_TriedGetEtwDelegates = true;
		}

		// Token: 0x06000F15 RID: 3861 RVA: 0x0004E61C File Offset: 0x0004C81C
		internal void LogBeginGetResponse(bool success, bool synchronous)
		{
			string originalString = this.RequestUri.OriginalString;
			if (!WebRequest.s_TriedGetEtwDelegates)
			{
				WebRequest.InitEtwMethods();
			}
			if (WebRequest.s_EtwFireBeginGetResponse != null)
			{
				WebRequest.s_EtwFireBeginGetResponse(this, originalString, success, synchronous);
			}
		}

		// Token: 0x06000F16 RID: 3862 RVA: 0x0004E658 File Offset: 0x0004C858
		internal void LogEndGetResponse(bool success, bool synchronous, int statusCode)
		{
			if (!WebRequest.s_TriedGetEtwDelegates)
			{
				WebRequest.InitEtwMethods();
			}
			if (WebRequest.s_EtwFireEndGetResponse != null)
			{
				WebRequest.s_EtwFireEndGetResponse(this, success, synchronous, statusCode);
			}
		}

		// Token: 0x06000F17 RID: 3863 RVA: 0x0004E680 File Offset: 0x0004C880
		internal void LogBeginGetRequestStream(bool success, bool synchronous)
		{
			string originalString = this.RequestUri.OriginalString;
			if (!WebRequest.s_TriedGetEtwDelegates)
			{
				WebRequest.InitEtwMethods();
			}
			if (WebRequest.s_EtwFireBeginGetRequestStream != null)
			{
				WebRequest.s_EtwFireBeginGetRequestStream(this, originalString, success, synchronous);
			}
		}

		// Token: 0x06000F18 RID: 3864 RVA: 0x0004E6BC File Offset: 0x0004C8BC
		internal void LogEndGetRequestStream(bool success, bool synchronous)
		{
			if (!WebRequest.s_TriedGetEtwDelegates)
			{
				WebRequest.InitEtwMethods();
			}
			if (WebRequest.s_EtwFireEndGetRequestStream != null)
			{
				WebRequest.s_EtwFireEndGetRequestStream(this, success, synchronous);
			}
		}

		// Token: 0x04001287 RID: 4743
		internal const int DefaultTimeout = 100000;

		// Token: 0x04001288 RID: 4744
		private static volatile ArrayList s_PrefixList;

		// Token: 0x04001289 RID: 4745
		private static object s_InternalSyncObject;

		// Token: 0x0400128A RID: 4746
		private static TimerThread.Queue s_DefaultTimerQueue = TimerThread.CreateQueue(100000);

		// Token: 0x0400128B RID: 4747
		private AuthenticationLevel m_AuthenticationLevel;

		// Token: 0x0400128C RID: 4748
		private TokenImpersonationLevel m_ImpersonationLevel;

		// Token: 0x0400128D RID: 4749
		private RequestCachePolicy m_CachePolicy;

		// Token: 0x0400128E RID: 4750
		private RequestCacheProtocol m_CacheProtocol;

		// Token: 0x0400128F RID: 4751
		private RequestCacheBinding m_CacheBinding;

		// Token: 0x04001290 RID: 4752
		private static WebRequest.DesignerWebRequestCreate webRequestCreate = new WebRequest.DesignerWebRequestCreate();

		// Token: 0x04001291 RID: 4753
		private static volatile IWebProxy s_DefaultWebProxy;

		// Token: 0x04001292 RID: 4754
		private static volatile bool s_DefaultWebProxyInitialized;

		// Token: 0x04001293 RID: 4755
		private static WebRequest.DelEtwFireBeginWRGet s_EtwFireBeginGetResponse;

		// Token: 0x04001294 RID: 4756
		private static WebRequest.DelEtwFireEndWRespGet s_EtwFireEndGetResponse;

		// Token: 0x04001295 RID: 4757
		private static WebRequest.DelEtwFireBeginWRGet s_EtwFireBeginGetRequestStream;

		// Token: 0x04001296 RID: 4758
		private static WebRequest.DelEtwFireEndWRGet s_EtwFireEndGetRequestStream;

		// Token: 0x04001297 RID: 4759
		private static volatile bool s_TriedGetEtwDelegates;

		// Token: 0x02000735 RID: 1845
		internal class DesignerWebRequestCreate : IWebRequestCreate
		{
			// Token: 0x060041A3 RID: 16803 RVA: 0x00110C44 File Offset: 0x0010EE44
			public WebRequest Create(Uri uri)
			{
				return WebRequest.Create(uri);
			}
		}

		// Token: 0x02000736 RID: 1846
		internal class WebProxyWrapperOpaque : IAutoWebProxy, IWebProxy
		{
			// Token: 0x060041A5 RID: 16805 RVA: 0x00110C54 File Offset: 0x0010EE54
			internal WebProxyWrapperOpaque(WebProxy webProxy)
			{
				this.webProxy = webProxy;
			}

			// Token: 0x060041A6 RID: 16806 RVA: 0x00110C63 File Offset: 0x0010EE63
			public Uri GetProxy(Uri destination)
			{
				return this.webProxy.GetProxy(destination);
			}

			// Token: 0x060041A7 RID: 16807 RVA: 0x00110C71 File Offset: 0x0010EE71
			public bool IsBypassed(Uri host)
			{
				return this.webProxy.IsBypassed(host);
			}

			// Token: 0x17000F02 RID: 3842
			// (get) Token: 0x060041A8 RID: 16808 RVA: 0x00110C7F File Offset: 0x0010EE7F
			// (set) Token: 0x060041A9 RID: 16809 RVA: 0x00110C8C File Offset: 0x0010EE8C
			public ICredentials Credentials
			{
				get
				{
					return this.webProxy.Credentials;
				}
				set
				{
					this.webProxy.Credentials = value;
				}
			}

			// Token: 0x060041AA RID: 16810 RVA: 0x00110C9A File Offset: 0x0010EE9A
			public ProxyChain GetProxies(Uri destination)
			{
				return ((IAutoWebProxy)this.webProxy).GetProxies(destination);
			}

			// Token: 0x040031A9 RID: 12713
			protected readonly WebProxy webProxy;
		}

		// Token: 0x02000737 RID: 1847
		internal class WebProxyWrapper : WebRequest.WebProxyWrapperOpaque
		{
			// Token: 0x060041AB RID: 16811 RVA: 0x00110CA8 File Offset: 0x0010EEA8
			internal WebProxyWrapper(WebProxy webProxy)
				: base(webProxy)
			{
			}

			// Token: 0x17000F03 RID: 3843
			// (get) Token: 0x060041AC RID: 16812 RVA: 0x00110CB1 File Offset: 0x0010EEB1
			internal WebProxy WebProxy
			{
				get
				{
					return this.webProxy;
				}
			}
		}

		// Token: 0x02000738 RID: 1848
		// (Invoke) Token: 0x060041AE RID: 16814
		private delegate void DelEtwFireBeginWRGet(object id, string uri, bool success, bool synchronous);

		// Token: 0x02000739 RID: 1849
		// (Invoke) Token: 0x060041B2 RID: 16818
		private delegate void DelEtwFireEndWRGet(object id, bool success, bool synchronous);

		// Token: 0x0200073A RID: 1850
		// (Invoke) Token: 0x060041B6 RID: 16822
		private delegate void DelEtwFireEndWRespGet(object id, bool success, bool synchronous, int statusCode);
	}
}
