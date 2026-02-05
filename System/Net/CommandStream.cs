using System;
using System.IO;
using System.Text;

namespace System.Net
{
	// Token: 0x02000198 RID: 408
	internal class CommandStream : PooledStream
	{
		// Token: 0x06000FD5 RID: 4053 RVA: 0x00052DA7 File Offset: 0x00050FA7
		internal CommandStream(ConnectionPool connectionPool, TimeSpan lifetime, bool checkLifetime)
			: base(connectionPool, lifetime, checkLifetime)
		{
			this.m_Decoder = this.m_Encoding.GetDecoder();
		}

		// Token: 0x06000FD6 RID: 4054 RVA: 0x00052DDC File Offset: 0x00050FDC
		internal virtual void Abort(Exception e)
		{
			lock (this)
			{
				if (this.m_Aborted)
				{
					return;
				}
				this.m_Aborted = true;
				base.CanBePooled = false;
			}
			try
			{
				base.Close(0);
			}
			finally
			{
				if (e != null)
				{
					this.InvokeRequestCallback(e);
				}
				else
				{
					this.InvokeRequestCallback(null);
				}
			}
		}

		// Token: 0x06000FD7 RID: 4055 RVA: 0x00052E54 File Offset: 0x00051054
		protected override void Dispose(bool disposing)
		{
			this.InvokeRequestCallback(null);
		}

		// Token: 0x06000FD8 RID: 4056 RVA: 0x00052E60 File Offset: 0x00051060
		protected void InvokeRequestCallback(object obj)
		{
			WebRequest request = this.m_Request;
			if (request != null)
			{
				request.RequestCallback(obj);
			}
		}

		// Token: 0x17000375 RID: 885
		// (get) Token: 0x06000FD9 RID: 4057 RVA: 0x00052E7E File Offset: 0x0005107E
		internal bool RecoverableFailure
		{
			get
			{
				return this.m_RecoverableFailure;
			}
		}

		// Token: 0x06000FDA RID: 4058 RVA: 0x00052E86 File Offset: 0x00051086
		protected void MarkAsRecoverableFailure()
		{
			if (this.m_Index <= 1)
			{
				this.m_RecoverableFailure = true;
			}
		}

		// Token: 0x06000FDB RID: 4059 RVA: 0x00052E98 File Offset: 0x00051098
		internal Stream SubmitRequest(WebRequest request, bool async, bool readInitalResponseOnConnect)
		{
			this.ClearState();
			base.UpdateLifetime();
			CommandStream.PipelineEntry[] array = this.BuildCommandsList(request);
			this.InitCommandPipeline(request, array, async);
			if (readInitalResponseOnConnect && base.JustConnected)
			{
				this.m_DoSend = false;
				this.m_Index = -1;
			}
			return this.ContinueCommandPipeline();
		}

		// Token: 0x06000FDC RID: 4060 RVA: 0x00052EE1 File Offset: 0x000510E1
		protected virtual void ClearState()
		{
			this.InitCommandPipeline(null, null, false);
		}

		// Token: 0x06000FDD RID: 4061 RVA: 0x00052EEC File Offset: 0x000510EC
		protected virtual CommandStream.PipelineEntry[] BuildCommandsList(WebRequest request)
		{
			return null;
		}

		// Token: 0x06000FDE RID: 4062 RVA: 0x00052EEF File Offset: 0x000510EF
		protected Exception GenerateException(WebExceptionStatus status, Exception innerException)
		{
			return new WebException(NetRes.GetWebStatusString("net_connclosed", status), innerException, status, null);
		}

		// Token: 0x06000FDF RID: 4063 RVA: 0x00052F04 File Offset: 0x00051104
		protected Exception GenerateException(FtpStatusCode code, string statusDescription, Exception innerException)
		{
			return new WebException(SR.GetString("net_servererror", new object[] { NetRes.GetWebStatusCodeString(code, statusDescription) }), innerException, WebExceptionStatus.ProtocolError, null);
		}

		// Token: 0x06000FE0 RID: 4064 RVA: 0x00052F28 File Offset: 0x00051128
		protected void InitCommandPipeline(WebRequest request, CommandStream.PipelineEntry[] commands, bool async)
		{
			this.m_Commands = commands;
			this.m_Index = 0;
			this.m_Request = request;
			this.m_Aborted = false;
			this.m_DoRead = true;
			this.m_DoSend = true;
			this.m_CurrentResponseDescription = null;
			this.m_Async = async;
			this.m_RecoverableFailure = false;
			this.m_AbortReason = string.Empty;
		}

		// Token: 0x06000FE1 RID: 4065 RVA: 0x00052F80 File Offset: 0x00051180
		internal void CheckContinuePipeline()
		{
			if (this.m_Async)
			{
				return;
			}
			try
			{
				this.ContinueCommandPipeline();
			}
			catch (Exception ex)
			{
				this.Abort(ex);
			}
		}

		// Token: 0x06000FE2 RID: 4066 RVA: 0x00052FBC File Offset: 0x000511BC
		protected Stream ContinueCommandPipeline()
		{
			bool async = this.m_Async;
			while (this.m_Index < this.m_Commands.Length)
			{
				if (this.m_DoSend)
				{
					if (this.m_Index < 0)
					{
						throw new InternalException();
					}
					byte[] bytes = this.Encoding.GetBytes(this.m_Commands[this.m_Index].Command);
					if (Logging.On)
					{
						string text = this.m_Commands[this.m_Index].Command.Substring(0, this.m_Commands[this.m_Index].Command.Length - 2);
						if (this.m_Commands[this.m_Index].HasFlag(CommandStream.PipelineEntryFlags.DontLogParameter))
						{
							int num = text.IndexOf(' ');
							if (num != -1)
							{
								text = text.Substring(0, num) + " ********";
							}
						}
						Logging.PrintInfo(Logging.Web, this, SR.GetString("net_log_sending_command", new object[] { text }));
					}
					try
					{
						if (async)
						{
							this.BeginWrite(bytes, 0, bytes.Length, CommandStream.m_WriteCallbackDelegate, this);
						}
						else
						{
							this.Write(bytes, 0, bytes.Length);
						}
					}
					catch (IOException)
					{
						this.MarkAsRecoverableFailure();
						throw;
					}
					catch
					{
						throw;
					}
					if (async)
					{
						return null;
					}
				}
				Stream stream = null;
				bool flag = this.PostSendCommandProcessing(ref stream);
				if (flag)
				{
					return stream;
				}
			}
			lock (this)
			{
				this.Close();
			}
			return null;
		}

		// Token: 0x06000FE3 RID: 4067 RVA: 0x0005314C File Offset: 0x0005134C
		private bool PostSendCommandProcessing(ref Stream stream)
		{
			if (this.m_DoRead)
			{
				bool async = this.m_Async;
				int index = this.m_Index;
				CommandStream.PipelineEntry[] commands = this.m_Commands;
				try
				{
					ResponseDescription responseDescription = this.ReceiveCommandResponse();
					if (async)
					{
						return true;
					}
					this.m_CurrentResponseDescription = responseDescription;
				}
				catch
				{
					if (index < 0 || index >= commands.Length || commands[index].Command != "QUIT\r\n")
					{
						throw;
					}
				}
			}
			return this.PostReadCommandProcessing(ref stream);
		}

		// Token: 0x06000FE4 RID: 4068 RVA: 0x000531CC File Offset: 0x000513CC
		private bool PostReadCommandProcessing(ref Stream stream)
		{
			if (this.m_Index >= this.m_Commands.Length)
			{
				return false;
			}
			this.m_DoSend = false;
			this.m_DoRead = false;
			CommandStream.PipelineEntry pipelineEntry;
			if (this.m_Index == -1)
			{
				pipelineEntry = null;
			}
			else
			{
				pipelineEntry = this.m_Commands[this.m_Index];
			}
			CommandStream.PipelineInstruction pipelineInstruction;
			if (this.m_CurrentResponseDescription == null && pipelineEntry.Command == "QUIT\r\n")
			{
				pipelineInstruction = CommandStream.PipelineInstruction.Advance;
			}
			else
			{
				pipelineInstruction = this.PipelineCallback(pipelineEntry, this.m_CurrentResponseDescription, false, ref stream);
			}
			if (pipelineInstruction == CommandStream.PipelineInstruction.Abort)
			{
				Exception ex;
				if (this.m_AbortReason != string.Empty)
				{
					ex = new WebException(this.m_AbortReason);
				}
				else
				{
					ex = this.GenerateException(WebExceptionStatus.ServerProtocolViolation, null);
				}
				this.Abort(ex);
				throw ex;
			}
			if (pipelineInstruction == CommandStream.PipelineInstruction.Advance)
			{
				this.m_CurrentResponseDescription = null;
				this.m_DoSend = true;
				this.m_DoRead = true;
				this.m_Index++;
			}
			else
			{
				if (pipelineInstruction == CommandStream.PipelineInstruction.Pause)
				{
					return true;
				}
				if (pipelineInstruction == CommandStream.PipelineInstruction.GiveStream)
				{
					this.m_CurrentResponseDescription = null;
					this.m_DoRead = true;
					if (this.m_Async)
					{
						this.ContinueCommandPipeline();
						this.InvokeRequestCallback(stream);
					}
					return true;
				}
				if (pipelineInstruction == CommandStream.PipelineInstruction.Reread)
				{
					this.m_CurrentResponseDescription = null;
					this.m_DoRead = true;
				}
			}
			return false;
		}

		// Token: 0x06000FE5 RID: 4069 RVA: 0x000532E5 File Offset: 0x000514E5
		protected virtual CommandStream.PipelineInstruction PipelineCallback(CommandStream.PipelineEntry entry, ResponseDescription response, bool timeout, ref Stream stream)
		{
			return CommandStream.PipelineInstruction.Abort;
		}

		// Token: 0x06000FE6 RID: 4070 RVA: 0x000532E8 File Offset: 0x000514E8
		private static void ReadCallback(IAsyncResult asyncResult)
		{
			ReceiveState receiveState = (ReceiveState)asyncResult.AsyncState;
			try
			{
				Stream connection = receiveState.Connection;
				int num = 0;
				try
				{
					num = connection.EndRead(asyncResult);
					if (num == 0)
					{
						receiveState.Connection.CloseSocket();
					}
				}
				catch (IOException)
				{
					receiveState.Connection.MarkAsRecoverableFailure();
					throw;
				}
				catch
				{
					throw;
				}
				receiveState.Connection.ReceiveCommandResponseCallback(receiveState, num);
			}
			catch (Exception ex)
			{
				receiveState.Connection.Abort(ex);
			}
		}

		// Token: 0x06000FE7 RID: 4071 RVA: 0x0005337C File Offset: 0x0005157C
		private static void WriteCallback(IAsyncResult asyncResult)
		{
			CommandStream commandStream = (CommandStream)asyncResult.AsyncState;
			try
			{
				try
				{
					commandStream.EndWrite(asyncResult);
				}
				catch (IOException)
				{
					commandStream.MarkAsRecoverableFailure();
					throw;
				}
				catch
				{
					throw;
				}
				Stream stream = null;
				if (!commandStream.PostSendCommandProcessing(ref stream))
				{
					commandStream.ContinueCommandPipeline();
				}
			}
			catch (Exception ex)
			{
				commandStream.Abort(ex);
			}
		}

		// Token: 0x17000376 RID: 886
		// (get) Token: 0x06000FE8 RID: 4072 RVA: 0x000533F4 File Offset: 0x000515F4
		// (set) Token: 0x06000FE9 RID: 4073 RVA: 0x000533FC File Offset: 0x000515FC
		protected Encoding Encoding
		{
			get
			{
				return this.m_Encoding;
			}
			set
			{
				this.m_Encoding = value;
				this.m_Decoder = this.m_Encoding.GetDecoder();
			}
		}

		// Token: 0x06000FEA RID: 4074 RVA: 0x00053416 File Offset: 0x00051616
		protected virtual bool CheckValid(ResponseDescription response, ref int validThrough, ref int completeLength)
		{
			return false;
		}

		// Token: 0x06000FEB RID: 4075 RVA: 0x0005341C File Offset: 0x0005161C
		private ResponseDescription ReceiveCommandResponse()
		{
			ReceiveState receiveState = new ReceiveState(this);
			try
			{
				if (this.m_Buffer.Length > 0)
				{
					this.ReceiveCommandResponseCallback(receiveState, -1);
				}
				else
				{
					try
					{
						if (this.m_Async)
						{
							this.BeginRead(receiveState.Buffer, 0, receiveState.Buffer.Length, CommandStream.m_ReadCallbackDelegate, receiveState);
							return null;
						}
						int num = this.Read(receiveState.Buffer, 0, receiveState.Buffer.Length);
						if (num == 0)
						{
							base.CloseSocket();
						}
						this.ReceiveCommandResponseCallback(receiveState, num);
					}
					catch (IOException)
					{
						this.MarkAsRecoverableFailure();
						throw;
					}
					catch
					{
						throw;
					}
				}
			}
			catch (Exception ex)
			{
				if (ex is WebException)
				{
					throw;
				}
				throw this.GenerateException(WebExceptionStatus.ReceiveFailure, ex);
			}
			return receiveState.Resp;
		}

		// Token: 0x06000FEC RID: 4076 RVA: 0x000534EC File Offset: 0x000516EC
		private void ReceiveCommandResponseCallback(ReceiveState state, int bytesRead)
		{
			int num = -1;
			for (;;)
			{
				int validThrough = state.ValidThrough;
				if (this.m_Buffer.Length > 0)
				{
					state.Resp.StatusBuffer.Append(this.m_Buffer);
					this.m_Buffer = string.Empty;
					if (!this.CheckValid(state.Resp, ref validThrough, ref num))
					{
						break;
					}
				}
				else
				{
					if (bytesRead <= 0)
					{
						goto Block_3;
					}
					char[] array = new char[this.m_Decoder.GetCharCount(state.Buffer, 0, bytesRead)];
					int chars = this.m_Decoder.GetChars(state.Buffer, 0, bytesRead, array, 0, false);
					string text = new string(array, 0, chars);
					state.Resp.StatusBuffer.Append(text);
					if (!this.CheckValid(state.Resp, ref validThrough, ref num))
					{
						goto Block_4;
					}
					if (num >= 0)
					{
						int num2 = state.Resp.StatusBuffer.Length - num;
						if (num2 > 0)
						{
							this.m_Buffer = text.Substring(text.Length - num2, num2);
						}
					}
				}
				if (num < 0)
				{
					state.ValidThrough = validThrough;
					try
					{
						if (this.m_Async)
						{
							this.BeginRead(state.Buffer, 0, state.Buffer.Length, CommandStream.m_ReadCallbackDelegate, state);
							return;
						}
						bytesRead = this.Read(state.Buffer, 0, state.Buffer.Length);
						if (bytesRead == 0)
						{
							base.CloseSocket();
						}
						continue;
					}
					catch (IOException)
					{
						this.MarkAsRecoverableFailure();
						throw;
					}
					catch
					{
						throw;
					}
					goto IL_016C;
				}
				goto IL_016C;
			}
			throw this.GenerateException(WebExceptionStatus.ServerProtocolViolation, null);
			Block_3:
			throw this.GenerateException(WebExceptionStatus.ServerProtocolViolation, null);
			Block_4:
			throw this.GenerateException(WebExceptionStatus.ServerProtocolViolation, null);
			IL_016C:
			string text2 = state.Resp.StatusBuffer.ToString();
			state.Resp.StatusDescription = text2.Substring(0, num);
			if (Logging.On)
			{
				Logging.PrintInfo(Logging.Web, this, SR.GetString("net_log_received_response", new object[] { text2.Substring(0, num - 2) }));
			}
			if (this.m_Async)
			{
				if (state.Resp != null)
				{
					this.m_CurrentResponseDescription = state.Resp;
				}
				Stream stream = null;
				if (this.PostReadCommandProcessing(ref stream))
				{
					return;
				}
				this.ContinueCommandPipeline();
			}
		}

		// Token: 0x040012EF RID: 4847
		private static readonly AsyncCallback m_WriteCallbackDelegate = new AsyncCallback(CommandStream.WriteCallback);

		// Token: 0x040012F0 RID: 4848
		private static readonly AsyncCallback m_ReadCallbackDelegate = new AsyncCallback(CommandStream.ReadCallback);

		// Token: 0x040012F1 RID: 4849
		private bool m_RecoverableFailure;

		// Token: 0x040012F2 RID: 4850
		protected WebRequest m_Request;

		// Token: 0x040012F3 RID: 4851
		protected bool m_Async;

		// Token: 0x040012F4 RID: 4852
		private bool m_Aborted;

		// Token: 0x040012F5 RID: 4853
		protected CommandStream.PipelineEntry[] m_Commands;

		// Token: 0x040012F6 RID: 4854
		protected int m_Index;

		// Token: 0x040012F7 RID: 4855
		private bool m_DoRead;

		// Token: 0x040012F8 RID: 4856
		private bool m_DoSend;

		// Token: 0x040012F9 RID: 4857
		private ResponseDescription m_CurrentResponseDescription;

		// Token: 0x040012FA RID: 4858
		protected string m_AbortReason;

		// Token: 0x040012FB RID: 4859
		private const int _WaitingForPipeline = 1;

		// Token: 0x040012FC RID: 4860
		private const int _CompletedPipeline = 2;

		// Token: 0x040012FD RID: 4861
		private string m_Buffer = string.Empty;

		// Token: 0x040012FE RID: 4862
		private Encoding m_Encoding = Encoding.UTF8;

		// Token: 0x040012FF RID: 4863
		private Decoder m_Decoder;

		// Token: 0x02000744 RID: 1860
		internal enum PipelineInstruction
		{
			// Token: 0x040031CF RID: 12751
			Abort,
			// Token: 0x040031D0 RID: 12752
			Advance,
			// Token: 0x040031D1 RID: 12753
			Pause,
			// Token: 0x040031D2 RID: 12754
			Reread,
			// Token: 0x040031D3 RID: 12755
			GiveStream
		}

		// Token: 0x02000745 RID: 1861
		[Flags]
		internal enum PipelineEntryFlags
		{
			// Token: 0x040031D5 RID: 12757
			UserCommand = 1,
			// Token: 0x040031D6 RID: 12758
			GiveDataStream = 2,
			// Token: 0x040031D7 RID: 12759
			CreateDataConnection = 4,
			// Token: 0x040031D8 RID: 12760
			DontLogParameter = 8
		}

		// Token: 0x02000746 RID: 1862
		internal class PipelineEntry
		{
			// Token: 0x060041E0 RID: 16864 RVA: 0x00111E71 File Offset: 0x00110071
			internal PipelineEntry(string command)
			{
				this.Command = command;
			}

			// Token: 0x060041E1 RID: 16865 RVA: 0x00111E80 File Offset: 0x00110080
			internal PipelineEntry(string command, CommandStream.PipelineEntryFlags flags)
			{
				this.Command = command;
				this.Flags = flags;
			}

			// Token: 0x060041E2 RID: 16866 RVA: 0x00111E96 File Offset: 0x00110096
			internal bool HasFlag(CommandStream.PipelineEntryFlags flags)
			{
				return (this.Flags & flags) > (CommandStream.PipelineEntryFlags)0;
			}

			// Token: 0x040031D9 RID: 12761
			internal string Command;

			// Token: 0x040031DA RID: 12762
			internal CommandStream.PipelineEntryFlags Flags;
		}
	}
}
