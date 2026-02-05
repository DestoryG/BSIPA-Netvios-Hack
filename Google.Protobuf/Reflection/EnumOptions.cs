using System;
using System.Diagnostics;
using Google.Protobuf.Collections;

namespace Google.Protobuf.Reflection
{
	// Token: 0x0200005E RID: 94
	public sealed class EnumOptions : IExtendableMessage<EnumOptions>, IMessage<EnumOptions>, IMessage, IEquatable<EnumOptions>, IDeepCloneable<EnumOptions>
	{
		// Token: 0x17000196 RID: 406
		// (get) Token: 0x0600066E RID: 1646 RVA: 0x000184F3 File Offset: 0x000166F3
		private ExtensionSet<EnumOptions> _Extensions
		{
			get
			{
				return this._extensions;
			}
		}

		// Token: 0x17000197 RID: 407
		// (get) Token: 0x0600066F RID: 1647 RVA: 0x000184FB File Offset: 0x000166FB
		[DebuggerNonUserCode]
		public static MessageParser<EnumOptions> Parser
		{
			get
			{
				return EnumOptions._parser;
			}
		}

		// Token: 0x17000198 RID: 408
		// (get) Token: 0x06000670 RID: 1648 RVA: 0x00018502 File Offset: 0x00016702
		[DebuggerNonUserCode]
		public static MessageDescriptor Descriptor
		{
			get
			{
				return DescriptorReflection.Descriptor.MessageTypes[14];
			}
		}

		// Token: 0x17000199 RID: 409
		// (get) Token: 0x06000671 RID: 1649 RVA: 0x00018515 File Offset: 0x00016715
		[DebuggerNonUserCode]
		MessageDescriptor IMessage.Descriptor
		{
			get
			{
				return EnumOptions.Descriptor;
			}
		}

		// Token: 0x06000672 RID: 1650 RVA: 0x0001851C File Offset: 0x0001671C
		[DebuggerNonUserCode]
		public EnumOptions()
		{
		}

		// Token: 0x06000673 RID: 1651 RVA: 0x00018530 File Offset: 0x00016730
		[DebuggerNonUserCode]
		public EnumOptions(EnumOptions other)
			: this()
		{
			this._hasBits0 = other._hasBits0;
			this.allowAlias_ = other.allowAlias_;
			this.deprecated_ = other.deprecated_;
			this.uninterpretedOption_ = other.uninterpretedOption_.Clone();
			this._unknownFields = UnknownFieldSet.Clone(other._unknownFields);
			this._extensions = ExtensionSet.Clone<EnumOptions>(other._extensions);
		}

		// Token: 0x06000674 RID: 1652 RVA: 0x0001859A File Offset: 0x0001679A
		[DebuggerNonUserCode]
		public EnumOptions Clone()
		{
			return new EnumOptions(this);
		}

		// Token: 0x1700019A RID: 410
		// (get) Token: 0x06000675 RID: 1653 RVA: 0x000185A2 File Offset: 0x000167A2
		// (set) Token: 0x06000676 RID: 1654 RVA: 0x000185BA File Offset: 0x000167BA
		[DebuggerNonUserCode]
		public bool AllowAlias
		{
			get
			{
				if ((this._hasBits0 & 1) != 0)
				{
					return this.allowAlias_;
				}
				return EnumOptions.AllowAliasDefaultValue;
			}
			set
			{
				this._hasBits0 |= 1;
				this.allowAlias_ = value;
			}
		}

		// Token: 0x1700019B RID: 411
		// (get) Token: 0x06000677 RID: 1655 RVA: 0x000185D1 File Offset: 0x000167D1
		[DebuggerNonUserCode]
		public bool HasAllowAlias
		{
			get
			{
				return (this._hasBits0 & 1) != 0;
			}
		}

		// Token: 0x06000678 RID: 1656 RVA: 0x000185DE File Offset: 0x000167DE
		[DebuggerNonUserCode]
		public void ClearAllowAlias()
		{
			this._hasBits0 &= -2;
		}

		// Token: 0x1700019C RID: 412
		// (get) Token: 0x06000679 RID: 1657 RVA: 0x000185EF File Offset: 0x000167EF
		// (set) Token: 0x0600067A RID: 1658 RVA: 0x00018607 File Offset: 0x00016807
		[DebuggerNonUserCode]
		public bool Deprecated
		{
			get
			{
				if ((this._hasBits0 & 2) != 0)
				{
					return this.deprecated_;
				}
				return EnumOptions.DeprecatedDefaultValue;
			}
			set
			{
				this._hasBits0 |= 2;
				this.deprecated_ = value;
			}
		}

		// Token: 0x1700019D RID: 413
		// (get) Token: 0x0600067B RID: 1659 RVA: 0x0001861E File Offset: 0x0001681E
		[DebuggerNonUserCode]
		public bool HasDeprecated
		{
			get
			{
				return (this._hasBits0 & 2) != 0;
			}
		}

		// Token: 0x0600067C RID: 1660 RVA: 0x0001862B File Offset: 0x0001682B
		[DebuggerNonUserCode]
		public void ClearDeprecated()
		{
			this._hasBits0 &= -3;
		}

		// Token: 0x1700019E RID: 414
		// (get) Token: 0x0600067D RID: 1661 RVA: 0x0001863C File Offset: 0x0001683C
		[DebuggerNonUserCode]
		public RepeatedField<UninterpretedOption> UninterpretedOption
		{
			get
			{
				return this.uninterpretedOption_;
			}
		}

		// Token: 0x0600067E RID: 1662 RVA: 0x00018644 File Offset: 0x00016844
		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return this.Equals(other as EnumOptions);
		}

		// Token: 0x0600067F RID: 1663 RVA: 0x00018654 File Offset: 0x00016854
		[DebuggerNonUserCode]
		public bool Equals(EnumOptions other)
		{
			return other != null && (other == this || (this.AllowAlias == other.AllowAlias && this.Deprecated == other.Deprecated && this.uninterpretedOption_.Equals(other.uninterpretedOption_) && object.Equals(this._extensions, other._extensions) && object.Equals(this._unknownFields, other._unknownFields)));
		}

		// Token: 0x06000680 RID: 1664 RVA: 0x000186C8 File Offset: 0x000168C8
		[DebuggerNonUserCode]
		public override int GetHashCode()
		{
			int num = 1;
			if (this.HasAllowAlias)
			{
				num ^= this.AllowAlias.GetHashCode();
			}
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

		// Token: 0x06000681 RID: 1665 RVA: 0x00018744 File Offset: 0x00016944
		[DebuggerNonUserCode]
		public override string ToString()
		{
			return JsonFormatter.ToDiagnosticString(this);
		}

		// Token: 0x06000682 RID: 1666 RVA: 0x0001874C File Offset: 0x0001694C
		[DebuggerNonUserCode]
		public void WriteTo(CodedOutputStream output)
		{
			if (this.HasAllowAlias)
			{
				output.WriteRawTag(16);
				output.WriteBool(this.AllowAlias);
			}
			if (this.HasDeprecated)
			{
				output.WriteRawTag(24);
				output.WriteBool(this.Deprecated);
			}
			this.uninterpretedOption_.WriteTo(output, EnumOptions._repeated_uninterpretedOption_codec);
			if (this._extensions != null)
			{
				this._extensions.WriteTo(output);
			}
			if (this._unknownFields != null)
			{
				this._unknownFields.WriteTo(output);
			}
		}

		// Token: 0x06000683 RID: 1667 RVA: 0x000187CC File Offset: 0x000169CC
		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (this.HasAllowAlias)
			{
				num += 2;
			}
			if (this.HasDeprecated)
			{
				num += 2;
			}
			num += this.uninterpretedOption_.CalculateSize(EnumOptions._repeated_uninterpretedOption_codec);
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

		// Token: 0x06000684 RID: 1668 RVA: 0x00018834 File Offset: 0x00016A34
		[DebuggerNonUserCode]
		public void MergeFrom(EnumOptions other)
		{
			if (other == null)
			{
				return;
			}
			if (other.HasAllowAlias)
			{
				this.AllowAlias = other.AllowAlias;
			}
			if (other.HasDeprecated)
			{
				this.Deprecated = other.Deprecated;
			}
			this.uninterpretedOption_.Add(other.uninterpretedOption_);
			ExtensionSet.MergeFrom<EnumOptions>(ref this._extensions, other._extensions);
			this._unknownFields = UnknownFieldSet.MergeFrom(this._unknownFields, other._unknownFields);
		}

		// Token: 0x06000685 RID: 1669 RVA: 0x000188A8 File Offset: 0x00016AA8
		[DebuggerNonUserCode]
		public void MergeFrom(CodedInputStream input)
		{
			uint num;
			while ((num = input.ReadTag()) != 0U)
			{
				if (num != 16U)
				{
					if (num != 24U)
					{
						if (num != 7994U)
						{
							if (!ExtensionSet.TryMergeFieldFrom<EnumOptions>(ref this._extensions, input))
							{
								this._unknownFields = UnknownFieldSet.MergeFieldFrom(this._unknownFields, input);
							}
						}
						else
						{
							this.uninterpretedOption_.AddEntriesFrom(input, EnumOptions._repeated_uninterpretedOption_codec);
						}
					}
					else
					{
						this.Deprecated = input.ReadBool();
					}
				}
				else
				{
					this.AllowAlias = input.ReadBool();
				}
			}
		}

		// Token: 0x06000686 RID: 1670 RVA: 0x00018922 File Offset: 0x00016B22
		public TValue GetExtension<TValue>(Extension<EnumOptions, TValue> extension)
		{
			return ExtensionSet.Get<EnumOptions, TValue>(ref this._extensions, extension);
		}

		// Token: 0x06000687 RID: 1671 RVA: 0x00018930 File Offset: 0x00016B30
		public RepeatedField<TValue> GetExtension<TValue>(RepeatedExtension<EnumOptions, TValue> extension)
		{
			return ExtensionSet.Get<EnumOptions, TValue>(ref this._extensions, extension);
		}

		// Token: 0x06000688 RID: 1672 RVA: 0x0001893E File Offset: 0x00016B3E
		public RepeatedField<TValue> GetOrInitializeExtension<TValue>(RepeatedExtension<EnumOptions, TValue> extension)
		{
			return ExtensionSet.GetOrInitialize<EnumOptions, TValue>(ref this._extensions, extension);
		}

		// Token: 0x06000689 RID: 1673 RVA: 0x0001894C File Offset: 0x00016B4C
		public void SetExtension<TValue>(Extension<EnumOptions, TValue> extension, TValue value)
		{
			ExtensionSet.Set<EnumOptions, TValue>(ref this._extensions, extension, value);
		}

		// Token: 0x0600068A RID: 1674 RVA: 0x0001895B File Offset: 0x00016B5B
		public bool HasExtension<TValue>(Extension<EnumOptions, TValue> extension)
		{
			return ExtensionSet.Has<EnumOptions, TValue>(ref this._extensions, extension);
		}

		// Token: 0x0600068B RID: 1675 RVA: 0x00018969 File Offset: 0x00016B69
		public void ClearExtension<TValue>(Extension<EnumOptions, TValue> extension)
		{
			ExtensionSet.Clear<EnumOptions, TValue>(ref this._extensions, extension);
		}

		// Token: 0x0600068C RID: 1676 RVA: 0x00018977 File Offset: 0x00016B77
		public void ClearExtension<TValue>(RepeatedExtension<EnumOptions, TValue> extension)
		{
			ExtensionSet.Clear<EnumOptions, TValue>(ref this._extensions, extension);
		}

		// Token: 0x04000279 RID: 633
		private static readonly MessageParser<EnumOptions> _parser = new MessageParser<EnumOptions>(() => new EnumOptions());

		// Token: 0x0400027A RID: 634
		private UnknownFieldSet _unknownFields;

		// Token: 0x0400027B RID: 635
		internal ExtensionSet<EnumOptions> _extensions;

		// Token: 0x0400027C RID: 636
		private int _hasBits0;

		// Token: 0x0400027D RID: 637
		public const int AllowAliasFieldNumber = 2;

		// Token: 0x0400027E RID: 638
		private static readonly bool AllowAliasDefaultValue = false;

		// Token: 0x0400027F RID: 639
		private bool allowAlias_;

		// Token: 0x04000280 RID: 640
		public const int DeprecatedFieldNumber = 3;

		// Token: 0x04000281 RID: 641
		private static readonly bool DeprecatedDefaultValue = false;

		// Token: 0x04000282 RID: 642
		private bool deprecated_;

		// Token: 0x04000283 RID: 643
		public const int UninterpretedOptionFieldNumber = 999;

		// Token: 0x04000284 RID: 644
		private static readonly FieldCodec<UninterpretedOption> _repeated_uninterpretedOption_codec = FieldCodec.ForMessage<UninterpretedOption>(7994U, Google.Protobuf.Reflection.UninterpretedOption.Parser);

		// Token: 0x04000285 RID: 645
		private readonly RepeatedField<UninterpretedOption> uninterpretedOption_ = new RepeatedField<UninterpretedOption>();
	}
}
