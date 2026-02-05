using System;
using System.IO;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace System.Net
{
	// Token: 0x0200018B RID: 395
	[global::__DynamicallyInvokable]
	[Serializable]
	public abstract class WebResponse : MarshalByRefObject, ISerializable, IDisposable
	{
		// Token: 0x06000F1C RID: 3868 RVA: 0x0004E749 File Offset: 0x0004C949
		[global::__DynamicallyInvokable]
		protected WebResponse()
		{
		}

		// Token: 0x06000F1D RID: 3869 RVA: 0x0004E751 File Offset: 0x0004C951
		protected WebResponse(SerializationInfo serializationInfo, StreamingContext streamingContext)
		{
		}

		// Token: 0x06000F1E RID: 3870 RVA: 0x0004E759 File Offset: 0x0004C959
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter, SerializationFormatter = true)]
		void ISerializable.GetObjectData(SerializationInfo serializationInfo, StreamingContext streamingContext)
		{
			this.GetObjectData(serializationInfo, streamingContext);
		}

		// Token: 0x06000F1F RID: 3871 RVA: 0x0004E763 File Offset: 0x0004C963
		[SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
		protected virtual void GetObjectData(SerializationInfo serializationInfo, StreamingContext streamingContext)
		{
		}

		// Token: 0x06000F20 RID: 3872 RVA: 0x0004E765 File Offset: 0x0004C965
		public virtual void Close()
		{
		}

		// Token: 0x06000F21 RID: 3873 RVA: 0x0004E767 File Offset: 0x0004C967
		[global::__DynamicallyInvokable]
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06000F22 RID: 3874 RVA: 0x0004E778 File Offset: 0x0004C978
		[global::__DynamicallyInvokable]
		protected virtual void Dispose(bool disposing)
		{
			if (!disposing)
			{
				return;
			}
			try
			{
				this.Close();
			}
			catch
			{
			}
		}

		// Token: 0x1700035A RID: 858
		// (get) Token: 0x06000F23 RID: 3875 RVA: 0x0004E7A8 File Offset: 0x0004C9A8
		public virtual bool IsFromCache
		{
			get
			{
				return this.m_IsFromCache;
			}
		}

		// Token: 0x1700035B RID: 859
		// (set) Token: 0x06000F24 RID: 3876 RVA: 0x0004E7B0 File Offset: 0x0004C9B0
		internal bool InternalSetFromCache
		{
			set
			{
				this.m_IsFromCache = value;
			}
		}

		// Token: 0x1700035C RID: 860
		// (get) Token: 0x06000F25 RID: 3877 RVA: 0x0004E7B9 File Offset: 0x0004C9B9
		internal virtual bool IsCacheFresh
		{
			get
			{
				return this.m_IsCacheFresh;
			}
		}

		// Token: 0x1700035D RID: 861
		// (set) Token: 0x06000F26 RID: 3878 RVA: 0x0004E7C1 File Offset: 0x0004C9C1
		internal bool InternalSetIsCacheFresh
		{
			set
			{
				this.m_IsCacheFresh = value;
			}
		}

		// Token: 0x1700035E RID: 862
		// (get) Token: 0x06000F27 RID: 3879 RVA: 0x0004E7CA File Offset: 0x0004C9CA
		public virtual bool IsMutuallyAuthenticated
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700035F RID: 863
		// (get) Token: 0x06000F28 RID: 3880 RVA: 0x0004E7CD File Offset: 0x0004C9CD
		// (set) Token: 0x06000F29 RID: 3881 RVA: 0x0004E7D4 File Offset: 0x0004C9D4
		[global::__DynamicallyInvokable]
		public virtual long ContentLength
		{
			[global::__DynamicallyInvokable]
			get
			{
				throw ExceptionHelper.PropertyNotImplementedException;
			}
			set
			{
				throw ExceptionHelper.PropertyNotImplementedException;
			}
		}

		// Token: 0x17000360 RID: 864
		// (get) Token: 0x06000F2A RID: 3882 RVA: 0x0004E7DB File Offset: 0x0004C9DB
		// (set) Token: 0x06000F2B RID: 3883 RVA: 0x0004E7E2 File Offset: 0x0004C9E2
		[global::__DynamicallyInvokable]
		public virtual string ContentType
		{
			[global::__DynamicallyInvokable]
			get
			{
				throw ExceptionHelper.PropertyNotImplementedException;
			}
			set
			{
				throw ExceptionHelper.PropertyNotImplementedException;
			}
		}

		// Token: 0x06000F2C RID: 3884 RVA: 0x0004E7E9 File Offset: 0x0004C9E9
		[global::__DynamicallyInvokable]
		public virtual Stream GetResponseStream()
		{
			throw ExceptionHelper.MethodNotImplementedException;
		}

		// Token: 0x17000361 RID: 865
		// (get) Token: 0x06000F2D RID: 3885 RVA: 0x0004E7F0 File Offset: 0x0004C9F0
		[global::__DynamicallyInvokable]
		public virtual Uri ResponseUri
		{
			[global::__DynamicallyInvokable]
			get
			{
				throw ExceptionHelper.PropertyNotImplementedException;
			}
		}

		// Token: 0x17000362 RID: 866
		// (get) Token: 0x06000F2E RID: 3886 RVA: 0x0004E7F7 File Offset: 0x0004C9F7
		[global::__DynamicallyInvokable]
		public virtual WebHeaderCollection Headers
		{
			[global::__DynamicallyInvokable]
			get
			{
				throw ExceptionHelper.PropertyNotImplementedException;
			}
		}

		// Token: 0x17000363 RID: 867
		// (get) Token: 0x06000F2F RID: 3887 RVA: 0x0004E7FE File Offset: 0x0004C9FE
		[global::__DynamicallyInvokable]
		public virtual bool SupportsHeaders
		{
			[global::__DynamicallyInvokable]
			get
			{
				return false;
			}
		}

		// Token: 0x04001298 RID: 4760
		private bool m_IsCacheFresh;

		// Token: 0x04001299 RID: 4761
		private bool m_IsFromCache;
	}
}
