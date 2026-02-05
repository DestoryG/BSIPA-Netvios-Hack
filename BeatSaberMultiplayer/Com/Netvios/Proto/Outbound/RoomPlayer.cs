using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Com.Netvios.Proto.Outbound
{
	// Token: 0x02000027 RID: 39
	public sealed class RoomPlayer : IMessage<RoomPlayer>, IMessage, IEquatable<RoomPlayer>, IDeepCloneable<RoomPlayer>
	{
		// Token: 0x17000108 RID: 264
		// (get) Token: 0x06000327 RID: 807 RVA: 0x0000EA41 File Offset: 0x0000CC41
		[DebuggerNonUserCode]
		public static MessageParser<RoomPlayer> Parser
		{
			get
			{
				return RoomPlayer._parser;
			}
		}

		// Token: 0x17000109 RID: 265
		// (get) Token: 0x06000328 RID: 808 RVA: 0x0000EA48 File Offset: 0x0000CC48
		[DebuggerNonUserCode]
		public static MessageDescriptor Descriptor
		{
			get
			{
				return BeatSaberOutboundMessageReflection.Descriptor.MessageTypes[31];
			}
		}

		// Token: 0x1700010A RID: 266
		// (get) Token: 0x06000329 RID: 809 RVA: 0x0000EA5B File Offset: 0x0000CC5B
		[DebuggerNonUserCode]
		MessageDescriptor IMessage.Descriptor
		{
			get
			{
				return RoomPlayer.Descriptor;
			}
		}

		// Token: 0x0600032A RID: 810 RVA: 0x0000EA62 File Offset: 0x0000CC62
		[DebuggerNonUserCode]
		public RoomPlayer()
		{
		}

		// Token: 0x0600032B RID: 811 RVA: 0x0000EA98 File Offset: 0x0000CC98
		[DebuggerNonUserCode]
		public RoomPlayer(RoomPlayer other)
			: this()
		{
			this.appChannel_ = other.appChannel_;
			this.playerId_ = other.playerId_;
			this.nickname_ = other.nickname_;
			this.avatar_ = other.avatar_;
			this.score_ = other.score_;
			this.personalCfg_ = ((other.personalCfg_ != null) ? other.personalCfg_.Clone() : null);
			this.status_ = other.status_;
			this._unknownFields = UnknownFieldSet.Clone(other._unknownFields);
		}

		// Token: 0x0600032C RID: 812 RVA: 0x0000EB20 File Offset: 0x0000CD20
		[DebuggerNonUserCode]
		public RoomPlayer Clone()
		{
			return new RoomPlayer(this);
		}

		// Token: 0x1700010B RID: 267
		// (get) Token: 0x0600032D RID: 813 RVA: 0x0000EB28 File Offset: 0x0000CD28
		// (set) Token: 0x0600032E RID: 814 RVA: 0x0000EB30 File Offset: 0x0000CD30
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

		// Token: 0x1700010C RID: 268
		// (get) Token: 0x0600032F RID: 815 RVA: 0x0000EB43 File Offset: 0x0000CD43
		// (set) Token: 0x06000330 RID: 816 RVA: 0x0000EB4B File Offset: 0x0000CD4B
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

		// Token: 0x1700010D RID: 269
		// (get) Token: 0x06000331 RID: 817 RVA: 0x0000EB54 File Offset: 0x0000CD54
		// (set) Token: 0x06000332 RID: 818 RVA: 0x0000EB5C File Offset: 0x0000CD5C
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

		// Token: 0x1700010E RID: 270
		// (get) Token: 0x06000333 RID: 819 RVA: 0x0000EB6F File Offset: 0x0000CD6F
		// (set) Token: 0x06000334 RID: 820 RVA: 0x0000EB77 File Offset: 0x0000CD77
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

		// Token: 0x1700010F RID: 271
		// (get) Token: 0x06000335 RID: 821 RVA: 0x0000EB8A File Offset: 0x0000CD8A
		// (set) Token: 0x06000336 RID: 822 RVA: 0x0000EB92 File Offset: 0x0000CD92
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

		// Token: 0x17000110 RID: 272
		// (get) Token: 0x06000337 RID: 823 RVA: 0x0000EB9B File Offset: 0x0000CD9B
		// (set) Token: 0x06000338 RID: 824 RVA: 0x0000EBA3 File Offset: 0x0000CDA3
		[DebuggerNonUserCode]
		public PersonalCfg PersonalCfg
		{
			get
			{
				return this.personalCfg_;
			}
			set
			{
				this.personalCfg_ = value;
			}
		}

		// Token: 0x17000111 RID: 273
		// (get) Token: 0x06000339 RID: 825 RVA: 0x0000EBAC File Offset: 0x0000CDAC
		// (set) Token: 0x0600033A RID: 826 RVA: 0x0000EBB4 File Offset: 0x0000CDB4
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

		// Token: 0x0600033B RID: 827 RVA: 0x0000EBC7 File Offset: 0x0000CDC7
		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return this.Equals(other as RoomPlayer);
		}

		// Token: 0x0600033C RID: 828 RVA: 0x0000EBD8 File Offset: 0x0000CDD8
		[DebuggerNonUserCode]
		public bool Equals(RoomPlayer other)
		{
			return other != null && (other == this || (!(this.AppChannel != other.AppChannel) && this.PlayerId == other.PlayerId && !(this.Nickname != other.Nickname) && !(this.Avatar != other.Avatar) && this.Score == other.Score && object.Equals(this.PersonalCfg, other.PersonalCfg) && !(this.Status != other.Status) && object.Equals(this._unknownFields, other._unknownFields)));
		}

		// Token: 0x0600033D RID: 829 RVA: 0x0000EC8C File Offset: 0x0000CE8C
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
			if (this.Score != 0)
			{
				num ^= this.Score.GetHashCode();
			}
			if (this.personalCfg_ != null)
			{
				num ^= this.PersonalCfg.GetHashCode();
			}
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

		// Token: 0x0600033E RID: 830 RVA: 0x0000460F File Offset: 0x0000280F
		[DebuggerNonUserCode]
		public override string ToString()
		{
			return JsonFormatter.ToDiagnosticString(this);
		}

		// Token: 0x0600033F RID: 831 RVA: 0x0000ED68 File Offset: 0x0000CF68
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
			if (this.Score != 0)
			{
				output.WriteRawTag(64);
				output.WriteInt32(this.Score);
			}
			if (this.personalCfg_ != null)
			{
				output.WriteRawTag(82);
				output.WriteMessage(this.PersonalCfg);
			}
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

		// Token: 0x06000340 RID: 832 RVA: 0x0000EE64 File Offset: 0x0000D064
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
			if (this.Score != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.Score);
			}
			if (this.personalCfg_ != null)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(this.PersonalCfg);
			}
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

		// Token: 0x06000341 RID: 833 RVA: 0x0000EF48 File Offset: 0x0000D148
		[DebuggerNonUserCode]
		public void MergeFrom(RoomPlayer other)
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
			if (other.Score != 0)
			{
				this.Score = other.Score;
			}
			if (other.personalCfg_ != null)
			{
				if (this.personalCfg_ == null)
				{
					this.PersonalCfg = new PersonalCfg();
				}
				this.PersonalCfg.MergeFrom(other.PersonalCfg);
			}
			if (other.Status.Length != 0)
			{
				this.Status = other.Status;
			}
			this._unknownFields = UnknownFieldSet.MergeFrom(this._unknownFields, other._unknownFields);
		}

		// Token: 0x06000342 RID: 834 RVA: 0x0000F028 File Offset: 0x0000D228
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
				else if (num <= 64U)
				{
					if (num == 34U)
					{
						this.Avatar = input.ReadString();
						continue;
					}
					if (num == 64U)
					{
						this.Score = input.ReadInt32();
						continue;
					}
				}
				else
				{
					if (num == 82U)
					{
						if (this.personalCfg_ == null)
						{
							this.PersonalCfg = new PersonalCfg();
						}
						input.ReadMessage(this.PersonalCfg);
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

		// Token: 0x040001A5 RID: 421
		private static readonly MessageParser<RoomPlayer> _parser = new MessageParser<RoomPlayer>(() => new RoomPlayer());

		// Token: 0x040001A6 RID: 422
		private UnknownFieldSet _unknownFields;

		// Token: 0x040001A7 RID: 423
		public const int AppChannelFieldNumber = 1;

		// Token: 0x040001A8 RID: 424
		private string appChannel_ = "";

		// Token: 0x040001A9 RID: 425
		public const int PlayerIdFieldNumber = 2;

		// Token: 0x040001AA RID: 426
		private long playerId_;

		// Token: 0x040001AB RID: 427
		public const int NicknameFieldNumber = 3;

		// Token: 0x040001AC RID: 428
		private string nickname_ = "";

		// Token: 0x040001AD RID: 429
		public const int AvatarFieldNumber = 4;

		// Token: 0x040001AE RID: 430
		private string avatar_ = "";

		// Token: 0x040001AF RID: 431
		public const int ScoreFieldNumber = 8;

		// Token: 0x040001B0 RID: 432
		private int score_;

		// Token: 0x040001B1 RID: 433
		public const int PersonalCfgFieldNumber = 10;

		// Token: 0x040001B2 RID: 434
		private PersonalCfg personalCfg_;

		// Token: 0x040001B3 RID: 435
		public const int StatusFieldNumber = 12;

		// Token: 0x040001B4 RID: 436
		private string status_ = "";
	}
}
