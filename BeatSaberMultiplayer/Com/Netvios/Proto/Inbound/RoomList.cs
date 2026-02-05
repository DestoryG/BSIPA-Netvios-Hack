using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Com.Netvios.Proto.Inbound
{
	// Token: 0x02000037 RID: 55
	public sealed class RoomList : IMessage<RoomList>, IMessage, IEquatable<RoomList>, IDeepCloneable<RoomList>
	{
		// Token: 0x1700017B RID: 379
		// (get) Token: 0x0600048A RID: 1162 RVA: 0x000143AE File Offset: 0x000125AE
		[DebuggerNonUserCode]
		public static MessageParser<RoomList> Parser
		{
			get
			{
				return RoomList._parser;
			}
		}

		// Token: 0x1700017C RID: 380
		// (get) Token: 0x0600048B RID: 1163 RVA: 0x000143B5 File Offset: 0x000125B5
		[DebuggerNonUserCode]
		public static MessageDescriptor Descriptor
		{
			get
			{
				return BeatSaberInboundMessageReflection.Descriptor.MessageTypes[7];
			}
		}

		// Token: 0x1700017D RID: 381
		// (get) Token: 0x0600048C RID: 1164 RVA: 0x000143C7 File Offset: 0x000125C7
		[DebuggerNonUserCode]
		MessageDescriptor IMessage.Descriptor
		{
			get
			{
				return RoomList.Descriptor;
			}
		}

		// Token: 0x0600048D RID: 1165 RVA: 0x0000370C File Offset: 0x0000190C
		[DebuggerNonUserCode]
		public RoomList()
		{
		}

		// Token: 0x0600048E RID: 1166 RVA: 0x000143CE File Offset: 0x000125CE
		[DebuggerNonUserCode]
		public RoomList(RoomList other)
			: this()
		{
			this.pageNumber_ = other.pageNumber_;
			this.pageSize_ = other.pageSize_;
			this._unknownFields = UnknownFieldSet.Clone(other._unknownFields);
		}

		// Token: 0x0600048F RID: 1167 RVA: 0x000143FF File Offset: 0x000125FF
		[DebuggerNonUserCode]
		public RoomList Clone()
		{
			return new RoomList(this);
		}

		// Token: 0x1700017E RID: 382
		// (get) Token: 0x06000490 RID: 1168 RVA: 0x00014407 File Offset: 0x00012607
		// (set) Token: 0x06000491 RID: 1169 RVA: 0x0001440F File Offset: 0x0001260F
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

		// Token: 0x1700017F RID: 383
		// (get) Token: 0x06000492 RID: 1170 RVA: 0x00014418 File Offset: 0x00012618
		// (set) Token: 0x06000493 RID: 1171 RVA: 0x00014420 File Offset: 0x00012620
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

		// Token: 0x06000494 RID: 1172 RVA: 0x00014429 File Offset: 0x00012629
		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return this.Equals(other as RoomList);
		}

		// Token: 0x06000495 RID: 1173 RVA: 0x00014437 File Offset: 0x00012637
		[DebuggerNonUserCode]
		public bool Equals(RoomList other)
		{
			return other != null && (other == this || (this.PageNumber == other.PageNumber && this.PageSize == other.PageSize && object.Equals(this._unknownFields, other._unknownFields)));
		}

		// Token: 0x06000496 RID: 1174 RVA: 0x00014478 File Offset: 0x00012678
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
			if (this._unknownFields != null)
			{
				num ^= this._unknownFields.GetHashCode();
			}
			return num;
		}

		// Token: 0x06000497 RID: 1175 RVA: 0x0000460F File Offset: 0x0000280F
		[DebuggerNonUserCode]
		public override string ToString()
		{
			return JsonFormatter.ToDiagnosticString(this);
		}

		// Token: 0x06000498 RID: 1176 RVA: 0x000144D0 File Offset: 0x000126D0
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
			if (this._unknownFields != null)
			{
				this._unknownFields.WriteTo(output);
			}
		}

		// Token: 0x06000499 RID: 1177 RVA: 0x00014528 File Offset: 0x00012728
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
			if (this._unknownFields != null)
			{
				num += this._unknownFields.CalculateSize();
			}
			return num;
		}

		// Token: 0x0600049A RID: 1178 RVA: 0x00014580 File Offset: 0x00012780
		[DebuggerNonUserCode]
		public void MergeFrom(RoomList other)
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
			this._unknownFields = UnknownFieldSet.MergeFrom(this._unknownFields, other._unknownFields);
		}

		// Token: 0x0600049B RID: 1179 RVA: 0x000145D0 File Offset: 0x000127D0
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
						this._unknownFields = UnknownFieldSet.MergeFieldFrom(this._unknownFields, input);
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

		// Token: 0x0400023B RID: 571
		private static readonly MessageParser<RoomList> _parser = new MessageParser<RoomList>(() => new RoomList());

		// Token: 0x0400023C RID: 572
		private UnknownFieldSet _unknownFields;

		// Token: 0x0400023D RID: 573
		public const int PageNumberFieldNumber = 1;

		// Token: 0x0400023E RID: 574
		private int pageNumber_;

		// Token: 0x0400023F RID: 575
		public const int PageSizeFieldNumber = 2;

		// Token: 0x04000240 RID: 576
		private int pageSize_;
	}
}
