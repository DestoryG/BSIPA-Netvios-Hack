using System;
using System.Diagnostics;
using Google.Protobuf.Reflection;

namespace Google.Protobuf.WellKnownTypes
{
	// Token: 0x02000043 RID: 67
	public sealed class Option : IMessage<Option>, IMessage, IEquatable<Option>, IDeepCloneable<Option>
	{
		// Token: 0x170000AE RID: 174
		// (get) Token: 0x060003AE RID: 942 RVA: 0x0000FC56 File Offset: 0x0000DE56
		[DebuggerNonUserCode]
		public static MessageParser<Option> Parser
		{
			get
			{
				return Option._parser;
			}
		}

		// Token: 0x170000AF RID: 175
		// (get) Token: 0x060003AF RID: 943 RVA: 0x0000FC5D File Offset: 0x0000DE5D
		[DebuggerNonUserCode]
		public static MessageDescriptor Descriptor
		{
			get
			{
				return TypeReflection.Descriptor.MessageTypes[4];
			}
		}

		// Token: 0x170000B0 RID: 176
		// (get) Token: 0x060003B0 RID: 944 RVA: 0x0000FC6F File Offset: 0x0000DE6F
		[DebuggerNonUserCode]
		MessageDescriptor IMessage.Descriptor
		{
			get
			{
				return Option.Descriptor;
			}
		}

		// Token: 0x060003B1 RID: 945 RVA: 0x0000FC76 File Offset: 0x0000DE76
		[DebuggerNonUserCode]
		public Option()
		{
		}

		// Token: 0x060003B2 RID: 946 RVA: 0x0000FC8C File Offset: 0x0000DE8C
		[DebuggerNonUserCode]
		public Option(Option other)
			: this()
		{
			this.name_ = other.name_;
			this.value_ = ((other.value_ != null) ? other.value_.Clone() : null);
			this._unknownFields = UnknownFieldSet.Clone(other._unknownFields);
		}

		// Token: 0x060003B3 RID: 947 RVA: 0x0000FCD8 File Offset: 0x0000DED8
		[DebuggerNonUserCode]
		public Option Clone()
		{
			return new Option(this);
		}

		// Token: 0x170000B1 RID: 177
		// (get) Token: 0x060003B4 RID: 948 RVA: 0x0000FCE0 File Offset: 0x0000DEE0
		// (set) Token: 0x060003B5 RID: 949 RVA: 0x0000FCE8 File Offset: 0x0000DEE8
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

		// Token: 0x170000B2 RID: 178
		// (get) Token: 0x060003B6 RID: 950 RVA: 0x0000FCFB File Offset: 0x0000DEFB
		// (set) Token: 0x060003B7 RID: 951 RVA: 0x0000FD03 File Offset: 0x0000DF03
		[DebuggerNonUserCode]
		public Any Value
		{
			get
			{
				return this.value_;
			}
			set
			{
				this.value_ = value;
			}
		}

		// Token: 0x060003B8 RID: 952 RVA: 0x0000FD0C File Offset: 0x0000DF0C
		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return this.Equals(other as Option);
		}

		// Token: 0x060003B9 RID: 953 RVA: 0x0000FD1C File Offset: 0x0000DF1C
		[DebuggerNonUserCode]
		public bool Equals(Option other)
		{
			return other != null && (other == this || (!(this.Name != other.Name) && object.Equals(this.Value, other.Value) && object.Equals(this._unknownFields, other._unknownFields)));
		}

		// Token: 0x060003BA RID: 954 RVA: 0x0000FD70 File Offset: 0x0000DF70
		[DebuggerNonUserCode]
		public override int GetHashCode()
		{
			int num = 1;
			if (this.Name.Length != 0)
			{
				num ^= this.Name.GetHashCode();
			}
			if (this.value_ != null)
			{
				num ^= this.Value.GetHashCode();
			}
			if (this._unknownFields != null)
			{
				num ^= this._unknownFields.GetHashCode();
			}
			return num;
		}

		// Token: 0x060003BB RID: 955 RVA: 0x0000FDC7 File Offset: 0x0000DFC7
		[DebuggerNonUserCode]
		public override string ToString()
		{
			return JsonFormatter.ToDiagnosticString(this);
		}

		// Token: 0x060003BC RID: 956 RVA: 0x0000FDD0 File Offset: 0x0000DFD0
		[DebuggerNonUserCode]
		public void WriteTo(CodedOutputStream output)
		{
			if (this.Name.Length != 0)
			{
				output.WriteRawTag(10);
				output.WriteString(this.Name);
			}
			if (this.value_ != null)
			{
				output.WriteRawTag(18);
				output.WriteMessage(this.Value);
			}
			if (this._unknownFields != null)
			{
				this._unknownFields.WriteTo(output);
			}
		}

		// Token: 0x060003BD RID: 957 RVA: 0x0000FE30 File Offset: 0x0000E030
		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (this.Name.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.Name);
			}
			if (this.value_ != null)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(this.Value);
			}
			if (this._unknownFields != null)
			{
				num += this._unknownFields.CalculateSize();
			}
			return num;
		}

		// Token: 0x060003BE RID: 958 RVA: 0x0000FE8C File Offset: 0x0000E08C
		[DebuggerNonUserCode]
		public void MergeFrom(Option other)
		{
			if (other == null)
			{
				return;
			}
			if (other.Name.Length != 0)
			{
				this.Name = other.Name;
			}
			if (other.value_ != null)
			{
				if (this.value_ == null)
				{
					this.Value = new Any();
				}
				this.Value.MergeFrom(other.Value);
			}
			this._unknownFields = UnknownFieldSet.MergeFrom(this._unknownFields, other._unknownFields);
		}

		// Token: 0x060003BF RID: 959 RVA: 0x0000FEFC File Offset: 0x0000E0FC
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
						if (this.value_ == null)
						{
							this.Value = new Any();
						}
						input.ReadMessage(this.Value);
					}
				}
				else
				{
					this.Name = input.ReadString();
				}
			}
		}

		// Token: 0x04000124 RID: 292
		private static readonly MessageParser<Option> _parser = new MessageParser<Option>(() => new Option());

		// Token: 0x04000125 RID: 293
		private UnknownFieldSet _unknownFields;

		// Token: 0x04000126 RID: 294
		public const int NameFieldNumber = 1;

		// Token: 0x04000127 RID: 295
		private string name_ = "";

		// Token: 0x04000128 RID: 296
		public const int ValueFieldNumber = 2;

		// Token: 0x04000129 RID: 297
		private Any value_;
	}
}
