using System;
using System.Diagnostics;
using Google.Protobuf.Collections;

namespace Google.Protobuf.Reflection
{
	// Token: 0x0200005F RID: 95
	public sealed class EnumValueOptions : IExtendableMessage<EnumValueOptions>, IMessage<EnumValueOptions>, IMessage, IEquatable<EnumValueOptions>, IDeepCloneable<EnumValueOptions>
	{
		// Token: 0x1700019F RID: 415
		// (get) Token: 0x0600068E RID: 1678 RVA: 0x000189C1 File Offset: 0x00016BC1
		private ExtensionSet<EnumValueOptions> _Extensions
		{
			get
			{
				return this._extensions;
			}
		}

		// Token: 0x170001A0 RID: 416
		// (get) Token: 0x0600068F RID: 1679 RVA: 0x000189C9 File Offset: 0x00016BC9
		[DebuggerNonUserCode]
		public static MessageParser<EnumValueOptions> Parser
		{
			get
			{
				return EnumValueOptions._parser;
			}
		}

		// Token: 0x170001A1 RID: 417
		// (get) Token: 0x06000690 RID: 1680 RVA: 0x000189D0 File Offset: 0x00016BD0
		[DebuggerNonUserCode]
		public static MessageDescriptor Descriptor
		{
			get
			{
				return DescriptorReflection.Descriptor.MessageTypes[15];
			}
		}

		// Token: 0x170001A2 RID: 418
		// (get) Token: 0x06000691 RID: 1681 RVA: 0x000189E3 File Offset: 0x00016BE3
		[DebuggerNonUserCode]
		MessageDescriptor IMessage.Descriptor
		{
			get
			{
				return EnumValueOptions.Descriptor;
			}
		}

		// Token: 0x06000692 RID: 1682 RVA: 0x000189EA File Offset: 0x00016BEA
		[DebuggerNonUserCode]
		public EnumValueOptions()
		{
		}

		// Token: 0x06000693 RID: 1683 RVA: 0x00018A00 File Offset: 0x00016C00
		[DebuggerNonUserCode]
		public EnumValueOptions(EnumValueOptions other)
			: this()
		{
			this._hasBits0 = other._hasBits0;
			this.deprecated_ = other.deprecated_;
			this.uninterpretedOption_ = other.uninterpretedOption_.Clone();
			this._unknownFields = UnknownFieldSet.Clone(other._unknownFields);
			this._extensions = ExtensionSet.Clone<EnumValueOptions>(other._extensions);
		}

		// Token: 0x06000694 RID: 1684 RVA: 0x00018A5E File Offset: 0x00016C5E
		[DebuggerNonUserCode]
		public EnumValueOptions Clone()
		{
			return new EnumValueOptions(this);
		}

		// Token: 0x170001A3 RID: 419
		// (get) Token: 0x06000695 RID: 1685 RVA: 0x00018A66 File Offset: 0x00016C66
		// (set) Token: 0x06000696 RID: 1686 RVA: 0x00018A7E File Offset: 0x00016C7E
		[DebuggerNonUserCode]
		public bool Deprecated
		{
			get
			{
				if ((this._hasBits0 & 1) != 0)
				{
					return this.deprecated_;
				}
				return EnumValueOptions.DeprecatedDefaultValue;
			}
			set
			{
				this._hasBits0 |= 1;
				this.deprecated_ = value;
			}
		}

		// Token: 0x170001A4 RID: 420
		// (get) Token: 0x06000697 RID: 1687 RVA: 0x00018A95 File Offset: 0x00016C95
		[DebuggerNonUserCode]
		public bool HasDeprecated
		{
			get
			{
				return (this._hasBits0 & 1) != 0;
			}
		}

		// Token: 0x06000698 RID: 1688 RVA: 0x00018AA2 File Offset: 0x00016CA2
		[DebuggerNonUserCode]
		public void ClearDeprecated()
		{
			this._hasBits0 &= -2;
		}

		// Token: 0x170001A5 RID: 421
		// (get) Token: 0x06000699 RID: 1689 RVA: 0x00018AB3 File Offset: 0x00016CB3
		[DebuggerNonUserCode]
		public RepeatedField<UninterpretedOption> UninterpretedOption
		{
			get
			{
				return this.uninterpretedOption_;
			}
		}

		// Token: 0x0600069A RID: 1690 RVA: 0x00018ABB File Offset: 0x00016CBB
		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return this.Equals(other as EnumValueOptions);
		}

		// Token: 0x0600069B RID: 1691 RVA: 0x00018ACC File Offset: 0x00016CCC
		[DebuggerNonUserCode]
		public bool Equals(EnumValueOptions other)
		{
			return other != null && (other == this || (this.Deprecated == other.Deprecated && this.uninterpretedOption_.Equals(other.uninterpretedOption_) && object.Equals(this._extensions, other._extensions) && object.Equals(this._unknownFields, other._unknownFields)));
		}

		// Token: 0x0600069C RID: 1692 RVA: 0x00018B30 File Offset: 0x00016D30
		[DebuggerNonUserCode]
		public override int GetHashCode()
		{
			int num = 1;
			if (this.HasDeprecated)
			{
				num ^= this.Deprecated.GetHashCode();
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

		// Token: 0x0600069D RID: 1693 RVA: 0x00018B93 File Offset: 0x00016D93
		[DebuggerNonUserCode]
		public override string ToString()
		{
			return JsonFormatter.ToDiagnosticString(this);
		}

		// Token: 0x0600069E RID: 1694 RVA: 0x00018B9C File Offset: 0x00016D9C
		[DebuggerNonUserCode]
		public void WriteTo(CodedOutputStream output)
		{
			if (this.HasDeprecated)
			{
				output.WriteRawTag(8);
				output.WriteBool(this.Deprecated);
			}
			this.uninterpretedOption_.WriteTo(output, EnumValueOptions._repeated_uninterpretedOption_codec);
			if (this._extensions != null)
			{
				this._extensions.WriteTo(output);
			}
			if (this._unknownFields != null)
			{
				this._unknownFields.WriteTo(output);
			}
		}

		// Token: 0x0600069F RID: 1695 RVA: 0x00018C00 File Offset: 0x00016E00
		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (this.HasDeprecated)
			{
				num += 2;
			}
			num += this.uninterpretedOption_.CalculateSize(EnumValueOptions._repeated_uninterpretedOption_codec);
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

		// Token: 0x060006A0 RID: 1696 RVA: 0x00018C5C File Offset: 0x00016E5C
		[DebuggerNonUserCode]
		public void MergeFrom(EnumValueOptions other)
		{
			if (other == null)
			{
				return;
			}
			if (other.HasDeprecated)
			{
				this.Deprecated = other.Deprecated;
			}
			this.uninterpretedOption_.Add(other.uninterpretedOption_);
			ExtensionSet.MergeFrom<EnumValueOptions>(ref this._extensions, other._extensions);
			this._unknownFields = UnknownFieldSet.MergeFrom(this._unknownFields, other._unknownFields);
		}

		// Token: 0x060006A1 RID: 1697 RVA: 0x00018CBC File Offset: 0x00016EBC
		[DebuggerNonUserCode]
		public void MergeFrom(CodedInputStream input)
		{
			uint num;
			while ((num = input.ReadTag()) != 0U)
			{
				if (num != 8U)
				{
					if (num != 7994U)
					{
						if (!ExtensionSet.TryMergeFieldFrom<EnumValueOptions>(ref this._extensions, input))
						{
							this._unknownFields = UnknownFieldSet.MergeFieldFrom(this._unknownFields, input);
						}
					}
					else
					{
						this.uninterpretedOption_.AddEntriesFrom(input, EnumValueOptions._repeated_uninterpretedOption_codec);
					}
				}
				else
				{
					this.Deprecated = input.ReadBool();
				}
			}
		}

		// Token: 0x060006A2 RID: 1698 RVA: 0x00018D22 File Offset: 0x00016F22
		public TValue GetExtension<TValue>(Extension<EnumValueOptions, TValue> extension)
		{
			return ExtensionSet.Get<EnumValueOptions, TValue>(ref this._extensions, extension);
		}

		// Token: 0x060006A3 RID: 1699 RVA: 0x00018D30 File Offset: 0x00016F30
		public RepeatedField<TValue> GetExtension<TValue>(RepeatedExtension<EnumValueOptions, TValue> extension)
		{
			return ExtensionSet.Get<EnumValueOptions, TValue>(ref this._extensions, extension);
		}

		// Token: 0x060006A4 RID: 1700 RVA: 0x00018D3E File Offset: 0x00016F3E
		public RepeatedField<TValue> GetOrInitializeExtension<TValue>(RepeatedExtension<EnumValueOptions, TValue> extension)
		{
			return ExtensionSet.GetOrInitialize<EnumValueOptions, TValue>(ref this._extensions, extension);
		}

		// Token: 0x060006A5 RID: 1701 RVA: 0x00018D4C File Offset: 0x00016F4C
		public void SetExtension<TValue>(Extension<EnumValueOptions, TValue> extension, TValue value)
		{
			ExtensionSet.Set<EnumValueOptions, TValue>(ref this._extensions, extension, value);
		}

		// Token: 0x060006A6 RID: 1702 RVA: 0x00018D5B File Offset: 0x00016F5B
		public bool HasExtension<TValue>(Extension<EnumValueOptions, TValue> extension)
		{
			return ExtensionSet.Has<EnumValueOptions, TValue>(ref this._extensions, extension);
		}

		// Token: 0x060006A7 RID: 1703 RVA: 0x00018D69 File Offset: 0x00016F69
		public void ClearExtension<TValue>(Extension<EnumValueOptions, TValue> extension)
		{
			ExtensionSet.Clear<EnumValueOptions, TValue>(ref this._extensions, extension);
		}

		// Token: 0x060006A8 RID: 1704 RVA: 0x00018D77 File Offset: 0x00016F77
		public void ClearExtension<TValue>(RepeatedExtension<EnumValueOptions, TValue> extension)
		{
			ExtensionSet.Clear<EnumValueOptions, TValue>(ref this._extensions, extension);
		}

		// Token: 0x04000286 RID: 646
		private static readonly MessageParser<EnumValueOptions> _parser = new MessageParser<EnumValueOptions>(() => new EnumValueOptions());

		// Token: 0x04000287 RID: 647
		private UnknownFieldSet _unknownFields;

		// Token: 0x04000288 RID: 648
		internal ExtensionSet<EnumValueOptions> _extensions;

		// Token: 0x04000289 RID: 649
		private int _hasBits0;

		// Token: 0x0400028A RID: 650
		public const int DeprecatedFieldNumber = 1;

		// Token: 0x0400028B RID: 651
		private static readonly bool DeprecatedDefaultValue = false;

		// Token: 0x0400028C RID: 652
		private bool deprecated_;

		// Token: 0x0400028D RID: 653
		public const int UninterpretedOptionFieldNumber = 999;

		// Token: 0x0400028E RID: 654
		private static readonly FieldCodec<UninterpretedOption> _repeated_uninterpretedOption_codec = FieldCodec.ForMessage<UninterpretedOption>(7994U, Google.Protobuf.Reflection.UninterpretedOption.Parser);

		// Token: 0x0400028F RID: 655
		private readonly RepeatedField<UninterpretedOption> uninterpretedOption_ = new RepeatedField<UninterpretedOption>();
	}
}
