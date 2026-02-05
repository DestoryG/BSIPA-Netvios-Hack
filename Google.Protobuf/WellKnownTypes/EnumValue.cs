using System;
using System.Diagnostics;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

namespace Google.Protobuf.WellKnownTypes
{
	// Token: 0x02000042 RID: 66
	public sealed class EnumValue : IMessage<EnumValue>, IMessage, IEquatable<EnumValue>, IDeepCloneable<EnumValue>
	{
		// Token: 0x170000A8 RID: 168
		// (get) Token: 0x0600039A RID: 922 RVA: 0x0000F8CE File Offset: 0x0000DACE
		[DebuggerNonUserCode]
		public static MessageParser<EnumValue> Parser
		{
			get
			{
				return EnumValue._parser;
			}
		}

		// Token: 0x170000A9 RID: 169
		// (get) Token: 0x0600039B RID: 923 RVA: 0x0000F8D5 File Offset: 0x0000DAD5
		[DebuggerNonUserCode]
		public static MessageDescriptor Descriptor
		{
			get
			{
				return TypeReflection.Descriptor.MessageTypes[3];
			}
		}

		// Token: 0x170000AA RID: 170
		// (get) Token: 0x0600039C RID: 924 RVA: 0x0000F8E7 File Offset: 0x0000DAE7
		[DebuggerNonUserCode]
		MessageDescriptor IMessage.Descriptor
		{
			get
			{
				return EnumValue.Descriptor;
			}
		}

		// Token: 0x0600039D RID: 925 RVA: 0x0000F8EE File Offset: 0x0000DAEE
		[DebuggerNonUserCode]
		public EnumValue()
		{
		}

		// Token: 0x0600039E RID: 926 RVA: 0x0000F90C File Offset: 0x0000DB0C
		[DebuggerNonUserCode]
		public EnumValue(EnumValue other)
			: this()
		{
			this.name_ = other.name_;
			this.number_ = other.number_;
			this.options_ = other.options_.Clone();
			this._unknownFields = UnknownFieldSet.Clone(other._unknownFields);
		}

		// Token: 0x0600039F RID: 927 RVA: 0x0000F959 File Offset: 0x0000DB59
		[DebuggerNonUserCode]
		public EnumValue Clone()
		{
			return new EnumValue(this);
		}

		// Token: 0x170000AB RID: 171
		// (get) Token: 0x060003A0 RID: 928 RVA: 0x0000F961 File Offset: 0x0000DB61
		// (set) Token: 0x060003A1 RID: 929 RVA: 0x0000F969 File Offset: 0x0000DB69
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

		// Token: 0x170000AC RID: 172
		// (get) Token: 0x060003A2 RID: 930 RVA: 0x0000F97C File Offset: 0x0000DB7C
		// (set) Token: 0x060003A3 RID: 931 RVA: 0x0000F984 File Offset: 0x0000DB84
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

		// Token: 0x170000AD RID: 173
		// (get) Token: 0x060003A4 RID: 932 RVA: 0x0000F98D File Offset: 0x0000DB8D
		[DebuggerNonUserCode]
		public RepeatedField<Option> Options
		{
			get
			{
				return this.options_;
			}
		}

		// Token: 0x060003A5 RID: 933 RVA: 0x0000F995 File Offset: 0x0000DB95
		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return this.Equals(other as EnumValue);
		}

		// Token: 0x060003A6 RID: 934 RVA: 0x0000F9A4 File Offset: 0x0000DBA4
		[DebuggerNonUserCode]
		public bool Equals(EnumValue other)
		{
			return other != null && (other == this || (!(this.Name != other.Name) && this.Number == other.Number && this.options_.Equals(other.options_) && object.Equals(this._unknownFields, other._unknownFields)));
		}

		// Token: 0x060003A7 RID: 935 RVA: 0x0000FA08 File Offset: 0x0000DC08
		[DebuggerNonUserCode]
		public override int GetHashCode()
		{
			int num = 1;
			if (this.Name.Length != 0)
			{
				num ^= this.Name.GetHashCode();
			}
			if (this.Number != 0)
			{
				num ^= this.Number.GetHashCode();
			}
			num ^= this.options_.GetHashCode();
			if (this._unknownFields != null)
			{
				num ^= this._unknownFields.GetHashCode();
			}
			return num;
		}

		// Token: 0x060003A8 RID: 936 RVA: 0x0000FA70 File Offset: 0x0000DC70
		[DebuggerNonUserCode]
		public override string ToString()
		{
			return JsonFormatter.ToDiagnosticString(this);
		}

		// Token: 0x060003A9 RID: 937 RVA: 0x0000FA78 File Offset: 0x0000DC78
		[DebuggerNonUserCode]
		public void WriteTo(CodedOutputStream output)
		{
			if (this.Name.Length != 0)
			{
				output.WriteRawTag(10);
				output.WriteString(this.Name);
			}
			if (this.Number != 0)
			{
				output.WriteRawTag(16);
				output.WriteInt32(this.Number);
			}
			this.options_.WriteTo(output, EnumValue._repeated_options_codec);
			if (this._unknownFields != null)
			{
				this._unknownFields.WriteTo(output);
			}
		}

		// Token: 0x060003AA RID: 938 RVA: 0x0000FAE8 File Offset: 0x0000DCE8
		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (this.Name.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.Name);
			}
			if (this.Number != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.Number);
			}
			num += this.options_.CalculateSize(EnumValue._repeated_options_codec);
			if (this._unknownFields != null)
			{
				num += this._unknownFields.CalculateSize();
			}
			return num;
		}

		// Token: 0x060003AB RID: 939 RVA: 0x0000FB58 File Offset: 0x0000DD58
		[DebuggerNonUserCode]
		public void MergeFrom(EnumValue other)
		{
			if (other == null)
			{
				return;
			}
			if (other.Name.Length != 0)
			{
				this.Name = other.Name;
			}
			if (other.Number != 0)
			{
				this.Number = other.Number;
			}
			this.options_.Add(other.options_);
			this._unknownFields = UnknownFieldSet.MergeFrom(this._unknownFields, other._unknownFields);
		}

		// Token: 0x060003AC RID: 940 RVA: 0x0000FBC0 File Offset: 0x0000DDC0
		[DebuggerNonUserCode]
		public void MergeFrom(CodedInputStream input)
		{
			uint num;
			while ((num = input.ReadTag()) != 0U)
			{
				if (num != 10U)
				{
					if (num != 16U)
					{
						if (num != 26U)
						{
							this._unknownFields = UnknownFieldSet.MergeFieldFrom(this._unknownFields, input);
						}
						else
						{
							this.options_.AddEntriesFrom(input, EnumValue._repeated_options_codec);
						}
					}
					else
					{
						this.Number = input.ReadInt32();
					}
				}
				else
				{
					this.Name = input.ReadString();
				}
			}
		}

		// Token: 0x0400011B RID: 283
		private static readonly MessageParser<EnumValue> _parser = new MessageParser<EnumValue>(() => new EnumValue());

		// Token: 0x0400011C RID: 284
		private UnknownFieldSet _unknownFields;

		// Token: 0x0400011D RID: 285
		public const int NameFieldNumber = 1;

		// Token: 0x0400011E RID: 286
		private string name_ = "";

		// Token: 0x0400011F RID: 287
		public const int NumberFieldNumber = 2;

		// Token: 0x04000120 RID: 288
		private int number_;

		// Token: 0x04000121 RID: 289
		public const int OptionsFieldNumber = 3;

		// Token: 0x04000122 RID: 290
		private static readonly FieldCodec<Option> _repeated_options_codec = FieldCodec.ForMessage<Option>(26U, Option.Parser);

		// Token: 0x04000123 RID: 291
		private readonly RepeatedField<Option> options_ = new RepeatedField<Option>();
	}
}
