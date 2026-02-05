using System;
using System.Collections;
using System.Security.Cryptography.X509Certificates;

namespace System.Net.Mail
{
	// Token: 0x02000298 RID: 664
	internal class SmtpTransport
	{
		// Token: 0x0600189E RID: 6302 RVA: 0x0007D0AE File Offset: 0x0007B2AE
		internal SmtpTransport(SmtpClient client)
			: this(client, SmtpAuthenticationManager.GetModules())
		{
		}

		// Token: 0x0600189F RID: 6303 RVA: 0x0007D0BC File Offset: 0x0007B2BC
		internal SmtpTransport(SmtpClient client, ISmtpAuthenticationModule[] authenticationModules)
		{
			this.client = client;
			if (authenticationModules == null)
			{
				throw new ArgumentNullException("authenticationModules");
			}
			this.authenticationModules = authenticationModules;
		}

		// Token: 0x17000514 RID: 1300
		// (get) Token: 0x060018A0 RID: 6304 RVA: 0x0007D0F6 File Offset: 0x0007B2F6
		// (set) Token: 0x060018A1 RID: 6305 RVA: 0x0007D0FE File Offset: 0x0007B2FE
		internal ICredentialsByHost Credentials
		{
			get
			{
				return this.credentials;
			}
			set
			{
				this.credentials = value;
			}
		}

		// Token: 0x17000515 RID: 1301
		// (get) Token: 0x060018A2 RID: 6306 RVA: 0x0007D107 File Offset: 0x0007B307
		// (set) Token: 0x060018A3 RID: 6307 RVA: 0x0007D10F File Offset: 0x0007B30F
		internal bool IdentityRequired
		{
			get
			{
				return this.m_IdentityRequired;
			}
			set
			{
				this.m_IdentityRequired = value;
			}
		}

		// Token: 0x17000516 RID: 1302
		// (get) Token: 0x060018A4 RID: 6308 RVA: 0x0007D118 File Offset: 0x0007B318
		internal bool IsConnected
		{
			get
			{
				return this.connection != null && this.connection.IsConnected;
			}
		}

		// Token: 0x17000517 RID: 1303
		// (get) Token: 0x060018A5 RID: 6309 RVA: 0x0007D12F File Offset: 0x0007B32F
		// (set) Token: 0x060018A6 RID: 6310 RVA: 0x0007D137 File Offset: 0x0007B337
		internal int Timeout
		{
			get
			{
				return this.timeout;
			}
			set
			{
				if (value < 0)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this.timeout = value;
			}
		}

		// Token: 0x17000518 RID: 1304
		// (get) Token: 0x060018A7 RID: 6311 RVA: 0x0007D14F File Offset: 0x0007B34F
		// (set) Token: 0x060018A8 RID: 6312 RVA: 0x0007D157 File Offset: 0x0007B357
		internal bool EnableSsl
		{
			get
			{
				return this.enableSsl;
			}
			set
			{
				this.enableSsl = value;
			}
		}

		// Token: 0x17000519 RID: 1305
		// (get) Token: 0x060018A9 RID: 6313 RVA: 0x0007D160 File Offset: 0x0007B360
		internal X509CertificateCollection ClientCertificates
		{
			get
			{
				if (this.clientCertificates == null)
				{
					this.clientCertificates = new X509CertificateCollection();
				}
				return this.clientCertificates;
			}
		}

		// Token: 0x1700051A RID: 1306
		// (get) Token: 0x060018AA RID: 6314 RVA: 0x0007D17B File Offset: 0x0007B37B
		internal bool ServerSupportsEai
		{
			get
			{
				return this.connection != null && this.connection.ServerSupportsEai;
			}
		}

		// Token: 0x060018AB RID: 6315 RVA: 0x0007D194 File Offset: 0x0007B394
		private void UpdateServicePoint(ServicePoint servicePoint)
		{
			if (this.lastUsedServicePoint == null)
			{
				this.lastUsedServicePoint = servicePoint;
				return;
			}
			if (this.lastUsedServicePoint.Host != servicePoint.Host || this.lastUsedServicePoint.Port != servicePoint.Port)
			{
				ConnectionPoolManager.CleanupConnectionPool(servicePoint, "");
				this.lastUsedServicePoint = servicePoint;
			}
		}

		// Token: 0x060018AC RID: 6316 RVA: 0x0007D1F0 File Offset: 0x0007B3F0
		internal void GetConnection(ServicePoint servicePoint)
		{
			this.UpdateServicePoint(servicePoint);
			this.connection = new SmtpConnection(this, this.client, this.credentials, this.authenticationModules);
			this.connection.Timeout = this.timeout;
			if (Logging.On)
			{
				Logging.Associate(Logging.Web, this, this.connection);
			}
			if (this.EnableSsl)
			{
				this.connection.EnableSsl = true;
				this.connection.ClientCertificates = this.ClientCertificates;
			}
			this.connection.GetConnection(servicePoint);
		}

		// Token: 0x060018AD RID: 6317 RVA: 0x0007D27C File Offset: 0x0007B47C
		internal IAsyncResult BeginGetConnection(ServicePoint servicePoint, ContextAwareResult outerResult, AsyncCallback callback, object state)
		{
			IAsyncResult asyncResult = null;
			try
			{
				this.UpdateServicePoint(servicePoint);
				this.connection = new SmtpConnection(this, this.client, this.credentials, this.authenticationModules);
				this.connection.Timeout = this.timeout;
				if (Logging.On)
				{
					Logging.Associate(Logging.Web, this, this.connection);
				}
				if (this.EnableSsl)
				{
					this.connection.EnableSsl = true;
					this.connection.ClientCertificates = this.ClientCertificates;
				}
				asyncResult = this.connection.BeginGetConnection(servicePoint, outerResult, callback, state);
			}
			catch (Exception ex)
			{
				throw new SmtpException(global::System.SR.GetString("MailHostNotFound"), ex);
			}
			return asyncResult;
		}

		// Token: 0x060018AE RID: 6318 RVA: 0x0007D334 File Offset: 0x0007B534
		internal void EndGetConnection(IAsyncResult result)
		{
			try
			{
				this.connection.EndGetConnection(result);
			}
			finally
			{
			}
		}

		// Token: 0x060018AF RID: 6319 RVA: 0x0007D360 File Offset: 0x0007B560
		internal IAsyncResult BeginSendMail(MailAddress sender, MailAddressCollection recipients, string deliveryNotify, bool allowUnicode, AsyncCallback callback, object state)
		{
			if (sender == null)
			{
				throw new ArgumentNullException("sender");
			}
			if (recipients == null)
			{
				throw new ArgumentNullException("recipients");
			}
			SendMailAsyncResult sendMailAsyncResult = new SendMailAsyncResult(this.connection, sender, recipients, allowUnicode, this.connection.DSNEnabled ? deliveryNotify : null, callback, state);
			sendMailAsyncResult.Send();
			return sendMailAsyncResult;
		}

		// Token: 0x060018B0 RID: 6320 RVA: 0x0007D3B5 File Offset: 0x0007B5B5
		internal void ReleaseConnection()
		{
			if (this.connection != null)
			{
				this.connection.ReleaseConnection();
			}
		}

		// Token: 0x060018B1 RID: 6321 RVA: 0x0007D3CA File Offset: 0x0007B5CA
		internal void Abort()
		{
			if (this.connection != null)
			{
				this.connection.Abort();
			}
		}

		// Token: 0x060018B2 RID: 6322 RVA: 0x0007D3DF File Offset: 0x0007B5DF
		internal MailWriter EndSendMail(IAsyncResult result)
		{
			return SendMailAsyncResult.End(result);
		}

		// Token: 0x060018B3 RID: 6323 RVA: 0x0007D3E8 File Offset: 0x0007B5E8
		internal MailWriter SendMail(MailAddress sender, MailAddressCollection recipients, string deliveryNotify, bool allowUnicode, out SmtpFailedRecipientException exception)
		{
			if (sender == null)
			{
				throw new ArgumentNullException("sender");
			}
			if (recipients == null)
			{
				throw new ArgumentNullException("recipients");
			}
			MailCommand.Send(this.connection, SmtpCommands.Mail, sender, allowUnicode);
			this.failedRecipientExceptions.Clear();
			exception = null;
			foreach (MailAddress mailAddress in recipients)
			{
				string smtpAddress = mailAddress.GetSmtpAddress(allowUnicode);
				string text = smtpAddress + (this.connection.DSNEnabled ? deliveryNotify : string.Empty);
				string text2;
				if (!RecipientCommand.Send(this.connection, text, out text2))
				{
					this.failedRecipientExceptions.Add(new SmtpFailedRecipientException(this.connection.Reader.StatusCode, smtpAddress, text2));
				}
			}
			if (this.failedRecipientExceptions.Count > 0)
			{
				if (this.failedRecipientExceptions.Count == 1)
				{
					exception = (SmtpFailedRecipientException)this.failedRecipientExceptions[0];
				}
				else
				{
					exception = new SmtpFailedRecipientsException(this.failedRecipientExceptions, this.failedRecipientExceptions.Count == recipients.Count);
				}
				if (this.failedRecipientExceptions.Count == recipients.Count)
				{
					exception.fatal = true;
					throw exception;
				}
			}
			DataCommand.Send(this.connection);
			return new MailWriter(this.connection.GetClosableStream());
		}

		// Token: 0x060018B4 RID: 6324 RVA: 0x0007D550 File Offset: 0x0007B750
		internal void CloseIdleConnections(ServicePoint servicePoint)
		{
			ConnectionPoolManager.CleanupConnectionPool(servicePoint, "");
		}

		// Token: 0x04001889 RID: 6281
		internal const int DefaultPort = 25;

		// Token: 0x0400188A RID: 6282
		private ISmtpAuthenticationModule[] authenticationModules;

		// Token: 0x0400188B RID: 6283
		private SmtpConnection connection;

		// Token: 0x0400188C RID: 6284
		private SmtpClient client;

		// Token: 0x0400188D RID: 6285
		private ICredentialsByHost credentials;

		// Token: 0x0400188E RID: 6286
		private int timeout = 100000;

		// Token: 0x0400188F RID: 6287
		private ArrayList failedRecipientExceptions = new ArrayList();

		// Token: 0x04001890 RID: 6288
		private bool m_IdentityRequired;

		// Token: 0x04001891 RID: 6289
		private bool enableSsl;

		// Token: 0x04001892 RID: 6290
		private X509CertificateCollection clientCertificates;

		// Token: 0x04001893 RID: 6291
		private ServicePoint lastUsedServicePoint;
	}
}
