using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

namespace Com.Netvios.Proto.Outbound
{
	// Token: 0x02000020 RID: 32
	public sealed class RoomUpdatedNotice : IMessage<RoomUpdatedNotice>, IMessage, IEquatable<RoomUpdatedNotice>, IDeepCloneable<RoomUpdatedNotice>
	{
		// Token: 0x170000CB RID: 203
		// (get) Token: 0x06000270 RID: 624 RVA: 0x0000C148 File Offset: 0x0000A348
		[DebuggerNonUserCode]
		public static MessageParser<RoomUpdatedNotice> Parser
		{
			get
			{
				return RoomUpdatedNotice._parser;
			}
		}

		// Token: 0x170000CC RID: 204
		// (get) Token: 0x06000271 RID: 625 RVA: 0x0000C14F File Offset: 0x0000A34F
		[DebuggerNonUserCode]
		public static MessageDescriptor Descriptor
		{
			get
			{
				return BeatSaberOutboundMessageReflection.Descriptor.MessageTypes[24];
			}
		}

		// Token: 0x170000CD RID: 205
		// (get) Token: 0x06000272 RID: 626 RVA: 0x0000C162 File Offset: 0x0000A362
		[DebuggerNonUserCode]
		MessageDescriptor IMessage.Descriptor
		{
			get
			{
				return RoomUpdatedNotice.Descriptor;
			}
		}

		// Token: 0x06000273 RID: 627 RVA: 0x0000C169 File Offset: 0x0000A369
		[DebuggerNonUserCode]
		public RoomUpdatedNotice()
		{
		}

		// Token: 0x06000274 RID: 628 RVA: 0x0000C1A8 File Offset: 0x0000A3A8
		[DebuggerNonUserCode]
		public RoomUpdatedNotice(RoomUpdatedNotice other)
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
			this.eventType_ = other.eventType_;
			this._unknownFields = UnknownFieldSet.Clone(other._unknownFields);
		}

		// Token: 0x06000275 RID: 629 RVA: 0x0000C25D File Offset: 0x0000A45D
		[DebuggerNonUserCode]
		public RoomUpdatedNotice Clone()
		{
			return new RoomUpdatedNotice(this);
		}

		// Token: 0x170000CE RID: 206
		// (get) Token: 0x06000276 RID: 630 RVA: 0x0000C265 File Offset: 0x0000A465
		// (set) Token: 0x06000277 RID: 631 RVA: 0x0000C26D File Offset: 0x0000A46D
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

		// Token: 0x170000CF RID: 207
		// (get) Token: 0x06000278 RID: 632 RVA: 0x0000C280 File Offset: 0x0000A480
		// (set) Token: 0x06000279 RID: 633 RVA: 0x0000C288 File Offset: 0x0000A488
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

		// Token: 0x170000D0 RID: 208
		// (get) Token: 0x0600027A RID: 634 RVA: 0x0000C29B File Offset: 0x0000A49B
		// (set) Token: 0x0600027B RID: 635 RVA: 0x0000C2A3 File Offset: 0x0000A4A3
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

		// Token: 0x170000D1 RID: 209
		// (get) Token: 0x0600027C RID: 636 RVA: 0x0000C2AC File Offset: 0x0000A4AC
		// (set) Token: 0x0600027D RID: 637 RVA: 0x0000C2B4 File Offset: 0x0000A4B4
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

		// Token: 0x170000D2 RID: 210
		// (get) Token: 0x0600027E RID: 638 RVA: 0x0000C2C7 File Offset: 0x0000A4C7
		// (set) Token: 0x0600027F RID: 639 RVA: 0x0000C2CF File Offset: 0x0000A4CF
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

		// Token: 0x170000D3 RID: 211
		// (get) Token: 0x06000280 RID: 640 RVA: 0x0000C2D8 File Offset: 0x0000A4D8
		// (set) Token: 0x06000281 RID: 641 RVA: 0x0000C2E0 File Offset: 0x0000A4E0
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

		// Token: 0x170000D4 RID: 212
		// (get) Token: 0x06000282 RID: 642 RVA: 0x0000C2E9 File Offset: 0x0000A4E9
		[DebuggerNonUserCode]
		public RepeatedField<RoomPlayer> Players
		{
			get
			{
				return this.players_;
			}
		}

		// Token: 0x170000D5 RID: 213
		// (get) Token: 0x06000283 RID: 643 RVA: 0x0000C2F1 File Offset: 0x0000A4F1
		// (set) Token: 0x06000284 RID: 644 RVA: 0x0000C2F9 File Offset: 0x0000A4F9
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

		// Token: 0x170000D6 RID: 214
		// (get) Token: 0x06000285 RID: 645 RVA: 0x0000C30C File Offset: 0x0000A50C
		// (set) Token: 0x06000286 RID: 646 RVA: 0x0000C314 File Offset: 0x0000A514
		[DebuggerNonUserCode]
		public DataType EventType
		{
			get
			{
				return this.eventType_;
			}
			set
			{
				this.eventType_ = value;
			}
		}

		// Token: 0x06000287 RID: 647 RVA: 0x0000C31D File Offset: 0x0000A51D
		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return this.Equals(other as RoomUpdatedNotice);
		}

		// Token: 0x06000288 RID: 648 RVA: 0x0000C32C File Offset: 0x0000A52C
		[DebuggerNonUserCode]
		public bool Equals(RoomUpdatedNotice other)
		{
			return other != null && (other == this || (!(this.RoomId != other.RoomId) && !(this.RoomName != other.RoomName) && this.RoomOwner == other.RoomOwner && !(this.RoomOwnerName != other.RoomOwnerName) && object.Equals(this.RoomCfg, other.RoomCfg) && object.Equals(this.SongCfg, other.SongCfg) && this.players_.Equals(other.players_) && !(this.Status != other.Status) && this.EventType == other.EventType && object.Equals(this._unknownFields, other._unknownFields)));
		}

		// Token: 0x06000289 RID: 649 RVA: 0x0000C408 File Offset: 0x0000A608
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
			if (this.EventType != DataType.Ping)
			{
				num ^= this.EventType.GetHashCode();
			}
			if (this._unknownFields != null)
			{
				num ^= this._unknownFields.GetHashCode();
			}
			return num;
		}

		// Token: 0x0600028A RID: 650 RVA: 0x0000460F File Offset: 0x0000280F
		[DebuggerNonUserCode]
		public override string ToString()
		{
			return JsonFormatter.ToDiagnosticString(this);
		}

		// Token: 0x0600028B RID: 651 RVA: 0x0000C50C File Offset: 0x0000A70C
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
			this.players_.WriteTo(output, RoomUpdatedNotice._repeated_players_codec);
			if (this.Status.Length != 0)
			{
				output.WriteRawTag(98);
				output.WriteString(this.Status);
			}
			if (this.EventType != DataType.Ping)
			{
				output.WriteRawTag(120);
				output.WriteEnum((int)this.EventType);
			}
			if (this._unknownFields != null)
			{
				this._unknownFields.WriteTo(output);
			}
		}

		// Token: 0x0600028C RID: 652 RVA: 0x0000C634 File Offset: 0x0000A834
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
			num += this.players_.CalculateSize(RoomUpdatedNotice._repeated_players_codec);
			if (this.Status.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.Status);
			}
			if (this.EventType != DataType.Ping)
			{
				num += 1 + CodedOutputStream.ComputeEnumSize((int)this.EventType);
			}
			if (this._unknownFields != null)
			{
				num += this._unknownFields.CalculateSize();
			}
			return num;
		}

		// Token: 0x0600028D RID: 653 RVA: 0x0000C744 File Offset: 0x0000A944
		[DebuggerNonUserCode]
		public void MergeFrom(RoomUpdatedNotice other)
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
			if (other.EventType != DataType.Ping)
			{
				this.EventType = other.EventType;
			}
			this._unknownFields = UnknownFieldSet.MergeFrom(this._unknownFields, other._unknownFields);
		}

		// Token: 0x0600028E RID: 654 RVA: 0x0000C864 File Offset: 0x0000AA64
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
						this.players_.AddEntriesFrom(input, RoomUpdatedNotice._repeated_players_codec);
						continue;
					}
					if (num == 98U)
					{
						this.Status = input.ReadString();
						continue;
					}
					if (num == 120U)
					{
						this.EventType = (DataType)input.ReadEnum();
						continue;
					}
				}
				this._unknownFields = UnknownFieldSet.MergeFieldFrom(this._unknownFields, input);
			}
		}

		// Token: 0x04000145 RID: 325
		private static readonly MessageParser<RoomUpdatedNotice> _parser = new MessageParser<RoomUpdatedNotice>(() => new RoomUpdatedNotice());

		// Token: 0x04000146 RID: 326
		private UnknownFieldSet _unknownFields;

		// Token: 0x04000147 RID: 327
		public const int RoomIdFieldNumber = 1;

		// Token: 0x04000148 RID: 328
		private string roomId_ = "";

		// Token: 0x04000149 RID: 329
		public const int RoomNameFieldNumber = 2;

		// Token: 0x0400014A RID: 330
		private string roomName_ = "";

		// Token: 0x0400014B RID: 331
		public const int RoomOwnerFieldNumber = 3;

		// Token: 0x0400014C RID: 332
		private long roomOwner_;

		// Token: 0x0400014D RID: 333
		public const int RoomOwnerNameFieldNumber = 4;

		// Token: 0x0400014E RID: 334
		private string roomOwnerName_ = "";

		// Token: 0x0400014F RID: 335
		public const int RoomCfgFieldNumber = 7;

		// Token: 0x04000150 RID: 336
		private RoomCfg roomCfg_;

		// Token: 0x04000151 RID: 337
		public const int SongCfgFieldNumber = 8;

		// Token: 0x04000152 RID: 338
		private SongCfg songCfg_;

		// Token: 0x04000153 RID: 339
		public const int PlayersFieldNumber = 10;

		// Token: 0x04000154 RID: 340
		private static readonly FieldCodec<RoomPlayer> _repeated_players_codec = FieldCodec.ForMessage<RoomPlayer>(82U, RoomPlayer.Parser);

		// Token: 0x04000155 RID: 341
		private readonly RepeatedField<RoomPlayer> players_ = new RepeatedField<RoomPlayer>();

		// Token: 0x04000156 RID: 342
		public const int StatusFieldNumber = 12;

		// Token: 0x04000157 RID: 343
		private string status_ = "";

		// Token: 0x04000158 RID: 344
		public const int EventTypeFieldNumber = 15;

		// Token: 0x04000159 RID: 345
		private DataType eventType_;
	}
}
