using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Com.Netvios.Proto.Outbound
{
	// Token: 0x0200002B RID: 43
	public sealed class SongCfg : IMessage<SongCfg>, IMessage, IEquatable<SongCfg>, IDeepCloneable<SongCfg>
	{
		// Token: 0x17000128 RID: 296
		// (get) Token: 0x0600038A RID: 906 RVA: 0x0000FE9E File Offset: 0x0000E09E
		[DebuggerNonUserCode]
		public static MessageParser<SongCfg> Parser
		{
			get
			{
				return SongCfg._parser;
			}
		}

		// Token: 0x17000129 RID: 297
		// (get) Token: 0x0600038B RID: 907 RVA: 0x0000FEA5 File Offset: 0x0000E0A5
		[DebuggerNonUserCode]
		public static MessageDescriptor Descriptor
		{
			get
			{
				return BeatSaberOutboundMessageReflection.Descriptor.MessageTypes[35];
			}
		}

		// Token: 0x1700012A RID: 298
		// (get) Token: 0x0600038C RID: 908 RVA: 0x0000FEB8 File Offset: 0x0000E0B8
		[DebuggerNonUserCode]
		MessageDescriptor IMessage.Descriptor
		{
			get
			{
				return SongCfg.Descriptor;
			}
		}

		// Token: 0x0600038D RID: 909 RVA: 0x0000FEC0 File Offset: 0x0000E0C0
		[DebuggerNonUserCode]
		public SongCfg()
		{
		}

		// Token: 0x0600038E RID: 910 RVA: 0x0000FF18 File Offset: 0x0000E118
		[DebuggerNonUserCode]
		public SongCfg(SongCfg other)
			: this()
		{
			this.songId_ = other.songId_;
			this.mode_ = other.mode_;
			this.difficulty_ = other.difficulty_;
			this.songName_ = other.songName_;
			this.songCoverImg_ = other.songCoverImg_;
			this.songDuration_ = other.songDuration_;
			this.rules_ = other.rules_;
			this._unknownFields = UnknownFieldSet.Clone(other._unknownFields);
		}

		// Token: 0x0600038F RID: 911 RVA: 0x0000FF90 File Offset: 0x0000E190
		[DebuggerNonUserCode]
		public SongCfg Clone()
		{
			return new SongCfg(this);
		}

		// Token: 0x1700012B RID: 299
		// (get) Token: 0x06000390 RID: 912 RVA: 0x0000FF98 File Offset: 0x0000E198
		// (set) Token: 0x06000391 RID: 913 RVA: 0x0000FFA0 File Offset: 0x0000E1A0
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

		// Token: 0x1700012C RID: 300
		// (get) Token: 0x06000392 RID: 914 RVA: 0x0000FFB3 File Offset: 0x0000E1B3
		// (set) Token: 0x06000393 RID: 915 RVA: 0x0000FFBB File Offset: 0x0000E1BB
		[DebuggerNonUserCode]
		public string Mode
		{
			get
			{
				return this.mode_;
			}
			set
			{
				this.mode_ = ProtoPreconditions.CheckNotNull<string>(value, "value");
			}
		}

		// Token: 0x1700012D RID: 301
		// (get) Token: 0x06000394 RID: 916 RVA: 0x0000FFCE File Offset: 0x0000E1CE
		// (set) Token: 0x06000395 RID: 917 RVA: 0x0000FFD6 File Offset: 0x0000E1D6
		[DebuggerNonUserCode]
		public string Difficulty
		{
			get
			{
				return this.difficulty_;
			}
			set
			{
				this.difficulty_ = ProtoPreconditions.CheckNotNull<string>(value, "value");
			}
		}

		// Token: 0x1700012E RID: 302
		// (get) Token: 0x06000396 RID: 918 RVA: 0x0000FFE9 File Offset: 0x0000E1E9
		// (set) Token: 0x06000397 RID: 919 RVA: 0x0000FFF1 File Offset: 0x0000E1F1
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

		// Token: 0x1700012F RID: 303
		// (get) Token: 0x06000398 RID: 920 RVA: 0x00010004 File Offset: 0x0000E204
		// (set) Token: 0x06000399 RID: 921 RVA: 0x0001000C File Offset: 0x0000E20C
		[DebuggerNonUserCode]
		public string SongCoverImg
		{
			get
			{
				return this.songCoverImg_;
			}
			set
			{
				this.songCoverImg_ = ProtoPreconditions.CheckNotNull<string>(value, "value");
			}
		}

		// Token: 0x17000130 RID: 304
		// (get) Token: 0x0600039A RID: 922 RVA: 0x0001001F File Offset: 0x0000E21F
		// (set) Token: 0x0600039B RID: 923 RVA: 0x00010027 File Offset: 0x0000E227
		[DebuggerNonUserCode]
		public int SongDuration
		{
			get
			{
				return this.songDuration_;
			}
			set
			{
				this.songDuration_ = value;
			}
		}

		// Token: 0x17000131 RID: 305
		// (get) Token: 0x0600039C RID: 924 RVA: 0x00010030 File Offset: 0x0000E230
		// (set) Token: 0x0600039D RID: 925 RVA: 0x00010038 File Offset: 0x0000E238
		[DebuggerNonUserCode]
		public string Rules
		{
			get
			{
				return this.rules_;
			}
			set
			{
				this.rules_ = ProtoPreconditions.CheckNotNull<string>(value, "value");
			}
		}

		// Token: 0x0600039E RID: 926 RVA: 0x0001004B File Offset: 0x0000E24B
		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return this.Equals(other as SongCfg);
		}

		// Token: 0x0600039F RID: 927 RVA: 0x0001005C File Offset: 0x0000E25C
		[DebuggerNonUserCode]
		public bool Equals(SongCfg other)
		{
			return other != null && (other == this || (!(this.SongId != other.SongId) && !(this.Mode != other.Mode) && !(this.Difficulty != other.Difficulty) && !(this.SongName != other.SongName) && !(this.SongCoverImg != other.SongCoverImg) && this.SongDuration == other.SongDuration && !(this.Rules != other.Rules) && object.Equals(this._unknownFields, other._unknownFields)));
		}

		// Token: 0x060003A0 RID: 928 RVA: 0x00010114 File Offset: 0x0000E314
		[DebuggerNonUserCode]
		public override int GetHashCode()
		{
			int num = 1;
			if (this.SongId.Length != 0)
			{
				num ^= this.SongId.GetHashCode();
			}
			if (this.Mode.Length != 0)
			{
				num ^= this.Mode.GetHashCode();
			}
			if (this.Difficulty.Length != 0)
			{
				num ^= this.Difficulty.GetHashCode();
			}
			if (this.SongName.Length != 0)
			{
				num ^= this.SongName.GetHashCode();
			}
			if (this.SongCoverImg.Length != 0)
			{
				num ^= this.SongCoverImg.GetHashCode();
			}
			if (this.SongDuration != 0)
			{
				num ^= this.SongDuration.GetHashCode();
			}
			if (this.Rules.Length != 0)
			{
				num ^= this.Rules.GetHashCode();
			}
			if (this._unknownFields != null)
			{
				num ^= this._unknownFields.GetHashCode();
			}
			return num;
		}

		// Token: 0x060003A1 RID: 929 RVA: 0x0000460F File Offset: 0x0000280F
		[DebuggerNonUserCode]
		public override string ToString()
		{
			return JsonFormatter.ToDiagnosticString(this);
		}

		// Token: 0x060003A2 RID: 930 RVA: 0x000101F8 File Offset: 0x0000E3F8
		[DebuggerNonUserCode]
		public void WriteTo(CodedOutputStream output)
		{
			if (this.SongId.Length != 0)
			{
				output.WriteRawTag(10);
				output.WriteString(this.SongId);
			}
			if (this.Mode.Length != 0)
			{
				output.WriteRawTag(18);
				output.WriteString(this.Mode);
			}
			if (this.Difficulty.Length != 0)
			{
				output.WriteRawTag(26);
				output.WriteString(this.Difficulty);
			}
			if (this.SongName.Length != 0)
			{
				output.WriteRawTag(42);
				output.WriteString(this.SongName);
			}
			if (this.SongCoverImg.Length != 0)
			{
				output.WriteRawTag(50);
				output.WriteString(this.SongCoverImg);
			}
			if (this.SongDuration != 0)
			{
				output.WriteRawTag(56);
				output.WriteInt32(this.SongDuration);
			}
			if (this.Rules.Length != 0)
			{
				output.WriteRawTag(82);
				output.WriteString(this.Rules);
			}
			if (this._unknownFields != null)
			{
				this._unknownFields.WriteTo(output);
			}
		}

		// Token: 0x060003A3 RID: 931 RVA: 0x000102FC File Offset: 0x0000E4FC
		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (this.SongId.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.SongId);
			}
			if (this.Mode.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.Mode);
			}
			if (this.Difficulty.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.Difficulty);
			}
			if (this.SongName.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.SongName);
			}
			if (this.SongCoverImg.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.SongCoverImg);
			}
			if (this.SongDuration != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.SongDuration);
			}
			if (this.Rules.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.Rules);
			}
			if (this._unknownFields != null)
			{
				num += this._unknownFields.CalculateSize();
			}
			return num;
		}

		// Token: 0x060003A4 RID: 932 RVA: 0x000103E8 File Offset: 0x0000E5E8
		[DebuggerNonUserCode]
		public void MergeFrom(SongCfg other)
		{
			if (other == null)
			{
				return;
			}
			if (other.SongId.Length != 0)
			{
				this.SongId = other.SongId;
			}
			if (other.Mode.Length != 0)
			{
				this.Mode = other.Mode;
			}
			if (other.Difficulty.Length != 0)
			{
				this.Difficulty = other.Difficulty;
			}
			if (other.SongName.Length != 0)
			{
				this.SongName = other.SongName;
			}
			if (other.SongCoverImg.Length != 0)
			{
				this.SongCoverImg = other.SongCoverImg;
			}
			if (other.SongDuration != 0)
			{
				this.SongDuration = other.SongDuration;
			}
			if (other.Rules.Length != 0)
			{
				this.Rules = other.Rules;
			}
			this._unknownFields = UnknownFieldSet.MergeFrom(this._unknownFields, other._unknownFields);
		}

		// Token: 0x060003A5 RID: 933 RVA: 0x000104BC File Offset: 0x0000E6BC
		[DebuggerNonUserCode]
		public void MergeFrom(CodedInputStream input)
		{
			uint num;
			while ((num = input.ReadTag()) != 0U)
			{
				if (num <= 26U)
				{
					if (num == 10U)
					{
						this.SongId = input.ReadString();
						continue;
					}
					if (num == 18U)
					{
						this.Mode = input.ReadString();
						continue;
					}
					if (num == 26U)
					{
						this.Difficulty = input.ReadString();
						continue;
					}
				}
				else if (num <= 50U)
				{
					if (num == 42U)
					{
						this.SongName = input.ReadString();
						continue;
					}
					if (num == 50U)
					{
						this.SongCoverImg = input.ReadString();
						continue;
					}
				}
				else
				{
					if (num == 56U)
					{
						this.SongDuration = input.ReadInt32();
						continue;
					}
					if (num == 82U)
					{
						this.Rules = input.ReadString();
						continue;
					}
				}
				this._unknownFields = UnknownFieldSet.MergeFieldFrom(this._unknownFields, input);
			}
		}

		// Token: 0x040001D6 RID: 470
		private static readonly MessageParser<SongCfg> _parser = new MessageParser<SongCfg>(() => new SongCfg());

		// Token: 0x040001D7 RID: 471
		private UnknownFieldSet _unknownFields;

		// Token: 0x040001D8 RID: 472
		public const int SongIdFieldNumber = 1;

		// Token: 0x040001D9 RID: 473
		private string songId_ = "";

		// Token: 0x040001DA RID: 474
		public const int ModeFieldNumber = 2;

		// Token: 0x040001DB RID: 475
		private string mode_ = "";

		// Token: 0x040001DC RID: 476
		public const int DifficultyFieldNumber = 3;

		// Token: 0x040001DD RID: 477
		private string difficulty_ = "";

		// Token: 0x040001DE RID: 478
		public const int SongNameFieldNumber = 5;

		// Token: 0x040001DF RID: 479
		private string songName_ = "";

		// Token: 0x040001E0 RID: 480
		public const int SongCoverImgFieldNumber = 6;

		// Token: 0x040001E1 RID: 481
		private string songCoverImg_ = "";

		// Token: 0x040001E2 RID: 482
		public const int SongDurationFieldNumber = 7;

		// Token: 0x040001E3 RID: 483
		private int songDuration_;

		// Token: 0x040001E4 RID: 484
		public const int RulesFieldNumber = 10;

		// Token: 0x040001E5 RID: 485
		private string rules_ = "";
	}
}
