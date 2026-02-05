using System;
using System.Diagnostics;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

namespace Google.Protobuf.WellKnownTypes
{
	// Token: 0x02000039 RID: 57
	public sealed class ListValue : IMessage<ListValue>, IMessage, IEquatable<ListValue>, IDeepCloneable<ListValue>
	{
		// Token: 0x1700007F RID: 127
		// (get) Token: 0x0600030D RID: 781 RVA: 0x0000D94D File Offset: 0x0000BB4D
		[DebuggerNonUserCode]
		public static MessageParser<ListValue> Parser
		{
			get
			{
				return ListValue._parser;
			}
		}

		// Token: 0x17000080 RID: 128
		// (get) Token: 0x0600030E RID: 782 RVA: 0x0000D954 File Offset: 0x0000BB54
		[DebuggerNonUserCode]
		public static MessageDescriptor Descriptor
		{
			get
			{
				return StructReflection.Descriptor.MessageTypes[2];
			}
		}

		// Token: 0x17000081 RID: 129
		// (get) Token: 0x0600030F RID: 783 RVA: 0x0000D966 File Offset: 0x0000BB66
		[DebuggerNonUserCode]
		MessageDescriptor IMessage.Descriptor
		{
			get
			{
				return ListValue.Descriptor;
			}
		}

		// Token: 0x06000310 RID: 784 RVA: 0x0000D96D File Offset: 0x0000BB6D
		[DebuggerNonUserCode]
		public ListValue()
		{
		}

		// Token: 0x06000311 RID: 785 RVA: 0x0000D980 File Offset: 0x0000BB80
		[DebuggerNonUserCode]
		public ListValue(ListValue other)
			: this()
		{
			this.values_ = other.values_.Clone();
			this._unknownFields = UnknownFieldSet.Clone(other._unknownFields);
		}

		// Token: 0x06000312 RID: 786 RVA: 0x0000D9AA File Offset: 0x0000BBAA
		[DebuggerNonUserCode]
		public ListValue Clone()
		{
			return new ListValue(this);
		}

		// Token: 0x17000082 RID: 130
		// (get) Token: 0x06000313 RID: 787 RVA: 0x0000D9B2 File Offset: 0x0000BBB2
		[DebuggerNonUserCode]
		public RepeatedField<Value> Values
		{
			get
			{
				return this.values_;
			}
		}

		// Token: 0x06000314 RID: 788 RVA: 0x0000D9BA File Offset: 0x0000BBBA
		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return this.Equals(other as ListValue);
		}

		// Token: 0x06000315 RID: 789 RVA: 0x0000D9C8 File Offset: 0x0000BBC8
		[DebuggerNonUserCode]
		public bool Equals(ListValue other)
		{
			return other != null && (other == this || (this.values_.Equals(other.values_) && object.Equals(this._unknownFields, other._unknownFields)));
		}

		// Token: 0x06000316 RID: 790 RVA: 0x0000D9FC File Offset: 0x0000BBFC
		[DebuggerNonUserCode]
		public override int GetHashCode()
		{
			int num = 1;
			num ^= this.values_.GetHashCode();
			if (this._unknownFields != null)
			{
				num ^= this._unknownFields.GetHashCode();
			}
			return num;
		}

		// Token: 0x06000317 RID: 791 RVA: 0x0000DA30 File Offset: 0x0000BC30
		[DebuggerNonUserCode]
		public override string ToString()
		{
			return JsonFormatter.ToDiagnosticString(this);
		}

		// Token: 0x06000318 RID: 792 RVA: 0x0000DA38 File Offset: 0x0000BC38
		[DebuggerNonUserCode]
		public void WriteTo(CodedOutputStream output)
		{
			this.values_.WriteTo(output, ListValue._repeated_values_codec);
			if (this._unknownFields != null)
			{
				this._unknownFields.WriteTo(output);
			}
		}

		// Token: 0x06000319 RID: 793 RVA: 0x0000DA60 File Offset: 0x0000BC60
		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			num += this.values_.CalculateSize(ListValue._repeated_values_codec);
			if (this._unknownFields != null)
			{
				num += this._unknownFields.CalculateSize();
			}
			return num;
		}

		// Token: 0x0600031A RID: 794 RVA: 0x0000DA99 File Offset: 0x0000BC99
		[DebuggerNonUserCode]
		public void MergeFrom(ListValue other)
		{
			if (other == null)
			{
				return;
			}
			this.values_.Add(other.values_);
			this._unknownFields = UnknownFieldSet.MergeFrom(this._unknownFields, other._unknownFields);
		}

		// Token: 0x0600031B RID: 795 RVA: 0x0000DAC8 File Offset: 0x0000BCC8
		[DebuggerNonUserCode]
		public void MergeFrom(CodedInputStream input)
		{
			uint num;
			while ((num = input.ReadTag()) != 0U)
			{
				if (num != 10U)
				{
					this._unknownFields = UnknownFieldSet.MergeFieldFrom(this._unknownFields, input);
				}
				else
				{
					this.values_.AddEntriesFrom(input, ListValue._repeated_values_codec);
				}
			}
		}

		// Token: 0x040000D0 RID: 208
		private static readonly MessageParser<ListValue> _parser = new MessageParser<ListValue>(() => new ListValue());

		// Token: 0x040000D1 RID: 209
		private UnknownFieldSet _unknownFields;

		// Token: 0x040000D2 RID: 210
		public const int ValuesFieldNumber = 1;

		// Token: 0x040000D3 RID: 211
		private static readonly FieldCodec<Value> _repeated_values_codec = FieldCodec.ForMessage<Value>(10U, Value.Parser);

		// Token: 0x040000D4 RID: 212
		private readonly RepeatedField<Value> values_ = new RepeatedField<Value>();
	}
}
