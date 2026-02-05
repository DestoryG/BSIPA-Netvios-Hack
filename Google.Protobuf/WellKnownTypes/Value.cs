using System;
using System.Diagnostics;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

namespace Google.Protobuf.WellKnownTypes
{
	// Token: 0x02000038 RID: 56
	public sealed class Value : IMessage<Value>, IMessage, IEquatable<Value>, IDeepCloneable<Value>
	{
		// Token: 0x17000075 RID: 117
		// (get) Token: 0x060002EA RID: 746 RVA: 0x0000D1BB File Offset: 0x0000B3BB
		[DebuggerNonUserCode]
		public static MessageParser<Value> Parser
		{
			get
			{
				return Value._parser;
			}
		}

		// Token: 0x17000076 RID: 118
		// (get) Token: 0x060002EB RID: 747 RVA: 0x0000D1C2 File Offset: 0x0000B3C2
		[DebuggerNonUserCode]
		public static MessageDescriptor Descriptor
		{
			get
			{
				return StructReflection.Descriptor.MessageTypes[1];
			}
		}

		// Token: 0x17000077 RID: 119
		// (get) Token: 0x060002EC RID: 748 RVA: 0x0000D1D4 File Offset: 0x0000B3D4
		[DebuggerNonUserCode]
		MessageDescriptor IMessage.Descriptor
		{
			get
			{
				return Value.Descriptor;
			}
		}

		// Token: 0x060002ED RID: 749 RVA: 0x0000D1DB File Offset: 0x0000B3DB
		[DebuggerNonUserCode]
		public Value()
		{
		}

		// Token: 0x060002EE RID: 750 RVA: 0x0000D1E4 File Offset: 0x0000B3E4
		[DebuggerNonUserCode]
		public Value(Value other)
			: this()
		{
			switch (other.KindCase)
			{
			case Value.KindOneofCase.NullValue:
				this.NullValue = other.NullValue;
				break;
			case Value.KindOneofCase.NumberValue:
				this.NumberValue = other.NumberValue;
				break;
			case Value.KindOneofCase.StringValue:
				this.StringValue = other.StringValue;
				break;
			case Value.KindOneofCase.BoolValue:
				this.BoolValue = other.BoolValue;
				break;
			case Value.KindOneofCase.StructValue:
				this.StructValue = other.StructValue.Clone();
				break;
			case Value.KindOneofCase.ListValue:
				this.ListValue = other.ListValue.Clone();
				break;
			}
			this._unknownFields = UnknownFieldSet.Clone(other._unknownFields);
		}

		// Token: 0x060002EF RID: 751 RVA: 0x0000D28D File Offset: 0x0000B48D
		[DebuggerNonUserCode]
		public Value Clone()
		{
			return new Value(this);
		}

		// Token: 0x17000078 RID: 120
		// (get) Token: 0x060002F0 RID: 752 RVA: 0x0000D295 File Offset: 0x0000B495
		// (set) Token: 0x060002F1 RID: 753 RVA: 0x0000D2AD File Offset: 0x0000B4AD
		[DebuggerNonUserCode]
		public NullValue NullValue
		{
			get
			{
				if (this.kindCase_ != Value.KindOneofCase.NullValue)
				{
					return NullValue.NullValue;
				}
				return (NullValue)this.kind_;
			}
			set
			{
				this.kind_ = value;
				this.kindCase_ = Value.KindOneofCase.NullValue;
			}
		}

		// Token: 0x17000079 RID: 121
		// (get) Token: 0x060002F2 RID: 754 RVA: 0x0000D2C2 File Offset: 0x0000B4C2
		// (set) Token: 0x060002F3 RID: 755 RVA: 0x0000D2E2 File Offset: 0x0000B4E2
		[DebuggerNonUserCode]
		public double NumberValue
		{
			get
			{
				if (this.kindCase_ != Value.KindOneofCase.NumberValue)
				{
					return 0.0;
				}
				return (double)this.kind_;
			}
			set
			{
				this.kind_ = value;
				this.kindCase_ = Value.KindOneofCase.NumberValue;
			}
		}

		// Token: 0x1700007A RID: 122
		// (get) Token: 0x060002F4 RID: 756 RVA: 0x0000D2F7 File Offset: 0x0000B4F7
		// (set) Token: 0x060002F5 RID: 757 RVA: 0x0000D313 File Offset: 0x0000B513
		[DebuggerNonUserCode]
		public string StringValue
		{
			get
			{
				if (this.kindCase_ != Value.KindOneofCase.StringValue)
				{
					return "";
				}
				return (string)this.kind_;
			}
			set
			{
				this.kind_ = ProtoPreconditions.CheckNotNull<string>(value, "value");
				this.kindCase_ = Value.KindOneofCase.StringValue;
			}
		}

		// Token: 0x1700007B RID: 123
		// (get) Token: 0x060002F6 RID: 758 RVA: 0x0000D32D File Offset: 0x0000B52D
		// (set) Token: 0x060002F7 RID: 759 RVA: 0x0000D345 File Offset: 0x0000B545
		[DebuggerNonUserCode]
		public bool BoolValue
		{
			get
			{
				return this.kindCase_ == Value.KindOneofCase.BoolValue && (bool)this.kind_;
			}
			set
			{
				this.kind_ = value;
				this.kindCase_ = Value.KindOneofCase.BoolValue;
			}
		}

		// Token: 0x1700007C RID: 124
		// (get) Token: 0x060002F8 RID: 760 RVA: 0x0000D35A File Offset: 0x0000B55A
		// (set) Token: 0x060002F9 RID: 761 RVA: 0x0000D372 File Offset: 0x0000B572
		[DebuggerNonUserCode]
		public Struct StructValue
		{
			get
			{
				if (this.kindCase_ != Value.KindOneofCase.StructValue)
				{
					return null;
				}
				return (Struct)this.kind_;
			}
			set
			{
				this.kind_ = value;
				this.kindCase_ = ((value == null) ? Value.KindOneofCase.None : Value.KindOneofCase.StructValue);
			}
		}

		// Token: 0x1700007D RID: 125
		// (get) Token: 0x060002FA RID: 762 RVA: 0x0000D388 File Offset: 0x0000B588
		// (set) Token: 0x060002FB RID: 763 RVA: 0x0000D3A0 File Offset: 0x0000B5A0
		[DebuggerNonUserCode]
		public ListValue ListValue
		{
			get
			{
				if (this.kindCase_ != Value.KindOneofCase.ListValue)
				{
					return null;
				}
				return (ListValue)this.kind_;
			}
			set
			{
				this.kind_ = value;
				this.kindCase_ = ((value == null) ? Value.KindOneofCase.None : Value.KindOneofCase.ListValue);
			}
		}

		// Token: 0x1700007E RID: 126
		// (get) Token: 0x060002FC RID: 764 RVA: 0x0000D3B6 File Offset: 0x0000B5B6
		[DebuggerNonUserCode]
		public Value.KindOneofCase KindCase
		{
			get
			{
				return this.kindCase_;
			}
		}

		// Token: 0x060002FD RID: 765 RVA: 0x0000D3BE File Offset: 0x0000B5BE
		[DebuggerNonUserCode]
		public void ClearKind()
		{
			this.kindCase_ = Value.KindOneofCase.None;
			this.kind_ = null;
		}

		// Token: 0x060002FE RID: 766 RVA: 0x0000D3CE File Offset: 0x0000B5CE
		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return this.Equals(other as Value);
		}

		// Token: 0x060002FF RID: 767 RVA: 0x0000D3DC File Offset: 0x0000B5DC
		[DebuggerNonUserCode]
		public bool Equals(Value other)
		{
			return other != null && (other == this || (this.NullValue == other.NullValue && ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.Equals(this.NumberValue, other.NumberValue) && !(this.StringValue != other.StringValue) && this.BoolValue == other.BoolValue && object.Equals(this.StructValue, other.StructValue) && object.Equals(this.ListValue, other.ListValue) && this.KindCase == other.KindCase && object.Equals(this._unknownFields, other._unknownFields)));
		}

		// Token: 0x06000300 RID: 768 RVA: 0x0000D490 File Offset: 0x0000B690
		[DebuggerNonUserCode]
		public override int GetHashCode()
		{
			int num = 1;
			if (this.kindCase_ == Value.KindOneofCase.NullValue)
			{
				num ^= this.NullValue.GetHashCode();
			}
			if (this.kindCase_ == Value.KindOneofCase.NumberValue)
			{
				num ^= ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.GetHashCode(this.NumberValue);
			}
			if (this.kindCase_ == Value.KindOneofCase.StringValue)
			{
				num ^= this.StringValue.GetHashCode();
			}
			if (this.kindCase_ == Value.KindOneofCase.BoolValue)
			{
				num ^= this.BoolValue.GetHashCode();
			}
			if (this.kindCase_ == Value.KindOneofCase.StructValue)
			{
				num ^= this.StructValue.GetHashCode();
			}
			if (this.kindCase_ == Value.KindOneofCase.ListValue)
			{
				num ^= this.ListValue.GetHashCode();
			}
			num ^= (int)this.kindCase_;
			if (this._unknownFields != null)
			{
				num ^= this._unknownFields.GetHashCode();
			}
			return num;
		}

		// Token: 0x06000301 RID: 769 RVA: 0x0000D55A File Offset: 0x0000B75A
		[DebuggerNonUserCode]
		public override string ToString()
		{
			return JsonFormatter.ToDiagnosticString(this);
		}

		// Token: 0x06000302 RID: 770 RVA: 0x0000D564 File Offset: 0x0000B764
		[DebuggerNonUserCode]
		public void WriteTo(CodedOutputStream output)
		{
			if (this.kindCase_ == Value.KindOneofCase.NullValue)
			{
				output.WriteRawTag(8);
				output.WriteEnum((int)this.NullValue);
			}
			if (this.kindCase_ == Value.KindOneofCase.NumberValue)
			{
				output.WriteRawTag(17);
				output.WriteDouble(this.NumberValue);
			}
			if (this.kindCase_ == Value.KindOneofCase.StringValue)
			{
				output.WriteRawTag(26);
				output.WriteString(this.StringValue);
			}
			if (this.kindCase_ == Value.KindOneofCase.BoolValue)
			{
				output.WriteRawTag(32);
				output.WriteBool(this.BoolValue);
			}
			if (this.kindCase_ == Value.KindOneofCase.StructValue)
			{
				output.WriteRawTag(42);
				output.WriteMessage(this.StructValue);
			}
			if (this.kindCase_ == Value.KindOneofCase.ListValue)
			{
				output.WriteRawTag(50);
				output.WriteMessage(this.ListValue);
			}
			if (this._unknownFields != null)
			{
				this._unknownFields.WriteTo(output);
			}
		}

		// Token: 0x06000303 RID: 771 RVA: 0x0000D634 File Offset: 0x0000B834
		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (this.kindCase_ == Value.KindOneofCase.NullValue)
			{
				num += 1 + CodedOutputStream.ComputeEnumSize((int)this.NullValue);
			}
			if (this.kindCase_ == Value.KindOneofCase.NumberValue)
			{
				num += 9;
			}
			if (this.kindCase_ == Value.KindOneofCase.StringValue)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.StringValue);
			}
			if (this.kindCase_ == Value.KindOneofCase.BoolValue)
			{
				num += 2;
			}
			if (this.kindCase_ == Value.KindOneofCase.StructValue)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(this.StructValue);
			}
			if (this.kindCase_ == Value.KindOneofCase.ListValue)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(this.ListValue);
			}
			if (this._unknownFields != null)
			{
				num += this._unknownFields.CalculateSize();
			}
			return num;
		}

		// Token: 0x06000304 RID: 772 RVA: 0x0000D6DC File Offset: 0x0000B8DC
		[DebuggerNonUserCode]
		public void MergeFrom(Value other)
		{
			if (other == null)
			{
				return;
			}
			switch (other.KindCase)
			{
			case Value.KindOneofCase.NullValue:
				this.NullValue = other.NullValue;
				break;
			case Value.KindOneofCase.NumberValue:
				this.NumberValue = other.NumberValue;
				break;
			case Value.KindOneofCase.StringValue:
				this.StringValue = other.StringValue;
				break;
			case Value.KindOneofCase.BoolValue:
				this.BoolValue = other.BoolValue;
				break;
			case Value.KindOneofCase.StructValue:
				if (this.StructValue == null)
				{
					this.StructValue = new Struct();
				}
				this.StructValue.MergeFrom(other.StructValue);
				break;
			case Value.KindOneofCase.ListValue:
				if (this.ListValue == null)
				{
					this.ListValue = new ListValue();
				}
				this.ListValue.MergeFrom(other.ListValue);
				break;
			}
			this._unknownFields = UnknownFieldSet.MergeFrom(this._unknownFields, other._unknownFields);
		}

		// Token: 0x06000305 RID: 773 RVA: 0x0000D7B4 File Offset: 0x0000B9B4
		[DebuggerNonUserCode]
		public void MergeFrom(CodedInputStream input)
		{
			uint num;
			while ((num = input.ReadTag()) != 0U)
			{
				if (num <= 26U)
				{
					if (num == 8U)
					{
						this.kind_ = input.ReadEnum();
						this.kindCase_ = Value.KindOneofCase.NullValue;
						continue;
					}
					if (num == 17U)
					{
						this.NumberValue = input.ReadDouble();
						continue;
					}
					if (num == 26U)
					{
						this.StringValue = input.ReadString();
						continue;
					}
				}
				else
				{
					if (num == 32U)
					{
						this.BoolValue = input.ReadBool();
						continue;
					}
					if (num == 42U)
					{
						Struct @struct = new Struct();
						if (this.kindCase_ == Value.KindOneofCase.StructValue)
						{
							@struct.MergeFrom(this.StructValue);
						}
						input.ReadMessage(@struct);
						this.StructValue = @struct;
						continue;
					}
					if (num == 50U)
					{
						ListValue listValue = new ListValue();
						if (this.kindCase_ == Value.KindOneofCase.ListValue)
						{
							listValue.MergeFrom(this.ListValue);
						}
						input.ReadMessage(listValue);
						this.ListValue = listValue;
						continue;
					}
				}
				this._unknownFields = UnknownFieldSet.MergeFieldFrom(this._unknownFields, input);
			}
		}

		// Token: 0x06000306 RID: 774 RVA: 0x0000D8A9 File Offset: 0x0000BAA9
		public static Value ForString(string value)
		{
			ProtoPreconditions.CheckNotNull<string>(value, "value");
			return new Value
			{
				StringValue = value
			};
		}

		// Token: 0x06000307 RID: 775 RVA: 0x0000D8C3 File Offset: 0x0000BAC3
		public static Value ForNumber(double value)
		{
			return new Value
			{
				NumberValue = value
			};
		}

		// Token: 0x06000308 RID: 776 RVA: 0x0000D8D1 File Offset: 0x0000BAD1
		public static Value ForBool(bool value)
		{
			return new Value
			{
				BoolValue = value
			};
		}

		// Token: 0x06000309 RID: 777 RVA: 0x0000D8DF File Offset: 0x0000BADF
		public static Value ForNull()
		{
			return new Value
			{
				NullValue = NullValue.NullValue
			};
		}

		// Token: 0x0600030A RID: 778 RVA: 0x0000D8ED File Offset: 0x0000BAED
		public static Value ForList(params Value[] values)
		{
			ProtoPreconditions.CheckNotNull<Value[]>(values, "values");
			return new Value
			{
				ListValue = new ListValue
				{
					Values = { values }
				}
			};
		}

		// Token: 0x0600030B RID: 779 RVA: 0x0000D917 File Offset: 0x0000BB17
		public static Value ForStruct(Struct value)
		{
			ProtoPreconditions.CheckNotNull<Struct>(value, "value");
			return new Value
			{
				StructValue = value
			};
		}

		// Token: 0x040000C6 RID: 198
		private static readonly MessageParser<Value> _parser = new MessageParser<Value>(() => new Value());

		// Token: 0x040000C7 RID: 199
		private UnknownFieldSet _unknownFields;

		// Token: 0x040000C8 RID: 200
		public const int NullValueFieldNumber = 1;

		// Token: 0x040000C9 RID: 201
		public const int NumberValueFieldNumber = 2;

		// Token: 0x040000CA RID: 202
		public const int StringValueFieldNumber = 3;

		// Token: 0x040000CB RID: 203
		public const int BoolValueFieldNumber = 4;

		// Token: 0x040000CC RID: 204
		public const int StructValueFieldNumber = 5;

		// Token: 0x040000CD RID: 205
		public const int ListValueFieldNumber = 6;

		// Token: 0x040000CE RID: 206
		private object kind_;

		// Token: 0x040000CF RID: 207
		private Value.KindOneofCase kindCase_;

		// Token: 0x020000B5 RID: 181
		public enum KindOneofCase
		{
			// Token: 0x040003D0 RID: 976
			None,
			// Token: 0x040003D1 RID: 977
			NullValue,
			// Token: 0x040003D2 RID: 978
			NumberValue,
			// Token: 0x040003D3 RID: 979
			StringValue,
			// Token: 0x040003D4 RID: 980
			BoolValue,
			// Token: 0x040003D5 RID: 981
			StructValue,
			// Token: 0x040003D6 RID: 982
			ListValue
		}
	}
}
