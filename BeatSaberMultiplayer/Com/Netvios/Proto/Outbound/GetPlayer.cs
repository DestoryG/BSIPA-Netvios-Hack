using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Com.Netvios.Proto.Outbound
{
	// Token: 0x0200000D RID: 13
	public sealed class GetPlayer : IMessage<GetPlayer>, IMessage, IEquatable<GetPlayer>, IDeepCloneable<GetPlayer>
	{
		// Token: 0x17000041 RID: 65
		// (get) Token: 0x060000B7 RID: 183 RVA: 0x00006A16 File Offset: 0x00004C16
		[DebuggerNonUserCode]
		public static MessageParser<GetPlayer> Parser
		{
			get
			{
				return GetPlayer._parser;
			}
		}

		// Token: 0x17000042 RID: 66
		// (get) Token: 0x060000B8 RID: 184 RVA: 0x00006A1D File Offset: 0x00004C1D
		[DebuggerNonUserCode]
		public static MessageDescriptor Descriptor
		{
			get
			{
				return BeatSaberOutboundMessageReflection.Descriptor.MessageTypes[5];
			}
		}

		// Token: 0x17000043 RID: 67
		// (get) Token: 0x060000B9 RID: 185 RVA: 0x00006A2F File Offset: 0x00004C2F
		[DebuggerNonUserCode]
		MessageDescriptor IMessage.Descriptor
		{
			get
			{
				return GetPlayer.Descriptor;
			}
		}

		// Token: 0x060000BA RID: 186 RVA: 0x00006A36 File Offset: 0x00004C36
		[DebuggerNonUserCode]
		public GetPlayer()
		{
		}

		// Token: 0x060000BB RID: 187 RVA: 0x00006A6C File Offset: 0x00004C6C
		[DebuggerNonUserCode]
		public GetPlayer(GetPlayer other)
			: this()
		{
			this.appChannel_ = other.appChannel_;
			this.token_ = other.token_;
			this.nickname_ = other.nickname_;
			this.avatar_ = other.avatar_;
			this.playerId_ = other.playerId_;
			this._unknownFields = UnknownFieldSet.Clone(other._unknownFields);
		}

		// Token: 0x060000BC RID: 188 RVA: 0x00006ACC File Offset: 0x00004CCC
		[DebuggerNonUserCode]
		public GetPlayer Clone()
		{
			return new GetPlayer(this);
		}

		// Token: 0x17000044 RID: 68
		// (get) Token: 0x060000BD RID: 189 RVA: 0x00006AD4 File Offset: 0x00004CD4
		// (set) Token: 0x060000BE RID: 190 RVA: 0x00006ADC File Offset: 0x00004CDC
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

		// Token: 0x17000045 RID: 69
		// (get) Token: 0x060000BF RID: 191 RVA: 0x00006AEF File Offset: 0x00004CEF
		// (set) Token: 0x060000C0 RID: 192 RVA: 0x00006AF7 File Offset: 0x00004CF7
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

		// Token: 0x17000046 RID: 70
		// (get) Token: 0x060000C1 RID: 193 RVA: 0x00006B0A File Offset: 0x00004D0A
		// (set) Token: 0x060000C2 RID: 194 RVA: 0x00006B12 File Offset: 0x00004D12
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

		// Token: 0x17000047 RID: 71
		// (get) Token: 0x060000C3 RID: 195 RVA: 0x00006B25 File Offset: 0x00004D25
		// (set) Token: 0x060000C4 RID: 196 RVA: 0x00006B2D File Offset: 0x00004D2D
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

		// Token: 0x17000048 RID: 72
		// (get) Token: 0x060000C5 RID: 197 RVA: 0x00006B40 File Offset: 0x00004D40
		// (set) Token: 0x060000C6 RID: 198 RVA: 0x00006B48 File Offset: 0x00004D48
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

		// Token: 0x060000C7 RID: 199 RVA: 0x00006B51 File Offset: 0x00004D51
		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return this.Equals(other as GetPlayer);
		}

		// Token: 0x060000C8 RID: 200 RVA: 0x00006B60 File Offset: 0x00004D60
		[DebuggerNonUserCode]
		public bool Equals(GetPlayer other)
		{
			return other != null && (other == this || (!(this.AppChannel != other.AppChannel) && !(this.Token != other.Token) && !(this.Nickname != other.Nickname) && !(this.Avatar != other.Avatar) && this.PlayerId == other.PlayerId && object.Equals(this._unknownFields, other._unknownFields)));
		}

		// Token: 0x060000C9 RID: 201 RVA: 0x00006BF0 File Offset: 0x00004DF0
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

		// Token: 0x060000CA RID: 202 RVA: 0x0000460F File Offset: 0x0000280F
		[DebuggerNonUserCode]
		public override string ToString()
		{
			return JsonFormatter.ToDiagnosticString(this);
		}

		// Token: 0x060000CB RID: 203 RVA: 0x00006C9C File Offset: 0x00004E9C
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

		// Token: 0x060000CC RID: 204 RVA: 0x00006D60 File Offset: 0x00004F60
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

		// Token: 0x060000CD RID: 205 RVA: 0x00006E14 File Offset: 0x00005014
		[DebuggerNonUserCode]
		public void MergeFrom(GetPlayer other)
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

		// Token: 0x060000CE RID: 206 RVA: 0x00006EB4 File Offset: 0x000050B4
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

		// Token: 0x04000077 RID: 119
		private static readonly MessageParser<GetPlayer> _parser = new MessageParser<GetPlayer>(() => new GetPlayer());

		// Token: 0x04000078 RID: 120
		private UnknownFieldSet _unknownFields;

		// Token: 0x04000079 RID: 121
		public const int AppChannelFieldNumber = 1;

		// Token: 0x0400007A RID: 122
		private string appChannel_ = "";

		// Token: 0x0400007B RID: 123
		public const int TokenFieldNumber = 2;

		// Token: 0x0400007C RID: 124
		private string token_ = "";

		// Token: 0x0400007D RID: 125
		public const int NicknameFieldNumber = 3;

		// Token: 0x0400007E RID: 126
		private string nickname_ = "";

		// Token: 0x0400007F RID: 127
		public const int AvatarFieldNumber = 4;

		// Token: 0x04000080 RID: 128
		private string avatar_ = "";

		// Token: 0x04000081 RID: 129
		public const int PlayerIdFieldNumber = 5;

		// Token: 0x04000082 RID: 130
		private long playerId_;
	}
}
