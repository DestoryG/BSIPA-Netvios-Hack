using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Com.Netvios.Proto.Outbound
{
	// Token: 0x02000024 RID: 36
	public sealed class RoomBroadcastNotice : IMessage<RoomBroadcastNotice>, IMessage, IEquatable<RoomBroadcastNotice>, IDeepCloneable<RoomBroadcastNotice>
	{
		// Token: 0x170000F1 RID: 241
		// (get) Token: 0x060002DF RID: 735 RVA: 0x0000DAF9 File Offset: 0x0000BCF9
		[DebuggerNonUserCode]
		public static MessageParser<RoomBroadcastNotice> Parser
		{
			get
			{
				return RoomBroadcastNotice._parser;
			}
		}

		// Token: 0x170000F2 RID: 242
		// (get) Token: 0x060002E0 RID: 736 RVA: 0x0000DB00 File Offset: 0x0000BD00
		[DebuggerNonUserCode]
		public static MessageDescriptor Descriptor
		{
			get
			{
				return BeatSaberOutboundMessageReflection.Descriptor.MessageTypes[28];
			}
		}

		// Token: 0x170000F3 RID: 243
		// (get) Token: 0x060002E1 RID: 737 RVA: 0x0000DB13 File Offset: 0x0000BD13
		[DebuggerNonUserCode]
		MessageDescriptor IMessage.Descriptor
		{
			get
			{
				return RoomBroadcastNotice.Descriptor;
			}
		}

		// Token: 0x060002E2 RID: 738 RVA: 0x0000DB1A File Offset: 0x0000BD1A
		[DebuggerNonUserCode]
		public RoomBroadcastNotice()
		{
		}

		// Token: 0x060002E3 RID: 739 RVA: 0x0000DB44 File Offset: 0x0000BD44
		[DebuggerNonUserCode]
		public RoomBroadcastNotice(RoomBroadcastNotice other)
			: this()
		{
			this.roomId_ = other.roomId_;
			this.from_ = other.from_;
			this.type_ = other.type_;
			this.content_ = other.content_;
			this._unknownFields = UnknownFieldSet.Clone(other._unknownFields);
		}

		// Token: 0x060002E4 RID: 740 RVA: 0x0000DB98 File Offset: 0x0000BD98
		[DebuggerNonUserCode]
		public RoomBroadcastNotice Clone()
		{
			return new RoomBroadcastNotice(this);
		}

		// Token: 0x170000F4 RID: 244
		// (get) Token: 0x060002E5 RID: 741 RVA: 0x0000DBA0 File Offset: 0x0000BDA0
		// (set) Token: 0x060002E6 RID: 742 RVA: 0x0000DBA8 File Offset: 0x0000BDA8
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

		// Token: 0x170000F5 RID: 245
		// (get) Token: 0x060002E7 RID: 743 RVA: 0x0000DBBB File Offset: 0x0000BDBB
		// (set) Token: 0x060002E8 RID: 744 RVA: 0x0000DBC3 File Offset: 0x0000BDC3
		[DebuggerNonUserCode]
		public long From
		{
			get
			{
				return this.from_;
			}
			set
			{
				this.from_ = value;
			}
		}

		// Token: 0x170000F6 RID: 246
		// (get) Token: 0x060002E9 RID: 745 RVA: 0x0000DBCC File Offset: 0x0000BDCC
		// (set) Token: 0x060002EA RID: 746 RVA: 0x0000DBD4 File Offset: 0x0000BDD4
		[DebuggerNonUserCode]
		public string Type
		{
			get
			{
				return this.type_;
			}
			set
			{
				this.type_ = ProtoPreconditions.CheckNotNull<string>(value, "value");
			}
		}

		// Token: 0x170000F7 RID: 247
		// (get) Token: 0x060002EB RID: 747 RVA: 0x0000DBE7 File Offset: 0x0000BDE7
		// (set) Token: 0x060002EC RID: 748 RVA: 0x0000DBEF File Offset: 0x0000BDEF
		[DebuggerNonUserCode]
		public string Content
		{
			get
			{
				return this.content_;
			}
			set
			{
				this.content_ = ProtoPreconditions.CheckNotNull<string>(value, "value");
			}
		}

		// Token: 0x060002ED RID: 749 RVA: 0x0000DC02 File Offset: 0x0000BE02
		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return this.Equals(other as RoomBroadcastNotice);
		}

		// Token: 0x060002EE RID: 750 RVA: 0x0000DC10 File Offset: 0x0000BE10
		[DebuggerNonUserCode]
		public bool Equals(RoomBroadcastNotice other)
		{
			return other != null && (other == this || (!(this.RoomId != other.RoomId) && this.From == other.From && !(this.Type != other.Type) && !(this.Content != other.Content) && object.Equals(this._unknownFields, other._unknownFields)));
		}

		// Token: 0x060002EF RID: 751 RVA: 0x0000DC88 File Offset: 0x0000BE88
		[DebuggerNonUserCode]
		public override int GetHashCode()
		{
			int num = 1;
			if (this.RoomId.Length != 0)
			{
				num ^= this.RoomId.GetHashCode();
			}
			if (this.From != 0L)
			{
				num ^= this.From.GetHashCode();
			}
			if (this.Type.Length != 0)
			{
				num ^= this.Type.GetHashCode();
			}
			if (this.Content.Length != 0)
			{
				num ^= this.Content.GetHashCode();
			}
			if (this._unknownFields != null)
			{
				num ^= this._unknownFields.GetHashCode();
			}
			return num;
		}

		// Token: 0x060002F0 RID: 752 RVA: 0x0000460F File Offset: 0x0000280F
		[DebuggerNonUserCode]
		public override string ToString()
		{
			return JsonFormatter.ToDiagnosticString(this);
		}

		// Token: 0x060002F1 RID: 753 RVA: 0x0000DD18 File Offset: 0x0000BF18
		[DebuggerNonUserCode]
		public void WriteTo(CodedOutputStream output)
		{
			if (this.RoomId.Length != 0)
			{
				output.WriteRawTag(10);
				output.WriteString(this.RoomId);
			}
			if (this.From != 0L)
			{
				output.WriteRawTag(16);
				output.WriteInt64(this.From);
			}
			if (this.Type.Length != 0)
			{
				output.WriteRawTag(26);
				output.WriteString(this.Type);
			}
			if (this.Content.Length != 0)
			{
				output.WriteRawTag(34);
				output.WriteString(this.Content);
			}
			if (this._unknownFields != null)
			{
				this._unknownFields.WriteTo(output);
			}
		}

		// Token: 0x060002F2 RID: 754 RVA: 0x0000DDB8 File Offset: 0x0000BFB8
		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (this.RoomId.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.RoomId);
			}
			if (this.From != 0L)
			{
				num += 1 + CodedOutputStream.ComputeInt64Size(this.From);
			}
			if (this.Type.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.Type);
			}
			if (this.Content.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.Content);
			}
			if (this._unknownFields != null)
			{
				num += this._unknownFields.CalculateSize();
			}
			return num;
		}

		// Token: 0x060002F3 RID: 755 RVA: 0x0000DE50 File Offset: 0x0000C050
		[DebuggerNonUserCode]
		public void MergeFrom(RoomBroadcastNotice other)
		{
			if (other == null)
			{
				return;
			}
			if (other.RoomId.Length != 0)
			{
				this.RoomId = other.RoomId;
			}
			if (other.From != 0L)
			{
				this.From = other.From;
			}
			if (other.Type.Length != 0)
			{
				this.Type = other.Type;
			}
			if (other.Content.Length != 0)
			{
				this.Content = other.Content;
			}
			this._unknownFields = UnknownFieldSet.MergeFrom(this._unknownFields, other._unknownFields);
		}

		// Token: 0x060002F4 RID: 756 RVA: 0x0000DED8 File Offset: 0x0000C0D8
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
						this.RoomId = input.ReadString();
						continue;
					}
					if (num == 16U)
					{
						this.From = input.ReadInt64();
						continue;
					}
				}
				else
				{
					if (num == 26U)
					{
						this.Type = input.ReadString();
						continue;
					}
					if (num == 34U)
					{
						this.Content = input.ReadString();
						continue;
					}
				}
				this._unknownFields = UnknownFieldSet.MergeFieldFrom(this._unknownFields, input);
			}
		}

		// Token: 0x04000182 RID: 386
		private static readonly MessageParser<RoomBroadcastNotice> _parser = new MessageParser<RoomBroadcastNotice>(() => new RoomBroadcastNotice());

		// Token: 0x04000183 RID: 387
		private UnknownFieldSet _unknownFields;

		// Token: 0x04000184 RID: 388
		public const int RoomIdFieldNumber = 1;

		// Token: 0x04000185 RID: 389
		private string roomId_ = "";

		// Token: 0x04000186 RID: 390
		public const int FromFieldNumber = 2;

		// Token: 0x04000187 RID: 391
		private long from_;

		// Token: 0x04000188 RID: 392
		public const int TypeFieldNumber = 3;

		// Token: 0x04000189 RID: 393
		private string type_ = "";

		// Token: 0x0400018A RID: 394
		public const int ContentFieldNumber = 4;

		// Token: 0x0400018B RID: 395
		private string content_ = "";
	}
}
