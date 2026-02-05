using System;
using System.Diagnostics;
using Google.Protobuf.Collections;

namespace Google.Protobuf.Reflection
{
	// Token: 0x02000062 RID: 98
	public sealed class UninterpretedOption : IMessage<UninterpretedOption>, IMessage, IEquatable<UninterpretedOption>, IDeepCloneable<UninterpretedOption>
	{
		// Token: 0x170001B6 RID: 438
		// (get) Token: 0x060006E6 RID: 1766 RVA: 0x000196AB File Offset: 0x000178AB
		[DebuggerNonUserCode]
		public static MessageParser<UninterpretedOption> Parser
		{
			get
			{
				return UninterpretedOption._parser;
			}
		}

		// Token: 0x170001B7 RID: 439
		// (get) Token: 0x060006E7 RID: 1767 RVA: 0x000196B2 File Offset: 0x000178B2
		[DebuggerNonUserCode]
		public static MessageDescriptor Descriptor
		{
			get
			{
				return DescriptorReflection.Descriptor.MessageTypes[18];
			}
		}

		// Token: 0x170001B8 RID: 440
		// (get) Token: 0x060006E8 RID: 1768 RVA: 0x000196C5 File Offset: 0x000178C5
		[DebuggerNonUserCode]
		MessageDescriptor IMessage.Descriptor
		{
			get
			{
				return UninterpretedOption.Descriptor;
			}
		}

		// Token: 0x060006E9 RID: 1769 RVA: 0x000196CC File Offset: 0x000178CC
		[DebuggerNonUserCode]
		public UninterpretedOption()
		{
		}

		// Token: 0x060006EA RID: 1770 RVA: 0x000196E0 File Offset: 0x000178E0
		[DebuggerNonUserCode]
		public UninterpretedOption(UninterpretedOption other)
			: this()
		{
			this._hasBits0 = other._hasBits0;
			this.name_ = other.name_.Clone();
			this.identifierValue_ = other.identifierValue_;
			this.positiveIntValue_ = other.positiveIntValue_;
			this.negativeIntValue_ = other.negativeIntValue_;
			this.doubleValue_ = other.doubleValue_;
			this.stringValue_ = other.stringValue_;
			this.aggregateValue_ = other.aggregateValue_;
			this._unknownFields = UnknownFieldSet.Clone(other._unknownFields);
		}

		// Token: 0x060006EB RID: 1771 RVA: 0x00019769 File Offset: 0x00017969
		[DebuggerNonUserCode]
		public UninterpretedOption Clone()
		{
			return new UninterpretedOption(this);
		}

		// Token: 0x170001B9 RID: 441
		// (get) Token: 0x060006EC RID: 1772 RVA: 0x00019771 File Offset: 0x00017971
		[DebuggerNonUserCode]
		public RepeatedField<UninterpretedOption.Types.NamePart> Name
		{
			get
			{
				return this.name_;
			}
		}

		// Token: 0x170001BA RID: 442
		// (get) Token: 0x060006ED RID: 1773 RVA: 0x00019779 File Offset: 0x00017979
		// (set) Token: 0x060006EE RID: 1774 RVA: 0x0001978A File Offset: 0x0001798A
		[DebuggerNonUserCode]
		public string IdentifierValue
		{
			get
			{
				return this.identifierValue_ ?? UninterpretedOption.IdentifierValueDefaultValue;
			}
			set
			{
				this.identifierValue_ = ProtoPreconditions.CheckNotNull<string>(value, "value");
			}
		}

		// Token: 0x170001BB RID: 443
		// (get) Token: 0x060006EF RID: 1775 RVA: 0x0001979D File Offset: 0x0001799D
		[DebuggerNonUserCode]
		public bool HasIdentifierValue
		{
			get
			{
				return this.identifierValue_ != null;
			}
		}

		// Token: 0x060006F0 RID: 1776 RVA: 0x000197A8 File Offset: 0x000179A8
		[DebuggerNonUserCode]
		public void ClearIdentifierValue()
		{
			this.identifierValue_ = null;
		}

		// Token: 0x170001BC RID: 444
		// (get) Token: 0x060006F1 RID: 1777 RVA: 0x000197B1 File Offset: 0x000179B1
		// (set) Token: 0x060006F2 RID: 1778 RVA: 0x000197C9 File Offset: 0x000179C9
		[DebuggerNonUserCode]
		public ulong PositiveIntValue
		{
			get
			{
				if ((this._hasBits0 & 1) != 0)
				{
					return this.positiveIntValue_;
				}
				return UninterpretedOption.PositiveIntValueDefaultValue;
			}
			set
			{
				this._hasBits0 |= 1;
				this.positiveIntValue_ = value;
			}
		}

		// Token: 0x170001BD RID: 445
		// (get) Token: 0x060006F3 RID: 1779 RVA: 0x000197E0 File Offset: 0x000179E0
		[DebuggerNonUserCode]
		public bool HasPositiveIntValue
		{
			get
			{
				return (this._hasBits0 & 1) != 0;
			}
		}

		// Token: 0x060006F4 RID: 1780 RVA: 0x000197ED File Offset: 0x000179ED
		[DebuggerNonUserCode]
		public void ClearPositiveIntValue()
		{
			this._hasBits0 &= -2;
		}

		// Token: 0x170001BE RID: 446
		// (get) Token: 0x060006F5 RID: 1781 RVA: 0x000197FE File Offset: 0x000179FE
		// (set) Token: 0x060006F6 RID: 1782 RVA: 0x00019816 File Offset: 0x00017A16
		[DebuggerNonUserCode]
		public long NegativeIntValue
		{
			get
			{
				if ((this._hasBits0 & 2) != 0)
				{
					return this.negativeIntValue_;
				}
				return UninterpretedOption.NegativeIntValueDefaultValue;
			}
			set
			{
				this._hasBits0 |= 2;
				this.negativeIntValue_ = value;
			}
		}

		// Token: 0x170001BF RID: 447
		// (get) Token: 0x060006F7 RID: 1783 RVA: 0x0001982D File Offset: 0x00017A2D
		[DebuggerNonUserCode]
		public bool HasNegativeIntValue
		{
			get
			{
				return (this._hasBits0 & 2) != 0;
			}
		}

		// Token: 0x060006F8 RID: 1784 RVA: 0x0001983A File Offset: 0x00017A3A
		[DebuggerNonUserCode]
		public void ClearNegativeIntValue()
		{
			this._hasBits0 &= -3;
		}

		// Token: 0x170001C0 RID: 448
		// (get) Token: 0x060006F9 RID: 1785 RVA: 0x0001984B File Offset: 0x00017A4B
		// (set) Token: 0x060006FA RID: 1786 RVA: 0x00019863 File Offset: 0x00017A63
		[DebuggerNonUserCode]
		public double DoubleValue
		{
			get
			{
				if ((this._hasBits0 & 4) != 0)
				{
					return this.doubleValue_;
				}
				return UninterpretedOption.DoubleValueDefaultValue;
			}
			set
			{
				this._hasBits0 |= 4;
				this.doubleValue_ = value;
			}
		}

		// Token: 0x170001C1 RID: 449
		// (get) Token: 0x060006FB RID: 1787 RVA: 0x0001987A File Offset: 0x00017A7A
		[DebuggerNonUserCode]
		public bool HasDoubleValue
		{
			get
			{
				return (this._hasBits0 & 4) != 0;
			}
		}

		// Token: 0x060006FC RID: 1788 RVA: 0x00019887 File Offset: 0x00017A87
		[DebuggerNonUserCode]
		public void ClearDoubleValue()
		{
			this._hasBits0 &= -5;
		}

		// Token: 0x170001C2 RID: 450
		// (get) Token: 0x060006FD RID: 1789 RVA: 0x00019898 File Offset: 0x00017A98
		// (set) Token: 0x060006FE RID: 1790 RVA: 0x000198A9 File Offset: 0x00017AA9
		[DebuggerNonUserCode]
		public ByteString StringValue
		{
			get
			{
				return this.stringValue_ ?? UninterpretedOption.StringValueDefaultValue;
			}
			set
			{
				this.stringValue_ = ProtoPreconditions.CheckNotNull<ByteString>(value, "value");
			}
		}

		// Token: 0x170001C3 RID: 451
		// (get) Token: 0x060006FF RID: 1791 RVA: 0x000198BC File Offset: 0x00017ABC
		[DebuggerNonUserCode]
		public bool HasStringValue
		{
			get
			{
				return this.stringValue_ != null;
			}
		}

		// Token: 0x06000700 RID: 1792 RVA: 0x000198CA File Offset: 0x00017ACA
		[DebuggerNonUserCode]
		public void ClearStringValue()
		{
			this.stringValue_ = null;
		}

		// Token: 0x170001C4 RID: 452
		// (get) Token: 0x06000701 RID: 1793 RVA: 0x000198D3 File Offset: 0x00017AD3
		// (set) Token: 0x06000702 RID: 1794 RVA: 0x000198E4 File Offset: 0x00017AE4
		[DebuggerNonUserCode]
		public string AggregateValue
		{
			get
			{
				return this.aggregateValue_ ?? UninterpretedOption.AggregateValueDefaultValue;
			}
			set
			{
				this.aggregateValue_ = ProtoPreconditions.CheckNotNull<string>(value, "value");
			}
		}

		// Token: 0x170001C5 RID: 453
		// (get) Token: 0x06000703 RID: 1795 RVA: 0x000198F7 File Offset: 0x00017AF7
		[DebuggerNonUserCode]
		public bool HasAggregateValue
		{
			get
			{
				return this.aggregateValue_ != null;
			}
		}

		// Token: 0x06000704 RID: 1796 RVA: 0x00019902 File Offset: 0x00017B02
		[DebuggerNonUserCode]
		public void ClearAggregateValue()
		{
			this.aggregateValue_ = null;
		}

		// Token: 0x06000705 RID: 1797 RVA: 0x0001990B File Offset: 0x00017B0B
		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return this.Equals(other as UninterpretedOption);
		}

		// Token: 0x06000706 RID: 1798 RVA: 0x0001991C File Offset: 0x00017B1C
		[DebuggerNonUserCode]
		public bool Equals(UninterpretedOption other)
		{
			return other != null && (other == this || (this.name_.Equals(other.name_) && !(this.IdentifierValue != other.IdentifierValue) && this.PositiveIntValue == other.PositiveIntValue && this.NegativeIntValue == other.NegativeIntValue && ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.Equals(this.DoubleValue, other.DoubleValue) && !(this.StringValue != other.StringValue) && !(this.AggregateValue != other.AggregateValue) && object.Equals(this._unknownFields, other._unknownFields)));
		}

		// Token: 0x06000707 RID: 1799 RVA: 0x000199D4 File Offset: 0x00017BD4
		[DebuggerNonUserCode]
		public override int GetHashCode()
		{
			int num = 1;
			num ^= this.name_.GetHashCode();
			if (this.HasIdentifierValue)
			{
				num ^= this.IdentifierValue.GetHashCode();
			}
			if (this.HasPositiveIntValue)
			{
				num ^= this.PositiveIntValue.GetHashCode();
			}
			if (this.HasNegativeIntValue)
			{
				num ^= this.NegativeIntValue.GetHashCode();
			}
			if (this.HasDoubleValue)
			{
				num ^= ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.GetHashCode(this.DoubleValue);
			}
			if (this.HasStringValue)
			{
				num ^= this.StringValue.GetHashCode();
			}
			if (this.HasAggregateValue)
			{
				num ^= this.AggregateValue.GetHashCode();
			}
			if (this._unknownFields != null)
			{
				num ^= this._unknownFields.GetHashCode();
			}
			return num;
		}

		// Token: 0x06000708 RID: 1800 RVA: 0x00019A97 File Offset: 0x00017C97
		[DebuggerNonUserCode]
		public override string ToString()
		{
			return JsonFormatter.ToDiagnosticString(this);
		}

		// Token: 0x06000709 RID: 1801 RVA: 0x00019AA0 File Offset: 0x00017CA0
		[DebuggerNonUserCode]
		public void WriteTo(CodedOutputStream output)
		{
			this.name_.WriteTo(output, UninterpretedOption._repeated_name_codec);
			if (this.HasIdentifierValue)
			{
				output.WriteRawTag(26);
				output.WriteString(this.IdentifierValue);
			}
			if (this.HasPositiveIntValue)
			{
				output.WriteRawTag(32);
				output.WriteUInt64(this.PositiveIntValue);
			}
			if (this.HasNegativeIntValue)
			{
				output.WriteRawTag(40);
				output.WriteInt64(this.NegativeIntValue);
			}
			if (this.HasDoubleValue)
			{
				output.WriteRawTag(49);
				output.WriteDouble(this.DoubleValue);
			}
			if (this.HasStringValue)
			{
				output.WriteRawTag(58);
				output.WriteBytes(this.StringValue);
			}
			if (this.HasAggregateValue)
			{
				output.WriteRawTag(66);
				output.WriteString(this.AggregateValue);
			}
			if (this._unknownFields != null)
			{
				this._unknownFields.WriteTo(output);
			}
		}

		// Token: 0x0600070A RID: 1802 RVA: 0x00019B7C File Offset: 0x00017D7C
		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			num += this.name_.CalculateSize(UninterpretedOption._repeated_name_codec);
			if (this.HasIdentifierValue)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.IdentifierValue);
			}
			if (this.HasPositiveIntValue)
			{
				num += 1 + CodedOutputStream.ComputeUInt64Size(this.PositiveIntValue);
			}
			if (this.HasNegativeIntValue)
			{
				num += 1 + CodedOutputStream.ComputeInt64Size(this.NegativeIntValue);
			}
			if (this.HasDoubleValue)
			{
				num += 9;
			}
			if (this.HasStringValue)
			{
				num += 1 + CodedOutputStream.ComputeBytesSize(this.StringValue);
			}
			if (this.HasAggregateValue)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.AggregateValue);
			}
			if (this._unknownFields != null)
			{
				num += this._unknownFields.CalculateSize();
			}
			return num;
		}

		// Token: 0x0600070B RID: 1803 RVA: 0x00019C3C File Offset: 0x00017E3C
		[DebuggerNonUserCode]
		public void MergeFrom(UninterpretedOption other)
		{
			if (other == null)
			{
				return;
			}
			this.name_.Add(other.name_);
			if (other.HasIdentifierValue)
			{
				this.IdentifierValue = other.IdentifierValue;
			}
			if (other.HasPositiveIntValue)
			{
				this.PositiveIntValue = other.PositiveIntValue;
			}
			if (other.HasNegativeIntValue)
			{
				this.NegativeIntValue = other.NegativeIntValue;
			}
			if (other.HasDoubleValue)
			{
				this.DoubleValue = other.DoubleValue;
			}
			if (other.HasStringValue)
			{
				this.StringValue = other.StringValue;
			}
			if (other.HasAggregateValue)
			{
				this.AggregateValue = other.AggregateValue;
			}
			this._unknownFields = UnknownFieldSet.MergeFrom(this._unknownFields, other._unknownFields);
		}

		// Token: 0x0600070C RID: 1804 RVA: 0x00019CF0 File Offset: 0x00017EF0
		[DebuggerNonUserCode]
		public void MergeFrom(CodedInputStream input)
		{
			uint num;
			while ((num = input.ReadTag()) != 0U)
			{
				if (num <= 32U)
				{
					if (num == 18U)
					{
						this.name_.AddEntriesFrom(input, UninterpretedOption._repeated_name_codec);
						continue;
					}
					if (num == 26U)
					{
						this.IdentifierValue = input.ReadString();
						continue;
					}
					if (num == 32U)
					{
						this.PositiveIntValue = input.ReadUInt64();
						continue;
					}
				}
				else if (num <= 49U)
				{
					if (num == 40U)
					{
						this.NegativeIntValue = input.ReadInt64();
						continue;
					}
					if (num == 49U)
					{
						this.DoubleValue = input.ReadDouble();
						continue;
					}
				}
				else
				{
					if (num == 58U)
					{
						this.StringValue = input.ReadBytes();
						continue;
					}
					if (num == 66U)
					{
						this.AggregateValue = input.ReadString();
						continue;
					}
				}
				this._unknownFields = UnknownFieldSet.MergeFieldFrom(this._unknownFields, input);
			}
		}

		// Token: 0x040002A7 RID: 679
		private static readonly MessageParser<UninterpretedOption> _parser = new MessageParser<UninterpretedOption>(() => new UninterpretedOption());

		// Token: 0x040002A8 RID: 680
		private UnknownFieldSet _unknownFields;

		// Token: 0x040002A9 RID: 681
		private int _hasBits0;

		// Token: 0x040002AA RID: 682
		public const int NameFieldNumber = 2;

		// Token: 0x040002AB RID: 683
		private static readonly FieldCodec<UninterpretedOption.Types.NamePart> _repeated_name_codec = FieldCodec.ForMessage<UninterpretedOption.Types.NamePart>(18U, UninterpretedOption.Types.NamePart.Parser);

		// Token: 0x040002AC RID: 684
		private readonly RepeatedField<UninterpretedOption.Types.NamePart> name_ = new RepeatedField<UninterpretedOption.Types.NamePart>();

		// Token: 0x040002AD RID: 685
		public const int IdentifierValueFieldNumber = 3;

		// Token: 0x040002AE RID: 686
		private static readonly string IdentifierValueDefaultValue = "";

		// Token: 0x040002AF RID: 687
		private string identifierValue_;

		// Token: 0x040002B0 RID: 688
		public const int PositiveIntValueFieldNumber = 4;

		// Token: 0x040002B1 RID: 689
		private static readonly ulong PositiveIntValueDefaultValue = 0UL;

		// Token: 0x040002B2 RID: 690
		private ulong positiveIntValue_;

		// Token: 0x040002B3 RID: 691
		public const int NegativeIntValueFieldNumber = 5;

		// Token: 0x040002B4 RID: 692
		private static readonly long NegativeIntValueDefaultValue = 0L;

		// Token: 0x040002B5 RID: 693
		private long negativeIntValue_;

		// Token: 0x040002B6 RID: 694
		public const int DoubleValueFieldNumber = 6;

		// Token: 0x040002B7 RID: 695
		private static readonly double DoubleValueDefaultValue = 0.0;

		// Token: 0x040002B8 RID: 696
		private double doubleValue_;

		// Token: 0x040002B9 RID: 697
		public const int StringValueFieldNumber = 7;

		// Token: 0x040002BA RID: 698
		private static readonly ByteString StringValueDefaultValue = ByteString.Empty;

		// Token: 0x040002BB RID: 699
		private ByteString stringValue_;

		// Token: 0x040002BC RID: 700
		public const int AggregateValueFieldNumber = 8;

		// Token: 0x040002BD RID: 701
		private static readonly string AggregateValueDefaultValue = "";

		// Token: 0x040002BE RID: 702
		private string aggregateValue_;

		// Token: 0x020000E1 RID: 225
		[DebuggerNonUserCode]
		public static class Types
		{
			// Token: 0x0200011C RID: 284
			public sealed class NamePart : IMessage<UninterpretedOption.Types.NamePart>, IMessage, IEquatable<UninterpretedOption.Types.NamePart>, IDeepCloneable<UninterpretedOption.Types.NamePart>
			{
				// Token: 0x1700028F RID: 655
				// (get) Token: 0x06000ACC RID: 2764 RVA: 0x00021F80 File Offset: 0x00020180
				[DebuggerNonUserCode]
				public static MessageParser<UninterpretedOption.Types.NamePart> Parser
				{
					get
					{
						return UninterpretedOption.Types.NamePart._parser;
					}
				}

				// Token: 0x17000290 RID: 656
				// (get) Token: 0x06000ACD RID: 2765 RVA: 0x00021F87 File Offset: 0x00020187
				[DebuggerNonUserCode]
				public static MessageDescriptor Descriptor
				{
					get
					{
						return UninterpretedOption.Descriptor.NestedTypes[0];
					}
				}

				// Token: 0x17000291 RID: 657
				// (get) Token: 0x06000ACE RID: 2766 RVA: 0x00021F99 File Offset: 0x00020199
				[DebuggerNonUserCode]
				MessageDescriptor IMessage.Descriptor
				{
					get
					{
						return UninterpretedOption.Types.NamePart.Descriptor;
					}
				}

				// Token: 0x06000ACF RID: 2767 RVA: 0x00021FA0 File Offset: 0x000201A0
				[DebuggerNonUserCode]
				public NamePart()
				{
				}

				// Token: 0x06000AD0 RID: 2768 RVA: 0x00021FA8 File Offset: 0x000201A8
				[DebuggerNonUserCode]
				public NamePart(UninterpretedOption.Types.NamePart other)
					: this()
				{
					this._hasBits0 = other._hasBits0;
					this.namePart_ = other.namePart_;
					this.isExtension_ = other.isExtension_;
					this._unknownFields = UnknownFieldSet.Clone(other._unknownFields);
				}

				// Token: 0x06000AD1 RID: 2769 RVA: 0x00021FE5 File Offset: 0x000201E5
				[DebuggerNonUserCode]
				public UninterpretedOption.Types.NamePart Clone()
				{
					return new UninterpretedOption.Types.NamePart(this);
				}

				// Token: 0x17000292 RID: 658
				// (get) Token: 0x06000AD2 RID: 2770 RVA: 0x00021FED File Offset: 0x000201ED
				// (set) Token: 0x06000AD3 RID: 2771 RVA: 0x00021FFE File Offset: 0x000201FE
				[DebuggerNonUserCode]
				public string NamePart_
				{
					get
					{
						return this.namePart_ ?? UninterpretedOption.Types.NamePart.NamePart_DefaultValue;
					}
					set
					{
						this.namePart_ = ProtoPreconditions.CheckNotNull<string>(value, "value");
					}
				}

				// Token: 0x17000293 RID: 659
				// (get) Token: 0x06000AD4 RID: 2772 RVA: 0x00022011 File Offset: 0x00020211
				[DebuggerNonUserCode]
				public bool HasNamePart_
				{
					get
					{
						return this.namePart_ != null;
					}
				}

				// Token: 0x06000AD5 RID: 2773 RVA: 0x0002201C File Offset: 0x0002021C
				[DebuggerNonUserCode]
				public void ClearNamePart_()
				{
					this.namePart_ = null;
				}

				// Token: 0x17000294 RID: 660
				// (get) Token: 0x06000AD6 RID: 2774 RVA: 0x00022025 File Offset: 0x00020225
				// (set) Token: 0x06000AD7 RID: 2775 RVA: 0x0002203D File Offset: 0x0002023D
				[DebuggerNonUserCode]
				public bool IsExtension
				{
					get
					{
						if ((this._hasBits0 & 1) != 0)
						{
							return this.isExtension_;
						}
						return UninterpretedOption.Types.NamePart.IsExtensionDefaultValue;
					}
					set
					{
						this._hasBits0 |= 1;
						this.isExtension_ = value;
					}
				}

				// Token: 0x17000295 RID: 661
				// (get) Token: 0x06000AD8 RID: 2776 RVA: 0x00022054 File Offset: 0x00020254
				[DebuggerNonUserCode]
				public bool HasIsExtension
				{
					get
					{
						return (this._hasBits0 & 1) != 0;
					}
				}

				// Token: 0x06000AD9 RID: 2777 RVA: 0x00022061 File Offset: 0x00020261
				[DebuggerNonUserCode]
				public void ClearIsExtension()
				{
					this._hasBits0 &= -2;
				}

				// Token: 0x06000ADA RID: 2778 RVA: 0x00022072 File Offset: 0x00020272
				[DebuggerNonUserCode]
				public override bool Equals(object other)
				{
					return this.Equals(other as UninterpretedOption.Types.NamePart);
				}

				// Token: 0x06000ADB RID: 2779 RVA: 0x00022080 File Offset: 0x00020280
				[DebuggerNonUserCode]
				public bool Equals(UninterpretedOption.Types.NamePart other)
				{
					return other != null && (other == this || (!(this.NamePart_ != other.NamePart_) && this.IsExtension == other.IsExtension && object.Equals(this._unknownFields, other._unknownFields)));
				}

				// Token: 0x06000ADC RID: 2780 RVA: 0x000220D0 File Offset: 0x000202D0
				[DebuggerNonUserCode]
				public override int GetHashCode()
				{
					int num = 1;
					if (this.HasNamePart_)
					{
						num ^= this.NamePart_.GetHashCode();
					}
					if (this.HasIsExtension)
					{
						num ^= this.IsExtension.GetHashCode();
					}
					if (this._unknownFields != null)
					{
						num ^= this._unknownFields.GetHashCode();
					}
					return num;
				}

				// Token: 0x06000ADD RID: 2781 RVA: 0x00022125 File Offset: 0x00020325
				[DebuggerNonUserCode]
				public override string ToString()
				{
					return JsonFormatter.ToDiagnosticString(this);
				}

				// Token: 0x06000ADE RID: 2782 RVA: 0x00022130 File Offset: 0x00020330
				[DebuggerNonUserCode]
				public void WriteTo(CodedOutputStream output)
				{
					if (this.HasNamePart_)
					{
						output.WriteRawTag(10);
						output.WriteString(this.NamePart_);
					}
					if (this.HasIsExtension)
					{
						output.WriteRawTag(16);
						output.WriteBool(this.IsExtension);
					}
					if (this._unknownFields != null)
					{
						this._unknownFields.WriteTo(output);
					}
				}

				// Token: 0x06000ADF RID: 2783 RVA: 0x0002218C File Offset: 0x0002038C
				[DebuggerNonUserCode]
				public int CalculateSize()
				{
					int num = 0;
					if (this.HasNamePart_)
					{
						num += 1 + CodedOutputStream.ComputeStringSize(this.NamePart_);
					}
					if (this.HasIsExtension)
					{
						num += 2;
					}
					if (this._unknownFields != null)
					{
						num += this._unknownFields.CalculateSize();
					}
					return num;
				}

				// Token: 0x06000AE0 RID: 2784 RVA: 0x000221D8 File Offset: 0x000203D8
				[DebuggerNonUserCode]
				public void MergeFrom(UninterpretedOption.Types.NamePart other)
				{
					if (other == null)
					{
						return;
					}
					if (other.HasNamePart_)
					{
						this.NamePart_ = other.NamePart_;
					}
					if (other.HasIsExtension)
					{
						this.IsExtension = other.IsExtension;
					}
					this._unknownFields = UnknownFieldSet.MergeFrom(this._unknownFields, other._unknownFields);
				}

				// Token: 0x06000AE1 RID: 2785 RVA: 0x00022228 File Offset: 0x00020428
				[DebuggerNonUserCode]
				public void MergeFrom(CodedInputStream input)
				{
					uint num;
					while ((num = input.ReadTag()) != 0U)
					{
						if (num != 10U)
						{
							if (num != 16U)
							{
								this._unknownFields = UnknownFieldSet.MergeFieldFrom(this._unknownFields, input);
							}
							else
							{
								this.IsExtension = input.ReadBool();
							}
						}
						else
						{
							this.NamePart_ = input.ReadString();
						}
					}
				}

				// Token: 0x040004B3 RID: 1203
				private static readonly MessageParser<UninterpretedOption.Types.NamePart> _parser = new MessageParser<UninterpretedOption.Types.NamePart>(() => new UninterpretedOption.Types.NamePart());

				// Token: 0x040004B4 RID: 1204
				private UnknownFieldSet _unknownFields;

				// Token: 0x040004B5 RID: 1205
				private int _hasBits0;

				// Token: 0x040004B6 RID: 1206
				public const int NamePart_FieldNumber = 1;

				// Token: 0x040004B7 RID: 1207
				private static readonly string NamePart_DefaultValue = "";

				// Token: 0x040004B8 RID: 1208
				private string namePart_;

				// Token: 0x040004B9 RID: 1209
				public const int IsExtensionFieldNumber = 2;

				// Token: 0x040004BA RID: 1210
				private static readonly bool IsExtensionDefaultValue = false;

				// Token: 0x040004BB RID: 1211
				private bool isExtension_;
			}
		}
	}
}
