using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

namespace Com.Netvios.Proto.Outbound
{
	// Token: 0x0200001D RID: 29
	public sealed class FastMatch : IMessage<FastMatch>, IMessage, IEquatable<FastMatch>, IDeepCloneable<FastMatch>
	{
		// Token: 0x170000B7 RID: 183
		// (get) Token: 0x0600022E RID: 558 RVA: 0x0000B4D9 File Offset: 0x000096D9
		[DebuggerNonUserCode]
		public static MessageParser<FastMatch> Parser
		{
			get
			{
				return FastMatch._parser;
			}
		}

		// Token: 0x170000B8 RID: 184
		// (get) Token: 0x0600022F RID: 559 RVA: 0x0000B4E0 File Offset: 0x000096E0
		[DebuggerNonUserCode]
		public static MessageDescriptor Descriptor
		{
			get
			{
				return BeatSaberOutboundMessageReflection.Descriptor.MessageTypes[21];
			}
		}

		// Token: 0x170000B9 RID: 185
		// (get) Token: 0x06000230 RID: 560 RVA: 0x0000B4F3 File Offset: 0x000096F3
		[DebuggerNonUserCode]
		MessageDescriptor IMessage.Descriptor
		{
			get
			{
				return FastMatch.Descriptor;
			}
		}

		// Token: 0x06000231 RID: 561 RVA: 0x0000B4FA File Offset: 0x000096FA
		[DebuggerNonUserCode]
		public FastMatch()
		{
		}

		// Token: 0x06000232 RID: 562 RVA: 0x0000B53C File Offset: 0x0000973C
		[DebuggerNonUserCode]
		public FastMatch(FastMatch other)
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

		// Token: 0x06000233 RID: 563 RVA: 0x0000B5E5 File Offset: 0x000097E5
		[DebuggerNonUserCode]
		public FastMatch Clone()
		{
			return new FastMatch(this);
		}

		// Token: 0x170000BA RID: 186
		// (get) Token: 0x06000234 RID: 564 RVA: 0x0000B5ED File Offset: 0x000097ED
		// (set) Token: 0x06000235 RID: 565 RVA: 0x0000B5F5 File Offset: 0x000097F5
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

		// Token: 0x170000BB RID: 187
		// (get) Token: 0x06000236 RID: 566 RVA: 0x0000B608 File Offset: 0x00009808
		// (set) Token: 0x06000237 RID: 567 RVA: 0x0000B610 File Offset: 0x00009810
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

		// Token: 0x170000BC RID: 188
		// (get) Token: 0x06000238 RID: 568 RVA: 0x0000B623 File Offset: 0x00009823
		// (set) Token: 0x06000239 RID: 569 RVA: 0x0000B62B File Offset: 0x0000982B
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

		// Token: 0x170000BD RID: 189
		// (get) Token: 0x0600023A RID: 570 RVA: 0x0000B634 File Offset: 0x00009834
		// (set) Token: 0x0600023B RID: 571 RVA: 0x0000B63C File Offset: 0x0000983C
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

		// Token: 0x170000BE RID: 190
		// (get) Token: 0x0600023C RID: 572 RVA: 0x0000B64F File Offset: 0x0000984F
		// (set) Token: 0x0600023D RID: 573 RVA: 0x0000B657 File Offset: 0x00009857
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

		// Token: 0x170000BF RID: 191
		// (get) Token: 0x0600023E RID: 574 RVA: 0x0000B660 File Offset: 0x00009860
		// (set) Token: 0x0600023F RID: 575 RVA: 0x0000B668 File Offset: 0x00009868
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

		// Token: 0x170000C0 RID: 192
		// (get) Token: 0x06000240 RID: 576 RVA: 0x0000B671 File Offset: 0x00009871
		[DebuggerNonUserCode]
		public RepeatedField<RoomPlayer> Players
		{
			get
			{
				return this.players_;
			}
		}

		// Token: 0x170000C1 RID: 193
		// (get) Token: 0x06000241 RID: 577 RVA: 0x0000B679 File Offset: 0x00009879
		// (set) Token: 0x06000242 RID: 578 RVA: 0x0000B681 File Offset: 0x00009881
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

		// Token: 0x06000243 RID: 579 RVA: 0x0000B694 File Offset: 0x00009894
		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return this.Equals(other as FastMatch);
		}

		// Token: 0x06000244 RID: 580 RVA: 0x0000B6A4 File Offset: 0x000098A4
		[DebuggerNonUserCode]
		public bool Equals(FastMatch other)
		{
			return other != null && (other == this || (!(this.RoomId != other.RoomId) && !(this.RoomName != other.RoomName) && this.RoomOwner == other.RoomOwner && !(this.RoomOwnerName != other.RoomOwnerName) && object.Equals(this.RoomCfg, other.RoomCfg) && object.Equals(this.SongCfg, other.SongCfg) && this.players_.Equals(other.players_) && !(this.Status != other.Status) && object.Equals(this._unknownFields, other._unknownFields)));
		}

		// Token: 0x06000245 RID: 581 RVA: 0x0000B770 File Offset: 0x00009970
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

		// Token: 0x06000246 RID: 582 RVA: 0x0000460F File Offset: 0x0000280F
		[DebuggerNonUserCode]
		public override string ToString()
		{
			return JsonFormatter.ToDiagnosticString(this);
		}

		// Token: 0x06000247 RID: 583 RVA: 0x0000B858 File Offset: 0x00009A58
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
			this.players_.WriteTo(output, FastMatch._repeated_players_codec);
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

		// Token: 0x06000248 RID: 584 RVA: 0x0000B964 File Offset: 0x00009B64
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
			num += this.players_.CalculateSize(FastMatch._repeated_players_codec);
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

		// Token: 0x06000249 RID: 585 RVA: 0x0000BA5C File Offset: 0x00009C5C
		[DebuggerNonUserCode]
		public void MergeFrom(FastMatch other)
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

		// Token: 0x0600024A RID: 586 RVA: 0x0000BB68 File Offset: 0x00009D68
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
						this.players_.AddEntriesFrom(input, FastMatch._repeated_players_codec);
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

		// Token: 0x04000128 RID: 296
		private static readonly MessageParser<FastMatch> _parser = new MessageParser<FastMatch>(() => new FastMatch());

		// Token: 0x04000129 RID: 297
		private UnknownFieldSet _unknownFields;

		// Token: 0x0400012A RID: 298
		public const int RoomIdFieldNumber = 1;

		// Token: 0x0400012B RID: 299
		private string roomId_ = "";

		// Token: 0x0400012C RID: 300
		public const int RoomNameFieldNumber = 2;

		// Token: 0x0400012D RID: 301
		private string roomName_ = "";

		// Token: 0x0400012E RID: 302
		public const int RoomOwnerFieldNumber = 3;

		// Token: 0x0400012F RID: 303
		private long roomOwner_;

		// Token: 0x04000130 RID: 304
		public const int RoomOwnerNameFieldNumber = 4;

		// Token: 0x04000131 RID: 305
		private string roomOwnerName_ = "";

		// Token: 0x04000132 RID: 306
		public const int RoomCfgFieldNumber = 7;

		// Token: 0x04000133 RID: 307
		private RoomCfg roomCfg_;

		// Token: 0x04000134 RID: 308
		public const int SongCfgFieldNumber = 8;

		// Token: 0x04000135 RID: 309
		private SongCfg songCfg_;

		// Token: 0x04000136 RID: 310
		public const int PlayersFieldNumber = 10;

		// Token: 0x04000137 RID: 311
		private static readonly FieldCodec<RoomPlayer> _repeated_players_codec = FieldCodec.ForMessage<RoomPlayer>(82U, RoomPlayer.Parser);

		// Token: 0x04000138 RID: 312
		private readonly RepeatedField<RoomPlayer> players_ = new RepeatedField<RoomPlayer>();

		// Token: 0x04000139 RID: 313
		public const int StatusFieldNumber = 12;

		// Token: 0x0400013A RID: 314
		private string status_ = "";
	}
}
