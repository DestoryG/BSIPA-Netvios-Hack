using System;
using System.Globalization;
using System.IO;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace System.Net
{
	// Token: 0x020000E9 RID: 233
	[Serializable]
	public class FileWebResponse : WebResponse, ISerializable, ICloseEx
	{
		// Token: 0x060007FF RID: 2047 RVA: 0x0002C1EC File Offset: 0x0002A3EC
		internal FileWebResponse(FileWebRequest request, Uri uri, FileAccess access, bool asyncHint)
		{
			try
			{
				this.m_fileAccess = access;
				if (access == FileAccess.Write)
				{
					this.m_stream = Stream.Null;
				}
				else
				{
					this.m_stream = new FileWebStream(request, uri.LocalPath, FileMode.Open, FileAccess.Read, FileShare.Read, 8192, asyncHint);
					this.m_contentLength = this.m_stream.Length;
				}
				this.m_headers = new WebHeaderCollection(WebHeaderCollectionType.FileWebResponse);
				this.m_headers.AddInternal("Content-Length", this.m_contentLength.ToString(NumberFormatInfo.InvariantInfo));
				this.m_headers.AddInternal("Content-Type", "application/octet-stream");
				this.m_uri = uri;
			}
			catch (Exception ex)
			{
				Exception ex2 = new WebException(ex.Message, ex, WebExceptionStatus.ConnectFailure, null);
				throw ex2;
			}
		}

		// Token: 0x06000800 RID: 2048 RVA: 0x0002C2B4 File Offset: 0x0002A4B4
		[Obsolete("Serialization is obsoleted for this type. http://go.microsoft.com/fwlink/?linkid=14202")]
		protected FileWebResponse(SerializationInfo serializationInfo, StreamingContext streamingContext)
			: base(serializationInfo, streamingContext)
		{
			this.m_headers = (WebHeaderCollection)serializationInfo.GetValue("headers", typeof(WebHeaderCollection));
			this.m_uri = (Uri)serializationInfo.GetValue("uri", typeof(Uri));
			this.m_contentLength = serializationInfo.GetInt64("contentLength");
			this.m_fileAccess = (FileAccess)serializationInfo.GetInt32("fileAccess");
		}

		// Token: 0x06000801 RID: 2049 RVA: 0x0002C32B File Offset: 0x0002A52B
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter, SerializationFormatter = true)]
		void ISerializable.GetObjectData(SerializationInfo serializationInfo, StreamingContext streamingContext)
		{
			this.GetObjectData(serializationInfo, streamingContext);
		}

		// Token: 0x06000802 RID: 2050 RVA: 0x0002C338 File Offset: 0x0002A538
		[SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
		protected override void GetObjectData(SerializationInfo serializationInfo, StreamingContext streamingContext)
		{
			serializationInfo.AddValue("headers", this.m_headers, typeof(WebHeaderCollection));
			serializationInfo.AddValue("uri", this.m_uri, typeof(Uri));
			serializationInfo.AddValue("contentLength", this.m_contentLength);
			serializationInfo.AddValue("fileAccess", this.m_fileAccess);
			base.GetObjectData(serializationInfo, streamingContext);
		}

		// Token: 0x17000176 RID: 374
		// (get) Token: 0x06000803 RID: 2051 RVA: 0x0002C3AA File Offset: 0x0002A5AA
		public override long ContentLength
		{
			get
			{
				this.CheckDisposed();
				return this.m_contentLength;
			}
		}

		// Token: 0x17000177 RID: 375
		// (get) Token: 0x06000804 RID: 2052 RVA: 0x0002C3B8 File Offset: 0x0002A5B8
		public override string ContentType
		{
			get
			{
				this.CheckDisposed();
				return "application/octet-stream";
			}
		}

		// Token: 0x17000178 RID: 376
		// (get) Token: 0x06000805 RID: 2053 RVA: 0x0002C3C5 File Offset: 0x0002A5C5
		public override WebHeaderCollection Headers
		{
			get
			{
				this.CheckDisposed();
				return this.m_headers;
			}
		}

		// Token: 0x17000179 RID: 377
		// (get) Token: 0x06000806 RID: 2054 RVA: 0x0002C3D3 File Offset: 0x0002A5D3
		public override bool SupportsHeaders
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700017A RID: 378
		// (get) Token: 0x06000807 RID: 2055 RVA: 0x0002C3D6 File Offset: 0x0002A5D6
		public override Uri ResponseUri
		{
			get
			{
				this.CheckDisposed();
				return this.m_uri;
			}
		}

		// Token: 0x06000808 RID: 2056 RVA: 0x0002C3E4 File Offset: 0x0002A5E4
		private void CheckDisposed()
		{
			if (this.m_closed)
			{
				throw new ObjectDisposedException(base.GetType().FullName);
			}
		}

		// Token: 0x06000809 RID: 2057 RVA: 0x0002C3FF File Offset: 0x0002A5FF
		public override void Close()
		{
			((ICloseEx)this).CloseEx(CloseExState.Normal);
		}

		// Token: 0x0600080A RID: 2058 RVA: 0x0002C408 File Offset: 0x0002A608
		void ICloseEx.CloseEx(CloseExState closeState)
		{
			try
			{
				if (!this.m_closed)
				{
					this.m_closed = true;
					Stream stream = this.m_stream;
					if (stream != null)
					{
						if (stream is ICloseEx)
						{
							((ICloseEx)stream).CloseEx(closeState);
						}
						else
						{
							stream.Close();
						}
						this.m_stream = null;
					}
				}
			}
			finally
			{
			}
		}

		// Token: 0x0600080B RID: 2059 RVA: 0x0002C464 File Offset: 0x0002A664
		public override Stream GetResponseStream()
		{
			try
			{
				this.CheckDisposed();
			}
			finally
			{
			}
			return this.m_stream;
		}

		// Token: 0x04000D53 RID: 3411
		private const int DefaultFileStreamBufferSize = 8192;

		// Token: 0x04000D54 RID: 3412
		private const string DefaultFileContentType = "application/octet-stream";

		// Token: 0x04000D55 RID: 3413
		private bool m_closed;

		// Token: 0x04000D56 RID: 3414
		private long m_contentLength;

		// Token: 0x04000D57 RID: 3415
		private FileAccess m_fileAccess;

		// Token: 0x04000D58 RID: 3416
		private WebHeaderCollection m_headers;

		// Token: 0x04000D59 RID: 3417
		private Stream m_stream;

		// Token: 0x04000D5A RID: 3418
		private Uri m_uri;
	}
}
