using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

namespace Com.Netvios.Proto.Outbound
{
	// Token: 0x02000010 RID: 16
	public sealed class GetRoom : IMessage<GetRoom>, IMessage, IEquatable<GetRoom>, IDeepCloneable<GetRoom>
	{
		// Token: 0x17000059 RID: 89
		// (get) Token: 0x06000101 RID: 257 RVA: 0x00007955 File Offset: 0x00005B55
		[DebuggerNonUserCode]
		public static MessageParser<GetRoom> Parser
		{
			get
			{
				return GetRoom._parser;
			}
		}

		// Token: 0x1700005A RID: 90
		// (get) Token: 0x06000102 RID: 258 RVA: 0x0000795C File Offset: 0x00005B5C
		[DebuggerNonUserCode]
		public static MessageDescriptor Descriptor
		{
			get
			{
				return BeatSaberOutboundMessageReflection.Descriptor.MessageTypes[8];
			}
		}

		// Token: 0x1700005B RID: 91
		// (get) Token: 0x06000103 RID: 259 RVA: 0x0000796E File Offset: 0x00005B6E
		[DebuggerNonUserCode]
		MessageDescriptor IMessage.Descriptor
		{
			get
			{
				return GetRoom.Descriptor;
			}
		}

		// Token: 0x06000104 RID: 260 RVA: 0x00007975 File Offset: 0x00005B75
		[DebuggerNonUserCode]
		public GetRoom()
		{
		}

		// Token: 0x06000105 RID: 261 RVA: 0x000079AC File Offset: 0x00005BAC
		[DebuggerNonUserCode]
		public GetRoom(GetRoom other)
			: this()
		{
			this.roomId_ = other.roomId_;
			this.roomOwner_ = other.roomOwner_;
			this.roomOwnerName_ = other.roomOwnerName_;
			this.roomCfg_ = ((other.roomCfg_ != null) ? other.roomCfg_.Clone() : null);
			this.songCfg_ = ((other.songCfg_ != null) ? other.songCfg_.Clone() : null);
			this.players_ = other.players_.Clone();
			this.status_ = other.status_;
			this._unknownFields = UnknownFieldSet.Clone(other._unknownFields);
		}

		// Token: 0x06000106 RID: 262 RVA: 0x00007A49 File Offset: 0x00005C49
		[DebuggerNonUserCode]
		public GetRoom Clone()
		{
			return new GetRoom(this);
		}

		// Token: 0x1700005C RID: 92
		// (get) Token: 0x06000107 RID: 263 RVA: 0x00007A51 File Offset: 0x00005C51
		// (set) Token: 0x06000108 RID: 264 RVA: 0x00007A59 File Offset: 0x00005C59
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

		// Token: 0x1700005D RID: 93
		// (get) Token: 0x06000109 RID: 265 RVA: 0x00007A6C File Offset: 0x00005C6C
		// (set) Token: 0x0600010A RID: 266 RVA: 0x00007A74 File Offset: 0x00005C74
		[DebuggerNonUserCode]
		public long RoomOwner
		{
			get
			{
				return this.roomOwner_;
			}
			set
			{
				this.roomOwner_ = value;
			}
		}

		// Token: 0x1700005E RID: 94
		// (get) Token: 0x0600010B RID: 267 RVA: 0x00007A7D File Offset: 0x00005C7D
		// (set) Token: 0x0600010C RID: 268 RVA: 0x00007A85 File Offset: 0x00005C85
		[DebuggerNonUserCode]
		public string RoomOwnerName
		{
			get
			{
				return this.roomOwnerName_;
			}
			set
			{
				this.roomOwnerName_ = ProtoPreconditions.CheckNotNull<string>(value, "value");
			}
		}

		// Token: 0x1700005F RID: 95
		// (get) Token: 0x0600010D RID: 269 RVA: 0x00007A98 File Offset: 0x00005C98
		// (set) Token: 0x0600010E RID: 270 RVA: 0x00007AA0 File Offset: 0x00005CA0
		[DebuggerNonUserCode]
		public RoomCfg RoomCfg
		{
			get
			{
				return this.roomCfg_;
			}
			set
			{
				this.roomCfg_ = value;
			}
		}

		// Token: 0x17000060 RID: 96
		// (get) Token: 0x0600010F RID: 271 RVA: 0x00007AA9 File Offset: 0x00005CA9
		// (set) Token: 0x06000110 RID: 272 RVA: 0x00007AB1 File Offset: 0x00005CB1
		[DebuggerNonUserCode]
		public SongCfg SongCfg
		{
			get
			{
				return this.songCfg_;
			}
			set
			{
				this.songCfg_ = value;
			}
		}

		// Token: 0x17000061 RID: 97
		// (get) Token: 0x06000111 RID: 273 RVA: 0x00007ABA File Offset: 0x00005CBA
		[DebuggerNonUserCode]
		public RepeatedField<RoomPlayer> Players
		{
			get
			{
				return this.players_;
			}
		}

		// Token: 0x17000062 RID: 98
		// (get) Token: 0x06000112 RID: 274 RVA: 0x00007AC2 File Offset: 0x00005CC2
		// (set) Token: 0x06000113 RID: 275 RVA: 0x00007ACA File Offset: 0x00005CCA
		[DebuggerNonUserCode]
		public string Status
		{
			get
			{
				return this.status_;
			}
			set
			{
				this.status_ = ProtoPreconditions.CheckNotNull<string>(value, "value");
			}
		}

		// Token: 0x06000114 RID: 276 RVA: 0x00007ADD File Offset: 0x00005CDD
		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return this.Equals(other as GetRoom);
		}

		// Token: 0x06000115 RID: 277 RVA: 0x00007AEC File Offset: 0x00005CEC
		[DebuggerNonUserCode]
		public bool Equals(GetRoom other)
		{
			return other != null && (other == this || (!(this.RoomId != other.RoomId) && this.RoomOwner == other.RoomOwner && !(this.RoomOwnerName != other.RoomOwnerName) && object.Equals(this.RoomCfg, other.RoomCfg) && object.Equals(this.SongCfg, other.SongCfg) && this.players_.Equals(other.players_) && !(this.Status != other.Status) && object.Equals(this._unknownFields, other._unknownFields)));
		}

		// Token: 0x06000116 RID: 278 RVA: 0x00007BA4 File Offset: 0x00005DA4
		[DebuggerNonUserCode]
		public override int GetHashCode()
		{
			int num = 1;
			if (this.RoomId.Length != 0)
			{
				num ^= this.RoomId.GetHashCode();
			}
			if (this.RoomOwner != 0L)
			{
				num ^= this.RoomOwner.GetHashCode();
			}
			if (this.RoomOwnerName.Length != 0)
			{
				num ^= this.RoomOwnerName.GetHashCode();
			}
			if (this.roomCfg_ != null)
			{
				num ^= this.RoomCfg.GetHashCode();
			}
			if (this.songCfg_ != null)
			{
				num ^= this.SongCfg.GetHashCode();
			}
			num ^= this.players_.GetHashCode();
			if (this.Status.Length != 0)
			{
				num ^= this.Status.GetHashCode();
			}
			if (this._unknownFields != null)
			{
				num ^= this._unknownFields.GetHashCode();
			}
			return num;
		}

		// Token: 0x06000117 RID: 279 RVA: 0x0000460F File Offset: 0x0000280F
		[DebuggerNonUserCode]
		public override string ToString()
		{
			return JsonFormatter.ToDiagnosticString(this);
		}

		// Token: 0x06000118 RID: 280 RVA: 0x00007C70 File Offset: 0x00005E70
		[DebuggerNonUserCode]
		public void WriteTo(CodedOutputStream output)
		{
			if (this.RoomId.Length != 0)
			{
				output.WriteRawTag(10);
				output.WriteString(this.RoomId);
			}
			if (this.RoomOwner != 0L)
			{
				output.WriteRawTag(16);
				output.WriteInt64(this.RoomOwner);
			}
			if (this.RoomOwnerName.Length != 0)
			{
				output.WriteRawTag(26);
				output.WriteString(this.RoomOwnerName);
			}
			if (this.roomCfg_ != null)
			{
				output.WriteRawTag(58);
				output.WriteMessage(this.RoomCfg);
			}
			if (this.songCfg_ != null)
			{
				output.WriteRawTag(66);
				output.WriteMessage(this.SongCfg);
			}
			this.players_.WriteTo(output, GetRoom._repeated_players_codec);
			if (this.Status.Length != 0)
			{
				output.WriteRawTag(98);
				output.WriteString(this.Status);
			}
			if (this._unknownFields != null)
			{
				this._unknownFields.WriteTo(output);
			}
		}

		// Token: 0x06000119 RID: 281 RVA: 0x00007D5C File Offset: 0x00005F5C
		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (this.RoomId.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.RoomId);
			}
			if (this.RoomOwner != 0L)
			{
				num += 1 + CodedOutputStream.ComputeInt64Size(this.RoomOwner);
			}
			if (this.RoomOwnerName.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.RoomOwnerName);
			}
			if (this.roomCfg_ != null)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(this.RoomCfg);
			}
			if (this.songCfg_ != null)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(this.SongCfg);
			}
			num += this.players_.CalculateSize(GetRoom._repeated_players_codec);
			if (this.Status.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.Status);
			}
			if (this._unknownFields != null)
			{
				num += this._unknownFields.CalculateSize();
			}
			return num;
		}

		// Token: 0x0600011A RID: 282 RVA: 0x00007E34 File Offset: 0x00006034
		[DebuggerNonUserCode]
		public void MergeFrom(GetRoom other)
		{
			if (other == null)
			{
				return;
			}
			if (other.RoomId.Length != 0)
			{
				this.RoomId = other.RoomId;
			}
			if (other.RoomOwner != 0L)
			{
				this.RoomOwner = other.RoomOwner;
			}
			if (other.RoomOwnerName.Length != 0)
			{
				this.RoomOwnerName = other.RoomOwnerName;
			}
			if (other.roomCfg_ != null)
			{
				if (this.roomCfg_ == null)
				{
					this.RoomCfg = new RoomCfg();
				}
				this.RoomCfg.MergeFrom(other.RoomCfg);
			}
			if (other.songCfg_ != null)
			{
				if (this.songCfg_ == null)
				{
					this.SongCfg = new SongCfg();
				}
				this.SongCfg.MergeFrom(other.SongCfg);
			}
			this.players_.Add(other.players_);
			if (other.Status.Length != 0)
			{
				this.Status = other.Status;
			}
			this._unknownFields = UnknownFieldSet.MergeFrom(this._unknownFields, other._unknownFields);
		}

		// Token: 0x0600011B RID: 283 RVA: 0x00007F24 File Offset: 0x00006124
		[DebuggerNonUserCode]
		public void MergeFrom(CodedInputStream input)
		{
			uint num;
			while ((num = input.ReadTag()) != 0U)
			{
				if (num <= 26U)
				{
					if (num == 10U)
					{
						this.RoomId = input.ReadString();
						continue;
					}
					if (num == 16U)
					{
						this.RoomOwner = input.ReadInt64();
						continue;
					}
					if (num == 26U)
					{
						this.RoomOwnerName = input.ReadString();
						continue;
					}
				}
				else if (num <= 66U)
				{
					if (num == 58U)
					{
						if (this.roomCfg_ == null)
						{
							this.RoomCfg = new RoomCfg();
						}
						input.ReadMessage(this.RoomCfg);
						continue;
					}
					if (num == 66U)
					{
						if (this.songCfg_ == null)
						{
							this.SongCfg = new SongCfg();
						}
						input.ReadMessage(this.SongCfg);
						continue;
					}
				}
				else
				{
					if (num == 82U)
					{
						this.players_.AddEntriesFrom(input, GetRoom._repeated_players_codec);
						continue;
					}
					if (num == 98U)
					{
						this.Status = input.ReadString();
						continue;
					}
				}
				this._unknownFields = UnknownFieldSet.MergeFieldFrom(this._unknownFields, input);
			}
		}

		// Token: 0x0400009C RID: 156
		private static readonly MessageParser<GetRoom> _parser = new MessageParser<GetRoom>(() => new GetRoom());

		// Token: 0x0400009D RID: 157
		private UnknownFieldSet _unknownFields;

		// Token: 0x0400009E RID: 158
		public const int RoomIdFieldNumber = 1;

		// Token: 0x0400009F RID: 159
		private string roomId_ = "";

		// Token: 0x040000A0 RID: 160
		public const int RoomOwnerFieldNumber = 2;

		// Token: 0x040000A1 RID: 161
		private long roomOwner_;

		// Token: 0x040000A2 RID: 162
		public const int RoomOwnerNameFieldNumber = 3;

		// Token: 0x040000A3 RID: 163
		private string roomOwnerName_ = "";

		// Token: 0x040000A4 RID: 164
		public const int RoomCfgFieldNumber = 7;

		// Token: 0x040000A5 RID: 165
		private RoomCfg roomCfg_;

		// Token: 0x040000A6 RID: 166
		public const int SongCfgFieldNumber = 8;

		// Token: 0x040000A7 RID: 167
		private SongCfg songCfg_;

		// Token: 0x040000A8 RID: 168
		public const int PlayersFieldNumber = 10;

		// Token: 0x040000A9 RID: 169
		private static readonly FieldCodec<RoomPlayer> _repeated_players_codec = FieldCodec.ForMessage<RoomPlayer>(82U, RoomPlayer.Parser);

		// Token: 0x040000AA RID: 170
		private readonly RepeatedField<RoomPlayer> players_ = new RepeatedField<RoomPlayer>();

		// Token: 0x040000AB RID: 171
		public const int StatusFieldNumber = 12;

		// Token: 0x040000AC RID: 172
		private string status_ = "";
	}
}
