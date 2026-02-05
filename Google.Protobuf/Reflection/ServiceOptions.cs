using System;
using System.Diagnostics;
using Google.Protobuf.Collections;

namespace Google.Protobuf.Reflection
{
	// Token: 0x02000060 RID: 96
	public sealed class ServiceOptions : IExtendableMessage<ServiceOptions>, IMessage<ServiceOptions>, IMessage, IEquatable<ServiceOptions>, IDeepCloneable<ServiceOptions>
	{
		// Token: 0x170001A6 RID: 422
		// (get) Token: 0x060006AA RID: 1706 RVA: 0x00018DBB File Offset: 0x00016FBB
		private ExtensionSet<ServiceOptions> _Extensions
		{
			get
			{
				return this._extensions;
			}
		}

		// Token: 0x170001A7 RID: 423
		// (get) Token: 0x060006AB RID: 1707 RVA: 0x00018DC3 File Offset: 0x00016FC3
		[DebuggerNonUserCode]
		public static MessageParser<ServiceOptions> Parser
		{
			get
			{
				return ServiceOptions._parser;
			}
		}

		// Token: 0x170001A8 RID: 424
		// (get) Token: 0x060006AC RID: 1708 RVA: 0x00018DCA File Offset: 0x00016FCA
		[DebuggerNonUserCode]
		public static MessageDescriptor Descriptor
		{
			get
			{
				return DescriptorReflection.Descriptor.MessageTypes[16];
			}
		}

		// Token: 0x170001A9 RID: 425
		// (get) Token: 0x060006AD RID: 1709 RVA: 0x00018DDD File Offset: 0x00016FDD
		[DebuggerNonUserCode]
		MessageDescriptor IMessage.Descriptor
		{
			get
			{
				return ServiceOptions.Descriptor;
			}
		}

		// Token: 0x060006AE RID: 1710 RVA: 0x00018DE4 File Offset: 0x00016FE4
		[DebuggerNonUserCode]
		public ServiceOptions()
		{
		}

		// Token: 0x060006AF RID: 1711 RVA: 0x00018DF8 File Offset: 0x00016FF8
		[DebuggerNonUserCode]
		public ServiceOptions(ServiceOptions other)
			: this()
		{
			this._hasBits0 = other._hasBits0;
			this.deprecated_ = other.deprecated_;
			this.uninterpretedOption_ = other.uninterpretedOption_.Clone();
			this._unknownFields = UnknownFieldSet.Clone(other._unknownFields);
			this._extensions = ExtensionSet.Clone<ServiceOptions>(other._extensions);
		}

		// Token: 0x060006B0 RID: 1712 RVA: 0x00018E56 File Offset: 0x00017056
		[DebuggerNonUserCode]
		public ServiceOptions Clone()
		{
			return new ServiceOptions(this);
		}

		// Token: 0x170001AA RID: 426
		// (get) Token: 0x060006B1 RID: 1713 RVA: 0x00018E5E File Offset: 0x0001705E
		// (set) Token: 0x060006B2 RID: 1714 RVA: 0x00018E76 File Offset: 0x00017076
		[DebuggerNonUserCode]
		public bool Deprecated
		{
			get
			{
				if ((this._hasBits0 & 1) != 0)
				{
					return this.deprecated_;
				}
				return ServiceOptions.DeprecatedDefaultValue;
			}
			set
			{
				this._hasBits0 |= 1;
				this.deprecated_ = value;
			}
		}

		// Token: 0x170001AB RID: 427
		// (get) Token: 0x060006B3 RID: 1715 RVA: 0x00018E8D File Offset: 0x0001708D
		[DebuggerNonUserCode]
		public bool HasDeprecated
		{
			get
			{
				return (this._hasBits0 & 1) != 0;
			}
		}

		// Token: 0x060006B4 RID: 1716 RVA: 0x00018E9A File Offset: 0x0001709A
		[DebuggerNonUserCode]
		public void ClearDeprecated()
		{
			this._hasBits0 &= -2;
		}

		// Token: 0x170001AC RID: 428
		// (get) Token: 0x060006B5 RID: 1717 RVA: 0x00018EAB File Offset: 0x000170AB
		[DebuggerNonUserCode]
		public RepeatedField<UninterpretedOption> UninterpretedOption
		{
			get
			{
				return this.uninterpretedOption_;
			}
		}

		// Token: 0x060006B6 RID: 1718 RVA: 0x00018EB3 File Offset: 0x000170B3
		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return this.Equals(other as ServiceOptions);
		}

		// Token: 0x060006B7 RID: 1719 RVA: 0x00018EC4 File Offset: 0x000170C4
		[DebuggerNonUserCode]
		public bool Equals(ServiceOptions other)
		{
			return other != null && (other == this || (this.Deprecated == other.Deprecated && this.uninterpretedOption_.Equals(other.uninterpretedOption_) && object.Equals(this._extensions, other._extensions) && object.Equals(this._unknownFields, other._unknownFields)));
		}

		// Token: 0x060006B8 RID: 1720 RVA: 0x00018F28 File Offset: 0x00017128
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

		// Token: 0x060006B9 RID: 1721 RVA: 0x00018F8B File Offset: 0x0001718B
		[DebuggerNonUserCode]
		public override string ToString()
		{
			return JsonFormatter.ToDiagnosticString(this);
		}

		// Token: 0x060006BA RID: 1722 RVA: 0x00018F94 File Offset: 0x00017194
		[DebuggerNonUserCode]
		public void WriteTo(CodedOutputStream output)
		{
			if (this.HasDeprecated)
			{
				output.WriteRawTag(136, 2);
				output.WriteBool(this.Deprecated);
			}
			this.uninterpretedOption_.WriteTo(output, ServiceOptions._repeated_uninterpretedOption_codec);
			if (this._extensions != null)
			{
				this._extensions.WriteTo(output);
			}
			if (this._unknownFields != null)
			{
				this._unknownFields.WriteTo(output);
			}
		}

		// Token: 0x060006BB RID: 1723 RVA: 0x00018FFC File Offset: 0x000171FC
		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (this.HasDeprecated)
			{
				num += 3;
			}
			num += this.uninterpretedOption_.CalculateSize(ServiceOptions._repeated_uninterpretedOption_codec);
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

		// Token: 0x060006BC RID: 1724 RVA: 0x00019058 File Offset: 0x00017258
		[DebuggerNonUserCode]
		public void MergeFrom(ServiceOptions other)
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
			ExtensionSet.MergeFrom<ServiceOptions>(ref this._extensions, other._extensions);
			this._unknownFields = UnknownFieldSet.MergeFrom(this._unknownFields, other._unknownFields);
		}

		// Token: 0x060006BD RID: 1725 RVA: 0x000190B8 File Offset: 0x000172B8
		[DebuggerNonUserCode]
		public void MergeFrom(CodedInputStream input)
		{
			uint num;
			while ((num = input.ReadTag()) != 0U)
			{
				if (num != 264U)
				{
					if (num != 7994U)
					{
						if (!ExtensionSet.TryMergeFieldFrom<ServiceOptions>(ref this._extensions, input))
						{
							this._unknownFields = UnknownFieldSet.MergeFieldFrom(this._unknownFields, input);
						}
					}
					else
					{
						this.uninterpretedOption_.AddEntriesFrom(input, ServiceOptions._repeated_uninterpretedOption_codec);
					}
				}
				else
				{
					this.Deprecated = input.ReadBool();
				}
			}
		}

		// Token: 0x060006BE RID: 1726 RVA: 0x00019122 File Offset: 0x00017322
		public TValue GetExtension<TValue>(Extension<ServiceOptions, TValue> extension)
		{
			return ExtensionSet.Get<ServiceOptions, TValue>(ref this._extensions, extension);
		}

		// Token: 0x060006BF RID: 1727 RVA: 0x00019130 File Offset: 0x00017330
		public RepeatedField<TValue> GetExtension<TValue>(RepeatedExtension<ServiceOptions, TValue> extension)
		{
			return ExtensionSet.Get<ServiceOptions, TValue>(ref this._extensions, extension);
		}

		// Token: 0x060006C0 RID: 1728 RVA: 0x0001913E File Offset: 0x0001733E
		public RepeatedField<TValue> GetOrInitializeExtension<TValue>(RepeatedExtension<ServiceOptions, TValue> extension)
		{
			return ExtensionSet.GetOrInitialize<ServiceOptions, TValue>(ref this._extensions, extension);
		}

		// Token: 0x060006C1 RID: 1729 RVA: 0x0001914C File Offset: 0x0001734C
		public void SetExtension<TValue>(Extension<ServiceOptions, TValue> extension, TValue value)
		{
			ExtensionSet.Set<ServiceOptions, TValue>(ref this._extensions, extension, value);
		}

		// Token: 0x060006C2 RID: 1730 RVA: 0x0001915B File Offset: 0x0001735B
		public bool HasExtension<TValue>(Extension<ServiceOptions, TValue> extension)
		{
			return ExtensionSet.Has<ServiceOptions, TValue>(ref this._extensions, extension);
		}

		// Token: 0x060006C3 RID: 1731 RVA: 0x00019169 File Offset: 0x00017369
		public void ClearExtension<TValue>(Extension<ServiceOptions, TValue> extension)
		{
			ExtensionSet.Clear<ServiceOptions, TValue>(ref this._extensions, extension);
		}

		// Token: 0x060006C4 RID: 1732 RVA: 0x00019177 File Offset: 0x00017377
		public void ClearExtension<TValue>(RepeatedExtension<ServiceOptions, TValue> extension)
		{
			ExtensionSet.Clear<ServiceOptions, TValue>(ref this._extensions, extension);
		}

		// Token: 0x04000290 RID: 656
		private static readonly MessageParser<ServiceOptions> _parser = new MessageParser<ServiceOptions>(() => new ServiceOptions());

		// Token: 0x04000291 RID: 657
		private UnknownFieldSet _unknownFields;

		// Token: 0x04000292 RID: 658
		internal ExtensionSet<ServiceOptions> _extensions;

		// Token: 0x04000293 RID: 659
		private int _hasBits0;

		// Token: 0x04000294 RID: 660
		public const int DeprecatedFieldNumber = 33;

		// Token: 0x04000295 RID: 661
		private static readonly bool DeprecatedDefaultValue = false;

		// Token: 0x04000296 RID: 662
		private bool deprecated_;

		// Token: 0x04000297 RID: 663
		public const int UninterpretedOptionFieldNumber = 999;

		// Token: 0x04000298 RID: 664
		private static readonly FieldCodec<UninterpretedOption> _repeated_uninterpretedOption_codec = FieldCodec.ForMessage<UninterpretedOption>(7994U, Google.Protobuf.Reflection.UninterpretedOption.Parser);

		// Token: 0x04000299 RID: 665
		private readonly RepeatedField<UninterpretedOption> uninterpretedOption_ = new RepeatedField<UninterpretedOption>();
	}
}
