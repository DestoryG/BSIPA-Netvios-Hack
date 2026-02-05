using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Com.Netvios.Proto.Outbound
{
	// Token: 0x0200000E RID: 14
	public sealed class ModifyNickname : IMessage<ModifyNickname>, IMessage, IEquatable<ModifyNickname>, IDeepCloneable<ModifyNickname>
	{
		// Token: 0x17000049 RID: 73
		// (get) Token: 0x060000D0 RID: 208 RVA: 0x00006F64 File Offset: 0x00005164
		[DebuggerNonUserCode]
		public static MessageParser<ModifyNickname> Parser
		{
			get
			{
				return ModifyNickname._parser;
			}
		}

		// Token: 0x1700004A RID: 74
		// (get) Token: 0x060000D1 RID: 209 RVA: 0x00006F6B File Offset: 0x0000516B
		[DebuggerNonUserCode]
		public static MessageDescriptor Descriptor
		{
			get
			{
				return BeatSaberOutboundMessageReflection.Descriptor.MessageTypes[6];
			}
		}

		// Token: 0x1700004B RID: 75
		// (get) Token: 0x060000D2 RID: 210 RVA: 0x00006F7D File Offset: 0x0000517D
		[DebuggerNonUserCode]
		MessageDescriptor IMessage.Descriptor
		{
			get
			{
				return ModifyNickname.Descriptor;
			}
		}

		// Token: 0x060000D3 RID: 211 RVA: 0x00006F84 File Offset: 0x00005184
		[DebuggerNonUserCode]
		public ModifyNickname()
		{
		}

		// Token: 0x060000D4 RID: 212 RVA: 0x00006FB8 File Offset: 0x000051B8
		[DebuggerNonUserCode]
		public ModifyNickname(ModifyNickname other)
			: this()
		{
			this.appChannel_ = other.appChannel_;
			this.token_ = other.token_;
			this.nickname_ = other.nickname_;
			this.avatar_ = other.avatar_;
			this.playerId_ = other.playerId_;
			this._unknownFields = UnknownFieldSet.Clone(other._unknownFields);
		}

		// Token: 0x060000D5 RID: 213 RVA: 0x00007018 File Offset: 0x00005218
		[DebuggerNonUserCode]
		public ModifyNickname Clone()
		{
			return new ModifyNickname(this);
		}

		// Token: 0x1700004C RID: 76
		// (get) Token: 0x060000D6 RID: 214 RVA: 0x00007020 File Offset: 0x00005220
		// (set) Token: 0x060000D7 RID: 215 RVA: 0x00007028 File Offset: 0x00005228
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

		// Token: 0x1700004D RID: 77
		// (get) Token: 0x060000D8 RID: 216 RVA: 0x0000703B File Offset: 0x0000523B
		// (set) Token: 0x060000D9 RID: 217 RVA: 0x00007043 File Offset: 0x00005243
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

		// Token: 0x1700004E RID: 78
		// (get) Token: 0x060000DA RID: 218 RVA: 0x00007056 File Offset: 0x00005256
		// (set) Token: 0x060000DB RID: 219 RVA: 0x0000705E File Offset: 0x0000525E
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

		// Token: 0x1700004F RID: 79
		// (get) Token: 0x060000DC RID: 220 RVA: 0x00007071 File Offset: 0x00005271
		// (set) Token: 0x060000DD RID: 221 RVA: 0x00007079 File Offset: 0x00005279
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

		// Token: 0x17000050 RID: 80
		// (get) Token: 0x060000DE RID: 222 RVA: 0x0000708C File Offset: 0x0000528C
		// (set) Token: 0x060000DF RID: 223 RVA: 0x00007094 File Offset: 0x00005294
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

		// Token: 0x060000E0 RID: 224 RVA: 0x0000709D File Offset: 0x0000529D
		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return this.Equals(other as ModifyNickname);
		}

		// Token: 0x060000E1 RID: 225 RVA: 0x000070AC File Offset: 0x000052AC
		[DebuggerNonUserCode]
		public bool Equals(ModifyNickname other)
		{
			return other != null && (other == this || (!(this.AppChannel != other.AppChannel) && !(this.Token != other.Token) && !(this.Nickname != other.Nickname) && !(this.Avatar != other.Avatar) && this.PlayerId == other.PlayerId && object.Equals(this._unknownFields, other._unknownFields)));
		}

		// Token: 0x060000E2 RID: 226 RVA: 0x0000713C File Offset: 0x0000533C
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

		// Token: 0x060000E3 RID: 227 RVA: 0x0000460F File Offset: 0x0000280F
		[DebuggerNonUserCode]
		public override string ToString()
		{
			return JsonFormatter.ToDiagnosticString(this);
		}

		// Token: 0x060000E4 RID: 228 RVA: 0x000071E8 File Offset: 0x000053E8
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

		// Token: 0x060000E5 RID: 229 RVA: 0x000072AC File Offset: 0x000054AC
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

		// Token: 0x060000E6 RID: 230 RVA: 0x00007360 File Offset: 0x00005560
		[DebuggerNonUserCode]
		public void MergeFrom(ModifyNickname other)
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

		// Token: 0x060000E7 RID: 231 RVA: 0x00007400 File Offset: 0x00005600
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

		// Token: 0x04000083 RID: 131
		private static readonly MessageParser<ModifyNickname> _parser = new MessageParser<ModifyNickname>(() => new ModifyNickname());

		// Token: 0x04000084 RID: 132
		private UnknownFieldSet _unknownFields;

		// Token: 0x04000085 RID: 133
		public const int AppChannelFieldNumber = 1;

		// Token: 0x04000086 RID: 134
		private string appChannel_ = "";

		// Token: 0x04000087 RID: 135
		public const int TokenFieldNumber = 2;

		// Token: 0x04000088 RID: 136
		private string token_ = "";

		// Token: 0x04000089 RID: 137
		public const int NicknameFieldNumber = 3;

		// Token: 0x0400008A RID: 138
		private string nickname_ = "";

		// Token: 0x0400008B RID: 139
		public const int AvatarFieldNumber = 4;

		// Token: 0x0400008C RID: 140
		private string avatar_ = "";

		// Token: 0x0400008D RID: 141
		public const int PlayerIdFieldNumber = 5;

		// Token: 0x0400008E RID: 142
		private long playerId_;
	}
}
