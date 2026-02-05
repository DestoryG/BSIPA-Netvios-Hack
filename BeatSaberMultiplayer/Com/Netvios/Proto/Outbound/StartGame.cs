using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Com.Netvios.Proto.Outbound
{
	// Token: 0x02000015 RID: 21
	public sealed class StartGame : IMessage<StartGame>, IMessage, IEquatable<StartGame>, IDeepCloneable<StartGame>
	{
		// Token: 0x17000084 RID: 132
		// (get) Token: 0x06000181 RID: 385 RVA: 0x00009646 File Offset: 0x00007846
		[DebuggerNonUserCode]
		public static MessageParser<StartGame> Parser
		{
			get
			{
				return StartGame._parser;
			}
		}

		// Token: 0x17000085 RID: 133
		// (get) Token: 0x06000182 RID: 386 RVA: 0x0000964D File Offset: 0x0000784D
		[DebuggerNonUserCode]
		public static MessageDescriptor Descriptor
		{
			get
			{
				return BeatSaberOutboundMessageReflection.Descriptor.MessageTypes[13];
			}
		}

		// Token: 0x17000086 RID: 134
		// (get) Token: 0x06000183 RID: 387 RVA: 0x00009660 File Offset: 0x00007860
		[DebuggerNonUserCode]
		MessageDescriptor IMessage.Descriptor
		{
			get
			{
				return StartGame.Descriptor;
			}
		}

		// Token: 0x06000184 RID: 388 RVA: 0x00009667 File Offset: 0x00007867
		[DebuggerNonUserCode]
		public StartGame()
		{
		}

		// Token: 0x06000185 RID: 389 RVA: 0x00009685 File Offset: 0x00007885
		[DebuggerNonUserCode]
		public StartGame(StartGame other)
			: this()
		{
			this.roomId_ = other.roomId_;
			this.success_ = other.success_;
			this.message_ = other.message_;
			this._unknownFields = UnknownFieldSet.Clone(other._unknownFields);
		}

		// Token: 0x06000186 RID: 390 RVA: 0x000096C2 File Offset: 0x000078C2
		[DebuggerNonUserCode]
		public StartGame Clone()
		{
			return new StartGame(this);
		}

		// Token: 0x17000087 RID: 135
		// (get) Token: 0x06000187 RID: 391 RVA: 0x000096CA File Offset: 0x000078CA
		// (set) Token: 0x06000188 RID: 392 RVA: 0x000096D2 File Offset: 0x000078D2
		[DebuggerNonUserCode]
		public string RoomId
		{
			get
			{
				return this.roomId_;
			}
			set
			{
				this.roomId_ = ProtoPreconditions.CheckNotNull<string>(value, "value");
			}
		}

		// Token: 0x17000088 RID: 136
		// (get) Token: 0x06000189 RID: 393 RVA: 0x000096E5 File Offset: 0x000078E5
		// (set) Token: 0x0600018A RID: 394 RVA: 0x000096ED File Offset: 0x000078ED
		[DebuggerNonUserCode]
		public bool Success
		{
			get
			{
				return this.success_;
			}
			set
			{
				this.success_ = value;
			}
		}

		// Token: 0x17000089 RID: 137
		// (get) Token: 0x0600018B RID: 395 RVA: 0x000096F6 File Offset: 0x000078F6
		// (set) Token: 0x0600018C RID: 396 RVA: 0x000096FE File Offset: 0x000078FE
		[DebuggerNonUserCode]
		public string Message
		{
			get
			{
				return this.message_;
			}
			set
			{
				this.message_ = ProtoPreconditions.CheckNotNull<string>(value, "value");
			}
		}

		// Token: 0x0600018D RID: 397 RVA: 0x00009711 File Offset: 0x00007911
		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return this.Equals(other as StartGame);
		}

		// Token: 0x0600018E RID: 398 RVA: 0x00009720 File Offset: 0x00007920
		[DebuggerNonUserCode]
		public bool Equals(StartGame other)
		{
			return other != null && (other == this || (!(this.RoomId != other.RoomId) && this.Success == other.Success && !(this.Message != other.Message) && object.Equals(this._unknownFields, other._unknownFields)));
		}

		// Token: 0x0600018F RID: 399 RVA: 0x00009784 File Offset: 0x00007984
		[DebuggerNonUserCode]
		public override int GetHashCode()
		{
			int num = 1;
			if (this.RoomId.Length != 0)
			{
				num ^= this.RoomId.GetHashCode();
			}
			if (this.Success)
			{
				num ^= this.Success.GetHashCode();
			}
			if (this.Message.Length != 0)
			{
				num ^= this.Message.GetHashCode();
			}
			if (this._unknownFields != null)
			{
				num ^= this._unknownFields.GetHashCode();
			}
			return num;
		}

		// Token: 0x06000190 RID: 400 RVA: 0x0000460F File Offset: 0x0000280F
		[DebuggerNonUserCode]
		public override string ToString()
		{
			return JsonFormatter.ToDiagnosticString(this);
		}

		// Token: 0x06000191 RID: 401 RVA: 0x000097FC File Offset: 0x000079FC
		[DebuggerNonUserCode]
		public void WriteTo(CodedOutputStream output)
		{
			if (this.RoomId.Length != 0)
			{
				output.WriteRawTag(10);
				output.WriteString(this.RoomId);
			}
			if (this.Success)
			{
				output.WriteRawTag(16);
				output.WriteBool(this.Success);
			}
			if (this.Message.Length != 0)
			{
				output.WriteRawTag(26);
				output.WriteString(this.Message);
			}
			if (this._unknownFields != null)
			{
				this._unknownFields.WriteTo(output);
			}
		}

		// Token: 0x06000192 RID: 402 RVA: 0x0000987C File Offset: 0x00007A7C
		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (this.RoomId.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.RoomId);
			}
			if (this.Success)
			{
				num += 2;
			}
			if (this.Message.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.Message);
			}
			if (this._unknownFields != null)
			{
				num += this._unknownFields.CalculateSize();
			}
			return num;
		}

		// Token: 0x06000193 RID: 403 RVA: 0x000098E8 File Offset: 0x00007AE8
		[DebuggerNonUserCode]
		public void MergeFrom(StartGame other)
		{
			if (other == null)
			{
				return;
			}
			if (other.RoomId.Length != 0)
			{
				this.RoomId = other.RoomId;
			}
			if (other.Success)
			{
				this.Success = other.Success;
			}
			if (other.Message.Length != 0)
			{
				this.Message = other.Message;
			}
			this._unknownFields = UnknownFieldSet.MergeFrom(this._unknownFields, other._unknownFields);
		}

		// Token: 0x06000194 RID: 404 RVA: 0x00009958 File Offset: 0x00007B58
		[DebuggerNonUserCode]
		public void MergeFrom(CodedInputStream input)
		{
			uint num;
			while ((num = input.ReadTag()) != 0U)
			{
				if (num != 10U)
				{
					if (num != 16U)
					{
						if (num != 26U)
						{
							this._unknownFields = UnknownFieldSet.MergeFieldFrom(this._unknownFields, input);
						}
						else
						{
							this.Message = input.ReadString();
						}
					}
					else
					{
						this.Success = input.ReadBool();
					}
				}
				else
				{
					this.RoomId = input.ReadString();
				}
			}
		}

		// Token: 0x040000E1 RID: 225
		private static readonly MessageParser<StartGame> _parser = new MessageParser<StartGame>(() => new StartGame());

		// Token: 0x040000E2 RID: 226
		private UnknownFieldSet _unknownFields;

		// Token: 0x040000E3 RID: 227
		public const int RoomIdFieldNumber = 1;

		// Token: 0x040000E4 RID: 228
		private string roomId_ = "";

		// Token: 0x040000E5 RID: 229
		public const int SuccessFieldNumber = 2;

		// Token: 0x040000E6 RID: 230
		private bool success_;

		// Token: 0x040000E7 RID: 231
		public const int MessageFieldNumber = 3;

		// Token: 0x040000E8 RID: 232
		private string message_ = "";
	}
}
