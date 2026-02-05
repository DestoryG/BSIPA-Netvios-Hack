using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace System.Net
{
	// Token: 0x0200017D RID: 381
	[global::__DynamicallyInvokable]
	[Serializable]
	public class WebException : InvalidOperationException, ISerializable
	{
		// Token: 0x06000E0D RID: 3597 RVA: 0x000499CE File Offset: 0x00047BCE
		[global::__DynamicallyInvokable]
		public WebException()
		{
		}

		// Token: 0x06000E0E RID: 3598 RVA: 0x000499DE File Offset: 0x00047BDE
		[global::__DynamicallyInvokable]
		public WebException(string message)
			: this(message, null)
		{
		}

		// Token: 0x06000E0F RID: 3599 RVA: 0x000499E8 File Offset: 0x00047BE8
		[global::__DynamicallyInvokable]
		public WebException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		// Token: 0x06000E10 RID: 3600 RVA: 0x000499FA File Offset: 0x00047BFA
		[global::__DynamicallyInvokable]
		public WebException(string message, WebExceptionStatus status)
			: this(message, null, status, null)
		{
		}

		// Token: 0x06000E11 RID: 3601 RVA: 0x00049A06 File Offset: 0x00047C06
		internal WebException(string message, WebExceptionStatus status, WebExceptionInternalStatus internalStatus, Exception innerException)
			: this(message, innerException, status, null, internalStatus)
		{
		}

		// Token: 0x06000E12 RID: 3602 RVA: 0x00049A14 File Offset: 0x00047C14
		[global::__DynamicallyInvokable]
		public WebException(string message, Exception innerException, WebExceptionStatus status, WebResponse response)
			: this(message, null, innerException, status, response)
		{
		}

		// Token: 0x06000E13 RID: 3603 RVA: 0x00049A24 File Offset: 0x00047C24
		internal WebException(string message, string data, Exception innerException, WebExceptionStatus status, WebResponse response)
			: base(message + ((data != null) ? (": '" + data + "'") : ""), innerException)
		{
			this.m_Status = status;
			this.m_Response = response;
		}

		// Token: 0x06000E14 RID: 3604 RVA: 0x00049A70 File Offset: 0x00047C70
		internal WebException(string message, Exception innerException, WebExceptionStatus status, WebResponse response, WebExceptionInternalStatus internalStatus)
			: this(message, null, innerException, status, response, internalStatus)
		{
		}

		// Token: 0x06000E15 RID: 3605 RVA: 0x00049A80 File Offset: 0x00047C80
		internal WebException(string message, string data, Exception innerException, WebExceptionStatus status, WebResponse response, WebExceptionInternalStatus internalStatus)
			: base(message + ((data != null) ? (": '" + data + "'") : ""), innerException)
		{
			this.m_Status = status;
			this.m_Response = response;
			this.m_InternalStatus = internalStatus;
		}

		// Token: 0x06000E16 RID: 3606 RVA: 0x00049AD4 File Offset: 0x00047CD4
		protected WebException(SerializationInfo serializationInfo, StreamingContext streamingContext)
			: base(serializationInfo, streamingContext)
		{
		}

		// Token: 0x06000E17 RID: 3607 RVA: 0x00049AE6 File Offset: 0x00047CE6
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		void ISerializable.GetObjectData(SerializationInfo serializationInfo, StreamingContext streamingContext)
		{
			this.GetObjectData(serializationInfo, streamingContext);
		}

		// Token: 0x06000E18 RID: 3608 RVA: 0x00049AF0 File Offset: 0x00047CF0
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		public override void GetObjectData(SerializationInfo serializationInfo, StreamingContext streamingContext)
		{
			base.GetObjectData(serializationInfo, streamingContext);
		}

		// Token: 0x17000318 RID: 792
		// (get) Token: 0x06000E19 RID: 3609 RVA: 0x00049AFA File Offset: 0x00047CFA
		[global::__DynamicallyInvokable]
		public WebExceptionStatus Status
		{
			[global::__DynamicallyInvokable]
			get
			{
				return this.m_Status;
			}
		}

		// Token: 0x17000319 RID: 793
		// (get) Token: 0x06000E1A RID: 3610 RVA: 0x00049B02 File Offset: 0x00047D02
		[global::__DynamicallyInvokable]
		public WebResponse Response
		{
			[global::__DynamicallyInvokable]
			get
			{
				return this.m_Response;
			}
		}

		// Token: 0x1700031A RID: 794
		// (get) Token: 0x06000E1B RID: 3611 RVA: 0x00049B0A File Offset: 0x00047D0A
		internal WebExceptionInternalStatus InternalStatus
		{
			get
			{
				return this.m_InternalStatus;
			}
		}

		// Token: 0x04001220 RID: 4640
		private WebExceptionStatus m_Status = WebExceptionStatus.UnknownError;

		// Token: 0x04001221 RID: 4641
		private WebResponse m_Response;

		// Token: 0x04001222 RID: 4642
		[NonSerialized]
		private WebExceptionInternalStatus m_InternalStatus;
	}
}
