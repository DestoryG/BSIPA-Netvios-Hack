using System;
using System.Diagnostics.Tracing;
using System.IO;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Threading;

namespace System.Net
{
	// Token: 0x020000E6 RID: 230
	[Serializable]
	public class FileWebRequest : WebRequest, ISerializable
	{
		// Token: 0x060007CD RID: 1997 RVA: 0x0002B4C0 File Offset: 0x000296C0
		internal FileWebRequest(Uri uri)
		{
			if (uri.Scheme != Uri.UriSchemeFile)
			{
				throw new ArgumentOutOfRangeException("uri");
			}
			this.m_uri = uri;
			this.m_fileAccess = FileAccess.Read;
			this.m_headers = new WebHeaderCollection(WebHeaderCollectionType.FileWebRequest);
		}

		// Token: 0x060007CE RID: 1998 RVA: 0x0002B51C File Offset: 0x0002971C
		[Obsolete("Serialization is obsoleted for this type. http://go.microsoft.com/fwlink/?linkid=14202")]
		protected FileWebRequest(SerializationInfo serializationInfo, StreamingContext streamingContext)
			: base(serializationInfo, streamingContext)
		{
			this.m_headers = (WebHeaderCollection)serializationInfo.GetValue("headers", typeof(WebHeaderCollection));
			this.m_proxy = (IWebProxy)serializationInfo.GetValue("proxy", typeof(IWebProxy));
			this.m_uri = (Uri)serializationInfo.GetValue("uri", typeof(Uri));
			this.m_connectionGroupName = serializationInfo.GetString("connectionGroupName");
			this.m_method = serializationInfo.GetString("method");
			this.m_contentLength = serializationInfo.GetInt64("contentLength");
			this.m_timeout = serializationInfo.GetInt32("timeout");
			this.m_fileAccess = (FileAccess)serializationInfo.GetInt32("fileAccess");
		}

		// Token: 0x060007CF RID: 1999 RVA: 0x0002B5FC File Offset: 0x000297FC
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter, SerializationFormatter = true)]
		void ISerializable.GetObjectData(SerializationInfo serializationInfo, StreamingContext streamingContext)
		{
			this.GetObjectData(serializationInfo, streamingContext);
		}

		// Token: 0x060007D0 RID: 2000 RVA: 0x0002B608 File Offset: 0x00029808
		[SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
		protected override void GetObjectData(SerializationInfo serializationInfo, StreamingContext streamingContext)
		{
			serializationInfo.AddValue("headers", this.m_headers, typeof(WebHeaderCollection));
			serializationInfo.AddValue("proxy", this.m_proxy, typeof(IWebProxy));
			serializationInfo.AddValue("uri", this.m_uri, typeof(Uri));
			serializationInfo.AddValue("connectionGroupName", this.m_connectionGroupName);
			serializationInfo.AddValue("method", this.m_method);
			serializationInfo.AddValue("contentLength", this.m_contentLength);
			serializationInfo.AddValue("timeout", this.m_timeout);
			serializationInfo.AddValue("fileAccess", this.m_fileAccess);
			serializationInfo.AddValue("preauthenticate", false);
			base.GetObjectData(serializationInfo, streamingContext);
		}

		// Token: 0x1700016A RID: 362
		// (get) Token: 0x060007D1 RID: 2001 RVA: 0x0002B6D4 File Offset: 0x000298D4
		internal bool Aborted
		{
			get
			{
				return this.m_Aborted != 0;
			}
		}

		// Token: 0x1700016B RID: 363
		// (get) Token: 0x060007D2 RID: 2002 RVA: 0x0002B6DF File Offset: 0x000298DF
		// (set) Token: 0x060007D3 RID: 2003 RVA: 0x0002B6E7 File Offset: 0x000298E7
		public override string ConnectionGroupName
		{
			get
			{
				return this.m_connectionGroupName;
			}
			set
			{
				this.m_connectionGroupName = value;
			}
		}

		// Token: 0x1700016C RID: 364
		// (get) Token: 0x060007D4 RID: 2004 RVA: 0x0002B6F0 File Offset: 0x000298F0
		// (set) Token: 0x060007D5 RID: 2005 RVA: 0x0002B6F8 File Offset: 0x000298F8
		public override long ContentLength
		{
			get
			{
				return this.m_contentLength;
			}
			set
			{
				if (value < 0L)
				{
					throw new ArgumentException(SR.GetString("net_clsmall"), "value");
				}
				this.m_contentLength = value;
			}
		}

		// Token: 0x1700016D RID: 365
		// (get) Token: 0x060007D6 RID: 2006 RVA: 0x0002B71B File Offset: 0x0002991B
		// (set) Token: 0x060007D7 RID: 2007 RVA: 0x0002B72D File Offset: 0x0002992D
		public override string ContentType
		{
			get
			{
				return this.m_headers["Content-Type"];
			}
			set
			{
				this.m_headers["Content-Type"] = value;
			}
		}

		// Token: 0x1700016E RID: 366
		// (get) Token: 0x060007D8 RID: 2008 RVA: 0x0002B740 File Offset: 0x00029940
		// (set) Token: 0x060007D9 RID: 2009 RVA: 0x0002B748 File Offset: 0x00029948
		public override ICredentials Credentials
		{
			get
			{
				return this.m_credentials;
			}
			set
			{
				this.m_credentials = value;
			}
		}

		// Token: 0x1700016F RID: 367
		// (get) Token: 0x060007DA RID: 2010 RVA: 0x0002B751 File Offset: 0x00029951
		public override WebHeaderCollection Headers
		{
			get
			{
				return this.m_headers;
			}
		}

		// Token: 0x17000170 RID: 368
		// (get) Token: 0x060007DB RID: 2011 RVA: 0x0002B759 File Offset: 0x00029959
		// (set) Token: 0x060007DC RID: 2012 RVA: 0x0002B761 File Offset: 0x00029961
		public override string Method
		{
			get
			{
				return this.m_method;
			}
			set
			{
				if (ValidationHelper.IsBlankString(value))
				{
					throw new ArgumentException(SR.GetString("net_badmethod"), "value");
				}
				this.m_method = value;
			}
		}

		// Token: 0x17000171 RID: 369
		// (get) Token: 0x060007DD RID: 2013 RVA: 0x0002B787 File Offset: 0x00029987
		// (set) Token: 0x060007DE RID: 2014 RVA: 0x0002B78F File Offset: 0x0002998F
		public override bool PreAuthenticate
		{
			get
			{
				return this.m_preauthenticate;
			}
			set
			{
				this.m_preauthenticate = true;
			}
		}

		// Token: 0x17000172 RID: 370
		// (get) Token: 0x060007DF RID: 2015 RVA: 0x0002B798 File Offset: 0x00029998
		// (set) Token: 0x060007E0 RID: 2016 RVA: 0x0002B7A0 File Offset: 0x000299A0
		public override IWebProxy Proxy
		{
			get
			{
				return this.m_proxy;
			}
			set
			{
				this.m_proxy = value;
			}
		}

		// Token: 0x17000173 RID: 371
		// (get) Token: 0x060007E1 RID: 2017 RVA: 0x0002B7A9 File Offset: 0x000299A9
		// (set) Token: 0x060007E2 RID: 2018 RVA: 0x0002B7B1 File Offset: 0x000299B1
		public override int Timeout
		{
			get
			{
				return this.m_timeout;
			}
			set
			{
				if (value < 0 && value != -1)
				{
					throw new ArgumentOutOfRangeException("value", SR.GetString("net_io_timeout_use_ge_zero"));
				}
				this.m_timeout = value;
			}
		}

		// Token: 0x17000174 RID: 372
		// (get) Token: 0x060007E3 RID: 2019 RVA: 0x0002B7D7 File Offset: 0x000299D7
		public override Uri RequestUri
		{
			get
			{
				return this.m_uri;
			}
		}

		// Token: 0x060007E4 RID: 2020 RVA: 0x0002B7E0 File Offset: 0x000299E0
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public override IAsyncResult BeginGetRequestStream(AsyncCallback callback, object state)
		{
			bool flag = true;
			try
			{
				if (this.Aborted)
				{
					throw ExceptionHelper.RequestAbortedException;
				}
				if (!this.CanGetRequestStream())
				{
					Exception ex = new ProtocolViolationException(SR.GetString("net_nouploadonget"));
					throw ex;
				}
				if (this.m_response != null)
				{
					Exception ex2 = new InvalidOperationException(SR.GetString("net_reqsubmitted"));
					throw ex2;
				}
				lock (this)
				{
					if (this.m_writePending)
					{
						Exception ex3 = new InvalidOperationException(SR.GetString("net_repcall"));
						throw ex3;
					}
					this.m_writePending = true;
				}
				this.m_ReadAResult = new LazyAsyncResult(this, state, callback);
				ThreadPool.QueueUserWorkItem(FileWebRequest.s_GetRequestStreamCallback, this.m_ReadAResult);
			}
			catch (Exception ex4)
			{
				flag = false;
				if (Logging.On)
				{
					Logging.Exception(Logging.Web, this, "BeginGetRequestStream", ex4);
				}
				throw;
			}
			finally
			{
				if (FrameworkEventSource.Log.IsEnabled())
				{
					base.LogBeginGetRequestStream(flag, false);
				}
			}
			return this.m_ReadAResult;
		}

		// Token: 0x060007E5 RID: 2021 RVA: 0x0002B8F4 File Offset: 0x00029AF4
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public override IAsyncResult BeginGetResponse(AsyncCallback callback, object state)
		{
			bool flag = true;
			try
			{
				if (this.Aborted)
				{
					throw ExceptionHelper.RequestAbortedException;
				}
				lock (this)
				{
					if (this.m_readPending)
					{
						Exception ex = new InvalidOperationException(SR.GetString("net_repcall"));
						throw ex;
					}
					this.m_readPending = true;
				}
				this.m_WriteAResult = new LazyAsyncResult(this, state, callback);
				ThreadPool.QueueUserWorkItem(FileWebRequest.s_GetResponseCallback, this.m_WriteAResult);
			}
			catch (Exception ex2)
			{
				flag = false;
				if (Logging.On)
				{
					Logging.Exception(Logging.Web, this, "BeginGetResponse", ex2);
				}
				throw;
			}
			finally
			{
				if (FrameworkEventSource.Log.IsEnabled())
				{
					base.LogBeginGetResponse(flag, false);
				}
			}
			return this.m_WriteAResult;
		}

		// Token: 0x060007E6 RID: 2022 RVA: 0x0002B9D0 File Offset: 0x00029BD0
		private bool CanGetRequestStream()
		{
			return !KnownHttpVerb.Parse(this.m_method).ContentBodyNotAllowed;
		}

		// Token: 0x060007E7 RID: 2023 RVA: 0x0002B9E8 File Offset: 0x00029BE8
		public override Stream EndGetRequestStream(IAsyncResult asyncResult)
		{
			bool flag = false;
			Stream stream;
			try
			{
				LazyAsyncResult lazyAsyncResult = asyncResult as LazyAsyncResult;
				if (asyncResult == null || lazyAsyncResult == null)
				{
					Exception ex = ((asyncResult == null) ? new ArgumentNullException("asyncResult") : new ArgumentException(SR.GetString("InvalidAsyncResult"), "asyncResult"));
					throw ex;
				}
				object obj = lazyAsyncResult.InternalWaitForCompletion();
				if (obj is Exception)
				{
					throw (Exception)obj;
				}
				stream = (Stream)obj;
				this.m_writePending = false;
				flag = true;
			}
			catch (Exception ex2)
			{
				if (Logging.On)
				{
					Logging.Exception(Logging.Web, this, "EndGetRequestStream", ex2);
				}
				throw;
			}
			finally
			{
				if (FrameworkEventSource.Log.IsEnabled())
				{
					base.LogEndGetRequestStream(flag, false);
				}
			}
			return stream;
		}

		// Token: 0x060007E8 RID: 2024 RVA: 0x0002BAA4 File Offset: 0x00029CA4
		public override WebResponse EndGetResponse(IAsyncResult asyncResult)
		{
			bool flag = false;
			WebResponse webResponse;
			try
			{
				LazyAsyncResult lazyAsyncResult = asyncResult as LazyAsyncResult;
				if (asyncResult == null || lazyAsyncResult == null)
				{
					Exception ex = ((asyncResult == null) ? new ArgumentNullException("asyncResult") : new ArgumentException(SR.GetString("InvalidAsyncResult"), "asyncResult"));
					throw ex;
				}
				object obj = lazyAsyncResult.InternalWaitForCompletion();
				if (obj is Exception)
				{
					throw (Exception)obj;
				}
				webResponse = (WebResponse)obj;
				this.m_readPending = false;
				flag = true;
			}
			catch (Exception ex2)
			{
				if (Logging.On)
				{
					Logging.Exception(Logging.Web, this, "EndGetResponse", ex2);
				}
				throw;
			}
			finally
			{
				if (FrameworkEventSource.Log.IsEnabled())
				{
					base.LogEndGetResponse(flag, false, 0);
				}
			}
			return webResponse;
		}

		// Token: 0x060007E9 RID: 2025 RVA: 0x0002BB60 File Offset: 0x00029D60
		public override Stream GetRequestStream()
		{
			IAsyncResult asyncResult;
			try
			{
				asyncResult = this.BeginGetRequestStream(null, null);
				if (this.Timeout != -1 && !asyncResult.IsCompleted && (!asyncResult.AsyncWaitHandle.WaitOne(this.Timeout, false) || !asyncResult.IsCompleted))
				{
					if (this.m_stream != null)
					{
						this.m_stream.Close();
					}
					Exception ex = new WebException(NetRes.GetWebStatusString(WebExceptionStatus.Timeout), WebExceptionStatus.Timeout);
					throw ex;
				}
			}
			catch (Exception ex2)
			{
				if (Logging.On)
				{
					Logging.Exception(Logging.Web, this, "GetRequestStream", ex2);
				}
				throw;
			}
			finally
			{
			}
			return this.EndGetRequestStream(asyncResult);
		}

		// Token: 0x060007EA RID: 2026 RVA: 0x0002BC08 File Offset: 0x00029E08
		public override WebResponse GetResponse()
		{
			this.m_syncHint = true;
			IAsyncResult asyncResult;
			try
			{
				asyncResult = this.BeginGetResponse(null, null);
				if (this.Timeout != -1 && !asyncResult.IsCompleted && (!asyncResult.AsyncWaitHandle.WaitOne(this.Timeout, false) || !asyncResult.IsCompleted))
				{
					if (this.m_response != null)
					{
						this.m_response.Close();
					}
					Exception ex = new WebException(NetRes.GetWebStatusString(WebExceptionStatus.Timeout), WebExceptionStatus.Timeout);
					throw ex;
				}
			}
			catch (Exception ex2)
			{
				if (Logging.On)
				{
					Logging.Exception(Logging.Web, this, "GetResponse", ex2);
				}
				throw;
			}
			finally
			{
			}
			return this.EndGetResponse(asyncResult);
		}

		// Token: 0x060007EB RID: 2027 RVA: 0x0002BCB8 File Offset: 0x00029EB8
		private static void GetRequestStreamCallback(object state)
		{
			LazyAsyncResult lazyAsyncResult = (LazyAsyncResult)state;
			FileWebRequest fileWebRequest = (FileWebRequest)lazyAsyncResult.AsyncObject;
			try
			{
				if (fileWebRequest.m_stream == null)
				{
					fileWebRequest.m_stream = new FileWebStream(fileWebRequest, fileWebRequest.m_uri.LocalPath, FileMode.Create, FileAccess.Write, FileShare.Read);
					fileWebRequest.m_fileAccess = FileAccess.Write;
					fileWebRequest.m_writing = true;
				}
			}
			catch (Exception ex)
			{
				Exception ex2 = new WebException(ex.Message, ex);
				lazyAsyncResult.InvokeCallback(ex2);
				return;
			}
			lazyAsyncResult.InvokeCallback(fileWebRequest.m_stream);
		}

		// Token: 0x060007EC RID: 2028 RVA: 0x0002BD40 File Offset: 0x00029F40
		private static void GetResponseCallback(object state)
		{
			LazyAsyncResult lazyAsyncResult = (LazyAsyncResult)state;
			FileWebRequest fileWebRequest = (FileWebRequest)lazyAsyncResult.AsyncObject;
			if (fileWebRequest.m_writePending || fileWebRequest.m_writing)
			{
				FileWebRequest fileWebRequest2 = fileWebRequest;
				lock (fileWebRequest2)
				{
					if (fileWebRequest.m_writePending || fileWebRequest.m_writing)
					{
						fileWebRequest.m_readerEvent = new ManualResetEvent(false);
					}
				}
			}
			if (fileWebRequest.m_readerEvent != null)
			{
				fileWebRequest.m_readerEvent.WaitOne();
			}
			try
			{
				if (fileWebRequest.m_response == null)
				{
					fileWebRequest.m_response = new FileWebResponse(fileWebRequest, fileWebRequest.m_uri, fileWebRequest.m_fileAccess, !fileWebRequest.m_syncHint);
				}
			}
			catch (Exception ex)
			{
				Exception ex2 = new WebException(ex.Message, ex);
				lazyAsyncResult.InvokeCallback(ex2);
				return;
			}
			lazyAsyncResult.InvokeCallback(fileWebRequest.m_response);
		}

		// Token: 0x060007ED RID: 2029 RVA: 0x0002BE28 File Offset: 0x0002A028
		internal void UnblockReader()
		{
			lock (this)
			{
				if (this.m_readerEvent != null)
				{
					this.m_readerEvent.Set();
				}
			}
			this.m_writing = false;
		}

		// Token: 0x17000175 RID: 373
		// (get) Token: 0x060007EE RID: 2030 RVA: 0x0002BE78 File Offset: 0x0002A078
		// (set) Token: 0x060007EF RID: 2031 RVA: 0x0002BE7F File Offset: 0x0002A07F
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

		// Token: 0x060007F0 RID: 2032 RVA: 0x0002BE88 File Offset: 0x0002A088
		public override void Abort()
		{
			if (Logging.On)
			{
				Logging.PrintWarning(Logging.Web, NetRes.GetWebStatusString("net_requestaborted", WebExceptionStatus.RequestCanceled));
			}
			try
			{
				if (Interlocked.Increment(ref this.m_Aborted) == 1)
				{
					LazyAsyncResult readAResult = this.m_ReadAResult;
					LazyAsyncResult writeAResult = this.m_WriteAResult;
					WebException ex = new WebException(NetRes.GetWebStatusString("net_requestaborted", WebExceptionStatus.RequestCanceled), WebExceptionStatus.RequestCanceled);
					Stream stream = this.m_stream;
					if (readAResult != null && !readAResult.IsCompleted)
					{
						readAResult.InvokeCallback(ex);
					}
					if (writeAResult != null && !writeAResult.IsCompleted)
					{
						writeAResult.InvokeCallback(ex);
					}
					if (stream != null)
					{
						if (stream is ICloseEx)
						{
							((ICloseEx)stream).CloseEx(CloseExState.Abort);
						}
						else
						{
							stream.Close();
						}
					}
					if (this.m_response != null)
					{
						((ICloseEx)this.m_response).CloseEx(CloseExState.Abort);
					}
				}
			}
			catch (Exception ex2)
			{
				if (Logging.On)
				{
					Logging.Exception(Logging.Web, this, "Abort", ex2);
				}
				throw;
			}
			finally
			{
			}
		}

		// Token: 0x04000D3A RID: 3386
		private static WaitCallback s_GetRequestStreamCallback = new WaitCallback(FileWebRequest.GetRequestStreamCallback);

		// Token: 0x04000D3B RID: 3387
		private static WaitCallback s_GetResponseCallback = new WaitCallback(FileWebRequest.GetResponseCallback);

		// Token: 0x04000D3C RID: 3388
		private static ContextCallback s_WrappedGetRequestStreamCallback = new ContextCallback(FileWebRequest.GetRequestStreamCallback);

		// Token: 0x04000D3D RID: 3389
		private static ContextCallback s_WrappedResponseCallback = new ContextCallback(FileWebRequest.GetResponseCallback);

		// Token: 0x04000D3E RID: 3390
		private string m_connectionGroupName;

		// Token: 0x04000D3F RID: 3391
		private long m_contentLength;

		// Token: 0x04000D40 RID: 3392
		private ICredentials m_credentials;

		// Token: 0x04000D41 RID: 3393
		private FileAccess m_fileAccess;

		// Token: 0x04000D42 RID: 3394
		private WebHeaderCollection m_headers;

		// Token: 0x04000D43 RID: 3395
		private string m_method = "GET";

		// Token: 0x04000D44 RID: 3396
		private bool m_preauthenticate;

		// Token: 0x04000D45 RID: 3397
		private IWebProxy m_proxy;

		// Token: 0x04000D46 RID: 3398
		private ManualResetEvent m_readerEvent;

		// Token: 0x04000D47 RID: 3399
		private bool m_readPending;

		// Token: 0x04000D48 RID: 3400
		private WebResponse m_response;

		// Token: 0x04000D49 RID: 3401
		private Stream m_stream;

		// Token: 0x04000D4A RID: 3402
		private bool m_syncHint;

		// Token: 0x04000D4B RID: 3403
		private int m_timeout = 100000;

		// Token: 0x04000D4C RID: 3404
		private Uri m_uri;

		// Token: 0x04000D4D RID: 3405
		private bool m_writePending;

		// Token: 0x04000D4E RID: 3406
		private bool m_writing;

		// Token: 0x04000D4F RID: 3407
		private LazyAsyncResult m_WriteAResult;

		// Token: 0x04000D50 RID: 3408
		private LazyAsyncResult m_ReadAResult;

		// Token: 0x04000D51 RID: 3409
		private int m_Aborted;
	}
}
