using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace System.Net.Mail
{
	// Token: 0x0200028A RID: 650
	[Serializable]
	public class SmtpException : Exception, ISerializable
	{
		// Token: 0x06001848 RID: 6216 RVA: 0x0007BB95 File Offset: 0x00079D95
		private static string GetMessageForStatus(SmtpStatusCode statusCode, string serverResponse)
		{
			return SmtpException.GetMessageForStatus(statusCode) + " " + SR.GetString("MailServerResponse", new object[] { serverResponse });
		}

		// Token: 0x06001849 RID: 6217 RVA: 0x0007BBBC File Offset: 0x00079DBC
		private static string GetMessageForStatus(SmtpStatusCode statusCode)
		{
			if (statusCode <= SmtpStatusCode.UserNotLocalWillForward)
			{
				if (statusCode <= SmtpStatusCode.ServiceReady)
				{
					if (statusCode == SmtpStatusCode.SystemStatus)
					{
						return SR.GetString("SmtpSystemStatus");
					}
					if (statusCode == SmtpStatusCode.HelpMessage)
					{
						return SR.GetString("SmtpHelpMessage");
					}
					if (statusCode == SmtpStatusCode.ServiceReady)
					{
						return SR.GetString("SmtpServiceReady");
					}
				}
				else
				{
					if (statusCode == SmtpStatusCode.ServiceClosingTransmissionChannel)
					{
						return SR.GetString("SmtpServiceClosingTransmissionChannel");
					}
					if (statusCode == SmtpStatusCode.Ok)
					{
						return SR.GetString("SmtpOK");
					}
					if (statusCode == SmtpStatusCode.UserNotLocalWillForward)
					{
						return SR.GetString("SmtpUserNotLocalWillForward");
					}
				}
			}
			else if (statusCode <= SmtpStatusCode.ClientNotPermitted)
			{
				if (statusCode == SmtpStatusCode.StartMailInput)
				{
					return SR.GetString("SmtpStartMailInput");
				}
				if (statusCode == SmtpStatusCode.ServiceNotAvailable)
				{
					return SR.GetString("SmtpServiceNotAvailable");
				}
				switch (statusCode)
				{
				case SmtpStatusCode.MailboxBusy:
					return SR.GetString("SmtpMailboxBusy");
				case SmtpStatusCode.LocalErrorInProcessing:
					return SR.GetString("SmtpLocalErrorInProcessing");
				case SmtpStatusCode.InsufficientStorage:
					return SR.GetString("SmtpInsufficientStorage");
				case SmtpStatusCode.ClientNotPermitted:
					return SR.GetString("SmtpClientNotPermitted");
				}
			}
			else
			{
				switch (statusCode)
				{
				case SmtpStatusCode.CommandUnrecognized:
					break;
				case SmtpStatusCode.SyntaxError:
					return SR.GetString("SmtpSyntaxError");
				case SmtpStatusCode.CommandNotImplemented:
					return SR.GetString("SmtpCommandNotImplemented");
				case SmtpStatusCode.BadCommandSequence:
					return SR.GetString("SmtpBadCommandSequence");
				case SmtpStatusCode.CommandParameterNotImplemented:
					return SR.GetString("SmtpCommandParameterNotImplemented");
				default:
					if (statusCode == SmtpStatusCode.MustIssueStartTlsFirst)
					{
						return SR.GetString("SmtpMustIssueStartTlsFirst");
					}
					switch (statusCode)
					{
					case SmtpStatusCode.MailboxUnavailable:
						return SR.GetString("SmtpMailboxUnavailable");
					case SmtpStatusCode.UserNotLocalTryAlternatePath:
						return SR.GetString("SmtpUserNotLocalTryAlternatePath");
					case SmtpStatusCode.ExceededStorageAllocation:
						return SR.GetString("SmtpExceededStorageAllocation");
					case SmtpStatusCode.MailboxNameNotAllowed:
						return SR.GetString("SmtpMailboxNameNotAllowed");
					case SmtpStatusCode.TransactionFailed:
						return SR.GetString("SmtpTransactionFailed");
					}
					break;
				}
			}
			return SR.GetString("SmtpCommandUnrecognized");
		}

		// Token: 0x0600184A RID: 6218 RVA: 0x0007BDAC File Offset: 0x00079FAC
		public SmtpException(SmtpStatusCode statusCode)
			: base(SmtpException.GetMessageForStatus(statusCode))
		{
			this.statusCode = statusCode;
		}

		// Token: 0x0600184B RID: 6219 RVA: 0x0007BDC8 File Offset: 0x00079FC8
		public SmtpException(SmtpStatusCode statusCode, string message)
			: base(message)
		{
			this.statusCode = statusCode;
		}

		// Token: 0x0600184C RID: 6220 RVA: 0x0007BDDF File Offset: 0x00079FDF
		public SmtpException()
			: this(SmtpStatusCode.GeneralFailure)
		{
		}

		// Token: 0x0600184D RID: 6221 RVA: 0x0007BDE8 File Offset: 0x00079FE8
		public SmtpException(string message)
			: base(message)
		{
		}

		// Token: 0x0600184E RID: 6222 RVA: 0x0007BDF8 File Offset: 0x00079FF8
		public SmtpException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		// Token: 0x0600184F RID: 6223 RVA: 0x0007BE09 File Offset: 0x0007A009
		protected SmtpException(SerializationInfo serializationInfo, StreamingContext streamingContext)
			: base(serializationInfo, streamingContext)
		{
			this.statusCode = (SmtpStatusCode)serializationInfo.GetInt32("Status");
		}

		// Token: 0x06001850 RID: 6224 RVA: 0x0007BE2B File Offset: 0x0007A02B
		internal SmtpException(SmtpStatusCode statusCode, string serverMessage, bool serverResponse)
			: base(SmtpException.GetMessageForStatus(statusCode, serverMessage))
		{
			this.statusCode = statusCode;
		}

		// Token: 0x06001851 RID: 6225 RVA: 0x0007BE48 File Offset: 0x0007A048
		internal SmtpException(string message, string serverResponse)
			: base(message + " " + SR.GetString("MailServerResponse", new object[] { serverResponse }))
		{
		}

		// Token: 0x06001852 RID: 6226 RVA: 0x0007BE76 File Offset: 0x0007A076
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		void ISerializable.GetObjectData(SerializationInfo serializationInfo, StreamingContext streamingContext)
		{
			this.GetObjectData(serializationInfo, streamingContext);
		}

		// Token: 0x06001853 RID: 6227 RVA: 0x0007BE80 File Offset: 0x0007A080
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		public override void GetObjectData(SerializationInfo serializationInfo, StreamingContext streamingContext)
		{
			base.GetObjectData(serializationInfo, streamingContext);
			serializationInfo.AddValue("Status", (int)this.statusCode, typeof(int));
		}

		// Token: 0x1700050A RID: 1290
		// (get) Token: 0x06001854 RID: 6228 RVA: 0x0007BEAA File Offset: 0x0007A0AA
		// (set) Token: 0x06001855 RID: 6229 RVA: 0x0007BEB2 File Offset: 0x0007A0B2
		public SmtpStatusCode StatusCode
		{
			get
			{
				return this.statusCode;
			}
			set
			{
				this.statusCode = value;
			}
		}

		// Token: 0x0400184E RID: 6222
		private SmtpStatusCode statusCode = SmtpStatusCode.GeneralFailure;
	}
}
