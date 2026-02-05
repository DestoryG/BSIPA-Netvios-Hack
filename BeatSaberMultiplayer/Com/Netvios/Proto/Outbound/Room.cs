using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

namespace Com.Netvios.Proto.Outbound
{
	// Token: 0x02000028 RID: 40
	public sealed class Room : IMessage<Room>, IMessage, IEquatable<Room>, IDeepCloneable<Room>
	{
		// Token: 0x17000112 RID: 274
		// (get) Token: 0x06000344 RID: 836 RVA: 0x0000F11B File Offset: 0x0000D31B
		[DebuggerNonUserCode]
		public static MessageParser<Room> Parser
		{
			get
			{
				return Room._parser;
			}
		}

		// Token: 0x17000113 RID: 275
		// (get) Token: 0x06000345 RID: 837 RVA: 0x0000F122 File Offset: 0x0000D322
		[DebuggerNonUserCode]
		public static MessageDescriptor Descriptor
		{
			get
			{
				return BeatSaberOutboundMessageReflection.Descriptor.MessageTypes[32];
			}
		}

		// Token: 0x17000114 RID: 276
		// (get) Token: 0x06000346 RID: 838 RVA: 0x0000F135 File Offset: 0x0000D335
		[DebuggerNonUserCode]
		MessageDescriptor IMessage.Descriptor
		{
			get
			{
				return Room.Descriptor;
			}
		}

		// Token: 0x06000347 RID: 839 RVA: 0x0000F13C File Offset: 0x0000D33C
		[DebuggerNonUserCode]
		public Room()
		{
		}

		// Token: 0x06000348 RID: 840 RVA: 0x0000F170 File Offset: 0x0000D370
		[DebuggerNonUserCode]
		public Room(Room other)
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

		// Token: 0x06000349 RID: 841 RVA: 0x0000F20D File Offset: 0x0000D40D
		[DebuggerNonUserCode]
		public Room Clone()
		{
			return new Room(this);
		}

		// Token: 0x17000115 RID: 277
		// (get) Token: 0x0600034A RID: 842 RVA: 0x0000F215 File Offset: 0x0000D415
		// (set) Token: 0x0600034B RID: 843 RVA: 0x0000F21D File Offset: 0x0000D41D
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

		// Token: 0x17000116 RID: 278
		// (get) Token: 0x0600034C RID: 844 RVA: 0x0000F230 File Offset: 0x0000D430
		// (set) Token: 0x0600034D RID: 845 RVA: 0x0000F238 File Offset: 0x0000D438
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

		// Token: 0x17000117 RID: 279
		// (get) Token: 0x0600034E RID: 846 RVA: 0x0000F241 File Offset: 0x0000D441
		// (set) Token: 0x0600034F RID: 847 RVA: 0x0000F249 File Offset: 0x0000D449
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

		// Token: 0x17000118 RID: 280
		// (get) Token: 0x06000350 RID: 848 RVA: 0x0000F25C File Offset: 0x0000D45C
		// (set) Token: 0x06000351 RID: 849 RVA: 0x0000F264 File Offset: 0x0000D464
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

		// Token: 0x17000119 RID: 281
		// (get) Token: 0x06000352 RID: 850 RVA: 0x0000F26D File Offset: 0x0000D46D
		// (set) Token: 0x06000353 RID: 851 RVA: 0x0000F275 File Offset: 0x0000D475
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

		// Token: 0x1700011A RID: 282
		// (get) Token: 0x06000354 RID: 852 RVA: 0x0000F27E File Offset: 0x0000D47E
		[DebuggerNonUserCode]
		public RepeatedField<RoomPlayer> Players
		{
			get
			{
				return this.players_;
			}
		}

		// Token: 0x1700011B RID: 283
		// (get) Token: 0x06000355 RID: 853 RVA: 0x0000F286 File Offset: 0x0000D486
		// (set) Token: 0x06000356 RID: 854 RVA: 0x0000F28E File Offset: 0x0000D48E
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

		// Token: 0x06000357 RID: 855 RVA: 0x0000F2A1 File Offset: 0x0000D4A1
		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return this.Equals(other as Room);
		}

		// Token: 0x06000358 RID: 856 RVA: 0x0000F2B0 File Offset: 0x0000D4B0
		[DebuggerNonUserCode]
		public bool Equals(Room other)
		{
			return other != null && (other == this || (!(this.RoomId != other.RoomId) && this.RoomOwner == other.RoomOwner && !(this.RoomOwnerName != other.RoomOwnerName) && object.Equals(this.RoomCfg, other.RoomCfg) && object.Equals(this.SongCfg, other.SongCfg) && this.players_.Equals(other.players_) && !(this.Status != other.Status) && object.Equals(this._unknownFields, other._unknownFields)));
		}

		// Token: 0x06000359 RID: 857 RVA: 0x0000F368 File Offset: 0x0000D568
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

		// Token: 0x0600035A RID: 858 RVA: 0x0000460F File Offset: 0x0000280F
		[DebuggerNonUserCode]
		public override string ToString()
		{
			return JsonFormatter.ToDiagnosticString(this);
		}

		// Token: 0x0600035B RID: 859 RVA: 0x0000F434 File Offset: 0x0000D634
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
			this.players_.WriteTo(output, Room._repeated_players_codec);
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

		// Token: 0x0600035C RID: 860 RVA: 0x0000F520 File Offset: 0x0000D720
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
			num += this.players_.CalculateSize(Room._repeated_players_codec);
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

		// Token: 0x0600035D RID: 861 RVA: 0x0000F5F8 File Offset: 0x0000D7F8
		[DebuggerNonUserCode]
		public void MergeFrom(Room other)
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

		// Token: 0x0600035E RID: 862 RVA: 0x0000F6E8 File Offset: 0x0000D8E8
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
						this.players_.AddEntriesFrom(input, Room._repeated_players_codec);
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

		// Token: 0x040001B5 RID: 437
		private static readonly MessageParser<Room> _parser = new MessageParser<Room>(() => new Room());

		// Token: 0x040001B6 RID: 438
		private UnknownFieldSet _unknownFields;

		// Token: 0x040001B7 RID: 439
		public const int RoomIdFieldNumber = 1;

		// Token: 0x040001B8 RID: 440
		private string roomId_ = "";

		// Token: 0x040001B9 RID: 441
		public const int RoomOwnerFieldNumber = 2;

		// Token: 0x040001BA RID: 442
		private long roomOwner_;

		// Token: 0x040001BB RID: 443
		public const int RoomOwnerNameFieldNumber = 3;

		// Token: 0x040001BC RID: 444
		private string roomOwnerName_ = "";

		// Token: 0x040001BD RID: 445
		public const int RoomCfgFieldNumber = 7;

		// Token: 0x040001BE RID: 446
		private RoomCfg roomCfg_;

		// Token: 0x040001BF RID: 447
		public const int SongCfgFieldNumber = 8;

		// Token: 0x040001C0 RID: 448
		private SongCfg songCfg_;

		// Token: 0x040001C1 RID: 449
		public const int PlayersFieldNumber = 10;

		// Token: 0x040001C2 RID: 450
		private static readonly FieldCodec<RoomPlayer> _repeated_players_codec = FieldCodec.ForMessage<RoomPlayer>(82U, RoomPlayer.Parser);

		// Token: 0x040001C3 RID: 451
		private readonly RepeatedField<RoomPlayer> players_ = new RepeatedField<RoomPlayer>();

		// Token: 0x040001C4 RID: 452
		public const int StatusFieldNumber = 12;

		// Token: 0x040001C5 RID: 453
		private string status_ = "";
	}
}
