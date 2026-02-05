using System;
using System.Diagnostics;
using Google.Protobuf.Collections;

namespace Google.Protobuf.Reflection
{
	// Token: 0x02000051 RID: 81
	public sealed class FileDescriptorProto : IMessage<FileDescriptorProto>, IMessage, IEquatable<FileDescriptorProto>, IDeepCloneable<FileDescriptorProto>
	{
		// Token: 0x170000DD RID: 221
		// (get) Token: 0x06000481 RID: 1153 RVA: 0x0001253C File Offset: 0x0001073C
		[DebuggerNonUserCode]
		public static MessageParser<FileDescriptorProto> Parser
		{
			get
			{
				return FileDescriptorProto._parser;
			}
		}

		// Token: 0x170000DE RID: 222
		// (get) Token: 0x06000482 RID: 1154 RVA: 0x00012543 File Offset: 0x00010743
		[DebuggerNonUserCode]
		public static MessageDescriptor Descriptor
		{
			get
			{
				return DescriptorReflection.Descriptor.MessageTypes[1];
			}
		}

		// Token: 0x170000DF RID: 223
		// (get) Token: 0x06000483 RID: 1155 RVA: 0x00012555 File Offset: 0x00010755
		[DebuggerNonUserCode]
		MessageDescriptor IMessage.Descriptor
		{
			get
			{
				return FileDescriptorProto.Descriptor;
			}
		}

		// Token: 0x06000484 RID: 1156 RVA: 0x0001255C File Offset: 0x0001075C
		[DebuggerNonUserCode]
		public FileDescriptorProto()
		{
		}

		// Token: 0x06000485 RID: 1157 RVA: 0x000125BC File Offset: 0x000107BC
		[DebuggerNonUserCode]
		public FileDescriptorProto(FileDescriptorProto other)
			: this()
		{
			this.name_ = other.name_;
			this.package_ = other.package_;
			this.dependency_ = other.dependency_.Clone();
			this.publicDependency_ = other.publicDependency_.Clone();
			this.weakDependency_ = other.weakDependency_.Clone();
			this.messageType_ = other.messageType_.Clone();
			this.enumType_ = other.enumType_.Clone();
			this.service_ = other.service_.Clone();
			this.extension_ = other.extension_.Clone();
			this.options_ = ((other.options_ != null) ? other.options_.Clone() : null);
			this.sourceCodeInfo_ = ((other.sourceCodeInfo_ != null) ? other.sourceCodeInfo_.Clone() : null);
			this.syntax_ = other.syntax_;
			this._unknownFields = UnknownFieldSet.Clone(other._unknownFields);
		}

		// Token: 0x06000486 RID: 1158 RVA: 0x000126B3 File Offset: 0x000108B3
		[DebuggerNonUserCode]
		public FileDescriptorProto Clone()
		{
			return new FileDescriptorProto(this);
		}

		// Token: 0x170000E0 RID: 224
		// (get) Token: 0x06000487 RID: 1159 RVA: 0x000126BB File Offset: 0x000108BB
		// (set) Token: 0x06000488 RID: 1160 RVA: 0x000126CC File Offset: 0x000108CC
		[DebuggerNonUserCode]
		public string Name
		{
			get
			{
				return this.name_ ?? FileDescriptorProto.NameDefaultValue;
			}
			set
			{
				this.name_ = ProtoPreconditions.CheckNotNull<string>(value, "value");
			}
		}

		// Token: 0x170000E1 RID: 225
		// (get) Token: 0x06000489 RID: 1161 RVA: 0x000126DF File Offset: 0x000108DF
		[DebuggerNonUserCode]
		public bool HasName
		{
			get
			{
				return this.name_ != null;
			}
		}

		// Token: 0x0600048A RID: 1162 RVA: 0x000126EA File Offset: 0x000108EA
		[DebuggerNonUserCode]
		public void ClearName()
		{
			this.name_ = null;
		}

		// Token: 0x170000E2 RID: 226
		// (get) Token: 0x0600048B RID: 1163 RVA: 0x000126F3 File Offset: 0x000108F3
		// (set) Token: 0x0600048C RID: 1164 RVA: 0x00012704 File Offset: 0x00010904
		[DebuggerNonUserCode]
		public string Package
		{
			get
			{
				return this.package_ ?? FileDescriptorProto.PackageDefaultValue;
			}
			set
			{
				this.package_ = ProtoPreconditions.CheckNotNull<string>(value, "value");
			}
		}

		// Token: 0x170000E3 RID: 227
		// (get) Token: 0x0600048D RID: 1165 RVA: 0x00012717 File Offset: 0x00010917
		[DebuggerNonUserCode]
		public bool HasPackage
		{
			get
			{
				return this.package_ != null;
			}
		}

		// Token: 0x0600048E RID: 1166 RVA: 0x00012722 File Offset: 0x00010922
		[DebuggerNonUserCode]
		public void ClearPackage()
		{
			this.package_ = null;
		}

		// Token: 0x170000E4 RID: 228
		// (get) Token: 0x0600048F RID: 1167 RVA: 0x0001272B File Offset: 0x0001092B
		[DebuggerNonUserCode]
		public RepeatedField<string> Dependency
		{
			get
			{
				return this.dependency_;
			}
		}

		// Token: 0x170000E5 RID: 229
		// (get) Token: 0x06000490 RID: 1168 RVA: 0x00012733 File Offset: 0x00010933
		[DebuggerNonUserCode]
		public RepeatedField<int> PublicDependency
		{
			get
			{
				return this.publicDependency_;
			}
		}

		// Token: 0x170000E6 RID: 230
		// (get) Token: 0x06000491 RID: 1169 RVA: 0x0001273B File Offset: 0x0001093B
		[DebuggerNonUserCode]
		public RepeatedField<int> WeakDependency
		{
			get
			{
				return this.weakDependency_;
			}
		}

		// Token: 0x170000E7 RID: 231
		// (get) Token: 0x06000492 RID: 1170 RVA: 0x00012743 File Offset: 0x00010943
		[DebuggerNonUserCode]
		public RepeatedField<DescriptorProto> MessageType
		{
			get
			{
				return this.messageType_;
			}
		}

		// Token: 0x170000E8 RID: 232
		// (get) Token: 0x06000493 RID: 1171 RVA: 0x0001274B File Offset: 0x0001094B
		[DebuggerNonUserCode]
		public RepeatedField<EnumDescriptorProto> EnumType
		{
			get
			{
				return this.enumType_;
			}
		}

		// Token: 0x170000E9 RID: 233
		// (get) Token: 0x06000494 RID: 1172 RVA: 0x00012753 File Offset: 0x00010953
		[DebuggerNonUserCode]
		public RepeatedField<ServiceDescriptorProto> Service
		{
			get
			{
				return this.service_;
			}
		}

		// Token: 0x170000EA RID: 234
		// (get) Token: 0x06000495 RID: 1173 RVA: 0x0001275B File Offset: 0x0001095B
		[DebuggerNonUserCode]
		public RepeatedField<FieldDescriptorProto> Extension
		{
			get
			{
				return this.extension_;
			}
		}

		// Token: 0x170000EB RID: 235
		// (get) Token: 0x06000496 RID: 1174 RVA: 0x00012763 File Offset: 0x00010963
		// (set) Token: 0x06000497 RID: 1175 RVA: 0x0001276B File Offset: 0x0001096B
		[DebuggerNonUserCode]
		public FileOptions Options
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

		// Token: 0x170000EC RID: 236
		// (get) Token: 0x06000498 RID: 1176 RVA: 0x00012774 File Offset: 0x00010974
		// (set) Token: 0x06000499 RID: 1177 RVA: 0x0001277C File Offset: 0x0001097C
		[DebuggerNonUserCode]
		public SourceCodeInfo SourceCodeInfo
		{
			get
			{
				return this.sourceCodeInfo_;
			}
			set
			{
				this.sourceCodeInfo_ = value;
			}
		}

		// Token: 0x170000ED RID: 237
		// (get) Token: 0x0600049A RID: 1178 RVA: 0x00012785 File Offset: 0x00010985
		// (set) Token: 0x0600049B RID: 1179 RVA: 0x00012796 File Offset: 0x00010996
		[DebuggerNonUserCode]
		public string Syntax
		{
			get
			{
				return this.syntax_ ?? FileDescriptorProto.SyntaxDefaultValue;
			}
			set
			{
				this.syntax_ = ProtoPreconditions.CheckNotNull<string>(value, "value");
			}
		}

		// Token: 0x170000EE RID: 238
		// (get) Token: 0x0600049C RID: 1180 RVA: 0x000127A9 File Offset: 0x000109A9
		[DebuggerNonUserCode]
		public bool HasSyntax
		{
			get
			{
				return this.syntax_ != null;
			}
		}

		// Token: 0x0600049D RID: 1181 RVA: 0x000127B4 File Offset: 0x000109B4
		[DebuggerNonUserCode]
		public void ClearSyntax()
		{
			this.syntax_ = null;
		}

		// Token: 0x0600049E RID: 1182 RVA: 0x000127BD File Offset: 0x000109BD
		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return this.Equals(other as FileDescriptorProto);
		}

		// Token: 0x0600049F RID: 1183 RVA: 0x000127CC File Offset: 0x000109CC
		[DebuggerNonUserCode]
		public bool Equals(FileDescriptorProto other)
		{
			return other != null && (other == this || (!(this.Name != other.Name) && !(this.Package != other.Package) && this.dependency_.Equals(other.dependency_) && this.publicDependency_.Equals(other.publicDependency_) && this.weakDependency_.Equals(other.weakDependency_) && this.messageType_.Equals(other.messageType_) && this.enumType_.Equals(other.enumType_) && this.service_.Equals(other.service_) && this.extension_.Equals(other.extension_) && object.Equals(this.Options, other.Options) && object.Equals(this.SourceCodeInfo, other.SourceCodeInfo) && !(this.Syntax != other.Syntax) && object.Equals(this._unknownFields, other._unknownFields)));
		}

		// Token: 0x060004A0 RID: 1184 RVA: 0x000128F4 File Offset: 0x00010AF4
		[DebuggerNonUserCode]
		public override int GetHashCode()
		{
			int num = 1;
			if (this.HasName)
			{
				num ^= this.Name.GetHashCode();
			}
			if (this.HasPackage)
			{
				num ^= this.Package.GetHashCode();
			}
			num ^= this.dependency_.GetHashCode();
			num ^= this.publicDependency_.GetHashCode();
			num ^= this.weakDependency_.GetHashCode();
			num ^= this.messageType_.GetHashCode();
			num ^= this.enumType_.GetHashCode();
			num ^= this.service_.GetHashCode();
			num ^= this.extension_.GetHashCode();
			if (this.options_ != null)
			{
				num ^= this.Options.GetHashCode();
			}
			if (this.sourceCodeInfo_ != null)
			{
				num ^= this.SourceCodeInfo.GetHashCode();
			}
			if (this.HasSyntax)
			{
				num ^= this.Syntax.GetHashCode();
			}
			if (this._unknownFields != null)
			{
				num ^= this._unknownFields.GetHashCode();
			}
			return num;
		}

		// Token: 0x060004A1 RID: 1185 RVA: 0x000129EA File Offset: 0x00010BEA
		[DebuggerNonUserCode]
		public override string ToString()
		{
			return JsonFormatter.ToDiagnosticString(this);
		}

		// Token: 0x060004A2 RID: 1186 RVA: 0x000129F4 File Offset: 0x00010BF4
		[DebuggerNonUserCode]
		public void WriteTo(CodedOutputStream output)
		{
			if (this.HasName)
			{
				output.WriteRawTag(10);
				output.WriteString(this.Name);
			}
			if (this.HasPackage)
			{
				output.WriteRawTag(18);
				output.WriteString(this.Package);
			}
			this.dependency_.WriteTo(output, FileDescriptorProto._repeated_dependency_codec);
			this.messageType_.WriteTo(output, FileDescriptorProto._repeated_messageType_codec);
			this.enumType_.WriteTo(output, FileDescriptorProto._repeated_enumType_codec);
			this.service_.WriteTo(output, FileDescriptorProto._repeated_service_codec);
			this.extension_.WriteTo(output, FileDescriptorProto._repeated_extension_codec);
			if (this.options_ != null)
			{
				output.WriteRawTag(66);
				output.WriteMessage(this.Options);
			}
			if (this.sourceCodeInfo_ != null)
			{
				output.WriteRawTag(74);
				output.WriteMessage(this.SourceCodeInfo);
			}
			this.publicDependency_.WriteTo(output, FileDescriptorProto._repeated_publicDependency_codec);
			this.weakDependency_.WriteTo(output, FileDescriptorProto._repeated_weakDependency_codec);
			if (this.HasSyntax)
			{
				output.WriteRawTag(98);
				output.WriteString(this.Syntax);
			}
			if (this._unknownFields != null)
			{
				this._unknownFields.WriteTo(output);
			}
		}

		// Token: 0x060004A3 RID: 1187 RVA: 0x00012B18 File Offset: 0x00010D18
		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (this.HasName)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.Name);
			}
			if (this.HasPackage)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.Package);
			}
			num += this.dependency_.CalculateSize(FileDescriptorProto._repeated_dependency_codec);
			num += this.publicDependency_.CalculateSize(FileDescriptorProto._repeated_publicDependency_codec);
			num += this.weakDependency_.CalculateSize(FileDescriptorProto._repeated_weakDependency_codec);
			num += this.messageType_.CalculateSize(FileDescriptorProto._repeated_messageType_codec);
			num += this.enumType_.CalculateSize(FileDescriptorProto._repeated_enumType_codec);
			num += this.service_.CalculateSize(FileDescriptorProto._repeated_service_codec);
			num += this.extension_.CalculateSize(FileDescriptorProto._repeated_extension_codec);
			if (this.options_ != null)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(this.Options);
			}
			if (this.sourceCodeInfo_ != null)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(this.SourceCodeInfo);
			}
			if (this.HasSyntax)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.Syntax);
			}
			if (this._unknownFields != null)
			{
				num += this._unknownFields.CalculateSize();
			}
			return num;
		}

		// Token: 0x060004A4 RID: 1188 RVA: 0x00012C3C File Offset: 0x00010E3C
		[DebuggerNonUserCode]
		public void MergeFrom(FileDescriptorProto other)
		{
			if (other == null)
			{
				return;
			}
			if (other.HasName)
			{
				this.Name = other.Name;
			}
			if (other.HasPackage)
			{
				this.Package = other.Package;
			}
			this.dependency_.Add(other.dependency_);
			this.publicDependency_.Add(other.publicDependency_);
			this.weakDependency_.Add(other.weakDependency_);
			this.messageType_.Add(other.messageType_);
			this.enumType_.Add(other.enumType_);
			this.service_.Add(other.service_);
			this.extension_.Add(other.extension_);
			if (other.options_ != null)
			{
				if (this.options_ == null)
				{
					this.Options = new FileOptions();
				}
				this.Options.MergeFrom(other.Options);
			}
			if (other.sourceCodeInfo_ != null)
			{
				if (this.sourceCodeInfo_ == null)
				{
					this.SourceCodeInfo = new SourceCodeInfo();
				}
				this.SourceCodeInfo.MergeFrom(other.SourceCodeInfo);
			}
			if (other.HasSyntax)
			{
				this.Syntax = other.Syntax;
			}
			this._unknownFields = UnknownFieldSet.MergeFrom(this._unknownFields, other._unknownFields);
		}

		// Token: 0x060004A5 RID: 1189 RVA: 0x00012D70 File Offset: 0x00010F70
		[DebuggerNonUserCode]
		public void MergeFrom(CodedInputStream input)
		{
			uint num;
			while ((num = input.ReadTag()) != 0U)
			{
				if (num > 58U)
				{
					if (num > 80U)
					{
						if (num <= 88U)
						{
							if (num == 82U)
							{
								goto IL_017E;
							}
							if (num != 88U)
							{
								goto IL_0098;
							}
						}
						else if (num != 90U)
						{
							if (num != 98U)
							{
								goto IL_0098;
							}
							this.Syntax = input.ReadString();
							continue;
						}
						this.weakDependency_.AddEntriesFrom(input, FileDescriptorProto._repeated_weakDependency_codec);
						continue;
					}
					if (num == 66U)
					{
						if (this.options_ == null)
						{
							this.Options = new FileOptions();
						}
						input.ReadMessage(this.Options);
						continue;
					}
					if (num == 74U)
					{
						if (this.sourceCodeInfo_ == null)
						{
							this.SourceCodeInfo = new SourceCodeInfo();
						}
						input.ReadMessage(this.SourceCodeInfo);
						continue;
					}
					if (num != 80U)
					{
						goto IL_0098;
					}
					IL_017E:
					this.publicDependency_.AddEntriesFrom(input, FileDescriptorProto._repeated_publicDependency_codec);
					continue;
				}
				if (num <= 26U)
				{
					if (num == 10U)
					{
						this.Name = input.ReadString();
						continue;
					}
					if (num == 18U)
					{
						this.Package = input.ReadString();
						continue;
					}
					if (num == 26U)
					{
						this.dependency_.AddEntriesFrom(input, FileDescriptorProto._repeated_dependency_codec);
						continue;
					}
				}
				else if (num <= 42U)
				{
					if (num == 34U)
					{
						this.messageType_.AddEntriesFrom(input, FileDescriptorProto._repeated_messageType_codec);
						continue;
					}
					if (num == 42U)
					{
						this.enumType_.AddEntriesFrom(input, FileDescriptorProto._repeated_enumType_codec);
						continue;
					}
				}
				else
				{
					if (num == 50U)
					{
						this.service_.AddEntriesFrom(input, FileDescriptorProto._repeated_service_codec);
						continue;
					}
					if (num == 58U)
					{
						this.extension_.AddEntriesFrom(input, FileDescriptorProto._repeated_extension_codec);
						continue;
					}
				}
				IL_0098:
				this._unknownFields = UnknownFieldSet.MergeFieldFrom(this._unknownFields, input);
			}
		}

		// Token: 0x04000158 RID: 344
		private static readonly MessageParser<FileDescriptorProto> _parser = new MessageParser<FileDescriptorProto>(() => new FileDescriptorProto());

		// Token: 0x04000159 RID: 345
		private UnknownFieldSet _unknownFields;

		// Token: 0x0400015A RID: 346
		public const int NameFieldNumber = 1;

		// Token: 0x0400015B RID: 347
		private static readonly string NameDefaultValue = "";

		// Token: 0x0400015C RID: 348
		private string name_;

		// Token: 0x0400015D RID: 349
		public const int PackageFieldNumber = 2;

		// Token: 0x0400015E RID: 350
		private static readonly string PackageDefaultValue = "";

		// Token: 0x0400015F RID: 351
		private string package_;

		// Token: 0x04000160 RID: 352
		public const int DependencyFieldNumber = 3;

		// Token: 0x04000161 RID: 353
		private static readonly FieldCodec<string> _repeated_dependency_codec = FieldCodec.ForString(26U);

		// Token: 0x04000162 RID: 354
		private readonly RepeatedField<string> dependency_ = new RepeatedField<string>();

		// Token: 0x04000163 RID: 355
		public const int PublicDependencyFieldNumber = 10;

		// Token: 0x04000164 RID: 356
		private static readonly FieldCodec<int> _repeated_publicDependency_codec = FieldCodec.ForInt32(80U);

		// Token: 0x04000165 RID: 357
		private readonly RepeatedField<int> publicDependency_ = new RepeatedField<int>();

		// Token: 0x04000166 RID: 358
		public const int WeakDependencyFieldNumber = 11;

		// Token: 0x04000167 RID: 359
		private static readonly FieldCodec<int> _repeated_weakDependency_codec = FieldCodec.ForInt32(88U);

		// Token: 0x04000168 RID: 360
		private readonly RepeatedField<int> weakDependency_ = new RepeatedField<int>();

		// Token: 0x04000169 RID: 361
		public const int MessageTypeFieldNumber = 4;

		// Token: 0x0400016A RID: 362
		private static readonly FieldCodec<DescriptorProto> _repeated_messageType_codec = FieldCodec.ForMessage<DescriptorProto>(34U, DescriptorProto.Parser);

		// Token: 0x0400016B RID: 363
		private readonly RepeatedField<DescriptorProto> messageType_ = new RepeatedField<DescriptorProto>();

		// Token: 0x0400016C RID: 364
		public const int EnumTypeFieldNumber = 5;

		// Token: 0x0400016D RID: 365
		private static readonly FieldCodec<EnumDescriptorProto> _repeated_enumType_codec = FieldCodec.ForMessage<EnumDescriptorProto>(42U, EnumDescriptorProto.Parser);

		// Token: 0x0400016E RID: 366
		private readonly RepeatedField<EnumDescriptorProto> enumType_ = new RepeatedField<EnumDescriptorProto>();

		// Token: 0x0400016F RID: 367
		public const int ServiceFieldNumber = 6;

		// Token: 0x04000170 RID: 368
		private static readonly FieldCodec<ServiceDescriptorProto> _repeated_service_codec = FieldCodec.ForMessage<ServiceDescriptorProto>(50U, ServiceDescriptorProto.Parser);

		// Token: 0x04000171 RID: 369
		private readonly RepeatedField<ServiceDescriptorProto> service_ = new RepeatedField<ServiceDescriptorProto>();

		// Token: 0x04000172 RID: 370
		public const int ExtensionFieldNumber = 7;

		// Token: 0x04000173 RID: 371
		private static readonly FieldCodec<FieldDescriptorProto> _repeated_extension_codec = FieldCodec.ForMessage<FieldDescriptorProto>(58U, FieldDescriptorProto.Parser);

		// Token: 0x04000174 RID: 372
		private readonly RepeatedField<FieldDescriptorProto> extension_ = new RepeatedField<FieldDescriptorProto>();

		// Token: 0x04000175 RID: 373
		public const int OptionsFieldNumber = 8;

		// Token: 0x04000176 RID: 374
		private FileOptions options_;

		// Token: 0x04000177 RID: 375
		public const int SourceCodeInfoFieldNumber = 9;

		// Token: 0x04000178 RID: 376
		private SourceCodeInfo sourceCodeInfo_;

		// Token: 0x04000179 RID: 377
		public const int SyntaxFieldNumber = 12;

		// Token: 0x0400017A RID: 378
		private static readonly string SyntaxDefaultValue = "";

		// Token: 0x0400017B RID: 379
		private string syntax_;
	}
}
