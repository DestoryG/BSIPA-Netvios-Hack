using System;
using System.Diagnostics;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

namespace Google.Protobuf.WellKnownTypes
{
	// Token: 0x0200002A RID: 42
	public sealed class Api : IMessage<Api>, IMessage, IEquatable<Api>, IDeepCloneable<Api>
	{
		// Token: 0x17000043 RID: 67
		// (get) Token: 0x06000229 RID: 553 RVA: 0x0000AC7A File Offset: 0x00008E7A
		[DebuggerNonUserCode]
		public static MessageParser<Api> Parser
		{
			get
			{
				return Api._parser;
			}
		}

		// Token: 0x17000044 RID: 68
		// (get) Token: 0x0600022A RID: 554 RVA: 0x0000AC81 File Offset: 0x00008E81
		[DebuggerNonUserCode]
		public static MessageDescriptor Descriptor
		{
			get
			{
				return ApiReflection.Descriptor.MessageTypes[0];
			}
		}

		// Token: 0x17000045 RID: 69
		// (get) Token: 0x0600022B RID: 555 RVA: 0x0000AC93 File Offset: 0x00008E93
		[DebuggerNonUserCode]
		MessageDescriptor IMessage.Descriptor
		{
			get
			{
				return Api.Descriptor;
			}
		}

		// Token: 0x0600022C RID: 556 RVA: 0x0000AC9A File Offset: 0x00008E9A
		[DebuggerNonUserCode]
		public Api()
		{
		}

		// Token: 0x0600022D RID: 557 RVA: 0x0000ACDC File Offset: 0x00008EDC
		[DebuggerNonUserCode]
		public Api(Api other)
			: this()
		{
			this.name_ = other.name_;
			this.methods_ = other.methods_.Clone();
			this.options_ = other.options_.Clone();
			this.version_ = other.version_;
			this.sourceContext_ = ((other.sourceContext_ != null) ? other.sourceContext_.Clone() : null);
			this.mixins_ = other.mixins_.Clone();
			this.syntax_ = other.syntax_;
			this._unknownFields = UnknownFieldSet.Clone(other._unknownFields);
		}

		// Token: 0x0600022E RID: 558 RVA: 0x0000AD73 File Offset: 0x00008F73
		[DebuggerNonUserCode]
		public Api Clone()
		{
			return new Api(this);
		}

		// Token: 0x17000046 RID: 70
		// (get) Token: 0x0600022F RID: 559 RVA: 0x0000AD7B File Offset: 0x00008F7B
		// (set) Token: 0x06000230 RID: 560 RVA: 0x0000AD83 File Offset: 0x00008F83
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

		// Token: 0x17000047 RID: 71
		// (get) Token: 0x06000231 RID: 561 RVA: 0x0000AD96 File Offset: 0x00008F96
		[DebuggerNonUserCode]
		public RepeatedField<Method> Methods
		{
			get
			{
				return this.methods_;
			}
		}

		// Token: 0x17000048 RID: 72
		// (get) Token: 0x06000232 RID: 562 RVA: 0x0000AD9E File Offset: 0x00008F9E
		[DebuggerNonUserCode]
		public RepeatedField<Option> Options
		{
			get
			{
				return this.options_;
			}
		}

		// Token: 0x17000049 RID: 73
		// (get) Token: 0x06000233 RID: 563 RVA: 0x0000ADA6 File Offset: 0x00008FA6
		// (set) Token: 0x06000234 RID: 564 RVA: 0x0000ADAE File Offset: 0x00008FAE
		[DebuggerNonUserCode]
		public string Version
		{
			get
			{
				return this.version_;
			}
			set
			{
				this.version_ = ProtoPreconditions.CheckNotNull<string>(value, "value");
			}
		}

		// Token: 0x1700004A RID: 74
		// (get) Token: 0x06000235 RID: 565 RVA: 0x0000ADC1 File Offset: 0x00008FC1
		// (set) Token: 0x06000236 RID: 566 RVA: 0x0000ADC9 File Offset: 0x00008FC9
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

		// Token: 0x1700004B RID: 75
		// (get) Token: 0x06000237 RID: 567 RVA: 0x0000ADD2 File Offset: 0x00008FD2
		[DebuggerNonUserCode]
		public RepeatedField<Mixin> Mixins
		{
			get
			{
				return this.mixins_;
			}
		}

		// Token: 0x1700004C RID: 76
		// (get) Token: 0x06000238 RID: 568 RVA: 0x0000ADDA File Offset: 0x00008FDA
		// (set) Token: 0x06000239 RID: 569 RVA: 0x0000ADE2 File Offset: 0x00008FE2
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

		// Token: 0x0600023A RID: 570 RVA: 0x0000ADEB File Offset: 0x00008FEB
		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return this.Equals(other as Api);
		}

		// Token: 0x0600023B RID: 571 RVA: 0x0000ADFC File Offset: 0x00008FFC
		[DebuggerNonUserCode]
		public bool Equals(Api other)
		{
			return other != null && (other == this || (!(this.Name != other.Name) && this.methods_.Equals(other.methods_) && this.options_.Equals(other.options_) && !(this.Version != other.Version) && object.Equals(this.SourceContext, other.SourceContext) && this.mixins_.Equals(other.mixins_) && this.Syntax == other.Syntax && object.Equals(this._unknownFields, other._unknownFields)));
		}

		// Token: 0x0600023C RID: 572 RVA: 0x0000AEB4 File Offset: 0x000090B4
		[DebuggerNonUserCode]
		public override int GetHashCode()
		{
			int num = 1;
			if (this.Name.Length != 0)
			{
				num ^= this.Name.GetHashCode();
			}
			num ^= this.methods_.GetHashCode();
			num ^= this.options_.GetHashCode();
			if (this.Version.Length != 0)
			{
				num ^= this.Version.GetHashCode();
			}
			if (this.sourceContext_ != null)
			{
				num ^= this.SourceContext.GetHashCode();
			}
			num ^= this.mixins_.GetHashCode();
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

		// Token: 0x0600023D RID: 573 RVA: 0x0000AF6F File Offset: 0x0000916F
		[DebuggerNonUserCode]
		public override string ToString()
		{
			return JsonFormatter.ToDiagnosticString(this);
		}

		// Token: 0x0600023E RID: 574 RVA: 0x0000AF78 File Offset: 0x00009178
		[DebuggerNonUserCode]
		public void WriteTo(CodedOutputStream output)
		{
			if (this.Name.Length != 0)
			{
				output.WriteRawTag(10);
				output.WriteString(this.Name);
			}
			this.methods_.WriteTo(output, Api._repeated_methods_codec);
			this.options_.WriteTo(output, Api._repeated_options_codec);
			if (this.Version.Length != 0)
			{
				output.WriteRawTag(34);
				output.WriteString(this.Version);
			}
			if (this.sourceContext_ != null)
			{
				output.WriteRawTag(42);
				output.WriteMessage(this.SourceContext);
			}
			this.mixins_.WriteTo(output, Api._repeated_mixins_codec);
			if (this.Syntax != Syntax.Proto2)
			{
				output.WriteRawTag(56);
				output.WriteEnum((int)this.Syntax);
			}
			if (this._unknownFields != null)
			{
				this._unknownFields.WriteTo(output);
			}
		}

		// Token: 0x0600023F RID: 575 RVA: 0x0000B048 File Offset: 0x00009248
		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (this.Name.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.Name);
			}
			num += this.methods_.CalculateSize(Api._repeated_methods_codec);
			num += this.options_.CalculateSize(Api._repeated_options_codec);
			if (this.Version.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.Version);
			}
			if (this.sourceContext_ != null)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(this.SourceContext);
			}
			num += this.mixins_.CalculateSize(Api._repeated_mixins_codec);
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

		// Token: 0x06000240 RID: 576 RVA: 0x0000B114 File Offset: 0x00009314
		[DebuggerNonUserCode]
		public void MergeFrom(Api other)
		{
			if (other == null)
			{
				return;
			}
			if (other.Name.Length != 0)
			{
				this.Name = other.Name;
			}
			this.methods_.Add(other.methods_);
			this.options_.Add(other.options_);
			if (other.Version.Length != 0)
			{
				this.Version = other.Version;
			}
			if (other.sourceContext_ != null)
			{
				if (this.sourceContext_ == null)
				{
					this.SourceContext = new SourceContext();
				}
				this.SourceContext.MergeFrom(other.SourceContext);
			}
			this.mixins_.Add(other.mixins_);
			if (other.Syntax != Syntax.Proto2)
			{
				this.Syntax = other.Syntax;
			}
			this._unknownFields = UnknownFieldSet.MergeFrom(this._unknownFields, other._unknownFields);
		}

		// Token: 0x06000241 RID: 577 RVA: 0x0000B1E4 File Offset: 0x000093E4
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
						this.methods_.AddEntriesFrom(input, Api._repeated_methods_codec);
						continue;
					}
					if (num == 26U)
					{
						this.options_.AddEntriesFrom(input, Api._repeated_options_codec);
						continue;
					}
				}
				else if (num <= 42U)
				{
					if (num == 34U)
					{
						this.Version = input.ReadString();
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
				}
				else
				{
					if (num == 50U)
					{
						this.mixins_.AddEntriesFrom(input, Api._repeated_mixins_codec);
						continue;
					}
					if (num == 56U)
					{
						this.Syntax = (Syntax)input.ReadEnum();
						continue;
					}
				}
				this._unknownFields = UnknownFieldSet.MergeFieldFrom(this._unknownFields, input);
			}
		}

		// Token: 0x04000077 RID: 119
		private static readonly MessageParser<Api> _parser = new MessageParser<Api>(() => new Api());

		// Token: 0x04000078 RID: 120
		private UnknownFieldSet _unknownFields;

		// Token: 0x04000079 RID: 121
		public const int NameFieldNumber = 1;

		// Token: 0x0400007A RID: 122
		private string name_ = "";

		// Token: 0x0400007B RID: 123
		public const int MethodsFieldNumber = 2;

		// Token: 0x0400007C RID: 124
		private static readonly FieldCodec<Method> _repeated_methods_codec = FieldCodec.ForMessage<Method>(18U, Method.Parser);

		// Token: 0x0400007D RID: 125
		private readonly RepeatedField<Method> methods_ = new RepeatedField<Method>();

		// Token: 0x0400007E RID: 126
		public const int OptionsFieldNumber = 3;

		// Token: 0x0400007F RID: 127
		private static readonly FieldCodec<Option> _repeated_options_codec = FieldCodec.ForMessage<Option>(26U, Option.Parser);

		// Token: 0x04000080 RID: 128
		private readonly RepeatedField<Option> options_ = new RepeatedField<Option>();

		// Token: 0x04000081 RID: 129
		public const int VersionFieldNumber = 4;

		// Token: 0x04000082 RID: 130
		private string version_ = "";

		// Token: 0x04000083 RID: 131
		public const int SourceContextFieldNumber = 5;

		// Token: 0x04000084 RID: 132
		private SourceContext sourceContext_;

		// Token: 0x04000085 RID: 133
		public const int MixinsFieldNumber = 6;

		// Token: 0x04000086 RID: 134
		private static readonly FieldCodec<Mixin> _repeated_mixins_codec = FieldCodec.ForMessage<Mixin>(50U, Mixin.Parser);

		// Token: 0x04000087 RID: 135
		private readonly RepeatedField<Mixin> mixins_ = new RepeatedField<Mixin>();

		// Token: 0x04000088 RID: 136
		public const int SyntaxFieldNumber = 7;

		// Token: 0x04000089 RID: 137
		private Syntax syntax_;
	}
}
