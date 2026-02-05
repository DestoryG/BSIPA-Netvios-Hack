using System;
using System.Diagnostics;

namespace Google.Protobuf.Reflection
{
	// Token: 0x02000054 RID: 84
	public sealed class FieldDescriptorProto : IMessage<FieldDescriptorProto>, IMessage, IEquatable<FieldDescriptorProto>, IDeepCloneable<FieldDescriptorProto>
	{
		// Token: 0x17000102 RID: 258
		// (get) Token: 0x060004DC RID: 1244 RVA: 0x00013B7B File Offset: 0x00011D7B
		[DebuggerNonUserCode]
		public static MessageParser<FieldDescriptorProto> Parser
		{
			get
			{
				return FieldDescriptorProto._parser;
			}
		}

		// Token: 0x17000103 RID: 259
		// (get) Token: 0x060004DD RID: 1245 RVA: 0x00013B82 File Offset: 0x00011D82
		[DebuggerNonUserCode]
		public static MessageDescriptor Descriptor
		{
			get
			{
				return DescriptorReflection.Descriptor.MessageTypes[4];
			}
		}

		// Token: 0x17000104 RID: 260
		// (get) Token: 0x060004DE RID: 1246 RVA: 0x00013B94 File Offset: 0x00011D94
		[DebuggerNonUserCode]
		MessageDescriptor IMessage.Descriptor
		{
			get
			{
				return FieldDescriptorProto.Descriptor;
			}
		}

		// Token: 0x060004DF RID: 1247 RVA: 0x00013B9B File Offset: 0x00011D9B
		[DebuggerNonUserCode]
		public FieldDescriptorProto()
		{
		}

		// Token: 0x060004E0 RID: 1248 RVA: 0x00013BA4 File Offset: 0x00011DA4
		[DebuggerNonUserCode]
		public FieldDescriptorProto(FieldDescriptorProto other)
			: this()
		{
			this._hasBits0 = other._hasBits0;
			this.name_ = other.name_;
			this.number_ = other.number_;
			this.label_ = other.label_;
			this.type_ = other.type_;
			this.typeName_ = other.typeName_;
			this.extendee_ = other.extendee_;
			this.defaultValue_ = other.defaultValue_;
			this.oneofIndex_ = other.oneofIndex_;
			this.jsonName_ = other.jsonName_;
			this.options_ = ((other.options_ != null) ? other.options_.Clone() : null);
			this.proto3Optional_ = other.proto3Optional_;
			this._unknownFields = UnknownFieldSet.Clone(other._unknownFields);
		}

		// Token: 0x060004E1 RID: 1249 RVA: 0x00013C68 File Offset: 0x00011E68
		[DebuggerNonUserCode]
		public FieldDescriptorProto Clone()
		{
			return new FieldDescriptorProto(this);
		}

		// Token: 0x17000105 RID: 261
		// (get) Token: 0x060004E2 RID: 1250 RVA: 0x00013C70 File Offset: 0x00011E70
		// (set) Token: 0x060004E3 RID: 1251 RVA: 0x00013C81 File Offset: 0x00011E81
		[DebuggerNonUserCode]
		public string Name
		{
			get
			{
				return this.name_ ?? FieldDescriptorProto.NameDefaultValue;
			}
			set
			{
				this.name_ = ProtoPreconditions.CheckNotNull<string>(value, "value");
			}
		}

		// Token: 0x17000106 RID: 262
		// (get) Token: 0x060004E4 RID: 1252 RVA: 0x00013C94 File Offset: 0x00011E94
		[DebuggerNonUserCode]
		public bool HasName
		{
			get
			{
				return this.name_ != null;
			}
		}

		// Token: 0x060004E5 RID: 1253 RVA: 0x00013C9F File Offset: 0x00011E9F
		[DebuggerNonUserCode]
		public void ClearName()
		{
			this.name_ = null;
		}

		// Token: 0x17000107 RID: 263
		// (get) Token: 0x060004E6 RID: 1254 RVA: 0x00013CA8 File Offset: 0x00011EA8
		// (set) Token: 0x060004E7 RID: 1255 RVA: 0x00013CC0 File Offset: 0x00011EC0
		[DebuggerNonUserCode]
		public int Number
		{
			get
			{
				if ((this._hasBits0 & 1) != 0)
				{
					return this.number_;
				}
				return FieldDescriptorProto.NumberDefaultValue;
			}
			set
			{
				this._hasBits0 |= 1;
				this.number_ = value;
			}
		}

		// Token: 0x17000108 RID: 264
		// (get) Token: 0x060004E8 RID: 1256 RVA: 0x00013CD7 File Offset: 0x00011ED7
		[DebuggerNonUserCode]
		public bool HasNumber
		{
			get
			{
				return (this._hasBits0 & 1) != 0;
			}
		}

		// Token: 0x060004E9 RID: 1257 RVA: 0x00013CE4 File Offset: 0x00011EE4
		[DebuggerNonUserCode]
		public void ClearNumber()
		{
			this._hasBits0 &= -2;
		}

		// Token: 0x17000109 RID: 265
		// (get) Token: 0x060004EA RID: 1258 RVA: 0x00013CF5 File Offset: 0x00011EF5
		// (set) Token: 0x060004EB RID: 1259 RVA: 0x00013D0D File Offset: 0x00011F0D
		[DebuggerNonUserCode]
		public FieldDescriptorProto.Types.Label Label
		{
			get
			{
				if ((this._hasBits0 & 2) != 0)
				{
					return this.label_;
				}
				return FieldDescriptorProto.LabelDefaultValue;
			}
			set
			{
				this._hasBits0 |= 2;
				this.label_ = value;
			}
		}

		// Token: 0x1700010A RID: 266
		// (get) Token: 0x060004EC RID: 1260 RVA: 0x00013D24 File Offset: 0x00011F24
		[DebuggerNonUserCode]
		public bool HasLabel
		{
			get
			{
				return (this._hasBits0 & 2) != 0;
			}
		}

		// Token: 0x060004ED RID: 1261 RVA: 0x00013D31 File Offset: 0x00011F31
		[DebuggerNonUserCode]
		public void ClearLabel()
		{
			this._hasBits0 &= -3;
		}

		// Token: 0x1700010B RID: 267
		// (get) Token: 0x060004EE RID: 1262 RVA: 0x00013D42 File Offset: 0x00011F42
		// (set) Token: 0x060004EF RID: 1263 RVA: 0x00013D5A File Offset: 0x00011F5A
		[DebuggerNonUserCode]
		public FieldDescriptorProto.Types.Type Type
		{
			get
			{
				if ((this._hasBits0 & 4) != 0)
				{
					return this.type_;
				}
				return FieldDescriptorProto.TypeDefaultValue;
			}
			set
			{
				this._hasBits0 |= 4;
				this.type_ = value;
			}
		}

		// Token: 0x1700010C RID: 268
		// (get) Token: 0x060004F0 RID: 1264 RVA: 0x00013D71 File Offset: 0x00011F71
		[DebuggerNonUserCode]
		public bool HasType
		{
			get
			{
				return (this._hasBits0 & 4) != 0;
			}
		}

		// Token: 0x060004F1 RID: 1265 RVA: 0x00013D7E File Offset: 0x00011F7E
		[DebuggerNonUserCode]
		public void ClearType()
		{
			this._hasBits0 &= -5;
		}

		// Token: 0x1700010D RID: 269
		// (get) Token: 0x060004F2 RID: 1266 RVA: 0x00013D8F File Offset: 0x00011F8F
		// (set) Token: 0x060004F3 RID: 1267 RVA: 0x00013DA0 File Offset: 0x00011FA0
		[DebuggerNonUserCode]
		public string TypeName
		{
			get
			{
				return this.typeName_ ?? FieldDescriptorProto.TypeNameDefaultValue;
			}
			set
			{
				this.typeName_ = ProtoPreconditions.CheckNotNull<string>(value, "value");
			}
		}

		// Token: 0x1700010E RID: 270
		// (get) Token: 0x060004F4 RID: 1268 RVA: 0x00013DB3 File Offset: 0x00011FB3
		[DebuggerNonUserCode]
		public bool HasTypeName
		{
			get
			{
				return this.typeName_ != null;
			}
		}

		// Token: 0x060004F5 RID: 1269 RVA: 0x00013DBE File Offset: 0x00011FBE
		[DebuggerNonUserCode]
		public void ClearTypeName()
		{
			this.typeName_ = null;
		}

		// Token: 0x1700010F RID: 271
		// (get) Token: 0x060004F6 RID: 1270 RVA: 0x00013DC7 File Offset: 0x00011FC7
		// (set) Token: 0x060004F7 RID: 1271 RVA: 0x00013DD8 File Offset: 0x00011FD8
		[DebuggerNonUserCode]
		public string Extendee
		{
			get
			{
				return this.extendee_ ?? FieldDescriptorProto.ExtendeeDefaultValue;
			}
			set
			{
				this.extendee_ = ProtoPreconditions.CheckNotNull<string>(value, "value");
			}
		}

		// Token: 0x17000110 RID: 272
		// (get) Token: 0x060004F8 RID: 1272 RVA: 0x00013DEB File Offset: 0x00011FEB
		[DebuggerNonUserCode]
		public bool HasExtendee
		{
			get
			{
				return this.extendee_ != null;
			}
		}

		// Token: 0x060004F9 RID: 1273 RVA: 0x00013DF6 File Offset: 0x00011FF6
		[DebuggerNonUserCode]
		public void ClearExtendee()
		{
			this.extendee_ = null;
		}

		// Token: 0x17000111 RID: 273
		// (get) Token: 0x060004FA RID: 1274 RVA: 0x00013DFF File Offset: 0x00011FFF
		// (set) Token: 0x060004FB RID: 1275 RVA: 0x00013E10 File Offset: 0x00012010
		[DebuggerNonUserCode]
		public string DefaultValue
		{
			get
			{
				return this.defaultValue_ ?? FieldDescriptorProto.DefaultValueDefaultValue;
			}
			set
			{
				this.defaultValue_ = ProtoPreconditions.CheckNotNull<string>(value, "value");
			}
		}

		// Token: 0x17000112 RID: 274
		// (get) Token: 0x060004FC RID: 1276 RVA: 0x00013E23 File Offset: 0x00012023
		[DebuggerNonUserCode]
		public bool HasDefaultValue
		{
			get
			{
				return this.defaultValue_ != null;
			}
		}

		// Token: 0x060004FD RID: 1277 RVA: 0x00013E2E File Offset: 0x0001202E
		[DebuggerNonUserCode]
		public void ClearDefaultValue()
		{
			this.defaultValue_ = null;
		}

		// Token: 0x17000113 RID: 275
		// (get) Token: 0x060004FE RID: 1278 RVA: 0x00013E37 File Offset: 0x00012037
		// (set) Token: 0x060004FF RID: 1279 RVA: 0x00013E4F File Offset: 0x0001204F
		[DebuggerNonUserCode]
		public int OneofIndex
		{
			get
			{
				if ((this._hasBits0 & 8) != 0)
				{
					return this.oneofIndex_;
				}
				return FieldDescriptorProto.OneofIndexDefaultValue;
			}
			set
			{
				this._hasBits0 |= 8;
				this.oneofIndex_ = value;
			}
		}

		// Token: 0x17000114 RID: 276
		// (get) Token: 0x06000500 RID: 1280 RVA: 0x00013E66 File Offset: 0x00012066
		[DebuggerNonUserCode]
		public bool HasOneofIndex
		{
			get
			{
				return (this._hasBits0 & 8) != 0;
			}
		}

		// Token: 0x06000501 RID: 1281 RVA: 0x00013E73 File Offset: 0x00012073
		[DebuggerNonUserCode]
		public void ClearOneofIndex()
		{
			this._hasBits0 &= -9;
		}

		// Token: 0x17000115 RID: 277
		// (get) Token: 0x06000502 RID: 1282 RVA: 0x00013E84 File Offset: 0x00012084
		// (set) Token: 0x06000503 RID: 1283 RVA: 0x00013E95 File Offset: 0x00012095
		[DebuggerNonUserCode]
		public string JsonName
		{
			get
			{
				return this.jsonName_ ?? FieldDescriptorProto.JsonNameDefaultValue;
			}
			set
			{
				this.jsonName_ = ProtoPreconditions.CheckNotNull<string>(value, "value");
			}
		}

		// Token: 0x17000116 RID: 278
		// (get) Token: 0x06000504 RID: 1284 RVA: 0x00013EA8 File Offset: 0x000120A8
		[DebuggerNonUserCode]
		public bool HasJsonName
		{
			get
			{
				return this.jsonName_ != null;
			}
		}

		// Token: 0x06000505 RID: 1285 RVA: 0x00013EB3 File Offset: 0x000120B3
		[DebuggerNonUserCode]
		public void ClearJsonName()
		{
			this.jsonName_ = null;
		}

		// Token: 0x17000117 RID: 279
		// (get) Token: 0x06000506 RID: 1286 RVA: 0x00013EBC File Offset: 0x000120BC
		// (set) Token: 0x06000507 RID: 1287 RVA: 0x00013EC4 File Offset: 0x000120C4
		[DebuggerNonUserCode]
		public FieldOptions Options
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

		// Token: 0x17000118 RID: 280
		// (get) Token: 0x06000508 RID: 1288 RVA: 0x00013ECD File Offset: 0x000120CD
		// (set) Token: 0x06000509 RID: 1289 RVA: 0x00013EE6 File Offset: 0x000120E6
		[DebuggerNonUserCode]
		public bool Proto3Optional
		{
			get
			{
				if ((this._hasBits0 & 16) != 0)
				{
					return this.proto3Optional_;
				}
				return FieldDescriptorProto.Proto3OptionalDefaultValue;
			}
			set
			{
				this._hasBits0 |= 16;
				this.proto3Optional_ = value;
			}
		}

		// Token: 0x17000119 RID: 281
		// (get) Token: 0x0600050A RID: 1290 RVA: 0x00013EFE File Offset: 0x000120FE
		[DebuggerNonUserCode]
		public bool HasProto3Optional
		{
			get
			{
				return (this._hasBits0 & 16) != 0;
			}
		}

		// Token: 0x0600050B RID: 1291 RVA: 0x00013F0C File Offset: 0x0001210C
		[DebuggerNonUserCode]
		public void ClearProto3Optional()
		{
			this._hasBits0 &= -17;
		}

		// Token: 0x0600050C RID: 1292 RVA: 0x00013F1D File Offset: 0x0001211D
		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return this.Equals(other as FieldDescriptorProto);
		}

		// Token: 0x0600050D RID: 1293 RVA: 0x00013F2C File Offset: 0x0001212C
		[DebuggerNonUserCode]
		public bool Equals(FieldDescriptorProto other)
		{
			return other != null && (other == this || (!(this.Name != other.Name) && this.Number == other.Number && this.Label == other.Label && this.Type == other.Type && !(this.TypeName != other.TypeName) && !(this.Extendee != other.Extendee) && !(this.DefaultValue != other.DefaultValue) && this.OneofIndex == other.OneofIndex && !(this.JsonName != other.JsonName) && object.Equals(this.Options, other.Options) && this.Proto3Optional == other.Proto3Optional && object.Equals(this._unknownFields, other._unknownFields)));
		}

		// Token: 0x0600050E RID: 1294 RVA: 0x00014024 File Offset: 0x00012224
		[DebuggerNonUserCode]
		public override int GetHashCode()
		{
			int num = 1;
			if (this.HasName)
			{
				num ^= this.Name.GetHashCode();
			}
			if (this.HasNumber)
			{
				num ^= this.Number.GetHashCode();
			}
			if (this.HasLabel)
			{
				num ^= this.Label.GetHashCode();
			}
			if (this.HasType)
			{
				num ^= this.Type.GetHashCode();
			}
			if (this.HasTypeName)
			{
				num ^= this.TypeName.GetHashCode();
			}
			if (this.HasExtendee)
			{
				num ^= this.Extendee.GetHashCode();
			}
			if (this.HasDefaultValue)
			{
				num ^= this.DefaultValue.GetHashCode();
			}
			if (this.HasOneofIndex)
			{
				num ^= this.OneofIndex.GetHashCode();
			}
			if (this.HasJsonName)
			{
				num ^= this.JsonName.GetHashCode();
			}
			if (this.options_ != null)
			{
				num ^= this.Options.GetHashCode();
			}
			if (this.HasProto3Optional)
			{
				num ^= this.Proto3Optional.GetHashCode();
			}
			if (this._unknownFields != null)
			{
				num ^= this._unknownFields.GetHashCode();
			}
			return num;
		}

		// Token: 0x0600050F RID: 1295 RVA: 0x00014158 File Offset: 0x00012358
		[DebuggerNonUserCode]
		public override string ToString()
		{
			return JsonFormatter.ToDiagnosticString(this);
		}

		// Token: 0x06000510 RID: 1296 RVA: 0x00014160 File Offset: 0x00012360
		[DebuggerNonUserCode]
		public void WriteTo(CodedOutputStream output)
		{
			if (this.HasName)
			{
				output.WriteRawTag(10);
				output.WriteString(this.Name);
			}
			if (this.HasExtendee)
			{
				output.WriteRawTag(18);
				output.WriteString(this.Extendee);
			}
			if (this.HasNumber)
			{
				output.WriteRawTag(24);
				output.WriteInt32(this.Number);
			}
			if (this.HasLabel)
			{
				output.WriteRawTag(32);
				output.WriteEnum((int)this.Label);
			}
			if (this.HasType)
			{
				output.WriteRawTag(40);
				output.WriteEnum((int)this.Type);
			}
			if (this.HasTypeName)
			{
				output.WriteRawTag(50);
				output.WriteString(this.TypeName);
			}
			if (this.HasDefaultValue)
			{
				output.WriteRawTag(58);
				output.WriteString(this.DefaultValue);
			}
			if (this.options_ != null)
			{
				output.WriteRawTag(66);
				output.WriteMessage(this.Options);
			}
			if (this.HasOneofIndex)
			{
				output.WriteRawTag(72);
				output.WriteInt32(this.OneofIndex);
			}
			if (this.HasJsonName)
			{
				output.WriteRawTag(82);
				output.WriteString(this.JsonName);
			}
			if (this.HasProto3Optional)
			{
				output.WriteRawTag(136, 1);
				output.WriteBool(this.Proto3Optional);
			}
			if (this._unknownFields != null)
			{
				this._unknownFields.WriteTo(output);
			}
		}

		// Token: 0x06000511 RID: 1297 RVA: 0x000142BC File Offset: 0x000124BC
		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (this.HasName)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.Name);
			}
			if (this.HasNumber)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.Number);
			}
			if (this.HasLabel)
			{
				num += 1 + CodedOutputStream.ComputeEnumSize((int)this.Label);
			}
			if (this.HasType)
			{
				num += 1 + CodedOutputStream.ComputeEnumSize((int)this.Type);
			}
			if (this.HasTypeName)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.TypeName);
			}
			if (this.HasExtendee)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.Extendee);
			}
			if (this.HasDefaultValue)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.DefaultValue);
			}
			if (this.HasOneofIndex)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.OneofIndex);
			}
			if (this.HasJsonName)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.JsonName);
			}
			if (this.options_ != null)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(this.Options);
			}
			if (this.HasProto3Optional)
			{
				num += 3;
			}
			if (this._unknownFields != null)
			{
				num += this._unknownFields.CalculateSize();
			}
			return num;
		}

		// Token: 0x06000512 RID: 1298 RVA: 0x000143E0 File Offset: 0x000125E0
		[DebuggerNonUserCode]
		public void MergeFrom(FieldDescriptorProto other)
		{
			if (other == null)
			{
				return;
			}
			if (other.HasName)
			{
				this.Name = other.Name;
			}
			if (other.HasNumber)
			{
				this.Number = other.Number;
			}
			if (other.HasLabel)
			{
				this.Label = other.Label;
			}
			if (other.HasType)
			{
				this.Type = other.Type;
			}
			if (other.HasTypeName)
			{
				this.TypeName = other.TypeName;
			}
			if (other.HasExtendee)
			{
				this.Extendee = other.Extendee;
			}
			if (other.HasDefaultValue)
			{
				this.DefaultValue = other.DefaultValue;
			}
			if (other.HasOneofIndex)
			{
				this.OneofIndex = other.OneofIndex;
			}
			if (other.HasJsonName)
			{
				this.JsonName = other.JsonName;
			}
			if (other.options_ != null)
			{
				if (this.options_ == null)
				{
					this.Options = new FieldOptions();
				}
				this.Options.MergeFrom(other.Options);
			}
			if (other.HasProto3Optional)
			{
				this.Proto3Optional = other.Proto3Optional;
			}
			this._unknownFields = UnknownFieldSet.MergeFrom(this._unknownFields, other._unknownFields);
		}

		// Token: 0x06000513 RID: 1299 RVA: 0x000144FC File Offset: 0x000126FC
		[DebuggerNonUserCode]
		public void MergeFrom(CodedInputStream input)
		{
			uint num;
			while ((num = input.ReadTag()) != 0U)
			{
				if (num <= 40U)
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
							this.Extendee = input.ReadString();
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
						if (num == 32U)
						{
							this.Label = (FieldDescriptorProto.Types.Label)input.ReadEnum();
							continue;
						}
						if (num == 40U)
						{
							this.Type = (FieldDescriptorProto.Types.Type)input.ReadEnum();
							continue;
						}
					}
				}
				else if (num <= 66U)
				{
					if (num == 50U)
					{
						this.TypeName = input.ReadString();
						continue;
					}
					if (num == 58U)
					{
						this.DefaultValue = input.ReadString();
						continue;
					}
					if (num == 66U)
					{
						if (this.options_ == null)
						{
							this.Options = new FieldOptions();
						}
						input.ReadMessage(this.Options);
						continue;
					}
				}
				else
				{
					if (num == 72U)
					{
						this.OneofIndex = input.ReadInt32();
						continue;
					}
					if (num == 82U)
					{
						this.JsonName = input.ReadString();
						continue;
					}
					if (num == 136U)
					{
						this.Proto3Optional = input.ReadBool();
						continue;
					}
				}
				this._unknownFields = UnknownFieldSet.MergeFieldFrom(this._unknownFields, input);
			}
		}

		// Token: 0x040001A1 RID: 417
		private static readonly MessageParser<FieldDescriptorProto> _parser = new MessageParser<FieldDescriptorProto>(() => new FieldDescriptorProto());

		// Token: 0x040001A2 RID: 418
		private UnknownFieldSet _unknownFields;

		// Token: 0x040001A3 RID: 419
		private int _hasBits0;

		// Token: 0x040001A4 RID: 420
		public const int NameFieldNumber = 1;

		// Token: 0x040001A5 RID: 421
		private static readonly string NameDefaultValue = "";

		// Token: 0x040001A6 RID: 422
		private string name_;

		// Token: 0x040001A7 RID: 423
		public const int NumberFieldNumber = 3;

		// Token: 0x040001A8 RID: 424
		private static readonly int NumberDefaultValue = 0;

		// Token: 0x040001A9 RID: 425
		private int number_;

		// Token: 0x040001AA RID: 426
		public const int LabelFieldNumber = 4;

		// Token: 0x040001AB RID: 427
		private static readonly FieldDescriptorProto.Types.Label LabelDefaultValue = FieldDescriptorProto.Types.Label.Optional;

		// Token: 0x040001AC RID: 428
		private FieldDescriptorProto.Types.Label label_;

		// Token: 0x040001AD RID: 429
		public const int TypeFieldNumber = 5;

		// Token: 0x040001AE RID: 430
		private static readonly FieldDescriptorProto.Types.Type TypeDefaultValue = FieldDescriptorProto.Types.Type.Double;

		// Token: 0x040001AF RID: 431
		private FieldDescriptorProto.Types.Type type_;

		// Token: 0x040001B0 RID: 432
		public const int TypeNameFieldNumber = 6;

		// Token: 0x040001B1 RID: 433
		private static readonly string TypeNameDefaultValue = "";

		// Token: 0x040001B2 RID: 434
		private string typeName_;

		// Token: 0x040001B3 RID: 435
		public const int ExtendeeFieldNumber = 2;

		// Token: 0x040001B4 RID: 436
		private static readonly string ExtendeeDefaultValue = "";

		// Token: 0x040001B5 RID: 437
		private string extendee_;

		// Token: 0x040001B6 RID: 438
		public const int DefaultValueFieldNumber = 7;

		// Token: 0x040001B7 RID: 439
		private static readonly string DefaultValueDefaultValue = "";

		// Token: 0x040001B8 RID: 440
		private string defaultValue_;

		// Token: 0x040001B9 RID: 441
		public const int OneofIndexFieldNumber = 9;

		// Token: 0x040001BA RID: 442
		private static readonly int OneofIndexDefaultValue = 0;

		// Token: 0x040001BB RID: 443
		private int oneofIndex_;

		// Token: 0x040001BC RID: 444
		public const int JsonNameFieldNumber = 10;

		// Token: 0x040001BD RID: 445
		private static readonly string JsonNameDefaultValue = "";

		// Token: 0x040001BE RID: 446
		private string jsonName_;

		// Token: 0x040001BF RID: 447
		public const int OptionsFieldNumber = 8;

		// Token: 0x040001C0 RID: 448
		private FieldOptions options_;

		// Token: 0x040001C1 RID: 449
		public const int Proto3OptionalFieldNumber = 17;

		// Token: 0x040001C2 RID: 450
		private static readonly bool Proto3OptionalDefaultValue = false;

		// Token: 0x040001C3 RID: 451
		private bool proto3Optional_;

		// Token: 0x020000CE RID: 206
		[DebuggerNonUserCode]
		public static class Types
		{
			// Token: 0x02000115 RID: 277
			public enum Type
			{
				// Token: 0x04000484 RID: 1156
				[OriginalName("TYPE_DOUBLE")]
				Double = 1,
				// Token: 0x04000485 RID: 1157
				[OriginalName("TYPE_FLOAT")]
				Float,
				// Token: 0x04000486 RID: 1158
				[OriginalName("TYPE_INT64")]
				Int64,
				// Token: 0x04000487 RID: 1159
				[OriginalName("TYPE_UINT64")]
				Uint64,
				// Token: 0x04000488 RID: 1160
				[OriginalName("TYPE_INT32")]
				Int32,
				// Token: 0x04000489 RID: 1161
				[OriginalName("TYPE_FIXED64")]
				Fixed64,
				// Token: 0x0400048A RID: 1162
				[OriginalName("TYPE_FIXED32")]
				Fixed32,
				// Token: 0x0400048B RID: 1163
				[OriginalName("TYPE_BOOL")]
				Bool,
				// Token: 0x0400048C RID: 1164
				[OriginalName("TYPE_STRING")]
				String,
				// Token: 0x0400048D RID: 1165
				[OriginalName("TYPE_GROUP")]
				Group,
				// Token: 0x0400048E RID: 1166
				[OriginalName("TYPE_MESSAGE")]
				Message,
				// Token: 0x0400048F RID: 1167
				[OriginalName("TYPE_BYTES")]
				Bytes,
				// Token: 0x04000490 RID: 1168
				[OriginalName("TYPE_UINT32")]
				Uint32,
				// Token: 0x04000491 RID: 1169
				[OriginalName("TYPE_ENUM")]
				Enum,
				// Token: 0x04000492 RID: 1170
				[OriginalName("TYPE_SFIXED32")]
				Sfixed32,
				// Token: 0x04000493 RID: 1171
				[OriginalName("TYPE_SFIXED64")]
				Sfixed64,
				// Token: 0x04000494 RID: 1172
				[OriginalName("TYPE_SINT32")]
				Sint32,
				// Token: 0x04000495 RID: 1173
				[OriginalName("TYPE_SINT64")]
				Sint64
			}

			// Token: 0x02000116 RID: 278
			public enum Label
			{
				// Token: 0x04000497 RID: 1175
				[OriginalName("LABEL_OPTIONAL")]
				Optional = 1,
				// Token: 0x04000498 RID: 1176
				[OriginalName("LABEL_REQUIRED")]
				Required,
				// Token: 0x04000499 RID: 1177
				[OriginalName("LABEL_REPEATED")]
				Repeated
			}
		}
	}
}
