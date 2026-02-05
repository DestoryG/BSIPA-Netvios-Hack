using System;
using System.IO;

namespace System.Net
{
	// Token: 0x020000F1 RID: 241
	public class FtpWebResponse : WebResponse, IDisposable
	{
		// Token: 0x06000867 RID: 2151 RVA: 0x0002F280 File Offset: 0x0002D480
		internal FtpWebResponse(Stream responseStream, long contentLength, Uri responseUri, FtpStatusCode statusCode, string statusLine, DateTime lastModified, string bannerMessage, string welcomeMessage, string exitMessage)
		{
			this.m_ResponseStream = responseStream;
			if (responseStream == null && contentLength < 0L)
			{
				contentLength = 0L;
			}
			this.m_ContentLength = contentLength;
			this.m_ResponseUri = responseUri;
			this.m_StatusCode = statusCode;
			this.m_StatusLine = statusLine;
			this.m_LastModified = lastModified;
			this.m_BannerMessage = bannerMessage;
			this.m_WelcomeMessage = welcomeMessage;
			this.m_ExitMessage = exitMessage;
		}

		// Token: 0x06000868 RID: 2152 RVA: 0x0002F2E4 File Offset: 0x0002D4E4
		internal FtpWebResponse(HttpWebResponse httpWebResponse)
		{
			this.m_HttpWebResponse = httpWebResponse;
			base.InternalSetFromCache = this.m_HttpWebResponse.IsFromCache;
			base.InternalSetIsCacheFresh = this.m_HttpWebResponse.IsCacheFresh;
		}

		// Token: 0x06000869 RID: 2153 RVA: 0x0002F315 File Offset: 0x0002D515
		internal void UpdateStatus(FtpStatusCode statusCode, string statusLine, string exitMessage)
		{
			this.m_StatusCode = statusCode;
			this.m_StatusLine = statusLine;
			this.m_ExitMessage = exitMessage;
		}

		// Token: 0x0600086A RID: 2154 RVA: 0x0002F32C File Offset: 0x0002D52C
		public override Stream GetResponseStream()
		{
			Stream stream;
			if (this.HttpProxyMode)
			{
				stream = this.m_HttpWebResponse.GetResponseStream();
			}
			else if (this.m_ResponseStream != null)
			{
				stream = this.m_ResponseStream;
			}
			else
			{
				stream = (this.m_ResponseStream = new FtpWebResponse.EmptyStream());
			}
			return stream;
		}

		// Token: 0x0600086B RID: 2155 RVA: 0x0002F372 File Offset: 0x0002D572
		internal void SetResponseStream(Stream stream)
		{
			if (stream == null || stream == Stream.Null || stream is FtpWebResponse.EmptyStream)
			{
				return;
			}
			this.m_ResponseStream = stream;
		}

		// Token: 0x0600086C RID: 2156 RVA: 0x0002F390 File Offset: 0x0002D590
		public override void Close()
		{
			if (Logging.On)
			{
				Logging.Enter(Logging.Web, this, "Close", "");
			}
			if (this.HttpProxyMode)
			{
				this.m_HttpWebResponse.Close();
			}
			else
			{
				Stream responseStream = this.m_ResponseStream;
				if (responseStream != null)
				{
					responseStream.Close();
				}
			}
			if (Logging.On)
			{
				Logging.Exit(Logging.Web, this, "Close", "");
			}
		}

		// Token: 0x1700019C RID: 412
		// (get) Token: 0x0600086D RID: 2157 RVA: 0x0002F3FA File Offset: 0x0002D5FA
		public override long ContentLength
		{
			get
			{
				if (this.HttpProxyMode)
				{
					return this.m_HttpWebResponse.ContentLength;
				}
				return this.m_ContentLength;
			}
		}

		// Token: 0x0600086E RID: 2158 RVA: 0x0002F416 File Offset: 0x0002D616
		internal void SetContentLength(long value)
		{
			if (this.HttpProxyMode)
			{
				return;
			}
			this.m_ContentLength = value;
		}

		// Token: 0x1700019D RID: 413
		// (get) Token: 0x0600086F RID: 2159 RVA: 0x0002F428 File Offset: 0x0002D628
		public override WebHeaderCollection Headers
		{
			get
			{
				if (this.HttpProxyMode)
				{
					return this.m_HttpWebResponse.Headers;
				}
				if (this.m_FtpRequestHeaders == null)
				{
					lock (this)
					{
						if (this.m_FtpRequestHeaders == null)
						{
							this.m_FtpRequestHeaders = new WebHeaderCollection(WebHeaderCollectionType.FtpWebResponse);
						}
					}
				}
				return this.m_FtpRequestHeaders;
			}
		}

		// Token: 0x1700019E RID: 414
		// (get) Token: 0x06000870 RID: 2160 RVA: 0x0002F494 File Offset: 0x0002D694
		public override bool SupportsHeaders
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700019F RID: 415
		// (get) Token: 0x06000871 RID: 2161 RVA: 0x0002F497 File Offset: 0x0002D697
		public override Uri ResponseUri
		{
			get
			{
				if (this.HttpProxyMode)
				{
					return this.m_HttpWebResponse.ResponseUri;
				}
				return this.m_ResponseUri;
			}
		}

		// Token: 0x170001A0 RID: 416
		// (get) Token: 0x06000872 RID: 2162 RVA: 0x0002F4B3 File Offset: 0x0002D6B3
		public FtpStatusCode StatusCode
		{
			get
			{
				if (this.HttpProxyMode)
				{
					return (FtpStatusCode)this.m_HttpWebResponse.StatusCode;
				}
				return this.m_StatusCode;
			}
		}

		// Token: 0x170001A1 RID: 417
		// (get) Token: 0x06000873 RID: 2163 RVA: 0x0002F4CF File Offset: 0x0002D6CF
		public string StatusDescription
		{
			get
			{
				if (this.HttpProxyMode)
				{
					return this.m_HttpWebResponse.StatusDescription;
				}
				return this.m_StatusLine;
			}
		}

		// Token: 0x170001A2 RID: 418
		// (get) Token: 0x06000874 RID: 2164 RVA: 0x0002F4EB File Offset: 0x0002D6EB
		public DateTime LastModified
		{
			get
			{
				if (this.HttpProxyMode)
				{
					return this.m_HttpWebResponse.LastModified;
				}
				return this.m_LastModified;
			}
		}

		// Token: 0x170001A3 RID: 419
		// (get) Token: 0x06000875 RID: 2165 RVA: 0x0002F507 File Offset: 0x0002D707
		public string BannerMessage
		{
			get
			{
				return this.m_BannerMessage;
			}
		}

		// Token: 0x170001A4 RID: 420
		// (get) Token: 0x06000876 RID: 2166 RVA: 0x0002F50F File Offset: 0x0002D70F
		public string WelcomeMessage
		{
			get
			{
				return this.m_WelcomeMessage;
			}
		}

		// Token: 0x170001A5 RID: 421
		// (get) Token: 0x06000877 RID: 2167 RVA: 0x0002F517 File Offset: 0x0002D717
		public string ExitMessage
		{
			get
			{
				return this.m_ExitMessage;
			}
		}

		// Token: 0x170001A6 RID: 422
		// (get) Token: 0x06000878 RID: 2168 RVA: 0x0002F51F File Offset: 0x0002D71F
		private bool HttpProxyMode
		{
			get
			{
				return this.m_HttpWebResponse != null;
			}
		}

		// Token: 0x04000DCD RID: 3533
		internal Stream m_ResponseStream;

		// Token: 0x04000DCE RID: 3534
		private long m_ContentLength;

		// Token: 0x04000DCF RID: 3535
		private Uri m_ResponseUri;

		// Token: 0x04000DD0 RID: 3536
		private FtpStatusCode m_StatusCode;

		// Token: 0x04000DD1 RID: 3537
		private string m_StatusLine;

		// Token: 0x04000DD2 RID: 3538
		private WebHeaderCollection m_FtpRequestHeaders;

		// Token: 0x04000DD3 RID: 3539
		private HttpWebResponse m_HttpWebResponse;

		// Token: 0x04000DD4 RID: 3540
		private DateTime m_LastModified;

		// Token: 0x04000DD5 RID: 3541
		private string m_BannerMessage;

		// Token: 0x04000DD6 RID: 3542
		private string m_WelcomeMessage;

		// Token: 0x04000DD7 RID: 3543
		private string m_ExitMessage;

		// Token: 0x020006FC RID: 1788
		internal class EmptyStream : MemoryStream
		{
			// Token: 0x0600407C RID: 16508 RVA: 0x0010E76F File Offset: 0x0010C96F
			internal EmptyStream()
				: base(new byte[0], false)
			{
			}
		}
	}
}
