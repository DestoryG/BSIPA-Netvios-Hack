using System;
using System.Diagnostics;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

namespace Google.Protobuf.WellKnownTypes
{
	// Token: 0x02000040 RID: 64
	public sealed class Field : IMessage<Field>, IMessage, IEquatable<Field>, IDeepCloneable<Field>
	{
		// Token: 0x17000093 RID: 147
		// (get) Token: 0x06000361 RID: 865 RVA: 0x0000EB01 File Offset: 0x0000CD01
		[DebuggerNonUserCode]
		public static MessageParser<Field> Parser
		{
			get
			{
				return Field._parser;
			}
		}

		// Token: 0x17000094 RID: 148
		// (get) Token: 0x06000362 RID: 866 RVA: 0x0000EB08 File Offset: 0x0000CD08
		[DebuggerNonUserCode]
		public static MessageDescriptor Descriptor
		{
			get
			{
				return TypeReflection.Descriptor.MessageTypes[1];
			}
		}

		// Token: 0x17000095 RID: 149
		// (get) Token: 0x06000363 RID: 867 RVA: 0x0000EB1A File Offset: 0x0000CD1A
		[DebuggerNonUserCode]
		MessageDescriptor IMessage.Descriptor
		{
			get
			{
				return Field.Descriptor;
			}
		}

		// Token: 0x06000364 RID: 868 RVA: 0x0000EB21 File Offset: 0x0000CD21
		[DebuggerNonUserCode]
		public Field()
		{
		}

		// Token: 0x06000365 RID: 869 RVA: 0x0000EB60 File Offset: 0x0000CD60
		[DebuggerNonUserCode]
		public Field(Field other)
			: this()
		{
			this.kind_ = other.kind_;
			this.cardinality_ = other.cardinality_;
			this.number_ = other.number_;
			this.name_ = other.name_;
			this.typeUrl_ = other.typeUrl_;
			this.oneofIndex_ = other.oneofIndex_;
			this.packed_ = other.packed_;
			this.options_ = other.options_.Clone();
			this.jsonName_ = other.jsonName_;
			this.defaultValue_ = other.defaultValue_;
			this._unknownFields = UnknownFieldSet.Clone(other._unknownFields);
		}

		// Token: 0x06000366 RID: 870 RVA: 0x0000EC01 File Offset: 0x0000CE01
		[DebuggerNonUserCode]
		public Field Clone()
		{
			return new Field(this);
		}

		// Token: 0x17000096 RID: 150
		// (get) Token: 0x06000367 RID: 871 RVA: 0x0000EC09 File Offset: 0x0000CE09
		// (set) Token: 0x06000368 RID: 872 RVA: 0x0000EC11 File Offset: 0x0000CE11
		[DebuggerNonUserCode]
		public Field.Types.Kind Kind
		{
			get
			{
				return this.kind_;
			}
			set
			{
				this.kind_ = value;
			}
		}

		// Token: 0x17000097 RID: 151
		// (get) Token: 0x06000369 RID: 873 RVA: 0x0000EC1A File Offset: 0x0000CE1A
		// (set) Token: 0x0600036A RID: 874 RVA: 0x0000EC22 File Offset: 0x0000CE22
		[DebuggerNonUserCode]
		public Field.Types.Cardinality Cardinality
		{
			get
			{
				return this.cardinality_;
			}
			set
			{
				this.cardinality_ = value;
			}
		}

		// Token: 0x17000098 RID: 152
		// (get) Token: 0x0600036B RID: 875 RVA: 0x0000EC2B File Offset: 0x0000CE2B
		// (set) Token: 0x0600036C RID: 876 RVA: 0x0000EC33 File Offset: 0x0000CE33
		[DebuggerNonUserCode]
		public int Number
		{
			get
			{
				return this.number_;
			}
			set
			{
				this.number_ = value;
			}
		}

		// Token: 0x17000099 RID: 153
		// (get) Token: 0x0600036D RID: 877 RVA: 0x0000EC3C File Offset: 0x0000CE3C
		// (set) Token: 0x0600036E RID: 878 RVA: 0x0000EC44 File Offset: 0x0000CE44
		[DebuggerNonUserCode]
		public string Name
		{
			get
			{
				return this.name_;
			}
			set
			{
				this.name_ = ProtoPreconditions.CheckNotNull<string>(value, "value");
			}
		}

		// Token: 0x1700009A RID: 154
		// (get) Token: 0x0600036F RID: 879 RVA: 0x0000EC57 File Offset: 0x0000CE57
		// (set) Token: 0x06000370 RID: 880 RVA: 0x0000EC5F File Offset: 0x0000CE5F
		[DebuggerNonUserCode]
		public string TypeUrl
		{
			get
			{
				return this.typeUrl_;
			}
			set
			{
				this.typeUrl_ = ProtoPreconditions.CheckNotNull<string>(value, "value");
			}
		}

		// Token: 0x1700009B RID: 155
		// (get) Token: 0x06000371 RID: 881 RVA: 0x0000EC72 File Offset: 0x0000CE72
		// (set) Token: 0x06000372 RID: 882 RVA: 0x0000EC7A File Offset: 0x0000CE7A
		[DebuggerNonUserCode]
		public int OneofIndex
		{
			get
			{
				return this.oneofIndex_;
			}
			set
			{
				this.oneofIndex_ = value;
			}
		}

		// Token: 0x1700009C RID: 156
		// (get) Token: 0x06000373 RID: 883 RVA: 0x0000EC83 File Offset: 0x0000CE83
		// (set) Token: 0x06000374 RID: 884 RVA: 0x0000EC8B File Offset: 0x0000CE8B
		[DebuggerNonUserCode]
		public bool Packed
		{
			get
			{
				return this.packed_;
			}
			set
			{
				this.packed_ = value;
			}
		}

		// Token: 0x1700009D RID: 157
		// (get) Token: 0x06000375 RID: 885 RVA: 0x0000EC94 File Offset: 0x0000CE94
		[DebuggerNonUserCode]
		public RepeatedField<Option> Options
		{
			get
			{
				return this.options_;
			}
		}

		// Token: 0x1700009E RID: 158
		// (get) Token: 0x06000376 RID: 886 RVA: 0x0000EC9C File Offset: 0x0000CE9C
		// (set) Token: 0x06000377 RID: 887 RVA: 0x0000ECA4 File Offset: 0x0000CEA4
		[DebuggerNonUserCode]
		public string JsonName
		{
			get
			{
				return this.jsonName_;
			}
			set
			{
				this.jsonName_ = ProtoPreconditions.CheckNotNull<string>(value, "value");
			}
		}

		// Token: 0x1700009F RID: 159
		// (get) Token: 0x06000378 RID: 888 RVA: 0x0000ECB7 File Offset: 0x0000CEB7
		// (set) Token: 0x06000379 RID: 889 RVA: 0x0000ECBF File Offset: 0x0000CEBF
		[DebuggerNonUserCode]
		public string DefaultValue
		{
			get
			{
				return this.defaultValue_;
			}
			set
			{
				this.defaultValue_ = ProtoPreconditions.CheckNotNull<string>(value, "value");
			}
		}

		// Token: 0x0600037A RID: 890 RVA: 0x0000ECD2 File Offset: 0x0000CED2
		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return this.Equals(other as Field);
		}

		// Token: 0x0600037B RID: 891 RVA: 0x0000ECE0 File Offset: 0x0000CEE0
		[DebuggerNonUserCode]
		public bool Equals(Field other)
		{
			return other != null && (other == this || (this.Kind == other.Kind && this.Cardinality == other.Cardinality && this.Number == other.Number && !(this.Name != other.Name) && !(this.TypeUrl != other.TypeUrl) && this.OneofIndex == other.OneofIndex && this.Packed == other.Packed && this.options_.Equals(other.options_) && !(this.JsonName != other.JsonName) && !(this.DefaultValue != other.DefaultValue) && object.Equals(this._unknownFields, other._unknownFields)));
		}

		// Token: 0x0600037C RID: 892 RVA: 0x0000EDC4 File Offset: 0x0000CFC4
		[DebuggerNonUserCode]
		public override int GetHashCode()
		{
			int num = 1;
			if (this.Kind != Field.Types.Kind.TypeUnknown)
			{
				num ^= this.Kind.GetHashCode();
			}
			if (this.Cardinality != Field.Types.Cardinality.Unknown)
			{
				num ^= this.Cardinality.GetHashCode();
			}
			if (this.Number != 0)
			{
				num ^= this.Number.GetHashCode();
			}
			if (this.Name.Length != 0)
			{
				num ^= this.Name.GetHashCode();
			}
			if (this.TypeUrl.Length != 0)
			{
				num ^= this.TypeUrl.GetHashCode();
			}
			if (this.OneofIndex != 0)
			{
				num ^= this.OneofIndex.GetHashCode();
			}
			if (this.Packed)
			{
				num ^= this.Packed.GetHashCode();
			}
			num ^= this.options_.GetHashCode();
			if (this.JsonName.Length != 0)
			{
				num ^= this.JsonName.GetHashCode();
			}
			if (this.DefaultValue.Length != 0)
			{
				num ^= this.DefaultValue.GetHashCode();
			}
			if (this._unknownFields != null)
			{
				num ^= this._unknownFields.GetHashCode();
			}
			return num;
		}

		// Token: 0x0600037D RID: 893 RVA: 0x0000EEEE File Offset: 0x0000D0EE
		[DebuggerNonUserCode]
		public override string ToString()
		{
			return JsonFormatter.ToDiagnosticString(this);
		}

		// Token: 0x0600037E RID: 894 RVA: 0x0000EEF8 File Offset: 0x0000D0F8
		[DebuggerNonUserCode]
		public void WriteTo(CodedOutputStream output)
		{
			if (this.Kind != Field.Types.Kind.TypeUnknown)
			{
				output.WriteRawTag(8);
				output.WriteEnum((int)this.Kind);
			}
			if (this.Cardinality != Field.Types.Cardinality.Unknown)
			{
				output.WriteRawTag(16);
				output.WriteEnum((int)this.Cardinality);
			}
			if (this.Number != 0)
			{
				output.WriteRawTag(24);
				output.WriteInt32(this.Number);
			}
			if (this.Name.Length != 0)
			{
				output.WriteRawTag(34);
				output.WriteString(this.Name);
			}
			if (this.TypeUrl.Length != 0)
			{
				output.WriteRawTag(50);
				output.WriteString(this.TypeUrl);
			}
			if (this.OneofIndex != 0)
			{
				output.WriteRawTag(56);
				output.WriteInt32(this.OneofIndex);
			}
			if (this.Packed)
			{
				output.WriteRawTag(64);
				output.WriteBool(this.Packed);
			}
			this.options_.WriteTo(output, Field._repeated_options_codec);
			if (this.JsonName.Length != 0)
			{
				output.WriteRawTag(82);
				output.WriteString(this.JsonName);
			}
			if (this.DefaultValue.Length != 0)
			{
				output.WriteRawTag(90);
				output.WriteString(this.DefaultValue);
			}
			if (this._unknownFields != null)
			{
				this._unknownFields.WriteTo(output);
			}
		}

		// Token: 0x0600037F RID: 895 RVA: 0x0000F03C File Offset: 0x0000D23C
		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (this.Kind != Field.Types.Kind.TypeUnknown)
			{
				num += 1 + CodedOutputStream.ComputeEnumSize((int)this.Kind);
			}
			if (this.Cardinality != Field.Types.Cardinality.Unknown)
			{
				num += 1 + CodedOutputStream.ComputeEnumSize((int)this.Cardinality);
			}
			if (this.Number != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.Number);
			}
			if (this.Name.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.Name);
			}
			if (this.TypeUrl.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.TypeUrl);
			}
			if (this.OneofIndex != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.OneofIndex);
			}
			if (this.Packed)
			{
				num += 2;
			}
			num += this.options_.CalculateSize(Field._repeated_options_codec);
			if (this.JsonName.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.JsonName);
			}
			if (this.DefaultValue.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.DefaultValue);
			}
			if (this._unknownFields != null)
			{
				num += this._unknownFields.CalculateSize();
			}
			return num;
		}

		// Token: 0x06000380 RID: 896 RVA: 0x0000F158 File Offset: 0x0000D358
		[DebuggerNonUserCode]
		public void MergeFrom(Field other)
		{
			if (other == null)
			{
				return;
			}
			if (other.Kind != Field.Types.Kind.TypeUnknown)
			{
				this.Kind = other.Kind;
			}
			if (other.Cardinality != Field.Types.Cardinality.Unknown)
			{
				this.Cardinality = other.Cardinality;
			}
			if (other.Number != 0)
			{
				this.Number = other.Number;
			}
			if (other.Name.Length != 0)
			{
				this.Name = other.Name;
			}
			if (other.TypeUrl.Length != 0)
			{
				this.TypeUrl = other.TypeUrl;
			}
			if (other.OneofIndex != 0)
			{
				this.OneofIndex = other.OneofIndex;
			}
			if (other.Packed)
			{
				this.Packed = other.Packed;
			}
			this.options_.Add(other.options_);
			if (other.JsonName.Length != 0)
			{
				this.JsonName = other.JsonName;
			}
			if (other.DefaultValue.Length != 0)
			{
				this.DefaultValue = other.DefaultValue;
			}
			this._unknownFields = UnknownFieldSet.MergeFrom(this._unknownFields, other._unknownFields);
		}

		// Token: 0x06000381 RID: 897 RVA: 0x0000F25C File Offset: 0x0000D45C
		[DebuggerNonUserCode]
		public void MergeFrom(CodedInputStream input)
		{
			uint num;
			while ((num = input.ReadTag()) != 0U)
			{
				if (num <= 50U)
				{
					if (num <= 16U)
					{
						if (num == 8U)
						{
							this.Kind = (Field.Types.Kind)input.ReadEnum();
							continue;
						}
						if (num == 16U)
						{
							this.Cardinality = (Field.Types.Cardinality)input.ReadEnum();
							continue;
						}
					}
					else
					{
						if (num == 24U)
						{
							this.Number = input.ReadInt32();
							continue;
						}
						if (num == 34U)
						{
							this.Name = input.ReadString();
							continue;
						}
						if (num == 50U)
						{
							this.TypeUrl = input.ReadString();
							continue;
						}
					}
				}
				else if (num <= 64U)
				{
					if (num == 56U)
					{
						this.OneofIndex = input.ReadInt32();
						continue;
					}
					if (num == 64U)
					{
						this.Packed = input.ReadBool();
						continue;
					}
				}
				else
				{
					if (num == 74U)
					{
						this.options_.AddEntriesFrom(input, Field._repeated_options_codec);
						continue;
					}
					if (num == 82U)
					{
						this.JsonName = input.ReadString();
						continue;
					}
					if (num == 90U)
					{
						this.DefaultValue = input.ReadString();
						continue;
					}
				}
				this._unknownFields = UnknownFieldSet.MergeFieldFrom(this._unknownFields, input);
			}
		}

		// Token: 0x040000F6 RID: 246
		private static readonly MessageParser<Field> _parser = new MessageParser<Field>(() => new Field());

		// Token: 0x040000F7 RID: 247
		private UnknownFieldSet _unknownFields;

		// Token: 0x040000F8 RID: 248
		public const int KindFieldNumber = 1;

		// Token: 0x040000F9 RID: 249
		private Field.Types.Kind kind_;

		// Token: 0x040000FA RID: 250
		public const int CardinalityFieldNumber = 2;

		// Token: 0x040000FB RID: 251
		private Field.Types.Cardinality cardinality_;

		// Token: 0x040000FC RID: 252
		public const int NumberFieldNumber = 3;

		// Token: 0x040000FD RID: 253
		private int number_;

		// Token: 0x040000FE RID: 254
		public const int NameFieldNumber = 4;

		// Token: 0x040000FF RID: 255
		private string name_ = "";

		// Token: 0x04000100 RID: 256
		public const int TypeUrlFieldNumber = 6;

		// Token: 0x04000101 RID: 257
		private string typeUrl_ = "";

		// Token: 0x04000102 RID: 258
		public const int OneofIndexFieldNumber = 7;

		// Token: 0x04000103 RID: 259
		private int oneofIndex_;

		// Token: 0x04000104 RID: 260
		public const int PackedFieldNumber = 8;

		// Token: 0x04000105 RID: 261
		private bool packed_;

		// Token: 0x04000106 RID: 262
		public const int OptionsFieldNumber = 9;

		// Token: 0x04000107 RID: 263
		private static readonly FieldCodec<Option> _repeated_options_codec = FieldCodec.ForMessage<Option>(74U, Option.Parser);

		// Token: 0x04000108 RID: 264
		private readonly RepeatedField<Option> options_ = new RepeatedField<Option>();

		// Token: 0x04000109 RID: 265
		public const int JsonNameFieldNumber = 10;

		// Token: 0x0400010A RID: 266
		private string jsonName_ = "";

		// Token: 0x0400010B RID: 267
		public const int DefaultValueFieldNumber = 11;

		// Token: 0x0400010C RID: 268
		private string defaultValue_ = "";

		// Token: 0x020000BA RID: 186
		[DebuggerNonUserCode]
		public static class Types
		{
			// Token: 0x02000111 RID: 273
			public enum Kind
			{
				// Token: 0x04000457 RID: 1111
				[OriginalName("TYPE_UNKNOWN")]
				TypeUnknown,
				// Token: 0x04000458 RID: 1112
				[OriginalName("TYPE_DOUBLE")]
				TypeDouble,
				// Token: 0x04000459 RID: 1113
				[OriginalName("TYPE_FLOAT")]
				TypeFloat,
				// Token: 0x0400045A RID: 1114
				[OriginalName("TYPE_INT64")]
				TypeInt64,
				// Token: 0x0400045B RID: 1115
				[OriginalName("TYPE_UINT64")]
				TypeUint64,
				// Token: 0x0400045C RID: 1116
				[OriginalName("TYPE_INT32")]
				TypeInt32,
				// Token: 0x0400045D RID: 1117
				[OriginalName("TYPE_FIXED64")]
				TypeFixed64,
				// Token: 0x0400045E RID: 1118
				[OriginalName("TYPE_FIXED32")]
				TypeFixed32,
				// Token: 0x0400045F RID: 1119
				[OriginalName("TYPE_BOOL")]
				TypeBool,
				// Token: 0x04000460 RID: 1120
				[OriginalName("TYPE_STRING")]
				TypeString,
				// Token: 0x04000461 RID: 1121
				[OriginalName("TYPE_GROUP")]
				TypeGroup,
				// Token: 0x04000462 RID: 1122
				[OriginalName("TYPE_MESSAGE")]
				TypeMessage,
				// Token: 0x04000463 RID: 1123
				[OriginalName("TYPE_BYTES")]
				TypeBytes,
				// Token: 0x04000464 RID: 1124
				[OriginalName("TYPE_UINT32")]
				TypeUint32,
				// Token: 0x04000465 RID: 1125
				[OriginalName("TYPE_ENUM")]
				TypeEnum,
				// Token: 0x04000466 RID: 1126
				[OriginalName("TYPE_SFIXED32")]
				TypeSfixed32,
				// Token: 0x04000467 RID: 1127
				[OriginalName("TYPE_SFIXED64")]
				TypeSfixed64,
				// Token: 0x04000468 RID: 1128
				[OriginalName("TYPE_SINT32")]
				TypeSint32,
				// Token: 0x04000469 RID: 1129
				[OriginalName("TYPE_SINT64")]
				TypeSint64
			}

			// Token: 0x02000112 RID: 274
			public enum Cardinality
			{
				// Token: 0x0400046B RID: 1131
				[OriginalName("CARDINALITY_UNKNOWN")]
				Unknown,
				// Token: 0x0400046C RID: 1132
				[OriginalName("CARDINALITY_OPTIONAL")]
				Optional,
				// Token: 0x0400046D RID: 1133
				[OriginalName("CARDINALITY_REQUIRED")]
				Required,
				// Token: 0x0400046E RID: 1134
				[OriginalName("CARDINALITY_REPEATED")]
				Repeated
			}
		}
	}
}
