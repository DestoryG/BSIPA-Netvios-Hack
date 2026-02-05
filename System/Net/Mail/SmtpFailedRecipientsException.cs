using System;
using System.Collections;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace System.Net.Mail
{
	// Token: 0x0200028C RID: 652
	[Serializable]
	public class SmtpFailedRecipientsException : SmtpFailedRecipientException, ISerializable
	{
		// Token: 0x06001860 RID: 6240 RVA: 0x0007BF5B File Offset: 0x0007A15B
		public SmtpFailedRecipientsException()
		{
			this.innerExceptions = new SmtpFailedRecipientException[0];
		}

		// Token: 0x06001861 RID: 6241 RVA: 0x0007BF6F File Offset: 0x0007A16F
		public SmtpFailedRecipientsException(string message)
			: base(message)
		{
			this.innerExceptions = new SmtpFailedRecipientException[0];
		}

		// Token: 0x06001862 RID: 6242 RVA: 0x0007BF84 File Offset: 0x0007A184
		public SmtpFailedRecipientsException(string message, Exception innerException)
			: base(message, innerException)
		{
			SmtpFailedRecipientException ex = innerException as SmtpFailedRecipientException;
			SmtpFailedRecipientException[] array;
			if (ex != null)
			{
				(array = new SmtpFailedRecipientException[1])[0] = ex;
			}
			else
			{
				array = new SmtpFailedRecipientException[0];
			}
			this.innerExceptions = array;
		}

		// Token: 0x06001863 RID: 6243 RVA: 0x0007BFBB File Offset: 0x0007A1BB
		protected SmtpFailedRecipientsException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
			this.innerExceptions = (SmtpFailedRecipientException[])info.GetValue("innerExceptions", typeof(SmtpFailedRecipientException[]));
		}

		// Token: 0x06001864 RID: 6244 RVA: 0x0007BFE8 File Offset: 0x0007A1E8
		public SmtpFailedRecipientsException(string message, SmtpFailedRecipientException[] innerExceptions)
			: base(message, (innerExceptions != null && innerExceptions.Length != 0) ? innerExceptions[0].FailedRecipient : null, (innerExceptions != null && innerExceptions.Length != 0) ? innerExceptions[0] : null)
		{
			if (innerExceptions == null)
			{
				throw new ArgumentNullException("innerExceptions");
			}
			this.innerExceptions = ((innerExceptions == null) ? new SmtpFailedRecipientException[0] : innerExceptions);
		}

		// Token: 0x06001865 RID: 6245 RVA: 0x0007C03C File Offset: 0x0007A23C
		internal SmtpFailedRecipientsException(ArrayList innerExceptions, bool allFailed)
			: base(allFailed ? SR.GetString("SmtpAllRecipientsFailed") : SR.GetString("SmtpRecipientFailed"), (innerExceptions != null && innerExceptions.Count > 0) ? ((SmtpFailedRecipientException)innerExceptions[0]).FailedRecipient : null, (innerExceptions != null && innerExceptions.Count > 0) ? ((SmtpFailedRecipientException)innerExceptions[0]) : null)
		{
			if (innerExceptions == null)
			{
				throw new ArgumentNullException("innerExceptions");
			}
			this.innerExceptions = new SmtpFailedRecipientException[innerExceptions.Count];
			int num = 0;
			foreach (object obj in innerExceptions)
			{
				SmtpFailedRecipientException ex = (SmtpFailedRecipientException)obj;
				this.innerExceptions[num++] = ex;
			}
		}

		// Token: 0x1700050C RID: 1292
		// (get) Token: 0x06001866 RID: 6246 RVA: 0x0007C114 File Offset: 0x0007A314
		public SmtpFailedRecipientException[] InnerExceptions
		{
			get
			{
				return this.innerExceptions;
			}
		}

		// Token: 0x06001867 RID: 6247 RVA: 0x0007C11C File Offset: 0x0007A31C
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		void ISerializable.GetObjectData(SerializationInfo serializationInfo, StreamingContext streamingContext)
		{
			this.GetObjectData(serializationInfo, streamingContext);
		}

		// Token: 0x06001868 RID: 6248 RVA: 0x0007C126 File Offset: 0x0007A326
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		public override void GetObjectData(SerializationInfo serializationInfo, StreamingContext streamingContext)
		{
			base.GetObjectData(serializationInfo, streamingContext);
			serializationInfo.AddValue("innerExceptions", this.innerExceptions, typeof(SmtpFailedRecipientException[]));
		}

		// Token: 0x04001851 RID: 6225
		private SmtpFailedRecipientException[] innerExceptions;
	}
}
