using System;
using System.Diagnostics;
using Google.Protobuf.Collections;

namespace Google.Protobuf.Reflection
{
	// Token: 0x02000056 RID: 86
	public sealed class EnumDescriptorProto : IMessage<EnumDescriptorProto>, IMessage, IEquatable<EnumDescriptorProto>, IDeepCloneable<EnumDescriptorProto>
	{
		// Token: 0x17000120 RID: 288
		// (get) Token: 0x0600052A RID: 1322 RVA: 0x000149F2 File Offset: 0x00012BF2
		[DebuggerNonUserCode]
		public static MessageParser<EnumDescriptorProto> Parser
		{
			get
			{
				return EnumDescriptorProto._parser;
			}
		}

		// Token: 0x17000121 RID: 289
		// (get) Token: 0x0600052B RID: 1323 RVA: 0x000149F9 File Offset: 0x00012BF9
		[DebuggerNonUserCode]
		public static MessageDescriptor Descriptor
		{
			get
			{
				return DescriptorReflection.Descriptor.MessageTypes[6];
			}
		}

		// Token: 0x17000122 RID: 290
		// (get) Token: 0x0600052C RID: 1324 RVA: 0x00014A0B File Offset: 0x00012C0B
		[DebuggerNonUserCode]
		MessageDescriptor IMessage.Descriptor
		{
			get
			{
				return EnumDescriptorProto.Descriptor;
			}
		}

		// Token: 0x0600052D RID: 1325 RVA: 0x00014A12 File Offset: 0x00012C12
		[DebuggerNonUserCode]
		public EnumDescriptorProto()
		{
		}

		// Token: 0x0600052E RID: 1326 RVA: 0x00014A3C File Offset: 0x00012C3C
		[DebuggerNonUserCode]
		public EnumDescriptorProto(EnumDescriptorProto other)
			: this()
		{
			this.name_ = other.name_;
			this.value_ = other.value_.Clone();
			this.options_ = ((other.options_ != null) ? other.options_.Clone() : null);
			this.reservedRange_ = other.reservedRange_.Clone();
			this.reservedName_ = other.reservedName_.Clone();
			this._unknownFields = UnknownFieldSet.Clone(other._unknownFields);
		}

		// Token: 0x0600052F RID: 1327 RVA: 0x00014ABB File Offset: 0x00012CBB
		[DebuggerNonUserCode]
		public EnumDescriptorProto Clone()
		{
			return new EnumDescriptorProto(this);
		}

		// Token: 0x17000123 RID: 291
		// (get) Token: 0x06000530 RID: 1328 RVA: 0x00014AC3 File Offset: 0x00012CC3
		// (set) Token: 0x06000531 RID: 1329 RVA: 0x00014AD4 File Offset: 0x00012CD4
		[DebuggerNonUserCode]
		public string Name
		{
			get
			{
				return this.name_ ?? EnumDescriptorProto.NameDefaultValue;
			}
			set
			{
				this.name_ = ProtoPreconditions.CheckNotNull<string>(value, "value");
			}
		}

		// Token: 0x17000124 RID: 292
		// (get) Token: 0x06000532 RID: 1330 RVA: 0x00014AE7 File Offset: 0x00012CE7
		[DebuggerNonUserCode]
		public bool HasName
		{
			get
			{
				return this.name_ != null;
			}
		}

		// Token: 0x06000533 RID: 1331 RVA: 0x00014AF2 File Offset: 0x00012CF2
		[DebuggerNonUserCode]
		public void ClearName()
		{
			this.name_ = null;
		}

		// Token: 0x17000125 RID: 293
		// (get) Token: 0x06000534 RID: 1332 RVA: 0x00014AFB File Offset: 0x00012CFB
		[DebuggerNonUserCode]
		public RepeatedField<EnumValueDescriptorProto> Value
		{
			get
			{
				return this.value_;
			}
		}

		// Token: 0x17000126 RID: 294
		// (get) Token: 0x06000535 RID: 1333 RVA: 0x00014B03 File Offset: 0x00012D03
		// (set) Token: 0x06000536 RID: 1334 RVA: 0x00014B0B File Offset: 0x00012D0B
		[DebuggerNonUserCode]
		public EnumOptions Options
		{
			get
			{
				return this.options_;
			}
			set
			{
				this.options_ = value;
			}
		}

		// Token: 0x17000127 RID: 295
		// (get) Token: 0x06000537 RID: 1335 RVA: 0x00014B14 File Offset: 0x00012D14
		[DebuggerNonUserCode]
		public RepeatedField<EnumDescriptorProto.Types.EnumReservedRange> ReservedRange
		{
			get
			{
				return this.reservedRange_;
			}
		}

		// Token: 0x17000128 RID: 296
		// (get) Token: 0x06000538 RID: 1336 RVA: 0x00014B1C File Offset: 0x00012D1C
		[DebuggerNonUserCode]
		public RepeatedField<string> ReservedName
		{
			get
			{
				return this.reservedName_;
			}
		}

		// Token: 0x06000539 RID: 1337 RVA: 0x00014B24 File Offset: 0x00012D24
		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return this.Equals(other as EnumDescriptorProto);
		}

		// Token: 0x0600053A RID: 1338 RVA: 0x00014B34 File Offset: 0x00012D34
		[DebuggerNonUserCode]
		public bool Equals(EnumDescriptorProto other)
		{
			return other != null && (other == this || (!(this.Name != other.Name) && this.value_.Equals(other.value_) && object.Equals(this.Options, other.Options) && this.reservedRange_.Equals(other.reservedRange_) && this.reservedName_.Equals(other.reservedName_) && object.Equals(this._unknownFields, other._unknownFields)));
		}

		// Token: 0x0600053B RID: 1339 RVA: 0x00014BC8 File Offset: 0x00012DC8
		[DebuggerNonUserCode]
		public override int GetHashCode()
		{
			int num = 1;
			if (this.HasName)
			{
				num ^= this.Name.GetHashCode();
			}
			num ^= this.value_.GetHashCode();
			if (this.options_ != null)
			{
				num ^= this.Options.GetHashCode();
			}
			num ^= this.reservedRange_.GetHashCode();
			num ^= this.reservedName_.GetHashCode();
			if (this._unknownFields != null)
			{
				num ^= this._unknownFields.GetHashCode();
			}
			return num;
		}

		// Token: 0x0600053C RID: 1340 RVA: 0x00014C44 File Offset: 0x00012E44
		[DebuggerNonUserCode]
		public override string ToString()
		{
			return JsonFormatter.ToDiagnosticString(this);
		}

		// Token: 0x0600053D RID: 1341 RVA: 0x00014C4C File Offset: 0x00012E4C
		[DebuggerNonUserCode]
		public void WriteTo(CodedOutputStream output)
		{
			if (this.HasName)
			{
				output.WriteRawTag(10);
				output.WriteString(this.Name);
			}
			this.value_.WriteTo(output, EnumDescriptorProto._repeated_value_codec);
			if (this.options_ != null)
			{
				output.WriteRawTag(26);
				output.WriteMessage(this.Options);
			}
			this.reservedRange_.WriteTo(output, EnumDescriptorProto._repeated_reservedRange_codec);
			this.reservedName_.WriteTo(output, EnumDescriptorProto._repeated_reservedName_codec);
			if (this._unknownFields != null)
			{
				this._unknownFields.WriteTo(output);
			}
		}

		// Token: 0x0600053E RID: 1342 RVA: 0x00014CD8 File Offset: 0x00012ED8
		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (this.HasName)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.Name);
			}
			num += this.value_.CalculateSize(EnumDescriptorProto._repeated_value_codec);
			if (this.options_ != null)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(this.Options);
			}
			num += this.reservedRange_.CalculateSize(EnumDescriptorProto._repeated_reservedRange_codec);
			num += this.reservedName_.CalculateSize(EnumDescriptorProto._repeated_reservedName_codec);
			if (this._unknownFields != null)
			{
				num += this._unknownFields.CalculateSize();
			}
			return num;
		}

		// Token: 0x0600053F RID: 1343 RVA: 0x00014D68 File Offset: 0x00012F68
		[DebuggerNonUserCode]
		public void MergeFrom(EnumDescriptorProto other)
		{
			if (other == null)
			{
				return;
			}
			if (other.HasName)
			{
				this.Name = other.Name;
			}
			this.value_.Add(other.value_);
			if (other.options_ != null)
			{
				if (this.options_ == null)
				{
					this.Options = new EnumOptions();
				}
				this.Options.MergeFrom(other.Options);
			}
			this.reservedRange_.Add(other.reservedRange_);
			this.reservedName_.Add(other.reservedName_);
			this._unknownFields = UnknownFieldSet.MergeFrom(this._unknownFields, other._unknownFields);
		}

		// Token: 0x06000540 RID: 1344 RVA: 0x00014E04 File Offset: 0x00013004
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
						this.Name = input.ReadString();
						continue;
					}
					if (num == 18U)
					{
						this.value_.AddEntriesFrom(input, EnumDescriptorProto._repeated_value_codec);
						continue;
					}
				}
				else
				{
					if (num == 26U)
					{
						if (this.options_ == null)
						{
							this.Options = new EnumOptions();
						}
						input.ReadMessage(this.Options);
						continue;
					}
					if (num == 34U)
					{
						this.reservedRange_.AddEntriesFrom(input, EnumDescriptorProto._repeated_reservedRange_codec);
						continue;
					}
					if (num == 42U)
					{
						this.reservedName_.AddEntriesFrom(input, EnumDescriptorProto._repeated_reservedName_codec);
						continue;
					}
				}
				this._unknownFields = UnknownFieldSet.MergeFieldFrom(this._unknownFields, input);
			}
		}

		// Token: 0x040001CB RID: 459
		private static readonly MessageParser<EnumDescriptorProto> _parser = new MessageParser<EnumDescriptorProto>(() => new EnumDescriptorProto());

		// Token: 0x040001CC RID: 460
		private UnknownFieldSet _unknownFields;

		// Token: 0x040001CD RID: 461
		public const int NameFieldNumber = 1;

		// Token: 0x040001CE RID: 462
		private static readonly string NameDefaultValue = "";

		// Token: 0x040001CF RID: 463
		private string name_;

		// Token: 0x040001D0 RID: 464
		public const int ValueFieldNumber = 2;

		// Token: 0x040001D1 RID: 465
		private static readonly FieldCodec<EnumValueDescriptorProto> _repeated_value_codec = FieldCodec.ForMessage<EnumValueDescriptorProto>(18U, EnumValueDescriptorProto.Parser);

		// Token: 0x040001D2 RID: 466
		private readonly RepeatedField<EnumValueDescriptorProto> value_ = new RepeatedField<EnumValueDescriptorProto>();

		// Token: 0x040001D3 RID: 467
		public const int OptionsFieldNumber = 3;

		// Token: 0x040001D4 RID: 468
		private EnumOptions options_;

		// Token: 0x040001D5 RID: 469
		public const int ReservedRangeFieldNumber = 4;

		// Token: 0x040001D6 RID: 470
		private static readonly FieldCodec<EnumDescriptorProto.Types.EnumReservedRange> _repeated_reservedRange_codec = FieldCodec.ForMessage<EnumDescriptorProto.Types.EnumReservedRange>(34U, EnumDescriptorProto.Types.EnumReservedRange.Parser);

		// Token: 0x040001D7 RID: 471
		private readonly RepeatedField<EnumDescriptorProto.Types.EnumReservedRange> reservedRange_ = new RepeatedField<EnumDescriptorProto.Types.EnumReservedRange>();

		// Token: 0x040001D8 RID: 472
		public const int ReservedNameFieldNumber = 5;

		// Token: 0x040001D9 RID: 473
		private static readonly FieldCodec<string> _repeated_reservedName_codec = FieldCodec.ForString(42U);

		// Token: 0x040001DA RID: 474
		private readonly RepeatedField<string> reservedName_ = new RepeatedField<string>();

		// Token: 0x020000D1 RID: 209
		[DebuggerNonUserCode]
		public static class Types
		{
			// Token: 0x02000117 RID: 279
			public sealed class EnumReservedRange : IMessage<EnumDescriptorProto.Types.EnumReservedRange>, IMessage, IEquatable<EnumDescriptorProto.Types.EnumReservedRange>, IDeepCloneable<EnumDescriptorProto.Types.EnumReservedRange>
			{
				// Token: 0x17000288 RID: 648
				// (get) Token: 0x06000AB5 RID: 2741 RVA: 0x00021C54 File Offset: 0x0001FE54
				[DebuggerNonUserCode]
				public static MessageParser<EnumDescriptorProto.Types.EnumReservedRange> Parser
				{
					get
					{
						return EnumDescriptorProto.Types.EnumReservedRange._parser;
					}
				}

				// Token: 0x17000289 RID: 649
				// (get) Token: 0x06000AB6 RID: 2742 RVA: 0x00021C5B File Offset: 0x0001FE5B
				[DebuggerNonUserCode]
				public static MessageDescriptor Descriptor
				{
					get
					{
						return EnumDescriptorProto.Descriptor.NestedTypes[0];
					}
				}

				// Token: 0x1700028A RID: 650
				// (get) Token: 0x06000AB7 RID: 2743 RVA: 0x00021C6D File Offset: 0x0001FE6D
				[DebuggerNonUserCode]
				MessageDescriptor IMessage.Descriptor
				{
					get
					{
						return EnumDescriptorProto.Types.EnumReservedRange.Descriptor;
					}
				}

				// Token: 0x06000AB8 RID: 2744 RVA: 0x00021C74 File Offset: 0x0001FE74
				[DebuggerNonUserCode]
				public EnumReservedRange()
				{
				}

				// Token: 0x06000AB9 RID: 2745 RVA: 0x00021C7C File Offset: 0x0001FE7C
				[DebuggerNonUserCode]
				public EnumReservedRange(EnumDescriptorProto.Types.EnumReservedRange other)
					: this()
				{
					this._hasBits0 = other._hasBits0;
					this.start_ = other.start_;
					this.end_ = other.end_;
					this._unknownFields = UnknownFieldSet.Clone(other._unknownFields);
				}

				// Token: 0x06000ABA RID: 2746 RVA: 0x00021CB9 File Offset: 0x0001FEB9
				[DebuggerNonUserCode]
				public EnumDescriptorProto.Types.EnumReservedRange Clone()
				{
					return new EnumDescriptorProto.Types.EnumReservedRange(this);
				}

				// Token: 0x1700028B RID: 651
				// (get) Token: 0x06000ABB RID: 2747 RVA: 0x00021CC1 File Offset: 0x0001FEC1
				// (set) Token: 0x06000ABC RID: 2748 RVA: 0x00021CD9 File Offset: 0x0001FED9
				[DebuggerNonUserCode]
				public int Start
				{
					get
					{
						if ((this._hasBits0 & 1) != 0)
						{
							return this.start_;
						}
						return EnumDescriptorProto.Types.EnumReservedRange.StartDefaultValue;
					}
					set
					{
						this._hasBits0 |= 1;
						this.start_ = value;
					}
				}

				// Token: 0x1700028C RID: 652
				// (get) Token: 0x06000ABD RID: 2749 RVA: 0x00021CF0 File Offset: 0x0001FEF0
				[DebuggerNonUserCode]
				public bool HasStart
				{
					get
					{
						return (this._hasBits0 & 1) != 0;
					}
				}

				// Token: 0x06000ABE RID: 2750 RVA: 0x00021CFD File Offset: 0x0001FEFD
				[DebuggerNonUserCode]
				public void ClearStart()
				{
					this._hasBits0 &= -2;
				}

				// Token: 0x1700028D RID: 653
				// (get) Token: 0x06000ABF RID: 2751 RVA: 0x00021D0E File Offset: 0x0001FF0E
				// (set) Token: 0x06000AC0 RID: 2752 RVA: 0x00021D26 File Offset: 0x0001FF26
				[DebuggerNonUserCode]
				public int End
				{
					get
					{
						if ((this._hasBits0 & 2) != 0)
						{
							return this.end_;
						}
						return EnumDescriptorProto.Types.EnumReservedRange.EndDefaultValue;
					}
					set
					{
						this._hasBits0 |= 2;
						this.end_ = value;
					}
				}

				// Token: 0x1700028E RID: 654
				// (get) Token: 0x06000AC1 RID: 2753 RVA: 0x00021D3D File Offset: 0x0001FF3D
				[DebuggerNonUserCode]
				public bool HasEnd
				{
					get
					{
						return (this._hasBits0 & 2) != 0;
					}
				}

				// Token: 0x06000AC2 RID: 2754 RVA: 0x00021D4A File Offset: 0x0001FF4A
				[DebuggerNonUserCode]
				public void ClearEnd()
				{
					this._hasBits0 &= -3;
				}

				// Token: 0x06000AC3 RID: 2755 RVA: 0x00021D5B File Offset: 0x0001FF5B
				[DebuggerNonUserCode]
				public override bool Equals(object other)
				{
					return this.Equals(other as EnumDescriptorProto.Types.EnumReservedRange);
				}

				// Token: 0x06000AC4 RID: 2756 RVA: 0x00021D69 File Offset: 0x0001FF69
				[DebuggerNonUserCode]
				public bool Equals(EnumDescriptorProto.Types.EnumReservedRange other)
				{
					return other != null && (other == this || (this.Start == other.Start && this.End == other.End && object.Equals(this._unknownFields, other._unknownFields)));
				}

				// Token: 0x06000AC5 RID: 2757 RVA: 0x00021DA8 File Offset: 0x0001FFA8
				[DebuggerNonUserCode]
				public override int GetHashCode()
				{
					int num = 1;
					if (this.HasStart)
					{
						num ^= this.Start.GetHashCode();
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

				// Token: 0x06000AC6 RID: 2758 RVA: 0x00021E00 File Offset: 0x00020000
				[DebuggerNonUserCode]
				public override string ToString()
				{
					return JsonFormatter.ToDiagnosticString(this);
				}

				// Token: 0x06000AC7 RID: 2759 RVA: 0x00021E08 File Offset: 0x00020008
				[DebuggerNonUserCode]
				public void WriteTo(CodedOutputStream output)
				{
					if (this.HasStart)
					{
						output.WriteRawTag(8);
						output.WriteInt32(this.Start);
					}
					if (this.HasEnd)
					{
						output.WriteRawTag(16);
						output.WriteInt32(this.End);
					}
					if (this._unknownFields != null)
					{
						this._unknownFields.WriteTo(output);
					}
				}

				// Token: 0x06000AC8 RID: 2760 RVA: 0x00021E60 File Offset: 0x00020060
				[DebuggerNonUserCode]
				public int CalculateSize()
				{
					int num = 0;
					if (this.HasStart)
					{
						num += 1 + CodedOutputStream.ComputeInt32Size(this.Start);
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

				// Token: 0x06000AC9 RID: 2761 RVA: 0x00021EB8 File Offset: 0x000200B8
				[DebuggerNonUserCode]
				public void MergeFrom(EnumDescriptorProto.Types.EnumReservedRange other)
				{
					if (other == null)
					{
						return;
					}
					if (other.HasStart)
					{
						this.Start = other.Start;
					}
					if (other.HasEnd)
					{
						this.End = other.End;
					}
					this._unknownFields = UnknownFieldSet.MergeFrom(this._unknownFields, other._unknownFields);
				}

				// Token: 0x06000ACA RID: 2762 RVA: 0x00021F08 File Offset: 0x00020108
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
								this.End = input.ReadInt32();
							}
						}
						else
						{
							this.Start = input.ReadInt32();
						}
					}
				}

				// Token: 0x0400049A RID: 1178
				private static readonly MessageParser<EnumDescriptorProto.Types.EnumReservedRange> _parser = new MessageParser<EnumDescriptorProto.Types.EnumReservedRange>(() => new EnumDescriptorProto.Types.EnumReservedRange());

				// Token: 0x0400049B RID: 1179
				private UnknownFieldSet _unknownFields;

				// Token: 0x0400049C RID: 1180
				private int _hasBits0;

				// Token: 0x0400049D RID: 1181
				public const int StartFieldNumber = 1;

				// Token: 0x0400049E RID: 1182
				private static readonly int StartDefaultValue = 0;

				// Token: 0x0400049F RID: 1183
				private int start_;

				// Token: 0x040004A0 RID: 1184
				public const int EndFieldNumber = 2;

				// Token: 0x040004A1 RID: 1185
				private static readonly int EndDefaultValue = 0;

				// Token: 0x040004A2 RID: 1186
				private int end_;
			}
		}
	}
}
