using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Com.Netvios.Proto.Outbound
{
	// Token: 0x0200002C RID: 44
	public sealed class Song : IMessage<Song>, IMessage, IEquatable<Song>, IDeepCloneable<Song>
	{
		// Token: 0x17000132 RID: 306
		// (get) Token: 0x060003A7 RID: 935 RVA: 0x0001059C File Offset: 0x0000E79C
		[DebuggerNonUserCode]
		public static MessageParser<Song> Parser
		{
			get
			{
				return Song._parser;
			}
		}

		// Token: 0x17000133 RID: 307
		// (get) Token: 0x060003A8 RID: 936 RVA: 0x000105A3 File Offset: 0x0000E7A3
		[DebuggerNonUserCode]
		public static MessageDescriptor Descriptor
		{
			get
			{
				return BeatSaberOutboundMessageReflection.Descriptor.MessageTypes[36];
			}
		}

		// Token: 0x17000134 RID: 308
		// (get) Token: 0x060003A9 RID: 937 RVA: 0x000105B6 File Offset: 0x0000E7B6
		[DebuggerNonUserCode]
		MessageDescriptor IMessage.Descriptor
		{
			get
			{
				return Song.Descriptor;
			}
		}

		// Token: 0x060003AA RID: 938 RVA: 0x000105BD File Offset: 0x0000E7BD
		[DebuggerNonUserCode]
		public Song()
		{
		}

		// Token: 0x060003AB RID: 939 RVA: 0x000105FC File Offset: 0x0000E7FC
		[DebuggerNonUserCode]
		public Song(Song other)
			: this()
		{
			this.songId_ = other.songId_;
			this.songName_ = other.songName_;
			this.album_ = other.album_;
			this.singer_ = other.singer_;
			this.publishDate_ = other.publishDate_;
			this.duration_ = other.duration_;
			this.coverImage_ = other.coverImage_;
			this.seq_ = other.seq_;
			this._unknownFields = UnknownFieldSet.Clone(other._unknownFields);
		}

		// Token: 0x060003AC RID: 940 RVA: 0x00010680 File Offset: 0x0000E880
		[DebuggerNonUserCode]
		public Song Clone()
		{
			return new Song(this);
		}

		// Token: 0x17000135 RID: 309
		// (get) Token: 0x060003AD RID: 941 RVA: 0x00010688 File Offset: 0x0000E888
		// (set) Token: 0x060003AE RID: 942 RVA: 0x00010690 File Offset: 0x0000E890
		[DebuggerNonUserCode]
		public string SongId
		{
			get
			{
				return this.songId_;
			}
			set
			{
				this.songId_ = ProtoPreconditions.CheckNotNull<string>(value, "value");
			}
		}

		// Token: 0x17000136 RID: 310
		// (get) Token: 0x060003AF RID: 943 RVA: 0x000106A3 File Offset: 0x0000E8A3
		// (set) Token: 0x060003B0 RID: 944 RVA: 0x000106AB File Offset: 0x0000E8AB
		[DebuggerNonUserCode]
		public string SongName
		{
			get
			{
				return this.songName_;
			}
			set
			{
				this.songName_ = ProtoPreconditions.CheckNotNull<string>(value, "value");
			}
		}

		// Token: 0x17000137 RID: 311
		// (get) Token: 0x060003B1 RID: 945 RVA: 0x000106BE File Offset: 0x0000E8BE
		// (set) Token: 0x060003B2 RID: 946 RVA: 0x000106C6 File Offset: 0x0000E8C6
		[DebuggerNonUserCode]
		public string Album
		{
			get
			{
				return this.album_;
			}
			set
			{
				this.album_ = ProtoPreconditions.CheckNotNull<string>(value, "value");
			}
		}

		// Token: 0x17000138 RID: 312
		// (get) Token: 0x060003B3 RID: 947 RVA: 0x000106D9 File Offset: 0x0000E8D9
		// (set) Token: 0x060003B4 RID: 948 RVA: 0x000106E1 File Offset: 0x0000E8E1
		[DebuggerNonUserCode]
		public string Singer
		{
			get
			{
				return this.singer_;
			}
			set
			{
				this.singer_ = ProtoPreconditions.CheckNotNull<string>(value, "value");
			}
		}

		// Token: 0x17000139 RID: 313
		// (get) Token: 0x060003B5 RID: 949 RVA: 0x000106F4 File Offset: 0x0000E8F4
		// (set) Token: 0x060003B6 RID: 950 RVA: 0x000106FC File Offset: 0x0000E8FC
		[DebuggerNonUserCode]
		public long PublishDate
		{
			get
			{
				return this.publishDate_;
			}
			set
			{
				this.publishDate_ = value;
			}
		}

		// Token: 0x1700013A RID: 314
		// (get) Token: 0x060003B7 RID: 951 RVA: 0x00010705 File Offset: 0x0000E905
		// (set) Token: 0x060003B8 RID: 952 RVA: 0x0001070D File Offset: 0x0000E90D
		[DebuggerNonUserCode]
		public int Duration
		{
			get
			{
				return this.duration_;
			}
			set
			{
				this.duration_ = value;
			}
		}

		// Token: 0x1700013B RID: 315
		// (get) Token: 0x060003B9 RID: 953 RVA: 0x00010716 File Offset: 0x0000E916
		// (set) Token: 0x060003BA RID: 954 RVA: 0x0001071E File Offset: 0x0000E91E
		[DebuggerNonUserCode]
		public string CoverImage
		{
			get
			{
				return this.coverImage_;
			}
			set
			{
				this.coverImage_ = ProtoPreconditions.CheckNotNull<string>(value, "value");
			}
		}

		// Token: 0x1700013C RID: 316
		// (get) Token: 0x060003BB RID: 955 RVA: 0x00010731 File Offset: 0x0000E931
		// (set) Token: 0x060003BC RID: 956 RVA: 0x00010739 File Offset: 0x0000E939
		[DebuggerNonUserCode]
		public int Seq
		{
			get
			{
				return this.seq_;
			}
			set
			{
				this.seq_ = value;
			}
		}

		// Token: 0x060003BD RID: 957 RVA: 0x00010742 File Offset: 0x0000E942
		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return this.Equals(other as Song);
		}

		// Token: 0x060003BE RID: 958 RVA: 0x00010750 File Offset: 0x0000E950
		[DebuggerNonUserCode]
		public bool Equals(Song other)
		{
			return other != null && (other == this || (!(this.SongId != other.SongId) && !(this.SongName != other.SongName) && !(this.Album != other.Album) && !(this.Singer != other.Singer) && this.PublishDate == other.PublishDate && this.Duration == other.Duration && !(this.CoverImage != other.CoverImage) && this.Seq == other.Seq && object.Equals(this._unknownFields, other._unknownFields)));
		}

		// Token: 0x060003BF RID: 959 RVA: 0x00010814 File Offset: 0x0000EA14
		[DebuggerNonUserCode]
		public override int GetHashCode()
		{
			int num = 1;
			if (this.SongId.Length != 0)
			{
				num ^= this.SongId.GetHashCode();
			}
			if (this.SongName.Length != 0)
			{
				num ^= this.SongName.GetHashCode();
			}
			if (this.Album.Length != 0)
			{
				num ^= this.Album.GetHashCode();
			}
			if (this.Singer.Length != 0)
			{
				num ^= this.Singer.GetHashCode();
			}
			if (this.PublishDate != 0L)
			{
				num ^= this.PublishDate.GetHashCode();
			}
			if (this.Duration != 0)
			{
				num ^= this.Duration.GetHashCode();
			}
			if (this.CoverImage.Length != 0)
			{
				num ^= this.CoverImage.GetHashCode();
			}
			if (this.Seq != 0)
			{
				num ^= this.Seq.GetHashCode();
			}
			if (this._unknownFields != null)
			{
				num ^= this._unknownFields.GetHashCode();
			}
			return num;
		}

		// Token: 0x060003C0 RID: 960 RVA: 0x0000460F File Offset: 0x0000280F
		[DebuggerNonUserCode]
		public override string ToString()
		{
			return JsonFormatter.ToDiagnosticString(this);
		}

		// Token: 0x060003C1 RID: 961 RVA: 0x0001090C File Offset: 0x0000EB0C
		[DebuggerNonUserCode]
		public void WriteTo(CodedOutputStream output)
		{
			if (this.SongId.Length != 0)
			{
				output.WriteRawTag(10);
				output.WriteString(this.SongId);
			}
			if (this.SongName.Length != 0)
			{
				output.WriteRawTag(18);
				output.WriteString(this.SongName);
			}
			if (this.Album.Length != 0)
			{
				output.WriteRawTag(26);
				output.WriteString(this.Album);
			}
			if (this.Singer.Length != 0)
			{
				output.WriteRawTag(34);
				output.WriteString(this.Singer);
			}
			if (this.PublishDate != 0L)
			{
				output.WriteRawTag(40);
				output.WriteInt64(this.PublishDate);
			}
			if (this.Duration != 0)
			{
				output.WriteRawTag(48);
				output.WriteInt32(this.Duration);
			}
			if (this.CoverImage.Length != 0)
			{
				output.WriteRawTag(58);
				output.WriteString(this.CoverImage);
			}
			if (this.Seq != 0)
			{
				output.WriteRawTag(64);
				output.WriteInt32(this.Seq);
			}
			if (this._unknownFields != null)
			{
				this._unknownFields.WriteTo(output);
			}
		}

		// Token: 0x060003C2 RID: 962 RVA: 0x00010A28 File Offset: 0x0000EC28
		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (this.SongId.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.SongId);
			}
			if (this.SongName.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.SongName);
			}
			if (this.Album.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.Album);
			}
			if (this.Singer.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.Singer);
			}
			if (this.PublishDate != 0L)
			{
				num += 1 + CodedOutputStream.ComputeInt64Size(this.PublishDate);
			}
			if (this.Duration != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.Duration);
			}
			if (this.CoverImage.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.CoverImage);
			}
			if (this.Seq != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.Seq);
			}
			if (this._unknownFields != null)
			{
				num += this._unknownFields.CalculateSize();
			}
			return num;
		}

		// Token: 0x060003C3 RID: 963 RVA: 0x00010B28 File Offset: 0x0000ED28
		[DebuggerNonUserCode]
		public void MergeFrom(Song other)
		{
			if (other == null)
			{
				return;
			}
			if (other.SongId.Length != 0)
			{
				this.SongId = other.SongId;
			}
			if (other.SongName.Length != 0)
			{
				this.SongName = other.SongName;
			}
			if (other.Album.Length != 0)
			{
				this.Album = other.Album;
			}
			if (other.Singer.Length != 0)
			{
				this.Singer = other.Singer;
			}
			if (other.PublishDate != 0L)
			{
				this.PublishDate = other.PublishDate;
			}
			if (other.Duration != 0)
			{
				this.Duration = other.Duration;
			}
			if (other.CoverImage.Length != 0)
			{
				this.CoverImage = other.CoverImage;
			}
			if (other.Seq != 0)
			{
				this.Seq = other.Seq;
			}
			this._unknownFields = UnknownFieldSet.MergeFrom(this._unknownFields, other._unknownFields);
		}

		// Token: 0x060003C4 RID: 964 RVA: 0x00010C0C File Offset: 0x0000EE0C
		[DebuggerNonUserCode]
		public void MergeFrom(CodedInputStream input)
		{
			uint num;
			while ((num = input.ReadTag()) != 0U)
			{
				if (num <= 34U)
				{
					if (num <= 18U)
					{
						if (num == 10U)
						{
							this.SongId = input.ReadString();
							continue;
						}
						if (num == 18U)
						{
							this.SongName = input.ReadString();
							continue;
						}
					}
					else
					{
						if (num == 26U)
						{
							this.Album = input.ReadString();
							continue;
						}
						if (num == 34U)
						{
							this.Singer = input.ReadString();
							continue;
						}
					}
				}
				else if (num <= 48U)
				{
					if (num == 40U)
					{
						this.PublishDate = input.ReadInt64();
						continue;
					}
					if (num == 48U)
					{
						this.Duration = input.ReadInt32();
						continue;
					}
				}
				else
				{
					if (num == 58U)
					{
						this.CoverImage = input.ReadString();
						continue;
					}
					if (num == 64U)
					{
						this.Seq = input.ReadInt32();
						continue;
					}
				}
				this._unknownFields = UnknownFieldSet.MergeFieldFrom(this._unknownFields, input);
			}
		}

		// Token: 0x040001E6 RID: 486
		private static readonly MessageParser<Song> _parser = new MessageParser<Song>(() => new Song());

		// Token: 0x040001E7 RID: 487
		private UnknownFieldSet _unknownFields;

		// Token: 0x040001E8 RID: 488
		public const int SongIdFieldNumber = 1;

		// Token: 0x040001E9 RID: 489
		private string songId_ = "";

		// Token: 0x040001EA RID: 490
		public const int SongNameFieldNumber = 2;

		// Token: 0x040001EB RID: 491
		private string songName_ = "";

		// Token: 0x040001EC RID: 492
		public const int AlbumFieldNumber = 3;

		// Token: 0x040001ED RID: 493
		private string album_ = "";

		// Token: 0x040001EE RID: 494
		public const int SingerFieldNumber = 4;

		// Token: 0x040001EF RID: 495
		private string singer_ = "";

		// Token: 0x040001F0 RID: 496
		public const int PublishDateFieldNumber = 5;

		// Token: 0x040001F1 RID: 497
		private long publishDate_;

		// Token: 0x040001F2 RID: 498
		public const int DurationFieldNumber = 6;

		// Token: 0x040001F3 RID: 499
		private int duration_;

		// Token: 0x040001F4 RID: 500
		public const int CoverImageFieldNumber = 7;

		// Token: 0x040001F5 RID: 501
		private string coverImage_ = "";

		// Token: 0x040001F6 RID: 502
		public const int SeqFieldNumber = 8;

		// Token: 0x040001F7 RID: 503
		private int seq_;
	}
}
