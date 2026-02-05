using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Net.Cache;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace System.Net
{
	// Token: 0x02000167 RID: 359
	[ComVisible(true)]
	public class WebClient : Component
	{
		// Token: 0x06000D03 RID: 3331 RVA: 0x00045AAD File Offset: 0x00043CAD
		public WebClient()
		{
			if (base.GetType() == typeof(WebClient))
			{
				GC.SuppressFinalize(this);
			}
		}

		// Token: 0x06000D04 RID: 3332 RVA: 0x00045AE8 File Offset: 0x00043CE8
		private void InitWebClientAsync()
		{
			if (!this.m_InitWebClientAsync)
			{
				this.openReadOperationCompleted = new SendOrPostCallback(this.OpenReadOperationCompleted);
				this.openWriteOperationCompleted = new SendOrPostCallback(this.OpenWriteOperationCompleted);
				this.downloadStringOperationCompleted = new SendOrPostCallback(this.DownloadStringOperationCompleted);
				this.downloadDataOperationCompleted = new SendOrPostCallback(this.DownloadDataOperationCompleted);
				this.downloadFileOperationCompleted = new SendOrPostCallback(this.DownloadFileOperationCompleted);
				this.uploadStringOperationCompleted = new SendOrPostCallback(this.UploadStringOperationCompleted);
				this.uploadDataOperationCompleted = new SendOrPostCallback(this.UploadDataOperationCompleted);
				this.uploadFileOperationCompleted = new SendOrPostCallback(this.UploadFileOperationCompleted);
				this.uploadValuesOperationCompleted = new SendOrPostCallback(this.UploadValuesOperationCompleted);
				this.reportDownloadProgressChanged = new SendOrPostCallback(this.ReportDownloadProgressChanged);
				this.reportUploadProgressChanged = new SendOrPostCallback(this.ReportUploadProgressChanged);
				this.m_Progress = new WebClient.ProgressData();
				this.m_InitWebClientAsync = true;
			}
		}

		// Token: 0x06000D05 RID: 3333 RVA: 0x00045BD8 File Offset: 0x00043DD8
		private void ClearWebClientState()
		{
			if (this.AnotherCallInProgress(Interlocked.Increment(ref this.m_CallNesting)))
			{
				this.CompleteWebClientState();
				throw new NotSupportedException(SR.GetString("net_webclient_no_concurrent_io_allowed"));
			}
			this.m_ContentLength = -1L;
			this.m_WebResponse = null;
			this.m_WebRequest = null;
			this.m_Method = null;
			this.m_Cancelled = false;
			if (this.m_Progress != null)
			{
				this.m_Progress.Reset();
			}
		}

		// Token: 0x06000D06 RID: 3334 RVA: 0x00045C45 File Offset: 0x00043E45
		private void CompleteWebClientState()
		{
			Interlocked.Decrement(ref this.m_CallNesting);
		}

		// Token: 0x170002FE RID: 766
		// (get) Token: 0x06000D07 RID: 3335 RVA: 0x00045C53 File Offset: 0x00043E53
		// (set) Token: 0x06000D08 RID: 3336 RVA: 0x00045C5B File Offset: 0x00043E5B
		[Obsolete("This API supports the .NET Framework infrastructure and is not intended to be used directly from your code.", true)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool AllowReadStreamBuffering { get; set; }

		// Token: 0x170002FF RID: 767
		// (get) Token: 0x06000D09 RID: 3337 RVA: 0x00045C64 File Offset: 0x00043E64
		// (set) Token: 0x06000D0A RID: 3338 RVA: 0x00045C6C File Offset: 0x00043E6C
		[Obsolete("This API supports the .NET Framework infrastructure and is not intended to be used directly from your code.", true)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool AllowWriteStreamBuffering { get; set; }

		// Token: 0x14000014 RID: 20
		// (add) Token: 0x06000D0B RID: 3339 RVA: 0x00045C75 File Offset: 0x00043E75
		// (remove) Token: 0x06000D0C RID: 3340 RVA: 0x00045C77 File Offset: 0x00043E77
		[Obsolete("This API supports the .NET Framework infrastructure and is not intended to be used directly from your code.", true)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public event WriteStreamClosedEventHandler WriteStreamClosed
		{
			add
			{
			}
			remove
			{
			}
		}

		// Token: 0x06000D0D RID: 3341 RVA: 0x00045C79 File Offset: 0x00043E79
		[Obsolete("This API supports the .NET Framework infrastructure and is not intended to be used directly from your code.", true)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected virtual void OnWriteStreamClosed(WriteStreamClosedEventArgs e)
		{
		}

		// Token: 0x17000300 RID: 768
		// (get) Token: 0x06000D0E RID: 3342 RVA: 0x00045C7B File Offset: 0x00043E7B
		// (set) Token: 0x06000D0F RID: 3343 RVA: 0x00045C83 File Offset: 0x00043E83
		public Encoding Encoding
		{
			get
			{
				return this.m_Encoding;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("Encoding");
				}
				this.m_Encoding = value;
			}
		}

		// Token: 0x17000301 RID: 769
		// (get) Token: 0x06000D10 RID: 3344 RVA: 0x00045C9A File Offset: 0x00043E9A
		// (set) Token: 0x06000D11 RID: 3345 RVA: 0x00045CBC File Offset: 0x00043EBC
		public string BaseAddress
		{
			get
			{
				if (!(this.m_baseAddress == null))
				{
					return this.m_baseAddress.ToString();
				}
				return string.Empty;
			}
			set
			{
				if (value == null || value.Length == 0)
				{
					this.m_baseAddress = null;
					return;
				}
				try
				{
					this.m_baseAddress = new Uri(value);
				}
				catch (UriFormatException ex)
				{
					throw new ArgumentException(SR.GetString("net_webclient_invalid_baseaddress"), "value", ex);
				}
			}
		}

		// Token: 0x17000302 RID: 770
		// (get) Token: 0x06000D12 RID: 3346 RVA: 0x00045D14 File Offset: 0x00043F14
		// (set) Token: 0x06000D13 RID: 3347 RVA: 0x00045D1C File Offset: 0x00043F1C
		public ICredentials Credentials
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

		// Token: 0x17000303 RID: 771
		// (get) Token: 0x06000D14 RID: 3348 RVA: 0x00045D25 File Offset: 0x00043F25
		// (set) Token: 0x06000D15 RID: 3349 RVA: 0x00045D37 File Offset: 0x00043F37
		public bool UseDefaultCredentials
		{
			get
			{
				return this.m_credentials is SystemNetworkCredential;
			}
			set
			{
				this.m_credentials = (value ? CredentialCache.DefaultCredentials : null);
			}
		}

		// Token: 0x17000304 RID: 772
		// (get) Token: 0x06000D16 RID: 3350 RVA: 0x00045D4A File Offset: 0x00043F4A
		// (set) Token: 0x06000D17 RID: 3351 RVA: 0x00045D66 File Offset: 0x00043F66
		public WebHeaderCollection Headers
		{
			get
			{
				if (this.m_headers == null)
				{
					this.m_headers = new WebHeaderCollection(WebHeaderCollectionType.WebRequest);
				}
				return this.m_headers;
			}
			set
			{
				this.m_headers = value;
			}
		}

		// Token: 0x17000305 RID: 773
		// (get) Token: 0x06000D18 RID: 3352 RVA: 0x00045D6F File Offset: 0x00043F6F
		// (set) Token: 0x06000D19 RID: 3353 RVA: 0x00045D8A File Offset: 0x00043F8A
		public NameValueCollection QueryString
		{
			get
			{
				if (this.m_requestParameters == null)
				{
					this.m_requestParameters = new NameValueCollection();
				}
				return this.m_requestParameters;
			}
			set
			{
				this.m_requestParameters = value;
			}
		}

		// Token: 0x17000306 RID: 774
		// (get) Token: 0x06000D1A RID: 3354 RVA: 0x00045D93 File Offset: 0x00043F93
		public WebHeaderCollection ResponseHeaders
		{
			get
			{
				if (this.m_WebResponse != null)
				{
					return this.m_WebResponse.Headers;
				}
				return null;
			}
		}

		// Token: 0x17000307 RID: 775
		// (get) Token: 0x06000D1B RID: 3355 RVA: 0x00045DAA File Offset: 0x00043FAA
		// (set) Token: 0x06000D1C RID: 3356 RVA: 0x00045DCA File Offset: 0x00043FCA
		public IWebProxy Proxy
		{
			get
			{
				ExceptionHelper.WebPermissionUnrestricted.Demand();
				if (!this.m_ProxySet)
				{
					return WebRequest.InternalDefaultWebProxy;
				}
				return this.m_Proxy;
			}
			set
			{
				ExceptionHelper.WebPermissionUnrestricted.Demand();
				this.m_Proxy = value;
				this.m_ProxySet = true;
			}
		}

		// Token: 0x17000308 RID: 776
		// (get) Token: 0x06000D1D RID: 3357 RVA: 0x00045DE4 File Offset: 0x00043FE4
		// (set) Token: 0x06000D1E RID: 3358 RVA: 0x00045DEC File Offset: 0x00043FEC
		public RequestCachePolicy CachePolicy
		{
			get
			{
				return this.m_CachePolicy;
			}
			set
			{
				this.m_CachePolicy = value;
			}
		}

		// Token: 0x17000309 RID: 777
		// (get) Token: 0x06000D1F RID: 3359 RVA: 0x00045DF5 File Offset: 0x00043FF5
		public bool IsBusy
		{
			get
			{
				return this.m_AsyncOp != null;
			}
		}

		// Token: 0x06000D20 RID: 3360 RVA: 0x00045E00 File Offset: 0x00044000
		protected virtual WebRequest GetWebRequest(Uri address)
		{
			WebRequest webRequest = WebRequest.Create(address);
			this.CopyHeadersTo(webRequest);
			if (this.Credentials != null)
			{
				webRequest.Credentials = this.Credentials;
			}
			if (this.m_Method != null)
			{
				webRequest.Method = this.m_Method;
			}
			if (this.m_ContentLength != -1L)
			{
				webRequest.ContentLength = this.m_ContentLength;
			}
			if (this.m_ProxySet)
			{
				webRequest.Proxy = this.m_Proxy;
			}
			if (this.m_CachePolicy != null)
			{
				webRequest.CachePolicy = this.m_CachePolicy;
			}
			return webRequest;
		}

		// Token: 0x06000D21 RID: 3361 RVA: 0x00045E84 File Offset: 0x00044084
		protected virtual WebResponse GetWebResponse(WebRequest request)
		{
			WebResponse response = request.GetResponse();
			this.m_WebResponse = response;
			return response;
		}

		// Token: 0x06000D22 RID: 3362 RVA: 0x00045EA0 File Offset: 0x000440A0
		protected virtual WebResponse GetWebResponse(WebRequest request, IAsyncResult result)
		{
			WebResponse webResponse = request.EndGetResponse(result);
			this.m_WebResponse = webResponse;
			return webResponse;
		}

		// Token: 0x06000D23 RID: 3363 RVA: 0x00045EBD File Offset: 0x000440BD
		public byte[] DownloadData(string address)
		{
			if (address == null)
			{
				throw new ArgumentNullException("address");
			}
			return this.DownloadData(this.GetUri(address));
		}

		// Token: 0x06000D24 RID: 3364 RVA: 0x00045EDC File Offset: 0x000440DC
		public byte[] DownloadData(Uri address)
		{
			if (Logging.On)
			{
				Logging.Enter(Logging.Web, this, "DownloadData", address);
			}
			if (address == null)
			{
				throw new ArgumentNullException("address");
			}
			this.ClearWebClientState();
			byte[] array2;
			try
			{
				WebRequest webRequest;
				byte[] array = this.DownloadDataInternal(address, out webRequest);
				if (Logging.On)
				{
					Logging.Exit(Logging.Web, this, "DownloadData", array);
				}
				array2 = array;
			}
			finally
			{
				this.CompleteWebClientState();
			}
			return array2;
		}

		// Token: 0x06000D25 RID: 3365 RVA: 0x00045F5C File Offset: 0x0004415C
		private byte[] DownloadDataInternal(Uri address, out WebRequest request)
		{
			if (Logging.On)
			{
				Logging.Enter(Logging.Web, this, "DownloadData", address);
			}
			request = null;
			byte[] array2;
			try
			{
				request = (this.m_WebRequest = this.GetWebRequest(this.GetUri(address)));
				byte[] array = this.DownloadBits(request, null, null, null);
				array2 = array;
			}
			catch (Exception ex)
			{
				if (ex is ThreadAbortException || ex is StackOverflowException || ex is OutOfMemoryException)
				{
					throw;
				}
				if (!(ex is WebException) && !(ex is SecurityException))
				{
					ex = new WebException(SR.GetString("net_webclient"), ex);
				}
				WebClient.AbortRequest(request);
				throw ex;
			}
			return array2;
		}

		// Token: 0x06000D26 RID: 3366 RVA: 0x00046004 File Offset: 0x00044204
		public void DownloadFile(string address, string fileName)
		{
			if (address == null)
			{
				throw new ArgumentNullException("address");
			}
			this.DownloadFile(this.GetUri(address), fileName);
		}

		// Token: 0x06000D27 RID: 3367 RVA: 0x00046024 File Offset: 0x00044224
		public void DownloadFile(Uri address, string fileName)
		{
			if (Logging.On)
			{
				Logging.Enter(Logging.Web, this, "DownloadFile", ((address != null) ? address.ToString() : null) + ", " + fileName);
			}
			if (address == null)
			{
				throw new ArgumentNullException("address");
			}
			if (fileName == null)
			{
				throw new ArgumentNullException("fileName");
			}
			WebRequest webRequest = null;
			FileStream fileStream = null;
			bool flag = false;
			this.ClearWebClientState();
			try
			{
				fileStream = new FileStream(fileName, FileMode.Create, FileAccess.Write);
				webRequest = (this.m_WebRequest = this.GetWebRequest(this.GetUri(address)));
				this.DownloadBits(webRequest, fileStream, null, null);
				flag = true;
			}
			catch (Exception ex)
			{
				if (ex is ThreadAbortException || ex is StackOverflowException || ex is OutOfMemoryException)
				{
					throw;
				}
				if (!(ex is WebException) && !(ex is SecurityException))
				{
					ex = new WebException(SR.GetString("net_webclient"), ex);
				}
				WebClient.AbortRequest(webRequest);
				throw ex;
			}
			finally
			{
				if (fileStream != null)
				{
					fileStream.Close();
					if (!flag)
					{
						File.Delete(fileName);
					}
					fileStream = null;
				}
				this.CompleteWebClientState();
			}
			if (Logging.On)
			{
				Logging.Exit(Logging.Web, this, "DownloadFile", "");
			}
		}

		// Token: 0x06000D28 RID: 3368 RVA: 0x00046160 File Offset: 0x00044360
		public Stream OpenRead(string address)
		{
			if (address == null)
			{
				throw new ArgumentNullException("address");
			}
			return this.OpenRead(this.GetUri(address));
		}

		// Token: 0x06000D29 RID: 3369 RVA: 0x00046180 File Offset: 0x00044380
		public Stream OpenRead(Uri address)
		{
			if (Logging.On)
			{
				Logging.Enter(Logging.Web, this, "OpenRead", address);
			}
			if (address == null)
			{
				throw new ArgumentNullException("address");
			}
			WebRequest webRequest = null;
			this.ClearWebClientState();
			Stream stream;
			try
			{
				webRequest = (this.m_WebRequest = this.GetWebRequest(this.GetUri(address)));
				WebResponse webResponse = (this.m_WebResponse = this.GetWebResponse(webRequest));
				Stream responseStream = webResponse.GetResponseStream();
				if (Logging.On)
				{
					Logging.Exit(Logging.Web, this, "OpenRead", responseStream);
				}
				stream = responseStream;
			}
			catch (Exception ex)
			{
				if (ex is ThreadAbortException || ex is StackOverflowException || ex is OutOfMemoryException)
				{
					throw;
				}
				if (!(ex is WebException) && !(ex is SecurityException))
				{
					ex = new WebException(SR.GetString("net_webclient"), ex);
				}
				WebClient.AbortRequest(webRequest);
				throw ex;
			}
			finally
			{
				this.CompleteWebClientState();
			}
			return stream;
		}

		// Token: 0x06000D2A RID: 3370 RVA: 0x00046284 File Offset: 0x00044484
		public Stream OpenWrite(string address)
		{
			if (address == null)
			{
				throw new ArgumentNullException("address");
			}
			return this.OpenWrite(this.GetUri(address), null);
		}

		// Token: 0x06000D2B RID: 3371 RVA: 0x000462A2 File Offset: 0x000444A2
		public Stream OpenWrite(Uri address)
		{
			return this.OpenWrite(address, null);
		}

		// Token: 0x06000D2C RID: 3372 RVA: 0x000462AC File Offset: 0x000444AC
		public Stream OpenWrite(string address, string method)
		{
			if (address == null)
			{
				throw new ArgumentNullException("address");
			}
			return this.OpenWrite(this.GetUri(address), method);
		}

		// Token: 0x06000D2D RID: 3373 RVA: 0x000462CC File Offset: 0x000444CC
		public Stream OpenWrite(Uri address, string method)
		{
			if (Logging.On)
			{
				Logging.Enter(Logging.Web, this, "OpenWrite", ((address != null) ? address.ToString() : null) + ", " + method);
			}
			if (address == null)
			{
				throw new ArgumentNullException("address");
			}
			if (method == null)
			{
				method = this.MapToDefaultMethod(address);
			}
			WebRequest webRequest = null;
			this.ClearWebClientState();
			Stream stream;
			try
			{
				this.m_Method = method;
				webRequest = (this.m_WebRequest = this.GetWebRequest(this.GetUri(address)));
				WebClient.WebClientWriteStream webClientWriteStream = new WebClient.WebClientWriteStream(webRequest.GetRequestStream(), webRequest, this);
				if (Logging.On)
				{
					Logging.Exit(Logging.Web, this, "OpenWrite", webClientWriteStream);
				}
				stream = webClientWriteStream;
			}
			catch (Exception ex)
			{
				if (ex is ThreadAbortException || ex is StackOverflowException || ex is OutOfMemoryException)
				{
					throw;
				}
				if (!(ex is WebException) && !(ex is SecurityException))
				{
					ex = new WebException(SR.GetString("net_webclient"), ex);
				}
				WebClient.AbortRequest(webRequest);
				throw ex;
			}
			finally
			{
				this.CompleteWebClientState();
			}
			return stream;
		}

		// Token: 0x06000D2E RID: 3374 RVA: 0x000463EC File Offset: 0x000445EC
		public byte[] UploadData(string address, byte[] data)
		{
			if (address == null)
			{
				throw new ArgumentNullException("address");
			}
			return this.UploadData(this.GetUri(address), null, data);
		}

		// Token: 0x06000D2F RID: 3375 RVA: 0x0004640B File Offset: 0x0004460B
		public byte[] UploadData(Uri address, byte[] data)
		{
			return this.UploadData(address, null, data);
		}

		// Token: 0x06000D30 RID: 3376 RVA: 0x00046416 File Offset: 0x00044616
		public byte[] UploadData(string address, string method, byte[] data)
		{
			if (address == null)
			{
				throw new ArgumentNullException("address");
			}
			return this.UploadData(this.GetUri(address), method, data);
		}

		// Token: 0x06000D31 RID: 3377 RVA: 0x00046438 File Offset: 0x00044638
		public byte[] UploadData(Uri address, string method, byte[] data)
		{
			if (Logging.On)
			{
				Logging.Enter(Logging.Web, this, "UploadData", ((address != null) ? address.ToString() : null) + ", " + method);
			}
			if (address == null)
			{
				throw new ArgumentNullException("address");
			}
			if (data == null)
			{
				throw new ArgumentNullException("data");
			}
			if (method == null)
			{
				method = this.MapToDefaultMethod(address);
			}
			this.ClearWebClientState();
			byte[] array2;
			try
			{
				WebRequest webRequest;
				byte[] array = this.UploadDataInternal(address, method, data, out webRequest);
				if (Logging.On)
				{
					Logging.Exit(Logging.Web, this, "UploadData", array);
				}
				array2 = array;
			}
			finally
			{
				this.CompleteWebClientState();
			}
			return array2;
		}

		// Token: 0x06000D32 RID: 3378 RVA: 0x000464E8 File Offset: 0x000446E8
		private byte[] UploadDataInternal(Uri address, string method, byte[] data, out WebRequest request)
		{
			request = null;
			byte[] array2;
			try
			{
				this.m_Method = method;
				this.m_ContentLength = (long)data.Length;
				request = (this.m_WebRequest = this.GetWebRequest(this.GetUri(address)));
				this.UploadBits(request, null, data, 0, null, null, null, null, null);
				byte[] array = this.DownloadBits(request, null, null, null);
				array2 = array;
			}
			catch (Exception ex)
			{
				if (ex is ThreadAbortException || ex is StackOverflowException || ex is OutOfMemoryException)
				{
					throw;
				}
				if (!(ex is WebException) && !(ex is SecurityException))
				{
					ex = new WebException(SR.GetString("net_webclient"), ex);
				}
				WebClient.AbortRequest(request);
				throw ex;
			}
			return array2;
		}

		// Token: 0x06000D33 RID: 3379 RVA: 0x0004659C File Offset: 0x0004479C
		private void OpenFileInternal(bool needsHeaderAndBoundary, string fileName, ref FileStream fs, ref byte[] buffer, ref byte[] formHeaderBytes, ref byte[] boundaryBytes)
		{
			fileName = Path.GetFullPath(fileName);
			if (this.m_headers == null)
			{
				this.m_headers = new WebHeaderCollection(WebHeaderCollectionType.WebRequest);
			}
			string text = this.m_headers["Content-Type"];
			if (text != null)
			{
				if (text.ToLower(CultureInfo.InvariantCulture).StartsWith("multipart/"))
				{
					throw new WebException(SR.GetString("net_webclient_Multipart"));
				}
			}
			else
			{
				text = "application/octet-stream";
			}
			fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
			int num = 8192;
			this.m_ContentLength = -1L;
			if (this.m_Method.ToUpper(CultureInfo.InvariantCulture) == "POST")
			{
				if (needsHeaderAndBoundary)
				{
					string text2 = "---------------------" + DateTime.Now.Ticks.ToString("x", NumberFormatInfo.InvariantInfo);
					this.m_headers["Content-Type"] = "multipart/form-data; boundary=" + text2;
					string text3 = string.Concat(new string[]
					{
						"--",
						text2,
						"\r\nContent-Disposition: form-data; name=\"file\"; filename=\"",
						Path.GetFileName(fileName),
						"\"\r\nContent-Type: ",
						text,
						"\r\n\r\n"
					});
					formHeaderBytes = Encoding.UTF8.GetBytes(text3);
					boundaryBytes = Encoding.ASCII.GetBytes("\r\n--" + text2 + "--\r\n");
				}
				else
				{
					formHeaderBytes = new byte[0];
					boundaryBytes = new byte[0];
				}
				if (fs.CanSeek)
				{
					this.m_ContentLength = fs.Length + (long)formHeaderBytes.Length + (long)boundaryBytes.Length;
					num = (int)Math.Min(8192L, fs.Length);
				}
			}
			else
			{
				this.m_headers["Content-Type"] = text;
				formHeaderBytes = null;
				boundaryBytes = null;
				if (fs.CanSeek)
				{
					this.m_ContentLength = fs.Length;
					num = (int)Math.Min(8192L, fs.Length);
				}
			}
			buffer = new byte[num];
		}

		// Token: 0x06000D34 RID: 3380 RVA: 0x0004678C File Offset: 0x0004498C
		public byte[] UploadFile(string address, string fileName)
		{
			if (address == null)
			{
				throw new ArgumentNullException("address");
			}
			return this.UploadFile(this.GetUri(address), fileName);
		}

		// Token: 0x06000D35 RID: 3381 RVA: 0x000467AA File Offset: 0x000449AA
		public byte[] UploadFile(Uri address, string fileName)
		{
			return this.UploadFile(address, null, fileName);
		}

		// Token: 0x06000D36 RID: 3382 RVA: 0x000467B5 File Offset: 0x000449B5
		public byte[] UploadFile(string address, string method, string fileName)
		{
			return this.UploadFile(this.GetUri(address), method, fileName);
		}

		// Token: 0x06000D37 RID: 3383 RVA: 0x000467C8 File Offset: 0x000449C8
		public byte[] UploadFile(Uri address, string method, string fileName)
		{
			if (Logging.On)
			{
				Logging.Enter(Logging.Web, this, "UploadFile", ((address != null) ? address.ToString() : null) + ", " + method);
			}
			if (address == null)
			{
				throw new ArgumentNullException("address");
			}
			if (fileName == null)
			{
				throw new ArgumentNullException("fileName");
			}
			if (method == null)
			{
				method = this.MapToDefaultMethod(address);
			}
			FileStream fileStream = null;
			WebRequest webRequest = null;
			this.ClearWebClientState();
			byte[] array5;
			try
			{
				this.m_Method = method;
				byte[] array = null;
				byte[] array2 = null;
				byte[] array3 = null;
				Uri uri = this.GetUri(address);
				bool flag = uri.Scheme != Uri.UriSchemeFile;
				this.OpenFileInternal(flag, fileName, ref fileStream, ref array3, ref array, ref array2);
				webRequest = (this.m_WebRequest = this.GetWebRequest(uri));
				this.UploadBits(webRequest, fileStream, array3, 0, array, array2, null, null, null);
				byte[] array4 = this.DownloadBits(webRequest, null, null, null);
				if (Logging.On)
				{
					Logging.Exit(Logging.Web, this, "UploadFile", array4);
				}
				array5 = array4;
			}
			catch (Exception ex)
			{
				if (fileStream != null)
				{
					fileStream.Close();
					fileStream = null;
				}
				if (ex is ThreadAbortException || ex is StackOverflowException || ex is OutOfMemoryException)
				{
					throw;
				}
				if (!(ex is WebException) && !(ex is SecurityException))
				{
					ex = new WebException(SR.GetString("net_webclient"), ex);
				}
				WebClient.AbortRequest(webRequest);
				throw ex;
			}
			finally
			{
				this.CompleteWebClientState();
			}
			return array5;
		}

		// Token: 0x06000D38 RID: 3384 RVA: 0x00046944 File Offset: 0x00044B44
		private byte[] UploadValuesInternal(NameValueCollection data)
		{
			if (this.m_headers == null)
			{
				this.m_headers = new WebHeaderCollection(WebHeaderCollectionType.WebRequest);
			}
			string text = this.m_headers["Content-Type"];
			if (text != null && string.Compare(text, "application/x-www-form-urlencoded", StringComparison.OrdinalIgnoreCase) != 0)
			{
				throw new WebException(SR.GetString("net_webclient_ContentType"));
			}
			this.m_headers["Content-Type"] = "application/x-www-form-urlencoded";
			string text2 = string.Empty;
			StringBuilder stringBuilder = new StringBuilder();
			foreach (string text3 in data.AllKeys)
			{
				stringBuilder.Append(text2);
				stringBuilder.Append(WebClient.UrlEncode(text3));
				stringBuilder.Append("=");
				stringBuilder.Append(WebClient.UrlEncode(data[text3]));
				text2 = "&";
			}
			byte[] bytes = Encoding.ASCII.GetBytes(stringBuilder.ToString());
			this.m_ContentLength = (long)bytes.Length;
			return bytes;
		}

		// Token: 0x06000D39 RID: 3385 RVA: 0x00046A32 File Offset: 0x00044C32
		public byte[] UploadValues(string address, NameValueCollection data)
		{
			if (address == null)
			{
				throw new ArgumentNullException("address");
			}
			return this.UploadValues(this.GetUri(address), null, data);
		}

		// Token: 0x06000D3A RID: 3386 RVA: 0x00046A51 File Offset: 0x00044C51
		public byte[] UploadValues(Uri address, NameValueCollection data)
		{
			return this.UploadValues(address, null, data);
		}

		// Token: 0x06000D3B RID: 3387 RVA: 0x00046A5C File Offset: 0x00044C5C
		public byte[] UploadValues(string address, string method, NameValueCollection data)
		{
			if (address == null)
			{
				throw new ArgumentNullException("address");
			}
			return this.UploadValues(this.GetUri(address), method, data);
		}

		// Token: 0x06000D3C RID: 3388 RVA: 0x00046A7C File Offset: 0x00044C7C
		public byte[] UploadValues(Uri address, string method, NameValueCollection data)
		{
			if (Logging.On)
			{
				Logging.Enter(Logging.Web, this, "UploadValues", ((address != null) ? address.ToString() : null) + ", " + method);
			}
			if (address == null)
			{
				throw new ArgumentNullException("address");
			}
			if (data == null)
			{
				throw new ArgumentNullException("data");
			}
			if (method == null)
			{
				method = this.MapToDefaultMethod(address);
			}
			WebRequest webRequest = null;
			this.ClearWebClientState();
			byte[] array3;
			try
			{
				byte[] array = this.UploadValuesInternal(data);
				this.m_Method = method;
				webRequest = (this.m_WebRequest = this.GetWebRequest(this.GetUri(address)));
				this.UploadBits(webRequest, null, array, 0, null, null, null, null, null);
				byte[] array2 = this.DownloadBits(webRequest, null, null, null);
				if (Logging.On)
				{
					Logging.Exit(Logging.Web, this, "UploadValues", ((address != null) ? address.ToString() : null) + ", " + method);
				}
				array3 = array2;
			}
			catch (Exception ex)
			{
				if (ex is ThreadAbortException || ex is StackOverflowException || ex is OutOfMemoryException)
				{
					throw;
				}
				if (!(ex is WebException) && !(ex is SecurityException))
				{
					ex = new WebException(SR.GetString("net_webclient"), ex);
				}
				WebClient.AbortRequest(webRequest);
				throw ex;
			}
			finally
			{
				this.CompleteWebClientState();
			}
			return array3;
		}

		// Token: 0x06000D3D RID: 3389 RVA: 0x00046BD4 File Offset: 0x00044DD4
		public string UploadString(string address, string data)
		{
			if (address == null)
			{
				throw new ArgumentNullException("address");
			}
			return this.UploadString(this.GetUri(address), null, data);
		}

		// Token: 0x06000D3E RID: 3390 RVA: 0x00046BF3 File Offset: 0x00044DF3
		public string UploadString(Uri address, string data)
		{
			return this.UploadString(address, null, data);
		}

		// Token: 0x06000D3F RID: 3391 RVA: 0x00046BFE File Offset: 0x00044DFE
		public string UploadString(string address, string method, string data)
		{
			if (address == null)
			{
				throw new ArgumentNullException("address");
			}
			return this.UploadString(this.GetUri(address), method, data);
		}

		// Token: 0x06000D40 RID: 3392 RVA: 0x00046C20 File Offset: 0x00044E20
		public string UploadString(Uri address, string method, string data)
		{
			if (Logging.On)
			{
				Logging.Enter(Logging.Web, this, "UploadString", ((address != null) ? address.ToString() : null) + ", " + method);
			}
			if (address == null)
			{
				throw new ArgumentNullException("address");
			}
			if (data == null)
			{
				throw new ArgumentNullException("data");
			}
			if (method == null)
			{
				method = this.MapToDefaultMethod(address);
			}
			this.ClearWebClientState();
			string text;
			try
			{
				byte[] bytes = this.Encoding.GetBytes(data);
				WebRequest webRequest;
				byte[] array = this.UploadDataInternal(address, method, bytes, out webRequest);
				string stringUsingEncoding = this.GetStringUsingEncoding(webRequest, array);
				if (Logging.On)
				{
					Logging.Exit(Logging.Web, this, "UploadString", stringUsingEncoding);
				}
				text = stringUsingEncoding;
			}
			finally
			{
				this.CompleteWebClientState();
			}
			return text;
		}

		// Token: 0x06000D41 RID: 3393 RVA: 0x00046CE8 File Offset: 0x00044EE8
		public string DownloadString(string address)
		{
			if (address == null)
			{
				throw new ArgumentNullException("address");
			}
			return this.DownloadString(this.GetUri(address));
		}

		// Token: 0x06000D42 RID: 3394 RVA: 0x00046D08 File Offset: 0x00044F08
		public string DownloadString(Uri address)
		{
			if (Logging.On)
			{
				Logging.Enter(Logging.Web, this, "DownloadString", address);
			}
			if (address == null)
			{
				throw new ArgumentNullException("address");
			}
			this.ClearWebClientState();
			string text;
			try
			{
				WebRequest webRequest;
				byte[] array = this.DownloadDataInternal(address, out webRequest);
				string stringUsingEncoding = this.GetStringUsingEncoding(webRequest, array);
				if (Logging.On)
				{
					Logging.Exit(Logging.Web, this, "DownloadString", stringUsingEncoding);
				}
				text = stringUsingEncoding;
			}
			finally
			{
				this.CompleteWebClientState();
			}
			return text;
		}

		// Token: 0x06000D43 RID: 3395 RVA: 0x00046D90 File Offset: 0x00044F90
		private static void AbortRequest(WebRequest request)
		{
			try
			{
				if (request != null)
				{
					request.Abort();
				}
			}
			catch (Exception ex)
			{
				if (ex is OutOfMemoryException || ex is StackOverflowException || ex is ThreadAbortException)
				{
					throw;
				}
			}
		}

		// Token: 0x06000D44 RID: 3396 RVA: 0x00046DD8 File Offset: 0x00044FD8
		private void CopyHeadersTo(WebRequest request)
		{
			if (this.m_headers != null && request is HttpWebRequest)
			{
				string text = this.m_headers["Accept"];
				string text2 = this.m_headers["Connection"];
				string text3 = this.m_headers["Content-Type"];
				string text4 = this.m_headers["Expect"];
				string text5 = this.m_headers["Referer"];
				string text6 = this.m_headers["User-Agent"];
				string text7 = this.m_headers["Host"];
				this.m_headers.RemoveInternal("Accept");
				this.m_headers.RemoveInternal("Connection");
				this.m_headers.RemoveInternal("Content-Type");
				this.m_headers.RemoveInternal("Expect");
				this.m_headers.RemoveInternal("Referer");
				this.m_headers.RemoveInternal("User-Agent");
				this.m_headers.RemoveInternal("Host");
				request.Headers = this.m_headers;
				if (text != null && text.Length > 0)
				{
					((HttpWebRequest)request).Accept = text;
				}
				if (text2 != null && text2.Length > 0)
				{
					((HttpWebRequest)request).Connection = text2;
				}
				if (text3 != null && text3.Length > 0)
				{
					((HttpWebRequest)request).ContentType = text3;
				}
				if (text4 != null && text4.Length > 0)
				{
					((HttpWebRequest)request).Expect = text4;
				}
				if (text5 != null && text5.Length > 0)
				{
					((HttpWebRequest)request).Referer = text5;
				}
				if (text6 != null && text6.Length > 0)
				{
					((HttpWebRequest)request).UserAgent = text6;
				}
				if (!string.IsNullOrEmpty(text7))
				{
					((HttpWebRequest)request).Host = text7;
				}
			}
		}

		// Token: 0x06000D45 RID: 3397 RVA: 0x00046FA0 File Offset: 0x000451A0
		private Uri GetUri(string path)
		{
			Uri uri;
			if (this.m_baseAddress != null)
			{
				if (!Uri.TryCreate(this.m_baseAddress, path, out uri))
				{
					return new Uri(Path.GetFullPath(path));
				}
			}
			else if (!Uri.TryCreate(path, UriKind.Absolute, out uri))
			{
				return new Uri(Path.GetFullPath(path));
			}
			return this.GetUri(uri);
		}

		// Token: 0x06000D46 RID: 3398 RVA: 0x00046FF8 File Offset: 0x000451F8
		private Uri GetUri(Uri address)
		{
			if (address == null)
			{
				throw new ArgumentNullException("address");
			}
			Uri uri = address;
			if (!address.IsAbsoluteUri && this.m_baseAddress != null && !Uri.TryCreate(this.m_baseAddress, address, out uri))
			{
				return address;
			}
			if ((uri.Query == null || uri.Query == string.Empty) && this.m_requestParameters != null)
			{
				StringBuilder stringBuilder = new StringBuilder();
				string text = string.Empty;
				for (int i = 0; i < this.m_requestParameters.Count; i++)
				{
					stringBuilder.Append(text + this.m_requestParameters.AllKeys[i] + "=" + this.m_requestParameters[i]);
					text = "&";
				}
				uri = new UriBuilder(uri)
				{
					Query = stringBuilder.ToString()
				}.Uri;
			}
			return uri;
		}

		// Token: 0x06000D47 RID: 3399 RVA: 0x000470DC File Offset: 0x000452DC
		private static void DownloadBitsResponseCallback(IAsyncResult result)
		{
			WebClient.DownloadBitsState downloadBitsState = (WebClient.DownloadBitsState)result.AsyncState;
			WebRequest request = downloadBitsState.Request;
			Exception ex = null;
			try
			{
				WebResponse webResponse = downloadBitsState.WebClient.GetWebResponse(request, result);
				downloadBitsState.WebClient.m_WebResponse = webResponse;
				downloadBitsState.SetResponse(webResponse);
			}
			catch (Exception ex2)
			{
				if (ex2 is ThreadAbortException || ex2 is StackOverflowException || ex2 is OutOfMemoryException)
				{
					throw;
				}
				ex = ex2;
				if (!(ex2 is WebException) && !(ex2 is SecurityException))
				{
					ex = new WebException(SR.GetString("net_webclient"), ex2);
				}
				WebClient.AbortRequest(request);
				if (downloadBitsState != null && downloadBitsState.WriteStream != null)
				{
					downloadBitsState.WriteStream.Close();
				}
			}
			finally
			{
				if (ex != null)
				{
					downloadBitsState.CompletionDelegate(null, ex, downloadBitsState.AsyncOp);
				}
			}
		}

		// Token: 0x06000D48 RID: 3400 RVA: 0x000471BC File Offset: 0x000453BC
		private static void DownloadBitsReadCallback(IAsyncResult result)
		{
			WebClient.DownloadBitsState downloadBitsState = (WebClient.DownloadBitsState)result.AsyncState;
			WebClient.DownloadBitsReadCallbackState(downloadBitsState, result);
		}

		// Token: 0x06000D49 RID: 3401 RVA: 0x000471DC File Offset: 0x000453DC
		private static void DownloadBitsReadCallbackState(WebClient.DownloadBitsState state, IAsyncResult result)
		{
			Stream readStream = state.ReadStream;
			Exception ex = null;
			bool flag = false;
			try
			{
				int num = 0;
				if (readStream != null && readStream != Stream.Null)
				{
					num = readStream.EndRead(result);
				}
				flag = state.RetrieveBytes(ref num);
			}
			catch (Exception ex2)
			{
				flag = true;
				if (ex2 is ThreadAbortException || ex2 is StackOverflowException || ex2 is OutOfMemoryException)
				{
					throw;
				}
				ex = ex2;
				state.InnerBuffer = null;
				if (!(ex2 is WebException) && !(ex2 is SecurityException))
				{
					ex = new WebException(SR.GetString("net_webclient"), ex2);
				}
				WebClient.AbortRequest(state.Request);
				if (state != null && state.WriteStream != null)
				{
					state.WriteStream.Close();
				}
			}
			finally
			{
				if (flag)
				{
					if (ex == null)
					{
						state.Close();
					}
					state.CompletionDelegate(state.InnerBuffer, ex, state.AsyncOp);
				}
			}
		}

		// Token: 0x06000D4A RID: 3402 RVA: 0x000472CC File Offset: 0x000454CC
		private byte[] DownloadBits(WebRequest request, Stream writeStream, CompletionDelegate completionDelegate, AsyncOperation asyncOp)
		{
			WebClient.DownloadBitsState downloadBitsState = new WebClient.DownloadBitsState(request, writeStream, completionDelegate, asyncOp, this.m_Progress, this);
			if (downloadBitsState.Async)
			{
				request.BeginGetResponse(new AsyncCallback(WebClient.DownloadBitsResponseCallback), downloadBitsState);
				return null;
			}
			WebResponse webResponse = (this.m_WebResponse = this.GetWebResponse(request));
			int num = downloadBitsState.SetResponse(webResponse);
			bool flag;
			do
			{
				flag = downloadBitsState.RetrieveBytes(ref num);
			}
			while (!flag);
			downloadBitsState.Close();
			return downloadBitsState.InnerBuffer;
		}

		// Token: 0x06000D4B RID: 3403 RVA: 0x00047340 File Offset: 0x00045540
		private static void UploadBitsRequestCallback(IAsyncResult result)
		{
			WebClient.UploadBitsState uploadBitsState = (WebClient.UploadBitsState)result.AsyncState;
			WebRequest request = uploadBitsState.Request;
			Exception ex = null;
			try
			{
				Stream stream = request.EndGetRequestStream(result);
				uploadBitsState.SetRequestStream(stream);
			}
			catch (Exception ex2)
			{
				if (ex2 is ThreadAbortException || ex2 is StackOverflowException || ex2 is OutOfMemoryException)
				{
					throw;
				}
				ex = ex2;
				if (!(ex2 is WebException) && !(ex2 is SecurityException))
				{
					ex = new WebException(SR.GetString("net_webclient"), ex2);
				}
				WebClient.AbortRequest(request);
				if (uploadBitsState != null && uploadBitsState.ReadStream != null)
				{
					uploadBitsState.ReadStream.Close();
				}
			}
			finally
			{
				if (ex != null)
				{
					uploadBitsState.UploadCompletionDelegate(null, ex, uploadBitsState);
				}
			}
		}

		// Token: 0x06000D4C RID: 3404 RVA: 0x00047408 File Offset: 0x00045608
		private static void UploadBitsWriteCallback(IAsyncResult result)
		{
			WebClient.UploadBitsState uploadBitsState = (WebClient.UploadBitsState)result.AsyncState;
			Stream writeStream = uploadBitsState.WriteStream;
			Exception ex = null;
			bool flag = false;
			try
			{
				writeStream.EndWrite(result);
				flag = uploadBitsState.WriteBytes();
			}
			catch (Exception ex2)
			{
				flag = true;
				if (ex2 is ThreadAbortException || ex2 is StackOverflowException || ex2 is OutOfMemoryException)
				{
					throw;
				}
				ex = ex2;
				if (!(ex2 is WebException) && !(ex2 is SecurityException))
				{
					ex = new WebException(SR.GetString("net_webclient"), ex2);
				}
				WebClient.AbortRequest(uploadBitsState.Request);
				if (uploadBitsState != null && uploadBitsState.ReadStream != null)
				{
					uploadBitsState.ReadStream.Close();
				}
			}
			finally
			{
				if (flag)
				{
					if (ex == null)
					{
						uploadBitsState.Close();
					}
					uploadBitsState.UploadCompletionDelegate(null, ex, uploadBitsState);
				}
			}
		}

		// Token: 0x06000D4D RID: 3405 RVA: 0x000474E4 File Offset: 0x000456E4
		private void UploadBits(WebRequest request, Stream readStream, byte[] buffer, int chunkSize, byte[] header, byte[] footer, CompletionDelegate uploadCompletionDelegate, CompletionDelegate downloadCompletionDelegate, AsyncOperation asyncOp)
		{
			if (request.RequestUri.Scheme == Uri.UriSchemeFile)
			{
				footer = (header = null);
			}
			WebClient.UploadBitsState uploadBitsState = new WebClient.UploadBitsState(request, readStream, buffer, chunkSize, header, footer, uploadCompletionDelegate, downloadCompletionDelegate, asyncOp, this.m_Progress, this);
			if (uploadBitsState.Async)
			{
				request.BeginGetRequestStream(new AsyncCallback(WebClient.UploadBitsRequestCallback), uploadBitsState);
				return;
			}
			Stream requestStream = request.GetRequestStream();
			uploadBitsState.SetRequestStream(requestStream);
			while (!uploadBitsState.WriteBytes())
			{
			}
			uploadBitsState.Close();
		}

		// Token: 0x06000D4E RID: 3406 RVA: 0x00047564 File Offset: 0x00045764
		private bool ByteArrayHasPrefix(byte[] prefix, byte[] byteArray)
		{
			if (prefix == null || byteArray == null || prefix.Length > byteArray.Length)
			{
				return false;
			}
			for (int i = 0; i < prefix.Length; i++)
			{
				if (prefix[i] != byteArray[i])
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06000D4F RID: 3407 RVA: 0x0004759C File Offset: 0x0004579C
		private string GetStringUsingEncoding(WebRequest request, byte[] data)
		{
			Encoding encoding = null;
			int num = -1;
			string text;
			try
			{
				text = request.ContentType;
			}
			catch (NotImplementedException)
			{
				text = null;
			}
			catch (NotSupportedException)
			{
				text = null;
			}
			if (text != null)
			{
				text = text.ToLower(CultureInfo.InvariantCulture);
				string[] array = text.Split(new char[] { ';', '=', ' ' });
				bool flag = false;
				foreach (string text2 in array)
				{
					if (text2 == "charset")
					{
						flag = true;
					}
					else if (flag)
					{
						try
						{
							encoding = Encoding.GetEncoding(text2);
						}
						catch (ArgumentException)
						{
							break;
						}
					}
				}
			}
			if (encoding == null)
			{
				Encoding[] array3 = new Encoding[]
				{
					Encoding.UTF8,
					Encoding.UTF32,
					Encoding.Unicode,
					Encoding.BigEndianUnicode
				};
				for (int j = 0; j < array3.Length; j++)
				{
					byte[] preamble = array3[j].GetPreamble();
					if (this.ByteArrayHasPrefix(preamble, data))
					{
						encoding = array3[j];
						num = preamble.Length;
						break;
					}
				}
			}
			if (encoding == null)
			{
				encoding = this.Encoding;
			}
			if (num == -1)
			{
				byte[] preamble2 = encoding.GetPreamble();
				if (this.ByteArrayHasPrefix(preamble2, data))
				{
					num = preamble2.Length;
				}
				else
				{
					num = 0;
				}
			}
			return encoding.GetString(data, num, data.Length - num);
		}

		// Token: 0x06000D50 RID: 3408 RVA: 0x000476EC File Offset: 0x000458EC
		private string MapToDefaultMethod(Uri address)
		{
			Uri uri;
			if (!address.IsAbsoluteUri && this.m_baseAddress != null)
			{
				uri = new Uri(this.m_baseAddress, address);
			}
			else
			{
				uri = address;
			}
			if (uri.Scheme.ToLower(CultureInfo.InvariantCulture) == "ftp")
			{
				return "STOR";
			}
			return "POST";
		}

		// Token: 0x06000D51 RID: 3409 RVA: 0x00047747 File Offset: 0x00045947
		private static string UrlEncode(string str)
		{
			if (str == null)
			{
				return null;
			}
			return WebClient.UrlEncode(str, Encoding.UTF8);
		}

		// Token: 0x06000D52 RID: 3410 RVA: 0x00047759 File Offset: 0x00045959
		private static string UrlEncode(string str, Encoding e)
		{
			if (str == null)
			{
				return null;
			}
			return Encoding.ASCII.GetString(WebClient.UrlEncodeToBytes(str, e));
		}

		// Token: 0x06000D53 RID: 3411 RVA: 0x00047774 File Offset: 0x00045974
		private static byte[] UrlEncodeToBytes(string str, Encoding e)
		{
			if (str == null)
			{
				return null;
			}
			byte[] bytes = e.GetBytes(str);
			return WebClient.UrlEncodeBytesToBytesInternal(bytes, 0, bytes.Length, false);
		}

		// Token: 0x06000D54 RID: 3412 RVA: 0x0004779C File Offset: 0x0004599C
		private static byte[] UrlEncodeBytesToBytesInternal(byte[] bytes, int offset, int count, bool alwaysCreateReturnValue)
		{
			int num = 0;
			int num2 = 0;
			for (int i = 0; i < count; i++)
			{
				char c = (char)bytes[offset + i];
				if (c == ' ')
				{
					num++;
				}
				else if (!WebClient.IsSafe(c))
				{
					num2++;
				}
			}
			if (!alwaysCreateReturnValue && num == 0 && num2 == 0)
			{
				return bytes;
			}
			byte[] array = new byte[count + num2 * 2];
			int num3 = 0;
			for (int j = 0; j < count; j++)
			{
				byte b = bytes[offset + j];
				char c2 = (char)b;
				if (WebClient.IsSafe(c2))
				{
					array[num3++] = b;
				}
				else if (c2 == ' ')
				{
					array[num3++] = 43;
				}
				else
				{
					array[num3++] = 37;
					array[num3++] = (byte)WebClient.IntToHex((b >> 4) & 15);
					array[num3++] = (byte)WebClient.IntToHex((int)(b & 15));
				}
			}
			return array;
		}

		// Token: 0x06000D55 RID: 3413 RVA: 0x00047867 File Offset: 0x00045A67
		private static char IntToHex(int n)
		{
			if (n <= 9)
			{
				return (char)(n + 48);
			}
			return (char)(n - 10 + 97);
		}

		// Token: 0x06000D56 RID: 3414 RVA: 0x0004787C File Offset: 0x00045A7C
		private static bool IsSafe(char ch)
		{
			if ((ch >= 'a' && ch <= 'z') || (ch >= 'A' && ch <= 'Z') || (ch >= '0' && ch <= '9'))
			{
				return true;
			}
			if (ch != '!')
			{
				switch (ch)
				{
				case '\'':
				case '(':
				case ')':
				case '*':
				case '-':
				case '.':
					return true;
				case '+':
				case ',':
					break;
				default:
					if (ch == '_')
					{
						return true;
					}
					break;
				}
				return false;
			}
			return true;
		}

		// Token: 0x06000D57 RID: 3415 RVA: 0x000478DF File Offset: 0x00045ADF
		private void InvokeOperationCompleted(AsyncOperation asyncOp, SendOrPostCallback callback, AsyncCompletedEventArgs eventArgs)
		{
			if (Interlocked.CompareExchange<AsyncOperation>(ref this.m_AsyncOp, null, asyncOp) == asyncOp)
			{
				this.CompleteWebClientState();
				asyncOp.PostOperationCompleted(callback, eventArgs);
			}
		}

		// Token: 0x06000D58 RID: 3416 RVA: 0x000478FF File Offset: 0x00045AFF
		private bool AnotherCallInProgress(int callNesting)
		{
			return callNesting > 1;
		}

		// Token: 0x14000015 RID: 21
		// (add) Token: 0x06000D59 RID: 3417 RVA: 0x00047908 File Offset: 0x00045B08
		// (remove) Token: 0x06000D5A RID: 3418 RVA: 0x00047940 File Offset: 0x00045B40
		public event OpenReadCompletedEventHandler OpenReadCompleted;

		// Token: 0x06000D5B RID: 3419 RVA: 0x00047975 File Offset: 0x00045B75
		protected virtual void OnOpenReadCompleted(OpenReadCompletedEventArgs e)
		{
			if (this.OpenReadCompleted != null)
			{
				this.OpenReadCompleted(this, e);
			}
		}

		// Token: 0x06000D5C RID: 3420 RVA: 0x0004798C File Offset: 0x00045B8C
		private void OpenReadOperationCompleted(object arg)
		{
			this.OnOpenReadCompleted((OpenReadCompletedEventArgs)arg);
		}

		// Token: 0x06000D5D RID: 3421 RVA: 0x0004799C File Offset: 0x00045B9C
		private void OpenReadAsyncCallback(IAsyncResult result)
		{
			LazyAsyncResult lazyAsyncResult = (LazyAsyncResult)result;
			AsyncOperation asyncOperation = (AsyncOperation)lazyAsyncResult.AsyncState;
			WebRequest webRequest = (WebRequest)lazyAsyncResult.AsyncObject;
			Stream stream = null;
			Exception ex = null;
			try
			{
				WebResponse webResponse = (this.m_WebResponse = this.GetWebResponse(webRequest, result));
				stream = webResponse.GetResponseStream();
			}
			catch (Exception ex2)
			{
				if (ex2 is ThreadAbortException || ex2 is StackOverflowException || ex2 is OutOfMemoryException)
				{
					throw;
				}
				ex = ex2;
				if (!(ex2 is WebException) && !(ex2 is SecurityException))
				{
					ex = new WebException(SR.GetString("net_webclient"), ex2);
				}
			}
			OpenReadCompletedEventArgs openReadCompletedEventArgs = new OpenReadCompletedEventArgs(stream, ex, this.m_Cancelled, asyncOperation.UserSuppliedState);
			this.InvokeOperationCompleted(asyncOperation, this.openReadOperationCompleted, openReadCompletedEventArgs);
		}

		// Token: 0x06000D5E RID: 3422 RVA: 0x00047A6C File Offset: 0x00045C6C
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public void OpenReadAsync(Uri address)
		{
			this.OpenReadAsync(address, null);
		}

		// Token: 0x06000D5F RID: 3423 RVA: 0x00047A78 File Offset: 0x00045C78
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public void OpenReadAsync(Uri address, object userToken)
		{
			if (Logging.On)
			{
				Logging.Enter(Logging.Web, this, "OpenReadAsync", address);
			}
			if (address == null)
			{
				throw new ArgumentNullException("address");
			}
			this.InitWebClientAsync();
			this.ClearWebClientState();
			AsyncOperation asyncOperation = AsyncOperationManager.CreateOperation(userToken);
			this.m_AsyncOp = asyncOperation;
			try
			{
				WebRequest webRequest = (this.m_WebRequest = this.GetWebRequest(this.GetUri(address)));
				webRequest.BeginGetResponse(new AsyncCallback(this.OpenReadAsyncCallback), asyncOperation);
			}
			catch (Exception ex)
			{
				if (ex is ThreadAbortException || ex is StackOverflowException || ex is OutOfMemoryException)
				{
					throw;
				}
				if (!(ex is WebException) && !(ex is SecurityException))
				{
					ex = new WebException(SR.GetString("net_webclient"), ex);
				}
				OpenReadCompletedEventArgs openReadCompletedEventArgs = new OpenReadCompletedEventArgs(null, ex, this.m_Cancelled, asyncOperation.UserSuppliedState);
				this.InvokeOperationCompleted(asyncOperation, this.openReadOperationCompleted, openReadCompletedEventArgs);
			}
			if (Logging.On)
			{
				Logging.Exit(Logging.Web, this, "OpenReadAsync", null);
			}
		}

		// Token: 0x14000016 RID: 22
		// (add) Token: 0x06000D60 RID: 3424 RVA: 0x00047B84 File Offset: 0x00045D84
		// (remove) Token: 0x06000D61 RID: 3425 RVA: 0x00047BBC File Offset: 0x00045DBC
		public event OpenWriteCompletedEventHandler OpenWriteCompleted;

		// Token: 0x06000D62 RID: 3426 RVA: 0x00047BF1 File Offset: 0x00045DF1
		protected virtual void OnOpenWriteCompleted(OpenWriteCompletedEventArgs e)
		{
			if (this.OpenWriteCompleted != null)
			{
				this.OpenWriteCompleted(this, e);
			}
		}

		// Token: 0x06000D63 RID: 3427 RVA: 0x00047C08 File Offset: 0x00045E08
		private void OpenWriteOperationCompleted(object arg)
		{
			this.OnOpenWriteCompleted((OpenWriteCompletedEventArgs)arg);
		}

		// Token: 0x06000D64 RID: 3428 RVA: 0x00047C18 File Offset: 0x00045E18
		private void OpenWriteAsyncCallback(IAsyncResult result)
		{
			LazyAsyncResult lazyAsyncResult = (LazyAsyncResult)result;
			AsyncOperation asyncOperation = (AsyncOperation)lazyAsyncResult.AsyncState;
			WebRequest webRequest = (WebRequest)lazyAsyncResult.AsyncObject;
			WebClient.WebClientWriteStream webClientWriteStream = null;
			Exception ex = null;
			try
			{
				webClientWriteStream = new WebClient.WebClientWriteStream(webRequest.EndGetRequestStream(result), webRequest, this);
			}
			catch (Exception ex2)
			{
				if (ex2 is ThreadAbortException || ex2 is StackOverflowException || ex2 is OutOfMemoryException)
				{
					throw;
				}
				ex = ex2;
				if (!(ex2 is WebException) && !(ex2 is SecurityException))
				{
					ex = new WebException(SR.GetString("net_webclient"), ex2);
				}
			}
			OpenWriteCompletedEventArgs openWriteCompletedEventArgs = new OpenWriteCompletedEventArgs(webClientWriteStream, ex, this.m_Cancelled, asyncOperation.UserSuppliedState);
			this.InvokeOperationCompleted(asyncOperation, this.openWriteOperationCompleted, openWriteCompletedEventArgs);
		}

		// Token: 0x06000D65 RID: 3429 RVA: 0x00047CDC File Offset: 0x00045EDC
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public void OpenWriteAsync(Uri address)
		{
			this.OpenWriteAsync(address, null, null);
		}

		// Token: 0x06000D66 RID: 3430 RVA: 0x00047CE7 File Offset: 0x00045EE7
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public void OpenWriteAsync(Uri address, string method)
		{
			this.OpenWriteAsync(address, method, null);
		}

		// Token: 0x06000D67 RID: 3431 RVA: 0x00047CF4 File Offset: 0x00045EF4
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public void OpenWriteAsync(Uri address, string method, object userToken)
		{
			if (Logging.On)
			{
				Logging.Enter(Logging.Web, this, "OpenWriteAsync", ((address != null) ? address.ToString() : null) + ", " + method);
			}
			if (address == null)
			{
				throw new ArgumentNullException("address");
			}
			if (method == null)
			{
				method = this.MapToDefaultMethod(address);
			}
			this.InitWebClientAsync();
			this.ClearWebClientState();
			AsyncOperation asyncOperation = AsyncOperationManager.CreateOperation(userToken);
			this.m_AsyncOp = asyncOperation;
			try
			{
				this.m_Method = method;
				WebRequest webRequest = (this.m_WebRequest = this.GetWebRequest(this.GetUri(address)));
				webRequest.BeginGetRequestStream(new AsyncCallback(this.OpenWriteAsyncCallback), asyncOperation);
			}
			catch (Exception ex)
			{
				if (ex is ThreadAbortException || ex is StackOverflowException || ex is OutOfMemoryException)
				{
					throw;
				}
				if (!(ex is WebException) && !(ex is SecurityException))
				{
					ex = new WebException(SR.GetString("net_webclient"), ex);
				}
				OpenWriteCompletedEventArgs openWriteCompletedEventArgs = new OpenWriteCompletedEventArgs(null, ex, this.m_Cancelled, asyncOperation.UserSuppliedState);
				this.InvokeOperationCompleted(asyncOperation, this.openWriteOperationCompleted, openWriteCompletedEventArgs);
			}
			if (Logging.On)
			{
				Logging.Exit(Logging.Web, this, "OpenWriteAsync", null);
			}
		}

		// Token: 0x14000017 RID: 23
		// (add) Token: 0x06000D68 RID: 3432 RVA: 0x00047E28 File Offset: 0x00046028
		// (remove) Token: 0x06000D69 RID: 3433 RVA: 0x00047E60 File Offset: 0x00046060
		public event DownloadStringCompletedEventHandler DownloadStringCompleted;

		// Token: 0x06000D6A RID: 3434 RVA: 0x00047E95 File Offset: 0x00046095
		protected virtual void OnDownloadStringCompleted(DownloadStringCompletedEventArgs e)
		{
			if (this.DownloadStringCompleted != null)
			{
				this.DownloadStringCompleted(this, e);
			}
		}

		// Token: 0x06000D6B RID: 3435 RVA: 0x00047EAC File Offset: 0x000460AC
		private void DownloadStringOperationCompleted(object arg)
		{
			this.OnDownloadStringCompleted((DownloadStringCompletedEventArgs)arg);
		}

		// Token: 0x06000D6C RID: 3436 RVA: 0x00047EBC File Offset: 0x000460BC
		private void DownloadStringAsyncCallback(byte[] returnBytes, Exception exception, object state)
		{
			AsyncOperation asyncOperation = (AsyncOperation)state;
			string text = null;
			try
			{
				if (returnBytes != null)
				{
					text = this.GetStringUsingEncoding(this.m_WebRequest, returnBytes);
				}
			}
			catch (Exception ex)
			{
				if (ex is ThreadAbortException || ex is StackOverflowException || ex is OutOfMemoryException)
				{
					throw;
				}
				exception = ex;
			}
			DownloadStringCompletedEventArgs downloadStringCompletedEventArgs = new DownloadStringCompletedEventArgs(text, exception, this.m_Cancelled, asyncOperation.UserSuppliedState);
			this.InvokeOperationCompleted(asyncOperation, this.downloadStringOperationCompleted, downloadStringCompletedEventArgs);
		}

		// Token: 0x06000D6D RID: 3437 RVA: 0x00047F38 File Offset: 0x00046138
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public void DownloadStringAsync(Uri address)
		{
			this.DownloadStringAsync(address, null);
		}

		// Token: 0x06000D6E RID: 3438 RVA: 0x00047F44 File Offset: 0x00046144
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public void DownloadStringAsync(Uri address, object userToken)
		{
			if (Logging.On)
			{
				Logging.Enter(Logging.Web, this, "DownloadStringAsync", address);
			}
			if (address == null)
			{
				throw new ArgumentNullException("address");
			}
			this.InitWebClientAsync();
			this.ClearWebClientState();
			AsyncOperation asyncOperation = AsyncOperationManager.CreateOperation(userToken);
			this.m_AsyncOp = asyncOperation;
			try
			{
				WebRequest webRequest = (this.m_WebRequest = this.GetWebRequest(this.GetUri(address)));
				this.DownloadBits(webRequest, null, new CompletionDelegate(this.DownloadStringAsyncCallback), asyncOperation);
			}
			catch (Exception ex)
			{
				if (ex is ThreadAbortException || ex is StackOverflowException || ex is OutOfMemoryException)
				{
					throw;
				}
				if (!(ex is WebException) && !(ex is SecurityException))
				{
					ex = new WebException(SR.GetString("net_webclient"), ex);
				}
				this.DownloadStringAsyncCallback(null, ex, asyncOperation);
			}
			if (Logging.On)
			{
				Logging.Exit(Logging.Web, this, "DownloadStringAsync", "");
			}
		}

		// Token: 0x14000018 RID: 24
		// (add) Token: 0x06000D6F RID: 3439 RVA: 0x0004803C File Offset: 0x0004623C
		// (remove) Token: 0x06000D70 RID: 3440 RVA: 0x00048074 File Offset: 0x00046274
		public event DownloadDataCompletedEventHandler DownloadDataCompleted;

		// Token: 0x06000D71 RID: 3441 RVA: 0x000480A9 File Offset: 0x000462A9
		protected virtual void OnDownloadDataCompleted(DownloadDataCompletedEventArgs e)
		{
			if (this.DownloadDataCompleted != null)
			{
				this.DownloadDataCompleted(this, e);
			}
		}

		// Token: 0x06000D72 RID: 3442 RVA: 0x000480C0 File Offset: 0x000462C0
		private void DownloadDataOperationCompleted(object arg)
		{
			this.OnDownloadDataCompleted((DownloadDataCompletedEventArgs)arg);
		}

		// Token: 0x06000D73 RID: 3443 RVA: 0x000480D0 File Offset: 0x000462D0
		private void DownloadDataAsyncCallback(byte[] returnBytes, Exception exception, object state)
		{
			AsyncOperation asyncOperation = (AsyncOperation)state;
			DownloadDataCompletedEventArgs downloadDataCompletedEventArgs = new DownloadDataCompletedEventArgs(returnBytes, exception, this.m_Cancelled, asyncOperation.UserSuppliedState);
			this.InvokeOperationCompleted(asyncOperation, this.downloadDataOperationCompleted, downloadDataCompletedEventArgs);
		}

		// Token: 0x06000D74 RID: 3444 RVA: 0x00048106 File Offset: 0x00046306
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public void DownloadDataAsync(Uri address)
		{
			this.DownloadDataAsync(address, null);
		}

		// Token: 0x06000D75 RID: 3445 RVA: 0x00048110 File Offset: 0x00046310
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public void DownloadDataAsync(Uri address, object userToken)
		{
			if (Logging.On)
			{
				Logging.Enter(Logging.Web, this, "DownloadDataAsync", address);
			}
			if (address == null)
			{
				throw new ArgumentNullException("address");
			}
			this.InitWebClientAsync();
			this.ClearWebClientState();
			AsyncOperation asyncOperation = AsyncOperationManager.CreateOperation(userToken);
			this.m_AsyncOp = asyncOperation;
			try
			{
				WebRequest webRequest = (this.m_WebRequest = this.GetWebRequest(this.GetUri(address)));
				this.DownloadBits(webRequest, null, new CompletionDelegate(this.DownloadDataAsyncCallback), asyncOperation);
			}
			catch (Exception ex)
			{
				if (ex is ThreadAbortException || ex is StackOverflowException || ex is OutOfMemoryException)
				{
					throw;
				}
				if (!(ex is WebException) && !(ex is SecurityException))
				{
					ex = new WebException(SR.GetString("net_webclient"), ex);
				}
				this.DownloadDataAsyncCallback(null, ex, asyncOperation);
			}
			if (Logging.On)
			{
				Logging.Exit(Logging.Web, this, "DownloadDataAsync", null);
			}
		}

		// Token: 0x14000019 RID: 25
		// (add) Token: 0x06000D76 RID: 3446 RVA: 0x00048204 File Offset: 0x00046404
		// (remove) Token: 0x06000D77 RID: 3447 RVA: 0x0004823C File Offset: 0x0004643C
		public event AsyncCompletedEventHandler DownloadFileCompleted;

		// Token: 0x06000D78 RID: 3448 RVA: 0x00048271 File Offset: 0x00046471
		protected virtual void OnDownloadFileCompleted(AsyncCompletedEventArgs e)
		{
			if (this.DownloadFileCompleted != null)
			{
				this.DownloadFileCompleted(this, e);
			}
		}

		// Token: 0x06000D79 RID: 3449 RVA: 0x00048288 File Offset: 0x00046488
		private void DownloadFileOperationCompleted(object arg)
		{
			this.OnDownloadFileCompleted((AsyncCompletedEventArgs)arg);
		}

		// Token: 0x06000D7A RID: 3450 RVA: 0x00048298 File Offset: 0x00046498
		private void DownloadFileAsyncCallback(byte[] returnBytes, Exception exception, object state)
		{
			AsyncOperation asyncOperation = (AsyncOperation)state;
			AsyncCompletedEventArgs asyncCompletedEventArgs = new AsyncCompletedEventArgs(exception, this.m_Cancelled, asyncOperation.UserSuppliedState);
			this.InvokeOperationCompleted(asyncOperation, this.downloadFileOperationCompleted, asyncCompletedEventArgs);
		}

		// Token: 0x06000D7B RID: 3451 RVA: 0x000482CD File Offset: 0x000464CD
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public void DownloadFileAsync(Uri address, string fileName)
		{
			this.DownloadFileAsync(address, fileName, null);
		}

		// Token: 0x06000D7C RID: 3452 RVA: 0x000482D8 File Offset: 0x000464D8
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public void DownloadFileAsync(Uri address, string fileName, object userToken)
		{
			if (Logging.On)
			{
				Logging.Enter(Logging.Web, this, "DownloadFileAsync", address);
			}
			if (address == null)
			{
				throw new ArgumentNullException("address");
			}
			if (fileName == null)
			{
				throw new ArgumentNullException("fileName");
			}
			FileStream fileStream = null;
			this.InitWebClientAsync();
			this.ClearWebClientState();
			AsyncOperation asyncOperation = AsyncOperationManager.CreateOperation(userToken);
			this.m_AsyncOp = asyncOperation;
			try
			{
				fileStream = new FileStream(fileName, FileMode.Create, FileAccess.Write);
				WebRequest webRequest = (this.m_WebRequest = this.GetWebRequest(this.GetUri(address)));
				this.DownloadBits(webRequest, fileStream, new CompletionDelegate(this.DownloadFileAsyncCallback), asyncOperation);
			}
			catch (Exception ex)
			{
				if (ex is ThreadAbortException || ex is StackOverflowException || ex is OutOfMemoryException)
				{
					throw;
				}
				if (fileStream != null)
				{
					fileStream.Close();
				}
				if (!(ex is WebException) && !(ex is SecurityException))
				{
					ex = new WebException(SR.GetString("net_webclient"), ex);
				}
				this.DownloadFileAsyncCallback(null, ex, asyncOperation);
			}
			if (Logging.On)
			{
				Logging.Exit(Logging.Web, this, "DownloadFileAsync", null);
			}
		}

		// Token: 0x1400001A RID: 26
		// (add) Token: 0x06000D7D RID: 3453 RVA: 0x000483F4 File Offset: 0x000465F4
		// (remove) Token: 0x06000D7E RID: 3454 RVA: 0x0004842C File Offset: 0x0004662C
		public event UploadStringCompletedEventHandler UploadStringCompleted;

		// Token: 0x06000D7F RID: 3455 RVA: 0x00048461 File Offset: 0x00046661
		protected virtual void OnUploadStringCompleted(UploadStringCompletedEventArgs e)
		{
			if (this.UploadStringCompleted != null)
			{
				this.UploadStringCompleted(this, e);
			}
		}

		// Token: 0x06000D80 RID: 3456 RVA: 0x00048478 File Offset: 0x00046678
		private void UploadStringOperationCompleted(object arg)
		{
			this.OnUploadStringCompleted((UploadStringCompletedEventArgs)arg);
		}

		// Token: 0x06000D81 RID: 3457 RVA: 0x00048488 File Offset: 0x00046688
		private void StartDownloadAsync(WebClient.UploadBitsState state)
		{
			try
			{
				this.DownloadBits(state.Request, null, state.DownloadCompletionDelegate, state.AsyncOp);
			}
			catch (Exception ex)
			{
				if (ex is ThreadAbortException || ex is StackOverflowException || ex is OutOfMemoryException)
				{
					throw;
				}
				if (!(ex is WebException) && !(ex is SecurityException))
				{
					ex = new WebException(SR.GetString("net_webclient"), ex);
				}
				state.DownloadCompletionDelegate(null, ex, state.AsyncOp);
			}
		}

		// Token: 0x06000D82 RID: 3458 RVA: 0x00048514 File Offset: 0x00046714
		private void UploadStringAsyncWriteCallback(byte[] returnBytes, Exception exception, object state)
		{
			WebClient.UploadBitsState uploadBitsState = (WebClient.UploadBitsState)state;
			if (exception != null)
			{
				UploadStringCompletedEventArgs uploadStringCompletedEventArgs = new UploadStringCompletedEventArgs(null, exception, this.m_Cancelled, uploadBitsState.AsyncOp.UserSuppliedState);
				this.InvokeOperationCompleted(uploadBitsState.AsyncOp, this.uploadStringOperationCompleted, uploadStringCompletedEventArgs);
				return;
			}
			this.StartDownloadAsync(uploadBitsState);
		}

		// Token: 0x06000D83 RID: 3459 RVA: 0x00048560 File Offset: 0x00046760
		private void UploadStringAsyncReadCallback(byte[] returnBytes, Exception exception, object state)
		{
			AsyncOperation asyncOperation = (AsyncOperation)state;
			string text = null;
			try
			{
				if (returnBytes != null)
				{
					text = this.GetStringUsingEncoding(this.m_WebRequest, returnBytes);
				}
			}
			catch (Exception ex)
			{
				if (ex is ThreadAbortException || ex is StackOverflowException || ex is OutOfMemoryException)
				{
					throw;
				}
				exception = ex;
			}
			UploadStringCompletedEventArgs uploadStringCompletedEventArgs = new UploadStringCompletedEventArgs(text, exception, this.m_Cancelled, asyncOperation.UserSuppliedState);
			this.InvokeOperationCompleted(asyncOperation, this.uploadStringOperationCompleted, uploadStringCompletedEventArgs);
		}

		// Token: 0x06000D84 RID: 3460 RVA: 0x000485DC File Offset: 0x000467DC
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public void UploadStringAsync(Uri address, string data)
		{
			this.UploadStringAsync(address, null, data, null);
		}

		// Token: 0x06000D85 RID: 3461 RVA: 0x000485E8 File Offset: 0x000467E8
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public void UploadStringAsync(Uri address, string method, string data)
		{
			this.UploadStringAsync(address, method, data, null);
		}

		// Token: 0x06000D86 RID: 3462 RVA: 0x000485F4 File Offset: 0x000467F4
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public void UploadStringAsync(Uri address, string method, string data, object userToken)
		{
			if (Logging.On)
			{
				Logging.Enter(Logging.Web, this, "UploadStringAsync", address);
			}
			if (address == null)
			{
				throw new ArgumentNullException("address");
			}
			if (data == null)
			{
				throw new ArgumentNullException("data");
			}
			if (method == null)
			{
				method = this.MapToDefaultMethod(address);
			}
			this.InitWebClientAsync();
			this.ClearWebClientState();
			AsyncOperation asyncOperation = AsyncOperationManager.CreateOperation(userToken);
			this.m_AsyncOp = asyncOperation;
			try
			{
				byte[] bytes = this.Encoding.GetBytes(data);
				this.m_Method = method;
				this.m_ContentLength = (long)bytes.Length;
				WebRequest webRequest = (this.m_WebRequest = this.GetWebRequest(this.GetUri(address)));
				this.UploadBits(webRequest, null, bytes, 0, null, null, new CompletionDelegate(this.UploadStringAsyncWriteCallback), new CompletionDelegate(this.UploadStringAsyncReadCallback), asyncOperation);
			}
			catch (Exception ex)
			{
				if (ex is ThreadAbortException || ex is StackOverflowException || ex is OutOfMemoryException)
				{
					throw;
				}
				if (!(ex is WebException) && !(ex is SecurityException))
				{
					ex = new WebException(SR.GetString("net_webclient"), ex);
				}
				UploadStringCompletedEventArgs uploadStringCompletedEventArgs = new UploadStringCompletedEventArgs(null, ex, this.m_Cancelled, asyncOperation.UserSuppliedState);
				this.InvokeOperationCompleted(asyncOperation, this.uploadStringOperationCompleted, uploadStringCompletedEventArgs);
			}
			if (Logging.On)
			{
				Logging.Exit(Logging.Web, this, "UploadStringAsync", null);
			}
		}

		// Token: 0x1400001B RID: 27
		// (add) Token: 0x06000D87 RID: 3463 RVA: 0x00048754 File Offset: 0x00046954
		// (remove) Token: 0x06000D88 RID: 3464 RVA: 0x0004878C File Offset: 0x0004698C
		public event UploadDataCompletedEventHandler UploadDataCompleted;

		// Token: 0x06000D89 RID: 3465 RVA: 0x000487C1 File Offset: 0x000469C1
		protected virtual void OnUploadDataCompleted(UploadDataCompletedEventArgs e)
		{
			if (this.UploadDataCompleted != null)
			{
				this.UploadDataCompleted(this, e);
			}
		}

		// Token: 0x06000D8A RID: 3466 RVA: 0x000487D8 File Offset: 0x000469D8
		private void UploadDataOperationCompleted(object arg)
		{
			this.OnUploadDataCompleted((UploadDataCompletedEventArgs)arg);
		}

		// Token: 0x06000D8B RID: 3467 RVA: 0x000487E8 File Offset: 0x000469E8
		private void UploadDataAsyncWriteCallback(byte[] returnBytes, Exception exception, object state)
		{
			WebClient.UploadBitsState uploadBitsState = (WebClient.UploadBitsState)state;
			if (exception != null)
			{
				UploadDataCompletedEventArgs uploadDataCompletedEventArgs = new UploadDataCompletedEventArgs(returnBytes, exception, this.m_Cancelled, uploadBitsState.AsyncOp.UserSuppliedState);
				this.InvokeOperationCompleted(uploadBitsState.AsyncOp, this.uploadDataOperationCompleted, uploadDataCompletedEventArgs);
				return;
			}
			this.StartDownloadAsync(uploadBitsState);
		}

		// Token: 0x06000D8C RID: 3468 RVA: 0x00048834 File Offset: 0x00046A34
		private void UploadDataAsyncReadCallback(byte[] returnBytes, Exception exception, object state)
		{
			AsyncOperation asyncOperation = (AsyncOperation)state;
			UploadDataCompletedEventArgs uploadDataCompletedEventArgs = new UploadDataCompletedEventArgs(returnBytes, exception, this.m_Cancelled, asyncOperation.UserSuppliedState);
			this.InvokeOperationCompleted(asyncOperation, this.uploadDataOperationCompleted, uploadDataCompletedEventArgs);
		}

		// Token: 0x06000D8D RID: 3469 RVA: 0x0004886A File Offset: 0x00046A6A
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public void UploadDataAsync(Uri address, byte[] data)
		{
			this.UploadDataAsync(address, null, data, null);
		}

		// Token: 0x06000D8E RID: 3470 RVA: 0x00048876 File Offset: 0x00046A76
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public void UploadDataAsync(Uri address, string method, byte[] data)
		{
			this.UploadDataAsync(address, method, data, null);
		}

		// Token: 0x06000D8F RID: 3471 RVA: 0x00048884 File Offset: 0x00046A84
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public void UploadDataAsync(Uri address, string method, byte[] data, object userToken)
		{
			if (Logging.On)
			{
				Logging.Enter(Logging.Web, this, "UploadDataAsync", ((address != null) ? address.ToString() : null) + ", " + method);
			}
			if (address == null)
			{
				throw new ArgumentNullException("address");
			}
			if (data == null)
			{
				throw new ArgumentNullException("data");
			}
			if (method == null)
			{
				method = this.MapToDefaultMethod(address);
			}
			this.InitWebClientAsync();
			this.ClearWebClientState();
			AsyncOperation asyncOperation = AsyncOperationManager.CreateOperation(userToken);
			this.m_AsyncOp = asyncOperation;
			int num = 0;
			try
			{
				this.m_Method = method;
				this.m_ContentLength = (long)data.Length;
				WebRequest webRequest = (this.m_WebRequest = this.GetWebRequest(this.GetUri(address)));
				if (this.UploadProgressChanged != null)
				{
					num = (int)Math.Min(8192L, (long)data.Length);
				}
				this.UploadBits(webRequest, null, data, num, null, null, new CompletionDelegate(this.UploadDataAsyncWriteCallback), new CompletionDelegate(this.UploadDataAsyncReadCallback), asyncOperation);
			}
			catch (Exception ex)
			{
				if (ex is ThreadAbortException || ex is StackOverflowException || ex is OutOfMemoryException)
				{
					throw;
				}
				if (!(ex is WebException) && !(ex is SecurityException))
				{
					ex = new WebException(SR.GetString("net_webclient"), ex);
				}
				UploadDataCompletedEventArgs uploadDataCompletedEventArgs = new UploadDataCompletedEventArgs(null, ex, this.m_Cancelled, asyncOperation.UserSuppliedState);
				this.InvokeOperationCompleted(asyncOperation, this.uploadDataOperationCompleted, uploadDataCompletedEventArgs);
			}
			if (Logging.On)
			{
				Logging.Exit(Logging.Web, this, "UploadDataAsync", null);
			}
		}

		// Token: 0x1400001C RID: 28
		// (add) Token: 0x06000D90 RID: 3472 RVA: 0x00048A08 File Offset: 0x00046C08
		// (remove) Token: 0x06000D91 RID: 3473 RVA: 0x00048A40 File Offset: 0x00046C40
		public event UploadFileCompletedEventHandler UploadFileCompleted;

		// Token: 0x06000D92 RID: 3474 RVA: 0x00048A75 File Offset: 0x00046C75
		protected virtual void OnUploadFileCompleted(UploadFileCompletedEventArgs e)
		{
			if (this.UploadFileCompleted != null)
			{
				this.UploadFileCompleted(this, e);
			}
		}

		// Token: 0x06000D93 RID: 3475 RVA: 0x00048A8C File Offset: 0x00046C8C
		private void UploadFileOperationCompleted(object arg)
		{
			this.OnUploadFileCompleted((UploadFileCompletedEventArgs)arg);
		}

		// Token: 0x06000D94 RID: 3476 RVA: 0x00048A9C File Offset: 0x00046C9C
		private void UploadFileAsyncWriteCallback(byte[] returnBytes, Exception exception, object state)
		{
			WebClient.UploadBitsState uploadBitsState = (WebClient.UploadBitsState)state;
			if (exception != null)
			{
				UploadFileCompletedEventArgs uploadFileCompletedEventArgs = new UploadFileCompletedEventArgs(returnBytes, exception, this.m_Cancelled, uploadBitsState.AsyncOp.UserSuppliedState);
				this.InvokeOperationCompleted(uploadBitsState.AsyncOp, this.uploadFileOperationCompleted, uploadFileCompletedEventArgs);
				return;
			}
			this.StartDownloadAsync(uploadBitsState);
		}

		// Token: 0x06000D95 RID: 3477 RVA: 0x00048AE8 File Offset: 0x00046CE8
		private void UploadFileAsyncReadCallback(byte[] returnBytes, Exception exception, object state)
		{
			AsyncOperation asyncOperation = (AsyncOperation)state;
			UploadFileCompletedEventArgs uploadFileCompletedEventArgs = new UploadFileCompletedEventArgs(returnBytes, exception, this.m_Cancelled, asyncOperation.UserSuppliedState);
			this.InvokeOperationCompleted(asyncOperation, this.uploadFileOperationCompleted, uploadFileCompletedEventArgs);
		}

		// Token: 0x06000D96 RID: 3478 RVA: 0x00048B1E File Offset: 0x00046D1E
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public void UploadFileAsync(Uri address, string fileName)
		{
			this.UploadFileAsync(address, null, fileName, null);
		}

		// Token: 0x06000D97 RID: 3479 RVA: 0x00048B2A File Offset: 0x00046D2A
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public void UploadFileAsync(Uri address, string method, string fileName)
		{
			this.UploadFileAsync(address, method, fileName, null);
		}

		// Token: 0x06000D98 RID: 3480 RVA: 0x00048B38 File Offset: 0x00046D38
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public void UploadFileAsync(Uri address, string method, string fileName, object userToken)
		{
			if (Logging.On)
			{
				Logging.Enter(Logging.Web, this, "UploadFileAsync", ((address != null) ? address.ToString() : null) + ", " + method);
			}
			if (address == null)
			{
				throw new ArgumentNullException("address");
			}
			if (fileName == null)
			{
				throw new ArgumentNullException("fileName");
			}
			if (method == null)
			{
				method = this.MapToDefaultMethod(address);
			}
			this.InitWebClientAsync();
			this.ClearWebClientState();
			AsyncOperation asyncOperation = AsyncOperationManager.CreateOperation(userToken);
			this.m_AsyncOp = asyncOperation;
			FileStream fileStream = null;
			try
			{
				this.m_Method = method;
				byte[] array = null;
				byte[] array2 = null;
				byte[] array3 = null;
				Uri uri = this.GetUri(address);
				bool flag = uri.Scheme != Uri.UriSchemeFile;
				this.OpenFileInternal(flag, fileName, ref fileStream, ref array3, ref array, ref array2);
				WebRequest webRequest = (this.m_WebRequest = this.GetWebRequest(uri));
				this.UploadBits(webRequest, fileStream, array3, 0, array, array2, new CompletionDelegate(this.UploadFileAsyncWriteCallback), new CompletionDelegate(this.UploadFileAsyncReadCallback), asyncOperation);
			}
			catch (Exception ex)
			{
				if (ex is ThreadAbortException || ex is StackOverflowException || ex is OutOfMemoryException)
				{
					throw;
				}
				if (fileStream != null)
				{
					fileStream.Close();
				}
				if (!(ex is WebException) && !(ex is SecurityException))
				{
					ex = new WebException(SR.GetString("net_webclient"), ex);
				}
				UploadFileCompletedEventArgs uploadFileCompletedEventArgs = new UploadFileCompletedEventArgs(null, ex, this.m_Cancelled, asyncOperation.UserSuppliedState);
				this.InvokeOperationCompleted(asyncOperation, this.uploadFileOperationCompleted, uploadFileCompletedEventArgs);
			}
			if (Logging.On)
			{
				Logging.Exit(Logging.Web, this, "UploadFileAsync", null);
			}
		}

		// Token: 0x1400001D RID: 29
		// (add) Token: 0x06000D99 RID: 3481 RVA: 0x00048CD4 File Offset: 0x00046ED4
		// (remove) Token: 0x06000D9A RID: 3482 RVA: 0x00048D0C File Offset: 0x00046F0C
		public event UploadValuesCompletedEventHandler UploadValuesCompleted;

		// Token: 0x06000D9B RID: 3483 RVA: 0x00048D41 File Offset: 0x00046F41
		protected virtual void OnUploadValuesCompleted(UploadValuesCompletedEventArgs e)
		{
			if (this.UploadValuesCompleted != null)
			{
				this.UploadValuesCompleted(this, e);
			}
		}

		// Token: 0x06000D9C RID: 3484 RVA: 0x00048D58 File Offset: 0x00046F58
		private void UploadValuesOperationCompleted(object arg)
		{
			this.OnUploadValuesCompleted((UploadValuesCompletedEventArgs)arg);
		}

		// Token: 0x06000D9D RID: 3485 RVA: 0x00048D68 File Offset: 0x00046F68
		private void UploadValuesAsyncWriteCallback(byte[] returnBytes, Exception exception, object state)
		{
			WebClient.UploadBitsState uploadBitsState = (WebClient.UploadBitsState)state;
			if (exception != null)
			{
				UploadValuesCompletedEventArgs uploadValuesCompletedEventArgs = new UploadValuesCompletedEventArgs(returnBytes, exception, this.m_Cancelled, uploadBitsState.AsyncOp.UserSuppliedState);
				this.InvokeOperationCompleted(uploadBitsState.AsyncOp, this.uploadValuesOperationCompleted, uploadValuesCompletedEventArgs);
				return;
			}
			this.StartDownloadAsync(uploadBitsState);
		}

		// Token: 0x06000D9E RID: 3486 RVA: 0x00048DB4 File Offset: 0x00046FB4
		private void UploadValuesAsyncReadCallback(byte[] returnBytes, Exception exception, object state)
		{
			AsyncOperation asyncOperation = (AsyncOperation)state;
			UploadValuesCompletedEventArgs uploadValuesCompletedEventArgs = new UploadValuesCompletedEventArgs(returnBytes, exception, this.m_Cancelled, asyncOperation.UserSuppliedState);
			this.InvokeOperationCompleted(asyncOperation, this.uploadValuesOperationCompleted, uploadValuesCompletedEventArgs);
		}

		// Token: 0x06000D9F RID: 3487 RVA: 0x00048DEA File Offset: 0x00046FEA
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public void UploadValuesAsync(Uri address, NameValueCollection data)
		{
			this.UploadValuesAsync(address, null, data, null);
		}

		// Token: 0x06000DA0 RID: 3488 RVA: 0x00048DF6 File Offset: 0x00046FF6
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public void UploadValuesAsync(Uri address, string method, NameValueCollection data)
		{
			this.UploadValuesAsync(address, method, data, null);
		}

		// Token: 0x06000DA1 RID: 3489 RVA: 0x00048E04 File Offset: 0x00047004
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public void UploadValuesAsync(Uri address, string method, NameValueCollection data, object userToken)
		{
			if (Logging.On)
			{
				Logging.Enter(Logging.Web, this, "UploadValuesAsync", ((address != null) ? address.ToString() : null) + ", " + method);
			}
			if (address == null)
			{
				throw new ArgumentNullException("address");
			}
			if (data == null)
			{
				throw new ArgumentNullException("data");
			}
			if (method == null)
			{
				method = this.MapToDefaultMethod(address);
			}
			this.InitWebClientAsync();
			this.ClearWebClientState();
			AsyncOperation asyncOperation = AsyncOperationManager.CreateOperation(userToken);
			this.m_AsyncOp = asyncOperation;
			int num = 0;
			try
			{
				byte[] array = this.UploadValuesInternal(data);
				this.m_Method = method;
				WebRequest webRequest = (this.m_WebRequest = this.GetWebRequest(this.GetUri(address)));
				if (this.UploadProgressChanged != null)
				{
					num = (int)Math.Min(8192L, (long)array.Length);
				}
				this.UploadBits(webRequest, null, array, num, null, null, new CompletionDelegate(this.UploadValuesAsyncWriteCallback), new CompletionDelegate(this.UploadValuesAsyncReadCallback), asyncOperation);
			}
			catch (Exception ex)
			{
				if (ex is ThreadAbortException || ex is StackOverflowException || ex is OutOfMemoryException)
				{
					throw;
				}
				if (!(ex is WebException) && !(ex is SecurityException))
				{
					ex = new WebException(SR.GetString("net_webclient"), ex);
				}
				UploadValuesCompletedEventArgs uploadValuesCompletedEventArgs = new UploadValuesCompletedEventArgs(null, ex, this.m_Cancelled, asyncOperation.UserSuppliedState);
				this.InvokeOperationCompleted(asyncOperation, this.uploadValuesOperationCompleted, uploadValuesCompletedEventArgs);
			}
			if (Logging.On)
			{
				Logging.Exit(Logging.Web, this, "UploadValuesAsync", null);
			}
		}

		// Token: 0x06000DA2 RID: 3490 RVA: 0x00048F88 File Offset: 0x00047188
		public void CancelAsync()
		{
			WebRequest webRequest = this.m_WebRequest;
			this.m_Cancelled = true;
			WebClient.AbortRequest(webRequest);
		}

		// Token: 0x06000DA3 RID: 3491 RVA: 0x00048FA9 File Offset: 0x000471A9
		[ComVisible(false)]
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public Task<string> DownloadStringTaskAsync(string address)
		{
			return this.DownloadStringTaskAsync(this.GetUri(address));
		}

		// Token: 0x06000DA4 RID: 3492 RVA: 0x00048FB8 File Offset: 0x000471B8
		[ComVisible(false)]
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public Task<string> DownloadStringTaskAsync(Uri address)
		{
			TaskCompletionSource<string> tcs = new TaskCompletionSource<string>(address);
			DownloadStringCompletedEventHandler handler = null;
			handler = delegate(object sender, DownloadStringCompletedEventArgs e)
			{
				this.HandleCompletion<DownloadStringCompletedEventArgs, DownloadStringCompletedEventHandler, string>(tcs, e, (DownloadStringCompletedEventArgs args) => args.Result, handler, delegate(WebClient webClient, DownloadStringCompletedEventHandler completion)
				{
					webClient.DownloadStringCompleted -= completion;
				});
			};
			this.DownloadStringCompleted += handler;
			try
			{
				this.DownloadStringAsync(address, tcs);
			}
			catch
			{
				this.DownloadStringCompleted -= handler;
				throw;
			}
			return tcs.Task;
		}

		// Token: 0x06000DA5 RID: 3493 RVA: 0x0004903C File Offset: 0x0004723C
		[ComVisible(false)]
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public Task<Stream> OpenReadTaskAsync(string address)
		{
			return this.OpenReadTaskAsync(this.GetUri(address));
		}

		// Token: 0x06000DA6 RID: 3494 RVA: 0x0004904C File Offset: 0x0004724C
		[ComVisible(false)]
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public Task<Stream> OpenReadTaskAsync(Uri address)
		{
			TaskCompletionSource<Stream> tcs = new TaskCompletionSource<Stream>(address);
			OpenReadCompletedEventHandler handler = null;
			handler = delegate(object sender, OpenReadCompletedEventArgs e)
			{
				this.HandleCompletion<OpenReadCompletedEventArgs, OpenReadCompletedEventHandler, Stream>(tcs, e, (OpenReadCompletedEventArgs args) => args.Result, handler, delegate(WebClient webClient, OpenReadCompletedEventHandler completion)
				{
					webClient.OpenReadCompleted -= completion;
				});
			};
			this.OpenReadCompleted += handler;
			try
			{
				this.OpenReadAsync(address, tcs);
			}
			catch
			{
				this.OpenReadCompleted -= handler;
				throw;
			}
			return tcs.Task;
		}

		// Token: 0x06000DA7 RID: 3495 RVA: 0x000490D0 File Offset: 0x000472D0
		[ComVisible(false)]
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public Task<Stream> OpenWriteTaskAsync(string address)
		{
			return this.OpenWriteTaskAsync(this.GetUri(address), null);
		}

		// Token: 0x06000DA8 RID: 3496 RVA: 0x000490E0 File Offset: 0x000472E0
		[ComVisible(false)]
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public Task<Stream> OpenWriteTaskAsync(Uri address)
		{
			return this.OpenWriteTaskAsync(address, null);
		}

		// Token: 0x06000DA9 RID: 3497 RVA: 0x000490EA File Offset: 0x000472EA
		[ComVisible(false)]
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public Task<Stream> OpenWriteTaskAsync(string address, string method)
		{
			return this.OpenWriteTaskAsync(this.GetUri(address), method);
		}

		// Token: 0x06000DAA RID: 3498 RVA: 0x000490FC File Offset: 0x000472FC
		[ComVisible(false)]
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public Task<Stream> OpenWriteTaskAsync(Uri address, string method)
		{
			TaskCompletionSource<Stream> tcs = new TaskCompletionSource<Stream>(address);
			OpenWriteCompletedEventHandler handler = null;
			handler = delegate(object sender, OpenWriteCompletedEventArgs e)
			{
				this.HandleCompletion<OpenWriteCompletedEventArgs, OpenWriteCompletedEventHandler, Stream>(tcs, e, (OpenWriteCompletedEventArgs args) => args.Result, handler, delegate(WebClient webClient, OpenWriteCompletedEventHandler completion)
				{
					webClient.OpenWriteCompleted -= completion;
				});
			};
			this.OpenWriteCompleted += handler;
			try
			{
				this.OpenWriteAsync(address, method, tcs);
			}
			catch
			{
				this.OpenWriteCompleted -= handler;
				throw;
			}
			return tcs.Task;
		}

		// Token: 0x06000DAB RID: 3499 RVA: 0x00049184 File Offset: 0x00047384
		[ComVisible(false)]
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public Task<string> UploadStringTaskAsync(string address, string data)
		{
			return this.UploadStringTaskAsync(address, null, data);
		}

		// Token: 0x06000DAC RID: 3500 RVA: 0x0004918F File Offset: 0x0004738F
		[ComVisible(false)]
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public Task<string> UploadStringTaskAsync(Uri address, string data)
		{
			return this.UploadStringTaskAsync(address, null, data);
		}

		// Token: 0x06000DAD RID: 3501 RVA: 0x0004919A File Offset: 0x0004739A
		[ComVisible(false)]
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public Task<string> UploadStringTaskAsync(string address, string method, string data)
		{
			return this.UploadStringTaskAsync(this.GetUri(address), method, data);
		}

		// Token: 0x06000DAE RID: 3502 RVA: 0x000491AC File Offset: 0x000473AC
		[ComVisible(false)]
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public Task<string> UploadStringTaskAsync(Uri address, string method, string data)
		{
			TaskCompletionSource<string> tcs = new TaskCompletionSource<string>(address);
			UploadStringCompletedEventHandler handler = null;
			handler = delegate(object sender, UploadStringCompletedEventArgs e)
			{
				this.HandleCompletion<UploadStringCompletedEventArgs, UploadStringCompletedEventHandler, string>(tcs, e, (UploadStringCompletedEventArgs args) => args.Result, handler, delegate(WebClient webClient, UploadStringCompletedEventHandler completion)
				{
					webClient.UploadStringCompleted -= completion;
				});
			};
			this.UploadStringCompleted += handler;
			try
			{
				this.UploadStringAsync(address, method, data, tcs);
			}
			catch
			{
				this.UploadStringCompleted -= handler;
				throw;
			}
			return tcs.Task;
		}

		// Token: 0x06000DAF RID: 3503 RVA: 0x00049234 File Offset: 0x00047434
		[ComVisible(false)]
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public Task<byte[]> DownloadDataTaskAsync(string address)
		{
			return this.DownloadDataTaskAsync(this.GetUri(address));
		}

		// Token: 0x06000DB0 RID: 3504 RVA: 0x00049244 File Offset: 0x00047444
		[ComVisible(false)]
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public Task<byte[]> DownloadDataTaskAsync(Uri address)
		{
			TaskCompletionSource<byte[]> tcs = new TaskCompletionSource<byte[]>(address);
			DownloadDataCompletedEventHandler handler = null;
			handler = delegate(object sender, DownloadDataCompletedEventArgs e)
			{
				this.HandleCompletion<DownloadDataCompletedEventArgs, DownloadDataCompletedEventHandler, byte[]>(tcs, e, (DownloadDataCompletedEventArgs args) => args.Result, handler, delegate(WebClient webClient, DownloadDataCompletedEventHandler completion)
				{
					webClient.DownloadDataCompleted -= completion;
				});
			};
			this.DownloadDataCompleted += handler;
			try
			{
				this.DownloadDataAsync(address, tcs);
			}
			catch
			{
				this.DownloadDataCompleted -= handler;
				throw;
			}
			return tcs.Task;
		}

		// Token: 0x06000DB1 RID: 3505 RVA: 0x000492C8 File Offset: 0x000474C8
		[ComVisible(false)]
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public Task DownloadFileTaskAsync(string address, string fileName)
		{
			return this.DownloadFileTaskAsync(this.GetUri(address), fileName);
		}

		// Token: 0x06000DB2 RID: 3506 RVA: 0x000492D8 File Offset: 0x000474D8
		[ComVisible(false)]
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public Task DownloadFileTaskAsync(Uri address, string fileName)
		{
			TaskCompletionSource<object> tcs = new TaskCompletionSource<object>(address);
			AsyncCompletedEventHandler handler = null;
			handler = delegate(object sender, AsyncCompletedEventArgs e)
			{
				this.HandleCompletion<AsyncCompletedEventArgs, AsyncCompletedEventHandler, object>(tcs, e, (AsyncCompletedEventArgs args) => null, handler, delegate(WebClient webClient, AsyncCompletedEventHandler completion)
				{
					webClient.DownloadFileCompleted -= completion;
				});
			};
			this.DownloadFileCompleted += handler;
			try
			{
				this.DownloadFileAsync(address, fileName, tcs);
			}
			catch
			{
				this.DownloadFileCompleted -= handler;
				throw;
			}
			return tcs.Task;
		}

		// Token: 0x06000DB3 RID: 3507 RVA: 0x00049360 File Offset: 0x00047560
		[ComVisible(false)]
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public Task<byte[]> UploadDataTaskAsync(string address, byte[] data)
		{
			return this.UploadDataTaskAsync(this.GetUri(address), null, data);
		}

		// Token: 0x06000DB4 RID: 3508 RVA: 0x00049371 File Offset: 0x00047571
		[ComVisible(false)]
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public Task<byte[]> UploadDataTaskAsync(Uri address, byte[] data)
		{
			return this.UploadDataTaskAsync(address, null, data);
		}

		// Token: 0x06000DB5 RID: 3509 RVA: 0x0004937C File Offset: 0x0004757C
		[ComVisible(false)]
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public Task<byte[]> UploadDataTaskAsync(string address, string method, byte[] data)
		{
			return this.UploadDataTaskAsync(this.GetUri(address), method, data);
		}

		// Token: 0x06000DB6 RID: 3510 RVA: 0x00049390 File Offset: 0x00047590
		[ComVisible(false)]
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public Task<byte[]> UploadDataTaskAsync(Uri address, string method, byte[] data)
		{
			TaskCompletionSource<byte[]> tcs = new TaskCompletionSource<byte[]>(address);
			UploadDataCompletedEventHandler handler = null;
			handler = delegate(object sender, UploadDataCompletedEventArgs e)
			{
				this.HandleCompletion<UploadDataCompletedEventArgs, UploadDataCompletedEventHandler, byte[]>(tcs, e, (UploadDataCompletedEventArgs args) => args.Result, handler, delegate(WebClient webClient, UploadDataCompletedEventHandler completion)
				{
					webClient.UploadDataCompleted -= completion;
				});
			};
			this.UploadDataCompleted += handler;
			try
			{
				this.UploadDataAsync(address, method, data, tcs);
			}
			catch
			{
				this.UploadDataCompleted -= handler;
				throw;
			}
			return tcs.Task;
		}

		// Token: 0x06000DB7 RID: 3511 RVA: 0x00049418 File Offset: 0x00047618
		[ComVisible(false)]
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public Task<byte[]> UploadFileTaskAsync(string address, string fileName)
		{
			return this.UploadFileTaskAsync(this.GetUri(address), null, fileName);
		}

		// Token: 0x06000DB8 RID: 3512 RVA: 0x00049429 File Offset: 0x00047629
		[ComVisible(false)]
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public Task<byte[]> UploadFileTaskAsync(Uri address, string fileName)
		{
			return this.UploadFileTaskAsync(address, null, fileName);
		}

		// Token: 0x06000DB9 RID: 3513 RVA: 0x00049434 File Offset: 0x00047634
		[ComVisible(false)]
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public Task<byte[]> UploadFileTaskAsync(string address, string method, string fileName)
		{
			return this.UploadFileTaskAsync(this.GetUri(address), method, fileName);
		}

		// Token: 0x06000DBA RID: 3514 RVA: 0x00049448 File Offset: 0x00047648
		[ComVisible(false)]
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public Task<byte[]> UploadFileTaskAsync(Uri address, string method, string fileName)
		{
			TaskCompletionSource<byte[]> tcs = new TaskCompletionSource<byte[]>(address);
			UploadFileCompletedEventHandler handler = null;
			handler = delegate(object sender, UploadFileCompletedEventArgs e)
			{
				this.HandleCompletion<UploadFileCompletedEventArgs, UploadFileCompletedEventHandler, byte[]>(tcs, e, (UploadFileCompletedEventArgs args) => args.Result, handler, delegate(WebClient webClient, UploadFileCompletedEventHandler completion)
				{
					webClient.UploadFileCompleted -= completion;
				});
			};
			this.UploadFileCompleted += handler;
			try
			{
				this.UploadFileAsync(address, method, fileName, tcs);
			}
			catch
			{
				this.UploadFileCompleted -= handler;
				throw;
			}
			return tcs.Task;
		}

		// Token: 0x06000DBB RID: 3515 RVA: 0x000494D0 File Offset: 0x000476D0
		[ComVisible(false)]
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public Task<byte[]> UploadValuesTaskAsync(string address, NameValueCollection data)
		{
			return this.UploadValuesTaskAsync(this.GetUri(address), null, data);
		}

		// Token: 0x06000DBC RID: 3516 RVA: 0x000494E1 File Offset: 0x000476E1
		[ComVisible(false)]
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public Task<byte[]> UploadValuesTaskAsync(string address, string method, NameValueCollection data)
		{
			return this.UploadValuesTaskAsync(this.GetUri(address), method, data);
		}

		// Token: 0x06000DBD RID: 3517 RVA: 0x000494F2 File Offset: 0x000476F2
		[ComVisible(false)]
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public Task<byte[]> UploadValuesTaskAsync(Uri address, NameValueCollection data)
		{
			return this.UploadValuesTaskAsync(address, null, data);
		}

		// Token: 0x06000DBE RID: 3518 RVA: 0x00049500 File Offset: 0x00047700
		[ComVisible(false)]
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public Task<byte[]> UploadValuesTaskAsync(Uri address, string method, NameValueCollection data)
		{
			TaskCompletionSource<byte[]> tcs = new TaskCompletionSource<byte[]>(address);
			UploadValuesCompletedEventHandler handler = null;
			handler = delegate(object sender, UploadValuesCompletedEventArgs e)
			{
				this.HandleCompletion<UploadValuesCompletedEventArgs, UploadValuesCompletedEventHandler, byte[]>(tcs, e, (UploadValuesCompletedEventArgs args) => args.Result, handler, delegate(WebClient webClient, UploadValuesCompletedEventHandler completion)
				{
					webClient.UploadValuesCompleted -= completion;
				});
			};
			this.UploadValuesCompleted += handler;
			try
			{
				this.UploadValuesAsync(address, method, data, tcs);
			}
			catch
			{
				this.UploadValuesCompleted -= handler;
				throw;
			}
			return tcs.Task;
		}

		// Token: 0x06000DBF RID: 3519 RVA: 0x00049588 File Offset: 0x00047788
		private void HandleCompletion<TAsyncCompletedEventArgs, TCompletionDelegate, T>(TaskCompletionSource<T> tcs, TAsyncCompletedEventArgs e, Func<TAsyncCompletedEventArgs, T> getResult, TCompletionDelegate handler, Action<WebClient, TCompletionDelegate> unregisterHandler) where TAsyncCompletedEventArgs : AsyncCompletedEventArgs
		{
			if (e.UserState == tcs)
			{
				try
				{
					unregisterHandler(this, handler);
				}
				finally
				{
					if (e.Error != null)
					{
						tcs.TrySetException(e.Error);
					}
					else if (e.Cancelled)
					{
						tcs.TrySetCanceled();
					}
					else
					{
						tcs.TrySetResult(getResult(e));
					}
				}
			}
		}

		// Token: 0x1400001E RID: 30
		// (add) Token: 0x06000DC0 RID: 3520 RVA: 0x00049608 File Offset: 0x00047808
		// (remove) Token: 0x06000DC1 RID: 3521 RVA: 0x00049640 File Offset: 0x00047840
		public event DownloadProgressChangedEventHandler DownloadProgressChanged;

		// Token: 0x1400001F RID: 31
		// (add) Token: 0x06000DC2 RID: 3522 RVA: 0x00049678 File Offset: 0x00047878
		// (remove) Token: 0x06000DC3 RID: 3523 RVA: 0x000496B0 File Offset: 0x000478B0
		public event UploadProgressChangedEventHandler UploadProgressChanged;

		// Token: 0x06000DC4 RID: 3524 RVA: 0x000496E5 File Offset: 0x000478E5
		protected virtual void OnDownloadProgressChanged(DownloadProgressChangedEventArgs e)
		{
			if (this.DownloadProgressChanged != null)
			{
				this.DownloadProgressChanged(this, e);
			}
		}

		// Token: 0x06000DC5 RID: 3525 RVA: 0x000496FC File Offset: 0x000478FC
		protected virtual void OnUploadProgressChanged(UploadProgressChangedEventArgs e)
		{
			if (this.UploadProgressChanged != null)
			{
				this.UploadProgressChanged(this, e);
			}
		}

		// Token: 0x06000DC6 RID: 3526 RVA: 0x00049713 File Offset: 0x00047913
		private void ReportDownloadProgressChanged(object arg)
		{
			this.OnDownloadProgressChanged((DownloadProgressChangedEventArgs)arg);
		}

		// Token: 0x06000DC7 RID: 3527 RVA: 0x00049721 File Offset: 0x00047921
		private void ReportUploadProgressChanged(object arg)
		{
			this.OnUploadProgressChanged((UploadProgressChangedEventArgs)arg);
		}

		// Token: 0x06000DC8 RID: 3528 RVA: 0x00049730 File Offset: 0x00047930
		private void PostProgressChanged(AsyncOperation asyncOp, WebClient.ProgressData progress)
		{
			if (asyncOp != null && progress.BytesSent + progress.BytesReceived > 0L)
			{
				int num;
				if (progress.HasUploadPhase)
				{
					if (progress.TotalBytesToReceive < 0L && progress.BytesReceived == 0L)
					{
						num = ((progress.TotalBytesToSend < 0L) ? 0 : ((progress.TotalBytesToSend == 0L) ? 50 : ((int)(50L * progress.BytesSent / progress.TotalBytesToSend))));
					}
					else
					{
						num = ((progress.TotalBytesToSend < 0L) ? 50 : ((progress.TotalBytesToReceive == 0L) ? 100 : ((int)(50L * progress.BytesReceived / progress.TotalBytesToReceive + 50L))));
					}
					asyncOp.Post(this.reportUploadProgressChanged, new UploadProgressChangedEventArgs(num, asyncOp.UserSuppliedState, progress.BytesSent, progress.TotalBytesToSend, progress.BytesReceived, progress.TotalBytesToReceive));
					return;
				}
				num = ((progress.TotalBytesToReceive < 0L) ? 0 : ((progress.TotalBytesToReceive == 0L) ? 100 : ((int)(100L * progress.BytesReceived / progress.TotalBytesToReceive))));
				asyncOp.Post(this.reportDownloadProgressChanged, new DownloadProgressChangedEventArgs(num, asyncOp.UserSuppliedState, progress.BytesReceived, progress.TotalBytesToReceive));
			}
		}

		// Token: 0x040011E4 RID: 4580
		private const int DefaultCopyBufferLength = 8192;

		// Token: 0x040011E5 RID: 4581
		private const int DefaultDownloadBufferLength = 65536;

		// Token: 0x040011E6 RID: 4582
		private const string DefaultUploadFileContentType = "application/octet-stream";

		// Token: 0x040011E7 RID: 4583
		private const string UploadFileContentType = "multipart/form-data";

		// Token: 0x040011E8 RID: 4584
		private const string UploadValuesContentType = "application/x-www-form-urlencoded";

		// Token: 0x040011E9 RID: 4585
		private Uri m_baseAddress;

		// Token: 0x040011EA RID: 4586
		private ICredentials m_credentials;

		// Token: 0x040011EB RID: 4587
		private WebHeaderCollection m_headers;

		// Token: 0x040011EC RID: 4588
		private NameValueCollection m_requestParameters;

		// Token: 0x040011ED RID: 4589
		private WebResponse m_WebResponse;

		// Token: 0x040011EE RID: 4590
		private WebRequest m_WebRequest;

		// Token: 0x040011EF RID: 4591
		private Encoding m_Encoding = Encoding.Default;

		// Token: 0x040011F0 RID: 4592
		private string m_Method;

		// Token: 0x040011F1 RID: 4593
		private long m_ContentLength = -1L;

		// Token: 0x040011F2 RID: 4594
		private bool m_InitWebClientAsync;

		// Token: 0x040011F3 RID: 4595
		private bool m_Cancelled;

		// Token: 0x040011F4 RID: 4596
		private WebClient.ProgressData m_Progress;

		// Token: 0x040011F5 RID: 4597
		private IWebProxy m_Proxy;

		// Token: 0x040011F6 RID: 4598
		private bool m_ProxySet;

		// Token: 0x040011F7 RID: 4599
		private RequestCachePolicy m_CachePolicy;

		// Token: 0x040011FA RID: 4602
		private int m_CallNesting;

		// Token: 0x040011FB RID: 4603
		private AsyncOperation m_AsyncOp;

		// Token: 0x040011FD RID: 4605
		private SendOrPostCallback openReadOperationCompleted;

		// Token: 0x040011FF RID: 4607
		private SendOrPostCallback openWriteOperationCompleted;

		// Token: 0x04001201 RID: 4609
		private SendOrPostCallback downloadStringOperationCompleted;

		// Token: 0x04001203 RID: 4611
		private SendOrPostCallback downloadDataOperationCompleted;

		// Token: 0x04001205 RID: 4613
		private SendOrPostCallback downloadFileOperationCompleted;

		// Token: 0x04001207 RID: 4615
		private SendOrPostCallback uploadStringOperationCompleted;

		// Token: 0x04001209 RID: 4617
		private SendOrPostCallback uploadDataOperationCompleted;

		// Token: 0x0400120B RID: 4619
		private SendOrPostCallback uploadFileOperationCompleted;

		// Token: 0x0400120D RID: 4621
		private SendOrPostCallback uploadValuesOperationCompleted;

		// Token: 0x04001210 RID: 4624
		private SendOrPostCallback reportDownloadProgressChanged;

		// Token: 0x04001211 RID: 4625
		private SendOrPostCallback reportUploadProgressChanged;

		// Token: 0x02000725 RID: 1829
		private class ProgressData
		{
			// Token: 0x06004154 RID: 16724 RVA: 0x0010FD6A File Offset: 0x0010DF6A
			internal void Reset()
			{
				this.BytesSent = 0L;
				this.TotalBytesToSend = -1L;
				this.BytesReceived = 0L;
				this.TotalBytesToReceive = -1L;
				this.HasUploadPhase = false;
			}

			// Token: 0x0400314F RID: 12623
			internal long BytesSent;

			// Token: 0x04003150 RID: 12624
			internal long TotalBytesToSend = -1L;

			// Token: 0x04003151 RID: 12625
			internal long BytesReceived;

			// Token: 0x04003152 RID: 12626
			internal long TotalBytesToReceive = -1L;

			// Token: 0x04003153 RID: 12627
			internal bool HasUploadPhase;
		}

		// Token: 0x02000726 RID: 1830
		private class DownloadBitsState
		{
			// Token: 0x06004156 RID: 16726 RVA: 0x0010FDAB File Offset: 0x0010DFAB
			internal DownloadBitsState(WebRequest request, Stream writeStream, CompletionDelegate completionDelegate, AsyncOperation asyncOp, WebClient.ProgressData progress, WebClient webClient)
			{
				this.WriteStream = writeStream;
				this.Request = request;
				this.AsyncOp = asyncOp;
				this.CompletionDelegate = completionDelegate;
				this.WebClient = webClient;
				this.Progress = progress;
			}

			// Token: 0x17000EF7 RID: 3831
			// (get) Token: 0x06004157 RID: 16727 RVA: 0x0010FDE0 File Offset: 0x0010DFE0
			internal bool Async
			{
				get
				{
					return this.AsyncOp != null;
				}
			}

			// Token: 0x06004158 RID: 16728 RVA: 0x0010FDEC File Offset: 0x0010DFEC
			internal int SetResponse(WebResponse response)
			{
				this.ContentLength = response.ContentLength;
				if (this.ContentLength == -1L || this.ContentLength > 65536L)
				{
					this.Length = 65536L;
				}
				else
				{
					this.Length = this.ContentLength;
				}
				if (this.WriteStream == null)
				{
					if (this.ContentLength > 2147483647L)
					{
						throw new WebException(SR.GetString("net_webstatus_MessageLengthLimitExceeded"), WebExceptionStatus.MessageLengthLimitExceeded);
					}
					this.SgBuffers = new ScatterGatherBuffers(this.Length);
				}
				this.InnerBuffer = new byte[(int)this.Length];
				this.ReadStream = response.GetResponseStream();
				if (this.Async && response.ContentLength >= 0L)
				{
					this.Progress.TotalBytesToReceive = response.ContentLength;
				}
				if (this.Async)
				{
					if (this.ReadStream == null || this.ReadStream == Stream.Null)
					{
						WebClient.DownloadBitsReadCallbackState(this, null);
					}
					else
					{
						this.ReadStream.BeginRead(this.InnerBuffer, this.Offset, (int)this.Length - this.Offset, new AsyncCallback(WebClient.DownloadBitsReadCallback), this);
					}
					return -1;
				}
				if (this.ReadStream == null || this.ReadStream == Stream.Null)
				{
					return 0;
				}
				return this.ReadStream.Read(this.InnerBuffer, this.Offset, (int)this.Length - this.Offset);
			}

			// Token: 0x06004159 RID: 16729 RVA: 0x0010FF4C File Offset: 0x0010E14C
			internal bool RetrieveBytes(ref int bytesRetrieved)
			{
				if (bytesRetrieved > 0)
				{
					if (this.WriteStream != null)
					{
						this.WriteStream.Write(this.InnerBuffer, 0, bytesRetrieved);
					}
					else
					{
						this.SgBuffers.Write(this.InnerBuffer, 0, bytesRetrieved);
					}
					if (this.Async)
					{
						this.Progress.BytesReceived += (long)bytesRetrieved;
					}
					if ((long)this.Offset != this.ContentLength)
					{
						if (this.Async)
						{
							this.WebClient.PostProgressChanged(this.AsyncOp, this.Progress);
							this.ReadStream.BeginRead(this.InnerBuffer, this.Offset, (int)this.Length - this.Offset, new AsyncCallback(WebClient.DownloadBitsReadCallback), this);
						}
						else
						{
							bytesRetrieved = this.ReadStream.Read(this.InnerBuffer, this.Offset, (int)this.Length - this.Offset);
						}
						return false;
					}
				}
				if (this.Async)
				{
					if (this.Progress.TotalBytesToReceive < 0L)
					{
						this.Progress.TotalBytesToReceive = this.Progress.BytesReceived;
					}
					this.WebClient.PostProgressChanged(this.AsyncOp, this.Progress);
				}
				if (this.ReadStream != null)
				{
					this.ReadStream.Close();
				}
				if (this.WriteStream != null)
				{
					this.WriteStream.Close();
				}
				else if (this.WriteStream == null)
				{
					byte[] array = new byte[this.SgBuffers.Length];
					if (this.SgBuffers.Length > 0)
					{
						BufferOffsetSize[] buffers = this.SgBuffers.GetBuffers();
						int num = 0;
						foreach (BufferOffsetSize bufferOffsetSize in buffers)
						{
							Buffer.BlockCopy(bufferOffsetSize.Buffer, 0, array, num, bufferOffsetSize.Size);
							num += bufferOffsetSize.Size;
						}
					}
					this.InnerBuffer = array;
				}
				return true;
			}

			// Token: 0x0600415A RID: 16730 RVA: 0x00110118 File Offset: 0x0010E318
			internal void Close()
			{
				if (this.WriteStream != null)
				{
					this.WriteStream.Close();
				}
				if (this.ReadStream != null)
				{
					this.ReadStream.Close();
				}
			}

			// Token: 0x04003154 RID: 12628
			internal WebClient WebClient;

			// Token: 0x04003155 RID: 12629
			internal Stream WriteStream;

			// Token: 0x04003156 RID: 12630
			internal byte[] InnerBuffer;

			// Token: 0x04003157 RID: 12631
			internal AsyncOperation AsyncOp;

			// Token: 0x04003158 RID: 12632
			internal WebRequest Request;

			// Token: 0x04003159 RID: 12633
			internal CompletionDelegate CompletionDelegate;

			// Token: 0x0400315A RID: 12634
			internal Stream ReadStream;

			// Token: 0x0400315B RID: 12635
			internal ScatterGatherBuffers SgBuffers;

			// Token: 0x0400315C RID: 12636
			internal long ContentLength;

			// Token: 0x0400315D RID: 12637
			internal long Length;

			// Token: 0x0400315E RID: 12638
			internal int Offset;

			// Token: 0x0400315F RID: 12639
			internal WebClient.ProgressData Progress;
		}

		// Token: 0x02000727 RID: 1831
		private class UploadBitsState
		{
			// Token: 0x0600415B RID: 16731 RVA: 0x00110140 File Offset: 0x0010E340
			internal UploadBitsState(WebRequest request, Stream readStream, byte[] buffer, int chunkSize, byte[] header, byte[] footer, CompletionDelegate uploadCompletionDelegate, CompletionDelegate downloadCompletionDelegate, AsyncOperation asyncOp, WebClient.ProgressData progress, WebClient webClient)
			{
				this.InnerBuffer = buffer;
				this.m_ChunkSize = chunkSize;
				this.m_BufferWritePosition = 0;
				this.Header = header;
				this.Footer = footer;
				this.ReadStream = readStream;
				this.Request = request;
				this.AsyncOp = asyncOp;
				this.UploadCompletionDelegate = uploadCompletionDelegate;
				this.DownloadCompletionDelegate = downloadCompletionDelegate;
				if (this.AsyncOp != null)
				{
					this.Progress = progress;
					this.Progress.HasUploadPhase = true;
					this.Progress.TotalBytesToSend = ((request.ContentLength < 0L) ? (-1L) : request.ContentLength);
				}
				this.WebClient = webClient;
			}

			// Token: 0x17000EF8 RID: 3832
			// (get) Token: 0x0600415C RID: 16732 RVA: 0x001101E2 File Offset: 0x0010E3E2
			internal bool FileUpload
			{
				get
				{
					return this.ReadStream != null;
				}
			}

			// Token: 0x17000EF9 RID: 3833
			// (get) Token: 0x0600415D RID: 16733 RVA: 0x001101ED File Offset: 0x0010E3ED
			internal bool Async
			{
				get
				{
					return this.AsyncOp != null;
				}
			}

			// Token: 0x0600415E RID: 16734 RVA: 0x001101F8 File Offset: 0x0010E3F8
			internal void SetRequestStream(Stream writeStream)
			{
				this.WriteStream = writeStream;
				byte[] array;
				if (this.Header != null)
				{
					array = this.Header;
					this.Header = null;
				}
				else
				{
					array = new byte[0];
				}
				if (this.Async)
				{
					this.Progress.BytesSent += (long)array.Length;
					this.WriteStream.BeginWrite(array, 0, array.Length, new AsyncCallback(WebClient.UploadBitsWriteCallback), this);
					return;
				}
				this.WriteStream.Write(array, 0, array.Length);
			}

			// Token: 0x0600415F RID: 16735 RVA: 0x0011027C File Offset: 0x0010E47C
			internal bool WriteBytes()
			{
				int num = 0;
				if (this.Async)
				{
					this.WebClient.PostProgressChanged(this.AsyncOp, this.Progress);
				}
				int num3;
				byte[] array;
				if (this.FileUpload)
				{
					int num2 = 0;
					if (this.InnerBuffer != null)
					{
						num2 = this.ReadStream.Read(this.InnerBuffer, 0, this.InnerBuffer.Length);
						if (num2 <= 0)
						{
							this.ReadStream.Close();
							this.InnerBuffer = null;
						}
					}
					if (this.InnerBuffer != null)
					{
						num3 = num2;
						array = this.InnerBuffer;
					}
					else
					{
						if (this.Footer == null)
						{
							return true;
						}
						num3 = this.Footer.Length;
						array = this.Footer;
						this.Footer = null;
					}
				}
				else
				{
					if (this.InnerBuffer == null)
					{
						return true;
					}
					array = this.InnerBuffer;
					if (this.m_ChunkSize != 0)
					{
						num = this.m_BufferWritePosition;
						this.m_BufferWritePosition += this.m_ChunkSize;
						num3 = this.m_ChunkSize;
						if (this.m_BufferWritePosition >= this.InnerBuffer.Length)
						{
							num3 = this.InnerBuffer.Length - num;
							this.InnerBuffer = null;
						}
					}
					else
					{
						num3 = this.InnerBuffer.Length;
						this.InnerBuffer = null;
					}
				}
				if (this.Async)
				{
					this.Progress.BytesSent += (long)num3;
					this.WriteStream.BeginWrite(array, num, num3, new AsyncCallback(WebClient.UploadBitsWriteCallback), this);
				}
				else
				{
					this.WriteStream.Write(array, 0, num3);
				}
				return false;
			}

			// Token: 0x06004160 RID: 16736 RVA: 0x001103E3 File Offset: 0x0010E5E3
			internal void Close()
			{
				if (this.WriteStream != null)
				{
					this.WriteStream.Close();
				}
				if (this.ReadStream != null)
				{
					this.ReadStream.Close();
				}
			}

			// Token: 0x04003160 RID: 12640
			private int m_ChunkSize;

			// Token: 0x04003161 RID: 12641
			private int m_BufferWritePosition;

			// Token: 0x04003162 RID: 12642
			internal WebClient WebClient;

			// Token: 0x04003163 RID: 12643
			internal Stream WriteStream;

			// Token: 0x04003164 RID: 12644
			internal byte[] InnerBuffer;

			// Token: 0x04003165 RID: 12645
			internal byte[] Header;

			// Token: 0x04003166 RID: 12646
			internal byte[] Footer;

			// Token: 0x04003167 RID: 12647
			internal AsyncOperation AsyncOp;

			// Token: 0x04003168 RID: 12648
			internal WebRequest Request;

			// Token: 0x04003169 RID: 12649
			internal CompletionDelegate UploadCompletionDelegate;

			// Token: 0x0400316A RID: 12650
			internal CompletionDelegate DownloadCompletionDelegate;

			// Token: 0x0400316B RID: 12651
			internal Stream ReadStream;

			// Token: 0x0400316C RID: 12652
			internal long Length;

			// Token: 0x0400316D RID: 12653
			internal int Offset;

			// Token: 0x0400316E RID: 12654
			internal WebClient.ProgressData Progress;
		}

		// Token: 0x02000728 RID: 1832
		private class WebClientWriteStream : Stream
		{
			// Token: 0x06004161 RID: 16737 RVA: 0x0011040B File Offset: 0x0010E60B
			public WebClientWriteStream(Stream stream, WebRequest request, WebClient webClient)
			{
				this.m_request = request;
				this.m_stream = stream;
				this.m_WebClient = webClient;
			}

			// Token: 0x17000EFA RID: 3834
			// (get) Token: 0x06004162 RID: 16738 RVA: 0x00110428 File Offset: 0x0010E628
			public override bool CanRead
			{
				get
				{
					return this.m_stream.CanRead;
				}
			}

			// Token: 0x17000EFB RID: 3835
			// (get) Token: 0x06004163 RID: 16739 RVA: 0x00110435 File Offset: 0x0010E635
			public override bool CanSeek
			{
				get
				{
					return this.m_stream.CanSeek;
				}
			}

			// Token: 0x17000EFC RID: 3836
			// (get) Token: 0x06004164 RID: 16740 RVA: 0x00110442 File Offset: 0x0010E642
			public override bool CanWrite
			{
				get
				{
					return this.m_stream.CanWrite;
				}
			}

			// Token: 0x17000EFD RID: 3837
			// (get) Token: 0x06004165 RID: 16741 RVA: 0x0011044F File Offset: 0x0010E64F
			public override bool CanTimeout
			{
				get
				{
					return this.m_stream.CanTimeout;
				}
			}

			// Token: 0x17000EFE RID: 3838
			// (get) Token: 0x06004166 RID: 16742 RVA: 0x0011045C File Offset: 0x0010E65C
			// (set) Token: 0x06004167 RID: 16743 RVA: 0x00110469 File Offset: 0x0010E669
			public override int ReadTimeout
			{
				get
				{
					return this.m_stream.ReadTimeout;
				}
				set
				{
					this.m_stream.ReadTimeout = value;
				}
			}

			// Token: 0x17000EFF RID: 3839
			// (get) Token: 0x06004168 RID: 16744 RVA: 0x00110477 File Offset: 0x0010E677
			// (set) Token: 0x06004169 RID: 16745 RVA: 0x00110484 File Offset: 0x0010E684
			public override int WriteTimeout
			{
				get
				{
					return this.m_stream.WriteTimeout;
				}
				set
				{
					this.m_stream.WriteTimeout = value;
				}
			}

			// Token: 0x17000F00 RID: 3840
			// (get) Token: 0x0600416A RID: 16746 RVA: 0x00110492 File Offset: 0x0010E692
			public override long Length
			{
				get
				{
					return this.m_stream.Length;
				}
			}

			// Token: 0x17000F01 RID: 3841
			// (get) Token: 0x0600416B RID: 16747 RVA: 0x0011049F File Offset: 0x0010E69F
			// (set) Token: 0x0600416C RID: 16748 RVA: 0x001104AC File Offset: 0x0010E6AC
			public override long Position
			{
				get
				{
					return this.m_stream.Position;
				}
				set
				{
					this.m_stream.Position = value;
				}
			}

			// Token: 0x0600416D RID: 16749 RVA: 0x001104BA File Offset: 0x0010E6BA
			[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
			public override IAsyncResult BeginRead(byte[] buffer, int offset, int size, AsyncCallback callback, object state)
			{
				return this.m_stream.BeginRead(buffer, offset, size, callback, state);
			}

			// Token: 0x0600416E RID: 16750 RVA: 0x001104CE File Offset: 0x0010E6CE
			[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
			public override IAsyncResult BeginWrite(byte[] buffer, int offset, int size, AsyncCallback callback, object state)
			{
				return this.m_stream.BeginWrite(buffer, offset, size, callback, state);
			}

			// Token: 0x0600416F RID: 16751 RVA: 0x001104E4 File Offset: 0x0010E6E4
			protected override void Dispose(bool disposing)
			{
				try
				{
					if (disposing)
					{
						this.m_stream.Close();
						this.m_WebClient.GetWebResponse(this.m_request).Close();
					}
				}
				finally
				{
					base.Dispose(disposing);
				}
			}

			// Token: 0x06004170 RID: 16752 RVA: 0x00110530 File Offset: 0x0010E730
			public override int EndRead(IAsyncResult result)
			{
				return this.m_stream.EndRead(result);
			}

			// Token: 0x06004171 RID: 16753 RVA: 0x0011053E File Offset: 0x0010E73E
			public override void EndWrite(IAsyncResult result)
			{
				this.m_stream.EndWrite(result);
			}

			// Token: 0x06004172 RID: 16754 RVA: 0x0011054C File Offset: 0x0010E74C
			public override void Flush()
			{
				this.m_stream.Flush();
			}

			// Token: 0x06004173 RID: 16755 RVA: 0x00110559 File Offset: 0x0010E759
			public override int Read(byte[] buffer, int offset, int count)
			{
				return this.m_stream.Read(buffer, offset, count);
			}

			// Token: 0x06004174 RID: 16756 RVA: 0x00110569 File Offset: 0x0010E769
			public override long Seek(long offset, SeekOrigin origin)
			{
				return this.m_stream.Seek(offset, origin);
			}

			// Token: 0x06004175 RID: 16757 RVA: 0x00110578 File Offset: 0x0010E778
			public override void SetLength(long value)
			{
				this.m_stream.SetLength(value);
			}

			// Token: 0x06004176 RID: 16758 RVA: 0x00110586 File Offset: 0x0010E786
			public override void Write(byte[] buffer, int offset, int count)
			{
				this.m_stream.Write(buffer, offset, count);
			}

			// Token: 0x0400316F RID: 12655
			private WebRequest m_request;

			// Token: 0x04003170 RID: 12656
			private Stream m_stream;

			// Token: 0x04003171 RID: 12657
			private WebClient m_WebClient;
		}
	}
}
