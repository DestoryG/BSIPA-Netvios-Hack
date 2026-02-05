using System;
using System.Diagnostics;
using Google.Protobuf.Collections;

namespace Google.Protobuf.Reflection
{
	// Token: 0x02000061 RID: 97
	public sealed class MethodOptions : IExtendableMessage<MethodOptions>, IMessage<MethodOptions>, IMessage, IEquatable<MethodOptions>, IDeepCloneable<MethodOptions>
	{
		// Token: 0x170001AD RID: 429
		// (get) Token: 0x060006C6 RID: 1734 RVA: 0x000191BB File Offset: 0x000173BB
		private ExtensionSet<MethodOptions> _Extensions
		{
			get
			{
				return this._extensions;
			}
		}

		// Token: 0x170001AE RID: 430
		// (get) Token: 0x060006C7 RID: 1735 RVA: 0x000191C3 File Offset: 0x000173C3
		[DebuggerNonUserCode]
		public static MessageParser<MethodOptions> Parser
		{
			get
			{
				return MethodOptions._parser;
			}
		}

		// Token: 0x170001AF RID: 431
		// (get) Token: 0x060006C8 RID: 1736 RVA: 0x000191CA File Offset: 0x000173CA
		[DebuggerNonUserCode]
		public static MessageDescriptor Descriptor
		{
			get
			{
				return DescriptorReflection.Descriptor.MessageTypes[17];
			}
		}

		// Token: 0x170001B0 RID: 432
		// (get) Token: 0x060006C9 RID: 1737 RVA: 0x000191DD File Offset: 0x000173DD
		[DebuggerNonUserCode]
		MessageDescriptor IMessage.Descriptor
		{
			get
			{
				return MethodOptions.Descriptor;
			}
		}

		// Token: 0x060006CA RID: 1738 RVA: 0x000191E4 File Offset: 0x000173E4
		[DebuggerNonUserCode]
		public MethodOptions()
		{
		}

		// Token: 0x060006CB RID: 1739 RVA: 0x000191F8 File Offset: 0x000173F8
		[DebuggerNonUserCode]
		public MethodOptions(MethodOptions other)
			: this()
		{
			this._hasBits0 = other._hasBits0;
			this.deprecated_ = other.deprecated_;
			this.idempotencyLevel_ = other.idempotencyLevel_;
			this.uninterpretedOption_ = other.uninterpretedOption_.Clone();
			this._unknownFields = UnknownFieldSet.Clone(other._unknownFields);
			this._extensions = ExtensionSet.Clone<MethodOptions>(other._extensions);
		}

		// Token: 0x060006CC RID: 1740 RVA: 0x00019262 File Offset: 0x00017462
		[DebuggerNonUserCode]
		public MethodOptions Clone()
		{
			return new MethodOptions(this);
		}

		// Token: 0x170001B1 RID: 433
		// (get) Token: 0x060006CD RID: 1741 RVA: 0x0001926A File Offset: 0x0001746A
		// (set) Token: 0x060006CE RID: 1742 RVA: 0x00019282 File Offset: 0x00017482
		[DebuggerNonUserCode]
		public bool Deprecated
		{
			get
			{
				if ((this._hasBits0 & 1) != 0)
				{
					return this.deprecated_;
				}
				return MethodOptions.DeprecatedDefaultValue;
			}
			set
			{
				this._hasBits0 |= 1;
				this.deprecated_ = value;
			}
		}

		// Token: 0x170001B2 RID: 434
		// (get) Token: 0x060006CF RID: 1743 RVA: 0x00019299 File Offset: 0x00017499
		[DebuggerNonUserCode]
		public bool HasDeprecated
		{
			get
			{
				return (this._hasBits0 & 1) != 0;
			}
		}

		// Token: 0x060006D0 RID: 1744 RVA: 0x000192A6 File Offset: 0x000174A6
		[DebuggerNonUserCode]
		public void ClearDeprecated()
		{
			this._hasBits0 &= -2;
		}

		// Token: 0x170001B3 RID: 435
		// (get) Token: 0x060006D1 RID: 1745 RVA: 0x000192B7 File Offset: 0x000174B7
		// (set) Token: 0x060006D2 RID: 1746 RVA: 0x000192CF File Offset: 0x000174CF
		[DebuggerNonUserCode]
		public MethodOptions.Types.IdempotencyLevel IdempotencyLevel
		{
			get
			{
				if ((this._hasBits0 & 2) != 0)
				{
					return this.idempotencyLevel_;
				}
				return MethodOptions.IdempotencyLevelDefaultValue;
			}
			set
			{
				this._hasBits0 |= 2;
				this.idempotencyLevel_ = value;
			}
		}

		// Token: 0x170001B4 RID: 436
		// (get) Token: 0x060006D3 RID: 1747 RVA: 0x000192E6 File Offset: 0x000174E6
		[DebuggerNonUserCode]
		public bool HasIdempotencyLevel
		{
			get
			{
				return (this._hasBits0 & 2) != 0;
			}
		}

		// Token: 0x060006D4 RID: 1748 RVA: 0x000192F3 File Offset: 0x000174F3
		[DebuggerNonUserCode]
		public void ClearIdempotencyLevel()
		{
			this._hasBits0 &= -3;
		}

		// Token: 0x170001B5 RID: 437
		// (get) Token: 0x060006D5 RID: 1749 RVA: 0x00019304 File Offset: 0x00017504
		[DebuggerNonUserCode]
		public RepeatedField<UninterpretedOption> UninterpretedOption
		{
			get
			{
				return this.uninterpretedOption_;
			}
		}

		// Token: 0x060006D6 RID: 1750 RVA: 0x0001930C File Offset: 0x0001750C
		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return this.Equals(other as MethodOptions);
		}

		// Token: 0x060006D7 RID: 1751 RVA: 0x0001931C File Offset: 0x0001751C
		[DebuggerNonUserCode]
		public bool Equals(MethodOptions other)
		{
			return other != null && (other == this || (this.Deprecated == other.Deprecated && this.IdempotencyLevel == other.IdempotencyLevel && this.uninterpretedOption_.Equals(other.uninterpretedOption_) && object.Equals(this._extensions, other._extensions) && object.Equals(this._unknownFields, other._unknownFields)));
		}

		// Token: 0x060006D8 RID: 1752 RVA: 0x00019390 File Offset: 0x00017590
		[DebuggerNonUserCode]
		public override int GetHashCode()
		{
			int num = 1;
			if (this.HasDeprecated)
			{
				num ^= this.Deprecated.GetHashCode();
			}
			if (this.HasIdempotencyLevel)
			{
				num ^= this.IdempotencyLevel.GetHashCode();
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

		// Token: 0x060006D9 RID: 1753 RVA: 0x00019412 File Offset: 0x00017612
		[DebuggerNonUserCode]
		public override string ToString()
		{
			return JsonFormatter.ToDiagnosticString(this);
		}

		// Token: 0x060006DA RID: 1754 RVA: 0x0001941C File Offset: 0x0001761C
		[DebuggerNonUserCode]
		public void WriteTo(CodedOutputStream output)
		{
			if (this.HasDeprecated)
			{
				output.WriteRawTag(136, 2);
				output.WriteBool(this.Deprecated);
			}
			if (this.HasIdempotencyLevel)
			{
				output.WriteRawTag(144, 2);
				output.WriteEnum((int)this.IdempotencyLevel);
			}
			this.uninterpretedOption_.WriteTo(output, MethodOptions._repeated_uninterpretedOption_codec);
			if (this._extensions != null)
			{
				this._extensions.WriteTo(output);
			}
			if (this._unknownFields != null)
			{
				this._unknownFields.WriteTo(output);
			}
		}

		// Token: 0x060006DB RID: 1755 RVA: 0x000194A4 File Offset: 0x000176A4
		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (this.HasDeprecated)
			{
				num += 3;
			}
			if (this.HasIdempotencyLevel)
			{
				num += 2 + CodedOutputStream.ComputeEnumSize((int)this.IdempotencyLevel);
			}
			num += this.uninterpretedOption_.CalculateSize(MethodOptions._repeated_uninterpretedOption_codec);
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

		// Token: 0x060006DC RID: 1756 RVA: 0x00019518 File Offset: 0x00017718
		[DebuggerNonUserCode]
		public void MergeFrom(MethodOptions other)
		{
			if (other == null)
			{
				return;
			}
			if (other.HasDeprecated)
			{
				this.Deprecated = other.Deprecated;
			}
			if (other.HasIdempotencyLevel)
			{
				this.IdempotencyLevel = other.IdempotencyLevel;
			}
			this.uninterpretedOption_.Add(other.uninterpretedOption_);
			ExtensionSet.MergeFrom<MethodOptions>(ref this._extensions, other._extensions);
			this._unknownFields = UnknownFieldSet.MergeFrom(this._unknownFields, other._unknownFields);
		}

		// Token: 0x060006DD RID: 1757 RVA: 0x0001958C File Offset: 0x0001778C
		[DebuggerNonUserCode]
		public void MergeFrom(CodedInputStream input)
		{
			uint num;
			while ((num = input.ReadTag()) != 0U)
			{
				if (num != 264U)
				{
					if (num != 272U)
					{
						if (num != 7994U)
						{
							if (!ExtensionSet.TryMergeFieldFrom<MethodOptions>(ref this._extensions, input))
							{
								this._unknownFields = UnknownFieldSet.MergeFieldFrom(this._unknownFields, input);
							}
						}
						else
						{
							this.uninterpretedOption_.AddEntriesFrom(input, MethodOptions._repeated_uninterpretedOption_codec);
						}
					}
					else
					{
						this.IdempotencyLevel = (MethodOptions.Types.IdempotencyLevel)input.ReadEnum();
					}
				}
				else
				{
					this.Deprecated = input.ReadBool();
				}
			}
		}

		// Token: 0x060006DE RID: 1758 RVA: 0x0001960C File Offset: 0x0001780C
		public TValue GetExtension<TValue>(Extension<MethodOptions, TValue> extension)
		{
			return ExtensionSet.Get<MethodOptions, TValue>(ref this._extensions, extension);
		}

		// Token: 0x060006DF RID: 1759 RVA: 0x0001961A File Offset: 0x0001781A
		public RepeatedField<TValue> GetExtension<TValue>(RepeatedExtension<MethodOptions, TValue> extension)
		{
			return ExtensionSet.Get<MethodOptions, TValue>(ref this._extensions, extension);
		}

		// Token: 0x060006E0 RID: 1760 RVA: 0x00019628 File Offset: 0x00017828
		public RepeatedField<TValue> GetOrInitializeExtension<TValue>(RepeatedExtension<MethodOptions, TValue> extension)
		{
			return ExtensionSet.GetOrInitialize<MethodOptions, TValue>(ref this._extensions, extension);
		}

		// Token: 0x060006E1 RID: 1761 RVA: 0x00019636 File Offset: 0x00017836
		public void SetExtension<TValue>(Extension<MethodOptions, TValue> extension, TValue value)
		{
			ExtensionSet.Set<MethodOptions, TValue>(ref this._extensions, extension, value);
		}

		// Token: 0x060006E2 RID: 1762 RVA: 0x00019645 File Offset: 0x00017845
		public bool HasExtension<TValue>(Extension<MethodOptions, TValue> extension)
		{
			return ExtensionSet.Has<MethodOptions, TValue>(ref this._extensions, extension);
		}

		// Token: 0x060006E3 RID: 1763 RVA: 0x00019653 File Offset: 0x00017853
		public void ClearExtension<TValue>(Extension<MethodOptions, TValue> extension)
		{
			ExtensionSet.Clear<MethodOptions, TValue>(ref this._extensions, extension);
		}

		// Token: 0x060006E4 RID: 1764 RVA: 0x00019661 File Offset: 0x00017861
		public void ClearExtension<TValue>(RepeatedExtension<MethodOptions, TValue> extension)
		{
			ExtensionSet.Clear<MethodOptions, TValue>(ref this._extensions, extension);
		}

		// Token: 0x0400029A RID: 666
		private static readonly MessageParser<MethodOptions> _parser = new MessageParser<MethodOptions>(() => new MethodOptions());

		// Token: 0x0400029B RID: 667
		private UnknownFieldSet _unknownFields;

		// Token: 0x0400029C RID: 668
		internal ExtensionSet<MethodOptions> _extensions;

		// Token: 0x0400029D RID: 669
		private int _hasBits0;

		// Token: 0x0400029E RID: 670
		public const int DeprecatedFieldNumber = 33;

		// Token: 0x0400029F RID: 671
		private static readonly bool DeprecatedDefaultValue = false;

		// Token: 0x040002A0 RID: 672
		private bool deprecated_;

		// Token: 0x040002A1 RID: 673
		public const int IdempotencyLevelFieldNumber = 34;

		// Token: 0x040002A2 RID: 674
		private static readonly MethodOptions.Types.IdempotencyLevel IdempotencyLevelDefaultValue = MethodOptions.Types.IdempotencyLevel.IdempotencyUnknown;

		// Token: 0x040002A3 RID: 675
		private MethodOptions.Types.IdempotencyLevel idempotencyLevel_;

		// Token: 0x040002A4 RID: 676
		public const int UninterpretedOptionFieldNumber = 999;

		// Token: 0x040002A5 RID: 677
		private static readonly FieldCodec<UninterpretedOption> _repeated_uninterpretedOption_codec = FieldCodec.ForMessage<UninterpretedOption>(7994U, Google.Protobuf.Reflection.UninterpretedOption.Parser);

		// Token: 0x040002A6 RID: 678
		private readonly RepeatedField<UninterpretedOption> uninterpretedOption_ = new RepeatedField<UninterpretedOption>();

		// Token: 0x020000DF RID: 223
		[DebuggerNonUserCode]
		public static class Types
		{
			// Token: 0x0200011B RID: 283
			public enum IdempotencyLevel
			{
				// Token: 0x040004B0 RID: 1200
				[OriginalName("IDEMPOTENCY_UNKNOWN")]
				IdempotencyUnknown,
				// Token: 0x040004B1 RID: 1201
				[OriginalName("NO_SIDE_EFFECTS")]
				NoSideEffects,
				// Token: 0x040004B2 RID: 1202
				[OriginalName("IDEMPOTENT")]
				Idempotent
			}
		}
	}
}
