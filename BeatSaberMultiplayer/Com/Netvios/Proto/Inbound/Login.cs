using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Com.Netvios.Proto.Inbound
{
	// Token: 0x02000032 RID: 50
	public sealed class Login : IMessage<Login>, IMessage, IEquatable<Login>, IDeepCloneable<Login>
	{
		// Token: 0x17000167 RID: 359
		// (get) Token: 0x06000435 RID: 1077 RVA: 0x0001397D File Offset: 0x00011B7D
		[DebuggerNonUserCode]
		public static MessageParser<Login> Parser
		{
			get
			{
				return Login._parser;
			}
		}

		// Token: 0x17000168 RID: 360
		// (get) Token: 0x06000436 RID: 1078 RVA: 0x00013984 File Offset: 0x00011B84
		[DebuggerNonUserCode]
		public static MessageDescriptor Descriptor
		{
			get
			{
				return BeatSaberInboundMessageReflection.Descriptor.MessageTypes[2];
			}
		}

		// Token: 0x17000169 RID: 361
		// (get) Token: 0x06000437 RID: 1079 RVA: 0x00013996 File Offset: 0x00011B96
		[DebuggerNonUserCode]
		MessageDescriptor IMessage.Descriptor
		{
			get
			{
				return Login.Descriptor;
			}
		}

		// Token: 0x06000438 RID: 1080 RVA: 0x0001399D File Offset: 0x00011B9D
		[DebuggerNonUserCode]
		public Login()
		{
		}

		// Token: 0x06000439 RID: 1081 RVA: 0x000139D4 File Offset: 0x00011BD4
		[DebuggerNonUserCode]
		public Login(Login other)
			: this()
		{
			this.appChannel_ = other.appChannel_;
			this.token_ = other.token_;
			this.nickname_ = other.nickname_;
			this.avatar_ = other.avatar_;
			this._unknownFields = UnknownFieldSet.Clone(other._unknownFields);
		}

		// Token: 0x0600043A RID: 1082 RVA: 0x00013A28 File Offset: 0x00011C28
		[DebuggerNonUserCode]
		public Login Clone()
		{
			return new Login(this);
		}

		// Token: 0x1700016A RID: 362
		// (get) Token: 0x0600043B RID: 1083 RVA: 0x00013A30 File Offset: 0x00011C30
		// (set) Token: 0x0600043C RID: 1084 RVA: 0x00013A38 File Offset: 0x00011C38
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

		// Token: 0x1700016B RID: 363
		// (get) Token: 0x0600043D RID: 1085 RVA: 0x00013A4B File Offset: 0x00011C4B
		// (set) Token: 0x0600043E RID: 1086 RVA: 0x00013A53 File Offset: 0x00011C53
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

		// Token: 0x1700016C RID: 364
		// (get) Token: 0x0600043F RID: 1087 RVA: 0x00013A66 File Offset: 0x00011C66
		// (set) Token: 0x06000440 RID: 1088 RVA: 0x00013A6E File Offset: 0x00011C6E
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

		// Token: 0x1700016D RID: 365
		// (get) Token: 0x06000441 RID: 1089 RVA: 0x00013A81 File Offset: 0x00011C81
		// (set) Token: 0x06000442 RID: 1090 RVA: 0x00013A89 File Offset: 0x00011C89
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

		// Token: 0x06000443 RID: 1091 RVA: 0x00013A9C File Offset: 0x00011C9C
		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return this.Equals(other as Login);
		}

		// Token: 0x06000444 RID: 1092 RVA: 0x00013AAC File Offset: 0x00011CAC
		[DebuggerNonUserCode]
		public bool Equals(Login other)
		{
			return other != null && (other == this || (!(this.AppChannel != other.AppChannel) && !(this.Token != other.Token) && !(this.Nickname != other.Nickname) && !(this.Avatar != other.Avatar) && object.Equals(this._unknownFields, other._unknownFields)));
		}

		// Token: 0x06000445 RID: 1093 RVA: 0x00013B2C File Offset: 0x00011D2C
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
			if (this._unknownFields != null)
			{
				num ^= this._unknownFields.GetHashCode();
			}
			return num;
		}

		// Token: 0x06000446 RID: 1094 RVA: 0x0000460F File Offset: 0x0000280F
		[DebuggerNonUserCode]
		public override string ToString()
		{
			return JsonFormatter.ToDiagnosticString(this);
		}

		// Token: 0x06000447 RID: 1095 RVA: 0x00013BC0 File Offset: 0x00011DC0
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
			if (this._unknownFields != null)
			{
				this._unknownFields.WriteTo(output);
			}
		}

		// Token: 0x06000448 RID: 1096 RVA: 0x00013C68 File Offset: 0x00011E68
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
			if (this._unknownFields != null)
			{
				num += this._unknownFields.CalculateSize();
			}
			return num;
		}

		// Token: 0x06000449 RID: 1097 RVA: 0x00013D04 File Offset: 0x00011F04
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
			this._unknownFields = UnknownFieldSet.MergeFrom(this._unknownFields, other._unknownFields);
		}

		// Token: 0x0600044A RID: 1098 RVA: 0x00013D90 File Offset: 0x00011F90
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
				}
				this._unknownFields = UnknownFieldSet.MergeFieldFrom(this._unknownFields, input);
			}
		}

		// Token: 0x04000227 RID: 551
		private static readonly MessageParser<Login> _parser = new MessageParser<Login>(() => new Login());

		// Token: 0x04000228 RID: 552
		private UnknownFieldSet _unknownFields;

		// Token: 0x04000229 RID: 553
		public const int AppChannelFieldNumber = 1;

		// Token: 0x0400022A RID: 554
		private string appChannel_ = "";

		// Token: 0x0400022B RID: 555
		public const int TokenFieldNumber = 2;

		// Token: 0x0400022C RID: 556
		private string token_ = "";

		// Token: 0x0400022D RID: 557
		public const int NicknameFieldNumber = 3;

		// Token: 0x0400022E RID: 558
		private string nickname_ = "";

		// Token: 0x0400022F RID: 559
		public const int AvatarFieldNumber = 4;

		// Token: 0x04000230 RID: 560
		private string avatar_ = "";
	}
}
