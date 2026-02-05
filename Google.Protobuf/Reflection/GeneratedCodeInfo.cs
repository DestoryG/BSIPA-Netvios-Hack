using System;
using System.Diagnostics;
using Google.Protobuf.Collections;

namespace Google.Protobuf.Reflection
{
	// Token: 0x02000064 RID: 100
	public sealed class GeneratedCodeInfo : IMessage<GeneratedCodeInfo>, IMessage, IEquatable<GeneratedCodeInfo>, IDeepCloneable<GeneratedCodeInfo>
	{
		// Token: 0x170001CA RID: 458
		// (get) Token: 0x0600071E RID: 1822 RVA: 0x0001A01C File Offset: 0x0001821C
		[DebuggerNonUserCode]
		public static MessageParser<GeneratedCodeInfo> Parser
		{
			get
			{
				return GeneratedCodeInfo._parser;
			}
		}

		// Token: 0x170001CB RID: 459
		// (get) Token: 0x0600071F RID: 1823 RVA: 0x0001A023 File Offset: 0x00018223
		[DebuggerNonUserCode]
		public static MessageDescriptor Descriptor
		{
			get
			{
				return DescriptorReflection.Descriptor.MessageTypes[20];
			}
		}

		// Token: 0x170001CC RID: 460
		// (get) Token: 0x06000720 RID: 1824 RVA: 0x0001A036 File Offset: 0x00018236
		[DebuggerNonUserCode]
		MessageDescriptor IMessage.Descriptor
		{
			get
			{
				return GeneratedCodeInfo.Descriptor;
			}
		}

		// Token: 0x06000721 RID: 1825 RVA: 0x0001A03D File Offset: 0x0001823D
		[DebuggerNonUserCode]
		public GeneratedCodeInfo()
		{
		}

		// Token: 0x06000722 RID: 1826 RVA: 0x0001A050 File Offset: 0x00018250
		[DebuggerNonUserCode]
		public GeneratedCodeInfo(GeneratedCodeInfo other)
			: this()
		{
			this.annotation_ = other.annotation_.Clone();
			this._unknownFields = UnknownFieldSet.Clone(other._unknownFields);
		}

		// Token: 0x06000723 RID: 1827 RVA: 0x0001A07A File Offset: 0x0001827A
		[DebuggerNonUserCode]
		public GeneratedCodeInfo Clone()
		{
			return new GeneratedCodeInfo(this);
		}

		// Token: 0x170001CD RID: 461
		// (get) Token: 0x06000724 RID: 1828 RVA: 0x0001A082 File Offset: 0x00018282
		[DebuggerNonUserCode]
		public RepeatedField<GeneratedCodeInfo.Types.Annotation> Annotation
		{
			get
			{
				return this.annotation_;
			}
		}

		// Token: 0x06000725 RID: 1829 RVA: 0x0001A08A File Offset: 0x0001828A
		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return this.Equals(other as GeneratedCodeInfo);
		}

		// Token: 0x06000726 RID: 1830 RVA: 0x0001A098 File Offset: 0x00018298
		[DebuggerNonUserCode]
		public bool Equals(GeneratedCodeInfo other)
		{
			return other != null && (other == this || (this.annotation_.Equals(other.annotation_) && object.Equals(this._unknownFields, other._unknownFields)));
		}

		// Token: 0x06000727 RID: 1831 RVA: 0x0001A0CC File Offset: 0x000182CC
		[DebuggerNonUserCode]
		public override int GetHashCode()
		{
			int num = 1;
			num ^= this.annotation_.GetHashCode();
			if (this._unknownFields != null)
			{
				num ^= this._unknownFields.GetHashCode();
			}
			return num;
		}

		// Token: 0x06000728 RID: 1832 RVA: 0x0001A100 File Offset: 0x00018300
		[DebuggerNonUserCode]
		public override string ToString()
		{
			return JsonFormatter.ToDiagnosticString(this);
		}

		// Token: 0x06000729 RID: 1833 RVA: 0x0001A108 File Offset: 0x00018308
		[DebuggerNonUserCode]
		public void WriteTo(CodedOutputStream output)
		{
			this.annotation_.WriteTo(output, GeneratedCodeInfo._repeated_annotation_codec);
			if (this._unknownFields != null)
			{
				this._unknownFields.WriteTo(output);
			}
		}

		// Token: 0x0600072A RID: 1834 RVA: 0x0001A130 File Offset: 0x00018330
		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			num += this.annotation_.CalculateSize(GeneratedCodeInfo._repeated_annotation_codec);
			if (this._unknownFields != null)
			{
				num += this._unknownFields.CalculateSize();
			}
			return num;
		}

		// Token: 0x0600072B RID: 1835 RVA: 0x0001A169 File Offset: 0x00018369
		[DebuggerNonUserCode]
		public void MergeFrom(GeneratedCodeInfo other)
		{
			if (other == null)
			{
				return;
			}
			this.annotation_.Add(other.annotation_);
			this._unknownFields = UnknownFieldSet.MergeFrom(this._unknownFields, other._unknownFields);
		}

		// Token: 0x0600072C RID: 1836 RVA: 0x0001A198 File Offset: 0x00018398
		[DebuggerNonUserCode]
		public void MergeFrom(CodedInputStream input)
		{
			uint num;
			while ((num = input.ReadTag()) != 0U)
			{
				if (num != 10U)
				{
					this._unknownFields = UnknownFieldSet.MergeFieldFrom(this._unknownFields, input);
				}
				else
				{
					this.annotation_.AddEntriesFrom(input, GeneratedCodeInfo._repeated_annotation_codec);
				}
			}
		}

		// Token: 0x040002C4 RID: 708
		private static readonly MessageParser<GeneratedCodeInfo> _parser = new MessageParser<GeneratedCodeInfo>(() => new GeneratedCodeInfo());

		// Token: 0x040002C5 RID: 709
		private UnknownFieldSet _unknownFields;

		// Token: 0x040002C6 RID: 710
		public const int AnnotationFieldNumber = 1;

		// Token: 0x040002C7 RID: 711
		private static readonly FieldCodec<GeneratedCodeInfo.Types.Annotation> _repeated_annotation_codec = FieldCodec.ForMessage<GeneratedCodeInfo.Types.Annotation>(10U, GeneratedCodeInfo.Types.Annotation.Parser);

		// Token: 0x040002C8 RID: 712
		private readonly RepeatedField<GeneratedCodeInfo.Types.Annotation> annotation_ = new RepeatedField<GeneratedCodeInfo.Types.Annotation>();

		// Token: 0x020000E5 RID: 229
		[DebuggerNonUserCode]
		public static class Types
		{
			// Token: 0x0200011E RID: 286
			public sealed class Annotation : IMessage<GeneratedCodeInfo.Types.Annotation>, IMessage, IEquatable<GeneratedCodeInfo.Types.Annotation>, IDeepCloneable<GeneratedCodeInfo.Types.Annotation>
			{
				// Token: 0x170002A0 RID: 672
				// (get) Token: 0x06000AFD RID: 2813 RVA: 0x000227CF File Offset: 0x000209CF
				[DebuggerNonUserCode]
				public static MessageParser<GeneratedCodeInfo.Types.Annotation> Parser
				{
					get
					{
						return GeneratedCodeInfo.Types.Annotation._parser;
					}
				}

				// Token: 0x170002A1 RID: 673
				// (get) Token: 0x06000AFE RID: 2814 RVA: 0x000227D6 File Offset: 0x000209D6
				[DebuggerNonUserCode]
				public static MessageDescriptor Descriptor
				{
					get
					{
						return GeneratedCodeInfo.Descriptor.NestedTypes[0];
					}
				}

				// Token: 0x170002A2 RID: 674
				// (get) Token: 0x06000AFF RID: 2815 RVA: 0x000227E8 File Offset: 0x000209E8
				[DebuggerNonUserCode]
				MessageDescriptor IMessage.Descriptor
				{
					get
					{
						return GeneratedCodeInfo.Types.Annotation.Descriptor;
					}
				}

				// Token: 0x06000B00 RID: 2816 RVA: 0x000227EF File Offset: 0x000209EF
				[DebuggerNonUserCode]
				public Annotation()
				{
				}

				// Token: 0x06000B01 RID: 2817 RVA: 0x00022804 File Offset: 0x00020A04
				[DebuggerNonUserCode]
				public Annotation(GeneratedCodeInfo.Types.Annotation other)
					: this()
				{
					this._hasBits0 = other._hasBits0;
					this.path_ = other.path_.Clone();
					this.sourceFile_ = other.sourceFile_;
					this.begin_ = other.begin_;
					this.end_ = other.end_;
					this._unknownFields = UnknownFieldSet.Clone(other._unknownFields);
				}

				// Token: 0x06000B02 RID: 2818 RVA: 0x00022869 File Offset: 0x00020A69
				[DebuggerNonUserCode]
				public GeneratedCodeInfo.Types.Annotation Clone()
				{
					return new GeneratedCodeInfo.Types.Annotation(this);
				}

				// Token: 0x170002A3 RID: 675
				// (get) Token: 0x06000B03 RID: 2819 RVA: 0x00022871 File Offset: 0x00020A71
				[DebuggerNonUserCode]
				public RepeatedField<int> Path
				{
					get
					{
						return this.path_;
					}
				}

				// Token: 0x170002A4 RID: 676
				// (get) Token: 0x06000B04 RID: 2820 RVA: 0x00022879 File Offset: 0x00020A79
				// (set) Token: 0x06000B05 RID: 2821 RVA: 0x0002288A File Offset: 0x00020A8A
				[DebuggerNonUserCode]
				public string SourceFile
				{
					get
					{
						return this.sourceFile_ ?? GeneratedCodeInfo.Types.Annotation.SourceFileDefaultValue;
					}
					set
					{
						this.sourceFile_ = ProtoPreconditions.CheckNotNull<string>(value, "value");
					}
				}

				// Token: 0x170002A5 RID: 677
				// (get) Token: 0x06000B06 RID: 2822 RVA: 0x0002289D File Offset: 0x00020A9D
				[DebuggerNonUserCode]
				public bool HasSourceFile
				{
					get
					{
						return this.sourceFile_ != null;
					}
				}

				// Token: 0x06000B07 RID: 2823 RVA: 0x000228A8 File Offset: 0x00020AA8
				[DebuggerNonUserCode]
				public void ClearSourceFile()
				{
					this.sourceFile_ = null;
				}

				// Token: 0x170002A6 RID: 678
				// (get) Token: 0x06000B08 RID: 2824 RVA: 0x000228B1 File Offset: 0x00020AB1
				// (set) Token: 0x06000B09 RID: 2825 RVA: 0x000228C9 File Offset: 0x00020AC9
				[DebuggerNonUserCode]
				public int Begin
				{
					get
					{
						if ((this._hasBits0 & 1) != 0)
						{
							return this.begin_;
						}
						return GeneratedCodeInfo.Types.Annotation.BeginDefaultValue;
					}
					set
					{
						this._hasBits0 |= 1;
						this.begin_ = value;
					}
				}

				// Token: 0x170002A7 RID: 679
				// (get) Token: 0x06000B0A RID: 2826 RVA: 0x000228E0 File Offset: 0x00020AE0
				[DebuggerNonUserCode]
				public bool HasBegin
				{
					get
					{
						return (this._hasBits0 & 1) != 0;
					}
				}

				// Token: 0x06000B0B RID: 2827 RVA: 0x000228ED File Offset: 0x00020AED
				[DebuggerNonUserCode]
				public void ClearBegin()
				{
					this._hasBits0 &= -2;
				}

				// Token: 0x170002A8 RID: 680
				// (get) Token: 0x06000B0C RID: 2828 RVA: 0x000228FE File Offset: 0x00020AFE
				// (set) Token: 0x06000B0D RID: 2829 RVA: 0x00022916 File Offset: 0x00020B16
				[DebuggerNonUserCode]
				public int End
				{
					get
					{
						if ((this._hasBits0 & 2) != 0)
						{
							return this.end_;
						}
						return GeneratedCodeInfo.Types.Annotation.EndDefaultValue;
					}
					set
					{
						this._hasBits0 |= 2;
						this.end_ = value;
					}
				}

				// Token: 0x170002A9 RID: 681
				// (get) Token: 0x06000B0E RID: 2830 RVA: 0x0002292D File Offset: 0x00020B2D
				[DebuggerNonUserCode]
				public bool HasEnd
				{
					get
					{
						return (this._hasBits0 & 2) != 0;
					}
				}

				// Token: 0x06000B0F RID: 2831 RVA: 0x0002293A File Offset: 0x00020B3A
				[DebuggerNonUserCode]
				public void ClearEnd()
				{
					this._hasBits0 &= -3;
				}

				// Token: 0x06000B10 RID: 2832 RVA: 0x0002294B File Offset: 0x00020B4B
				[DebuggerNonUserCode]
				public override bool Equals(object other)
				{
					return this.Equals(other as GeneratedCodeInfo.Types.Annotation);
				}

				// Token: 0x06000B11 RID: 2833 RVA: 0x0002295C File Offset: 0x00020B5C
				[DebuggerNonUserCode]
				public bool Equals(GeneratedCodeInfo.Types.Annotation other)
				{
					return other != null && (other == this || (this.path_.Equals(other.path_) && !(this.SourceFile != other.SourceFile) && this.Begin == other.Begin && this.End == other.End && object.Equals(this._unknownFields, other._unknownFields)));
				}

				// Token: 0x06000B12 RID: 2834 RVA: 0x000229D0 File Offset: 0x00020BD0
				[DebuggerNonUserCode]
				public override int GetHashCode()
				{
					int num = 1;
					num ^= this.path_.GetHashCode();
					if (this.HasSourceFile)
					{
						num ^= this.SourceFile.GetHashCode();
					}
					if (this.HasBegin)
					{
						num ^= this.Begin.GetHashCode();
					}
					if (this.HasEnd)
					{
						num ^= this.End.GetHashCode();
					}
					if (this._unknownFields != null)
					{
						num ^= this._unknownFields.GetHashCode();
					}
					return num;
				}

				// Token: 0x06000B13 RID: 2835 RVA: 0x00022A4C File Offset: 0x00020C4C
				[DebuggerNonUserCode]
				public override string ToString()
				{
					return JsonFormatter.ToDiagnosticString(this);
				}

				// Token: 0x06000B14 RID: 2836 RVA: 0x00022A54 File Offset: 0x00020C54
				[DebuggerNonUserCode]
				public void WriteTo(CodedOutputStream output)
				{
					this.path_.WriteTo(output, GeneratedCodeInfo.Types.Annotation._repeated_path_codec);
					if (this.HasSourceFile)
					{
						output.WriteRawTag(18);
						output.WriteString(this.SourceFile);
					}
					if (this.HasBegin)
					{
						output.WriteRawTag(24);
						output.WriteInt32(this.Begin);
					}
					if (this.HasEnd)
					{
						output.WriteRawTag(32);
						output.WriteInt32(this.End);
					}
					if (this._unknownFields != null)
					{
						this._unknownFields.WriteTo(output);
					}
				}

				// Token: 0x06000B15 RID: 2837 RVA: 0x00022ADC File Offset: 0x00020CDC
				[DebuggerNonUserCode]
				public int CalculateSize()
				{
					int num = 0;
					num += this.path_.CalculateSize(GeneratedCodeInfo.Types.Annotation._repeated_path_codec);
					if (this.HasSourceFile)
					{
						num += 1 + CodedOutputStream.ComputeStringSize(this.SourceFile);
					}
					if (this.HasBegin)
					{
						num += 1 + CodedOutputStream.ComputeInt32Size(this.Begin);
					}
					if (this.HasEnd)
					{
						num += 1 + CodedOutputStream.ComputeInt32Size(this.End);
					}
					if (this._unknownFields != null)
					{
						num += this._unknownFields.CalculateSize();
					}
					return num;
				}

				// Token: 0x06000B16 RID: 2838 RVA: 0x00022B60 File Offset: 0x00020D60
				[DebuggerNonUserCode]
				public void MergeFrom(GeneratedCodeInfo.Types.Annotation other)
				{
					if (other == null)
					{
						return;
					}
					this.path_.Add(other.path_);
					if (other.HasSourceFile)
					{
						this.SourceFile = other.SourceFile;
					}
					if (other.HasBegin)
					{
						this.Begin = other.Begin;
					}
					if (other.HasEnd)
					{
						this.End = other.End;
					}
					this._unknownFields = UnknownFieldSet.MergeFrom(this._unknownFields, other._unknownFields);
				}

				// Token: 0x06000B17 RID: 2839 RVA: 0x00022BD8 File Offset: 0x00020DD8
				[DebuggerNonUserCode]
				public void MergeFrom(CodedInputStream input)
				{
					uint num;
					while ((num = input.ReadTag()) != 0U)
					{
						if (num <= 10U)
						{
							if (num == 8U || num == 10U)
							{
								this.path_.AddEntriesFrom(input, GeneratedCodeInfo.Types.Annotation._repeated_path_codec);
								continue;
							}
						}
						else
						{
							if (num == 18U)
							{
								this.SourceFile = input.ReadString();
								continue;
							}
							if (num == 24U)
							{
								this.Begin = input.ReadInt32();
								continue;
							}
							if (num == 32U)
							{
								this.End = input.ReadInt32();
								continue;
							}
						}
						this._unknownFields = UnknownFieldSet.MergeFieldFrom(this._unknownFields, input);
					}
				}

				// Token: 0x040004CD RID: 1229
				private static readonly MessageParser<GeneratedCodeInfo.Types.Annotation> _parser = new MessageParser<GeneratedCodeInfo.Types.Annotation>(() => new GeneratedCodeInfo.Types.Annotation());

				// Token: 0x040004CE RID: 1230
				private UnknownFieldSet _unknownFields;

				// Token: 0x040004CF RID: 1231
				private int _hasBits0;

				// Token: 0x040004D0 RID: 1232
				public const int PathFieldNumber = 1;

				// Token: 0x040004D1 RID: 1233
				private static readonly FieldCodec<int> _repeated_path_codec = FieldCodec.ForInt32(10U);

				// Token: 0x040004D2 RID: 1234
				private readonly RepeatedField<int> path_ = new RepeatedField<int>();

				// Token: 0x040004D3 RID: 1235
				public const int SourceFileFieldNumber = 2;

				// Token: 0x040004D4 RID: 1236
				private static readonly string SourceFileDefaultValue = "";

				// Token: 0x040004D5 RID: 1237
				private string sourceFile_;

				// Token: 0x040004D6 RID: 1238
				public const int BeginFieldNumber = 3;

				// Token: 0x040004D7 RID: 1239
				private static readonly int BeginDefaultValue = 0;

				// Token: 0x040004D8 RID: 1240
				private int begin_;

				// Token: 0x040004D9 RID: 1241
				public const int EndFieldNumber = 4;

				// Token: 0x040004DA RID: 1242
				private static readonly int EndDefaultValue = 0;

				// Token: 0x040004DB RID: 1243
				private int end_;
			}
		}
	}
}
