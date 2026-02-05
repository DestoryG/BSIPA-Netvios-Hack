using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Com.Netvios.Proto.Outbound
{
	// Token: 0x02000023 RID: 35
	public sealed class RoomSubmitScoreNotice : IMessage<RoomSubmitScoreNotice>, IMessage, IEquatable<RoomSubmitScoreNotice>, IDeepCloneable<RoomSubmitScoreNotice>
	{
		// Token: 0x170000E2 RID: 226
		// (get) Token: 0x060002B8 RID: 696 RVA: 0x0000D07C File Offset: 0x0000B27C
		[DebuggerNonUserCode]
		public static MessageParser<RoomSubmitScoreNotice> Parser
		{
			get
			{
				return RoomSubmitScoreNotice._parser;
			}
		}

		// Token: 0x170000E3 RID: 227
		// (get) Token: 0x060002B9 RID: 697 RVA: 0x0000D083 File Offset: 0x0000B283
		[DebuggerNonUserCode]
		public static MessageDescriptor Descriptor
		{
			get
			{
				return BeatSaberOutboundMessageReflection.Descriptor.MessageTypes[27];
			}
		}

		// Token: 0x170000E4 RID: 228
		// (get) Token: 0x060002BA RID: 698 RVA: 0x0000D096 File Offset: 0x0000B296
		[DebuggerNonUserCode]
		MessageDescriptor IMessage.Descriptor
		{
			get
			{
				return RoomSubmitScoreNotice.Descriptor;
			}
		}

		// Token: 0x060002BB RID: 699 RVA: 0x0000D0A0 File Offset: 0x0000B2A0
		[DebuggerNonUserCode]
		public RoomSubmitScoreNotice()
		{
		}

		// Token: 0x060002BC RID: 700 RVA: 0x0000D0F8 File Offset: 0x0000B2F8
		[DebuggerNonUserCode]
		public RoomSubmitScoreNotice(RoomSubmitScoreNotice other)
			: this()
		{
			this.appChannel_ = other.appChannel_;
			this.playerId_ = other.playerId_;
			this.nickname_ = other.nickname_;
			this.avatar_ = other.avatar_;
			this.songCfg_ = ((other.songCfg_ != null) ? other.songCfg_.Clone() : null);
			this.valid_ = other.valid_;
			this.score_ = other.score_;
			this.rank_ = other.rank_;
			this.songDidFinish_ = other.songDidFinish_;
			this.message_ = other.message_;
			this.resultDisplaySeconds_ = other.resultDisplaySeconds_;
			this.roomId_ = other.roomId_;
			this._unknownFields = UnknownFieldSet.Clone(other._unknownFields);
		}

		// Token: 0x060002BD RID: 701 RVA: 0x0000D1BC File Offset: 0x0000B3BC
		[DebuggerNonUserCode]
		public RoomSubmitScoreNotice Clone()
		{
			return new RoomSubmitScoreNotice(this);
		}

		// Token: 0x170000E5 RID: 229
		// (get) Token: 0x060002BE RID: 702 RVA: 0x0000D1C4 File Offset: 0x0000B3C4
		// (set) Token: 0x060002BF RID: 703 RVA: 0x0000D1CC File Offset: 0x0000B3CC
		[DebuggerNonUserCode]
		public string AppChannel
		{
			get
			{
				return this.appChannel_;
			}
			set
			{
				this.appChannel_ = ProtoPreconditions.CheckNotNull<string>(value, "value");
			}
		}

		// Token: 0x170000E6 RID: 230
		// (get) Token: 0x060002C0 RID: 704 RVA: 0x0000D1DF File Offset: 0x0000B3DF
		// (set) Token: 0x060002C1 RID: 705 RVA: 0x0000D1E7 File Offset: 0x0000B3E7
		[DebuggerNonUserCode]
		public long PlayerId
		{
			get
			{
				return this.playerId_;
			}
			set
			{
				this.playerId_ = value;
			}
		}

		// Token: 0x170000E7 RID: 231
		// (get) Token: 0x060002C2 RID: 706 RVA: 0x0000D1F0 File Offset: 0x0000B3F0
		// (set) Token: 0x060002C3 RID: 707 RVA: 0x0000D1F8 File Offset: 0x0000B3F8
		[DebuggerNonUserCode]
		public string Nickname
		{
			get
			{
				return this.nickname_;
			}
			set
			{
				this.nickname_ = ProtoPreconditions.CheckNotNull<string>(value, "value");
			}
		}

		// Token: 0x170000E8 RID: 232
		// (get) Token: 0x060002C4 RID: 708 RVA: 0x0000D20B File Offset: 0x0000B40B
		// (set) Token: 0x060002C5 RID: 709 RVA: 0x0000D213 File Offset: 0x0000B413
		[DebuggerNonUserCode]
		public string Avatar
		{
			get
			{
				return this.avatar_;
			}
			set
			{
				this.avatar_ = ProtoPreconditions.CheckNotNull<string>(value, "value");
			}
		}

		// Token: 0x170000E9 RID: 233
		// (get) Token: 0x060002C6 RID: 710 RVA: 0x0000D226 File Offset: 0x0000B426
		// (set) Token: 0x060002C7 RID: 711 RVA: 0x0000D22E File Offset: 0x0000B42E
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

		// Token: 0x170000EA RID: 234
		// (get) Token: 0x060002C8 RID: 712 RVA: 0x0000D237 File Offset: 0x0000B437
		// (set) Token: 0x060002C9 RID: 713 RVA: 0x0000D23F File Offset: 0x0000B43F
		[DebuggerNonUserCode]
		public bool Valid
		{
			get
			{
				return this.valid_;
			}
			set
			{
				this.valid_ = value;
			}
		}

		// Token: 0x170000EB RID: 235
		// (get) Token: 0x060002CA RID: 714 RVA: 0x0000D248 File Offset: 0x0000B448
		// (set) Token: 0x060002CB RID: 715 RVA: 0x0000D250 File Offset: 0x0000B450
		[DebuggerNonUserCode]
		public int Score
		{
			get
			{
				return this.score_;
			}
			set
			{
				this.score_ = value;
			}
		}

		// Token: 0x170000EC RID: 236
		// (get) Token: 0x060002CC RID: 716 RVA: 0x0000D259 File Offset: 0x0000B459
		// (set) Token: 0x060002CD RID: 717 RVA: 0x0000D261 File Offset: 0x0000B461
		[DebuggerNonUserCode]
		public string Rank
		{
			get
			{
				return this.rank_;
			}
			set
			{
				this.rank_ = ProtoPreconditions.CheckNotNull<string>(value, "value");
			}
		}

		// Token: 0x170000ED RID: 237
		// (get) Token: 0x060002CE RID: 718 RVA: 0x0000D274 File Offset: 0x0000B474
		// (set) Token: 0x060002CF RID: 719 RVA: 0x0000D27C File Offset: 0x0000B47C
		[DebuggerNonUserCode]
		public bool SongDidFinish
		{
			get
			{
				return this.songDidFinish_;
			}
			set
			{
				this.songDidFinish_ = value;
			}
		}

		// Token: 0x170000EE RID: 238
		// (get) Token: 0x060002D0 RID: 720 RVA: 0x0000D285 File Offset: 0x0000B485
		// (set) Token: 0x060002D1 RID: 721 RVA: 0x0000D28D File Offset: 0x0000B48D
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

		// Token: 0x170000EF RID: 239
		// (get) Token: 0x060002D2 RID: 722 RVA: 0x0000D2A0 File Offset: 0x0000B4A0
		// (set) Token: 0x060002D3 RID: 723 RVA: 0x0000D2A8 File Offset: 0x0000B4A8
		[DebuggerNonUserCode]
		public int ResultDisplaySeconds
		{
			get
			{
				return this.resultDisplaySeconds_;
			}
			set
			{
				this.resultDisplaySeconds_ = value;
			}
		}

		// Token: 0x170000F0 RID: 240
		// (get) Token: 0x060002D4 RID: 724 RVA: 0x0000D2B1 File Offset: 0x0000B4B1
		// (set) Token: 0x060002D5 RID: 725 RVA: 0x0000D2B9 File Offset: 0x0000B4B9
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

		// Token: 0x060002D6 RID: 726 RVA: 0x0000D2CC File Offset: 0x0000B4CC
		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return this.Equals(other as RoomSubmitScoreNotice);
		}

		// Token: 0x060002D7 RID: 727 RVA: 0x0000D2DC File Offset: 0x0000B4DC
		[DebuggerNonUserCode]
		public bool Equals(RoomSubmitScoreNotice other)
		{
			return other != null && (other == this || (!(this.AppChannel != other.AppChannel) && this.PlayerId == other.PlayerId && !(this.Nickname != other.Nickname) && !(this.Avatar != other.Avatar) && object.Equals(this.SongCfg, other.SongCfg) && this.Valid == other.Valid && this.Score == other.Score && !(this.Rank != other.Rank) && this.SongDidFinish == other.SongDidFinish && !(this.Message != other.Message) && this.ResultDisplaySeconds == other.ResultDisplaySeconds && !(this.RoomId != other.RoomId) && object.Equals(this._unknownFields, other._unknownFields)));
		}

		// Token: 0x060002D8 RID: 728 RVA: 0x0000D3E8 File Offset: 0x0000B5E8
		[DebuggerNonUserCode]
		public override int GetHashCode()
		{
			int num = 1;
			if (this.AppChannel.Length != 0)
			{
				num ^= this.AppChannel.GetHashCode();
			}
			if (this.PlayerId != 0L)
			{
				num ^= this.PlayerId.GetHashCode();
			}
			if (this.Nickname.Length != 0)
			{
				num ^= this.Nickname.GetHashCode();
			}
			if (this.Avatar.Length != 0)
			{
				num ^= this.Avatar.GetHashCode();
			}
			if (this.songCfg_ != null)
			{
				num ^= this.SongCfg.GetHashCode();
			}
			if (this.Valid)
			{
				num ^= this.Valid.GetHashCode();
			}
			if (this.Score != 0)
			{
				num ^= this.Score.GetHashCode();
			}
			if (this.Rank.Length != 0)
			{
				num ^= this.Rank.GetHashCode();
			}
			if (this.SongDidFinish)
			{
				num ^= this.SongDidFinish.GetHashCode();
			}
			if (this.Message.Length != 0)
			{
				num ^= this.Message.GetHashCode();
			}
			if (this.ResultDisplaySeconds != 0)
			{
				num ^= this.ResultDisplaySeconds.GetHashCode();
			}
			if (this.RoomId.Length != 0)
			{
				num ^= this.RoomId.GetHashCode();
			}
			if (this._unknownFields != null)
			{
				num ^= this._unknownFields.GetHashCode();
			}
			return num;
		}

		// Token: 0x060002D9 RID: 729 RVA: 0x0000460F File Offset: 0x0000280F
		[DebuggerNonUserCode]
		public override string ToString()
		{
			return JsonFormatter.ToDiagnosticString(this);
		}

		// Token: 0x060002DA RID: 730 RVA: 0x0000D544 File Offset: 0x0000B744
		[DebuggerNonUserCode]
		public void WriteTo(CodedOutputStream output)
		{
			if (this.AppChannel.Length != 0)
			{
				output.WriteRawTag(10);
				output.WriteString(this.AppChannel);
			}
			if (this.PlayerId != 0L)
			{
				output.WriteRawTag(16);
				output.WriteInt64(this.PlayerId);
			}
			if (this.Nickname.Length != 0)
			{
				output.WriteRawTag(26);
				output.WriteString(this.Nickname);
			}
			if (this.Avatar.Length != 0)
			{
				output.WriteRawTag(34);
				output.WriteString(this.Avatar);
			}
			if (this.songCfg_ != null)
			{
				output.WriteRawTag(42);
				output.WriteMessage(this.SongCfg);
			}
			if (this.Valid)
			{
				output.WriteRawTag(48);
				output.WriteBool(this.Valid);
			}
			if (this.Score != 0)
			{
				output.WriteRawTag(56);
				output.WriteInt32(this.Score);
			}
			if (this.Rank.Length != 0)
			{
				output.WriteRawTag(66);
				output.WriteString(this.Rank);
			}
			if (this.SongDidFinish)
			{
				output.WriteRawTag(72);
				output.WriteBool(this.SongDidFinish);
			}
			if (this.Message.Length != 0)
			{
				output.WriteRawTag(82);
				output.WriteString(this.Message);
			}
			if (this.ResultDisplaySeconds != 0)
			{
				output.WriteRawTag(88);
				output.WriteInt32(this.ResultDisplaySeconds);
			}
			if (this.RoomId.Length != 0)
			{
				output.WriteRawTag(122);
				output.WriteString(this.RoomId);
			}
			if (this._unknownFields != null)
			{
				this._unknownFields.WriteTo(output);
			}
		}

		// Token: 0x060002DB RID: 731 RVA: 0x0000D6D4 File Offset: 0x0000B8D4
		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (this.AppChannel.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.AppChannel);
			}
			if (this.PlayerId != 0L)
			{
				num += 1 + CodedOutputStream.ComputeInt64Size(this.PlayerId);
			}
			if (this.Nickname.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.Nickname);
			}
			if (this.Avatar.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.Avatar);
			}
			if (this.songCfg_ != null)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(this.SongCfg);
			}
			if (this.Valid)
			{
				num += 2;
			}
			if (this.Score != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.Score);
			}
			if (this.Rank.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.Rank);
			}
			if (this.SongDidFinish)
			{
				num += 2;
			}
			if (this.Message.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.Message);
			}
			if (this.ResultDisplaySeconds != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.ResultDisplaySeconds);
			}
			if (this.RoomId.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.RoomId);
			}
			if (this._unknownFields != null)
			{
				num += this._unknownFields.CalculateSize();
			}
			return num;
		}

		// Token: 0x060002DC RID: 732 RVA: 0x0000D820 File Offset: 0x0000BA20
		[DebuggerNonUserCode]
		public void MergeFrom(RoomSubmitScoreNotice other)
		{
			if (other == null)
			{
				return;
			}
			if (other.AppChannel.Length != 0)
			{
				this.AppChannel = other.AppChannel;
			}
			if (other.PlayerId != 0L)
			{
				this.PlayerId = other.PlayerId;
			}
			if (other.Nickname.Length != 0)
			{
				this.Nickname = other.Nickname;
			}
			if (other.Avatar.Length != 0)
			{
				this.Avatar = other.Avatar;
			}
			if (other.songCfg_ != null)
			{
				if (this.songCfg_ == null)
				{
					this.SongCfg = new SongCfg();
				}
				this.SongCfg.MergeFrom(other.SongCfg);
			}
			if (other.Valid)
			{
				this.Valid = other.Valid;
			}
			if (other.Score != 0)
			{
				this.Score = other.Score;
			}
			if (other.Rank.Length != 0)
			{
				this.Rank = other.Rank;
			}
			if (other.SongDidFinish)
			{
				this.SongDidFinish = other.SongDidFinish;
			}
			if (other.Message.Length != 0)
			{
				this.Message = other.Message;
			}
			if (other.ResultDisplaySeconds != 0)
			{
				this.ResultDisplaySeconds = other.ResultDisplaySeconds;
			}
			if (other.RoomId.Length != 0)
			{
				this.RoomId = other.RoomId;
			}
			this._unknownFields = UnknownFieldSet.MergeFrom(this._unknownFields, other._unknownFields);
		}

		// Token: 0x060002DD RID: 733 RVA: 0x0000D970 File Offset: 0x0000BB70
		[DebuggerNonUserCode]
		public void MergeFrom(CodedInputStream input)
		{
			uint num;
			while ((num = input.ReadTag()) != 0U)
			{
				if (num <= 48U)
				{
					if (num <= 26U)
					{
						if (num == 10U)
						{
							this.AppChannel = input.ReadString();
							continue;
						}
						if (num == 16U)
						{
							this.PlayerId = input.ReadInt64();
							continue;
						}
						if (num == 26U)
						{
							this.Nickname = input.ReadString();
							continue;
						}
					}
					else
					{
						if (num == 34U)
						{
							this.Avatar = input.ReadString();
							continue;
						}
						if (num == 42U)
						{
							if (this.songCfg_ == null)
							{
								this.SongCfg = new SongCfg();
							}
							input.ReadMessage(this.SongCfg);
							continue;
						}
						if (num == 48U)
						{
							this.Valid = input.ReadBool();
							continue;
						}
					}
				}
				else if (num <= 72U)
				{
					if (num == 56U)
					{
						this.Score = input.ReadInt32();
						continue;
					}
					if (num == 66U)
					{
						this.Rank = input.ReadString();
						continue;
					}
					if (num == 72U)
					{
						this.SongDidFinish = input.ReadBool();
						continue;
					}
				}
				else
				{
					if (num == 82U)
					{
						this.Message = input.ReadString();
						continue;
					}
					if (num == 88U)
					{
						this.ResultDisplaySeconds = input.ReadInt32();
						continue;
					}
					if (num == 122U)
					{
						this.RoomId = input.ReadString();
						continue;
					}
				}
				this._unknownFields = UnknownFieldSet.MergeFieldFrom(this._unknownFields, input);
			}
		}

		// Token: 0x04000168 RID: 360
		private static readonly MessageParser<RoomSubmitScoreNotice> _parser = new MessageParser<RoomSubmitScoreNotice>(() => new RoomSubmitScoreNotice());

		// Token: 0x04000169 RID: 361
		private UnknownFieldSet _unknownFields;

		// Token: 0x0400016A RID: 362
		public const int AppChannelFieldNumber = 1;

		// Token: 0x0400016B RID: 363
		private string appChannel_ = "";

		// Token: 0x0400016C RID: 364
		public const int PlayerIdFieldNumber = 2;

		// Token: 0x0400016D RID: 365
		private long playerId_;

		// Token: 0x0400016E RID: 366
		public const int NicknameFieldNumber = 3;

		// Token: 0x0400016F RID: 367
		private string nickname_ = "";

		// Token: 0x04000170 RID: 368
		public const int AvatarFieldNumber = 4;

		// Token: 0x04000171 RID: 369
		private string avatar_ = "";

		// Token: 0x04000172 RID: 370
		public const int SongCfgFieldNumber = 5;

		// Token: 0x04000173 RID: 371
		private SongCfg songCfg_;

		// Token: 0x04000174 RID: 372
		public const int ValidFieldNumber = 6;

		// Token: 0x04000175 RID: 373
		private bool valid_;

		// Token: 0x04000176 RID: 374
		public const int ScoreFieldNumber = 7;

		// Token: 0x04000177 RID: 375
		private int score_;

		// Token: 0x04000178 RID: 376
		public const int RankFieldNumber = 8;

		// Token: 0x04000179 RID: 377
		private string rank_ = "";

		// Token: 0x0400017A RID: 378
		public const int SongDidFinishFieldNumber = 9;

		// Token: 0x0400017B RID: 379
		private bool songDidFinish_;

		// Token: 0x0400017C RID: 380
		public const int MessageFieldNumber = 10;

		// Token: 0x0400017D RID: 381
		private string message_ = "";

		// Token: 0x0400017E RID: 382
		public const int ResultDisplaySecondsFieldNumber = 11;

		// Token: 0x0400017F RID: 383
		private int resultDisplaySeconds_;

		// Token: 0x04000180 RID: 384
		public const int RoomIdFieldNumber = 15;

		// Token: 0x04000181 RID: 385
		private string roomId_ = "";
	}
}
