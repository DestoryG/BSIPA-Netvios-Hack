using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

namespace Com.Netvios.Proto.Outbound
{
	// Token: 0x02000025 RID: 37
	public sealed class AutoMatchNotice : IMessage<AutoMatchNotice>, IMessage, IEquatable<AutoMatchNotice>, IDeepCloneable<AutoMatchNotice>
	{
		// Token: 0x170000F8 RID: 248
		// (get) Token: 0x060002F6 RID: 758 RVA: 0x0000DF72 File Offset: 0x0000C172
		[DebuggerNonUserCode]
		public static MessageParser<AutoMatchNotice> Parser
		{
			get
			{
				return AutoMatchNotice._parser;
			}
		}

		// Token: 0x170000F9 RID: 249
		// (get) Token: 0x060002F7 RID: 759 RVA: 0x0000DF79 File Offset: 0x0000C179
		[DebuggerNonUserCode]
		public static MessageDescriptor Descriptor
		{
			get
			{
				return BeatSaberOutboundMessageReflection.Descriptor.MessageTypes[29];
			}
		}

		// Token: 0x170000FA RID: 250
		// (get) Token: 0x060002F8 RID: 760 RVA: 0x0000DF8C File Offset: 0x0000C18C
		[DebuggerNonUserCode]
		MessageDescriptor IMessage.Descriptor
		{
			get
			{
				return AutoMatchNotice.Descriptor;
			}
		}

		// Token: 0x060002F9 RID: 761 RVA: 0x0000DF93 File Offset: 0x0000C193
		[DebuggerNonUserCode]
		public AutoMatchNotice()
		{
		}

		// Token: 0x060002FA RID: 762 RVA: 0x0000DFD4 File Offset: 0x0000C1D4
		[DebuggerNonUserCode]
		public AutoMatchNotice(AutoMatchNotice other)
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

		// Token: 0x060002FB RID: 763 RVA: 0x0000E07D File Offset: 0x0000C27D
		[DebuggerNonUserCode]
		public AutoMatchNotice Clone()
		{
			return new AutoMatchNotice(this);
		}

		// Token: 0x170000FB RID: 251
		// (get) Token: 0x060002FC RID: 764 RVA: 0x0000E085 File Offset: 0x0000C285
		// (set) Token: 0x060002FD RID: 765 RVA: 0x0000E08D File Offset: 0x0000C28D
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

		// Token: 0x170000FC RID: 252
		// (get) Token: 0x060002FE RID: 766 RVA: 0x0000E0A0 File Offset: 0x0000C2A0
		// (set) Token: 0x060002FF RID: 767 RVA: 0x0000E0A8 File Offset: 0x0000C2A8
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

		// Token: 0x170000FD RID: 253
		// (get) Token: 0x06000300 RID: 768 RVA: 0x0000E0BB File Offset: 0x0000C2BB
		// (set) Token: 0x06000301 RID: 769 RVA: 0x0000E0C3 File Offset: 0x0000C2C3
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

		// Token: 0x170000FE RID: 254
		// (get) Token: 0x06000302 RID: 770 RVA: 0x0000E0CC File Offset: 0x0000C2CC
		// (set) Token: 0x06000303 RID: 771 RVA: 0x0000E0D4 File Offset: 0x0000C2D4
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

		// Token: 0x170000FF RID: 255
		// (get) Token: 0x06000304 RID: 772 RVA: 0x0000E0E7 File Offset: 0x0000C2E7
		// (set) Token: 0x06000305 RID: 773 RVA: 0x0000E0EF File Offset: 0x0000C2EF
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

		// Token: 0x17000100 RID: 256
		// (get) Token: 0x06000306 RID: 774 RVA: 0x0000E0F8 File Offset: 0x0000C2F8
		// (set) Token: 0x06000307 RID: 775 RVA: 0x0000E100 File Offset: 0x0000C300
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

		// Token: 0x17000101 RID: 257
		// (get) Token: 0x06000308 RID: 776 RVA: 0x0000E109 File Offset: 0x0000C309
		[DebuggerNonUserCode]
		public RepeatedField<RoomPlayer> Players
		{
			get
			{
				return this.players_;
			}
		}

		// Token: 0x17000102 RID: 258
		// (get) Token: 0x06000309 RID: 777 RVA: 0x0000E111 File Offset: 0x0000C311
		// (set) Token: 0x0600030A RID: 778 RVA: 0x0000E119 File Offset: 0x0000C319
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

		// Token: 0x0600030B RID: 779 RVA: 0x0000E12C File Offset: 0x0000C32C
		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return this.Equals(other as AutoMatchNotice);
		}

		// Token: 0x0600030C RID: 780 RVA: 0x0000E13C File Offset: 0x0000C33C
		[DebuggerNonUserCode]
		public bool Equals(AutoMatchNotice other)
		{
			return other != null && (other == this || (!(this.RoomId != other.RoomId) && !(this.RoomName != other.RoomName) && this.RoomOwner == other.RoomOwner && !(this.RoomOwnerName != other.RoomOwnerName) && object.Equals(this.RoomCfg, other.RoomCfg) && object.Equals(this.SongCfg, other.SongCfg) && this.players_.Equals(other.players_) && !(this.Status != other.Status) && object.Equals(this._unknownFields, other._unknownFields)));
		}

		// Token: 0x0600030D RID: 781 RVA: 0x0000E208 File Offset: 0x0000C408
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

		// Token: 0x0600030E RID: 782 RVA: 0x0000460F File Offset: 0x0000280F
		[DebuggerNonUserCode]
		public override string ToString()
		{
			return JsonFormatter.ToDiagnosticString(this);
		}

		// Token: 0x0600030F RID: 783 RVA: 0x0000E2F0 File Offset: 0x0000C4F0
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
			this.players_.WriteTo(output, AutoMatchNotice._repeated_players_codec);
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

		// Token: 0x06000310 RID: 784 RVA: 0x0000E3FC File Offset: 0x0000C5FC
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
			num += this.players_.CalculateSize(AutoMatchNotice._repeated_players_codec);
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

		// Token: 0x06000311 RID: 785 RVA: 0x0000E4F4 File Offset: 0x0000C6F4
		[DebuggerNonUserCode]
		public void MergeFrom(AutoMatchNotice other)
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

		// Token: 0x06000312 RID: 786 RVA: 0x0000E600 File Offset: 0x0000C800
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
						this.players_.AddEntriesFrom(input, AutoMatchNotice._repeated_players_codec);
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

		// Token: 0x0400018C RID: 396
		private static readonly MessageParser<AutoMatchNotice> _parser = new MessageParser<AutoMatchNotice>(() => new AutoMatchNotice());

		// Token: 0x0400018D RID: 397
		private UnknownFieldSet _unknownFields;

		// Token: 0x0400018E RID: 398
		public const int RoomIdFieldNumber = 1;

		// Token: 0x0400018F RID: 399
		private string roomId_ = "";

		// Token: 0x04000190 RID: 400
		public const int RoomNameFieldNumber = 2;

		// Token: 0x04000191 RID: 401
		private string roomName_ = "";

		// Token: 0x04000192 RID: 402
		public const int RoomOwnerFieldNumber = 3;

		// Token: 0x04000193 RID: 403
		private long roomOwner_;

		// Token: 0x04000194 RID: 404
		public const int RoomOwnerNameFieldNumber = 4;

		// Token: 0x04000195 RID: 405
		private string roomOwnerName_ = "";

		// Token: 0x04000196 RID: 406
		public const int RoomCfgFieldNumber = 7;

		// Token: 0x04000197 RID: 407
		private RoomCfg roomCfg_;

		// Token: 0x04000198 RID: 408
		public const int SongCfgFieldNumber = 8;

		// Token: 0x04000199 RID: 409
		private SongCfg songCfg_;

		// Token: 0x0400019A RID: 410
		public const int PlayersFieldNumber = 10;

		// Token: 0x0400019B RID: 411
		private static readonly FieldCodec<RoomPlayer> _repeated_players_codec = FieldCodec.ForMessage<RoomPlayer>(82U, RoomPlayer.Parser);

		// Token: 0x0400019C RID: 412
		private readonly RepeatedField<RoomPlayer> players_ = new RepeatedField<RoomPlayer>();

		// Token: 0x0400019D RID: 413
		public const int StatusFieldNumber = 12;

		// Token: 0x0400019E RID: 414
		private string status_ = "";
	}
}
