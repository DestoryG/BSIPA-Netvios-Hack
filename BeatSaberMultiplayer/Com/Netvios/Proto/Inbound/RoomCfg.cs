using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Com.Netvios.Proto.Inbound
{
	// Token: 0x02000048 RID: 72
	public sealed class RoomCfg : IMessage<RoomCfg>, IMessage, IEquatable<RoomCfg>, IDeepCloneable<RoomCfg>
	{
		// Token: 0x170001EB RID: 491
		// (get) Token: 0x06000602 RID: 1538 RVA: 0x0001890C File Offset: 0x00016B0C
		[DebuggerNonUserCode]
		public static MessageParser<RoomCfg> Parser
		{
			get
			{
				return RoomCfg._parser;
			}
		}

		// Token: 0x170001EC RID: 492
		// (get) Token: 0x06000603 RID: 1539 RVA: 0x00018913 File Offset: 0x00016B13
		[DebuggerNonUserCode]
		public static MessageDescriptor Descriptor
		{
			get
			{
				return BeatSaberInboundMessageReflection.Descriptor.MessageTypes[24];
			}
		}

		// Token: 0x170001ED RID: 493
		// (get) Token: 0x06000604 RID: 1540 RVA: 0x00018926 File Offset: 0x00016B26
		[DebuggerNonUserCode]
		MessageDescriptor IMessage.Descriptor
		{
			get
			{
				return RoomCfg.Descriptor;
			}
		}

		// Token: 0x06000605 RID: 1541 RVA: 0x0001892D File Offset: 0x00016B2D
		[DebuggerNonUserCode]
		public RoomCfg()
		{
		}

		// Token: 0x06000606 RID: 1542 RVA: 0x0001894C File Offset: 0x00016B4C
		[DebuggerNonUserCode]
		public RoomCfg(RoomCfg other)
			: this()
		{
			this.roomName_ = other.roomName_;
			this.maxPlayers_ = other.maxPlayers_;
			this.password_ = other.password_;
			this.resultDisplaySeconds_ = other.resultDisplaySeconds_;
			this._unknownFields = UnknownFieldSet.Clone(other._unknownFields);
		}

		// Token: 0x06000607 RID: 1543 RVA: 0x000189A0 File Offset: 0x00016BA0
		[DebuggerNonUserCode]
		public RoomCfg Clone()
		{
			return new RoomCfg(this);
		}

		// Token: 0x170001EE RID: 494
		// (get) Token: 0x06000608 RID: 1544 RVA: 0x000189A8 File Offset: 0x00016BA8
		// (set) Token: 0x06000609 RID: 1545 RVA: 0x000189B0 File Offset: 0x00016BB0
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

		// Token: 0x170001EF RID: 495
		// (get) Token: 0x0600060A RID: 1546 RVA: 0x000189C3 File Offset: 0x00016BC3
		// (set) Token: 0x0600060B RID: 1547 RVA: 0x000189CB File Offset: 0x00016BCB
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

		// Token: 0x170001F0 RID: 496
		// (get) Token: 0x0600060C RID: 1548 RVA: 0x000189D4 File Offset: 0x00016BD4
		// (set) Token: 0x0600060D RID: 1549 RVA: 0x000189DC File Offset: 0x00016BDC
		[DebuggerNonUserCode]
		public string Password
		{
			get
			{
				return this.password_;
			}
			set
			{
				this.password_ = ProtoPreconditions.CheckNotNull<string>(value, "value");
			}
		}

		// Token: 0x170001F1 RID: 497
		// (get) Token: 0x0600060E RID: 1550 RVA: 0x000189EF File Offset: 0x00016BEF
		// (set) Token: 0x0600060F RID: 1551 RVA: 0x000189F7 File Offset: 0x00016BF7
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

		// Token: 0x06000610 RID: 1552 RVA: 0x00018A00 File Offset: 0x00016C00
		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return this.Equals(other as RoomCfg);
		}

		// Token: 0x06000611 RID: 1553 RVA: 0x00018A10 File Offset: 0x00016C10
		[DebuggerNonUserCode]
		public bool Equals(RoomCfg other)
		{
			return other != null && (other == this || (!(this.RoomName != other.RoomName) && this.MaxPlayers == other.MaxPlayers && !(this.Password != other.Password) && this.ResultDisplaySeconds == other.ResultDisplaySeconds && object.Equals(this._unknownFields, other._unknownFields)));
		}

		// Token: 0x06000612 RID: 1554 RVA: 0x00018A84 File Offset: 0x00016C84
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
			if (this.Password.Length != 0)
			{
				num ^= this.Password.GetHashCode();
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

		// Token: 0x06000613 RID: 1555 RVA: 0x0000460F File Offset: 0x0000280F
		[DebuggerNonUserCode]
		public override string ToString()
		{
			return JsonFormatter.ToDiagnosticString(this);
		}

		// Token: 0x06000614 RID: 1556 RVA: 0x00018B14 File Offset: 0x00016D14
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
			if (this.Password.Length != 0)
			{
				output.WriteRawTag(26);
				output.WriteString(this.Password);
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

		// Token: 0x06000615 RID: 1557 RVA: 0x00018BB0 File Offset: 0x00016DB0
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
			if (this.Password.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.Password);
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

		// Token: 0x06000616 RID: 1558 RVA: 0x00018C40 File Offset: 0x00016E40
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
			if (other.Password.Length != 0)
			{
				this.Password = other.Password;
			}
			if (other.ResultDisplaySeconds != 0)
			{
				this.ResultDisplaySeconds = other.ResultDisplaySeconds;
			}
			this._unknownFields = UnknownFieldSet.MergeFrom(this._unknownFields, other._unknownFields);
		}

		// Token: 0x06000617 RID: 1559 RVA: 0x00018CC4 File Offset: 0x00016EC4
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
					if (num == 26U)
					{
						this.Password = input.ReadString();
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

		// Token: 0x040002DB RID: 731
		private static readonly MessageParser<RoomCfg> _parser = new MessageParser<RoomCfg>(() => new RoomCfg());

		// Token: 0x040002DC RID: 732
		private UnknownFieldSet _unknownFields;

		// Token: 0x040002DD RID: 733
		public const int RoomNameFieldNumber = 1;

		// Token: 0x040002DE RID: 734
		private string roomName_ = "";

		// Token: 0x040002DF RID: 735
		public const int MaxPlayersFieldNumber = 2;

		// Token: 0x040002E0 RID: 736
		private int maxPlayers_;

		// Token: 0x040002E1 RID: 737
		public const int PasswordFieldNumber = 3;

		// Token: 0x040002E2 RID: 738
		private string password_ = "";

		// Token: 0x040002E3 RID: 739
		public const int ResultDisplaySecondsFieldNumber = 4;

		// Token: 0x040002E4 RID: 740
		private int resultDisplaySeconds_;
	}
}
