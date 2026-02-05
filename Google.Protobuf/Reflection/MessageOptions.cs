using System;
using System.Diagnostics;
using Google.Protobuf.Collections;

namespace Google.Protobuf.Reflection
{
	// Token: 0x0200005B RID: 91
	public sealed class MessageOptions : IExtendableMessage<MessageOptions>, IMessage<MessageOptions>, IMessage, IEquatable<MessageOptions>, IDeepCloneable<MessageOptions>
	{
		// Token: 0x17000173 RID: 371
		// (get) Token: 0x060005FE RID: 1534 RVA: 0x000172FF File Offset: 0x000154FF
		private ExtensionSet<MessageOptions> _Extensions
		{
			get
			{
				return this._extensions;
			}
		}

		// Token: 0x17000174 RID: 372
		// (get) Token: 0x060005FF RID: 1535 RVA: 0x00017307 File Offset: 0x00015507
		[DebuggerNonUserCode]
		public static MessageParser<MessageOptions> Parser
		{
			get
			{
				return MessageOptions._parser;
			}
		}

		// Token: 0x17000175 RID: 373
		// (get) Token: 0x06000600 RID: 1536 RVA: 0x0001730E File Offset: 0x0001550E
		[DebuggerNonUserCode]
		public static MessageDescriptor Descriptor
		{
			get
			{
				return DescriptorReflection.Descriptor.MessageTypes[11];
			}
		}

		// Token: 0x17000176 RID: 374
		// (get) Token: 0x06000601 RID: 1537 RVA: 0x00017321 File Offset: 0x00015521
		[DebuggerNonUserCode]
		MessageDescriptor IMessage.Descriptor
		{
			get
			{
				return MessageOptions.Descriptor;
			}
		}

		// Token: 0x06000602 RID: 1538 RVA: 0x00017328 File Offset: 0x00015528
		[DebuggerNonUserCode]
		public MessageOptions()
		{
		}

		// Token: 0x06000603 RID: 1539 RVA: 0x0001733C File Offset: 0x0001553C
		[DebuggerNonUserCode]
		public MessageOptions(MessageOptions other)
			: this()
		{
			this._hasBits0 = other._hasBits0;
			this.messageSetWireFormat_ = other.messageSetWireFormat_;
			this.noStandardDescriptorAccessor_ = other.noStandardDescriptorAccessor_;
			this.deprecated_ = other.deprecated_;
			this.mapEntry_ = other.mapEntry_;
			this.uninterpretedOption_ = other.uninterpretedOption_.Clone();
			this._unknownFields = UnknownFieldSet.Clone(other._unknownFields);
			this._extensions = ExtensionSet.Clone<MessageOptions>(other._extensions);
		}

		// Token: 0x06000604 RID: 1540 RVA: 0x000173BE File Offset: 0x000155BE
		[DebuggerNonUserCode]
		public MessageOptions Clone()
		{
			return new MessageOptions(this);
		}

		// Token: 0x17000177 RID: 375
		// (get) Token: 0x06000605 RID: 1541 RVA: 0x000173C6 File Offset: 0x000155C6
		// (set) Token: 0x06000606 RID: 1542 RVA: 0x000173DE File Offset: 0x000155DE
		[DebuggerNonUserCode]
		public bool MessageSetWireFormat
		{
			get
			{
				if ((this._hasBits0 & 1) != 0)
				{
					return this.messageSetWireFormat_;
				}
				return MessageOptions.MessageSetWireFormatDefaultValue;
			}
			set
			{
				this._hasBits0 |= 1;
				this.messageSetWireFormat_ = value;
			}
		}

		// Token: 0x17000178 RID: 376
		// (get) Token: 0x06000607 RID: 1543 RVA: 0x000173F5 File Offset: 0x000155F5
		[DebuggerNonUserCode]
		public bool HasMessageSetWireFormat
		{
			get
			{
				return (this._hasBits0 & 1) != 0;
			}
		}

		// Token: 0x06000608 RID: 1544 RVA: 0x00017402 File Offset: 0x00015602
		[DebuggerNonUserCode]
		public void ClearMessageSetWireFormat()
		{
			this._hasBits0 &= -2;
		}

		// Token: 0x17000179 RID: 377
		// (get) Token: 0x06000609 RID: 1545 RVA: 0x00017413 File Offset: 0x00015613
		// (set) Token: 0x0600060A RID: 1546 RVA: 0x0001742B File Offset: 0x0001562B
		[DebuggerNonUserCode]
		public bool NoStandardDescriptorAccessor
		{
			get
			{
				if ((this._hasBits0 & 2) != 0)
				{
					return this.noStandardDescriptorAccessor_;
				}
				return MessageOptions.NoStandardDescriptorAccessorDefaultValue;
			}
			set
			{
				this._hasBits0 |= 2;
				this.noStandardDescriptorAccessor_ = value;
			}
		}

		// Token: 0x1700017A RID: 378
		// (get) Token: 0x0600060B RID: 1547 RVA: 0x00017442 File Offset: 0x00015642
		[DebuggerNonUserCode]
		public bool HasNoStandardDescriptorAccessor
		{
			get
			{
				return (this._hasBits0 & 2) != 0;
			}
		}

		// Token: 0x0600060C RID: 1548 RVA: 0x0001744F File Offset: 0x0001564F
		[DebuggerNonUserCode]
		public void ClearNoStandardDescriptorAccessor()
		{
			this._hasBits0 &= -3;
		}

		// Token: 0x1700017B RID: 379
		// (get) Token: 0x0600060D RID: 1549 RVA: 0x00017460 File Offset: 0x00015660
		// (set) Token: 0x0600060E RID: 1550 RVA: 0x00017478 File Offset: 0x00015678
		[DebuggerNonUserCode]
		public bool Deprecated
		{
			get
			{
				if ((this._hasBits0 & 4) != 0)
				{
					return this.deprecated_;
				}
				return MessageOptions.DeprecatedDefaultValue;
			}
			set
			{
				this._hasBits0 |= 4;
				this.deprecated_ = value;
			}
		}

		// Token: 0x1700017C RID: 380
		// (get) Token: 0x0600060F RID: 1551 RVA: 0x0001748F File Offset: 0x0001568F
		[DebuggerNonUserCode]
		public bool HasDeprecated
		{
			get
			{
				return (this._hasBits0 & 4) != 0;
			}
		}

		// Token: 0x06000610 RID: 1552 RVA: 0x0001749C File Offset: 0x0001569C
		[DebuggerNonUserCode]
		public void ClearDeprecated()
		{
			this._hasBits0 &= -5;
		}

		// Token: 0x1700017D RID: 381
		// (get) Token: 0x06000611 RID: 1553 RVA: 0x000174AD File Offset: 0x000156AD
		// (set) Token: 0x06000612 RID: 1554 RVA: 0x000174C5 File Offset: 0x000156C5
		[DebuggerNonUserCode]
		public bool MapEntry
		{
			get
			{
				if ((this._hasBits0 & 8) != 0)
				{
					return this.mapEntry_;
				}
				return MessageOptions.MapEntryDefaultValue;
			}
			set
			{
				this._hasBits0 |= 8;
				this.mapEntry_ = value;
			}
		}

		// Token: 0x1700017E RID: 382
		// (get) Token: 0x06000613 RID: 1555 RVA: 0x000174DC File Offset: 0x000156DC
		[DebuggerNonUserCode]
		public bool HasMapEntry
		{
			get
			{
				return (this._hasBits0 & 8) != 0;
			}
		}

		// Token: 0x06000614 RID: 1556 RVA: 0x000174E9 File Offset: 0x000156E9
		[DebuggerNonUserCode]
		public void ClearMapEntry()
		{
			this._hasBits0 &= -9;
		}

		// Token: 0x1700017F RID: 383
		// (get) Token: 0x06000615 RID: 1557 RVA: 0x000174FA File Offset: 0x000156FA
		[DebuggerNonUserCode]
		public RepeatedField<UninterpretedOption> UninterpretedOption
		{
			get
			{
				return this.uninterpretedOption_;
			}
		}

		// Token: 0x06000616 RID: 1558 RVA: 0x00017502 File Offset: 0x00015702
		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return this.Equals(other as MessageOptions);
		}

		// Token: 0x06000617 RID: 1559 RVA: 0x00017510 File Offset: 0x00015710
		[DebuggerNonUserCode]
		public bool Equals(MessageOptions other)
		{
			return other != null && (other == this || (this.MessageSetWireFormat == other.MessageSetWireFormat && this.NoStandardDescriptorAccessor == other.NoStandardDescriptorAccessor && this.Deprecated == other.Deprecated && this.MapEntry == other.MapEntry && this.uninterpretedOption_.Equals(other.uninterpretedOption_) && object.Equals(this._extensions, other._extensions) && object.Equals(this._unknownFields, other._unknownFields)));
		}

		// Token: 0x06000618 RID: 1560 RVA: 0x000175A4 File Offset: 0x000157A4
		[DebuggerNonUserCode]
		public override int GetHashCode()
		{
			int num = 1;
			if (this.HasMessageSetWireFormat)
			{
				num ^= this.MessageSetWireFormat.GetHashCode();
			}
			if (this.HasNoStandardDescriptorAccessor)
			{
				num ^= this.NoStandardDescriptorAccessor.GetHashCode();
			}
			if (this.HasDeprecated)
			{
				num ^= this.Deprecated.GetHashCode();
			}
			if (this.HasMapEntry)
			{
				num ^= this.MapEntry.GetHashCode();
			}
			num ^= this.uninterpretedOption_.GetHashCode();
			if (this._extensions != null)
			{
				num ^= this._extensions.GetHashCode();
			}
			if (this._unknownFields != null)
			{
				num ^= this._unknownFields.GetHashCode();
			}
			return num;
		}

		// Token: 0x06000619 RID: 1561 RVA: 0x00017652 File Offset: 0x00015852
		[DebuggerNonUserCode]
		public override string ToString()
		{
			return JsonFormatter.ToDiagnosticString(this);
		}

		// Token: 0x0600061A RID: 1562 RVA: 0x0001765C File Offset: 0x0001585C
		[DebuggerNonUserCode]
		public void WriteTo(CodedOutputStream output)
		{
			if (this.HasMessageSetWireFormat)
			{
				output.WriteRawTag(8);
				output.WriteBool(this.MessageSetWireFormat);
			}
			if (this.HasNoStandardDescriptorAccessor)
			{
				output.WriteRawTag(16);
				output.WriteBool(this.NoStandardDescriptorAccessor);
			}
			if (this.HasDeprecated)
			{
				output.WriteRawTag(24);
				output.WriteBool(this.Deprecated);
			}
			if (this.HasMapEntry)
			{
				output.WriteRawTag(56);
				output.WriteBool(this.MapEntry);
			}
			this.uninterpretedOption_.WriteTo(output, MessageOptions._repeated_uninterpretedOption_codec);
			if (this._extensions != null)
			{
				this._extensions.WriteTo(output);
			}
			if (this._unknownFields != null)
			{
				this._unknownFields.WriteTo(output);
			}
		}

		// Token: 0x0600061B RID: 1563 RVA: 0x00017714 File Offset: 0x00015914
		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (this.HasMessageSetWireFormat)
			{
				num += 2;
			}
			if (this.HasNoStandardDescriptorAccessor)
			{
				num += 2;
			}
			if (this.HasDeprecated)
			{
				num += 2;
			}
			if (this.HasMapEntry)
			{
				num += 2;
			}
			num += this.uninterpretedOption_.CalculateSize(MessageOptions._repeated_uninterpretedOption_codec);
			if (this._extensions != null)
			{
				num += this._extensions.CalculateSize();
			}
			if (this._unknownFields != null)
			{
				num += this._unknownFields.CalculateSize();
			}
			return num;
		}

		// Token: 0x0600061C RID: 1564 RVA: 0x00017794 File Offset: 0x00015994
		[DebuggerNonUserCode]
		public void MergeFrom(MessageOptions other)
		{
			if (other == null)
			{
				return;
			}
			if (other.HasMessageSetWireFormat)
			{
				this.MessageSetWireFormat = other.MessageSetWireFormat;
			}
			if (other.HasNoStandardDescriptorAccessor)
			{
				this.NoStandardDescriptorAccessor = other.NoStandardDescriptorAccessor;
			}
			if (other.HasDeprecated)
			{
				this.Deprecated = other.Deprecated;
			}
			if (other.HasMapEntry)
			{
				this.MapEntry = other.MapEntry;
			}
			this.uninterpretedOption_.Add(other.uninterpretedOption_);
			ExtensionSet.MergeFrom<MessageOptions>(ref this._extensions, other._extensions);
			this._unknownFields = UnknownFieldSet.MergeFrom(this._unknownFields, other._unknownFields);
		}

		// Token: 0x0600061D RID: 1565 RVA: 0x00017830 File Offset: 0x00015A30
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
						this.MessageSetWireFormat = input.ReadBool();
						continue;
					}
					if (num == 16U)
					{
						this.NoStandardDescriptorAccessor = input.ReadBool();
						continue;
					}
				}
				else
				{
					if (num == 24U)
					{
						this.Deprecated = input.ReadBool();
						continue;
					}
					if (num == 56U)
					{
						this.MapEntry = input.ReadBool();
						continue;
					}
					if (num == 7994U)
					{
						this.uninterpretedOption_.AddEntriesFrom(input, MessageOptions._repeated_uninterpretedOption_codec);
						continue;
					}
				}
				if (!ExtensionSet.TryMergeFieldFrom<MessageOptions>(ref this._extensions, input))
				{
					this._unknownFields = UnknownFieldSet.MergeFieldFrom(this._unknownFields, input);
				}
			}
		}

		// Token: 0x0600061E RID: 1566 RVA: 0x000178DC File Offset: 0x00015ADC
		public TValue GetExtension<TValue>(Extension<MessageOptions, TValue> extension)
		{
			return ExtensionSet.Get<MessageOptions, TValue>(ref this._extensions, extension);
		}

		// Token: 0x0600061F RID: 1567 RVA: 0x000178EA File Offset: 0x00015AEA
		public RepeatedField<TValue> GetExtension<TValue>(RepeatedExtension<MessageOptions, TValue> extension)
		{
			return ExtensionSet.Get<MessageOptions, TValue>(ref this._extensions, extension);
		}

		// Token: 0x06000620 RID: 1568 RVA: 0x000178F8 File Offset: 0x00015AF8
		public RepeatedField<TValue> GetOrInitializeExtension<TValue>(RepeatedExtension<MessageOptions, TValue> extension)
		{
			return ExtensionSet.GetOrInitialize<MessageOptions, TValue>(ref this._extensions, extension);
		}

		// Token: 0x06000621 RID: 1569 RVA: 0x00017906 File Offset: 0x00015B06
		public void SetExtension<TValue>(Extension<MessageOptions, TValue> extension, TValue value)
		{
			ExtensionSet.Set<MessageOptions, TValue>(ref this._extensions, extension, value);
		}

		// Token: 0x06000622 RID: 1570 RVA: 0x00017915 File Offset: 0x00015B15
		public bool HasExtension<TValue>(Extension<MessageOptions, TValue> extension)
		{
			return ExtensionSet.Has<MessageOptions, TValue>(ref this._extensions, extension);
		}

		// Token: 0x06000623 RID: 1571 RVA: 0x00017923 File Offset: 0x00015B23
		public void ClearExtension<TValue>(Extension<MessageOptions, TValue> extension)
		{
			ExtensionSet.Clear<MessageOptions, TValue>(ref this._extensions, extension);
		}

		// Token: 0x06000624 RID: 1572 RVA: 0x00017931 File Offset: 0x00015B31
		public void ClearExtension<TValue>(RepeatedExtension<MessageOptions, TValue> extension)
		{
			ExtensionSet.Clear<MessageOptions, TValue>(ref this._extensions, extension);
		}

		// Token: 0x04000247 RID: 583
		private static readonly MessageParser<MessageOptions> _parser = new MessageParser<MessageOptions>(() => new MessageOptions());

		// Token: 0x04000248 RID: 584
		private UnknownFieldSet _unknownFields;

		// Token: 0x04000249 RID: 585
		internal ExtensionSet<MessageOptions> _extensions;

		// Token: 0x0400024A RID: 586
		private int _hasBits0;

		// Token: 0x0400024B RID: 587
		public const int MessageSetWireFormatFieldNumber = 1;

		// Token: 0x0400024C RID: 588
		private static readonly bool MessageSetWireFormatDefaultValue = false;

		// Token: 0x0400024D RID: 589
		private bool messageSetWireFormat_;

		// Token: 0x0400024E RID: 590
		public const int NoStandardDescriptorAccessorFieldNumber = 2;

		// Token: 0x0400024F RID: 591
		private static readonly bool NoStandardDescriptorAccessorDefaultValue = false;

		// Token: 0x04000250 RID: 592
		private bool noStandardDescriptorAccessor_;

		// Token: 0x04000251 RID: 593
		public const int DeprecatedFieldNumber = 3;

		// Token: 0x04000252 RID: 594
		private static readonly bool DeprecatedDefaultValue = false;

		// Token: 0x04000253 RID: 595
		private bool deprecated_;

		// Token: 0x04000254 RID: 596
		public const int MapEntryFieldNumber = 7;

		// Token: 0x04000255 RID: 597
		private static readonly bool MapEntryDefaultValue = false;

		// Token: 0x04000256 RID: 598
		private bool mapEntry_;

		// Token: 0x04000257 RID: 599
		public const int UninterpretedOptionFieldNumber = 999;

		// Token: 0x04000258 RID: 600
		private static readonly FieldCodec<UninterpretedOption> _repeated_uninterpretedOption_codec = FieldCodec.ForMessage<UninterpretedOption>(7994U, Google.Protobuf.Reflection.UninterpretedOption.Parser);

		// Token: 0x04000259 RID: 601
		private readonly RepeatedField<UninterpretedOption> uninterpretedOption_ = new RepeatedField<UninterpretedOption>();
	}
}
