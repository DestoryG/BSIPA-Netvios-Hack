using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Com.Netvios.Proto.Outbound
{
	// Token: 0x02000016 RID: 22
	public sealed class ChangeRoomOwner : IMessage<ChangeRoomOwner>, IMessage, IEquatable<ChangeRoomOwner>, IDeepCloneable<ChangeRoomOwner>
	{
		// Token: 0x1700008A RID: 138
		// (get) Token: 0x06000196 RID: 406 RVA: 0x000099D8 File Offset: 0x00007BD8
		[DebuggerNonUserCode]
		public static MessageParser<ChangeRoomOwner> Parser
		{
			get
			{
				return ChangeRoomOwner._parser;
			}
		}

		// Token: 0x1700008B RID: 139
		// (get) Token: 0x06000197 RID: 407 RVA: 0x000099DF File Offset: 0x00007BDF
		[DebuggerNonUserCode]
		public static MessageDescriptor Descriptor
		{
			get
			{
				return BeatSaberOutboundMessageReflection.Descriptor.MessageTypes[14];
			}
		}

		// Token: 0x1700008C RID: 140
		// (get) Token: 0x06000198 RID: 408 RVA: 0x000099F2 File Offset: 0x00007BF2
		[DebuggerNonUserCode]
		MessageDescriptor IMessage.Descriptor
		{
			get
			{
				return ChangeRoomOwner.Descriptor;
			}
		}

		// Token: 0x06000199 RID: 409 RVA: 0x000099F9 File Offset: 0x00007BF9
		[DebuggerNonUserCode]
		public ChangeRoomOwner()
		{
		}

		// Token: 0x0600019A RID: 410 RVA: 0x00009A18 File Offset: 0x00007C18
		[DebuggerNonUserCode]
		public ChangeRoomOwner(ChangeRoomOwner other)
			: this()
		{
			this.roomId_ = other.roomId_;
			this.success_ = other.success_;
			this.message_ = other.message_;
			this.targetPlayerId_ = other.targetPlayerId_;
			this._unknownFields = UnknownFieldSet.Clone(other._unknownFields);
		}

		// Token: 0x0600019B RID: 411 RVA: 0x00009A6C File Offset: 0x00007C6C
		[DebuggerNonUserCode]
		public ChangeRoomOwner Clone()
		{
			return new ChangeRoomOwner(this);
		}

		// Token: 0x1700008D RID: 141
		// (get) Token: 0x0600019C RID: 412 RVA: 0x00009A74 File Offset: 0x00007C74
		// (set) Token: 0x0600019D RID: 413 RVA: 0x00009A7C File Offset: 0x00007C7C
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

		// Token: 0x1700008E RID: 142
		// (get) Token: 0x0600019E RID: 414 RVA: 0x00009A8F File Offset: 0x00007C8F
		// (set) Token: 0x0600019F RID: 415 RVA: 0x00009A97 File Offset: 0x00007C97
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

		// Token: 0x1700008F RID: 143
		// (get) Token: 0x060001A0 RID: 416 RVA: 0x00009AA0 File Offset: 0x00007CA0
		// (set) Token: 0x060001A1 RID: 417 RVA: 0x00009AA8 File Offset: 0x00007CA8
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

		// Token: 0x17000090 RID: 144
		// (get) Token: 0x060001A2 RID: 418 RVA: 0x00009ABB File Offset: 0x00007CBB
		// (set) Token: 0x060001A3 RID: 419 RVA: 0x00009AC3 File Offset: 0x00007CC3
		[DebuggerNonUserCode]
		public long TargetPlayerId
		{
			get
			{
				return this.targetPlayerId_;
			}
			set
			{
				this.targetPlayerId_ = value;
			}
		}

		// Token: 0x060001A4 RID: 420 RVA: 0x00009ACC File Offset: 0x00007CCC
		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return this.Equals(other as ChangeRoomOwner);
		}

		// Token: 0x060001A5 RID: 421 RVA: 0x00009ADC File Offset: 0x00007CDC
		[DebuggerNonUserCode]
		public bool Equals(ChangeRoomOwner other)
		{
			return other != null && (other == this || (!(this.RoomId != other.RoomId) && this.Success == other.Success && !(this.Message != other.Message) && this.TargetPlayerId == other.TargetPlayerId && object.Equals(this._unknownFields, other._unknownFields)));
		}

		// Token: 0x060001A6 RID: 422 RVA: 0x00009B50 File Offset: 0x00007D50
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
			if (this.TargetPlayerId != 0L)
			{
				num ^= this.TargetPlayerId.GetHashCode();
			}
			if (this._unknownFields != null)
			{
				num ^= this._unknownFields.GetHashCode();
			}
			return num;
		}

		// Token: 0x060001A7 RID: 423 RVA: 0x0000460F File Offset: 0x0000280F
		[DebuggerNonUserCode]
		public override string ToString()
		{
			return JsonFormatter.ToDiagnosticString(this);
		}

		// Token: 0x060001A8 RID: 424 RVA: 0x00009BE0 File Offset: 0x00007DE0
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
			if (this.TargetPlayerId != 0L)
			{
				output.WriteRawTag(32);
				output.WriteInt64(this.TargetPlayerId);
			}
			if (this._unknownFields != null)
			{
				this._unknownFields.WriteTo(output);
			}
		}

		// Token: 0x060001A9 RID: 425 RVA: 0x00009C7C File Offset: 0x00007E7C
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
			if (this.TargetPlayerId != 0L)
			{
				num += 1 + CodedOutputStream.ComputeInt64Size(this.TargetPlayerId);
			}
			if (this._unknownFields != null)
			{
				num += this._unknownFields.CalculateSize();
			}
			return num;
		}

		// Token: 0x060001AA RID: 426 RVA: 0x00009D00 File Offset: 0x00007F00
		[DebuggerNonUserCode]
		public void MergeFrom(ChangeRoomOwner other)
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
			if (other.TargetPlayerId != 0L)
			{
				this.TargetPlayerId = other.TargetPlayerId;
			}
			this._unknownFields = UnknownFieldSet.MergeFrom(this._unknownFields, other._unknownFields);
		}

		// Token: 0x060001AB RID: 427 RVA: 0x00009D84 File Offset: 0x00007F84
		[DebuggerNonUserCode]
		public void MergeFrom(CodedInputStream input)
		{
			uint num;
			while ((num = input.ReadTag()) != 0U)
			{
				if (num <= 16U)
				{
					if (num == 10U)
					{
						this.RoomId = input.ReadString();
						continue;
					}
					if (num == 16U)
					{
						this.Success = input.ReadBool();
						continue;
					}
				}
				else
				{
					if (num == 26U)
					{
						this.Message = input.ReadString();
						continue;
					}
					if (num == 32U)
					{
						this.TargetPlayerId = input.ReadInt64();
						continue;
					}
				}
				this._unknownFields = UnknownFieldSet.MergeFieldFrom(this._unknownFields, input);
			}
		}

		// Token: 0x040000E9 RID: 233
		private static readonly MessageParser<ChangeRoomOwner> _parser = new MessageParser<ChangeRoomOwner>(() => new ChangeRoomOwner());

		// Token: 0x040000EA RID: 234
		private UnknownFieldSet _unknownFields;

		// Token: 0x040000EB RID: 235
		public const int RoomIdFieldNumber = 1;

		// Token: 0x040000EC RID: 236
		private string roomId_ = "";

		// Token: 0x040000ED RID: 237
		public const int SuccessFieldNumber = 2;

		// Token: 0x040000EE RID: 238
		private bool success_;

		// Token: 0x040000EF RID: 239
		public const int MessageFieldNumber = 3;

		// Token: 0x040000F0 RID: 240
		private string message_ = "";

		// Token: 0x040000F1 RID: 241
		public const int TargetPlayerIdFieldNumber = 4;

		// Token: 0x040000F2 RID: 242
		private long targetPlayerId_;
	}
}
