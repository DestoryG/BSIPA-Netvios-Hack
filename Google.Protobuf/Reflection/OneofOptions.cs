using System;
using System.Diagnostics;
using Google.Protobuf.Collections;

namespace Google.Protobuf.Reflection
{
	// Token: 0x0200005D RID: 93
	public sealed class OneofOptions : IExtendableMessage<OneofOptions>, IMessage<OneofOptions>, IMessage, IEquatable<OneofOptions>, IDeepCloneable<OneofOptions>
	{
		// Token: 0x17000191 RID: 401
		// (get) Token: 0x06000656 RID: 1622 RVA: 0x00018203 File Offset: 0x00016403
		private ExtensionSet<OneofOptions> _Extensions
		{
			get
			{
				return this._extensions;
			}
		}

		// Token: 0x17000192 RID: 402
		// (get) Token: 0x06000657 RID: 1623 RVA: 0x0001820B File Offset: 0x0001640B
		[DebuggerNonUserCode]
		public static MessageParser<OneofOptions> Parser
		{
			get
			{
				return OneofOptions._parser;
			}
		}

		// Token: 0x17000193 RID: 403
		// (get) Token: 0x06000658 RID: 1624 RVA: 0x00018212 File Offset: 0x00016412
		[DebuggerNonUserCode]
		public static MessageDescriptor Descriptor
		{
			get
			{
				return DescriptorReflection.Descriptor.MessageTypes[13];
			}
		}

		// Token: 0x17000194 RID: 404
		// (get) Token: 0x06000659 RID: 1625 RVA: 0x00018225 File Offset: 0x00016425
		[DebuggerNonUserCode]
		MessageDescriptor IMessage.Descriptor
		{
			get
			{
				return OneofOptions.Descriptor;
			}
		}

		// Token: 0x0600065A RID: 1626 RVA: 0x0001822C File Offset: 0x0001642C
		[DebuggerNonUserCode]
		public OneofOptions()
		{
		}

		// Token: 0x0600065B RID: 1627 RVA: 0x0001823F File Offset: 0x0001643F
		[DebuggerNonUserCode]
		public OneofOptions(OneofOptions other)
			: this()
		{
			this.uninterpretedOption_ = other.uninterpretedOption_.Clone();
			this._unknownFields = UnknownFieldSet.Clone(other._unknownFields);
			this._extensions = ExtensionSet.Clone<OneofOptions>(other._extensions);
		}

		// Token: 0x0600065C RID: 1628 RVA: 0x0001827A File Offset: 0x0001647A
		[DebuggerNonUserCode]
		public OneofOptions Clone()
		{
			return new OneofOptions(this);
		}

		// Token: 0x17000195 RID: 405
		// (get) Token: 0x0600065D RID: 1629 RVA: 0x00018282 File Offset: 0x00016482
		[DebuggerNonUserCode]
		public RepeatedField<UninterpretedOption> UninterpretedOption
		{
			get
			{
				return this.uninterpretedOption_;
			}
		}

		// Token: 0x0600065E RID: 1630 RVA: 0x0001828A File Offset: 0x0001648A
		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return this.Equals(other as OneofOptions);
		}

		// Token: 0x0600065F RID: 1631 RVA: 0x00018298 File Offset: 0x00016498
		[DebuggerNonUserCode]
		public bool Equals(OneofOptions other)
		{
			return other != null && (other == this || (this.uninterpretedOption_.Equals(other.uninterpretedOption_) && object.Equals(this._extensions, other._extensions) && object.Equals(this._unknownFields, other._unknownFields)));
		}

		// Token: 0x06000660 RID: 1632 RVA: 0x000182EC File Offset: 0x000164EC
		[DebuggerNonUserCode]
		public override int GetHashCode()
		{
			int num = 1;
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

		// Token: 0x06000661 RID: 1633 RVA: 0x00018336 File Offset: 0x00016536
		[DebuggerNonUserCode]
		public override string ToString()
		{
			return JsonFormatter.ToDiagnosticString(this);
		}

		// Token: 0x06000662 RID: 1634 RVA: 0x0001833E File Offset: 0x0001653E
		[DebuggerNonUserCode]
		public void WriteTo(CodedOutputStream output)
		{
			this.uninterpretedOption_.WriteTo(output, OneofOptions._repeated_uninterpretedOption_codec);
			if (this._extensions != null)
			{
				this._extensions.WriteTo(output);
			}
			if (this._unknownFields != null)
			{
				this._unknownFields.WriteTo(output);
			}
		}

		// Token: 0x06000663 RID: 1635 RVA: 0x0001837C File Offset: 0x0001657C
		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			num += this.uninterpretedOption_.CalculateSize(OneofOptions._repeated_uninterpretedOption_codec);
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

		// Token: 0x06000664 RID: 1636 RVA: 0x000183CB File Offset: 0x000165CB
		[DebuggerNonUserCode]
		public void MergeFrom(OneofOptions other)
		{
			if (other == null)
			{
				return;
			}
			this.uninterpretedOption_.Add(other.uninterpretedOption_);
			ExtensionSet.MergeFrom<OneofOptions>(ref this._extensions, other._extensions);
			this._unknownFields = UnknownFieldSet.MergeFrom(this._unknownFields, other._unknownFields);
		}

		// Token: 0x06000665 RID: 1637 RVA: 0x0001840C File Offset: 0x0001660C
		[DebuggerNonUserCode]
		public void MergeFrom(CodedInputStream input)
		{
			uint num;
			while ((num = input.ReadTag()) != 0U)
			{
				if (num != 7994U)
				{
					if (!ExtensionSet.TryMergeFieldFrom<OneofOptions>(ref this._extensions, input))
					{
						this._unknownFields = UnknownFieldSet.MergeFieldFrom(this._unknownFields, input);
					}
				}
				else
				{
					this.uninterpretedOption_.AddEntriesFrom(input, OneofOptions._repeated_uninterpretedOption_codec);
				}
			}
		}

		// Token: 0x06000666 RID: 1638 RVA: 0x00018460 File Offset: 0x00016660
		public TValue GetExtension<TValue>(Extension<OneofOptions, TValue> extension)
		{
			return ExtensionSet.Get<OneofOptions, TValue>(ref this._extensions, extension);
		}

		// Token: 0x06000667 RID: 1639 RVA: 0x0001846E File Offset: 0x0001666E
		public RepeatedField<TValue> GetExtension<TValue>(RepeatedExtension<OneofOptions, TValue> extension)
		{
			return ExtensionSet.Get<OneofOptions, TValue>(ref this._extensions, extension);
		}

		// Token: 0x06000668 RID: 1640 RVA: 0x0001847C File Offset: 0x0001667C
		public RepeatedField<TValue> GetOrInitializeExtension<TValue>(RepeatedExtension<OneofOptions, TValue> extension)
		{
			return ExtensionSet.GetOrInitialize<OneofOptions, TValue>(ref this._extensions, extension);
		}

		// Token: 0x06000669 RID: 1641 RVA: 0x0001848A File Offset: 0x0001668A
		public void SetExtension<TValue>(Extension<OneofOptions, TValue> extension, TValue value)
		{
			ExtensionSet.Set<OneofOptions, TValue>(ref this._extensions, extension, value);
		}

		// Token: 0x0600066A RID: 1642 RVA: 0x00018499 File Offset: 0x00016699
		public bool HasExtension<TValue>(Extension<OneofOptions, TValue> extension)
		{
			return ExtensionSet.Has<OneofOptions, TValue>(ref this._extensions, extension);
		}

		// Token: 0x0600066B RID: 1643 RVA: 0x000184A7 File Offset: 0x000166A7
		public void ClearExtension<TValue>(Extension<OneofOptions, TValue> extension)
		{
			ExtensionSet.Clear<OneofOptions, TValue>(ref this._extensions, extension);
		}

		// Token: 0x0600066C RID: 1644 RVA: 0x000184B5 File Offset: 0x000166B5
		public void ClearExtension<TValue>(RepeatedExtension<OneofOptions, TValue> extension)
		{
			ExtensionSet.Clear<OneofOptions, TValue>(ref this._extensions, extension);
		}

		// Token: 0x04000273 RID: 627
		private static readonly MessageParser<OneofOptions> _parser = new MessageParser<OneofOptions>(() => new OneofOptions());

		// Token: 0x04000274 RID: 628
		private UnknownFieldSet _unknownFields;

		// Token: 0x04000275 RID: 629
		internal ExtensionSet<OneofOptions> _extensions;

		// Token: 0x04000276 RID: 630
		public const int UninterpretedOptionFieldNumber = 999;

		// Token: 0x04000277 RID: 631
		private static readonly FieldCodec<UninterpretedOption> _repeated_uninterpretedOption_codec = FieldCodec.ForMessage<UninterpretedOption>(7994U, Google.Protobuf.Reflection.UninterpretedOption.Parser);

		// Token: 0x04000278 RID: 632
		private readonly RepeatedField<UninterpretedOption> uninterpretedOption_ = new RepeatedField<UninterpretedOption>();
	}
}
