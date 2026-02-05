using System;
using System.Collections;
using System.Globalization;
using System.IO;
using System.Net.Cache;
using System.Net.Sockets;
using System.Security;
using System.Security.Authentication;
using System.Security.Permissions;
using System.Text;

namespace System.Net
{
	// Token: 0x020001B1 RID: 433
	internal class FtpControlStream : CommandStream
	{
		// Token: 0x170003AD RID: 941
		// (get) Token: 0x060010F3 RID: 4339 RVA: 0x0005BA59 File Offset: 0x00059C59
		// (set) Token: 0x060010F4 RID: 4340 RVA: 0x0005BA82 File Offset: 0x00059C82
		internal NetworkCredential Credentials
		{
			get
			{
				if (this.m_Credentials != null && this.m_Credentials.IsAlive)
				{
					return (NetworkCredential)this.m_Credentials.Target;
				}
				return null;
			}
			set
			{
				if (this.m_Credentials == null)
				{
					this.m_Credentials = new WeakReference(null);
				}
				this.m_Credentials.Target = value;
			}
		}

		// Token: 0x060010F5 RID: 4341 RVA: 0x0005BAA4 File Offset: 0x00059CA4
		internal FtpControlStream(ConnectionPool connectionPool, TimeSpan lifetime, bool checkLifetime)
			: base(connectionPool, lifetime, checkLifetime)
		{
		}

		// Token: 0x060010F6 RID: 4342 RVA: 0x0005BAC4 File Offset: 0x00059CC4
		internal void AbortConnect()
		{
			Socket dataSocket = this.m_DataSocket;
			if (dataSocket != null)
			{
				try
				{
					dataSocket.Close();
				}
				catch (ObjectDisposedException)
				{
				}
			}
		}

		// Token: 0x060010F7 RID: 4343 RVA: 0x0005BAF8 File Offset: 0x00059CF8
		private static void AcceptCallback(IAsyncResult asyncResult)
		{
			FtpControlStream ftpControlStream = (FtpControlStream)asyncResult.AsyncState;
			LazyAsyncResult lazyAsyncResult = asyncResult as LazyAsyncResult;
			Socket socket = (Socket)lazyAsyncResult.AsyncObject;
			try
			{
				ftpControlStream.m_DataSocket = socket.EndAccept(asyncResult);
				if (!ftpControlStream.ServerAddress.Equals(((IPEndPoint)ftpControlStream.m_DataSocket.RemoteEndPoint).Address))
				{
					ftpControlStream.m_DataSocket.Close();
					throw new WebException(SR.GetString("net_ftp_active_address_different"), WebExceptionStatus.ProtocolError);
				}
				ftpControlStream.ContinueCommandPipeline();
			}
			catch (Exception ex)
			{
				ftpControlStream.CloseSocket();
				ftpControlStream.InvokeRequestCallback(ex);
			}
			finally
			{
				socket.Close();
			}
		}

		// Token: 0x060010F8 RID: 4344 RVA: 0x0005BBAC File Offset: 0x00059DAC
		private static void ConnectCallback(IAsyncResult asyncResult)
		{
			FtpControlStream ftpControlStream = (FtpControlStream)asyncResult.AsyncState;
			try
			{
				LazyAsyncResult lazyAsyncResult = asyncResult as LazyAsyncResult;
				Socket socket = (Socket)lazyAsyncResult.AsyncObject;
				socket.EndConnect(asyncResult);
				ftpControlStream.ContinueCommandPipeline();
			}
			catch (Exception ex)
			{
				ftpControlStream.CloseSocket();
				ftpControlStream.InvokeRequestCallback(ex);
			}
		}

		// Token: 0x060010F9 RID: 4345 RVA: 0x0005BC08 File Offset: 0x00059E08
		private static void SSLHandshakeCallback(IAsyncResult asyncResult)
		{
			FtpControlStream ftpControlStream = (FtpControlStream)asyncResult.AsyncState;
			try
			{
				ftpControlStream.ContinueCommandPipeline();
			}
			catch (Exception ex)
			{
				ftpControlStream.CloseSocket();
				ftpControlStream.InvokeRequestCallback(ex);
			}
		}

		// Token: 0x060010FA RID: 4346 RVA: 0x0005BC4C File Offset: 0x00059E4C
		private CommandStream.PipelineInstruction QueueOrCreateFtpDataStream(ref Stream stream)
		{
			if (this.m_DataSocket == null)
			{
				throw new InternalException();
			}
			if (this.m_TlsStream != null)
			{
				stream = new FtpDataStream(this.m_TlsStream, (FtpWebRequest)this.m_Request, this.IsFtpDataStreamWriteable());
				this.m_TlsStream = null;
				return CommandStream.PipelineInstruction.GiveStream;
			}
			NetworkStream networkStream = new NetworkStream(this.m_DataSocket, true);
			if (base.UsingSecureStream)
			{
				FtpWebRequest ftpWebRequest = (FtpWebRequest)this.m_Request;
				TlsStream tlsStream = new TlsStream(ftpWebRequest.RequestUri.Host, networkStream, ServicePointManager.CheckCertificateRevocationList, (SslProtocols)ServicePointManager.SecurityProtocol, ftpWebRequest.ClientCertificates, base.Pool.ServicePoint, ftpWebRequest, this.m_Async ? ftpWebRequest.GetWritingContext().ContextCopy : null);
				networkStream = tlsStream;
				if (this.m_Async)
				{
					this.m_TlsStream = tlsStream;
					LazyAsyncResult lazyAsyncResult = new LazyAsyncResult(null, this, FtpControlStream.m_SSLHandshakeCallback);
					tlsStream.ProcessAuthentication(lazyAsyncResult);
					return CommandStream.PipelineInstruction.Pause;
				}
				tlsStream.ProcessAuthentication(null);
			}
			stream = new FtpDataStream(networkStream, (FtpWebRequest)this.m_Request, this.IsFtpDataStreamWriteable());
			return CommandStream.PipelineInstruction.GiveStream;
		}

		// Token: 0x060010FB RID: 4347 RVA: 0x0005BD48 File Offset: 0x00059F48
		protected override void ClearState()
		{
			this.m_ContentLength = -1L;
			this.m_LastModified = DateTime.MinValue;
			this.m_ResponseUri = null;
			this.m_DataHandshakeStarted = false;
			this.StatusCode = FtpStatusCode.Undefined;
			this.StatusLine = null;
			this.m_DataSocket = null;
			this.m_PassiveEndPoint = null;
			this.m_TlsStream = null;
			base.ClearState();
		}

		// Token: 0x060010FC RID: 4348 RVA: 0x0005BDA0 File Offset: 0x00059FA0
		protected override CommandStream.PipelineInstruction PipelineCallback(CommandStream.PipelineEntry entry, ResponseDescription response, bool timeout, ref Stream stream)
		{
			if (response == null)
			{
				return CommandStream.PipelineInstruction.Abort;
			}
			FtpStatusCode status = (FtpStatusCode)response.Status;
			if (status != FtpStatusCode.ClosingControl)
			{
				this.StatusCode = status;
				this.StatusLine = response.StatusDescription;
			}
			if (response.InvalidStatusCode)
			{
				throw new WebException(SR.GetString("net_InvalidStatusCode"), WebExceptionStatus.ProtocolError);
			}
			if (this.m_Index == -1)
			{
				if (status == FtpStatusCode.SendUserCommand)
				{
					this.m_BannerMessage = new StringBuilder();
					this.m_BannerMessage.Append(this.StatusLine);
					return CommandStream.PipelineInstruction.Advance;
				}
				if (status == FtpStatusCode.ServiceTemporarilyNotAvailable)
				{
					return CommandStream.PipelineInstruction.Reread;
				}
				throw base.GenerateException(status, response.StatusDescription, null);
			}
			else
			{
				if (entry.Command == "OPTS utf8 on\r\n")
				{
					if (response.PositiveCompletion)
					{
						base.Encoding = Encoding.UTF8;
					}
					else
					{
						base.Encoding = Encoding.Default;
					}
					return CommandStream.PipelineInstruction.Advance;
				}
				if (entry.Command.IndexOf("USER") != -1)
				{
					if (status == FtpStatusCode.LoggedInProceed)
					{
						this.m_LoginState = FtpLoginState.LoggedIn;
						this.m_Index++;
					}
					else if (status == FtpStatusCode.NotLoggedIn && this.m_LoginState != FtpLoginState.NotLoggedIn)
					{
						this.m_LoginState = FtpLoginState.ReloginFailed;
						throw ExceptionHelper.IsolatedException;
					}
				}
				if (response.TransientFailure || response.PermanentFailure)
				{
					if (status == FtpStatusCode.ServiceNotAvailable)
					{
						base.MarkAsRecoverableFailure();
					}
					throw base.GenerateException(status, response.StatusDescription, null);
				}
				if (this.m_LoginState != FtpLoginState.LoggedIn && entry.Command.IndexOf("PASS") != -1)
				{
					if (status != FtpStatusCode.NeedLoginAccount && status != FtpStatusCode.LoggedInProceed)
					{
						throw base.GenerateException(status, response.StatusDescription, null);
					}
					this.m_LoginState = FtpLoginState.LoggedIn;
				}
				if (entry.HasFlag(CommandStream.PipelineEntryFlags.CreateDataConnection) && (response.PositiveCompletion || response.PositiveIntermediate))
				{
					bool flag;
					CommandStream.PipelineInstruction pipelineInstruction = this.QueueOrCreateDataConection(entry, response, timeout, ref stream, out flag);
					if (!flag)
					{
						return pipelineInstruction;
					}
				}
				if (status == FtpStatusCode.OpeningData || status == FtpStatusCode.DataAlreadyOpen)
				{
					if (this.m_DataSocket == null)
					{
						return CommandStream.PipelineInstruction.Abort;
					}
					if (!entry.HasFlag(CommandStream.PipelineEntryFlags.GiveDataStream))
					{
						this.m_AbortReason = SR.GetString("net_ftp_invalid_status_response", new object[] { status, entry.Command });
						return CommandStream.PipelineInstruction.Abort;
					}
					this.TryUpdateContentLength(response.StatusDescription);
					FtpWebRequest ftpWebRequest = (FtpWebRequest)this.m_Request;
					if (ftpWebRequest.MethodInfo.ShouldParseForResponseUri)
					{
						this.TryUpdateResponseUri(response.StatusDescription, ftpWebRequest);
					}
					return this.QueueOrCreateFtpDataStream(ref stream);
				}
				else
				{
					if (status == FtpStatusCode.LoggedInProceed)
					{
						this.m_WelcomeMessage.Append(this.StatusLine);
					}
					else if (status == FtpStatusCode.ClosingControl)
					{
						this.m_ExitMessage.Append(response.StatusDescription);
						base.CloseSocket();
					}
					else if (status == FtpStatusCode.ServerWantsSecureSession)
					{
						FtpWebRequest ftpWebRequest2 = (FtpWebRequest)this.m_Request;
						TlsStream tlsStream = new TlsStream(ftpWebRequest2.RequestUri.Host, base.NetworkStream, ServicePointManager.CheckCertificateRevocationList, (SslProtocols)ServicePointManager.SecurityProtocol, ftpWebRequest2.ClientCertificates, base.Pool.ServicePoint, ftpWebRequest2, this.m_Async ? ftpWebRequest2.GetWritingContext().ContextCopy : null);
						base.NetworkStream = tlsStream;
					}
					else if (status == FtpStatusCode.FileStatus)
					{
						FtpWebRequest ftpWebRequest3 = (FtpWebRequest)this.m_Request;
						if (entry.Command.StartsWith("SIZE "))
						{
							this.m_ContentLength = this.GetContentLengthFrom213Response(response.StatusDescription);
						}
						else if (entry.Command.StartsWith("MDTM "))
						{
							this.m_LastModified = this.GetLastModifiedFrom213Response(response.StatusDescription);
						}
					}
					else if (status == FtpStatusCode.PathnameCreated)
					{
						if (entry.Command == "PWD\r\n" && !entry.HasFlag(CommandStream.PipelineEntryFlags.UserCommand))
						{
							this.m_LoginDirectory = this.GetLoginDirectory(response.StatusDescription);
						}
					}
					else if (entry.Command.IndexOf("CWD") != -1)
					{
						this.m_EstablishedServerDirectory = this.m_RequestedServerDirectory;
					}
					if (response.PositiveIntermediate || (!base.UsingSecureStream && entry.Command == "AUTH TLS\r\n"))
					{
						return CommandStream.PipelineInstruction.Reread;
					}
					return CommandStream.PipelineInstruction.Advance;
				}
			}
		}

		// Token: 0x060010FD RID: 4349 RVA: 0x0005C168 File Offset: 0x0005A368
		protected override CommandStream.PipelineEntry[] BuildCommandsList(WebRequest req)
		{
			bool flag = false;
			FtpWebRequest ftpWebRequest = (FtpWebRequest)req;
			this.m_ResponseUri = ftpWebRequest.RequestUri;
			ArrayList arrayList = new ArrayList();
			if (ftpWebRequest.EnableSsl && !base.UsingSecureStream)
			{
				arrayList.Add(new CommandStream.PipelineEntry(this.FormatFtpCommand("AUTH", "TLS")));
				flag = true;
			}
			if (flag)
			{
				this.m_LoginDirectory = null;
				this.m_EstablishedServerDirectory = null;
				this.m_RequestedServerDirectory = null;
				this.m_CurrentTypeSetting = string.Empty;
				if (this.m_LoginState == FtpLoginState.LoggedIn)
				{
					this.m_LoginState = FtpLoginState.LoggedInButNeedsRelogin;
				}
			}
			if (this.m_LoginState != FtpLoginState.LoggedIn)
			{
				this.Credentials = ftpWebRequest.Credentials.GetCredential(ftpWebRequest.RequestUri, "basic");
				this.m_WelcomeMessage = new StringBuilder();
				this.m_ExitMessage = new StringBuilder();
				string text = string.Empty;
				string text2 = string.Empty;
				if (this.Credentials != null)
				{
					text = this.Credentials.InternalGetDomainUserName();
					text2 = this.Credentials.InternalGetPassword();
				}
				if (text.Length == 0 && text2.Length == 0)
				{
					text = "anonymous";
					text2 = "anonymous@";
				}
				arrayList.Add(new CommandStream.PipelineEntry(this.FormatFtpCommand("USER", text)));
				arrayList.Add(new CommandStream.PipelineEntry(this.FormatFtpCommand("PASS", text2), CommandStream.PipelineEntryFlags.DontLogParameter));
				if (ftpWebRequest.EnableSsl && !base.UsingSecureStream)
				{
					arrayList.Add(new CommandStream.PipelineEntry(this.FormatFtpCommand("PBSZ", "0")));
					arrayList.Add(new CommandStream.PipelineEntry(this.FormatFtpCommand("PROT", "P")));
				}
				arrayList.Add(new CommandStream.PipelineEntry(this.FormatFtpCommand("OPTS", "utf8 on")));
				arrayList.Add(new CommandStream.PipelineEntry(this.FormatFtpCommand("PWD", null)));
			}
			FtpControlStream.GetPathOption getPathOption = FtpControlStream.GetPathOption.Normal;
			if (ftpWebRequest.MethodInfo.HasFlag(FtpMethodFlags.DoesNotTakeParameter))
			{
				getPathOption = FtpControlStream.GetPathOption.AssumeNoFilename;
			}
			else if (ftpWebRequest.MethodInfo.HasFlag(FtpMethodFlags.ParameterIsDirectory))
			{
				getPathOption = FtpControlStream.GetPathOption.AssumeFilename;
			}
			string text3;
			string text4;
			string text5;
			FtpControlStream.GetPathInfo(getPathOption, ftpWebRequest.RequestUri, out text3, out text4, out text5);
			if (text5.Length == 0 && ftpWebRequest.MethodInfo.HasFlag(FtpMethodFlags.TakesParameter))
			{
				throw new WebException(SR.GetString("net_ftp_invalid_uri"));
			}
			if (this.m_EstablishedServerDirectory != null && this.m_LoginDirectory != null && this.m_EstablishedServerDirectory != this.m_LoginDirectory)
			{
				arrayList.Add(new CommandStream.PipelineEntry(this.FormatFtpCommand("CWD", this.m_LoginDirectory), CommandStream.PipelineEntryFlags.UserCommand));
				this.m_RequestedServerDirectory = this.m_LoginDirectory;
			}
			if (ftpWebRequest.MethodInfo.HasFlag(FtpMethodFlags.MustChangeWorkingDirectoryToPath) && text4.Length > 0)
			{
				arrayList.Add(new CommandStream.PipelineEntry(this.FormatFtpCommand("CWD", text4), CommandStream.PipelineEntryFlags.UserCommand));
				this.m_RequestedServerDirectory = text4;
			}
			if (ftpWebRequest.CacheProtocol != null && ftpWebRequest.CacheProtocol.ProtocolStatus == CacheValidationStatus.DoNotTakeFromCache && ftpWebRequest.MethodInfo.Operation == FtpOperation.DownloadFile)
			{
				arrayList.Add(new CommandStream.PipelineEntry(this.FormatFtpCommand("MDTM", text3)));
			}
			if (!ftpWebRequest.MethodInfo.IsCommandOnly)
			{
				if (ftpWebRequest.CacheProtocol == null || ftpWebRequest.CacheProtocol.ProtocolStatus != CacheValidationStatus.Continue)
				{
					string text6 = (ftpWebRequest.UseBinary ? "I" : "A");
					if (this.m_CurrentTypeSetting != text6)
					{
						arrayList.Add(new CommandStream.PipelineEntry(this.FormatFtpCommand("TYPE", text6)));
						this.m_CurrentTypeSetting = text6;
					}
					if (ftpWebRequest.UsePassive)
					{
						string text7 = ((base.ServerAddress.AddressFamily == AddressFamily.InterNetwork) ? "PASV" : "EPSV");
						arrayList.Add(new CommandStream.PipelineEntry(this.FormatFtpCommand(text7, null), CommandStream.PipelineEntryFlags.CreateDataConnection));
					}
					else
					{
						string text8 = ((base.ServerAddress.AddressFamily == AddressFamily.InterNetwork) ? "PORT" : "EPRT");
						this.CreateFtpListenerSocket(ftpWebRequest);
						arrayList.Add(new CommandStream.PipelineEntry(this.FormatFtpCommand(text8, this.GetPortCommandLine(ftpWebRequest))));
					}
					if (ftpWebRequest.CacheProtocol != null && ftpWebRequest.CacheProtocol.ProtocolStatus == CacheValidationStatus.CombineCachedAndServerResponse)
					{
						if (ftpWebRequest.CacheProtocol.Validator.CacheEntry.StreamSize > 0L)
						{
							arrayList.Add(new CommandStream.PipelineEntry(this.FormatFtpCommand("REST", ftpWebRequest.CacheProtocol.Validator.CacheEntry.StreamSize.ToString(CultureInfo.InvariantCulture))));
						}
					}
					else if (ftpWebRequest.ContentOffset > 0L)
					{
						arrayList.Add(new CommandStream.PipelineEntry(this.FormatFtpCommand("REST", ftpWebRequest.ContentOffset.ToString(CultureInfo.InvariantCulture))));
					}
				}
				else
				{
					arrayList.Add(new CommandStream.PipelineEntry(this.FormatFtpCommand("SIZE", text3)));
					arrayList.Add(new CommandStream.PipelineEntry(this.FormatFtpCommand("MDTM", text3)));
				}
			}
			if (ftpWebRequest.CacheProtocol == null || ftpWebRequest.CacheProtocol.ProtocolStatus != CacheValidationStatus.Continue)
			{
				CommandStream.PipelineEntryFlags pipelineEntryFlags = CommandStream.PipelineEntryFlags.UserCommand;
				if (!ftpWebRequest.MethodInfo.IsCommandOnly)
				{
					pipelineEntryFlags |= CommandStream.PipelineEntryFlags.GiveDataStream;
					if (!ftpWebRequest.UsePassive)
					{
						pipelineEntryFlags |= CommandStream.PipelineEntryFlags.CreateDataConnection;
					}
				}
				if (ftpWebRequest.MethodInfo.Operation == FtpOperation.Rename)
				{
					string text9 = ((text4 == string.Empty) ? string.Empty : (text4 + "/"));
					arrayList.Add(new CommandStream.PipelineEntry(this.FormatFtpCommand("RNFR", text9 + text5), pipelineEntryFlags));
					string text10;
					if (!string.IsNullOrEmpty(ftpWebRequest.RenameTo) && ftpWebRequest.RenameTo.StartsWith("/", StringComparison.OrdinalIgnoreCase))
					{
						text10 = ftpWebRequest.RenameTo;
					}
					else
					{
						text10 = text9 + ftpWebRequest.RenameTo;
					}
					arrayList.Add(new CommandStream.PipelineEntry(this.FormatFtpCommand("RNTO", text10), pipelineEntryFlags));
				}
				else if (ftpWebRequest.MethodInfo.HasFlag(FtpMethodFlags.DoesNotTakeParameter))
				{
					arrayList.Add(new CommandStream.PipelineEntry(this.FormatFtpCommand(ftpWebRequest.Method, string.Empty), pipelineEntryFlags));
				}
				else if (ftpWebRequest.MethodInfo.HasFlag(FtpMethodFlags.MustChangeWorkingDirectoryToPath))
				{
					arrayList.Add(new CommandStream.PipelineEntry(this.FormatFtpCommand(ftpWebRequest.Method, text5), pipelineEntryFlags));
				}
				else
				{
					arrayList.Add(new CommandStream.PipelineEntry(this.FormatFtpCommand(ftpWebRequest.Method, text3), pipelineEntryFlags));
				}
				if (!ftpWebRequest.KeepAlive)
				{
					arrayList.Add(new CommandStream.PipelineEntry(this.FormatFtpCommand("QUIT", null)));
				}
			}
			return (CommandStream.PipelineEntry[])arrayList.ToArray(typeof(CommandStream.PipelineEntry));
		}

		// Token: 0x060010FE RID: 4350 RVA: 0x0005C7B4 File Offset: 0x0005A9B4
		private CommandStream.PipelineInstruction QueueOrCreateDataConection(CommandStream.PipelineEntry entry, ResponseDescription response, bool timeout, ref Stream stream, out bool isSocketReady)
		{
			isSocketReady = false;
			if (this.m_DataHandshakeStarted)
			{
				isSocketReady = true;
				return CommandStream.PipelineInstruction.Pause;
			}
			this.m_DataHandshakeStarted = true;
			bool flag = false;
			int num = -1;
			if (entry.Command == "PASV\r\n" || entry.Command == "EPSV\r\n")
			{
				if (!response.PositiveCompletion)
				{
					this.m_AbortReason = SR.GetString("net_ftp_server_failed_passive", new object[] { response.Status });
					return CommandStream.PipelineInstruction.Abort;
				}
				if (entry.Command == "PASV\r\n")
				{
					num = this.GetPortV4(response.StatusDescription);
				}
				else
				{
					num = this.GetPortV6(response.StatusDescription);
				}
				flag = true;
			}
			new SocketPermission(PermissionState.Unrestricted).Assert();
			CommandStream.PipelineInstruction pipelineInstruction2;
			try
			{
				if (flag)
				{
					try
					{
						this.m_DataSocket = this.CreateFtpDataSocket((FtpWebRequest)this.m_Request, base.Socket);
					}
					catch (ObjectDisposedException)
					{
						throw ExceptionHelper.RequestAbortedException;
					}
					IPEndPoint ipendPoint = new IPEndPoint(((IPEndPoint)base.Socket.LocalEndPoint).Address, 0);
					this.m_DataSocket.Bind(ipendPoint);
					this.m_PassiveEndPoint = new IPEndPoint(base.ServerAddress, num);
				}
				CommandStream.PipelineInstruction pipelineInstruction;
				if (this.m_PassiveEndPoint != null)
				{
					IPEndPoint passiveEndPoint = this.m_PassiveEndPoint;
					this.m_PassiveEndPoint = null;
					if (this.m_Async)
					{
						this.m_DataSocket.BeginConnect(passiveEndPoint, FtpControlStream.m_ConnectCallbackDelegate, this);
						pipelineInstruction = CommandStream.PipelineInstruction.Pause;
					}
					else
					{
						this.m_DataSocket.Connect(passiveEndPoint);
						pipelineInstruction = CommandStream.PipelineInstruction.Advance;
					}
				}
				else if (this.m_Async)
				{
					this.m_DataSocket.BeginAccept(FtpControlStream.m_AcceptCallbackDelegate, this);
					pipelineInstruction = CommandStream.PipelineInstruction.Pause;
				}
				else
				{
					Socket dataSocket = this.m_DataSocket;
					try
					{
						this.m_DataSocket = this.m_DataSocket.Accept();
						if (!base.ServerAddress.Equals(((IPEndPoint)this.m_DataSocket.RemoteEndPoint).Address))
						{
							this.m_DataSocket.Close();
							throw new WebException(SR.GetString("net_ftp_active_address_different"), WebExceptionStatus.ProtocolError);
						}
						isSocketReady = true;
						pipelineInstruction = CommandStream.PipelineInstruction.Pause;
					}
					finally
					{
						dataSocket.Close();
					}
				}
				pipelineInstruction2 = pipelineInstruction;
			}
			finally
			{
				CodeAccessPermission.RevertAssert();
			}
			return pipelineInstruction2;
		}

		// Token: 0x060010FF RID: 4351 RVA: 0x0005C9FC File Offset: 0x0005ABFC
		internal void Quit()
		{
			base.CloseSocket();
		}

		// Token: 0x06001100 RID: 4352 RVA: 0x0005CA04 File Offset: 0x0005AC04
		private static void GetPathInfo(FtpControlStream.GetPathOption pathOption, Uri uri, out string path, out string directory, out string filename)
		{
			path = uri.GetComponents(UriComponents.Path, UriFormat.Unescaped);
			int num = path.LastIndexOf('/');
			if (pathOption == FtpControlStream.GetPathOption.AssumeFilename && num != -1 && num == path.Length - 1)
			{
				path = path.Substring(0, path.Length - 1);
				num = path.LastIndexOf('/');
			}
			if (pathOption == FtpControlStream.GetPathOption.AssumeNoFilename)
			{
				directory = path;
				filename = string.Empty;
			}
			else
			{
				directory = path.Substring(0, num + 1);
				filename = path.Substring(num + 1, path.Length - (num + 1));
			}
			if (directory.Length > 1 && directory[directory.Length - 1] == '/')
			{
				directory = directory.Substring(0, directory.Length - 1);
			}
		}

		// Token: 0x06001101 RID: 4353 RVA: 0x0005CAC0 File Offset: 0x0005ACC0
		private string FormatAddress(IPAddress address, int Port)
		{
			byte[] addressBytes = address.GetAddressBytes();
			StringBuilder stringBuilder = new StringBuilder(32);
			foreach (byte b in addressBytes)
			{
				stringBuilder.Append(b);
				stringBuilder.Append(',');
			}
			stringBuilder.Append(Port / 256);
			stringBuilder.Append(',');
			stringBuilder.Append(Port % 256);
			return stringBuilder.ToString();
		}

		// Token: 0x06001102 RID: 4354 RVA: 0x0005CB30 File Offset: 0x0005AD30
		private string FormatAddressV6(IPAddress address, int port)
		{
			StringBuilder stringBuilder = new StringBuilder(43);
			string text = address.ToString();
			stringBuilder.Append("|2|");
			stringBuilder.Append(text);
			stringBuilder.Append('|');
			stringBuilder.Append(port.ToString(NumberFormatInfo.InvariantInfo));
			stringBuilder.Append('|');
			return stringBuilder.ToString();
		}

		// Token: 0x170003AE RID: 942
		// (get) Token: 0x06001103 RID: 4355 RVA: 0x0005CB8B File Offset: 0x0005AD8B
		internal long ContentLength
		{
			get
			{
				return this.m_ContentLength;
			}
		}

		// Token: 0x170003AF RID: 943
		// (get) Token: 0x06001104 RID: 4356 RVA: 0x0005CB93 File Offset: 0x0005AD93
		internal DateTime LastModified
		{
			get
			{
				return this.m_LastModified;
			}
		}

		// Token: 0x170003B0 RID: 944
		// (get) Token: 0x06001105 RID: 4357 RVA: 0x0005CB9B File Offset: 0x0005AD9B
		internal Uri ResponseUri
		{
			get
			{
				return this.m_ResponseUri;
			}
		}

		// Token: 0x170003B1 RID: 945
		// (get) Token: 0x06001106 RID: 4358 RVA: 0x0005CBA3 File Offset: 0x0005ADA3
		internal string BannerMessage
		{
			get
			{
				if (this.m_BannerMessage == null)
				{
					return null;
				}
				return this.m_BannerMessage.ToString();
			}
		}

		// Token: 0x170003B2 RID: 946
		// (get) Token: 0x06001107 RID: 4359 RVA: 0x0005CBBA File Offset: 0x0005ADBA
		internal string WelcomeMessage
		{
			get
			{
				if (this.m_WelcomeMessage == null)
				{
					return null;
				}
				return this.m_WelcomeMessage.ToString();
			}
		}

		// Token: 0x170003B3 RID: 947
		// (get) Token: 0x06001108 RID: 4360 RVA: 0x0005CBD1 File Offset: 0x0005ADD1
		internal string ExitMessage
		{
			get
			{
				if (this.m_ExitMessage == null)
				{
					return null;
				}
				return this.m_ExitMessage.ToString();
			}
		}

		// Token: 0x06001109 RID: 4361 RVA: 0x0005CBE8 File Offset: 0x0005ADE8
		private long GetContentLengthFrom213Response(string responseString)
		{
			string[] array = responseString.Split(new char[] { ' ' });
			if (array.Length < 2)
			{
				throw new FormatException(SR.GetString("net_ftp_response_invalid_format", new object[] { responseString }));
			}
			return Convert.ToInt64(array[1], NumberFormatInfo.InvariantInfo);
		}

		// Token: 0x0600110A RID: 4362 RVA: 0x0005CC34 File Offset: 0x0005AE34
		private DateTime GetLastModifiedFrom213Response(string str)
		{
			DateTime dateTime = this.m_LastModified;
			string[] array = str.Split(new char[] { ' ', '.' });
			if (array.Length < 2)
			{
				return dateTime;
			}
			string text = array[1];
			if (text.Length < 14)
			{
				return dateTime;
			}
			int num = Convert.ToInt32(text.Substring(0, 4), NumberFormatInfo.InvariantInfo);
			int num2 = (int)Convert.ToInt16(text.Substring(4, 2), NumberFormatInfo.InvariantInfo);
			int num3 = (int)Convert.ToInt16(text.Substring(6, 2), NumberFormatInfo.InvariantInfo);
			int num4 = (int)Convert.ToInt16(text.Substring(8, 2), NumberFormatInfo.InvariantInfo);
			int num5 = (int)Convert.ToInt16(text.Substring(10, 2), NumberFormatInfo.InvariantInfo);
			int num6 = (int)Convert.ToInt16(text.Substring(12, 2), NumberFormatInfo.InvariantInfo);
			int num7 = 0;
			if (array.Length > 2)
			{
				num7 = (int)Convert.ToInt16(array[2], NumberFormatInfo.InvariantInfo);
			}
			try
			{
				dateTime = new DateTime(num, num2, num3, num4, num5, num6, num7);
				dateTime = dateTime.ToLocalTime();
			}
			catch (ArgumentOutOfRangeException)
			{
			}
			catch (ArgumentException)
			{
			}
			return dateTime;
		}

		// Token: 0x0600110B RID: 4363 RVA: 0x0005CD4C File Offset: 0x0005AF4C
		private void TryUpdateResponseUri(string str, FtpWebRequest request)
		{
			Uri uri = request.RequestUri;
			int num = str.IndexOf("for ");
			if (num == -1)
			{
				return;
			}
			num += 4;
			int num2 = str.LastIndexOf('(');
			if (num2 == -1)
			{
				num2 = str.Length;
			}
			if (num2 <= num)
			{
				return;
			}
			string text = str.Substring(num, num2 - num);
			text = text.TrimEnd(new char[] { ' ', '.', '\r', '\n' });
			string text2 = text.Replace("%", "%25");
			text2 = text2.Replace("#", "%23");
			string absolutePath = uri.AbsolutePath;
			if (absolutePath.Length > 0 && absolutePath[absolutePath.Length - 1] != '/')
			{
				uri = new UriBuilder(uri)
				{
					Path = absolutePath + "/"
				}.Uri;
			}
			Uri uri2;
			if (!Uri.TryCreate(uri, text2, out uri2))
			{
				throw new FormatException(SR.GetString("net_ftp_invalid_response_filename", new object[] { text }));
			}
			if (!uri.IsBaseOf(uri2) || uri.Segments.Length != uri2.Segments.Length - 1)
			{
				throw new FormatException(SR.GetString("net_ftp_invalid_response_filename", new object[] { text }));
			}
			this.m_ResponseUri = uri2;
		}

		// Token: 0x0600110C RID: 4364 RVA: 0x0005CE88 File Offset: 0x0005B088
		private void TryUpdateContentLength(string str)
		{
			int num = str.LastIndexOf("(");
			if (num != -1)
			{
				int num2 = str.IndexOf(" bytes).");
				if (num2 != -1 && num2 > num)
				{
					num++;
					long num3;
					if (long.TryParse(str.Substring(num, num2 - num), NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite, NumberFormatInfo.InvariantInfo, out num3))
					{
						this.m_ContentLength = num3;
					}
				}
			}
		}

		// Token: 0x0600110D RID: 4365 RVA: 0x0005CEE0 File Offset: 0x0005B0E0
		private string GetLoginDirectory(string str)
		{
			int num = str.IndexOf('"');
			int num2 = str.LastIndexOf('"');
			if (num != -1 && num2 != -1 && num != num2)
			{
				return str.Substring(num + 1, num2 - num - 1);
			}
			return string.Empty;
		}

		// Token: 0x0600110E RID: 4366 RVA: 0x0005CF20 File Offset: 0x0005B120
		private int GetPortV4(string responseString)
		{
			string[] array = responseString.Split(new char[] { ' ', '(', ',', ')' });
			if (array.Length <= 7)
			{
				throw new FormatException(SR.GetString("net_ftp_response_invalid_format", new object[] { responseString }));
			}
			int num = array.Length - 1;
			if (!char.IsNumber(array[num], 0))
			{
				num--;
			}
			int num2 = (int)Convert.ToByte(array[num--], NumberFormatInfo.InvariantInfo);
			return num2 | ((int)Convert.ToByte(array[num--], NumberFormatInfo.InvariantInfo) << 8);
		}

		// Token: 0x0600110F RID: 4367 RVA: 0x0005CFA4 File Offset: 0x0005B1A4
		private int GetPortV6(string responseString)
		{
			int num = responseString.LastIndexOf("(");
			int num2 = responseString.LastIndexOf(")");
			if (num == -1 || num2 <= num)
			{
				throw new FormatException(SR.GetString("net_ftp_response_invalid_format", new object[] { responseString }));
			}
			string text = responseString.Substring(num + 1, num2 - num - 1);
			string[] array = text.Split(new char[] { '|' });
			if (array.Length < 4)
			{
				throw new FormatException(SR.GetString("net_ftp_response_invalid_format", new object[] { responseString }));
			}
			return Convert.ToInt32(array[3], NumberFormatInfo.InvariantInfo);
		}

		// Token: 0x06001110 RID: 4368 RVA: 0x0005D03C File Offset: 0x0005B23C
		private void CreateFtpListenerSocket(FtpWebRequest request)
		{
			IPEndPoint ipendPoint = new IPEndPoint(((IPEndPoint)base.Socket.LocalEndPoint).Address, 0);
			try
			{
				this.m_DataSocket = this.CreateFtpDataSocket(request, base.Socket);
			}
			catch (ObjectDisposedException)
			{
				throw ExceptionHelper.RequestAbortedException;
			}
			new SocketPermission(PermissionState.Unrestricted).Assert();
			try
			{
				this.m_DataSocket.Bind(ipendPoint);
				this.m_DataSocket.Listen(1);
			}
			finally
			{
				CodeAccessPermission.RevertAssert();
			}
		}

		// Token: 0x06001111 RID: 4369 RVA: 0x0005D0C8 File Offset: 0x0005B2C8
		private string GetPortCommandLine(FtpWebRequest request)
		{
			string text;
			try
			{
				IPEndPoint ipendPoint = (IPEndPoint)this.m_DataSocket.LocalEndPoint;
				if (base.ServerAddress.AddressFamily == AddressFamily.InterNetwork)
				{
					text = this.FormatAddress(ipendPoint.Address, ipendPoint.Port);
				}
				else
				{
					if (base.ServerAddress.AddressFamily != AddressFamily.InterNetworkV6)
					{
						throw new InternalException();
					}
					text = this.FormatAddressV6(ipendPoint.Address, ipendPoint.Port);
				}
			}
			catch (Exception ex)
			{
				throw base.GenerateException(WebExceptionStatus.ProtocolError, ex);
			}
			return text;
		}

		// Token: 0x06001112 RID: 4370 RVA: 0x0005D150 File Offset: 0x0005B350
		private string FormatFtpCommand(string command, string parameter)
		{
			if (!ServicePointManager.AllowNewLineInFtpCommand && parameter != null)
			{
				int num = parameter.IndexOf("\r\n");
				if (num != -1)
				{
					parameter = parameter.Substring(0, num);
				}
			}
			StringBuilder stringBuilder = new StringBuilder(command.Length + ((parameter != null) ? parameter.Length : 0) + 3);
			stringBuilder.Append(command);
			if (!ValidationHelper.IsBlankString(parameter))
			{
				stringBuilder.Append(' ');
				stringBuilder.Append(parameter);
			}
			stringBuilder.Append("\r\n");
			return stringBuilder.ToString();
		}

		// Token: 0x06001113 RID: 4371 RVA: 0x0005D1D0 File Offset: 0x0005B3D0
		protected Socket CreateFtpDataSocket(FtpWebRequest request, Socket templateSocket)
		{
			return new Socket(templateSocket.AddressFamily, templateSocket.SocketType, templateSocket.ProtocolType);
		}

		// Token: 0x06001114 RID: 4372 RVA: 0x0005D1F8 File Offset: 0x0005B3F8
		protected override bool CheckValid(ResponseDescription response, ref int validThrough, ref int completeLength)
		{
			if (response.StatusBuffer.Length < 4)
			{
				return true;
			}
			string text = response.StatusBuffer.ToString();
			if (response.Status == -1)
			{
				if (!char.IsDigit(text[0]) || !char.IsDigit(text[1]) || !char.IsDigit(text[2]) || (text[3] != ' ' && text[3] != '-'))
				{
					return false;
				}
				response.StatusCodeString = text.Substring(0, 3);
				response.Status = (int)Convert.ToInt16(response.StatusCodeString, NumberFormatInfo.InvariantInfo);
				if (text[3] == '-')
				{
					response.Multiline = true;
				}
			}
			int num;
			while ((num = text.IndexOf("\r\n", validThrough)) != -1)
			{
				int num2 = validThrough;
				validThrough = num + 2;
				if (!response.Multiline)
				{
					completeLength = validThrough;
					return true;
				}
				if (text.Length > num2 + 4 && text.Substring(num2, 3) == response.StatusCodeString && text[num2 + 3] == ' ')
				{
					completeLength = validThrough;
					return true;
				}
			}
			return true;
		}

		// Token: 0x06001115 RID: 4373 RVA: 0x0005D304 File Offset: 0x0005B504
		private TriState IsFtpDataStreamWriteable()
		{
			FtpWebRequest ftpWebRequest = this.m_Request as FtpWebRequest;
			if (ftpWebRequest != null)
			{
				if (ftpWebRequest.MethodInfo.IsUpload)
				{
					return TriState.True;
				}
				if (ftpWebRequest.MethodInfo.IsDownload)
				{
					return TriState.False;
				}
			}
			return TriState.Unspecified;
		}

		// Token: 0x040013EF RID: 5103
		private Socket m_DataSocket;

		// Token: 0x040013F0 RID: 5104
		private IPEndPoint m_PassiveEndPoint;

		// Token: 0x040013F1 RID: 5105
		private TlsStream m_TlsStream;

		// Token: 0x040013F2 RID: 5106
		private StringBuilder m_BannerMessage;

		// Token: 0x040013F3 RID: 5107
		private StringBuilder m_WelcomeMessage;

		// Token: 0x040013F4 RID: 5108
		private StringBuilder m_ExitMessage;

		// Token: 0x040013F5 RID: 5109
		private WeakReference m_Credentials;

		// Token: 0x040013F6 RID: 5110
		private string m_CurrentTypeSetting = string.Empty;

		// Token: 0x040013F7 RID: 5111
		private long m_ContentLength = -1L;

		// Token: 0x040013F8 RID: 5112
		private DateTime m_LastModified;

		// Token: 0x040013F9 RID: 5113
		private bool m_DataHandshakeStarted;

		// Token: 0x040013FA RID: 5114
		private string m_LoginDirectory;

		// Token: 0x040013FB RID: 5115
		private string m_EstablishedServerDirectory;

		// Token: 0x040013FC RID: 5116
		private string m_RequestedServerDirectory;

		// Token: 0x040013FD RID: 5117
		private Uri m_ResponseUri;

		// Token: 0x040013FE RID: 5118
		private FtpLoginState m_LoginState;

		// Token: 0x040013FF RID: 5119
		internal FtpStatusCode StatusCode;

		// Token: 0x04001400 RID: 5120
		internal string StatusLine;

		// Token: 0x04001401 RID: 5121
		private static readonly AsyncCallback m_AcceptCallbackDelegate = new AsyncCallback(FtpControlStream.AcceptCallback);

		// Token: 0x04001402 RID: 5122
		private static readonly AsyncCallback m_ConnectCallbackDelegate = new AsyncCallback(FtpControlStream.ConnectCallback);

		// Token: 0x04001403 RID: 5123
		private static readonly AsyncCallback m_SSLHandshakeCallback = new AsyncCallback(FtpControlStream.SSLHandshakeCallback);

		// Token: 0x0200074F RID: 1871
		private enum GetPathOption
		{
			// Token: 0x040031F6 RID: 12790
			Normal,
			// Token: 0x040031F7 RID: 12791
			AssumeFilename,
			// Token: 0x040031F8 RID: 12792
			AssumeNoFilename
		}
	}
}
