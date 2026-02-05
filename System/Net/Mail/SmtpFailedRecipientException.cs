using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace System.Net.Mail
{
	// Token: 0x0200028B RID: 651
	[Serializable]
	public class SmtpFailedRecipientException : SmtpException, ISerializable
	{
		// Token: 0x06001856 RID: 6230 RVA: 0x0007BEBB File Offset: 0x0007A0BB
		public SmtpFailedRecipientException()
		{
		}

		// Token: 0x06001857 RID: 6231 RVA: 0x0007BEC3 File Offset: 0x0007A0C3
		public SmtpFailedRecipientException(string message)
			: base(message)
		{
		}

		// Token: 0x06001858 RID: 6232 RVA: 0x0007BECC File Offset: 0x0007A0CC
		public SmtpFailedRecipientException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		// Token: 0x06001859 RID: 6233 RVA: 0x0007BED6 File Offset: 0x0007A0D6
		protected SmtpFailedRecipientException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
			this.failedRecipient = info.GetString("failedRecipient");
		}

		// Token: 0x0600185A RID: 6234 RVA: 0x0007BEF1 File Offset: 0x0007A0F1
		public SmtpFailedRecipientException(SmtpStatusCode statusCode, string failedRecipient)
			: base(statusCode)
		{
			this.failedRecipient = failedRecipient;
		}

		// Token: 0x0600185B RID: 6235 RVA: 0x0007BF01 File Offset: 0x0007A101
		public SmtpFailedRecipientException(SmtpStatusCode statusCode, string failedRecipient, string serverResponse)
			: base(statusCode, serverResponse, true)
		{
			this.failedRecipient = failedRecipient;
		}

		// Token: 0x0600185C RID: 6236 RVA: 0x0007BF13 File Offset: 0x0007A113
		public SmtpFailedRecipientException(string message, string failedRecipient, Exception innerException)
			: base(message, innerException)
		{
			this.failedRecipient = failedRecipient;
		}

		// Token: 0x1700050B RID: 1291
		// (get) Token: 0x0600185D RID: 6237 RVA: 0x0007BF24 File Offset: 0x0007A124
		public string FailedRecipient
		{
			get
			{
				return this.failedRecipient;
			}
		}

		// Token: 0x0600185E RID: 6238 RVA: 0x0007BF2C File Offset: 0x0007A12C
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		void ISerializable.GetObjectData(SerializationInfo serializationInfo, StreamingContext streamingContext)
		{
			this.GetObjectData(serializationInfo, streamingContext);
		}

		// Token: 0x0600185F RID: 6239 RVA: 0x0007BF36 File Offset: 0x0007A136
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		public override void GetObjectData(SerializationInfo serializationInfo, StreamingContext streamingContext)
		{
			base.GetObjectData(serializationInfo, streamingContext);
			serializationInfo.AddValue("failedRecipient", this.failedRecipient, typeof(string));
		}

		// Token: 0x0400184F RID: 6223
		private string failedRecipient;

		// Token: 0x04001850 RID: 6224
		internal bool fatal;
	}
}
