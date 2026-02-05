using System;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Net.Configuration;
using System.Net.NetworkInformation;
using System.Security;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Security.Permissions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace System.Net.Mail
{
	// Token: 0x0200027B RID: 635
	public class SmtpClient : IDisposable
	{
		// Token: 0x14000020 RID: 32
		// (add) Token: 0x060017B5 RID: 6069 RVA: 0x00078C8C File Offset: 0x00076E8C
		// (remove) Token: 0x060017B6 RID: 6070 RVA: 0x00078CC4 File Offset: 0x00076EC4
		public event SendCompletedEventHandler SendCompleted;

		// Token: 0x060017B7 RID: 6071 RVA: 0x00078CFC File Offset: 0x00076EFC
		public SmtpClient()
		{
			if (Logging.On)
			{
				Logging.Enter(Logging.Web, "SmtpClient", ".ctor", "");
			}
			try
			{
				this.Initialize();
			}
			finally
			{
				if (Logging.On)
				{
					Logging.Exit(Logging.Web, "SmtpClient", ".ctor", this);
				}
			}
		}

		// Token: 0x060017B8 RID: 6072 RVA: 0x00078D68 File Offset: 0x00076F68
		public SmtpClient(string host)
		{
			if (Logging.On)
			{
				Logging.Enter(Logging.Web, "SmtpClient", ".ctor", "host=" + host);
			}
			try
			{
				this.host = host;
				this.Initialize();
			}
			finally
			{
				if (Logging.On)
				{
					Logging.Exit(Logging.Web, "SmtpClient", ".ctor", this);
				}
			}
		}

		// Token: 0x060017B9 RID: 6073 RVA: 0x00078DE0 File Offset: 0x00076FE0
		public SmtpClient(string host, int port)
		{
			if (Logging.On)
			{
				Logging.Enter(Logging.Web, "SmtpClient", ".ctor", "host=" + host + ", port=" + port.ToString());
			}
			try
			{
				if (port < 0)
				{
					throw new ArgumentOutOfRangeException("port");
				}
				this.host = host;
				this.port = port;
				this.Initialize();
			}
			finally
			{
				if (Logging.On)
				{
					Logging.Exit(Logging.Web, "SmtpClient", ".ctor", this);
				}
			}
		}

		// Token: 0x060017BA RID: 6074 RVA: 0x00078E78 File Offset: 0x00077078
		private void Initialize()
		{
			if (this.port == SmtpClient.defaultPort || this.port == 0)
			{
				new SmtpPermission(SmtpAccess.Connect).Demand();
			}
			else
			{
				new SmtpPermission(SmtpAccess.ConnectToUnrestrictedPort).Demand();
			}
			this.transport = new SmtpTransport(this);
			if (Logging.On)
			{
				Logging.Associate(Logging.Web, this, this.transport);
			}
			this.onSendCompletedDelegate = new SendOrPostCallback(this.SendCompletedWaitCallback);
			if (SmtpClient.MailConfiguration.Smtp != null)
			{
				if (SmtpClient.MailConfiguration.Smtp.Network != null)
				{
					if (this.host == null || this.host.Length == 0)
					{
						this.host = SmtpClient.MailConfiguration.Smtp.Network.Host;
					}
					if (this.port == 0)
					{
						this.port = SmtpClient.MailConfiguration.Smtp.Network.Port;
					}
					this.transport.Credentials = SmtpClient.MailConfiguration.Smtp.Network.Credential;
					this.transport.EnableSsl = SmtpClient.MailConfiguration.Smtp.Network.EnableSsl;
					if (SmtpClient.MailConfiguration.Smtp.Network.TargetName != null)
					{
						this.targetName = SmtpClient.MailConfiguration.Smtp.Network.TargetName;
					}
					this.clientDomain = SmtpClient.MailConfiguration.Smtp.Network.ClientDomain;
				}
				this.deliveryFormat = SmtpClient.MailConfiguration.Smtp.DeliveryFormat;
				this.deliveryMethod = SmtpClient.MailConfiguration.Smtp.DeliveryMethod;
				if (SmtpClient.MailConfiguration.Smtp.SpecifiedPickupDirectory != null)
				{
					this.pickupDirectoryLocation = SmtpClient.MailConfiguration.Smtp.SpecifiedPickupDirectory.PickupDirectoryLocation;
				}
			}
			if (this.host != null && this.host.Length != 0)
			{
				this.host = this.host.Trim();
			}
			if (this.port == 0)
			{
				this.port = SmtpClient.defaultPort;
			}
			if (this.targetName == null)
			{
				this.targetName = "SMTPSVC/" + this.host;
			}
			if (this.clientDomain == null)
			{
				string text = IPGlobalProperties.InternalGetIPGlobalProperties().HostName;
				IdnMapping idnMapping = new IdnMapping();
				try
				{
					text = idnMapping.GetAscii(text);
				}
				catch (ArgumentException)
				{
				}
				StringBuilder stringBuilder = new StringBuilder();
				foreach (char c in text)
				{
					if (c <= '\u007f')
					{
						stringBuilder.Append(c);
					}
				}
				if (stringBuilder.Length > 0)
				{
					this.clientDomain = stringBuilder.ToString();
					return;
				}
				this.clientDomain = "LocalHost";
			}
		}

		// Token: 0x170004EF RID: 1263
		// (get) Token: 0x060017BB RID: 6075 RVA: 0x00079118 File Offset: 0x00077318
		// (set) Token: 0x060017BC RID: 6076 RVA: 0x00079120 File Offset: 0x00077320
		public string Host
		{
			get
			{
				return this.host;
			}
			set
			{
				if (this.InCall)
				{
					throw new InvalidOperationException(global::System.SR.GetString("SmtpInvalidOperationDuringSend"));
				}
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				if (value == string.Empty)
				{
					throw new ArgumentException(global::System.SR.GetString("net_emptystringset"), "value");
				}
				value = value.Trim();
				if (value != this.host)
				{
					this.host = value;
					this.servicePointChanged = true;
				}
			}
		}

		// Token: 0x170004F0 RID: 1264
		// (get) Token: 0x060017BD RID: 6077 RVA: 0x00079199 File Offset: 0x00077399
		// (set) Token: 0x060017BE RID: 6078 RVA: 0x000791A4 File Offset: 0x000773A4
		public int Port
		{
			get
			{
				return this.port;
			}
			set
			{
				if (this.InCall)
				{
					throw new InvalidOperationException(global::System.SR.GetString("SmtpInvalidOperationDuringSend"));
				}
				if (value <= 0)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				if (value != SmtpClient.defaultPort)
				{
					new SmtpPermission(SmtpAccess.ConnectToUnrestrictedPort).Demand();
				}
				if (value != this.port)
				{
					this.port = value;
					this.servicePointChanged = true;
				}
			}
		}

		// Token: 0x170004F1 RID: 1265
		// (get) Token: 0x060017BF RID: 6079 RVA: 0x00079202 File Offset: 0x00077402
		// (set) Token: 0x060017C0 RID: 6080 RVA: 0x00079219 File Offset: 0x00077419
		public bool UseDefaultCredentials
		{
			get
			{
				return this.transport.Credentials is SystemNetworkCredential;
			}
			set
			{
				if (this.InCall)
				{
					throw new InvalidOperationException(global::System.SR.GetString("SmtpInvalidOperationDuringSend"));
				}
				this.transport.Credentials = (value ? CredentialCache.DefaultNetworkCredentials : null);
			}
		}

		// Token: 0x170004F2 RID: 1266
		// (get) Token: 0x060017C1 RID: 6081 RVA: 0x00079249 File Offset: 0x00077449
		// (set) Token: 0x060017C2 RID: 6082 RVA: 0x00079256 File Offset: 0x00077456
		public ICredentialsByHost Credentials
		{
			get
			{
				return this.transport.Credentials;
			}
			set
			{
				if (this.InCall)
				{
					throw new InvalidOperationException(global::System.SR.GetString("SmtpInvalidOperationDuringSend"));
				}
				this.transport.Credentials = value;
			}
		}

		// Token: 0x170004F3 RID: 1267
		// (get) Token: 0x060017C3 RID: 6083 RVA: 0x0007927C File Offset: 0x0007747C
		// (set) Token: 0x060017C4 RID: 6084 RVA: 0x00079289 File Offset: 0x00077489
		public int Timeout
		{
			get
			{
				return this.transport.Timeout;
			}
			set
			{
				if (this.InCall)
				{
					throw new InvalidOperationException(global::System.SR.GetString("SmtpInvalidOperationDuringSend"));
				}
				if (value < 0)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this.transport.Timeout = value;
			}
		}

		// Token: 0x170004F4 RID: 1268
		// (get) Token: 0x060017C5 RID: 6085 RVA: 0x000792BE File Offset: 0x000774BE
		public ServicePoint ServicePoint
		{
			get
			{
				this.CheckHostAndPort();
				if (this.servicePoint == null || this.servicePointChanged)
				{
					this.servicePoint = ServicePointManager.FindServicePoint(this.host, this.port);
					this.servicePointChanged = false;
				}
				return this.servicePoint;
			}
		}

		// Token: 0x170004F5 RID: 1269
		// (get) Token: 0x060017C6 RID: 6086 RVA: 0x000792FA File Offset: 0x000774FA
		// (set) Token: 0x060017C7 RID: 6087 RVA: 0x00079302 File Offset: 0x00077502
		public SmtpDeliveryMethod DeliveryMethod
		{
			get
			{
				return this.deliveryMethod;
			}
			set
			{
				this.deliveryMethod = value;
			}
		}

		// Token: 0x170004F6 RID: 1270
		// (get) Token: 0x060017C8 RID: 6088 RVA: 0x0007930B File Offset: 0x0007750B
		// (set) Token: 0x060017C9 RID: 6089 RVA: 0x00079313 File Offset: 0x00077513
		public SmtpDeliveryFormat DeliveryFormat
		{
			get
			{
				return this.deliveryFormat;
			}
			set
			{
				this.deliveryFormat = value;
			}
		}

		// Token: 0x170004F7 RID: 1271
		// (get) Token: 0x060017CA RID: 6090 RVA: 0x0007931C File Offset: 0x0007751C
		// (set) Token: 0x060017CB RID: 6091 RVA: 0x00079324 File Offset: 0x00077524
		public string PickupDirectoryLocation
		{
			get
			{
				return this.pickupDirectoryLocation;
			}
			set
			{
				this.pickupDirectoryLocation = value;
			}
		}

		// Token: 0x170004F8 RID: 1272
		// (get) Token: 0x060017CC RID: 6092 RVA: 0x0007932D File Offset: 0x0007752D
		// (set) Token: 0x060017CD RID: 6093 RVA: 0x0007933A File Offset: 0x0007753A
		public bool EnableSsl
		{
			get
			{
				return this.transport.EnableSsl;
			}
			set
			{
				this.transport.EnableSsl = value;
			}
		}

		// Token: 0x170004F9 RID: 1273
		// (get) Token: 0x060017CE RID: 6094 RVA: 0x00079348 File Offset: 0x00077548
		public X509CertificateCollection ClientCertificates
		{
			get
			{
				return this.transport.ClientCertificates;
			}
		}

		// Token: 0x170004FA RID: 1274
		// (get) Token: 0x060017D0 RID: 6096 RVA: 0x0007935E File Offset: 0x0007755E
		// (set) Token: 0x060017CF RID: 6095 RVA: 0x00079355 File Offset: 0x00077555
		public string TargetName
		{
			get
			{
				return this.targetName;
			}
			set
			{
				this.targetName = value;
			}
		}

		// Token: 0x170004FB RID: 1275
		// (get) Token: 0x060017D1 RID: 6097 RVA: 0x00079366 File Offset: 0x00077566
		private bool ServerSupportsEai
		{
			get
			{
				return this.transport.ServerSupportsEai;
			}
		}

		// Token: 0x060017D2 RID: 6098 RVA: 0x00079373 File Offset: 0x00077573
		private bool IsUnicodeSupported()
		{
			if (this.DeliveryMethod == SmtpDeliveryMethod.Network)
			{
				return this.ServerSupportsEai && this.DeliveryFormat == SmtpDeliveryFormat.International;
			}
			return this.DeliveryFormat == SmtpDeliveryFormat.International;
		}

		// Token: 0x060017D3 RID: 6099 RVA: 0x0007939C File Offset: 0x0007759C
		internal MailWriter GetFileMailWriter(string pickupDirectory)
		{
			if (Logging.On)
			{
				Logging.PrintInfo(Logging.Web, "SmtpClient.Send() pickupDirectory=" + pickupDirectory);
			}
			if (!Path.IsPathRooted(pickupDirectory))
			{
				throw new SmtpException(global::System.SR.GetString("SmtpNeedAbsolutePickupDirectory"));
			}
			string text2;
			do
			{
				string text = Guid.NewGuid().ToString() + ".eml";
				text2 = Path.Combine(pickupDirectory, text);
			}
			while (File.Exists(text2));
			FileStream fileStream = new FileStream(text2, FileMode.CreateNew);
			return new MailWriter(fileStream);
		}

		// Token: 0x060017D4 RID: 6100 RVA: 0x00079419 File Offset: 0x00077619
		protected void OnSendCompleted(AsyncCompletedEventArgs e)
		{
			if (this.SendCompleted != null)
			{
				this.SendCompleted(this, e);
			}
		}

		// Token: 0x060017D5 RID: 6101 RVA: 0x00079430 File Offset: 0x00077630
		private void SendCompletedWaitCallback(object operationState)
		{
			this.OnSendCompleted((AsyncCompletedEventArgs)operationState);
		}

		// Token: 0x060017D6 RID: 6102 RVA: 0x00079440 File Offset: 0x00077640
		public void Send(string from, string recipients, string subject, string body)
		{
			if (this.disposed)
			{
				throw new ObjectDisposedException(base.GetType().FullName);
			}
			MailMessage mailMessage = new MailMessage(from, recipients, subject, body);
			this.Send(mailMessage);
		}

		// Token: 0x060017D7 RID: 6103 RVA: 0x00079478 File Offset: 0x00077678
		public void Send(MailMessage message)
		{
			if (Logging.On)
			{
				Logging.Enter(Logging.Web, this, "Send", message);
			}
			if (this.disposed)
			{
				throw new ObjectDisposedException(base.GetType().FullName);
			}
			try
			{
				if (Logging.On)
				{
					Logging.PrintInfo(Logging.Web, this, "Send", "DeliveryMethod=" + this.DeliveryMethod.ToString());
				}
				if (Logging.On)
				{
					Logging.Associate(Logging.Web, this, message);
				}
				SmtpFailedRecipientException ex = null;
				if (this.InCall)
				{
					throw new InvalidOperationException(global::System.SR.GetString("net_inasync"));
				}
				if (message == null)
				{
					throw new ArgumentNullException("message");
				}
				if (this.DeliveryMethod == SmtpDeliveryMethod.Network)
				{
					this.CheckHostAndPort();
				}
				MailAddressCollection mailAddressCollection = new MailAddressCollection();
				if (message.From == null)
				{
					throw new InvalidOperationException(global::System.SR.GetString("SmtpFromRequired"));
				}
				if (message.To != null)
				{
					foreach (MailAddress mailAddress in message.To)
					{
						mailAddressCollection.Add(mailAddress);
					}
				}
				if (message.Bcc != null)
				{
					foreach (MailAddress mailAddress2 in message.Bcc)
					{
						mailAddressCollection.Add(mailAddress2);
					}
				}
				if (message.CC != null)
				{
					foreach (MailAddress mailAddress3 in message.CC)
					{
						mailAddressCollection.Add(mailAddress3);
					}
				}
				if (mailAddressCollection.Count == 0)
				{
					throw new InvalidOperationException(global::System.SR.GetString("SmtpRecipientRequired"));
				}
				this.transport.IdentityRequired = false;
				try
				{
					this.InCall = true;
					this.timedOut = false;
					this.timer = new Timer(new TimerCallback(this.TimeOutCallback), null, this.Timeout, this.Timeout);
					string pickupDirectory = this.PickupDirectoryLocation;
					switch (this.DeliveryMethod)
					{
					case SmtpDeliveryMethod.Network:
						goto IL_0241;
					case SmtpDeliveryMethod.SpecifiedPickupDirectory:
						break;
					case SmtpDeliveryMethod.PickupDirectoryFromIis:
						pickupDirectory = IisPickupDirectory.GetPickupDirectory();
						break;
					default:
						goto IL_0241;
					}
					if (this.EnableSsl)
					{
						throw new SmtpException(global::System.SR.GetString("SmtpPickupDirectoryDoesnotSupportSsl"));
					}
					bool flag = this.IsUnicodeSupported();
					this.ValidateUnicodeRequirement(message, mailAddressCollection, flag);
					MailWriter mailWriter = this.GetFileMailWriter(pickupDirectory);
					goto IL_0281;
					IL_0241:
					this.GetConnection();
					flag = this.IsUnicodeSupported();
					this.ValidateUnicodeRequirement(message, mailAddressCollection, flag);
					mailWriter = this.transport.SendMail(message.Sender ?? message.From, mailAddressCollection, message.BuildDeliveryStatusNotificationString(), flag, out ex);
					IL_0281:
					this.message = message;
					message.Send(mailWriter, this.DeliveryMethod > SmtpDeliveryMethod.Network, flag);
					mailWriter.Close();
					this.transport.ReleaseConnection();
					if (this.DeliveryMethod == SmtpDeliveryMethod.Network && ex != null)
					{
						throw ex;
					}
				}
				catch (Exception ex2)
				{
					if (Logging.On)
					{
						Logging.Exception(Logging.Web, this, "Send", ex2);
					}
					if (ex2 is SmtpFailedRecipientException && !((SmtpFailedRecipientException)ex2).fatal)
					{
						throw;
					}
					this.Abort();
					if (this.timedOut)
					{
						throw new SmtpException(global::System.SR.GetString("net_timeout"));
					}
					if (ex2 is SecurityException || ex2 is AuthenticationException || ex2 is SmtpException)
					{
						throw;
					}
					throw new SmtpException(global::System.SR.GetString("SmtpSendMailFailure"), ex2);
				}
				finally
				{
					this.InCall = false;
					if (this.timer != null)
					{
						this.timer.Dispose();
					}
				}
			}
			finally
			{
				if (Logging.On)
				{
					Logging.Exit(Logging.Web, this, "Send", null);
				}
			}
		}

		// Token: 0x060017D8 RID: 6104 RVA: 0x00079890 File Offset: 0x00077A90
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public void SendAsync(string from, string recipients, string subject, string body, object userToken)
		{
			if (this.disposed)
			{
				throw new ObjectDisposedException(base.GetType().FullName);
			}
			this.SendAsync(new MailMessage(from, recipients, subject, body), userToken);
		}

		// Token: 0x060017D9 RID: 6105 RVA: 0x000798C0 File Offset: 0x00077AC0
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public void SendAsync(MailMessage message, object userToken)
		{
			if (this.disposed)
			{
				throw new ObjectDisposedException(base.GetType().FullName);
			}
			if (Logging.On)
			{
				Logging.Enter(Logging.Web, this, "SendAsync", "DeliveryMethod=" + this.DeliveryMethod.ToString());
			}
			try
			{
				if (this.InCall)
				{
					throw new InvalidOperationException(global::System.SR.GetString("net_inasync"));
				}
				if (message == null)
				{
					throw new ArgumentNullException("message");
				}
				if (this.DeliveryMethod == SmtpDeliveryMethod.Network)
				{
					this.CheckHostAndPort();
				}
				this.recipients = new MailAddressCollection();
				if (message.From == null)
				{
					throw new InvalidOperationException(global::System.SR.GetString("SmtpFromRequired"));
				}
				if (message.To != null)
				{
					foreach (MailAddress mailAddress in message.To)
					{
						this.recipients.Add(mailAddress);
					}
				}
				if (message.Bcc != null)
				{
					foreach (MailAddress mailAddress2 in message.Bcc)
					{
						this.recipients.Add(mailAddress2);
					}
				}
				if (message.CC != null)
				{
					foreach (MailAddress mailAddress3 in message.CC)
					{
						this.recipients.Add(mailAddress3);
					}
				}
				if (this.recipients.Count == 0)
				{
					throw new InvalidOperationException(global::System.SR.GetString("SmtpRecipientRequired"));
				}
				try
				{
					this.InCall = true;
					this.cancelled = false;
					this.message = message;
					string pickupDirectory = this.PickupDirectoryLocation;
					CredentialCache credentialCache;
					this.transport.IdentityRequired = this.Credentials != null && (this.Credentials is SystemNetworkCredential || (credentialCache = this.Credentials as CredentialCache) == null || credentialCache.IsDefaultInCache);
					this.asyncOp = AsyncOperationManager.CreateOperation(userToken);
					switch (this.DeliveryMethod)
					{
					case SmtpDeliveryMethod.Network:
						goto IL_02AB;
					case SmtpDeliveryMethod.SpecifiedPickupDirectory:
						break;
					case SmtpDeliveryMethod.PickupDirectoryFromIis:
						pickupDirectory = IisPickupDirectory.GetPickupDirectory();
						break;
					default:
						goto IL_02AB;
					}
					if (this.EnableSsl)
					{
						throw new SmtpException(global::System.SR.GetString("SmtpPickupDirectoryDoesnotSupportSsl"));
					}
					this.writer = this.GetFileMailWriter(pickupDirectory);
					bool flag = this.IsUnicodeSupported();
					this.ValidateUnicodeRequirement(message, this.recipients, flag);
					message.Send(this.writer, true, flag);
					if (this.writer != null)
					{
						this.writer.Close();
					}
					this.transport.ReleaseConnection();
					AsyncCompletedEventArgs asyncCompletedEventArgs = new AsyncCompletedEventArgs(null, false, this.asyncOp.UserSuppliedState);
					this.InCall = false;
					this.asyncOp.PostOperationCompleted(this.onSendCompletedDelegate, asyncCompletedEventArgs);
					goto IL_0326;
					IL_02AB:
					this.operationCompletedResult = new ContextAwareResult(this.transport.IdentityRequired, true, null, this, SmtpClient._ContextSafeCompleteCallback);
					object obj = this.operationCompletedResult.StartPostingAsyncOp();
					lock (obj)
					{
						this.transport.BeginGetConnection(this.ServicePoint, this.operationCompletedResult, new AsyncCallback(this.ConnectCallback), this.operationCompletedResult);
						this.operationCompletedResult.FinishPostingAsyncOp();
					}
					IL_0326:;
				}
				catch (Exception ex)
				{
					this.InCall = false;
					if (Logging.On)
					{
						Logging.Exception(Logging.Web, this, "Send", ex);
					}
					if (ex is SmtpFailedRecipientException && !((SmtpFailedRecipientException)ex).fatal)
					{
						throw;
					}
					this.Abort();
					if (this.timedOut)
					{
						throw new SmtpException(global::System.SR.GetString("net_timeout"));
					}
					if (ex is SecurityException || ex is AuthenticationException || ex is SmtpException)
					{
						throw;
					}
					throw new SmtpException(global::System.SR.GetString("SmtpSendMailFailure"), ex);
				}
			}
			finally
			{
				if (Logging.On)
				{
					Logging.Exit(Logging.Web, this, "SendAsync", null);
				}
			}
		}

		// Token: 0x060017DA RID: 6106 RVA: 0x00079D30 File Offset: 0x00077F30
		public void SendAsyncCancel()
		{
			if (this.disposed)
			{
				throw new ObjectDisposedException(base.GetType().FullName);
			}
			if (Logging.On)
			{
				Logging.Enter(Logging.Web, this, "SendAsyncCancel", null);
			}
			try
			{
				if (this.InCall && !this.cancelled)
				{
					this.cancelled = true;
					this.Abort();
				}
			}
			finally
			{
				if (Logging.On)
				{
					Logging.Exit(Logging.Web, this, "SendAsyncCancel", null);
				}
			}
		}

		// Token: 0x060017DB RID: 6107 RVA: 0x00079DBC File Offset: 0x00077FBC
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public Task SendMailAsync(string from, string recipients, string subject, string body)
		{
			MailMessage mailMessage = new MailMessage(from, recipients, subject, body);
			return this.SendMailAsync(mailMessage);
		}

		// Token: 0x060017DC RID: 6108 RVA: 0x00079DDC File Offset: 0x00077FDC
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public Task SendMailAsync(MailMessage message)
		{
			TaskCompletionSource<object> tcs = new TaskCompletionSource<object>();
			SendCompletedEventHandler handler = null;
			handler = delegate(object sender, AsyncCompletedEventArgs e)
			{
				this.HandleCompletion(tcs, e, handler);
			};
			this.SendCompleted += handler;
			try
			{
				this.SendAsync(message, tcs);
			}
			catch
			{
				this.SendCompleted -= handler;
				throw;
			}
			return tcs.Task;
		}

		// Token: 0x060017DD RID: 6109 RVA: 0x00079E60 File Offset: 0x00078060
		private void HandleCompletion(TaskCompletionSource<object> tcs, AsyncCompletedEventArgs e, SendCompletedEventHandler handler)
		{
			if (e.UserState == tcs)
			{
				try
				{
					this.SendCompleted -= handler;
				}
				finally
				{
					if (e.Error != null)
					{
						tcs.TrySetException(e.Error);
					}
					else if (e.Cancelled)
					{
						tcs.TrySetCanceled();
					}
					else
					{
						tcs.TrySetResult(null);
					}
				}
			}
		}

		// Token: 0x170004FC RID: 1276
		// (get) Token: 0x060017DE RID: 6110 RVA: 0x00079EC0 File Offset: 0x000780C0
		// (set) Token: 0x060017DF RID: 6111 RVA: 0x00079EC8 File Offset: 0x000780C8
		internal bool InCall
		{
			get
			{
				return this.inCall;
			}
			set
			{
				this.inCall = value;
			}
		}

		// Token: 0x170004FD RID: 1277
		// (get) Token: 0x060017E0 RID: 6112 RVA: 0x00079ED1 File Offset: 0x000780D1
		internal static MailSettingsSectionGroupInternal MailConfiguration
		{
			get
			{
				if (SmtpClient.mailConfiguration == null)
				{
					SmtpClient.mailConfiguration = MailSettingsSectionGroupInternal.GetSection();
				}
				return SmtpClient.mailConfiguration;
			}
		}

		// Token: 0x060017E1 RID: 6113 RVA: 0x00079EF0 File Offset: 0x000780F0
		private void CheckHostAndPort()
		{
			if (this.host == null || this.host.Length == 0)
			{
				throw new InvalidOperationException(global::System.SR.GetString("UnspecifiedHost"));
			}
			if (this.port <= 0 || this.port > 65535)
			{
				throw new InvalidOperationException(global::System.SR.GetString("InvalidPort"));
			}
		}

		// Token: 0x060017E2 RID: 6114 RVA: 0x00079F48 File Offset: 0x00078148
		private void TimeOutCallback(object state)
		{
			if (!this.timedOut)
			{
				this.timedOut = true;
				this.Abort();
			}
		}

		// Token: 0x060017E3 RID: 6115 RVA: 0x00079F60 File Offset: 0x00078160
		private void Complete(Exception exception, IAsyncResult result)
		{
			ContextAwareResult contextAwareResult = (ContextAwareResult)result.AsyncState;
			try
			{
				if (this.cancelled)
				{
					exception = null;
					this.Abort();
				}
				else if (exception != null && (!(exception is SmtpFailedRecipientException) || ((SmtpFailedRecipientException)exception).fatal))
				{
					this.Abort();
					if (!(exception is SmtpException))
					{
						exception = new SmtpException(global::System.SR.GetString("SmtpSendMailFailure"), exception);
					}
				}
				else
				{
					if (this.writer != null)
					{
						try
						{
							this.writer.Close();
						}
						catch (SmtpException ex)
						{
							exception = ex;
						}
					}
					this.transport.ReleaseConnection();
				}
			}
			finally
			{
				contextAwareResult.InvokeCallback(exception);
			}
		}

		// Token: 0x060017E4 RID: 6116 RVA: 0x0007A014 File Offset: 0x00078214
		private static void ContextSafeCompleteCallback(IAsyncResult ar)
		{
			ContextAwareResult contextAwareResult = (ContextAwareResult)ar;
			SmtpClient smtpClient = (SmtpClient)ar.AsyncState;
			Exception ex = contextAwareResult.Result as Exception;
			AsyncOperation asyncOperation = smtpClient.asyncOp;
			AsyncCompletedEventArgs asyncCompletedEventArgs = new AsyncCompletedEventArgs(ex, smtpClient.cancelled, asyncOperation.UserSuppliedState);
			smtpClient.InCall = false;
			smtpClient.failedRecipientException = null;
			asyncOperation.PostOperationCompleted(smtpClient.onSendCompletedDelegate, asyncCompletedEventArgs);
		}

		// Token: 0x060017E5 RID: 6117 RVA: 0x0007A078 File Offset: 0x00078278
		private void SendMessageCallback(IAsyncResult result)
		{
			try
			{
				this.message.EndSend(result);
				this.Complete(this.failedRecipientException, result);
			}
			catch (Exception ex)
			{
				this.Complete(ex, result);
			}
		}

		// Token: 0x060017E6 RID: 6118 RVA: 0x0007A0BC File Offset: 0x000782BC
		private void SendMailCallback(IAsyncResult result)
		{
			try
			{
				this.writer = this.transport.EndSendMail(result);
				SendMailAsyncResult sendMailAsyncResult = (SendMailAsyncResult)result;
				this.failedRecipientException = sendMailAsyncResult.GetFailedRecipientException();
			}
			catch (Exception ex)
			{
				this.Complete(ex, result);
				return;
			}
			try
			{
				if (this.cancelled)
				{
					this.Complete(null, result);
				}
				else
				{
					this.message.BeginSend(this.writer, this.DeliveryMethod > SmtpDeliveryMethod.Network, this.ServerSupportsEai, new AsyncCallback(this.SendMessageCallback), result.AsyncState);
				}
			}
			catch (Exception ex2)
			{
				this.Complete(ex2, result);
			}
		}

		// Token: 0x060017E7 RID: 6119 RVA: 0x0007A16C File Offset: 0x0007836C
		private void ConnectCallback(IAsyncResult result)
		{
			try
			{
				this.transport.EndGetConnection(result);
				if (this.cancelled)
				{
					this.Complete(null, result);
				}
				else
				{
					bool flag = this.IsUnicodeSupported();
					this.ValidateUnicodeRequirement(this.message, this.recipients, flag);
					this.transport.BeginSendMail(this.message.Sender ?? this.message.From, this.recipients, this.message.BuildDeliveryStatusNotificationString(), flag, new AsyncCallback(this.SendMailCallback), result.AsyncState);
				}
			}
			catch (Exception ex)
			{
				this.Complete(ex, result);
			}
		}

		// Token: 0x060017E8 RID: 6120 RVA: 0x0007A218 File Offset: 0x00078418
		private void ValidateUnicodeRequirement(MailMessage message, MailAddressCollection recipients, bool allowUnicode)
		{
			foreach (MailAddress mailAddress in recipients)
			{
				mailAddress.GetSmtpAddress(allowUnicode);
			}
			if (message.Sender != null)
			{
				message.Sender.GetSmtpAddress(allowUnicode);
			}
			message.From.GetSmtpAddress(allowUnicode);
		}

		// Token: 0x060017E9 RID: 6121 RVA: 0x0007A284 File Offset: 0x00078484
		private void GetConnection()
		{
			if (!this.transport.IsConnected)
			{
				this.transport.GetConnection(this.ServicePoint);
			}
		}

		// Token: 0x060017EA RID: 6122 RVA: 0x0007A2A4 File Offset: 0x000784A4
		private void Abort()
		{
			try
			{
				this.transport.Abort();
			}
			catch
			{
			}
		}

		// Token: 0x060017EB RID: 6123 RVA: 0x0007A2D4 File Offset: 0x000784D4
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x060017EC RID: 6124 RVA: 0x0007A2E4 File Offset: 0x000784E4
		protected virtual void Dispose(bool disposing)
		{
			if (disposing && !this.disposed)
			{
				if (this.InCall && !this.cancelled)
				{
					this.cancelled = true;
					this.Abort();
				}
				if (this.transport != null && this.servicePoint != null)
				{
					this.transport.CloseIdleConnections(this.servicePoint);
				}
				if (this.timer != null)
				{
					this.timer.Dispose();
				}
				this.disposed = true;
			}
		}

		// Token: 0x040017FC RID: 6140
		private string host;

		// Token: 0x040017FD RID: 6141
		private int port;

		// Token: 0x040017FE RID: 6142
		private bool inCall;

		// Token: 0x040017FF RID: 6143
		private bool cancelled;

		// Token: 0x04001800 RID: 6144
		private bool timedOut;

		// Token: 0x04001801 RID: 6145
		private string targetName;

		// Token: 0x04001802 RID: 6146
		private SmtpDeliveryMethod deliveryMethod;

		// Token: 0x04001803 RID: 6147
		private SmtpDeliveryFormat deliveryFormat;

		// Token: 0x04001804 RID: 6148
		private string pickupDirectoryLocation;

		// Token: 0x04001805 RID: 6149
		private SmtpTransport transport;

		// Token: 0x04001806 RID: 6150
		private MailMessage message;

		// Token: 0x04001807 RID: 6151
		private MailWriter writer;

		// Token: 0x04001808 RID: 6152
		private MailAddressCollection recipients;

		// Token: 0x04001809 RID: 6153
		private SendOrPostCallback onSendCompletedDelegate;

		// Token: 0x0400180A RID: 6154
		private Timer timer;

		// Token: 0x0400180B RID: 6155
		private static volatile MailSettingsSectionGroupInternal mailConfiguration;

		// Token: 0x0400180C RID: 6156
		private ContextAwareResult operationCompletedResult;

		// Token: 0x0400180D RID: 6157
		private AsyncOperation asyncOp;

		// Token: 0x0400180E RID: 6158
		private static AsyncCallback _ContextSafeCompleteCallback = new AsyncCallback(SmtpClient.ContextSafeCompleteCallback);

		// Token: 0x0400180F RID: 6159
		private static int defaultPort = 25;

		// Token: 0x04001810 RID: 6160
		internal string clientDomain;

		// Token: 0x04001811 RID: 6161
		private bool disposed;

		// Token: 0x04001812 RID: 6162
		private bool servicePointChanged;

		// Token: 0x04001813 RID: 6163
		private ServicePoint servicePoint;

		// Token: 0x04001814 RID: 6164
		private SmtpFailedRecipientException failedRecipientException;

		// Token: 0x04001815 RID: 6165
		private const int maxPortValue = 65535;
	}
}
