using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Com.Netvios.Proto.Outbound
{
	// Token: 0x0200002A RID: 42
	public sealed class RoomCfg : IMessage<RoomCfg>, IMessage, IEquatable<RoomCfg>, IDeepCloneable<RoomCfg>
	{
		// Token: 0x17000121 RID: 289
		// (get) Token: 0x06000373 RID: 883 RVA: 0x0000FA84 File Offset: 0x0000DC84
		[DebuggerNonUserCode]
		public static MessageParser<RoomCfg> Parser
		{
			get
			{
				return RoomCfg._parser;
			}
		}

		// Token: 0x17000122 RID: 290
		// (get) Token: 0x06000374 RID: 884 RVA: 0x0000FA8B File Offset: 0x0000DC8B
		[DebuggerNonUserCode]
		public static MessageDescriptor Descriptor
		{
			get
			{
				return BeatSaberOutboundMessageReflection.Descriptor.MessageTypes[34];
			}
		}

		// Token: 0x17000123 RID: 291
		// (get) Token: 0x06000375 RID: 885 RVA: 0x0000FA9E File Offset: 0x0000DC9E
		[DebuggerNonUserCode]
		MessageDescriptor IMessage.Descriptor
		{
			get
			{
				return RoomCfg.Descriptor;
			}
		}

		// Token: 0x06000376 RID: 886 RVA: 0x0000FAA5 File Offset: 0x0000DCA5
		[DebuggerNonUserCode]
		public RoomCfg()
		{
		}

		// Token: 0x06000377 RID: 887 RVA: 0x0000FAB8 File Offset: 0x0000DCB8
		[DebuggerNonUserCode]
		public RoomCfg(RoomCfg other)
			: this()
		{
			this.roomName_ = other.roomName_;
			this.maxPlayers_ = other.maxPlayers_;
			this.isPrivate_ = other.isPrivate_;
			this.resultDisplaySeconds_ = other.resultDisplaySeconds_;
			this._unknownFields = UnknownFieldSet.Clone(other._unknownFields);
		}

		// Token: 0x06000378 RID: 888 RVA: 0x0000FB0C File Offset: 0x0000DD0C
		[DebuggerNonUserCode]
		public RoomCfg Clone()
		{
			return new RoomCfg(this);
		}

		// Token: 0x17000124 RID: 292
		// (get) Token: 0x06000379 RID: 889 RVA: 0x0000FB14 File Offset: 0x0000DD14
		// (set) Token: 0x0600037A RID: 890 RVA: 0x0000FB1C File Offset: 0x0000DD1C
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

		// Token: 0x17000125 RID: 293
		// (get) Token: 0x0600037B RID: 891 RVA: 0x0000FB2F File Offset: 0x0000DD2F
		// (set) Token: 0x0600037C RID: 892 RVA: 0x0000FB37 File Offset: 0x0000DD37
		[DebuggerNonUserCode]
		public int MaxPlayers
		{
			get
			{
				return this.maxPlayers_;
			}
			set
			{
				this.maxPlayers_ = value;
			}
		}

		// Token: 0x17000126 RID: 294
		// (get) Token: 0x0600037D RID: 893 RVA: 0x0000FB40 File Offset: 0x0000DD40
		// (set) Token: 0x0600037E RID: 894 RVA: 0x0000FB48 File Offset: 0x0000DD48
		[DebuggerNonUserCode]
		public bool IsPrivate
		{
			get
			{
				return this.isPrivate_;
			}
			set
			{
				this.isPrivate_ = value;
			}
		}

		// Token: 0x17000127 RID: 295
		// (get) Token: 0x0600037F RID: 895 RVA: 0x0000FB51 File Offset: 0x0000DD51
		// (set) Token: 0x06000380 RID: 896 RVA: 0x0000FB59 File Offset: 0x0000DD59
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

		// Token: 0x06000381 RID: 897 RVA: 0x0000FB62 File Offset: 0x0000DD62
		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return this.Equals(other as RoomCfg);
		}

		// Token: 0x06000382 RID: 898 RVA: 0x0000FB70 File Offset: 0x0000DD70
		[DebuggerNonUserCode]
		public bool Equals(RoomCfg other)
		{
			return other != null && (other == this || (!(this.RoomName != other.RoomName) && this.MaxPlayers == other.MaxPlayers && this.IsPrivate == other.IsPrivate && this.ResultDisplaySeconds == other.ResultDisplaySeconds && object.Equals(this._unknownFields, other._unknownFields)));
		}

		// Token: 0x06000383 RID: 899 RVA: 0x0000FBE0 File Offset: 0x0000DDE0
		[DebuggerNonUserCode]
		public override int GetHashCode()
		{
			int num = 1;
			if (this.RoomName.Length != 0)
			{
				num ^= this.RoomName.GetHashCode();
			}
			if (this.MaxPlayers != 0)
			{
				num ^= this.MaxPlayers.GetHashCode();
			}
			if (this.IsPrivate)
			{
				num ^= this.IsPrivate.GetHashCode();
			}
			if (this.ResultDisplaySeconds != 0)
			{
				num ^= this.ResultDisplaySeconds.GetHashCode();
			}
			if (this._unknownFields != null)
			{
				num ^= this._unknownFields.GetHashCode();
			}
			return num;
		}

		// Token: 0x06000384 RID: 900 RVA: 0x0000460F File Offset: 0x0000280F
		[DebuggerNonUserCode]
		public override string ToString()
		{
			return JsonFormatter.ToDiagnosticString(this);
		}

		// Token: 0x06000385 RID: 901 RVA: 0x0000FC6C File Offset: 0x0000DE6C
		[DebuggerNonUserCode]
		public void WriteTo(CodedOutputStream output)
		{
			if (this.RoomName.Length != 0)
			{
				output.WriteRawTag(10);
				output.WriteString(this.RoomName);
			}
			if (this.MaxPlayers != 0)
			{
				output.WriteRawTag(16);
				output.WriteInt32(this.MaxPlayers);
			}
			if (this.IsPrivate)
			{
				output.WriteRawTag(24);
				output.WriteBool(this.IsPrivate);
			}
			if (this.ResultDisplaySeconds != 0)
			{
				output.WriteRawTag(32);
				output.WriteInt32(this.ResultDisplaySeconds);
			}
			if (this._unknownFields != null)
			{
				this._unknownFields.WriteTo(output);
			}
		}

		// Token: 0x06000386 RID: 902 RVA: 0x0000FD04 File Offset: 0x0000DF04
		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (this.RoomName.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.RoomName);
			}
			if (this.MaxPlayers != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.MaxPlayers);
			}
			if (this.IsPrivate)
			{
				num += 2;
			}
			if (this.ResultDisplaySeconds != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.ResultDisplaySeconds);
			}
			if (this._unknownFields != null)
			{
				num += this._unknownFields.CalculateSize();
			}
			return num;
		}

		// Token: 0x06000387 RID: 903 RVA: 0x0000FD84 File Offset: 0x0000DF84
		[DebuggerNonUserCode]
		public void MergeFrom(RoomCfg other)
		{
			if (other == null)
			{
				return;
			}
			if (other.RoomName.Length != 0)
			{
				this.RoomName = other.RoomName;
			}
			if (other.MaxPlayers != 0)
			{
				this.MaxPlayers = other.MaxPlayers;
			}
			if (other.IsPrivate)
			{
				this.IsPrivate = other.IsPrivate;
			}
			if (other.ResultDisplaySeconds != 0)
			{
				this.ResultDisplaySeconds = other.ResultDisplaySeconds;
			}
			this._unknownFields = UnknownFieldSet.MergeFrom(this._unknownFields, other._unknownFields);
		}

		// Token: 0x06000388 RID: 904 RVA: 0x0000FE04 File Offset: 0x0000E004
		[DebuggerNonUserCode]
		public void MergeFrom(CodedInputStream input)
		{
			uint num;
			while ((num = input.ReadTag()) != 0U)
			{
				if (num <= 16U)
				{
					if (num == 10U)
					{
						this.RoomName = input.ReadString();
						continue;
					}
					if (num == 16U)
					{
						this.MaxPlayers = input.ReadInt32();
						continue;
					}
				}
				else
				{
					if (num == 24U)
					{
						this.IsPrivate = input.ReadBool();
						continue;
					}
					if (num == 32U)
					{
						this.ResultDisplaySeconds = input.ReadInt32();
						continue;
					}
				}
				this._unknownFields = UnknownFieldSet.MergeFieldFrom(this._unknownFields, input);
			}
		}

		// Token: 0x040001CC RID: 460
		private static readonly MessageParser<RoomCfg> _parser = new MessageParser<RoomCfg>(() => new RoomCfg());

		// Token: 0x040001CD RID: 461
		private UnknownFieldSet _unknownFields;

		// Token: 0x040001CE RID: 462
		public const int RoomNameFieldNumber = 1;

		// Token: 0x040001CF RID: 463
		private string roomName_ = "";

		// Token: 0x040001D0 RID: 464
		public const int MaxPlayersFieldNumber = 2;

		// Token: 0x040001D1 RID: 465
		private int maxPlayers_;

		// Token: 0x040001D2 RID: 466
		public const int IsPrivateFieldNumber = 3;

		// Token: 0x040001D3 RID: 467
		private bool isPrivate_;

		// Token: 0x040001D4 RID: 468
		public const int ResultDisplaySecondsFieldNumber = 4;

		// Token: 0x040001D5 RID: 469
		private int resultDisplaySeconds_;
	}
}
