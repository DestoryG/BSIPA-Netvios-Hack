using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Com.Netvios.Proto.Outbound
{
	// Token: 0x02000014 RID: 20
	public sealed class KickOutRoomPlayer : IMessage<KickOutRoomPlayer>, IMessage, IEquatable<KickOutRoomPlayer>, IDeepCloneable<KickOutRoomPlayer>
	{
		// Token: 0x1700007D RID: 125
		// (get) Token: 0x0600016A RID: 362 RVA: 0x000091FE File Offset: 0x000073FE
		[DebuggerNonUserCode]
		public static MessageParser<KickOutRoomPlayer> Parser
		{
			get
			{
				return KickOutRoomPlayer._parser;
			}
		}

		// Token: 0x1700007E RID: 126
		// (get) Token: 0x0600016B RID: 363 RVA: 0x00009205 File Offset: 0x00007405
		[DebuggerNonUserCode]
		public static MessageDescriptor Descriptor
		{
			get
			{
				return BeatSaberOutboundMessageReflection.Descriptor.MessageTypes[12];
			}
		}

		// Token: 0x1700007F RID: 127
		// (get) Token: 0x0600016C RID: 364 RVA: 0x00009218 File Offset: 0x00007418
		[DebuggerNonUserCode]
		MessageDescriptor IMessage.Descriptor
		{
			get
			{
				return KickOutRoomPlayer.Descriptor;
			}
		}

		// Token: 0x0600016D RID: 365 RVA: 0x0000921F File Offset: 0x0000741F
		[DebuggerNonUserCode]
		public KickOutRoomPlayer()
		{
		}

		// Token: 0x0600016E RID: 366 RVA: 0x00009240 File Offset: 0x00007440
		[DebuggerNonUserCode]
		public KickOutRoomPlayer(KickOutRoomPlayer other)
			: this()
		{
			this.roomId_ = other.roomId_;
			this.success_ = other.success_;
			this.message_ = other.message_;
			this.targetPlayerId_ = other.targetPlayerId_;
			this._unknownFields = UnknownFieldSet.Clone(other._unknownFields);
		}

		// Token: 0x0600016F RID: 367 RVA: 0x00009294 File Offset: 0x00007494
		[DebuggerNonUserCode]
		public KickOutRoomPlayer Clone()
		{
			return new KickOutRoomPlayer(this);
		}

		// Token: 0x17000080 RID: 128
		// (get) Token: 0x06000170 RID: 368 RVA: 0x0000929C File Offset: 0x0000749C
		// (set) Token: 0x06000171 RID: 369 RVA: 0x000092A4 File Offset: 0x000074A4
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

		// Token: 0x17000081 RID: 129
		// (get) Token: 0x06000172 RID: 370 RVA: 0x000092B7 File Offset: 0x000074B7
		// (set) Token: 0x06000173 RID: 371 RVA: 0x000092BF File Offset: 0x000074BF
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

		// Token: 0x17000082 RID: 130
		// (get) Token: 0x06000174 RID: 372 RVA: 0x000092C8 File Offset: 0x000074C8
		// (set) Token: 0x06000175 RID: 373 RVA: 0x000092D0 File Offset: 0x000074D0
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

		// Token: 0x17000083 RID: 131
		// (get) Token: 0x06000176 RID: 374 RVA: 0x000092E3 File Offset: 0x000074E3
		// (set) Token: 0x06000177 RID: 375 RVA: 0x000092EB File Offset: 0x000074EB
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

		// Token: 0x06000178 RID: 376 RVA: 0x000092F4 File Offset: 0x000074F4
		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return this.Equals(other as KickOutRoomPlayer);
		}

		// Token: 0x06000179 RID: 377 RVA: 0x00009304 File Offset: 0x00007504
		[DebuggerNonUserCode]
		public bool Equals(KickOutRoomPlayer other)
		{
			return other != null && (other == this || (!(this.RoomId != other.RoomId) && this.Success == other.Success && !(this.Message != other.Message) && this.TargetPlayerId == other.TargetPlayerId && object.Equals(this._unknownFields, other._unknownFields)));
		}

		// Token: 0x0600017A RID: 378 RVA: 0x00009378 File Offset: 0x00007578
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

		// Token: 0x0600017B RID: 379 RVA: 0x0000460F File Offset: 0x0000280F
		[DebuggerNonUserCode]
		public override string ToString()
		{
			return JsonFormatter.ToDiagnosticString(this);
		}

		// Token: 0x0600017C RID: 380 RVA: 0x00009408 File Offset: 0x00007608
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

		// Token: 0x0600017D RID: 381 RVA: 0x000094A4 File Offset: 0x000076A4
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

		// Token: 0x0600017E RID: 382 RVA: 0x00009528 File Offset: 0x00007728
		[DebuggerNonUserCode]
		public void MergeFrom(KickOutRoomPlayer other)
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

		// Token: 0x0600017F RID: 383 RVA: 0x000095AC File Offset: 0x000077AC
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

		// Token: 0x040000D7 RID: 215
		private static readonly MessageParser<KickOutRoomPlayer> _parser = new MessageParser<KickOutRoomPlayer>(() => new KickOutRoomPlayer());

		// Token: 0x040000D8 RID: 216
		private UnknownFieldSet _unknownFields;

		// Token: 0x040000D9 RID: 217
		public const int RoomIdFieldNumber = 1;

		// Token: 0x040000DA RID: 218
		private string roomId_ = "";

		// Token: 0x040000DB RID: 219
		public const int SuccessFieldNumber = 2;

		// Token: 0x040000DC RID: 220
		private bool success_;

		// Token: 0x040000DD RID: 221
		public const int MessageFieldNumber = 3;

		// Token: 0x040000DE RID: 222
		private string message_ = "";

		// Token: 0x040000DF RID: 223
		public const int TargetPlayerIdFieldNumber = 4;

		// Token: 0x040000E0 RID: 224
		private long targetPlayerId_;
	}
}
