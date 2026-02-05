using System;
using System.Diagnostics;
using Google.Protobuf.Collections;

namespace Google.Protobuf.Reflection
{
	// Token: 0x0200005C RID: 92
	public sealed class FieldOptions : IExtendableMessage<FieldOptions>, IMessage<FieldOptions>, IMessage, IEquatable<FieldOptions>, IDeepCloneable<FieldOptions>
	{
		// Token: 0x17000180 RID: 384
		// (get) Token: 0x06000626 RID: 1574 RVA: 0x00017993 File Offset: 0x00015B93
		private ExtensionSet<FieldOptions> _Extensions
		{
			get
			{
				return this._extensions;
			}
		}

		// Token: 0x17000181 RID: 385
		// (get) Token: 0x06000627 RID: 1575 RVA: 0x0001799B File Offset: 0x00015B9B
		[DebuggerNonUserCode]
		public static MessageParser<FieldOptions> Parser
		{
			get
			{
				return FieldOptions._parser;
			}
		}

		// Token: 0x17000182 RID: 386
		// (get) Token: 0x06000628 RID: 1576 RVA: 0x000179A2 File Offset: 0x00015BA2
		[DebuggerNonUserCode]
		public static MessageDescriptor Descriptor
		{
			get
			{
				return DescriptorReflection.Descriptor.MessageTypes[12];
			}
		}

		// Token: 0x17000183 RID: 387
		// (get) Token: 0x06000629 RID: 1577 RVA: 0x000179B5 File Offset: 0x00015BB5
		[DebuggerNonUserCode]
		MessageDescriptor IMessage.Descriptor
		{
			get
			{
				return FieldOptions.Descriptor;
			}
		}

		// Token: 0x0600062A RID: 1578 RVA: 0x000179BC File Offset: 0x00015BBC
		[DebuggerNonUserCode]
		public FieldOptions()
		{
		}

		// Token: 0x0600062B RID: 1579 RVA: 0x000179D0 File Offset: 0x00015BD0
		[DebuggerNonUserCode]
		public FieldOptions(FieldOptions other)
			: this()
		{
			this._hasBits0 = other._hasBits0;
			this.ctype_ = other.ctype_;
			this.packed_ = other.packed_;
			this.jstype_ = other.jstype_;
			this.lazy_ = other.lazy_;
			this.deprecated_ = other.deprecated_;
			this.weak_ = other.weak_;
			this.uninterpretedOption_ = other.uninterpretedOption_.Clone();
			this._unknownFields = UnknownFieldSet.Clone(other._unknownFields);
			this._extensions = ExtensionSet.Clone<FieldOptions>(other._extensions);
		}

		// Token: 0x0600062C RID: 1580 RVA: 0x00017A6A File Offset: 0x00015C6A
		[DebuggerNonUserCode]
		public FieldOptions Clone()
		{
			return new FieldOptions(this);
		}

		// Token: 0x17000184 RID: 388
		// (get) Token: 0x0600062D RID: 1581 RVA: 0x00017A72 File Offset: 0x00015C72
		// (set) Token: 0x0600062E RID: 1582 RVA: 0x00017A8A File Offset: 0x00015C8A
		[DebuggerNonUserCode]
		public FieldOptions.Types.CType Ctype
		{
			get
			{
				if ((this._hasBits0 & 1) != 0)
				{
					return this.ctype_;
				}
				return FieldOptions.CtypeDefaultValue;
			}
			set
			{
				this._hasBits0 |= 1;
				this.ctype_ = value;
			}
		}

		// Token: 0x17000185 RID: 389
		// (get) Token: 0x0600062F RID: 1583 RVA: 0x00017AA1 File Offset: 0x00015CA1
		[DebuggerNonUserCode]
		public bool HasCtype
		{
			get
			{
				return (this._hasBits0 & 1) != 0;
			}
		}

		// Token: 0x06000630 RID: 1584 RVA: 0x00017AAE File Offset: 0x00015CAE
		[DebuggerNonUserCode]
		public void ClearCtype()
		{
			this._hasBits0 &= -2;
		}

		// Token: 0x17000186 RID: 390
		// (get) Token: 0x06000631 RID: 1585 RVA: 0x00017ABF File Offset: 0x00015CBF
		// (set) Token: 0x06000632 RID: 1586 RVA: 0x00017AD7 File Offset: 0x00015CD7
		[DebuggerNonUserCode]
		public bool Packed
		{
			get
			{
				if ((this._hasBits0 & 2) != 0)
				{
					return this.packed_;
				}
				return FieldOptions.PackedDefaultValue;
			}
			set
			{
				this._hasBits0 |= 2;
				this.packed_ = value;
			}
		}

		// Token: 0x17000187 RID: 391
		// (get) Token: 0x06000633 RID: 1587 RVA: 0x00017AEE File Offset: 0x00015CEE
		[DebuggerNonUserCode]
		public bool HasPacked
		{
			get
			{
				return (this._hasBits0 & 2) != 0;
			}
		}

		// Token: 0x06000634 RID: 1588 RVA: 0x00017AFB File Offset: 0x00015CFB
		[DebuggerNonUserCode]
		public void ClearPacked()
		{
			this._hasBits0 &= -3;
		}

		// Token: 0x17000188 RID: 392
		// (get) Token: 0x06000635 RID: 1589 RVA: 0x00017B0C File Offset: 0x00015D0C
		// (set) Token: 0x06000636 RID: 1590 RVA: 0x00017B25 File Offset: 0x00015D25
		[DebuggerNonUserCode]
		public FieldOptions.Types.JSType Jstype
		{
			get
			{
				if ((this._hasBits0 & 16) != 0)
				{
					return this.jstype_;
				}
				return FieldOptions.JstypeDefaultValue;
			}
			set
			{
				this._hasBits0 |= 16;
				this.jstype_ = value;
			}
		}

		// Token: 0x17000189 RID: 393
		// (get) Token: 0x06000637 RID: 1591 RVA: 0x00017B3D File Offset: 0x00015D3D
		[DebuggerNonUserCode]
		public bool HasJstype
		{
			get
			{
				return (this._hasBits0 & 16) != 0;
			}
		}

		// Token: 0x06000638 RID: 1592 RVA: 0x00017B4B File Offset: 0x00015D4B
		[DebuggerNonUserCode]
		public void ClearJstype()
		{
			this._hasBits0 &= -17;
		}

		// Token: 0x1700018A RID: 394
		// (get) Token: 0x06000639 RID: 1593 RVA: 0x00017B5C File Offset: 0x00015D5C
		// (set) Token: 0x0600063A RID: 1594 RVA: 0x00017B74 File Offset: 0x00015D74
		[DebuggerNonUserCode]
		public bool Lazy
		{
			get
			{
				if ((this._hasBits0 & 8) != 0)
				{
					return this.lazy_;
				}
				return FieldOptions.LazyDefaultValue;
			}
			set
			{
				this._hasBits0 |= 8;
				this.lazy_ = value;
			}
		}

		// Token: 0x1700018B RID: 395
		// (get) Token: 0x0600063B RID: 1595 RVA: 0x00017B8B File Offset: 0x00015D8B
		[DebuggerNonUserCode]
		public bool HasLazy
		{
			get
			{
				return (this._hasBits0 & 8) != 0;
			}
		}

		// Token: 0x0600063C RID: 1596 RVA: 0x00017B98 File Offset: 0x00015D98
		[DebuggerNonUserCode]
		public void ClearLazy()
		{
			this._hasBits0 &= -9;
		}

		// Token: 0x1700018C RID: 396
		// (get) Token: 0x0600063D RID: 1597 RVA: 0x00017BA9 File Offset: 0x00015DA9
		// (set) Token: 0x0600063E RID: 1598 RVA: 0x00017BC1 File Offset: 0x00015DC1
		[DebuggerNonUserCode]
		public bool Deprecated
		{
			get
			{
				if ((this._hasBits0 & 4) != 0)
				{
					return this.deprecated_;
				}
				return FieldOptions.DeprecatedDefaultValue;
			}
			set
			{
				this._hasBits0 |= 4;
				this.deprecated_ = value;
			}
		}

		// Token: 0x1700018D RID: 397
		// (get) Token: 0x0600063F RID: 1599 RVA: 0x00017BD8 File Offset: 0x00015DD8
		[DebuggerNonUserCode]
		public bool HasDeprecated
		{
			get
			{
				return (this._hasBits0 & 4) != 0;
			}
		}

		// Token: 0x06000640 RID: 1600 RVA: 0x00017BE5 File Offset: 0x00015DE5
		[DebuggerNonUserCode]
		public void ClearDeprecated()
		{
			this._hasBits0 &= -5;
		}

		// Token: 0x1700018E RID: 398
		// (get) Token: 0x06000641 RID: 1601 RVA: 0x00017BF6 File Offset: 0x00015DF6
		// (set) Token: 0x06000642 RID: 1602 RVA: 0x00017C0F File Offset: 0x00015E0F
		[DebuggerNonUserCode]
		public bool Weak
		{
			get
			{
				if ((this._hasBits0 & 32) != 0)
				{
					return this.weak_;
				}
				return FieldOptions.WeakDefaultValue;
			}
			set
			{
				this._hasBits0 |= 32;
				this.weak_ = value;
			}
		}

		// Token: 0x1700018F RID: 399
		// (get) Token: 0x06000643 RID: 1603 RVA: 0x00017C27 File Offset: 0x00015E27
		[DebuggerNonUserCode]
		public bool HasWeak
		{
			get
			{
				return (this._hasBits0 & 32) != 0;
			}
		}

		// Token: 0x06000644 RID: 1604 RVA: 0x00017C35 File Offset: 0x00015E35
		[DebuggerNonUserCode]
		public void ClearWeak()
		{
			this._hasBits0 &= -33;
		}

		// Token: 0x17000190 RID: 400
		// (get) Token: 0x06000645 RID: 1605 RVA: 0x00017C46 File Offset: 0x00015E46
		[DebuggerNonUserCode]
		public RepeatedField<UninterpretedOption> UninterpretedOption
		{
			get
			{
				return this.uninterpretedOption_;
			}
		}

		// Token: 0x06000646 RID: 1606 RVA: 0x00017C4E File Offset: 0x00015E4E
		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return this.Equals(other as FieldOptions);
		}

		// Token: 0x06000647 RID: 1607 RVA: 0x00017C5C File Offset: 0x00015E5C
		[DebuggerNonUserCode]
		public bool Equals(FieldOptions other)
		{
			return other != null && (other == this || (this.Ctype == other.Ctype && this.Packed == other.Packed && this.Jstype == other.Jstype && this.Lazy == other.Lazy && this.Deprecated == other.Deprecated && this.Weak == other.Weak && this.uninterpretedOption_.Equals(other.uninterpretedOption_) && object.Equals(this._extensions, other._extensions) && object.Equals(this._unknownFields, other._unknownFields)));
		}

		// Token: 0x06000648 RID: 1608 RVA: 0x00017D10 File Offset: 0x00015F10
		[DebuggerNonUserCode]
		public override int GetHashCode()
		{
			int num = 1;
			if (this.HasCtype)
			{
				num ^= this.Ctype.GetHashCode();
			}
			if (this.HasPacked)
			{
				num ^= this.Packed.GetHashCode();
			}
			if (this.HasJstype)
			{
				num ^= this.Jstype.GetHashCode();
			}
			if (this.HasLazy)
			{
				num ^= this.Lazy.GetHashCode();
			}
			if (this.HasDeprecated)
			{
				num ^= this.Deprecated.GetHashCode();
			}
			if (this.HasWeak)
			{
				num ^= this.Weak.GetHashCode();
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

		// Token: 0x06000649 RID: 1609 RVA: 0x00017DFC File Offset: 0x00015FFC
		[DebuggerNonUserCode]
		public override string ToString()
		{
			return JsonFormatter.ToDiagnosticString(this);
		}

		// Token: 0x0600064A RID: 1610 RVA: 0x00017E04 File Offset: 0x00016004
		[DebuggerNonUserCode]
		public void WriteTo(CodedOutputStream output)
		{
			if (this.HasCtype)
			{
				output.WriteRawTag(8);
				output.WriteEnum((int)this.Ctype);
			}
			if (this.HasPacked)
			{
				output.WriteRawTag(16);
				output.WriteBool(this.Packed);
			}
			if (this.HasDeprecated)
			{
				output.WriteRawTag(24);
				output.WriteBool(this.Deprecated);
			}
			if (this.HasLazy)
			{
				output.WriteRawTag(40);
				output.WriteBool(this.Lazy);
			}
			if (this.HasJstype)
			{
				output.WriteRawTag(48);
				output.WriteEnum((int)this.Jstype);
			}
			if (this.HasWeak)
			{
				output.WriteRawTag(80);
				output.WriteBool(this.Weak);
			}
			this.uninterpretedOption_.WriteTo(output, FieldOptions._repeated_uninterpretedOption_codec);
			if (this._extensions != null)
			{
				this._extensions.WriteTo(output);
			}
			if (this._unknownFields != null)
			{
				this._unknownFields.WriteTo(output);
			}
		}

		// Token: 0x0600064B RID: 1611 RVA: 0x00017EF4 File Offset: 0x000160F4
		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (this.HasCtype)
			{
				num += 1 + CodedOutputStream.ComputeEnumSize((int)this.Ctype);
			}
			if (this.HasPacked)
			{
				num += 2;
			}
			if (this.HasJstype)
			{
				num += 1 + CodedOutputStream.ComputeEnumSize((int)this.Jstype);
			}
			if (this.HasLazy)
			{
				num += 2;
			}
			if (this.HasDeprecated)
			{
				num += 2;
			}
			if (this.HasWeak)
			{
				num += 2;
			}
			num += this.uninterpretedOption_.CalculateSize(FieldOptions._repeated_uninterpretedOption_codec);
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

		// Token: 0x0600064C RID: 1612 RVA: 0x00017FA4 File Offset: 0x000161A4
		[DebuggerNonUserCode]
		public void MergeFrom(FieldOptions other)
		{
			if (other == null)
			{
				return;
			}
			if (other.HasCtype)
			{
				this.Ctype = other.Ctype;
			}
			if (other.HasPacked)
			{
				this.Packed = other.Packed;
			}
			if (other.HasJstype)
			{
				this.Jstype = other.Jstype;
			}
			if (other.HasLazy)
			{
				this.Lazy = other.Lazy;
			}
			if (other.HasDeprecated)
			{
				this.Deprecated = other.Deprecated;
			}
			if (other.HasWeak)
			{
				this.Weak = other.Weak;
			}
			this.uninterpretedOption_.Add(other.uninterpretedOption_);
			ExtensionSet.MergeFrom<FieldOptions>(ref this._extensions, other._extensions);
			this._unknownFields = UnknownFieldSet.MergeFrom(this._unknownFields, other._unknownFields);
		}

		// Token: 0x0600064D RID: 1613 RVA: 0x00018068 File Offset: 0x00016268
		[DebuggerNonUserCode]
		public void MergeFrom(CodedInputStream input)
		{
			uint num;
			while ((num = input.ReadTag()) != 0U)
			{
				if (num <= 24U)
				{
					if (num == 8U)
					{
						this.Ctype = (FieldOptions.Types.CType)input.ReadEnum();
						continue;
					}
					if (num == 16U)
					{
						this.Packed = input.ReadBool();
						continue;
					}
					if (num == 24U)
					{
						this.Deprecated = input.ReadBool();
						continue;
					}
				}
				else if (num <= 48U)
				{
					if (num == 40U)
					{
						this.Lazy = input.ReadBool();
						continue;
					}
					if (num == 48U)
					{
						this.Jstype = (FieldOptions.Types.JSType)input.ReadEnum();
						continue;
					}
				}
				else
				{
					if (num == 80U)
					{
						this.Weak = input.ReadBool();
						continue;
					}
					if (num == 7994U)
					{
						this.uninterpretedOption_.AddEntriesFrom(input, FieldOptions._repeated_uninterpretedOption_codec);
						continue;
					}
				}
				if (!ExtensionSet.TryMergeFieldFrom<FieldOptions>(ref this._extensions, input))
				{
					this._unknownFields = UnknownFieldSet.MergeFieldFrom(this._unknownFields, input);
				}
			}
		}

		// Token: 0x0600064E RID: 1614 RVA: 0x00018141 File Offset: 0x00016341
		public TValue GetExtension<TValue>(Extension<FieldOptions, TValue> extension)
		{
			return ExtensionSet.Get<FieldOptions, TValue>(ref this._extensions, extension);
		}

		// Token: 0x0600064F RID: 1615 RVA: 0x0001814F File Offset: 0x0001634F
		public RepeatedField<TValue> GetExtension<TValue>(RepeatedExtension<FieldOptions, TValue> extension)
		{
			return ExtensionSet.Get<FieldOptions, TValue>(ref this._extensions, extension);
		}

		// Token: 0x06000650 RID: 1616 RVA: 0x0001815D File Offset: 0x0001635D
		public RepeatedField<TValue> GetOrInitializeExtension<TValue>(RepeatedExtension<FieldOptions, TValue> extension)
		{
			return ExtensionSet.GetOrInitialize<FieldOptions, TValue>(ref this._extensions, extension);
		}

		// Token: 0x06000651 RID: 1617 RVA: 0x0001816B File Offset: 0x0001636B
		public void SetExtension<TValue>(Extension<FieldOptions, TValue> extension, TValue value)
		{
			ExtensionSet.Set<FieldOptions, TValue>(ref this._extensions, extension, value);
		}

		// Token: 0x06000652 RID: 1618 RVA: 0x0001817A File Offset: 0x0001637A
		public bool HasExtension<TValue>(Extension<FieldOptions, TValue> extension)
		{
			return ExtensionSet.Has<FieldOptions, TValue>(ref this._extensions, extension);
		}

		// Token: 0x06000653 RID: 1619 RVA: 0x00018188 File Offset: 0x00016388
		public void ClearExtension<TValue>(Extension<FieldOptions, TValue> extension)
		{
			ExtensionSet.Clear<FieldOptions, TValue>(ref this._extensions, extension);
		}

		// Token: 0x06000654 RID: 1620 RVA: 0x00018196 File Offset: 0x00016396
		public void ClearExtension<TValue>(RepeatedExtension<FieldOptions, TValue> extension)
		{
			ExtensionSet.Clear<FieldOptions, TValue>(ref this._extensions, extension);
		}

		// Token: 0x0400025A RID: 602
		private static readonly MessageParser<FieldOptions> _parser = new MessageParser<FieldOptions>(() => new FieldOptions());

		// Token: 0x0400025B RID: 603
		private UnknownFieldSet _unknownFields;

		// Token: 0x0400025C RID: 604
		internal ExtensionSet<FieldOptions> _extensions;

		// Token: 0x0400025D RID: 605
		private int _hasBits0;

		// Token: 0x0400025E RID: 606
		public const int CtypeFieldNumber = 1;

		// Token: 0x0400025F RID: 607
		private static readonly FieldOptions.Types.CType CtypeDefaultValue = FieldOptions.Types.CType.String;

		// Token: 0x04000260 RID: 608
		private FieldOptions.Types.CType ctype_;

		// Token: 0x04000261 RID: 609
		public const int PackedFieldNumber = 2;

		// Token: 0x04000262 RID: 610
		private static readonly bool PackedDefaultValue = false;

		// Token: 0x04000263 RID: 611
		private bool packed_;

		// Token: 0x04000264 RID: 612
		public const int JstypeFieldNumber = 6;

		// Token: 0x04000265 RID: 613
		private static readonly FieldOptions.Types.JSType JstypeDefaultValue = FieldOptions.Types.JSType.JsNormal;

		// Token: 0x04000266 RID: 614
		private FieldOptions.Types.JSType jstype_;

		// Token: 0x04000267 RID: 615
		public const int LazyFieldNumber = 5;

		// Token: 0x04000268 RID: 616
		private static readonly bool LazyDefaultValue = false;

		// Token: 0x04000269 RID: 617
		private bool lazy_;

		// Token: 0x0400026A RID: 618
		public const int DeprecatedFieldNumber = 3;

		// Token: 0x0400026B RID: 619
		private static readonly bool DeprecatedDefaultValue = false;

		// Token: 0x0400026C RID: 620
		private bool deprecated_;

		// Token: 0x0400026D RID: 621
		public const int WeakFieldNumber = 10;

		// Token: 0x0400026E RID: 622
		private static readonly bool WeakDefaultValue = false;

		// Token: 0x0400026F RID: 623
		private bool weak_;

		// Token: 0x04000270 RID: 624
		public const int UninterpretedOptionFieldNumber = 999;

		// Token: 0x04000271 RID: 625
		private static readonly FieldCodec<UninterpretedOption> _repeated_uninterpretedOption_codec = FieldCodec.ForMessage<UninterpretedOption>(7994U, Google.Protobuf.Reflection.UninterpretedOption.Parser);

		// Token: 0x04000272 RID: 626
		private readonly RepeatedField<UninterpretedOption> uninterpretedOption_ = new RepeatedField<UninterpretedOption>();

		// Token: 0x020000D9 RID: 217
		[DebuggerNonUserCode]
		public static class Types
		{
			// Token: 0x02000119 RID: 281
			public enum CType
			{
				// Token: 0x040004A8 RID: 1192
				[OriginalName("STRING")]
				String,
				// Token: 0x040004A9 RID: 1193
				[OriginalName("CORD")]
				Cord,
				// Token: 0x040004AA RID: 1194
				[OriginalName("STRING_PIECE")]
				StringPiece
			}

			// Token: 0x0200011A RID: 282
			public enum JSType
			{
				// Token: 0x040004AC RID: 1196
				[OriginalName("JS_NORMAL")]
				JsNormal,
				// Token: 0x040004AD RID: 1197
				[OriginalName("JS_STRING")]
				JsString,
				// Token: 0x040004AE RID: 1198
				[OriginalName("JS_NUMBER")]
				JsNumber
			}
		}
	}
}
