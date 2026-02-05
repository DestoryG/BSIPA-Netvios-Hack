using System;
using System.IO;
using System.Security.Authentication;
using System.Security.Authentication.ExtendedProtection;
using System.Security.Cryptography.X509Certificates;
using System.Security.Permissions;
using System.Threading.Tasks;

namespace System.Net.Security
{
	// Token: 0x0200035E RID: 862
	public class SslStream : AuthenticatedStream
	{
		// Token: 0x06001EEF RID: 7919 RVA: 0x00090BC0 File Offset: 0x0008EDC0
		public SslStream(Stream innerStream)
			: this(innerStream, false, null, null)
		{
		}

		// Token: 0x06001EF0 RID: 7920 RVA: 0x00090BCC File Offset: 0x0008EDCC
		public SslStream(Stream innerStream, bool leaveInnerStreamOpen)
			: this(innerStream, leaveInnerStreamOpen, null, null, EncryptionPolicy.RequireEncryption)
		{
		}

		// Token: 0x06001EF1 RID: 7921 RVA: 0x00090BD9 File Offset: 0x0008EDD9
		public SslStream(Stream innerStream, bool leaveInnerStreamOpen, RemoteCertificateValidationCallback userCertificateValidationCallback)
			: this(innerStream, leaveInnerStreamOpen, userCertificateValidationCallback, null, EncryptionPolicy.RequireEncryption)
		{
		}

		// Token: 0x06001EF2 RID: 7922 RVA: 0x00090BE6 File Offset: 0x0008EDE6
		public SslStream(Stream innerStream, bool leaveInnerStreamOpen, RemoteCertificateValidationCallback userCertificateValidationCallback, LocalCertificateSelectionCallback userCertificateSelectionCallback)
			: this(innerStream, leaveInnerStreamOpen, userCertificateValidationCallback, userCertificateSelectionCallback, EncryptionPolicy.RequireEncryption)
		{
		}

		// Token: 0x06001EF3 RID: 7923 RVA: 0x00090BF4 File Offset: 0x0008EDF4
		public SslStream(Stream innerStream, bool leaveInnerStreamOpen, RemoteCertificateValidationCallback userCertificateValidationCallback, LocalCertificateSelectionCallback userCertificateSelectionCallback, EncryptionPolicy encryptionPolicy)
			: base(innerStream, leaveInnerStreamOpen)
		{
			if (encryptionPolicy != EncryptionPolicy.RequireEncryption && encryptionPolicy != EncryptionPolicy.AllowNoEncryption && encryptionPolicy != EncryptionPolicy.NoEncryption)
			{
				throw new ArgumentException(global::System.SR.GetString("net_invalid_enum", new object[] { "EncryptionPolicy" }), "encryptionPolicy");
			}
			this._userCertificateValidationCallback = userCertificateValidationCallback;
			this._userCertificateSelectionCallback = userCertificateSelectionCallback;
			RemoteCertValidationCallback remoteCertValidationCallback = new RemoteCertValidationCallback(this.userCertValidationCallbackWrapper);
			LocalCertSelectionCallback localCertSelectionCallback = ((userCertificateSelectionCallback == null) ? null : new LocalCertSelectionCallback(this.userCertSelectionCallbackWrapper));
			this._SslState = new SslState(innerStream, remoteCertValidationCallback, localCertSelectionCallback, encryptionPolicy);
		}

		// Token: 0x06001EF4 RID: 7924 RVA: 0x00090C7C File Offset: 0x0008EE7C
		private bool userCertValidationCallbackWrapper(string hostName, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
		{
			this.m_RemoteCertificateOrBytes = ((certificate == null) ? null : certificate.GetRawCertData());
			if (this._userCertificateValidationCallback == null)
			{
				if (!this._SslState.RemoteCertRequired)
				{
					sslPolicyErrors &= ~SslPolicyErrors.RemoteCertificateNotAvailable;
				}
				return sslPolicyErrors == SslPolicyErrors.None;
			}
			return this._userCertificateValidationCallback(this, certificate, chain, sslPolicyErrors);
		}

		// Token: 0x06001EF5 RID: 7925 RVA: 0x00090CCD File Offset: 0x0008EECD
		private X509Certificate userCertSelectionCallbackWrapper(string targetHost, X509CertificateCollection localCertificates, X509Certificate remoteCertificate, string[] acceptableIssuers)
		{
			return this._userCertificateSelectionCallback(this, targetHost, localCertificates, remoteCertificate, acceptableIssuers);
		}

		// Token: 0x06001EF6 RID: 7926 RVA: 0x00090CE0 File Offset: 0x0008EEE0
		public virtual void AuthenticateAsClient(string targetHost)
		{
			this.AuthenticateAsClient(targetHost, new X509CertificateCollection(), ServicePointManager.DefaultSslProtocols, false);
		}

		// Token: 0x06001EF7 RID: 7927 RVA: 0x00090CF4 File Offset: 0x0008EEF4
		public virtual void AuthenticateAsClient(string targetHost, X509CertificateCollection clientCertificates, bool checkCertificateRevocation)
		{
			this.AuthenticateAsClient(targetHost, clientCertificates, ServicePointManager.DefaultSslProtocols, !LocalAppContextSwitches.DontCheckCertificateRevocation && checkCertificateRevocation);
		}

		// Token: 0x06001EF8 RID: 7928 RVA: 0x00090D0E File Offset: 0x0008EF0E
		public virtual void AuthenticateAsClient(string targetHost, X509CertificateCollection clientCertificates, SslProtocols enabledSslProtocols, bool checkCertificateRevocation)
		{
			this._SslState.ValidateCreateContext(false, targetHost, enabledSslProtocols, null, clientCertificates, true, checkCertificateRevocation);
			this._SslState.ProcessAuthentication(null);
		}

		// Token: 0x06001EF9 RID: 7929 RVA: 0x00090D2F File Offset: 0x0008EF2F
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public virtual IAsyncResult BeginAuthenticateAsClient(string targetHost, AsyncCallback asyncCallback, object asyncState)
		{
			return this.BeginAuthenticateAsClient(targetHost, new X509CertificateCollection(), ServicePointManager.DefaultSslProtocols, false, asyncCallback, asyncState);
		}

		// Token: 0x06001EFA RID: 7930 RVA: 0x00090D45 File Offset: 0x0008EF45
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public virtual IAsyncResult BeginAuthenticateAsClient(string targetHost, X509CertificateCollection clientCertificates, bool checkCertificateRevocation, AsyncCallback asyncCallback, object asyncState)
		{
			return this.BeginAuthenticateAsClient(targetHost, clientCertificates, ServicePointManager.DefaultSslProtocols, checkCertificateRevocation, asyncCallback, asyncState);
		}

		// Token: 0x06001EFB RID: 7931 RVA: 0x00090D5C File Offset: 0x0008EF5C
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public virtual IAsyncResult BeginAuthenticateAsClient(string targetHost, X509CertificateCollection clientCertificates, SslProtocols enabledSslProtocols, bool checkCertificateRevocation, AsyncCallback asyncCallback, object asyncState)
		{
			this._SslState.ValidateCreateContext(false, targetHost, enabledSslProtocols, null, clientCertificates, true, checkCertificateRevocation);
			LazyAsyncResult lazyAsyncResult = new LazyAsyncResult(this._SslState, asyncState, asyncCallback);
			this._SslState.ProcessAuthentication(lazyAsyncResult);
			return lazyAsyncResult;
		}

		// Token: 0x06001EFC RID: 7932 RVA: 0x00090D99 File Offset: 0x0008EF99
		public virtual void EndAuthenticateAsClient(IAsyncResult asyncResult)
		{
			this._SslState.EndProcessAuthentication(asyncResult);
		}

		// Token: 0x06001EFD RID: 7933 RVA: 0x00090DA7 File Offset: 0x0008EFA7
		public virtual void AuthenticateAsServer(X509Certificate serverCertificate)
		{
			this.AuthenticateAsServer(serverCertificate, false, ServicePointManager.DefaultSslProtocols, false);
		}

		// Token: 0x06001EFE RID: 7934 RVA: 0x00090DB7 File Offset: 0x0008EFB7
		public virtual void AuthenticateAsServer(X509Certificate serverCertificate, bool clientCertificateRequired, bool checkCertificateRevocation)
		{
			this.AuthenticateAsServer(serverCertificate, clientCertificateRequired, ServicePointManager.DefaultSslProtocols, checkCertificateRevocation);
		}

		// Token: 0x06001EFF RID: 7935 RVA: 0x00090DC7 File Offset: 0x0008EFC7
		public virtual void AuthenticateAsServer(X509Certificate serverCertificate, bool clientCertificateRequired, SslProtocols enabledSslProtocols, bool checkCertificateRevocation)
		{
			this._SslState.ValidateCreateContext(true, string.Empty, enabledSslProtocols, serverCertificate, null, clientCertificateRequired, checkCertificateRevocation);
			this._SslState.ProcessAuthentication(null);
		}

		// Token: 0x06001F00 RID: 7936 RVA: 0x00090DEC File Offset: 0x0008EFEC
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public virtual IAsyncResult BeginAuthenticateAsServer(X509Certificate serverCertificate, AsyncCallback asyncCallback, object asyncState)
		{
			return this.BeginAuthenticateAsServer(serverCertificate, false, ServicePointManager.DefaultSslProtocols, false, asyncCallback, asyncState);
		}

		// Token: 0x06001F01 RID: 7937 RVA: 0x00090DFE File Offset: 0x0008EFFE
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public virtual IAsyncResult BeginAuthenticateAsServer(X509Certificate serverCertificate, bool clientCertificateRequired, bool checkCertificateRevocation, AsyncCallback asyncCallback, object asyncState)
		{
			return this.BeginAuthenticateAsServer(serverCertificate, clientCertificateRequired, ServicePointManager.DefaultSslProtocols, checkCertificateRevocation, asyncCallback, asyncState);
		}

		// Token: 0x06001F02 RID: 7938 RVA: 0x00090E14 File Offset: 0x0008F014
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public virtual IAsyncResult BeginAuthenticateAsServer(X509Certificate serverCertificate, bool clientCertificateRequired, SslProtocols enabledSslProtocols, bool checkCertificateRevocation, AsyncCallback asyncCallback, object asyncState)
		{
			this._SslState.ValidateCreateContext(true, string.Empty, enabledSslProtocols, serverCertificate, null, clientCertificateRequired, checkCertificateRevocation);
			LazyAsyncResult lazyAsyncResult = new LazyAsyncResult(this._SslState, asyncState, asyncCallback);
			this._SslState.ProcessAuthentication(lazyAsyncResult);
			return lazyAsyncResult;
		}

		// Token: 0x06001F03 RID: 7939 RVA: 0x00090E55 File Offset: 0x0008F055
		public virtual void EndAuthenticateAsServer(IAsyncResult asyncResult)
		{
			this._SslState.EndProcessAuthentication(asyncResult);
		}

		// Token: 0x06001F04 RID: 7940 RVA: 0x00090E63 File Offset: 0x0008F063
		internal virtual IAsyncResult BeginShutdown(AsyncCallback asyncCallback, object asyncState)
		{
			return this._SslState.BeginShutdown(asyncCallback, asyncState);
		}

		// Token: 0x06001F05 RID: 7941 RVA: 0x00090E72 File Offset: 0x0008F072
		internal virtual void EndShutdown(IAsyncResult asyncResult)
		{
			this._SslState.EndShutdown(asyncResult);
		}

		// Token: 0x17000807 RID: 2055
		// (get) Token: 0x06001F06 RID: 7942 RVA: 0x00090E80 File Offset: 0x0008F080
		public TransportContext TransportContext
		{
			get
			{
				return new SslStreamContext(this);
			}
		}

		// Token: 0x06001F07 RID: 7943 RVA: 0x00090E88 File Offset: 0x0008F088
		internal ChannelBinding GetChannelBinding(ChannelBindingKind kind)
		{
			return this._SslState.GetChannelBinding(kind);
		}

		// Token: 0x06001F08 RID: 7944 RVA: 0x00090E96 File Offset: 0x0008F096
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public virtual Task AuthenticateAsClientAsync(string targetHost)
		{
			return Task.Factory.FromAsync<string>(new Func<string, AsyncCallback, object, IAsyncResult>(this.BeginAuthenticateAsClient), new Action<IAsyncResult>(this.EndAuthenticateAsClient), targetHost, null);
		}

		// Token: 0x06001F09 RID: 7945 RVA: 0x00090EBE File Offset: 0x0008F0BE
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public virtual Task AuthenticateAsClientAsync(string targetHost, X509CertificateCollection clientCertificates, bool checkCertificateRevocation)
		{
			return this.AuthenticateAsClientAsync(targetHost, clientCertificates, ServicePointManager.DefaultSslProtocols, checkCertificateRevocation);
		}

		// Token: 0x06001F0A RID: 7946 RVA: 0x00090ED0 File Offset: 0x0008F0D0
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public virtual Task AuthenticateAsClientAsync(string targetHost, X509CertificateCollection clientCertificates, SslProtocols enabledSslProtocols, bool checkCertificateRevocation)
		{
			return Task.Factory.FromAsync((AsyncCallback callback, object state) => this.BeginAuthenticateAsClient(targetHost, clientCertificates, enabledSslProtocols, checkCertificateRevocation, callback, state), new Action<IAsyncResult>(this.EndAuthenticateAsClient), null);
		}

		// Token: 0x06001F0B RID: 7947 RVA: 0x00090F2B File Offset: 0x0008F12B
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public virtual Task AuthenticateAsServerAsync(X509Certificate serverCertificate)
		{
			return Task.Factory.FromAsync<X509Certificate>(new Func<X509Certificate, AsyncCallback, object, IAsyncResult>(this.BeginAuthenticateAsServer), new Action<IAsyncResult>(this.EndAuthenticateAsServer), serverCertificate, null);
		}

		// Token: 0x06001F0C RID: 7948 RVA: 0x00090F53 File Offset: 0x0008F153
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public virtual Task AuthenticateAsServerAsync(X509Certificate serverCertificate, bool clientCertificateRequired, bool checkCertificateRevocation)
		{
			return this.AuthenticateAsServerAsync(serverCertificate, clientCertificateRequired, ServicePointManager.DefaultSslProtocols, checkCertificateRevocation);
		}

		// Token: 0x06001F0D RID: 7949 RVA: 0x00090F64 File Offset: 0x0008F164
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public virtual Task AuthenticateAsServerAsync(X509Certificate serverCertificate, bool clientCertificateRequired, SslProtocols enabledSslProtocols, bool checkCertificateRevocation)
		{
			return Task.Factory.FromAsync((AsyncCallback callback, object state) => this.BeginAuthenticateAsServer(serverCertificate, clientCertificateRequired, enabledSslProtocols, checkCertificateRevocation, callback, state), new Action<IAsyncResult>(this.EndAuthenticateAsServer), null);
		}

		// Token: 0x06001F0E RID: 7950 RVA: 0x00090FBF File Offset: 0x0008F1BF
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public virtual Task ShutdownAsync()
		{
			return Task.Factory.FromAsync((AsyncCallback callback, object state) => this.BeginShutdown(callback, state), new Action<IAsyncResult>(this.EndShutdown), null);
		}

		// Token: 0x17000808 RID: 2056
		// (get) Token: 0x06001F0F RID: 7951 RVA: 0x00090FE5 File Offset: 0x0008F1E5
		public override bool IsAuthenticated
		{
			get
			{
				return this._SslState.IsAuthenticated;
			}
		}

		// Token: 0x17000809 RID: 2057
		// (get) Token: 0x06001F10 RID: 7952 RVA: 0x00090FF2 File Offset: 0x0008F1F2
		public override bool IsMutuallyAuthenticated
		{
			get
			{
				return this._SslState.IsMutuallyAuthenticated;
			}
		}

		// Token: 0x1700080A RID: 2058
		// (get) Token: 0x06001F11 RID: 7953 RVA: 0x00090FFF File Offset: 0x0008F1FF
		public override bool IsEncrypted
		{
			get
			{
				return this.IsAuthenticated;
			}
		}

		// Token: 0x1700080B RID: 2059
		// (get) Token: 0x06001F12 RID: 7954 RVA: 0x00091007 File Offset: 0x0008F207
		public override bool IsSigned
		{
			get
			{
				return this.IsAuthenticated;
			}
		}

		// Token: 0x1700080C RID: 2060
		// (get) Token: 0x06001F13 RID: 7955 RVA: 0x0009100F File Offset: 0x0008F20F
		public override bool IsServer
		{
			get
			{
				return this._SslState.IsServer;
			}
		}

		// Token: 0x1700080D RID: 2061
		// (get) Token: 0x06001F14 RID: 7956 RVA: 0x0009101C File Offset: 0x0008F21C
		public virtual SslProtocols SslProtocol
		{
			get
			{
				return this._SslState.SslProtocol;
			}
		}

		// Token: 0x1700080E RID: 2062
		// (get) Token: 0x06001F15 RID: 7957 RVA: 0x00091029 File Offset: 0x0008F229
		public virtual bool CheckCertRevocationStatus
		{
			get
			{
				return this._SslState.CheckCertRevocationStatus;
			}
		}

		// Token: 0x1700080F RID: 2063
		// (get) Token: 0x06001F16 RID: 7958 RVA: 0x00091036 File Offset: 0x0008F236
		public virtual X509Certificate LocalCertificate
		{
			get
			{
				return this._SslState.LocalCertificate;
			}
		}

		// Token: 0x17000810 RID: 2064
		// (get) Token: 0x06001F17 RID: 7959 RVA: 0x00091044 File Offset: 0x0008F244
		public virtual X509Certificate RemoteCertificate
		{
			get
			{
				this._SslState.CheckThrow(true, false);
				object remoteCertificateOrBytes = this.m_RemoteCertificateOrBytes;
				if (remoteCertificateOrBytes != null && remoteCertificateOrBytes.GetType() == typeof(byte[]))
				{
					return (X509Certificate)(this.m_RemoteCertificateOrBytes = new X509Certificate((byte[])remoteCertificateOrBytes));
				}
				return remoteCertificateOrBytes as X509Certificate;
			}
		}

		// Token: 0x17000811 RID: 2065
		// (get) Token: 0x06001F18 RID: 7960 RVA: 0x0009109F File Offset: 0x0008F29F
		public virtual CipherAlgorithmType CipherAlgorithm
		{
			get
			{
				return this._SslState.CipherAlgorithm;
			}
		}

		// Token: 0x17000812 RID: 2066
		// (get) Token: 0x06001F19 RID: 7961 RVA: 0x000910AC File Offset: 0x0008F2AC
		public virtual int CipherStrength
		{
			get
			{
				return this._SslState.CipherStrength;
			}
		}

		// Token: 0x17000813 RID: 2067
		// (get) Token: 0x06001F1A RID: 7962 RVA: 0x000910B9 File Offset: 0x0008F2B9
		public virtual HashAlgorithmType HashAlgorithm
		{
			get
			{
				return this._SslState.HashAlgorithm;
			}
		}

		// Token: 0x17000814 RID: 2068
		// (get) Token: 0x06001F1B RID: 7963 RVA: 0x000910C6 File Offset: 0x0008F2C6
		public virtual int HashStrength
		{
			get
			{
				return this._SslState.HashStrength;
			}
		}

		// Token: 0x17000815 RID: 2069
		// (get) Token: 0x06001F1C RID: 7964 RVA: 0x000910D3 File Offset: 0x0008F2D3
		public virtual ExchangeAlgorithmType KeyExchangeAlgorithm
		{
			get
			{
				return this._SslState.KeyExchangeAlgorithm;
			}
		}

		// Token: 0x17000816 RID: 2070
		// (get) Token: 0x06001F1D RID: 7965 RVA: 0x000910E0 File Offset: 0x0008F2E0
		public virtual int KeyExchangeStrength
		{
			get
			{
				return this._SslState.KeyExchangeStrength;
			}
		}

		// Token: 0x17000817 RID: 2071
		// (get) Token: 0x06001F1E RID: 7966 RVA: 0x000910ED File Offset: 0x0008F2ED
		public override bool CanSeek
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000818 RID: 2072
		// (get) Token: 0x06001F1F RID: 7967 RVA: 0x000910F0 File Offset: 0x0008F2F0
		public override bool CanRead
		{
			get
			{
				return this._SslState.IsAuthenticated && base.InnerStream.CanRead;
			}
		}

		// Token: 0x17000819 RID: 2073
		// (get) Token: 0x06001F20 RID: 7968 RVA: 0x0009110C File Offset: 0x0008F30C
		public override bool CanTimeout
		{
			get
			{
				return base.InnerStream.CanTimeout;
			}
		}

		// Token: 0x1700081A RID: 2074
		// (get) Token: 0x06001F21 RID: 7969 RVA: 0x00091119 File Offset: 0x0008F319
		public override bool CanWrite
		{
			get
			{
				return this._SslState.IsAuthenticated && base.InnerStream.CanWrite && !this._SslState.IsShutdown;
			}
		}

		// Token: 0x1700081B RID: 2075
		// (get) Token: 0x06001F22 RID: 7970 RVA: 0x00091145 File Offset: 0x0008F345
		// (set) Token: 0x06001F23 RID: 7971 RVA: 0x00091152 File Offset: 0x0008F352
		public override int ReadTimeout
		{
			get
			{
				return base.InnerStream.ReadTimeout;
			}
			set
			{
				base.InnerStream.ReadTimeout = value;
			}
		}

		// Token: 0x1700081C RID: 2076
		// (get) Token: 0x06001F24 RID: 7972 RVA: 0x00091160 File Offset: 0x0008F360
		// (set) Token: 0x06001F25 RID: 7973 RVA: 0x0009116D File Offset: 0x0008F36D
		public override int WriteTimeout
		{
			get
			{
				return base.InnerStream.WriteTimeout;
			}
			set
			{
				base.InnerStream.WriteTimeout = value;
			}
		}

		// Token: 0x1700081D RID: 2077
		// (get) Token: 0x06001F26 RID: 7974 RVA: 0x0009117B File Offset: 0x0008F37B
		public override long Length
		{
			get
			{
				return base.InnerStream.Length;
			}
		}

		// Token: 0x1700081E RID: 2078
		// (get) Token: 0x06001F27 RID: 7975 RVA: 0x00091188 File Offset: 0x0008F388
		// (set) Token: 0x06001F28 RID: 7976 RVA: 0x00091195 File Offset: 0x0008F395
		public override long Position
		{
			get
			{
				return base.InnerStream.Position;
			}
			set
			{
				throw new NotSupportedException(global::System.SR.GetString("net_noseek"));
			}
		}

		// Token: 0x06001F29 RID: 7977 RVA: 0x000911A6 File Offset: 0x0008F3A6
		public override void SetLength(long value)
		{
			base.InnerStream.SetLength(value);
		}

		// Token: 0x06001F2A RID: 7978 RVA: 0x000911B4 File Offset: 0x0008F3B4
		public override long Seek(long offset, SeekOrigin origin)
		{
			throw new NotSupportedException(global::System.SR.GetString("net_noseek"));
		}

		// Token: 0x06001F2B RID: 7979 RVA: 0x000911C5 File Offset: 0x0008F3C5
		public override void Flush()
		{
			this._SslState.Flush();
		}

		// Token: 0x06001F2C RID: 7980 RVA: 0x000911D4 File Offset: 0x0008F3D4
		protected override void Dispose(bool disposing)
		{
			try
			{
				this._SslState.Close();
			}
			finally
			{
				base.Dispose(disposing);
			}
		}

		// Token: 0x06001F2D RID: 7981 RVA: 0x00091208 File Offset: 0x0008F408
		public override int Read(byte[] buffer, int offset, int count)
		{
			return this._SslState.SecureStream.Read(buffer, offset, count);
		}

		// Token: 0x06001F2E RID: 7982 RVA: 0x0009121D File Offset: 0x0008F41D
		public void Write(byte[] buffer)
		{
			this._SslState.SecureStream.Write(buffer, 0, buffer.Length);
		}

		// Token: 0x06001F2F RID: 7983 RVA: 0x00091234 File Offset: 0x0008F434
		public override void Write(byte[] buffer, int offset, int count)
		{
			this._SslState.SecureStream.Write(buffer, offset, count);
		}

		// Token: 0x06001F30 RID: 7984 RVA: 0x00091249 File Offset: 0x0008F449
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public override IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback asyncCallback, object asyncState)
		{
			return this._SslState.SecureStream.BeginRead(buffer, offset, count, asyncCallback, asyncState);
		}

		// Token: 0x06001F31 RID: 7985 RVA: 0x00091262 File Offset: 0x0008F462
		public override int EndRead(IAsyncResult asyncResult)
		{
			return this._SslState.SecureStream.EndRead(asyncResult);
		}

		// Token: 0x06001F32 RID: 7986 RVA: 0x00091275 File Offset: 0x0008F475
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public override IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback asyncCallback, object asyncState)
		{
			return this._SslState.SecureStream.BeginWrite(buffer, offset, count, asyncCallback, asyncState);
		}

		// Token: 0x06001F33 RID: 7987 RVA: 0x0009128E File Offset: 0x0008F48E
		public override void EndWrite(IAsyncResult asyncResult)
		{
			this._SslState.SecureStream.EndWrite(asyncResult);
		}

		// Token: 0x04001D02 RID: 7426
		private SslState _SslState;

		// Token: 0x04001D03 RID: 7427
		private RemoteCertificateValidationCallback _userCertificateValidationCallback;

		// Token: 0x04001D04 RID: 7428
		private LocalCertificateSelectionCallback _userCertificateSelectionCallback;

		// Token: 0x04001D05 RID: 7429
		private object m_RemoteCertificateOrBytes;
	}
}
