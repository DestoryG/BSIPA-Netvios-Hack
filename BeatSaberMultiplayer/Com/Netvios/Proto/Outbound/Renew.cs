using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Com.Netvios.Proto.Outbound
{
	// Token: 0x0200000B RID: 11
	public sealed class Renew : IMessage<Renew>, IMessage, IEquatable<Renew>, IDeepCloneable<Renew>
	{
		// Token: 0x17000034 RID: 52
		// (get) Token: 0x0600008B RID: 139 RVA: 0x0000621C File Offset: 0x0000441C
		[DebuggerNonUserCode]
		public static MessageParser<Renew> Parser
		{
			get
			{
				return Renew._parser;
			}
		}

		// Token: 0x17000035 RID: 53
		// (get) Token: 0x0600008C RID: 140 RVA: 0x00006223 File Offset: 0x00004423
		[DebuggerNonUserCode]
		public static MessageDescriptor Descriptor
		{
			get
			{
				return BeatSaberOutboundMessageReflection.Descriptor.MessageTypes[3];
			}
		}

		// Token: 0x17000036 RID: 54
		// (get) Token: 0x0600008D RID: 141 RVA: 0x00006235 File Offset: 0x00004435
		[DebuggerNonUserCode]
		MessageDescriptor IMessage.Descriptor
		{
			get
			{
				return Renew.Descriptor;
			}
		}

		// Token: 0x0600008E RID: 142 RVA: 0x0000623C File Offset: 0x0000443C
		[DebuggerNonUserCode]
		public Renew()
		{
		}

		// Token: 0x0600008F RID: 143 RVA: 0x00006270 File Offset: 0x00004470
		[DebuggerNonUserCode]
		public Renew(Renew other)
			: this()
		{
			this.appChannel_ = other.appChannel_;
			this.token_ = other.token_;
			this.nickname_ = other.nickname_;
			this.avatar_ = other.avatar_;
			this.playerId_ = other.playerId_;
			this.expire_ = other.expire_;
			this._unknownFields = UnknownFieldSet.Clone(other._unknownFields);
		}

		// Token: 0x06000090 RID: 144 RVA: 0x000062DC File Offset: 0x000044DC
		[DebuggerNonUserCode]
		public Renew Clone()
		{
			return new Renew(this);
		}

		// Token: 0x17000037 RID: 55
		// (get) Token: 0x06000091 RID: 145 RVA: 0x000062E4 File Offset: 0x000044E4
		// (set) Token: 0x06000092 RID: 146 RVA: 0x000062EC File Offset: 0x000044EC
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

		// Token: 0x17000038 RID: 56
		// (get) Token: 0x06000093 RID: 147 RVA: 0x000062FF File Offset: 0x000044FF
		// (set) Token: 0x06000094 RID: 148 RVA: 0x00006307 File Offset: 0x00004507
		[DebuggerNonUserCode]
		public string Token
		{
			get
			{
				return this.token_;
			}
			set
			{
				this.token_ = ProtoPreconditions.CheckNotNull<string>(value, "value");
			}
		}

		// Token: 0x17000039 RID: 57
		// (get) Token: 0x06000095 RID: 149 RVA: 0x0000631A File Offset: 0x0000451A
		// (set) Token: 0x06000096 RID: 150 RVA: 0x00006322 File Offset: 0x00004522
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

		// Token: 0x1700003A RID: 58
		// (get) Token: 0x06000097 RID: 151 RVA: 0x00006335 File Offset: 0x00004535
		// (set) Token: 0x06000098 RID: 152 RVA: 0x0000633D File Offset: 0x0000453D
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

		// Token: 0x1700003B RID: 59
		// (get) Token: 0x06000099 RID: 153 RVA: 0x00006350 File Offset: 0x00004550
		// (set) Token: 0x0600009A RID: 154 RVA: 0x00006358 File Offset: 0x00004558
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

		// Token: 0x1700003C RID: 60
		// (get) Token: 0x0600009B RID: 155 RVA: 0x00006361 File Offset: 0x00004561
		// (set) Token: 0x0600009C RID: 156 RVA: 0x00006369 File Offset: 0x00004569
		[DebuggerNonUserCode]
		public int Expire
		{
			get
			{
				return this.expire_;
			}
			set
			{
				this.expire_ = value;
			}
		}

		// Token: 0x0600009D RID: 157 RVA: 0x00006372 File Offset: 0x00004572
		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return this.Equals(other as Renew);
		}

		// Token: 0x0600009E RID: 158 RVA: 0x00006380 File Offset: 0x00004580
		[DebuggerNonUserCode]
		public bool Equals(Renew other)
		{
			return other != null && (other == this || (!(this.AppChannel != other.AppChannel) && !(this.Token != other.Token) && !(this.Nickname != other.Nickname) && !(this.Avatar != other.Avatar) && this.PlayerId == other.PlayerId && this.Expire == other.Expire && object.Equals(this._unknownFields, other._unknownFields)));
		}

		// Token: 0x0600009F RID: 159 RVA: 0x00006420 File Offset: 0x00004620
		[DebuggerNonUserCode]
		public override int GetHashCode()
		{
			int num = 1;
			if (this.AppChannel.Length != 0)
			{
				num ^= this.AppChannel.GetHashCode();
			}
			if (this.Token.Length != 0)
			{
				num ^= this.Token.GetHashCode();
			}
			if (this.Nickname.Length != 0)
			{
				num ^= this.Nickname.GetHashCode();
			}
			if (this.Avatar.Length != 0)
			{
				num ^= this.Avatar.GetHashCode();
			}
			if (this.PlayerId != 0L)
			{
				num ^= this.PlayerId.GetHashCode();
			}
			if (this.Expire != 0)
			{
				num ^= this.Expire.GetHashCode();
			}
			if (this._unknownFields != null)
			{
				num ^= this._unknownFields.GetHashCode();
			}
			return num;
		}

		// Token: 0x060000A0 RID: 160 RVA: 0x0000460F File Offset: 0x0000280F
		[DebuggerNonUserCode]
		public override string ToString()
		{
			return JsonFormatter.ToDiagnosticString(this);
		}

		// Token: 0x060000A1 RID: 161 RVA: 0x000064E4 File Offset: 0x000046E4
		[DebuggerNonUserCode]
		public void WriteTo(CodedOutputStream output)
		{
			if (this.AppChannel.Length != 0)
			{
				output.WriteRawTag(10);
				output.WriteString(this.AppChannel);
			}
			if (this.Token.Length != 0)
			{
				output.WriteRawTag(18);
				output.WriteString(this.Token);
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
			if (this.PlayerId != 0L)
			{
				output.WriteRawTag(40);
				output.WriteInt64(this.PlayerId);
			}
			if (this.Expire != 0)
			{
				output.WriteRawTag(48);
				output.WriteInt32(this.Expire);
			}
			if (this._unknownFields != null)
			{
				this._unknownFields.WriteTo(output);
			}
		}

		// Token: 0x060000A2 RID: 162 RVA: 0x000065C4 File Offset: 0x000047C4
		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (this.AppChannel.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.AppChannel);
			}
			if (this.Token.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.Token);
			}
			if (this.Nickname.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.Nickname);
			}
			if (this.Avatar.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.Avatar);
			}
			if (this.PlayerId != 0L)
			{
				num += 1 + CodedOutputStream.ComputeInt64Size(this.PlayerId);
			}
			if (this.Expire != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.Expire);
			}
			if (this._unknownFields != null)
			{
				num += this._unknownFields.CalculateSize();
			}
			return num;
		}

		// Token: 0x060000A3 RID: 163 RVA: 0x00006690 File Offset: 0x00004890
		[DebuggerNonUserCode]
		public void MergeFrom(Renew other)
		{
			if (other == null)
			{
				return;
			}
			if (other.AppChannel.Length != 0)
			{
				this.AppChannel = other.AppChannel;
			}
			if (other.Token.Length != 0)
			{
				this.Token = other.Token;
			}
			if (other.Nickname.Length != 0)
			{
				this.Nickname = other.Nickname;
			}
			if (other.Avatar.Length != 0)
			{
				this.Avatar = other.Avatar;
			}
			if (other.PlayerId != 0L)
			{
				this.PlayerId = other.PlayerId;
			}
			if (other.Expire != 0)
			{
				this.Expire = other.Expire;
			}
			this._unknownFields = UnknownFieldSet.MergeFrom(this._unknownFields, other._unknownFields);
		}

		// Token: 0x060000A4 RID: 164 RVA: 0x00006744 File Offset: 0x00004944
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
					if (num == 18U)
					{
						this.Token = input.ReadString();
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
					if (num == 40U)
					{
						this.PlayerId = input.ReadInt64();
						continue;
					}
					if (num == 48U)
					{
						this.Expire = input.ReadInt32();
						continue;
					}
				}
				this._unknownFields = UnknownFieldSet.MergeFieldFrom(this._unknownFields, input);
			}
		}

		// Token: 0x04000065 RID: 101
		private static readonly MessageParser<Renew> _parser = new MessageParser<Renew>(() => new Renew());

		// Token: 0x04000066 RID: 102
		private UnknownFieldSet _unknownFields;

		// Token: 0x04000067 RID: 103
		public const int AppChannelFieldNumber = 1;

		// Token: 0x04000068 RID: 104
		private string appChannel_ = "";

		// Token: 0x04000069 RID: 105
		public const int TokenFieldNumber = 2;

		// Token: 0x0400006A RID: 106
		private string token_ = "";

		// Token: 0x0400006B RID: 107
		public const int NicknameFieldNumber = 3;

		// Token: 0x0400006C RID: 108
		private string nickname_ = "";

		// Token: 0x0400006D RID: 109
		public const int AvatarFieldNumber = 4;

		// Token: 0x0400006E RID: 110
		private string avatar_ = "";

		// Token: 0x0400006F RID: 111
		public const int PlayerIdFieldNumber = 5;

		// Token: 0x04000070 RID: 112
		private long playerId_;

		// Token: 0x04000071 RID: 113
		public const int ExpireFieldNumber = 6;

		// Token: 0x04000072 RID: 114
		private int expire_;
	}
}
