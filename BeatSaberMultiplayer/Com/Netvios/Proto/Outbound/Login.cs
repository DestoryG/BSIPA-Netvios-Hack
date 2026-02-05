using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Com.Netvios.Proto.Outbound
{
	// Token: 0x0200000A RID: 10
	public sealed class Login : IMessage<Login>, IMessage, IEquatable<Login>, IDeepCloneable<Login>
	{
		// Token: 0x1700002C RID: 44
		// (get) Token: 0x06000072 RID: 114 RVA: 0x00005CCD File Offset: 0x00003ECD
		[DebuggerNonUserCode]
		public static MessageParser<Login> Parser
		{
			get
			{
				return Login._parser;
			}
		}

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x06000073 RID: 115 RVA: 0x00005CD4 File Offset: 0x00003ED4
		[DebuggerNonUserCode]
		public static MessageDescriptor Descriptor
		{
			get
			{
				return BeatSaberOutboundMessageReflection.Descriptor.MessageTypes[2];
			}
		}

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x06000074 RID: 116 RVA: 0x00005CE6 File Offset: 0x00003EE6
		[DebuggerNonUserCode]
		MessageDescriptor IMessage.Descriptor
		{
			get
			{
				return Login.Descriptor;
			}
		}

		// Token: 0x06000075 RID: 117 RVA: 0x00005CED File Offset: 0x00003EED
		[DebuggerNonUserCode]
		public Login()
		{
		}

		// Token: 0x06000076 RID: 118 RVA: 0x00005D24 File Offset: 0x00003F24
		[DebuggerNonUserCode]
		public Login(Login other)
			: this()
		{
			this.appChannel_ = other.appChannel_;
			this.token_ = other.token_;
			this.nickname_ = other.nickname_;
			this.avatar_ = other.avatar_;
			this.playerId_ = other.playerId_;
			this._unknownFields = UnknownFieldSet.Clone(other._unknownFields);
		}

		// Token: 0x06000077 RID: 119 RVA: 0x00005D84 File Offset: 0x00003F84
		[DebuggerNonUserCode]
		public Login Clone()
		{
			return new Login(this);
		}

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x06000078 RID: 120 RVA: 0x00005D8C File Offset: 0x00003F8C
		// (set) Token: 0x06000079 RID: 121 RVA: 0x00005D94 File Offset: 0x00003F94
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

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x0600007A RID: 122 RVA: 0x00005DA7 File Offset: 0x00003FA7
		// (set) Token: 0x0600007B RID: 123 RVA: 0x00005DAF File Offset: 0x00003FAF
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

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x0600007C RID: 124 RVA: 0x00005DC2 File Offset: 0x00003FC2
		// (set) Token: 0x0600007D RID: 125 RVA: 0x00005DCA File Offset: 0x00003FCA
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

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x0600007E RID: 126 RVA: 0x00005DDD File Offset: 0x00003FDD
		// (set) Token: 0x0600007F RID: 127 RVA: 0x00005DE5 File Offset: 0x00003FE5
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

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x06000080 RID: 128 RVA: 0x00005DF8 File Offset: 0x00003FF8
		// (set) Token: 0x06000081 RID: 129 RVA: 0x00005E00 File Offset: 0x00004000
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

		// Token: 0x06000082 RID: 130 RVA: 0x00005E09 File Offset: 0x00004009
		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return this.Equals(other as Login);
		}

		// Token: 0x06000083 RID: 131 RVA: 0x00005E18 File Offset: 0x00004018
		[DebuggerNonUserCode]
		public bool Equals(Login other)
		{
			return other != null && (other == this || (!(this.AppChannel != other.AppChannel) && !(this.Token != other.Token) && !(this.Nickname != other.Nickname) && !(this.Avatar != other.Avatar) && this.PlayerId == other.PlayerId && object.Equals(this._unknownFields, other._unknownFields)));
		}

		// Token: 0x06000084 RID: 132 RVA: 0x00005EA8 File Offset: 0x000040A8
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
			if (this._unknownFields != null)
			{
				num ^= this._unknownFields.GetHashCode();
			}
			return num;
		}

		// Token: 0x06000085 RID: 133 RVA: 0x0000460F File Offset: 0x0000280F
		[DebuggerNonUserCode]
		public override string ToString()
		{
			return JsonFormatter.ToDiagnosticString(this);
		}

		// Token: 0x06000086 RID: 134 RVA: 0x00005F54 File Offset: 0x00004154
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
			if (this._unknownFields != null)
			{
				this._unknownFields.WriteTo(output);
			}
		}

		// Token: 0x06000087 RID: 135 RVA: 0x00006018 File Offset: 0x00004218
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
			if (this._unknownFields != null)
			{
				num += this._unknownFields.CalculateSize();
			}
			return num;
		}

		// Token: 0x06000088 RID: 136 RVA: 0x000060CC File Offset: 0x000042CC
		[DebuggerNonUserCode]
		public void MergeFrom(Login other)
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
			this._unknownFields = UnknownFieldSet.MergeFrom(this._unknownFields, other._unknownFields);
		}

		// Token: 0x06000089 RID: 137 RVA: 0x0000616C File Offset: 0x0000436C
		[DebuggerNonUserCode]
		public void MergeFrom(CodedInputStream input)
		{
			uint num;
			while ((num = input.ReadTag()) != 0U)
			{
				if (num <= 18U)
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
				}
				else
				{
					if (num == 26U)
					{
						this.Nickname = input.ReadString();
						continue;
					}
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
				}
				this._unknownFields = UnknownFieldSet.MergeFieldFrom(this._unknownFields, input);
			}
		}

		// Token: 0x04000059 RID: 89
		private static readonly MessageParser<Login> _parser = new MessageParser<Login>(() => new Login());

		// Token: 0x0400005A RID: 90
		private UnknownFieldSet _unknownFields;

		// Token: 0x0400005B RID: 91
		public const int AppChannelFieldNumber = 1;

		// Token: 0x0400005C RID: 92
		private string appChannel_ = "";

		// Token: 0x0400005D RID: 93
		public const int TokenFieldNumber = 2;

		// Token: 0x0400005E RID: 94
		private string token_ = "";

		// Token: 0x0400005F RID: 95
		public const int NicknameFieldNumber = 3;

		// Token: 0x04000060 RID: 96
		private string nickname_ = "";

		// Token: 0x04000061 RID: 97
		public const int AvatarFieldNumber = 4;

		// Token: 0x04000062 RID: 98
		private string avatar_ = "";

		// Token: 0x04000063 RID: 99
		public const int PlayerIdFieldNumber = 5;

		// Token: 0x04000064 RID: 100
		private long playerId_;
	}
}
