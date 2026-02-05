using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.IO;
using System.Net.Mime;

namespace System.Net.Mail
{
	// Token: 0x02000299 RID: 665
	internal class SendMailAsyncResult : LazyAsyncResult
	{
		// Token: 0x060018B5 RID: 6325 RVA: 0x0007D55D File Offset: 0x0007B75D
		internal SendMailAsyncResult(SmtpConnection connection, MailAddress from, MailAddressCollection toCollection, bool allowUnicode, string deliveryNotify, AsyncCallback callback, object state)
			: base(null, state, callback)
		{
			this.toCollection = toCollection;
			this.connection = connection;
			this.from = from;
			this.deliveryNotify = deliveryNotify;
			this.allowUnicode = allowUnicode;
		}

		// Token: 0x060018B6 RID: 6326 RVA: 0x0007D59A File Offset: 0x0007B79A
		internal void Send()
		{
			this.SendMailFrom();
		}

		// Token: 0x060018B7 RID: 6327 RVA: 0x0007D5A4 File Offset: 0x0007B7A4
		internal static MailWriter End(IAsyncResult result)
		{
			SendMailAsyncResult sendMailAsyncResult = (SendMailAsyncResult)result;
			object obj = sendMailAsyncResult.InternalWaitForCompletion();
			if (obj is Exception && (!(obj is SmtpFailedRecipientException) || ((SmtpFailedRecipientException)obj).fatal))
			{
				throw (Exception)obj;
			}
			return new MailWriter(sendMailAsyncResult.stream);
		}

		// Token: 0x060018B8 RID: 6328 RVA: 0x0007D5F0 File Offset: 0x0007B7F0
		private void SendMailFrom()
		{
			IAsyncResult asyncResult = MailCommand.BeginSend(this.connection, SmtpCommands.Mail, this.from, this.allowUnicode, SendMailAsyncResult.sendMailFromCompleted, this);
			if (!asyncResult.CompletedSynchronously)
			{
				return;
			}
			MailCommand.EndSend(asyncResult);
			this.SendToCollection();
		}

		// Token: 0x060018B9 RID: 6329 RVA: 0x0007D638 File Offset: 0x0007B838
		private static void SendMailFromCompleted(IAsyncResult result)
		{
			if (!result.CompletedSynchronously)
			{
				SendMailAsyncResult sendMailAsyncResult = (SendMailAsyncResult)result.AsyncState;
				try
				{
					MailCommand.EndSend(result);
					sendMailAsyncResult.SendToCollection();
				}
				catch (Exception ex)
				{
					sendMailAsyncResult.InvokeCallback(ex);
				}
			}
		}

		// Token: 0x060018BA RID: 6330 RVA: 0x0007D684 File Offset: 0x0007B884
		private void SendToCollection()
		{
			while (this.toIndex < this.toCollection.Count)
			{
				SmtpConnection smtpConnection = this.connection;
				Collection<MailAddress> collection = this.toCollection;
				int num = this.toIndex;
				this.toIndex = num + 1;
				MultiAsyncResult multiAsyncResult = (MultiAsyncResult)RecipientCommand.BeginSend(smtpConnection, collection[num].GetSmtpAddress(this.allowUnicode) + this.deliveryNotify, SendMailAsyncResult.sendToCollectionCompleted, this);
				if (!multiAsyncResult.CompletedSynchronously)
				{
					return;
				}
				string text;
				if (!RecipientCommand.EndSend(multiAsyncResult, out text))
				{
					this.failedRecipientExceptions.Add(new SmtpFailedRecipientException(this.connection.Reader.StatusCode, this.toCollection[this.toIndex - 1].GetSmtpAddress(this.allowUnicode), text));
				}
			}
			this.SendData();
		}

		// Token: 0x060018BB RID: 6331 RVA: 0x0007D750 File Offset: 0x0007B950
		private static void SendToCollectionCompleted(IAsyncResult result)
		{
			if (!result.CompletedSynchronously)
			{
				SendMailAsyncResult sendMailAsyncResult = (SendMailAsyncResult)result.AsyncState;
				try
				{
					string text;
					if (!RecipientCommand.EndSend(result, out text))
					{
						sendMailAsyncResult.failedRecipientExceptions.Add(new SmtpFailedRecipientException(sendMailAsyncResult.connection.Reader.StatusCode, sendMailAsyncResult.toCollection[sendMailAsyncResult.toIndex - 1].GetSmtpAddress(sendMailAsyncResult.allowUnicode), text));
						if (sendMailAsyncResult.failedRecipientExceptions.Count == sendMailAsyncResult.toCollection.Count)
						{
							SmtpFailedRecipientException ex;
							if (sendMailAsyncResult.toCollection.Count == 1)
							{
								ex = (SmtpFailedRecipientException)sendMailAsyncResult.failedRecipientExceptions[0];
							}
							else
							{
								ex = new SmtpFailedRecipientsException(sendMailAsyncResult.failedRecipientExceptions, true);
							}
							ex.fatal = true;
							sendMailAsyncResult.InvokeCallback(ex);
							return;
						}
					}
					sendMailAsyncResult.SendToCollection();
				}
				catch (Exception ex2)
				{
					sendMailAsyncResult.InvokeCallback(ex2);
				}
			}
		}

		// Token: 0x060018BC RID: 6332 RVA: 0x0007D83C File Offset: 0x0007BA3C
		private void SendData()
		{
			IAsyncResult asyncResult = DataCommand.BeginSend(this.connection, SendMailAsyncResult.sendDataCompleted, this);
			if (!asyncResult.CompletedSynchronously)
			{
				return;
			}
			DataCommand.EndSend(asyncResult);
			this.stream = this.connection.GetClosableStream();
			if (this.failedRecipientExceptions.Count > 1)
			{
				base.InvokeCallback(new SmtpFailedRecipientsException(this.failedRecipientExceptions, this.failedRecipientExceptions.Count == this.toCollection.Count));
				return;
			}
			if (this.failedRecipientExceptions.Count == 1)
			{
				base.InvokeCallback(this.failedRecipientExceptions[0]);
				return;
			}
			base.InvokeCallback();
		}

		// Token: 0x060018BD RID: 6333 RVA: 0x0007D8DC File Offset: 0x0007BADC
		private static void SendDataCompleted(IAsyncResult result)
		{
			if (!result.CompletedSynchronously)
			{
				SendMailAsyncResult sendMailAsyncResult = (SendMailAsyncResult)result.AsyncState;
				try
				{
					DataCommand.EndSend(result);
					sendMailAsyncResult.stream = sendMailAsyncResult.connection.GetClosableStream();
					if (sendMailAsyncResult.failedRecipientExceptions.Count > 1)
					{
						sendMailAsyncResult.InvokeCallback(new SmtpFailedRecipientsException(sendMailAsyncResult.failedRecipientExceptions, sendMailAsyncResult.failedRecipientExceptions.Count == sendMailAsyncResult.toCollection.Count));
					}
					else if (sendMailAsyncResult.failedRecipientExceptions.Count == 1)
					{
						sendMailAsyncResult.InvokeCallback(sendMailAsyncResult.failedRecipientExceptions[0]);
					}
					else
					{
						sendMailAsyncResult.InvokeCallback();
					}
				}
				catch (Exception ex)
				{
					sendMailAsyncResult.InvokeCallback(ex);
				}
			}
		}

		// Token: 0x060018BE RID: 6334 RVA: 0x0007D994 File Offset: 0x0007BB94
		internal SmtpFailedRecipientException GetFailedRecipientException()
		{
			if (this.failedRecipientExceptions.Count == 1)
			{
				return (SmtpFailedRecipientException)this.failedRecipientExceptions[0];
			}
			if (this.failedRecipientExceptions.Count > 1)
			{
				return new SmtpFailedRecipientsException(this.failedRecipientExceptions, false);
			}
			return null;
		}

		// Token: 0x04001894 RID: 6292
		private SmtpConnection connection;

		// Token: 0x04001895 RID: 6293
		private MailAddress from;

		// Token: 0x04001896 RID: 6294
		private string deliveryNotify;

		// Token: 0x04001897 RID: 6295
		private static AsyncCallback sendMailFromCompleted = new AsyncCallback(SendMailAsyncResult.SendMailFromCompleted);

		// Token: 0x04001898 RID: 6296
		private static AsyncCallback sendToCollectionCompleted = new AsyncCallback(SendMailAsyncResult.SendToCollectionCompleted);

		// Token: 0x04001899 RID: 6297
		private static AsyncCallback sendDataCompleted = new AsyncCallback(SendMailAsyncResult.SendDataCompleted);

		// Token: 0x0400189A RID: 6298
		private ArrayList failedRecipientExceptions = new ArrayList();

		// Token: 0x0400189B RID: 6299
		private Stream stream;

		// Token: 0x0400189C RID: 6300
		private MailAddressCollection toCollection;

		// Token: 0x0400189D RID: 6301
		private int toIndex;

		// Token: 0x0400189E RID: 6302
		private bool allowUnicode;
	}
}
