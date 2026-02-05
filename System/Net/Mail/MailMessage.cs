using System;
using System.Collections.Specialized;
using System.Net.Mime;
using System.Text;

namespace System.Net.Mail
{
	// Token: 0x02000270 RID: 624
	public class MailMessage : IDisposable
	{
		// Token: 0x0600175A RID: 5978 RVA: 0x00077530 File Offset: 0x00075730
		public MailMessage()
		{
			this.message = new Message();
			if (Logging.On)
			{
				Logging.Associate(Logging.Web, this, this.message);
			}
			string from = SmtpClient.MailConfiguration.Smtp.From;
			if (from != null && from.Length > 0)
			{
				this.message.From = new MailAddress(from);
			}
		}

		// Token: 0x0600175B RID: 5979 RVA: 0x000775A8 File Offset: 0x000757A8
		public MailMessage(string from, string to)
		{
			if (from == null)
			{
				throw new ArgumentNullException("from");
			}
			if (to == null)
			{
				throw new ArgumentNullException("to");
			}
			if (from == string.Empty)
			{
				throw new ArgumentException(SR.GetString("net_emptystringcall", new object[] { "from" }), "from");
			}
			if (to == string.Empty)
			{
				throw new ArgumentException(SR.GetString("net_emptystringcall", new object[] { "to" }), "to");
			}
			this.message = new Message(from, to);
			if (Logging.On)
			{
				Logging.Associate(Logging.Web, this, this.message);
			}
		}

		// Token: 0x0600175C RID: 5980 RVA: 0x0007766E File Offset: 0x0007586E
		public MailMessage(string from, string to, string subject, string body)
			: this(from, to)
		{
			this.Subject = subject;
			this.Body = body;
		}

		// Token: 0x0600175D RID: 5981 RVA: 0x00077688 File Offset: 0x00075888
		public MailMessage(MailAddress from, MailAddress to)
		{
			if (from == null)
			{
				throw new ArgumentNullException("from");
			}
			if (to == null)
			{
				throw new ArgumentNullException("to");
			}
			this.message = new Message(from, to);
		}

		// Token: 0x170004CE RID: 1230
		// (get) Token: 0x0600175E RID: 5982 RVA: 0x000776D6 File Offset: 0x000758D6
		// (set) Token: 0x0600175F RID: 5983 RVA: 0x000776E3 File Offset: 0x000758E3
		public MailAddress From
		{
			get
			{
				return this.message.From;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				this.message.From = value;
			}
		}

		// Token: 0x170004CF RID: 1231
		// (get) Token: 0x06001760 RID: 5984 RVA: 0x000776FF File Offset: 0x000758FF
		// (set) Token: 0x06001761 RID: 5985 RVA: 0x0007770C File Offset: 0x0007590C
		public MailAddress Sender
		{
			get
			{
				return this.message.Sender;
			}
			set
			{
				this.message.Sender = value;
			}
		}

		// Token: 0x170004D0 RID: 1232
		// (get) Token: 0x06001762 RID: 5986 RVA: 0x0007771A File Offset: 0x0007591A
		// (set) Token: 0x06001763 RID: 5987 RVA: 0x00077727 File Offset: 0x00075927
		[Obsolete("ReplyTo is obsoleted for this type.  Please use ReplyToList instead which can accept multiple addresses. http://go.microsoft.com/fwlink/?linkid=14202")]
		public MailAddress ReplyTo
		{
			get
			{
				return this.message.ReplyTo;
			}
			set
			{
				this.message.ReplyTo = value;
			}
		}

		// Token: 0x170004D1 RID: 1233
		// (get) Token: 0x06001764 RID: 5988 RVA: 0x00077735 File Offset: 0x00075935
		public MailAddressCollection ReplyToList
		{
			get
			{
				return this.message.ReplyToList;
			}
		}

		// Token: 0x170004D2 RID: 1234
		// (get) Token: 0x06001765 RID: 5989 RVA: 0x00077742 File Offset: 0x00075942
		public MailAddressCollection To
		{
			get
			{
				return this.message.To;
			}
		}

		// Token: 0x170004D3 RID: 1235
		// (get) Token: 0x06001766 RID: 5990 RVA: 0x0007774F File Offset: 0x0007594F
		public MailAddressCollection Bcc
		{
			get
			{
				return this.message.Bcc;
			}
		}

		// Token: 0x170004D4 RID: 1236
		// (get) Token: 0x06001767 RID: 5991 RVA: 0x0007775C File Offset: 0x0007595C
		public MailAddressCollection CC
		{
			get
			{
				return this.message.CC;
			}
		}

		// Token: 0x170004D5 RID: 1237
		// (get) Token: 0x06001768 RID: 5992 RVA: 0x00077769 File Offset: 0x00075969
		// (set) Token: 0x06001769 RID: 5993 RVA: 0x00077776 File Offset: 0x00075976
		public MailPriority Priority
		{
			get
			{
				return this.message.Priority;
			}
			set
			{
				this.message.Priority = value;
			}
		}

		// Token: 0x170004D6 RID: 1238
		// (get) Token: 0x0600176A RID: 5994 RVA: 0x00077784 File Offset: 0x00075984
		// (set) Token: 0x0600176B RID: 5995 RVA: 0x0007778C File Offset: 0x0007598C
		public DeliveryNotificationOptions DeliveryNotificationOptions
		{
			get
			{
				return this.deliveryStatusNotification;
			}
			set
			{
				if ((DeliveryNotificationOptions.OnSuccess | DeliveryNotificationOptions.OnFailure | DeliveryNotificationOptions.Delay) < value && value != DeliveryNotificationOptions.Never)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this.deliveryStatusNotification = value;
			}
		}

		// Token: 0x170004D7 RID: 1239
		// (get) Token: 0x0600176C RID: 5996 RVA: 0x000777AC File Offset: 0x000759AC
		// (set) Token: 0x0600176D RID: 5997 RVA: 0x000777CC File Offset: 0x000759CC
		public string Subject
		{
			get
			{
				if (this.message.Subject == null)
				{
					return string.Empty;
				}
				return this.message.Subject;
			}
			set
			{
				this.message.Subject = value;
			}
		}

		// Token: 0x170004D8 RID: 1240
		// (get) Token: 0x0600176E RID: 5998 RVA: 0x000777DA File Offset: 0x000759DA
		// (set) Token: 0x0600176F RID: 5999 RVA: 0x000777E7 File Offset: 0x000759E7
		public Encoding SubjectEncoding
		{
			get
			{
				return this.message.SubjectEncoding;
			}
			set
			{
				this.message.SubjectEncoding = value;
			}
		}

		// Token: 0x170004D9 RID: 1241
		// (get) Token: 0x06001770 RID: 6000 RVA: 0x000777F5 File Offset: 0x000759F5
		public NameValueCollection Headers
		{
			get
			{
				return this.message.Headers;
			}
		}

		// Token: 0x170004DA RID: 1242
		// (get) Token: 0x06001771 RID: 6001 RVA: 0x00077802 File Offset: 0x00075A02
		// (set) Token: 0x06001772 RID: 6002 RVA: 0x0007780F File Offset: 0x00075A0F
		public Encoding HeadersEncoding
		{
			get
			{
				return this.message.HeadersEncoding;
			}
			set
			{
				this.message.HeadersEncoding = value;
			}
		}

		// Token: 0x170004DB RID: 1243
		// (get) Token: 0x06001773 RID: 6003 RVA: 0x0007781D File Offset: 0x00075A1D
		// (set) Token: 0x06001774 RID: 6004 RVA: 0x00077834 File Offset: 0x00075A34
		public string Body
		{
			get
			{
				if (this.body == null)
				{
					return string.Empty;
				}
				return this.body;
			}
			set
			{
				this.body = value;
				if (this.bodyEncoding == null && this.body != null)
				{
					if (MimeBasePart.IsAscii(this.body, true))
					{
						this.bodyEncoding = Encoding.ASCII;
						return;
					}
					this.bodyEncoding = Encoding.GetEncoding("utf-8");
				}
			}
		}

		// Token: 0x170004DC RID: 1244
		// (get) Token: 0x06001775 RID: 6005 RVA: 0x00077882 File Offset: 0x00075A82
		// (set) Token: 0x06001776 RID: 6006 RVA: 0x0007788A File Offset: 0x00075A8A
		public Encoding BodyEncoding
		{
			get
			{
				return this.bodyEncoding;
			}
			set
			{
				this.bodyEncoding = value;
			}
		}

		// Token: 0x170004DD RID: 1245
		// (get) Token: 0x06001777 RID: 6007 RVA: 0x00077893 File Offset: 0x00075A93
		// (set) Token: 0x06001778 RID: 6008 RVA: 0x0007789B File Offset: 0x00075A9B
		public TransferEncoding BodyTransferEncoding
		{
			get
			{
				return this.bodyTransferEncoding;
			}
			set
			{
				this.bodyTransferEncoding = value;
			}
		}

		// Token: 0x170004DE RID: 1246
		// (get) Token: 0x06001779 RID: 6009 RVA: 0x000778A4 File Offset: 0x00075AA4
		// (set) Token: 0x0600177A RID: 6010 RVA: 0x000778AC File Offset: 0x00075AAC
		public bool IsBodyHtml
		{
			get
			{
				return this.isBodyHtml;
			}
			set
			{
				this.isBodyHtml = value;
			}
		}

		// Token: 0x170004DF RID: 1247
		// (get) Token: 0x0600177B RID: 6011 RVA: 0x000778B5 File Offset: 0x00075AB5
		public AttachmentCollection Attachments
		{
			get
			{
				if (this.disposed)
				{
					throw new ObjectDisposedException(base.GetType().FullName);
				}
				if (this.attachments == null)
				{
					this.attachments = new AttachmentCollection();
				}
				return this.attachments;
			}
		}

		// Token: 0x170004E0 RID: 1248
		// (get) Token: 0x0600177C RID: 6012 RVA: 0x000778E9 File Offset: 0x00075AE9
		public AlternateViewCollection AlternateViews
		{
			get
			{
				if (this.disposed)
				{
					throw new ObjectDisposedException(base.GetType().FullName);
				}
				if (this.views == null)
				{
					this.views = new AlternateViewCollection();
				}
				return this.views;
			}
		}

		// Token: 0x0600177D RID: 6013 RVA: 0x0007791D File Offset: 0x00075B1D
		public void Dispose()
		{
			this.Dispose(true);
		}

		// Token: 0x0600177E RID: 6014 RVA: 0x00077928 File Offset: 0x00075B28
		protected virtual void Dispose(bool disposing)
		{
			if (disposing && !this.disposed)
			{
				this.disposed = true;
				if (this.views != null)
				{
					this.views.Dispose();
				}
				if (this.attachments != null)
				{
					this.attachments.Dispose();
				}
				if (this.bodyView != null)
				{
					this.bodyView.Dispose();
				}
			}
		}

		// Token: 0x0600177F RID: 6015 RVA: 0x00077980 File Offset: 0x00075B80
		private void SetContent(bool allowUnicode)
		{
			if (this.bodyView != null)
			{
				this.bodyView.Dispose();
				this.bodyView = null;
			}
			if (this.AlternateViews.Count == 0 && this.Attachments.Count == 0)
			{
				if (!string.IsNullOrEmpty(this.body))
				{
					this.bodyView = AlternateView.CreateAlternateViewFromString(this.body, this.bodyEncoding, this.isBodyHtml ? "text/html" : null);
					this.message.Content = this.bodyView.MimePart;
				}
			}
			else if (this.AlternateViews.Count == 0 && this.Attachments.Count > 0)
			{
				MimeMultiPart mimeMultiPart = new MimeMultiPart(MimeMultiPartType.Mixed);
				if (!string.IsNullOrEmpty(this.body))
				{
					this.bodyView = AlternateView.CreateAlternateViewFromString(this.body, this.bodyEncoding, this.isBodyHtml ? "text/html" : null);
				}
				else
				{
					this.bodyView = AlternateView.CreateAlternateViewFromString(string.Empty);
				}
				mimeMultiPart.Parts.Add(this.bodyView.MimePart);
				foreach (Attachment attachment in this.Attachments)
				{
					if (attachment != null)
					{
						attachment.PrepareForSending(allowUnicode);
						mimeMultiPart.Parts.Add(attachment.MimePart);
					}
				}
				this.message.Content = mimeMultiPart;
			}
			else
			{
				MimeMultiPart mimeMultiPart2 = null;
				MimeMultiPart mimeMultiPart3 = new MimeMultiPart(MimeMultiPartType.Alternative);
				if (!string.IsNullOrEmpty(this.body))
				{
					this.bodyView = AlternateView.CreateAlternateViewFromString(this.body, this.bodyEncoding, null);
					mimeMultiPart3.Parts.Add(this.bodyView.MimePart);
				}
				foreach (AlternateView alternateView in this.AlternateViews)
				{
					if (alternateView != null)
					{
						alternateView.PrepareForSending(allowUnicode);
						if (alternateView.LinkedResources.Count > 0)
						{
							MimeMultiPart mimeMultiPart4 = new MimeMultiPart(MimeMultiPartType.Related);
							mimeMultiPart4.ContentType.Parameters["type"] = alternateView.ContentType.MediaType;
							mimeMultiPart4.ContentLocation = alternateView.MimePart.ContentLocation;
							mimeMultiPart4.Parts.Add(alternateView.MimePart);
							foreach (LinkedResource linkedResource in alternateView.LinkedResources)
							{
								linkedResource.PrepareForSending(allowUnicode);
								mimeMultiPart4.Parts.Add(linkedResource.MimePart);
							}
							mimeMultiPart3.Parts.Add(mimeMultiPart4);
						}
						else
						{
							mimeMultiPart3.Parts.Add(alternateView.MimePart);
						}
					}
				}
				if (this.Attachments.Count > 0)
				{
					mimeMultiPart2 = new MimeMultiPart(MimeMultiPartType.Mixed);
					mimeMultiPart2.Parts.Add(mimeMultiPart3);
					MimeMultiPart mimeMultiPart5 = new MimeMultiPart(MimeMultiPartType.Mixed);
					foreach (Attachment attachment2 in this.Attachments)
					{
						if (attachment2 != null)
						{
							attachment2.PrepareForSending(allowUnicode);
							mimeMultiPart5.Parts.Add(attachment2.MimePart);
						}
					}
					mimeMultiPart2.Parts.Add(mimeMultiPart5);
					this.message.Content = mimeMultiPart2;
				}
				else if (mimeMultiPart3.Parts.Count == 1 && string.IsNullOrEmpty(this.body))
				{
					this.message.Content = mimeMultiPart3.Parts[0];
				}
				else
				{
					this.message.Content = mimeMultiPart3;
				}
			}
			if (this.bodyView != null && this.bodyTransferEncoding != TransferEncoding.Unknown)
			{
				this.bodyView.TransferEncoding = this.bodyTransferEncoding;
			}
		}

		// Token: 0x06001780 RID: 6016 RVA: 0x00077D78 File Offset: 0x00075F78
		internal void Send(BaseWriter writer, bool sendEnvelope, bool allowUnicode)
		{
			this.SetContent(allowUnicode);
			this.message.Send(writer, sendEnvelope, allowUnicode);
		}

		// Token: 0x06001781 RID: 6017 RVA: 0x00077D8F File Offset: 0x00075F8F
		internal IAsyncResult BeginSend(BaseWriter writer, bool sendEnvelope, bool allowUnicode, AsyncCallback callback, object state)
		{
			this.SetContent(allowUnicode);
			return this.message.BeginSend(writer, sendEnvelope, allowUnicode, callback, state);
		}

		// Token: 0x06001782 RID: 6018 RVA: 0x00077DAA File Offset: 0x00075FAA
		internal void EndSend(IAsyncResult asyncResult)
		{
			this.message.EndSend(asyncResult);
		}

		// Token: 0x06001783 RID: 6019 RVA: 0x00077DB8 File Offset: 0x00075FB8
		internal string BuildDeliveryStatusNotificationString()
		{
			if (this.deliveryStatusNotification == DeliveryNotificationOptions.None)
			{
				return string.Empty;
			}
			StringBuilder stringBuilder = new StringBuilder(" NOTIFY=");
			bool flag = false;
			if (this.deliveryStatusNotification == DeliveryNotificationOptions.Never)
			{
				stringBuilder.Append("NEVER");
				return stringBuilder.ToString();
			}
			if ((this.deliveryStatusNotification & DeliveryNotificationOptions.OnSuccess) > DeliveryNotificationOptions.None)
			{
				stringBuilder.Append("SUCCESS");
				flag = true;
			}
			if ((this.deliveryStatusNotification & DeliveryNotificationOptions.OnFailure) > DeliveryNotificationOptions.None)
			{
				if (flag)
				{
					stringBuilder.Append(",");
				}
				stringBuilder.Append("FAILURE");
				flag = true;
			}
			if ((this.deliveryStatusNotification & DeliveryNotificationOptions.Delay) > DeliveryNotificationOptions.None)
			{
				if (flag)
				{
					stringBuilder.Append(",");
				}
				stringBuilder.Append("DELAY");
			}
			return stringBuilder.ToString();
		}

		// Token: 0x040017D2 RID: 6098
		private AlternateViewCollection views;

		// Token: 0x040017D3 RID: 6099
		private AttachmentCollection attachments;

		// Token: 0x040017D4 RID: 6100
		private AlternateView bodyView;

		// Token: 0x040017D5 RID: 6101
		private string body = string.Empty;

		// Token: 0x040017D6 RID: 6102
		private Encoding bodyEncoding;

		// Token: 0x040017D7 RID: 6103
		private TransferEncoding bodyTransferEncoding = TransferEncoding.Unknown;

		// Token: 0x040017D8 RID: 6104
		private bool isBodyHtml;

		// Token: 0x040017D9 RID: 6105
		private bool disposed;

		// Token: 0x040017DA RID: 6106
		private Message message;

		// Token: 0x040017DB RID: 6107
		private DeliveryNotificationOptions deliveryStatusNotification;
	}
}
