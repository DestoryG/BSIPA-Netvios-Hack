using System;
using System.Diagnostics;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

namespace Google.Protobuf.WellKnownTypes
{
	// Token: 0x0200003F RID: 63
	public sealed class Type : IMessage<Type>, IMessage, IEquatable<Type>, IDeepCloneable<Type>
	{
		// Token: 0x1700008A RID: 138
		// (get) Token: 0x06000349 RID: 841 RVA: 0x0000E536 File Offset: 0x0000C736
		[DebuggerNonUserCode]
		public static MessageParser<Type> Parser
		{
			get
			{
				return Type._parser;
			}
		}

		// Token: 0x1700008B RID: 139
		// (get) Token: 0x0600034A RID: 842 RVA: 0x0000E53D File Offset: 0x0000C73D
		[DebuggerNonUserCode]
		public static MessageDescriptor Descriptor
		{
			get
			{
				return TypeReflection.Descriptor.MessageTypes[0];
			}
		}

		// Token: 0x1700008C RID: 140
		// (get) Token: 0x0600034B RID: 843 RVA: 0x0000E54F File Offset: 0x0000C74F
		[DebuggerNonUserCode]
		MessageDescriptor IMessage.Descriptor
		{
			get
			{
				return Type.Descriptor;
			}
		}

		// Token: 0x0600034C RID: 844 RVA: 0x0000E556 File Offset: 0x0000C756
		[DebuggerNonUserCode]
		public Type()
		{
		}

		// Token: 0x0600034D RID: 845 RVA: 0x0000E58C File Offset: 0x0000C78C
		[DebuggerNonUserCode]
		public Type(Type other)
			: this()
		{
			this.name_ = other.name_;
			this.fields_ = other.fields_.Clone();
			this.oneofs_ = other.oneofs_.Clone();
			this.options_ = other.options_.Clone();
			this.sourceContext_ = ((other.sourceContext_ != null) ? other.sourceContext_.Clone() : null);
			this.syntax_ = other.syntax_;
			this._unknownFields = UnknownFieldSet.Clone(other._unknownFields);
		}

		// Token: 0x0600034E RID: 846 RVA: 0x0000E617 File Offset: 0x0000C817
		[DebuggerNonUserCode]
		public Type Clone()
		{
			return new Type(this);
		}

		// Token: 0x1700008D RID: 141
		// (get) Token: 0x0600034F RID: 847 RVA: 0x0000E61F File Offset: 0x0000C81F
		// (set) Token: 0x06000350 RID: 848 RVA: 0x0000E627 File Offset: 0x0000C827
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

		// Token: 0x1700008E RID: 142
		// (get) Token: 0x06000351 RID: 849 RVA: 0x0000E63A File Offset: 0x0000C83A
		[DebuggerNonUserCode]
		public RepeatedField<Field> Fields
		{
			get
			{
				return this.fields_;
			}
		}

		// Token: 0x1700008F RID: 143
		// (get) Token: 0x06000352 RID: 850 RVA: 0x0000E642 File Offset: 0x0000C842
		[DebuggerNonUserCode]
		public RepeatedField<string> Oneofs
		{
			get
			{
				return this.oneofs_;
			}
		}

		// Token: 0x17000090 RID: 144
		// (get) Token: 0x06000353 RID: 851 RVA: 0x0000E64A File Offset: 0x0000C84A
		[DebuggerNonUserCode]
		public RepeatedField<Option> Options
		{
			get
			{
				return this.options_;
			}
		}

		// Token: 0x17000091 RID: 145
		// (get) Token: 0x06000354 RID: 852 RVA: 0x0000E652 File Offset: 0x0000C852
		// (set) Token: 0x06000355 RID: 853 RVA: 0x0000E65A File Offset: 0x0000C85A
		[DebuggerNonUserCode]
		public SourceContext SourceContext
		{
			get
			{
				return this.sourceContext_;
			}
			set
			{
				this.sourceContext_ = value;
			}
		}

		// Token: 0x17000092 RID: 146
		// (get) Token: 0x06000356 RID: 854 RVA: 0x0000E663 File Offset: 0x0000C863
		// (set) Token: 0x06000357 RID: 855 RVA: 0x0000E66B File Offset: 0x0000C86B
		[DebuggerNonUserCode]
		public Syntax Syntax
		{
			get
			{
				return this.syntax_;
			}
			set
			{
				this.syntax_ = value;
			}
		}

		// Token: 0x06000358 RID: 856 RVA: 0x0000E674 File Offset: 0x0000C874
		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return this.Equals(other as Type);
		}

		// Token: 0x06000359 RID: 857 RVA: 0x0000E684 File Offset: 0x0000C884
		[DebuggerNonUserCode]
		public bool Equals(Type other)
		{
			return other != null && (other == this || (!(this.Name != other.Name) && this.fields_.Equals(other.fields_) && this.oneofs_.Equals(other.oneofs_) && this.options_.Equals(other.options_) && object.Equals(this.SourceContext, other.SourceContext) && this.Syntax == other.Syntax && object.Equals(this._unknownFields, other._unknownFields)));
		}

		// Token: 0x0600035A RID: 858 RVA: 0x0000E728 File Offset: 0x0000C928
		[DebuggerNonUserCode]
		public override int GetHashCode()
		{
			int num = 1;
			if (this.Name.Length != 0)
			{
				num ^= this.Name.GetHashCode();
			}
			num ^= this.fields_.GetHashCode();
			num ^= this.oneofs_.GetHashCode();
			num ^= this.options_.GetHashCode();
			if (this.sourceContext_ != null)
			{
				num ^= this.SourceContext.GetHashCode();
			}
			if (this.Syntax != Syntax.Proto2)
			{
				num ^= this.Syntax.GetHashCode();
			}
			if (this._unknownFields != null)
			{
				num ^= this._unknownFields.GetHashCode();
			}
			return num;
		}

		// Token: 0x0600035B RID: 859 RVA: 0x0000E7C8 File Offset: 0x0000C9C8
		[DebuggerNonUserCode]
		public override string ToString()
		{
			return JsonFormatter.ToDiagnosticString(this);
		}

		// Token: 0x0600035C RID: 860 RVA: 0x0000E7D0 File Offset: 0x0000C9D0
		[DebuggerNonUserCode]
		public void WriteTo(CodedOutputStream output)
		{
			if (this.Name.Length != 0)
			{
				output.WriteRawTag(10);
				output.WriteString(this.Name);
			}
			this.fields_.WriteTo(output, Type._repeated_fields_codec);
			this.oneofs_.WriteTo(output, Type._repeated_oneofs_codec);
			this.options_.WriteTo(output, Type._repeated_options_codec);
			if (this.sourceContext_ != null)
			{
				output.WriteRawTag(42);
				output.WriteMessage(this.SourceContext);
			}
			if (this.Syntax != Syntax.Proto2)
			{
				output.WriteRawTag(48);
				output.WriteEnum((int)this.Syntax);
			}
			if (this._unknownFields != null)
			{
				this._unknownFields.WriteTo(output);
			}
		}

		// Token: 0x0600035D RID: 861 RVA: 0x0000E880 File Offset: 0x0000CA80
		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (this.Name.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.Name);
			}
			num += this.fields_.CalculateSize(Type._repeated_fields_codec);
			num += this.oneofs_.CalculateSize(Type._repeated_oneofs_codec);
			num += this.options_.CalculateSize(Type._repeated_options_codec);
			if (this.sourceContext_ != null)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(this.SourceContext);
			}
			if (this.Syntax != Syntax.Proto2)
			{
				num += 1 + CodedOutputStream.ComputeEnumSize((int)this.Syntax);
			}
			if (this._unknownFields != null)
			{
				num += this._unknownFields.CalculateSize();
			}
			return num;
		}

		// Token: 0x0600035E RID: 862 RVA: 0x0000E92C File Offset: 0x0000CB2C
		[DebuggerNonUserCode]
		public void MergeFrom(Type other)
		{
			if (other == null)
			{
				return;
			}
			if (other.Name.Length != 0)
			{
				this.Name = other.Name;
			}
			this.fields_.Add(other.fields_);
			this.oneofs_.Add(other.oneofs_);
			this.options_.Add(other.options_);
			if (other.sourceContext_ != null)
			{
				if (this.sourceContext_ == null)
				{
					this.SourceContext = new SourceContext();
				}
				this.SourceContext.MergeFrom(other.SourceContext);
			}
			if (other.Syntax != Syntax.Proto2)
			{
				this.Syntax = other.Syntax;
			}
			this._unknownFields = UnknownFieldSet.MergeFrom(this._unknownFields, other._unknownFields);
		}

		// Token: 0x0600035F RID: 863 RVA: 0x0000E9E0 File Offset: 0x0000CBE0
		[DebuggerNonUserCode]
		public void MergeFrom(CodedInputStream input)
		{
			uint num;
			while ((num = input.ReadTag()) != 0U)
			{
				if (num <= 26U)
				{
					if (num == 10U)
					{
						this.Name = input.ReadString();
						continue;
					}
					if (num == 18U)
					{
						this.fields_.AddEntriesFrom(input, Type._repeated_fields_codec);
						continue;
					}
					if (num == 26U)
					{
						this.oneofs_.AddEntriesFrom(input, Type._repeated_oneofs_codec);
						continue;
					}
				}
				else
				{
					if (num == 34U)
					{
						this.options_.AddEntriesFrom(input, Type._repeated_options_codec);
						continue;
					}
					if (num == 42U)
					{
						if (this.sourceContext_ == null)
						{
							this.SourceContext = new SourceContext();
						}
						input.ReadMessage(this.SourceContext);
						continue;
					}
					if (num == 48U)
					{
						this.Syntax = (Syntax)input.ReadEnum();
						continue;
					}
				}
				this._unknownFields = UnknownFieldSet.MergeFieldFrom(this._unknownFields, input);
			}
		}

		// Token: 0x040000E5 RID: 229
		private static readonly MessageParser<Type> _parser = new MessageParser<Type>(() => new Type());

		// Token: 0x040000E6 RID: 230
		private UnknownFieldSet _unknownFields;

		// Token: 0x040000E7 RID: 231
		public const int NameFieldNumber = 1;

		// Token: 0x040000E8 RID: 232
		private string name_ = "";

		// Token: 0x040000E9 RID: 233
		public const int FieldsFieldNumber = 2;

		// Token: 0x040000EA RID: 234
		private static readonly FieldCodec<Field> _repeated_fields_codec = FieldCodec.ForMessage<Field>(18U, Field.Parser);

		// Token: 0x040000EB RID: 235
		private readonly RepeatedField<Field> fields_ = new RepeatedField<Field>();

		// Token: 0x040000EC RID: 236
		public const int OneofsFieldNumber = 3;

		// Token: 0x040000ED RID: 237
		private static readonly FieldCodec<string> _repeated_oneofs_codec = FieldCodec.ForString(26U);

		// Token: 0x040000EE RID: 238
		private readonly RepeatedField<string> oneofs_ = new RepeatedField<string>();

		// Token: 0x040000EF RID: 239
		public const int OptionsFieldNumber = 4;

		// Token: 0x040000F0 RID: 240
		private static readonly FieldCodec<Option> _repeated_options_codec = FieldCodec.ForMessage<Option>(34U, Option.Parser);

		// Token: 0x040000F1 RID: 241
		private readonly RepeatedField<Option> options_ = new RepeatedField<Option>();

		// Token: 0x040000F2 RID: 242
		public const int SourceContextFieldNumber = 5;

		// Token: 0x040000F3 RID: 243
		private SourceContext sourceContext_;

		// Token: 0x040000F4 RID: 244
		public const int SyntaxFieldNumber = 6;

		// Token: 0x040000F5 RID: 245
		private Syntax syntax_;
	}
}
