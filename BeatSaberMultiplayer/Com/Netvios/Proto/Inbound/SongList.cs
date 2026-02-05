using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Com.Netvios.Proto.Inbound
{
	// Token: 0x02000044 RID: 68
	public sealed class SongList : IMessage<SongList>, IMessage, IEquatable<SongList>, IDeepCloneable<SongList>
	{
		// Token: 0x170001D8 RID: 472
		// (get) Token: 0x060005B8 RID: 1464 RVA: 0x00017F0C File Offset: 0x0001610C
		[DebuggerNonUserCode]
		public static MessageParser<SongList> Parser
		{
			get
			{
				return SongList._parser;
			}
		}

		// Token: 0x170001D9 RID: 473
		// (get) Token: 0x060005B9 RID: 1465 RVA: 0x00017F13 File Offset: 0x00016113
		[DebuggerNonUserCode]
		public static MessageDescriptor Descriptor
		{
			get
			{
				return BeatSaberInboundMessageReflection.Descriptor.MessageTypes[20];
			}
		}

		// Token: 0x170001DA RID: 474
		// (get) Token: 0x060005BA RID: 1466 RVA: 0x00017F26 File Offset: 0x00016126
		[DebuggerNonUserCode]
		MessageDescriptor IMessage.Descriptor
		{
			get
			{
				return SongList.Descriptor;
			}
		}

		// Token: 0x060005BB RID: 1467 RVA: 0x00017F2D File Offset: 0x0001612D
		[DebuggerNonUserCode]
		public SongList()
		{
		}

		// Token: 0x060005BC RID: 1468 RVA: 0x00017F40 File Offset: 0x00016140
		[DebuggerNonUserCode]
		public SongList(SongList other)
			: this()
		{
			this.pageNumber_ = other.pageNumber_;
			this.pageSize_ = other.pageSize_;
			this.queryJson_ = other.queryJson_;
			this._unknownFields = UnknownFieldSet.Clone(other._unknownFields);
		}

		// Token: 0x060005BD RID: 1469 RVA: 0x00017F7D File Offset: 0x0001617D
		[DebuggerNonUserCode]
		public SongList Clone()
		{
			return new SongList(this);
		}

		// Token: 0x170001DB RID: 475
		// (get) Token: 0x060005BE RID: 1470 RVA: 0x00017F85 File Offset: 0x00016185
		// (set) Token: 0x060005BF RID: 1471 RVA: 0x00017F8D File Offset: 0x0001618D
		[DebuggerNonUserCode]
		public int PageNumber
		{
			get
			{
				return this.pageNumber_;
			}
			set
			{
				this.pageNumber_ = value;
			}
		}

		// Token: 0x170001DC RID: 476
		// (get) Token: 0x060005C0 RID: 1472 RVA: 0x00017F96 File Offset: 0x00016196
		// (set) Token: 0x060005C1 RID: 1473 RVA: 0x00017F9E File Offset: 0x0001619E
		[DebuggerNonUserCode]
		public int PageSize
		{
			get
			{
				return this.pageSize_;
			}
			set
			{
				this.pageSize_ = value;
			}
		}

		// Token: 0x170001DD RID: 477
		// (get) Token: 0x060005C2 RID: 1474 RVA: 0x00017FA7 File Offset: 0x000161A7
		// (set) Token: 0x060005C3 RID: 1475 RVA: 0x00017FAF File Offset: 0x000161AF
		[DebuggerNonUserCode]
		public string QueryJson
		{
			get
			{
				return this.queryJson_;
			}
			set
			{
				this.queryJson_ = ProtoPreconditions.CheckNotNull<string>(value, "value");
			}
		}

		// Token: 0x060005C4 RID: 1476 RVA: 0x00017FC2 File Offset: 0x000161C2
		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return this.Equals(other as SongList);
		}

		// Token: 0x060005C5 RID: 1477 RVA: 0x00017FD0 File Offset: 0x000161D0
		[DebuggerNonUserCode]
		public bool Equals(SongList other)
		{
			return other != null && (other == this || (this.PageNumber == other.PageNumber && this.PageSize == other.PageSize && !(this.QueryJson != other.QueryJson) && object.Equals(this._unknownFields, other._unknownFields)));
		}

		// Token: 0x060005C6 RID: 1478 RVA: 0x00018030 File Offset: 0x00016230
		[DebuggerNonUserCode]
		public override int GetHashCode()
		{
			int num = 1;
			if (this.PageNumber != 0)
			{
				num ^= this.PageNumber.GetHashCode();
			}
			if (this.PageSize != 0)
			{
				num ^= this.PageSize.GetHashCode();
			}
			if (this.QueryJson.Length != 0)
			{
				num ^= this.QueryJson.GetHashCode();
			}
			if (this._unknownFields != null)
			{
				num ^= this._unknownFields.GetHashCode();
			}
			return num;
		}

		// Token: 0x060005C7 RID: 1479 RVA: 0x0000460F File Offset: 0x0000280F
		[DebuggerNonUserCode]
		public override string ToString()
		{
			return JsonFormatter.ToDiagnosticString(this);
		}

		// Token: 0x060005C8 RID: 1480 RVA: 0x000180A4 File Offset: 0x000162A4
		[DebuggerNonUserCode]
		public void WriteTo(CodedOutputStream output)
		{
			if (this.PageNumber != 0)
			{
				output.WriteRawTag(8);
				output.WriteInt32(this.PageNumber);
			}
			if (this.PageSize != 0)
			{
				output.WriteRawTag(16);
				output.WriteInt32(this.PageSize);
			}
			if (this.QueryJson.Length != 0)
			{
				output.WriteRawTag(26);
				output.WriteString(this.QueryJson);
			}
			if (this._unknownFields != null)
			{
				this._unknownFields.WriteTo(output);
			}
		}

		// Token: 0x060005C9 RID: 1481 RVA: 0x00018120 File Offset: 0x00016320
		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (this.PageNumber != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.PageNumber);
			}
			if (this.PageSize != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.PageSize);
			}
			if (this.QueryJson.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.QueryJson);
			}
			if (this._unknownFields != null)
			{
				num += this._unknownFields.CalculateSize();
			}
			return num;
		}

		// Token: 0x060005CA RID: 1482 RVA: 0x00018194 File Offset: 0x00016394
		[DebuggerNonUserCode]
		public void MergeFrom(SongList other)
		{
			if (other == null)
			{
				return;
			}
			if (other.PageNumber != 0)
			{
				this.PageNumber = other.PageNumber;
			}
			if (other.PageSize != 0)
			{
				this.PageSize = other.PageSize;
			}
			if (other.QueryJson.Length != 0)
			{
				this.QueryJson = other.QueryJson;
			}
			this._unknownFields = UnknownFieldSet.MergeFrom(this._unknownFields, other._unknownFields);
		}

		// Token: 0x060005CB RID: 1483 RVA: 0x00018200 File Offset: 0x00016400
		[DebuggerNonUserCode]
		public void MergeFrom(CodedInputStream input)
		{
			uint num;
			while ((num = input.ReadTag()) != 0U)
			{
				if (num != 8U)
				{
					if (num != 16U)
					{
						if (num != 26U)
						{
							this._unknownFields = UnknownFieldSet.MergeFieldFrom(this._unknownFields, input);
						}
						else
						{
							this.QueryJson = input.ReadString();
						}
					}
					else
					{
						this.PageSize = input.ReadInt32();
					}
				}
				else
				{
					this.PageNumber = input.ReadInt32();
				}
			}
		}

		// Token: 0x040002C5 RID: 709
		private static readonly MessageParser<SongList> _parser = new MessageParser<SongList>(() => new SongList());

		// Token: 0x040002C6 RID: 710
		private UnknownFieldSet _unknownFields;

		// Token: 0x040002C7 RID: 711
		public const int PageNumberFieldNumber = 1;

		// Token: 0x040002C8 RID: 712
		private int pageNumber_;

		// Token: 0x040002C9 RID: 713
		public const int PageSizeFieldNumber = 2;

		// Token: 0x040002CA RID: 714
		private int pageSize_;

		// Token: 0x040002CB RID: 715
		public const int QueryJsonFieldNumber = 3;

		// Token: 0x040002CC RID: 716
		private string queryJson_ = "";
	}
}
