using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

namespace Com.Netvios.Proto.Outbound
{
	// Token: 0x02000012 RID: 18
	public sealed class JoinRoom : IMessage<JoinRoom>, IMessage, IEquatable<JoinRoom>, IDeepCloneable<JoinRoom>
	{
		// Token: 0x1700006E RID: 110
		// (get) Token: 0x0600013B RID: 315 RVA: 0x0000881D File Offset: 0x00006A1D
		[DebuggerNonUserCode]
		public static MessageParser<JoinRoom> Parser
		{
			get
			{
				return JoinRoom._parser;
			}
		}

		// Token: 0x1700006F RID: 111
		// (get) Token: 0x0600013C RID: 316 RVA: 0x00008824 File Offset: 0x00006A24
		[DebuggerNonUserCode]
		public static MessageDescriptor Descriptor
		{
			get
			{
				return BeatSaberOutboundMessageReflection.Descriptor.MessageTypes[10];
			}
		}

		// Token: 0x17000070 RID: 112
		// (get) Token: 0x0600013D RID: 317 RVA: 0x00008837 File Offset: 0x00006A37
		[DebuggerNonUserCode]
		MessageDescriptor IMessage.Descriptor
		{
			get
			{
				return JoinRoom.Descriptor;
			}
		}

		// Token: 0x0600013E RID: 318 RVA: 0x0000883E File Offset: 0x00006A3E
		[DebuggerNonUserCode]
		public JoinRoom()
		{
		}

		// Token: 0x0600013F RID: 319 RVA: 0x00008880 File Offset: 0x00006A80
		[DebuggerNonUserCode]
		public JoinRoom(JoinRoom other)
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

		// Token: 0x06000140 RID: 320 RVA: 0x00008929 File Offset: 0x00006B29
		[DebuggerNonUserCode]
		public JoinRoom Clone()
		{
			return new JoinRoom(this);
		}

		// Token: 0x17000071 RID: 113
		// (get) Token: 0x06000141 RID: 321 RVA: 0x00008931 File Offset: 0x00006B31
		// (set) Token: 0x06000142 RID: 322 RVA: 0x00008939 File Offset: 0x00006B39
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

		// Token: 0x17000072 RID: 114
		// (get) Token: 0x06000143 RID: 323 RVA: 0x0000894C File Offset: 0x00006B4C
		// (set) Token: 0x06000144 RID: 324 RVA: 0x00008954 File Offset: 0x00006B54
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

		// Token: 0x17000073 RID: 115
		// (get) Token: 0x06000145 RID: 325 RVA: 0x00008967 File Offset: 0x00006B67
		// (set) Token: 0x06000146 RID: 326 RVA: 0x0000896F File Offset: 0x00006B6F
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

		// Token: 0x17000074 RID: 116
		// (get) Token: 0x06000147 RID: 327 RVA: 0x00008978 File Offset: 0x00006B78
		// (set) Token: 0x06000148 RID: 328 RVA: 0x00008980 File Offset: 0x00006B80
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

		// Token: 0x17000075 RID: 117
		// (get) Token: 0x06000149 RID: 329 RVA: 0x00008993 File Offset: 0x00006B93
		// (set) Token: 0x0600014A RID: 330 RVA: 0x0000899B File Offset: 0x00006B9B
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

		// Token: 0x17000076 RID: 118
		// (get) Token: 0x0600014B RID: 331 RVA: 0x000089A4 File Offset: 0x00006BA4
		// (set) Token: 0x0600014C RID: 332 RVA: 0x000089AC File Offset: 0x00006BAC
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

		// Token: 0x17000077 RID: 119
		// (get) Token: 0x0600014D RID: 333 RVA: 0x000089B5 File Offset: 0x00006BB5
		[DebuggerNonUserCode]
		public RepeatedField<RoomPlayer> Players
		{
			get
			{
				return this.players_;
			}
		}

		// Token: 0x17000078 RID: 120
		// (get) Token: 0x0600014E RID: 334 RVA: 0x000089BD File Offset: 0x00006BBD
		// (set) Token: 0x0600014F RID: 335 RVA: 0x000089C5 File Offset: 0x00006BC5
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

		// Token: 0x06000150 RID: 336 RVA: 0x000089D8 File Offset: 0x00006BD8
		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return this.Equals(other as JoinRoom);
		}

		// Token: 0x06000151 RID: 337 RVA: 0x000089E8 File Offset: 0x00006BE8
		[DebuggerNonUserCode]
		public bool Equals(JoinRoom other)
		{
			return other != null && (other == this || (!(this.RoomId != other.RoomId) && !(this.RoomName != other.RoomName) && this.RoomOwner == other.RoomOwner && !(this.RoomOwnerName != other.RoomOwnerName) && object.Equals(this.RoomCfg, other.RoomCfg) && object.Equals(this.SongCfg, other.SongCfg) && this.players_.Equals(other.players_) && !(this.Status != other.Status) && object.Equals(this._unknownFields, other._unknownFields)));
		}

		// Token: 0x06000152 RID: 338 RVA: 0x00008AB4 File Offset: 0x00006CB4
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

		// Token: 0x06000153 RID: 339 RVA: 0x0000460F File Offset: 0x0000280F
		[DebuggerNonUserCode]
		public override string ToString()
		{
			return JsonFormatter.ToDiagnosticString(this);
		}

		// Token: 0x06000154 RID: 340 RVA: 0x00008B9C File Offset: 0x00006D9C
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
			this.players_.WriteTo(output, JoinRoom._repeated_players_codec);
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

		// Token: 0x06000155 RID: 341 RVA: 0x00008CA8 File Offset: 0x00006EA8
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
			num += this.players_.CalculateSize(JoinRoom._repeated_players_codec);
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

		// Token: 0x06000156 RID: 342 RVA: 0x00008DA0 File Offset: 0x00006FA0
		[DebuggerNonUserCode]
		public void MergeFrom(JoinRoom other)
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

		// Token: 0x06000157 RID: 343 RVA: 0x00008EAC File Offset: 0x000070AC
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
						this.players_.AddEntriesFrom(input, JoinRoom._repeated_players_codec);
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

		// Token: 0x040000C0 RID: 192
		private static readonly MessageParser<JoinRoom> _parser = new MessageParser<JoinRoom>(() => new JoinRoom());

		// Token: 0x040000C1 RID: 193
		private UnknownFieldSet _unknownFields;

		// Token: 0x040000C2 RID: 194
		public const int RoomIdFieldNumber = 1;

		// Token: 0x040000C3 RID: 195
		private string roomId_ = "";

		// Token: 0x040000C4 RID: 196
		public const int RoomNameFieldNumber = 2;

		// Token: 0x040000C5 RID: 197
		private string roomName_ = "";

		// Token: 0x040000C6 RID: 198
		public const int RoomOwnerFieldNumber = 3;

		// Token: 0x040000C7 RID: 199
		private long roomOwner_;

		// Token: 0x040000C8 RID: 200
		public const int RoomOwnerNameFieldNumber = 4;

		// Token: 0x040000C9 RID: 201
		private string roomOwnerName_ = "";

		// Token: 0x040000CA RID: 202
		public const int RoomCfgFieldNumber = 7;

		// Token: 0x040000CB RID: 203
		private RoomCfg roomCfg_;

		// Token: 0x040000CC RID: 204
		public const int SongCfgFieldNumber = 8;

		// Token: 0x040000CD RID: 205
		private SongCfg songCfg_;

		// Token: 0x040000CE RID: 206
		public const int PlayersFieldNumber = 10;

		// Token: 0x040000CF RID: 207
		private static readonly FieldCodec<RoomPlayer> _repeated_players_codec = FieldCodec.ForMessage<RoomPlayer>(82U, RoomPlayer.Parser);

		// Token: 0x040000D0 RID: 208
		private readonly RepeatedField<RoomPlayer> players_ = new RepeatedField<RoomPlayer>();

		// Token: 0x040000D1 RID: 209
		public const int StatusFieldNumber = 12;

		// Token: 0x040000D2 RID: 210
		private string status_ = "";
	}
}
