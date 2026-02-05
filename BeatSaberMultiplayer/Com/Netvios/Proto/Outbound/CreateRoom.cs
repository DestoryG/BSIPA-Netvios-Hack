using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

namespace Com.Netvios.Proto.Outbound
{
	// Token: 0x02000011 RID: 17
	public sealed class CreateRoom : IMessage<CreateRoom>, IMessage, IEquatable<CreateRoom>, IDeepCloneable<CreateRoom>
	{
		// Token: 0x17000063 RID: 99
		// (get) Token: 0x0600011D RID: 285 RVA: 0x00008049 File Offset: 0x00006249
		[DebuggerNonUserCode]
		public static MessageParser<CreateRoom> Parser
		{
			get
			{
				return CreateRoom._parser;
			}
		}

		// Token: 0x17000064 RID: 100
		// (get) Token: 0x0600011E RID: 286 RVA: 0x00008050 File Offset: 0x00006250
		[DebuggerNonUserCode]
		public static MessageDescriptor Descriptor
		{
			get
			{
				return BeatSaberOutboundMessageReflection.Descriptor.MessageTypes[9];
			}
		}

		// Token: 0x17000065 RID: 101
		// (get) Token: 0x0600011F RID: 287 RVA: 0x00008063 File Offset: 0x00006263
		[DebuggerNonUserCode]
		MessageDescriptor IMessage.Descriptor
		{
			get
			{
				return CreateRoom.Descriptor;
			}
		}

		// Token: 0x06000120 RID: 288 RVA: 0x0000806A File Offset: 0x0000626A
		[DebuggerNonUserCode]
		public CreateRoom()
		{
		}

		// Token: 0x06000121 RID: 289 RVA: 0x000080AC File Offset: 0x000062AC
		[DebuggerNonUserCode]
		public CreateRoom(CreateRoom other)
			: this()
		{
			this.roomId_ = other.roomId_;
			this.roomName_ = other.roomName_;
			this.roomOwner_ = other.roomOwner_;
			this.roomOwnerName_ = other.roomOwnerName_;
			this.roomCfg_ = ((other.roomCfg_ != null) ? other.roomCfg_.Clone() : null);
			this.songCfg_ = ((other.songCfg_ != null) ? other.songCfg_.Clone() : null);
			this.players_ = other.players_.Clone();
			this.status_ = other.status_;
			this._unknownFields = UnknownFieldSet.Clone(other._unknownFields);
		}

		// Token: 0x06000122 RID: 290 RVA: 0x00008155 File Offset: 0x00006355
		[DebuggerNonUserCode]
		public CreateRoom Clone()
		{
			return new CreateRoom(this);
		}

		// Token: 0x17000066 RID: 102
		// (get) Token: 0x06000123 RID: 291 RVA: 0x0000815D File Offset: 0x0000635D
		// (set) Token: 0x06000124 RID: 292 RVA: 0x00008165 File Offset: 0x00006365
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

		// Token: 0x17000067 RID: 103
		// (get) Token: 0x06000125 RID: 293 RVA: 0x00008178 File Offset: 0x00006378
		// (set) Token: 0x06000126 RID: 294 RVA: 0x00008180 File Offset: 0x00006380
		[DebuggerNonUserCode]
		public string RoomName
		{
			get
			{
				return this.roomName_;
			}
			set
			{
				this.roomName_ = ProtoPreconditions.CheckNotNull<string>(value, "value");
			}
		}

		// Token: 0x17000068 RID: 104
		// (get) Token: 0x06000127 RID: 295 RVA: 0x00008193 File Offset: 0x00006393
		// (set) Token: 0x06000128 RID: 296 RVA: 0x0000819B File Offset: 0x0000639B
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

		// Token: 0x17000069 RID: 105
		// (get) Token: 0x06000129 RID: 297 RVA: 0x000081A4 File Offset: 0x000063A4
		// (set) Token: 0x0600012A RID: 298 RVA: 0x000081AC File Offset: 0x000063AC
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

		// Token: 0x1700006A RID: 106
		// (get) Token: 0x0600012B RID: 299 RVA: 0x000081BF File Offset: 0x000063BF
		// (set) Token: 0x0600012C RID: 300 RVA: 0x000081C7 File Offset: 0x000063C7
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

		// Token: 0x1700006B RID: 107
		// (get) Token: 0x0600012D RID: 301 RVA: 0x000081D0 File Offset: 0x000063D0
		// (set) Token: 0x0600012E RID: 302 RVA: 0x000081D8 File Offset: 0x000063D8
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

		// Token: 0x1700006C RID: 108
		// (get) Token: 0x0600012F RID: 303 RVA: 0x000081E1 File Offset: 0x000063E1
		[DebuggerNonUserCode]
		public RepeatedField<RoomPlayer> Players
		{
			get
			{
				return this.players_;
			}
		}

		// Token: 0x1700006D RID: 109
		// (get) Token: 0x06000130 RID: 304 RVA: 0x000081E9 File Offset: 0x000063E9
		// (set) Token: 0x06000131 RID: 305 RVA: 0x000081F1 File Offset: 0x000063F1
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

		// Token: 0x06000132 RID: 306 RVA: 0x00008204 File Offset: 0x00006404
		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return this.Equals(other as CreateRoom);
		}

		// Token: 0x06000133 RID: 307 RVA: 0x00008214 File Offset: 0x00006414
		[DebuggerNonUserCode]
		public bool Equals(CreateRoom other)
		{
			return other != null && (other == this || (!(this.RoomId != other.RoomId) && !(this.RoomName != other.RoomName) && this.RoomOwner == other.RoomOwner && !(this.RoomOwnerName != other.RoomOwnerName) && object.Equals(this.RoomCfg, other.RoomCfg) && object.Equals(this.SongCfg, other.SongCfg) && this.players_.Equals(other.players_) && !(this.Status != other.Status) && object.Equals(this._unknownFields, other._unknownFields)));
		}

		// Token: 0x06000134 RID: 308 RVA: 0x000082E0 File Offset: 0x000064E0
		[DebuggerNonUserCode]
		public override int GetHashCode()
		{
			int num = 1;
			if (this.RoomId.Length != 0)
			{
				num ^= this.RoomId.GetHashCode();
			}
			if (this.RoomName.Length != 0)
			{
				num ^= this.RoomName.GetHashCode();
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

		// Token: 0x06000135 RID: 309 RVA: 0x0000460F File Offset: 0x0000280F
		[DebuggerNonUserCode]
		public override string ToString()
		{
			return JsonFormatter.ToDiagnosticString(this);
		}

		// Token: 0x06000136 RID: 310 RVA: 0x000083C8 File Offset: 0x000065C8
		[DebuggerNonUserCode]
		public void WriteTo(CodedOutputStream output)
		{
			if (this.RoomId.Length != 0)
			{
				output.WriteRawTag(10);
				output.WriteString(this.RoomId);
			}
			if (this.RoomName.Length != 0)
			{
				output.WriteRawTag(18);
				output.WriteString(this.RoomName);
			}
			if (this.RoomOwner != 0L)
			{
				output.WriteRawTag(24);
				output.WriteInt64(this.RoomOwner);
			}
			if (this.RoomOwnerName.Length != 0)
			{
				output.WriteRawTag(34);
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
			this.players_.WriteTo(output, CreateRoom._repeated_players_codec);
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

		// Token: 0x06000137 RID: 311 RVA: 0x000084D4 File Offset: 0x000066D4
		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (this.RoomId.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.RoomId);
			}
			if (this.RoomName.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.RoomName);
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
			num += this.players_.CalculateSize(CreateRoom._repeated_players_codec);
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

		// Token: 0x06000138 RID: 312 RVA: 0x000085CC File Offset: 0x000067CC
		[DebuggerNonUserCode]
		public void MergeFrom(CreateRoom other)
		{
			if (other == null)
			{
				return;
			}
			if (other.RoomId.Length != 0)
			{
				this.RoomId = other.RoomId;
			}
			if (other.RoomName.Length != 0)
			{
				this.RoomName = other.RoomName;
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

		// Token: 0x06000139 RID: 313 RVA: 0x000086D8 File Offset: 0x000068D8
		[DebuggerNonUserCode]
		public void MergeFrom(CodedInputStream input)
		{
			uint num;
			while ((num = input.ReadTag()) != 0U)
			{
				if (num <= 34U)
				{
					if (num <= 18U)
					{
						if (num == 10U)
						{
							this.RoomId = input.ReadString();
							continue;
						}
						if (num == 18U)
						{
							this.RoomName = input.ReadString();
							continue;
						}
					}
					else
					{
						if (num == 24U)
						{
							this.RoomOwner = input.ReadInt64();
							continue;
						}
						if (num == 34U)
						{
							this.RoomOwnerName = input.ReadString();
							continue;
						}
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
						this.players_.AddEntriesFrom(input, CreateRoom._repeated_players_codec);
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

		// Token: 0x040000AD RID: 173
		private static readonly MessageParser<CreateRoom> _parser = new MessageParser<CreateRoom>(() => new CreateRoom());

		// Token: 0x040000AE RID: 174
		private UnknownFieldSet _unknownFields;

		// Token: 0x040000AF RID: 175
		public const int RoomIdFieldNumber = 1;

		// Token: 0x040000B0 RID: 176
		private string roomId_ = "";

		// Token: 0x040000B1 RID: 177
		public const int RoomNameFieldNumber = 2;

		// Token: 0x040000B2 RID: 178
		private string roomName_ = "";

		// Token: 0x040000B3 RID: 179
		public const int RoomOwnerFieldNumber = 3;

		// Token: 0x040000B4 RID: 180
		private long roomOwner_;

		// Token: 0x040000B5 RID: 181
		public const int RoomOwnerNameFieldNumber = 4;

		// Token: 0x040000B6 RID: 182
		private string roomOwnerName_ = "";

		// Token: 0x040000B7 RID: 183
		public const int RoomCfgFieldNumber = 7;

		// Token: 0x040000B8 RID: 184
		private RoomCfg roomCfg_;

		// Token: 0x040000B9 RID: 185
		public const int SongCfgFieldNumber = 8;

		// Token: 0x040000BA RID: 186
		private SongCfg songCfg_;

		// Token: 0x040000BB RID: 187
		public const int PlayersFieldNumber = 10;

		// Token: 0x040000BC RID: 188
		private static readonly FieldCodec<RoomPlayer> _repeated_players_codec = FieldCodec.ForMessage<RoomPlayer>(82U, RoomPlayer.Parser);

		// Token: 0x040000BD RID: 189
		private readonly RepeatedField<RoomPlayer> players_ = new RepeatedField<RoomPlayer>();

		// Token: 0x040000BE RID: 190
		public const int StatusFieldNumber = 12;

		// Token: 0x040000BF RID: 191
		private string status_ = "";
	}
}
