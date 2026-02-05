using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

namespace Com.Netvios.Proto.Outbound
{
	// Token: 0x0200001C RID: 28
	public sealed class SongList : IMessage<SongList>, IMessage, IEquatable<SongList>, IDeepCloneable<SongList>
	{
		// Token: 0x170000AF RID: 175
		// (get) Token: 0x06000216 RID: 534 RVA: 0x0000B034 File Offset: 0x00009234
		[DebuggerNonUserCode]
		public static MessageParser<SongList> Parser
		{
			get
			{
				return SongList._parser;
			}
		}

		// Token: 0x170000B0 RID: 176
		// (get) Token: 0x06000217 RID: 535 RVA: 0x0000B03B File Offset: 0x0000923B
		[DebuggerNonUserCode]
		public static MessageDescriptor Descriptor
		{
			get
			{
				return BeatSaberOutboundMessageReflection.Descriptor.MessageTypes[20];
			}
		}

		// Token: 0x170000B1 RID: 177
		// (get) Token: 0x06000218 RID: 536 RVA: 0x0000B04E File Offset: 0x0000924E
		[DebuggerNonUserCode]
		MessageDescriptor IMessage.Descriptor
		{
			get
			{
				return SongList.Descriptor;
			}
		}

		// Token: 0x06000219 RID: 537 RVA: 0x0000B055 File Offset: 0x00009255
		[DebuggerNonUserCode]
		public SongList()
		{
		}

		// Token: 0x0600021A RID: 538 RVA: 0x0000B068 File Offset: 0x00009268
		[DebuggerNonUserCode]
		public SongList(SongList other)
			: this()
		{
			this.pageNumber_ = other.pageNumber_;
			this.pageSize_ = other.pageSize_;
			this.size_ = other.size_;
			this.count_ = other.count_;
			this.songs_ = other.songs_.Clone();
			this._unknownFields = UnknownFieldSet.Clone(other._unknownFields);
		}

		// Token: 0x0600021B RID: 539 RVA: 0x0000B0CD File Offset: 0x000092CD
		[DebuggerNonUserCode]
		public SongList Clone()
		{
			return new SongList(this);
		}

		// Token: 0x170000B2 RID: 178
		// (get) Token: 0x0600021C RID: 540 RVA: 0x0000B0D5 File Offset: 0x000092D5
		// (set) Token: 0x0600021D RID: 541 RVA: 0x0000B0DD File Offset: 0x000092DD
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

		// Token: 0x170000B3 RID: 179
		// (get) Token: 0x0600021E RID: 542 RVA: 0x0000B0E6 File Offset: 0x000092E6
		// (set) Token: 0x0600021F RID: 543 RVA: 0x0000B0EE File Offset: 0x000092EE
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

		// Token: 0x170000B4 RID: 180
		// (get) Token: 0x06000220 RID: 544 RVA: 0x0000B0F7 File Offset: 0x000092F7
		// (set) Token: 0x06000221 RID: 545 RVA: 0x0000B0FF File Offset: 0x000092FF
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

		// Token: 0x170000B5 RID: 181
		// (get) Token: 0x06000222 RID: 546 RVA: 0x0000B108 File Offset: 0x00009308
		// (set) Token: 0x06000223 RID: 547 RVA: 0x0000B110 File Offset: 0x00009310
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

		// Token: 0x170000B6 RID: 182
		// (get) Token: 0x06000224 RID: 548 RVA: 0x0000B119 File Offset: 0x00009319
		[DebuggerNonUserCode]
		public RepeatedField<Song> Songs
		{
			get
			{
				return this.songs_;
			}
		}

		// Token: 0x06000225 RID: 549 RVA: 0x0000B121 File Offset: 0x00009321
		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return this.Equals(other as SongList);
		}

		// Token: 0x06000226 RID: 550 RVA: 0x0000B130 File Offset: 0x00009330
		[DebuggerNonUserCode]
		public bool Equals(SongList other)
		{
			return other != null && (other == this || (this.PageNumber == other.PageNumber && this.PageSize == other.PageSize && this.Size == other.Size && this.Count == other.Count && this.songs_.Equals(other.songs_) && object.Equals(this._unknownFields, other._unknownFields)));
		}

		// Token: 0x06000227 RID: 551 RVA: 0x0000B1B0 File Offset: 0x000093B0
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
			num ^= this.songs_.GetHashCode();
			if (this._unknownFields != null)
			{
				num ^= this._unknownFields.GetHashCode();
			}
			return num;
		}

		// Token: 0x06000228 RID: 552 RVA: 0x0000460F File Offset: 0x0000280F
		[DebuggerNonUserCode]
		public override string ToString()
		{
			return JsonFormatter.ToDiagnosticString(this);
		}

		// Token: 0x06000229 RID: 553 RVA: 0x0000B248 File Offset: 0x00009448
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
			this.songs_.WriteTo(output, SongList._repeated_songs_codec);
			if (this._unknownFields != null)
			{
				this._unknownFields.WriteTo(output);
			}
		}

		// Token: 0x0600022A RID: 554 RVA: 0x0000B2EC File Offset: 0x000094EC
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
			num += this.songs_.CalculateSize(SongList._repeated_songs_codec);
			if (this._unknownFields != null)
			{
				num += this._unknownFields.CalculateSize();
			}
			return num;
		}

		// Token: 0x0600022B RID: 555 RVA: 0x0000B388 File Offset: 0x00009588
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
			if (other.Size != 0)
			{
				this.Size = other.Size;
			}
			if (other.Count != 0)
			{
				this.Count = other.Count;
			}
			this.songs_.Add(other.songs_);
			this._unknownFields = UnknownFieldSet.MergeFrom(this._unknownFields, other._unknownFields);
		}

		// Token: 0x0600022C RID: 556 RVA: 0x0000B414 File Offset: 0x00009614
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
						this.songs_.AddEntriesFrom(input, SongList._repeated_songs_codec);
						continue;
					}
				}
				this._unknownFields = UnknownFieldSet.MergeFieldFrom(this._unknownFields, input);
			}
		}

		// Token: 0x0400011B RID: 283
		private static readonly MessageParser<SongList> _parser = new MessageParser<SongList>(() => new SongList());

		// Token: 0x0400011C RID: 284
		private UnknownFieldSet _unknownFields;

		// Token: 0x0400011D RID: 285
		public const int PageNumberFieldNumber = 1;

		// Token: 0x0400011E RID: 286
		private int pageNumber_;

		// Token: 0x0400011F RID: 287
		public const int PageSizeFieldNumber = 2;

		// Token: 0x04000120 RID: 288
		private int pageSize_;

		// Token: 0x04000121 RID: 289
		public const int SizeFieldNumber = 3;

		// Token: 0x04000122 RID: 290
		private int size_;

		// Token: 0x04000123 RID: 291
		public const int CountFieldNumber = 4;

		// Token: 0x04000124 RID: 292
		private int count_;

		// Token: 0x04000125 RID: 293
		public const int SongsFieldNumber = 10;

		// Token: 0x04000126 RID: 294
		private static readonly FieldCodec<Song> _repeated_songs_codec = FieldCodec.ForMessage<Song>(82U, Song.Parser);

		// Token: 0x04000127 RID: 295
		private readonly RepeatedField<Song> songs_ = new RepeatedField<Song>();
	}
}
