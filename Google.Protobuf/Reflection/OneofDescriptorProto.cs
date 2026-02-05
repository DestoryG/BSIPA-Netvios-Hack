using System;
using System.Diagnostics;

namespace Google.Protobuf.Reflection
{
	// Token: 0x02000055 RID: 85
	public sealed class OneofDescriptorProto : IMessage<OneofDescriptorProto>, IMessage, IEquatable<OneofDescriptorProto>, IDeepCloneable<OneofDescriptorProto>
	{
		// Token: 0x1700011A RID: 282
		// (get) Token: 0x06000515 RID: 1301 RVA: 0x000146C7 File Offset: 0x000128C7
		[DebuggerNonUserCode]
		public static MessageParser<OneofDescriptorProto> Parser
		{
			get
			{
				return OneofDescriptorProto._parser;
			}
		}

		// Token: 0x1700011B RID: 283
		// (get) Token: 0x06000516 RID: 1302 RVA: 0x000146CE File Offset: 0x000128CE
		[DebuggerNonUserCode]
		public static MessageDescriptor Descriptor
		{
			get
			{
				return DescriptorReflection.Descriptor.MessageTypes[5];
			}
		}

		// Token: 0x1700011C RID: 284
		// (get) Token: 0x06000517 RID: 1303 RVA: 0x000146E0 File Offset: 0x000128E0
		[DebuggerNonUserCode]
		MessageDescriptor IMessage.Descriptor
		{
			get
			{
				return OneofDescriptorProto.Descriptor;
			}
		}

		// Token: 0x06000518 RID: 1304 RVA: 0x000146E7 File Offset: 0x000128E7
		[DebuggerNonUserCode]
		public OneofDescriptorProto()
		{
		}

		// Token: 0x06000519 RID: 1305 RVA: 0x000146F0 File Offset: 0x000128F0
		[DebuggerNonUserCode]
		public OneofDescriptorProto(OneofDescriptorProto other)
			: this()
		{
			this.name_ = other.name_;
			this.options_ = ((other.options_ != null) ? other.options_.Clone() : null);
			this._unknownFields = UnknownFieldSet.Clone(other._unknownFields);
		}

		// Token: 0x0600051A RID: 1306 RVA: 0x0001473C File Offset: 0x0001293C
		[DebuggerNonUserCode]
		public OneofDescriptorProto Clone()
		{
			return new OneofDescriptorProto(this);
		}

		// Token: 0x1700011D RID: 285
		// (get) Token: 0x0600051B RID: 1307 RVA: 0x00014744 File Offset: 0x00012944
		// (set) Token: 0x0600051C RID: 1308 RVA: 0x00014755 File Offset: 0x00012955
		[DebuggerNonUserCode]
		public string Name
		{
			get
			{
				return this.name_ ?? OneofDescriptorProto.NameDefaultValue;
			}
			set
			{
				this.name_ = ProtoPreconditions.CheckNotNull<string>(value, "value");
			}
		}

		// Token: 0x1700011E RID: 286
		// (get) Token: 0x0600051D RID: 1309 RVA: 0x00014768 File Offset: 0x00012968
		[DebuggerNonUserCode]
		public bool HasName
		{
			get
			{
				return this.name_ != null;
			}
		}

		// Token: 0x0600051E RID: 1310 RVA: 0x00014773 File Offset: 0x00012973
		[DebuggerNonUserCode]
		public void ClearName()
		{
			this.name_ = null;
		}

		// Token: 0x1700011F RID: 287
		// (get) Token: 0x0600051F RID: 1311 RVA: 0x0001477C File Offset: 0x0001297C
		// (set) Token: 0x06000520 RID: 1312 RVA: 0x00014784 File Offset: 0x00012984
		[DebuggerNonUserCode]
		public OneofOptions Options
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

		// Token: 0x06000521 RID: 1313 RVA: 0x0001478D File Offset: 0x0001298D
		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return this.Equals(other as OneofDescriptorProto);
		}

		// Token: 0x06000522 RID: 1314 RVA: 0x0001479C File Offset: 0x0001299C
		[DebuggerNonUserCode]
		public bool Equals(OneofDescriptorProto other)
		{
			return other != null && (other == this || (!(this.Name != other.Name) && object.Equals(this.Options, other.Options) && object.Equals(this._unknownFields, other._unknownFields)));
		}

		// Token: 0x06000523 RID: 1315 RVA: 0x000147F0 File Offset: 0x000129F0
		[DebuggerNonUserCode]
		public override int GetHashCode()
		{
			int num = 1;
			if (this.HasName)
			{
				num ^= this.Name.GetHashCode();
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

		// Token: 0x06000524 RID: 1316 RVA: 0x00014842 File Offset: 0x00012A42
		[DebuggerNonUserCode]
		public override string ToString()
		{
			return JsonFormatter.ToDiagnosticString(this);
		}

		// Token: 0x06000525 RID: 1317 RVA: 0x0001484C File Offset: 0x00012A4C
		[DebuggerNonUserCode]
		public void WriteTo(CodedOutputStream output)
		{
			if (this.HasName)
			{
				output.WriteRawTag(10);
				output.WriteString(this.Name);
			}
			if (this.options_ != null)
			{
				output.WriteRawTag(18);
				output.WriteMessage(this.Options);
			}
			if (this._unknownFields != null)
			{
				this._unknownFields.WriteTo(output);
			}
		}

		// Token: 0x06000526 RID: 1318 RVA: 0x000148A8 File Offset: 0x00012AA8
		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (this.HasName)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.Name);
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

		// Token: 0x06000527 RID: 1319 RVA: 0x00014900 File Offset: 0x00012B00
		[DebuggerNonUserCode]
		public void MergeFrom(OneofDescriptorProto other)
		{
			if (other == null)
			{
				return;
			}
			if (other.HasName)
			{
				this.Name = other.Name;
			}
			if (other.options_ != null)
			{
				if (this.options_ == null)
				{
					this.Options = new OneofOptions();
				}
				this.Options.MergeFrom(other.Options);
			}
			this._unknownFields = UnknownFieldSet.MergeFrom(this._unknownFields, other._unknownFields);
		}

		// Token: 0x06000528 RID: 1320 RVA: 0x00014968 File Offset: 0x00012B68
		[DebuggerNonUserCode]
		public void MergeFrom(CodedInputStream input)
		{
			uint num;
			while ((num = input.ReadTag()) != 0U)
			{
				if (num != 10U)
				{
					if (num != 18U)
					{
						this._unknownFields = UnknownFieldSet.MergeFieldFrom(this._unknownFields, input);
					}
					else
					{
						if (this.options_ == null)
						{
							this.Options = new OneofOptions();
						}
						input.ReadMessage(this.Options);
					}
				}
				else
				{
					this.Name = input.ReadString();
				}
			}
		}

		// Token: 0x040001C4 RID: 452
		private static readonly MessageParser<OneofDescriptorProto> _parser = new MessageParser<OneofDescriptorProto>(() => new OneofDescriptorProto());

		// Token: 0x040001C5 RID: 453
		private UnknownFieldSet _unknownFields;

		// Token: 0x040001C6 RID: 454
		public const int NameFieldNumber = 1;

		// Token: 0x040001C7 RID: 455
		private static readonly string NameDefaultValue = "";

		// Token: 0x040001C8 RID: 456
		private string name_;

		// Token: 0x040001C9 RID: 457
		public const int OptionsFieldNumber = 2;

		// Token: 0x040001CA RID: 458
		private OneofOptions options_;
	}
}
