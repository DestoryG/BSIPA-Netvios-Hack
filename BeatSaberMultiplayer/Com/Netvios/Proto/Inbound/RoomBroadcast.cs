using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

namespace Com.Netvios.Proto.Inbound
{
	// Token: 0x02000043 RID: 67
	public sealed class RoomBroadcast : IMessage<RoomBroadcast>, IMessage, IEquatable<RoomBroadcast>, IDeepCloneable<RoomBroadcast>
	{
		// Token: 0x170001D1 RID: 465
		// (get) Token: 0x060005A2 RID: 1442 RVA: 0x00017A88 File Offset: 0x00015C88
		[DebuggerNonUserCode]
		public static MessageParser<RoomBroadcast> Parser
		{
			get
			{
				return RoomBroadcast._parser;
			}
		}

		// Token: 0x170001D2 RID: 466
		// (get) Token: 0x060005A3 RID: 1443 RVA: 0x00017A8F File Offset: 0x00015C8F
		[DebuggerNonUserCode]
		public static MessageDescriptor Descriptor
		{
			get
			{
				return BeatSaberInboundMessageReflection.Descriptor.MessageTypes[19];
			}
		}

		// Token: 0x170001D3 RID: 467
		// (get) Token: 0x060005A4 RID: 1444 RVA: 0x00017AA2 File Offset: 0x00015CA2
		[DebuggerNonUserCode]
		MessageDescriptor IMessage.Descriptor
		{
			get
			{
				return RoomBroadcast.Descriptor;
			}
		}

		// Token: 0x060005A5 RID: 1445 RVA: 0x00017AA9 File Offset: 0x00015CA9
		[DebuggerNonUserCode]
		public RoomBroadcast()
		{
		}

		// Token: 0x060005A6 RID: 1446 RVA: 0x00017AE0 File Offset: 0x00015CE0
		[DebuggerNonUserCode]
		public RoomBroadcast(RoomBroadcast other)
			: this()
		{
			this.roomId_ = other.roomId_;
			this.type_ = other.type_;
			this.content_ = other.content_;
			this.excludePlayerIds_ = other.excludePlayerIds_.Clone();
			this._unknownFields = UnknownFieldSet.Clone(other._unknownFields);
		}

		// Token: 0x060005A7 RID: 1447 RVA: 0x00017B39 File Offset: 0x00015D39
		[DebuggerNonUserCode]
		public RoomBroadcast Clone()
		{
			return new RoomBroadcast(this);
		}

		// Token: 0x170001D4 RID: 468
		// (get) Token: 0x060005A8 RID: 1448 RVA: 0x00017B41 File Offset: 0x00015D41
		// (set) Token: 0x060005A9 RID: 1449 RVA: 0x00017B49 File Offset: 0x00015D49
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

		// Token: 0x170001D5 RID: 469
		// (get) Token: 0x060005AA RID: 1450 RVA: 0x00017B5C File Offset: 0x00015D5C
		// (set) Token: 0x060005AB RID: 1451 RVA: 0x00017B64 File Offset: 0x00015D64
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

		// Token: 0x170001D6 RID: 470
		// (get) Token: 0x060005AC RID: 1452 RVA: 0x00017B77 File Offset: 0x00015D77
		// (set) Token: 0x060005AD RID: 1453 RVA: 0x00017B7F File Offset: 0x00015D7F
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

		// Token: 0x170001D7 RID: 471
		// (get) Token: 0x060005AE RID: 1454 RVA: 0x00017B92 File Offset: 0x00015D92
		[DebuggerNonUserCode]
		public RepeatedField<long> ExcludePlayerIds
		{
			get
			{
				return this.excludePlayerIds_;
			}
		}

		// Token: 0x060005AF RID: 1455 RVA: 0x00017B9A File Offset: 0x00015D9A
		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return this.Equals(other as RoomBroadcast);
		}

		// Token: 0x060005B0 RID: 1456 RVA: 0x00017BA8 File Offset: 0x00015DA8
		[DebuggerNonUserCode]
		public bool Equals(RoomBroadcast other)
		{
			return other != null && (other == this || (!(this.RoomId != other.RoomId) && !(this.Type != other.Type) && !(this.Content != other.Content) && this.excludePlayerIds_.Equals(other.excludePlayerIds_) && object.Equals(this._unknownFields, other._unknownFields)));
		}

		// Token: 0x060005B1 RID: 1457 RVA: 0x00017C28 File Offset: 0x00015E28
		[DebuggerNonUserCode]
		public override int GetHashCode()
		{
			int num = 1;
			if (this.RoomId.Length != 0)
			{
				num ^= this.RoomId.GetHashCode();
			}
			if (this.Type.Length != 0)
			{
				num ^= this.Type.GetHashCode();
			}
			if (this.Content.Length != 0)
			{
				num ^= this.Content.GetHashCode();
			}
			num ^= this.excludePlayerIds_.GetHashCode();
			if (this._unknownFields != null)
			{
				num ^= this._unknownFields.GetHashCode();
			}
			return num;
		}

		// Token: 0x060005B2 RID: 1458 RVA: 0x0000460F File Offset: 0x0000280F
		[DebuggerNonUserCode]
		public override string ToString()
		{
			return JsonFormatter.ToDiagnosticString(this);
		}

		// Token: 0x060005B3 RID: 1459 RVA: 0x00017CB0 File Offset: 0x00015EB0
		[DebuggerNonUserCode]
		public void WriteTo(CodedOutputStream output)
		{
			if (this.RoomId.Length != 0)
			{
				output.WriteRawTag(10);
				output.WriteString(this.RoomId);
			}
			if (this.Type.Length != 0)
			{
				output.WriteRawTag(18);
				output.WriteString(this.Type);
			}
			if (this.Content.Length != 0)
			{
				output.WriteRawTag(26);
				output.WriteString(this.Content);
			}
			this.excludePlayerIds_.WriteTo(output, RoomBroadcast._repeated_excludePlayerIds_codec);
			if (this._unknownFields != null)
			{
				this._unknownFields.WriteTo(output);
			}
		}

		// Token: 0x060005B4 RID: 1460 RVA: 0x00017D48 File Offset: 0x00015F48
		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (this.RoomId.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.RoomId);
			}
			if (this.Type.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.Type);
			}
			if (this.Content.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.Content);
			}
			num += this.excludePlayerIds_.CalculateSize(RoomBroadcast._repeated_excludePlayerIds_codec);
			if (this._unknownFields != null)
			{
				num += this._unknownFields.CalculateSize();
			}
			return num;
		}

		// Token: 0x060005B5 RID: 1461 RVA: 0x00017DD8 File Offset: 0x00015FD8
		[DebuggerNonUserCode]
		public void MergeFrom(RoomBroadcast other)
		{
			if (other == null)
			{
				return;
			}
			if (other.RoomId.Length != 0)
			{
				this.RoomId = other.RoomId;
			}
			if (other.Type.Length != 0)
			{
				this.Type = other.Type;
			}
			if (other.Content.Length != 0)
			{
				this.Content = other.Content;
			}
			this.excludePlayerIds_.Add(other.excludePlayerIds_);
			this._unknownFields = UnknownFieldSet.MergeFrom(this._unknownFields, other._unknownFields);
		}

		// Token: 0x060005B6 RID: 1462 RVA: 0x00017E5C File Offset: 0x0001605C
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
						this.RoomId = input.ReadString();
						continue;
					}
					if (num == 18U)
					{
						this.Type = input.ReadString();
						continue;
					}
				}
				else
				{
					if (num == 26U)
					{
						this.Content = input.ReadString();
						continue;
					}
					if (num == 32U || num == 34U)
					{
						this.excludePlayerIds_.AddEntriesFrom(input, RoomBroadcast._repeated_excludePlayerIds_codec);
						continue;
					}
				}
				this._unknownFields = UnknownFieldSet.MergeFieldFrom(this._unknownFields, input);
			}
		}

		// Token: 0x040002BA RID: 698
		private static readonly MessageParser<RoomBroadcast> _parser = new MessageParser<RoomBroadcast>(() => new RoomBroadcast());

		// Token: 0x040002BB RID: 699
		private UnknownFieldSet _unknownFields;

		// Token: 0x040002BC RID: 700
		public const int RoomIdFieldNumber = 1;

		// Token: 0x040002BD RID: 701
		private string roomId_ = "";

		// Token: 0x040002BE RID: 702
		public const int TypeFieldNumber = 2;

		// Token: 0x040002BF RID: 703
		private string type_ = "";

		// Token: 0x040002C0 RID: 704
		public const int ContentFieldNumber = 3;

		// Token: 0x040002C1 RID: 705
		private string content_ = "";

		// Token: 0x040002C2 RID: 706
		public const int ExcludePlayerIdsFieldNumber = 4;

		// Token: 0x040002C3 RID: 707
		private static readonly FieldCodec<long> _repeated_excludePlayerIds_codec = FieldCodec.ForInt64(34U);

		// Token: 0x040002C4 RID: 708
		private readonly RepeatedField<long> excludePlayerIds_ = new RepeatedField<long>();
	}
}
