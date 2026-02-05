using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Com.Netvios.Proto.Outbound
{
	// Token: 0x02000021 RID: 33
	public sealed class KickedOutRoomNotice : IMessage<KickedOutRoomNotice>, IMessage, IEquatable<KickedOutRoomNotice>, IDeepCloneable<KickedOutRoomNotice>
	{
		// Token: 0x170000D7 RID: 215
		// (get) Token: 0x06000290 RID: 656 RVA: 0x0000C9C2 File Offset: 0x0000ABC2
		[DebuggerNonUserCode]
		public static MessageParser<KickedOutRoomNotice> Parser
		{
			get
			{
				return KickedOutRoomNotice._parser;
			}
		}

		// Token: 0x170000D8 RID: 216
		// (get) Token: 0x06000291 RID: 657 RVA: 0x0000C9C9 File Offset: 0x0000ABC9
		[DebuggerNonUserCode]
		public static MessageDescriptor Descriptor
		{
			get
			{
				return BeatSaberOutboundMessageReflection.Descriptor.MessageTypes[25];
			}
		}

		// Token: 0x170000D9 RID: 217
		// (get) Token: 0x06000292 RID: 658 RVA: 0x0000C9DC File Offset: 0x0000ABDC
		[DebuggerNonUserCode]
		MessageDescriptor IMessage.Descriptor
		{
			get
			{
				return KickedOutRoomNotice.Descriptor;
			}
		}

		// Token: 0x06000293 RID: 659 RVA: 0x0000C9E3 File Offset: 0x0000ABE3
		[DebuggerNonUserCode]
		public KickedOutRoomNotice()
		{
		}

		// Token: 0x06000294 RID: 660 RVA: 0x0000CA01 File Offset: 0x0000AC01
		[DebuggerNonUserCode]
		public KickedOutRoomNotice(KickedOutRoomNotice other)
			: this()
		{
			this.roomId_ = other.roomId_;
			this.targetPlayerId_ = other.targetPlayerId_;
			this.message_ = other.message_;
			this._unknownFields = UnknownFieldSet.Clone(other._unknownFields);
		}

		// Token: 0x06000295 RID: 661 RVA: 0x0000CA3E File Offset: 0x0000AC3E
		[DebuggerNonUserCode]
		public KickedOutRoomNotice Clone()
		{
			return new KickedOutRoomNotice(this);
		}

		// Token: 0x170000DA RID: 218
		// (get) Token: 0x06000296 RID: 662 RVA: 0x0000CA46 File Offset: 0x0000AC46
		// (set) Token: 0x06000297 RID: 663 RVA: 0x0000CA4E File Offset: 0x0000AC4E
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

		// Token: 0x170000DB RID: 219
		// (get) Token: 0x06000298 RID: 664 RVA: 0x0000CA61 File Offset: 0x0000AC61
		// (set) Token: 0x06000299 RID: 665 RVA: 0x0000CA69 File Offset: 0x0000AC69
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

		// Token: 0x170000DC RID: 220
		// (get) Token: 0x0600029A RID: 666 RVA: 0x0000CA72 File Offset: 0x0000AC72
		// (set) Token: 0x0600029B RID: 667 RVA: 0x0000CA7A File Offset: 0x0000AC7A
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

		// Token: 0x0600029C RID: 668 RVA: 0x0000CA8D File Offset: 0x0000AC8D
		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return this.Equals(other as KickedOutRoomNotice);
		}

		// Token: 0x0600029D RID: 669 RVA: 0x0000CA9C File Offset: 0x0000AC9C
		[DebuggerNonUserCode]
		public bool Equals(KickedOutRoomNotice other)
		{
			return other != null && (other == this || (!(this.RoomId != other.RoomId) && this.TargetPlayerId == other.TargetPlayerId && !(this.Message != other.Message) && object.Equals(this._unknownFields, other._unknownFields)));
		}

		// Token: 0x0600029E RID: 670 RVA: 0x0000CB00 File Offset: 0x0000AD00
		[DebuggerNonUserCode]
		public override int GetHashCode()
		{
			int num = 1;
			if (this.RoomId.Length != 0)
			{
				num ^= this.RoomId.GetHashCode();
			}
			if (this.TargetPlayerId != 0L)
			{
				num ^= this.TargetPlayerId.GetHashCode();
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

		// Token: 0x0600029F RID: 671 RVA: 0x0000460F File Offset: 0x0000280F
		[DebuggerNonUserCode]
		public override string ToString()
		{
			return JsonFormatter.ToDiagnosticString(this);
		}

		// Token: 0x060002A0 RID: 672 RVA: 0x0000CB78 File Offset: 0x0000AD78
		[DebuggerNonUserCode]
		public void WriteTo(CodedOutputStream output)
		{
			if (this.RoomId.Length != 0)
			{
				output.WriteRawTag(10);
				output.WriteString(this.RoomId);
			}
			if (this.TargetPlayerId != 0L)
			{
				output.WriteRawTag(16);
				output.WriteInt64(this.TargetPlayerId);
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

		// Token: 0x060002A1 RID: 673 RVA: 0x0000CBF8 File Offset: 0x0000ADF8
		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (this.RoomId.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.RoomId);
			}
			if (this.TargetPlayerId != 0L)
			{
				num += 1 + CodedOutputStream.ComputeInt64Size(this.TargetPlayerId);
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

		// Token: 0x060002A2 RID: 674 RVA: 0x0000CC70 File Offset: 0x0000AE70
		[DebuggerNonUserCode]
		public void MergeFrom(KickedOutRoomNotice other)
		{
			if (other == null)
			{
				return;
			}
			if (other.RoomId.Length != 0)
			{
				this.RoomId = other.RoomId;
			}
			if (other.TargetPlayerId != 0L)
			{
				this.TargetPlayerId = other.TargetPlayerId;
			}
			if (other.Message.Length != 0)
			{
				this.Message = other.Message;
			}
			this._unknownFields = UnknownFieldSet.MergeFrom(this._unknownFields, other._unknownFields);
		}

		// Token: 0x060002A3 RID: 675 RVA: 0x0000CCE0 File Offset: 0x0000AEE0
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
						this.TargetPlayerId = input.ReadInt64();
					}
				}
				else
				{
					this.RoomId = input.ReadString();
				}
			}
		}

		// Token: 0x0400015A RID: 346
		private static readonly MessageParser<KickedOutRoomNotice> _parser = new MessageParser<KickedOutRoomNotice>(() => new KickedOutRoomNotice());

		// Token: 0x0400015B RID: 347
		private UnknownFieldSet _unknownFields;

		// Token: 0x0400015C RID: 348
		public const int RoomIdFieldNumber = 1;

		// Token: 0x0400015D RID: 349
		private string roomId_ = "";

		// Token: 0x0400015E RID: 350
		public const int TargetPlayerIdFieldNumber = 2;

		// Token: 0x0400015F RID: 351
		private long targetPlayerId_;

		// Token: 0x04000160 RID: 352
		public const int MessageFieldNumber = 3;

		// Token: 0x04000161 RID: 353
		private string message_ = "";
	}
}
