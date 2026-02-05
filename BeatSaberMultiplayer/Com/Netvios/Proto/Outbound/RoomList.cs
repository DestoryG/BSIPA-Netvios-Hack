using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

namespace Com.Netvios.Proto.Outbound
{
	// Token: 0x0200000F RID: 15
	public sealed class RoomList : IMessage<RoomList>, IMessage, IEquatable<RoomList>, IDeepCloneable<RoomList>
	{
		// Token: 0x17000051 RID: 81
		// (get) Token: 0x060000E9 RID: 233 RVA: 0x000074B0 File Offset: 0x000056B0
		[DebuggerNonUserCode]
		public static MessageParser<RoomList> Parser
		{
			get
			{
				return RoomList._parser;
			}
		}

		// Token: 0x17000052 RID: 82
		// (get) Token: 0x060000EA RID: 234 RVA: 0x000074B7 File Offset: 0x000056B7
		[DebuggerNonUserCode]
		public static MessageDescriptor Descriptor
		{
			get
			{
				return BeatSaberOutboundMessageReflection.Descriptor.MessageTypes[7];
			}
		}

		// Token: 0x17000053 RID: 83
		// (get) Token: 0x060000EB RID: 235 RVA: 0x000074C9 File Offset: 0x000056C9
		[DebuggerNonUserCode]
		MessageDescriptor IMessage.Descriptor
		{
			get
			{
				return RoomList.Descriptor;
			}
		}

		// Token: 0x060000EC RID: 236 RVA: 0x000074D0 File Offset: 0x000056D0
		[DebuggerNonUserCode]
		public RoomList()
		{
		}

		// Token: 0x060000ED RID: 237 RVA: 0x000074E4 File Offset: 0x000056E4
		[DebuggerNonUserCode]
		public RoomList(RoomList other)
			: this()
		{
			this.pageNumber_ = other.pageNumber_;
			this.pageSize_ = other.pageSize_;
			this.size_ = other.size_;
			this.count_ = other.count_;
			this.rooms_ = other.rooms_.Clone();
			this._unknownFields = UnknownFieldSet.Clone(other._unknownFields);
		}

		// Token: 0x060000EE RID: 238 RVA: 0x00007549 File Offset: 0x00005749
		[DebuggerNonUserCode]
		public RoomList Clone()
		{
			return new RoomList(this);
		}

		// Token: 0x17000054 RID: 84
		// (get) Token: 0x060000EF RID: 239 RVA: 0x00007551 File Offset: 0x00005751
		// (set) Token: 0x060000F0 RID: 240 RVA: 0x00007559 File Offset: 0x00005759
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

		// Token: 0x17000055 RID: 85
		// (get) Token: 0x060000F1 RID: 241 RVA: 0x00007562 File Offset: 0x00005762
		// (set) Token: 0x060000F2 RID: 242 RVA: 0x0000756A File Offset: 0x0000576A
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

		// Token: 0x17000056 RID: 86
		// (get) Token: 0x060000F3 RID: 243 RVA: 0x00007573 File Offset: 0x00005773
		// (set) Token: 0x060000F4 RID: 244 RVA: 0x0000757B File Offset: 0x0000577B
		[DebuggerNonUserCode]
		public int Size
		{
			get
			{
				return this.size_;
			}
			set
			{
				this.size_ = value;
			}
		}

		// Token: 0x17000057 RID: 87
		// (get) Token: 0x060000F5 RID: 245 RVA: 0x00007584 File Offset: 0x00005784
		// (set) Token: 0x060000F6 RID: 246 RVA: 0x0000758C File Offset: 0x0000578C
		[DebuggerNonUserCode]
		public int Count
		{
			get
			{
				return this.count_;
			}
			set
			{
				this.count_ = value;
			}
		}

		// Token: 0x17000058 RID: 88
		// (get) Token: 0x060000F7 RID: 247 RVA: 0x00007595 File Offset: 0x00005795
		[DebuggerNonUserCode]
		public RepeatedField<Room> Rooms
		{
			get
			{
				return this.rooms_;
			}
		}

		// Token: 0x060000F8 RID: 248 RVA: 0x0000759D File Offset: 0x0000579D
		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return this.Equals(other as RoomList);
		}

		// Token: 0x060000F9 RID: 249 RVA: 0x000075AC File Offset: 0x000057AC
		[DebuggerNonUserCode]
		public bool Equals(RoomList other)
		{
			return other != null && (other == this || (this.PageNumber == other.PageNumber && this.PageSize == other.PageSize && this.Size == other.Size && this.Count == other.Count && this.rooms_.Equals(other.rooms_) && object.Equals(this._unknownFields, other._unknownFields)));
		}

		// Token: 0x060000FA RID: 250 RVA: 0x0000762C File Offset: 0x0000582C
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
			if (this.Size != 0)
			{
				num ^= this.Size.GetHashCode();
			}
			if (this.Count != 0)
			{
				num ^= this.Count.GetHashCode();
			}
			num ^= this.rooms_.GetHashCode();
			if (this._unknownFields != null)
			{
				num ^= this._unknownFields.GetHashCode();
			}
			return num;
		}

		// Token: 0x060000FB RID: 251 RVA: 0x0000460F File Offset: 0x0000280F
		[DebuggerNonUserCode]
		public override string ToString()
		{
			return JsonFormatter.ToDiagnosticString(this);
		}

		// Token: 0x060000FC RID: 252 RVA: 0x000076C4 File Offset: 0x000058C4
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
			if (this.Size != 0)
			{
				output.WriteRawTag(24);
				output.WriteInt32(this.Size);
			}
			if (this.Count != 0)
			{
				output.WriteRawTag(32);
				output.WriteInt32(this.Count);
			}
			this.rooms_.WriteTo(output, RoomList._repeated_rooms_codec);
			if (this._unknownFields != null)
			{
				this._unknownFields.WriteTo(output);
			}
		}

		// Token: 0x060000FD RID: 253 RVA: 0x00007768 File Offset: 0x00005968
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
			if (this.Size != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.Size);
			}
			if (this.Count != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.Count);
			}
			num += this.rooms_.CalculateSize(RoomList._repeated_rooms_codec);
			if (this._unknownFields != null)
			{
				num += this._unknownFields.CalculateSize();
			}
			return num;
		}

		// Token: 0x060000FE RID: 254 RVA: 0x00007804 File Offset: 0x00005A04
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
			if (other.Size != 0)
			{
				this.Size = other.Size;
			}
			if (other.Count != 0)
			{
				this.Count = other.Count;
			}
			this.rooms_.Add(other.rooms_);
			this._unknownFields = UnknownFieldSet.MergeFrom(this._unknownFields, other._unknownFields);
		}

		// Token: 0x060000FF RID: 255 RVA: 0x00007890 File Offset: 0x00005A90
		[DebuggerNonUserCode]
		public void MergeFrom(CodedInputStream input)
		{
			uint num;
			while ((num = input.ReadTag()) != 0U)
			{
				if (num <= 16U)
				{
					if (num == 8U)
					{
						this.PageNumber = input.ReadInt32();
						continue;
					}
					if (num == 16U)
					{
						this.PageSize = input.ReadInt32();
						continue;
					}
				}
				else
				{
					if (num == 24U)
					{
						this.Size = input.ReadInt32();
						continue;
					}
					if (num == 32U)
					{
						this.Count = input.ReadInt32();
						continue;
					}
					if (num == 82U)
					{
						this.rooms_.AddEntriesFrom(input, RoomList._repeated_rooms_codec);
						continue;
					}
				}
				this._unknownFields = UnknownFieldSet.MergeFieldFrom(this._unknownFields, input);
			}
		}

		// Token: 0x0400008F RID: 143
		private static readonly MessageParser<RoomList> _parser = new MessageParser<RoomList>(() => new RoomList());

		// Token: 0x04000090 RID: 144
		private UnknownFieldSet _unknownFields;

		// Token: 0x04000091 RID: 145
		public const int PageNumberFieldNumber = 1;

		// Token: 0x04000092 RID: 146
		private int pageNumber_;

		// Token: 0x04000093 RID: 147
		public const int PageSizeFieldNumber = 2;

		// Token: 0x04000094 RID: 148
		private int pageSize_;

		// Token: 0x04000095 RID: 149
		public const int SizeFieldNumber = 3;

		// Token: 0x04000096 RID: 150
		private int size_;

		// Token: 0x04000097 RID: 151
		public const int CountFieldNumber = 4;

		// Token: 0x04000098 RID: 152
		private int count_;

		// Token: 0x04000099 RID: 153
		public const int RoomsFieldNumber = 10;

		// Token: 0x0400009A RID: 154
		private static readonly FieldCodec<Room> _repeated_rooms_codec = FieldCodec.ForMessage<Room>(82U, Room.Parser);

		// Token: 0x0400009B RID: 155
		private readonly RepeatedField<Room> rooms_ = new RepeatedField<Room>();
	}
}
