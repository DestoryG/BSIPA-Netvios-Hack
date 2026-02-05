using System;
using System.Diagnostics;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

namespace Google.Protobuf.WellKnownTypes
{
	// Token: 0x02000041 RID: 65
	public sealed class Enum : IMessage<Enum>, IMessage, IEquatable<Enum>, IDeepCloneable<Enum>
	{
		// Token: 0x170000A0 RID: 160
		// (get) Token: 0x06000383 RID: 899 RVA: 0x0000F3A9 File Offset: 0x0000D5A9
		[DebuggerNonUserCode]
		public static MessageParser<Enum> Parser
		{
			get
			{
				return Enum._parser;
			}
		}

		// Token: 0x170000A1 RID: 161
		// (get) Token: 0x06000384 RID: 900 RVA: 0x0000F3B0 File Offset: 0x0000D5B0
		[DebuggerNonUserCode]
		public static MessageDescriptor Descriptor
		{
			get
			{
				return TypeReflection.Descriptor.MessageTypes[2];
			}
		}

		// Token: 0x170000A2 RID: 162
		// (get) Token: 0x06000385 RID: 901 RVA: 0x0000F3C2 File Offset: 0x0000D5C2
		[DebuggerNonUserCode]
		MessageDescriptor IMessage.Descriptor
		{
			get
			{
				return Enum.Descriptor;
			}
		}

		// Token: 0x06000386 RID: 902 RVA: 0x0000F3C9 File Offset: 0x0000D5C9
		[DebuggerNonUserCode]
		public Enum()
		{
		}

		// Token: 0x06000387 RID: 903 RVA: 0x0000F3F4 File Offset: 0x0000D5F4
		[DebuggerNonUserCode]
		public Enum(Enum other)
			: this()
		{
			this.name_ = other.name_;
			this.enumvalue_ = other.enumvalue_.Clone();
			this.options_ = other.options_.Clone();
			this.sourceContext_ = ((other.sourceContext_ != null) ? other.sourceContext_.Clone() : null);
			this.syntax_ = other.syntax_;
			this._unknownFields = UnknownFieldSet.Clone(other._unknownFields);
		}

		// Token: 0x06000388 RID: 904 RVA: 0x0000F46E File Offset: 0x0000D66E
		[DebuggerNonUserCode]
		public Enum Clone()
		{
			return new Enum(this);
		}

		// Token: 0x170000A3 RID: 163
		// (get) Token: 0x06000389 RID: 905 RVA: 0x0000F476 File Offset: 0x0000D676
		// (set) Token: 0x0600038A RID: 906 RVA: 0x0000F47E File Offset: 0x0000D67E
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

		// Token: 0x170000A4 RID: 164
		// (get) Token: 0x0600038B RID: 907 RVA: 0x0000F491 File Offset: 0x0000D691
		[DebuggerNonUserCode]
		public RepeatedField<EnumValue> Enumvalue
		{
			get
			{
				return this.enumvalue_;
			}
		}

		// Token: 0x170000A5 RID: 165
		// (get) Token: 0x0600038C RID: 908 RVA: 0x0000F499 File Offset: 0x0000D699
		[DebuggerNonUserCode]
		public RepeatedField<Option> Options
		{
			get
			{
				return this.options_;
			}
		}

		// Token: 0x170000A6 RID: 166
		// (get) Token: 0x0600038D RID: 909 RVA: 0x0000F4A1 File Offset: 0x0000D6A1
		// (set) Token: 0x0600038E RID: 910 RVA: 0x0000F4A9 File Offset: 0x0000D6A9
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

		// Token: 0x170000A7 RID: 167
		// (get) Token: 0x0600038F RID: 911 RVA: 0x0000F4B2 File Offset: 0x0000D6B2
		// (set) Token: 0x06000390 RID: 912 RVA: 0x0000F4BA File Offset: 0x0000D6BA
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

		// Token: 0x06000391 RID: 913 RVA: 0x0000F4C3 File Offset: 0x0000D6C3
		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return this.Equals(other as Enum);
		}

		// Token: 0x06000392 RID: 914 RVA: 0x0000F4D4 File Offset: 0x0000D6D4
		[DebuggerNonUserCode]
		public bool Equals(Enum other)
		{
			return other != null && (other == this || (!(this.Name != other.Name) && this.enumvalue_.Equals(other.enumvalue_) && this.options_.Equals(other.options_) && object.Equals(this.SourceContext, other.SourceContext) && this.Syntax == other.Syntax && object.Equals(this._unknownFields, other._unknownFields)));
		}

		// Token: 0x06000393 RID: 915 RVA: 0x0000F564 File Offset: 0x0000D764
		[DebuggerNonUserCode]
		public override int GetHashCode()
		{
			int num = 1;
			if (this.Name.Length != 0)
			{
				num ^= this.Name.GetHashCode();
			}
			num ^= this.enumvalue_.GetHashCode();
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

		// Token: 0x06000394 RID: 916 RVA: 0x0000F5F6 File Offset: 0x0000D7F6
		[DebuggerNonUserCode]
		public override string ToString()
		{
			return JsonFormatter.ToDiagnosticString(this);
		}

		// Token: 0x06000395 RID: 917 RVA: 0x0000F600 File Offset: 0x0000D800
		[DebuggerNonUserCode]
		public void WriteTo(CodedOutputStream output)
		{
			if (this.Name.Length != 0)
			{
				output.WriteRawTag(10);
				output.WriteString(this.Name);
			}
			this.enumvalue_.WriteTo(output, Enum._repeated_enumvalue_codec);
			this.options_.WriteTo(output, Enum._repeated_options_codec);
			if (this.sourceContext_ != null)
			{
				output.WriteRawTag(34);
				output.WriteMessage(this.SourceContext);
			}
			if (this.Syntax != Syntax.Proto2)
			{
				output.WriteRawTag(40);
				output.WriteEnum((int)this.Syntax);
			}
			if (this._unknownFields != null)
			{
				this._unknownFields.WriteTo(output);
			}
		}

		// Token: 0x06000396 RID: 918 RVA: 0x0000F69C File Offset: 0x0000D89C
		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (this.Name.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.Name);
			}
			num += this.enumvalue_.CalculateSize(Enum._repeated_enumvalue_codec);
			num += this.options_.CalculateSize(Enum._repeated_options_codec);
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

		// Token: 0x06000397 RID: 919 RVA: 0x0000F738 File Offset: 0x0000D938
		[DebuggerNonUserCode]
		public void MergeFrom(Enum other)
		{
			if (other == null)
			{
				return;
			}
			if (other.Name.Length != 0)
			{
				this.Name = other.Name;
			}
			this.enumvalue_.Add(other.enumvalue_);
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

		// Token: 0x06000398 RID: 920 RVA: 0x0000F7DC File Offset: 0x0000D9DC
		[DebuggerNonUserCode]
		public void MergeFrom(CodedInputStream input)
		{
			uint num;
			while ((num = input.ReadTag()) != 0U)
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
						this.enumvalue_.AddEntriesFrom(input, Enum._repeated_enumvalue_codec);
						continue;
					}
				}
				else
				{
					if (num == 26U)
					{
						this.options_.AddEntriesFrom(input, Enum._repeated_options_codec);
						continue;
					}
					if (num == 34U)
					{
						if (this.sourceContext_ == null)
						{
							this.SourceContext = new SourceContext();
						}
						input.ReadMessage(this.SourceContext);
						continue;
					}
					if (num == 40U)
					{
						this.Syntax = (Syntax)input.ReadEnum();
						continue;
					}
				}
				this._unknownFields = UnknownFieldSet.MergeFieldFrom(this._unknownFields, input);
			}
		}

		// Token: 0x0400010D RID: 269
		private static readonly MessageParser<Enum> _parser = new MessageParser<Enum>(() => new Enum());

		// Token: 0x0400010E RID: 270
		private UnknownFieldSet _unknownFields;

		// Token: 0x0400010F RID: 271
		public const int NameFieldNumber = 1;

		// Token: 0x04000110 RID: 272
		private string name_ = "";

		// Token: 0x04000111 RID: 273
		public const int EnumvalueFieldNumber = 2;

		// Token: 0x04000112 RID: 274
		private static readonly FieldCodec<EnumValue> _repeated_enumvalue_codec = FieldCodec.ForMessage<EnumValue>(18U, EnumValue.Parser);

		// Token: 0x04000113 RID: 275
		private readonly RepeatedField<EnumValue> enumvalue_ = new RepeatedField<EnumValue>();

		// Token: 0x04000114 RID: 276
		public const int OptionsFieldNumber = 3;

		// Token: 0x04000115 RID: 277
		private static readonly FieldCodec<Option> _repeated_options_codec = FieldCodec.ForMessage<Option>(26U, Option.Parser);

		// Token: 0x04000116 RID: 278
		private readonly RepeatedField<Option> options_ = new RepeatedField<Option>();

		// Token: 0x04000117 RID: 279
		public const int SourceContextFieldNumber = 4;

		// Token: 0x04000118 RID: 280
		private SourceContext sourceContext_;

		// Token: 0x04000119 RID: 281
		public const int SyntaxFieldNumber = 5;

		// Token: 0x0400011A RID: 282
		private Syntax syntax_;
	}
}
