using System;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Net.WebSockets;
using System.Runtime.Serialization;
using System.Security.Cryptography.X509Certificates;
using System.Security.Permissions;

namespace System.Net
{
	// Token: 0x0200010B RID: 267
	[global::__DynamicallyInvokable]
	[Serializable]
	public class HttpWebResponse : WebResponse, ISerializable
	{
		// Token: 0x1700026E RID: 622
		// (get) Token: 0x06000AC3 RID: 2755 RVA: 0x0003BD06 File Offset: 0x00039F06
		// (set) Token: 0x06000AC4 RID: 2756 RVA: 0x0003BD0E File Offset: 0x00039F0E
		internal bool IsWebSocketResponse
		{
			get
			{
				return this.m_IsWebSocketResponse;
			}
			set
			{
				this.m_IsWebSocketResponse = value;
			}
		}

		// Token: 0x1700026F RID: 623
		// (get) Token: 0x06000AC5 RID: 2757 RVA: 0x0003BD17 File Offset: 0x00039F17
		// (set) Token: 0x06000AC6 RID: 2758 RVA: 0x0003BD1F File Offset: 0x00039F1F
		internal string ConnectionGroupName
		{
			get
			{
				return this.m_ConnectionGroupName;
			}
			set
			{
				this.m_ConnectionGroupName = value;
			}
		}

		// Token: 0x17000270 RID: 624
		// (get) Token: 0x06000AC7 RID: 2759 RVA: 0x0003BD28 File Offset: 0x00039F28
		// (set) Token: 0x06000AC8 RID: 2760 RVA: 0x0003BD30 File Offset: 0x00039F30
		internal Stream ResponseStream
		{
			get
			{
				return this.m_ConnectStream;
			}
			set
			{
				this.m_ConnectStream = value;
			}
		}

		// Token: 0x17000271 RID: 625
		// (get) Token: 0x06000AC9 RID: 2761 RVA: 0x0003BD39 File Offset: 0x00039F39
		internal CoreResponseData CoreResponseData
		{
			get
			{
				return this.m_CoreResponseData;
			}
		}

		// Token: 0x17000272 RID: 626
		// (get) Token: 0x06000ACA RID: 2762 RVA: 0x0003BD41 File Offset: 0x00039F41
		public override bool IsMutuallyAuthenticated
		{
			get
			{
				this.CheckDisposed();
				return this.m_IsMutuallyAuthenticated;
			}
		}

		// Token: 0x17000273 RID: 627
		// (set) Token: 0x06000ACB RID: 2763 RVA: 0x0003BD4F File Offset: 0x00039F4F
		internal bool InternalSetIsMutuallyAuthenticated
		{
			set
			{
				this.m_IsMutuallyAuthenticated = value;
			}
		}

		// Token: 0x17000274 RID: 628
		// (get) Token: 0x06000ACC RID: 2764 RVA: 0x0003BD58 File Offset: 0x00039F58
		// (set) Token: 0x06000ACD RID: 2765 RVA: 0x0003BD79 File Offset: 0x00039F79
		[global::__DynamicallyInvokable]
		public virtual CookieCollection Cookies
		{
			[global::__DynamicallyInvokable]
			get
			{
				this.CheckDisposed();
				if (this.m_cookies == null)
				{
					this.m_cookies = new CookieCollection();
				}
				return this.m_cookies;
			}
			set
			{
				this.CheckDisposed();
				this.m_cookies = value;
			}
		}

		// Token: 0x17000275 RID: 629
		// (get) Token: 0x06000ACE RID: 2766 RVA: 0x0003BD88 File Offset: 0x00039F88
		[global::__DynamicallyInvokable]
		public override WebHeaderCollection Headers
		{
			[global::__DynamicallyInvokable]
			get
			{
				this.CheckDisposed();
				return this.m_HttpResponseHeaders;
			}
		}

		// Token: 0x17000276 RID: 630
		// (get) Token: 0x06000ACF RID: 2767 RVA: 0x0003BD96 File Offset: 0x00039F96
		[global::__DynamicallyInvokable]
		public override bool SupportsHeaders
		{
			[global::__DynamicallyInvokable]
			get
			{
				return true;
			}
		}

		// Token: 0x17000277 RID: 631
		// (get) Token: 0x06000AD0 RID: 2768 RVA: 0x0003BD99 File Offset: 0x00039F99
		[global::__DynamicallyInvokable]
		public override long ContentLength
		{
			[global::__DynamicallyInvokable]
			get
			{
				this.CheckDisposed();
				return this.m_ContentLength;
			}
		}

		// Token: 0x17000278 RID: 632
		// (get) Token: 0x06000AD1 RID: 2769 RVA: 0x0003BDA8 File Offset: 0x00039FA8
		public string ContentEncoding
		{
			get
			{
				this.CheckDisposed();
				string text = this.m_HttpResponseHeaders["Content-Encoding"];
				if (text != null)
				{
					return text;
				}
				return string.Empty;
			}
		}

		// Token: 0x17000279 RID: 633
		// (get) Token: 0x06000AD2 RID: 2770 RVA: 0x0003BDD8 File Offset: 0x00039FD8
		[global::__DynamicallyInvokable]
		public override string ContentType
		{
			[global::__DynamicallyInvokable]
			get
			{
				this.CheckDisposed();
				string contentType = this.m_HttpResponseHeaders.ContentType;
				if (contentType != null)
				{
					return contentType;
				}
				return string.Empty;
			}
		}

		// Token: 0x1700027A RID: 634
		// (get) Token: 0x06000AD3 RID: 2771 RVA: 0x0003BE04 File Offset: 0x0003A004
		public string CharacterSet
		{
			get
			{
				this.CheckDisposed();
				string contentType = this.m_HttpResponseHeaders.ContentType;
				if (this.m_CharacterSet == null && !ValidationHelper.IsBlankString(contentType))
				{
					this.m_CharacterSet = string.Empty;
					string text = contentType.ToLower(CultureInfo.InvariantCulture);
					if (text.Trim().StartsWith("text/"))
					{
						this.m_CharacterSet = "ISO-8859-1";
					}
					int num = text.IndexOf(";");
					if (num > 0)
					{
						while ((num = text.IndexOf("charset", num)) >= 0)
						{
							num += 7;
							if (text[num - 8] != ';')
							{
								if (text[num - 8] != ' ')
								{
									continue;
								}
							}
							while (num < text.Length && text[num] == ' ')
							{
								num++;
							}
							if (num < text.Length - 1 && text[num] == '=')
							{
								num++;
								int num2 = text.IndexOf(';', num);
								if (num2 > num)
								{
									this.m_CharacterSet = contentType.Substring(num, num2 - num).Trim();
									break;
								}
								this.m_CharacterSet = contentType.Substring(num).Trim();
								break;
							}
						}
					}
				}
				return this.m_CharacterSet;
			}
		}

		// Token: 0x1700027B RID: 635
		// (get) Token: 0x06000AD4 RID: 2772 RVA: 0x0003BF2C File Offset: 0x0003A12C
		public string Server
		{
			get
			{
				this.CheckDisposed();
				string server = this.m_HttpResponseHeaders.Server;
				if (server != null)
				{
					return server;
				}
				return string.Empty;
			}
		}

		// Token: 0x1700027C RID: 636
		// (get) Token: 0x06000AD5 RID: 2773 RVA: 0x0003BF58 File Offset: 0x0003A158
		public DateTime LastModified
		{
			get
			{
				this.CheckDisposed();
				string lastModified = this.m_HttpResponseHeaders.LastModified;
				if (lastModified == null)
				{
					return DateTime.Now;
				}
				return HttpProtocolUtils.string2date(lastModified);
			}
		}

		// Token: 0x1700027D RID: 637
		// (get) Token: 0x06000AD6 RID: 2774 RVA: 0x0003BF86 File Offset: 0x0003A186
		[global::__DynamicallyInvokable]
		public virtual HttpStatusCode StatusCode
		{
			[global::__DynamicallyInvokable]
			get
			{
				this.CheckDisposed();
				return this.m_StatusCode;
			}
		}

		// Token: 0x1700027E RID: 638
		// (get) Token: 0x06000AD7 RID: 2775 RVA: 0x0003BF94 File Offset: 0x0003A194
		[global::__DynamicallyInvokable]
		public virtual string StatusDescription
		{
			[global::__DynamicallyInvokable]
			get
			{
				this.CheckDisposed();
				return this.m_StatusDescription;
			}
		}

		// Token: 0x1700027F RID: 639
		// (get) Token: 0x06000AD8 RID: 2776 RVA: 0x0003BFA2 File Offset: 0x0003A1A2
		public Version ProtocolVersion
		{
			get
			{
				this.CheckDisposed();
				if (!this.m_IsVersionHttp11)
				{
					return HttpVersion.Version10;
				}
				return HttpVersion.Version11;
			}
		}

		// Token: 0x17000280 RID: 640
		// (get) Token: 0x06000AD9 RID: 2777 RVA: 0x0003BFC0 File Offset: 0x0003A1C0
		internal bool KeepAlive
		{
			get
			{
				if (this.m_UsesProxySemantics)
				{
					string text = this.Headers["Proxy-Connection"];
					if (text != null)
					{
						return text.ToLower(CultureInfo.InvariantCulture).IndexOf("close") < 0 || text.ToLower(CultureInfo.InvariantCulture).IndexOf("keep-alive") >= 0;
					}
				}
				string text2 = this.Headers["Connection"];
				if (text2 != null)
				{
					text2 = text2.ToLower(CultureInfo.InvariantCulture);
				}
				if (this.ProtocolVersion == HttpVersion.Version10)
				{
					return text2 != null && text2.IndexOf("keep-alive") >= 0;
				}
				return this.ProtocolVersion >= HttpVersion.Version11 && (text2 == null || text2.IndexOf("close") < 0 || text2.IndexOf("keep-alive") >= 0);
			}
		}

		// Token: 0x06000ADA RID: 2778 RVA: 0x0003C0A0 File Offset: 0x0003A2A0
		[global::__DynamicallyInvokable]
		public override Stream GetResponseStream()
		{
			if (Logging.On)
			{
				Logging.Enter(Logging.Web, this, "GetResponseStream", "");
			}
			this.CheckDisposed();
			if (Logging.On)
			{
				Logging.PrintInfo(Logging.Web, "ContentLength=" + this.m_ContentLength.ToString());
			}
			Stream stream;
			if (this.m_IsWebSocketResponse && this.m_StatusCode == HttpStatusCode.SwitchingProtocols)
			{
				if (this.m_WebSocketConnectionStream == null)
				{
					ConnectStream connectStream = this.m_ConnectStream as ConnectStream;
					this.m_WebSocketConnectionStream = new WebSocketConnectionStream(connectStream, this.ConnectionGroupName);
				}
				stream = this.m_WebSocketConnectionStream;
			}
			else
			{
				stream = this.m_ConnectStream;
			}
			if (Logging.On)
			{
				Logging.Exit(Logging.Web, this, "GetResponseStream", stream);
			}
			return stream;
		}

		// Token: 0x06000ADB RID: 2779 RVA: 0x0003C158 File Offset: 0x0003A358
		public override void Close()
		{
			if (Logging.On)
			{
				Logging.Enter(Logging.Web, this, "Close", "");
			}
			if (!this.m_disposed)
			{
				this.m_disposed = true;
				try
				{
					Stream connectStream = this.m_ConnectStream;
					ICloseEx closeEx = connectStream as ICloseEx;
					if (closeEx != null)
					{
						closeEx.CloseEx(CloseExState.Normal);
					}
					else if (connectStream != null)
					{
						connectStream.Close();
					}
				}
				finally
				{
					if (this.IsWebSocketResponse)
					{
						ConnectStream connectStream2 = this.m_ConnectStream as ConnectStream;
						if (connectStream2 != null && connectStream2.Connection != null)
						{
							connectStream2.Connection.ServicePoint.CloseConnectionGroup(this.ConnectionGroupName);
						}
					}
				}
			}
			if (Logging.On)
			{
				Logging.Exit(Logging.Web, this, "Close", "");
			}
		}

		// Token: 0x06000ADC RID: 2780 RVA: 0x0003C218 File Offset: 0x0003A418
		internal void Abort()
		{
			Stream connectStream = this.m_ConnectStream;
			ICloseEx closeEx = connectStream as ICloseEx;
			try
			{
				if (closeEx != null)
				{
					closeEx.CloseEx(CloseExState.Abort);
				}
				else if (connectStream != null)
				{
					connectStream.Close();
				}
			}
			catch
			{
			}
		}

		// Token: 0x06000ADD RID: 2781 RVA: 0x0003C260 File Offset: 0x0003A460
		[global::__DynamicallyInvokable]
		protected override void Dispose(bool disposing)
		{
			if (!disposing)
			{
				return;
			}
			base.Dispose(true);
			this.m_propertiesDisposed = true;
		}

		// Token: 0x06000ADE RID: 2782 RVA: 0x0003C274 File Offset: 0x0003A474
		[Obsolete("This API supports the .NET Framework infrastructure and is not intended to be used directly from your code.", true)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public HttpWebResponse()
		{
		}

		// Token: 0x06000ADF RID: 2783 RVA: 0x0003C27C File Offset: 0x0003A47C
		internal HttpWebResponse(Uri responseUri, KnownHttpVerb verb, CoreResponseData coreData, string mediaType, bool usesProxySemantics, DecompressionMethods decompressionMethod, bool isWebSocketResponse, string connectionGroupName)
		{
			this.m_Uri = responseUri;
			this.m_Verb = verb;
			this.m_MediaType = mediaType;
			this.m_UsesProxySemantics = usesProxySemantics;
			this.m_CoreResponseData = coreData;
			this.m_ConnectStream = coreData.m_ConnectStream;
			this.m_HttpResponseHeaders = coreData.m_ResponseHeaders;
			this.m_ContentLength = coreData.m_ContentLength;
			this.m_StatusCode = coreData.m_StatusCode;
			this.m_StatusDescription = coreData.m_StatusDescription;
			this.m_IsVersionHttp11 = coreData.m_IsVersionHttp11;
			this.m_IsWebSocketResponse = isWebSocketResponse;
			this.m_ConnectionGroupName = connectionGroupName;
			if (this.m_ContentLength == 0L && this.m_ConnectStream is ConnectStream)
			{
				((ConnectStream)this.m_ConnectStream).CallDone();
			}
			string text = this.m_HttpResponseHeaders["Content-Location"];
			if (text != null)
			{
				try
				{
					this.m_Uri = new Uri(this.m_Uri, text);
				}
				catch (UriFormatException ex)
				{
				}
			}
			if (decompressionMethod != DecompressionMethods.None)
			{
				string text2 = this.m_HttpResponseHeaders["Content-Encoding"];
				if (text2 != null)
				{
					if ((decompressionMethod & DecompressionMethods.GZip) != DecompressionMethods.None && text2.IndexOf("gzip", StringComparison.CurrentCulture) != -1)
					{
						this.m_ConnectStream = new GZipWrapperStream(this.m_ConnectStream, CompressionMode.Decompress);
						this.m_ContentLength = -1L;
						this.m_HttpResponseHeaders["Content-Encoding"] = null;
						return;
					}
					if ((decompressionMethod & DecompressionMethods.Deflate) != DecompressionMethods.None && text2.IndexOf("deflate", StringComparison.CurrentCulture) != -1)
					{
						this.m_ConnectStream = new DeflateWrapperStream(this.m_ConnectStream, CompressionMode.Decompress);
						this.m_ContentLength = -1L;
						this.m_HttpResponseHeaders["Content-Encoding"] = null;
					}
				}
			}
		}

		// Token: 0x06000AE0 RID: 2784 RVA: 0x0003C40C File Offset: 0x0003A60C
		[Obsolete("Serialization is obsoleted for this type.  http://go.microsoft.com/fwlink/?linkid=14202")]
		protected HttpWebResponse(SerializationInfo serializationInfo, StreamingContext streamingContext)
			: base(serializationInfo, streamingContext)
		{
			this.m_HttpResponseHeaders = (WebHeaderCollection)serializationInfo.GetValue("m_HttpResponseHeaders", typeof(WebHeaderCollection));
			this.m_Uri = (Uri)serializationInfo.GetValue("m_Uri", typeof(Uri));
			this.m_Certificate = (X509Certificate)serializationInfo.GetValue("m_Certificate", typeof(X509Certificate));
			Version version = (Version)serializationInfo.GetValue("m_Version", typeof(Version));
			this.m_IsVersionHttp11 = version.Equals(HttpVersion.Version11);
			this.m_StatusCode = (HttpStatusCode)serializationInfo.GetInt32("m_StatusCode");
			this.m_ContentLength = serializationInfo.GetInt64("m_ContentLength");
			this.m_Verb = KnownHttpVerb.Parse(serializationInfo.GetString("m_Verb"));
			this.m_StatusDescription = serializationInfo.GetString("m_StatusDescription");
			this.m_MediaType = serializationInfo.GetString("m_MediaType");
		}

		// Token: 0x06000AE1 RID: 2785 RVA: 0x0003C507 File Offset: 0x0003A707
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter, SerializationFormatter = true)]
		void ISerializable.GetObjectData(SerializationInfo serializationInfo, StreamingContext streamingContext)
		{
			this.GetObjectData(serializationInfo, streamingContext);
		}

		// Token: 0x06000AE2 RID: 2786 RVA: 0x0003C514 File Offset: 0x0003A714
		[SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
		protected override void GetObjectData(SerializationInfo serializationInfo, StreamingContext streamingContext)
		{
			serializationInfo.AddValue("m_HttpResponseHeaders", this.m_HttpResponseHeaders, typeof(WebHeaderCollection));
			serializationInfo.AddValue("m_Uri", this.m_Uri, typeof(Uri));
			serializationInfo.AddValue("m_Certificate", this.m_Certificate, typeof(X509Certificate));
			serializationInfo.AddValue("m_Version", this.ProtocolVersion, typeof(Version));
			serializationInfo.AddValue("m_StatusCode", this.m_StatusCode);
			serializationInfo.AddValue("m_ContentLength", this.m_ContentLength);
			serializationInfo.AddValue("m_Verb", this.m_Verb.Name);
			serializationInfo.AddValue("m_StatusDescription", this.m_StatusDescription);
			serializationInfo.AddValue("m_MediaType", this.m_MediaType);
			base.GetObjectData(serializationInfo, streamingContext);
		}

		// Token: 0x06000AE3 RID: 2787 RVA: 0x0003C5F4 File Offset: 0x0003A7F4
		public string GetResponseHeader(string headerName)
		{
			this.CheckDisposed();
			string text = this.m_HttpResponseHeaders[headerName];
			if (text != null)
			{
				return text;
			}
			return string.Empty;
		}

		// Token: 0x17000281 RID: 641
		// (get) Token: 0x06000AE4 RID: 2788 RVA: 0x0003C61E File Offset: 0x0003A81E
		[global::__DynamicallyInvokable]
		public override Uri ResponseUri
		{
			[global::__DynamicallyInvokable]
			get
			{
				this.CheckDisposed();
				return this.m_Uri;
			}
		}

		// Token: 0x17000282 RID: 642
		// (get) Token: 0x06000AE5 RID: 2789 RVA: 0x0003C62C File Offset: 0x0003A82C
		[global::__DynamicallyInvokable]
		public virtual string Method
		{
			[global::__DynamicallyInvokable]
			get
			{
				this.CheckDisposed();
				return this.m_Verb.Name;
			}
		}

		// Token: 0x06000AE6 RID: 2790 RVA: 0x0003C63F File Offset: 0x0003A83F
		private void CheckDisposed()
		{
			if (this.m_propertiesDisposed)
			{
				throw new ObjectDisposedException(base.GetType().FullName);
			}
		}

		// Token: 0x04000F3B RID: 3899
		private Uri m_Uri;

		// Token: 0x04000F3C RID: 3900
		private KnownHttpVerb m_Verb;

		// Token: 0x04000F3D RID: 3901
		private HttpStatusCode m_StatusCode;

		// Token: 0x04000F3E RID: 3902
		private string m_StatusDescription;

		// Token: 0x04000F3F RID: 3903
		private Stream m_ConnectStream;

		// Token: 0x04000F40 RID: 3904
		private CoreResponseData m_CoreResponseData;

		// Token: 0x04000F41 RID: 3905
		private WebHeaderCollection m_HttpResponseHeaders;

		// Token: 0x04000F42 RID: 3906
		private long m_ContentLength;

		// Token: 0x04000F43 RID: 3907
		private string m_MediaType;

		// Token: 0x04000F44 RID: 3908
		private string m_CharacterSet;

		// Token: 0x04000F45 RID: 3909
		private bool m_IsVersionHttp11;

		// Token: 0x04000F46 RID: 3910
		internal X509Certificate m_Certificate;

		// Token: 0x04000F47 RID: 3911
		private CookieCollection m_cookies;

		// Token: 0x04000F48 RID: 3912
		private bool m_disposed;

		// Token: 0x04000F49 RID: 3913
		private bool m_propertiesDisposed;

		// Token: 0x04000F4A RID: 3914
		private bool m_UsesProxySemantics;

		// Token: 0x04000F4B RID: 3915
		private bool m_IsMutuallyAuthenticated;

		// Token: 0x04000F4C RID: 3916
		private bool m_IsWebSocketResponse;

		// Token: 0x04000F4D RID: 3917
		private string m_ConnectionGroupName;

		// Token: 0x04000F4E RID: 3918
		private Stream m_WebSocketConnectionStream;
	}
}
