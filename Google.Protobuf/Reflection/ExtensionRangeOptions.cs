using System;
using System.Diagnostics;
using Google.Protobuf.Collections;

namespace Google.Protobuf.Reflection
{
	// Token: 0x02000053 RID: 83
	public sealed class ExtensionRangeOptions : IExtendableMessage<ExtensionRangeOptions>, IMessage<ExtensionRangeOptions>, IMessage, IEquatable<ExtensionRangeOptions>, IDeepCloneable<ExtensionRangeOptions>
	{
		// Token: 0x170000FD RID: 253
		// (get) Token: 0x060004C4 RID: 1220 RVA: 0x0001388C File Offset: 0x00011A8C
		private ExtensionSet<ExtensionRangeOptions> _Extensions
		{
			get
			{
				return this._extensions;
			}
		}

		// Token: 0x170000FE RID: 254
		// (get) Token: 0x060004C5 RID: 1221 RVA: 0x00013894 File Offset: 0x00011A94
		[DebuggerNonUserCode]
		public static MessageParser<ExtensionRangeOptions> Parser
		{
			get
			{
				return ExtensionRangeOptions._parser;
			}
		}

		// Token: 0x170000FF RID: 255
		// (get) Token: 0x060004C6 RID: 1222 RVA: 0x0001389B File Offset: 0x00011A9B
		[DebuggerNonUserCode]
		public static MessageDescriptor Descriptor
		{
			get
			{
				return DescriptorReflection.Descriptor.MessageTypes[3];
			}
		}

		// Token: 0x17000100 RID: 256
		// (get) Token: 0x060004C7 RID: 1223 RVA: 0x000138AD File Offset: 0x00011AAD
		[DebuggerNonUserCode]
		MessageDescriptor IMessage.Descriptor
		{
			get
			{
				return ExtensionRangeOptions.Descriptor;
			}
		}

		// Token: 0x060004C8 RID: 1224 RVA: 0x000138B4 File Offset: 0x00011AB4
		[DebuggerNonUserCode]
		public ExtensionRangeOptions()
		{
		}

		// Token: 0x060004C9 RID: 1225 RVA: 0x000138C7 File Offset: 0x00011AC7
		[DebuggerNonUserCode]
		public ExtensionRangeOptions(ExtensionRangeOptions other)
			: this()
		{
			this.uninterpretedOption_ = other.uninterpretedOption_.Clone();
			this._unknownFields = UnknownFieldSet.Clone(other._unknownFields);
			this._extensions = ExtensionSet.Clone<ExtensionRangeOptions>(other._extensions);
		}

		// Token: 0x060004CA RID: 1226 RVA: 0x00013902 File Offset: 0x00011B02
		[DebuggerNonUserCode]
		public ExtensionRangeOptions Clone()
		{
			return new ExtensionRangeOptions(this);
		}

		// Token: 0x17000101 RID: 257
		// (get) Token: 0x060004CB RID: 1227 RVA: 0x0001390A File Offset: 0x00011B0A
		[DebuggerNonUserCode]
		public RepeatedField<UninterpretedOption> UninterpretedOption
		{
			get
			{
				return this.uninterpretedOption_;
			}
		}

		// Token: 0x060004CC RID: 1228 RVA: 0x00013912 File Offset: 0x00011B12
		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return this.Equals(other as ExtensionRangeOptions);
		}

		// Token: 0x060004CD RID: 1229 RVA: 0x00013920 File Offset: 0x00011B20
		[DebuggerNonUserCode]
		public bool Equals(ExtensionRangeOptions other)
		{
			return other != null && (other == this || (this.uninterpretedOption_.Equals(other.uninterpretedOption_) && object.Equals(this._extensions, other._extensions) && object.Equals(this._unknownFields, other._unknownFields)));
		}

		// Token: 0x060004CE RID: 1230 RVA: 0x00013974 File Offset: 0x00011B74
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

		// Token: 0x060004CF RID: 1231 RVA: 0x000139BE File Offset: 0x00011BBE
		[DebuggerNonUserCode]
		public override string ToString()
		{
			return JsonFormatter.ToDiagnosticString(this);
		}

		// Token: 0x060004D0 RID: 1232 RVA: 0x000139C6 File Offset: 0x00011BC6
		[DebuggerNonUserCode]
		public void WriteTo(CodedOutputStream output)
		{
			this.uninterpretedOption_.WriteTo(output, ExtensionRangeOptions._repeated_uninterpretedOption_codec);
			if (this._extensions != null)
			{
				this._extensions.WriteTo(output);
			}
			if (this._unknownFields != null)
			{
				this._unknownFields.WriteTo(output);
			}
		}

		// Token: 0x060004D1 RID: 1233 RVA: 0x00013A04 File Offset: 0x00011C04
		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			num += this.uninterpretedOption_.CalculateSize(ExtensionRangeOptions._repeated_uninterpretedOption_codec);
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

		// Token: 0x060004D2 RID: 1234 RVA: 0x00013A53 File Offset: 0x00011C53
		[DebuggerNonUserCode]
		public void MergeFrom(ExtensionRangeOptions other)
		{
			if (other == null)
			{
				return;
			}
			this.uninterpretedOption_.Add(other.uninterpretedOption_);
			ExtensionSet.MergeFrom<ExtensionRangeOptions>(ref this._extensions, other._extensions);
			this._unknownFields = UnknownFieldSet.MergeFrom(this._unknownFields, other._unknownFields);
		}

		// Token: 0x060004D3 RID: 1235 RVA: 0x00013A94 File Offset: 0x00011C94
		[DebuggerNonUserCode]
		public void MergeFrom(CodedInputStream input)
		{
			uint num;
			while ((num = input.ReadTag()) != 0U)
			{
				if (num != 7994U)
				{
					if (!ExtensionSet.TryMergeFieldFrom<ExtensionRangeOptions>(ref this._extensions, input))
					{
						this._unknownFields = UnknownFieldSet.MergeFieldFrom(this._unknownFields, input);
					}
				}
				else
				{
					this.uninterpretedOption_.AddEntriesFrom(input, ExtensionRangeOptions._repeated_uninterpretedOption_codec);
				}
			}
		}

		// Token: 0x060004D4 RID: 1236 RVA: 0x00013AE8 File Offset: 0x00011CE8
		public TValue GetExtension<TValue>(Extension<ExtensionRangeOptions, TValue> extension)
		{
			return ExtensionSet.Get<ExtensionRangeOptions, TValue>(ref this._extensions, extension);
		}

		// Token: 0x060004D5 RID: 1237 RVA: 0x00013AF6 File Offset: 0x00011CF6
		public RepeatedField<TValue> GetExtension<TValue>(RepeatedExtension<ExtensionRangeOptions, TValue> extension)
		{
			return ExtensionSet.Get<ExtensionRangeOptions, TValue>(ref this._extensions, extension);
		}

		// Token: 0x060004D6 RID: 1238 RVA: 0x00013B04 File Offset: 0x00011D04
		public RepeatedField<TValue> GetOrInitializeExtension<TValue>(RepeatedExtension<ExtensionRangeOptions, TValue> extension)
		{
			return ExtensionSet.GetOrInitialize<ExtensionRangeOptions, TValue>(ref this._extensions, extension);
		}

		// Token: 0x060004D7 RID: 1239 RVA: 0x00013B12 File Offset: 0x00011D12
		public void SetExtension<TValue>(Extension<ExtensionRangeOptions, TValue> extension, TValue value)
		{
			ExtensionSet.Set<ExtensionRangeOptions, TValue>(ref this._extensions, extension, value);
		}

		// Token: 0x060004D8 RID: 1240 RVA: 0x00013B21 File Offset: 0x00011D21
		public bool HasExtension<TValue>(Extension<ExtensionRangeOptions, TValue> extension)
		{
			return ExtensionSet.Has<ExtensionRangeOptions, TValue>(ref this._extensions, extension);
		}

		// Token: 0x060004D9 RID: 1241 RVA: 0x00013B2F File Offset: 0x00011D2F
		public void ClearExtension<TValue>(Extension<ExtensionRangeOptions, TValue> extension)
		{
			ExtensionSet.Clear<ExtensionRangeOptions, TValue>(ref this._extensions, extension);
		}

		// Token: 0x060004DA RID: 1242 RVA: 0x00013B3D File Offset: 0x00011D3D
		public void ClearExtension<TValue>(RepeatedExtension<ExtensionRangeOptions, TValue> extension)
		{
			ExtensionSet.Clear<ExtensionRangeOptions, TValue>(ref this._extensions, extension);
		}

		// Token: 0x0400019B RID: 411
		private static readonly MessageParser<ExtensionRangeOptions> _parser = new MessageParser<ExtensionRangeOptions>(() => new ExtensionRangeOptions());

		// Token: 0x0400019C RID: 412
		private UnknownFieldSet _unknownFields;

		// Token: 0x0400019D RID: 413
		internal ExtensionSet<ExtensionRangeOptions> _extensions;

		// Token: 0x0400019E RID: 414
		public const int UninterpretedOptionFieldNumber = 999;

		// Token: 0x0400019F RID: 415
		private static readonly FieldCodec<UninterpretedOption> _repeated_uninterpretedOption_codec = FieldCodec.ForMessage<UninterpretedOption>(7994U, Google.Protobuf.Reflection.UninterpretedOption.Parser);

		// Token: 0x040001A0 RID: 416
		private readonly RepeatedField<UninterpretedOption> uninterpretedOption_ = new RepeatedField<UninterpretedOption>();
	}
}
