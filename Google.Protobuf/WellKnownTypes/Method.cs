using System;
using System.Diagnostics;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

namespace Google.Protobuf.WellKnownTypes
{
	// Token: 0x0200002B RID: 43
	public sealed class Method : IMessage<Method>, IMessage, IEquatable<Method>, IDeepCloneable<Method>
	{
		// Token: 0x1700004D RID: 77
		// (get) Token: 0x06000243 RID: 579 RVA: 0x0000B32E File Offset: 0x0000952E
		[DebuggerNonUserCode]
		public static MessageParser<Method> Parser
		{
			get
			{
				return Method._parser;
			}
		}

		// Token: 0x1700004E RID: 78
		// (get) Token: 0x06000244 RID: 580 RVA: 0x0000B335 File Offset: 0x00009535
		[DebuggerNonUserCode]
		public static MessageDescriptor Descriptor
		{
			get
			{
				return ApiReflection.Descriptor.MessageTypes[1];
			}
		}

		// Token: 0x1700004F RID: 79
		// (get) Token: 0x06000245 RID: 581 RVA: 0x0000B347 File Offset: 0x00009547
		[DebuggerNonUserCode]
		MessageDescriptor IMessage.Descriptor
		{
			get
			{
				return Method.Descriptor;
			}
		}

		// Token: 0x06000246 RID: 582 RVA: 0x0000B34E File Offset: 0x0000954E
		[DebuggerNonUserCode]
		public Method()
		{
		}

		// Token: 0x06000247 RID: 583 RVA: 0x0000B384 File Offset: 0x00009584
		[DebuggerNonUserCode]
		public Method(Method other)
			: this()
		{
			this.name_ = other.name_;
			this.requestTypeUrl_ = other.requestTypeUrl_;
			this.requestStreaming_ = other.requestStreaming_;
			this.responseTypeUrl_ = other.responseTypeUrl_;
			this.responseStreaming_ = other.responseStreaming_;
			this.options_ = other.options_.Clone();
			this.syntax_ = other.syntax_;
			this._unknownFields = UnknownFieldSet.Clone(other._unknownFields);
		}

		// Token: 0x06000248 RID: 584 RVA: 0x0000B401 File Offset: 0x00009601
		[DebuggerNonUserCode]
		public Method Clone()
		{
			return new Method(this);
		}

		// Token: 0x17000050 RID: 80
		// (get) Token: 0x06000249 RID: 585 RVA: 0x0000B409 File Offset: 0x00009609
		// (set) Token: 0x0600024A RID: 586 RVA: 0x0000B411 File Offset: 0x00009611
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

		// Token: 0x17000051 RID: 81
		// (get) Token: 0x0600024B RID: 587 RVA: 0x0000B424 File Offset: 0x00009624
		// (set) Token: 0x0600024C RID: 588 RVA: 0x0000B42C File Offset: 0x0000962C
		[DebuggerNonUserCode]
		public string RequestTypeUrl
		{
			get
			{
				return this.requestTypeUrl_;
			}
			set
			{
				this.requestTypeUrl_ = ProtoPreconditions.CheckNotNull<string>(value, "value");
			}
		}

		// Token: 0x17000052 RID: 82
		// (get) Token: 0x0600024D RID: 589 RVA: 0x0000B43F File Offset: 0x0000963F
		// (set) Token: 0x0600024E RID: 590 RVA: 0x0000B447 File Offset: 0x00009647
		[DebuggerNonUserCode]
		public bool RequestStreaming
		{
			get
			{
				return this.requestStreaming_;
			}
			set
			{
				this.requestStreaming_ = value;
			}
		}

		// Token: 0x17000053 RID: 83
		// (get) Token: 0x0600024F RID: 591 RVA: 0x0000B450 File Offset: 0x00009650
		// (set) Token: 0x06000250 RID: 592 RVA: 0x0000B458 File Offset: 0x00009658
		[DebuggerNonUserCode]
		public string ResponseTypeUrl
		{
			get
			{
				return this.responseTypeUrl_;
			}
			set
			{
				this.responseTypeUrl_ = ProtoPreconditions.CheckNotNull<string>(value, "value");
			}
		}

		// Token: 0x17000054 RID: 84
		// (get) Token: 0x06000251 RID: 593 RVA: 0x0000B46B File Offset: 0x0000966B
		// (set) Token: 0x06000252 RID: 594 RVA: 0x0000B473 File Offset: 0x00009673
		[DebuggerNonUserCode]
		public bool ResponseStreaming
		{
			get
			{
				return this.responseStreaming_;
			}
			set
			{
				this.responseStreaming_ = value;
			}
		}

		// Token: 0x17000055 RID: 85
		// (get) Token: 0x06000253 RID: 595 RVA: 0x0000B47C File Offset: 0x0000967C
		[DebuggerNonUserCode]
		public RepeatedField<Option> Options
		{
			get
			{
				return this.options_;
			}
		}

		// Token: 0x17000056 RID: 86
		// (get) Token: 0x06000254 RID: 596 RVA: 0x0000B484 File Offset: 0x00009684
		// (set) Token: 0x06000255 RID: 597 RVA: 0x0000B48C File Offset: 0x0000968C
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

		// Token: 0x06000256 RID: 598 RVA: 0x0000B495 File Offset: 0x00009695
		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return this.Equals(other as Method);
		}

		// Token: 0x06000257 RID: 599 RVA: 0x0000B4A4 File Offset: 0x000096A4
		[DebuggerNonUserCode]
		public bool Equals(Method other)
		{
			return other != null && (other == this || (!(this.Name != other.Name) && !(this.RequestTypeUrl != other.RequestTypeUrl) && this.RequestStreaming == other.RequestStreaming && !(this.ResponseTypeUrl != other.ResponseTypeUrl) && this.ResponseStreaming == other.ResponseStreaming && this.options_.Equals(other.options_) && this.Syntax == other.Syntax && object.Equals(this._unknownFields, other._unknownFields)));
		}

		// Token: 0x06000258 RID: 600 RVA: 0x0000B554 File Offset: 0x00009754
		[DebuggerNonUserCode]
		public override int GetHashCode()
		{
			int num = 1;
			if (this.Name.Length != 0)
			{
				num ^= this.Name.GetHashCode();
			}
			if (this.RequestTypeUrl.Length != 0)
			{
				num ^= this.RequestTypeUrl.GetHashCode();
			}
			if (this.RequestStreaming)
			{
				num ^= this.RequestStreaming.GetHashCode();
			}
			if (this.ResponseTypeUrl.Length != 0)
			{
				num ^= this.ResponseTypeUrl.GetHashCode();
			}
			if (this.ResponseStreaming)
			{
				num ^= this.ResponseStreaming.GetHashCode();
			}
			num ^= this.options_.GetHashCode();
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

		// Token: 0x06000259 RID: 601 RVA: 0x0000B62A File Offset: 0x0000982A
		[DebuggerNonUserCode]
		public override string ToString()
		{
			return JsonFormatter.ToDiagnosticString(this);
		}

		// Token: 0x0600025A RID: 602 RVA: 0x0000B634 File Offset: 0x00009834
		[DebuggerNonUserCode]
		public void WriteTo(CodedOutputStream output)
		{
			if (this.Name.Length != 0)
			{
				output.WriteRawTag(10);
				output.WriteString(this.Name);
			}
			if (this.RequestTypeUrl.Length != 0)
			{
				output.WriteRawTag(18);
				output.WriteString(this.RequestTypeUrl);
			}
			if (this.RequestStreaming)
			{
				output.WriteRawTag(24);
				output.WriteBool(this.RequestStreaming);
			}
			if (this.ResponseTypeUrl.Length != 0)
			{
				output.WriteRawTag(34);
				output.WriteString(this.ResponseTypeUrl);
			}
			if (this.ResponseStreaming)
			{
				output.WriteRawTag(40);
				output.WriteBool(this.ResponseStreaming);
			}
			this.options_.WriteTo(output, Method._repeated_options_codec);
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

		// Token: 0x0600025B RID: 603 RVA: 0x0000B720 File Offset: 0x00009920
		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (this.Name.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.Name);
			}
			if (this.RequestTypeUrl.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.RequestTypeUrl);
			}
			if (this.RequestStreaming)
			{
				num += 2;
			}
			if (this.ResponseTypeUrl.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.ResponseTypeUrl);
			}
			if (this.ResponseStreaming)
			{
				num += 2;
			}
			num += this.options_.CalculateSize(Method._repeated_options_codec);
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

		// Token: 0x0600025C RID: 604 RVA: 0x0000B7E0 File Offset: 0x000099E0
		[DebuggerNonUserCode]
		public void MergeFrom(Method other)
		{
			if (other == null)
			{
				return;
			}
			if (other.Name.Length != 0)
			{
				this.Name = other.Name;
			}
			if (other.RequestTypeUrl.Length != 0)
			{
				this.RequestTypeUrl = other.RequestTypeUrl;
			}
			if (other.RequestStreaming)
			{
				this.RequestStreaming = other.RequestStreaming;
			}
			if (other.ResponseTypeUrl.Length != 0)
			{
				this.ResponseTypeUrl = other.ResponseTypeUrl;
			}
			if (other.ResponseStreaming)
			{
				this.ResponseStreaming = other.ResponseStreaming;
			}
			this.options_.Add(other.options_);
			if (other.Syntax != Syntax.Proto2)
			{
				this.Syntax = other.Syntax;
			}
			this._unknownFields = UnknownFieldSet.MergeFrom(this._unknownFields, other._unknownFields);
		}

		// Token: 0x0600025D RID: 605 RVA: 0x0000B8A0 File Offset: 0x00009AA0
		[DebuggerNonUserCode]
		public void MergeFrom(CodedInputStream input)
		{
			uint num;
			while ((num = input.ReadTag()) != 0U)
			{
				if (num <= 24U)
				{
					if (num == 10U)
					{
						this.Name = input.ReadString();
						continue;
					}
					if (num == 18U)
					{
						this.RequestTypeUrl = input.ReadString();
						continue;
					}
					if (num == 24U)
					{
						this.RequestStreaming = input.ReadBool();
						continue;
					}
				}
				else if (num <= 40U)
				{
					if (num == 34U)
					{
						this.ResponseTypeUrl = input.ReadString();
						continue;
					}
					if (num == 40U)
					{
						this.ResponseStreaming = input.ReadBool();
						continue;
					}
				}
				else
				{
					if (num == 50U)
					{
						this.options_.AddEntriesFrom(input, Method._repeated_options_codec);
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

		// Token: 0x0400008A RID: 138
		private static readonly MessageParser<Method> _parser = new MessageParser<Method>(() => new Method());

		// Token: 0x0400008B RID: 139
		private UnknownFieldSet _unknownFields;

		// Token: 0x0400008C RID: 140
		public const int NameFieldNumber = 1;

		// Token: 0x0400008D RID: 141
		private string name_ = "";

		// Token: 0x0400008E RID: 142
		public const int RequestTypeUrlFieldNumber = 2;

		// Token: 0x0400008F RID: 143
		private string requestTypeUrl_ = "";

		// Token: 0x04000090 RID: 144
		public const int RequestStreamingFieldNumber = 3;

		// Token: 0x04000091 RID: 145
		private bool requestStreaming_;

		// Token: 0x04000092 RID: 146
		public const int ResponseTypeUrlFieldNumber = 4;

		// Token: 0x04000093 RID: 147
		private string responseTypeUrl_ = "";

		// Token: 0x04000094 RID: 148
		public const int ResponseStreamingFieldNumber = 5;

		// Token: 0x04000095 RID: 149
		private bool responseStreaming_;

		// Token: 0x04000096 RID: 150
		public const int OptionsFieldNumber = 6;

		// Token: 0x04000097 RID: 151
		private static readonly FieldCodec<Option> _repeated_options_codec = FieldCodec.ForMessage<Option>(50U, Option.Parser);

		// Token: 0x04000098 RID: 152
		private readonly RepeatedField<Option> options_ = new RepeatedField<Option>();

		// Token: 0x04000099 RID: 153
		public const int SyntaxFieldNumber = 7;

		// Token: 0x0400009A RID: 154
		private Syntax syntax_;
	}
}
