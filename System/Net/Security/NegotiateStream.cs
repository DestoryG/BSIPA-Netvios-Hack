using System;
using System.IO;
using System.Security.Authentication.ExtendedProtection;
using System.Security.Permissions;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;

namespace System.Net.Security
{
	// Token: 0x02000357 RID: 855
	public class NegotiateStream : AuthenticatedStream
	{
		// Token: 0x06001E92 RID: 7826 RVA: 0x0008FC58 File Offset: 0x0008DE58
		public NegotiateStream(Stream innerStream)
			: this(innerStream, false)
		{
		}

		// Token: 0x06001E93 RID: 7827 RVA: 0x0008FC62 File Offset: 0x0008DE62
		public NegotiateStream(Stream innerStream, bool leaveInnerStreamOpen)
			: base(innerStream, leaveInnerStreamOpen)
		{
			this._NegoState = new NegoState(innerStream, leaveInnerStreamOpen);
			this._Package = NegoState.DefaultPackage;
			this.InitializeStreamPart();
		}

		// Token: 0x06001E94 RID: 7828 RVA: 0x0008FC8A File Offset: 0x0008DE8A
		public virtual void AuthenticateAsClient()
		{
			this.AuthenticateAsClient((NetworkCredential)CredentialCache.DefaultCredentials, null, string.Empty, ProtectionLevel.EncryptAndSign, TokenImpersonationLevel.Identification);
		}

		// Token: 0x06001E95 RID: 7829 RVA: 0x0008FCA4 File Offset: 0x0008DEA4
		public virtual void AuthenticateAsClient(NetworkCredential credential, string targetName)
		{
			this.AuthenticateAsClient(credential, null, targetName, ProtectionLevel.EncryptAndSign, TokenImpersonationLevel.Identification);
		}

		// Token: 0x06001E96 RID: 7830 RVA: 0x0008FCB1 File Offset: 0x0008DEB1
		public virtual void AuthenticateAsClient(NetworkCredential credential, ChannelBinding binding, string targetName)
		{
			this.AuthenticateAsClient(credential, binding, targetName, ProtectionLevel.EncryptAndSign, TokenImpersonationLevel.Identification);
		}

		// Token: 0x06001E97 RID: 7831 RVA: 0x0008FCBE File Offset: 0x0008DEBE
		public virtual void AuthenticateAsClient(NetworkCredential credential, string targetName, ProtectionLevel requiredProtectionLevel, TokenImpersonationLevel allowedImpersonationLevel)
		{
			this.AuthenticateAsClient(credential, null, targetName, requiredProtectionLevel, allowedImpersonationLevel);
		}

		// Token: 0x06001E98 RID: 7832 RVA: 0x0008FCCC File Offset: 0x0008DECC
		public virtual void AuthenticateAsClient(NetworkCredential credential, ChannelBinding binding, string targetName, ProtectionLevel requiredProtectionLevel, TokenImpersonationLevel allowedImpersonationLevel)
		{
			this._NegoState.ValidateCreateContext(this._Package, false, credential, targetName, binding, requiredProtectionLevel, allowedImpersonationLevel);
			this._NegoState.ProcessAuthentication(null);
		}

		// Token: 0x06001E99 RID: 7833 RVA: 0x0008FCF3 File Offset: 0x0008DEF3
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public virtual IAsyncResult BeginAuthenticateAsClient(AsyncCallback asyncCallback, object asyncState)
		{
			return this.BeginAuthenticateAsClient((NetworkCredential)CredentialCache.DefaultCredentials, null, string.Empty, ProtectionLevel.EncryptAndSign, TokenImpersonationLevel.Identification, asyncCallback, asyncState);
		}

		// Token: 0x06001E9A RID: 7834 RVA: 0x0008FD0F File Offset: 0x0008DF0F
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public virtual IAsyncResult BeginAuthenticateAsClient(NetworkCredential credential, string targetName, AsyncCallback asyncCallback, object asyncState)
		{
			return this.BeginAuthenticateAsClient(credential, null, targetName, ProtectionLevel.EncryptAndSign, TokenImpersonationLevel.Identification, asyncCallback, asyncState);
		}

		// Token: 0x06001E9B RID: 7835 RVA: 0x0008FD1F File Offset: 0x0008DF1F
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public virtual IAsyncResult BeginAuthenticateAsClient(NetworkCredential credential, ChannelBinding binding, string targetName, AsyncCallback asyncCallback, object asyncState)
		{
			return this.BeginAuthenticateAsClient(credential, binding, targetName, ProtectionLevel.EncryptAndSign, TokenImpersonationLevel.Identification, asyncCallback, asyncState);
		}

		// Token: 0x06001E9C RID: 7836 RVA: 0x0008FD30 File Offset: 0x0008DF30
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public virtual IAsyncResult BeginAuthenticateAsClient(NetworkCredential credential, string targetName, ProtectionLevel requiredProtectionLevel, TokenImpersonationLevel allowedImpersonationLevel, AsyncCallback asyncCallback, object asyncState)
		{
			return this.BeginAuthenticateAsClient(credential, null, targetName, requiredProtectionLevel, allowedImpersonationLevel, asyncCallback, asyncState);
		}

		// Token: 0x06001E9D RID: 7837 RVA: 0x0008FD44 File Offset: 0x0008DF44
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public virtual IAsyncResult BeginAuthenticateAsClient(NetworkCredential credential, ChannelBinding binding, string targetName, ProtectionLevel requiredProtectionLevel, TokenImpersonationLevel allowedImpersonationLevel, AsyncCallback asyncCallback, object asyncState)
		{
			this._NegoState.ValidateCreateContext(this._Package, false, credential, targetName, binding, requiredProtectionLevel, allowedImpersonationLevel);
			LazyAsyncResult lazyAsyncResult = new LazyAsyncResult(this._NegoState, asyncState, asyncCallback);
			this._NegoState.ProcessAuthentication(lazyAsyncResult);
			return lazyAsyncResult;
		}

		// Token: 0x06001E9E RID: 7838 RVA: 0x0008FD87 File Offset: 0x0008DF87
		public virtual void EndAuthenticateAsClient(IAsyncResult asyncResult)
		{
			this._NegoState.EndProcessAuthentication(asyncResult);
		}

		// Token: 0x06001E9F RID: 7839 RVA: 0x0008FD95 File Offset: 0x0008DF95
		public virtual void AuthenticateAsServer()
		{
			this.AuthenticateAsServer((NetworkCredential)CredentialCache.DefaultCredentials, null, ProtectionLevel.EncryptAndSign, TokenImpersonationLevel.Identification);
		}

		// Token: 0x06001EA0 RID: 7840 RVA: 0x0008FDAA File Offset: 0x0008DFAA
		public virtual void AuthenticateAsServer(ExtendedProtectionPolicy policy)
		{
			this.AuthenticateAsServer((NetworkCredential)CredentialCache.DefaultCredentials, policy, ProtectionLevel.EncryptAndSign, TokenImpersonationLevel.Identification);
		}

		// Token: 0x06001EA1 RID: 7841 RVA: 0x0008FDBF File Offset: 0x0008DFBF
		public virtual void AuthenticateAsServer(NetworkCredential credential, ProtectionLevel requiredProtectionLevel, TokenImpersonationLevel requiredImpersonationLevel)
		{
			this.AuthenticateAsServer(credential, null, requiredProtectionLevel, requiredImpersonationLevel);
		}

		// Token: 0x06001EA2 RID: 7842 RVA: 0x0008FDCB File Offset: 0x0008DFCB
		public virtual void AuthenticateAsServer(NetworkCredential credential, ExtendedProtectionPolicy policy, ProtectionLevel requiredProtectionLevel, TokenImpersonationLevel requiredImpersonationLevel)
		{
			this._NegoState.ValidateCreateContext(this._Package, credential, string.Empty, policy, requiredProtectionLevel, requiredImpersonationLevel);
			this._NegoState.ProcessAuthentication(null);
		}

		// Token: 0x06001EA3 RID: 7843 RVA: 0x0008FDF4 File Offset: 0x0008DFF4
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public virtual IAsyncResult BeginAuthenticateAsServer(AsyncCallback asyncCallback, object asyncState)
		{
			return this.BeginAuthenticateAsServer((NetworkCredential)CredentialCache.DefaultCredentials, null, ProtectionLevel.EncryptAndSign, TokenImpersonationLevel.Identification, asyncCallback, asyncState);
		}

		// Token: 0x06001EA4 RID: 7844 RVA: 0x0008FE0B File Offset: 0x0008E00B
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public virtual IAsyncResult BeginAuthenticateAsServer(ExtendedProtectionPolicy policy, AsyncCallback asyncCallback, object asyncState)
		{
			return this.BeginAuthenticateAsServer((NetworkCredential)CredentialCache.DefaultCredentials, policy, ProtectionLevel.EncryptAndSign, TokenImpersonationLevel.Identification, asyncCallback, asyncState);
		}

		// Token: 0x06001EA5 RID: 7845 RVA: 0x0008FE22 File Offset: 0x0008E022
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public virtual IAsyncResult BeginAuthenticateAsServer(NetworkCredential credential, ProtectionLevel requiredProtectionLevel, TokenImpersonationLevel requiredImpersonationLevel, AsyncCallback asyncCallback, object asyncState)
		{
			return this.BeginAuthenticateAsServer(credential, null, requiredProtectionLevel, requiredImpersonationLevel, asyncCallback, asyncState);
		}

		// Token: 0x06001EA6 RID: 7846 RVA: 0x0008FE34 File Offset: 0x0008E034
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public virtual IAsyncResult BeginAuthenticateAsServer(NetworkCredential credential, ExtendedProtectionPolicy policy, ProtectionLevel requiredProtectionLevel, TokenImpersonationLevel requiredImpersonationLevel, AsyncCallback asyncCallback, object asyncState)
		{
			this._NegoState.ValidateCreateContext(this._Package, credential, string.Empty, policy, requiredProtectionLevel, requiredImpersonationLevel);
			LazyAsyncResult lazyAsyncResult = new LazyAsyncResult(this._NegoState, asyncState, asyncCallback);
			this._NegoState.ProcessAuthentication(lazyAsyncResult);
			return lazyAsyncResult;
		}

		// Token: 0x06001EA7 RID: 7847 RVA: 0x0008FE79 File Offset: 0x0008E079
		public virtual void EndAuthenticateAsServer(IAsyncResult asyncResult)
		{
			this._NegoState.EndProcessAuthentication(asyncResult);
		}

		// Token: 0x06001EA8 RID: 7848 RVA: 0x0008FE87 File Offset: 0x0008E087
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public virtual Task AuthenticateAsClientAsync()
		{
			return Task.Factory.FromAsync(new Func<AsyncCallback, object, IAsyncResult>(this.BeginAuthenticateAsClient), new Action<IAsyncResult>(this.EndAuthenticateAsClient), null);
		}

		// Token: 0x06001EA9 RID: 7849 RVA: 0x0008FEAE File Offset: 0x0008E0AE
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public virtual Task AuthenticateAsClientAsync(NetworkCredential credential, string targetName)
		{
			return Task.Factory.FromAsync<NetworkCredential, string>(new Func<NetworkCredential, string, AsyncCallback, object, IAsyncResult>(this.BeginAuthenticateAsClient), new Action<IAsyncResult>(this.EndAuthenticateAsClient), credential, targetName, null);
		}

		// Token: 0x06001EAA RID: 7850 RVA: 0x0008FED8 File Offset: 0x0008E0D8
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public virtual Task AuthenticateAsClientAsync(NetworkCredential credential, string targetName, ProtectionLevel requiredProtectionLevel, TokenImpersonationLevel allowedImpersonationLevel)
		{
			return Task.Factory.FromAsync((AsyncCallback callback, object state) => this.BeginAuthenticateAsClient(credential, targetName, requiredProtectionLevel, allowedImpersonationLevel, callback, state), new Action<IAsyncResult>(this.EndAuthenticateAsClient), null);
		}

		// Token: 0x06001EAB RID: 7851 RVA: 0x0008FF33 File Offset: 0x0008E133
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public virtual Task AuthenticateAsClientAsync(NetworkCredential credential, ChannelBinding binding, string targetName)
		{
			return Task.Factory.FromAsync<NetworkCredential, ChannelBinding, string>(new Func<NetworkCredential, ChannelBinding, string, AsyncCallback, object, IAsyncResult>(this.BeginAuthenticateAsClient), new Action<IAsyncResult>(this.EndAuthenticateAsClient), credential, binding, targetName, null);
		}

		// Token: 0x06001EAC RID: 7852 RVA: 0x0008FF60 File Offset: 0x0008E160
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public virtual Task AuthenticateAsClientAsync(NetworkCredential credential, ChannelBinding binding, string targetName, ProtectionLevel requiredProtectionLevel, TokenImpersonationLevel allowedImpersonationLevel)
		{
			return Task.Factory.FromAsync((AsyncCallback callback, object state) => this.BeginAuthenticateAsClient(credential, binding, targetName, requiredProtectionLevel, allowedImpersonationLevel, callback, state), new Action<IAsyncResult>(this.EndAuthenticateAsClient), null);
		}

		// Token: 0x06001EAD RID: 7853 RVA: 0x0008FFC3 File Offset: 0x0008E1C3
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public virtual Task AuthenticateAsServerAsync()
		{
			return Task.Factory.FromAsync(new Func<AsyncCallback, object, IAsyncResult>(this.BeginAuthenticateAsServer), new Action<IAsyncResult>(this.EndAuthenticateAsServer), null);
		}

		// Token: 0x06001EAE RID: 7854 RVA: 0x0008FFEA File Offset: 0x0008E1EA
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public virtual Task AuthenticateAsServerAsync(ExtendedProtectionPolicy policy)
		{
			return Task.Factory.FromAsync<ExtendedProtectionPolicy>(new Func<ExtendedProtectionPolicy, AsyncCallback, object, IAsyncResult>(this.BeginAuthenticateAsServer), new Action<IAsyncResult>(this.EndAuthenticateAsServer), policy, null);
		}

		// Token: 0x06001EAF RID: 7855 RVA: 0x00090012 File Offset: 0x0008E212
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public virtual Task AuthenticateAsServerAsync(NetworkCredential credential, ProtectionLevel requiredProtectionLevel, TokenImpersonationLevel requiredImpersonationLevel)
		{
			return Task.Factory.FromAsync<NetworkCredential, ProtectionLevel, TokenImpersonationLevel>(new Func<NetworkCredential, ProtectionLevel, TokenImpersonationLevel, AsyncCallback, object, IAsyncResult>(this.BeginAuthenticateAsServer), new Action<IAsyncResult>(this.EndAuthenticateAsServer), credential, requiredProtectionLevel, requiredImpersonationLevel, null);
		}

		// Token: 0x06001EB0 RID: 7856 RVA: 0x0009003C File Offset: 0x0008E23C
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public virtual Task AuthenticateAsServerAsync(NetworkCredential credential, ExtendedProtectionPolicy policy, ProtectionLevel requiredProtectionLevel, TokenImpersonationLevel requiredImpersonationLevel)
		{
			return Task.Factory.FromAsync((AsyncCallback callback, object state) => this.BeginAuthenticateAsServer(credential, policy, requiredProtectionLevel, requiredImpersonationLevel, callback, state), new Action<IAsyncResult>(this.EndAuthenticateAsClient), null);
		}

		// Token: 0x170007F5 RID: 2037
		// (get) Token: 0x06001EB1 RID: 7857 RVA: 0x00090097 File Offset: 0x0008E297
		public override bool IsAuthenticated
		{
			get
			{
				return this._NegoState.IsAuthenticated;
			}
		}

		// Token: 0x170007F6 RID: 2038
		// (get) Token: 0x06001EB2 RID: 7858 RVA: 0x000900A4 File Offset: 0x0008E2A4
		public override bool IsMutuallyAuthenticated
		{
			get
			{
				return this._NegoState.IsMutuallyAuthenticated;
			}
		}

		// Token: 0x170007F7 RID: 2039
		// (get) Token: 0x06001EB3 RID: 7859 RVA: 0x000900B1 File Offset: 0x0008E2B1
		public override bool IsEncrypted
		{
			get
			{
				return this._NegoState.IsEncrypted;
			}
		}

		// Token: 0x170007F8 RID: 2040
		// (get) Token: 0x06001EB4 RID: 7860 RVA: 0x000900BE File Offset: 0x0008E2BE
		public override bool IsSigned
		{
			get
			{
				return this._NegoState.IsSigned;
			}
		}

		// Token: 0x170007F9 RID: 2041
		// (get) Token: 0x06001EB5 RID: 7861 RVA: 0x000900CB File Offset: 0x0008E2CB
		public override bool IsServer
		{
			get
			{
				return this._NegoState.IsServer;
			}
		}

		// Token: 0x170007FA RID: 2042
		// (get) Token: 0x06001EB6 RID: 7862 RVA: 0x000900D8 File Offset: 0x0008E2D8
		public virtual TokenImpersonationLevel ImpersonationLevel
		{
			get
			{
				return this._NegoState.AllowedImpersonation;
			}
		}

		// Token: 0x170007FB RID: 2043
		// (get) Token: 0x06001EB7 RID: 7863 RVA: 0x000900E5 File Offset: 0x0008E2E5
		public virtual IIdentity RemoteIdentity
		{
			get
			{
				new SecurityPermission(SecurityPermissionFlag.UnmanagedCode).Demand();
				if (this._RemoteIdentity == null)
				{
					this._RemoteIdentity = this._NegoState.GetIdentity();
				}
				return this._RemoteIdentity;
			}
		}

		// Token: 0x170007FC RID: 2044
		// (get) Token: 0x06001EB8 RID: 7864 RVA: 0x00090111 File Offset: 0x0008E311
		public override bool CanSeek
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170007FD RID: 2045
		// (get) Token: 0x06001EB9 RID: 7865 RVA: 0x00090114 File Offset: 0x0008E314
		public override bool CanRead
		{
			get
			{
				return this.IsAuthenticated && base.InnerStream.CanRead;
			}
		}

		// Token: 0x170007FE RID: 2046
		// (get) Token: 0x06001EBA RID: 7866 RVA: 0x0009012B File Offset: 0x0008E32B
		public override bool CanTimeout
		{
			get
			{
				return base.InnerStream.CanTimeout;
			}
		}

		// Token: 0x170007FF RID: 2047
		// (get) Token: 0x06001EBB RID: 7867 RVA: 0x00090138 File Offset: 0x0008E338
		public override bool CanWrite
		{
			get
			{
				return this.IsAuthenticated && base.InnerStream.CanWrite;
			}
		}

		// Token: 0x17000800 RID: 2048
		// (get) Token: 0x06001EBC RID: 7868 RVA: 0x0009014F File Offset: 0x0008E34F
		// (set) Token: 0x06001EBD RID: 7869 RVA: 0x0009015C File Offset: 0x0008E35C
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

		// Token: 0x17000801 RID: 2049
		// (get) Token: 0x06001EBE RID: 7870 RVA: 0x0009016A File Offset: 0x0008E36A
		// (set) Token: 0x06001EBF RID: 7871 RVA: 0x00090177 File Offset: 0x0008E377
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

		// Token: 0x17000802 RID: 2050
		// (get) Token: 0x06001EC0 RID: 7872 RVA: 0x00090185 File Offset: 0x0008E385
		public override long Length
		{
			get
			{
				return base.InnerStream.Length;
			}
		}

		// Token: 0x17000803 RID: 2051
		// (get) Token: 0x06001EC1 RID: 7873 RVA: 0x00090192 File Offset: 0x0008E392
		// (set) Token: 0x06001EC2 RID: 7874 RVA: 0x0009019F File Offset: 0x0008E39F
		public override long Position
		{
			get
			{
				return base.InnerStream.Position;
			}
			set
			{
				throw new NotSupportedException(SR.GetString("net_noseek"));
			}
		}

		// Token: 0x06001EC3 RID: 7875 RVA: 0x000901B0 File Offset: 0x0008E3B0
		public override void SetLength(long value)
		{
			base.InnerStream.SetLength(value);
		}

		// Token: 0x06001EC4 RID: 7876 RVA: 0x000901BE File Offset: 0x0008E3BE
		public override long Seek(long offset, SeekOrigin origin)
		{
			throw new NotSupportedException(SR.GetString("net_noseek"));
		}

		// Token: 0x06001EC5 RID: 7877 RVA: 0x000901CF File Offset: 0x0008E3CF
		public override void Flush()
		{
			base.InnerStream.Flush();
		}

		// Token: 0x06001EC6 RID: 7878 RVA: 0x000901DC File Offset: 0x0008E3DC
		protected override void Dispose(bool disposing)
		{
			try
			{
				this._NegoState.Close();
			}
			finally
			{
				base.Dispose(disposing);
			}
		}

		// Token: 0x06001EC7 RID: 7879 RVA: 0x00090210 File Offset: 0x0008E410
		public override int Read(byte[] buffer, int offset, int count)
		{
			this._NegoState.CheckThrow(true);
			if (!this._NegoState.CanGetSecureStream)
			{
				return base.InnerStream.Read(buffer, offset, count);
			}
			return this.ProcessRead(buffer, offset, count, null);
		}

		// Token: 0x06001EC8 RID: 7880 RVA: 0x00090244 File Offset: 0x0008E444
		public override void Write(byte[] buffer, int offset, int count)
		{
			this._NegoState.CheckThrow(true);
			if (!this._NegoState.CanGetSecureStream)
			{
				base.InnerStream.Write(buffer, offset, count);
				return;
			}
			this.ProcessWrite(buffer, offset, count, null);
		}

		// Token: 0x06001EC9 RID: 7881 RVA: 0x00090278 File Offset: 0x0008E478
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public override IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback asyncCallback, object asyncState)
		{
			this._NegoState.CheckThrow(true);
			if (!this._NegoState.CanGetSecureStream)
			{
				return base.InnerStream.BeginRead(buffer, offset, count, asyncCallback, asyncState);
			}
			BufferAsyncResult bufferAsyncResult = new BufferAsyncResult(this, buffer, offset, count, asyncState, asyncCallback);
			AsyncProtocolRequest asyncProtocolRequest = new AsyncProtocolRequest(bufferAsyncResult);
			this.ProcessRead(buffer, offset, count, asyncProtocolRequest);
			return bufferAsyncResult;
		}

		// Token: 0x06001ECA RID: 7882 RVA: 0x000902D4 File Offset: 0x0008E4D4
		public override int EndRead(IAsyncResult asyncResult)
		{
			this._NegoState.CheckThrow(true);
			if (!this._NegoState.CanGetSecureStream)
			{
				return base.InnerStream.EndRead(asyncResult);
			}
			if (asyncResult == null)
			{
				throw new ArgumentNullException("asyncResult");
			}
			BufferAsyncResult bufferAsyncResult = asyncResult as BufferAsyncResult;
			if (bufferAsyncResult == null)
			{
				throw new ArgumentException(SR.GetString("net_io_async_result", new object[] { asyncResult.GetType().FullName }), "asyncResult");
			}
			if (Interlocked.Exchange(ref this._NestedRead, 0) == 0)
			{
				throw new InvalidOperationException(SR.GetString("net_io_invalidendcall", new object[] { "EndRead" }));
			}
			bufferAsyncResult.InternalWaitForCompletion();
			if (!(bufferAsyncResult.Result is Exception))
			{
				return (int)bufferAsyncResult.Result;
			}
			if (bufferAsyncResult.Result is IOException)
			{
				throw (Exception)bufferAsyncResult.Result;
			}
			throw new IOException(SR.GetString("net_io_read"), (Exception)bufferAsyncResult.Result);
		}

		// Token: 0x06001ECB RID: 7883 RVA: 0x000903C8 File Offset: 0x0008E5C8
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public override IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback asyncCallback, object asyncState)
		{
			this._NegoState.CheckThrow(true);
			if (!this._NegoState.CanGetSecureStream)
			{
				return base.InnerStream.BeginWrite(buffer, offset, count, asyncCallback, asyncState);
			}
			BufferAsyncResult bufferAsyncResult = new BufferAsyncResult(this, buffer, offset, count, true, asyncState, asyncCallback);
			AsyncProtocolRequest asyncProtocolRequest = new AsyncProtocolRequest(bufferAsyncResult);
			this.ProcessWrite(buffer, offset, count, asyncProtocolRequest);
			return bufferAsyncResult;
		}

		// Token: 0x06001ECC RID: 7884 RVA: 0x00090424 File Offset: 0x0008E624
		public override void EndWrite(IAsyncResult asyncResult)
		{
			this._NegoState.CheckThrow(true);
			if (!this._NegoState.CanGetSecureStream)
			{
				base.InnerStream.EndWrite(asyncResult);
				return;
			}
			if (asyncResult == null)
			{
				throw new ArgumentNullException("asyncResult");
			}
			BufferAsyncResult bufferAsyncResult = asyncResult as BufferAsyncResult;
			if (bufferAsyncResult == null)
			{
				throw new ArgumentException(SR.GetString("net_io_async_result", new object[] { asyncResult.GetType().FullName }), "asyncResult");
			}
			if (Interlocked.Exchange(ref this._NestedWrite, 0) == 0)
			{
				throw new InvalidOperationException(SR.GetString("net_io_invalidendcall", new object[] { "EndWrite" }));
			}
			bufferAsyncResult.InternalWaitForCompletion();
			if (!(bufferAsyncResult.Result is Exception))
			{
				return;
			}
			if (bufferAsyncResult.Result is IOException)
			{
				throw (Exception)bufferAsyncResult.Result;
			}
			throw new IOException(SR.GetString("net_io_write"), (Exception)bufferAsyncResult.Result);
		}

		// Token: 0x06001ECD RID: 7885 RVA: 0x0009050C File Offset: 0x0008E70C
		private void InitializeStreamPart()
		{
			this._ReadHeader = new byte[4];
			this._FrameReader = new FixedSizeReader(base.InnerStream);
		}

		// Token: 0x17000804 RID: 2052
		// (get) Token: 0x06001ECE RID: 7886 RVA: 0x0009052B File Offset: 0x0008E72B
		private byte[] InternalBuffer
		{
			get
			{
				return this._InternalBuffer;
			}
		}

		// Token: 0x17000805 RID: 2053
		// (get) Token: 0x06001ECF RID: 7887 RVA: 0x00090533 File Offset: 0x0008E733
		private int InternalOffset
		{
			get
			{
				return this._InternalOffset;
			}
		}

		// Token: 0x17000806 RID: 2054
		// (get) Token: 0x06001ED0 RID: 7888 RVA: 0x0009053B File Offset: 0x0008E73B
		private int InternalBufferCount
		{
			get
			{
				return this._InternalBufferCount;
			}
		}

		// Token: 0x06001ED1 RID: 7889 RVA: 0x00090543 File Offset: 0x0008E743
		private void DecrementInternalBufferCount(int decrCount)
		{
			this._InternalOffset += decrCount;
			this._InternalBufferCount -= decrCount;
		}

		// Token: 0x06001ED2 RID: 7890 RVA: 0x00090561 File Offset: 0x0008E761
		private void EnsureInternalBufferSize(int bytes)
		{
			this._InternalBufferCount = bytes;
			this._InternalOffset = 0;
			if (this.InternalBuffer == null || this.InternalBuffer.Length < bytes)
			{
				this._InternalBuffer = new byte[bytes];
			}
		}

		// Token: 0x06001ED3 RID: 7891 RVA: 0x00090590 File Offset: 0x0008E790
		private void AdjustInternalBufferOffsetSize(int bytes, int offset)
		{
			this._InternalBufferCount = bytes;
			this._InternalOffset = offset;
		}

		// Token: 0x06001ED4 RID: 7892 RVA: 0x000905A0 File Offset: 0x0008E7A0
		private void ValidateParameters(byte[] buffer, int offset, int count)
		{
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			if (offset < 0)
			{
				throw new ArgumentOutOfRangeException("offset");
			}
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count");
			}
			if (count > buffer.Length - offset)
			{
				throw new ArgumentOutOfRangeException("count", SR.GetString("net_offset_plus_count"));
			}
		}

		// Token: 0x06001ED5 RID: 7893 RVA: 0x000905F8 File Offset: 0x0008E7F8
		private void ProcessWrite(byte[] buffer, int offset, int count, AsyncProtocolRequest asyncRequest)
		{
			this.ValidateParameters(buffer, offset, count);
			if (Interlocked.Exchange(ref this._NestedWrite, 1) == 1)
			{
				throw new NotSupportedException(SR.GetString("net_io_invalidnestedcall", new object[]
				{
					(asyncRequest != null) ? "BeginWrite" : "Write",
					"write"
				}));
			}
			bool flag = false;
			try
			{
				this.StartWriting(buffer, offset, count, asyncRequest);
			}
			catch (Exception ex)
			{
				flag = true;
				if (ex is IOException)
				{
					throw;
				}
				throw new IOException(SR.GetString("net_io_write"), ex);
			}
			finally
			{
				if (asyncRequest == null || flag)
				{
					this._NestedWrite = 0;
				}
			}
		}

		// Token: 0x06001ED6 RID: 7894 RVA: 0x000906A8 File Offset: 0x0008E8A8
		private void StartWriting(byte[] buffer, int offset, int count, AsyncProtocolRequest asyncRequest)
		{
			if (count >= 0)
			{
				byte[] array = null;
				for (;;)
				{
					int num = Math.Min(count, 64512);
					int num2;
					try
					{
						num2 = this._NegoState.EncryptData(buffer, offset, num, ref array);
					}
					catch (Exception ex)
					{
						throw new IOException(SR.GetString("net_io_encrypt"), ex);
					}
					if (asyncRequest != null)
					{
						asyncRequest.SetNextRequest(buffer, offset + num, count - num, null);
						IAsyncResult asyncResult = base.InnerStream.BeginWrite(array, 0, num2, NegotiateStream._WriteCallback, asyncRequest);
						if (!asyncResult.CompletedSynchronously)
						{
							break;
						}
						base.InnerStream.EndWrite(asyncResult);
					}
					else
					{
						base.InnerStream.Write(array, 0, num2);
					}
					offset += num;
					count -= num;
					if (count == 0)
					{
						goto IL_009B;
					}
				}
				return;
			}
			IL_009B:
			if (asyncRequest != null)
			{
				asyncRequest.CompleteUser();
			}
		}

		// Token: 0x06001ED7 RID: 7895 RVA: 0x0009076C File Offset: 0x0008E96C
		private int ProcessRead(byte[] buffer, int offset, int count, AsyncProtocolRequest asyncRequest)
		{
			this.ValidateParameters(buffer, offset, count);
			if (Interlocked.Exchange(ref this._NestedRead, 1) == 1)
			{
				throw new NotSupportedException(SR.GetString("net_io_invalidnestedcall", new object[]
				{
					(asyncRequest != null) ? "BeginRead" : "Read",
					"read"
				}));
			}
			bool flag = false;
			int num2;
			try
			{
				if (this.InternalBufferCount != 0)
				{
					int num = ((this.InternalBufferCount > count) ? count : this.InternalBufferCount);
					if (num != 0)
					{
						Buffer.BlockCopy(this.InternalBuffer, this.InternalOffset, buffer, offset, num);
						this.DecrementInternalBufferCount(num);
					}
					if (asyncRequest != null)
					{
						asyncRequest.CompleteUser(num);
					}
					num2 = num;
				}
				else
				{
					num2 = this.StartReading(buffer, offset, count, asyncRequest);
				}
			}
			catch (Exception ex)
			{
				flag = true;
				if (ex is IOException)
				{
					throw;
				}
				throw new IOException(SR.GetString("net_io_read"), ex);
			}
			finally
			{
				if (asyncRequest == null || flag)
				{
					this._NestedRead = 0;
				}
			}
			return num2;
		}

		// Token: 0x06001ED8 RID: 7896 RVA: 0x0009086C File Offset: 0x0008EA6C
		private int StartReading(byte[] buffer, int offset, int count, AsyncProtocolRequest asyncRequest)
		{
			int num;
			while ((num = this.StartFrameHeader(buffer, offset, count, asyncRequest)) == -1)
			{
			}
			return num;
		}

		// Token: 0x06001ED9 RID: 7897 RVA: 0x0009088C File Offset: 0x0008EA8C
		private int StartFrameHeader(byte[] buffer, int offset, int count, AsyncProtocolRequest asyncRequest)
		{
			int num;
			if (asyncRequest != null)
			{
				asyncRequest.SetNextRequest(this._ReadHeader, 0, this._ReadHeader.Length, NegotiateStream._ReadCallback);
				this._FrameReader.AsyncReadPacket(asyncRequest);
				if (!asyncRequest.MustCompleteSynchronously)
				{
					return 0;
				}
				num = asyncRequest.Result;
			}
			else
			{
				num = this._FrameReader.ReadPacket(this._ReadHeader, 0, this._ReadHeader.Length);
			}
			return this.StartFrameBody(num, buffer, offset, count, asyncRequest);
		}

		// Token: 0x06001EDA RID: 7898 RVA: 0x00090904 File Offset: 0x0008EB04
		private int StartFrameBody(int readBytes, byte[] buffer, int offset, int count, AsyncProtocolRequest asyncRequest)
		{
			if (readBytes == 0)
			{
				if (asyncRequest != null)
				{
					asyncRequest.CompleteUser(0);
				}
				return 0;
			}
			readBytes = (int)this._ReadHeader[3];
			readBytes = (readBytes << 8) | (int)this._ReadHeader[2];
			readBytes = (readBytes << 8) | (int)this._ReadHeader[1];
			readBytes = (readBytes << 8) | (int)this._ReadHeader[0];
			if (readBytes <= 4 || readBytes > 65536)
			{
				throw new IOException(SR.GetString("net_frame_read_size"));
			}
			this.EnsureInternalBufferSize(readBytes);
			if (asyncRequest != null)
			{
				asyncRequest.SetNextRequest(this.InternalBuffer, 0, readBytes, NegotiateStream._ReadCallback);
				this._FrameReader.AsyncReadPacket(asyncRequest);
				if (!asyncRequest.MustCompleteSynchronously)
				{
					return 0;
				}
				readBytes = asyncRequest.Result;
			}
			else
			{
				readBytes = this._FrameReader.ReadPacket(this.InternalBuffer, 0, readBytes);
			}
			return this.ProcessFrameBody(readBytes, buffer, offset, count, asyncRequest);
		}

		// Token: 0x06001EDB RID: 7899 RVA: 0x000909DC File Offset: 0x0008EBDC
		private int ProcessFrameBody(int readBytes, byte[] buffer, int offset, int count, AsyncProtocolRequest asyncRequest)
		{
			if (readBytes == 0)
			{
				throw new IOException(SR.GetString("net_io_eof"));
			}
			int num;
			readBytes = this._NegoState.DecryptData(this.InternalBuffer, 0, readBytes, out num);
			this.AdjustInternalBufferOffsetSize(readBytes, num);
			if (readBytes == 0 && count != 0)
			{
				return -1;
			}
			if (readBytes > count)
			{
				readBytes = count;
			}
			Buffer.BlockCopy(this.InternalBuffer, this.InternalOffset, buffer, offset, readBytes);
			this.DecrementInternalBufferCount(readBytes);
			if (asyncRequest != null)
			{
				asyncRequest.CompleteUser(readBytes);
			}
			return readBytes;
		}

		// Token: 0x06001EDC RID: 7900 RVA: 0x00090A5C File Offset: 0x0008EC5C
		private static void WriteCallback(IAsyncResult transportResult)
		{
			if (transportResult.CompletedSynchronously)
			{
				return;
			}
			AsyncProtocolRequest asyncProtocolRequest = (AsyncProtocolRequest)transportResult.AsyncState;
			try
			{
				NegotiateStream negotiateStream = (NegotiateStream)asyncProtocolRequest.AsyncObject;
				negotiateStream.InnerStream.EndWrite(transportResult);
				if (asyncProtocolRequest.Count == 0)
				{
					asyncProtocolRequest.Count = -1;
				}
				negotiateStream.StartWriting(asyncProtocolRequest.Buffer, asyncProtocolRequest.Offset, asyncProtocolRequest.Count, asyncProtocolRequest);
			}
			catch (Exception ex)
			{
				if (asyncProtocolRequest.IsUserCompleted)
				{
					throw;
				}
				asyncProtocolRequest.CompleteWithError(ex);
			}
		}

		// Token: 0x06001EDD RID: 7901 RVA: 0x00090AE4 File Offset: 0x0008ECE4
		private static void ReadCallback(AsyncProtocolRequest asyncRequest)
		{
			try
			{
				NegotiateStream negotiateStream = (NegotiateStream)asyncRequest.AsyncObject;
				BufferAsyncResult bufferAsyncResult = (BufferAsyncResult)asyncRequest.UserAsyncResult;
				if (asyncRequest.Buffer == negotiateStream._ReadHeader)
				{
					negotiateStream.StartFrameBody(asyncRequest.Result, bufferAsyncResult.Buffer, bufferAsyncResult.Offset, bufferAsyncResult.Count, asyncRequest);
				}
				else if (-1 == negotiateStream.ProcessFrameBody(asyncRequest.Result, bufferAsyncResult.Buffer, bufferAsyncResult.Offset, bufferAsyncResult.Count, asyncRequest))
				{
					negotiateStream.StartReading(bufferAsyncResult.Buffer, bufferAsyncResult.Offset, bufferAsyncResult.Count, asyncRequest);
				}
			}
			catch (Exception ex)
			{
				if (asyncRequest.IsUserCompleted)
				{
					throw;
				}
				asyncRequest.CompleteWithError(ex);
			}
		}

		// Token: 0x04001CED RID: 7405
		private NegoState _NegoState;

		// Token: 0x04001CEE RID: 7406
		private string _Package;

		// Token: 0x04001CEF RID: 7407
		private IIdentity _RemoteIdentity;

		// Token: 0x04001CF0 RID: 7408
		private static AsyncCallback _WriteCallback = new AsyncCallback(NegotiateStream.WriteCallback);

		// Token: 0x04001CF1 RID: 7409
		private static AsyncProtocolCallback _ReadCallback = new AsyncProtocolCallback(NegotiateStream.ReadCallback);

		// Token: 0x04001CF2 RID: 7410
		private int _NestedWrite;

		// Token: 0x04001CF3 RID: 7411
		private int _NestedRead;

		// Token: 0x04001CF4 RID: 7412
		private byte[] _ReadHeader;

		// Token: 0x04001CF5 RID: 7413
		private byte[] _InternalBuffer;

		// Token: 0x04001CF6 RID: 7414
		private int _InternalOffset;

		// Token: 0x04001CF7 RID: 7415
		private int _InternalBufferCount;

		// Token: 0x04001CF8 RID: 7416
		private FixedSizeReader _FrameReader;
	}
}
