using System;
using System.Diagnostics;
using Google.Protobuf.Collections;

namespace Google.Protobuf.Reflection
{
	// Token: 0x02000052 RID: 82
	public sealed class DescriptorProto : IMessage<DescriptorProto>, IMessage, IEquatable<DescriptorProto>, IDeepCloneable<DescriptorProto>
	{
		// Token: 0x170000EF RID: 239
		// (get) Token: 0x060004A7 RID: 1191 RVA: 0x00012FE9 File Offset: 0x000111E9
		[DebuggerNonUserCode]
		public static MessageParser<DescriptorProto> Parser
		{
			get
			{
				return DescriptorProto._parser;
			}
		}

		// Token: 0x170000F0 RID: 240
		// (get) Token: 0x060004A8 RID: 1192 RVA: 0x00012FF0 File Offset: 0x000111F0
		[DebuggerNonUserCode]
		public static MessageDescriptor Descriptor
		{
			get
			{
				return DescriptorReflection.Descriptor.MessageTypes[2];
			}
		}

		// Token: 0x170000F1 RID: 241
		// (get) Token: 0x060004A9 RID: 1193 RVA: 0x00013002 File Offset: 0x00011202
		[DebuggerNonUserCode]
		MessageDescriptor IMessage.Descriptor
		{
			get
			{
				return DescriptorProto.Descriptor;
			}
		}

		// Token: 0x060004AA RID: 1194 RVA: 0x0001300C File Offset: 0x0001120C
		[DebuggerNonUserCode]
		public DescriptorProto()
		{
		}

		// Token: 0x060004AB RID: 1195 RVA: 0x00013078 File Offset: 0x00011278
		[DebuggerNonUserCode]
		public DescriptorProto(DescriptorProto other)
			: this()
		{
			this.name_ = other.name_;
			this.field_ = other.field_.Clone();
			this.extension_ = other.extension_.Clone();
			this.nestedType_ = other.nestedType_.Clone();
			this.enumType_ = other.enumType_.Clone();
			this.extensionRange_ = other.extensionRange_.Clone();
			this.oneofDecl_ = other.oneofDecl_.Clone();
			this.options_ = ((other.options_ != null) ? other.options_.Clone() : null);
			this.reservedRange_ = other.reservedRange_.Clone();
			this.reservedName_ = other.reservedName_.Clone();
			this._unknownFields = UnknownFieldSet.Clone(other._unknownFields);
		}

		// Token: 0x060004AC RID: 1196 RVA: 0x0001314C File Offset: 0x0001134C
		[DebuggerNonUserCode]
		public DescriptorProto Clone()
		{
			return new DescriptorProto(this);
		}

		// Token: 0x170000F2 RID: 242
		// (get) Token: 0x060004AD RID: 1197 RVA: 0x00013154 File Offset: 0x00011354
		// (set) Token: 0x060004AE RID: 1198 RVA: 0x00013165 File Offset: 0x00011365
		[DebuggerNonUserCode]
		public string Name
		{
			get
			{
				return this.name_ ?? DescriptorProto.NameDefaultValue;
			}
			set
			{
				this.name_ = ProtoPreconditions.CheckNotNull<string>(value, "value");
			}
		}

		// Token: 0x170000F3 RID: 243
		// (get) Token: 0x060004AF RID: 1199 RVA: 0x00013178 File Offset: 0x00011378
		[DebuggerNonUserCode]
		public bool HasName
		{
			get
			{
				return this.name_ != null;
			}
		}

		// Token: 0x060004B0 RID: 1200 RVA: 0x00013183 File Offset: 0x00011383
		[DebuggerNonUserCode]
		public void ClearName()
		{
			this.name_ = null;
		}

		// Token: 0x170000F4 RID: 244
		// (get) Token: 0x060004B1 RID: 1201 RVA: 0x0001318C File Offset: 0x0001138C
		[DebuggerNonUserCode]
		public RepeatedField<FieldDescriptorProto> Field
		{
			get
			{
				return this.field_;
			}
		}

		// Token: 0x170000F5 RID: 245
		// (get) Token: 0x060004B2 RID: 1202 RVA: 0x00013194 File Offset: 0x00011394
		[DebuggerNonUserCode]
		public RepeatedField<FieldDescriptorProto> Extension
		{
			get
			{
				return this.extension_;
			}
		}

		// Token: 0x170000F6 RID: 246
		// (get) Token: 0x060004B3 RID: 1203 RVA: 0x0001319C File Offset: 0x0001139C
		[DebuggerNonUserCode]
		public RepeatedField<DescriptorProto> NestedType
		{
			get
			{
				return this.nestedType_;
			}
		}

		// Token: 0x170000F7 RID: 247
		// (get) Token: 0x060004B4 RID: 1204 RVA: 0x000131A4 File Offset: 0x000113A4
		[DebuggerNonUserCode]
		public RepeatedField<EnumDescriptorProto> EnumType
		{
			get
			{
				return this.enumType_;
			}
		}

		// Token: 0x170000F8 RID: 248
		// (get) Token: 0x060004B5 RID: 1205 RVA: 0x000131AC File Offset: 0x000113AC
		[DebuggerNonUserCode]
		public RepeatedField<DescriptorProto.Types.ExtensionRange> ExtensionRange
		{
			get
			{
				return this.extensionRange_;
			}
		}

		// Token: 0x170000F9 RID: 249
		// (get) Token: 0x060004B6 RID: 1206 RVA: 0x000131B4 File Offset: 0x000113B4
		[DebuggerNonUserCode]
		public RepeatedField<OneofDescriptorProto> OneofDecl
		{
			get
			{
				return this.oneofDecl_;
			}
		}

		// Token: 0x170000FA RID: 250
		// (get) Token: 0x060004B7 RID: 1207 RVA: 0x000131BC File Offset: 0x000113BC
		// (set) Token: 0x060004B8 RID: 1208 RVA: 0x000131C4 File Offset: 0x000113C4
		[DebuggerNonUserCode]
		public MessageOptions Options
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

		// Token: 0x170000FB RID: 251
		// (get) Token: 0x060004B9 RID: 1209 RVA: 0x000131CD File Offset: 0x000113CD
		[DebuggerNonUserCode]
		public RepeatedField<DescriptorProto.Types.ReservedRange> ReservedRange
		{
			get
			{
				return this.reservedRange_;
			}
		}

		// Token: 0x170000FC RID: 252
		// (get) Token: 0x060004BA RID: 1210 RVA: 0x000131D5 File Offset: 0x000113D5
		[DebuggerNonUserCode]
		public RepeatedField<string> ReservedName
		{
			get
			{
				return this.reservedName_;
			}
		}

		// Token: 0x060004BB RID: 1211 RVA: 0x000131DD File Offset: 0x000113DD
		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return this.Equals(other as DescriptorProto);
		}

		// Token: 0x060004BC RID: 1212 RVA: 0x000131EC File Offset: 0x000113EC
		[DebuggerNonUserCode]
		public bool Equals(DescriptorProto other)
		{
			return other != null && (other == this || (!(this.Name != other.Name) && this.field_.Equals(other.field_) && this.extension_.Equals(other.extension_) && this.nestedType_.Equals(other.nestedType_) && this.enumType_.Equals(other.enumType_) && this.extensionRange_.Equals(other.extensionRange_) && this.oneofDecl_.Equals(other.oneofDecl_) && object.Equals(this.Options, other.Options) && this.reservedRange_.Equals(other.reservedRange_) && this.reservedName_.Equals(other.reservedName_) && object.Equals(this._unknownFields, other._unknownFields)));
		}

		// Token: 0x060004BD RID: 1213 RVA: 0x000132E8 File Offset: 0x000114E8
		[DebuggerNonUserCode]
		public override int GetHashCode()
		{
			int num = 1;
			if (this.HasName)
			{
				num ^= this.Name.GetHashCode();
			}
			num ^= this.field_.GetHashCode();
			num ^= this.extension_.GetHashCode();
			num ^= this.nestedType_.GetHashCode();
			num ^= this.enumType_.GetHashCode();
			num ^= this.extensionRange_.GetHashCode();
			num ^= this.oneofDecl_.GetHashCode();
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

		// Token: 0x060004BE RID: 1214 RVA: 0x000133AA File Offset: 0x000115AA
		[DebuggerNonUserCode]
		public override string ToString()
		{
			return JsonFormatter.ToDiagnosticString(this);
		}

		// Token: 0x060004BF RID: 1215 RVA: 0x000133B4 File Offset: 0x000115B4
		[DebuggerNonUserCode]
		public void WriteTo(CodedOutputStream output)
		{
			if (this.HasName)
			{
				output.WriteRawTag(10);
				output.WriteString(this.Name);
			}
			this.field_.WriteTo(output, DescriptorProto._repeated_field_codec);
			this.nestedType_.WriteTo(output, DescriptorProto._repeated_nestedType_codec);
			this.enumType_.WriteTo(output, DescriptorProto._repeated_enumType_codec);
			this.extensionRange_.WriteTo(output, DescriptorProto._repeated_extensionRange_codec);
			this.extension_.WriteTo(output, DescriptorProto._repeated_extension_codec);
			if (this.options_ != null)
			{
				output.WriteRawTag(58);
				output.WriteMessage(this.Options);
			}
			this.oneofDecl_.WriteTo(output, DescriptorProto._repeated_oneofDecl_codec);
			this.reservedRange_.WriteTo(output, DescriptorProto._repeated_reservedRange_codec);
			this.reservedName_.WriteTo(output, DescriptorProto._repeated_reservedName_codec);
			if (this._unknownFields != null)
			{
				this._unknownFields.WriteTo(output);
			}
		}

		// Token: 0x060004C0 RID: 1216 RVA: 0x00013498 File Offset: 0x00011698
		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (this.HasName)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.Name);
			}
			num += this.field_.CalculateSize(DescriptorProto._repeated_field_codec);
			num += this.extension_.CalculateSize(DescriptorProto._repeated_extension_codec);
			num += this.nestedType_.CalculateSize(DescriptorProto._repeated_nestedType_codec);
			num += this.enumType_.CalculateSize(DescriptorProto._repeated_enumType_codec);
			num += this.extensionRange_.CalculateSize(DescriptorProto._repeated_extensionRange_codec);
			num += this.oneofDecl_.CalculateSize(DescriptorProto._repeated_oneofDecl_codec);
			if (this.options_ != null)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(this.Options);
			}
			num += this.reservedRange_.CalculateSize(DescriptorProto._repeated_reservedRange_codec);
			num += this.reservedName_.CalculateSize(DescriptorProto._repeated_reservedName_codec);
			if (this._unknownFields != null)
			{
				num += this._unknownFields.CalculateSize();
			}
			return num;
		}

		// Token: 0x060004C1 RID: 1217 RVA: 0x00013588 File Offset: 0x00011788
		[DebuggerNonUserCode]
		public void MergeFrom(DescriptorProto other)
		{
			if (other == null)
			{
				return;
			}
			if (other.HasName)
			{
				this.Name = other.Name;
			}
			this.field_.Add(other.field_);
			this.extension_.Add(other.extension_);
			this.nestedType_.Add(other.nestedType_);
			this.enumType_.Add(other.enumType_);
			this.extensionRange_.Add(other.extensionRange_);
			this.oneofDecl_.Add(other.oneofDecl_);
			if (other.options_ != null)
			{
				if (this.options_ == null)
				{
					this.Options = new MessageOptions();
				}
				this.Options.MergeFrom(other.Options);
			}
			this.reservedRange_.Add(other.reservedRange_);
			this.reservedName_.Add(other.reservedName_);
			this._unknownFields = UnknownFieldSet.MergeFrom(this._unknownFields, other._unknownFields);
		}

		// Token: 0x060004C2 RID: 1218 RVA: 0x00013678 File Offset: 0x00011878
		[DebuggerNonUserCode]
		public void MergeFrom(CodedInputStream input)
		{
			uint num;
			while ((num = input.ReadTag()) != 0U)
			{
				if (num <= 42U)
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
							this.field_.AddEntriesFrom(input, DescriptorProto._repeated_field_codec);
							continue;
						}
					}
					else
					{
						if (num == 26U)
						{
							this.nestedType_.AddEntriesFrom(input, DescriptorProto._repeated_nestedType_codec);
							continue;
						}
						if (num == 34U)
						{
							this.enumType_.AddEntriesFrom(input, DescriptorProto._repeated_enumType_codec);
							continue;
						}
						if (num == 42U)
						{
							this.extensionRange_.AddEntriesFrom(input, DescriptorProto._repeated_extensionRange_codec);
							continue;
						}
					}
				}
				else if (num <= 58U)
				{
					if (num == 50U)
					{
						this.extension_.AddEntriesFrom(input, DescriptorProto._repeated_extension_codec);
						continue;
					}
					if (num == 58U)
					{
						if (this.options_ == null)
						{
							this.Options = new MessageOptions();
						}
						input.ReadMessage(this.Options);
						continue;
					}
				}
				else
				{
					if (num == 66U)
					{
						this.oneofDecl_.AddEntriesFrom(input, DescriptorProto._repeated_oneofDecl_codec);
						continue;
					}
					if (num == 74U)
					{
						this.reservedRange_.AddEntriesFrom(input, DescriptorProto._repeated_reservedRange_codec);
						continue;
					}
					if (num == 82U)
					{
						this.reservedName_.AddEntriesFrom(input, DescriptorProto._repeated_reservedName_codec);
						continue;
					}
				}
				this._unknownFields = UnknownFieldSet.MergeFieldFrom(this._unknownFields, input);
			}
		}

		// Token: 0x0400017C RID: 380
		private static readonly MessageParser<DescriptorProto> _parser = new MessageParser<DescriptorProto>(() => new DescriptorProto());

		// Token: 0x0400017D RID: 381
		private UnknownFieldSet _unknownFields;

		// Token: 0x0400017E RID: 382
		public const int NameFieldNumber = 1;

		// Token: 0x0400017F RID: 383
		private static readonly string NameDefaultValue = "";

		// Token: 0x04000180 RID: 384
		private string name_;

		// Token: 0x04000181 RID: 385
		public const int FieldFieldNumber = 2;

		// Token: 0x04000182 RID: 386
		private static readonly FieldCodec<FieldDescriptorProto> _repeated_field_codec = FieldCodec.ForMessage<FieldDescriptorProto>(18U, FieldDescriptorProto.Parser);

		// Token: 0x04000183 RID: 387
		private readonly RepeatedField<FieldDescriptorProto> field_ = new RepeatedField<FieldDescriptorProto>();

		// Token: 0x04000184 RID: 388
		public const int ExtensionFieldNumber = 6;

		// Token: 0x04000185 RID: 389
		private static readonly FieldCodec<FieldDescriptorProto> _repeated_extension_codec = FieldCodec.ForMessage<FieldDescriptorProto>(50U, FieldDescriptorProto.Parser);

		// Token: 0x04000186 RID: 390
		private readonly RepeatedField<FieldDescriptorProto> extension_ = new RepeatedField<FieldDescriptorProto>();

		// Token: 0x04000187 RID: 391
		public const int NestedTypeFieldNumber = 3;

		// Token: 0x04000188 RID: 392
		private static readonly FieldCodec<DescriptorProto> _repeated_nestedType_codec = FieldCodec.ForMessage<DescriptorProto>(26U, DescriptorProto.Parser);

		// Token: 0x04000189 RID: 393
		private readonly RepeatedField<DescriptorProto> nestedType_ = new RepeatedField<DescriptorProto>();

		// Token: 0x0400018A RID: 394
		public const int EnumTypeFieldNumber = 4;

		// Token: 0x0400018B RID: 395
		private static readonly FieldCodec<EnumDescriptorProto> _repeated_enumType_codec = FieldCodec.ForMessage<EnumDescriptorProto>(34U, EnumDescriptorProto.Parser);

		// Token: 0x0400018C RID: 396
		private readonly RepeatedField<EnumDescriptorProto> enumType_ = new RepeatedField<EnumDescriptorProto>();

		// Token: 0x0400018D RID: 397
		public const int ExtensionRangeFieldNumber = 5;

		// Token: 0x0400018E RID: 398
		private static readonly FieldCodec<DescriptorProto.Types.ExtensionRange> _repeated_extensionRange_codec = FieldCodec.ForMessage<DescriptorProto.Types.ExtensionRange>(42U, DescriptorProto.Types.ExtensionRange.Parser);

		// Token: 0x0400018F RID: 399
		private readonly RepeatedField<DescriptorProto.Types.ExtensionRange> extensionRange_ = new RepeatedField<DescriptorProto.Types.ExtensionRange>();

		// Token: 0x04000190 RID: 400
		public const int OneofDeclFieldNumber = 8;

		// Token: 0x04000191 RID: 401
		private static readonly FieldCodec<OneofDescriptorProto> _repeated_oneofDecl_codec = FieldCodec.ForMessage<OneofDescriptorProto>(66U, OneofDescriptorProto.Parser);

		// Token: 0x04000192 RID: 402
		private readonly RepeatedField<OneofDescriptorProto> oneofDecl_ = new RepeatedField<OneofDescriptorProto>();

		// Token: 0x04000193 RID: 403
		public const int OptionsFieldNumber = 7;

		// Token: 0x04000194 RID: 404
		private MessageOptions options_;

		// Token: 0x04000195 RID: 405
		public const int ReservedRangeFieldNumber = 9;

		// Token: 0x04000196 RID: 406
		private static readonly FieldCodec<DescriptorProto.Types.ReservedRange> _repeated_reservedRange_codec = FieldCodec.ForMessage<DescriptorProto.Types.ReservedRange>(74U, DescriptorProto.Types.ReservedRange.Parser);

		// Token: 0x04000197 RID: 407
		private readonly RepeatedField<DescriptorProto.Types.ReservedRange> reservedRange_ = new RepeatedField<DescriptorProto.Types.ReservedRange>();

		// Token: 0x04000198 RID: 408
		public const int ReservedNameFieldNumber = 10;

		// Token: 0x04000199 RID: 409
		private static readonly FieldCodec<string> _repeated_reservedName_codec = FieldCodec.ForString(82U);

		// Token: 0x0400019A RID: 410
		private readonly RepeatedField<string> reservedName_ = new RepeatedField<string>();

		// Token: 0x020000CB RID: 203
		[DebuggerNonUserCode]
		public static class Types
		{
			// Token: 0x02000113 RID: 275
			public sealed class ExtensionRange : IMessage<DescriptorProto.Types.ExtensionRange>, IMessage, IEquatable<DescriptorProto.Types.ExtensionRange>, IDeepCloneable<DescriptorProto.Types.ExtensionRange>
			{
				// Token: 0x17000279 RID: 633
				// (get) Token: 0x06000A85 RID: 2693 RVA: 0x000214FE File Offset: 0x0001F6FE
				[DebuggerNonUserCode]
				public static MessageParser<DescriptorProto.Types.ExtensionRange> Parser
				{
					get
					{
						return DescriptorProto.Types.ExtensionRange._parser;
					}
				}

				// Token: 0x1700027A RID: 634
				// (get) Token: 0x06000A86 RID: 2694 RVA: 0x00021505 File Offset: 0x0001F705
				[DebuggerNonUserCode]
				public static MessageDescriptor Descriptor
				{
					get
					{
						return DescriptorProto.Descriptor.NestedTypes[0];
					}
				}

				// Token: 0x1700027B RID: 635
				// (get) Token: 0x06000A87 RID: 2695 RVA: 0x00021517 File Offset: 0x0001F717
				[DebuggerNonUserCode]
				MessageDescriptor IMessage.Descriptor
				{
					get
					{
						return DescriptorProto.Types.ExtensionRange.Descriptor;
					}
				}

				// Token: 0x06000A88 RID: 2696 RVA: 0x0002151E File Offset: 0x0001F71E
				[DebuggerNonUserCode]
				public ExtensionRange()
				{
				}

				// Token: 0x06000A89 RID: 2697 RVA: 0x00021528 File Offset: 0x0001F728
				[DebuggerNonUserCode]
				public ExtensionRange(DescriptorProto.Types.ExtensionRange other)
					: this()
				{
					this._hasBits0 = other._hasBits0;
					this.start_ = other.start_;
					this.end_ = other.end_;
					this.options_ = ((other.options_ != null) ? other.options_.Clone() : null);
					this._unknownFields = UnknownFieldSet.Clone(other._unknownFields);
				}

				// Token: 0x06000A8A RID: 2698 RVA: 0x0002158C File Offset: 0x0001F78C
				[DebuggerNonUserCode]
				public DescriptorProto.Types.ExtensionRange Clone()
				{
					return new DescriptorProto.Types.ExtensionRange(this);
				}

				// Token: 0x1700027C RID: 636
				// (get) Token: 0x06000A8B RID: 2699 RVA: 0x00021594 File Offset: 0x0001F794
				// (set) Token: 0x06000A8C RID: 2700 RVA: 0x000215AC File Offset: 0x0001F7AC
				[DebuggerNonUserCode]
				public int Start
				{
					get
					{
						if ((this._hasBits0 & 1) != 0)
						{
							return this.start_;
						}
						return DescriptorProto.Types.ExtensionRange.StartDefaultValue;
					}
					set
					{
						this._hasBits0 |= 1;
						this.start_ = value;
					}
				}

				// Token: 0x1700027D RID: 637
				// (get) Token: 0x06000A8D RID: 2701 RVA: 0x000215C3 File Offset: 0x0001F7C3
				[DebuggerNonUserCode]
				public bool HasStart
				{
					get
					{
						return (this._hasBits0 & 1) != 0;
					}
				}

				// Token: 0x06000A8E RID: 2702 RVA: 0x000215D0 File Offset: 0x0001F7D0
				[DebuggerNonUserCode]
				public void ClearStart()
				{
					this._hasBits0 &= -2;
				}

				// Token: 0x1700027E RID: 638
				// (get) Token: 0x06000A8F RID: 2703 RVA: 0x000215E1 File Offset: 0x0001F7E1
				// (set) Token: 0x06000A90 RID: 2704 RVA: 0x000215F9 File Offset: 0x0001F7F9
				[DebuggerNonUserCode]
				public int End
				{
					get
					{
						if ((this._hasBits0 & 2) != 0)
						{
							return this.end_;
						}
						return DescriptorProto.Types.ExtensionRange.EndDefaultValue;
					}
					set
					{
						this._hasBits0 |= 2;
						this.end_ = value;
					}
				}

				// Token: 0x1700027F RID: 639
				// (get) Token: 0x06000A91 RID: 2705 RVA: 0x00021610 File Offset: 0x0001F810
				[DebuggerNonUserCode]
				public bool HasEnd
				{
					get
					{
						return (this._hasBits0 & 2) != 0;
					}
				}

				// Token: 0x06000A92 RID: 2706 RVA: 0x0002161D File Offset: 0x0001F81D
				[DebuggerNonUserCode]
				public void ClearEnd()
				{
					this._hasBits0 &= -3;
				}

				// Token: 0x17000280 RID: 640
				// (get) Token: 0x06000A93 RID: 2707 RVA: 0x0002162E File Offset: 0x0001F82E
				// (set) Token: 0x06000A94 RID: 2708 RVA: 0x00021636 File Offset: 0x0001F836
				[DebuggerNonUserCode]
				public ExtensionRangeOptions Options
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

				// Token: 0x06000A95 RID: 2709 RVA: 0x0002163F File Offset: 0x0001F83F
				[DebuggerNonUserCode]
				public override bool Equals(object other)
				{
					return this.Equals(other as DescriptorProto.Types.ExtensionRange);
				}

				// Token: 0x06000A96 RID: 2710 RVA: 0x00021650 File Offset: 0x0001F850
				[DebuggerNonUserCode]
				public bool Equals(DescriptorProto.Types.ExtensionRange other)
				{
					return other != null && (other == this || (this.Start == other.Start && this.End == other.End && object.Equals(this.Options, other.Options) && object.Equals(this._unknownFields, other._unknownFields)));
				}

				// Token: 0x06000A97 RID: 2711 RVA: 0x000216B0 File Offset: 0x0001F8B0
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
					if (this.options_ != null)
					{
						num ^= this.Options.GetHashCode();
					}
					if (this._unknownFields != null)
					{
						num ^= this._unknownFields.GetHashCode();
					}
					return num;
				}

				// Token: 0x06000A98 RID: 2712 RVA: 0x0002171E File Offset: 0x0001F91E
				[DebuggerNonUserCode]
				public override string ToString()
				{
					return JsonFormatter.ToDiagnosticString(this);
				}

				// Token: 0x06000A99 RID: 2713 RVA: 0x00021728 File Offset: 0x0001F928
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
					if (this.options_ != null)
					{
						output.WriteRawTag(26);
						output.WriteMessage(this.Options);
					}
					if (this._unknownFields != null)
					{
						this._unknownFields.WriteTo(output);
					}
				}

				// Token: 0x06000A9A RID: 2714 RVA: 0x0002179C File Offset: 0x0001F99C
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
					if (this.options_ != null)
					{
						num += 1 + CodedOutputStream.ComputeMessageSize(this.Options);
					}
					if (this._unknownFields != null)
					{
						num += this._unknownFields.CalculateSize();
					}
					return num;
				}

				// Token: 0x06000A9B RID: 2715 RVA: 0x0002180C File Offset: 0x0001FA0C
				[DebuggerNonUserCode]
				public void MergeFrom(DescriptorProto.Types.ExtensionRange other)
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
					if (other.options_ != null)
					{
						if (this.options_ == null)
						{
							this.Options = new ExtensionRangeOptions();
						}
						this.Options.MergeFrom(other.Options);
					}
					this._unknownFields = UnknownFieldSet.MergeFrom(this._unknownFields, other._unknownFields);
				}

				// Token: 0x06000A9C RID: 2716 RVA: 0x00021888 File Offset: 0x0001FA88
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
								if (num != 26U)
								{
									this._unknownFields = UnknownFieldSet.MergeFieldFrom(this._unknownFields, input);
								}
								else
								{
									if (this.options_ == null)
									{
										this.Options = new ExtensionRangeOptions();
									}
									input.ReadMessage(this.Options);
								}
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

				// Token: 0x0400046F RID: 1135
				private static readonly MessageParser<DescriptorProto.Types.ExtensionRange> _parser = new MessageParser<DescriptorProto.Types.ExtensionRange>(() => new DescriptorProto.Types.ExtensionRange());

				// Token: 0x04000470 RID: 1136
				private UnknownFieldSet _unknownFields;

				// Token: 0x04000471 RID: 1137
				private int _hasBits0;

				// Token: 0x04000472 RID: 1138
				public const int StartFieldNumber = 1;

				// Token: 0x04000473 RID: 1139
				private static readonly int StartDefaultValue = 0;

				// Token: 0x04000474 RID: 1140
				private int start_;

				// Token: 0x04000475 RID: 1141
				public const int EndFieldNumber = 2;

				// Token: 0x04000476 RID: 1142
				private static readonly int EndDefaultValue = 0;

				// Token: 0x04000477 RID: 1143
				private int end_;

				// Token: 0x04000478 RID: 1144
				public const int OptionsFieldNumber = 3;

				// Token: 0x04000479 RID: 1145
				private ExtensionRangeOptions options_;
			}

			// Token: 0x02000114 RID: 276
			public sealed class ReservedRange : IMessage<DescriptorProto.Types.ReservedRange>, IMessage, IEquatable<DescriptorProto.Types.ReservedRange>, IDeepCloneable<DescriptorProto.Types.ReservedRange>
			{
				// Token: 0x17000281 RID: 641
				// (get) Token: 0x06000A9E RID: 2718 RVA: 0x00021926 File Offset: 0x0001FB26
				[DebuggerNonUserCode]
				public static MessageParser<DescriptorProto.Types.ReservedRange> Parser
				{
					get
					{
						return DescriptorProto.Types.ReservedRange._parser;
					}
				}

				// Token: 0x17000282 RID: 642
				// (get) Token: 0x06000A9F RID: 2719 RVA: 0x0002192D File Offset: 0x0001FB2D
				[DebuggerNonUserCode]
				public static MessageDescriptor Descriptor
				{
					get
					{
						return DescriptorProto.Descriptor.NestedTypes[1];
					}
				}

				// Token: 0x17000283 RID: 643
				// (get) Token: 0x06000AA0 RID: 2720 RVA: 0x0002193F File Offset: 0x0001FB3F
				[DebuggerNonUserCode]
				MessageDescriptor IMessage.Descriptor
				{
					get
					{
						return DescriptorProto.Types.ReservedRange.Descriptor;
					}
				}

				// Token: 0x06000AA1 RID: 2721 RVA: 0x00021946 File Offset: 0x0001FB46
				[DebuggerNonUserCode]
				public ReservedRange()
				{
				}

				// Token: 0x06000AA2 RID: 2722 RVA: 0x0002194E File Offset: 0x0001FB4E
				[DebuggerNonUserCode]
				public ReservedRange(DescriptorProto.Types.ReservedRange other)
					: this()
				{
					this._hasBits0 = other._hasBits0;
					this.start_ = other.start_;
					this.end_ = other.end_;
					this._unknownFields = UnknownFieldSet.Clone(other._unknownFields);
				}

				// Token: 0x06000AA3 RID: 2723 RVA: 0x0002198B File Offset: 0x0001FB8B
				[DebuggerNonUserCode]
				public DescriptorProto.Types.ReservedRange Clone()
				{
					return new DescriptorProto.Types.ReservedRange(this);
				}

				// Token: 0x17000284 RID: 644
				// (get) Token: 0x06000AA4 RID: 2724 RVA: 0x00021993 File Offset: 0x0001FB93
				// (set) Token: 0x06000AA5 RID: 2725 RVA: 0x000219AB File Offset: 0x0001FBAB
				[DebuggerNonUserCode]
				public int Start
				{
					get
					{
						if ((this._hasBits0 & 1) != 0)
						{
							return this.start_;
						}
						return DescriptorProto.Types.ReservedRange.StartDefaultValue;
					}
					set
					{
						this._hasBits0 |= 1;
						this.start_ = value;
					}
				}

				// Token: 0x17000285 RID: 645
				// (get) Token: 0x06000AA6 RID: 2726 RVA: 0x000219C2 File Offset: 0x0001FBC2
				[DebuggerNonUserCode]
				public bool HasStart
				{
					get
					{
						return (this._hasBits0 & 1) != 0;
					}
				}

				// Token: 0x06000AA7 RID: 2727 RVA: 0x000219CF File Offset: 0x0001FBCF
				[DebuggerNonUserCode]
				public void ClearStart()
				{
					this._hasBits0 &= -2;
				}

				// Token: 0x17000286 RID: 646
				// (get) Token: 0x06000AA8 RID: 2728 RVA: 0x000219E0 File Offset: 0x0001FBE0
				// (set) Token: 0x06000AA9 RID: 2729 RVA: 0x000219F8 File Offset: 0x0001FBF8
				[DebuggerNonUserCode]
				public int End
				{
					get
					{
						if ((this._hasBits0 & 2) != 0)
						{
							return this.end_;
						}
						return DescriptorProto.Types.ReservedRange.EndDefaultValue;
					}
					set
					{
						this._hasBits0 |= 2;
						this.end_ = value;
					}
				}

				// Token: 0x17000287 RID: 647
				// (get) Token: 0x06000AAA RID: 2730 RVA: 0x00021A0F File Offset: 0x0001FC0F
				[DebuggerNonUserCode]
				public bool HasEnd
				{
					get
					{
						return (this._hasBits0 & 2) != 0;
					}
				}

				// Token: 0x06000AAB RID: 2731 RVA: 0x00021A1C File Offset: 0x0001FC1C
				[DebuggerNonUserCode]
				public void ClearEnd()
				{
					this._hasBits0 &= -3;
				}

				// Token: 0x06000AAC RID: 2732 RVA: 0x00021A2D File Offset: 0x0001FC2D
				[DebuggerNonUserCode]
				public override bool Equals(object other)
				{
					return this.Equals(other as DescriptorProto.Types.ReservedRange);
				}

				// Token: 0x06000AAD RID: 2733 RVA: 0x00021A3B File Offset: 0x0001FC3B
				[DebuggerNonUserCode]
				public bool Equals(DescriptorProto.Types.ReservedRange other)
				{
					return other != null && (other == this || (this.Start == other.Start && this.End == other.End && object.Equals(this._unknownFields, other._unknownFields)));
				}

				// Token: 0x06000AAE RID: 2734 RVA: 0x00021A7C File Offset: 0x0001FC7C
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

				// Token: 0x06000AAF RID: 2735 RVA: 0x00021AD4 File Offset: 0x0001FCD4
				[DebuggerNonUserCode]
				public override string ToString()
				{
					return JsonFormatter.ToDiagnosticString(this);
				}

				// Token: 0x06000AB0 RID: 2736 RVA: 0x00021ADC File Offset: 0x0001FCDC
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

				// Token: 0x06000AB1 RID: 2737 RVA: 0x00021B34 File Offset: 0x0001FD34
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

				// Token: 0x06000AB2 RID: 2738 RVA: 0x00021B8C File Offset: 0x0001FD8C
				[DebuggerNonUserCode]
				public void MergeFrom(DescriptorProto.Types.ReservedRange other)
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

				// Token: 0x06000AB3 RID: 2739 RVA: 0x00021BDC File Offset: 0x0001FDDC
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

				// Token: 0x0400047A RID: 1146
				private static readonly MessageParser<DescriptorProto.Types.ReservedRange> _parser = new MessageParser<DescriptorProto.Types.ReservedRange>(() => new DescriptorProto.Types.ReservedRange());

				// Token: 0x0400047B RID: 1147
				private UnknownFieldSet _unknownFields;

				// Token: 0x0400047C RID: 1148
				private int _hasBits0;

				// Token: 0x0400047D RID: 1149
				public const int StartFieldNumber = 1;

				// Token: 0x0400047E RID: 1150
				private static readonly int StartDefaultValue = 0;

				// Token: 0x0400047F RID: 1151
				private int start_;

				// Token: 0x04000480 RID: 1152
				public const int EndFieldNumber = 2;

				// Token: 0x04000481 RID: 1153
				private static readonly int EndDefaultValue = 0;

				// Token: 0x04000482 RID: 1154
				private int end_;
			}
		}
	}
}
