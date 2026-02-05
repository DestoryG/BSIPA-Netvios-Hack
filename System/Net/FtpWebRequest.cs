using System;
using System.Diagnostics.Tracing;
using System.IO;
using System.Net.Cache;
using System.Net.Sockets;
using System.Security;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Security.Permissions;
using System.Threading;

namespace System.Net
{
	// Token: 0x020000EF RID: 239
	public sealed class FtpWebRequest : WebRequest
	{
		// Token: 0x17000180 RID: 384
		// (get) Token: 0x06000815 RID: 2069 RVA: 0x0002C670 File Offset: 0x0002A870
		internal FtpMethodInfo MethodInfo
		{
			get
			{
				return this.m_MethodInfo;
			}
		}

		// Token: 0x17000181 RID: 385
		// (get) Token: 0x06000816 RID: 2070 RVA: 0x0002C678 File Offset: 0x0002A878
		internal static NetworkCredential DefaultNetworkCredential
		{
			get
			{
				return FtpWebRequest.DefaultFtpNetworkCredential;
			}
		}

		// Token: 0x17000182 RID: 386
		// (get) Token: 0x06000817 RID: 2071 RVA: 0x0002C680 File Offset: 0x0002A880
		// (set) Token: 0x06000818 RID: 2072 RVA: 0x0002C6A8 File Offset: 0x0002A8A8
		public new static RequestCachePolicy DefaultCachePolicy
		{
			get
			{
				RequestCachePolicy policy = RequestCacheManager.GetBinding(Uri.UriSchemeFtp).Policy;
				if (policy == null)
				{
					return WebRequest.DefaultCachePolicy;
				}
				return policy;
			}
			set
			{
				ExceptionHelper.WebPermissionUnrestricted.Demand();
				RequestCacheBinding binding = RequestCacheManager.GetBinding(Uri.UriSchemeFtp);
				RequestCacheManager.SetBinding(Uri.UriSchemeFtp, new RequestCacheBinding(binding.Cache, binding.Validator, value));
			}
		}

		// Token: 0x17000183 RID: 387
		// (get) Token: 0x06000819 RID: 2073 RVA: 0x0002C6E6 File Offset: 0x0002A8E6
		// (set) Token: 0x0600081A RID: 2074 RVA: 0x0002C6F4 File Offset: 0x0002A8F4
		public override string Method
		{
			get
			{
				return this.m_MethodInfo.Method;
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					throw new ArgumentException(global::System.SR.GetString("net_ftp_invalid_method_name"), "value");
				}
				if (this.InUse)
				{
					throw new InvalidOperationException(global::System.SR.GetString("net_reqsubmitted"));
				}
				try
				{
					this.m_MethodInfo = FtpMethodInfo.GetMethodInfo(value);
				}
				catch (ArgumentException)
				{
					throw new ArgumentException(global::System.SR.GetString("net_ftp_unsupported_method"), "value");
				}
			}
		}

		// Token: 0x17000184 RID: 388
		// (get) Token: 0x0600081B RID: 2075 RVA: 0x0002C76C File Offset: 0x0002A96C
		// (set) Token: 0x0600081C RID: 2076 RVA: 0x0002C774 File Offset: 0x0002A974
		public string RenameTo
		{
			get
			{
				return this.m_RenameTo;
			}
			set
			{
				if (this.InUse)
				{
					throw new InvalidOperationException(global::System.SR.GetString("net_reqsubmitted"));
				}
				if (string.IsNullOrEmpty(value))
				{
					throw new ArgumentException(global::System.SR.GetString("net_ftp_invalid_renameto"), "value");
				}
				this.m_RenameTo = value;
			}
		}

		// Token: 0x17000185 RID: 389
		// (get) Token: 0x0600081D RID: 2077 RVA: 0x0002C7B2 File Offset: 0x0002A9B2
		// (set) Token: 0x0600081E RID: 2078 RVA: 0x0002C7BC File Offset: 0x0002A9BC
		public override ICredentials Credentials
		{
			get
			{
				return this.m_AuthInfo;
			}
			set
			{
				if (this.InUse)
				{
					throw new InvalidOperationException(global::System.SR.GetString("net_reqsubmitted"));
				}
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				if (value is SystemNetworkCredential)
				{
					throw new ArgumentException(global::System.SR.GetString("net_ftp_no_defaultcreds"), "value");
				}
				this.m_AuthInfo = value;
			}
		}

		// Token: 0x17000186 RID: 390
		// (get) Token: 0x0600081F RID: 2079 RVA: 0x0002C813 File Offset: 0x0002AA13
		public override Uri RequestUri
		{
			get
			{
				return this.m_Uri;
			}
		}

		// Token: 0x17000187 RID: 391
		// (get) Token: 0x06000820 RID: 2080 RVA: 0x0002C81B File Offset: 0x0002AA1B
		// (set) Token: 0x06000821 RID: 2081 RVA: 0x0002C824 File Offset: 0x0002AA24
		public override int Timeout
		{
			get
			{
				return this.m_Timeout;
			}
			set
			{
				if (this.InUse)
				{
					throw new InvalidOperationException(global::System.SR.GetString("net_reqsubmitted"));
				}
				if (value < 0 && value != -1)
				{
					throw new ArgumentOutOfRangeException("value", global::System.SR.GetString("net_io_timeout_use_ge_zero"));
				}
				if (this.m_Timeout != value)
				{
					this.m_Timeout = value;
					this.m_TimerQueue = null;
				}
			}
		}

		// Token: 0x17000188 RID: 392
		// (get) Token: 0x06000822 RID: 2082 RVA: 0x0002C87D File Offset: 0x0002AA7D
		internal int RemainingTimeout
		{
			get
			{
				return this.m_RemainingTimeout;
			}
		}

		// Token: 0x17000189 RID: 393
		// (get) Token: 0x06000823 RID: 2083 RVA: 0x0002C885 File Offset: 0x0002AA85
		// (set) Token: 0x06000824 RID: 2084 RVA: 0x0002C88D File Offset: 0x0002AA8D
		public int ReadWriteTimeout
		{
			get
			{
				return this.m_ReadWriteTimeout;
			}
			set
			{
				if (this.m_GetResponseStarted)
				{
					throw new InvalidOperationException(global::System.SR.GetString("net_reqsubmitted"));
				}
				if (value <= 0 && value != -1)
				{
					throw new ArgumentOutOfRangeException("value", global::System.SR.GetString("net_io_timeout_use_gt_zero"));
				}
				this.m_ReadWriteTimeout = value;
			}
		}

		// Token: 0x1700018A RID: 394
		// (get) Token: 0x06000825 RID: 2085 RVA: 0x0002C8CB File Offset: 0x0002AACB
		// (set) Token: 0x06000826 RID: 2086 RVA: 0x0002C8D3 File Offset: 0x0002AAD3
		public long ContentOffset
		{
			get
			{
				return this.m_ContentOffset;
			}
			set
			{
				if (this.InUse)
				{
					throw new InvalidOperationException(global::System.SR.GetString("net_reqsubmitted"));
				}
				if (value < 0L)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this.m_ContentOffset = value;
			}
		}

		// Token: 0x1700018B RID: 395
		// (get) Token: 0x06000827 RID: 2087 RVA: 0x0002C904 File Offset: 0x0002AB04
		// (set) Token: 0x06000828 RID: 2088 RVA: 0x0002C90C File Offset: 0x0002AB0C
		public override long ContentLength
		{
			get
			{
				return this.m_ContentLength;
			}
			set
			{
				this.m_ContentLength = value;
			}
		}

		// Token: 0x1700018C RID: 396
		// (get) Token: 0x06000829 RID: 2089 RVA: 0x0002C915 File Offset: 0x0002AB15
		// (set) Token: 0x0600082A RID: 2090 RVA: 0x0002C928 File Offset: 0x0002AB28
		public override IWebProxy Proxy
		{
			get
			{
				ExceptionHelper.WebPermissionUnrestricted.Demand();
				return this.m_Proxy;
			}
			set
			{
				ExceptionHelper.WebPermissionUnrestricted.Demand();
				if (this.InUse)
				{
					throw new InvalidOperationException(global::System.SR.GetString("net_reqsubmitted"));
				}
				this.m_ProxyUserSet = true;
				this.m_Proxy = value;
				this.m_ServicePoint = null;
				ServicePoint servicePoint = this.ServicePoint;
			}
		}

		// Token: 0x1700018D RID: 397
		// (get) Token: 0x0600082B RID: 2091 RVA: 0x0002C973 File Offset: 0x0002AB73
		// (set) Token: 0x0600082C RID: 2092 RVA: 0x0002C97B File Offset: 0x0002AB7B
		public override string ConnectionGroupName
		{
			get
			{
				return this.m_ConnectionGroupName;
			}
			set
			{
				if (this.InUse)
				{
					throw new InvalidOperationException(global::System.SR.GetString("net_reqsubmitted"));
				}
				this.m_ConnectionGroupName = value;
			}
		}

		// Token: 0x1700018E RID: 398
		// (get) Token: 0x0600082D RID: 2093 RVA: 0x0002C99C File Offset: 0x0002AB9C
		public ServicePoint ServicePoint
		{
			get
			{
				if (this.m_ServicePoint == null)
				{
					IWebProxy webProxy = this.m_Proxy;
					if (!this.m_ProxyUserSet)
					{
						webProxy = WebRequest.InternalDefaultWebProxy;
					}
					ServicePoint servicePoint = ServicePointManager.FindServicePoint(this.m_Uri, webProxy);
					object syncObject = this.m_SyncObject;
					lock (syncObject)
					{
						if (this.m_ServicePoint == null)
						{
							this.m_ServicePoint = servicePoint;
							this.m_Proxy = webProxy;
						}
					}
				}
				return this.m_ServicePoint;
			}
		}

		// Token: 0x1700018F RID: 399
		// (get) Token: 0x0600082E RID: 2094 RVA: 0x0002CA1C File Offset: 0x0002AC1C
		internal bool Aborted
		{
			get
			{
				return this.m_Aborted;
			}
		}

		// Token: 0x0600082F RID: 2095 RVA: 0x0002CA24 File Offset: 0x0002AC24
		internal FtpWebRequest(Uri uri)
		{
			new WebPermission(NetworkAccess.Connect, uri).Demand();
			if (Logging.On)
			{
				Logging.PrintInfo(Logging.Web, this, ".ctor", uri.ToString());
			}
			if (uri.Scheme != Uri.UriSchemeFtp)
			{
				throw new ArgumentOutOfRangeException("uri");
			}
			this.m_TimerCallback = new TimerThread.Callback(this.TimerCallback);
			this.m_SyncObject = new object();
			NetworkCredential networkCredential = null;
			this.m_Uri = uri;
			this.m_MethodInfo = FtpMethodInfo.GetMethodInfo("RETR");
			if (this.m_Uri.UserInfo != null && this.m_Uri.UserInfo.Length != 0)
			{
				string userInfo = this.m_Uri.UserInfo;
				string text = userInfo;
				string text2 = "";
				int num = userInfo.IndexOf(':');
				if (num != -1)
				{
					text = Uri.UnescapeDataString(userInfo.Substring(0, num));
					num++;
					text2 = Uri.UnescapeDataString(userInfo.Substring(num, userInfo.Length - num));
				}
				networkCredential = new NetworkCredential(text, text2);
			}
			if (networkCredential == null)
			{
				networkCredential = FtpWebRequest.DefaultFtpNetworkCredential;
			}
			this.m_AuthInfo = networkCredential;
			base.SetupCacheProtocol(this.m_Uri);
		}

		// Token: 0x06000830 RID: 2096 RVA: 0x0002CB78 File Offset: 0x0002AD78
		public override WebResponse GetResponse()
		{
			if (Logging.On)
			{
				Logging.Enter(Logging.Web, this, "GetResponse", "");
			}
			if (Logging.On)
			{
				Logging.PrintInfo(Logging.Web, this, "GetResponse", global::System.SR.GetString("net_log_method_equal", new object[] { this.m_MethodInfo.Method }));
			}
			if (FrameworkEventSource.Log.IsEnabled())
			{
				base.LogBeginGetResponse(true, true);
			}
			bool flag = false;
			int num = -1;
			try
			{
				this.CheckError();
				if (this.m_FtpWebResponse != null)
				{
					flag = true;
					num = FtpWebRequest.GetStatusCode(this.m_FtpWebResponse);
					return this.m_FtpWebResponse;
				}
				if (this.m_GetResponseStarted)
				{
					throw new InvalidOperationException(global::System.SR.GetString("net_repcall"));
				}
				this.m_GetResponseStarted = true;
				this.m_StartTime = DateTime.UtcNow;
				this.m_RemainingTimeout = this.Timeout;
				ServicePoint servicePoint = this.ServicePoint;
				if (this.Timeout != -1)
				{
					this.m_RemainingTimeout = this.Timeout - (int)(DateTime.UtcNow - this.m_StartTime).TotalMilliseconds;
					if (this.m_RemainingTimeout <= 0)
					{
						throw new WebException(NetRes.GetWebStatusString(WebExceptionStatus.Timeout), WebExceptionStatus.Timeout);
					}
				}
				if (this.ServicePoint.InternalProxyServicePoint)
				{
					if (this.EnableSsl)
					{
						this.m_GetResponseStarted = false;
						throw new WebException(global::System.SR.GetString("net_ftp_proxy_does_not_support_ssl"));
					}
					try
					{
						HttpWebRequest httpWebRequest = this.GetHttpWebRequest();
						if (Logging.On)
						{
							Logging.Associate(Logging.Web, this, httpWebRequest);
						}
						this.m_FtpWebResponse = new FtpWebResponse((HttpWebResponse)httpWebRequest.GetResponse());
						goto IL_02AD;
					}
					catch (WebException ex)
					{
						if (ex.Response != null && ex.Response is HttpWebResponse)
						{
							ex = new WebException(ex.Message, null, ex.Status, new FtpWebResponse((HttpWebResponse)ex.Response), ex.InternalStatus);
						}
						this.SetException(ex);
						num = FtpWebRequest.GetStatusCode(ex);
						throw ex;
					}
					catch (InvalidOperationException ex2)
					{
						this.SetException(ex2);
						this.FinishRequestStage(FtpWebRequest.RequestStage.CheckForError);
						throw;
					}
				}
				FtpWebRequest.RequestStage requestStage = this.FinishRequestStage(FtpWebRequest.RequestStage.RequestStarted);
				if (requestStage >= FtpWebRequest.RequestStage.RequestStarted)
				{
					if (requestStage < FtpWebRequest.RequestStage.ReadReady)
					{
						object syncObject = this.m_SyncObject;
						lock (syncObject)
						{
							if (this.m_RequestStage < FtpWebRequest.RequestStage.ReadReady)
							{
								this.m_ReadAsyncResult = new LazyAsyncResult(null, null, null);
							}
						}
						if (this.m_ReadAsyncResult != null)
						{
							this.m_ReadAsyncResult.InternalWaitForCompletion();
						}
						this.CheckError();
					}
				}
				else
				{
					do
					{
						this.SubmitRequest(false);
						if (this.m_MethodInfo.IsUpload)
						{
							this.FinishRequestStage(FtpWebRequest.RequestStage.WriteReady);
						}
						else
						{
							this.FinishRequestStage(FtpWebRequest.RequestStage.ReadReady);
						}
						this.CheckError();
					}
					while (!this.CheckCacheRetrieveOnResponse());
					this.EnsureFtpWebResponse(null);
					this.CheckCacheUpdateOnResponse();
					if (this.m_FtpWebResponse.IsFromCache)
					{
						this.FinishRequestStage(FtpWebRequest.RequestStage.ReleaseConnection);
					}
				}
				IL_02AD:
				num = FtpWebRequest.GetStatusCode(this.m_FtpWebResponse);
				flag = true;
			}
			catch (Exception ex3)
			{
				if (FrameworkEventSource.Log.IsEnabled())
				{
					WebException ex4 = ex3 as WebException;
					if (ex4 != null)
					{
						num = FtpWebRequest.GetStatusCode(ex4);
					}
				}
				if (Logging.On)
				{
					Logging.Exception(Logging.Web, this, "GetResponse", ex3);
				}
				if (this.m_Exception == null)
				{
					if (Logging.On)
					{
						Logging.PrintWarning(Logging.Web, global::System.SR.GetString("net_log_unexpected_exception", new object[] { "GetResponse()" }));
					}
					NclUtilities.IsFatal(ex3);
					this.SetException(ex3);
					this.FinishRequestStage(FtpWebRequest.RequestStage.CheckForError);
				}
				throw;
			}
			finally
			{
				if (Logging.On)
				{
					Logging.Exit(Logging.Web, this, "GetResponse", "");
				}
				if (FrameworkEventSource.Log.IsEnabled())
				{
					base.LogEndGetResponse(flag, true, num);
				}
			}
			return this.m_FtpWebResponse;
		}

		// Token: 0x06000831 RID: 2097 RVA: 0x0002CF84 File Offset: 0x0002B184
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public override IAsyncResult BeginGetResponse(AsyncCallback callback, object state)
		{
			if (Logging.On)
			{
				Logging.Enter(Logging.Web, this, "BeginGetResponse", "");
			}
			if (Logging.On)
			{
				Logging.PrintInfo(Logging.Web, this, "BeginGetResponse", global::System.SR.GetString("net_log_method_equal", new object[] { this.m_MethodInfo.Method }));
			}
			bool flag = true;
			ContextAwareResult contextAwareResult;
			try
			{
				if (this.m_FtpWebResponse != null)
				{
					contextAwareResult = new ContextAwareResult(this, state, callback);
					contextAwareResult.InvokeCallback(this.m_FtpWebResponse);
					return contextAwareResult;
				}
				if (this.m_GetResponseStarted)
				{
					throw new InvalidOperationException(global::System.SR.GetString("net_repcall"));
				}
				this.m_GetResponseStarted = true;
				this.CheckError();
				if (this.ServicePoint.InternalProxyServicePoint)
				{
					HttpWebRequest httpWebRequest = this.GetHttpWebRequest();
					if (Logging.On)
					{
						Logging.Associate(Logging.Web, this, httpWebRequest);
					}
					contextAwareResult = (ContextAwareResult)httpWebRequest.BeginGetResponse(callback, state);
				}
				else
				{
					FtpWebRequest.RequestStage requestStage = this.FinishRequestStage(FtpWebRequest.RequestStage.RequestStarted);
					contextAwareResult = new ContextAwareResult(true, true, this, state, callback);
					this.m_ReadAsyncResult = contextAwareResult;
					if (requestStage >= FtpWebRequest.RequestStage.RequestStarted)
					{
						contextAwareResult.StartPostingAsyncOp();
						contextAwareResult.FinishPostingAsyncOp();
						if (requestStage >= FtpWebRequest.RequestStage.ReadReady)
						{
							contextAwareResult = null;
						}
						else
						{
							object syncObject = this.m_SyncObject;
							lock (syncObject)
							{
								if (this.m_RequestStage >= FtpWebRequest.RequestStage.ReadReady)
								{
									contextAwareResult = null;
								}
							}
						}
						if (contextAwareResult == null)
						{
							contextAwareResult = (ContextAwareResult)this.m_ReadAsyncResult;
							if (!contextAwareResult.InternalPeekCompleted)
							{
								contextAwareResult.InvokeCallback();
							}
						}
					}
					else
					{
						object obj = contextAwareResult.StartPostingAsyncOp();
						lock (obj)
						{
							this.SubmitRequest(true);
							contextAwareResult.FinishPostingAsyncOp();
						}
						this.FinishRequestStage(FtpWebRequest.RequestStage.CheckForError);
					}
				}
			}
			catch (Exception ex)
			{
				flag = false;
				if (Logging.On)
				{
					Logging.Exception(Logging.Web, this, "BeginGetResponse", ex);
				}
				throw;
			}
			finally
			{
				if (FrameworkEventSource.Log.IsEnabled())
				{
					base.LogBeginGetResponse(flag, false);
				}
				if (Logging.On)
				{
					Logging.Exit(Logging.Web, this, "BeginGetResponse", "");
				}
			}
			return contextAwareResult;
		}

		// Token: 0x06000832 RID: 2098 RVA: 0x0002D1DC File Offset: 0x0002B3DC
		public override WebResponse EndGetResponse(IAsyncResult asyncResult)
		{
			if (Logging.On)
			{
				Logging.Enter(Logging.Web, this, "EndGetResponse", "");
			}
			bool flag = false;
			int num = -1;
			try
			{
				if (asyncResult == null)
				{
					throw new ArgumentNullException("asyncResult");
				}
				LazyAsyncResult lazyAsyncResult = asyncResult as LazyAsyncResult;
				if (lazyAsyncResult == null)
				{
					throw new ArgumentException(global::System.SR.GetString("net_io_invalidasyncresult"), "asyncResult");
				}
				if (this.HttpProxyMode ? (lazyAsyncResult.AsyncObject != this.GetHttpWebRequest()) : (lazyAsyncResult.AsyncObject != this))
				{
					throw new ArgumentException(global::System.SR.GetString("net_io_invalidasyncresult"), "asyncResult");
				}
				if (lazyAsyncResult.EndCalled)
				{
					throw new InvalidOperationException(global::System.SR.GetString("net_io_invalidendcall", new object[] { "EndGetResponse" }));
				}
				if (this.HttpProxyMode)
				{
					try
					{
						this.CheckError();
						if (this.m_FtpWebResponse == null)
						{
							this.m_FtpWebResponse = new FtpWebResponse((HttpWebResponse)this.GetHttpWebRequest().EndGetResponse(asyncResult));
							num = FtpWebRequest.GetStatusCode(this.m_FtpWebResponse);
						}
						goto IL_0150;
					}
					catch (WebException ex)
					{
						num = FtpWebRequest.GetStatusCode(ex);
						if (ex.Response != null && ex.Response is HttpWebResponse)
						{
							throw new WebException(ex.Message, null, ex.Status, new FtpWebResponse((HttpWebResponse)ex.Response), ex.InternalStatus);
						}
						throw;
					}
				}
				lazyAsyncResult.InternalWaitForCompletion();
				lazyAsyncResult.EndCalled = true;
				this.CheckError();
				IL_0150:
				flag = true;
			}
			catch (Exception ex2)
			{
				if (FrameworkEventSource.Log.IsEnabled())
				{
					WebException ex3 = ex2 as WebException;
					if (ex3 != null)
					{
						num = FtpWebRequest.GetStatusCode(ex3);
					}
				}
				if (Logging.On)
				{
					Logging.Exception(Logging.Web, this, "EndGetResponse", ex2);
				}
				throw;
			}
			finally
			{
				if (Logging.On)
				{
					Logging.Exit(Logging.Web, this, "EndGetResponse", "");
				}
				if (FrameworkEventSource.Log.IsEnabled())
				{
					base.LogEndGetResponse(flag, false, num);
				}
			}
			return this.m_FtpWebResponse;
		}

		// Token: 0x06000833 RID: 2099 RVA: 0x0002D400 File Offset: 0x0002B600
		public override Stream GetRequestStream()
		{
			if (Logging.On)
			{
				Logging.Enter(Logging.Web, this, "GetRequestStream", "");
			}
			if (Logging.On)
			{
				Logging.PrintInfo(Logging.Web, this, "GetRequestStream", global::System.SR.GetString("net_log_method_equal", new object[] { this.m_MethodInfo.Method }));
			}
			if (FrameworkEventSource.Log.IsEnabled())
			{
				base.LogBeginGetRequestStream(true, true);
			}
			bool flag = false;
			try
			{
				if (this.m_GetRequestStreamStarted)
				{
					throw new InvalidOperationException(global::System.SR.GetString("net_repcall"));
				}
				this.m_GetRequestStreamStarted = true;
				if (!this.m_MethodInfo.IsUpload)
				{
					throw new ProtocolViolationException(global::System.SR.GetString("net_nouploadonget"));
				}
				this.CheckError();
				this.m_StartTime = DateTime.UtcNow;
				this.m_RemainingTimeout = this.Timeout;
				ServicePoint servicePoint = this.ServicePoint;
				if (this.Timeout != -1)
				{
					this.m_RemainingTimeout = this.Timeout - (int)(DateTime.UtcNow - this.m_StartTime).TotalMilliseconds;
					if (this.m_RemainingTimeout <= 0)
					{
						throw new WebException(NetRes.GetWebStatusString(WebExceptionStatus.Timeout), WebExceptionStatus.Timeout);
					}
				}
				if (this.ServicePoint.InternalProxyServicePoint)
				{
					HttpWebRequest httpWebRequest = this.GetHttpWebRequest();
					if (Logging.On)
					{
						Logging.Associate(Logging.Web, this, httpWebRequest);
					}
					this.m_Stream = httpWebRequest.GetRequestStream();
				}
				else
				{
					this.FinishRequestStage(FtpWebRequest.RequestStage.RequestStarted);
					this.SubmitRequest(false);
					this.FinishRequestStage(FtpWebRequest.RequestStage.WriteReady);
					this.CheckError();
				}
				if (this.m_Stream.CanTimeout)
				{
					this.m_Stream.WriteTimeout = this.ReadWriteTimeout;
					this.m_Stream.ReadTimeout = this.ReadWriteTimeout;
				}
				flag = true;
			}
			catch (Exception ex)
			{
				if (Logging.On)
				{
					Logging.Exception(Logging.Web, this, "GetRequestStream", ex);
				}
				throw;
			}
			finally
			{
				if (Logging.On)
				{
					Logging.Exit(Logging.Web, this, "GetRequestStream", "");
				}
				if (FrameworkEventSource.Log.IsEnabled())
				{
					base.LogEndGetRequestStream(flag, true);
				}
			}
			return this.m_Stream;
		}

		// Token: 0x06000834 RID: 2100 RVA: 0x0002D628 File Offset: 0x0002B828
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public override IAsyncResult BeginGetRequestStream(AsyncCallback callback, object state)
		{
			if (Logging.On)
			{
				Logging.Enter(Logging.Web, this, "BeginGetRequestStream", "");
			}
			if (Logging.On)
			{
				Logging.PrintInfo(Logging.Web, this, "BeginGetRequestStream", global::System.SR.GetString("net_log_method_equal", new object[] { this.m_MethodInfo.Method }));
			}
			ContextAwareResult contextAwareResult = null;
			bool flag = false;
			try
			{
				if (this.m_GetRequestStreamStarted)
				{
					throw new InvalidOperationException(global::System.SR.GetString("net_repcall"));
				}
				this.m_GetRequestStreamStarted = true;
				if (!this.m_MethodInfo.IsUpload)
				{
					throw new ProtocolViolationException(global::System.SR.GetString("net_nouploadonget"));
				}
				this.CheckError();
				if (this.ServicePoint.InternalProxyServicePoint)
				{
					HttpWebRequest httpWebRequest = this.GetHttpWebRequest();
					if (Logging.On)
					{
						Logging.Associate(Logging.Web, this, httpWebRequest);
					}
					contextAwareResult = (ContextAwareResult)httpWebRequest.BeginGetRequestStream(callback, state);
				}
				else
				{
					this.FinishRequestStage(FtpWebRequest.RequestStage.RequestStarted);
					contextAwareResult = new ContextAwareResult(true, true, this, state, callback);
					object obj = contextAwareResult.StartPostingAsyncOp();
					lock (obj)
					{
						this.m_WriteAsyncResult = contextAwareResult;
						this.SubmitRequest(true);
						contextAwareResult.FinishPostingAsyncOp();
						this.FinishRequestStage(FtpWebRequest.RequestStage.CheckForError);
					}
				}
				flag = true;
			}
			catch (Exception ex)
			{
				if (Logging.On)
				{
					Logging.Exception(Logging.Web, this, "BeginGetRequestStream", ex);
				}
				throw;
			}
			finally
			{
				if (FrameworkEventSource.Log.IsEnabled())
				{
					base.LogBeginGetRequestStream(flag, false);
				}
				if (Logging.On)
				{
					Logging.Exit(Logging.Web, this, "BeginGetRequestStream", "");
				}
			}
			return contextAwareResult;
		}

		// Token: 0x06000835 RID: 2101 RVA: 0x0002D7D0 File Offset: 0x0002B9D0
		public override Stream EndGetRequestStream(IAsyncResult asyncResult)
		{
			if (Logging.On)
			{
				Logging.Enter(Logging.Web, this, "EndGetRequestStream", "");
			}
			Stream stream = null;
			bool flag = false;
			try
			{
				if (asyncResult == null)
				{
					throw new ArgumentNullException("asyncResult");
				}
				LazyAsyncResult lazyAsyncResult = asyncResult as LazyAsyncResult;
				if (lazyAsyncResult == null || (this.HttpProxyMode ? (lazyAsyncResult.AsyncObject != this.GetHttpWebRequest()) : (lazyAsyncResult.AsyncObject != this)))
				{
					throw new ArgumentException(global::System.SR.GetString("net_io_invalidasyncresult"), "asyncResult");
				}
				if (lazyAsyncResult.EndCalled)
				{
					throw new InvalidOperationException(global::System.SR.GetString("net_io_invalidendcall", new object[] { "EndGetResponse" }));
				}
				if (this.HttpProxyMode)
				{
					stream = this.GetHttpWebRequest().EndGetRequestStream(asyncResult);
				}
				else
				{
					lazyAsyncResult.InternalWaitForCompletion();
					lazyAsyncResult.EndCalled = true;
					this.CheckError();
					stream = this.m_Stream;
					lazyAsyncResult.EndCalled = true;
				}
				if (stream.CanTimeout)
				{
					stream.WriteTimeout = this.ReadWriteTimeout;
					stream.ReadTimeout = this.ReadWriteTimeout;
				}
				flag = true;
			}
			catch (Exception ex)
			{
				if (Logging.On)
				{
					Logging.Exception(Logging.Web, this, "EndGetRequestStream", ex);
				}
				throw;
			}
			finally
			{
				if (Logging.On)
				{
					Logging.Exit(Logging.Web, this, "EndGetRequestStream", "");
				}
			}
			if (FrameworkEventSource.Log.IsEnabled())
			{
				base.LogEndGetRequestStream(flag, false);
			}
			return stream;
		}

		// Token: 0x06000836 RID: 2102 RVA: 0x0002D940 File Offset: 0x0002BB40
		private void SubmitRequest(bool async)
		{
			try
			{
				this.m_Async = async;
				if (!this.CheckCacheRetrieveBeforeSubmit())
				{
					if (this.m_ConnectionPool == null)
					{
						this.m_ConnectionPool = ConnectionPoolManager.GetConnectionPool(this.ServicePoint, this.GetConnectionGroupLine(), FtpWebRequest.m_CreateConnectionCallback);
					}
					for (;;)
					{
						FtpControlStream ftpControlStream = this.m_Connection;
						if (ftpControlStream == null)
						{
							ftpControlStream = this.QueueOrCreateConnection();
							if (ftpControlStream == null)
							{
								break;
							}
						}
						if (!async && this.Timeout != -1)
						{
							this.m_RemainingTimeout = this.Timeout - (int)(DateTime.UtcNow - this.m_StartTime).TotalMilliseconds;
							if (this.m_RemainingTimeout <= 0)
							{
								goto Block_8;
							}
						}
						ftpControlStream.SetSocketTimeoutOption(SocketShutdown.Both, this.RemainingTimeout, false);
						try
						{
							Stream stream = this.TimedSubmitRequestHelper(async);
						}
						catch (Exception ex)
						{
							if (this.AttemptedRecovery(ex))
							{
								if (!async && this.Timeout != -1)
								{
									this.m_RemainingTimeout = this.Timeout - (int)(DateTime.UtcNow - this.m_StartTime).TotalMilliseconds;
									if (this.m_RemainingTimeout <= 0)
									{
										throw;
									}
								}
								continue;
							}
							throw;
						}
						break;
					}
					return;
					Block_8:
					throw new WebException(NetRes.GetWebStatusString(WebExceptionStatus.Timeout), WebExceptionStatus.Timeout);
				}
				this.RequestCallback(null);
			}
			catch (WebException ex2)
			{
				IOException ex3 = ex2.InnerException as IOException;
				if (ex3 != null)
				{
					SocketException ex4 = ex3.InnerException as SocketException;
					if (ex4 != null && ex4.ErrorCode == 10060)
					{
						this.SetException(new WebException(global::System.SR.GetString("net_timeout"), WebExceptionStatus.Timeout));
					}
				}
				this.SetException(ex2);
			}
			catch (Exception ex5)
			{
				this.SetException(ex5);
			}
		}

		// Token: 0x06000837 RID: 2103 RVA: 0x0002DB04 File Offset: 0x0002BD04
		private FtpControlStream QueueOrCreateConnection()
		{
			FtpControlStream ftpControlStream = (FtpControlStream)this.m_ConnectionPool.GetConnection(this, this.m_Async ? FtpWebRequest.m_AsyncCallback : null, this.m_Async ? (-1) : this.RemainingTimeout);
			if (ftpControlStream == null)
			{
				return null;
			}
			object syncObject = this.m_SyncObject;
			lock (syncObject)
			{
				if (this.m_Aborted)
				{
					if (Logging.On)
					{
						Logging.PrintInfo(Logging.Web, this, "", global::System.SR.GetString("net_log_releasing_connection", new object[] { ValidationHelper.HashString(ftpControlStream) }));
					}
					this.m_ConnectionPool.PutConnection(ftpControlStream, this, this.RemainingTimeout);
					this.CheckError();
					throw new InternalException();
				}
				this.m_Connection = ftpControlStream;
				if (Logging.On)
				{
					Logging.Associate(Logging.Web, this, this.m_Connection);
				}
			}
			return ftpControlStream;
		}

		// Token: 0x06000838 RID: 2104 RVA: 0x0002DBF0 File Offset: 0x0002BDF0
		private Stream TimedSubmitRequestHelper(bool async)
		{
			if (async)
			{
				if (this.m_RequestCompleteAsyncResult == null)
				{
					this.m_RequestCompleteAsyncResult = new LazyAsyncResult(null, null, null);
				}
				return this.m_Connection.SubmitRequest(this, true, true);
			}
			Stream stream = null;
			bool flag = false;
			TimerThread.Timer timer = this.TimerQueue.CreateTimer(this.m_TimerCallback, null);
			try
			{
				stream = this.m_Connection.SubmitRequest(this, false, true);
			}
			catch (Exception ex)
			{
				if ((!(ex is SocketException) && !(ex is ObjectDisposedException)) || !timer.HasExpired)
				{
					timer.Cancel();
					throw;
				}
				flag = true;
			}
			if (flag || !timer.Cancel())
			{
				this.m_TimedOut = true;
				throw new WebException(NetRes.GetWebStatusString(WebExceptionStatus.Timeout), WebExceptionStatus.Timeout);
			}
			if (stream != null)
			{
				object syncObject = this.m_SyncObject;
				lock (syncObject)
				{
					if (this.m_Aborted)
					{
						((ICloseEx)stream).CloseEx(CloseExState.Abort | CloseExState.Silent);
						this.CheckError();
						throw new InternalException();
					}
					this.m_Stream = stream;
				}
			}
			return stream;
		}

		// Token: 0x06000839 RID: 2105 RVA: 0x0002DD00 File Offset: 0x0002BF00
		private void TimerCallback(TimerThread.Timer timer, int timeNoticed, object context)
		{
			FtpControlStream connection = this.m_Connection;
			if (connection != null)
			{
				connection.AbortConnect();
			}
		}

		// Token: 0x17000190 RID: 400
		// (get) Token: 0x0600083A RID: 2106 RVA: 0x0002DD1D File Offset: 0x0002BF1D
		private TimerThread.Queue TimerQueue
		{
			get
			{
				if (this.m_TimerQueue == null)
				{
					this.m_TimerQueue = TimerThread.GetOrCreateQueue(this.RemainingTimeout);
				}
				return this.m_TimerQueue;
			}
		}

		// Token: 0x0600083B RID: 2107 RVA: 0x0002DD40 File Offset: 0x0002BF40
		private bool AttemptedRecovery(Exception e)
		{
			if (!(e is WebException) || ((WebException)e).InternalStatus != WebExceptionInternalStatus.Isolated)
			{
				if (e is ThreadAbortException || e is StackOverflowException || e is OutOfMemoryException || this.m_OnceFailed || this.m_Aborted || this.m_TimedOut || this.m_Connection == null || !this.m_Connection.RecoverableFailure)
				{
					return false;
				}
				this.m_OnceFailed = true;
			}
			object syncObject = this.m_SyncObject;
			lock (syncObject)
			{
				if (this.m_ConnectionPool == null || this.m_Connection == null)
				{
					return false;
				}
				this.m_Connection.CloseSocket();
				if (Logging.On)
				{
					Logging.PrintInfo(Logging.Web, this, "", global::System.SR.GetString("net_log_releasing_connection", new object[] { ValidationHelper.HashString(this.m_Connection) }));
				}
				this.m_ConnectionPool.PutConnection(this.m_Connection, this, this.RemainingTimeout);
				this.m_Connection = null;
			}
			return true;
		}

		// Token: 0x0600083C RID: 2108 RVA: 0x0002DE54 File Offset: 0x0002C054
		private void SetException(Exception exception)
		{
			if (exception is ThreadAbortException || exception is StackOverflowException || exception is OutOfMemoryException)
			{
				this.m_Exception = exception;
				throw exception;
			}
			FtpControlStream connection = this.m_Connection;
			if (this.m_Exception == null)
			{
				if (exception is WebException)
				{
					this.EnsureFtpWebResponse(exception);
					this.m_Exception = new WebException(exception.Message, null, ((WebException)exception).Status, this.m_FtpWebResponse);
				}
				else if (exception is AuthenticationException || exception is SecurityException)
				{
					this.m_Exception = exception;
				}
				else if (connection != null && connection.StatusCode != FtpStatusCode.Undefined)
				{
					this.EnsureFtpWebResponse(exception);
					this.m_Exception = new WebException(global::System.SR.GetString("net_servererror", new object[] { connection.StatusLine }), exception, WebExceptionStatus.ProtocolError, this.m_FtpWebResponse);
				}
				else
				{
					this.m_Exception = new WebException(exception.Message, exception);
				}
				if (connection != null && this.m_FtpWebResponse != null)
				{
					this.m_FtpWebResponse.UpdateStatus(connection.StatusCode, connection.StatusLine, connection.ExitMessage);
				}
			}
		}

		// Token: 0x0600083D RID: 2109 RVA: 0x0002DF5B File Offset: 0x0002C15B
		private void CheckError()
		{
			if (this.m_Exception != null)
			{
				throw this.m_Exception;
			}
		}

		// Token: 0x0600083E RID: 2110 RVA: 0x0002DF6C File Offset: 0x0002C16C
		internal override ContextAwareResult GetWritingContext()
		{
			if (this.m_ReadAsyncResult != null && this.m_ReadAsyncResult is ContextAwareResult)
			{
				return (ContextAwareResult)this.m_ReadAsyncResult;
			}
			if (this.m_WriteAsyncResult != null)
			{
				return this.m_WriteAsyncResult;
			}
			return null;
		}

		// Token: 0x0600083F RID: 2111 RVA: 0x0002DF9F File Offset: 0x0002C19F
		internal override void RequestCallback(object obj)
		{
			if (this.m_Async)
			{
				this.AsyncRequestCallback(obj);
				return;
			}
			this.SyncRequestCallback(obj);
		}

		// Token: 0x06000840 RID: 2112 RVA: 0x0002DFB8 File Offset: 0x0002C1B8
		private void SyncRequestCallback(object obj)
		{
			FtpWebRequest.RequestStage requestStage = FtpWebRequest.RequestStage.CheckForError;
			try
			{
				bool flag = obj == null;
				Exception ex = obj as Exception;
				if (ex != null)
				{
					this.SetException(ex);
				}
				else
				{
					if (!flag)
					{
						throw new InternalException();
					}
					FtpControlStream connection = this.m_Connection;
					bool flag2 = false;
					if (connection != null)
					{
						this.EnsureFtpWebResponse(null);
						this.m_FtpWebResponse.UpdateStatus(connection.StatusCode, connection.StatusLine, connection.ExitMessage);
						flag2 = !this.m_CacheDone && (base.CacheProtocol.ProtocolStatus == CacheValidationStatus.Continue || base.CacheProtocol.ProtocolStatus == CacheValidationStatus.RetryResponseFromServer);
						if (this.m_MethodInfo.IsUpload)
						{
							this.CheckCacheRetrieveOnResponse();
							this.CheckCacheUpdateOnResponse();
						}
					}
					if (!flag2)
					{
						requestStage = FtpWebRequest.RequestStage.ReleaseConnection;
					}
				}
			}
			catch (Exception ex2)
			{
				this.SetException(ex2);
			}
			finally
			{
				this.FinishRequestStage(requestStage);
				this.CheckError();
			}
		}

		// Token: 0x06000841 RID: 2113 RVA: 0x0002E0A4 File Offset: 0x0002C2A4
		private void AsyncRequestCallback(object obj)
		{
			FtpWebRequest.RequestStage requestStage = FtpWebRequest.RequestStage.CheckForError;
			try
			{
				FtpControlStream ftpControlStream = obj as FtpControlStream;
				FtpDataStream ftpDataStream = obj as FtpDataStream;
				Exception ex = obj as Exception;
				bool flag = obj == null;
				bool flag3;
				for (;;)
				{
					if (ex != null)
					{
						if (this.AttemptedRecovery(ex))
						{
							ftpControlStream = this.QueueOrCreateConnection();
							if (ftpControlStream == null)
							{
								break;
							}
							ex = null;
						}
						if (ex != null)
						{
							goto Block_7;
						}
					}
					if (ftpControlStream != null)
					{
						object syncObject = this.m_SyncObject;
						lock (syncObject)
						{
							if (this.m_Aborted)
							{
								if (Logging.On)
								{
									Logging.PrintInfo(Logging.Web, this, "", global::System.SR.GetString("net_log_releasing_connection", new object[] { ValidationHelper.HashString(ftpControlStream) }));
								}
								this.m_ConnectionPool.PutConnection(ftpControlStream, this, this.Timeout);
								break;
							}
							this.m_Connection = ftpControlStream;
							if (Logging.On)
							{
								Logging.Associate(Logging.Web, this, this.m_Connection);
							}
						}
						try
						{
							ftpDataStream = (FtpDataStream)this.TimedSubmitRequestHelper(true);
						}
						catch (Exception ex2)
						{
							ex = ex2;
							continue;
						}
						break;
					}
					if (ftpDataStream != null)
					{
						goto Block_11;
					}
					if (!flag)
					{
						goto IL_0228;
					}
					ftpControlStream = this.m_Connection;
					flag3 = false;
					if (ftpControlStream != null)
					{
						this.EnsureFtpWebResponse(null);
						this.m_FtpWebResponse.UpdateStatus(ftpControlStream.StatusCode, ftpControlStream.StatusLine, ftpControlStream.ExitMessage);
						flag3 = !this.m_CacheDone && (base.CacheProtocol.ProtocolStatus == CacheValidationStatus.Continue || base.CacheProtocol.ProtocolStatus == CacheValidationStatus.RetryResponseFromServer);
						object syncObject2 = this.m_SyncObject;
						lock (syncObject2)
						{
							if (!this.CheckCacheRetrieveOnResponse())
							{
								continue;
							}
							if (this.m_FtpWebResponse.IsFromCache)
							{
								flag3 = false;
							}
							this.CheckCacheUpdateOnResponse();
						}
						goto IL_0220;
					}
					goto IL_0220;
				}
				return;
				Block_7:
				this.SetException(ex);
				return;
				Block_11:
				object syncObject3 = this.m_SyncObject;
				lock (syncObject3)
				{
					if (this.m_Aborted)
					{
						((ICloseEx)ftpDataStream).CloseEx(CloseExState.Abort | CloseExState.Silent);
						goto IL_022E;
					}
					this.m_Stream = ftpDataStream;
				}
				ftpDataStream.SetSocketTimeoutOption(SocketShutdown.Both, this.Timeout, true);
				this.EnsureFtpWebResponse(null);
				this.CheckCacheRetrieveOnResponse();
				this.CheckCacheUpdateOnResponse();
				requestStage = (ftpDataStream.CanRead ? FtpWebRequest.RequestStage.ReadReady : FtpWebRequest.RequestStage.WriteReady);
				goto IL_022E;
				IL_0220:
				if (!flag3)
				{
					requestStage = FtpWebRequest.RequestStage.ReleaseConnection;
					goto IL_022E;
				}
				goto IL_022E;
				IL_0228:
				throw new InternalException();
				IL_022E:;
			}
			catch (Exception ex3)
			{
				this.SetException(ex3);
			}
			finally
			{
				this.FinishRequestStage(requestStage);
			}
		}

		// Token: 0x06000842 RID: 2114 RVA: 0x0002E38C File Offset: 0x0002C58C
		private FtpWebRequest.RequestStage FinishRequestStage(FtpWebRequest.RequestStage stage)
		{
			if (this.m_Exception != null)
			{
				stage = FtpWebRequest.RequestStage.ReleaseConnection;
			}
			object syncObject = this.m_SyncObject;
			FtpWebRequest.RequestStage requestStage;
			LazyAsyncResult writeAsyncResult;
			LazyAsyncResult readAsyncResult;
			FtpControlStream connection;
			lock (syncObject)
			{
				requestStage = this.m_RequestStage;
				if (stage == FtpWebRequest.RequestStage.CheckForError)
				{
					return requestStage;
				}
				if (requestStage == FtpWebRequest.RequestStage.ReleaseConnection && stage == FtpWebRequest.RequestStage.ReleaseConnection)
				{
					return FtpWebRequest.RequestStage.ReleaseConnection;
				}
				if (stage > requestStage)
				{
					this.m_RequestStage = stage;
				}
				if (stage <= FtpWebRequest.RequestStage.RequestStarted)
				{
					return requestStage;
				}
				writeAsyncResult = this.m_WriteAsyncResult;
				readAsyncResult = this.m_ReadAsyncResult;
				connection = this.m_Connection;
				if (stage == FtpWebRequest.RequestStage.ReleaseConnection)
				{
					if (this.m_Exception == null && !this.m_Aborted && requestStage != FtpWebRequest.RequestStage.ReadReady && this.m_MethodInfo.IsDownload && !this.m_FtpWebResponse.IsFromCache)
					{
						return requestStage;
					}
					if (this.m_Exception != null || !this.m_FtpWebResponse.IsFromCache || this.KeepAlive)
					{
						this.m_Connection = null;
					}
				}
			}
			FtpWebRequest.RequestStage requestStage2;
			try
			{
				if ((stage == FtpWebRequest.RequestStage.ReleaseConnection || requestStage == FtpWebRequest.RequestStage.ReleaseConnection) && connection != null)
				{
					try
					{
						if (this.m_Exception != null)
						{
							connection.Abort(this.m_Exception);
						}
						else if (this.m_FtpWebResponse.IsFromCache && !this.KeepAlive)
						{
							connection.Quit();
						}
					}
					finally
					{
						if (Logging.On)
						{
							Logging.PrintInfo(Logging.Web, this, "", global::System.SR.GetString("net_log_releasing_connection", new object[] { ValidationHelper.HashString(connection) }));
						}
						this.m_ConnectionPool.PutConnection(connection, this, this.RemainingTimeout);
						if (this.m_Async && this.m_RequestCompleteAsyncResult != null)
						{
							this.m_RequestCompleteAsyncResult.InvokeCallback();
						}
					}
				}
				requestStage2 = requestStage;
			}
			finally
			{
				try
				{
					if (stage >= FtpWebRequest.RequestStage.WriteReady)
					{
						if (this.m_MethodInfo.IsUpload && !this.m_GetRequestStreamStarted)
						{
							if (this.m_Stream != null)
							{
								this.m_Stream.Close();
							}
						}
						else if (writeAsyncResult != null && !writeAsyncResult.InternalPeekCompleted)
						{
							writeAsyncResult.InvokeCallback();
						}
					}
				}
				finally
				{
					if (stage >= FtpWebRequest.RequestStage.ReadReady && readAsyncResult != null && !readAsyncResult.InternalPeekCompleted)
					{
						readAsyncResult.InvokeCallback();
					}
				}
			}
			return requestStage2;
		}

		// Token: 0x06000843 RID: 2115 RVA: 0x0002E5AC File Offset: 0x0002C7AC
		private static void AsyncCallbackWrapper(object request, object state)
		{
			FtpWebRequest ftpWebRequest = (FtpWebRequest)request;
			ftpWebRequest.RequestCallback(state);
		}

		// Token: 0x06000844 RID: 2116 RVA: 0x0002E5C7 File Offset: 0x0002C7C7
		private static PooledStream CreateFtpConnection(ConnectionPool pool)
		{
			return new FtpControlStream(pool, TimeSpan.MaxValue, false);
		}

		// Token: 0x06000845 RID: 2117 RVA: 0x0002E5D8 File Offset: 0x0002C7D8
		public override void Abort()
		{
			if (this.m_Aborted)
			{
				return;
			}
			if (Logging.On)
			{
				Logging.Enter(Logging.Web, this, "Abort", "");
			}
			try
			{
				if (this.HttpProxyMode)
				{
					this.GetHttpWebRequest().Abort();
				}
				else
				{
					if (base.CacheProtocol != null)
					{
						base.CacheProtocol.Abort();
					}
					object syncObject = this.m_SyncObject;
					Stream stream;
					FtpControlStream connection;
					lock (syncObject)
					{
						if (this.m_RequestStage >= FtpWebRequest.RequestStage.ReleaseConnection)
						{
							return;
						}
						this.m_Aborted = true;
						stream = this.m_Stream;
						connection = this.m_Connection;
						this.m_Exception = new WebException(NetRes.GetWebStatusString("net_requestaborted", WebExceptionStatus.RequestCanceled), WebExceptionStatus.RequestCanceled);
					}
					if (stream != null)
					{
						((ICloseEx)stream).CloseEx(CloseExState.Abort | CloseExState.Silent);
					}
					if (connection != null)
					{
						connection.Abort(ExceptionHelper.RequestAbortedException);
					}
				}
			}
			catch (Exception ex)
			{
				if (Logging.On)
				{
					Logging.Exception(Logging.Web, this, "Abort", ex);
				}
				throw;
			}
			finally
			{
				if (Logging.On)
				{
					Logging.Exit(Logging.Web, this, "Abort", "");
				}
			}
		}

		// Token: 0x17000191 RID: 401
		// (get) Token: 0x06000846 RID: 2118 RVA: 0x0002E710 File Offset: 0x0002C910
		// (set) Token: 0x06000847 RID: 2119 RVA: 0x0002E718 File Offset: 0x0002C918
		public bool KeepAlive
		{
			get
			{
				return this.m_KeepAlive;
			}
			set
			{
				if (this.InUse)
				{
					throw new InvalidOperationException(global::System.SR.GetString("net_reqsubmitted"));
				}
				this.m_KeepAlive = value;
			}
		}

		// Token: 0x17000192 RID: 402
		// (get) Token: 0x06000848 RID: 2120 RVA: 0x0002E739 File Offset: 0x0002C939
		// (set) Token: 0x06000849 RID: 2121 RVA: 0x0002E741 File Offset: 0x0002C941
		public bool UseBinary
		{
			get
			{
				return this.m_Binary;
			}
			set
			{
				if (this.InUse)
				{
					throw new InvalidOperationException(global::System.SR.GetString("net_reqsubmitted"));
				}
				this.m_Binary = value;
			}
		}

		// Token: 0x17000193 RID: 403
		// (get) Token: 0x0600084A RID: 2122 RVA: 0x0002E762 File Offset: 0x0002C962
		// (set) Token: 0x0600084B RID: 2123 RVA: 0x0002E76A File Offset: 0x0002C96A
		public bool UsePassive
		{
			get
			{
				return this.m_Passive;
			}
			set
			{
				if (this.InUse)
				{
					throw new InvalidOperationException(global::System.SR.GetString("net_reqsubmitted"));
				}
				this.m_Passive = value;
			}
		}

		// Token: 0x17000194 RID: 404
		// (get) Token: 0x0600084C RID: 2124 RVA: 0x0002E78C File Offset: 0x0002C98C
		// (set) Token: 0x0600084D RID: 2125 RVA: 0x0002E7E8 File Offset: 0x0002C9E8
		public X509CertificateCollection ClientCertificates
		{
			get
			{
				if (this.m_ClientCertificates == null)
				{
					object syncObject = this.m_SyncObject;
					lock (syncObject)
					{
						if (this.m_ClientCertificates == null)
						{
							this.m_ClientCertificates = new X509CertificateCollection();
						}
					}
				}
				return this.m_ClientCertificates;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				this.m_ClientCertificates = value;
			}
		}

		// Token: 0x17000195 RID: 405
		// (get) Token: 0x0600084E RID: 2126 RVA: 0x0002E7FF File Offset: 0x0002C9FF
		// (set) Token: 0x0600084F RID: 2127 RVA: 0x0002E807 File Offset: 0x0002CA07
		public bool EnableSsl
		{
			get
			{
				return this.m_EnableSsl;
			}
			set
			{
				if (this.InUse)
				{
					throw new InvalidOperationException(global::System.SR.GetString("net_reqsubmitted"));
				}
				this.m_EnableSsl = value;
			}
		}

		// Token: 0x17000196 RID: 406
		// (get) Token: 0x06000850 RID: 2128 RVA: 0x0002E828 File Offset: 0x0002CA28
		// (set) Token: 0x06000851 RID: 2129 RVA: 0x0002E858 File Offset: 0x0002CA58
		public override WebHeaderCollection Headers
		{
			get
			{
				if (this.HttpProxyMode)
				{
					return this.GetHttpWebRequest().Headers;
				}
				if (this.m_FtpRequestHeaders == null)
				{
					this.m_FtpRequestHeaders = new WebHeaderCollection(WebHeaderCollectionType.FtpWebRequest);
				}
				return this.m_FtpRequestHeaders;
			}
			set
			{
				if (this.HttpProxyMode)
				{
					this.GetHttpWebRequest().Headers = value;
				}
				this.m_FtpRequestHeaders = value;
			}
		}

		// Token: 0x17000197 RID: 407
		// (get) Token: 0x06000852 RID: 2130 RVA: 0x0002E875 File Offset: 0x0002CA75
		// (set) Token: 0x06000853 RID: 2131 RVA: 0x0002E87C File Offset: 0x0002CA7C
		public override string ContentType
		{
			get
			{
				throw ExceptionHelper.PropertyNotSupportedException;
			}
			set
			{
				throw ExceptionHelper.PropertyNotSupportedException;
			}
		}

		// Token: 0x17000198 RID: 408
		// (get) Token: 0x06000854 RID: 2132 RVA: 0x0002E883 File Offset: 0x0002CA83
		// (set) Token: 0x06000855 RID: 2133 RVA: 0x0002E88A File Offset: 0x0002CA8A
		public override bool UseDefaultCredentials
		{
			get
			{
				throw ExceptionHelper.PropertyNotSupportedException;
			}
			set
			{
				throw ExceptionHelper.PropertyNotSupportedException;
			}
		}

		// Token: 0x17000199 RID: 409
		// (get) Token: 0x06000856 RID: 2134 RVA: 0x0002E891 File Offset: 0x0002CA91
		// (set) Token: 0x06000857 RID: 2135 RVA: 0x0002E898 File Offset: 0x0002CA98
		public override bool PreAuthenticate
		{
			get
			{
				throw ExceptionHelper.PropertyNotSupportedException;
			}
			set
			{
				throw ExceptionHelper.PropertyNotSupportedException;
			}
		}

		// Token: 0x1700019A RID: 410
		// (get) Token: 0x06000858 RID: 2136 RVA: 0x0002E89F File Offset: 0x0002CA9F
		private bool InUse
		{
			get
			{
				return this.m_GetRequestStreamStarted || this.m_GetResponseStarted;
			}
		}

		// Token: 0x1700019B RID: 411
		// (get) Token: 0x06000859 RID: 2137 RVA: 0x0002E8B4 File Offset: 0x0002CAB4
		private bool HttpProxyMode
		{
			get
			{
				return this.m_HttpWebRequest != null;
			}
		}

		// Token: 0x0600085A RID: 2138 RVA: 0x0002E8C0 File Offset: 0x0002CAC0
		private void EnsureFtpWebResponse(Exception exception)
		{
			if (this.m_FtpWebResponse == null || (this.m_FtpWebResponse.GetResponseStream() is FtpWebResponse.EmptyStream && this.m_Stream != null))
			{
				object syncObject = this.m_SyncObject;
				lock (syncObject)
				{
					if (this.m_FtpWebResponse == null || (this.m_FtpWebResponse.GetResponseStream() is FtpWebResponse.EmptyStream && this.m_Stream != null))
					{
						Stream stream = this.m_Stream;
						if (this.m_MethodInfo.IsUpload)
						{
							stream = null;
						}
						if (this.m_Stream != null && this.m_Stream.CanRead && this.m_Stream.CanTimeout)
						{
							this.m_Stream.ReadTimeout = this.ReadWriteTimeout;
							this.m_Stream.WriteTimeout = this.ReadWriteTimeout;
						}
						FtpControlStream connection = this.m_Connection;
						long num = ((connection != null) ? connection.ContentLength : (-1L));
						if (stream == null && num < 0L)
						{
							num = 0L;
						}
						if (this.m_FtpWebResponse != null)
						{
							this.m_FtpWebResponse.SetResponseStream(stream);
						}
						else if (connection != null)
						{
							this.m_FtpWebResponse = new FtpWebResponse(stream, num, connection.ResponseUri, connection.StatusCode, connection.StatusLine, connection.LastModified, connection.BannerMessage, connection.WelcomeMessage, connection.ExitMessage);
						}
						else
						{
							this.m_FtpWebResponse = new FtpWebResponse(stream, -1L, this.m_Uri, FtpStatusCode.Undefined, null, DateTime.Now, null, null, null);
						}
					}
				}
			}
		}

		// Token: 0x0600085B RID: 2139 RVA: 0x0002EA44 File Offset: 0x0002CC44
		private HttpWebRequest GetHttpWebRequest()
		{
			object syncObject = this.m_SyncObject;
			lock (syncObject)
			{
				if (this.m_HttpWebRequest == null)
				{
					if (this.m_ContentOffset > 0L)
					{
						throw new InvalidOperationException(global::System.SR.GetString("net_ftp_no_offsetforhttp"));
					}
					if (!this.m_MethodInfo.HasHttpCommand)
					{
						throw new InvalidOperationException(global::System.SR.GetString("net_ftp_no_http_cmd"));
					}
					this.m_HttpWebRequest = new HttpWebRequest(this.m_Uri, this.ServicePoint);
					this.m_HttpWebRequest.Credentials = this.Credentials;
					this.m_HttpWebRequest.InternalProxy = this.m_Proxy;
					this.m_HttpWebRequest.KeepAlive = this.KeepAlive;
					this.m_HttpWebRequest.Timeout = this.Timeout;
					this.m_HttpWebRequest.Method = this.m_MethodInfo.HttpCommand;
					this.m_HttpWebRequest.CacheProtocol = base.CacheProtocol;
					RequestCacheLevel requestCacheLevel;
					if (this.CachePolicy == null)
					{
						requestCacheLevel = RequestCacheLevel.BypassCache;
					}
					else
					{
						requestCacheLevel = this.CachePolicy.Level;
					}
					if (requestCacheLevel == RequestCacheLevel.Revalidate)
					{
						requestCacheLevel = RequestCacheLevel.Reload;
					}
					this.m_HttpWebRequest.CachePolicy = new HttpRequestCachePolicy((HttpRequestCacheLevel)requestCacheLevel);
					base.CacheProtocol = null;
				}
			}
			return this.m_HttpWebRequest;
		}

		// Token: 0x0600085C RID: 2140 RVA: 0x0002EB8C File Offset: 0x0002CD8C
		private string GetConnectionGroupLine()
		{
			return this.ConnectionGroupName + "_" + this.GetUserString();
		}

		// Token: 0x0600085D RID: 2141 RVA: 0x0002EBA4 File Offset: 0x0002CDA4
		internal string GetUserString()
		{
			string text = null;
			if (this.Credentials != null)
			{
				NetworkCredential credential = this.Credentials.GetCredential(this.m_Uri, "basic");
				if (credential != null)
				{
					text = credential.InternalGetUserName();
					string text2 = credential.InternalGetDomain();
					if (!ValidationHelper.IsBlankString(text2))
					{
						text = text2 + "\\" + text;
					}
				}
			}
			if (text == null)
			{
				return null;
			}
			if (string.Compare(text, "anonymous", StringComparison.InvariantCultureIgnoreCase) != 0)
			{
				return text;
			}
			return null;
		}

		// Token: 0x0600085E RID: 2142 RVA: 0x0002EC10 File Offset: 0x0002CE10
		private bool CheckCacheRetrieveBeforeSubmit()
		{
			if (base.CacheProtocol == null || this.m_CacheDone)
			{
				this.m_CacheDone = true;
				return false;
			}
			if (base.CacheProtocol.ProtocolStatus == CacheValidationStatus.CombineCachedAndServerResponse || base.CacheProtocol.ProtocolStatus == CacheValidationStatus.DoNotTakeFromCache)
			{
				return false;
			}
			Uri uri = this.RequestUri;
			string text = this.GetUserString();
			if (text != null)
			{
				text = Uri.EscapeDataString(text);
			}
			if (uri.Fragment.Length != 0 || text != null)
			{
				if (text == null)
				{
					uri = new Uri(uri.GetParts(UriComponents.HttpRequestUrl, UriFormat.SafeUnescaped));
				}
				else
				{
					text = uri.GetParts(UriComponents.Scheme | UriComponents.KeepDelimiter, UriFormat.SafeUnescaped) + text + "@";
					text += uri.GetParts(UriComponents.Host | UriComponents.Port | UriComponents.Path | UriComponents.Query, UriFormat.SafeUnescaped);
					uri = new Uri(text);
				}
			}
			base.CacheProtocol.GetRetrieveStatus(uri, this);
			if (base.CacheProtocol.ProtocolStatus == CacheValidationStatus.Fail)
			{
				throw base.CacheProtocol.ProtocolException;
			}
			if (base.CacheProtocol.ProtocolStatus != CacheValidationStatus.ReturnCachedResponse)
			{
				return false;
			}
			if (this.m_MethodInfo.Operation != FtpOperation.DownloadFile)
			{
				throw new NotSupportedException(global::System.SR.GetString("net_cache_not_supported_command"));
			}
			if (base.CacheProtocol.ProtocolStatus == CacheValidationStatus.ReturnCachedResponse)
			{
				FtpRequestCacheValidator ftpRequestCacheValidator = (FtpRequestCacheValidator)base.CacheProtocol.Validator;
				this.m_FtpWebResponse = new FtpWebResponse(base.CacheProtocol.ResponseStream, base.CacheProtocol.ResponseStreamLength, this.RequestUri, this.UsePassive ? FtpStatusCode.DataAlreadyOpen : FtpStatusCode.OpeningData, (this.UsePassive ? FtpStatusCode.DataAlreadyOpen : FtpStatusCode.OpeningData).ToString(), (ftpRequestCacheValidator.CacheEntry.LastModifiedUtc == DateTime.MinValue) ? DateTime.Now : ftpRequestCacheValidator.CacheEntry.LastModifiedUtc.ToLocalTime(), string.Empty, string.Empty, string.Empty);
				this.m_FtpWebResponse.InternalSetFromCache = true;
				this.m_FtpWebResponse.InternalSetIsCacheFresh = ftpRequestCacheValidator.CacheFreshnessStatus != CacheFreshnessStatus.Stale;
			}
			return true;
		}

		// Token: 0x0600085F RID: 2143 RVA: 0x0002EDF4 File Offset: 0x0002CFF4
		private bool CheckCacheRetrieveOnResponse()
		{
			if (base.CacheProtocol == null || this.m_CacheDone)
			{
				return true;
			}
			if (base.CacheProtocol.ProtocolStatus != CacheValidationStatus.Continue)
			{
				return true;
			}
			if (base.CacheProtocol.ProtocolStatus == CacheValidationStatus.Fail)
			{
				if (Logging.On)
				{
					Logging.Exception(Logging.Web, this, "CheckCacheRetrieveOnResponse", base.CacheProtocol.ProtocolException);
				}
				throw base.CacheProtocol.ProtocolException;
			}
			base.CacheProtocol.GetRevalidateStatus(this.m_FtpWebResponse, null);
			if (base.CacheProtocol.ProtocolStatus == CacheValidationStatus.RetryResponseFromServer)
			{
				if (this.m_FtpWebResponse != null)
				{
					this.m_FtpWebResponse.SetResponseStream(null);
				}
				return false;
			}
			if (base.CacheProtocol.ProtocolStatus != CacheValidationStatus.ReturnCachedResponse)
			{
				return false;
			}
			if (this.m_MethodInfo.Operation != FtpOperation.DownloadFile)
			{
				throw new NotSupportedException(global::System.SR.GetString("net_cache_not_supported_command"));
			}
			FtpRequestCacheValidator ftpRequestCacheValidator = (FtpRequestCacheValidator)base.CacheProtocol.Validator;
			FtpWebResponse ftpWebResponse = this.m_FtpWebResponse;
			this.m_Stream = base.CacheProtocol.ResponseStream;
			this.m_FtpWebResponse = new FtpWebResponse(base.CacheProtocol.ResponseStream, base.CacheProtocol.ResponseStreamLength, this.RequestUri, this.UsePassive ? FtpStatusCode.DataAlreadyOpen : FtpStatusCode.OpeningData, (this.UsePassive ? FtpStatusCode.DataAlreadyOpen : FtpStatusCode.OpeningData).ToString(), (ftpRequestCacheValidator.CacheEntry.LastModifiedUtc == DateTime.MinValue) ? DateTime.Now : ftpRequestCacheValidator.CacheEntry.LastModifiedUtc.ToLocalTime(), string.Empty, string.Empty, string.Empty);
			this.m_FtpWebResponse.InternalSetFromCache = true;
			this.m_FtpWebResponse.InternalSetIsCacheFresh = base.CacheProtocol.IsCacheFresh;
			ftpWebResponse.Close();
			return true;
		}

		// Token: 0x06000860 RID: 2144 RVA: 0x0002EFB0 File Offset: 0x0002D1B0
		private void CheckCacheUpdateOnResponse()
		{
			if (base.CacheProtocol == null || this.m_CacheDone)
			{
				return;
			}
			this.m_CacheDone = true;
			if (this.m_Connection != null)
			{
				this.m_FtpWebResponse.UpdateStatus(this.m_Connection.StatusCode, this.m_Connection.StatusLine, this.m_Connection.ExitMessage);
				if (this.m_Connection.StatusCode == FtpStatusCode.OpeningData && this.m_FtpWebResponse.ContentLength == 0L)
				{
					this.m_FtpWebResponse.SetContentLength(this.m_Connection.ContentLength);
				}
			}
			if (base.CacheProtocol.ProtocolStatus == CacheValidationStatus.CombineCachedAndServerResponse)
			{
				this.m_Stream = new CombinedReadStream(base.CacheProtocol.Validator.CacheStream, this.m_FtpWebResponse.GetResponseStream());
				FtpStatusCode ftpStatusCode = (this.UsePassive ? FtpStatusCode.DataAlreadyOpen : FtpStatusCode.OpeningData);
				this.m_FtpWebResponse.UpdateStatus(ftpStatusCode, ftpStatusCode.ToString(), string.Empty);
				this.m_FtpWebResponse.SetResponseStream(this.m_Stream);
			}
			if (base.CacheProtocol.GetUpdateStatus(this.m_FtpWebResponse, this.m_FtpWebResponse.GetResponseStream()) == CacheValidationStatus.UpdateResponseInformation)
			{
				this.m_Stream = base.CacheProtocol.ResponseStream;
				this.m_FtpWebResponse.SetResponseStream(this.m_Stream);
				return;
			}
			if (base.CacheProtocol.ProtocolStatus == CacheValidationStatus.Fail)
			{
				throw base.CacheProtocol.ProtocolException;
			}
		}

		// Token: 0x06000861 RID: 2145 RVA: 0x0002F110 File Offset: 0x0002D310
		internal void DataStreamClosed(CloseExState closeState)
		{
			if ((closeState & CloseExState.Abort) == CloseExState.Normal)
			{
				if (this.m_Async)
				{
					this.m_RequestCompleteAsyncResult.InternalWaitForCompletion();
					this.CheckError();
					return;
				}
				if (this.m_Connection != null)
				{
					this.m_Connection.CheckContinuePipeline();
					return;
				}
			}
			else
			{
				FtpControlStream connection = this.m_Connection;
				if (connection != null)
				{
					connection.Abort(ExceptionHelper.RequestAbortedException);
				}
			}
		}

		// Token: 0x06000862 RID: 2146 RVA: 0x0002F168 File Offset: 0x0002D368
		private static int GetStatusCode(WebException webException)
		{
			int num = -1;
			if (FrameworkEventSource.Log.IsEnabled() && webException != null && webException.Response != null)
			{
				HttpWebResponse httpWebResponse = webException.Response as HttpWebResponse;
				if (httpWebResponse != null)
				{
					try
					{
						return (int)httpWebResponse.StatusCode;
					}
					catch (ObjectDisposedException)
					{
						return num;
					}
				}
				FtpWebResponse ftpWebResponse = webException.Response as FtpWebResponse;
				num = FtpWebRequest.GetStatusCode(ftpWebResponse);
			}
			return num;
		}

		// Token: 0x06000863 RID: 2147 RVA: 0x0002F1D0 File Offset: 0x0002D3D0
		private static int GetStatusCode(FtpWebResponse ftpWebResponse)
		{
			int num = -1;
			if (FrameworkEventSource.Log.IsEnabled() && ftpWebResponse != null)
			{
				try
				{
					num = (int)ftpWebResponse.StatusCode;
				}
				catch (ObjectDisposedException)
				{
				}
			}
			return num;
		}

		// Token: 0x04000DA0 RID: 3488
		private object m_SyncObject;

		// Token: 0x04000DA1 RID: 3489
		private ICredentials m_AuthInfo;

		// Token: 0x04000DA2 RID: 3490
		private readonly Uri m_Uri;

		// Token: 0x04000DA3 RID: 3491
		private FtpMethodInfo m_MethodInfo;

		// Token: 0x04000DA4 RID: 3492
		private string m_RenameTo;

		// Token: 0x04000DA5 RID: 3493
		private bool m_GetRequestStreamStarted;

		// Token: 0x04000DA6 RID: 3494
		private bool m_GetResponseStarted;

		// Token: 0x04000DA7 RID: 3495
		private DateTime m_StartTime;

		// Token: 0x04000DA8 RID: 3496
		private int m_Timeout = FtpWebRequest.s_DefaultTimeout;

		// Token: 0x04000DA9 RID: 3497
		private int m_RemainingTimeout;

		// Token: 0x04000DAA RID: 3498
		private long m_ContentLength;

		// Token: 0x04000DAB RID: 3499
		private long m_ContentOffset;

		// Token: 0x04000DAC RID: 3500
		private IWebProxy m_Proxy;

		// Token: 0x04000DAD RID: 3501
		private X509CertificateCollection m_ClientCertificates;

		// Token: 0x04000DAE RID: 3502
		private bool m_KeepAlive = true;

		// Token: 0x04000DAF RID: 3503
		private bool m_Passive = true;

		// Token: 0x04000DB0 RID: 3504
		private bool m_Binary = true;

		// Token: 0x04000DB1 RID: 3505
		private string m_ConnectionGroupName;

		// Token: 0x04000DB2 RID: 3506
		private ServicePoint m_ServicePoint;

		// Token: 0x04000DB3 RID: 3507
		private bool m_CacheDone;

		// Token: 0x04000DB4 RID: 3508
		private bool m_Async;

		// Token: 0x04000DB5 RID: 3509
		private bool m_Aborted;

		// Token: 0x04000DB6 RID: 3510
		private bool m_TimedOut;

		// Token: 0x04000DB7 RID: 3511
		private HttpWebRequest m_HttpWebRequest;

		// Token: 0x04000DB8 RID: 3512
		private Exception m_Exception;

		// Token: 0x04000DB9 RID: 3513
		private TimerThread.Queue m_TimerQueue = FtpWebRequest.s_DefaultTimerQueue;

		// Token: 0x04000DBA RID: 3514
		private TimerThread.Callback m_TimerCallback;

		// Token: 0x04000DBB RID: 3515
		private bool m_EnableSsl;

		// Token: 0x04000DBC RID: 3516
		private bool m_ProxyUserSet;

		// Token: 0x04000DBD RID: 3517
		private ConnectionPool m_ConnectionPool;

		// Token: 0x04000DBE RID: 3518
		private FtpControlStream m_Connection;

		// Token: 0x04000DBF RID: 3519
		private Stream m_Stream;

		// Token: 0x04000DC0 RID: 3520
		private FtpWebRequest.RequestStage m_RequestStage;

		// Token: 0x04000DC1 RID: 3521
		private bool m_OnceFailed;

		// Token: 0x04000DC2 RID: 3522
		private WebHeaderCollection m_FtpRequestHeaders;

		// Token: 0x04000DC3 RID: 3523
		private FtpWebResponse m_FtpWebResponse;

		// Token: 0x04000DC4 RID: 3524
		private int m_ReadWriteTimeout = 300000;

		// Token: 0x04000DC5 RID: 3525
		private ContextAwareResult m_WriteAsyncResult;

		// Token: 0x04000DC6 RID: 3526
		private LazyAsyncResult m_ReadAsyncResult;

		// Token: 0x04000DC7 RID: 3527
		private LazyAsyncResult m_RequestCompleteAsyncResult;

		// Token: 0x04000DC8 RID: 3528
		private static readonly GeneralAsyncDelegate m_AsyncCallback = new GeneralAsyncDelegate(FtpWebRequest.AsyncCallbackWrapper);

		// Token: 0x04000DC9 RID: 3529
		private static readonly CreateConnectionDelegate m_CreateConnectionCallback = new CreateConnectionDelegate(FtpWebRequest.CreateFtpConnection);

		// Token: 0x04000DCA RID: 3530
		private static readonly NetworkCredential DefaultFtpNetworkCredential = new NetworkCredential("anonymous", "anonymous@", string.Empty);

		// Token: 0x04000DCB RID: 3531
		private static readonly int s_DefaultTimeout = 100000;

		// Token: 0x04000DCC RID: 3532
		private static readonly TimerThread.Queue s_DefaultTimerQueue = TimerThread.GetOrCreateQueue(FtpWebRequest.s_DefaultTimeout);

		// Token: 0x020006FB RID: 1787
		private enum RequestStage
		{
			// Token: 0x040030B3 RID: 12467
			CheckForError,
			// Token: 0x040030B4 RID: 12468
			RequestStarted,
			// Token: 0x040030B5 RID: 12469
			WriteReady,
			// Token: 0x040030B6 RID: 12470
			ReadReady,
			// Token: 0x040030B7 RID: 12471
			ReleaseConnection
		}
	}
}
